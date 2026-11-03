namespace PacketParser.Entities;

public class WebPrintMapingOrder : Serializable<WebPrintMapingOrder>
{
	public int UserId { get; set; }

	public string StrUserName { get; set; }

	public PrintType PrintType { get; set; }

	public string StrPrintType => PrintType.GetDisplay();

	public string Title { get; set; }

	public short Count { get; set; }
}
