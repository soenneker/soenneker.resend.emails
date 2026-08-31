[![](https://img.shields.io/nuget/v/soenneker.resend.emails.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.resend.emails/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.resend.emails/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.resend.emails/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.resend.emails.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.resend.emails/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.resend.emails/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.resend.emails/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Resend.Emails

Sends individual or batched email, retrieves sent-email details, and cancels scheduled messages through Resend.

## Installation

```bash
dotnet add package Soenneker.Resend.Emails
```

## Configuration

```json
{
  "Resend": {
    "ApiKey": "re_xxxxxxxxx"
  }
}
```

## Usage

```csharp
using Soenneker.Resend.Emails.Abstract;
using Soenneker.Resend.Emails.Registrars;

services.AddResendEmailsUtilAsSingleton();

public sealed class WelcomeEmailSender
{
    private readonly IResendEmailsUtil _emails;

    public WelcomeEmailSender(IResendEmailsUtil emails)
    {
        _emails = emails;
    }

    public ValueTask<string?> Send(
        string recipient,
        CancellationToken cancellationToken)
    {
        return _emails.Send(
            from: "Acme <welcome@example.com>",
            to: [recipient],
            subject: "Welcome",
            html: "<strong>Thanks for joining.</strong>",
            cancellationToken: cancellationToken);
    }
}
```

`Send` performs the API call immediately unless `scheduledAt` is supplied and returns Resend's email ID. The sender must use a domain authorized in Resend. Use `SendBatch` for up to 100 messages in one request, `Get` to retrieve an email, and `CancelScheduled` before a scheduled message is sent.
