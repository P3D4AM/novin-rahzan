using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;

namespace AccSqlServerPersistence.Entities.Accounting.Tolid;

[Table("Acc_Tolid_Mavad")]
public class TolidMavad : ITolidMavad, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, ITolidFormulaRelation, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public string? Serial { get; set; }

	[Required]
	public string Description { get; set; }

	public Guid? V2Guid { get; set; }

	public decimal? V2Qt { get; set; }

	public decimal? V2Price { get; set; }

	public decimal? RowSum { get; set; }

	[NotMapped]
	public bool Status { get; }

	public Guid? TolidFormulaGuid { get; set; }

	public short TolidFormulaIndex { get; set; }

	public string? MetaData { get; set; }

	public Tolid Master { get; set; }

	public Product Product { get; set; }

	public Depot Depot { get; set; }

	public ProductVahed Vahed { get; set; }
}
