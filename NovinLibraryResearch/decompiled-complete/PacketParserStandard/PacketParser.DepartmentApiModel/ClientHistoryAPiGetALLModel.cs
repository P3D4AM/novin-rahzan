using System;

namespace PacketParser.DepartmentApiModel;

public class ClientHistoryAPiGetALLModel
{
	public string UserName { get; set; }

	public string Password { get; set; }

	public int? Top { get; set; }

	public long? HashId { get; set; }

	public DateTime? StarTime { get; set; }
}
