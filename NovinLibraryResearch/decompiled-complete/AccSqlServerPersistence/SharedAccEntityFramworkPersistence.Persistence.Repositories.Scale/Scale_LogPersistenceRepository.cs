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

internal class Scale_LogPersistenceRepository : PersistenceRepository<Scale_LogBusiness, Scale_Log>, IScale_LogRepository, IRepository<Scale_LogBusiness>, INoGuidRepository<Scale_LogBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Log, Scale_LogBusiness>, IItemAsyncMapper<Scale_LogBusiness, Scale_Log>
	{
		public Task<Scale_LogBusiness> MapAsync(string contextGuid, Scale_Log source, Scale_LogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_LogBusiness(contextGuid);
				}
				destination.DateM = source.DateM;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_LogPersistenceRepository.cs", 68);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Log> MapAsync(string contextGuid, Scale_LogBusiness source, Scale_Log destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Log();
				}
				destination.DateM = source.DateM;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_LogPersistenceRepository.cs", 92);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_LogBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Log> source, List<Scale_LogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Log>> MapListAsync(string contextGuid, IEnumerable<Scale_LogBusiness> source, List<Scale_Log> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Log, Scale_LogBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Log, Scale_LogBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_LogBusiness, Scale_Log> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_LogPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_LogPersistenceRepository.cs", 43);
		}
	}
}
