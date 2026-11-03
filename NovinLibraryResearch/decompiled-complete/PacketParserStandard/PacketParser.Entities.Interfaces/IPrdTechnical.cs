using System;

namespace PacketParser.Entities.Interfaces;

public interface IPrdTechnical : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Code { get; set; }

	string Name { get; set; }

	short TechnicalType { get; set; }

	Guid ProductGuid { get; set; }
}
