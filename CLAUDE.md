# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build src/postmark-dotnet.sln

# Run all tests (requires API tokens - see Testing section)
dotnet test src/postmark-dotnet.sln

# Run a single test class
dotnet test src/Postmark.Tests/Postmark.Tests.csproj --filter "ClassName=ClientSendingTests"

# Run a single test method
dotnet test src/Postmark.Tests/Postmark.Tests.csproj --filter "FullyQualifiedName~ClientCanSendMessage"

# Build NuGet package
dotnet pack src/Postmark/Postmark.csproj
```

## Architecture

This is a .NET Standard 2.0 HTTP client library for the [Postmark](https://postmarkapp.com) transactional email API. There are no web servers, databases, or startup code — only the library and its integration tests.

**Client hierarchy:**

- `PostmarkClientBase` — abstract base handling all HTTP communication. Contains the two core methods (`ProcessRequestAsync` for POST/PUT with body, `ProcessNoBodyRequestAsync` for GET/DELETE with query params). Uses a static `ClientFactory` property returning `ISimpleHttpClient` — replace this to mock HTTP in tests.
- `PostmarkClient` — server-level API using `X-Postmark-Server-Token` header. Handles email sending, bounces, templates, message streams, suppressions, statistics, webhooks, and inbound messages.
- `PostmarkAdminClient` — account-level API using `X-Postmark-Account-Token` header. Handles servers, sender signatures, and domains.
- `PostmarkClientExtensions` — convenience overloads for `SendMessageAsync` (accepts primitives instead of a `PostmarkMessage` object).
- `LegacyClientExtensions` — deprecated IAsyncResult-style wrappers from the 1.x API. Do not use; all new code should use the `Async` task-based methods.

**Serialization:**

Uses `System.Text.Json` exclusively with two custom converters registered in `JsonExtensions.TryDeserializeObject`:
- `DateTimeConverter` — handles the Postmark API's `" (GMT)"` date format suffix.
- `UnicodeJsonStringConverter` — handles Unicode string encoding quirks.

All models live in `src/Postmark/Model/` with subdirectories for `MessageStreams/`, `Suppressions/`, and `Webhooks/`.

## Testing

All tests are **integration tests** that make real HTTP calls to the Postmark API. They require API tokens configured either via a `testing_keys.json` file (searched up the directory tree from the test assembly) or environment variables.

Required keys:
```json
{
  "READ_SELENIUM_TEST_SERVER_TOKEN": "...",
  "READ_LINK_TRACKING_TEST_SERVER_TOKEN": "...",
  "READ_SELENIUM_OPEN_TRACKING_TOKEN": "...",
  "READ_BOUNCE_TEST_SERVER_TOKEN": "...",
  "WRITE_ACCOUNT_TOKEN": "...",
  "WRITE_TEST_SERVER_TOKEN": "...",
  "WRITE_TEST_SENDER_EMAIL_ADDRESS": "...",
  "WRITE_TEST_EMAIL_RECIPIENT_ADDRESS": "...",
  "WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE": "...",
  "BASE_URL": "https://api.postmarkapp.com"
}
```

The config file location can be overridden with the `POSTMARK_SDK_CONFIG_FILE_NAME` environment variable.

Tests that create or modify data (servers, signatures, domains) require `WRITE_*` tokens. Read-only tests use `READ_*` tokens pointing to pre-configured test servers. `ClientBaseFixture` is the abstract base class all test classes inherit from.
