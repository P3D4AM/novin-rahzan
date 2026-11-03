using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel.RequestModel;

public class OrderRequestModelcs : BaseRequestModel
{
	public List<ReadStatus> ReadStatusList { get; set; }

	public int Top { get; set; }

	public DateTime DateTimeTo { get; set; }
}
