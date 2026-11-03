using System;
using System.Collections.Generic;
using System.Linq;
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

public class ProductGroupWebServiceRepository : WebServiceRepository<PrdGroupBusiness>
{
	public static event EventHandler ClientSyncDoneResponse;

	public ProductGroupWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<PrdGroupBusiness>)cache.CacheRepository.IProductGroup, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebPrdGroup.OnReadResponse += PrdGroupInstance_OnReadResponse;
			Cache.Context.WebPrdGroup.OnSync += PrdGroupInstance_OnSync;
			Cache.Context.WebPrdGroup.OnDeleteRequest += PrdGroupInstance_OnDeleteRequest;
			Cache.Context.WebPrdGroup.OnSaveRequest += PrdGroupInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 29);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebPrdGroup.OnReadResponse -= PrdGroupInstance_OnReadResponse;
				Cache.Context.WebPrdGroup.OnSync -= PrdGroupInstance_OnSync;
				Cache.Context.WebPrdGroup.OnDeleteRequest -= PrdGroupInstance_OnDeleteRequest;
				Cache.Context.WebPrdGroup.OnSaveRequest -= PrdGroupInstance_OnSaveRequest;
				ProductGroupWebServiceRepository.ClientSyncDoneResponse = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 47);
		}
	}

	private async Task PrdGroupInstance_OnDeleteRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("PrdGroupInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs"))
		{
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("PrdGroup Ondelete received but data is null!", "", "PrdGroupInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 57);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("PrdGroup Ondelete received but data is empty!", "", "PrdGroupInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 62);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PrdGroupInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 66);
			}
		}
	}

	private async Task PrdGroupInstance_OnSync(object sender, ConnectionEventArgs<WebPrdGroup> e)
	{
		using (new JaegerMonitor("PrdGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				line = 2;
				if (e.ConnectionData == null)
				{
					line = 3;
					WebErrorLog.Instance.Warning("Product group sync received but data is null!", "", "PrdGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 111);
					return;
				}
				line = 5;
				List<WebPrdGroup> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					line = 6;
					WebErrorLog.Instance.Warning("Product group sync received but data is empty!", "", "PrdGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 118);
					return;
				}
				line = 7;
				List<WebPrdGroup> source = e.ConnectionData.ToList();
				List<PrdGroupBusiness> lst = await PrdGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				line = 10;
				bool send2Server = Cache.Source == ENSource.Accounting;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await PrdGroupBusiness.SaveRangeAsync(base.ContextGuid, EnSystem.SignalR, lst, 10, send2Server, Priority.High, 10, "", e.Connection.cancellationToken);
				line = 15;
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				returnedSaveFuncInfo.throwExceptionIfError();
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					line = 20;
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						line = 21;
						Packet packetData = Sync.GenerateSyncDone(EntityPart.ProductGroup, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
						line = 30;
						ProductGroupWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
						line = 40;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "PrdGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 146);
			}
		}
	}

	private async Task PrdGroupInstance_OnReadResponse(object sender, ConnectionEventArgs<WebPrdGroup> e)
	{
		using (new JaegerMonitor("PrdGroupInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Product group received but data is null!", "", "PrdGroupInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 156);
					return;
				}
				List<WebPrdGroup> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Product group received but data is empty!", "", "PrdGroupInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 161);
					return;
				}
				List<WebPrdGroup> source = e.ConnectionData.ToList();
				bool send2Server = Cache.Source == ENSource.Accounting;
				List<PrdGroupBusiness> lst = await PrdGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				await PrdGroupBusiness.SaveRangeAsync(base.ContextGuid, EnSystem.SignalR, lst, 10, send2Server, Priority.High, 10, "", e.Connection.cancellationToken);
				ProductGroupWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PrdGroupInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 172);
			}
		}
	}

	private async Task PrdGroupInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebPrdGroup> e)
	{
		using (new JaegerMonitor("PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					WebErrorLog.Instance.StartLog("فراخوانی سیو رکوئست گروه کالا در محصولی بغیر از حسابداری", "", "PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 181);
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("PrdGroupInstance_OnSaveRequest received but data is null!", "", "PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 184);
					return;
				}
				List<WebPrdGroup> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("PrdGroupInstance_OnSaveRequest received but data is empty!", "", "PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 190);
					return;
				}
				WebErrorLog.Instance.Debug("PrdGroupInstance_OnSaveRequest received ....", "", "PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 194);
				List<PrdGroupBusiness> list = await PrdGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				bool send2Server = Cache.Source == ENSource.Accounting;
				foreach (PrdGroupBusiness webPrdGroup in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await webPrdGroup.SaveAsync(EnSystem.SignalR, e.Connection.cancellationToken, 10, send2Server, Priority.High, awaitOnSemaphore: true, 10);
					WebDelivery item = new WebDelivery
					{
						Guid = webPrdGroup.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.ProductGroup
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
				WebErrorLog.Instance.StartLog(ex, "", "PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductGroupWebServiceRepository.cs", 227);
			}
		}
	}
}
