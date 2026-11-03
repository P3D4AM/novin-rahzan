using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class AghsatMGroupWebServiceRepository : WebServiceRepository<AghsatM_GroupBusiness>, IDisposable
{
	private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

	public AghsatMGroupWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<AghsatM_GroupBusiness>)cache.CacheRepository.AghsatMGroupRepository, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.AghsatMGroup.OnReadResponse += AghsatAshkhasInstance_OnReadResponse;
			Cache.Context.AghsatMGroup.OnSync += AghsatAshkhasInstance_OnSync;
			Cache.Context.AghsatMGroup.OnSaveRequest += AghsatM_GroupBusiness_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs", 29);
		}
	}

	private async Task AghsatM_GroupBusiness_OnSaveRequest(object sender, ConnectionEventArgs<WebAghsatMGroup> e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("AghsatM_GroupBusiness_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					return;
				}
				List<AghsatM_GroupBusiness> list = await AghsatMGroupMapper.Instance.MapListAsync(base.ContextGuid, e.ConnectionData, new List<AghsatM_GroupBusiness>());
				List<SaveResponse> responses = new List<SaveResponse>();
				foreach (AghsatM_GroupBusiness item in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await item.SaveAsync(EnSystem.SignalR, e.Connection.cancellationToken, Priority.Low, sendToServer: false);
					responses.Add(new SaveResponse
					{
						Date = DateTime.Now,
						Guid = item.Guid,
						HostRefrenced = item.HostRefrenced,
						Message = returnedSaveFuncInfo.Messages,
						PartitionId = e.PartitionId,
						RequestPart = e.Part,
						RequestRefrencedId = e.PacketReferenceId,
						Shortmessage = returnedSaveFuncInfo.Messages,
						SaveResultStatus = !returnedSaveFuncInfo.HasError
					});
					ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "AghsatM_GroupBusiness_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs", 56);
				}
				await Cache.SendData(WebAghsatMGroup.GenerateSaveResponse(responses), Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatM_GroupBusiness_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs", 60);
			}
		}
	}

	private async Task AghsatAshkhasInstance_OnSync(object arg1, ConnectionEventArgs<WebAghsatMGroup> e)
	{
		using (new JaegerMonitor("AghsatAshkhasInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs"))
		{
			_ = 3;
			try
			{
				await semaphoreSlim.WaitAsync(e.Connection.cancellationToken);
				if (!(e.ConnectionData?.Any() ?? false))
				{
					return;
				}
				List<WebAghsatMGroup> connectionData = e.ConnectionData;
				bool send2Server = Cache.Source == ENSource.Accounting;
				List<AghsatM_GroupBusiness> lst = await AghsatMGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, connectionData, new List<AghsatM_GroupBusiness>());
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await AghsatM_GroupBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, Priority.High, lst, send2Server);
				if (!(e?.Connection?.IsDisposed).GetValueOrDefault(true) && !e.Connection.cancellationToken.IsCancellationRequested)
				{
					returnedSaveFuncInfo.throwExceptionIfError();
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.AghsatMGroup, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatAshkhasInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs", 81);
			}
		}
	}

	private async Task AghsatAshkhasInstance_OnReadResponse(object arg1, ConnectionEventArgs<WebAghsatMGroup> e)
	{
		using (new JaegerMonitor("AghsatAshkhasInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData?.Any() ?? false)
				{
					List<WebAghsatMGroup> source = e.ConnectionData.ToList();
					bool send2Server = Cache.Source == ENSource.Accounting;
					List<AghsatM_GroupBusiness> lst = await AghsatMGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, source, new List<AghsatM_GroupBusiness>());
					await AghsatM_GroupBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, Priority.High, lst, send2Server);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatAshkhasInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs", 94);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				if (Cache.Context.AghsatMGroup != null)
				{
					Cache.Context.AghsatMGroup.OnSaveRequest -= AghsatM_GroupBusiness_OnSaveRequest;
					Cache.Context.AghsatMGroup.OnReadResponse -= AghsatAshkhasInstance_OnReadResponse;
					Cache.Context.AghsatMGroup.OnSync -= AghsatAshkhasInstance_OnSync;
				}
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatMGroupWebServiceRepository.cs", 109);
		}
	}
}
