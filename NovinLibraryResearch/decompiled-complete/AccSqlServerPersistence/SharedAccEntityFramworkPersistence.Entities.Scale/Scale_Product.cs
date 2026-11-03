using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace SharedAccEntityFramworkPersistence.Entities.Scale;

[Table("Scale_Product")]
public class Scale_Product : IScale_Product, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	[MaxLength(250)]
	public string Code { get; set; }

	public Guid GroupGuid { get; set; }

	public Guid VahedGuid { get; set; }

	public bool IsWeight { get; set; }

	public decimal Price { get; set; }

	[MaxLength(250)]
	public string Barcode { get; set; }

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

	public string Description { get; set; }
}
