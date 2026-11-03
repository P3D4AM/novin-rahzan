using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Models.Accounting.Prd;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;
using PacketParser.Records.Kardex;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal abstract class KardexPersistenceRepositoryBase : PersistenceNoGuidRepository<KardexBusiness, Kardex>, IKardexRepository, INoGuidRepository<KardexBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<KardexBusiness, Kardex>, IItemAsyncMapper<Kardex, KardexBusiness>
	{
		public Task<Kardex> MapAsync(string contextGuid, KardexBusiness source, Kardex destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new Kardex();
					}
					destination.Modified = source.Modified;
					destination.ProductGuid = source.GetProductGuid();
					destination.DepotGuid = source.DepotGuid;
					destination.DateM = source.DateM;
					destination.Discount = source.Discount;
					destination.Fare = source.Fare;
					destination.VisitorPrice = source.VisitorPrice;
					destination.Tax = source.Tax;
					destination.SanadGuid = source.SanadGuid;
					destination.SanadType = source.SanadType;
					destination.DetailGuid = source.DetailGuid;
					destination.VorodQT = source.VorodQT;
					destination.KhorojQT = source.KhorojQT;
					destination.UnitPrice = source.UnitPrice;
					destination.RemQT = source.RemQT;
					destination.UnitSood = source.UnitSood;
					destination.TotalSood = source.TotalSood;
					destination.RowSood = source.RowSood;
					destination.RemUnitPrice = source.RemUnitPrice;
					destination.RowSum = source.RowSum;
					destination.RemTotalPrice = source.RemTotalPrice;
					destination.UserGuid = source.UserGuid;
					destination.Description = source.Description;
					destination.RDescription = source.RDescription;
					destination.ID2 = source.ID2;
					destination.TotalHazine = source.TotalHazine;
					destination.Serial = source.Serial;
					destination.Avarez = source.Avarez;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.Ordinal = source.Ordinal;
					destination.AmaniCount = source.AmaniCount;
					destination.V2Guid = source.V2Guid;
					destination.V2Qt = source.V2Qt;
					destination.DiscountPercent = source.DiscountPercent;
					destination.DiscountPercentString = source.DiscountPercentString;
					destination.KardexAffective = source.KardexAffective;
					destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
					destination.CostPrice = source.CostPrice;
					destination.TotalCostPrice = source.TotalCostPrice;
					destination.ProductTafsilGuid = source.ProductTafsilGuid;
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 431);
					throw;
				}
			}
		}

		public async Task<KardexBusiness> MapAsync(string contextGuid, Kardex source, KardexBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("ابتدا شی مقصد را new کنید", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs"))
			{
				if (destination == null)
				{
					throw new Exception();
				}
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
					destination.DepotGuid = source.DepotGuid;
					destination.DateM = source.DateM;
					destination.Discount = source.Discount;
					destination.Fare = source.Fare;
					destination.VisitorPrice = source.VisitorPrice;
					destination.Tax = source.Tax;
					destination.SanadGuid = source.SanadGuid;
					destination.SanadType = source.SanadType;
					destination.DetailGuid = source.DetailGuid;
					destination.VorodQT = source.VorodQT;
					destination.KhorojQT = source.KhorojQT;
					destination.UnitPrice = source.UnitPrice;
					destination.RemQT = source.RemQT;
					destination.UnitSood = source.UnitSood;
					destination.TotalSood = source.TotalSood;
					destination.RowSood = source.RowSood;
					destination.RemUnitPrice = source.RemUnitPrice;
					destination.RowSum = source.RowSum;
					destination.RemTotalPrice = source.RemTotalPrice;
					destination.Description = source.Description;
					destination.RDescription = source.RDescription;
					destination.ID2 = source.ID2;
					destination.TotalHazine = source.TotalHazine;
					destination.Serial = source.Serial;
					destination.Avarez = source.Avarez;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.Ordinal = source.Ordinal;
					destination.AmaniCount = source.AmaniCount;
					await destination.SetV2GuidAsync(source.V2Guid, token, tranName);
					destination.V2Qt = source.V2Qt;
					destination.DiscountPercentString = source.DiscountPercentString;
					destination.KardexAffective = source.KardexAffective;
					destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
					destination.CostPrice = source.CostPrice;
					destination.TotalCostPrice = source.TotalCostPrice;
					destination.ProductTafsilGuid = source.ProductTafsilGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 493);
					throw;
				}
			}
		}

		public Task<List<Kardex>> MapListAsync(string contextGuid, IEnumerable<KardexBusiness> source, List<Kardex> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<KardexBusiness>> MapListAsync(string contextGuid, IEnumerable<Kardex> source, List<KardexBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Kardex, KardexBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<KardexBusiness, Kardex> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	internal KardexPersistenceRepositoryBase(ContextHolder context)
		: base(context)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 42);
		}
	}

	public abstract Task<decimal> LastVorodPriceAvgAsync(Guid prdGuid, DateTime? Date, Guid? depotGuid, bool? kardexAffective, CancellationToken token, string tranName);

	public async Task<IEnumerable<KardexBusiness>> GetAllAsync(KardexFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 48));
			List<KardexBusiness> ret = null;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Kardex> queryable = (IQueryable<Kardex>)c.Kardex;
					if (filter != null)
					{
						if (filter.PrdGuid.HasValue && filter.PrdGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex p) => p.ProductGuid == filter.PrdGuid);
						}
						if (filter.DepotGuid.HasValue && filter.DepotGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex p) => p.DepotGuid == filter.DepotGuid);
						}
						if (filter.Date1.HasValue)
						{
							queryable = queryable.Where((Kardex p) => p.DateM >= filter.Date1);
						}
						if (filter.Date2.HasValue)
						{
							queryable = queryable.Where((Kardex p) => p.DateM < filter.Date2);
						}
						if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex p) => p.TafsilGuid == filter.TafsilGuid);
						}
						if (filter.FactorType.HasValue && filter.FactorType != Document.None)
						{
							queryable = queryable.Where((Kardex p) => (int?)p.SanadType == (int?)filter.FactorType);
						}
						if (filter.MoeinGuid.HasValue && filter.MoeinGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex p) => p.MoeinGuid == filter.MoeinGuid);
						}
						if (filter.UserGuid.HasValue && filter.UserGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex p) => p.UserGuid == filter.UserGuid);
						}
						if ((filter.PrdGroupGuid ?? Guid.Empty) != Guid.Empty)
						{
							List<Guid> filteredProductGuidList = ((IQueryable<ProductGroupRelation>)c.PrdGroupRelation).Where((ProductGroupRelation p) => p.GroupGuid == filter.PrdGroupGuid.Value)?.Select((ProductGroupRelation q) => q.ProductGuid)?.ToList();
							if (filteredProductGuidList?.Any() ?? false)
							{
								queryable = queryable?.Where((Kardex q) => filteredProductGuidList.Contains(q.ProductGuid));
							}
						}
						if (!string.IsNullOrEmpty(filter.Serial))
						{
							queryable = queryable.Where((Kardex p) => DbFunctionsExtensions.Like(EF.Functions, p.Serial, $"%{filter.Serial}%"));
						}
						if (filter.UnitPrice1.HasValue)
						{
							decimal? unitPrice = filter.UnitPrice1;
							if ((unitPrice.GetValueOrDefault() > default(decimal)) & unitPrice.HasValue)
							{
								queryable = ((filter.UnitPrice1Type != 0) ? queryable.Where((Kardex p) => (decimal?)p.UnitPrice > filter.UnitPrice1) : queryable.Where((Kardex p) => (decimal?)p.UnitPrice == filter.UnitPrice1));
							}
						}
						if (filter.UnitPrice2.HasValue)
						{
							decimal? unitPrice = filter.UnitPrice2;
							if ((unitPrice.GetValueOrDefault() > default(decimal)) & unitPrice.HasValue)
							{
								queryable = ((filter.UnitPrice2Type != 0) ? queryable.Where((Kardex p) => (decimal?)p.UnitPrice < filter.UnitPrice2) : queryable.Where((Kardex p) => (decimal?)p.UnitPrice == filter.UnitPrice2));
							}
						}
						if (filter.KardexAffective.HasValue)
						{
							queryable = queryable?.Where((Kardex q) => q.KardexAffective == filter.KardexAffective.Value);
						}
						foreach (string item in filter.Description.SplitString())
						{
							string desc = item.RemoveCharactersForLinkSearch().FixString();
							if (!string.IsNullOrEmpty(desc))
							{
								queryable = queryable.Where((Kardex p) => DbFunctionsExtensions.Like(EF.Functions, p.Description.ToLower(), $"%{desc.ToLower()}%") || DbFunctionsExtensions.Like(EF.Functions, p.RDescription.ToLower(), $"%{desc.ToLower()}%"));
							}
						}
					}
					queryable = queryable.OrderByDescending((Kardex p) => p.DateM).ThenByDescending((Kardex q) => q.Ordinal).ThenByDescending((Kardex p) => p.Guid);
					if (filter?.CountForDisplay.HasValue ?? false)
					{
						queryable = queryable.Take(filter.CountForDisplay.Value);
					}
					ret = await GenerateListAsync(c, await EntityFrameworkQueryableExtensions.ToListAsync<Kardex>(queryable, token), token, tranName);
					ret = ret?.OrderBy((KardexBusiness p) => p.DateM)?.ThenBy((KardexBusiness p) => p.Ordinal)?.ThenBy((KardexBusiness q) => q.Guid)?.ToList();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 102);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs");
				}
			}
			return ret;
		}
	}

	private async Task<List<KardexBusiness>> GenerateListAsync(Acc_ModelContext context, IEnumerable<Kardex> dbList, CancellationToken token, string tranName)
	{
		_ = 3;
		try
		{
			List<KardexBusiness> ret = new List<KardexBusiness>();
			if (!(dbList?.Any() ?? false))
			{
				return null;
			}
			List<clsKardexAccess> kardexAccessList = new List<clsKardexAccess>();
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid, token);
			_ = context.Products;
			if (cashierBusiness != null)
			{
				UserAccBusiness userAccBusiness = await cashierBusiness.GetUserAsync(token, tranName);
				if (userAccBusiness != null)
				{
					kardexAccessList = userAccBusiness.UserAccess.KardexAccessList;
				}
			}
			IEnumerable<IGrouping<Guid, Kardex>> enumerable = dbList?.GroupBy((Kardex q) => q.ProductGuid);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			foreach (IGrouping<Guid, Kardex> item in enumerable)
			{
				ProductGroupRelation prdGroupRelation = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductGroupRelation>((IQueryable<ProductGroupRelation>)context.PrdGroupRelation, (Expression<Func<ProductGroupRelation, bool>>)((ProductGroupRelation q) => q.ProductGuid == item.Key && q.Ordinal == 1), token);
				clsKardexAccess access = kardexAccessList?.FirstOrDefault((clsKardexAccess q) => q.Guid == (prdGroupRelation?.GroupGuid ?? Guid.Empty));
				foreach (Kardex item2 in item)
				{
					KardexBusiness destination = new KardexBusiness(base.ContextGuid, access);
					List<KardexBusiness> list = ret;
					list.Add(await MapFromDatabase.MapAsync(base.ContextGuid, item2, destination, tranName, token));
				}
			}
			return ret;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GenerateListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 148);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<IEnumerable<KardexInvalidatedModel>>> GetAllInvalidated_PrdId_DepotId_Async(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllInvalidated_PrdId_DepotId_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 154));
		using (new JaegerMonitor("GetAllInvalidated_PrdId_DepotId_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfoWithValue<IEnumerable<KardexInvalidatedModel>> ret = new ReturnedSaveFuncInfoWithValue<IEnumerable<KardexInvalidatedModel>>();
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfoWithValue<IEnumerable<KardexInvalidatedModel>> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await EntityFrameworkQueryableExtensions.ToListAsync<KardexInvalidatedModel>(from p in (IQueryable<Kardex>)c.Kardex
						where p.Invalidated
						group p by new { p.ProductGuid, p.DepotGuid } into p
						select new KardexInvalidatedModel
						{
							DepotGuid = p.Key.DepotGuid,
							ProductGuid = p.Key.ProductGuid
						}, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllInvalidated_PrdId_DepotId_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 176);
					ret.AddReturnedValue(ex2, "", "GetAllInvalidated_PrdId_DepotId_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 177);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllInvalidated_PrdId_DepotId_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs");
				}
			}
			return ret;
		}
	}

	public abstract Task<ReturnedSaveFuncInfo> ProgressRemAsync(Guid prdGuid, Guid dptGuid, CancellationToken token, string tranName);

	public abstract Task<ReturnedSaveFuncInfo> FillAsync(Guid? sanadGuid, Document? sanadType, DateTime? newDate, CancellationToken token, string tranName);

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<KardexBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs"))
		{
			try
			{
				foreach (KardexBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 198);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 202);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 203);
			}
			return ret;
		}
	}

	public async Task<KardexSumModel> GetSummaryInfoAsync(KardexSummaryFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSummaryInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = null;
			KardexSumModel result;
			try
			{
				_ = 1;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetSummaryInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 213));
					IQueryable<Kardex> queryable = (IQueryable<Kardex>)c.Kardex;
					if (filter != null)
					{
						queryable = queryable?.Where((Kardex w) => w.SanadType == filter.FactorType);
						if (filter.MoeinGuid.HasValue && filter.MoeinGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex w) => w.MoeinGuid == filter.MoeinGuid);
						}
						if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex w) => w.TafsilGuid == filter.TafsilGuid);
						}
						if (filter.ProductGuid.HasValue && filter.ProductGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex w) => w.ProductGuid == filter.ProductGuid);
						}
						if (filter.DepotGuid.HasValue && filter.DepotGuid != Guid.Empty)
						{
							queryable = queryable.Where((Kardex w) => w.DepotGuid == filter.DepotGuid);
						}
						if (filter.KardexAffective.HasValue)
						{
							queryable = queryable.Where((Kardex w) => w.KardexAffective == filter.KardexAffective.Value);
						}
					}
					KardexType? type = filter?.Type;
					result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<KardexSumModel>(from g in queryable
						group g by 1 into s
						select new KardexSumModel
						{
							AvarezSum = s.Sum((Kardex d) => d.Avarez),
							Bazaryab1Sum = s.Sum((Kardex d) => d.VisitorPrice),
							CountOfOrders = s.Count(),
							SumHazine1 = s.Sum((Kardex d) => d.TotalHazine),
							RowDiscount = s.Sum((Kardex f) => f.Discount),
							SumOfRowSumExcept_TaxAvarez = s.Sum((Kardex s) => s.RowSum),
							SumQuantity = s.Sum((Kardex d) => d.KhorojQT) + s.Sum((Kardex d) => d.VorodQT),
							TaxSum = s.Sum((Kardex d) => d.Tax),
							TotalSood = s.Sum((Kardex d) => d.RowSood),
							Type = type
						}, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetSummaryInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 244);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetSummaryInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<List<KardexViewModel>> GetAllViewModelAsync(CancellationToken token, DateTime? date1, DateTime? date2, bool GardeshManfi, bool NoghteSegaresh, bool? kardexAffective, string tranName);

	public abstract Task<List<KardexCustomerModel>> GetAllCustomerGardeshBetweenAsync(CancellationToken token, DateTime date1, DateTime date2, bool? kardexAffective, string tranName);

	public abstract Task<List<KardexTajmieReportBusiness>> GetAllTajmieProductAsync(CancellationToken token, TajmieeProductFilter filter, string tranName);

	public abstract Task<List<KardexTajmieReportBusiness>> GetAllGardeshProductAsync(DateTime date1, DateTime date2, bool? kardexAffective, CancellationToken token, string tranName);

	public abstract Task<double> GetPrdAmountAsync(Guid prdGuid, Guid depotGuid, DateTime date, bool? kardexAffective, CancellationToken token, string tranName);

	public abstract Task<List<OrderMoghayeratModel>> GetAllOrderMoghayeratAsync(CancellationToken token, string tranName);

	public async Task<DateTime?> GetLastRecordDateAsync(bool? kardexAffective, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastRecordDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 261));
		DateTime? result;
		try
		{
			_ = 1;
			try
			{
				DbSet<Kardex> kardex = c.Kardex;
				object obj;
				if (kardex == null)
				{
					obj = null;
				}
				else
				{
					IQueryable<Kardex> queryable = ((IQueryable<Kardex>)kardex).Where((Kardex q) => kardexAffective == null || q.KardexAffective == ((bool?)kardexAffective).Value);
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IOrderedQueryable<Kardex> orderedQueryable = queryable.OrderByDescending((Kardex q) => q.DateM);
						obj = ((orderedQueryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Kardex>((IQueryable<Kardex>)orderedQueryable, token) : null);
					}
				}
				result = (await (Task<Kardex>)obj)?.DateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetLastRecordDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 272);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetLastRecordDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public virtual Task<IEnumerable<KardexLinkedOrder>> GetAllLinkSellAsync(Guid tafsilGuid, Guid productGuid, DateTime dateM, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<KardexBusiness> GetAsync(Guid productGuid, Guid depotGuid, DateTime date, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 284));
		KardexBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<Kardex> list = await EntityFrameworkQueryableExtensions.ToListAsync<Kardex>(((IQueryable<Kardex>)c.Kardex).Where((Kardex q) => q.ProductGuid == productGuid && q.DepotGuid == depotGuid && q.DateM < date), token);
				if (list == null || !list.Any())
				{
					result = null;
				}
				else
				{
					list = list?.OrderBy((Kardex p) => p.DateM)?.ThenBy((Kardex p) => p.Ordinal)?.ThenBy((Kardex q) => q.Guid)?.ToList();
					result = await MapFromDatabase.MapAsync(base.ContextGuid, list.LastOrDefault(), new KardexBusiness(base.ContextGuid, null), tranName, token);
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 296);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public virtual Task<IEnumerable<KardexConflictRecord>> GetAllConflictsAsync(MaliYearSanadType sanadType, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<KardexBusiness> GetLastRecordsByDateAsync(DateTime date, Guid prdGuid, Guid depotGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetLastRecordsByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "GetLastRecordsByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 309));
			KardexBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Kardex> source = (IQueryable<Kardex>)c.Kardex;
					if (prdGuid != Guid.Empty)
					{
						source = source.Where((Kardex p) => p.ProductGuid == prdGuid);
					}
					if (depotGuid != Guid.Empty)
					{
						source = source.Where((Kardex p) => p.DepotGuid == depotGuid);
					}
					source = source.Where((Kardex p) => p.DateM < date);
					source = source.OrderByDescending((Kardex p) => p.DateM).ThenByDescending((Kardex q) => q.Ordinal).ThenByDescending((Kardex p) => p.Guid);
					Kardex kardex = await ((source != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Kardex>(source, token) : null);
					result = ((kardex != null) ? (await MapFromDatabase.MapAsync(base.ContextGuid, kardex, new KardexBusiness(base.ContextGuid, null), tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetLastRecordsByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 330);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastRecordsByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<KardexBusiness> GetLastEnteredRecordAsync(Guid productGuid, Guid? depotGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetLastEnteredRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "GetLastEnteredRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 342));
			KardexBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Kardex> source = ((IQueryable<Kardex>)c.Kardex).Where((Kardex p) => p.ProductGuid == productGuid && p.VorodQT > 0m && (p.SanadType == Document.OrderBuy || p.SanadType == Document.AnbarGardani || p.SanadType == Document.Tolid));
					if ((depotGuid ?? Guid.Empty) != Guid.Empty)
					{
						source = source.Where((Kardex p) => p.DepotGuid == depotGuid);
					}
					source = source.OrderByDescending((Kardex p) => p.DateM).ThenByDescending((Kardex q) => q.Ordinal).ThenByDescending((Kardex p) => p.Guid);
					Kardex kardex = await ((source != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Kardex>(source, token) : null);
					result = ((kardex != null) ? (await MapFromDatabase.MapAsync(base.ContextGuid, kardex, new KardexBusiness(base.ContextGuid, null), tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetLastEnteredRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs", 361);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastEnteredRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\KardexPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}
}
