using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebCheckedSave : ICheckedSave, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid EntityGuid { get; set; }

	public Guid DetailGuid { get; set; }

	public bool IsDebit { get; set; }

	public checkedTablReportType ReportType { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;

}
