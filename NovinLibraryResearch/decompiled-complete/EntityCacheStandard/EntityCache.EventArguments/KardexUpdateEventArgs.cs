using System;

namespace EntityCache.EventArguments;

public class KardexUpdateEventArgs
{
	public Guid? ProductGuid { get; private set; }

	public Guid? DepotGuid { get; private set; }

	public KardexUpdateEventArgs(Guid? productGuid, Guid? depotGuid)
	{
		ProductGuid = productGuid;
		DepotGuid = depotGuid;
	}
}
