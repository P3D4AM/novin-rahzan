using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Memory : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	Guid ScaleGuid { get; set; }

	int MyProperty { get; set; }

	Guid ProductGuid { get; set; }

	short MemoryNumber { get; set; }

	string MemoryName { get; set; }

	decimal MemoryPrice { get; set; }

	DateTime Date { get; set; }

	bool Taeed { get; set; }

	bool Vazni { get; set; }

	decimal TotalQT { get; set; }

	decimal TotalPrice { get; set; }
}
