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

internal class CouponDiscountDetailPersistenceRepository : PersistenceMasterRepository<CouponDiscountDetailBusiness, CouponDiscountDetail>, ICouponDiscountDetailRepository, IPersistenceMasterRepository<CouponDiscountDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CouponDiscountDetailBusiness, CouponDiscountDetail>, IItemAsyncMapper<CouponDiscountDetail, CouponDiscountDetailBusiness>
	{
		public Task<CouponDiscountDetail> MapAsync(string contextGuid, CouponDiscountDetailBusiness source, CouponDiscountDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CouponDiscountDetail();
				}
				destination.Guid = source.Guid;
				destination.AmountCoupon = source.AmountCoupon;
				destination.MinBuy = source.MinBuy;
				destination.MaxBuy = source.MaxBuy;
				destination.MaxDiscountPrice = source.MaxDiscountPrice;
				destination.EnDiscountType = source.EnDiscountType;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.MasterGuid = source.MasterGuid;
				destination.GiftGuid = source.GiftGuid;
				destination.CustomerContains = source.CustomerContains;
				destination.ProductContains = source.ProductContains;
				destination.GardeshAffectiveDays = source.GardeshAffectiveDays;
				destination.GardeshStartPrice = source.GardeshStartPrice;
				destination.GardeshEndPrice = source.GardeshEndPrice;
				destination.MinBuyQuantity = source.MinBuyQuantity;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 166);
				throw;
			}
		}

		public Task<CouponDiscountDetailBusiness> MapAsync(string contextGuid, CouponDiscountDetail source, CouponDiscountDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			if (destination == null)
			{
				throw new NullReferenceException("Please New kon destination re");
			}
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.AmountCoupon = source.AmountCoupon;
				destination.MinBuy = source.MinBuy;
				destination.MaxBuy = source.MaxBuy;
				destination.MaxDiscountPrice = source.MaxDiscountPrice;
				destination.EnDiscountType = source.EnDiscountType;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.MasterGuid = source.MasterGuid;
				destination.GiftGuid = source.GiftGuid;
				destination.CustomerContains = source.CustomerContains;
				destination.ProductContains = source.ProductContains;
				destination.GardeshAffectiveDays = source.GardeshAffectiveDays;
				destination.GardeshStartPrice = source.GardeshStartPrice;
				destination.GardeshEndPrice = source.GardeshEndPrice;
				destination.MinBuyQuantity = source.MinBuyQuantity;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 203);
				throw;
			}
		}

		public Task<List<CouponDiscountDetail>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountDetailBusiness> source, List<CouponDiscountDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CouponDiscountDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountDetail> source, List<CouponDiscountDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private CouponDiscountEntityRelationPersistenceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CouponDiscountDetail, CouponDiscountDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CouponDiscountDetailBusiness, CouponDiscountDetail> MapToDatabase => Mapper;

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

	public CouponDiscountDetailPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new CouponDiscountEntityRelationPersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				_detRepo?.Dispose();
				_detRepo = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 44);
		}
	}

	public override Task<List<CouponDiscountDetailBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use another overLoad");
	}

	public async Task GetAllAsync(CouponDiscountBusiness master, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 51));
			try
			{
				int num;
				_ = num - 1;
				_ = 4;
				try
				{
					IQueryable<CouponDiscountDetail> queryable = ((IQueryable<CouponDiscountDetail>)c.CouponDiscountDetail).Where((CouponDiscountDetail x) => x.MasterGuid == master.Guid);
					if (queryable == null)
					{
						return;
					}
					foreach (CouponDiscountDetail det in queryable)
					{
						IItemAsyncMapper<CouponDiscountDetail, CouponDiscountDetailBusiness> mapFromDatabase = MapFromDatabase;
						string guid = base.Context.Guid;
						CouponDiscountDetail source = det;
						CouponDiscountDetailBusiness detmodel = await mapFromDatabase.MapAsync(guid, source, await CouponDiscountDetailBusiness.CouponDetailFactoryAsync(base.ContextGuid, master, token, tranName), tranName, token);
						IEnumerable<CouponDiscountEntityRelationBusiness> enumerable = await _detRepo.GetAllAsync(det.Guid, token, tranName);
						if (enumerable != null && enumerable.Any())
						{
							foreach (CouponDiscountEntityRelationBusiness item in enumerable)
							{
								await detmodel.AddEntityAsync(item, token);
							}
						}
						await master.AddCouponDetailListAsync(detmodel);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 73);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs");
				}
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<CouponDiscountDetailBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (items?.Any((CouponDiscountDetailBusiness p) => p.Guid == Guid.Empty) ?? false)
			{
				throw new ArgumentException("one or more items in list contains empty value for field Guid");
			}
			try
			{
				if (!(items?.Any() ?? false))
				{
					return res;
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 91));
				IQueryable<Guid> queryable = (((IQueryable<CouponDiscountDetail>)dbcontext.CouponDiscountDetail).Where((CouponDiscountDetail q) => q.MasterGuid == masterGuid)?.Select((CouponDiscountDetail q) => q.Guid))?.Where((Guid q) => !items.Select((CouponDiscountDetailBusiness q) => q.Guid).Contains(q));
				if (queryable != null && queryable.Any())
				{
					foreach (Guid item2 in queryable)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveRangeAsync(item2, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 98);
					}
				}
				if (res.HasError)
				{
					return res;
				}
				foreach (CouponDiscountDetailBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveRangeAsync(item.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 104);
					if (res.HasError)
					{
						return res;
					}
					item.MasterGuid = masterGuid;
					CouponDiscountDetail couponDiscountDetail = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(couponDiscountDetail, couponDiscountDetail.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 109);
					if (res.HasError)
					{
						return res;
					}
					List<CouponDiscountEntityRelationBusiness> list = new List<CouponDiscountEntityRelationBusiness>();
					if ((item?.ProductGroupList?.Any()).GetValueOrDefault())
					{
						list.AddRange(item.ProductGroupList);
					}
					if ((item?.ProductList?.Any()).GetValueOrDefault())
					{
						list.AddRange(item.ProductList);
					}
					if ((item?.CustomerGroupList?.Any()).GetValueOrDefault())
					{
						list.AddRange(item.CustomerGroupList);
					}
					if ((item?.CustomerList?.Any()).GetValueOrDefault())
					{
						list.AddRange(item.CustomerList);
					}
					if (list != null && list.Any())
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveRangeAsync(list, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 119);
					}
					if (res.HasError)
					{
						return res;
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 126);
				res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountDetailPersistenceRepository.cs", 127);
			}
			return res;
		}
	}
}
