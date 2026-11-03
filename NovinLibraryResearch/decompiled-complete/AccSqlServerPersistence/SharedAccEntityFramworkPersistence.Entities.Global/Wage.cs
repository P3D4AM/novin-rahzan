using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace SharedAccEntityFramworkPersistence.Entities.Global;

[Table("Global_Wage")]
public class Wage : IWage, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid? ServerGuid { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	public EnWageType Type { get; set; }

	public EnWageCalculationMethod CalculationMethod { get; set; }

	public decimal PriceOrPercent { get; set; }

	public decimal MinTransaction { get; set; }

	public decimal MaxTransaction { get; set; }

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

	public bool Status { get; set; }

	public EnKhazaneMaster MasterType { get; set; }
}
