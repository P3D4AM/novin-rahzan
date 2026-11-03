using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class LoginResponseEventArgs : Serializable<LoginResponseEventArgs>
{
	[JsonIgnore]
	[XmlIgnore]
	public WebContextHolder Context { get; set; }

	public string LoginResultMessage { get; set; }

	public EnLoginResult Status { get; set; }

	public WebUserInformation UserInfo { get; set; }

	[JsonIgnore]
	public string ConnectionUniqueId { get; set; }

	public string CustomerUserName { get; set; } = "";


	public string CustomerPassword { get; set; } = "";


	[JsonIgnore]
	public ConnectionData connection { get; set; }
}
