using System;

namespace PacketParser.DepartmentApiModel;

public class ApplicationSettingsRequestModel
{
	public short DepId { get; set; }

	public string KeyName { get; set; }

	public DateTime DateTime { get; set; }
}
