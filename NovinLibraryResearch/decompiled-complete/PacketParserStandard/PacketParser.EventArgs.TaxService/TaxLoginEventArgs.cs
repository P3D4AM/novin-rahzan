namespace PacketParser.EventArgs.TaxService;

public class TaxLoginEventArgs
{
	public string Token { get; set; }

	public long TimeOutExpire { get; set; }

	public string Message { get; set; }

	public bool IsValid => !string.IsNullOrEmpty(Token);
}
