namespace Xiting.Atomic.Tests;

/// <summary>
/// <see cref="AtomicEnum{TEnum}"/> 的并发测试
/// </summary>
public sealed class AtomicEnumConcurrencyTests
{
    /// <summary>
    /// 并发递增的目标值
    /// </summary>
    private AtomicEnum<Test> _target;

    /// <summary>
    /// 验证多线程并发自旋递增不会丢失任何一次操作
    /// </summary>
    [Fact]
    public async Task ConcurrentIncrements_AllApplied()
    {
        const int threadCount = 8;
        const int operationsPerThread = 500;

        var barrier = new Barrier(threadCount);
        var tasks = new Task[threadCount];
        for (var i = 0; i < threadCount; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                barrier.SignalAndWait();
                for (var j = 0; j < operationsPerThread; j++)
                {
                    while (!_target.SpinPredicateAndSet(
                        _ => true, x => (Test)(Convert.ToInt32(x) + 1), out _, out _
                    )) { }
                }
            }, TestContext.Current.CancellationToken);
        }

        await Task.WhenAll(tasks).WaitAsync(TestContext.Current.CancellationToken);

        Assert.Equal(threadCount * operationsPerThread, (int)_target.Value);
    }
}
