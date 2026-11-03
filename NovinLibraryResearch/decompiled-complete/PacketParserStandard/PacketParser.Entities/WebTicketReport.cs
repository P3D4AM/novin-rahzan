using System;

namespace PacketParser.Entities;

public class WebTicketReport
{
	public Guid Guid { get; set; }

	public int Number { get; set; }

	public Guid SourceGuid { get; set; }

	public EnTargetType SourceType { get; set; }

	public string SourceName { get; set; }

	public string Title { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public EnTargetType TargetType { get; set; }

	public Guid TargetGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public string PartName { get; set; }

	public short StatusId { get; set; }

	public string StatusName { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public Priority Priority { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }
}
