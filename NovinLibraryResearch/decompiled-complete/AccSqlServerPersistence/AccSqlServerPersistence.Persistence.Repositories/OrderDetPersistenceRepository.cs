using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Gift;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class OrderDetPersistenceRepository : PersistenceMasterRepository<OrderDetailBusiness, OrderDetail>, IOrderDetailRepository, IPersistenceMasterRepository<OrderDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<OrderDetailBusiness, OrderDetail>, IItemAsyncMapper<OrderDetail, OrderDetailBusiness>
	{
		public async Task<OrderDetail> MapAsync(string contextGuid, OrderDetailBusiness source, OrderDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs"))
			{
				_ = 1;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new OrderDetail();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ProductGuid = source.ProductGuid;
					destination.Quantity = source.Quantity;
					destination.Price = source.Price;
					destination.Price2 = source.Price2;
					OrderDetail orderDetail = destination;
					orderDetail.DepotGuid = await source.GetDepotGuidAsync();
					destination.Description = source.Description;
					destination.Discount = source.Discount;
					destination.VGuid = source.VGuid;
					destination.V2Qt = source.V2Qt;
					destination.VisitorPrice1 = source.VisitorPrice1;
					destination.VisitorPrice2 = source.VisitorPrice2;
					destination.VisitorPrice3 = source.VisitorPrice3;
					destination.VisitorPrice4 = source.VisitorPrice4;
					destination.TotalVisitorPrice = source.TotalVisitorPrice;
					destination.TotalHazinePrice = source.TotalHazinePrice;
					destination.TaxPercent = source.TaxPercent;
					destination.AvarezPercent = source.AvarezPercent;
					destination.Tax = source.Tax;
					destination.Avarez = source.Avarez;
					destination.RowSum = source.RowSum;
					destination.V2Price = source.V2Price;
					destination.Hazine1Price = source.Hazine1Price;
					destination.Hazine2Price = source.Hazine2Price;
					destination.Hazine3Price = source.Hazine3Price;
					destination.Hazine4Price = source.Hazine4Price;
					destination.Hazine5Price = source.Hazine5Price;
					destination.DecimalGuid = source.DecimalGuid;
					destination.DecimalQt = source.DecimalQt;
					destination.DecimalPrice = source.DecimalPrice;
					destination.DecimalName = source.DecimalName;
					destination.DecimalSum = source.DecimalSum;
					destination.PriceTypeGuid = (((source.PriceTypeGuid ?? Guid.Empty) == Guid.Empty) ? null : source.PriceTypeGuid);
					destination.OrderDetState = source.OrderDetState;
					destination.ProductName = source.ProductName;
					destination.Ordinal = source.Ordinal;
					if (source.SerialzedData != null)
					{
						destination.SerialzeData = Json.ToStringJson(source.SerialzedData);
					}
					destination.DiscountPercent = source.DiscountPercent;
					destination.DiscountPercentString = source.DiscountPercentString;
					destination.DicountCouponDetailGuid = source.DicountCouponDetailGuid;
					destination.ReferenceGuid = (((source?.ReferenceGuid ?? Guid.Empty) == Guid.Empty) ? null : source.ReferenceGuid);
					destination.FeePrice = source.FeePrice;
					destination.ProfitPrice = source.ProfitPrice;
					destination.GrouppingGuid = (((source?.GrouppingGuid ?? Guid.Empty) == Guid.Empty) ? null : source.GrouppingGuid);
					destination.Serial = source.Serial;
					destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
					orderDetail = destination;
					orderDetail.ProductTafsilGuid = await source.GetProductTafsilGuidAsync(token);
					destination.CostPrice = source.CostPrice;
					destination.TotalCostPrice = source.TotalCostPrice;
					destination.SteppedDiscount = source.SteppedDiscount ?? "";
					if (source.GiftDetailList?.Any() ?? false)
					{
						destination.GiftDetailList = Json.ToStringJson(source.GiftDetailList);
					}
					else
					{
						destination.GiftDetailList = "";
					}
					return destination;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 332);
					throw;
				}
			}
		}

		public async Task<OrderDetailBusiness> MapAsync(string contextGuid, OrderDetail source, OrderDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			if (destination == null)
			{
				throw new Exception("دوست عزیز لطفا ابتدا destination رو درست کن و بعد بده دست من!");
			}
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs"))
			{
				_ = 11;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					destination.IsEdit = true;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity, token, tranName);
					destination.ChangeInPrice = false;
					destination.ChangeInDiscount = false;
					await destination.SetProductGuidAsync(EnSystem.Mapper, source.ProductGuid, isLoadOriginalPrice: true, tranName, token);
					await destination.SetPriceAsync(EnSystem.Mapper, source.Price, token, tranName);
					destination.Price2 = source.Price2;
					await destination.SetDepotGuidAsync(source.DepotGuid, token);
					destination.Description = source.Description;
					await destination.SetDiscountAsync(EnSystem.Mapper, source.Discount, token, tranName);
					await destination.SetVGuidAsync(EnSystem.Mapper, source.VGuid, token, tranName);
					await destination.SetV2QtAsync(EnSystem.Mapper, source.V2Qt, token, tranName);
					destination.VisitorPrice1 = source.VisitorPrice1;
					destination.VisitorPrice2 = source.VisitorPrice2;
					destination.VisitorPrice3 = source.VisitorPrice3;
					destination.VisitorPrice4 = source.VisitorPrice4;
					destination.TotalHazinePrice = source.TotalHazinePrice;
					destination.Avarez = source.Avarez;
					destination.V2Price = source.V2Price;
					destination.Hazine1Price = source.Hazine1Price;
					destination.Hazine2Price = source.Hazine2Price;
					destination.Hazine3Price = source.Hazine3Price;
					destination.Hazine4Price = source.Hazine4Price;
					destination.Hazine5Price = source.Hazine5Price;
					destination.DecimalGuid = source.DecimalGuid;
					await destination.SetDecimalQuantityAsync(EnSystem.Mapper, source.DecimalQt, handleSameProducts: false, token);
					destination.DecimalPrice = source.DecimalPrice;
					destination.PriceTypeGuid = source.PriceTypeGuid;
					destination.OrderDetState = source.OrderDetState;
					destination.ChangeInPrice = false;
					destination.ChangeInDiscount = false;
					destination.SetTax(EnSystem.Mapper, source.Tax);
					destination.SetOrdinalChange(EnSystem.Mapper, source.Ordinal, destination, token);
					if (!string.IsNullOrEmpty(source.SerialzeData))
					{
						await destination.SetSerialzedDataAsync(source.SerialzeData.FromJson<OrderDetailSerializedData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 384));
						if (source.TaxPercent != 0m && destination.SerialzedData.TaxMode == PercentageMode.Percent)
						{
							destination.TaxPercent = source.TaxPercent;
						}
						if (source.AvarezPercent != 0m && destination.SerialzedData.AvarezMode == PercentageMode.Percent)
						{
							destination.AvarezPercent = source.AvarezPercent;
						}
					}
					else
					{
						if (source.TaxPercent != 0m)
						{
							destination.TaxPercent = source.TaxPercent;
						}
						if (source.AvarezPercent != 0m)
						{
							destination.AvarezPercent = source.AvarezPercent;
						}
					}
					destination.DicountCouponDetailGuid = (((source?.DicountCouponDetailGuid ?? Guid.Empty) == Guid.Empty) ? null : source.DicountCouponDetailGuid);
					destination.ReferenceGuid = (((source?.ReferenceGuid ?? Guid.Empty) == Guid.Empty) ? null : source.ReferenceGuid);
					await destination.SetFeePriceAsync(EnSystem.Mapper, source.FeePrice, token);
					await destination.SetProfitPrice(EnSystem.Mapper, source.ProfitPrice, token);
					destination.GrouppingGuid = (((source?.GrouppingGuid ?? Guid.Empty) == Guid.Empty) ? null : source.GrouppingGuid);
					destination.Serial = source.Serial;
					destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
					destination.ProductTafsilGuid = source.ProductTafsilGuid;
					destination.CostPrice = source.CostPrice;
					destination.TotalCostPrice = source.TotalCostPrice;
					await destination.SetSteppedDiscountAsync(source.SteppedDiscount, EnSystem.Mapper, token);
					if (!string.IsNullOrEmpty(source.GiftDetailList))
					{
						destination.GiftDetailList = source.GiftDetailList.FromJson<List<GiftDetailBusiness>>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 405);
					}
					return destination;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 412);
					throw;
				}
			}
		}

		public Task<List<OrderDetail>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<OrderDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<OrderDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetail> source, List<OrderDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _disposed;

	private PrdSerialPersistanceRepository _serialRepo;

	private OrderAbadPersistanceRepository _abadRepo;

	protected LastPricePersistenceRepository _lastPriceRepo;

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed
	{
		get
		{
			if (!_disposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public override IItemAsyncMapper<OrderDetail, OrderDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<OrderDetailBusiness, OrderDetail> MapToDatabase => Mapper;

	public OrderDetPersistenceRepository(ContextHolder db)
		: base(db)
	{
		try
		{
			_serialRepo = new PrdSerialPersistanceRepository(db);
			_abadRepo = new OrderAbadPersistanceRepository(db);
			_lastPriceRepo = new LastPricePersistenceRepository(db);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 41);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!_disposed)
			{
				_disposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 53);
		}
	}

	public override Task<List<OrderDetailBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use another overLoad");
	}

	public async Task GetAllAsync(OrderBusiness order, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 61));
			try
			{
				int num;
				_ = num - 1;
				_ = 9;
				try
				{
					line = 1;
					IQueryable<OrderDetail> queryable = ((IQueryable<OrderDetail>)c.OrderDet).Where((OrderDetail x) => x.MasterGuid == order.Guid);
					line = 20;
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IOrderedQueryable<OrderDetail> orderedQueryable = queryable.OrderBy((OrderDetail q) => q.Ordinal);
						obj = ((orderedQueryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<OrderDetail>((IQueryable<OrderDetail>)orderedQueryable, token) : null);
					}
					List<OrderDetail> list = await (Task<List<OrderDetail>>)obj;
					if (list == null)
					{
						return;
					}
					foreach (OrderDetail det in list)
					{
						IItemAsyncMapper<OrderDetail, OrderDetailBusiness> mapFromDatabase = MapFromDatabase;
						string guid = base.Context.Guid;
						OrderDetail source = det;
						OrderDetailBusiness detmodel = await mapFromDatabase.MapAsync(guid, source, await OrderDetailBusiness.GetOrderDetailBusinessAsync(base.ContextGuid, order, token, tranName), tranName, token);
						IQueryable<Guid> queryable2 = ((IQueryable<PrdSerialDetails>)c.PrdSerialDetails).Where((PrdSerialDetails q) => q.DetailGuid == detmodel.Guid && q.AsType == order.FactorType)?.Select((PrdSerialDetails q) => q.MasterGuid);
						if (queryable2 != null && queryable2.Any())
						{
							foreach (Guid item in queryable2)
							{
								PrdSerialBusiness prdSerialBusiness = await _serialRepo.GetAsync(item, token, tranName);
								if (prdSerialBusiness != null)
								{
									await detmodel.SetSerialAsync(prdSerialBusiness, EnSystem.Mapper);
								}
							}
						}
						OrderDetailBusiness orderDetailBusiness = detmodel;
						orderDetailBusiness.AbadStr = await _abadRepo.GetAsync(det.Guid, order.FactorType, token, tranName);
						await order.AddDetAsync(detmodel, EnSystem.Mapper, awaitOnSemaphor: true, token);
						IQueryable<OrderDetail> queryable3 = ((IQueryable<OrderDetail>)c.OrderDet).Where((OrderDetail x) => x.MasterGuid == order.Guid && (x.GrouppingGuid ?? Guid.Empty) == det.Guid && (x.GrouppingGuid ?? Guid.Empty) != x.Guid);
						if (queryable3 == null || !queryable3.Any())
						{
							continue;
						}
						foreach (OrderDetail item2 in queryable3)
						{
							mapFromDatabase = MapFromDatabase;
							guid = base.Context.Guid;
							source = item2;
							OrderDetailBusiness det2 = await mapFromDatabase.MapAsync(guid, source, await OrderDetailBusiness.GetOrderDetailBusinessAsync(base.ContextGuid, order, token, tranName), tranName, token);
							await detmodel.AddDetailListAsync(EnSystem.Mapper, det2, token);
						}
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, $"Line:{line}//MapFromDatabase is null:{MapFromDatabase == null}//IsDisposed:{IsDisposed}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 101);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs");
				}
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<OrderDetailBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (items?.Any((OrderDetailBusiness p) => p.Guid == Guid.Empty) ?? false)
			{
				throw new ArgumentException($"one or more items in list contains empty value for feild Guid \r\n masterGuid:{masterGuid}");
			}
			try
			{
				if (!(items?.Any() ?? false))
				{
					return ret;
				}
				int ordinal = 1;
				int maxOrdinal = items.Max((OrderDetailBusiness q) => q.Ordinal) + 1;
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 122));
				foreach (OrderDetailBusiness item in items)
				{
					OrderDetail orderDetail = await MapToDatabase.MapAsync(base.ContextGuid, item, new OrderDetail(), tranName, token);
					orderDetail.MasterGuid = masterGuid;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(orderDetail, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 127);
					if (ret.HasError)
					{
						return ret;
					}
					if (item.SerialStr?.Any() ?? false)
					{
						IQueryable<PrdSerialDetails> queryable = ((IQueryable<PrdSerialDetails>)dbcontext.PrdSerialDetails).Where((PrdSerialDetails q) => q.DetailGuid == item.Guid);
						if (queryable != null && queryable.Any())
						{
							IQueryable<PrdSerialDetails> queryable2 = queryable.Where((PrdSerialDetails q) => !item.SerialStr.Select((PrdSerialBusiness q) => q.Guid).Contains(q.MasterGuid));
							if (queryable2 != null && queryable2.Any())
							{
								dbcontext.PrdSerialDetails.RemoveRange((IEnumerable<PrdSerialDetails>)queryable2);
							}
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await _serialRepo.SaveRangeAsync(item.SerialStr, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 138);
					}
					if (ret.HasError)
					{
						return ret;
					}
					if (item.AbadStr != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await _abadRepo.SaveAsync(item.AbadStr, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 142);
					}
					if ((item.DetailList?.Any() ?? false) && item.ProductKind == EnProductKind.Decimal)
					{
						foreach (OrderDetail item2 in await MapToDatabase.MapListAsync(base.ContextGuid, item.DetailList, null, tranName, token))
						{
							item2.Ordinal = maxOrdinal;
							item2.MasterGuid = masterGuid;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item2, item2.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 150);
							maxOrdinal++;
						}
						if (ret.HasError)
						{
							return ret;
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveLastPriceAsync(item, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 155);
					ordinal++;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 162);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 163);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SaveLastPriceAsync(OrderDetailBusiness det, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		try
		{
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "SaveLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 173);
			LastPriceBusiness businessLastPrice = det.MasterOrder.LastPriceList?.SingleOrDefault((LastPriceBusiness q) => q.PrdGuid == det.ProductGuid);
			IEnumerable<LastPriceBusiness> enumerable = await _lastPriceRepo.GetAllAsync(det.ProductGuid, det.MasterOrder.TafsilGuid, det.MasterOrder.FactorType, token, tranName);
			if (businessLastPrice != null && !(enumerable?.Select((LastPriceBusiness q) => q.PrdGuid)?.Contains(businessLastPrice?.PrdGuid ?? Guid.Empty)).GetValueOrDefault())
			{
				LastPrice lastPrice = await _lastPriceRepo.MapToDatabase.MapAsync(base.ContextGuid, businessLastPrice, null, tranName, token);
				lastPrice.ServerStatus = ServerStatusForClient.None;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(lastPrice, lastPrice.TafsilGuid, lastPrice.PrdGuid), ReturnedState.None, "SaveLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 183);
				return res;
			}
			if (enumerable.Select((LastPriceBusiness q) => q.PrdGuid).Contains(businessLastPrice.PrdGuid))
			{
				LastPriceBusiness lastPriceBusiness = enumerable.Single((LastPriceBusiness q) => q.PrdGuid == det.ProductGuid);
				if (lastPriceBusiness.Date > businessLastPrice.Date)
				{
					return res;
				}
				if (lastPriceBusiness.Date <= businessLastPrice.Date && (businessLastPrice.Price != lastPriceBusiness.Price || businessLastPrice.Date != lastPriceBusiness.Date))
				{
					LastPrice lastPrice2 = await _lastPriceRepo.MapToDatabase.MapAsync(base.ContextGuid, businessLastPrice, null, tranName, token);
					lastPrice2.ServerStatus = ServerStatusForClient.None;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(lastPrice2, lastPrice2.TafsilGuid, lastPrice2.PrdGuid), ReturnedState.None, "SaveLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 201);
					return res;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 209);
			res.AddReturnedValue(ex, "", "SaveLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 210);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, Document orderType, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 221));
				IQueryable<OrderDetail> queryable = ((IQueryable<OrderDetail>)acc_ModelContext.OrderDet).Where((OrderDetail x) => x.MasterGuid == MasterGuid);
				acc_ModelContext.OrderDet.RemoveRange((IEnumerable<OrderDetail>)queryable);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 228);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 229);
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short trycount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use anotheroverLoads and pass orderType");
	}

	public async Task<Guid> GetMasterGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 236));
		Guid result;
		try
		{
			_ = 1;
			try
			{
				result = (await c.OrderDet.FindAsync(new object[1] { guid }))?.MasterGuid ?? Guid.Empty;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs", 246);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderDetPersistenceRepository.cs");
			}
		}
		return result;
	}
}
