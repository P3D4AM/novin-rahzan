using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Accounting;

public class CashierLogWebServiceRepository : WebServiceRepository<CashiersLogBusiness>, IDisposable
{
	public CashierLogWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<CashiersLogBusiness>)cache.CacheRepository.CashiersLogRepository, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebUserLog.OnReadResponse += Instance_OnReadResponse;
			Cache.Context.WebUserLog.OnSync += Instance_OnSync;
			Cache.Context.WebUserLog.OnSaveRequest += Instance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 27);
		}
	}

	private async Task Instance_OnSync(object sender, ConnectionEventArgs<WebCashierLog> e)
	{
		using (new JaegerMonitor("Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Instance sync received but data is null!", "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 38);
					return;
				}
				List<WebCashierLog> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Instance sync received but data is empty!", "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 43);
					return;
				}
				List<WebCashierLog> source = e.ConnectionData.ToList();
				List<CashiersLogBusiness> list = await CashierLogMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CashiersLogBusiness.SaveRangeAsync(EnSystem.SignalR, list, sendToServer: false), ReturnedState.None, "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 49);
				if (!ret.HasError && !e.Connection.cancellationToken.IsCancellationRequested)
				{
					IEnumerable<SaveResponse> responses = e.ConnectionData.Select((WebCashierLog p) => new SaveResponse(0, p.Guid, DateTime.Now, saveResultStatus: true));
					await Cache.SendData(WebCashierLog.GenerateSaveResponse(responses), Priority.High);
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.UserLog, e.PartitionId, ret);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 61);
			}
		}
	}

	private async Task Instance_OnReadResponse(object sender, ConnectionEventArgs<WebCashierLog> e)
	{
		using (new JaegerMonitor("Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Instance received but data is null!", "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 72);
					return;
				}
				List<WebCashierLog> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Instance received but data is empty!", "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 77);
					return;
				}
				List<WebCashierLog> source = e.ConnectionData.ToList();
				List<CashiersLogBusiness> list = await CashierLogMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CashiersLogBusiness.SaveRangeAsync(EnSystem.SignalR, list, sendToServer: false), ReturnedState.None, "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 83);
				if (!ret.HasError && !e.Connection.cancellationToken.IsCancellationRequested)
				{
					IEnumerable<SaveResponse> responses = e.ConnectionData.Select((WebCashierLog p) => new SaveResponse(0, p.Guid, DateTime.Now, saveResultStatus: true));
					await Cache.SendData(WebCashierLog.GenerateSaveResponse(responses), Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 90);
			}
		}
	}

	private async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebCashierLog> e)
	{
		using (new JaegerMonitor("Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					return;
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Instance_OnSaveRequest received but data is null!", "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 101);
					return;
				}
				List<WebCashierLog> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Instance_OnSaveRequest received but data is empty!", "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 107);
					return;
				}
				WebErrorLog.Instance.Debug("Instance_OnSaveRequest received ....", "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 111);
				List<CashiersLogBusiness> list = await CashierLogMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<SaveResponse> responseList = new List<SaveResponse>();
				foreach (CashiersLogBusiness log in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await log.SaveAsync(EnSystem.SignalR, sendToServer: false, awaitOnSemaphore: true, default(CancellationToken), 10);
					SaveResponse item = new SaveResponse
					{
						Guid = log.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Date = DateTime.Now,
						HostRefrenced = log.Guid.ToString(),
						PartitionId = e.PartitionId,
						ReferencedId = 0L,
						SaveResultStatus = !returnedSaveFuncInfo.HasError,
						Shortmessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage()
					};
					responseList.Add(item);
				}
				if (responseList?.Any() ?? false)
				{
					List<Packet> lstPacket = WebCashierLog.GenerateSaveResponse(responseList);
					await Cache.SendData(lstPacket, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\CashierLogWebServiceRepository.cs", 140);
			}
		}
	}

	public override void Dispose(bool disposing)
	{
		Cache.Context.WebUserLog.OnReadResponse -= Instance_OnReadResponse;
		Cache.Context.WebUserLog.OnSync -= Instance_OnSync;
		Cache.Context.WebUserLog.OnSaveRequest -= Instance_OnSaveRequest;
		base.Dispose(disposing);
	}
}
