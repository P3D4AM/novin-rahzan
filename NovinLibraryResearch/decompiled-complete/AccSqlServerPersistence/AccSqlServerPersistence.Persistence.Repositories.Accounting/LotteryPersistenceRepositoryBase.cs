using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal abstract class LotteryPersistenceRepositoryBase : PersistenceRepository<LotteryBusiness, Lottery>, ILotteryRepository, IRepository<LotteryBusiness>, INoGuidRepository<LotteryBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<LotteryBusiness, Lottery>, IItemAsyncMapper<Lottery, LotteryBusiness>
	{
		public Task<Lottery> MapAsync(string contextGuid, LotteryBusiness source, Lottery destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepositoryBase.cs"))
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
						destination = new Lottery();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.TafsilGuid = source.TafsilGuid;
					destination.CashierGuid = source.CashierGuid;
					destination.PardakhtGuid = source.PardakhtGuid;
					destination.DateM = source.DateM;
					destination.Status = source.Status;
					destination.Description = source.Description;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepositoryBase.cs", 72);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<LotteryBusiness> MapAsync(string contextGuid, Lottery source, LotteryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepositoryBase.cs"))
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
						destination = new LotteryBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.TafsilGuid = source.TafsilGuid;
					destination.CashierGuid = source.CashierGuid;
					destination.PardakhtGuid = source.PardakhtGuid;
					destination.DateM = source.DateM;
					destination.Status = source.Status;
					destination.Description = source.Description;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepositoryBase.cs", 103);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<Lottery>> MapListAsync(string contextGuid, IEnumerable<LotteryBusiness> source, List<Lottery> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<LotteryBusiness>> MapListAsync(string contextGuid, IEnumerable<Lottery> source, List<LotteryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Lottery, LotteryBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<LotteryBusiness, Lottery> MapToDatabase => Mapper;

	public override IItemMapper<Lottery, LotteryBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public LotteryPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public abstract Task<IEnumerable<LotteryBusiness>> GetAllAsync(LotteryStatus status, DateTime date1, DateTime date2, Guid cusGuid, CancellationToken token, string tranName);

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepositoryBase.cs", 34);
		}
	}
}
