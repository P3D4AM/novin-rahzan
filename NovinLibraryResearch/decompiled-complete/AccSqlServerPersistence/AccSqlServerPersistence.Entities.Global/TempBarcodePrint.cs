using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace AccSqlServerPersistence.Entities.Global;

[Table("Acc_TempBarcodePrint")]
public class TempBarcodePrint : ITempBarcodePrint, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
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

	[NotMapped]
	public bool Status { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid DetailGuid { get; set; }

	[MaxLength(500)]
	public string? Barcode { get; set; }

	[MaxLength(500)]
	public string? GuarantyCode { get; set; }

	[MaxLength(500)]
	public string? TempDescription { get; set; }

	public Document AsType { get; set; }

	public string? MasterDescription { get; set; }

	public string? DetailDescription { get; set; }

	public DateTime MasterDate { get; set; }

	public DateTime? GuarantyStartDate { get; set; }

	public DateTime? GuarantyEndDate { get; set; }

	public Product ProductItem { get; set; }

	public Tafsil TafsilItem { get; set; }
}
