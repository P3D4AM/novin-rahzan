using System;
using EntityCache.Business.Accounting.Tolid;
using PacketParser.Entities;

namespace EntityCache.Business.Extensions;

public class CheckProductAmountConfig
{
	public string ContextGuid { get; private set; }

	public DateTime Date { get; set; }

	public Document FactorType { get; set; }

	public bool IsTempOrder { get; set; }

	public bool IsDeleteAction { get; set; }

	public EnSystem System { get; set; }

	public TolidBusiness Tolid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; } = EntityChangeSt.New;


	public CheckProductAmountConfig(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
