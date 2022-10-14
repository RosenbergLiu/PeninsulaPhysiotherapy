﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using PeninsulaPhysiotherapy.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Xml.Linq;

namespace PeninsulaPhysiotherapy.Services;

public class emailSender : IEmailSender
{
    private readonly ILogger _logger;

    public emailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<emailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("rcxiangyuliu@gmail.com", "PeninsulaPhysiotherapy");
        var to = new EmailAddress(toEmail, "PeninsulaPhysiotherapy");
        var plainTextContent = message;


        var htmlContent = message;
        

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
  
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}
