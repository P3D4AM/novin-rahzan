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

internal class Scale_CommandPersistenceRepository : PersistenceRepository<Scale_CommandBusiness, Scale_Command>, IScale_CommandRepository, IRepository<Scale_CommandBusiness>, INoGuidRepository<Scale_CommandBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Command, Scale_CommandBusiness>, IItemAsyncMapper<Scale_CommandBusiness, Scale_Command>
	{
		public Task<Scale_CommandBusiness> MapAsync(string contextGuid, Scale_Command source, Scale_CommandBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_CommandBusiness(contextGuid);
				}
				destination.DateM = source.DateM;
				destination.DateSH = source.DateSH;
				destination.Command = source.Command;
				destination.Sender = source.Sender;
				destination.ScaleGuid = source.ScaleGuid;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_CommandPersistenceRepository.cs", 70);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Command> MapAsync(string contextGuid, Scale_CommandBusiness source, Scale_Command destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Command();
				}
				destination.DateM = source.DateM;
				destination.DateSH = source.DateSH;
				destination.Command = source.Command;
				destination.Sender = source.Sender;
				destination.ScaleGuid = source.ScaleGuid;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_CommandPersistenceRepository.cs", 97);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_CommandBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Command> source, List<Scale_CommandBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Command>> MapListAsync(string contextGuid, IEnumerable<Scale_CommandBusiness> source, List<Scale_Command> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Command, Scale_CommandBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Command, Scale_CommandBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_CommandBusiness, Scale_Command> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_CommandPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_CommandPersistenceRepository.cs", 43);
		}
	}
}
