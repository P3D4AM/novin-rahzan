using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.DiscountCoupon.Reports;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.DiscountCoupon;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Accounting.DiscountCoupon;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.CouponDiscount;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.DiscountCoupon;

internal abstract class CouponDiscountPersistenceRepositoryBase : PersistenceSyncRepository<CouponDiscountBusiness, CouponDiscount>, ICouponDiscountRepository, ISyncRepository<CouponDiscountBusiness>, IEditableDelivery, IRepository<CouponDiscountBusiness>, INoGuidRepository<CouponDiscountBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CouponDiscount, CouponDiscountBusiness>, IItemAsyncMapper<CouponDiscountBusiness, CouponDiscount>
	{
		public Task<CouponDiscountBusiness> MapAsync(string contextGuid, CouponDiscount source, CouponDiscountBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CouponDiscountBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.DateFrom = source.DateFrom;
				destination.DateTo = source.DateTo;
				destination.SingleUse = source.SingleUse;
				destination.Status = source.Status;
				destination.CountCouponUse = source.CountCouponUse;
				destination.CountCouponUseForUser = source.CountCouponUseForUser;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.CashierGuid = source.CashierGuid;
				destination.Type = source.Type;
				destination.Mode = source.Mode;
				destination.DeadLineForUse = source.DeadLineForUse;
				destination.IsImmediateUseAllowed = source.IsImmediateUseAllowed;
				destination.Version = source.Version;
				destination.UsedCount = source.UsedCount;
				destination.MetaData = source.MetaData.FromJson<CouponDiscountMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 229);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 236);
				throw;
			}
		}

		public async Task<CouponDiscount> MapAsync(string contextGuid, CouponDiscountBusiness source, CouponDiscount destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				if (destination == null)
				{
					destination = new CouponDiscount();
				}
				destination.Guid = source.Guid;
				destination.DateFrom = source.DateFrom;
				destination.DateTo = source.DateTo;
				destination.SingleUse = source.SingleUse;
				destination.Status = source.Status;
				destination.CountCouponUse = source.CountCouponUse;
				destination.CountCouponUseForUser = source.CountCouponUseForUser;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.CashierGuid = source.CashierGuid;
				destination.Type = source.Type;
				destination.Mode = source.Mode;
				destination.DeadLineForUse = source.DeadLineForUse;
				destination.IsImmediateUseAllowed = source.IsImmediateUseAllowed;
				destination.Version = source.Version;
				CouponDiscount couponDiscount = destination;
				couponDiscount.UsedCount = await source.GetUsedCountAsync(token, tranName);
				destination.MetaData = Json.ToStringJson(source.MetaData);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 278);
				throw;
			}
		}

		public Task<List<CouponDiscountBusiness>> MapListAsync(string contextGuid, IEnumerable<CouponDiscount> source, List<CouponDiscountBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CouponDiscount>> MapListAsync(string contextGuid, IEnumerable<CouponDiscountBusiness> source, List<CouponDiscount> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private CouponDiscountDetailPersistenceRepository _detRep;

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<CouponDiscount, CouponDiscountBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<CouponDiscount, CouponDiscountBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CouponDiscountBusiness, CouponDiscount> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public CouponDiscountPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
		_detRep = new CouponDiscountDetailPersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				_detRep?.Dispose();
				_detRep = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 52);
		}
	}

	public override async Task<CouponDiscountBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 56));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs"))
		{
			CouponDiscountBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					CouponDiscount couponDiscount = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<CouponDiscount>((IQueryable<CouponDiscount>)c.CouponDiscount, (Expression<Func<CouponDiscount, bool>>)((CouponDiscount x) => x.Guid == guid), token);
					if (couponDiscount == null)
					{
						result = null;
					}
					else
					{
						CouponDiscountBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, couponDiscount, null, tranName, token);
						ret.ChangeStatus = EntityChangeSt.Edited;
						await _detRep.GetAllAsync(ret, token, tranName);
						result = ret;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 72);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public virtual Task<IEnumerable<CouponDiscountReportBusiness>> GetAllAsync(CouponFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CouponDiscountBusiness coupon, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs"))
		{
			_ = 4;
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 89));
				if (coupon.Guid == Guid.Empty)
				{
					Guid.NewGuid();
				}
				CouponDiscount couponDiscount = await MapToDatabase.MapAsync(ContextGuid, coupon, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(couponDiscount, couponDiscount.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 92);
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRep.RemoveAsync(coupon.Guid, 0, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 94);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRep.SaveAsync(coupon?.CouponDetailList.ToList(), coupon.Guid, 0, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 95);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 99);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 100);
			}
			return res;
		}
	}

	public virtual Task<IEnumerable<Guid>> GetAllOrderRelationAsync(Guid tafsilGuid, DateTime orderDate, decimal orderSum, int refId, Guid? specificCouponGuid, long mobile, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<CouponDiscountReportBusiness> GetAsync(string title, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 107));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs"))
		{
			CouponDiscountReportBusiness result;
			try
			{
				_ = 1;
				try
				{
					CouponDiscount couponDiscount = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CouponDiscount>((IQueryable<CouponDiscount>)EntityFrameworkQueryableExtensions.Include<CouponDiscount, Cashier>((IQueryable<CouponDiscount>)c.CouponDiscount, (Expression<Func<CouponDiscount, Cashier>>)((CouponDiscount q) => q.Cashier)), (Expression<Func<CouponDiscount, bool>>)((CouponDiscount q) => q.Title == title), default(CancellationToken));
					result = ((couponDiscount != null) ? new CouponDiscountReportBusiness
					{
						CountCouponUse = couponDiscount.CountCouponUse,
						DateFrom = couponDiscount.DateFrom,
						DateTo = couponDiscount.DateTo,
						CountCouponUseForUser = couponDiscount.CountCouponUseForUser,
						Title = couponDiscount.Title,
						Status = couponDiscount.Status,
						Guid = couponDiscount.Guid,
						UserName = couponDiscount.Cashier.Name,
						ServerStatus = couponDiscount.ServerStatus,
						Mode = couponDiscount.Mode
					} : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 131);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public virtual Task<IEnumerable<Guid>> GetAllForScheduleAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<int> GetUsedCountAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllUagesAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<Guid> couponGuids, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!(couponGuids?.Any() ?? false))
				{
					return res;
				}
				ReturnedSaveFuncInfoWithValue<int> refIf = await StimulQueryBusiness.SaveAsync(ContextGuid, couponGuids, token);
				res.AddReturnedValue(refIf, ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 152);
				BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 154);
				string text = "Update Acc_CouponDiscount Set ServerStatus = {0} , modified=getdate() from Acc_CouponDiscount inner join StimulQuery on StimulQuery.Guid=Acc_CouponDiscount.Guid Where ServerStatus<>{0} And StimulQuery.RefrencedId={1}";
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)obj).Database, text, (IEnumerable<object>)new List<object>
				{
					(short)status,
					refIf.value
				}, token);
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 158);
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 162);
				res.AddReturnedValue(ex2, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 163);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateServerStatusByGiftGuidAsync(Guid giftGuid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (giftGuid == Guid.Empty)
				{
					return res;
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 174));
				List<CouponDiscountDetail> list = await EntityFrameworkQueryableExtensions.ToListAsync<CouponDiscountDetail>(((IQueryable<CouponDiscountDetail>)dbcontext.CouponDiscountDetail).Where((CouponDiscountDetail q) => q.GiftGuid == giftGuid), token);
				if (list == null || !list.Any())
				{
					return res;
				}
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(ContextGuid, list.Select((CouponDiscountDetail q) => q.MasterGuid), token);
				res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 180);
				string text = "Update Acc_CouponDiscount Set ServerStatus = {0} , modified=getdate() from Acc_CouponDiscount inner join StimulQuery on StimulQuery.Guid=Acc_CouponDiscount.Guid Where ServerStatus<>{0} And StimulQuery.RefrencedId={1}";
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)dbcontext).Database, text, (IEnumerable<object>)new List<object>
				{
					(short)status,
					returnedSaveFuncInfoWithValue.value
				}, token);
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 185);
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 189);
				res.AddReturnedValue(ex2, "", "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DiscountCoupon\\CouponDiscountPersistenceRepositoryBase.cs", 190);
			}
			return res;
		}
	}
}
