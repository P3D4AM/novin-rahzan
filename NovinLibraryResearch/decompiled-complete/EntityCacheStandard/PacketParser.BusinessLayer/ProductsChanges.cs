using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.BusinessLayer;

public class ProductsChanges : IHasContext
{
	private ProductCompactBusiness _prdInfo;

	public Guid PrdGuid { get; set; }

	public Guid depGuid { get; set; }

	public decimal qt { get; set; }

	public string ContextGuid { get; set; }

	public ProductsChanges(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ProductCompactBusiness> GetPrdInfoAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (PrdGuid == Guid.Empty)
			{
				_prdInfo = null;
				return _prdInfo;
			}
			if ((_prdInfo?.Guid ?? Guid.Empty) != PrdGuid && PrdGuid != Guid.Empty)
			{
				_prdInfo = await ProductCompactBusiness.GetAsync(ContextGuid, PrdGuid, system, null, tranName, token, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPrdInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\ProductsChanges.cs", 33);
		}
		return _prdInfo;
	}

	internal async Task<string> DepotNameAsync(CancellationToken token, string tranName)
	{
		try
		{
			return (await DepotBusiness.GetAsync(ContextGuid, depGuid, token, tranName))?.Name ?? "Null!!";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DepotNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\ProductsChanges.cs", 45);
			return "";
		}
	}
}
