using System;
using System.Collections.Generic;
using System.Threading;

namespace Xiting.Atomic;

/// <summary>
/// 提供原子操作的静态方法
/// </summary>
/// <remarks>
/// 支持操作的类型包括: <see cref="byte"/>, <see cref="sbyte"/>, <see cref="short"/>, <see cref="ushort"/>,
/// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>
/// 以及类型为引用类型、primitive 类型或枚举类型的泛型类型
/// </remarks>
public static class AtomicOperation
{
    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet(ref byte target, Func<byte, bool> predicate, byte newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        byte currentValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet(ref byte target, Func<byte, bool> predicate, Func<byte, byte> newValueFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        byte currentValue, newValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            newValue = newValueFactory(currentValue);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet(ref sbyte target, Func<sbyte, bool> predicate, sbyte newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        sbyte currentValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet(ref sbyte target, Func<sbyte, bool> predicate, Func<sbyte, sbyte> newValueFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        sbyte currentValue, newValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            newValue = newValueFactory(currentValue);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet(ref short target, Func<short, bool> predicate, short newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        short currentValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet(ref short target, Func<short, bool> predicate, Func<short, short> newValueFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        short currentValue, newValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            newValue = newValueFactory(currentValue);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet(ref ushort target, Func<ushort, bool> predicate, ushort newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        ushort currentValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet(ref ushort target, Func<ushort, bool> predicate, Func<ushort, ushort> newValueFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        ushort currentValue, newValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            newValue = newValueFactory(currentValue);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet(ref int target, Func<int, bool> predicate, int newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        int currentValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet(ref int target, Func<int, bool> predicate, Func<int, int> newValueFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        int currentValue, newValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            newValue = newValueFactory(currentValue);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet(ref uint target, Func<uint, bool> predicate, uint newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        uint currentValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet(ref uint target, Func<uint, bool> predicate, Func<uint, uint> newValueFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        uint currentValue, newValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            newValue = newValueFactory(currentValue);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet(ref long target, Func<long, bool> predicate, long newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        long currentValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet(ref long target, Func<long, bool> predicate, Func<long, long> newValueFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        long currentValue, newValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            newValue = newValueFactory(currentValue);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet(ref ulong target, Func<ulong, bool> predicate, ulong newValue)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        ulong currentValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet(ref ulong target, Func<ulong, bool> predicate, Func<ulong, ulong> newValueFactory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        ulong currentValue, newValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Volatile.Read(ref target);
            newValue = newValueFactory(currentValue);
            if (currentValue == newValue || !predicate(currentValue))
            {
                return false;
            }
            if (Interlocked.CompareExchange(ref target, newValue, currentValue) == currentValue)
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <remarks>
    /// 对于枚举类型, 更推荐创建 <see cref="AtomicEnum{TEnum}"/> 结构体进行原子操作
    /// </remarks>
    /// <typeparam name="T">要操作的目标值类型, 必须是引用类型、primitive 类型或枚举类型</typeparam>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValue">要设置的新值</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    public static bool SpinPredicateAndSet<T>(ref T target, Func<T, bool> predicate, T newValue)
    {
        var equalityComparer = GetEqualityComparer<T>();
        ArgumentNullException.ThrowIfNull(predicate);

        T currentValue, originalValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Interlocked.CompareExchange(ref target, default!, default!);
            if (equalityComparer(currentValue, newValue) || !predicate(currentValue))
            {
                return false;
            }
            originalValue = Interlocked.CompareExchange(ref target, newValue, currentValue);
            if (equalityComparer(originalValue, currentValue))
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 使用自旋模式, 如果当前值满足条件, 则将值设置为新值
    /// </summary>
    /// <remarks>
    /// 对于枚举类型, 更推荐创建 <see cref="AtomicEnum{TEnum}"/> 结构体进行原子操作
    /// </remarks>
    /// <typeparam name="T">要操作的目标值类型, 必须是引用类型、primitive 类型或枚举类型</typeparam>
    /// <param name="target">要操作的目标值</param>
    /// <param name="predicate">判断条件</param>
    /// <param name="newValueFactory">用于生成新值的函数</param>
    /// <returns><see langword="true"/> 如果当前值发生了变化, 否则为 <see langword="false"/></returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="newValueFactory"/></exception>
    public static bool SpinPredicateAndSet<T>(ref T target, Func<T, bool> predicate, Func<T, T> newValueFactory)
    {
        var equalityComparer = GetEqualityComparer<T>();
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(newValueFactory);

        T currentValue, newValue, originalValue;
        var spinWait = new SpinWait();
        while (true)
        {
            currentValue = Interlocked.CompareExchange(ref target, default!, default!);
            newValue = newValueFactory(currentValue);
            if (equalityComparer(currentValue, newValue) || !predicate(currentValue))
            {
                return false;
            }
            originalValue = Interlocked.CompareExchange(ref target, newValue, currentValue);
            if (equalityComparer(originalValue, currentValue))
            {
                return true;
            }
            spinWait.SpinOnce();
        }
    }

    /// <summary>
    /// 获取类型 <typeparamref name="T"/> 的相等比较器
    /// </summary>
    /// <typeparam name="T">要操作的目标值类型</typeparam>
    /// <returns>相等比较器</returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/></exception>
    private static Func<T, T, bool> GetEqualityComparer<T>()
    {
        var type = typeof(T);
        return type.IsValueType
            ? type.IsPrimitive || type.IsEnum
                ? EqualityComparer<T>.Default.Equals
                : throw new NotSupportedException($"类型 {type.FullName} 不支持自旋模式的原子操作")
            : ((x, y) => ReferenceEquals(x, y));
    }
}
