using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_State")]
public class State : IState, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	[Required]
	public Guid Guid { get; set; }

	[Required]
	public DateTime Modified { get; set; } = DateTime.Now;


	[NotMapped]
	public bool Status { get; }

	[Required]
	public string StateName { get; set; }

	public ICollection<City> Cities { get; set; }
}
