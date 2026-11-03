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

public class ChequeMOperationWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	private bool IsDisposed { get; set; }

	public ChequeMOperationWebServiceRepository(ClsCache cache)
	{
		Cache = cache;
		if (Cache.Source == ENSource.Accounting)
		{
			Cache.Context.WebChequeMOperation.OnSaveResponse += Instance_OnSaveResponse;
			Cache.Context.WebChequeMOperation.OnSaveRequest += WebChequeM_OnSaveRequest;
		}
	}

	private async Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeMOperationWebServiceRepository.cs"))
		{
			try
			{
				foreach (SaveResponse item in e.SaveResponses)
				{
					(await Cache.CacheRepository.ChequeMOperation.GetAsync(item.Guid, default(CancellationToken), ""))?.SetSaveResultAsync(item, e.Connection.cancellationToken, 10);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeMOperationWebServiceRepository.cs", 41);
			}
		}
	}

	private async Task WebChequeM_OnSaveRequest(object sender, ConnectionEventArgs<WebChequeMOperation> e)
	{
		_ = 3;
		try
		{
			if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
			{
				return;
			}
			string contextGuid = Cache.ContextGuid;
			List<WebDelivery> deliveryList = new List<WebDelivery>();
			List<WebChequeMOperation> list = e.ConnectionData?.OrderBy((WebChequeMOperation q) => q.GuidCheque)?.ThenBy((WebChequeMOperation q) => q.Ordinal)?.ToList();
			foreach (WebChequeMOperation cheque in list)
			{
				if (cheque == null)
				{
					continue;
				}
				if ((e?.Connection?.IsDisposed).GetValueOrDefault(true))
				{
					return;
				}
				if (await ChequeMOperationBusiness.GetAsync(contextGuid, cheque.Guid) != null)
				{
					deliveryList.Add(GenerateDelveryResponce(new ReturnedSaveFuncInfo(), cheque.Guid, isSuccess: true));
					continue;
				}
				ChequeMOperationBusiness chequeBusiness = await ChequeMOperationMapper.Instance.MapAsync(contextGuid, cheque, new ChequeMOperationBusiness(Cache.ContextGuid));
				ReturnedSaveFuncInfo res = await chequeBusiness.CheckValidationAsync(EnSystem.SignalR, isDeleteAction: false, "");
				if (res.HasError)
				{
					deliveryList.Add(GenerateDelveryResponce(res, chequeBusiness.Guid, isSuccess: false));
				}
				else if (chequeBusiness.OldState.HasValue)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await chequeBusiness.SaveFromWebAsync(EnSystem.SignalR), ReturnedState.None, "WebChequeM_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeMOperationWebServiceRepository.cs", 69);
					deliveryList.Add(GenerateDelveryResponce(res, chequeBusiness.Guid, !res.HasError));
				}
			}
			Cache.SendData(WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId), Priority.High);
			ViewCheckMBusiness.ResendNotSentAsync(contextGuid, default(CancellationToken), "", 20);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebChequeM_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeMOperationWebServiceRepository.cs", 75);
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
					Cache.Context.WebChequeMOperation.OnSaveResponse -= Instance_OnSaveResponse;
					Cache.Context.WebChequeMOperation.OnSaveRequest -= WebChequeM_OnSaveRequest;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeMOperationWebServiceRepository.cs", 95);
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
				Part = EntityPart.ChequeMOperation,
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
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDelveryResponce", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\ChequeMOperationWebServiceRepository.cs", 120);
			throw;
		}
	}
}
