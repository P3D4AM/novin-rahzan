using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Mem : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ScaleGuid { get; set; }

	Guid ProductGuid { get; set; }

	int MemoryNumber { get; set; }

	DateTime Date { get; set; }

	bool ISSaved { get; set; }

	decimal PriceOnScale { get; set; }

	string CodeOnScale { get; set; }

	string NameScale { get; set; }

	string BarCode { get; set; }

	string Name { get; set; }

	decimal Price { get; set; }

	bool Vazni { get; set; }
}
