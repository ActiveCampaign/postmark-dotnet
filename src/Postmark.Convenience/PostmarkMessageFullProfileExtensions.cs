using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public static class PostmarkMessageFullProfileExtensions
    {
        /// <summary>
        /// Add a file from the specified path to this message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="path"></param>
        /// <param name="contentType"></param>
        /// <param name="contentId"></param>
        public static void AddAttachment(this PostmarkMessage message,
            string path, string contentType = null, string contentId = null)
        {
            var fi = new FileInfo(path);
            var content = File.ReadAllBytes(path);
            message.AddAttachment(content, fi.Name, contentType, contentId);
        }
    }
}
