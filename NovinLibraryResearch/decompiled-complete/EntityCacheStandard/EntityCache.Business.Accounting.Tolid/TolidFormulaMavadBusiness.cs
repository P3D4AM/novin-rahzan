using System;
using EntityCache.Business.Accounting.Tolid.Base;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidFormulaMavadBusiness : BaseMavadBusiness, IBaseMavadBusiness, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public override decimal Price
	{
		get
		{
			if (base.ProductGuid == Guid.Empty || base.DepotGuid == Guid.Empty)
			{
				return 0m;
			}
			return (_lastKardexRow?.RemUnitPrice).GetValueOrDefault();
		}
	}

	public TolidFormulaMavadBusiness(string contextGuid)
		: base(contextGuid)
	{
	}
}
