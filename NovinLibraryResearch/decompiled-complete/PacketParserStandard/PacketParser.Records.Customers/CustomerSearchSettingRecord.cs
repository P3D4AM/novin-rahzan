using PacketParser.Entities;

namespace PacketParser.Records.Customers;

public record CustomerSearchSettingRecord
{
	public CustomerSearchOption Option { get; set; }

	public SearchLocation Location { get; set; }
}
