namespace PacketParser.Entities;

public class WooOrder : Serializable<WooOrder>
{
	public int Id { get; set; }

	public string OrderKey { get; set; }

	public string ShippingFirstName { get; set; }

	public string ShippingLastName { get; set; }

	public string ShippingCompany { get; set; }

	public string ShippingAddress1 { get; set; }

	public string ShippingAddress2 { get; set; }

	public string ShippingCity { get; set; }

	public WooOrderStatus State { get; set; }

	public string Postcode { get; set; }

	public string Phone { get; set; }

	public decimal Tax { get; set; }

	public decimal Shipping { get; set; }

	public decimal Discount { get; set; }

	public decimal Total { get; set; }
}
