using System;

namespace PacketParser.DepartmentApiModel;

public class GetGardeshModel
{
	public Guid CusGuid { get; set; }

	public short DepId { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public int EachPageCount { get; set; }

	public int CurrentPageNumber { get; set; }
}
