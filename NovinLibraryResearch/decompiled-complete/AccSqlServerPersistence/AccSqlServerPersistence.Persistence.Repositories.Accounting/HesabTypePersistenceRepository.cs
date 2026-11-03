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

internal class HesabTypePersistenceRepository : PersistenceRepository<HesabTypeBusiness, HesabTypes>, IHesabTypeRepository, IRepository<HesabTypeBusiness>, INoGuidRepository<HesabTypeBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<HesabTypeBusiness, HesabTypes>, IItemAsyncMapper<HesabTypes, HesabTypeBusiness>
	{
		public Task<HesabTypes> MapAsync(string contextGuid, HesabTypeBusiness source, HesabTypes destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabTypePersistenceRepository.cs"))
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
						destination = new HesabTypes();
					}
					destination.Guid = source.Guid;
					destination.Name = source.Name;
					destination.Code = source.Code;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabTypePersistenceRepository.cs", 60);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<HesabTypeBusiness> MapAsync(string contextGuid, HesabTypes source, HesabTypeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabTypePersistenceRepository.cs"))
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
						destination = new HesabTypeBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Name = source.Name;
					destination.Code = source.Code;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabTypePersistenceRepository.cs", 85);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<HesabTypes>> MapListAsync(string contextGuid, IEnumerable<HesabTypeBusiness> source, List<HesabTypes> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<HesabTypeBusiness>> MapListAsync(string contextGuid, IEnumerable<HesabTypes> source, List<HesabTypeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => false;

	public override bool HasModified => false;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; }

	public override IItemAsyncMapper<HesabTypes, HesabTypeBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<HesabTypeBusiness, HesabTypes> MapToDatabase => Mapper;

	public override IItemMapper<HesabTypes, HesabTypeBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public HesabTypePersistenceRepository(ContextHolder db)
		: base(db)
	{
		Mapper = new mapper();
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabTypePersistenceRepository.cs", 30);
		}
	}
}
