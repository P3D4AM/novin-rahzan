using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.OrderDepot;

public class clsMultiVahed : IHasContext
{
	public Guid PrdGuid { get; set; }

	public string ContextGuid { get; set; }

	public string VahedName => Vahed?.Name ?? "";

	public Guid VahedGuid { get; set; }

	public decimal Quantity { get; set; }

	public PrdVahedBusiness Vahed => PrdVahedBusiness.Get(ContextGuid, VahedGuid);

	private clsMultiVahed(string contextGuid, Guid vahedGuid, ProductCompactBusiness product)
	{
		ContextGuid = contextGuid;
		VahedGuid = vahedGuid;
		PrdGuid = PrdGuid;
		Guid vahedGuid2 = VahedGuid;
		Guid? obj = product?.GuidVahed;
		if (vahedGuid2 == obj)
		{
			Quantity = 1m;
			return;
		}
		vahedGuid2 = VahedGuid;
		Guid? obj2 = product?.V2Guid;
		if (vahedGuid2 == obj2)
		{
			Quantity = product?.V2Qt ?? 0m;
		}
	}

	private async Task SetVahedAsync()
	{
		try
		{
			ProductBusiness productBusiness = await ProductBusiness.GetAsync(ContextGuid, PrdGuid);
			Guid vahedGuid = VahedGuid;
			Guid? v3Guid = productBusiness.V3Guid;
			if (vahedGuid == v3Guid)
			{
				Quantity = productBusiness?.V3Qt ?? 0m;
				return;
			}
			vahedGuid = VahedGuid;
			v3Guid = productBusiness.V4Guid;
			if (vahedGuid == v3Guid)
			{
				Quantity = productBusiness?.V4Qt ?? 0m;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\clsMultiVahed.cs", 50);
		}
	}

	public static async Task<clsMultiVahed> GetMiltiVahedInstanceAsync(string contextGuid, Guid vahedGuid, Guid prdGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		clsMultiVahed ret = new clsMultiVahed(contextGuid, vahedGuid, await ProductCompactBusiness.GetAsync(contextGuid, prdGuid, system, null, "", token, true));
		await ret.SetVahedAsync();
		return ret;
	}
}
