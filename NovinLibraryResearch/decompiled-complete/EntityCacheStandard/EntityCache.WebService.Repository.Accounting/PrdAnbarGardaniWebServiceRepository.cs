using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Accounting;

public class PrdAnbarGardaniWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	private bool IsDisposed { get; set; }

	public PrdAnbarGardaniWebServiceRepository(ClsCache cache)
	{
		Cache = cache;
		if (Cache.Source == ENSource.Accounting)
		{
			Cache.Context.WebPrdAnbarGardani.OnSaveResponse += Instance_OnSaveResponse;
			Cache.Context.WebPrdAnbarGardani.OnSaveRequest += WebChequeM_OnSaveRequest;
		}
	}

	private async Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\PrdAnbarGardaniWebServiceRepository.cs"))
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
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\PrdAnbarGardaniWebServiceRepository.cs", 39);
			}
		}
	}

	private async Task WebChequeM_OnSaveRequest(object sender, ConnectionEventArgs<WebPrdAnbarGardani> e)
	{
		_ = 2;
		try
		{
			if (Cache.Source != ENSource.Accounting || !(e?.ConnectionData?.Any()).GetValueOrDefault())
			{
				return;
			}
			string contextGuid = Cache.ContextGuid;
			List<WebDelivery> deliveryList = new List<WebDelivery>();
			List<PrdAnbarGardaniBusiness> entityList = await PrdAnbarGardaniMapper.Instance.MapListAsync(contextGuid, e.ConnectionData.ToList());
			foreach (PrdAnbarGardaniBusiness item2 in entityList)
			{
				await PrdAnbarGardaniBusiness.UpdateUnitPriceAsync(contextGuid, item2);
			}
			if (!(entityList?.Any() ?? false))
			{
				return;
			}
			foreach (PrdAnbarGardaniBusiness item in entityList)
			{
				if (item != null)
				{
					if ((e?.Connection?.IsDisposed).GetValueOrDefault(true))
					{
						return;
					}
					if (item.Type <= (SanadType)0)
					{
						item.Type = SanadType.Movaghat;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await item.SaveAsync(EnSystem.SignalR, 10, isGenerateSanad: true, isTemp: false, e.Connection.cancellationToken, awaitOnSemaphore: true, 10);
					deliveryList.Add(GenerateDelveryResponce(returnedSaveFuncInfo, item.Guid, !returnedSaveFuncInfo.HasError));
				}
			}
			Cache.SendData(WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId), Priority.High);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebChequeM_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\PrdAnbarGardaniWebServiceRepository.cs", 63);
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
					Cache.Context.WebPrdAnbarGardani.OnSaveResponse -= Instance_OnSaveResponse;
					Cache.Context.WebPrdAnbarGardani.OnSaveRequest -= WebChequeM_OnSaveRequest;
				}
				Cache?.CacheRepository?.IPrdAnbarGardani.Dispose();
				if (Cache?.CacheRepository != null)
				{
					Cache.CacheRepository.IPrdAnbarGardani = null;
				}
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\PrdAnbarGardaniWebServiceRepository.cs", 82);
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
				Part = EntityPart.PrdAnbarGardani,
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
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDelveryResponce", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\PrdAnbarGardaniWebServiceRepository.cs", 106);
			throw;
		}
	}
}
