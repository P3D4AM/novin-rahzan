using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Product;

public class ProductKindForDisplay
{
	public EnProductKind Kind { get; private set; }

	public string KindForDisplay => Kind.GetDisplay().GetTranslate();

	public ProductKindForDisplay(EnProductKind kind)
	{
		Kind = kind;
	}
}
