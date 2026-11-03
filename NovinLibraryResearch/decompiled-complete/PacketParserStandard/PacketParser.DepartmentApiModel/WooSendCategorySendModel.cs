namespace PacketParser.DepartmentApiModel;

public class WooSendCategorySendModel
{
	public string name { get; set; }

	public Imagewoo Image { get; set; }

	public WooSendCategorySendModel(string NameCategory, string imageAddress)
	{
		Image = new Imagewoo();
		Image.Src = imageAddress;
		name = NameCategory;
	}
}
