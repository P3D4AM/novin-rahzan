using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Product;

public class ProductUpdateDetailModel : IHasProductUpdateDetailModel
{
	public EnProductUpdateKeys Key { get; set; }

	public object Value { get; set; }
}
