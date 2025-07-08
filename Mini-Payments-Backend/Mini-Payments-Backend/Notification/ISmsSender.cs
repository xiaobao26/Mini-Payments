namespace Mini_Payments_Backend.Notification;

public interface ISmsSender
{
    Task SendSmsAsync(string to, string message);
}