using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class WebResponseModelDetail
{
	public string Message { get; set; }

	public string Key { get; set; }

	public ReturnedState State { get; set; }
}
