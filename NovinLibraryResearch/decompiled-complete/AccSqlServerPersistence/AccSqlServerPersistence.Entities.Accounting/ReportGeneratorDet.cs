using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Global_ReportGeneratorDet")]
public class ReportGeneratorDet : IReportGeneratorDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[Column("RepGuid")]
	public Guid MasterGuid { get; set; }

	[Required]
	[MaxLength(200)]
	public string ParameterName { get; set; }

	public EnReportGeneratorFilterType ParameterType { get; set; }

	public short OrderIndex { get; set; }

	public bool Status { get; set; }

	public ReportGenerator Master { get; set; }
}
