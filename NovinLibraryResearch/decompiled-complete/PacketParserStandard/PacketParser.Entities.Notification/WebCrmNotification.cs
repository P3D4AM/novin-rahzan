using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Notification;

namespace PacketParser.Entities.Notification;

public class WebCrmNotification : Serializable<WebCrmNotification>, ICrmNotification, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public string Body { get; set; }

	public EnumNotification Type { get; set; }

	public DateTime EventTime { get; set; }

	public string Uuid { get; set; }
}
