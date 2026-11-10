using System.Collections.Generic;
using Soenneker.Resend.OpenApiClient.Models;

namespace Soenneker.Resend.Emails;

internal static class ResendEmailRequestFactory
{
    internal static SendEmailRequest Create(string from, List<string> to, string subject, string? html, string? text,
        List<string>? cc, List<string>? bcc, List<string>? replyTo, List<Attachment>? attachments,
        List<Tag>? tags, string? scheduledAt)
    {
        var request = new SendEmailRequest { From = from, Subject = subject };

        // Generated setters preserve explicit nulls. Omit absent optional values: Resend
        // distinguishes an omitted field from JSON null (notably scheduled_at and tags).
        if (html != null) request.Html = html;
        if (text != null) request.Text = text;
        if (attachments != null) request.Attachments = attachments;
        if (tags != null) request.Tags = tags;
        if (scheduledAt != null) request.ScheduledAt = scheduledAt;

        request.AdditionalData["to"] = to;
        if (cc is { Count: > 0 }) request.AdditionalData["cc"] = cc;
        if (bcc is { Count: > 0 }) request.AdditionalData["bcc"] = bcc;
        if (replyTo is { Count: > 0 }) request.AdditionalData["reply_to"] = replyTo;
        return request;
    }
}
