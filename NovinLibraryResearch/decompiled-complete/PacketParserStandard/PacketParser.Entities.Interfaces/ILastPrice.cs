using System;

namespace PacketParser.Entities.Interfaces;

public interface ILastPrice : IHasModified, IHasJustGuid, IHasStatus
{
	DateTime Date { get; set; }

	decimal Price { get; set; }

	Guid TafsilGuid { get; set; }

	Guid PrdGuid { get; set; }

	Guid AsGuid { get; set; }

	Document AsType { get; set; }

	bool Invalidated { get; set; }
}
