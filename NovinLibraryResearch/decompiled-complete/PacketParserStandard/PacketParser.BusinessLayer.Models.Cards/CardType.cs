using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Cards;

public class CardType
{
	public EnCardType Type { get; set; }

	public string TypeName => Type.GetDisplay().GetTranslate();

	public CardType(EnCardType _type)
	{
		Type = _type;
	}
}
