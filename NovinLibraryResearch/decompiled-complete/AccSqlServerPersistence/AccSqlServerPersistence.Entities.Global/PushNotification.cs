using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace AccSqlServerPersistence.Entities.Global;

[Table("Global_PushNotifications")]
public class PushNotification : IPushNotification, IHasJustGuid, IHasStatus
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Message { get; set; }

	[Required]
	public string Title { get; set; }

	public EnPushNotificationType Type { get; set; }

	public short DisplayCount { get; set; }

	public ENSource Source { get; set; }

	public DateTime Date { get; set; }

	public DateTime ExpireDate { get; set; }

	public DateTime ModifiedDate { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[MaxLength(500)]
	public string? HelperUri { get; set; }
}
