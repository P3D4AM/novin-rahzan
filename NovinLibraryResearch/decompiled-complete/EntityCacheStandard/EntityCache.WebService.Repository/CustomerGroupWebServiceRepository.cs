using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Customer;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class CustomerGroupWebServiceRepository : WebServiceRepository<CusGroupBusiness>, IDisposable
{
	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebCusGroup.OnReadResponse -= CusGroupInstance_OnReadResponse;
				Cache.Context.WebCusGroup.OnSync -= CusGroupInstance_OnSync;
				Cache.Context.WebCusGroup.OnSaveRequest -= CusGroupInstance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 30);
		}
	}

	public CustomerGroupWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<CusGroupBusiness>)cache.CacheRepository.ICustomerGroup, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebCusGroup.OnReadResponse += CusGroupInstance_OnReadResponse;
			Cache.Context.WebCusGroup.OnSync += CusGroupInstance_OnSync;
			Cache.Context.WebCusGroup.OnSaveRequest += CusGroupInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 45);
		}
	}

	private async Task CusGroupInstance_OnSync(object sender, ConnectionEventArgs<WebCusGroup> e)
	{
		using (new JaegerMonitor("CusGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				line = 5;
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("CustomerGroup sync received but data is null!", "", "CusGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 98);
					return;
				}
				List<WebCusGroup> list = e.ConnectionData.ToList();
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				if (Cache.Source == ENSource.Android_HesabYar)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await CusGroupBusiness.SaveRangeForHesabyarAsync(base.ContextGuid, list, e.Connection.cancellationToken), ReturnedState.None, "CusGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 104);
				}
				else
				{
					bool send2Server = Cache.Source == ENSource.Accounting;
					List<CusGroupBusiness> lst = await CusGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, list);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await CusGroupBusiness.SaveRangeAsync(EnSystem.SignalR, lst, 10, send2Server, Priority.High, 10, "", e.Connection.cancellationToken), ReturnedState.None, "CusGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 109);
				}
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				res.throwExceptionIfError();
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					line = 15;
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						line = 16;
						Packet packetData = Sync.GenerateSyncDone(EntityPart.CustomerGroup, e.PartitionId, res);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "CusGroupInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 122);
			}
		}
	}

	private async Task CusGroupInstance_OnReadResponse(object sender, ConnectionEventArgs<WebCusGroup> e)
	{
		using (new JaegerMonitor("CusGroupInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.Connection.Source == ENSource.Accounting)
				{
					throw new Exception("چرا سرور داره این رویداد رو برای حسابداری میفرسته ؟");
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("CustomerGroup received but data is null!", "", "CusGroupInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 134);
					return;
				}
				List<WebCusGroup> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("CustomerGroup received but data is empty!", "", "CusGroupInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 139);
					return;
				}
				List<WebCusGroup> connectionData2 = e.ConnectionData;
				await CusGroupBusiness.SaveRangeAsync(send2Server: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, lst: await CusGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, connectionData2), autoFixValidationErrorsCount: 10, priority: Priority.High, tryCount: 10);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CusGroupInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 149);
			}
		}
	}

	private async Task CusGroupInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCusGroup> e)
	{
		using (new JaegerMonitor("CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					WebErrorLog.Instance.StartLog("فراخوانی سیو رکوئست گروه اشخاص در محصولی بغیر از حسابداری", "", "CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 158);
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("CusGroupInstance_OnSaveRequest received but data is null!", "", "CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 161);
					return;
				}
				List<WebCusGroup> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("CusGroupInstance_OnSaveRequest received but data is empty!", "", "CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 167);
					return;
				}
				WebErrorLog.Instance.Debug("CusGroupInstance_OnSaveRequest received ....", "", "CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 171);
				List<CusGroupBusiness> list = await CusGroupMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (CusGroupBusiness webCusGroup in list)
				{
					bool send2Server = Cache.Source == ENSource.Accounting;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await webCusGroup.SaveAsync(EnSystem.SignalR, send2Server, 10, Priority.High, awaitOnSemaphore: true, await4Send: false, 10, "", e.Connection.cancellationToken);
					WebDelivery item = new WebDelivery
					{
						Guid = webCusGroup.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.CustomerGroup
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
				WebErrorLog.Instance.StartLog(ex, "", "CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerGroupWebServiceRepository.cs", 200);
			}
		}
	}
}
