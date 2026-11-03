using PacketParser.Entities;

namespace PacketParser.Records.Products;

public record ProductSearchOptionRecord
{
	public ProductSearchOption Option { get; private set; }

	public string OptionForDisplay => Option.GetDisplay().GetTranslate();

	public ProductSearchOptionRecord(ProductSearchOption option)
	{
		Option = option;
	}
}
