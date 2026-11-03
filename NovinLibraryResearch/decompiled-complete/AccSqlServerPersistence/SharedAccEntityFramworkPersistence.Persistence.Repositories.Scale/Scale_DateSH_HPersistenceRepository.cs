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

internal class Scale_DateSH_HPersistenceRepository : PersistenceRepository<Scale_DateSH_HBusiness, Scale_DateSH_H>, IScale_DateSH_HRepository, IRepository<Scale_DateSH_HBusiness>, INoGuidRepository<Scale_DateSH_HBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_DateSH_H, Scale_DateSH_HBusiness>, IItemAsyncMapper<Scale_DateSH_HBusiness, Scale_DateSH_H>
	{
		public Task<Scale_DateSH_HBusiness> MapAsync(string contextGuid, Scale_DateSH_H source, Scale_DateSH_HBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_DateSH_HBusiness(contextGuid);
				}
				destination.FactorNumber = source.FactorNumber;
				destination.UpdateDate = source.UpdateDate;
				destination.BatchGuid = source.BatchGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_DateSH_HPersistenceRepository.cs", 72);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_DateSH_H> MapAsync(string contextGuid, Scale_DateSH_HBusiness source, Scale_DateSH_H destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_DateSH_H();
				}
				destination.FactorNumber = source.FactorNumber;
				destination.UpdateDate = source.UpdateDate;
				destination.BatchGuid = source.BatchGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_DateSH_HPersistenceRepository.cs", 100);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_DateSH_HBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_DateSH_H> source, List<Scale_DateSH_HBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_DateSH_H>> MapListAsync(string contextGuid, IEnumerable<Scale_DateSH_HBusiness> source, List<Scale_DateSH_H> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_DateSH_H, Scale_DateSH_HBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_DateSH_H, Scale_DateSH_HBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_DateSH_HBusiness, Scale_DateSH_H> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_DateSH_HPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_DateSH_HPersistenceRepository.cs", 43);
		}
	}
}
