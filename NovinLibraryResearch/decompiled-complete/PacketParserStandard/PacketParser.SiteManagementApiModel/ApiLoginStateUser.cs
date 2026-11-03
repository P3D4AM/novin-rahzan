using System;
using PacketParser.Entities;

namespace PacketParser.SiteManagementApiModel;

public class ApiLoginStateUser
{
	public int UserID { get; set; }

	public short DepID { get; set; }

	public int User_Department_ID { get; set; }

	public EnLoginResult LoginResult { get; set; }

	public DateTime RegisterDate { get; set; }

	public ENSource Source { get; set; }
}
