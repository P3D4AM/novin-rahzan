using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_Command : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime DateM { get; set; }

	string DateSH { get; set; }

	string Command { get; set; }

	string Sender { get; set; }

	Guid ScaleGuid { get; set; }
}
