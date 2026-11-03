using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Business.Accounting.Note;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class AghsatAshkhasWebServiceRepository : WebServiceRepository<AghsatMBusiness>, IDisposable
{
	public static event EventHandler ClientSyncDoneResponse;

	public AghsatAshkhasWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<AghsatMBusiness>)cache.CacheRepository.AghsatMRepository, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebAghsatAshkhas.OnReadResponse += AghsatAshkhasInstance_OnReadResponse;
			Cache.Context.WebAghsatAshkhas.OnSync += AghsatAshkhasInstance_OnSync;
			Cache.Context.WebAghsatAshkhas.OnDeletedRequest += AghsatAshkhasInstance_OnDeletedRequest;
			Cache.Context.WebAghsatAshkhas.OnSaveRequest += WebAghsatAshkhas_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 31);
		}
	}

	private async Task WebAghsatAshkhas_OnSaveRequest(object sender, ConnectionEventArgs<WebAghsatAshkhas> e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("WebAghsatAshkhas_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs"))
		{
			_ = 4;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					return;
				}
				List<AghsatMBusiness> list = await AghsatMMapper.Instance.MapListAsync(base.ContextGuid, e.ConnectionData, new List<AghsatMBusiness>());
				List<SaveResponse> responses = new List<SaveResponse>();
				foreach (AghsatMBusiness item in list)
				{
					ReturnedSaveFuncInfo saveResult = new ReturnedSaveFuncInfo();
					if (await NoteGroupBusiness.GetAsync(base.ContextGuid, item.GuidGroup, e.Connection.cancellationToken) == null)
					{
						await saveResult.AddErrorAsync("Group is Null", "", "WebAghsatAshkhas_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 47);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = saveResult;
						returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(EnSystem.SignalR, e.Connection.cancellationToken, Priority.Low, sendToServer: false, awaitOnSemaphore: true, await4Send: false, 10), ReturnedState.None, "WebAghsatAshkhas_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 48);
					}
					responses.Add(new SaveResponse
					{
						Date = DateTime.Now,
						Guid = item.Guid,
						HostRefrenced = item.HostRefrenced,
						Message = saveResult.Messages,
						PartitionId = e.PartitionId,
						ReferencedId = (item.HostRefrenced ?? "").ParseToLong(0L),
						RequestPart = e.Part,
						RequestRefrencedId = e.PacketReferenceId,
						Shortmessage = saveResult.Messages,
						SaveResultStatus = !saveResult.HasError
					});
					ret.AddReturnedValue(saveResult, ReturnedState.None, "WebAghsatAshkhas_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 62);
				}
				await Cache.SendData(WebAghsatAshkhas.GenerateSaveResponse(responses), Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebAghsatAshkhas_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 66);
			}
		}
	}

	private async Task AghsatAshkhasInstance_OnDeletedRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("AghsatAshkhasInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("AghsatAshkhasInstance_OnDeletedRequest received but data is null!", "", "AghsatAshkhasInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 75);
					return;
				}
				(await AghsatMBusiness.DeleteRangeAsync(Cache.ContextGuid, e.ConnectionData)).throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.AghsatAshkhas, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatAshkhasInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 86);
			}
		}
	}

	private async Task AghsatAshkhasInstance_OnSync(object arg1, ConnectionEventArgs<WebAghsatAshkhas> e)
	{
		using (new JaegerMonitor("AghsatAshkhasInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("AghsatAshkhas sync received but data is null!", "", "AghsatAshkhasInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 95);
					return;
				}
				List<WebAghsatAshkhas> connectionData = e.ConnectionData;
				bool send2Server = Cache.Source == ENSource.Accounting;
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await AghsatMBusiness.SaveRangeAsync(EnSystem.SignalR, cancellationToken, Priority.High, await AghsatMMapper.Instance.MapListAsync(e.Connection.Context.Guid, connectionData, new List<AghsatMBusiness>()), send2Server, awit4Send: false, 10);
				if (!(e?.Connection?.IsDisposed).GetValueOrDefault(true) && !e.Connection.cancellationToken.IsCancellationRequested)
				{
					returnedSaveFuncInfo.throwExceptionIfError();
					if (!(e?.Connection?.IsDisposed).GetValueOrDefault(true) && !e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.AghsatAshkhas, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
						AghsatAshkhasWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "اطلاعات قسط : \r\n" + Json.ToStringJson(e.ConnectionData) + "\r\n", "AghsatAshkhasInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 113);
			}
		}
	}

	private async Task AghsatAshkhasInstance_OnReadResponse(object arg1, ConnectionEventArgs<WebAghsatAshkhas> e)
	{
		using (new JaegerMonitor("AghsatAshkhasInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("AghsatAshkhas received but data is null!", "", "AghsatAshkhasInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 124);
					return;
				}
				List<WebAghsatAshkhas> source = e.ConnectionData.ToList();
				bool send2Server = Cache.Source == ENSource.Accounting;
				CancellationToken cancellationToken = e.Connection.cancellationToken;
				await AghsatMBusiness.SaveRangeAsync(EnSystem.SignalR, cancellationToken, Priority.High, await AghsatMMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), send2Server, awit4Send: false, 10);
				AghsatAshkhasWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatAshkhasInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 132);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				if (Cache.Context.WebAghsatAshkhas != null)
				{
					Cache.Context.WebAghsatAshkhas.OnSaveRequest -= WebAghsatAshkhas_OnSaveRequest;
					Cache.Context.WebAghsatAshkhas.OnReadResponse -= AghsatAshkhasInstance_OnReadResponse;
					Cache.Context.WebAghsatAshkhas.OnSync -= AghsatAshkhasInstance_OnSync;
					Cache.Context.WebAghsatAshkhas.OnDeletedRequest -= AghsatAshkhasInstance_OnDeletedRequest;
				}
				AghsatAshkhasWebServiceRepository.ClientSyncDoneResponse = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\AghsatAshkhasWebServiceRepository.cs", 149);
		}
	}
}
