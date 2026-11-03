using System;
using System.Collections.Generic;
using System.Drawing;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Global.Tickets;

public class TicketReportBusiness : ITicket, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
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

	public IReadOnlyCollection<FileInfoBusiness> AddedFiles { get; set; }

	public IReadOnlyCollection<FileInfoBusiness> RemovedFiles { get; set; }

	public bool Status { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	[JsonIgnore]
	public byte[] StatusImage => ImageResourceManager.TicketStatusImage;

	[JsonIgnore]
	public byte[] SeeImage => ImageResourceManager.TicketSeeImage;

	[JsonIgnore]
	public byte[] AnswerImage => ImageResourceManager.TicketAnswerImage;

	[JsonIgnore]
	public byte[] DeleteImage => ImageResourceManager.TicketDeleteImage;

	[JsonIgnore]
	public Color BackColor { get; set; }

	[JsonIgnore]
	public Color ForeColor { get; set; }
}
