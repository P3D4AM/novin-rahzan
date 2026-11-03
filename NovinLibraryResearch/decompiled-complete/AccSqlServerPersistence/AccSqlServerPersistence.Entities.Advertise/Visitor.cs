using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_Visitor")]
public class Visitor : IVisitor, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	[Required]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[Required]
	public short MasterCount { get; set; }

	[Required]
	public short SlaveCount { get; set; }

	[Required]
	public bool IsAgent { get; set; }

	[MaxLength(200)]
	[Required]
	public string AdvReplacement { get; set; }

	[MaxLength(50)]
	[Required(AllowEmptyStrings = true)]
	public string FullName { get; set; }

	[Required]
	public bool Status { get; set; }

	[Required]
	[MaxLength(7)]
	public string Enable_Status { get; set; }

	public string? Phone { get; set; }

	public Guid DepartmentGuid { get; set; }

	public ICollection<VisitorCity> VisitorCities { get; set; }

	public ICollection<Owner> Owners { get; set; }
}
