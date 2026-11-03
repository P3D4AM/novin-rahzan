using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface IPost : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	byte[] Image { get; set; }

	string Link { get; set; }

	DateTime Startdate { get; set; }

	DateTime EndDate { get; set; }

	string Title { get; set; }

	EnTypePost Type { get; set; }
}
