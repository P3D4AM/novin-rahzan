using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IScript : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DateM { get; set; }

	string Description { get; set; }

	string ExecDesc { get; set; }

	int ExecSt { get; set; }

	DateTime? ExecTime { get; set; }

	DateTime NextTime { get; set; }

	string Script { get; set; }
}
