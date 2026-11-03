using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting.NewFolder;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Serilog;

namespace EntityCache.WebService.Repository.Accounting;

public class DiscountCouponWebServiceRepository : WebServiceRepository<CouponDiscountBusiness>, IDisposable
{
	public DiscountCouponWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<CouponDiscountBusiness>)cache.CacheRepository.CouponDiscount, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.DiscoupntCoupon.OnReadResponse += Instance_OnReadResponse;
			Cache.Context.DiscoupntCoupon.OnSync += Instance_OnSync;
			Cache.Context.DiscoupntCoupon.OnSaveRequest += Instance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DiscountCouponWebServiceRepository.cs", 26);
		}
	}

	private async Task Instance_OnSync(object sender, ConnectionEventArgs<WebDiscountCoupon> e)
	{
		using (new JaegerMonitor("Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DiscountCouponWebServiceRepository.cs"))
		{
			CancellationToken? cancellationToken = e?.Connection?.cancellationToken;
			if (!cancellationToken.HasValue)
			{
				new CancellationToken(canceled: true);
			}
			else
			{
				cancellationToken.GetValueOrDefault();
			}
			try
			{
				if (e.ConnectionData != null && Cache.Source != ENSource.Accounting && Cache.Source != ENSource.Android_HesabYar && (e.ConnectionData?.Any() ?? false))
				{
					List<WebDiscountCoupon> list = e.ConnectionData.ToList();
					bool sendToServer = Cache.Source == ENSource.Accounting;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await CouponDiscountBusiness.SaveRangeFromWebAsync(base.ContextGuid, list, sendToServer, Priority.Low, e.Connection.cancellationToken);
					returnedSaveFuncInfo.throwExceptionIfError();
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.DiscountCoupon, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DiscountCouponWebServiceRepository.cs", 50);
			}
		}
	}

	private async Task Instance_OnReadResponse(object sender, ConnectionEventArgs<WebDiscountCoupon> e)
	{
		using (new JaegerMonitor("Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DiscountCouponWebServiceRepository.cs"))
		{
			try
			{
				if (e.ConnectionData != null && Cache.Source != ENSource.Accounting && Cache.Source != ENSource.Android_HesabYar)
				{
					List<WebDiscountCoupon> connectionData = e.ConnectionData;
					if (connectionData == null || connectionData.Any())
					{
						List<WebDiscountCoupon> list = e.ConnectionData.ToList();
						bool sendToServer = Cache.Source == ENSource.Accounting;
						await CouponDiscountBusiness.SaveRangeFromWebAsync(base.ContextGuid, list, sendToServer, Priority.Low, e.Connection.cancellationToken);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DiscountCouponWebServiceRepository.cs", 63);
			}
		}
	}

	private async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebDiscountCoupon> e)
	{
		using (new JaegerMonitor("Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DiscountCouponWebServiceRepository.cs"))
		{
			_ = 3;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					throw new Exception("فراخوانی سیو رکوئست وضعیت اسناد در محصولی بغیر از حسابداری".GetTranslate());
				}
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					Log.Warning("WebSanadPrint received but data is null or empty!");
					return;
				}
				List<CouponDiscountBusiness> list = await CouponDiscountMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || !list.Any())
				{
					throw new Exception("data was exists but mapped data is empty!");
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (CouponDiscountBusiness data in list)
				{
					CouponDiscountBusiness couponDiscountBusiness = await CouponDiscountBusiness.GetAsync(base.ContextGuid, data.Guid, e.Connection.cancellationToken);
					if (couponDiscountBusiness != null && couponDiscountBusiness.Version > data.Version)
					{
						WebDelivery item = new WebDelivery
						{
							Guid = data.Guid,
							Message = "",
							DeliveryStatus = true,
							DeliveryDate = DateTime.Now,
							ShortMessage = "",
							Part = EntityPart.DiscountCoupon
						};
						deliveryList.Add(item);
						continue;
					}
					bool flag = Cache.Source == ENSource.Accounting;
					if (flag)
					{
						data.ServerStatus = ServerStatusForClient.None;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await data.SaveAsync(EnSystem.SignalR, null, flag, Priority.Low, e.Connection.cancellationToken);
					WebDelivery item2 = new WebDelivery
					{
						Guid = data.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.DiscountCoupon
					};
					deliveryList.Add(item2);
				}
				if (deliveryList?.Any() ?? false)
				{
					List<Packet> lstPacket = WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId);
					await Cache.SendData(lstPacket, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DiscountCouponWebServiceRepository.cs", 121);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.DiscoupntCoupon.OnReadResponse -= Instance_OnReadResponse;
				Cache.Context.DiscoupntCoupon.OnSync -= Instance_OnSync;
				Cache.Context.DiscoupntCoupon.OnSaveRequest -= Instance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DiscountCouponWebServiceRepository.cs", 133);
		}
	}
}
