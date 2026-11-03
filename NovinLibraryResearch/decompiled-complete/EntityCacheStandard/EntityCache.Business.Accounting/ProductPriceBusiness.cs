using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class ProductPriceBusiness : IHasContext
{
	public decimal Price { get; set; }

	public string PriceTitsl { get; set; }

	public int Ordinal { get; set; }

	public string ContextGuid { get; set; }

	public ProductPriceBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
