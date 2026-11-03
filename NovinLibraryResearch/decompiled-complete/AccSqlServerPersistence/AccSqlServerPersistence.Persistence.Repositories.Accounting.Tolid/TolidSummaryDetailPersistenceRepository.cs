using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using EntityCache.Business.Accounting.Tolid.Summary;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal class TolidSummaryDetailPersistenceRepository : PersistenceMasterRepository<TolidSummaryDetailBusiness, TolidSummaryDetail>, ITolidSummaryDetailRepository, IPersistenceMasterRepository<TolidSummaryDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidSummaryDetailBusiness, TolidSummaryDetail>, IItemAsyncMapper<TolidSummaryDetail, TolidSummaryDetailBusiness>
	{
		public Task<TolidSummaryDetail> MapAsync(string contextGuid, TolidSummaryDetailBusiness source, TolidSummaryDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidSummaryDetail();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ProductGuid = source.ProductGuid;
				destination.DepotGuid = source.DepotGuid;
				destination.TolidGuid = source.TolidGuid;
				destination.RealAmount = source.RealAmount;
				destination.AvailableAmount = source.AvailableAmount;
				destination.ProducedAmount = source.ProducedAmount;
				destination.MasterGuid = source.MasterGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryDetailPersistenceRepository.cs", 61);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<TolidSummaryDetailBusiness> MapAsync(string contextGuid, TolidSummaryDetail source, TolidSummaryDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidSummaryDetailBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ProductGuid = source.ProductGuid;
				destination.DepotGuid = source.DepotGuid;
				destination.TolidGuid = source.TolidGuid;
				destination.RealAmount = source.RealAmount;
				destination.AvailableAmount = source.AvailableAmount;
				destination.ProducedAmount = source.ProducedAmount;
				destination.MasterGuid = source.MasterGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryDetailPersistenceRepository.cs", 91);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<TolidSummaryDetail>> MapListAsync(string contextGuid, IEnumerable<TolidSummaryDetailBusiness> source, List<TolidSummaryDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidSummaryDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidSummaryDetail> source, List<TolidSummaryDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TolidSummaryDetail, TolidSummaryDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidSummaryDetailBusiness, TolidSummaryDetail> MapToDatabase => Mapper;

	public TolidSummaryDetailPersistenceRepository(ContextHolder dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryDetailPersistenceRepository.cs", 31);
		}
	}
}
