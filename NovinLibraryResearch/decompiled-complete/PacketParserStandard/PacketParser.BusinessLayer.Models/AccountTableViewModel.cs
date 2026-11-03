using System;

namespace PacketParser.BusinessLayer.Models;

public class AccountTableViewModel
{
	public Guid Guid { get; set; }

	public Guid MoeinGuid { get; set; }

	public long MoeinCode { get; set; }

	public string MoeinName { get; set; }

	public Guid TafsilGuid { get; set; }

	public long TafsilCode { get; set; }

	public string TafsilName { get; set; }

	public decimal Account { get; set; }
}
