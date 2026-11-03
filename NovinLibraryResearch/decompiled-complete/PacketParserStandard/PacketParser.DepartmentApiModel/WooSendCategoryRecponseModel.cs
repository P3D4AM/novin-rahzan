namespace PacketParser.DepartmentApiModel;

public class WooSendCategoryRecponseModel
{
	public long id { get; set; }

	public string name { get; set; }

	public string slug { get; set; }

	public long parent { get; set; }

	public string description { get; set; }

	public string display { get; set; }

	public int menu_order { get; set; }

	public Imagewoo image { get; set; }

	public int count { get; set; }
}
