using System;

namespace PacketParser.Entities.Interfaces.Accounting.Note;

public interface INote : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Title { get; set; }

	string Text { get; set; }

	DateTime DateM { get; set; }

	DateTime? DateSarResidM { get; set; }

	NotePirority Pirority { get; set; }

	Guid? CashierGuid { get; set; }

	Guid GroupGuid { get; set; }

	Guid? EntityGuid { get; set; }

	NoteDisplayState DisplayState { get; set; }

	DateTime? DateUtc { get; set; }

	Guid? TafsilGuid { get; set; }
}
