using PacketParser.Entities;

namespace PacketParser.Filters.Khazane;

public class WageFilter
{
	public EnWageType WageType { get; set; }

	public decimal Price { get; set; }

	public bool? Status { get; set; }

	public EnKhazaneMaster? MasterType { get; set; } = EnKhazaneMaster.Pardakht;

}
