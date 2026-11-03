using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Note;

namespace PacketParser.Entities;

public class WebNoteGroup : INoteGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid ParentGuid { get; set; }

	public string Name { get; set; }

	public bool Status { get; set; }
}
