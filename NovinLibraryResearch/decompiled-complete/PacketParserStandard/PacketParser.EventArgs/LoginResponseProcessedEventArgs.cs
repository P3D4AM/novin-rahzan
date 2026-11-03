using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class LoginResponseProcessedEventArgs : BaseEventArgs
{
	public string LoginResultMessage { get; set; }

	public EnLoginResult Status { get; set; }

	public WebUserInformation UserInfo { get; set; }

	public string CustomerUserName { get; set; } = "";


	public string CustomerPassword { get; set; } = "";


	[JsonIgnore]
	public ConnectionData connection { get; set; }
}
