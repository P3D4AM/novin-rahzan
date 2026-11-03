using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tax;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Tax;

[Table("Acc_TaxDetail")]
public class TaxClassDetail : ITaxDetail, IHasJustGuid, IHasMasterGuid
{
	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public decimal ProductTaxPercent { get; set; }

	public decimal FeeTaxPercent { get; set; }

	public decimal ProfitTaxPercent { get; set; }

	public short Ordinal { get; set; }

	public string Description { get; set; }

	public TaxClass MasterItem { get; set; }
}
