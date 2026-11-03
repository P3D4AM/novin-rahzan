using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_MemoryHistory : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ScaleGuid { get; set; }

	string ScaleName { get; set; }

	string ProductCode { get; set; }

	string BarCode { get; set; }

	string ProductName { get; set; }

	int MemoryNumber { get; set; }

	DateTime Date { get; set; }

	bool Vazni { get; set; }

	decimal UnitPrice { get; set; }
}
