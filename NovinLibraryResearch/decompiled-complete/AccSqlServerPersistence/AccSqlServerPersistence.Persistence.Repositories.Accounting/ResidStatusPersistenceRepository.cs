using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ResidStatusPersistenceRepository : PersistenceRepository<ResidStatusBusiness, ResidStatus>, IResidStatusRepository, IRepository<ResidStatusBusiness>, INoGuidRepository<ResidStatusBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ResidStatusBusiness, ResidStatus>, IItemAsyncMapper<ResidStatus, ResidStatusBusiness>
	{
		public Task<ResidStatus> MapAsync(string contextGuid, ResidStatusBusiness source, ResidStatus destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ResidStatusPersistenceRepository.cs"))
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
						destination = new ResidStatus();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ResidStatusPersistenceRepository.cs", 66);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<ResidStatusBusiness> MapAsync(string contextGuid, ResidStatus source, ResidStatusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ResidStatusBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Status = source.Status;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ResidStatusPersistenceRepository.cs", 93);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ResidStatus>> MapListAsync(string contextGuid, IEnumerable<ResidStatusBusiness> source, List<ResidStatus> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ResidStatusBusiness>> MapListAsync(string contextGuid, IEnumerable<ResidStatus> source, List<ResidStatusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ResidStatus, ResidStatusBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ResidStatusBusiness, ResidStatus> MapToDatabase => Mapper;

	public override IItemMapper<ResidStatus, ResidStatusBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ResidStatusPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ResidStatusPersistenceRepository.cs", 31);
		}
	}
}
