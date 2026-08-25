namespace Xiting.Atomic.Tests;

/// <summary>
/// <see cref="AtomicOperation"/> 的 primitive 类型重载单元测试
/// </summary>
public sealed class AtomicOperationPrimitiveTests
{
    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref byte, Func{byte, bool}, byte)"/>
    /// </summary>
    [Fact]
    public void Byte_SpinPredicateAndSet_Works()
    {
        byte target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal((byte)5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal((byte)5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x > 10, 6));
        Assert.Equal((byte)5, target);
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref byte, Func{byte, bool}, Func{byte, byte})"/>
    /// </summary>
    [Fact]
    public void Byte_SpinPredicateAndSet_WithFactory_Works()
    {
        byte target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 1, x => (byte)(x + 1)));
        Assert.Equal((byte)2, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 5, x => (byte)(x + 1)));
        Assert.Equal((byte)2, target);

        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, null!, 3));
        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, x => true, null!));
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref sbyte, Func{sbyte, bool}, sbyte)"/>
    /// </summary>
    [Fact]
    public void SByte_SpinPredicateAndSet_Works()
    {
        sbyte target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal((sbyte)5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal((sbyte)5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x > 10, 6));
        Assert.Equal((sbyte)5, target);
    }

    /// <summary>
    /// 验证
    /// <see cref="AtomicOperation.SpinPredicateAndSet(ref sbyte, Func{sbyte, bool}, Func{sbyte, sbyte})"/>
    /// </summary>
    [Fact]
    public void SByte_SpinPredicateAndSet_WithFactory_Works()
    {
        sbyte target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 1, x => (sbyte)(x + 1)));
        Assert.Equal((sbyte)2, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 5, x => (sbyte)(x + 1)));
        Assert.Equal((sbyte)2, target);

        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, null!, 3));
        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, x => true, null!));
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref short, Func{short, bool}, short)"/>
    /// </summary>
    [Fact]
    public void Short_SpinPredicateAndSet_Works()
    {
        short target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal((short)5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal((short)5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x > 10, 6));
        Assert.Equal((short)5, target);
    }

    /// <summary>
    /// 验证
    /// <see cref="AtomicOperation.SpinPredicateAndSet(ref short, Func{short, bool}, Func{short, short})"/>
    /// </summary>
    [Fact]
    public void Short_SpinPredicateAndSet_WithFactory_Works()
    {
        short target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 1, x => (short)(x + 1)));
        Assert.Equal((short)2, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 5, x => (short)(x + 1)));
        Assert.Equal((short)2, target);

        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, null!, 3));
        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, x => true, null!));
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref ushort, Func{ushort, bool}, ushort)"/>
    /// </summary>
    [Fact]
    public void UShort_SpinPredicateAndSet_Works()
    {
        ushort target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal((ushort)5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal((ushort)5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x > 10, 6));
        Assert.Equal((ushort)5, target);
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref ushort, Func{ushort, bool},
    /// Func{ushort, ushort})"/>
    /// </summary>
    [Fact]
    public void UShort_SpinPredicateAndSet_WithFactory_Works()
    {
        ushort target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 1, x => (ushort)(x + 1)));
        Assert.Equal((ushort)2, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 5, x => (ushort)(x + 1)));
        Assert.Equal((ushort)2, target);

        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, null!, 3));
        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, x => true, null!));
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref int, Func{int, bool}, int)"/>
    /// </summary>
    [Fact]
    public void Int_SpinPredicateAndSet_Works()
    {
        var target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal(5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5));
        Assert.Equal(5, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x > 10, 6));
        Assert.Equal(5, target);
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref int, Func{int, bool}, Func{int, int})"/>
    /// </summary>
    [Fact]
    public void Int_SpinPredicateAndSet_WithFactory_Works()
    {
        var target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 1, x => x + 1));
        Assert.Equal(2, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 5, x => x + 1));
        Assert.Equal(2, target);

        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, null!, 3));
        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, x => true, null!));
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref uint, Func{uint, bool}, uint)"/>
    /// </summary>
    [Fact]
    public void UInt_SpinPredicateAndSet_Works()
    {
        uint target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5u));
        Assert.Equal(5u, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5u));
        Assert.Equal(5u, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x > 10, 6u));
        Assert.Equal(5u, target);
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref uint, Func{uint, bool}, Func{uint, uint})"/>
    /// </summary>
    [Fact]
    public void UInt_SpinPredicateAndSet_WithFactory_Works()
    {
        uint target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 1, x => x + 1));
        Assert.Equal(2u, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 5, x => x + 1));
        Assert.Equal(2u, target);

        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, null!, 3u));
        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, x => true, null!));
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref long, Func{long, bool}, long)"/>
    /// </summary>
    [Fact]
    public void Long_SpinPredicateAndSet_Works()
    {
        long target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5L));
        Assert.Equal(5L, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5L));
        Assert.Equal(5L, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x > 10, 6L));
        Assert.Equal(5L, target);
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref long, Func{long, bool}, Func{long, long})"/>
    /// </summary>
    [Fact]
    public void Long_SpinPredicateAndSet_WithFactory_Works()
    {
        long target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 1, x => x + 1));
        Assert.Equal(2L, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 5, x => x + 1));
        Assert.Equal(2L, target);

        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, null!, 3L));
        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, x => true, null!));
    }

    /// <summary>
    /// 验证 <see cref="AtomicOperation.SpinPredicateAndSet(ref ulong, Func{ulong, bool}, ulong)"/>
    /// </summary>
    [Fact]
    public void ULong_SpinPredicateAndSet_Works()
    {
        ulong target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5UL));
        Assert.Equal(5UL, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x < 10, 5UL));
        Assert.Equal(5UL, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x > 10, 6UL));
        Assert.Equal(5UL, target);
    }

    /// <summary>
    /// 验证
    /// <see cref="AtomicOperation.SpinPredicateAndSet(ref ulong, Func{ulong, bool}, Func{ulong, ulong})"/>
    /// </summary>
    [Fact]
    public void ULong_SpinPredicateAndSet_WithFactory_Works()
    {
        ulong target = 1;

        Assert.True(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 1, x => x + 1));
        Assert.Equal(2UL, target);

        Assert.False(AtomicOperation.SpinPredicateAndSet(ref target, x => x == 5, x => x + 1));
        Assert.Equal(2UL, target);

        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, null!, 3UL));
        _ = Assert.Throws<ArgumentNullException>(() => AtomicOperation.SpinPredicateAndSet(ref target, x => true, null!));
    }
}
