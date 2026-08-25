namespace Xiting.Atomic.Tests;

/// <summary>
/// <see cref="AtomicOperation"/> 的泛型重载单元测试
/// </summary>
public sealed class AtomicOperationGenericTests
{
    /// <summary>
    /// 验证引用类型使用引用相等语义
    /// </summary>
    [Fact]
    public void ReferenceType_UsesReferenceEquality()
    {
        var first = new object();
        var second = new object();
        var target = first;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => ReferenceEquals(x, first), second));
        Assert.Same(second, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => ReferenceEquals(x, first), first));
        Assert.Same(second, target);
    }

    /// <summary>
    /// 验证引用类型的 <see langword="null"/> 目标值正常工作
    /// </summary>
    [Fact]
    public void ReferenceType_NullTarget_Works()
    {
        string? target = null;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x is null, "value"));
        Assert.Equal("value", target);
    }

    /// <summary>
    /// 验证可空引用类型的 null 值可以正常设置和读取
    /// </summary>
    [Fact]
    public void ReferenceType_NullValue_Works()
    {
        var target = "value";

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == "value", (string?)null));
        Assert.Null(target);

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x is null, "restored"));
        Assert.Equal("restored", target);

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == "restored", _ => null));
        Assert.Null(target);
    }

    /// <summary>
    /// 验证 primitive 类型正常工作
    /// </summary>
    [Fact]
    public void Primitive_Works()
    {
        var target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet<int>(ref target, x => x == 1, 2));
        Assert.Equal(2, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet<int>(ref target, x => x == 1, 3));
        Assert.Equal(2, target);
    }

    /// <summary>
    /// 验证枚举类型正常工作
    /// </summary>
    [Fact]
    public void Enum_Works()
    {
        var target = Test.ValueA;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x is Test.ValueA, Test.ValueB));
        Assert.Equal(Test.ValueB, target);
    }

    /// <summary>
    /// 验证不支持的引用类型以外的结构体抛出异常
    /// </summary>
    [Fact]
    public void UnsupportedStruct_Throws()
    {
        UnsupportedStruct target = new(1);

        _ = Assert.Throws<NotSupportedException>(() =>
            AtomicOperation.SpinPredicateAndSet(ref target, x => true, new UnsupportedStruct(2))
        );
    }

    /// <summary>
    /// 验证 <see langword="null"/> 参数抛出异常
    /// </summary>
    [Fact]
    public void NullArguments_Throws()
    {
        var target = 1;

        _ = Assert.Throws<ArgumentNullException>(() =>
            AtomicOperation.SpinPredicateAndSet<int>(ref target, null!, 2)
        );
        _ = Assert.Throws<ArgumentNullException>(() =>
            AtomicOperation.SpinPredicateAndSet<int>(ref target, _ => true, null!)
        );
    }
}
