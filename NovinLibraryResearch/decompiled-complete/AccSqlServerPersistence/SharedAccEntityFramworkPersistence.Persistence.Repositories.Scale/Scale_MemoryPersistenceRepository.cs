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

internal class Scale_MemoryPersistenceRepository : PersistenceRepository<Scale_MemoryBusiness, Scale_Memory>, IScale_MemoryRepository, IRepository<Scale_MemoryBusiness>, INoGuidRepository<Scale_MemoryBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Memory, Scale_MemoryBusiness>, IItemAsyncMapper<Scale_MemoryBusiness, Scale_Memory>
	{
		public Task<Scale_MemoryBusiness> MapAsync(string contextGuid, Scale_Memory source, Scale_MemoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_MemoryBusiness(contextGuid);
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.MyProperty = source.MyProperty;
				destination.ProductGuid = source.ProductGuid;
				destination.MemoryNumber = source.MemoryNumber;
				destination.MemoryName = source.MemoryName;
				destination.MemoryPrice = source.MemoryPrice;
				destination.Date = source.Date;
				destination.Taeed = source.Taeed;
				destination.Vazni = source.Vazni;
				destination.TotalQT = source.TotalQT;
				destination.TotalPrice = source.TotalPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemoryPersistenceRepository.cs", 77);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Memory> MapAsync(string contextGuid, Scale_MemoryBusiness source, Scale_Memory destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Memory();
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.MyProperty = source.MyProperty;
				destination.ProductGuid = source.ProductGuid;
				destination.MemoryNumber = source.MemoryNumber;
				destination.MemoryName = source.MemoryName;
				destination.MemoryPrice = source.MemoryPrice;
				destination.Date = source.Date;
				destination.Taeed = source.Taeed;
				destination.Vazni = source.Vazni;
				destination.TotalQT = source.TotalQT;
				destination.TotalPrice = source.TotalPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemoryPersistenceRepository.cs", 109);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_MemoryBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Memory> source, List<Scale_MemoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Memory>> MapListAsync(string contextGuid, IEnumerable<Scale_MemoryBusiness> source, List<Scale_Memory> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Memory, Scale_MemoryBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Memory, Scale_MemoryBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_MemoryBusiness, Scale_Memory> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_MemoryPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemoryPersistenceRepository.cs", 43);
		}
	}
}
