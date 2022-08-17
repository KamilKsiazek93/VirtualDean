namespace VirtualDean.Data
{
    public interface INotifications
    {
        void SendEmail(string[] reciepients, string mailContent, string subject);
    }
}
