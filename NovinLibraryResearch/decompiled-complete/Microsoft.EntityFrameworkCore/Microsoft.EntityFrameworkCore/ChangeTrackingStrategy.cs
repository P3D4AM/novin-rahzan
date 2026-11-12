namespace Microsoft.EntityFrameworkCore;

public enum ChangeTrackingStrategy
{
	Snapshot,
	ChangedNotifications,
	ChangingAndChangedNotifications,
	ChangingAndChangedNotificationsWithOriginalValues
}
