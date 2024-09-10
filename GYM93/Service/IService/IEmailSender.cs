namespace GYM93.Service.IService
{
    public interface IEmailSender
    {
        Task SendEmail(string email, string subject, string message);
    }
}
