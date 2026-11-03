using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tax;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting.Tax;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Accounting;

public class TaxWebServiceRepository : WebServiceRepository<TaxBusiness>, IDisposable
{
	public TaxWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<TaxBusiness>)cache.CacheRepository.Tax, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.Tax.OnReadResponse += Instance_OnReadResponse;
			Cache.Context.Tax.OnSync += Instance_OnSync;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TaxWebServiceRepository.cs", 23);
		}
	}

	private async Task Instance_OnSync(object sender, ConnectionEventArgs<WebTax> e)
	{
		using (new JaegerMonitor("Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TaxWebServiceRepository.cs"))
		{
			CancellationToken? cancellationToken = e?.Connection?.cancellationToken;
			if (!cancellationToken.HasValue)
			{
				new CancellationToken(canceled: true);
			}
			else
			{
				cancellationToken.GetValueOrDefault();
			}
			try
			{
				if (e.ConnectionData != null && Cache.Source != ENSource.Accounting && (e.ConnectionData?.Any() ?? false))
				{
					List<WebTax> source = e.ConnectionData.ToList();
					List<TaxBusiness> lst = await TaxMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await TaxBusiness.SaveRangeAsync(EnSystem.SignalR, sendToServer: false, e.Connection.cancellationToken, lst);
					returnedSaveFuncInfo.throwExceptionIfError();
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.Tax, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TaxWebServiceRepository.cs", 44);
			}
		}
	}

	private async Task Instance_OnReadResponse(object sender, ConnectionEventArgs<WebTax> e)
	{
		using (new JaegerMonitor("Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TaxWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData != null && Cache.Source != ENSource.Accounting)
				{
					List<WebTax> connectionData = e.ConnectionData;
					if (connectionData == null || connectionData.Any())
					{
						List<WebTax> source = e.ConnectionData.ToList();
						List<TaxBusiness> lst = await TaxMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
						await TaxBusiness.SaveRangeAsync(EnSystem.SignalR, sendToServer: false, e.Connection.cancellationToken, lst);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TaxWebServiceRepository.cs", 57);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.Tax.OnReadResponse -= Instance_OnReadResponse;
				Cache.Context.Tax.OnSync -= Instance_OnSync;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TaxWebServiceRepository.cs", 69);
		}
	}
}
