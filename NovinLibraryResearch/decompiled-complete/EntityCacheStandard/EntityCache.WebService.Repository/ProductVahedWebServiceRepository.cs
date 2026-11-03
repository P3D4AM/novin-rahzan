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

public class ProductVahedWebServiceRepository : WebServiceRepository<PrdVahedBusiness>
{
	public static event EventHandler ClientSyncDoneResponse;

	public ProductVahedWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<PrdVahedBusiness>)cache.CacheRepository.IProductVahed, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebPrdVahed.OnReadResponse += PrdVahedInstance_OnReadResponse;
			Cache.Context.WebPrdVahed.OnSync += PrdVahedInstance_OnSync;
			Cache.Context.WebPrdVahed.OnSaveRequest += PrdVahedInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 28);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebPrdVahed.OnReadResponse -= PrdVahedInstance_OnReadResponse;
				Cache.Context.WebPrdVahed.OnSync -= PrdVahedInstance_OnSync;
				Cache.Context.WebPrdVahed.OnSaveRequest -= PrdVahedInstance_OnSaveRequest;
				ProductVahedWebServiceRepository.ClientSyncDoneResponse = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 43);
		}
	}

	private async Task PrdVahedInstance_OnSync(object sender, ConnectionEventArgs<WebPrdVahed> e)
	{
		using (new JaegerMonitor("PrdVahedInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				CancellationToken token = e.Connection.cancellationToken;
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Product Vahed sync received but data is null!", "", "PrdVahedInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 54);
					return;
				}
				List<WebPrdVahed> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Product Vahed sync received but data is empty!", "", "PrdVahedInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 59);
					return;
				}
				List<WebPrdVahed> source = e.ConnectionData.ToList();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await PrdVahedBusiness.SaveRangeAsync(send2Server: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, lst: await PrdVahedMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), autoFixValidationCount: 10, priority: Priority.High, tryCount: 10, tranName: "", token: e.Connection.cancellationToken);
				if (!token.IsCancellationRequested)
				{
					returnedSaveFuncInfo.throwExceptionIfError();
					if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.ProductVahed, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
						ProductVahedWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PrdVahedInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 81);
			}
		}
	}

	private async Task PrdVahedInstance_OnReadResponse(object sender, ConnectionEventArgs<WebPrdVahed> e)
	{
		using (new JaegerMonitor("PrdVahedInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Product Vahed received but data is null!", "", "PrdVahedInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 91);
					return;
				}
				List<WebPrdVahed> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Product Vahed received but data is empty!", "", "PrdVahedInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 96);
					return;
				}
				List<WebPrdVahed> source = e.ConnectionData.ToList();
				await PrdVahedBusiness.SaveRangeAsync(send2Server: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, lst: await PrdVahedMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), autoFixValidationCount: 10, priority: Priority.Low, tryCount: 10, tranName: "", token: e.Connection.cancellationToken);
				ProductVahedWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PrdVahedInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 107);
			}
		}
	}

	private async Task PrdVahedInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebPrdVahed> e)
	{
		using (new JaegerMonitor("PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					WebErrorLog.Instance.StartLog("فراخوانی سیو رکوئست واحد کالا در محصولی بغیر از حسابداری", "", "PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 116);
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("PrdVahedInstance_OnSaveRequest received but data is null!", "", "PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 119);
					return;
				}
				List<WebPrdVahed> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("PrdVahedInstance_OnSaveRequest received but data is empty!", "", "PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 125);
					return;
				}
				WebErrorLog.Instance.Debug("PrdVahedInstance_OnSaveRequest received ....", "", "PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 129);
				List<PrdVahedBusiness> list = await PrdVahedMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (PrdVahedBusiness webPrdVahed in list)
				{
					bool send2Server = Cache.Source == ENSource.Accounting;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await webPrdVahed.SaveAsync(EnSystem.SignalR, 10, awaitOnSemaphore: true, send2Server, Priority.High, await4Send: false, e.Connection.cancellationToken, 10);
					WebDelivery item = new WebDelivery
					{
						Guid = webPrdVahed.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.ProductVahed
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
				WebErrorLog.Instance.StartLog(ex, "", "PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductVahedWebServiceRepository.cs", 157);
			}
		}
	}
}
