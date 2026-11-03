using System;

namespace PacketParser.Entities.Interfaces.Accounting.Tolid;

public interface ITolidFormulaRelation
{
	Guid? TolidFormulaGuid { get; set; }

	short TolidFormulaIndex { get; set; }
}
