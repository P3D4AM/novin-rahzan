using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Crm;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting.Crm;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Crm;

[Table("Acc_CallingLogDetail")]
public class CallingLogDetail : ICallingLogDetail
{
	[NotMapped]
	private DateTime _modified;

	public Guid? CashierGuid { get; set; }

	public DateTime? PickUpDateM { get; set; }

	public DateTime? HangUpDateM { get; set; }

	public DateTime DateInsert { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	public EnumNotification Status { get; set; }

	[Key]
	[MaxLength(150)]
	public string Uuid { get; set; }

	[MaxLength(150)]
	public string LinkedId { get; set; }

	[MaxLength(15)]
	public string CallFrom_Number { get; set; }

	[MaxLength(500)]
	public string CallFrom_Name { get; set; }

	[MaxLength(15)]
	public string CallTo_Number { get; set; }

	[MaxLength(500)]
	public string CallTo_Name { get; set; }

	public string Description { get; set; }

	public short Rings { get; set; }

	public int Version { get; set; }

	public Cashier Cashier { get; set; }

	public CallingLog Master { get; set; }
}
