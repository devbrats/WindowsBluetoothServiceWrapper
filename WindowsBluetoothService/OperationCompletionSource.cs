using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace WindowsBluetoothService
{
    /// <summary>
    /// Wrapper class for TaskCompletionSource. This can be used for operation which doesn't contain GetAwaiter.
    /// </summary>
    /// <typeparam name="T">Type of Result expected from operation.</typeparam>
    public class OperationCompletionSource<T>
    {
        private readonly TaskCompletionSource<T> taskCompletion;

        /// <summary>
        /// Result Task.
        /// </summary>
        public Task<T> Result
        {
            get { return taskCompletion.Task; }
        }

        public OperationCompletionSource(IAsyncOperation<T> operation)
        {
            taskCompletion= new TaskCompletionSource<T>();
            operation.Completed = OnOperationComplete;
        }

        private void OnOperationComplete(IAsyncOperation<T> asyncInfo, AsyncStatus asyncStatus)
        {
            if (asyncStatus == AsyncStatus.Completed)
            {
                taskCompletion.TrySetResult(asyncInfo.GetResults());
            }
            else
            {
                taskCompletion.TrySetException(new Exception("Failed to complete operation."));
            }
        }
    }
}