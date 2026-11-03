using System;

namespace PacketParser.Entities.Interfaces.Accounting.Coding;

public interface IAccount : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid MoeinGuid { get; set; }

	Guid TafsilGuid { get; set; }

	decimal Account { get; set; }
}
