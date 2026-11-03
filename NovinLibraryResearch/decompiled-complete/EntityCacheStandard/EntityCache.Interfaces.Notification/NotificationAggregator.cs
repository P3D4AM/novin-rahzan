namespace EntityCache.Interfaces.Notification;

public class NotificationAggregator
{
	public IUiNotificationFactory Notification { get; set; }

	public NotificationAggregator(IUiNotificationFactory notif)
	{
		Notification = notif;
	}
}
