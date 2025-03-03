namespace Controller_based_APIs.Services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}
