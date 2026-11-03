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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.DiscountCoupon;

internal class CouponDiscountEntityRelationPersistenceRepository : PersistenceNoGuidRepository<CouponDiscountEntityRelationBusiness, CouponDiscountEntityRelation>, ICouponDiscountEntityRelationRepository, INoGuidRepository<CouponDiscountEntityRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CouponDiscountEntityRelationBusiness, CouponDiscountEntityRelation>, IItemAsyncMapper<CouponDiscountEntityRelation, CouponDiscountEntityRelationBusiness>
	{
		public Task<CouponDiscountEntityRelation> MapAsync(string contextGuid, CouponDiscountEntityRelationBusiness source, CouponDiscountEntityRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CouponDiscountEntityRelation();
				}
				destination.EntityGuid = source.EntityGuid;
				destination.CouponDetailGuid = source.CouponDetailGuid;
				destination.EntityType = source.EntityType;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 141);
				throw;
			}
		}

		public Task<CouponDiscountEntityRelationBusiness> MapAsync(string contextGuid, CouponDiscountEntityRelation source, CouponDiscountEntityRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CouponDiscountEntityRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.EntityGuid = source.EntityGuid;
				destination.CouponDetailGuid = source.CouponDetailGuid;
				destination.EntityType = source.EntityType;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 165);
				throw;
			}
		}

		public Task<List<CouponDiscountEntityRelation>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountEntityRelationBusiness> source, List<CouponDiscountEntityRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CouponDiscountEntityRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountEntityRelation> source, List<CouponDiscountEntityRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CouponDiscountEntityRelation, CouponDiscountEntityRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CouponDiscountEntityRelationBusiness, CouponDiscountEntityRelation> MapToDatabase => Mapper;

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

	public CouponDiscountEntityRelationPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 39);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CouponDiscountEntityRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 48));
				CouponDiscountEntityRelation couponDiscountEntityRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(couponDiscountEntityRelation, couponDiscountEntityRelation.CouponDetailGuid, couponDiscountEntityRelation.EntityType, couponDiscountEntityRelation.EntityGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 50);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 54);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 55);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CouponDiscountEntityRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (items != null && items.Any())
				{
					foreach (CouponDiscountEntityRelationBusiness item in items)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 68);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 72);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 73);
			}
			return ret;
		}
	}

	public override Task<IEnumerable<CouponDiscountEntityRelationBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<CouponDiscountEntityRelationBusiness>> GetAllAsync(Guid couponDetailGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 80));
		IEnumerable<CouponDiscountEntityRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<CouponDiscountEntityRelation> queryable = ((IQueryable<CouponDiscountEntityRelation>)c.CouponDiscountEntityRelation).Where((CouponDiscountEntityRelation q) => q.CouponDetailGuid == couponDetailGuid);
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 91);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid couponDetailGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 106));
			IQueryable<CouponDiscountEntityRelation> queryable = ((IQueryable<CouponDiscountEntityRelation>)acc_ModelContext.CouponDiscountEntityRelation).Where((CouponDiscountEntityRelation q) => q.CouponDetailGuid == couponDetailGuid);
			if (token.IsCancellationRequested || IsDisposed || queryable == null || !queryable.Any())
			{
				return res;
			}
			acc_ModelContext.CouponDiscountEntityRelation.RemoveRange((IEnumerable<CouponDiscountEntityRelation>)queryable);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 113);
			res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountEntityRelationPersistenceRepository.cs", 114);
		}
		return res;
	}
}
