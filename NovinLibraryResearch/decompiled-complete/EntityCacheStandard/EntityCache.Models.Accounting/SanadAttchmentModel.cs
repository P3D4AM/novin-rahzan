using System;
using PacketParser.Entities;

namespace EntityCache.Models.Accounting;

public class SanadAttchmentModel
{
	public Document AsType { get; set; }

	public Guid AsGuid { get; set; }

	public Guid SanadDetailGuid { get; set; }
}
