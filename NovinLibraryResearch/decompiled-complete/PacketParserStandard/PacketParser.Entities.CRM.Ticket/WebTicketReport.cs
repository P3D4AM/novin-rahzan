using System;
using System.Drawing;
using System.Text.Json.Serialization;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities.CRM.Ticket;

internal class WebTicketReport : ITicket, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public int Number { get; set; }

	[JsonIgnore]
	public string SourceTypeForDisplay => SourceType.GetDisplay().GetTranslate();

	public Guid SourceGuid { get; set; }

	public Guid? MasterGuid { get; set; }

	public EnTargetType SourceType { get; set; }

	public string SourceName { get; set; }

	public string Title { get; set; }

	public DateTime DateM { get; set; }

	[JsonIgnore]
	public string DateSh => DateM.CallCenterDate();

	[JsonIgnore]
	public string Time => DateM.ToShortTimeString();

	public string Description { get; set; }

	public EnTargetType TargetType { get; set; }

	public Guid TargetGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public string PartName { get; set; }

	public short StatusId { get; set; }

	public string StatusColor { get; set; }

	public string StatusName { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public Priority Priority { get; set; }

	[JsonIgnore]
	public string PriorityForDisplay => Priority.GetDisplay();

	public DateTime Modified { get; set; }

	[JsonIgnore]
	public string ModifiedSh => Modified.CallCenterDate();

	[JsonIgnore]
	public string ModifiedTime => Modified.ToShortTimeString();

	public bool Status { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	[JsonIgnore]
	public Color BackColor { get; set; }

	[JsonIgnore]
	public Color ForeColor { get; set; }
}
