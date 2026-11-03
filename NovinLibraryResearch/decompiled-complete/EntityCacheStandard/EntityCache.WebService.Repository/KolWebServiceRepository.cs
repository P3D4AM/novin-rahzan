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

public class KolWebServiceRepository : WebServiceRepository<KolBusiness>, IDisposable
{
	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebKol.OnReadResponse -= KolInstance_OnReadResponse;
				Cache.Context.WebKol.OnSync -= KolInstance_OnSync;
				Cache.Context.WebKol.OnDeleteRequest -= KolInstance_OnDeleteRequest;
				Cache.Context.WebKol.OnSaveRequest -= KolInstance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 28);
		}
	}

	public KolWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<KolBusiness>)cache.CacheRepository.IKol, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebKol.OnReadResponse += KolInstance_OnReadResponse;
			Cache.Context.WebKol.OnSync += KolInstance_OnSync;
			Cache.Context.WebKol.OnDeleteRequest += KolInstance_OnDeleteRequest;
			Cache.Context.WebKol.OnSaveRequest += KolInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 41);
		}
	}

	private async Task KolInstance_OnDeleteRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("KolInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("KolInstance Ondelete received but data is null!", "", "KolInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 51);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("KolInstance Ondelete received but data is empty!", "", "KolInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 56);
					return;
				}
				(await KolBusiness.DeleteRangeAsync(e.Connection.ContextGuid, e.ConnectionData, 10)).throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.Kol, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KolInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 70);
			}
		}
	}

	private async Task KolInstance_OnSync(object sender, ConnectionEventArgs<WebKol> e)
	{
		using (new JaegerMonitor("KolInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("KolInstance sync received but data is null!", "", "KolInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 82);
					return;
				}
				List<WebKol> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("KolInstance sync received but data is empty!", "", "KolInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 87);
					return;
				}
				line = 2;
				line = 3;
				List<WebKol> list = e.ConnectionData.ToList();
				line = 4;
				if (Cache.Source == ENSource.Android_HesabYar)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await KolBusiness.SaveRangeForHesabyarAsync(e.Connection.ContextGuid, list, e.Connection.cancellationToken), ReturnedState.None, "KolInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 96);
				}
				else
				{
					_ = Cache.Source;
					line = 5;
					bool sendToServer = Cache.Source == ENSource.Accounting;
					line = 6;
					List<KolBusiness> lstHesabsListKol = await KolMapper.Instance.MapListAsync(e.Connection.Context.Guid, list);
					line = 7;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await KolBusiness.SaveRangeAsync(EnSystem.SignalR, lstHesabsListKol, sendToServer, Priority.High, updateAccount: true, 10, awaitOnSemaphore: true, e.Connection.cancellationToken, wait4Send: false, 10), ReturnedState.None, "KolInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 105);
				}
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					line = 8;
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.Kol, e.PartitionId, ret);
						line = 9;
						await Cache.SendData(packetData, Priority.High);
					}
					ret.throwExceptionIfError();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The Line Number oF Error  Has : {line}", "KolInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 118);
			}
		}
	}

	private async Task KolInstance_OnReadResponse(object sender, ConnectionEventArgs<WebKol> e)
	{
		using (new JaegerMonitor("KolInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("KolInstance received but data is null!", "", "KolInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 128);
					return;
				}
				List<WebKol> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("KolInstance received but data is empty!", "", "KolInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 133);
					return;
				}
				List<WebKol> source = e.ConnectionData.ToList();
				await KolBusiness.SaveRangeAsync(updateAccount: Cache.Source != ENSource.Accounting, sendToServer: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, lstHesabsListKol: await KolMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), priority: Priority.Medium, autoFixValidationErrorsCount: 10, awaitOnSemaphore: true, token: e.Connection.cancellationToken, wait4Send: false, tryCount: 10);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KolInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 144);
			}
		}
	}

	private async Task KolInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebKol> e)
	{
		using (new JaegerMonitor("KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					WebErrorLog.Instance.StartLog("فراخوانی سیو رکوئست حساب کل در محصولی بغیر از حسابداری", "", "KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 153);
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("KolInstance_OnSaveRequest received but data is null!", "", "KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 156);
					return;
				}
				List<WebKol> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("KolInstance_OnSaveRequest received but data is empty!", "", "KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 162);
					return;
				}
				WebErrorLog.Instance.Debug("KolInstance_OnSaveRequest received ....", "", "KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 166);
				List<KolBusiness> list = await KolMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (KolBusiness webKol in list)
				{
					bool updateAccount = Cache.Source != ENSource.Accounting;
					bool sendToServer = Cache.Source == ENSource.Accounting;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await webKol.SaveAsync(EnSystem.SignalR, awaitOnSemaphore: true, updateAccount, 10, e.Connection.cancellationToken, Priority.High, sendToServer, await4send: false, 10);
					WebDelivery item = new WebDelivery
					{
						Guid = webKol.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.Kol
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
				WebErrorLog.Instance.StartLog(ex, "", "KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\KolWebServiceRepository.cs", 196);
			}
		}
	}
}
