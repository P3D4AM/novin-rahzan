using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_RasGiri")]
public class RasGiri : IRasGiri, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Title { get; set; }

	[Required]
	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public DateTime BaseDateM { get; set; }

	public string? BaseDateSh { get; }

	public int DayCount { get; set; }

	public int RecordCount { get; set; }

	public decimal Sood_Mounthly { get; set; }

	public decimal Sood_Total { get; set; }

	public decimal Sood_Percent { get; set; }

	public decimal Total_Sum { get; set; }

	public string? Reserve1 { get; set; }

	public string? Reserve2 { get; set; }

	public string? Reserve3 { get; set; }

	public string? Reserve4 { get; set; }

	public string? Reserve5 { get; set; }

	public string? Reserve6 { get; set; }

	public string? Reserve7 { get; set; }

	public string? Reserve8 { get; set; }

	public string? Reserve9 { get; set; }

	public string? Reserve10 { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid SanadGuid { get; set; }

	public SanadSt SanadType { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public ICollection<RasGiriDet> Details { get; set; }
}
