using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class NoteFilter
{
	public string Search { get; set; } = "";


	public DateTime? Date1 { get; set; }

	public DateTime? Date2 { get; set; }

	public DateTime? DateSarresidFrom { get; set; }

	public DateTime? DateSarresidTo { get; set; }

	public Guid? CashierGuid { get; set; }

	public NotePirority? Pirority { get; set; }

	public NoteStatus? Status { get; set; }

	public List<Guid> GroupGuids { get; set; }

	public Guid? AsGuid { get; set; }

	public Document? AsType { get; set; }
}
