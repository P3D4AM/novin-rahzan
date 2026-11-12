using System;

namespace Stimulsoft.Base.Server;

internal interface IStiAccount
{
	IStiAccountUser User { get; set; }

	IStiAccountActions Actions { get; set; }

	IStiAccountChecker AccountChecker { get; set; }

	IStiAccountCreator AccountCreater { get; set; }

	event EventHandler UserInfoChanged;

	event EventHandler<EventArgs> ShowNewNotifications;

	event EventHandler<EventArgs> ShowNewMessage;

	event EventHandler NotificationsLoaded;

	event EventHandler NotificationsChanged;

	event EventHandler NotificationsRequest;

	event EventHandler DesignerSpecificationChanged;

	void CheckLicense();

	void Logout();

	void InvokeUserInfoChanged();

	void InvokeNotificationsRequest();

	void InvokeNotificationsLoaded();

	void InvokeNotificationsChanged();

	void InvokeShowNewNotifications(EventArgs arg);

	void InvokeShowNewMessage(EventArgs arg);

	void InvokeDesignerSpecificationChanged();
}
