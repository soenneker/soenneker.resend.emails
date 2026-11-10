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

/// <inheritdoc cref="IResendEmailsUtil" />
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
        ResendOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();

        SendEmailRequest request = ResendEmailRequestFactory.Create(from, to, subject, html, text, cc, bcc,
            replyTo, attachments, tags, scheduledAt);

        SendEmailResponse? response = await client.Emails.PostAsync(request, null, cancellationToken).NoSync();
        return response?.Id;
    }
    public async ValueTask<List<string>> SendBatch(List<SendEmailRequest> emails, CancellationToken cancellationToken = default)

    {
        ResendOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();

        CreateBatchEmailsResponse? response = await client.Emails.Batch.PostAsync(emails, null, cancellationToken).NoSync();
        var ids = new List<string>();
        if (response?.Data is { } data)
        {
            foreach (var email in data)
            {
                if (email.Id is { } id)
                    ids.Add(id);
            }
        }
        return ids;
    }
    public async ValueTask CancelScheduled(string emailId, CancellationToken cancellationToken = default)

    {
        ResendOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();

        await client.Emails[emailId].Cancel.PostAsync(null, cancellationToken).NoSync();
    }
    public async ValueTask<Email?> Get(string emailId, CancellationToken cancellationToken = default)

    {
        ResendOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();

        return await client.Emails[emailId].GetAsync(null, cancellationToken).NoSync();
    }
}
