namespace Xiting.Atomic.Tests;

/// <summary>
/// <see cref="AtomicOperation"/> 的并发测试
/// </summary>
public sealed class AtomicOperationConcurrencyTests
{
    /// <summary>
    /// 并发递增的目标值
    /// </summary>
    private int _target;

    /// <summary>
    /// 验证多线程并发自旋递增不会丢失任何一次操作
    /// </summary>
    [Fact]
    public async Task ConcurrentIncrements_AllApplied()
    {
        const int threadCount = 8;
        const int operationsPerThread = 1000;

        var barrier = new Barrier(threadCount);
        var tasks = new Task[threadCount];
        for (var i = 0; i < threadCount; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                barrier.SignalAndWait();
                for (var j = 0; j < operationsPerThread; j++)
                {
                    while (!AtomicOperation.SpinPredicateAndSet(ref _target, _ => true, x => x + 1)) { }
                }
            }, TestContext.Current.CancellationToken);
        }

        await Task.WhenAll(tasks).WaitAsync(TestContext.Current.CancellationToken);

        Assert.Equal(threadCount * operationsPerThread, _target);
    }
}
