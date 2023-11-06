namespace Website
{
    //TODO potential to work https://devblogs.microsoft.com/pfxteam/crafting-a-task-timeoutafter-method/
    //creates time out for task especial js task
    internal struct VoidTypeStruct { }  // See Footnote #1

    public static class TaskExtensions
    {

        public static Task TimeoutAfter(this Task task, int millisecondsTimeout)
        {
            // tcs.Task will be returned as a proxy to the caller
            TaskCompletionSource<VoidTypeStruct> tcs =
                new TaskCompletionSource<VoidTypeStruct>();

            // Set up a timer to complete after the specified timeout period
            Timer timer = new Timer(state =>
            {
                // Recover our state data
                var myTcs = (TaskCompletionSource<VoidTypeStruct>)state;

                // Fault our proxy Task with a TimeoutException
                myTcs.TrySetException(new TimeoutException());
            }, tcs, millisecondsTimeout, Timeout.Infinite);

            // Wire up the logic for what happens when source task completes
            task.ContinueWith((antecedent, state) =>
                {
                    // Recover our state data
                    var tuple =
                        (Tuple<Timer, TaskCompletionSource<VoidTypeStruct>>)state;

                    // Cancel the timer
                    tuple.Item1.Dispose();
                    // Marshal results to proxy
                    MarshalTaskResults(antecedent, tuple.Item2);
                },
                Tuple.Create(timer, tcs),  // See Footnote #2
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);

            return tcs.Task;
        }

        internal static void MarshalTaskResults<TResult>(
            Task source, TaskCompletionSource<TResult> proxy)
        {
            switch (source.Status)
            {
                case TaskStatus.Faulted:
                    proxy.TrySetException(source.Exception);
                    break;
                case TaskStatus.Canceled:
                    proxy.TrySetCanceled();
                    break;
                case TaskStatus.RanToCompletion:
                    Task<TResult> castedSource = source as Task<TResult>;
                    proxy.TrySetResult(
                        castedSource == null ? default(TResult) : // source is a Task
                            castedSource.Result); // source is a Task<TResult>
                    break;
            }
        }

    }
}
