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

internal class PrdSerialDetailPersistenceRepository : PersistenceMasterRepository<PrdSerialDetailBusiness, PrdSerialDetails>, IPrdSerialDetailRepository, IPersistenceMasterRepository<PrdSerialDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdSerialDetailBusiness, PrdSerialDetails>, IItemAsyncMapper<PrdSerialDetails, PrdSerialDetailBusiness>
	{
		public Task<PrdSerialDetails> MapAsync(string contextGuid, PrdSerialDetailBusiness source, PrdSerialDetails destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialDetailPersistenceRepository.cs"))
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
						destination = new PrdSerialDetails();
					}
					destination.Guid = source.Guid;
					destination.MasterGuid = source.MasterGuid;
					destination.Modified = source.Modified;
					destination.DetailGuid = source.DetailGuid;
					destination.AsType = source.AsType;
					destination.IsVorod = source.IsVorod;
					destination.Ordinal = source.Ordinal;
					destination.SanadDate = source.SanadDate;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialDetailPersistenceRepository.cs", 65);
					throw;
				}
			}
		}

		public Task<PrdSerialDetailBusiness> MapAsync(string contextGuid, PrdSerialDetails source, PrdSerialDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialDetailPersistenceRepository.cs"))
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
						destination = new PrdSerialDetailBusiness(contextGuid);
					}
					destination.Guid = source.Guid;
					destination.MasterGuid = source.MasterGuid;
					destination.Modified = source.Modified;
					destination.DetailGuid = source.DetailGuid;
					destination.AsType = source.AsType;
					destination.IsVorod = source.IsVorod;
					destination.Ordinal = source.Ordinal;
					destination.SanadDate = source.SanadDate;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialDetailPersistenceRepository.cs", 94);
					throw;
				}
			}
		}

		public Task<List<PrdSerialDetails>> MapListAsync(string contextGuid, IEnumerable<PrdSerialDetailBusiness> source, List<PrdSerialDetails> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdSerialDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdSerialDetails> source, List<PrdSerialDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _disposed;

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed
	{
		get
		{
			if (!_disposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public override IItemAsyncMapper<PrdSerialDetails, PrdSerialDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdSerialDetailBusiness, PrdSerialDetails> MapToDatabase => Mapper;

	public PrdSerialDetailPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_disposed)
			{
				_disposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialDetailPersistenceRepository.cs", 34);
		}
	}
}
