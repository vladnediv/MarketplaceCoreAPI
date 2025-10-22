using BLL.Model.SendModels;

namespace BLL.Service.Interface;

public interface IEmailService
{
    public Task SendEmailAsync(EmailMessage emailMessage);
}