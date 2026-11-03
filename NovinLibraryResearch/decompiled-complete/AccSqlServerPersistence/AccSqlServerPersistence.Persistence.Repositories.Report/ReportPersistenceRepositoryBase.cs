using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Business.Report;
using EntityCache.Core.Repository.Report;
using EntityCache.Models.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;
using PacketParser.Filters;
using PacketParser.Filters.Khazane;
using PacketParser.Records;
using PacketParser.Records.Kardex;
using PacketParser.Records.OccasionCalendar;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Tax;
using SharedAccEntityFramworkPersistence.Entities.Customer;

namespace AccSqlServerPersistence.Persistence.Repositories.Report;

internal abstract class ReportPersistenceRepositoryBase : IReportRepository, IDisposable
{
	private bool _isDisposed;

	public ContextHolder DatabaseContext { get; private set; }

	public WebContextHolder Context => DatabaseContext.Context;

	public string ContextGuid => Context?.Guid;

	public ReportPersistenceRepositoryBase(ContextHolder context)
	{
		DatabaseContext = context;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				DatabaseContext?.Dispose();
				DatabaseContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepositoryBase.cs", 47);
		}
	}

	public async Task<IEnumerable<BedehiTalabModel>> GetBedehiTalabReportAsync(EnBedehiTalabType type, DateTime? date1, DateTime? date2, short status, CancellationToken token, string tranName)
	{
		_ = 3;
		try
		{
			IEnumerable<BedehiTalabModel> result = null;
			switch (type)
			{
			case EnBedehiTalabType.CheckM:
			{
				if (status <= 0)
				{
					status = 1;
				}
				ReceptionChequeFilter filter4 = new ReceptionChequeFilter
				{
					DateFrom = date1,
					DateTo = date2,
					Status = (ENUMCheck)status
				};
				IEnumerable<ReceptionCheckReportBusiness> enumerable3 = await ReceptionCheckBusiness.GetAllAsync(ContextGuid, filter4, token, tranName);
				if (enumerable3 == null || !enumerable3.Any())
				{
					return null;
				}
				result = (from q in enumerable3
					group q by q.DateSh into q
					select new BedehiTalabModel
					{
						Count = q.Count(),
						DateSh = q.Key,
						Sum = q.Sum((ReceptionCheckReportBusiness w) => w.Price)
					})?.OrderBy((BedehiTalabModel q) => q.DateSh);
				break;
			}
			case EnBedehiTalabType.CheckSh:
			{
				if (status <= 0)
				{
					status = 101;
				}
				PardakhtChequeFilter filter3 = new PardakhtChequeFilter
				{
					DateFrom = date1,
					DateTo = date2,
					Status = (ENCheckSHStatus)status
				};
				IEnumerable<PardakhtCheckShReportBusiness> enumerable2 = await PardakhtCheckShBusiness.GetAllAsync(ContextGuid, filter3, token, tranName);
				if (enumerable2 == null || !enumerable2.Any())
				{
					return null;
				}
				result = (from q in enumerable2
					group q by q.DateSarresidSh into q
					select new BedehiTalabModel
					{
						Count = q.Count(),
						DateSh = q.Key,
						Sum = q.Sum((PardakhtCheckShReportBusiness w) => w.Price)
					})?.OrderBy((BedehiTalabModel q) => q.DateSh);
				break;
			}
			case EnBedehiTalabType.AghsatM:
			{
				if (status <= 0)
				{
					status = 2;
				}
				AghsatMFilter filter2 = new AghsatMFilter
				{
					DateFrom = date1,
					DateTo = date2,
					Status = (EnAghsatMStatus)status
				};
				IEnumerable<AghsatMBusiness> enumerable = await AghsatMBusiness.GetAllAsync(ContextGuid, filter2, token, tranName);
				if (enumerable == null || !enumerable.Any())
				{
					return null;
				}
				result = enumerable?.GroupBy((AghsatMBusiness q) => q.DateSh)?.Select((IGrouping<string, AghsatMBusiness> q) => new BedehiTalabModel
				{
					Count = q.Count(),
					DateSh = q.Key,
					Sum = q.Sum((AghsatMBusiness w) => w.TotalPrice)
				})?.OrderBy((BedehiTalabModel q) => q.DateSh);
				break;
			}
			case EnBedehiTalabType.AghsatSh:
			{
				if (status <= 0)
				{
					status = 2;
				}
				AghsatShFilter filter = new AghsatShFilter
				{
					DateFrom = date1,
					DateTo = date2,
					Status = (EnAghsatShStatus)status
				};
				List<AghsatShBusiness> list = await AghsatShBusiness.GetAllAsync(ContextGuid, filter, token, tranName);
				if (list == null || !list.Any())
				{
					return null;
				}
				result = (from q in list
					group q by q.DateSarresidSh into q
					select new BedehiTalabModel
					{
						Count = q.Count(),
						DateSh = q.Key,
						Sum = q.Sum((AghsatShBusiness w) => w.Price)
					})?.OrderBy((BedehiTalabModel q) => q.DateSh);
				break;
			}
			}
			return result;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetBedehiTalabReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepositoryBase.cs", 131);
			throw;
		}
	}

	public abstract Task<List<TarazHesabBusiness>> GetAllTarazHesabAsync(CancellationToken token, TarazHesabFilter filter, string tranName);

	public abstract Task<IEnumerable<AccountTreeModel>> GetAllAccountTreeAsync(CancellationToken token, string tranName);

	public async Task<PayamakManage> GetPayamakManageAsync(DateTime d1, DateTime d2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await DatabaseContext.GetContextAsync(tranName, 60000, "GetPayamakManageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepositoryBase.cs", 139));
		PayamakManage result;
		try
		{
			int num;
			_ = num - 1;
			_ = 5;
			try
			{
				PayamakManage payamakManage = new PayamakManage();
				PayamakManage payamakManage2 = payamakManage;
				payamakManage2.SumDiscount = await EntityFrameworkQueryableExtensions.SumAsync<Order>(((IQueryable<Order>)c.Order).Where((Order x) => x.Status && x.Date >= d1 && x.Date <= d2 && x.FactorType == Document.OrderSell), (Expression<Func<Order, decimal>>)((Order p) => p.Discount), token);
				PayamakManage payamakManage3 = payamakManage;
				payamakManage3.SumSell = await EntityFrameworkQueryableExtensions.SumAsync<Order>(((IQueryable<Order>)c.Order).Where((Order x) => x.Status && x.Date >= d1 && x.Date <= d2 && x.FactorType == Document.OrderSell), (Expression<Func<Order, decimal>>)((Order p) => p.TotalSum), token);
				PayamakManage payamakManage4 = payamakManage;
				payamakManage4.SumNaghdi = await EntityFrameworkQueryableExtensions.SumAsync<Reception>(((IQueryable<Reception>)c.Receptions).Where((Reception x) => x.Date >= d1 && x.Date <= d2), (Expression<Func<Reception, decimal>>)((Reception p) => p.SumNaghd), token);
				PayamakManage payamakManage5 = payamakManage;
				payamakManage5.SumBank = await EntityFrameworkQueryableExtensions.SumAsync<Reception>(((IQueryable<Reception>)c.Receptions).Where((Reception x) => x.Date >= d1 && x.Date <= d2), (Expression<Func<Reception, decimal>>)((Reception p) => p.SumBank), token);
				PayamakManage payamakManage6 = payamakManage;
				payamakManage6.CheckCount = await EntityFrameworkQueryableExtensions.CountAsync<ReceptionCheck>((IQueryable<ReceptionCheck>)EntityFrameworkQueryableExtensions.Include<ReceptionCheck, Reception>((IQueryable<ReceptionCheck>)c.ReceptionCheck, (Expression<Func<ReceptionCheck, Reception>>)((ReceptionCheck q) => q.ReceptionItem)), (Expression<Func<ReceptionCheck, bool>>)((ReceptionCheck x) => x.ReceptionItem.Date >= d1 && x.ReceptionItem.Date <= d2 && (int)x.Status != 6), token);
				PayamakManage payamakManage7 = payamakManage;
				payamakManage7.CheckSum = await EntityFrameworkQueryableExtensions.SumAsync<ReceptionCheck>(((IQueryable<ReceptionCheck>)EntityFrameworkQueryableExtensions.Include<ReceptionCheck, Reception>((IQueryable<ReceptionCheck>)c.ReceptionCheck, (Expression<Func<ReceptionCheck, Reception>>)((ReceptionCheck q) => q.ReceptionItem))).Where((ReceptionCheck x) => x.ReceptionItem.Date >= d1 && x.ReceptionItem.Date <= d2 && (int)x.Status != 6), (Expression<Func<ReceptionCheck, decimal>>)((ReceptionCheck w) => w.Price), token);
				result = payamakManage;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetPayamakManageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepositoryBase.cs", 157);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetPayamakManageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public abstract Task<IEnumerable<SumKolBusiness>> GetSumKolListAsync(SumKolFilter filter, CancellationToken token, string tranName);

	public virtual Task<WebDepartmentEntitySummery> GetDepartmentEntitySummeryAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<List<OccasionCalendarRecord>> GetOccasionCalendarListAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<List<ServerStatusReportModel>> GetServerStatusAsync(ENSource source, List<ServerStatusForClient> serverStatuses, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<IEnumerable<KardexAndAccountConflictRecord>> GetAllKardexAndAccountConflictAsync(Guid moeinGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<IEnumerable<DocumentRoundMismatchRecord>> GetAllDocumentRoundMismatchAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<ServerStatusReportModel> list, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			Acc_ModelContext c = (Acc_ModelContext)(await DatabaseContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepositoryBase.cs", 178));
			IEnumerable<IGrouping<EntityPart, ServerStatusReportModel>> enumerable = from q in list
				group q by q.Type;
			foreach (IGrouping<EntityPart, ServerStatusReportModel> item in enumerable)
			{
				if (item == null)
				{
					continue;
				}
				List<Guid> guidList = item.Select((ServerStatusReportModel q) => q.Guid)?.ToList();
				if (!(guidList?.Any() ?? false))
				{
					continue;
				}
				switch (item.Key)
				{
				case EntityPart.Customer:
				{
					List<Customer> list7 = await EntityFrameworkQueryableExtensions.ToListAsync<Customer>(((IQueryable<Customer>)c.Customers).Where((Customer q) => guidList.Contains(q.Guid)), token);
					foreach (Customer item2 in list7)
					{
						item2.ServerStatus = ServerStatusForClient.None;
						item2.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list7);
					break;
				}
				case EntityPart.TafsilGroup:
				{
					List<TafsilGroup> list15 = await EntityFrameworkQueryableExtensions.ToListAsync<TafsilGroup>(((IQueryable<TafsilGroup>)c.TafsilLevel).Where((TafsilGroup q) => guidList.Contains(q.Guid)), token);
					foreach (TafsilGroup item3 in list15)
					{
						item3.ServerStatus = ServerStatusForClient.None;
						item3.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list15);
					break;
				}
				case EntityPart.Tafsil:
				{
					List<Tafsil> list19 = await EntityFrameworkQueryableExtensions.ToListAsync<Tafsil>(((IQueryable<Tafsil>)c.Tafsil).Where((Tafsil q) => guidList.Contains(q.Guid)), token);
					foreach (Tafsil item4 in list19)
					{
						item4.ServerStatus = ServerStatusForClient.None;
						item4.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list19);
					break;
				}
				case EntityPart.CustomerGroup:
				{
					List<CustomerGroup> list11 = await EntityFrameworkQueryableExtensions.ToListAsync<CustomerGroup>(((IQueryable<CustomerGroup>)c.Customers_group).Where((CustomerGroup q) => guidList.Contains(q.Guid)), token);
					foreach (CustomerGroup item5 in list11)
					{
						item5.ServerStatus = ServerStatusForClient.None;
						item5.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list11);
					break;
				}
				case EntityPart.CustomerTitle:
				{
					List<CustomerTitle> list3 = await EntityFrameworkQueryableExtensions.ToListAsync<CustomerTitle>(((IQueryable<CustomerTitle>)c.CustomersTitle).Where((CustomerTitle q) => guidList.Contains(q.Guid)), token);
					foreach (CustomerTitle item6 in list3)
					{
						item6.ServerStatus = ServerStatusForClient.None;
						item6.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list3);
					break;
				}
				case EntityPart.Product:
				{
					List<Product> list17 = await EntityFrameworkQueryableExtensions.ToListAsync<Product>(((IQueryable<Product>)c.Products).Where((Product q) => guidList.Contains(q.Guid)), token);
					foreach (Product item7 in list17)
					{
						item7.ServerStatus = ServerStatusForClient.None;
						item7.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list17);
					break;
				}
				case EntityPart.ProductGroup:
				{
					List<ProductGroup> list13 = await EntityFrameworkQueryableExtensions.ToListAsync<ProductGroup>(((IQueryable<ProductGroup>)c.Products_group).Where((ProductGroup q) => guidList.Contains(q.Guid)), token);
					foreach (ProductGroup item8 in list13)
					{
						item8.ServerStatus = ServerStatusForClient.None;
						item8.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list13);
					break;
				}
				case EntityPart.ProductVahed:
				{
					List<ProductVahed> list9 = await EntityFrameworkQueryableExtensions.ToListAsync<ProductVahed>(((IQueryable<ProductVahed>)c.Products_vahed).Where((ProductVahed q) => guidList.Contains(q.Guid)), token);
					foreach (ProductVahed item9 in list9)
					{
						item9.ServerStatus = ServerStatusForClient.None;
						item9.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list9);
					break;
				}
				case EntityPart.Order:
				{
					List<Order> list5 = await EntityFrameworkQueryableExtensions.ToListAsync<Order>(((IQueryable<Order>)c.Order).Where((Order q) => guidList.Contains(q.Guid)), token);
					foreach (Order item10 in list5)
					{
						item10.ServerStatus = ServerStatusForClient.None;
						item10.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list5);
					break;
				}
				case EntityPart.Depot:
				{
					List<Depot> list20 = await EntityFrameworkQueryableExtensions.ToListAsync<Depot>(((IQueryable<Depot>)c.Depot).Where((Depot q) => guidList.Contains(q.Guid)), token);
					foreach (Depot item11 in list20)
					{
						item11.ServerStatus = ServerStatusForClient.None;
						item11.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list20);
					break;
				}
				case EntityPart.Reception:
				{
					List<Reception> list18 = await EntityFrameworkQueryableExtensions.ToListAsync<Reception>(((IQueryable<Reception>)c.Receptions).Where((Reception q) => guidList.Contains(q.Guid)), token);
					foreach (Reception item12 in list18)
					{
						item12.ServerStatus = ServerStatusForClient.None;
						item12.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list18);
					break;
				}
				case EntityPart.Pardakht:
				{
					List<Pardakht> list16 = await EntityFrameworkQueryableExtensions.ToListAsync<Pardakht>(((IQueryable<Pardakht>)c.Pardakht).Where((Pardakht q) => guidList.Contains(q.Guid)), token);
					foreach (Pardakht item13 in list16)
					{
						item13.ServerStatus = ServerStatusForClient.None;
						item13.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list16);
					break;
				}
				case EntityPart.Moein:
				{
					List<Moein> list14 = await EntityFrameworkQueryableExtensions.ToListAsync<Moein>(((IQueryable<Moein>)c.Moein).Where((Moein q) => guidList.Contains(q.Guid)), token);
					foreach (Moein item14 in list14)
					{
						item14.ServerStatus = ServerStatusForClient.None;
						item14.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list14);
					break;
				}
				case EntityPart.Kol:
				{
					List<Kol> list12 = await EntityFrameworkQueryableExtensions.ToListAsync<Kol>(((IQueryable<Kol>)c.Kol).Where((Kol q) => guidList.Contains(q.Guid)), token);
					foreach (Kol item15 in list12)
					{
						item15.ServerStatus = ServerStatusForClient.None;
						item15.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list12);
					break;
				}
				case EntityPart.Account:
				{
					List<AccountClass> list10 = await EntityFrameworkQueryableExtensions.ToListAsync<AccountClass>(((IQueryable<AccountClass>)c.Accounts).Where((AccountClass q) => guidList.Contains(q.Guid)), token);
					foreach (AccountClass item16 in list10)
					{
						item16.ServerStatus = ServerStatusForClient.None;
						item16.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list10);
					break;
				}
				case EntityPart.CustomerRole:
				{
					List<CustomerRole> list8 = await EntityFrameworkQueryableExtensions.ToListAsync<CustomerRole>(((IQueryable<CustomerRole>)c.CustomerRole).Where((CustomerRole q) => guidList.Contains(q.Guid)), token);
					foreach (CustomerRole item17 in list8)
					{
						item17.ServerStatus = ServerStatusForClient.None;
						item17.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list8);
					break;
				}
				case EntityPart.DiscountCoupon:
				{
					List<CouponDiscount> list6 = await EntityFrameworkQueryableExtensions.ToListAsync<CouponDiscount>(((IQueryable<CouponDiscount>)c.CouponDiscount).Where((CouponDiscount q) => guidList.Contains(q.Guid)), token);
					foreach (CouponDiscount item18 in list6)
					{
						item18.ServerStatus = ServerStatusForClient.None;
						item18.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list6);
					break;
				}
				case EntityPart.Tax:
				{
					List<TaxClass> list4 = await EntityFrameworkQueryableExtensions.ToListAsync<TaxClass>(((IQueryable<TaxClass>)c.Tax).Where((TaxClass q) => guidList.Contains(q.Guid)), token);
					foreach (TaxClass item19 in list4)
					{
						item19.ServerStatus = ServerStatusForClient.None;
						item19.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list4);
					break;
				}
				case EntityPart.ProductPriceLevel:
				{
					List<ProductPriceLevel> list2 = await EntityFrameworkQueryableExtensions.ToListAsync<ProductPriceLevel>(((IQueryable<ProductPriceLevel>)c.ProductPriceLevel).Where((ProductPriceLevel q) => guidList.Contains(q.Guid)), token);
					foreach (ProductPriceLevel item20 in list2)
					{
						item20.ServerStatus = ServerStatusForClient.None;
						item20.Modified = DateTime.Now;
					}
					((DbContext)c).UpdateRange((IEnumerable<object>)list2);
					break;
				}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepositoryBase.cs", 364);
			res.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\ReportPersistenceRepositoryBase.cs", 365);
		}
		return res;
	}
}
