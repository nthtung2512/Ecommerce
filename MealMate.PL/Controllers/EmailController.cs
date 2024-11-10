using MealMate.DAL.Entities.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailSetting emailRequest)
        {
            await _emailSender.SendEmailAsync(emailRequest.Email, emailRequest.Subject, emailRequest.Message);
            return Ok("Email sent successfully");
        }
    }



}
