using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Scale;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Scale;

internal class Scale_PrdVahedPersistenceRepository : PersistenceRepository<Scale_PrdVahedBusiness, Scale_PrdVahed>, IScale_PrdVahedRepository, IRepository<Scale_PrdVahedBusiness>, INoGuidRepository<Scale_PrdVahedBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_PrdVahed, Scale_PrdVahedBusiness>, IItemAsyncMapper<Scale_PrdVahedBusiness, Scale_PrdVahed>
	{
		public Task<Scale_PrdVahed> MapAsync(string contextGuid, Scale_PrdVahedBusiness source, Scale_PrdVahed destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_PrdVahed();
				}
				destination.Guid = source.Guid;
				destination.Name = source.Name;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PrdVahedPersistenceRepository.cs", 69);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_PrdVahedBusiness> MapAsync(string contextGuid, Scale_PrdVahed source, Scale_PrdVahedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_PrdVahedBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Name = source.Name;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PrdVahedPersistenceRepository.cs", 93);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_PrdVahed>> MapListAsync(string contextGuid, IEnumerable<Scale_PrdVahedBusiness> source, List<Scale_PrdVahed> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_PrdVahedBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_PrdVahed> source, List<Scale_PrdVahedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_PrdVahed, Scale_PrdVahedBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_PrdVahed, Scale_PrdVahedBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_PrdVahedBusiness, Scale_PrdVahed> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_PrdVahedPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PrdVahedPersistenceRepository.cs", 45);
		}
	}
}
