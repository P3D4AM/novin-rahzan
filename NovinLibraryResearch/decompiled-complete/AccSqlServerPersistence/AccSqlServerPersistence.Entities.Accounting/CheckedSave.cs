using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Global_CheckedSave")]
public class CheckedSave : ICheckedSave, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; set; } = true;


	public Guid EntityGuid { get; set; }

	public Guid DetailGuid { get; set; }

	public bool IsDebit { get; set; }

	public checkedTablReportType ReportType { get; set; }

	public DateTime DateM { get; set; }
}
