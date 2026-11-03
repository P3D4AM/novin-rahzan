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

internal class TolidFormulaExportPersistenceRepository : PersistenceMasterRepository<TolidFormulaExportBusiness, TolidFormulaExport>, ITolidFormulaExportRepository, IPersistenceMasterRepository<TolidFormulaExportBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidFormulaExport, TolidFormulaExportBusiness>, IItemAsyncMapper<TolidFormulaExportBusiness, TolidFormulaExport>
	{
		public async Task<TolidFormulaExportBusiness> MapAsync(string contextGuid, TolidFormulaExport source, TolidFormulaExportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaExportPersistenceRepository.cs"))
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
						destination = new TolidFormulaExportBusiness(contextGuid);
					}
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
					await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
					destination.DepotGuid = source.DepotGuid;
					destination.Quantity = source.Quantity;
					destination.V2Guid = source.V2Guid;
					return destination;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaExportPersistenceRepository.cs", 62);
					throw;
				}
			}
		}

		public Task<TolidFormulaExport> MapAsync(string contextGuid, TolidFormulaExportBusiness source, TolidFormulaExport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaExportPersistenceRepository.cs"))
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
						destination = new TolidFormulaExport();
					}
					destination.Description = source.Description ?? "";
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
					destination.ProductGuid = source.ProductGuid;
					destination.DepotGuid = source.DepotGuid;
					destination.Quantity = source.Quantity;
					destination.V2Guid = source.V2Guid;
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaExportPersistenceRepository.cs", 91);
					throw;
				}
			}
		}

		public Task<List<TolidFormulaExportBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaExport> source, List<TolidFormulaExportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidFormulaExport>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaExportBusiness> source, List<TolidFormulaExport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TolidFormulaExport, TolidFormulaExportBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidFormulaExportBusiness, TolidFormulaExport> MapToDatabase => Mapper;

	public TolidFormulaExportPersistenceRepository(ContextHolder dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaExportPersistenceRepository.cs", 33);
		}
	}
}
