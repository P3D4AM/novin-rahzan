using System;

namespace PacketParser.Entities.Interfaces.Notification;

public interface ICrmNotification : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	EnumNotification Type { get; set; }

	DateTime EventTime { get; set; }
}
