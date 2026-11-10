using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Kiota.Serialization.Json;
using Soenneker.Resend.OpenApiClient.Models;

namespace Soenneker.Resend.Emails.Tests;

public sealed class ResendEmailRequestFactoryTests
{
    [Test]
    public async Task Absent_optional_fields_are_omitted_from_the_wire_request()
    {
        SendEmailRequest request = ResendEmailRequestFactory.Create("sender@example.com", ["recipient@example.com"],
            "Subject", "<p>Body</p>", null, null, null, ["reply@example.com"], null, null, null);
        using JsonDocument json = await Serialize(request);
        foreach (string field in new[] { "text", "attachments", "tags", "scheduled_at", "cc", "bcc" })
            if (json.RootElement.TryGetProperty(field, out _))
                throw new InvalidOperationException($"Absent optional field {field} was serialized.");
        if (json.RootElement.GetProperty("to")[0].GetString() != "recipient@example.com" ||
            json.RootElement.GetProperty("reply_to")[0].GetString() != "reply@example.com")
            throw new InvalidOperationException("Recipients did not survive serialization.");
    }

    [Test]
    public async Task Supplied_optional_fields_are_preserved()
    {
        SendEmailRequest request = ResendEmailRequestFactory.Create("sender@example.com", ["recipient@example.com"],
            "Subject", null, "Body", ["cc@example.com"], ["bcc@example.com"], null, [],
            [new Tag { Name = "source", Value = "support" }], "2030-01-01T12:00:00Z");
        using JsonDocument json = await Serialize(request);
        if (json.RootElement.GetProperty("text").GetString() != "Body" ||
            json.RootElement.GetProperty("scheduled_at").GetString() != "2030-01-01T12:00:00Z" ||
            json.RootElement.GetProperty("tags")[0].GetProperty("value").GetString() != "support" ||
            json.RootElement.GetProperty("cc")[0].GetString() != "cc@example.com" ||
            json.RootElement.GetProperty("bcc")[0].GetString() != "bcc@example.com" ||
            json.RootElement.GetProperty("attachments").GetArrayLength() != 0 ||
            json.RootElement.TryGetProperty("html", out _))
            throw new InvalidOperationException("Optional values were not serialized correctly.");
    }

    private static async Task<JsonDocument> Serialize(SendEmailRequest request)
    {
        using var writer = new JsonSerializationWriter();
        writer.WriteObjectValue(null, request);
        return await JsonDocument.ParseAsync(writer.GetSerializedContent());
    }
}
