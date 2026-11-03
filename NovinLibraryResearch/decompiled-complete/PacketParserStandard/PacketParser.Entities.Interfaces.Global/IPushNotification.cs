using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface IPushNotification : IHasJustGuid, IHasStatus
{
	string Message { get; set; }

	string Title { get; set; }

	EnPushNotificationType Type { get; set; }

	short DisplayCount { get; set; }

	ENSource Source { get; set; }

	DateTime Date { get; set; }

	DateTime ExpireDate { get; set; }

	DateTime ModifiedDate { get; set; }

	string HelperUri { get; set; }
}
