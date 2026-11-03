using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebReportGeneratorDet : Serializable<WebReportGeneratorDet>, IReportGeneratorDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string ParameterName { get; set; }

	public EnReportGeneratorFilterType ParameterType { get; set; }

	public short OrderIndex { get; set; }

	public bool Status { get; set; }
}
