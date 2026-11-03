namespace PacketParser.Entities.Interfaces.Accounting.Tax;

public interface ITaxDetail : IHasJustGuid, IHasMasterGuid
{
	decimal ProductTaxPercent { get; set; }

	decimal FeeTaxPercent { get; set; }

	decimal ProfitTaxPercent { get; set; }

	short Ordinal { get; set; }

	string Description { get; set; }
}
