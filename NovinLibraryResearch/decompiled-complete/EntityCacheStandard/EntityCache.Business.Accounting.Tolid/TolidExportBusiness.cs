using System;
using EntityCache.Business.Accounting.Tolid.Base;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidExportBusiness : BaseExportBusiness, IBaseExportBusiness, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, ITolidFormulaRelation
{
	public new decimal Price { get; set; }

	public string Serial { get; set; }

	public decimal? V2Qt { get; set; }

	public decimal? V2Price { get; set; }

	public Guid? TolidFormulaGuid { get; set; }

	public short TolidFormulaIndex { get; set; }

	public TolidExportBusiness(string contextGuid)
		: base(contextGuid)
	{
	}
}
