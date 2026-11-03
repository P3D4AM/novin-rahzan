using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business.Crm;
using EntityCache.Core;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class CallingLogWebServiceRepository : WebServiceRepository<CallingLogBusiness>, IDisposable
{
	public static event EventHandler ClientSyncDoneResponse;

	public CallingLogWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<CallingLogBusiness>)cache.CacheRepository.ICallingLog, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebCallingLog.OnReadResponse += CallingLogInstance_OnReadResponse;
			Cache.Context.WebCallingLog.OnSync += CallingLogInstance_OnSync;
			Cache.Context.WebCallingLog.OnSaveResponse += CallingLogInstance_OnSaveResponse;
			Cache.Context.WebCallingLog.OnDeletedRequest += CallingLogInstance_OnDeletedRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 25);
		}
	}

	public override void Dispose()
	{
		try
		{
			Cache.Context.WebCallingLog.OnReadResponse -= CallingLogInstance_OnReadResponse;
			Cache.Context.WebCallingLog.OnSync -= CallingLogInstance_OnSync;
			Cache.Context.WebCallingLog.OnSaveResponse -= CallingLogInstance_OnSaveResponse;
			Cache.Context.WebCallingLog.OnDeletedRequest -= CallingLogInstance_OnDeletedRequest;
			CallingLogWebServiceRepository.ClientSyncDoneResponse = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 39);
		}
	}

	private async Task CallingLogInstance_OnDeletedRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("CallingLogInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("CallingLogInstance_OnDeletedRequest received but data is null!", "", "CallingLogInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 48);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("CallingLogInstance_OnDeletedRequest received but data is empty!", "", "CallingLogInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 53);
					return;
				}
				(await CallingLogBusiness.DeleteRangeAsync(Cache.ContextGuid, e.ConnectionData)).throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.CallingLog, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CallingLogInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 66);
			}
		}
	}

	private async Task CallingLogInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("CallingLogInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs"))
		{
			try
			{
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "CallingLogInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 92);
			}
		}
	}

	private async Task CallingLogInstance_OnSync(object arg1, ConnectionEventArgs<WebCallingLog> e)
	{
		using (new JaegerMonitor("CallingLogInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("CallingLog sync received but data is null!", "", "CallingLogInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 102);
					return;
				}
				List<WebCallingLog> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("CallingLog sync received but data is empty!", "", "CallingLogInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 107);
					return;
				}
				List<WebCallingLog> connectionData2 = e.ConnectionData;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await CallingLogBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.Context.Guid, connectionData2, isFromSync: true);
				returnedSaveFuncInfo.throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.CallingLog, e.PartitionId, returnedSaveFuncInfo);
					await Cache.SendData(packetData, Priority.High);
					CallingLogWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CallingLogInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 125);
			}
		}
	}

	private async Task CallingLogInstance_OnReadResponse(object sender, ConnectionEventArgs<WebCallingLog> e)
	{
		int line = 0;
		using (new JaegerMonitor("CallingLogInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs"))
		{
			try
			{
				line = 2;
				if (e.ConnectionData == null)
				{
					line = 3;
					WebErrorLog.Instance.Warning("CallingLog received but data is null!", "", "CallingLogInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 138);
					return;
				}
				line = 4;
				List<WebCallingLog> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					line = 5;
					WebErrorLog.Instance.Warning("CallingLog received but data is empty!", "", "CallingLogInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 145);
					return;
				}
				line = 6;
				List<WebCallingLog> lst = e.ConnectionData.ToList();
				line = 7;
				await CallingLogBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.Context.Guid, lst, isFromSync: false);
				line = 8;
				CallingLogWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
				line = 10;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "CallingLogInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogWebServiceRepository.cs", 158);
			}
		}
	}
}
