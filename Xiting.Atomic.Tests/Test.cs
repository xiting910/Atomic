namespace Xiting.Atomic.Tests;

/// <summary>
/// 测试用的普通枚举
/// </summary>
public enum Test
{
    None = 0,
    ValueA = 1,
    ValueB = 2,
    ValueC = 3
}

/// <summary>
/// 测试用的标记枚举
/// </summary>
[Flags]
public enum FlagsTest
{
    None = 0,
    A = 1 << 0,
    B = 1 << 1
}

/// <summary>
/// 测试用的不支持原子操作的自定义结构体
/// </summary>
/// <param name="Value">值</param>
public readonly record struct UnsupportedStruct(int Value);
