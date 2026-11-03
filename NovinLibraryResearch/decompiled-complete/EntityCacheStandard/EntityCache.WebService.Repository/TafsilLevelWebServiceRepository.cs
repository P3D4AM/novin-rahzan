using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class TafsilLevelWebServiceRepository : WebServiceRepository<TafsilGroupBusiness>, IDisposable
{
	private new ClsCache Cache;

	public static event EventHandler ClientSyncDoneResponse;

	public TafsilLevelWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<TafsilGroupBusiness>)cache.CacheRepository.ITafsilGroup, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebTafsilGroup.OnReadResponse += TafsilLevelInstance_OnReadResponse;
			Cache.Context.WebTafsilGroup.OnSync += TafsilLevelInstance_OnSync;
			Cache.Context.WebTafsilGroup.OnSaveRequest += TafsilLevelInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 30);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (base.IsDisposed)
			{
				Cache.Context.WebTafsilGroup.OnReadResponse -= TafsilLevelInstance_OnReadResponse;
			}
			Cache.Context.WebTafsilGroup.OnSync -= TafsilLevelInstance_OnSync;
			Cache.Context.WebTafsilGroup.OnSaveRequest -= TafsilLevelInstance_OnSaveRequest;
			TafsilLevelWebServiceRepository.ClientSyncDoneResponse = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 46);
		}
	}

	private async Task TafsilLevelInstance_OnSync(object sender, ConnectionEventArgs<WebTafsilGroup> e)
	{
		using (new JaegerMonitor("TafsilLevelInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs"))
		{
			_ = 3;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("TafsilLevel sync received but data is null!", "", "TafsilLevelInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 57);
					return;
				}
				ENSource source = ClsCache.GetCache(e.Connection.ContextGuid).Source;
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				if (source == ENSource.Android_HesabYar)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await TafsilGroupBusiness.SaveRangeForHesabyarAsync(base.ContextGuid, e.ConnectionData, e.Connection.cancellationToken), ReturnedState.None, "TafsilLevelInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 64);
				}
				else
				{
					bool isRemoveDataWithGuid = ClsCache.GetCache(e.Connection.ContextGuid).Source != ENSource.Accounting;
					bool sendToServer = ClsCache.GetCache(e.Connection.ContextGuid).Source == ENSource.Accounting;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await TafsilGroupBusiness.SaveRangeAsync(EnSystem.SignalR, await TafsilGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData), sendToServer, Priority.High, isRemoveDataWithGuid, awaitOnSend: false, e.Connection.cancellationToken, 10), ReturnedState.None, "TafsilLevelInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 70);
				}
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					res.throwExceptionIfError();
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.TafsilGroup, e.PartitionId, res);
						await Cache.SendData(packetData, Priority.High);
						TafsilLevelWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TafsilLevelInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 84);
			}
		}
	}

	private async Task TafsilLevelInstance_OnReadResponse(object sender, ConnectionEventArgs<WebTafsilGroup> e)
	{
		using (new JaegerMonitor("TafsilLevelInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("TafsilLevel received but data is null!", "", "TafsilLevelInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 94);
					return;
				}
				List<WebTafsilGroup> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("TafsilLevel received but data is empty!", "", "TafsilLevelInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 99);
					return;
				}
				List<WebTafsilGroup> source = e.ConnectionData.ToList();
				await TafsilGroupBusiness.SaveRangeAsync(isRemoveWithGuid: Cache.Source != ENSource.Accounting, sendToServer: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, lstHesabsList: await TafsilGroupMapper.Instance.MapListAsync(e.Connection.ContextGuid, source), priority: Priority.Low, awaitOnSend: false, token: e.Connection.cancellationToken, tryCount: 10);
				TafsilLevelWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TafsilLevelInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 110);
			}
		}
	}

	private async Task TafsilLevelInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebTafsilGroup> e)
	{
		using (new JaegerMonitor("TafsilLevelInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					WebErrorLog.Instance.StartLog("فراخوانی سیو رکوئست  گروه تفصیل در محصولی بغیر از حسابداری", "", "TafsilLevelInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 119);
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("TafsilLevelInstance_OnSaveRequest received but data is null!", "", "TafsilLevelInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 122);
					return;
				}
				List<WebTafsilGroup> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("TafsilLevelInstance_OnSaveRequest received but data is empty!", "", "TafsilLevelInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 128);
					return;
				}
				List<TafsilGroupBusiness> list = await TafsilGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (TafsilGroupBusiness webTafsilLevel in list)
				{
					bool send2Server = Cache.Source == ENSource.Accounting;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await webTafsilLevel.SaveAsync(EnSystem.SignalR, send2Server, null, Priority.High, awaitOnSemaphore: true, await4send: false, isRemoveWithGuid: false, 10, "", e.Connection.cancellationToken, 0);
					WebDelivery item = new WebDelivery
					{
						Guid = webTafsilLevel.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.TafsilGroup
					};
					deliveryList.Add(item);
				}
				if (deliveryList != null && deliveryList.Any())
				{
					List<Packet> lstPacket = WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId);
					await Cache.SendData(lstPacket, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TafsilLevelInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\TafsilLevelWebServiceRepository.cs", 165);
			}
		}
	}
}
