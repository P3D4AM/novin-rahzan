using System;
using PacketParser.Entities.Attendance;

namespace PacketParser.BusinessLayer.Models.Attendance;

public class HardWareCodeCustomerModel
{
	public Guid Guid { get; set; }

	public Guid DeviceGuid { get; set; }

	public DeviceModelEnumType DeviceType { get; set; }

	public Guid PersonGuid { get; set; }

	public string PersonName { get; set; }

	public string HardWareName => PersonName.Negasht().Replace("-", "").Replace(" ", "");

	public string HardWareCode { get; set; }
}
