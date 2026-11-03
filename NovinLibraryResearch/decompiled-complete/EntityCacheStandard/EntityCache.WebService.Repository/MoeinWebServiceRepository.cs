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

namespace EntityCache.WebService.Repository;

public class MoeinWebServiceRepository : WebServiceRepository<MoeinBusiness>, IDisposable
{
	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebMoein.OnReadResponse -= MoeinInstance_OnReadResponse;
				Cache.Context.WebMoein.OnSync -= MoeinInstance_OnSync;
				Cache.Context.WebMoein.OnDeleteRequest -= MoeinInstance_OnDeleteRequest;
				Cache.Context.WebMoein.OnSaveRequest -= MoeinInstance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 29);
		}
	}

	public MoeinWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<MoeinBusiness>)cache.CacheRepository.IMoein, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebMoein.OnReadResponse += MoeinInstance_OnReadResponse;
			Cache.Context.WebMoein.OnSync += MoeinInstance_OnSync;
			Cache.Context.WebMoein.OnDeleteRequest += MoeinInstance_OnDeleteRequest;
			Cache.Context.WebMoein.OnSaveRequest += MoeinInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 41);
		}
	}

	private async Task MoeinInstance_OnDeleteRequest(object sender, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("MoeinInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("MoeinInstance Ondelete received but data is null!", "", "MoeinInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 51);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("MoeinInstance Ondelete received but data is empty!", "", "MoeinInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 56);
					return;
				}
				(await MoeinBusiness.DeleteRangeAsync(e.Connection.ContextGuid, e.ConnectionData, 10)).throwExceptionIfError();
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.Moein, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 70);
			}
		}
	}

	private async Task MoeinInstance_OnSync(object sender, ConnectionEventArgs<WebMoein> e)
	{
		using (new JaegerMonitor("MoeinInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs"))
		{
			int line = 0;
			CancellationToken token = e?.Connection?.cancellationToken ?? new CancellationToken(canceled: true);
			try
			{
				line = 1;
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("MoeinInstance sync received but data is null!", "", "MoeinInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 82);
					return;
				}
				line = 3;
				List<WebMoein> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("MoeinInstance sync received but data is empty!", "", "MoeinInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 88);
					return;
				}
				line = 4;
				List<WebMoein> lstHesabsListMoein = e.ConnectionData.ToList();
				line = 5;
				bool updateAccount = Cache.Source != ENSource.Accounting;
				line = 6;
				bool sendToServer = Cache.Source == ENSource.Accounting;
				line = 7;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await MoeinBusiness.SaveRangeAsync(e.Connection.Context.Guid, EnSystem.SignalR, lstHesabsListMoein, updateAccount, sendToServer, e.Connection.cancellationToken);
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					returnedSaveFuncInfo.throwExceptionIfError();
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.Moein, e.PartitionId, returnedSaveFuncInfo);
						line = 8;
						await Cache.SendData(packetData, Priority.High);
						line = 9;
					}
				}
			}
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "MoeinInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 111);
				}
			}
		}
	}

	private async Task MoeinInstance_OnReadResponse(object sender, ConnectionEventArgs<WebMoein> e)
	{
		using (new JaegerMonitor("MoeinInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("MoeinInstance received but data is null!", "", "MoeinInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 120);
					return;
				}
				List<WebMoein> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("MoeinInstance received but data is empty!", "", "MoeinInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 125);
					return;
				}
				List<WebMoein> entityList = e.ConnectionData.ToList();
				bool updateAccount = Cache.Source != ENSource.Accounting;
				bool sendToServer = Cache.Source == ENSource.Accounting;
				await MoeinMapper.Instance.MapListAsync(e.Connection.Context.Guid, entityList);
				await MoeinBusiness.SaveRangeAsync(e.Connection.Context.Guid, EnSystem.SignalR, entityList, updateAccount, sendToServer, e.Connection.cancellationToken);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 134);
			}
		}
	}

	private async Task MoeinInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebMoein> e)
	{
		using (new JaegerMonitor("MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs"))
		{
			_ = 4;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					WebErrorLog.Instance.StartLog("فراخوانی سیو رکوئست حساب معین در محصولی بغیر از حسابداری", "", "MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 142);
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("MoeinInstance_OnSaveRequest received but data is null!", "", "MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 146);
					return;
				}
				List<WebMoein> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("MoeinInstance_OnSaveRequest received but data is empty!", "", "MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 152);
					return;
				}
				WebErrorLog.Instance.Information("MoeinInstance_OnSaveRequest received ....", "", "MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 156);
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					return;
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (WebMoein webMoein in e.ConnectionData.ToList())
				{
					bool updateAccount = Cache.Source != ENSource.Accounting;
					bool sendToServer = Cache.Source == ENSource.Accounting;
					ReturnedSaveFuncInfo ret = await (await MoeinMapper.Instance.MapAsync(base.ContextGuid, webMoein)).SaveAsync(EnSystem.SignalR, updateAccount, sendToServer, Priority.Low, null, 10, awaitOnSemaphore: true, await4Send: false, 10);
					if ((webMoein?.TafsilGroupRelationList?.Any()).GetValueOrDefault())
					{
						List<MoeinTafsilGroupRelationsBusiness> list = await MoeinTafsilGroupRelationMapper.Instance.MapListAsync(e.Connection.Context.Guid, webMoein.TafsilGroupRelationList, new List<MoeinTafsilGroupRelationsBusiness>());
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await MoeinTafsilGroupRelationsBusiness.SaveRangeAsync(list, 10), ReturnedState.None, "MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 170);
					}
					WebDelivery item = new WebDelivery
					{
						Guid = webMoein.Guid,
						Message = ret.GetErrorMessage() + ret.GetInformationMessage(),
						DeliveryStatus = !ret.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = ret.GetErrorMessage() + ret.GetInformationMessage(),
						Part = EntityPart.Moein
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
				WebErrorLog.Instance.StartLog(ex, "", "MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\MoeinWebServiceRepository.cs", 190);
			}
		}
	}
}
