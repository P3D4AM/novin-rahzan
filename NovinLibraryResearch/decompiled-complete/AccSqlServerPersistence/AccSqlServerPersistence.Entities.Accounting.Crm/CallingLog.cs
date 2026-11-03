using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Crm;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Crm;

namespace AccSqlServerPersistence.Entities.Accounting.Crm;

[Table("Acc_CallingLog")]
public class CallingLog : ICallingLog, IHasModified, IHasStatus, IDeliverableEntity, IHasMetaData
{
	[Key]
	[MaxLength(150)]
	public string Uuid { get; set; }

	[MaxLength(150)]
	public string? MasterUuid { get; set; }

	public Guid? CoGuid { get; set; }

	public Guid? ExGuid { get; set; }

	public DateTime Modified { get; set; }

	bool IHasStatus.Status { get; }

	public short Rings { get; set; }

	[MaxLength(15)]
	[Required]
	public string CallerIdNumber { get; set; }

	[MaxLength(15)]
	[Required]
	public string DialedNumber { get; set; }

	public DateTime? PickUpDateM { get; set; }

	public DateTime? HangUpDateM { get; set; }

	public DateTime DateM { get; set; }

	public short StatusId { get; set; }

	public string Message { get; set; }

	public EnumNotification Status { get; set; }

	public bool IsIncoming { get; set; }

	public bool IsCompleteCall { get; set; }

	[Required]
	public string Description { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public string? MetaData { get; set; }

	public int Version { get; set; }

	public long CalcedNumber { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public ICollection<CallingLogDetail> CallingLogDetail { get; set; }
}
