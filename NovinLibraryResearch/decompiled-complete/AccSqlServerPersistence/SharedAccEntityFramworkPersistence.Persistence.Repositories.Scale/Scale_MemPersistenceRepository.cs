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

internal class Scale_MemPersistenceRepository : PersistenceRepository<Scale_MemBusiness, Scale_Mem>, IScale_MemRepository, IRepository<Scale_MemBusiness>, INoGuidRepository<Scale_MemBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Mem, Scale_MemBusiness>, IItemAsyncMapper<Scale_MemBusiness, Scale_Mem>
	{
		public Task<Scale_MemBusiness> MapAsync(string contextGuid, Scale_Mem source, Scale_MemBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_MemBusiness(contextGuid);
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.MemoryNumber = source.MemoryNumber;
				destination.Date = source.Date;
				destination.ISSaved = source.ISSaved;
				destination.PriceOnScale = source.PriceOnScale;
				destination.CodeOnScale = source.CodeOnScale;
				destination.NameScale = source.NameScale;
				destination.BarCode = source.BarCode;
				destination.Name = source.Name;
				destination.Price = source.Price;
				destination.Vazni = source.Vazni;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemPersistenceRepository.cs", 75);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Mem> MapAsync(string contextGuid, Scale_MemBusiness source, Scale_Mem destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Mem();
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.MemoryNumber = source.MemoryNumber;
				destination.Date = source.Date;
				destination.ISSaved = source.ISSaved;
				destination.PriceOnScale = source.PriceOnScale;
				destination.CodeOnScale = source.CodeOnScale;
				destination.NameScale = source.NameScale;
				destination.BarCode = source.BarCode;
				destination.Name = source.Name;
				destination.Price = source.Price;
				destination.Vazni = source.Vazni;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemPersistenceRepository.cs", 109);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_MemBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Mem> source, List<Scale_MemBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Mem>> MapListAsync(string contextGuid, IEnumerable<Scale_MemBusiness> source, List<Scale_Mem> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Mem, Scale_MemBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Mem, Scale_MemBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_MemBusiness, Scale_Mem> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_MemPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_MemPersistenceRepository.cs", 42);
		}
	}
}
