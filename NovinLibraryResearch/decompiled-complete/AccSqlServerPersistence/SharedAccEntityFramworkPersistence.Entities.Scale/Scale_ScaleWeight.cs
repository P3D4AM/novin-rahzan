using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_ScaleWeight")]
public class Scale_ScaleWeight : IScale_ScaleWeight, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ScaleGuid { get; set; }

	public DateTime Date { get; set; }

	public decimal UnitPrice { get; set; }

	[MaxLength(250)]
	public string UnitPriceText { get; set; }

	public decimal Weight { get; set; }

	[MaxLength(250)]
	public string WeightText { get; set; }

	public decimal Totalprice { get; set; }

	[MaxLength(250)]
	public string TotalPriceText { get; set; }

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
}
