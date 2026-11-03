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

internal class Scale_PrdGroupPersistenceRepository : PersistenceRepository<Scale_PrdGroupBusiness, Scale_PrdGroup>, IScale_PrdGroupRepository, IRepository<Scale_PrdGroupBusiness>, INoGuidRepository<Scale_PrdGroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_PrdGroup, Scale_PrdGroupBusiness>, IItemAsyncMapper<Scale_PrdGroupBusiness, Scale_PrdGroup>
	{
		public Task<Scale_PrdGroupBusiness> MapAsync(string contextGuid, Scale_PrdGroup source, Scale_PrdGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_PrdGroupBusiness(contextGuid);
				}
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PrdGroupPersistenceRepository.cs", 70);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_PrdGroup> MapAsync(string contextGuid, Scale_PrdGroupBusiness source, Scale_PrdGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_PrdGroup();
				}
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PrdGroupPersistenceRepository.cs", 95);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_PrdGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_PrdGroup> source, List<Scale_PrdGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_PrdGroup>> MapListAsync(string contextGuid, IEnumerable<Scale_PrdGroupBusiness> source, List<Scale_PrdGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_PrdGroup, Scale_PrdGroupBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_PrdGroup, Scale_PrdGroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_PrdGroupBusiness, Scale_PrdGroup> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_PrdGroupPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PrdGroupPersistenceRepository.cs", 46);
		}
	}
}
