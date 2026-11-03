namespace PacketParser.Entities;

public class TelegramMessage : Serializable<TelegramMessage>
{
	private string _exceptionMessage = "";

	public int HashAll { get; set; }

	public ENSource Source { get; set; }

	public string Version { get; set; }

	public string ClassName { get; set; }

	public string FuncName { get; set; }

	public int Count { get; set; }

	public string ExceptionMessage
	{
		get
		{
			return _exceptionMessage?.Replace("\r\n", "\n").Replace("\r", "\r\n").Replace("\n", "\r\n");
		}
		set
		{
			_exceptionMessage = value;
		}
	}
}
