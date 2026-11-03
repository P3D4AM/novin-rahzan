using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Mapping.BusinessMappers.Accounting.Cheque;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Accounting;

public class ChequeShOperationWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	private bool IsDisposed { get; set; }

	public ChequeShOperationWebServiceRepository(ClsCache cache)
	{
		Cache = cache;
		if (Cache.Source == ENSource.Accounting)
		{
			Cache.Context.WebChequeShOperation.OnSaveResponse += Instance_OnSaveResponse;
			Cache.Context.WebChequeShOperation.OnSaveRequest += WebChequeSh_OnSaveRequest;
		}
	}

	private async Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeShOperationWebServiceRepository.cs"))
		{
			try
			{
				foreach (SaveResponse item in e.SaveResponses)
				{
					(await Cache.CacheRepository.ChequeShOperation.GetAsync(item.Guid, default(CancellationToken), ""))?.SetSaveResultAsync(item, e.Connection.cancellationToken, 10);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeShOperationWebServiceRepository.cs", 39);
			}
		}
	}

	private async Task WebChequeSh_OnSaveRequest(object sender, ConnectionEventArgs<WebChequeShOperation> e)
	{
		_ = 2;
		try
		{
			if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
			{
				return;
			}
			string contextGuid = Cache.ContextGuid;
			List<WebDelivery> deliveryList = new List<WebDelivery>();
			List<WebChequeShOperation> list = e.ConnectionData?.OrderBy((WebChequeShOperation q) => q.ChequeGuid)?.ThenBy((WebChequeShOperation q) => q.Ordinal)?.ToList();
			foreach (WebChequeShOperation cheque in list)
			{
				if (cheque == null)
				{
					continue;
				}
				if ((e?.Connection?.IsDisposed).GetValueOrDefault(true))
				{
					return;
				}
				if (await ChequeShOperationBusiness.GetAsync(contextGuid, cheque.Guid) != null)
				{
					deliveryList.Add(GenerateDelveryResponce(new ReturnedSaveFuncInfo(), cheque.Guid, isSuccess: true));
					continue;
				}
				ChequeShOperationBusiness chequeBusiness = await ChequeShOperationMapper.Instance.MapAsync(contextGuid, cheque, new ChequeShOperationBusiness(Cache.ContextGuid));
				if (chequeBusiness.OldState.HasValue)
				{
					deliveryList.Add(GenerateDelveryResponce(await chequeBusiness.SaveFromWebAsync(EnSystem.SignalR), chequeBusiness.Guid, isSuccess: true));
				}
			}
			Cache.SendData(WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId), Priority.High);
			ViewCheckShBusiness.ResendNotSentAsync(contextGuid, default(CancellationToken), "", 20);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebChequeSh_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeShOperationWebServiceRepository.cs", 68);
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (Cache.Context.WebChequeMOperation != null)
				{
					Cache.Context.WebChequeShOperation.OnSaveResponse -= Instance_OnSaveResponse;
					Cache.Context.WebChequeShOperation.OnSaveRequest -= WebChequeSh_OnSaveRequest;
				}
				Cache?.CacheRepository?.ChequeMOperation.Dispose();
				if (Cache?.CacheRepository != null)
				{
					Cache.CacheRepository.ChequeMOperation = null;
				}
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeShOperationWebServiceRepository.cs", 87);
		}
	}

	private WebDelivery GenerateDelveryResponce(ReturnedSaveFuncInfo res, Guid guid, bool isSuccess)
	{
		try
		{
			WebDelivery webDelivery = new WebDelivery
			{
				DeliveryDate = DateTime.Now,
				DeliveryId = 10000,
				Guid = guid,
				Message = res.GetErrorMessage(),
				ShortMessage = res.GetErrorMessage(),
				Modified = DateTime.Now,
				Part = EntityPart.ChequeShOperation,
				Status = true
			};
			if (!isSuccess)
			{
				webDelivery.DeliveryStatus = false;
			}
			else if (isSuccess && res.HasError)
			{
				webDelivery.DeliveryStatus = false;
			}
			else
			{
				webDelivery.DeliveryStatus = true;
			}
			return webDelivery;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDelveryResponce", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeShOperationWebServiceRepository.cs", 111);
			throw;
		}
	}
}
