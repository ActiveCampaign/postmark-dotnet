namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// Basic HTTP Authentication
    /// </summary>
    public class HttpAuth
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}