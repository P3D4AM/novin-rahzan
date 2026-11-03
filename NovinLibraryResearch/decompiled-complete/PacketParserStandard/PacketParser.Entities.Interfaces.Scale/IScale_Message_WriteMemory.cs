using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Message_WriteMemory : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Code { get; set; }

	string Barcode { get; set; }

	string Name { get; set; }

	decimal UnitPrice { get; set; }

	DateTime DateM { get; set; }

	DateTime Expiredate { get; set; }

	short ST { get; set; }

	bool Vazni { get; set; }

	short NextTry { get; set; }

	Guid ScaleGuid { get; set; }

	short TryCount { get; set; }

	short MaxTryCount { get; set; }

	string Parametr1 { get; set; }

	string Parametr2 { get; set; }

	string Parametr3 { get; set; }

	string Parametr4 { get; set; }

	int MemoryNumber { get; set; }
}
