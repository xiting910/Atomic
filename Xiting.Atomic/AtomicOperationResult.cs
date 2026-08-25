namespace Xiting.Atomic;

/// <summary>
/// 原子操作的结果
/// </summary>
public enum AtomicOperationResult : byte
{
    /// <summary>
    /// 已经是指定的值, 操作未执行
    /// </summary>
    AlreadySet,

    /// <summary>
    /// 谓词条件不满足, 操作未执行
    /// </summary>
    PredicateFailed,

    /// <summary>
    /// 发生竞态条件, 操作未成功
    /// </summary>
    RaceCondition,

    /// <summary>
    /// 操作成功, 当前值发生了变化
    /// </summary>
    Success
}
