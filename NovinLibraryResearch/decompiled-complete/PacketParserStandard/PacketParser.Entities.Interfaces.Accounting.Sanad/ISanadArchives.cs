using System;

namespace PacketParser.Entities.Interfaces.Accounting.Sanad;

public interface ISanadArchives : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid AsGuid { get; set; }

	Document AsType { get; set; }

	byte[] Data { get; set; }

	decimal Price { get; set; }

	string Description { get; set; }
}
