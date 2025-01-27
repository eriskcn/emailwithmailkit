namespace demo_email.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EmailController(EmailService emailService, ILogger<EmailController> logger) : ControllerBase
{
    private readonly EmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    private readonly ILogger<EmailController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpPost("send")]
    public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest? emailRequest)
    {
        if (emailRequest == null)
        {
            return BadRequest("Email request is null.");
        }

        try
        {
            _logger.LogInformation("Sending email to {ToEmail}", emailRequest.ToEmail);
            await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
            _logger.LogInformation("Email sent successfully to {ToEmail}", emailRequest.ToEmail);
            return Ok(new { Message = "Email sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {ToEmail}", emailRequest.ToEmail);
            return BadRequest(new { Message = "Failed to send email", Error = ex.Message });
        }
    }
}