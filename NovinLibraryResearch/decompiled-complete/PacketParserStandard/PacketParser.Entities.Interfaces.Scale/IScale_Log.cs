using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Log : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription
{
	DateTime DateM { get; set; }
}
