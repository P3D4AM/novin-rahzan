namespace PacketParser.EventArgs;

public class RecoverPasswordModel : BaseEventArgs
{
	public string UserName { get; set; }

	public string MobileNumber { get; set; }

	public string Email { get; set; }
}
