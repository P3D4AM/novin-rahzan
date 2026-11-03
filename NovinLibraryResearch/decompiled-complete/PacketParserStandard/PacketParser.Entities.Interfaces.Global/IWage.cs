using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface IWage : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	EnWageType Type { get; set; }

	EnWageCalculationMethod CalculationMethod { get; set; }

	decimal PriceOrPercent { get; set; }

	decimal MinTransaction { get; set; }

	decimal MaxTransaction { get; set; }

	EnKhazaneMaster MasterType { get; set; }

	Guid? ServerGuid { get; set; }
}
