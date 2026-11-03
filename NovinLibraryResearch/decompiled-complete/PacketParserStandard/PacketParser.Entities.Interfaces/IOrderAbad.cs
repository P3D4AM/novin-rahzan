using System;

namespace PacketParser.Entities.Interfaces;

public interface IOrderAbad : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Document OrderType { get; set; }

	Guid DetailGuid { get; set; }

	Guid RefrencedGuid { get; set; }

	ShapeInfo ShapeInfo { get; set; }
}
