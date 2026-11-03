using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_CashiersLog")]
public class CashiersLog : ICashierLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	public Guid SanadGuid { get; set; }

	public EnLog SanadType { get; set; }

	[Required]
	public string? Title { get; set; }

	public DateTime DateM { get; set; }

	public Guid CashierGuid { get; set; }

	public byte[]? OldSerializedData { get; set; }

	public byte[]? NewSerializedData { get; set; }

	public ENAction Action { get; set; }

	public EnSystem System { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public short StatusId { get; set; }

	public ReturnedState Level { get; set; }

	public string MetaData { get; set; }

	public Cashier Cashier { get; set; }

	public ENSource Source { get; set; }

	public string ApplicationVersion { get; set; }
}
