using PacketParser.Entities;

namespace PacketParser.Filters;

public class ProductPriceLevelFilter
{
	public EnPriceLevelType? Type { get; set; }

	public bool? Status { get; set; } = true;


	public bool IsLoadDefault { get; set; }

	public bool IsLoadSystem { get; set; }
}
