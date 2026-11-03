namespace PacketParser.Entities.Product;

public class ProductPrice
{
	private decimal price;

	public decimal Price
	{
		get
		{
			return price.FixDecimal(18, 3);
		}
		set
		{
			price = value;
		}
	}

	public string WordPressRoleName { get; set; }
}
