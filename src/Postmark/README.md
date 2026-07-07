# Postmark .NET

The official .NET client for [Postmark](https://postmarkapp.com). Postmark helps
you deliver and track transactional and broadcast email for your applications,
replacing SMTP with a reliable, scalable HTTP API. This package wraps the
Postmark API so you can send email, manage templates, read bounces, configure
webhooks, and more without building your own HTTP calls.

Get a free API token at https://postmarkapp.com.

## Installation

```
dotnet add package PostmarkDotNet
```

## Quick start

```csharp
using PostmarkDotNet;

var client = new PostmarkClient("your-server-token");

var message = new PostmarkMessage
{
    To = "recipient@example.com",
    From = "sender@example.com",
    Subject = "Hello from Postmark",
    TextBody = "This is a test message.",
    HtmlBody = "<strong>This is a test message.</strong>",
    MessageStream = "outbound"
};

var response = await client.SendMessageAsync(message);
```

See the [wiki](https://github.com/ActiveCampaign/postmark-dotnet/wiki) for guides on
sending email, using the bounce API, templates, and additional options.

## What's New

### 5.4.2

- All async methods on `PostmarkClient` and `PostmarkAdminClient` now accept an optional
  `CancellationToken`, threaded through to the underlying HTTP request. Existing calls are
  unaffected (the parameter defaults to `default`). For the batch/templated-send methods that
  use `params`, a `(CancellationToken, params …)` overload is provided.

### 5.4.1

- Added missing webhook payload models for type-safe deserialization of incoming
  webhook requests:
  - `PostmarkClickWebhookMessage` — click tracking webhook.
  - `PostmarkSpamComplaintWebhookMessage` — spam complaint webhook.
  - `PostmarkSubscriptionChangeWebhookMessage` — subscription change webhook.

  These join the existing `PostmarkBounceWebhookMessage`, `PostmarkDeliveryWebhookMessage`,
  `PostmarkOpenWebhookMessage`, and `PostmarkInboundWebhookMessage`.

### 5.4.0

- Added Bulk Email API support: `SendBulkEmailAsync` (`POST /email/bulk`) and
  `GetBulkEmailStatusAsync` (`GET /email/bulk/{id}`) for broadcast/marketing sends.

## Links

- [Source & issues](https://github.com/ActiveCampaign/postmark-dotnet)
- [Documentation wiki](https://github.com/ActiveCampaign/postmark-dotnet/wiki)
- [Postmark API reference](https://postmarkapp.com/developer)

## License

Licensed under the [MIT](https://github.com/ActiveCampaign/postmark-dotnet/blob/main/LICENSE) license.
