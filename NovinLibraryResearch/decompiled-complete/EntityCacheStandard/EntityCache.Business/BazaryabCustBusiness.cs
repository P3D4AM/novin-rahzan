using System;
using System.Threading;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class BazaryabCustBusiness : Serializable<BazaryabCustBusiness>, IBazaryabCust, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid BazaryabGuid { get; set; }

	public Guid CustGuid { get; set; }

	public string Description { get; set; }

	public Guid Guid { get; set; }

	public Guid MantagheGuid { get; set; }

	public string ContextGuid { get; set; }

	public BazaryabCustBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
