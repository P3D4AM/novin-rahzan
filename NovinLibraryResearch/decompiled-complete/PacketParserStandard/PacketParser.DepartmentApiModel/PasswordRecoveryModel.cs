using System;

namespace PacketParser.DepartmentApiModel;

public class PasswordRecoveryModel
{
	public Guid Token { get; set; }

	public string Username { get; set; }

	public string Mobile { get; set; }

	public string EMail { get; set; }
}
