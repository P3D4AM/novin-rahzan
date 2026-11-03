using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TozieList;

namespace AccSqlServerPersistence.Entities.Accounting.TozieLists;

[Table("Acc_TozieList")]
public class TozieList : ITozieList, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

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

	public int Number { get; set; }

	public DateTime DateM { get; set; }

	[Required]
	[MaxLength(250)]
	public string DriverName { get; set; }

	[MaxLength(250)]
	public string? DriverMobile1 { get; set; }

	[MaxLength(250)]
	public string? DriverMobile2 { get; set; }

	[MaxLength(250)]
	public string? CarNumber { get; set; }

	[MaxLength(500)]
	public string? Description { get; set; }

	public Guid CashierGuid { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public ICollection<TozieListDetail> DetailList { get; set; }

	public Cashier CashierItem { get; set; }

	public string? MetaData { get; set; }
}
