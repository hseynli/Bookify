using Bookify.Application.Abstractions.Email;

namespace Bookify.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public async Task SendAsync(Domain.Users.Email recipient, string subject, string body)
    {
        await Task.CompletedTask;
    }
}
