using System;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Serilog;

namespace EntityCache.WebService.Repository;

public class GpsWebServiceRepository : WebServiceRepository<GPSBusiness>, IDisposable
{
	public GpsWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<GPSBusiness>)cache.CacheRepository.IGps, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebGPS.OnSaveResponse += GpsInstance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\GpsWebServiceRepository.cs", 20);
		}
	}

	public new virtual void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebGPS.OnSaveResponse -= GpsInstance_OnSaveResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\GpsWebServiceRepository.cs", 32);
		}
	}

	private async Task GpsInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("GpsInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\GpsWebServiceRepository.cs"))
		{
			int num = 0;
			try
			{
				if (e?.SaveResponses == null || e.SaveResponses.Count == 0)
				{
					return;
				}
				num = 2;
				num = 3;
				foreach (SaveResponse saveResponse in e.SaveResponses)
				{
					if (saveResponse != null)
					{
						num = 4;
					}
				}
			}
			catch (Exception ex)
			{
				if (base.IsDisposed)
				{
					Log.Warning(ex, $"Line:{num}//IsDisposed:{base.IsDisposed}");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{num}//IsDisposed:{base.IsDisposed}", "GpsInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\GpsWebServiceRepository.cs", 63);
				}
			}
		}
	}
}
