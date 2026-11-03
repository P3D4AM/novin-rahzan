using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_DepartmentCategory")]
public class DepartmentCategory : IDepartmentCategory, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public Guid DepartmentGuid { get; set; }

	[MaxLength(50)]
	public string? FirstLevel { get; set; }

	[MaxLength(50)]
	public string? SecondLevel { get; set; }

	[MaxLength(50)]
	public string? ThirdLevel { get; set; }
}
