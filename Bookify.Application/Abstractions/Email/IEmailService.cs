namespace Bookify.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(Bookify.Domain.Users.Email recipient, string subject, string body);
}
