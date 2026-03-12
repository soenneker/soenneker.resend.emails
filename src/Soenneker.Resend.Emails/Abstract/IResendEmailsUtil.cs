using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Resend.OpenApiClient.Models;

namespace Soenneker.Resend.Emails.Abstract;

/// <summary>
/// Interface for sending emails using the Resend API client.
/// </summary>
public interface IResendEmailsUtil
{
    /// <summary>
    /// Sends a single email.
    /// </summary>
    /// <param name="from">Sender email address. To include a friendly name, use the format "Your Name &lt;sender@domain.com&gt;".</param>
    /// <param name="to">Recipient email address(es).</param>
    /// <param name="subject">Email subject.</param>
    /// <param name="html">The HTML version of the message.</param>
    /// <param name="text">The plain text version of the message.</param>
    /// <param name="cc">Cc recipient email address(es).</param>
    /// <param name="bcc">Bcc recipient email address(es).</param>
    /// <param name="replyTo">Reply-to email address(es).</param>
    /// <param name="attachments">List of attachments.</param>
    /// <param name="tags">List of tags.</param>
    /// <param name="scheduledAt">Schedule email to be sent later. The date should be in ISO 8601 format.</param>
    /// <param name="cancellationToken">Cancellation token to use when cancelling requests.</param>
    /// <returns>The ID of the sent email.</returns>
    ValueTask<string?> Send(string from, List<string> to, string subject, string? html = null, string? text = null, List<string>? cc = null,
        List<string>? bcc = null, List<string>? replyTo = null, List<Attachment>? attachments = null, List<Tag>? tags = null, string? scheduledAt = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends multiple emails in a batch (up to 100 emails).
    /// </summary>
    /// <param name="emails">List of email requests to send.</param>
    /// <param name="cancellationToken">Cancellation token to use when cancelling requests.</param>
    /// <returns>List of email IDs for the sent emails.</returns>
    ValueTask<List<string>> SendBatch(List<SendEmailRequest> emails, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a scheduled email.
    /// </summary>
    /// <param name="emailId">The ID of the email to cancel.</param>
    /// <param name="cancellationToken">Cancellation token to use when cancelling requests.</param>
    ValueTask CancelScheduled(string emailId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the details of a sent email.
    /// </summary>
    /// <param name="emailId">The ID of the email to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token to use when cancelling requests.</param>
    /// <returns>The email details.</returns>
    ValueTask<Email?> Get(string emailId, CancellationToken cancellationToken = default);
}