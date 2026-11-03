namespace PacketParser.Entities.Interfaces.Accounting.Product;

public interface IProductPriceLevel : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Description { get; set; }

	short Ordinal { get; set; }

	EnPriceLevelType Type { get; set; }

	decimal DiscountCoefficient { get; set; }

	bool IsSystem { get; set; }

	bool IsDefault { get; set; }
}
