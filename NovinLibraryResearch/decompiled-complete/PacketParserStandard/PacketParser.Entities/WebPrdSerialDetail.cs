using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebPrdSerialDetail : IPrdSerialDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public Guid DetailGuid { get; set; }

	public Document AsType { get; set; }

	public bool IsVorod { get; set; }

	public DateTime Modified { get; set; }

	public bool Status => true;

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public short Ordinal { get; set; }

	public DateTime? SanadDate { get; set; }
}
