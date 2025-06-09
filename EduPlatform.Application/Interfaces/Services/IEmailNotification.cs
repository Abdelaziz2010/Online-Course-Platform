
using EduPlatform.Application.DTOs;
using SendGrid;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface IEmailNotification
    {
        Task<Response> SendEmailForContactUs(ContactMessageDTO contactMessage);
    }
}
