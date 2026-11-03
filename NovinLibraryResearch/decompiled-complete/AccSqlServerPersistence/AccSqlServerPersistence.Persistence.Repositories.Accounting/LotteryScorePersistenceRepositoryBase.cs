using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal abstract class LotteryScorePersistenceRepositoryBase : PersistenceRepository<LotteryScoreBusiness, LotteryScore>, ILotteryScoreRepository, IRepository<LotteryScoreBusiness>, INoGuidRepository<LotteryScoreBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<LotteryScoreBusiness, LotteryScore>, IItemAsyncMapper<LotteryScore, LotteryScoreBusiness>
	{
		public Task<LotteryScore> MapAsync(string contextGuid, LotteryScoreBusiness source, LotteryScore destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs"))
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
						destination = new LotteryScore();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.OrderGuid = source.OrderGuid;
					destination.DateM = source.DateM;
					destination.TafsilGuid = source.TafsilGuid;
					destination.ExpirationDate = source.ExpirationDate;
					destination.CashierGuid = source.CashierGuid;
					destination.Score = source.Score;
					destination.Description = source.Description;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs", 101);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<LotteryScoreBusiness> MapAsync(string contextGuid, LotteryScore source, LotteryScoreBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs"))
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
						destination = new LotteryScoreBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.OrderGuid = source.OrderGuid;
					destination.DateM = source.DateM;
					destination.TafsilGuid = source.TafsilGuid;
					destination.ExpirationDate = source.ExpirationDate;
					destination.CashierGuid = source.CashierGuid;
					destination.Score = source.Score;
					destination.Description = source.Description;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs", 134);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<LotteryScore>> MapListAsync(string contextGuid, IEnumerable<LotteryScoreBusiness> source, List<LotteryScore> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<LotteryScoreBusiness>> MapListAsync(string contextGuid, IEnumerable<LotteryScore> source, List<LotteryScoreBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<LotteryScore, LotteryScoreBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<LotteryScoreBusiness, LotteryScore> MapToDatabase => Mapper;

	public override IItemMapper<LotteryScore, LotteryScoreBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public LotteryScorePersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<LotteryScoreBusiness>> GetAllAsync(DateTime date1, DateTime date2, LotteryStatus status, Guid cusGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs", 25));
			IEnumerable<LotteryScoreBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<LotteryScore> queryable = ((IQueryable<LotteryScore>)c.LotteryScores).Where((LotteryScore w) => w.Status == status);
					if (cusGuid != Guid.Empty)
					{
						queryable = queryable?.Where((LotteryScore w) => w.TafsilGuid == cusGuid);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						List<LotteryScore> source = await EntityFrameworkQueryableExtensions.ToListAsync<LotteryScore>(queryable.Where((LotteryScore w) => w.DateM >= date1 && w.DateM <= date2), token);
						result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs", 39);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<List<Guid>> StartLottery(int count, CancellationToken token, string tranName);

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepositoryBase.cs", 61);
		}
	}
}
