using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ICusCompact : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Name { get; set; }

	string Family { get; set; }

	bool Special { get; set; }

	string PicFace { get; set; }

	string PicSign { get; set; }

	Guid GroupGuid { get; set; }

	Guid TitleGuid { get; set; }

	bool IsBazaryab { get; set; }

	bool IsSupervisor { get; set; }

	bool IsEtebari { get; set; }

	string FullName { get; }

	ServerStatusForClient ServerStatus { get; set; }
}
