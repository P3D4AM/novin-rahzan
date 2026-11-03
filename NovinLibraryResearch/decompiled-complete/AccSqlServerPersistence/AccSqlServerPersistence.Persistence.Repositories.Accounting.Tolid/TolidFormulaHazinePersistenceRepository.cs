using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal class TolidFormulaHazinePersistenceRepository : PersistenceMasterRepository<TolidFormulaHazineBusiness, TolidFormulaHazine>, ITolidFormulaHazineRepository, IPersistenceMasterRepository<TolidFormulaHazineBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidFormulaHazine, TolidFormulaHazineBusiness>, IItemAsyncMapper<TolidFormulaHazineBusiness, TolidFormulaHazine>
	{
		public Task<TolidFormulaHazineBusiness> MapAsync(string contextGuid, TolidFormulaHazine source, TolidFormulaHazineBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaHazinePersistenceRepository.cs"))
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
						destination = new TolidFormulaHazineBusiness(contextGuid);
					}
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.TafsilGuid = source.TafsilGuid;
					destination.Price = source.Price;
					destination.Type = source.Type;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaHazinePersistenceRepository.cs", 61);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<TolidFormulaHazine> MapAsync(string contextGuid, TolidFormulaHazineBusiness source, TolidFormulaHazine destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaHazinePersistenceRepository.cs"))
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
						destination = new TolidFormulaHazine();
					}
					destination.Description = source.Description ?? "";
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.TafsilGuid = source.TafsilGuid;
					destination.Price = source.Price;
					destination.Type = source.Type;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaHazinePersistenceRepository.cs", 90);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<TolidFormulaHazineBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaHazine> source, List<TolidFormulaHazineBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidFormulaHazine>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaHazineBusiness> source, List<TolidFormulaHazine> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TolidFormulaHazine, TolidFormulaHazineBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidFormulaHazineBusiness, TolidFormulaHazine> MapToDatabase => Mapper;

	public TolidFormulaHazinePersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaHazinePersistenceRepository.cs", 33);
		}
	}
}
