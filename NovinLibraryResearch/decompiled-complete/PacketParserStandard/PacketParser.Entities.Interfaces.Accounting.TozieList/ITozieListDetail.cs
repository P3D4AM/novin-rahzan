using System;

namespace PacketParser.Entities.Interfaces.Accounting.TozieList;

public interface ITozieListDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid AsGuid { get; set; }

	Document AsType { get; set; }
}
