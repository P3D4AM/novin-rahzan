using System;

namespace PacketParser.Entities.Interfaces.Accounting.TTMS;

public interface ITTMSDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid AsGuid { get; set; }

	Document AsType { get; set; }

	string Description { get; set; }
}
