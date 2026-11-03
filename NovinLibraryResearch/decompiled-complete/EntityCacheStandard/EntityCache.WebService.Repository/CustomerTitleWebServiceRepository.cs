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

public class CustomerTitleWebServiceRepository : WebServiceRepository<CusTitleBusiness>
{
	public CustomerTitleWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<CusTitleBusiness>)cache.CacheRepository.ICustomerTitle, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebCusTitle.OnReadResponse += CusTitleInstance_OnReadResponse;
			Cache.Context.WebCusTitle.OnSync += CusTitleInstance_OnSync;
			Cache.Context.WebCusTitle.OnSaveRequest += CusTitleInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 26);
		}
	}

	private async Task CusTitleInstance_OnSync(object sender, ConnectionEventArgs<WebCusTitle> e)
	{
		using (new JaegerMonitor("CusTitleInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					WebErrorLog.Instance.Warning("Customer Title sync received but data is empty!", "", "CusTitleInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 36);
					return;
				}
				List<WebCusTitle> source = e.ConnectionData.ToList();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await CusTitleBusiness.SaveRangeAsync(send2Server: Cache.Source == ENSource.Accounting, system: EnSystem.SignalR, lst: await CusTitleMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), autoFixValidationCount: 10, priority: Priority.High, awit4Send: false, tryCount: 10, tranName: "", token: e.Connection.cancellationToken);
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					returnedSaveFuncInfo.throwExceptionIfError();
					if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.CustomerTitle, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "CusTitleInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 53);
			}
		}
	}

	private async Task CusTitleInstance_OnReadResponse(object sender, ConnectionEventArgs<WebCusTitle> e)
	{
		using (new JaegerMonitor("CusTitleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					WebErrorLog.Instance.Warning("Customer Title received but data is empty!", "", "CusTitleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 62);
					return;
				}
				List<WebCusTitle> source = e.ConnectionData.ToList();
				List<CusTitleBusiness> lst = await CusTitleMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				bool send2Server = Cache.Source == ENSource.Accounting;
				await CusTitleBusiness.SaveRangeAsync(EnSystem.SignalR, lst, 10, send2Server, Priority.High, awit4Send: false, 10, "", e.Connection.cancellationToken);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CusTitleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 70);
			}
		}
	}

	private async Task CusTitleInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCusTitle> e)
	{
		using (new JaegerMonitor("CusTitleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					throw new Exception("فراخوانی سیو رکوئست عنوان اشخاص در محصولی بغیر از حسابداری".GetTranslate());
				}
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("CusTitleInstance_OnSaveRequest received but data is null or empty!", "", "CusTitleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 82);
					return;
				}
				WebErrorLog.Instance.Debug("CusTitleInstance_OnSaveRequest received ....", "", "CusTitleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 86);
				List<CusTitleBusiness> list = await CusTitleMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || !list.Any())
				{
					throw new Exception("data was exists but mapped data is empty!");
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (CusTitleBusiness cusTitle in list)
				{
					bool flag = Cache.Source == ENSource.Accounting;
					if (flag)
					{
						cusTitle.ServerStatus = ServerStatusForClient.None;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await cusTitle.SaveAsync(EnSystem.SignalR, 10, flag, Priority.High, awaitOnSemaphore: true, await4Send: false, 10, "", e.Connection.cancellationToken);
					WebDelivery item = new WebDelivery
					{
						Guid = cusTitle.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.CustomerTitle
					};
					deliveryList.Add(item);
				}
				if (deliveryList?.Any() ?? false)
				{
					List<Packet> lstPacket = WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId);
					await Cache.SendData(lstPacket, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CusTitleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 112);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				if (Cache.Context.WebCusTitle != null)
				{
					Cache.Context.WebCusTitle.OnReadResponse -= CusTitleInstance_OnReadResponse;
					Cache.Context.WebCusTitle.OnSync -= CusTitleInstance_OnSync;
					Cache.Context.WebCusTitle.OnSaveRequest -= CusTitleInstance_OnSaveRequest;
				}
				Cache = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerTitleWebServiceRepository.cs", 129);
		}
	}
}
