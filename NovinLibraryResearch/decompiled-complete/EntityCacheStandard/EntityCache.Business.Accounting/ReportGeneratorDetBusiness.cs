using System;
using System.Threading;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class ReportGeneratorDetBusiness : Serializable<ReportGeneratorDetBusiness>, IReportGeneratorDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string ParameterName { get; set; }

	public EnReportGeneratorFilterType ParameterType { get; set; }

	public short OrderIndex { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public ReportGeneratorDetBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
