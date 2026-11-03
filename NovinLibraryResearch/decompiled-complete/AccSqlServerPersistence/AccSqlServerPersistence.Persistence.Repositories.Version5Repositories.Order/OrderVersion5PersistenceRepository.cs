using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Pardakht;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Reception;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Sanad;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Tolid;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.EventArgs.TaxService;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Order;

public class OrderVersion5PersistenceRepository : IOrderRepository, IEntityRepository<OrderBusiness, OrderReportBusiness>, IEditableDelivery, IDisposable
{
	private static ConcurrentDictionary<string, OrderVersion5PersistenceRepository> _instances = new ConcurrentDictionary<string, OrderVersion5PersistenceRepository>();

	private bool _isDisposed;

	private OrderDetailVersion5PersistenceRepository orderDetV5Repo;

	private ReceptionVersion5PersistenceRepository _receptionV5;

	private PardakhtVersion5PersistenceRepository _pardakhtV5;

	private TolidVersion5PersistenceRepository _tolid;

	private TableRelatioShipPersistenceRepository _relationRepo;

	public IOrderDetailRepository OrderDetailRepo { get; private set; }

	public WebContextHolder Context => DatabaseContext.Context;

	public string ContextGuid => Context?.Guid;

	public ContextHolder DatabaseContext { get; private set; }

	public bool IsDisposed => _isDisposed;

	public Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	internal OrderVersion5PersistenceRepository(ContextHolder context)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs"))
		{
			try
			{
				_instances.TryAdd(context.Context.Guid, this);
				DatabaseContext = context;
				orderDetV5Repo = new OrderDetailVersion5PersistenceRepository(context);
				_receptionV5 = new ReceptionVersion5PersistenceRepository(context);
				_pardakhtV5 = new PardakhtVersion5PersistenceRepository(context);
				_tolid = new TolidVersion5PersistenceRepository(context);
				_relationRepo = new TableRelatioShipPersistenceRepository(context);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 65);
				throw;
			}
		}
	}

	public static OrderVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		OrderVersion5PersistenceRepository orderVersion5PersistenceRepository = null;
		try
		{
			_instances.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 79);
			throw;
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				OrderDetailRepo?.Dispose();
				OrderDetailRepo = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 97);
			throw;
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(OrderBusiness item, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 106);
		try
		{
			Guid guid = item.Guid;
			Document orderType = item.FactorType;
			int orderId = await Guid_To_IdAsync(guid, orderType, token, tranName);
			if (orderId <= 0)
			{
				throw new ArgumentException($"value({orderId}) for orderId is not value");
			}
			if (orderType == Document.AmaniKhoroj || orderType == Document.AmaniVorod)
			{
				orderType = Document.PishFactor;
			}
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@SanadID", (object)orderId),
				new SqlParameter("@SanadType", (object)orderType)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_SanadDet_DeleteRows", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs"), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 120);
			OrderDetailVersion5PersistenceRepository instance = OrderDetailVersion5PersistenceRepository.GetInstance(ContextGuid);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await instance.RemoveRangeDetailsAsync(orderId, orderType, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 123);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _relationRepo.RemoveRangeAsync(guid, orderType, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 126);
			if (res.HasError)
			{
				return res;
			}
			parameters = new List<SqlParameter>
			{
				new SqlParameter("@OrderId", (object)orderId),
				new SqlParameter("@FactorType", (object)orderType),
				new SqlParameter("@DiscountSellTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.SellCoding.Debit_TafsilGuid_Discount),
				new SqlParameter("@DiscountSellMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.SellCoding.Debit_MoeinGuid_Discount),
				new SqlParameter("@DiscountBuyTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.BuyCoding.Credit_TafsilGuid_Discount),
				new SqlParameter("@DiscountBuyMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.BuyCoding.Credit_MoeinGuid_Discount),
				new SqlParameter("@SellTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.SellCoding.Credit_TafsilGuid_Product),
				new SqlParameter("@SellMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.SellCoding.Credit_MoeinGuid_Product),
				new SqlParameter("@BuyTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.BuyCoding.Debit_TafsilGuid_Product),
				new SqlParameter("@BuyMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.BuyCoding.Debit_MoeinGuid_Product),
				new SqlParameter("@BackBuyTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.BackBuyCoding.Credit_TafsilGuid_Product),
				new SqlParameter("@BackBuyMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.BackBuyCoding.Credit_MoeinGuid_Product),
				new SqlParameter("@BackSellTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.BackSellCoding.Debit_TafsilGuid_Product),
				new SqlParameter("@BackSellMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.BackSellCoding.Debit_MoeinGuid_Product),
				new SqlParameter("@AmaniVoroodMoeinGuid", (object)ParentDefaults.MoeinCoding.CLSMoein29860),
				new SqlParameter("@AmaniKhoroojMoeinGuid", (object)ParentDefaults.MoeinCoding.CLSMoein29957),
				new SqlParameter("@ProductMoeinGuid", (object)ParentDefaults.MoeinCoding.CLSMoein50)
			};
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Orders_Delete", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs"), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 149);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 153);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 154);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetPrintCountAsync(OrderBusiness item, DateTime dateM, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 162);
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@OrderGuid", (object)item.Guid),
					new SqlParameter("@DateM", (object)dateM)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("SP_Set_OrderPrintCount", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs"), ReturnedState.None, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 170);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 174);
				res.AddReturnedValue(ex, "", "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 175);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> OrderSortAsync(Document orderType, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 182);
		try
		{
			string spName = "";
			switch (orderType)
			{
			case Document.OrderSell:
				spName = "SP_Sell_Sort";
				break;
			case Document.OrderBuy:
				spName = "SP_Buy_Sort";
				break;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync(spName, null, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs"), ReturnedState.None, "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 191);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 195);
			res.AddReturnedValue(ex, "", "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 196);
		}
		return res;
	}

	public async Task<OrderBusiness> GetAsync(Guid guid, Document factorType, CancellationToken token, string tranName)
	{
		_ = 4;
		try
		{
			WebOrder web = await GetWebOrderAsync(guid, factorType, token, tranName);
			if (web == null)
			{
				return null;
			}
			web.ChangeStatus = EntityChangeSt.Edited;
			OrderBusiness res = await OrderMapper.Instance.MapAsync(ContextGuid, web, null, tranName, token);
			await res.SetDateAsync(web.Date, EnSystem.Mapper, token, tranName);
			if ((res?.ListKala?.Any()).GetValueOrDefault() && res.Discount <= 0m && res.ListKala.Any((OrderDetailBusiness q) => q.Discount >= q.RowSum))
			{
				foreach (OrderDetailBusiness item in res.ListKala)
				{
					if (!(item.RowSum > item.Discount))
					{
						res.Discount += item.Discount;
						await item.SetDiscountAsync(EnSystem.Mapper, 0m, token, tranName);
					}
				}
			}
			if (res.FactorType == Document.Zayeat && res.GetTafsilGuid() == Guid.Empty)
			{
				await res.SetTafsilGuid(ParentDefaults.TafsilCoding.CLSTafsilZayeat, EnSystem.Transfer, updateVisitorGuid: false, token, tranName);
			}
			return res;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 228);
			throw;
		}
	}

	public Task<OrderBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("Please Use Get By FactorType !!");
	}

	public Task<OrderReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(OrderBusiness order, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		await DatabaseContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 241);
		try
		{
			throw new NotImplementedException();
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 398);
			res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 399);
			return res;
		}
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<OrderBusiness>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return Task.FromResult<List<OrderBusiness>>(null);
	}

	public Task Add2CacheAsync(OrderBusiness item, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	private async Task<WebOrder> GetWebOrderAsync(Guid guid, Document type, CancellationToken token, string tranName = "")
	{
		WebOrder item = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetWebOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 422);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@guid", (object)guid),
					new SqlParameter("@FactorType", (object)(short)type)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Orders_GetByGuid", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetWebOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync(token))
				{
					item = await LoadDataWebOrderAsync(c, dr, token, tranName);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetWebOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 437);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetWebOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return item;
	}

	public Task ClearCachedItemAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<Guid> GetById2Async(bool? status, CancellationToken token, string tranName, int id2, Document factorType)
	{
		Guid guid = Guid.Empty;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 458);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@id2", (object)id2),
					new SqlParameter("@FactorType", (object)(short)factorType)
				};
				guid = await c.ExecuteScalarAsync<Guid>("sp_Orders_ID2_To_Guid", parameters, tranName, default(CancellationToken), 30, "GetById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 470);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return guid;
	}

	public async Task<int> NextId2Async(Document factorType, bool? status, int? statusId, CancellationToken token, string tranName)
	{
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 484);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@FactorType", (object)(short)factorType)
			};
			return await baseModelContext.ExecuteScalarAsync<int>("sp_Orders_NextId2", parameters, tranName, default(CancellationToken), 30, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 492);
			throw;
		}
	}

	public Task<bool> CustomerIsSend(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<OrderReportBusiness>> GetAllAsync(OrderFilter filter, CancellationToken token, string tranName)
	{
		IEnumerable<OrderReportBusiness> list = new List<OrderReportBusiness>();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 506);
		try
		{
			List<SqlParameter> list2 = new List<SqlParameter>
			{
				new SqlParameter("@Numbers", (object)filter.Numbers)
			};
			if (filter.DateFrom.HasValue)
			{
				list2.Add(new SqlParameter("@Date1", (object)filter.DateFrom));
			}
			if (filter.DateTo.HasValue)
			{
				list2.Add(new SqlParameter("@Date2", (object)filter.DateTo));
			}
			if (filter.IsLast30.HasValue)
			{
				list2.Add(new SqlParameter("@last30", (object)filter.IsLast30));
			}
			if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@tafsilGuid", (object)filter.TafsilGuid));
			}
			if (filter.FareFrom.HasValue)
			{
				list2.Add(new SqlParameter("@Fair1", (object)filter.FareFrom));
			}
			if (filter.FareTo.HasValue)
			{
				list2.Add(new SqlParameter("@Fair2", (object)filter.FareTo));
			}
			if (filter.DiscountFrom.HasValue)
			{
				list2.Add(new SqlParameter("@Discount1", (object)filter.DiscountFrom));
			}
			if (filter.DiscountTo.HasValue)
			{
				list2.Add(new SqlParameter("@Discount2", (object)filter.DiscountTo));
			}
			if (filter.SumFrom.HasValue)
			{
				list2.Add(new SqlParameter("@SUM1", (object)filter.SumFrom));
			}
			if (filter.SumTo.HasValue)
			{
				list2.Add(new SqlParameter("@Sum2", (object)filter.SumTo));
			}
			if (filter.Description != "%%")
			{
				list2.Add(new SqlParameter("@Description", (object)filter.Description));
			}
			if (filter.VisitorTafsilGuid.HasValue && filter.VisitorTafsilGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@VTafsilGuid", (object)filter.VisitorTafsilGuid));
			}
			if (filter.CashierGuid.HasValue && filter.CashierGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@CashierGuid", (object)filter.CashierGuid));
			}
			if (filter.VisitorPriceFrom.HasValue)
			{
				list2.Add(new SqlParameter("@VisitorPrice1", (object)filter.VisitorPriceFrom));
			}
			if (filter.VisitorPriceTo.HasValue)
			{
				list2.Add(new SqlParameter("@VisitorPrice2", (object)filter.VisitorPriceTo));
			}
			if (filter.TaxFrom.HasValue)
			{
				list2.Add(new SqlParameter("@Tax1", (object)filter.TaxFrom));
			}
			if (filter.TaxTo.HasValue)
			{
				list2.Add(new SqlParameter("@Tax2", (object)filter.TaxTo));
			}
			if (filter.Id2From.HasValue)
			{
				list2.Add(new SqlParameter("@Number1", (object)filter.Id2From));
			}
			if (filter.Id2To.HasValue)
			{
				list2.Add(new SqlParameter("@Number2", (object)filter.Id2To));
			}
			if (filter.CustomerGroupGuid.HasValue && filter.CustomerGroupGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@CustGroupGuid", (object)filter.CustomerGroupGuid));
			}
			_ = filter.FactorType;
			list2.Add(new SqlParameter("@FactorType", (object)(short)filter.FactorType));
			using DbDataReader dr = await baseModelContext.ExecuteReaderAsync("sp_Order_SelectAll", list2, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			while (await dr.ReadAsync())
			{
				List<OrderReportBusiness> list3 = (List<OrderReportBusiness>)list;
				list3.Add(await LoadDataOrderReportAsync(dr));
			}
			dr.Close();
			if (string.IsNullOrEmpty(filter.Search))
			{
				filter.Search = "";
			}
			List<string> list4 = filter.Search.SplitString();
			if (list4 != null && list4.Count > 0)
			{
				foreach (string item in list4)
				{
					if (!string.IsNullOrEmpty(item) && item.Trim() != "")
					{
						decimal price = item.ParseToDecimal();
						list = list.Where((OrderReportBusiness x) => x.CustomerFullName.Contains(item) || x.Id2.ToString().Contains(item) || x.Description.Contains(item) || x.BazaryabFullName.Contains(item) || (price > 0m && (x.Sum == price || x.B1 == price || x.B2 == price || x.B3 == price || x.B4 == price || x.Discount == price || x.DiscountRows == price || x.Keraye == price || x.Tax == price || x.Avarez == price)));
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 568);
			throw;
		}
		return list?.OrderBy((OrderReportBusiness q) => q.Date)?.ToList();
	}

	public async Task<OrderBusiness> GetAsyncByReceptionGuidAsync(Guid receptionGuid, CancellationToken token, string tranName)
	{
		OrderBusiness order = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsyncByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 576);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@receptionGuid", (object)receptionGuid)
				};
				object orderGuid = await c.ExecuteScalarAsync<object>("sp_GetOrderGuid_By_ReceptionGuid", parameters, tranName, default(CancellationToken), 30, "GetAsyncByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (orderGuid != null && !string.IsNullOrEmpty(orderGuid.ToString()))
				{
					order = await GetAsync((Guid)orderGuid, Document.OrderSell, token, tranName);
				}
				if (order == null && orderGuid != null && !string.IsNullOrEmpty(orderGuid.ToString()))
				{
					order = await GetAsync((Guid)orderGuid, Document.OrderBackBuy, token, tranName);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsyncByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 588);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsyncByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return order;
	}

	public async Task<OrderBusiness> GetAsyncByPardakhtGuidAsync(Guid pardakhtGuid, CancellationToken token, string tranName)
	{
		OrderBusiness order = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsyncByPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 601);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@pardakhtGuid", (object)pardakhtGuid)
				};
				object orderGuid = await c.ExecuteScalarAsync<object>("sp_GetOrderGuid_By_PardakhtGuid", parameters, tranName, default(CancellationToken), 30, "GetAsyncByPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (orderGuid != null && !string.IsNullOrEmpty(orderGuid.ToString()))
				{
					order = await GetAsync((Guid)orderGuid, Document.OrderBuy, token, tranName);
				}
				if (order == null && orderGuid != null && !string.IsNullOrEmpty(orderGuid.ToString()))
				{
					order = await GetAsync((Guid)orderGuid, Document.OrderBackSell, token, tranName);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsyncByPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 613);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsyncByPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return order;
	}

	public Task<OrderBusiness> GetFromDbAsync(Guid guid, Document factorType, CancellationToken token, string tranName)
	{
		return GetAsync(guid, factorType, token, tranName);
	}

	public Task<IEnumerable<OrderBusiness>> GetAllAsyncSearchAsync(string search, CancellationToken token, short tasvieshode, short printshode, bool? status, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<decimal> TotalSumOfSales(Guid tafsilGuid, DateTime date, Document orderType, CancellationToken token, string tranName)
	{
		decimal res = default(decimal);
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "TotalSumOfSales", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 631);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@tafsilGuid", (object)tafsilGuid),
					new SqlParameter("@FactorType", (object)(short)orderType),
					new SqlParameter("@Date", (object)date)
				};
				object obj = await c.ExecuteScalarAsync<object>("Sp_Order_GetTotalSumOFOrderToCustomer", parameters, tranName, default(CancellationToken), 30, "TotalSumOfSales", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (obj != null)
				{
					res = (decimal)obj;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TotalSumOfSales", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 645);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("TotalSumOfSales", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<Guid> Id_To_GuidAsync(int id, Document type, CancellationToken token, string tranName = "")
	{
		Guid guid = Guid.Empty;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 658);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@id", (object)id),
					new SqlParameter("@FactorType", (object)(short)type)
				};
				guid = await c.ExecuteScalarAsync<Guid>("sp_Orders_ID_To_Guid", parameters, tranName, default(CancellationToken), 30, "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 670);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return guid;
	}

	public async Task<int> Guid_To_IdAsync(Guid guid, Document type, CancellationToken token, string tranName = "")
	{
		int id = 0;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 683);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid),
					new SqlParameter("@FactorType", (object)(short)type)
				};
				id = await c.ExecuteScalarAsync<int>("sp_Orders_Guid_To_Id", parameters, tranName, default(CancellationToken), 30, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 695);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return id;
	}

	private async Task<OrderReportBusiness> LoadDataOrderReportAsync(DbDataReader dr)
	{
		OrderReportBusiness result = new OrderReportBusiness(ContextGuid);
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataOrderReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 800);
			return result;
		}
	}

	private async Task<WebOrder> LoadDataWebOrderAsync(BaseModelContext context, DbDataReader dr, CancellationToken token, string tranName)
	{
		WebOrder res = new WebOrder();
		try
		{
			res.ChangeStatus = EntityChangeSt.Edited;
			if (dr["Guid"] != DBNull.Value)
			{
				res.Guid = (Guid)dr["Guid"];
			}
			if (res.Guid == Guid.Empty)
			{
				throw new InvalidOperationException("orderDetail guid is null or empty!");
			}
			if (dr["Id"] != DBNull.Value)
			{
				res.Id = (int)dr["Id"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				res.Description = dr["Description"].ToString();
			}
			if (dr["SanadSt"] != DBNull.Value)
			{
				res.SanadSt = (SanadSt)dr["SanadSt"];
			}
			if (dr["B1"] != DBNull.Value)
			{
				res.B1 = (decimal)dr["B1"];
			}
			if (dr["B2"] != DBNull.Value)
			{
				res.B2 = (decimal)dr["B2"];
			}
			if (dr["B3"] != DBNull.Value)
			{
				res.B3 = (decimal)dr["B3"];
			}
			if (dr["B4"] != DBNull.Value)
			{
				res.B4 = (decimal)dr["B4"];
			}
			if (dr["B1TafsilGuid"] != DBNull.Value)
			{
				res.B1TafsilGuid = (Guid)dr["B1TafsilGuid"];
			}
			if (dr["B2TafsilGuid"] != DBNull.Value)
			{
				res.B2TafsilGuid = (Guid)dr["B2TafsilGuid"];
			}
			if (dr["B3TafsilGuid"] != DBNull.Value)
			{
				res.B3TafsilGuid = (Guid)dr["B3TafsilGuid"];
			}
			if (dr["B4TafsilGuid"] != DBNull.Value)
			{
				res.B4TafsilGuid = (Guid)dr["B4TafsilGuid"];
			}
			if (dr["B1MoeinGuid"] != DBNull.Value)
			{
				res.B1MoeinGuid = (Guid)dr["B1MoeinGuid"];
			}
			if (dr["B2MoeinGuid"] != DBNull.Value)
			{
				res.B2MoeinGuid = (Guid)dr["B2MoeinGuid"];
			}
			if (dr["B3MoeinGuid"] != DBNull.Value)
			{
				res.B3MoeinGuid = (Guid)dr["B3MoeinGuid"];
			}
			if (dr["B4MoeinGuid"] != DBNull.Value)
			{
				res.B4MoeinGuid = (Guid)dr["B4MoeinGuid"];
			}
			if (dr["Id2"] != DBNull.Value)
			{
				res.Id2 = (int)dr["Id2"];
			}
			if (dr["Tax"] != DBNull.Value)
			{
				res.Tax = (decimal)dr["Tax"];
			}
			if (dr["TaxTafsilGuid"] != DBNull.Value)
			{
				res.TaxTafsilGuid = (Guid)dr["TaxTafsilGuid"];
			}
			if (dr["TaxMoeinGuid"] != DBNull.Value)
			{
				res.TaxMoeinGuid = (Guid)dr["TaxMoeinGuid"];
			}
			if (dr["Avarez"] != DBNull.Value)
			{
				res.B4 = (decimal)dr["Avarez"];
			}
			if (dr["AvarezTafsilGuid"] != DBNull.Value)
			{
				res.AvarezTafsilGuid = (Guid)dr["AvarezTafsilGuid"];
			}
			if (dr["AvarezMoeinGuid"] != DBNull.Value)
			{
				res.AvarezMoeinGuid = (Guid)dr["AvarezMoeinGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				res.UserAccGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["Discount"] != DBNull.Value)
			{
				res.Discount = (decimal)dr["Discount"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				res.Date = (DateTime)dr["DateM"];
			}
			if (dr["ServerId"] != DBNull.Value)
			{
				res.HostRefrenced = dr["ServerId"].ToString();
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				res.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["SellerFactorNo"] != DBNull.Value)
			{
				res.SellerFactorNo = dr["SellerFactorNo"].ToString();
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				res.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (res.FactorType == Document.OrderSell && res.MoeinGuid == ParentDefaults.MoeinCoding.CLSMoein16)
			{
				res.MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
			}
			else if (res.FactorType == Document.OrderBuy && res.MoeinGuid == ParentDefaults.MoeinCoding.CLSMoein17)
			{
				res.MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
			}
			else if (res.FactorType == Document.OrderBackSell && res.MoeinGuid == ParentDefaults.MoeinCoding.CLSMoein32)
			{
				res.MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
			}
			else if (res.FactorType == Document.OrderBackBuy && res.MoeinGuid == ParentDefaults.MoeinCoding.CLSMoein33)
			{
				res.MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
			}
			if (dr["Fare"] != DBNull.Value)
			{
				res.Keraye = (decimal)dr["Fare"];
			}
			if (dr["FareTafsilGuid"] != DBNull.Value)
			{
				res.FareTafsilGuid = (Guid)dr["FareTafsilGuid"];
			}
			if (dr["FareMoeinGuid"] != DBNull.Value)
			{
				res.FareMoeinGuid = (Guid)dr["FareMoeinGuid"];
			}
			if (dr["CustomerFullName"] != DBNull.Value)
			{
				res.CustomerFullName = dr["CustomerFullName"].ToString();
			}
			if (dr["SanadNo"] != DBNull.Value)
			{
				res.SanadNo = (int)dr["SanadNo"];
			}
			if (dr["ReceptionGuid"] != DBNull.Value)
			{
				res.ReceptionGuid = (Guid)dr["ReceptionGuid"];
			}
			if (dr["PardakhtGuid"] != DBNull.Value)
			{
				res.PardakhtGuid = (Guid)dr["PardakhtGuid"];
			}
			if (dr["DateSabt"] != DBNull.Value)
			{
				res.DateSabt = (DateTime)dr["DateSabt"];
			}
			if (dr["TasviyeDay"] != DBNull.Value)
			{
				res.TasvieDay = (int)dr["TasviyeDay"];
			}
			if (dr["IP"] != DBNull.Value)
			{
				res.Ip = dr["IP"].ToString();
			}
			if (dr["Source"] != DBNull.Value)
			{
				res.Source = (ENSource)dr["Source"];
			}
			if (dr["PrintCount"] != DBNull.Value)
			{
				res.PrintCount = (short)dr["PrintCount"];
			}
			if (dr["TotalHazine"] != DBNull.Value)
			{
				res.TotalHazine = (decimal)dr["TotalHazine"];
			}
			if (dr["TotalHazine1"] != DBNull.Value)
			{
				res.TotalHazine1 = (decimal)dr["TotalHazine1"];
			}
			if (dr["TotalHazine2"] != DBNull.Value)
			{
				res.TotalHazine2 = (decimal)dr["TotalHazine2"];
			}
			if (dr["TotalHazine3"] != DBNull.Value)
			{
				res.TotalHazine3 = (decimal)dr["TotalHazine3"];
			}
			if (dr["TotalHazine4"] != DBNull.Value)
			{
				res.TotalHazine4 = (decimal)dr["TotalHazine4"];
			}
			if (dr["TotalHazine5"] != DBNull.Value)
			{
				res.TotalHazine5 = (decimal)dr["TotalHazine5"];
			}
			if (dr["HazineTafsilGuid1"] != DBNull.Value)
			{
				res.HazineTafsilGuid1 = (Guid)dr["HazineTafsilGuid1"];
			}
			if (dr["HazineTafsilGuid2"] != DBNull.Value)
			{
				res.HazineTafsilGuid2 = (Guid)dr["HazineTafsilGuid2"];
			}
			if (dr["HazineTafsilGuid3"] != DBNull.Value)
			{
				res.HazineTafsilGuid3 = (Guid)dr["HazineTafsilGuid3"];
			}
			if (dr["HazineTafsilGuid4"] != DBNull.Value)
			{
				res.HazineTafsilGuid4 = (Guid)dr["HazineTafsilGuid4"];
			}
			if (dr["HazineTafsilGuid5"] != DBNull.Value)
			{
				res.HazineTafsilGuid5 = (Guid)dr["HazineTafsilGuid5"];
			}
			if (dr["HazineMoeinGuid1"] != DBNull.Value)
			{
				res.HazineMoeinGuid1 = (Guid)dr["HazineMoeinGuid1"];
			}
			if (dr["HazineMoeinGuid2"] != DBNull.Value)
			{
				res.HazineMoeinGuid2 = (Guid)dr["HazineMoeinGuid2"];
			}
			if (dr["HazineMoeinGuid3"] != DBNull.Value)
			{
				res.HazineMoeinGuid3 = (Guid)dr["HazineMoeinGuid3"];
			}
			if (dr["HazineMoeinGuid4"] != DBNull.Value)
			{
				res.HazineMoeinGuid4 = (Guid)dr["HazineMoeinGuid4"];
			}
			if (dr["HazineMoeinGuid5"] != DBNull.Value)
			{
				res.HazineMoeinGuid5 = (Guid)dr["HazineMoeinGuid5"];
			}
			if (dr["BarnameNo"] != DBNull.Value)
			{
				res.BarnameNo = dr["BarnameNo"].ToString();
			}
			if (dr["FactorType"] != DBNull.Value)
			{
				short factorType = dr["FactorType"].ToString().ParseToShort(0);
				res.FactorType = (Document)factorType;
			}
			if (dr["TransferDate"] != DBNull.Value)
			{
				res.TransferDate = (DateTime?)dr["TransferDate"];
			}
			if (dr["ProductMoeinGuid"] != DBNull.Value)
			{
				res.ProductMoeinGuid = (Guid)dr["ProductMoeinGuid"];
			}
			if (res.FactorType == Document.Zayeat)
			{
				res.ProductMoeinGuid = (await LocalSettingBusiness.GetSettingAsync(ContextGuid)).CodeSetting.ZayeatCoding.Credit_MoeinGuid_Product;
			}
			if (dr["DiscountMoeinGuid"] != DBNull.Value)
			{
				res.DiscountMoeinGuid = (Guid)dr["DiscountMoeinGuid"];
			}
			if (dr["DiscountTafsilGuid"] != DBNull.Value)
			{
				res.DiscountTafsilGuid = (Guid)dr["DiscountTafsilGuid"];
			}
			List<WebOrderDetail> detList = await orderDetV5Repo.GetAllDetailsAsync(res.Guid, res.FactorType, tranName);
			if (detList == null || detList.Count <= 0)
			{
				WebErrorLog.Instance.StartLog("فاکتور " + res.FactorType.GetDisplay() + " با اقلام خالی \r\n" + $"شماره فاکتور: {res.Id2}\r\n" + $"شناسه فاکتور: {res.Guid}", "", "LoadDataWebOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 899);
			}
			if (res.Keraye > 0m && res.FactorType == Document.OrderBuy && (res.FareMoeinGuid ?? Guid.Empty) == Guid.Empty)
			{
				res.FareMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein31;
				res.FareTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil29771;
			}
			if (res.Keraye > 0m && res.FactorType != Document.OrderSell && (res.FareMoeinGuid ?? Guid.Empty) == Guid.Empty)
			{
				throw new NotImplementedException($"FactorType({res.FactorType}) keraye moeinGuid is empty!");
			}
			if (res.Keraye > 0m && (res.FareMoeinGuid ?? Guid.Empty) == Guid.Empty)
			{
				string text = $"OrderGuid:{res.Guid}\r\n" + $"Keraye:{res.Keraye}\r\n" + $"FactorType:{res.FactorType}\r\n" + $"FareMoeinGuid:{res.FareMoeinGuid}\r\n" + $"FareTafsilGuid:{res.FareTafsilGuid}\r\n" + "طرف حساب کرایه مشخص نشده است ولیکن کرایه مقدار دهی شده.".GetTranslate();
				if (string.IsNullOrEmpty(res.Description))
				{
					res.Description = text;
				}
				else
				{
					res.Description = res.Description + "\r\n" + text;
				}
				res.FareMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein31;
			}
			Guid? productTafsilGuid = null;
			if (dr["ProductTafsilGuid"] != DBNull.Value)
			{
				productTafsilGuid = (Guid)dr["ProductTafsilGuid"];
			}
			if (res.FactorType == Document.Zayeat)
			{
				productTafsilGuid = (await LocalSettingBusiness.GetSettingAsync(ContextGuid)).CodeSetting.ZayeatCoding.Credit_TafsilGuid_Product;
			}
			if (detList?.Any() ?? false)
			{
				foreach (WebOrderDetail item in detList)
				{
					if (item != null)
					{
						item.ProductTafsilGuid = productTafsilGuid;
					}
				}
			}
			res.ListKala = detList ?? new List<WebOrderDetail>();
			if (!(res?.ListKala?.Any()).GetValueOrDefault() && res.FactorType == Document.OrderSell)
			{
				string contextGuid = ContextGuid;
				string tranName2 = tranName;
				IEnumerable<DepotBusiness> depots = await DepotBusiness.GetAllAsync(contextGuid, EnSystem.Transfer, true, tranName2);
				DbDataReader reader = await context.ExecuteReaderAsync($"Select sumKhadamat from [order.sell] where Guid='{res.Guid}'", null, tranName, default(CancellationToken), CommandType.Text, 30, "LoadDataWebOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				await reader.ReadAsync();
				decimal sumKhadat = (decimal)reader[0];
				if (sumKhadat > 0m)
				{
					string contextGuid2 = ContextGuid;
					tranName2 = tranName;
					ProductCompactBusiness productCompactBusiness = (await ProductCompactBusiness.GetAllAsync(contextGuid2, EnSystem.Transfer, null, default(CancellationToken), tranName2))?.FirstOrDefault((ProductCompactBusiness p) => p.Type == EnProductType.Service);
					res.ListKala.Add(new WebOrderDetail
					{
						DepotGuid = depots.FirstOrDefault().Guid,
						Description = "اطلاعاتی از حدمات فاکتور شده موجود نبود به همین دلیل از اولین خدمات موجود در سیستم جهت عدم بروز مغایرت در حساب مشتری فاکتور صادر شد.".GetTranslate(),
						Guid = Guid.NewGuid(),
						Price = sumKhadat,
						ProductGuid = productCompactBusiness.Guid,
						Quantity = 1m
					});
				}
			}
			SanadDetailV5PersistenceRepository instance = SanadDetailV5PersistenceRepository.GetInstance(ContextGuid);
			WebOrder webOrder = res;
			webOrder.ListSanad = await instance.GetListAsync(res.Guid, res.FactorType, Guid.Empty, Guid.Empty, tranName);
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataWebOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 977);
			throw;
		}
	}

	public async Task<Guid> LastOrderGuidAsync(Document factorType, DateTime date, CancellationToken token, string tranName)
	{
		Guid res = Guid.Empty;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "LastOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 984);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@factorType", (object)(short)factorType),
					new SqlParameter("@date", (object)date)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Order_GetLastGuid", parameters, tranName, default(CancellationToken), 30, "LastOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					res = (Guid)obj;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "LastOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 998);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("LastOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use another overLoad and pass orderType");
	}

	public async Task<OrderBusiness> GetPreviewsOrderAsync(Document factorType, Guid currentOrderGuid, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetPreviewsOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1014);
		OrderBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Guid? orderGuid = null;
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@factorType", (object)(short)factorType),
					new SqlParameter("@currentOrderGuid", (object)currentOrderGuid)
				};
				if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("cashierGuid", (object)cashierGuid.Value));
				}
				object obj = await c.ExecuteScalarAsync<object>("sp_order_SelectPreviewsOrder", list, tranName, default(CancellationToken), 30, "GetPreviewsOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					orderGuid = (Guid)obj;
				}
				result = ((!((orderGuid ?? Guid.Empty) != Guid.Empty)) ? null : (await GetAsync(orderGuid.Value, factorType, token, tranName)));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetPreviewsOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1032);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetPreviewsOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<OrderBusiness> GetNextOrderAsync(Document factorType, Guid currentOrderGuid, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetNextOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1040);
		OrderBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Guid? orderGuid = null;
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@factorType", (object)(short)factorType),
					new SqlParameter("@currentOrderGuid", (object)currentOrderGuid)
				};
				if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("cashierGuid", (object)cashierGuid.Value));
				}
				object obj = await c.ExecuteScalarAsync<object>("sp_order_SelectNextOrder", list, tranName, default(CancellationToken), 30, "GetNextOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					orderGuid = (Guid)obj;
				}
				result = ((!((orderGuid ?? Guid.Empty) != Guid.Empty)) ? null : (await GetAsync(orderGuid.Value, factorType, token, tranName)));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNextOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1058);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetNextOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<OrderBusiness> GetFirstOrderAsync(Document factorType, DateTime date, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetFirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1066);
		OrderBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Guid? orderGuid = null;
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@factorType", (object)(short)factorType),
					new SqlParameter("@date", (object)date)
				};
				if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("cashierGuid", (object)cashierGuid.Value));
				}
				object obj = await c.ExecuteScalarAsync<object>("sp_order_SelectFirstOrder", list, tranName, default(CancellationToken), 30, "GetFirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					orderGuid = (Guid)obj;
				}
				result = ((!((orderGuid ?? Guid.Empty) != Guid.Empty)) ? null : (await GetAsync(orderGuid.Value, factorType, token, tranName)));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1084);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetFirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<OrderBusiness> GetLastOrderAsync(Document factorType, DateTime date, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1092);
		OrderBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Guid? orderGuid = null;
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@factorType", (object)(short)factorType),
					new SqlParameter("@date", (object)date)
				};
				if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("cashierGuid", (object)cashierGuid.Value));
				}
				object obj = await c.ExecuteScalarAsync<object>("sp_order_SelectLastOrder", list, tranName, default(CancellationToken), 30, "GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					orderGuid = (Guid)obj;
				}
				result = ((!((orderGuid ?? Guid.Empty) != Guid.Empty)) ? null : (await GetAsync(orderGuid.Value, factorType, token, tranName)));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1110);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<List<Guid>> GetNotTransferedAsync(Document factorType, CancellationToken token, string tranName)
	{
		List<Guid> res = new List<Guid>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1118);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@FactorType", (object)(short)factorType)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_order_SelectNotTransfered", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					res.Add((Guid)dr["Guid"]);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1129);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1143);
			try
			{
				_ = 1;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@OrderGuid", (object)filter.ObjectGuid),
						new SqlParameter("@FactorType", (object)(short)filter.ObjectType),
						new SqlParameter("@TransferDate", (object)filter.TransferDate)
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("SP_Order_UpdateTransferDate", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs"), ReturnedState.None, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1152);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1156);
					res.AddReturnedValue(ex, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1157);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs");
				}
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateFareCreditAsync(Document factorType, Guid fareMoeinGuid, Guid fareTafsilGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(ContextGuid.GetSqlConnectionString());
			try
			{
				SqlCommand val = new SqlCommand("sp_Order_UpdateFareCredit", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@factorType", (object)(short)factorType);
					cmd.Parameters.AddWithValue("@FareMoeinGuid", (object)fareMoeinGuid);
					cmd.Parameters.AddWithValue("@FareTafsilGuid", (object)fareTafsilGuid);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
					((DbConnection)(object)cn).Close();
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateFareCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1183);
			res.AddReturnedValue(ex, "", "UpdateFareCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1184);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateProductAsync(Document factorType, Guid prdGuid, string desc = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(ContextGuid.GetSqlConnectionString());
			try
			{
				SqlCommand val = new SqlCommand("sp_Order_UpdateDetailsNotFound", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				((DbCommand)val).CommandTimeout = 120;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@factorType", (object)(short)factorType);
					cmd.Parameters.AddWithValue("@ProductGuid", (object)prdGuid);
					cmd.Parameters.AddWithValue("@Desc", (object)desc);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
					((DbConnection)(object)cn).Close();
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1209);
			res.AddReturnedValue(ex, "", "UpdateProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderVersion5PersistenceRepository.cs", 1210);
			throw;
		}
	}

	public Task<int> NextAttachmentNoAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateAttachmentNoAsync(Guid orderGuid, int attachmentNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<Guid> GetDepotGuidAsync(Guid detailGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<OrderSummaryViewModel>> GetAllSummaryAsync(Document factorType, DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<OrderReportBusiness> GetLastOrderByTafsilGuidAsync(Guid tafsilGuid, Document orderType, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<short> GetNextDailyFactorNoAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<OrderNotPayedViewModel>> GetAllOrderNotPayedAsync(DateTime orderDate, Guid tafsilGuid, Guid orderGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateCustomerNameAsync(OrderBusiness item, string name, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<decimal> GetVisitorSumPriceAsync(OrderFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetTaxResultAsync(TaxInvoiceResultEventArgs result, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetTaxResultAsync(List<TaxInvoiceTrackingEventArgs> list, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateSanadAffectiveAsync(bool value, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<OrderDetailSerialModel>> GetAllDetailSerialAsync(string serial, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateKardexAffectiveAsync(Document factorType, bool kardexAffective, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<OrderTtmsReport>> GetOrderTtmsReportAsync(List<Guid> orderGuidList, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<OrderReportBusiness> GetReportById2Async(int id2, Document type, bool status, CancellationToken token, string tranName)
	{
		return null;
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid orderGuid, int statusId, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<OrderReportBusiness>> GetAllMoghayerat50Async(List<Guid> kolGuidList, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<OrdersForUpdateCashBackModel>> GetAllForUpdateCashBackAsync(DateTime? startDate, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateCashBackAsync(Guid orderGuid, Guid? cashBackGuid, bool isProgressed, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<decimal> GetRelatedBackSellSumAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
