using System;
using PacketParser.Entities;

namespace EntityCache.Business.Extensions;

public class CheckAccountConfig
{
	public EnSystem System { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public decimal Account { get; set; }

	public Document AsType { get; set; }

	public bool IsDeleteAction { get; set; }
}
