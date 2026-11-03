using System;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IAdvGroup : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string GroupName { get; set; }

	Guid ParentGuid { get; set; }
}
