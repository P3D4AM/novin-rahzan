using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Product;

public interface IHasProductUpdateDetailModel
{
	EnProductUpdateKeys Key { get; set; }

	object Value { get; set; }
}
