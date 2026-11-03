using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface IStimulQuery : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int? Id { get; set; }

	int RefrencedId { get; set; }

	int Order { get; set; }

	DateTime Date { get; set; }

	string SecondKey { get; set; }
}
