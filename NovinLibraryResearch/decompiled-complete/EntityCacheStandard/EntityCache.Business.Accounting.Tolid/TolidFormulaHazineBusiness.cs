using EntityCache.Business.Accounting.Tolid.Base;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidFormulaHazineBusiness : BaseHazineBusiness, IBaseHazineBusiness, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public TolidFormulaHazineBusiness(string contextGuid)
		: base(contextGuid)
	{
	}
}
