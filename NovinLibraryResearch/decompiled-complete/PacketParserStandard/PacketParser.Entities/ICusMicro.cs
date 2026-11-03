using System.Collections.Generic;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ICusMicro : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string FullName { get; set; }

	List<WebTafsilNumbers> Numbers { get; set; }
}
