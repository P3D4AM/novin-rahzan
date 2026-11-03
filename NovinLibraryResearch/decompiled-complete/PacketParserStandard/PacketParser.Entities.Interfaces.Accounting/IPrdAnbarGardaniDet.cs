using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IPrdAnbarGardaniDet : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid MasterGuid { get; set; }

	Guid PrdGuid { get; set; }

	Guid DptGuid { get; set; }

	decimal QtKasr { get; set; }

	decimal QtEzafe { get; set; }

	decimal UnitPrice { get; set; }

	decimal KasrSum { get; }

	decimal EzafeSum { get; }

	decimal? QtRealAmount { get; set; }
}
