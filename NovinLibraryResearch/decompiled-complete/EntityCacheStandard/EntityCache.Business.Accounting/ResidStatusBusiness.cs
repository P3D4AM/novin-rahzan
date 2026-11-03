using System;
using System.Threading;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class ResidStatusBusiness : Serializable<ResidStatusBusiness>, IResidStatus, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string Name { get; set; }

	public bool Status { get; set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public ResidStatusBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
