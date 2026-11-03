using PacketParser.Entities;

namespace PacketParser.Records.Products;

public record SearchLocationRecord
{
	public SearchLocation Location { get; private set; }

	public string LocationForDisplay => Location.GetDisplay().GetTranslate();

	public SearchLocationRecord(SearchLocation location)
	{
		Location = location;
	}
}
