# Postmark
## Leave transactional email delivery to the experts

Postmark helps deliver and track transactional emails for web applications. In a nutshell, the service replaces SMTP (or Sendmail) with a far more reliable, scalable and care-free environment. In addition, you can track statistics for number of emails sent, bounces and spam complaints.

The Postmark.NET NuGet package makes it easier to use the Postmark API from your .NET 2, 3.x, and 4.x projects without having to build your own API calls. You can get your free API token at http://www.postmarkapp.com

### Installaion
From NuGet

	PM> Install-Package Postmark

### Basic Usage
	
	using PostmarkDotNet;
	
	PostmarkMessage message = new PostmarkMessage
	{
	    From = "sender@example.com",
	    To = "receiver@example.com",
	    Subject = "Test",
	    HtmlBody = "Hello",
	    TextBody = "Hello",
	    ReplyTo = "reply@example.com",
	    Headers = new NameValueCollection {{"CUSTOM-HEADER", "value"}}
	};
	
	// Adding attachments
	// See http://developer.postmarkapp.com/developer-build.html#attachments for allowed extensions.
	message.AddAttachment(fileName, "Content-Type");
	
	PostmarkClient client = new PostmarkClient(my_server_token);
	
	PostmarkResponse response = client.SendMessage(message);
	
	if(response.Status != PostmarkStatus.Success)
	{
	    Console.WriteLine("Response was: " + response.Message);
	}

### Additional Usage
Please see the [wiki](https://github.com/jptoto/postmark-dotnet/wiki) for additional instruction about using the bounce api and additional sending options.

### Issues &amp; Comments
Please leave all comments, bugs, requests, and issues on the Issues page. We'll respond to your request ASAP!

### License
The Postmark .NET Library is licensed under the [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form") license. Refere to the [LICENSE](https://github.com/jptoto/postmark-dotnet/blob/master/LICENSE) file for more information.
