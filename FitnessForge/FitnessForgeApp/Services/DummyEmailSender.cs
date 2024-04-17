using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class DummyEmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public DummyEmailSender(ILogger<DummyEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation($"Sending email to {email} with subject {subject}.");
        return Task.CompletedTask;
    }
}