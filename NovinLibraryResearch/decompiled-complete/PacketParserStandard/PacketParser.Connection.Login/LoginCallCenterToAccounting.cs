using PacketParser.Entities;

namespace PacketParser.Connection.Login;

public class LoginCallCenterToAccounting
{
	public string UserName { get; set; }

	public string Password { get; set; }

	public int ExtentionNumber { get; set; }

	public TypeLogin TypeLogin { get; set; }
}
