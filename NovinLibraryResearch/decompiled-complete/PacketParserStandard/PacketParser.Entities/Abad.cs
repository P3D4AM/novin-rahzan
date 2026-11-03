using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class Abad : IAbadDet
{
	public decimal X { get; set; }

	public decimal Y { get; set; }

	public decimal Z { get; set; }

	public decimal Count { get; set; }

	public Shape ShapeType { get; set; }

	public ShapeInfo ShapeInformation { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }
}
