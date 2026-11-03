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

internal class PrdSelectedDetListPersistenceRepository : PersistenceMasterRepository<PrdSelectedDetListBusiness, Prd_SelectedDetails>, IPrdSelectedDetListRepository, IPersistenceMasterRepository<PrdSelectedDetListBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdSelectedDetListBusiness, Prd_SelectedDetails>, IItemAsyncMapper<Prd_SelectedDetails, PrdSelectedDetListBusiness>
	{
		public Task<Prd_SelectedDetails> MapAsync(string contextGuid, PrdSelectedDetListBusiness source, Prd_SelectedDetails destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedDetListPersistenceRepository.cs"))
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
						destination = new Prd_SelectedDetails();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.PrdGuid = source.GetPrdGuid();
					destination.MasterGuid = source.MasterGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedDetListPersistenceRepository.cs", 60);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public async Task<PrdSelectedDetListBusiness> MapAsync(string contextGuid, Prd_SelectedDetails source, PrdSelectedDetListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedDetListPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new PrdSelectedDetListBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					await destination.SetPrdGuid(source.PrdGuid, EnSystem.Mapper);
					destination.MasterGuid = source.MasterGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedDetListPersistenceRepository.cs", 88);
					throw;
				}
				return destination;
			}
		}

		public Task<List<Prd_SelectedDetails>> MapListAsync(string contextGuid, IEnumerable<PrdSelectedDetListBusiness> source, List<Prd_SelectedDetails> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdSelectedDetListBusiness>> MapListAsync(string contextGuid, IEnumerable<Prd_SelectedDetails> source, List<PrdSelectedDetListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Prd_SelectedDetails, PrdSelectedDetListBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdSelectedDetListBusiness, Prd_SelectedDetails> MapToDatabase => Mapper;

	public PrdSelectedDetListPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedDetListPersistenceRepository.cs", 28);
		}
	}
}
