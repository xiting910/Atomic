using System;
using System.Collections.Generic;
using System.Threading;
using Result = Xiting.Atomic.AtomicOperationResult;

namespace Xiting.Atomic;

/// <summary>
/// 对枚举类型的原子操作封装结构体
/// </summary>
/// <typeparam name="TEnum">枚举类型</typeparam>
#pragma warning disable CA1711 // 标识符应采用正确的后缀
public struct AtomicEnum<TEnum> : IEquatable<AtomicEnum<TEnum>>, IEquatable<TEnum> where TEnum : struct, Enum
#pragma warning restore CA1711 // 标识符应采用正确的后缀
{
    /// <summary>
    /// 枚举的值
    /// </summary>
    private TEnum _value = default;

    /// <summary>
    /// 获取或设置当前枚举值
    /// </summary>
    /// <remarks>
    /// 该属性的获取和设置操作是原子操作, 但如果要进行 读取-修改-写入 的原子操作, 请改为使用
    /// <list type="bullet">
    /// <item><see cref="CompareAndSet(TEnum, TEnum, out TEnum)"/></item>
    /// <item><see cref="PredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/></item>
    /// <item><see cref="PredicateAndSet(Func{TEnum, bool}, Func{TEnum, TEnum}, out TEnum)"/></item>
    /// <item><see cref="SpinPredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/></item>
    /// <item><see cref="SpinPredicateAndSet(Func{TEnum, bool}, Func{TEnum, TEnum}, out TEnum, out TEnum)"/>
    /// </item></list></remarks>
    public TEnum Value
    {
        get => Interlocked.CompareExchange(ref _value, default, default);
        set => Interlocked.Exchange(ref _value, value);
    }

    /// <summary>
    /// 初始化一个新的 <see cref="AtomicEnum{TEnum}"/> 实例
    /// </summary>
    /// <param name="initialValue">初始值</param>
    public AtomicEnum(TEnum initialValue)
    {
        _value = initialValue;
    }

    /// <summary>
    /// 初始化一个新的 <see cref="AtomicEnum{TEnum}"/> 实例
    /// </summary>
    /// <param name="atomicEnum">另一个 <see cref="AtomicEnum{TEnum}"/> 实例</param>
    public AtomicEnum(AtomicEnum<TEnum> atomicEnum)
    {
        _value = atomicEnum.Value;
    }

    /// <summary>
    /// 将当前枚举值设置为指定的值
    /// </summary>
    /// <remarks>
    /// 该方法是原子操作, 但如果要进行 读取-修改-写入 的原子操作, 请改为使用
    /// <list type="bullet">
    /// <item><see cref="CompareAndSet(TEnum, TEnum, out TEnum)"/></item>
    /// <item><see cref="PredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/></item>
    /// <item><see cref="PredicateAndSet(Func{TEnum, bool}, Func{TEnum, TEnum}, out TEnum)"/></item>
    /// <item><see cref="SpinPredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/></item>
    /// <item><see cref="SpinPredicateAndSet(Func{TEnum, bool}, Func{TEnum, TEnum}, out TEnum, out TEnum)"/>
    /// </item></list></remarks>
    /// <param name="newValue">要设置的新值</param>
    /// <param name="originalValue">执行操作前的原始值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    public bool Set(TEnum newValue, out TEnum originalValue)
    {
        originalValue = Interlocked.Exchange(ref _value, newValue);
        return !EqualityComparer<TEnum>.Default.Equals(originalValue, newValue);
    }

    /// <summary>
    /// 如果当前枚举值等于指定的比较值, 则将其设置为新值
    /// </summary>
    /// <param name="comparisonValue">要比较的值</param>
    /// <param name="newValue">要设置的新值</param>
    /// <param name="originalValue">执行操作前的原始值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    public bool CompareAndSet(TEnum comparisonValue, TEnum newValue, out TEnum originalValue)
    {
        if (EqualityComparer<TEnum>.Default.Equals(comparisonValue, newValue))
        {
            originalValue = Value;
            return false;
        }

        originalValue = Interlocked.CompareExchange(ref _value, newValue, comparisonValue);
        return EqualityComparer<TEnum>.Default.Equals(originalValue, comparisonValue);
    }

    /// <summary>
    /// 如果当前枚举值满足指定的谓词条件, 则将其设置为新值, 发生竞态时不会自动重试
    /// </summary>
    /// <param name="predicate">要检查的谓词条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <param name="originalValue">执行操作前的原始值</param>
    /// <returns>操作结果</returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public Result PredicateAndSet(Func<TEnum, bool> predicate, TEnum newValue, out TEnum originalValue)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        originalValue = Value;
        return PredicateAndSetCore(predicate, originalValue, newValue);
    }

    /// <summary>
    /// 如果当前枚举值满足指定的谓词条件, 则将其设置为由工厂方法生成的新值, 发生竞态时不会自动重试
    /// </summary>
    /// <param name="predicate">要检查的谓词条件</param>
    /// <param name="newValueFactory">用于生成要设置的新值的工厂方法</param>
    /// <param name="originalValue">执行操作前的原始值</param>
    /// <returns>操作结果</returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> 或
    /// <paramref name="newValueFactory"/></exception>
    public Result PredicateAndSet(
        Func<TEnum, bool> predicate,
        Func<TEnum, TEnum> newValueFactory,
        out TEnum originalValue)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        ArgumentNullException.ThrowIfNull(newValueFactory, nameof(newValueFactory));

        originalValue = Value;
        return PredicateAndSetCore(predicate, originalValue, newValueFactory(originalValue));
    }

    /// <summary>
    /// 使用自旋模式, 如果当前枚举值满足指定的谓词条件, 则将其设置为新值
    /// </summary>
    /// <param name="predicate">要检查的谓词条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <param name="originalValue">执行操作前的原始值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public bool SpinPredicateAndSet(Func<TEnum, bool> predicate, TEnum newValue, out TEnum originalValue)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        TEnum? original = null;
        var spinWait = new SpinWait();
        while (true)
        {
            var currentValue = Value;
            original ??= currentValue;
            switch (PredicateAndSetCore(predicate, currentValue, newValue))
            {
                case Result.AlreadySet or Result.PredicateFailed:
                    originalValue = original.Value;
                    return false;
                case Result.Success:
                    originalValue = original.Value;
                    return true;
                case Result.RaceCondition:
                    spinWait.SpinOnce();
                    continue;
            }
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前枚举值满足指定的谓词条件, 则将其设置为由工厂方法生成的新值
    /// </summary>
    /// <param name="predicate">要检查的谓词条件</param>
    /// <param name="newValueFactory">用于生成要设置的新值的工厂方法</param>
    /// <param name="originalValue">执行操作前的原始值</param>
    /// <param name="finalValue">执行操作后最终的值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> 或
    /// <paramref name="newValueFactory"/></exception>
    public bool SpinPredicateAndSet(
        Func<TEnum, bool> predicate,
        Func<TEnum, TEnum> newValueFactory,
        out TEnum originalValue,
        out TEnum finalValue)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        ArgumentNullException.ThrowIfNull(newValueFactory, nameof(newValueFactory));

        TEnum? original = null;
        var spinWait = new SpinWait();
        while (true)
        {
            var currentValue = Value;
            original ??= currentValue;
            finalValue = newValueFactory(currentValue);
            switch (PredicateAndSetCore(predicate, currentValue, finalValue))
            {
                case Result.AlreadySet:
                    originalValue = original.Value;
                    return false;
                case Result.PredicateFailed:
                    originalValue = original.Value;
                    finalValue = currentValue;
                    return false;
                case Result.Success:
                    originalValue = original.Value;
                    return true;
                case Result.RaceCondition:
                    spinWait.SpinOnce();
                    continue;
            }
        }
    }

    /// <summary>
    /// 如果当前枚举值满足指定的谓词条件, 则将其设置为新值
    /// </summary>
    /// <param name="predicate">要检查的谓词条件</param>
    /// <param name="currentValue">当前值</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns>操作结果</returns>
    private Result PredicateAndSetCore(Func<TEnum, bool> predicate, TEnum currentValue, TEnum newValue)
    {
        if (EqualityComparer<TEnum>.Default.Equals(currentValue, newValue))
        {
            return Result.AlreadySet;
        }

        if (!predicate(currentValue))
        {
            return Result.PredicateFailed;
        }

        var previousValue = Interlocked.CompareExchange(ref _value, newValue, currentValue);
        if (!EqualityComparer<TEnum>.Default.Equals(previousValue, currentValue))
        {
            // previousValue 不是 currentValue, 说明在执行 CompareExchange 时发生了竞态条件
            return Result.RaceCondition;
        }
        return Result.Success;
    }

    /// <inheritdoc/>
    public bool Equals(AtomicEnum<TEnum> other)
    {
        return EqualityComparer<TEnum>.Default.Equals(Value, other.Value);
    }

    /// <inheritdoc/>
    public bool Equals(TEnum other)
    {
        return EqualityComparer<TEnum>.Default.Equals(Value, other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return (obj is AtomicEnum<TEnum> other && Equals(other))
            || (obj is TEnum otherValue && Equals(otherValue));
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value.ToString();
    }

    /// <summary>
    /// 比较两个 <see cref="AtomicEnum{TEnum}"/> 实例是否相等
    /// </summary>
    /// <param name="left">左操作数</param>
    /// <param name="right">右操作数</param>
    /// <returns><see langword="true"/> 如果两个操作数相等, 否则为 <see langword="false"/></returns>
    public static bool operator ==(AtomicEnum<TEnum> left, AtomicEnum<TEnum> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// 比较两个 <see cref="AtomicEnum{TEnum}"/> 实例是否不相等
    /// </summary>
    /// <param name="left">左操作数</param>
    /// <param name="right">右操作数</param>
    /// <returns><see langword="true"/> 如果两个操作数不等, 否则为 <see langword="false"/></returns>
    public static bool operator !=(AtomicEnum<TEnum> left, AtomicEnum<TEnum> right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// 比较 <see cref="AtomicEnum{TEnum}"/> 实例与 <typeparamref name="TEnum"/> 是否相等
    /// </summary>
    /// <param name="left">左操作数</param>
    /// <param name="right">右操作数</param>
    /// <returns><see langword="true"/> 如果两个操作数相等, 否则为 <see langword="false"/></returns>
    public static bool operator ==(AtomicEnum<TEnum> left, TEnum right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// 比较 <see cref="AtomicEnum{TEnum}"/> 实例与 <typeparamref name="TEnum"/> 是否不相等
    /// </summary>
    /// <param name="left">左操作数</param>
    /// <param name="right">右操作数</param>
    /// <returns><see langword="true"/> 如果两个操作数不等, 否则为 <see langword="false"/></returns>
    public static bool operator !=(AtomicEnum<TEnum> left, TEnum right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// 比较 <typeparamref name="TEnum"/> 与 <see cref="AtomicEnum{TEnum}"/> 实例是否相等
    /// </summary>
    /// <param name="left">左操作数</param>
    /// <param name="right">右操作数</param>
    /// <returns><see langword="true"/> 如果两个操作数相等, 否则为 <see langword="false"/></returns>
    public static bool operator ==(TEnum left, AtomicEnum<TEnum> right)
    {
        return right.Equals(left);
    }

    /// <summary>
    /// 比较 <typeparamref name="TEnum"/> 与 <see cref="AtomicEnum{TEnum}"/> 实例是否不相等
    /// </summary>
    /// <param name="left">左操作数</param>
    /// <param name="right">右操作数</param>
    /// <returns><see langword="true"/> 如果两个操作数不等, 否则为 <see langword="false"/></returns>
    public static bool operator !=(TEnum left, AtomicEnum<TEnum> right)
    {
        return !right.Equals(left);
    }

    /// <summary>
    /// 将 <see cref="AtomicEnum{TEnum}"/> 转换为 <typeparamref name="TEnum"/> 的隐式转换运算符
    /// </summary>
    /// <param name="atomicEnum">原子枚举</param>
    /// <returns>转换后的枚举值</returns>
    public static implicit operator TEnum(AtomicEnum<TEnum> atomicEnum)
    {
        return atomicEnum.Value;
    }

    /// <summary>
    /// 将 <typeparamref name="TEnum"/> 转换为 <see cref="AtomicEnum{TEnum}"/> 的隐式转换运算符
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>转换后的原子枚举</returns>
    public static implicit operator AtomicEnum<TEnum>(TEnum value)
    {
        return new(value);
    }
}
