using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_Advertise")]
public class Advertise : IAdvAdvertise, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[MaxLength(200)]
	public string? Name { get; set; }

	public Guid GroupGuid { get; set; }

	public decimal Price { get; set; }

	public bool IsPishNevis { get; set; }

	public string? ImageList { get; set; }

	public string? TitleList { get; set; }

	public string? ContentList { get; set; }

	public string? DivatCategoryList { get; set; }

	public string? SheypoorCategoryList { get; set; }
}
