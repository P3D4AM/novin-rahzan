using System;
using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebReportGenerator : Serializable<WebReportGenerator>, IReportGenerator, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string Title { get; set; }

	public string Description { get; set; }

	public string Rpt { get; set; }

	public int Version { get; set; }

	public DateTime LastUpdate { get; set; }

	public string CompatibleVerStart { get; set; }

	public string ComPatibleVerEnd { get; set; }

	public List<WebReportGeneratorDet> webReportGeneratorDet { get; set; }

	public StiType? Type { get; set; }

	public Document? Document { get; set; }

	public MaliYearSanadType MaliYearSanadType { get; set; }
}
