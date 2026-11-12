using System;

namespace Stimulsoft.Base.Server;

public interface IStiNotificationsControl
{
	int? NotificationCount { get; set; }

	event EventHandler ProcessClose;

	event EventHandler NotificationCountChanged;

	void ShowBorder(bool show);

	void BuildNotifications();
}
