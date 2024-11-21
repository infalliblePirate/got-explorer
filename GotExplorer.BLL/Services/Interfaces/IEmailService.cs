namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}
