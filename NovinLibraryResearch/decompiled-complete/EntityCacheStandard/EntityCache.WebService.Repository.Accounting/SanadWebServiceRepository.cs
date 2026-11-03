using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Accounting;

public class SanadWebServiceRepository : IDisposable
{
	private ClsCache Cache;

	private bool IsDisposed { get; set; }

	public SanadWebServiceRepository(ClsCache cache)
	{
		Cache = cache;
		Cache.Context.WebSanad.OnSaveResponse += Instance_OnSaveResponse;
		Cache.Context.WebSanad.OnSaveRequest += WebChequeM_OnSaveRequest;
	}

	private async Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\SanadWebServiceRepository.cs"))
		{
			try
			{
				foreach (SaveResponse item in e.SaveResponses)
				{
					(await Cache.CacheRepository.ISanad.GetAsync(item.Guid, default(CancellationToken), ""))?.SetSaveResultAsync(item, e.Connection.cancellationToken, 10);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\SanadWebServiceRepository.cs", 34);
			}
		}
	}

	private async Task WebChequeM_OnSaveRequest(object sender, ConnectionEventArgs<WebSanad> e)
	{
		_ = 2;
		try
		{
			if (Cache.Source == ENSource.Baje || !(e?.ConnectionData?.Any()).GetValueOrDefault())
			{
				return;
			}
			_ = Cache.ContextGuid;
			List<WebDelivery> deliveryList = new List<WebDelivery>();
			foreach (WebSanad connectionDatum in e.ConnectionData)
			{
				connectionDatum.ChangeStatus = EntityChangeSt.New;
			}
			List<SanadBusiness> list = await SanadMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (SanadBusiness sanad in list)
			{
				if (sanad != null)
				{
					if ((e?.Connection?.IsDisposed).GetValueOrDefault(true))
					{
						return;
					}
					List<ViewSanadDetailCoverBusiness> lst = await sanad.GetSanadCoverListAsync(EnSystem.SignalR);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await sanad.SaveAsync(EnSystem.SignalR, e.Connection.cancellationToken, false, true, "", lst, new Tuple<Document, Guid>(Document.Sanad, sanad.Guid));
					deliveryList.Add(GenerateDelveryResponce(returnedSaveFuncInfo, sanad.Guid, !returnedSaveFuncInfo.HasError));
				}
			}
			Cache.SendData(WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId), Priority.High);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebChequeM_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\SanadWebServiceRepository.cs", 58);
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
					Cache.Context.WebSanad.OnSaveResponse -= Instance_OnSaveResponse;
					Cache.Context.WebSanad.OnSaveRequest -= WebChequeM_OnSaveRequest;
				}
				Cache?.CacheRepository?.ISanad.Dispose();
				if (Cache?.CacheRepository != null)
				{
					Cache.CacheRepository.ISanad = null;
				}
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\SanadWebServiceRepository.cs", 77);
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
				Part = EntityPart.Sanad,
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
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDelveryResponce", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\SanadWebServiceRepository.cs", 101);
			throw;
		}
	}
}
