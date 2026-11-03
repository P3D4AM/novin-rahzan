using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Reservation;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Reservation;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.ReservationClass;

internal abstract class ReservationBasePersistanceRepository : PersistenceRepository<ReservationBusiness, Reservation>, IReservationRepository, IRepository<ReservationBusiness>, INoGuidRepository<ReservationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Reservation, ReservationBusiness>, IItemAsyncMapper<ReservationBusiness, Reservation>
	{
		public Task<ReservationBusiness> MapAsync(string contextGuid, Reservation source, ReservationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReservationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.DateM = source.DateM;
				destination.TafsilGuid = source.TafsilGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.Number = source.Number;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 178);
				throw;
			}
		}

		public Task<Reservation> MapAsync(string contextGuid, ReservationBusiness source, Reservation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Reservation();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.DateM = source.DateM;
				destination.TafsilGuid = source.TafsilGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.Number = source.Number;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 208);
				throw;
			}
		}

		public Task<List<ReservationBusiness>> MapListAsync(string contextGuid, IEnumerable<Reservation> source, List<ReservationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Reservation>> MapListAsync(string contextGuid, IEnumerable<ReservationBusiness> source, List<Reservation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private ReservationDetailPersistanceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemMapper<Reservation, ReservationBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Reservation, ReservationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReservationBusiness, Reservation> MapToDatabase => Mapper;

	public ReservationBasePersistanceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new ReservationDetailPersistanceRepository(db);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 45);
		}
	}

	public virtual Task<IEnumerable<ReservationReportBusiness>> GetAllModelAsync(ReservationFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ReservationBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 55));
				Reservation reservation = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(reservation, reservation.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 57);
				if (res.HasError)
				{
					return res;
				}
				if (item.DetailList?.Any() ?? false)
				{
					IQueryable<ReservationDetail> queryable = ((IQueryable<ReservationDetail>)dbcontext.ReservationDetail).Where((ReservationDetail q) => q.MasterGuid == item.Guid);
					if (queryable != null && queryable.Any())
					{
						IQueryable<ReservationDetail> queryable2 = queryable.Where((ReservationDetail q) => !item.DetailList.Select((ReservationDetailBusiness q) => q.Guid).Contains(q.Guid));
						if (queryable2 != null && queryable2.Any())
						{
							dbcontext.ReservationDetail.RemoveRange((IEnumerable<ReservationDetail>)queryable2);
						}
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetailList, item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 67);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 72);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 73);
			}
			return res;
		}
	}

	public override async Task<ReservationBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 79));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs"))
		{
			ReservationBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					Reservation reservation = await c.Reservation.FindAsync(new object[1] { guid });
					if (reservation == null)
					{
						result = null;
					}
					else
					{
						ReservationBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, reservation, null, tranName, token);
						await _detRepo.GetAllAsync(ret, token, tranName);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 94);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> GetNextNumberAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 105));
		int result;
		try
		{
			_ = 1;
			try
			{
				result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Reservation>(((IQueryable<Reservation>)c.Reservation).OrderByDescending((Reservation x) => x.Number).Take(1), token))?.Number ?? 0) + 1;
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 116);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReservationBusiness> GetAsync(int number, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 127));
		ReservationBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Reservation reservation = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Reservation>((IQueryable<Reservation>)c.Reservation, (Expression<Func<Reservation, bool>>)((Reservation q) => q.Number == number), token);
				result = ((reservation != null) ? (await MapFromDatabase.MapAsync(ContextGuid, reservation, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs", 138);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationBasePersistanceRepository.cs");
			}
		}
		return result;
	}
}
