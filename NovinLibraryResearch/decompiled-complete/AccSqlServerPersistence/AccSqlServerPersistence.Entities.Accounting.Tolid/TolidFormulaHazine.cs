using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;

namespace AccSqlServerPersistence.Entities.Accounting.Tolid;

[Table("Acc_Tolid_Formula_Hazine")]
public class TolidFormulaHazine : ITolidFormulaHazine, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public decimal Price { get; set; }

	public HazineType Type { get; set; }

	[Required]
	public string Description { get; set; }

	[NotMapped]
	public bool Status { get; }

	public TolidFormula Master { get; set; }

	public Tafsil Tafsil { get; set; }

	public Moein Moein { get; set; }

	public string? MetaData { get; set; }
}
