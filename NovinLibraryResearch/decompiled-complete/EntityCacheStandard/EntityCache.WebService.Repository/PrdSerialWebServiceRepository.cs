using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class PrdSerialWebServiceRepository : WebServiceRepository<PrdSerialBusiness>, IDisposable
{
	public PrdSerialWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<PrdSerialBusiness>)cache.CacheRepository.IPrdSerial, cache)
	{
		Cache.Context.WebPrdSerial.OnReadResponse += WebPrdSerial_OnReadResponse;
	}

	private async Task WebPrdSerial_OnReadResponse(object sender, ConnectionEventArgs<WebPrdSerial> e)
	{
		using (new JaegerMonitor("WebPrdSerial_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PrdSerialWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("PrdSerial received but data is empty!", "", "WebPrdSerial_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PrdSerialWebServiceRepository.cs", 25);
					return;
				}
				List<WebPrdSerial> source = e.ConnectionData.ToList();
				List<PrdSerialBusiness> items = await PrdSerialMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				await PrdSerialBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, items);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebPrdSerial_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PrdSerialWebServiceRepository.cs", 32);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebPrdSerial.OnReadResponse -= WebPrdSerial_OnReadResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\PrdSerialWebServiceRepository.cs", 42);
		}
	}
}
