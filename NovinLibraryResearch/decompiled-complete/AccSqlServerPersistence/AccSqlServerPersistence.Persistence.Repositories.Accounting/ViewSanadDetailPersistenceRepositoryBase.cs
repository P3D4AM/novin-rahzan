using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using PacketParser.Records.Sanad;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal abstract class ViewSanadDetailPersistenceRepositoryBase : PersistenceSyncRepository<ViewSanadDetailBusiness, ViewSanadDetail>, IViewSanadDetailRepository, ISyncRepository<ViewSanadDetailBusiness>, IEditableDelivery, IRepository<ViewSanadDetailBusiness>, INoGuidRepository<ViewSanadDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ViewSanadDetailBusiness, ViewSanadDetail>, IItemAsyncMapper<ViewSanadDetail, ViewSanadDetailBusiness>, IItemMapper<ViewSanadDetail, ViewSanadDetailBusiness>
	{
		public Task<ViewSanadDetail> MapAsync(string contextGuid, ViewSanadDetailBusiness source, ViewSanadDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ViewSanadDetail();
				}
				destination.Guid = source.Guid;
				destination.KolGuid = source.KolGuid;
				destination.Status = source.Status;
				destination.Description = source.Description;
				destination.DateM = source.DateM;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.Debit = source.Debit;
				destination.Credit = source.Credit;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = source.SanadSt;
				destination.Qt = source.Qt;
				destination.DecimalGuid = source.DecimalGuid;
				destination.RowDesc = source.RowDesc;
				destination.KolGuid = source.KolGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Serial = source.Serial;
				destination.CashierGuid = source.CashierGuid;
				destination.TafsilGuid2 = source.TafsilGuid2;
				destination.TafsilGuid3 = source.TafsilGuid3;
				destination.TafsilGuid4 = source.TafsilGuid4;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 194);
				throw;
			}
		}

		public Task<ViewSanadDetailBusiness> MapAsync(string contextGuid, ViewSanadDetail source, ViewSanadDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public ViewSanadDetailBusiness Map(string contextGuid, ViewSanadDetail source, ViewSanadDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ViewSanadDetailBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.KolGuid = source.KolGuid;
				destination.Status = source.Status;
				destination.Description = source.Description;
				destination.DateM = source.DateM;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.Debit = source.Debit;
				destination.Credit = source.Credit;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = source.SanadSt;
				destination.Qt = source.Qt;
				destination.DecimalGuid = source.DecimalGuid;
				destination.RowDesc = source.RowDesc;
				destination.KolGuid = source.KolGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.Serial = source.Serial;
				destination.TafsilGuid2 = source.TafsilGuid2;
				destination.TafsilGuid3 = source.TafsilGuid3;
				destination.TafsilGuid4 = source.TafsilGuid4;
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
				WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 244);
				throw;
			}
		}

		public Task<List<ViewSanadDetail>> MapListAsync(string contextGuid, IEnumerable<ViewSanadDetailBusiness> source, List<ViewSanadDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ViewSanadDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<ViewSanadDetail> source, List<ViewSanadDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemMapper<ViewSanadDetail, ViewSanadDetailBusiness> MapFromDatabaseNoneAsync => Mapper;

	public override IItemAsyncMapper<ViewSanadDetail, ViewSanadDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ViewSanadDetailBusiness, ViewSanadDetail> MapToDatabase => Mapper;

	public ViewSanadDetailPersistenceRepositoryBase(ContextHolder db)
		: base(db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 42);
		}
	}

	public abstract Task<List<WebSanad_Det>> GetAllSanadDetailsAsync(int sanadNo, CancellationToken token, string tranName = "");

	public abstract Task<ReturnedSaveFuncInfo> RemoveAutoArticlesAsync(Document asType, Guid asGuid, IEnumerable<Sanad_DetBusiness> containsItems, string tranName, CancellationToken token);

	public abstract Task<ReturnedSaveFuncInfo> SaveRangeAsync(string caller, SanadBusiness sanadMaster, string tranName, CancellationToken token);

	public async Task<int> NextSanadNoAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 49));
		using (new JaegerMonitor("NextSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs"))
		{
			int result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<int> noteSanadNumbers = await EntityFrameworkQueryableExtensions.ToListAsync<int>(from q in (IQueryable<Sanads>)c.Sanads
						where q.SanadType == SanadType.Note
						select q.SanadNo, token);
					int i = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ViewSanadDetail>(((IQueryable<ViewSanadDetail>)c.ViewSanadDetail).OrderByDescending((ViewSanadDetail x) => x.SanadNo).Take(1), token))?.SanadNo ?? 0) + 1;
					if (i <= 1)
					{
						i = 2;
					}
					if (noteSanadNumbers?.Any() ?? false)
					{
						for (; noteSanadNumbers.Contains(i); i++)
						{
						}
					}
					result = i;
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
					WebErrorLog.Instance.StartLog(ex3, "", "NextSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 69);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<ViewSanadDetailBusiness>> GetAllSanadOutOfFinancialPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllSanadOutOfFinancialPeriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 80));
		IEnumerable<ViewSanadDetailBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ViewSanadDetail> queryable = ((IQueryable<ViewSanadDetail>)c.ViewSanadDetail).Where((ViewSanadDetail q) => q.DateM < startDate || q.DateM > endDate);
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable.ToList(), null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllSanadOutOfFinancialPeriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 92);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllSanadOutOfFinancialPeriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<ViewSanadDetailBusiness>> GetAllAsync(int sanadNo, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 103));
		IEnumerable<ViewSanadDetailBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ViewSanadDetail> queryable = ((IQueryable<ViewSanadDetail>)c.ViewSanadDetail).Where((ViewSanadDetail q) => q.SanadNo == sanadNo);
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable.ToList(), null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 115);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<int> GetSanadNoAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 126));
		int result;
		try
		{
			_ = 1;
			try
			{
				result = (await c.ViewSanadDetail.FindAsync(new object[1] { guid }))?.SanadNo ?? 0;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs", 136);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public virtual Task<int> GetFutureSanadCountAsync(DateTime lastDate, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<List<ViewSanadExistRecord>> GetAllExistRecordAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
