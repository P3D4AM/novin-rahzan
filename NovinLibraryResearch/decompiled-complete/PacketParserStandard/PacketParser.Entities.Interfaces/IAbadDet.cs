using System;

namespace PacketParser.Entities.Interfaces;

public interface IAbadDet
{
	decimal X { get; set; }

	decimal Y { get; set; }

	decimal Z { get; set; }

	decimal Count { get; set; }

	Shape ShapeType { get; set; }

	ShapeInfo ShapeInformation { get; set; }

	Guid Guid { get; set; }

	DateTime Modified { get; set; }
}
