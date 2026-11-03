using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Crm;

namespace AccSqlServerPersistence.Entities.Accounting.Crm;

[Table("Acc_CallingLine")]
public class CallingLine : ICallingLine, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[MaxLength(200)]
	public string ComputerName { get; set; }

	public short DeviceNumber { get; set; }

	public short LineNumber { get; set; }

	[MaxLength(15)]
	[Required]
	public string Number { get; set; }

	public bool IsCo { get; set; }

	public Guid CashierGuid { get; set; }

	public bool Status { get; set; }

	public string? MetaData { get; set; }

	public Cashier Cashier { get; set; }
}
