using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;

namespace AccSqlServerPersistence.Entities;

[Table("Global_Calender")]
public class Calender : ICalender, IHasJustGuid, IHasStatus, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	[Required]
	public string Monasebat { get; set; }

	[Required]
	public string Description { get; set; }

	public bool STRasmi { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string? MetaData { get; set; }
}
