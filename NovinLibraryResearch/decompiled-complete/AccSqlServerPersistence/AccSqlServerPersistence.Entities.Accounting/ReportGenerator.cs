using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Global_ReportGenerator")]
public class ReportGenerator : IReportGenerator, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[Required]
	[MaxLength(200)]
	public string? Title { get; set; }

	[Required]
	public string? Description { get; set; }

	[Required]
	public string? Rpt { get; set; }

	public int Version { get; set; }

	public DateTime LastUpdate { get; set; }

	public StiType? Type { get; set; }

	public Document? Document { get; set; }

	public MaliYearSanadType MaliYearSanadType { get; set; }

	public ICollection<ReportGeneratorDet> Details { get; set; }
}
