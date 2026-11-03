using System;
using EntityCache.Business.Accounting.Tolid.Base;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidHazineBusiness : BaseHazineBusiness, IBaseHazineBusiness, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, ITolidFormulaRelation
{
	public Guid? TolidFormulaGuid { get; set; }

	public short TolidFormulaIndex { get; set; }

	public TolidHazineBusiness(string contextGuid)
		: base(contextGuid)
	{
	}
}
