using System;

namespace PacketParser.Entities.Interfaces;

public interface IProductPriceUpdate : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ProductGuid { get; set; }

	decimal RegularPrice { get; set; }

	DateTime DateM { get; set; }
}
