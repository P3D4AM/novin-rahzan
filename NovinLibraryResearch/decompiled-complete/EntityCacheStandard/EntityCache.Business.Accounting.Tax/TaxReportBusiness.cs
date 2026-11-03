using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tax;

namespace EntityCache.Business.Accounting.Tax;

public class TaxReportBusiness : ITax, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public string Name { get; set; }

	public string Slug { get; set; }
}
