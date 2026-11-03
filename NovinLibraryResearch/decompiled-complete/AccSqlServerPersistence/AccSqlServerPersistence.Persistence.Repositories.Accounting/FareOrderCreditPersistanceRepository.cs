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

internal class FareOrderCreditPersistanceRepository : PersistenceRepository<FareOrderCreditBusiness, FareOrderCredit>, IFareOrderCredit, IRepository<FareOrderCreditBusiness>, INoGuidRepository<FareOrderCreditBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<FareOrderCreditBusiness, FareOrderCredit>, IItemAsyncMapper<FareOrderCredit, FareOrderCreditBusiness>
	{
		public Task<FareOrderCredit> MapAsync(string contextGuid, FareOrderCreditBusiness source, FareOrderCredit destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\FareOrderCreditPersistanceRepository.cs"))
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
						destination = new FareOrderCredit();
					}
					destination.Guid = source.Guid;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.Status = source.Status;
					destination.ViewOrder = source.ViewOrder;
					destination.Modified = source.Modified;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\FareOrderCreditPersistanceRepository.cs", 69);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<FareOrderCreditBusiness> MapAsync(string contextGuid, FareOrderCredit source, FareOrderCreditBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\FareOrderCreditPersistanceRepository.cs"))
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
						destination = new FareOrderCreditBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.Status = source.Status;
					destination.ViewOrder = source.ViewOrder;
					destination.Modified = source.Modified;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\FareOrderCreditPersistanceRepository.cs", 98);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<FareOrderCredit>> MapListAsync(string contextGuid, IEnumerable<FareOrderCreditBusiness> source, List<FareOrderCredit> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<FareOrderCreditBusiness>> MapListAsync(string contextGuid, IEnumerable<FareOrderCredit> source, List<FareOrderCreditBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<FareOrderCredit, FareOrderCreditBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<FareOrderCreditBusiness, FareOrderCredit> MapToDatabase => Mapper;

	public override IItemMapper<FareOrderCredit, FareOrderCreditBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public FareOrderCreditPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\FareOrderCreditPersistanceRepository.cs", 33);
		}
	}
}
