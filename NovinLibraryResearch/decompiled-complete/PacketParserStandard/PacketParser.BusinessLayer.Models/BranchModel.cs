using System;

namespace PacketParser.BusinessLayer.Models;

public class BranchModel
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }

	public string ManagerName { get; set; }

	public string State { get; set; }

	public string City { get; set; }

	public string Phone { get; set; }

	public string Address { get; set; }

	public string Description { get; set; }
}
