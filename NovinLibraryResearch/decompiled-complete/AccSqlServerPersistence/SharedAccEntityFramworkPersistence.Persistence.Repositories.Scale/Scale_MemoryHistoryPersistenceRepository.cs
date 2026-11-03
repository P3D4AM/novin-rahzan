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

internal class Scale_MemoryHistoryPersistenceRepository : PersistenceRepository<Scale_MemoryHistoryBusiness, Scale_MemoryHistory>, IScale_MemoryHistoryRepository, IRepository<Scale_MemoryHistoryBusiness>, INoGuidRepository<Scale_MemoryHistoryBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_MemoryHistory, Scale_MemoryHistoryBusiness>, IItemAsyncMapper<Scale_MemoryHistoryBusiness, Scale_MemoryHistory>
	{
		public Task<Scale_MemoryHistoryBusiness> MapAsync(string contextGuid, Scale_MemoryHistory source, Scale_MemoryHistoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_MemoryHistoryBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.ScaleGuid = source.ScaleGuid;
				destination.Date = source.Date;
				destination.ScaleName = source.ScaleName;
				destination.ProductCode = source.ProductCode;
				destination.BarCode = source.BarCode;
				destination.ProductName = source.ProductName;
				destination.MemoryNumber = source.MemoryNumber;
				destination.Vazni = source.Vazni;
				destination.UnitPrice = source.UnitPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemoryHistoryPersistenceRepository.cs", 73);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_MemoryHistory> MapAsync(string contextGuid, Scale_MemoryHistoryBusiness source, Scale_MemoryHistory destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_MemoryHistory();
				}
				destination.Guid = source.Guid;
				destination.ScaleGuid = source.ScaleGuid;
				destination.Date = source.Date;
				destination.ScaleName = source.ScaleName;
				destination.ProductCode = source.ProductCode;
				destination.BarCode = source.BarCode;
				destination.ProductName = source.ProductName;
				destination.MemoryNumber = source.MemoryNumber;
				destination.Vazni = source.Vazni;
				destination.UnitPrice = source.UnitPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemoryHistoryPersistenceRepository.cs", 104);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_MemoryHistoryBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_MemoryHistory> source, List<Scale_MemoryHistoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_MemoryHistory>> MapListAsync(string contextGuid, IEnumerable<Scale_MemoryHistoryBusiness> source, List<Scale_MemoryHistory> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_MemoryHistory, Scale_MemoryHistoryBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_MemoryHistory, Scale_MemoryHistoryBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_MemoryHistoryBusiness, Scale_MemoryHistory> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_MemoryHistoryPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemoryHistoryPersistenceRepository.cs", 43);
		}
	}
}
