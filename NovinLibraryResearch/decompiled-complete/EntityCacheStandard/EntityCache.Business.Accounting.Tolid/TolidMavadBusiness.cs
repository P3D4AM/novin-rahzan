using System;
using EntityCache.Business.Accounting.Tolid.Base;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidMavadBusiness : BaseMavadBusiness, IBaseMavadBusiness, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, ITolidFormulaRelation
{
	public decimal? V2Qt { get; set; }

	public decimal? V2Price { get; set; }

	public decimal? RowSum { get; set; }

	public new decimal Price { get; set; }

	public Guid? TolidFormulaGuid { get; set; }

	public short TolidFormulaIndex { get; set; }

	public TolidMavadBusiness(string contextGuid)
		: base(contextGuid)
	{
	}
}
