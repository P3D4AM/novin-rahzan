using PacketParser.Entities;

namespace PacketParser.Filters;

public class ProductGroupFilter
{
	public EnSystem System { get; set; }

	public bool LoadJustParents { get; set; } = true;


	public bool? Status { get; set; } = true;


	public bool LoadParentWithGuidEmpty { get; set; } = true;


	public bool FillFullGroupName { get; set; }

	public EnProductType? Type { get; set; }
}
