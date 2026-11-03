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

internal class Scale_Message_ReadSalePersistenceRepository : PersistenceRepository<Scale_Message_ReadSaleBusiness, Scale_Message_ReadSale>, IScale_Message_ReadSaleRepository, IRepository<Scale_Message_ReadSaleBusiness>, INoGuidRepository<Scale_Message_ReadSaleBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Message_ReadSale, Scale_Message_ReadSaleBusiness>, IItemAsyncMapper<Scale_Message_ReadSaleBusiness, Scale_Message_ReadSale>
	{
		public Task<Scale_Message_ReadSaleBusiness> MapAsync(string contextGuid, Scale_Message_ReadSale source, Scale_Message_ReadSaleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Message_ReadSaleBusiness(contextGuid);
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.MemoryGuid = source.MemoryGuid;
				destination.Count = source.Count;
				destination.DateM = source.DateM;
				destination.Priority = source.Priority;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_Message_ReadSalePersistenceRepository.cs", 68);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Message_ReadSale> MapAsync(string contextGuid, Scale_Message_ReadSaleBusiness source, Scale_Message_ReadSale destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Message_ReadSale();
				}
				destination.ScaleGuid = source.ScaleGuid;
				destination.MemoryGuid = source.MemoryGuid;
				destination.Count = source.Count;
				destination.DateM = source.DateM;
				destination.Priority = source.Priority;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_Message_ReadSalePersistenceRepository.cs", 95);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_Message_ReadSaleBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Message_ReadSale> source, List<Scale_Message_ReadSaleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Message_ReadSale>> MapListAsync(string contextGuid, IEnumerable<Scale_Message_ReadSaleBusiness> source, List<Scale_Message_ReadSale> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Message_ReadSale, Scale_Message_ReadSaleBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Message_ReadSale, Scale_Message_ReadSaleBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_Message_ReadSaleBusiness, Scale_Message_ReadSale> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_Message_ReadSalePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_Message_ReadSalePersistenceRepository.cs", 42);
		}
	}
}
