namespace PacketParser.Entities;

public class EnPriceTypeValueDisplay
{
	public EnPriceType value { get; set; }

	public string Display => value.GetDisplay();

	public EnPriceTypeValueDisplay(EnPriceType item)
	{
		value = item;
	}
}
