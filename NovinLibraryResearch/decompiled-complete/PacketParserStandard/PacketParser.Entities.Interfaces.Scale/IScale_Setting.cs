using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Setting : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Val { get; set; }

	Guid UserGuid { get; set; }
}
