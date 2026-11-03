using System;

namespace PacketParser.Entities.Interfaces.Accounting.Khazane;

public interface IHasWage
{
	Guid? WageGuid { get; set; }

	Guid? WageMoeinGuid { get; set; }

	Guid? WageTafsilGuid { get; set; }

	decimal WagePrice { get; set; }

	decimal Price { get; }
}
