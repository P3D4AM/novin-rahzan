namespace PacketParser.Entities;

public class ServerCheckLogin
{
	public string DepartmentUserName { get; set; }

	public string DepartmentPassword { get; set; }

	public string RestToken { get; set; }
}
