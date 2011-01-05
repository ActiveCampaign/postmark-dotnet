namespace PostmarkDotNet
{
    ///<summary>
    ///  Represents the raw SMTP details of a particular bounce.
    ///</summary>
    public class PostmarkBounceDump
    {
        /// <summary>
        ///   The body of the bounce dump.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }
    }
}