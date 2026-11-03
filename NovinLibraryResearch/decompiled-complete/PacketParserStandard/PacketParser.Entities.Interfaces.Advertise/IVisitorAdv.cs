using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IVisitorAdv : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid VisitorGuid { get; set; }

	Guid AdvGuid { get; set; }
}
