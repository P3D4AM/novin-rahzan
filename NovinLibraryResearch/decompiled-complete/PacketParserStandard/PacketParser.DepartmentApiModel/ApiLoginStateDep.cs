using System;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class ApiLoginStateDep
{
	public string UserName { get; set; }

	public string UserPass { get; set; }

	public short DepID { get; set; }

	public EnLoginResult LoginResult { get; set; }

	public DateTime RegisterDate { get; set; }
}
