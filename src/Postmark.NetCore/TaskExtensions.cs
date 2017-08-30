using System;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public static class TaskExtensions
    {

        /// <summary>
        /// Escape async/await by runing the task synchronously, 
        /// will block the current thread until the task completes.
        /// 
        /// DO NOT USE THIS IN A UI THREAD.
        /// It will cause your application to become deadlocked.
        /// <see cref="http://blogs.msdn.com/b/pfxteam/archive/2011/01/13/10115163.aspx"/>
        /// </summary>
        /// <remarks>
        /// You should avoid using this method in new code. If you're unable to use async/await support,
        /// consider using "ContinueWith()" <see cref="https://msdn.microsoft.com/en-us/library/dd321474(v=vs.110).aspx"/> 
        /// in order to ensure proper sequencing operations.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        [Obsolete("This convenience method has been deprecated" +
            " because it is known cause issues in specific use cases.")]
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
