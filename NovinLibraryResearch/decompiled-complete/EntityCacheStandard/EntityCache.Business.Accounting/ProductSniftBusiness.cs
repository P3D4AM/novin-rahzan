using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Mapping.BusinessMappers.Snift;
using PacketParser;
using PacketParser.Compression;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Snifts;

namespace EntityCache.Business.Accounting;

public class ProductSniftBusiness : IHasContext
{
	public string ContextGuid { get; private set; }

	public long LkSerial { get; private set; }

	public ProductSniftBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task SendToServerAsync(long lkSerial, CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			await Task.Delay(300000);
			while (!token.IsCancellationRequested && !(ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true))
			{
				LkSerial = lkSerial;
				IEnumerable<ProductCompactBusiness> enumerable = await ClsCache.GetCache(ContextGuid).CacheRepository.IProductCompact.GetAllForSendToServerSniftsAsync(token, "");
				if (enumerable == null || !enumerable.Any())
				{
					break;
				}
				foreach (ProductCompactBusiness item in enumerable)
				{
					await SendAsync(await item.GetFullProductAsync(token), token);
				}
				await Task.Delay(600000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSniftBusiness.cs", 41);
		}
	}

	private async Task SendAsync(ProductBusiness prd, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (prd == null)
			{
				return;
			}
			WebProductSnift mappedItem = await ProductSniftMapper.Instance.MapAsync(ContextGuid, prd, null, "", token);
			KardexBusiness kardexBusiness = await KardexBusiness.GetLastAsync(ContextGuid, null, prd.Guid, Document.OrderBuy, token);
			if (kardexBusiness != null)
			{
				OrderBusiness order = await OrderBusiness.GetAsync(ContextGuid, kardexBusiness.SanadGuid, Document.OrderBuy, fromDatabase: true, token);
				OrderDetailBusiness orderDetailBusiness = order?.ListKala?.FirstOrDefault((OrderDetailBusiness q) => q.ProductGuid == prd.Guid);
				if (orderDetailBusiness != null)
				{
					WebProductSnift webProductSnift = mappedItem;
					webProductSnift.LastOrderBuy = await OrderSniftMapper.Instance.MapAsync(ContextGuid, orderDetailBusiness, null, "", token);
				}
				CusCompactBusiness seller = await CusCompactBusiness.GetAsync(ContextGuid, EnSystem.None, order?.TafsilGuid ?? Guid.Empty, null, "", token);
				if (seller != null && mappedItem.LastOrderBuy != null)
				{
					WebOrderSnifts lastOrderBuy = mappedItem.LastOrderBuy;
					lastOrderBuy.Seller = await CustomerSniftMapper.Instance.MapAsync(ContextGuid, seller, null, "", token);
					List<TafsilNumberBusiness> list = await TafsilNumberBusiness.GetAsync(ContextGuid, seller.Guid, token);
					if (list != null && list.Any())
					{
						WebCustomerSnifts seller2 = mappedItem.LastOrderBuy.Seller;
						seller2.TafsilNumberList = await TafsilNumberMapper.Instance.MapListAsync(ContextGuid, list, new List<WebTafsilNumbers>(), "", token);
					}
				}
			}
			if (!(await HttpHelpers.PostToApi((await GZip.Compress(Json.ToBytesJson(mappedItem)))?.ToList(), headers: new Dictionary<string, string> { 
			{
				"LkSerial",
				LkSerial.ToString()
			} }, url: HttpHelpers.ApiUrl + "LkActivation/Save", strongDeserialize: false, timeOutMilliSeconds: 15000, tryCount: 3)).HasError)
			{
				prd.SendToServerDate = DateTime.Now;
				await prd.UpdateSendToServerDateAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSniftBusiness.cs", 81);
		}
	}
}
