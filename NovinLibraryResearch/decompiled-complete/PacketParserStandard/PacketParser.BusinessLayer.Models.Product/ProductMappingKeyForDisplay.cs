using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Product;

public class ProductMappingKeyForDisplay
{
	public EnProductMappingKey Key { get; private set; }

	public string KeyForDisplay => Key.GetDisplay().GetTranslate();

	public ProductMappingKeyForDisplay(EnProductMappingKey key)
	{
		Key = key;
	}
}
