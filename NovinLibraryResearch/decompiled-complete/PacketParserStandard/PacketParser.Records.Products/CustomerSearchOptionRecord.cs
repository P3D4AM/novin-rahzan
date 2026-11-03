using PacketParser.Entities;

namespace PacketParser.Records.Products;

public record CustomerSearchOptionRecord
{
	public CustomerSearchOption Option { get; private set; }

	public string OptionForDisplay => Option.GetDisplay().GetTranslate();

	public CustomerSearchOptionRecord(CustomerSearchOption option)
	{
		Option = option;
	}
}
