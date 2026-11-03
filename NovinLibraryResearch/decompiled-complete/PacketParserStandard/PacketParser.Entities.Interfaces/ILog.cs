using System;

namespace PacketParser.Entities.Interfaces;

public interface ILog : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DateM { get; set; }

	string Message { get; set; }

	string UserDescription { get; set; }

	LogPartEnum LogPart { get; set; }
}
