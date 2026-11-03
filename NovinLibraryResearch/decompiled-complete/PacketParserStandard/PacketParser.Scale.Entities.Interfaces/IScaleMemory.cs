using System;

namespace PacketParser.Scale.Entities.Interfaces;

public interface IScaleMemory
{
	string ContextGuid { get; set; }

	string CurrentName { get; }

	decimal CurrentPrice { get; }

	bool CurrentVazni { get; }

	DateTime DateM { get; set; }

	bool IsSaved { get; set; }

	int MemoryNumber { get; set; }

	Guid PrdGuid { get; set; }

	int ScaleId { get; set; }

	int RowVersion { get; set; }
}
