using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Note;

namespace AccSqlServerPersistence.Entities.Accounting.Note;

[Table("Acc_NoteGroup")]
public class NoteGroup : INoteGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid ParentGuid { get; set; }

	[Required]
	public string Name { get; set; }

	public bool Status { get; set; }

	public ICollection<Note> Notes { get; set; }

	public string? MetaData { get; set; }
}
