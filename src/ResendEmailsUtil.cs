using Soenneker.Extensions.Enumerable.String;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Resend.ClientUtil.Abstract;
using Soenneker.Resend.Emails.Abstract;
using Soenneker.Resend.OpenApiClient;
using Soenneker.Resend.OpenApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Resend.Emails;

///<inheritdoc cref="IResendEmailsUtil"/>
public sealed class ResendEmailsUtil : IResendEmailsUtil
{
    private readonly IResendClientUtil _clientUtil;

    public ResendEmailsUtil(IResendClientUtil clientUtil)
    {
        _clientUtil = clientUtil;
    }

    public async ValueTask<string?> Send(string from, List<string> to, string subject, string? html = null, string? text = null, List<string>? cc = null,
        List<string>? bcc = null, List<string>? replyTo = null, List<Attachment>? attachments = null, List<Tag>? tags = null, string? scheduledAt = null,
        CancellationToken cancellationToken = default)
    {
        ResendOpenApiClient client = await _clientUtil.Get(cancellationToken)
                                                      .NoSync();

        var request = new SendEmailRequest
        {
            From = from,
            To = new SendEmailRequest.SendEmailRequest_to { String = to },
            Subject = subject,
            Html = html,
            Text = text,
            Cc = new SendEmailRequest.SendEmailRequest_cc { SendEmailRequestCcString = cc?.ToCommaSeparatedString() },
            Bcc = new SendEmailRequest.SendEmailRequest_bcc { SendEmailRequestBccString = bcc?.ToCommaSeparatedString() },
            ReplyTo = new SendEmailRequest.SendEmailRequest_reply_to { SendEmailRequestReplyToString = replyTo?.ToCommaSeparatedString() },
            Attachments = attachments,
            Tags = tags,
            ScheduledAt = scheduledAt
        };

        SendEmailResponse? response = await client.Emails.PostAsync(request, null, cancellationToken)
                                                  .NoSync();
        return response?.Id;
    }

    public async ValueTask<List<string>> SendBatch(List<SendEmailRequest> emails, CancellationToken cancellationToken = default)
    {
        if (emails == null || emails.Count == 0)
            throw new ArgumentException("At least one email request is required.", nameof(emails));

        if (emails.Count > 100)
            throw new ArgumentException("Maximum of 100 emails can be sent in a batch.", nameof(emails));

        ResendOpenApiClient client = await _clientUtil.Get(cancellationToken)
                                                      .NoSync();

        CreateBatchEmailsResponse? response = await client.Emails.Batch.PostAsync(emails, null, cancellationToken)
                                                          .NoSync();
        return response?.Data?.Select(e => e.Id)
                       .ToList() ?? new List<string>();
    }

    public async ValueTask CancelScheduled(string emailId, CancellationToken cancellationToken = default)
    {
        if (emailId.IsNullOrEmpty())
            throw new ArgumentException("Email ID is required.", nameof(emailId));

        ResendOpenApiClient client = await _clientUtil.Get(cancellationToken)
                                                      .NoSync();

        await client.Emails[emailId]
                    .Cancel.PostAsync(null, cancellationToken)
                    .NoSync();
    }

    public async ValueTask<Email?> Get(string emailId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(emailId))
            throw new ArgumentException("Email ID is required.", nameof(emailId));

        ResendOpenApiClient client = await _clientUtil.Get(cancellationToken)
                                                      .NoSync();

        return await client.Emails[emailId]
                           .GetAsync(null, cancellationToken)
                           .NoSync();
    }
}