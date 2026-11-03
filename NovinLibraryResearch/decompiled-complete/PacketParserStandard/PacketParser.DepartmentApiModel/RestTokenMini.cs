using System;

namespace PacketParser.DepartmentApiModel;

public class RestTokenMini
{
	public string Token { get; set; }

	public DateTime ExpirationDate { get; set; }

	public DateTime CreationDate { get; set; }
}
