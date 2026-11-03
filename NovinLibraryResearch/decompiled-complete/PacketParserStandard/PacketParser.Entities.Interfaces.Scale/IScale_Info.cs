using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Info : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	Guid ScaleGuid { get; set; }

	int PortNumber { get; set; }

	int TimerInterval { get; set; }

	string Name { get; set; }

	bool Deleted { get; set; }

	bool SellJamandeh { get; set; }

	string IP { get; set; }

	string EndMessage { get; set; }

	string ShopName { get; set; }

	bool AccountingMemory { get; set; }

	bool AccountingDate { get; set; }

	bool AccountingDivision { get; set; }

	bool AccountingID { get; set; }

	bool AccountingIP { get; set; }

	bool MNGLock { get; set; }

	short FactorNumber { get; set; }

	short ConnectionType { get; set; }

	string Model { get; set; }

	string Serial { get; set; }
}
