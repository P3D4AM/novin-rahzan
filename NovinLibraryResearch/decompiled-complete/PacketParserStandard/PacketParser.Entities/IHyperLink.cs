using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IHyperLink : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Category { get; set; }

	bool Enable { get; set; }

	string Icon { get; set; }

	short Order { get; set; }

	Guid ParentGuid { get; set; }

	string Style { get; set; }

	string Target { get; set; }

	string Title { get; set; }

	string Url { get; set; }
}
