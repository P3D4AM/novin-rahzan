using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class OrderCacheRepository : CacheEntityRepository<OrderBusiness, OrderReportBusiness>, IOrderRepository, IEntityRepository<OrderBusiness, OrderReportBusiness>, IEditableDelivery, IDisposable
{
	private IOrderRepository dbRepository { get; set; }

	public IOrderDetailRepository OrderDetailRepo { get; private set; }

	public ISanad_DetRepository OrderSanadDetRepo { get; private set; }

	public override IItemAsyncMapper<OrderBusiness, OrderBusiness> Mapper => OrderMapper.Instance;

	public OrderCacheRepository(IOrderRepository dbRepository, IOrderDetailRepository orderDetailRepo, ISanad_DetRepository orderSanadDetRepo, int CachingTimeSeconds)
		: base((IEntityRepository<OrderBusiness, OrderReportBusiness>)dbRepository, CachingTimeSeconds)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		OrderDetailRepo = orderDetailRepo;
		OrderSanadDetRepo = orderSanadDetRepo;
		this.dbRepository = dbRepository;
	}

	public Task<Guid> GetById2Async(bool? status, CancellationToken token, string tranName, int id2, Document factorType)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetById2Async(status, token, tranName, id2, factorType);
	}

	public async Task<int> NextId2Async(Document factorType, bool? status, int? statusId, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs"))
		{
			int num;
			try
			{
				num = await dbRepository.NextId2Async(factorType, status, statusId, token, tranName);
				if (num == 1 && ClsCache.GetCache(base.ContextGuid).Source != ENSource.Accounting)
				{
					num = await GetNextId2Async(factorType);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs", 51);
				throw;
			}
			return num;
		}
	}

	private async Task<int> GetNextId2Async(Document factorType)
	{
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			UserInformationBusiness userInformationBusiness = await UserInformationBusiness.GetAsync(base.ContextGuid);
			if (userInformationBusiness == null)
			{
				return 1;
			}
			int num = 0;
			switch (factorType)
			{
			case Document.Reception:
				num = (userInformationBusiness?.LastSaved?.LastReceptionId2).GetValueOrDefault();
				break;
			case Document.Pardakht:
				num = (userInformationBusiness?.LastSaved?.LastPardakhtId2).GetValueOrDefault();
				break;
			case Document.OrderSell:
				num = (userInformationBusiness?.LastSaved?.LastOrderSellId2).GetValueOrDefault();
				break;
			case Document.OrderBuy:
				num = (userInformationBusiness?.LastSaved?.LastOrderBuyId2).GetValueOrDefault();
				break;
			case Document.OrderBackBuy:
				num = (userInformationBusiness?.LastSaved?.LastOrderBackBuyId2).GetValueOrDefault();
				break;
			case Document.OrderBackSell:
				num = (userInformationBusiness?.LastSaved?.LastOrderBackSellId2).GetValueOrDefault();
				break;
			case Document.PishFactor:
				num = (userInformationBusiness?.LastSaved?.LastPishFactorId2).GetValueOrDefault();
				break;
			case Document.OrderPreBuy:
				num = (userInformationBusiness?.LastSaved?.LastPreBuyId2).GetValueOrDefault();
				break;
			case Document.Zayeat:
				num = (userInformationBusiness?.LastSaved?.LastZayeatId2).GetValueOrDefault();
				break;
			case Document.AmaniVorod:
				num = (userInformationBusiness?.LastSaved?.LastAmaniVorodId2).GetValueOrDefault();
				break;
			case Document.AmaniKhoroj:
				num = (userInformationBusiness?.LastSaved?.LastAmaniKhorojId2).GetValueOrDefault();
				break;
			}
			return checked(num + 1);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs", 104);
			throw;
		}
	}

	public Task<IEnumerable<OrderReportBusiness>> GetAllAsync(OrderFilter filter, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<OrderBusiness> GetAsyncByReceptionGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsyncByReceptionGuidAsync(guid, token, tranName);
	}

	public Task<OrderBusiness> GetAsyncByPardakhtGuidAsync(Guid guid, CancellationToken token, string tranName = "")
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsyncByPardakhtGuidAsync(guid, token, tranName);
	}

	public override async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery item, CancellationToken token, short tryCount, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.SetDeliveryAsync(item, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs", 133);
			line = 1;
			OrderBusiness x = await GetAsync(item.Guid, token, tranName);
			line = 2;
			await ClsCache.GetCache(base.ContextGuid).CacheRepository.IReception.ClearCachedItemAsync(x?.ReceptionGuid ?? Guid.Empty, token, tranName);
			line = 3;
			await ClsCache.GetCache(base.ContextGuid).CacheRepository.IPardakht.ClearCachedItemAsync(x?.PardakhtGuid ?? Guid.Empty, token, tranName);
			line = 4;
			if (x != null)
			{
				x.SetReception(null);
				x.SetPardakht(null);
			}
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs", 147);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs", 150);
			res.AddReturnedValue(ex2, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs", 151);
		}
		return res;
	}

	public override async Task Add2CacheAsync(OrderBusiness item, CancellationToken token, string tranName)
	{
		_ = 4;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			await base.Add2CacheAsync(item, token);
			ReceptionBusiness reception = await item.GetReceptionAsync(EnSystem.UserInterface, tranName, fromDatabase: false, token);
			PardakhtBusiness pardakht = await item.GetPardakhtAsync(EnSystem.UserInterface, token, tranName);
			if (reception != null && reception.Guid != Guid.Empty)
			{
				await ClsCache.GetCache(base.ContextGuid).CacheRepository.IReception.Add2CacheAsync(reception, token, tranName);
			}
			if (pardakht != null && pardakht.Guid != Guid.Empty)
			{
				await ClsCache.GetCache(base.ContextGuid).CacheRepository.IPardakht.Add2CacheAsync(pardakht, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Add2CacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs", 172);
			throw;
		}
	}

	public Task<OrderBusiness> GetFromDbAsync(Guid guid, Document factorType, CancellationToken token, string tranName = "")
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(guid, factorType, token, tranName);
	}

	public async Task<IEnumerable<OrderBusiness>> GetAllAsyncSearchAsync(string search, CancellationToken token, short tasvieshode, short printshode, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsyncSearchAsync(search, token, tasvieshode, printshode, status, tranName);
	}

	public async Task<decimal> TotalSumOfSales(Guid tafsilGuid, DateTime date, Document orderType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.TotalSumOfSales(tafsilGuid, date, orderType, token, tranName);
	}

	public Task<bool> CustomerIsSend(Guid guid, CancellationToken token, string tranName)
	{
		return dbRepository.CustomerIsSend(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(OrderBusiness item, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		base.TimingCache.TryRemove(item.Guid, out var _);
		return dbRepository.RemoveAsync(item, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetPrintCountAsync(OrderBusiness item, DateTime dateM, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SetPrintCountAsync(item, dateM, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateCustomerNameAsync(OrderBusiness item, string name, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.UpdateCustomerNameAsync(item, name, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> OrderSortAsync(Document orderType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.OrderSortAsync(orderType, token, tranName);
	}

	public override Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use another overLoad and pass orderType");
	}

	public async Task<OrderBusiness> GetAsync(Guid guid, Document factorType, CancellationToken token, string tranName = "")
	{
		if (ClsCache.GetCache(base.ContextGuid).Source == ENSource.Accounting)
		{
			throw new Exception("برای سورس حسابداری لطفا از دیتابیس مستقیم بگیرید");
		}
		base.TimingCache.TryGetValue(guid, out var order);
		if (order == null)
		{
			order = await GetFromDbAsync(guid, Document.None, token, tranName);
			await base.TimingCache.UpdateTimeAsync(guid, order);
		}
		return order;
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderCacheRepository.cs", 240);
		}
	}

	public Task<OrderBusiness> GetPreviewsOrderAsync(Document factorType, Guid guid, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetPreviewsOrderAsync(factorType, guid, cashierGuid, token, tranName);
	}

	public Task<OrderBusiness> GetFirstOrderAsync(Document factorType, DateTime date, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetFirstOrderAsync(factorType, date, cashierGuid, token, tranName);
	}

	public Task<OrderBusiness> GetNextOrderAsync(Document factorType, Guid guid, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetNextOrderAsync(factorType, guid, cashierGuid, token, tranName);
	}

	public Task<OrderBusiness> GetLastOrderAsync(Document factorType, DateTime date, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetLastOrderAsync(factorType, date, cashierGuid, token, tranName);
	}

	public Task<List<Guid>> GetNotTransferedAsync(Document factorType, CancellationToken token, string tranName)
	{
		return dbRepository.GetNotTransferedAsync(factorType, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateTransferDateAsync(filter, token, tranName);
	}

	public Task<int> NextAttachmentNoAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.NextAttachmentNoAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateAttachmentNoAsync(Guid orderGuid, int attachmentNo, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.UpdateAttachmentNoAsync(orderGuid, attachmentNo, token, tranName);
	}

	public Task<Guid> GetDepotGuidAsync(Guid detailGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetDepotGuidAsync(detailGuid, token, tranName);
	}

	public Task<OrderReportBusiness> GetLastOrderByTafsilGuidAsync(Guid tafsilGuid, Document orderType, CancellationToken token, string tranName)
	{
		return dbRepository.GetLastOrderByTafsilGuidAsync(tafsilGuid, orderType, token, tranName);
	}

	public Task<short> GetNextDailyFactorNoAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		return dbRepository.GetNextDailyFactorNoAsync(dateM, token, tranName);
	}

	public Task<IEnumerable<OrderNotPayedViewModel>> GetAllOrderNotPayedAsync(DateTime orderDate, Guid tafsilGuid, Guid orderGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllOrderNotPayedAsync(orderDate, tafsilGuid, orderGuid, token, tranName);
	}

	public Task<decimal> GetVisitorSumPriceAsync(OrderFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetVisitorSumPriceAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateSanadAffectiveAsync(bool value, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateSanadAffectiveAsync(value, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateCustomerFullNameAsync(tafsilGuid, name, token, tranName);
	}

	public Task<IEnumerable<OrderDetailSerialModel>> GetAllDetailSerialAsync(string serial, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllDetailSerialAsync(serial, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateDateMoaserAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateKardexAffectiveAsync(Document factorType, bool kardexAffective, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateKardexAffectiveAsync(factorType, kardexAffective, token, tranName);
	}

	public Task<IEnumerable<OrderTtmsReport>> GetOrderTtmsReportAsync(List<Guid> orderGuidList, CancellationToken token, string tranName)
	{
		return dbRepository.GetOrderTtmsReportAsync(orderGuidList, token, tranName);
	}

	public Task<OrderReportBusiness> GetReportById2Async(int id2, Document type, bool status, CancellationToken token, string tranName)
	{
		return dbRepository.GetReportById2Async(id2, type, status, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid orderGuid, int statusId, CancellationToken token, string tranName)
	{
		return dbRepository.ChangeStatusIdAsync(orderGuid, statusId, token, tranName);
	}

	public Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllExistingSourceAsync(token, tranName);
	}

	public Task<List<OrderReportBusiness>> GetAllMoghayerat50Async(List<Guid> kolGuidList, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllMoghayerat50Async(kolGuidList, token, tranName);
	}

	public Task<IEnumerable<OrdersForUpdateCashBackModel>> GetAllForUpdateCashBackAsync(DateTime? startDate, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllForUpdateCashBackAsync(startDate, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateCashBackAsync(Guid orderGuid, Guid? cashBackGuid, bool isProgressed, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateCashBackAsync(orderGuid, cashBackGuid, isProgressed, token, tranName);
	}

	public Task<decimal> GetRelatedBackSellSumAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetRelatedBackSellSumAsync(orderGuid, token, tranName);
	}
}
