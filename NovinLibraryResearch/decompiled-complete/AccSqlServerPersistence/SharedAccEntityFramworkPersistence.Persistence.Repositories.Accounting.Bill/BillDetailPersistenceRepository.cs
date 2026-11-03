using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Bill;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Bill;

internal class BillDetailPersistenceRepository : PersistenceMasterRepository<BillDetailBusiness, BillDetail>, IBillDetailRepository, IPersistenceMasterRepository<BillDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BillDetailBusiness, BillDetail>, IItemAsyncMapper<BillDetail, BillDetailBusiness>
	{
		public Task<BillDetail> MapAsync(string contextGuid, BillDetailBusiness source, BillDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BillDetail();
				}
				destination.OrderGuid = source.OrderGuid;
				destination.MasterGuid = source.MasterGuid;
				destination.PlacementGuid = source.PlacementGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\BillDetailPersistenceRepository.cs", 51);
			}
			return Task.FromResult(destination);
		}

		public Task<BillDetailBusiness> MapAsync(string contextGuid, BillDetail source, BillDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BillDetailBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.MasterGuid = source.MasterGuid;
				destination.PlacementGuid = source.PlacementGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\BillDetailPersistenceRepository.cs", 69);
			}
			return Task.FromResult(destination);
		}

		public Task<List<BillDetail>> MapListAsync(string contextGuid, IEnumerable<BillDetailBusiness> source, List<BillDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BillDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<BillDetail> source, List<BillDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<BillDetail, BillDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BillDetailBusiness, BillDetail> MapToDatabase => Mapper;

	public BillDetailPersistenceRepository(ContextHolder dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\BillDetailPersistenceRepository.cs", 33);
		}
	}
}
