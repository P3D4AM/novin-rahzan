using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_SellDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int PortNumber { get; set; }

	Guid ScaleGuid { get; set; }

	int FactorNumber { get; set; }

	short MemoryNumber { get; set; }

	decimal Quantity { get; set; }

	decimal UnitPrice { get; set; }

	bool Type_Weight { get; set; }

	DateTime DateM { get; set; }

	string DateSH_H { get; set; }

	DateTime DateUpdate { get; set; }

	string ComputerName { get; set; }

	bool IsRead { get; set; }

	short Ghorfe { get; set; }

	decimal RowSum { get; set; }

	int RInt1 { get; set; }

	int RInt2 { get; set; }

	string RText1 { get; set; }

	int RText2 { get; set; }

	decimal RDbl1 { get; set; }

	decimal RDbl2 { get; set; }

	string IP { get; set; }

	Guid SC_Guid { get; set; }
}
