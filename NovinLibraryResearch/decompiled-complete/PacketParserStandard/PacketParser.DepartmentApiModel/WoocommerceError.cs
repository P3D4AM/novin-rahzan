namespace PacketParser.DepartmentApiModel;

public class WoocommerceError
{
	public string code { get; set; }

	public string message { get; set; }

	public DataErrorWoocommerce data { get; set; }
}
