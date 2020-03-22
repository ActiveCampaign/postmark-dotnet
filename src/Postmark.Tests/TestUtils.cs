using System;
using System.Threading.Tasks;

namespace Postmark.Tests
{
    /// <summary>
    /// Utility methods useful in integration and unit tests
    /// </summary>
    public static class TestUtils
    {
        public static T MakeSynchronous<T>(Func<Task<T>> t)
        {
            var f = Task.Run(async () => await t().ConfigureAwait(false));
            f.Wait();
            return f.Result;
        }

        /// <summary>
        /// Run a task until the provided condition is met or the number of retries reaches 0.
        /// </summary>
        /// <typeparam name="T">Type of the returned result.</typeparam>
        /// <param name="pollingTaskFunc">Function that generates the task to be polled.</param>
        /// <param name="isComplete">Condition that stops the polling if true.</param>
        /// <param name="retriesLeft">Number of retries left. Defaults to 5.</param>
        /// <param name="delayInMs">Number of milliseconds to add delay before the next poll attempt. Defaults to 1000ms.</param>
        /// <returns></returns>
        public static async Task<T> PollUntil<T>(Func<Task<T>> pollingTaskFunc, Func<T, bool> isComplete, int retriesLeft = 5,
            int delayInMs = 1000)
        {
            var result = await pollingTaskFunc();

            if (isComplete(result) || retriesLeft == 0)
            {
                return result;
            }

            await Task.Delay(delayInMs).ConfigureAwait(false);

            return await PollUntil(pollingTaskFunc, isComplete, --retriesLeft);
        }
    }
}
