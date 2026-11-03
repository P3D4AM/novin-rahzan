using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class DescWebServiceRepository : WebServiceRepository<DescBusiness>, IDisposable
{
	public DescWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<DescBusiness>)cache.CacheRepository.IDesc, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebDesc.OnReadResponse += DescInstance_OnReadResponse;
			Cache.Context.WebDesc.OnSync += DescInstance_OnSync;
			cache.Context.Sync.OnSyncReset += SyncInstance_OnSyncReset;
			Cache.Context.WebDesc.OnSaveResponse += DescInstance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 25);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebDesc.OnReadResponse -= DescInstance_OnReadResponse;
				Cache.Context.WebDesc.OnSync -= DescInstance_OnSync;
				Cache.Context.Sync.OnSyncReset -= SyncInstance_OnSyncReset;
				Cache.Context.WebDesc.OnSaveResponse -= DescInstance_OnSaveResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 41);
		}
	}

	private async Task SyncInstance_OnSyncReset(object arg1, SyncResetEventArgs e)
	{
		using (new JaegerMonitor("SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				Task<IEnumerable<DescBusiness>> allAsync = base.CacheRepository.GetAllAsync(default(CancellationToken), "");
				HashSet<Guid> keepIds = new HashSet<Guid>(e.SyncResetItems.Select((SyncResetItem x) => x.Guid));
				IEnumerable<DescBusiness> list = (await allAsync).Where((DescBusiness x) => keepIds.Contains(x.Guid)).ToList();
				bool send2Server = Cache.Source == ENSource.Accounting;
				(await DescBusiness.SaveRangeAsync(EnSystem.SignalR, list, send2Server, Priority.High, awaitOnSemaphore: true, 10, "", e.Connection.cancellationToken)).throwExceptionIfError();
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					await Cache.SendData(Sync.GenerateSyncResetDone(e.EntityPart), Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 59);
			}
		}
	}

	private async Task DescInstance_OnSync(object sender, ConnectionEventArgs<WebDesc> e)
	{
		using (new JaegerMonitor("DescInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				if (e.ConnectionData == null)
				{
					line = 3;
					WebErrorLog.Instance.Warning("desc sync received but data is null!", "", "DescInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 70);
					return;
				}
				line = 4;
				List<WebDesc> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("desc sync received but data is empty!", "", "DescInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 76);
					return;
				}
				line = 6;
				List<WebDesc> source = e.ConnectionData.ToList();
				line = 7;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await DescBusiness.SaveRangeAsync(EnSystem.SignalR, await DescMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), send2Server: false, Priority.High, awaitOnSemaphore: true, 10, "", e.Connection.cancellationToken);
				line = 80;
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				line = 81;
				returnedSaveFuncInfo.throwExceptionIfError();
				line = 10;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 11;
					Packet packet = Sync.GenerateSyncDone(EntityPart.Desc, e.PartitionId, returnedSaveFuncInfo);
					line = 120;
					if (packet == null)
					{
						throw new NullReferenceException("pack");
					}
					line = 121;
					if (!base.IsDisposed && !(Cache?.IsDisposed ?? true) && Cache.BufferEnable)
					{
						line = 122;
						await Cache.SendData(packet, Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + $"#IsDisposed_{base.IsDisposed}", "DescInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 101);
			}
		}
	}

	private async Task DescInstance_OnReadResponse(object sender, ConnectionEventArgs<WebDesc> e)
	{
		using (new JaegerMonitor("DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Customer Title received but data is null!", "", "DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 110);
					return;
				}
				List<WebDesc> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Customer Title received but data is empty!", "", "DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 115);
					return;
				}
				List<WebDesc> source = e.ConnectionData.ToList();
				await DescBusiness.SaveRangeAsync(send2Server: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, list: await DescMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), priority: Priority.High, awaitOnSemaphore: true, tryCount: 10, tranName: "", token: e.Connection.cancellationToken);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 123);
			}
		}
	}

	private async Task DescInstance_OnSaveResponse(object arg1, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("DescInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				if (e == null || !(e.SaveResponses?.Count > 0))
				{
					return;
				}
				foreach (SaveResponse resopnse in e.SaveResponses)
				{
					line = 2;
					if (resopnse != null)
					{
						line = 3;
						DescBusiness descBusiness = await DescBusiness.GetAsync(Cache.ContextGuid, resopnse.Guid);
						line = 4;
						if (descBusiness != null)
						{
							line = 5;
							await descBusiness.SetSaveResultAsync(resopnse, e.Connection.cancellationToken, 10);
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "DescInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DescWebServiceRepository.cs", 149);
			}
		}
	}
}
