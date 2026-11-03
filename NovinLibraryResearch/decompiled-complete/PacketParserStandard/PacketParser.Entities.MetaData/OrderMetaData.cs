namespace PacketParser.Entities.MetaData;

public class OrderMetaData
{
	public int PeopleCount { get; set; }

	public OrderTaxMetaData Tax { get; set; }

	public WebOrder BackSellForTaxService { get; set; }
}
