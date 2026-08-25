namespace Xiting.Atomic.Tests;

/// <summary>
/// <see cref="AtomicEnum{TEnum}"/> 的单元测试
/// </summary>
public sealed class AtomicEnumTests
{
    /// <summary>
    /// 验证默认实例的值为默认枚举值
    /// </summary>
    [Fact]
    public void DefaultInstance_ValueIsDefault()
    {
        Assert.Equal(Test.None, default(AtomicEnum<Test>).Value);
        Assert.Equal(Test.None, new AtomicEnum<Test>().Value);
    }

    /// <summary>
    /// 验证构造函数设置初始值
    /// </summary>
    [Fact]
    public void Constructor_SetsInitialValue()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueB);

        Assert.Equal(Test.ValueB, atomic.Value);
    }

    /// <summary>
    /// 验证拷贝构造函数复制值
    /// </summary>
    [Fact]
    public void CopyConstructor_CopiesValue()
    {
        var original = new AtomicEnum<Test>(Test.ValueA);
        var copy = new AtomicEnum<Test>(original);

        Assert.Equal(Test.ValueA, copy.Value);
    }

    /// <summary>
    /// 验证 <see cref="AtomicEnum{TEnum}.Value"/> 属性的读写
    /// </summary>
    [Fact]
    public void Value_SetAndGet()
    {
        var atomic = new AtomicEnum<Test>(Test.None)
        {
            Value = Test.ValueC
        };

        Assert.Equal(Test.ValueC, atomic.Value);
    }

    /// <summary>
    /// 验证值发生变化时 <see cref="AtomicEnum{TEnum}.Set(TEnum, out TEnum)"/> 返回
    /// <see langword="true"/> 并输出原始值
    /// </summary>
    [Fact]
    public void Set_WhenValueChanges_ReturnsTrueAndOutputsOriginalValue()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.Set(Test.ValueB, out var original);

        Assert.True(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueB, atomic.Value);
    }

    /// <summary>
    /// 验证值相同时 <see cref="AtomicEnum{TEnum}.Set(TEnum, out TEnum)"/> 返回 <see langword="false"/>
    /// </summary>
    [Fact]
    public void Set_WhenValueSame_ReturnsFalse()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.Set(Test.ValueA, out var original);

        Assert.False(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueA, atomic.Value);
    }

    /// <summary>
    /// 验证当前值匹配时 <see cref="AtomicEnum{TEnum}.CompareAndSet(TEnum, TEnum, out TEnum)"/> 成功
    /// </summary>
    [Fact]
    public void CompareAndSet_WhenMatch_Succeeds()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.CompareAndSet(Test.ValueA, Test.ValueB, out var original);

        Assert.True(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueB, atomic.Value);
    }

    /// <summary>
    /// 验证当前值不匹配时 <see cref="AtomicEnum{TEnum}.CompareAndSet(TEnum, TEnum, out TEnum)"/>
    /// 失败且不改变值
    /// </summary>
    [Fact]
    public void CompareAndSet_WhenMismatch_Fails()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.CompareAndSet(Test.ValueB, Test.ValueC, out var original);

        Assert.False(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueA, atomic.Value);
    }

    /// <summary>
    /// 验证比较值与新值相同时 <see cref="AtomicEnum{TEnum}.CompareAndSet(TEnum, TEnum, out TEnum)"/>
    /// 返回 <see langword="false"/>
    /// </summary>
    [Fact]
    public void CompareAndSet_WhenComparisonEqualsNewValue_ReturnsFalse()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.CompareAndSet(Test.ValueA, Test.ValueA, out var original);

        Assert.False(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueA, atomic.Value);
    }

    /// <summary>
    /// 验证谓词满足时 <see cref="AtomicEnum{TEnum}.PredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/>
    /// 返回 <see cref="AtomicOperationResult.Success"/>
    /// </summary>
    [Fact]
    public void PredicateAndSet_WhenPredicateTrue_ReturnsSuccess()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var result = atomic.PredicateAndSet(x => x is Test.ValueA, Test.ValueB, out var original);

        Assert.Equal(AtomicOperationResult.Success, result);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueB, atomic.Value);
    }

    /// <summary>
    /// 验证谓词不满足时 <see cref="AtomicEnum{TEnum}.PredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/>
    /// 返回 <see cref="AtomicOperationResult.PredicateFailed"/>
    /// </summary>
    [Fact]
    public void PredicateAndSet_WhenPredicateFalse_ReturnsPredicateFailed()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var result = atomic.PredicateAndSet(x => x is Test.ValueB, Test.ValueC, out var original);

        Assert.Equal(AtomicOperationResult.PredicateFailed, result);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueA, atomic.Value);
    }

    /// <summary>
    /// 验证当前值已等于新值时
    /// <see cref="AtomicEnum{TEnum}.PredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/>
    /// 返回 <see cref="AtomicOperationResult.AlreadySet"/>
    /// </summary>
    [Fact]
    public void PredicateAndSet_WhenValueAlreadySet_ReturnsAlreadySet()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var result = atomic.PredicateAndSet(x => true, Test.ValueA, out var original);

        Assert.Equal(AtomicOperationResult.AlreadySet, result);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueA, atomic.Value);
    }

    /// <summary>
    /// 验证使用工厂方法的
    /// <see cref="AtomicEnum{TEnum}.PredicateAndSet(Func{TEnum, bool}, Func{TEnum, TEnum}, out TEnum)"/>
    /// 正常工作
    /// </summary>
    [Fact]
    public void PredicateAndSet_WithFactory_ReturnsSuccess()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var result = atomic.PredicateAndSet(x => x is Test.ValueA, _ => Test.ValueB, out var original);

        Assert.Equal(AtomicOperationResult.Success, result);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueB, atomic.Value);
    }

    /// <summary>
    /// 验证
    /// <see cref="AtomicEnum{TEnum}.PredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/>
    /// 的 <see langword="null"/> 参数抛出异常
    /// </summary>
    [Fact]
    public void PredicateAndSet_NullArguments_Throws()
    {
        var atomic = new AtomicEnum<Test>(Test.None);

        _ = Assert.Throws<ArgumentNullException>(() => atomic.PredicateAndSet(null!, Test.ValueA, out _));
        _ = Assert.Throws<ArgumentNullException>(() => atomic.PredicateAndSet(null!, _ => Test.ValueA, out _));
        _ = Assert.Throws<ArgumentNullException>(() => atomic.PredicateAndSet(x => true, null!, out _));
    }

    /// <summary>
    /// 验证谓词满足时
    /// <see cref="AtomicEnum{TEnum}.SpinPredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/>
    /// 返回 <see langword="true"/>
    /// </summary>
    [Fact]
    public void SpinPredicateAndSet_WhenPredicateTrue_ReturnsTrue()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.SpinPredicateAndSet(x => x is Test.ValueA, Test.ValueB, out var original);

        Assert.True(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueB, atomic.Value);
    }

    /// <summary>
    /// 验证谓词不满足时
    /// <see cref="AtomicEnum{TEnum}.SpinPredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/>
    /// 返回 <see langword="false"/>
    /// </summary>
    [Fact]
    public void SpinPredicateAndSet_WhenPredicateFalse_ReturnsFalse()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.SpinPredicateAndSet(x => x is Test.ValueC, Test.ValueB, out var original);

        Assert.False(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueA, atomic.Value);
    }

    /// <summary>
    /// 验证使用工厂方法的 <see cref="AtomicEnum{TEnum}.SpinPredicateAndSet(Func{TEnum, bool},
    /// Func{TEnum, TEnum}, out TEnum, out TEnum)"/> 输出最终值
    /// </summary>
    [Fact]
    public void SpinPredicateAndSet_WithFactory_OutputsFinalValue()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.SpinPredicateAndSet(x => x is Test.ValueA, _ => Test.ValueB, out var original, out var final);

        Assert.True(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueB, final);
        Assert.Equal(Test.ValueB, atomic.Value);
    }

    /// <summary>
    /// 验证谓词失败时 <see cref="AtomicEnum{TEnum}.SpinPredicateAndSet(Func{TEnum, bool},
    /// Func{TEnum, TEnum}, out TEnum, out TEnum)"/> 的最终值为当前值
    /// </summary>
    [Fact]
    public void SpinPredicateAndSet_WithFactory_PredicateFailed_FinalValueIsCurrent()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        var changed = atomic.SpinPredicateAndSet(x => x is Test.ValueC, _ => Test.ValueB, out var original, out var final);

        Assert.False(changed);
        Assert.Equal(Test.ValueA, original);
        Assert.Equal(Test.ValueA, final);
        Assert.Equal(Test.ValueA, atomic.Value);
    }

    /// <summary>
    /// 验证 <see cref="AtomicEnum{TEnum}.SpinPredicateAndSet(Func{TEnum, bool}, TEnum, out TEnum)"/>
    /// 的 <see langword="null"/> 参数抛出异常
    /// </summary>
    [Fact]
    public void SpinPredicateAndSet_NullArguments_Throws()
    {
        var atomic = new AtomicEnum<Test>(Test.None);

        _ = Assert.Throws<ArgumentNullException>(() => atomic.SpinPredicateAndSet(null!, Test.ValueA, out _));
        _ = Assert.Throws<ArgumentNullException>(() => atomic.SpinPredicateAndSet(null!, _ => Test.ValueA, out _, out _));
        _ = Assert.Throws<ArgumentNullException>(() => atomic.SpinPredicateAndSet(x => true, null!, out _, out _));
    }

    /// <summary>
    /// 验证 <see cref="AtomicEnum{TEnum}.Equals(AtomicEnum{TEnum})"/>
    /// </summary>
    [Fact]
    public void Equals_AtomicEnum_Works()
    {
        var a = new AtomicEnum<Test>(Test.ValueA);
        var b = new AtomicEnum<Test>(Test.ValueA);
        var c = new AtomicEnum<Test>(Test.ValueB);

        Assert.True(a.Equals(b));
        Assert.False(a.Equals(c));
    }

    /// <summary>
    /// 验证 <see cref="AtomicEnum{TEnum}.Equals(TEnum)"/>
    /// </summary>
    [Fact]
    public void Equals_TEnum_Works()
    {
        var a = new AtomicEnum<Test>(Test.ValueA);

        Assert.True(a.Equals(Test.ValueA));
        Assert.False(a.Equals(Test.ValueB));
    }

    /// <summary>
    /// 验证 <see cref="AtomicEnum{TEnum}.Equals(object)"/>
    /// </summary>
    [Fact]
    public void Equals_Object_Works()
    {
        var a = new AtomicEnum<Test>(Test.ValueA);
        var b = new AtomicEnum<Test>(Test.ValueA);
        var c = new AtomicEnum<Test>(Test.ValueB);

        Assert.True(a.Equals((object)b));
        Assert.False(a.Equals((object)c));
        Assert.False(a.Equals(null));
    }

    /// <summary>
    /// 验证与装箱枚举比较
    /// </summary>
    [Fact]
    public void Equals_Object_BoxedEnum_Works()
    {
        var a = new AtomicEnum<Test>(Test.ValueA);

        Assert.True(a.Equals((object)Test.ValueA));
        Assert.False(a.Equals((object)Test.ValueB));
    }

    /// <summary>
    /// 验证相等运算符
    /// </summary>
    [Fact]
    public void Operators_Work()
    {
        var a = new AtomicEnum<Test>(Test.ValueA);
        var b = new AtomicEnum<Test>(Test.ValueA);
        var c = new AtomicEnum<Test>(Test.ValueB);

        Assert.True(a == b);
        Assert.False(a == c);
        Assert.True(a != c);

#pragma warning disable xUnit2024 // Do not use boolean asserts for simple equality tests
        Assert.True(a == Test.ValueA);
        Assert.False(a == Test.ValueB);
        Assert.True(a != Test.ValueB);

        Assert.True(Test.ValueA == a);
        Assert.False(Test.ValueB == a);
        Assert.True(Test.ValueB != a);
#pragma warning restore xUnit2024 // Do not use boolean asserts for simple equality tests
    }

    /// <summary>
    /// 验证隐式转换
    /// </summary>
    [Fact]
    public void ImplicitConversions_Work()
    {
        AtomicEnum<Test> atomic = Test.ValueC;
        Assert.Equal(Test.ValueC, atomic.Value);

        Test value = new AtomicEnum<Test>(Test.ValueB);
        Assert.Equal(Test.ValueB, value);
    }

    /// <summary>
    /// 验证 <see cref="AtomicEnum{TEnum}.GetHashCode"/> 与枚举一致
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEnum()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        Assert.Equal(Test.ValueA.GetHashCode(), atomic.GetHashCode());
    }

    /// <summary>
    /// 验证 <see cref="AtomicEnum{TEnum}.ToString"/> 返回枚举名称
    /// </summary>
    [Fact]
    public void ToString_ReturnsEnumName()
    {
        var atomic = new AtomicEnum<Test>(Test.ValueA);

        Assert.Equal(Test.ValueA.ToString(), atomic.ToString());
    }

    /// <summary>
    /// 验证标记枚举的原子操作
    /// </summary>
    [Fact]
    public void FlagsEnum_Works()
    {
        var atomic = new AtomicEnum<FlagsTest>(FlagsTest.A);

        Assert.True(atomic.Set(FlagsTest.A | FlagsTest.B, out _));
        Assert.Equal(FlagsTest.A | FlagsTest.B, atomic.Value);

        Assert.True(atomic.CompareAndSet(FlagsTest.A | FlagsTest.B, FlagsTest.B, out var original));
        Assert.Equal(FlagsTest.A | FlagsTest.B, original);
        Assert.Equal(FlagsTest.B, atomic.Value);
    }
}
