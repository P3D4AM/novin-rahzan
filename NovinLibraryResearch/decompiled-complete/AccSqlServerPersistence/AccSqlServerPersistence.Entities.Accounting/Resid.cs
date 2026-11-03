using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Resid")]
public class Resid : IResid, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[NotMapped]
	private DateTime _DateM;

	[NotMapped]
	private DateTime _Date1;

	[NotMapped]
	private DateTime _Date2;

	[Key]
	public Guid Guid { get; set; }

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

	[NotMapped]
	public bool Status { get; set; } = true;


	public long ID2 { get; set; }

	public DateTime DateM
	{
		get
		{
			return _DateM;
		}
		set
		{
			DateTime dateM = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_DateM = dateM;
		}
	}

	[Required]
	public string Description { get; set; }

	public DateTime Date1
	{
		get
		{
			return _Date1;
		}
		set
		{
			DateTime date = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_Date1 = date;
		}
	}

	public DateTime Date2
	{
		get
		{
			return _Date2;
		}
		set
		{
			DateTime date = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_Date2 = date;
		}
	}

	public Guid? GuidVahed { get; set; }

	public decimal Price { get; set; }

	public decimal Qt { get; set; }

	public decimal Mandeh { get; set; }

	public Guid? PrdGuid { get; set; }

	[Required]
	public string PrdModel { get; set; }

	[Required]
	public string PrdColor { get; set; }

	[Required]
	public string PrdSerial { get; set; }

	public Guid? TafsilGuid { get; set; }

	[Required]
	public string Problems { get; set; }

	[Required]
	public string Darkhast { get; set; }

	public short StatusId { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public Guid? OrderGuid { get; set; }

	[MaxLength(500)]
	public string? Reserve { get; set; }

	public string? MetaData { get; set; }

	public Tafsil Tafsil { get; set; }

	public Product Product { get; set; }

	public Cashier Cashier { get; set; }

	public Order Order { get; set; }

	public Reception Reception { get; set; }
}
