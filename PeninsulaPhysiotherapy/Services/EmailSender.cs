using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PeninsulaPhysiotherapy.Services;

public class emailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly IWebHostEnvironment environment;


    public emailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<emailSender> logger,
                       IWebHostEnvironment environment)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
        this.environment = environment;
    }

    public AuthMessageSenderOptions Options { get; } 

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
        bool hasVoucher = false;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("rcxiangyuliu@gmail.com", "PeninsulaPhysiotherapy");
        var to = new EmailAddress(toEmail, "PeninsulaPhysiotherapy");

        if (message[0].Equals('@'))
        {
            hasVoucher = true;
            message = message.Remove(0,1);
        }
        var plainTextContent = message;
        var htmlContent = message;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        if (hasVoucher == true)
        {
            var filePath = Path.Combine(environment.ContentRootPath, @"wwwroot\storage", "VOUCHER.png");
            msg.AddAttachment("VOUCHER.png", Convert.ToBase64String(File.ReadAllBytes(filePath)));
        }

        msg.SetClickTracking(false, false);

        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}
