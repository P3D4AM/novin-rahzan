using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.DiscountCoupon;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.DiscountCoupon;

internal class CouponDiscountUsagesPersistenceRepository : PersistenceNoGuidRepository<CouponDiscountUsagesBusiness, CouponDiscountUsages>, ICouponDiscountUsagesRepository, INoGuidRepository<CouponDiscountUsagesBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CouponDiscountUsagesBusiness, CouponDiscountUsages>, IItemAsyncMapper<CouponDiscountUsages, CouponDiscountUsagesBusiness>
	{
		public Task<CouponDiscountUsages> MapAsync(string contextGuid, CouponDiscountUsagesBusiness source, CouponDiscountUsages destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CouponDiscountUsages();
				}
				destination.TafsilGuid = source.TafsilGuid;
				destination.CouponGuid = source.CouponGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 124);
				throw;
			}
		}

		public Task<CouponDiscountUsagesBusiness> MapAsync(string contextGuid, CouponDiscountUsages source, CouponDiscountUsagesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CouponDiscountUsagesBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.CouponGuid = source.CouponGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 147);
				throw;
			}
		}

		public Task<List<CouponDiscountUsages>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountUsagesBusiness> source, List<CouponDiscountUsages> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CouponDiscountUsagesBusiness>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountUsages> source, List<CouponDiscountUsagesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CouponDiscountUsages, CouponDiscountUsagesBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CouponDiscountUsagesBusiness, CouponDiscountUsages> MapToDatabase => Mapper;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public CouponDiscountUsagesPersistenceRepository(ContextHolder db)
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
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 40);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(CouponDiscountUsagesBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CouponDiscountUsagesBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!(items?.Any() ?? false))
				{
					return res;
				}
				if (((items?.GroupBy((CouponDiscountUsagesBusiness q) => q.CouponGuid))?.Count() ?? 0) > 1)
				{
					throw new ArgumentException("CouponDiscountUsages for Another Coupon");
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 54));
				IEnumerable<CouponDiscountUsagesBusiness> enumerable = await GetAllAsync(items.First().CouponGuid, token, tranName);
				if (enumerable != null && enumerable.Any())
				{
					List<Guid> source = enumerable.Select((CouponDiscountUsagesBusiness q) => q.TafsilGuid)?.ToList();
					List<Guid> detailGuids = items?.Select((CouponDiscountUsagesBusiness q) => q.TafsilGuid)?.ToList() ?? new List<Guid>();
					IEnumerable<Guid> enumerable2 = source.Where((Guid q) => !detailGuids.Contains(q));
					if (enumerable2 != null && enumerable2.Any())
					{
						foreach (Guid item in enumerable2)
						{
							((DbContext)dbcontext).Remove((object)item);
						}
					}
				}
				foreach (CouponDiscountUsagesBusiness item2 in items)
				{
					if (item2 != null)
					{
						CouponDiscountUsages couponDiscountUsages = await MapToDatabase.MapAsync(base.ContextGuid, item2, null, tranName, token);
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(couponDiscountUsages, couponDiscountUsages.CouponGuid, couponDiscountUsages.TafsilGuid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 72);
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 78);
				res.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 79);
			}
			return res;
		}
	}

	public async Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllAsync(Guid couponGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 85));
		IEnumerable<CouponDiscountUsagesBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<CouponDiscountUsages> list = await EntityFrameworkQueryableExtensions.ToListAsync<CouponDiscountUsages>(((IQueryable<CouponDiscountUsages>)c.CouponDiscountUsages).Where((CouponDiscountUsages q) => q.CouponGuid == couponGuid), token);
				result = ((list != null && list.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, list, null, tranName, token)) : null);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs", 94);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountUsagesPersistenceRepository.cs");
			}
		}
		return result;
	}
}
