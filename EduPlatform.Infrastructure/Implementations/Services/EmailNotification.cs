using EduPlatform.Application.DTOs;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class EmailNotification : IEmailNotification
    {
        private readonly IConfiguration configuration;

        public EmailNotification(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Response> SendEmailForContactUs(ContactMessageDTO contactMessage)
        {
            var apiKey = configuration["SendGridSettings:SENDGRID_API_KEY"];
            var from = new EmailAddress(configuration["SendGridSettings:From"]);
            var to = new EmailAddress(contactMessage.Email);

            var sendGridMessage = new SendGridMessage()
            {
                From = from,
                ReplyTo = to,
                Subject = "Contact page: Received a request from user"
            };

            sendGridMessage.AddContent(MimeType.Html, GetEmailBody(contactMessage));
            
            sendGridMessage.AddTo(to);

            Console.WriteLine($"Sending email with payload: \n{sendGridMessage.Serialize()}");

            var response = await new SendGridClient(apiKey).SendEmailAsync(sendGridMessage).ConfigureAwait(false);
           
            Console.WriteLine($"Response: {response.StatusCode}");
            
            Console.WriteLine(response.Headers);

            return response;
        }
        private string GetEmailBody(ContactMessageDTO contactMessage)
        {

            return $$"""
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>An enquiry received  - {{contactMessage.Subject}}</title>
                    </head>
                    <body>                        
                        <p>Dear EduPlatfrom Admin</p>
                        <p>You have received an enquiry from a user and the details as follows.</p>
                    
                        <p><strong>Message details</strong></p>
                        <ul>
                            <li>User Name: {{contactMessage.Name}}</li>
                            <li>User Email: {{contactMessage.Email}}</li>
                    <li>Subject: {{contactMessage.Subject}}</li>
                    <li>Message: {{contactMessage.Message}}</li>
                        </ul>
                    
                    
                        <p><strong>Warm regards,</strong></p>
                        <p>EduPlatfrom [Automated]</p>
                    </body>
                    </html>                    
                    """;
        }
    }
}
