using System;

namespace PacketParser.Entities.Interfaces.Accounting.Note;

public interface INoteGroup : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	new DateTime Modified { get; set; }

	Guid ParentGuid { get; set; }

	string Name { get; set; }
}
