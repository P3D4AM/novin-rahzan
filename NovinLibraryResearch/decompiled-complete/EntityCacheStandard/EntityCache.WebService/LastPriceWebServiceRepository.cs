using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Core.Repository.Accounting;
using EntityCache.EventArguments;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService;

public class LastPriceWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	private ILastPriceRepository CacheRepository { get; set; }

	public bool IsDisposed { get; set; }

	public event EventHandler<CountChangedEventArgs> CountChanged;

	public static event EventHandler ClientSyncDoneResponse;

	public LastPriceWebServiceRepository(ClsCache cache)
	{
		int num = 0;
		try
		{
			Cache = cache;
			num = 1;
			CacheRepository = cache.CacheRepository.LastPriceRepository;
			num = 2;
			Cache.Context.WebLastPrice.OnReadResponse += WebLastPrice_OnReadResponse;
			num = 3;
			Cache.Context.WebLastPrice.OnSync += WebLastPrice_OnSync;
			num = 4;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, string.Format("The line Number Error Is{0} And IsDispose is{1}", num, IsDisposed.ToString() ?? "Null"), ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\LastPriceWebServiceRepository.cs", 37);
		}
	}

	private async Task WebLastPrice_OnSync(object arg1, ConnectionEventArgs<WebLastPrice> e)
	{
		using (new JaegerMonitor("WebLastPrice_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\LastPriceWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					return;
				}
				line = 2;
				List<Guid> existTafsil = await TafsilBusiness.CheckExistAsync(e.Connection.Context.Guid, e.ConnectionData.Select((WebLastPrice q) => q.TafsilGuid).Distinct().ToList(), e.Connection.cancellationToken);
				if (!(existTafsil?.Any() ?? false))
				{
					return;
				}
				List<Guid> existProduct = await ProductCompactBusiness.CheckExistAsync(e.Connection.Context.Guid, e.ConnectionData.Select((WebLastPrice q) => q.PrdGuid).Distinct().ToList(), e.Connection.cancellationToken);
				if (!(existProduct?.Any() ?? false))
				{
					return;
				}
				IEnumerable<WebLastPrice> enumerable = e.ConnectionData.Where((WebLastPrice q) => existTafsil.Contains(q.TafsilGuid) && existProduct.Contains(q.PrdGuid));
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				ReturnedSaveFuncInfo res1 = await LastPriceBusiness.SaveRangeAsync(EnSystem.SignalR, cancellationToken, Priority.High, await LastPriceMapper.Instance.MapListAsync(e.Connection.Context.Guid, enumerable), sendToServer: false, flush: false, 10);
				line = 5;
				if (this.CountChanged != null)
				{
					int newCount = await CacheRepository.DbCountAsync(true, default(CancellationToken), "");
					this.CountChanged?.Invoke(this, new CountChangedEventArgs(newCount));
				}
				line = 8;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId) && !res1.HasError)
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.LastPrice, e.PartitionId, res1);
					line = 130;
					if (!(e.Connection?.cancellationToken.IsCancellationRequested ?? true))
					{
						line = 131;
						await Cache.SendData(packetData, Priority.High);
						LastPriceWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//cache is null:{Cache == null}//IsDisposed:{IsDisposed}", "WebLastPrice_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\LastPriceWebServiceRepository.cs", 74);
			}
		}
	}

	private async Task WebLastPrice_OnReadResponse(object arg1, ConnectionEventArgs<WebLastPrice> e)
	{
		using (new JaegerMonitor("WebLastPrice_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\LastPriceWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				if (e?.ConnectionData == null || e.ConnectionData.Count <= 0)
				{
					return;
				}
				line = 2;
				line = 3;
				List<WebLastPrice> connectionData = e.ConnectionData;
				line = 41;
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				if (!(await LastPriceBusiness.SaveRangeAsync(EnSystem.SignalR, cancellationToken, Priority.High, await LastPriceMapper.Instance.MapListAsync(e.Connection.Context.Guid, connectionData), Cache.Source == ENSource.Accounting, flush: false, 10)).HasError && (LastPriceWebServiceRepository.ClientSyncDoneResponse != null || this.CountChanged != null))
				{
					int count = await CacheRepository.DbCountAsync(true, default(CancellationToken), "");
					Task.Run(delegate
					{
						this.CountChanged?.Invoke(this, new CountChangedEventArgs(count));
						LastPriceWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
					});
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "WebLastPrice_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\LastPriceWebServiceRepository.cs", 104);
			}
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Cache.Context.WebLastPrice.OnReadResponse -= WebLastPrice_OnReadResponse;
				Cache.Context.WebLastPrice.OnSync -= WebLastPrice_OnSync;
				CacheRepository?.Dispose();
				CacheRepository = null;
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\LastPriceWebServiceRepository.cs", 121);
		}
	}
}
