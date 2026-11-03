using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;

namespace AccSqlServerPersistence.Entities.Accounting.Tolid;

[Table("Acc_Tolid_Hazine")]
public class TolidHazine : ITolidHazine, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, ITolidFormulaRelation, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public decimal Price { get; set; }

	[Required]
	public string Description { get; set; }

	public HazineType Type { get; set; }

	[NotMapped]
	public bool Status { get; }

	public Guid? TolidFormulaGuid { get; set; }

	public short TolidFormulaIndex { get; set; }

	public string? MetaData { get; set; }

	public Tolid Master { get; set; }

	public Moein Moein { get; set; }

	public Tafsil Tafsil { get; set; }
}
