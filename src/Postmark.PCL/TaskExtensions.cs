using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public static class TaskExtensions
    {

        /// <summary>
        /// Escape async/await by runing the task synchronously, 
        /// will block the current thread until the task completes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static T WaitForResult<T>(this Task<T> task)
        {
            task.Wait();
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
            return task.Result;
        }
    }
}
