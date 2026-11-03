using PacketParser.Entities;

namespace PacketParser.Records.Products;

public record ProductSearchSettingRecord
{
	public ProductSearchOption Option { get; set; }

	public SearchLocation Location { get; set; }
}
