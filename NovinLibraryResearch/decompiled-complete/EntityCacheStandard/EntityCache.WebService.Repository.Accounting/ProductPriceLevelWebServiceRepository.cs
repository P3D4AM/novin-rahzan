using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting.Product;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Serilog;

namespace EntityCache.WebService.Repository.Accounting;

public class ProductPriceLevelWebServiceRepository : WebServiceRepository<ProductPriceLevelBusiness>, IDisposable
{
	public ProductPriceLevelWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<ProductPriceLevelBusiness>)cache.CacheRepository.ProductPriceLevel, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.ProductPriceLevel.OnReadResponse += Instance_OnReadResponse;
			Cache.Context.ProductPriceLevel.OnSync += Instance_OnSync;
			Cache.Context.ProductPriceLevel.OnSaveRequest += Instance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductPriceLevelWebServiceRepository.cs", 26);
		}
	}

	private async Task Instance_OnSync(object sender, ConnectionEventArgs<WebProductPriceLevel> e)
	{
		using (new JaegerMonitor("Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductPriceLevelWebServiceRepository.cs"))
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
				if (e.ConnectionData?.Any() ?? false)
				{
					List<WebProductPriceLevel> source = e.ConnectionData.ToList();
					bool send2Server = Cache.Source == ENSource.Accounting;
					List<ProductPriceLevelBusiness> lst = await ProductPriceLevelMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await ProductPriceLevelBusiness.SaveRangeAsync(EnSystem.SignalR, send2Server, e.Connection.cancellationToken, lst);
					returnedSaveFuncInfo.throwExceptionIfError();
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.ProductPriceLevel, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductPriceLevelWebServiceRepository.cs", 48);
			}
		}
	}

	private async Task Instance_OnReadResponse(object sender, ConnectionEventArgs<WebProductPriceLevel> e)
	{
		using (new JaegerMonitor("Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductPriceLevelWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					Log.Warning("Customer Title received but data is empty!");
					return;
				}
				List<WebProductPriceLevel> source = e.ConnectionData.ToList();
				List<ProductPriceLevelBusiness> lst = await ProductPriceLevelMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				bool sendToServer = Cache.Source == ENSource.Accounting;
				await ProductPriceLevelBusiness.SaveRangeAsync(EnSystem.SignalR, sendToServer, e.Connection.cancellationToken, lst, Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductPriceLevelWebServiceRepository.cs", 65);
			}
		}
	}

	private async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebProductPriceLevel> e)
	{
		using (new JaegerMonitor("Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductPriceLevelWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					throw new Exception("فراخوانی سیو رکوئست سطوح قیمت کالا در محصولی بغیر از حسابداری".GetTranslate());
				}
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					Log.Warning("CusTitleInstance_OnSaveRequest received but data is null or empty!");
					return;
				}
				List<ProductPriceLevelBusiness> list = await ProductPriceLevelMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || !list.Any())
				{
					throw new Exception("data was exists but mapped data is empty!");
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (ProductPriceLevelBusiness data in list)
				{
					bool flag = Cache.Source == ENSource.Accounting;
					if (flag)
					{
						data.ServerStatus = ServerStatusForClient.None;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await data.SaveAsync(EnSystem.SignalR, flag, Priority.High, e.Connection.cancellationToken);
					WebDelivery item = new WebDelivery
					{
						Guid = data.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.ProductPriceLevel
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
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductPriceLevelWebServiceRepository.cs", 106);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.ProductPriceLevel.OnReadResponse -= Instance_OnReadResponse;
				Cache.Context.ProductPriceLevel.OnSync -= Instance_OnSync;
				Cache.Context.ProductPriceLevel.OnSaveRequest -= Instance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductPriceLevelWebServiceRepository.cs", 118);
		}
	}
}
