namespace ServiceName.Core.Common.Interfaces
{
    public interface INotificationService
    {
        void SendEmail();
        void SendSMS();
    }
}
