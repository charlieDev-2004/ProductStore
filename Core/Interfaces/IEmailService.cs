using Core.Models.Email;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(Email email);
    }
}