using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Customers;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Filters.Customer;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting.UserSettings;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;

namespace AccSqlServerPersistence.Persistence.Repositories;

public class CustomerCompactPersistenceRepositoryBase : PersistenceCompactRepository<CusCompactBusiness, Customer>, ICustomerCompactRepository, ICompactRepository<CusCompactBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Customer, CusCompactBusiness>, IItemMapper<Customer, CusCompactBusiness>
	{
		public async Task<Customer> MapAsync(string contextGuid, CusCompactBusiness source, Customer destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			throw new Exception("آقاجون نباید کامپکت رو به customer تبدیل کنی!!");
		}

		public CusCompactBusiness Map(string contextGuid, Customer source, CusCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			throw new Exception("آقاجون نباید کامپکت رو به customer تبدیل کنی!!");
		}

		public Task<CusCompactBusiness> MapAsync(string contextGuid, Customer source, CusCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			throw new Exception("آقاجون نباید کامپکت رو به customer تبدیل کنی!!");
		}

		public Task<List<CusCompactBusiness>> MapListAsync(string contextGuid, IEnumerable<Customer> source, List<CusCompactBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	public override IItemMapper<Customer, CusCompactBusiness> MapFromDatabase_nonAsync { get; set; } = new mapper();


	public override IItemAsyncMapper<Customer, CusCompactBusiness> MapFromDatabase { get; set; } = new mapper();


	public IItemAsyncMapper<CusCompactBusiness, Customer> MapToDatabase { get; set; }

	public CustomerCompactPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				MapFromDatabase = null;
				MapToDatabase = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 40);
		}
	}

	public Task<List<CusCompactBusiness>> GetAllAsync(string name, string family, List<Guid> lstMoeinGuid, string tranName, CancellationToken token)
	{
		return GetAllAsync(name, family, 0L, null, true, lstMoeinGuid, null, tranName, token);
	}

	public async Task<IEnumerable<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs"))
		{
			BaseModelContext c = null;
			IEnumerable<Guid> result;
			try
			{
				_ = 1;
				try
				{
					c = await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 49);
					IQueryable<Customer> queryable = ((IQueryable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, Tafsil>((IQueryable<Customer>)((DbContext)c).Set<Customer>(), (Expression<Func<Customer, Tafsil>>)((Customer p) => p.Tafsil))).Where((Customer x) => x.ServerStatus != ServerStatusForClient.Delivered && x.ServerStatus != ServerStatusForClient.DirectDelivery && x.ServerStatus != ServerStatusForClient.Sent);
					result = ((queryable != null && queryable.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<Guid>((IQueryable<Guid>)(from p in queryable
						select p.Guid into x
						orderby Guid.NewGuid()
						select x), token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 63);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual async Task<List<CusCompactBusiness>> GetAllAsync(string name, string family, long code, Guid? guid, bool? status, List<Guid> lstMoeinGuids, List<Guid> lstTafsilGroupGuids, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 75));
			Stopwatch.StartNew();
			List<CusCompactBusiness> result;
			try
			{
				_ = 1;
				try
				{
					if (lstMoeinGuids == null)
					{
						lstMoeinGuids = new List<Guid>();
					}
					IQueryable<Customer> source = ((IEnumerable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, Tafsil>((IQueryable<Customer>)((DbContext)c).Set<Customer>(), (Expression<Func<Customer, Tafsil>>)((Customer p) => p.Tafsil))).AsQueryable();
					if (status.HasValue)
					{
						source = source.Where((Customer w) => (bool?)w.Status == status);
					}
					if (!string.IsNullOrEmpty(name))
					{
						source = source.Where((Customer w) => w.Name == name);
					}
					if (!string.IsNullOrEmpty(family))
					{
						source = source.Where((Customer w) => w.Family == family);
					}
					if (code > 0)
					{
						source = source.Where((Customer w) => w.Tafsil.Code == code);
					}
					if (guid.HasValue)
					{
						source = source.Where((Customer w) => w.Guid == guid);
					}
					IEnumerable<CusCompactBusiness> enumerable = await EntityFrameworkQueryableExtensions.ToListAsync<CusCompactBusiness>(source.Select((Expression<Func<Customer, CusCompactBusiness>>)((Customer p) => new CusCompactBusinessWithEmptyConstructor
					{
						ContextGuid = ContextGuid,
						Account = (lstMoeinGuids.Any() ? p.Tafsil.Accounts.Where((AccountClass q) => lstMoeinGuids.Contains(q.MoeinGuid)).Sum((AccountClass w) => w.Account) : p.Tafsil.Accounts.Sum((AccountClass w) => w.Account)),
						Guid = p.Guid,
						Address1 = "",
						Address2 = "",
						Code = p.Tafsil.Code,
						Description = "",
						Family = p.Family,
						GroupGuid = p.GroupGuid,
						GroupName = "",
						IsBazaryab = p.IsBazaryab,
						IsSupervisor = p.IsSupervisor,
						IsEtebari = p.IsEtebari,
						Modified = p.Modified,
						Name = p.Name,
						PicFace = p.PicFace,
						PicSign = p.PicSign,
						Special = p.Tafsil.IsSpecial,
						Status = p.Status,
						ServerStatus = p.ServerStatus,
						TitleGuid = p.TitleGuid,
						VisitorGuid = p.VisitorGuid,
						Zip = p.Zip,
						TafsilGroupGuid = p.Tafsil.TafsilGroupGuid,
						SumAghsat = 0m,
						RoleGuid = p.RoleGuid,
						PhoneNumbers = string.Join(",", p.Tafsil.TafsilNumber.Select((TafsilNumber q) => q.CalcedNumber))
					})), token);
					ApplyTafsilGroupAccess(c, lstTafsilGroupGuids, enumerable);
					result = enumerable?.ToList();
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user") || (ex2.InnerException != null && ex2.InnerException.Message.Contains("Operation cancelled by user")))
					{
						throw new OperationCanceledException("sqlexception=>" + ex2.Message);
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 127);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual async Task<CusCompactBusiness> GetAsync(long code, List<Guid> lstMoeinGuids, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 140));
			CusCompactBusiness result;
			try
			{
				_ = 1;
				try
				{
					if (lstMoeinGuids == null)
					{
						lstMoeinGuids = new List<Guid>();
					}
					result = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<CusCompactBusiness>(((IEnumerable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, Tafsil>((IQueryable<Customer>)((DbContext)c).Set<Customer>(), (Expression<Func<Customer, Tafsil>>)((Customer p) => p.Tafsil))).AsQueryable().Select((Expression<Func<Customer, CusCompactBusiness>>)((Customer p) => new CusCompactBusinessWithEmptyConstructor
					{
						ContextGuid = ContextGuid,
						Account = (lstMoeinGuids.Any() ? p.Tafsil.Accounts.Where((AccountClass q) => lstMoeinGuids.Contains(q.MoeinGuid)).Sum((AccountClass w) => w.Account) : p.Tafsil.Accounts.Sum((AccountClass w) => w.Account)),
						Guid = p.Guid,
						Address1 = p.Address1,
						Address2 = p.Address2,
						Code = p.Tafsil.Code,
						Description = p.Description,
						Family = p.Family,
						GroupGuid = p.GroupGuid,
						GroupName = p.CusGroup.Name,
						IsBazaryab = p.IsBazaryab,
						IsSupervisor = p.IsSupervisor,
						IsEtebari = p.IsEtebari,
						Modified = p.Modified,
						Name = p.Name,
						PicFace = p.PicFace,
						PicSign = p.PicSign,
						Special = p.Tafsil.IsSpecial,
						Status = p.Status,
						ServerStatus = p.ServerStatus,
						TitleGuid = p.TitleGuid,
						VisitorGuid = p.VisitorGuid,
						TafsilGroupGuid = p.Tafsil.TafsilGroupGuid,
						SumAghsat = p.Tafsil.SumAghsat,
						RoleGuid = p.RoleGuid,
						Zip = p.Zip
					})), (Expression<Func<CusCompactBusiness, bool>>)((CusCompactBusiness p) => p.Code == code), token);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user"))
					{
						throw new OperationCanceledException("sqlexception=>" + ex2.Message);
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 179);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> CountWithThisTitleAsync(Guid titleGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("CountWithThisTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CountWithThisTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 191));
			int result;
			try
			{
				result = ((IQueryable<Customer>)acc_ModelContext.Customers).Count((Customer x) => x.TitleGuid == titleGuid && x.Status);
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
				WebErrorLog.Instance.StartLog(ex2, "", "CountWithThisTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 200);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("CountWithThisTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task UpdateAccountAsync(Guid guid, decimal account, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task UpdateSumAghsatAsync(Guid guid, decimal account, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<CusCompactBusiness>> GetAllAsync(List<Guid> moeinGuids, bool? status, CancellationToken token, string tranName)
	{
		return GetAllAsync(null, null, 0L, null, status, moeinGuids, null, tranName, token);
	}

	public override Task<List<CusCompactBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		return GetAllAsync(null, null, 0L, null, status, null, null, tranName, token);
	}

	public override Task<CusCompactBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, null, token, tranName);
	}

	public virtual async Task<CusCompactBusiness> GetAsync(Guid guid, List<Guid> lstMoeinGuids, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 227));
			CusCompactBusiness result;
			try
			{
				_ = 1;
				try
				{
					if (lstMoeinGuids == null)
					{
						lstMoeinGuids = new List<Guid>();
					}
					result = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<CusCompactBusinessWithEmptyConstructor>(from p in ((IEnumerable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, Tafsil>((IQueryable<Customer>)((DbContext)c).Set<Customer>(), (Expression<Func<Customer, Tafsil>>)((Customer p) => p.Tafsil))).AsQueryable()
						select new CusCompactBusinessWithEmptyConstructor
						{
							ContextGuid = ContextGuid,
							Account = (lstMoeinGuids.Any() ? p.Tafsil.Accounts.Where((AccountClass q) => lstMoeinGuids.Contains(q.MoeinGuid)).Sum((AccountClass w) => w.Account) : p.Tafsil.Accounts.Sum((AccountClass w) => w.Account)),
							Guid = p.Guid,
							Address1 = p.Address1,
							Address2 = p.Address2,
							Code = p.Tafsil.Code,
							Description = p.Description,
							Family = p.Family,
							GroupGuid = p.GroupGuid,
							GroupName = p.CusGroup.Name,
							IsBazaryab = p.IsBazaryab,
							IsSupervisor = p.IsSupervisor,
							IsEtebari = p.IsEtebari,
							Modified = p.Modified,
							Name = p.Name,
							PicFace = p.PicFace,
							PicSign = p.PicSign,
							Special = p.Tafsil.IsSpecial,
							Status = p.Status,
							ServerStatus = p.ServerStatus,
							TitleGuid = p.TitleGuid,
							VisitorGuid = p.VisitorGuid,
							TafsilGroupGuid = p.Tafsil.TafsilGroupGuid,
							SumAghsat = p.Tafsil.SumAghsat,
							RoleGuid = p.RoleGuid,
							Zip = p.Zip
						}, (Expression<Func<CusCompactBusinessWithEmptyConstructor, bool>>)((CusCompactBusinessWithEmptyConstructor p) => p.Guid == guid), token);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user"))
					{
						throw new OperationCanceledException("sqlexception=>" + ex2.Message);
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 266);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override CusCompactBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		return Get(guid, null, token, tranName);
	}

	public virtual CusCompactBusiness Get(Guid guid, List<Guid> lstMoeinGuids, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)dbContext.GetContext(tranName, 60000, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 279);
			try
			{
				if (lstMoeinGuids == null)
				{
					lstMoeinGuids = new List<Guid>();
				}
				return (from p in ((IEnumerable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, Tafsil>((IQueryable<Customer>)((DbContext)acc_ModelContext).Set<Customer>(), (Expression<Func<Customer, Tafsil>>)((Customer p) => p.Tafsil))).AsQueryable()
					select new CusCompactBusinessWithEmptyConstructor
					{
						ContextGuid = ContextGuid,
						Account = (lstMoeinGuids.Any() ? p.Tafsil.Accounts.Where((AccountClass q) => lstMoeinGuids.Contains(q.MoeinGuid)).Sum((AccountClass w) => w.Account) : p.Tafsil.Accounts.Sum((AccountClass w) => w.Account)),
						Guid = p.Guid,
						Address1 = p.Address1,
						Address2 = p.Address2,
						Code = p.Tafsil.Code,
						Description = p.Description,
						Family = p.Family,
						GroupGuid = p.GroupGuid,
						GroupName = p.CusGroup.Name,
						IsBazaryab = p.IsBazaryab,
						IsSupervisor = p.IsSupervisor,
						IsEtebari = p.IsEtebari,
						Modified = p.Modified,
						Name = p.Name,
						PicFace = p.PicFace,
						PicSign = p.PicSign,
						Special = p.Tafsil.IsSpecial,
						Status = p.Status,
						ServerStatus = p.ServerStatus,
						TitleGuid = p.TitleGuid,
						VisitorGuid = p.VisitorGuid,
						TafsilGroupGuid = p.Tafsil.TafsilGroupGuid,
						SumAghsat = p.Tafsil.SumAghsat,
						RoleGuid = p.RoleGuid,
						Zip = p.Zip
					}).SingleOrDefault((CusCompactBusinessWithEmptyConstructor p) => p.Guid == guid);
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
				WebErrorLog.Instance.StartLog(ex2, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 318);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					acc_ModelContext.Dispose("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs");
				}
			}
		}
	}

	public virtual async Task<IEnumerable<CusCompactBusiness>> GetAllAsync(CustomerCompactFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 329));
		Stopwatch.StartNew();
		IEnumerable<CusCompactBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 3;
			try
			{
				if (filter != null)
				{
					CustomerCompactFilter customerCompactFilter = filter;
					if (customerCompactFilter.MoeinGuidList == null)
					{
						customerCompactFilter.MoeinGuidList = new List<Guid>();
					}
				}
				IQueryable<Customer> temp = ((IEnumerable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, Tafsil>((IQueryable<Customer>)((DbContext)c).Set<Customer>(), (Expression<Func<Customer, Tafsil>>)((Customer p) => p.Tafsil))).AsQueryable();
				UserCustomerSettings set = (await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid, token))?.Settings?.CustomerS;
				if (filter == null)
				{
					goto IL_1ef0;
				}
				if (filter.Status.HasValue)
				{
					temp = temp?.Where((Customer q) => (bool?)q.Status == filter.Status);
				}
				foreach (string item in filter.SearchString?.FixString()?.RemoveCharactersForLinkSearch()?.SplitString()?.Distinct() ?? new List<string>())
				{
					long itemLong = item.ParseToLong(0L);
					if (!string.IsNullOrEmpty(item?.Trim()) && set != null)
					{
						temp = temp?.Where((Customer x) => (((int)filter.SearchOption == 0 || (int)filter.SearchOption == 1 || (int)filter.SearchOption == 6) && set.SrchByCode && DbFunctionsExtensions.Like(EF.Functions, x.Tafsil.Code.ToString(), $"%{item}%")) || (((int)filter.SearchOption == 0 || (int)filter.SearchOption == 6) && set.SrchByName && set.SrchByName && DbFunctionsExtensions.Like(EF.Functions, x.Name, $"%{item}%")) || (((int)filter.SearchOption == 0 || (int)filter.SearchOption == 2 || (int)filter.SearchOption == 6) && set.SrchByFamily && set.SrchByFamily && DbFunctionsExtensions.Like(EF.Functions, x.Family, $"%{item}%")) || ((int)filter.SearchOption == 0 && ((int)filter.SearchOption == 0 || (int)filter.SearchOption == 5) && set.SrchByAdd1 && DbFunctionsExtensions.Like(EF.Functions, x.Address1, $"%{item}%")) || (((int)filter.SearchOption == 0 || (int)filter.SearchOption == 5) && set.SrchByAdd2 && DbFunctionsExtensions.Like(EF.Functions, x.Address2, $"%{item}%")) || ((int)filter.SearchOption == 0 && set.SrchByZip && DbFunctionsExtensions.Like(EF.Functions, x.Zip, $"%{item}%")) || ((int)filter.SearchOption == 0 && set.SrchByDecription && DbFunctionsExtensions.Like(EF.Functions, x.Description, $"%{item}%")) || (((int)filter.SearchOption == 0 || (int)filter.SearchOption == 3 || (int)filter.SearchOption == 4) && set.SrchByTelMob && (DbFunctionsExtensions.Like(EF.Functions, x.PhoneNumbers, $"%{item}%") || (itemLong > 0 && DbFunctionsExtensions.Like(EF.Functions, x.PhoneNumbers, $"%{itemLong}%")))));
					}
				}
				if (!token.IsCancellationRequested)
				{
					if (filter.GroupGuid.HasValue && filter.GroupGuid != Guid.Empty)
					{
						temp = temp?.Where((Customer q) => q.GroupGuid == filter.GroupGuid);
					}
					if (filter.SelectedCustomers?.Any() ?? false)
					{
						List<Guid> cusList = new List<Guid>();
						foreach (TafsilSelectedBusiness selectedCustomer in filter.SelectedCustomers)
						{
							cusList.AddRange(selectedCustomer.DetList.Select((TafsilSelectedDetListBusiness q) => q.HGuid));
						}
						if (cusList?.Any() ?? false)
						{
							temp = temp?.Where((Customer q) => cusList.Contains(q.Guid));
						}
					}
					if (!string.IsNullOrEmpty(filter.UserName) && !string.IsNullOrEmpty(filter.Password))
					{
						temp = temp?.Where((Customer q) => q.User != null && q.User == filter.UserName && q.Password != null && q.Password == filter.Password);
					}
					if ((filter?.AreaGuidList?.Any()).GetValueOrDefault())
					{
						temp = temp?.Where((Customer q) => (q.AreaGuid ?? Guid.Empty) != Guid.Empty && filter.AreaGuidList.Contains(q.AreaGuid.Value));
					}
					if ((filter?.RoleGuid ?? Guid.Empty) != Guid.Empty)
					{
						temp = temp?.Where((Customer q) => (q.RoleGuid ?? Guid.Empty) != Guid.Empty && q.RoleGuid.Value == filter.RoleGuid.Value);
					}
					if ((filter?.Type ?? EnPersonType.None) != EnPersonType.None)
					{
						temp = filter.Type switch
						{
							EnPersonType.Visitor => temp?.Where((Customer q) => q.IsBazaryab), 
							EnPersonType.Supervisor => temp?.Where((Customer q) => q.IsSupervisor), 
							_ => throw new NotImplementedException(), 
						};
					}
					goto IL_1ef0;
				}
				result = null;
				goto end_IL_0117;
				IL_1ef0:
				if (token.IsCancellationRequested)
				{
					result = null;
				}
				else
				{
					IEnumerable<CusCompactBusiness> enumerable = await EntityFrameworkQueryableExtensions.ToListAsync<CusCompactBusiness>(temp.Select((Expression<Func<Customer, CusCompactBusiness>>)((Customer p) => new CusCompactBusinessWithEmptyConstructor
					{
						ContextGuid = ContextGuid,
						Account = (filter.MoeinGuidList.Any() ? p.Tafsil.Accounts.Where((AccountClass q) => filter.MoeinGuidList.Contains(q.MoeinGuid)).Sum((AccountClass w) => w.Account) : p.Tafsil.Accounts.Sum((AccountClass w) => w.Account)),
						Guid = p.Guid,
						Address1 = p.Address1,
						Address2 = p.Address2,
						Code = p.Tafsil.Code,
						Description = p.Description,
						Family = p.Family,
						GroupGuid = p.GroupGuid,
						GroupName = p.CusGroup.Name,
						IsBazaryab = p.IsBazaryab,
						IsSupervisor = p.IsSupervisor,
						IsEtebari = p.IsEtebari,
						Modified = p.Modified,
						Name = p.Name,
						PicFace = p.PicFace,
						PicSign = p.PicSign,
						Special = p.Tafsil.IsSpecial,
						Status = p.Status,
						ServerStatus = p.ServerStatus,
						TitleGuid = p.TitleGuid,
						VisitorGuid = p.VisitorGuid,
						Zip = p.Zip,
						TafsilGroupGuid = p.Tafsil.TafsilGroupGuid,
						SumAghsat = p.Tafsil.SumAghsat,
						RoleGuid = p.RoleGuid
					})), token);
					List<CusCompactBusiness> lst = null;
					if (filter.Debit)
					{
						if (lst == null)
						{
							lst = new List<CusCompactBusiness>();
						}
						lst.AddRange(enumerable.Where((CusCompactBusiness q) => q.Account < 0m));
					}
					if (filter.Credit)
					{
						if (lst == null)
						{
							lst = new List<CusCompactBusiness>();
						}
						lst.AddRange(enumerable.Where((CusCompactBusiness q) => q.Account > 0m));
					}
					if (filter.Countless)
					{
						if (lst == null)
						{
							lst = new List<CusCompactBusiness>();
						}
						lst.AddRange(enumerable.Where((CusCompactBusiness q) => q.Account == 0m));
					}
					if (lst == null)
					{
						lst = enumerable?.ToList();
					}
					foreach (string item2 in filter.SearchString?.SplitString()?.Distinct() ?? new List<string>())
					{
						if (item2 == null || item2.ParseToLong(0L) <= 9999999)
						{
							continue;
						}
						if (token.IsCancellationRequested)
						{
							result = null;
						}
						else
						{
							CusCardBusiness cusCardBusiness = await CusCardBusiness.GetAsync(base.ContextGuid, item2, token, tranName);
							if (token.IsCancellationRequested)
							{
								result = null;
							}
							else
							{
								if (cusCardBusiness != null)
								{
									List<CusCompactBusiness> list = lst;
									list.Add(await GetAsync(cusCardBusiness.CusGuid, token, tranName));
								}
								if (!token.IsCancellationRequested)
								{
									continue;
								}
								result = null;
							}
						}
						goto end_IL_0117;
					}
					ApplyTafsilGroupAccess(c, filter?.TafsilGroupGuidList, lst);
					if (set != null)
					{
						switch (set.CustomerSort)
						{
						case CustomerSort.NameAsc:
							lst = lst?.OrderBy((CusCompactBusiness o) => o.Family)?.ThenBy((CusCompactBusiness o) => o.Name)?.ToList();
							break;
						case CustomerSort.NameDesc:
							lst = lst?.OrderByDescending((CusCompactBusiness o) => o.Family)?.ThenBy((CusCompactBusiness o) => o.Name)?.ToList();
							break;
						case CustomerSort.CodeAsc:
							lst = lst?.OrderBy((CusCompactBusiness o) => o.Code)?.ToList();
							break;
						case CustomerSort.CodeDesc:
							lst = lst?.OrderByDescending((CusCompactBusiness o) => o.Code)?.ToList();
							break;
						case CustomerSort.BalanceAsc:
							lst = lst?.OrderBy((CusCompactBusiness o) => o.Account)?.ToList();
							break;
						case CustomerSort.BalanceDesc:
							lst = lst?.OrderByDescending((CusCompactBusiness o) => o.Account)?.ToList();
							break;
						}
					}
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						if ((filter?.TakeCount).GetValueOrDefault() != 0)
						{
							lst = lst?.Take(filter.TakeCount.Value)?.ToList();
						}
						result = lst?.ToList();
					}
				}
				end_IL_0117:;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 491);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<CusCompactBusiness>> GetAllForSendToServerSniftsAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllForSendToServerSniftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllForSendToServerSniftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 505));
			IEnumerable<CusCompactBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Customer> queryable = ((IQueryable<Customer>)c.Customers)?.Where((Customer q) => q.SendToServerDate == null || q.SendToServerDate < q.Modified)?.OrderBy((Customer q) => Guid.NewGuid())?.Take(50);
					result = ((queryable != null && queryable.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<CusCompactBusiness>(queryable.Select((Expression<Func<Customer, CusCompactBusiness>>)((Customer p) => new CusCompactBusinessWithEmptyConstructor
					{
						ContextGuid = ContextGuid,
						Guid = p.Guid,
						Address1 = p.Address1,
						Address2 = p.Address2,
						Code = p.Tafsil.Code,
						Description = p.Description,
						Family = p.Family,
						GroupGuid = p.GroupGuid,
						GroupName = p.CusGroup.Name,
						IsBazaryab = p.IsBazaryab,
						IsSupervisor = p.IsSupervisor,
						IsEtebari = p.IsEtebari,
						Modified = p.Modified,
						Name = p.Name,
						PicFace = p.PicFace,
						PicSign = p.PicSign,
						Special = p.Tafsil.IsSpecial,
						Status = p.Status,
						ServerStatus = p.ServerStatus,
						TitleGuid = p.TitleGuid,
						VisitorGuid = p.VisitorGuid,
						Zip = p.Zip,
						TafsilGroupGuid = p.Tafsil.TafsilGroupGuid,
						SumAghsat = p.Tafsil.SumAghsat,
						RoleGuid = p.RoleGuid
					})), token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllForSendToServerSniftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 543);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllForSendToServerSniftsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	protected void ApplyTafsilGroupAccess(Acc_ModelContext c, List<Guid> tafsilGroupGuidList, IEnumerable<CusCompactBusiness> lst)
	{
		try
		{
			if (tafsilGroupGuidList == null)
			{
				return;
			}
			List<Guid> list = tafsilGroupGuidList;
			if (list == null || !list.Any())
			{
				if (lst == null || !lst.Any())
				{
					return;
				}
				{
					foreach (CusCompactBusiness item in lst)
					{
						item.Account = 0m;
					}
					return;
				}
			}
			tafsilGroupGuidList = tafsilGroupGuidList?.Where((Guid q) => q != Guid.Empty)?.ToList();
			List<Guid> list2 = tafsilGroupGuidList;
			if (list2 == null || !list2.Any())
			{
				return;
			}
			List<Guid> validGuids = ((IQueryable<Tafsil>)c.Tafsil).Where((Tafsil q) => tafsilGroupGuidList.Contains(q.TafsilGroupGuid))?.Select((Tafsil q) => q.Guid)?.ToList();
			List<Guid> list3 = validGuids;
			if (list3 == null || !list3.Any())
			{
				return;
			}
			IEnumerable<CusCompactBusiness> enumerable = lst?.Where((CusCompactBusiness q) => !validGuids.Contains(q.Guid));
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (CusCompactBusiness item2 in enumerable)
			{
				item2.Account = 0m;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyTafsilGroupAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerCompactPersistenceRepository.cs", 578);
			throw;
		}
	}
}
