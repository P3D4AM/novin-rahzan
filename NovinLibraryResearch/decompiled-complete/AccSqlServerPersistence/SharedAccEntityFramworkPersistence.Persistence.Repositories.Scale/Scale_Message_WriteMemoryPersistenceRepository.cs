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

internal class Scale_Message_WriteMemoryPersistenceRepository : PersistenceRepository<Scale_Message_WriteMemoryBusiness, Scale_Message_WriteMemory>, IScale_Message_WriteMemoryRepository, IRepository<Scale_Message_WriteMemoryBusiness>, INoGuidRepository<Scale_Message_WriteMemoryBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Message_WriteMemory, Scale_Message_WriteMemoryBusiness>, IItemAsyncMapper<Scale_Message_WriteMemoryBusiness, Scale_Message_WriteMemory>
	{
		public Task<Scale_Message_WriteMemoryBusiness> MapAsync(string contextGuid, Scale_Message_WriteMemory source, Scale_Message_WriteMemoryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Message_WriteMemoryBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.ScaleGuid = source.ScaleGuid;
				destination.Code = source.Code;
				destination.Barcode = source.Barcode;
				destination.Name = source.Name;
				destination.UnitPrice = source.UnitPrice;
				destination.DateM = source.DateM;
				destination.Expiredate = source.Expiredate;
				destination.ST = source.ST;
				destination.Vazni = source.Vazni;
				destination.NextTry = source.NextTry;
				destination.TryCount = source.TryCount;
				destination.MaxTryCount = source.MaxTryCount;
				destination.Parametr1 = source.Parametr1;
				destination.Parametr2 = source.Parametr2;
				destination.Parametr3 = source.Parametr3;
				destination.Parametr4 = source.Parametr4;
				destination.MemoryNumber = source.MemoryNumber;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_WriteMemoryPersistenceRepository.cs", 77);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Message_WriteMemory> MapAsync(string contextGuid, Scale_Message_WriteMemoryBusiness source, Scale_Message_WriteMemory destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Message_WriteMemory();
				}
				destination.Guid = source.Guid;
				destination.ScaleGuid = source.ScaleGuid;
				destination.Code = source.Code;
				destination.Barcode = source.Barcode;
				destination.Name = source.Name;
				destination.UnitPrice = source.UnitPrice;
				destination.DateM = source.DateM;
				destination.Expiredate = source.Expiredate;
				destination.ST = source.ST;
				destination.Vazni = source.Vazni;
				destination.NextTry = source.NextTry;
				destination.TryCount = source.TryCount;
				destination.MaxTryCount = source.MaxTryCount;
				destination.Parametr1 = source.Parametr1;
				destination.Parametr2 = source.Parametr2;
				destination.Parametr3 = source.Parametr3;
				destination.Parametr4 = source.Parametr4;
				destination.MemoryNumber = source.MemoryNumber;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_WriteMemoryPersistenceRepository.cs", 115);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_Message_WriteMemoryBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Message_WriteMemory> source, List<Scale_Message_WriteMemoryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Message_WriteMemory>> MapListAsync(string contextGuid, IEnumerable<Scale_Message_WriteMemoryBusiness> source, List<Scale_Message_WriteMemory> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Message_WriteMemory, Scale_Message_WriteMemoryBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Message_WriteMemory, Scale_Message_WriteMemoryBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_Message_WriteMemoryBusiness, Scale_Message_WriteMemory> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_Message_WriteMemoryPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_WriteMemoryPersistenceRepository.cs", 39);
		}
	}
}
