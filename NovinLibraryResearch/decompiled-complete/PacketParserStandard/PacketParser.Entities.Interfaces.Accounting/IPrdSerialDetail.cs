using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IPrdSerialDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	Guid DetailGuid { get; set; }

	Document AsType { get; set; }

	bool IsVorod { get; set; }

	short Ordinal { get; set; }

	DateTime? SanadDate { get; set; }
}
