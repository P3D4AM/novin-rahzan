using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;

namespace AccSqlServerPersistence.Entities.Accounting.Tolid;

[Table("Acc_Tolid_Formula_Export")]
public class TolidFormulaExport : ITolidFormulaExport, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public decimal Quantity { get; set; }

	public Guid? V2Guid { get; set; }

	[Required]
	public string Description { get; set; }

	[NotMapped]
	public bool Status { get; }

	public TolidFormula Master { get; set; }

	public Product Product { get; set; }

	public Depot Depot { get; set; }

	public ProductVahed Vahed { get; set; }

	public string? MetaData { get; set; }
}
