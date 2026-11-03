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

internal class Scale_PRDPersistenceRepository : PersistenceRepository<Scale_PRDBusiness, Scale_PRD>, IScale_PRDRepository, IRepository<Scale_PRDBusiness>, INoGuidRepository<Scale_PRDBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_PRD, Scale_PRDBusiness>, IItemAsyncMapper<Scale_PRDBusiness, Scale_PRD>
	{
		public Task<Scale_PRDBusiness> MapAsync(string contextGuid, Scale_PRD source, Scale_PRDBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_PRDBusiness(contextGuid);
				}
				destination.MemoryNumber = source.MemoryNumber;
				destination.MemoryName = source.MemoryName;
				destination.MemoryPrice = source.MemoryPrice;
				destination.Vazni = source.Vazni;
				destination.TotalQT = source.TotalQT;
				destination.TotalPrice = source.TotalPrice;
				destination.ProductGuid = source.ProductGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PRDPersistenceRepository.cs", 74);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_PRD> MapAsync(string contextGuid, Scale_PRDBusiness source, Scale_PRD destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_PRD();
				}
				destination.MemoryNumber = source.MemoryNumber;
				destination.MemoryName = source.MemoryName;
				destination.MemoryPrice = source.MemoryPrice;
				destination.Vazni = source.Vazni;
				destination.TotalQT = source.TotalQT;
				destination.TotalPrice = source.TotalPrice;
				destination.ProductGuid = source.ProductGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PRDPersistenceRepository.cs", 103);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_PRDBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_PRD> source, List<Scale_PRDBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_PRD>> MapListAsync(string contextGuid, IEnumerable<Scale_PRDBusiness> source, List<Scale_PRD> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_PRD, Scale_PRDBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_PRD, Scale_PRDBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_PRDBusiness, Scale_PRD> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_PRDPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_PRDPersistenceRepository.cs", 46);
		}
	}
}
