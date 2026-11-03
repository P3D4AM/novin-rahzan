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

public class CustomerSniftBusiness : IHasContext
{
	public string ContextGuid { get; private set; }

	public long LkSerial { get; private set; }

	public CustomerSniftBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task SendToServerAsync(long lkSerial, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			await Task.Delay(90000);
			while (!token.IsCancellationRequested && !(ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true))
			{
				LkSerial = lkSerial;
				IEnumerable<CusCompactBusiness> enumerable = await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomerCompact.GetAllForSendToServerSniftsAsync(token, "");
				if (enumerable == null || !enumerable.Any())
				{
					break;
				}
				foreach (CusCompactBusiness item in enumerable)
				{
					await SendAsync(item);
				}
				await Task.Delay(600000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerSniftBusiness.cs", 39);
		}
	}

	private async Task SendAsync(CusCompactBusiness customer)
	{
		_ = 6;
		try
		{
			CancellationToken token = default(CancellationToken);
			if (customer == null)
			{
				return;
			}
			WebCustomerSnifts mappedItem = await CustomerSniftMapper.Instance.MapAsync(ContextGuid, customer, null, "", token);
			List<TafsilNumberBusiness> list = await TafsilNumberBusiness.GetAsync(ContextGuid, customer.Guid, token);
			if (list != null && list.Any())
			{
				WebCustomerSnifts webCustomerSnifts = mappedItem;
				webCustomerSnifts.TafsilNumberList = await TafsilNumberMapper.Instance.MapListAsync(ContextGuid, list, new List<WebTafsilNumbers>(), "", token);
			}
			if (!(await HttpHelpers.PostToApi((await GZip.Compress(Json.ToBytesJson(mappedItem)))?.ToList(), headers: new Dictionary<string, string> { 
			{
				"LkSerial",
				LkSerial.ToString()
			} }, url: HttpHelpers.ApiUrl + "ActivationLk/Save", strongDeserialize: false, timeOutMilliSeconds: 15000, tryCount: 3)).HasError)
			{
				CusBusiness cusBusiness = await CusBusiness.GetAsync(ContextGuid, EnSystem.None, customer.Guid, token);
				if (cusBusiness != null)
				{
					await cusBusiness.UpdateSendToServerDateAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerSniftBusiness.cs", 68);
		}
	}
}
