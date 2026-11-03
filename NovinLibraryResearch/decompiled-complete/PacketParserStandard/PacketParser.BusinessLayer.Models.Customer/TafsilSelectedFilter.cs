using System;

namespace PacketParser.BusinessLayer.Models.Customer;

public class TafsilSelectedFilter
{
	public bool? Status { get; set; }

	public Guid? TafsilGuid { get; set; }
}
