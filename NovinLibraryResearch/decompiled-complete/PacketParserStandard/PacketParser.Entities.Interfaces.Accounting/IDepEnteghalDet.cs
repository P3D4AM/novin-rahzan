using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IDepEnteghalDet : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid MasterGuid { get; set; }

	Guid DptGuidSrc { get; set; }

	Guid DptGuidDst { get; set; }

	short Ordinal { get; set; }

	decimal Quantity { get; set; }

	Guid? VahedGuid { get; set; }

	string Description { get; set; }

	string Serial { get; set; }

	decimal Price { get; set; }

	decimal V2Qt { get; set; }

	decimal V2Price { get; set; }
}
