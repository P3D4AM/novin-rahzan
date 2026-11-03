using System;

namespace PacketParser.Scale.Entities.Interfaces;

public interface IScaleMessage_ReadSale
{
	short Count { get; set; }

	DateTime DateM { get; set; }

	int Id { get; set; }

	int MemId { get; set; }

	short Priority { get; set; }

	int ScaleId { get; set; }
}
