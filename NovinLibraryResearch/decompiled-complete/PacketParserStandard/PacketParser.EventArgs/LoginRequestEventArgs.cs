using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class LoginRequestEventArgs : Serializable<LoginRequestEventArgs>
{
	[JsonIgnore]
	public LoginExtention ConnectionData { get; set; }

	[JsonIgnore]
	public string ConnectionUniqueId { get; set; }

	[JsonIgnore]
	public string Ip { get; set; }

	[JsonIgnore]
	public WebContextHolder Context { get; set; }
}
