namespace MailService.Interface
{
    public interface IMailService
    {
        Task<bool> sendMail(string htmlString);
    }
}
