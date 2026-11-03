using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IProductDecimalRelation
{
	Guid ProductGuid { get; set; }

	Guid DecimalGuid { get; set; }

	decimal Amount { get; set; }
}
