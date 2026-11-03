using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_VisitorCity")]
public class VisitorCity : IVisitorCity, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	[Required]
	public Guid Guid { get; set; }

	[Required]
	public DateTime Modified { get; set; } = DateTime.Now;


	[NotMapped]
	public bool Status { get; set; }

	[Required]
	public Guid VisitorGuid { get; set; }

	[Required]
	public Guid CityGuid { get; set; }

	public City City { get; set; }

	public Visitor Visitor { get; set; }
}
