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

internal class BillMasterPersistenceRepository : PersistenceRepository<BillMasterBusiness, BillMaster>, IBillMasterRepository, IRepository<BillMasterBusiness>, INoGuidRepository<BillMasterBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BillMasterBusiness, BillMaster>, IItemAsyncMapper<BillMaster, BillMasterBusiness>
	{
		public Task<BillMaster> MapAsync(string contextGuid, BillMasterBusiness source, BillMaster destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BillMaster();
				}
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.Status = source.Status;
				destination.Date = source.Date;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.Guid = source.Guid;
				destination.SanadNumber = source.SanadNumber;
				destination.SanadSt = source.SanadSt;
				destination.Sum = source.Sum;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\BillMasterPersistenceRepository.cs", 62);
			}
			return Task.FromResult(destination);
		}

		public Task<BillMasterBusiness> MapAsync(string contextGuid, BillMaster source, BillMasterBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BillMasterBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.Status = source.Status;
				destination.Date = source.Date;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.Guid = source.Guid;
				destination.SanadNumber = source.SanadNumber;
				destination.SanadSt = source.SanadSt;
				destination.Sum = source.Sum;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\BillMasterPersistenceRepository.cs", 87);
			}
			return Task.FromResult(destination);
		}

		public Task<List<BillMaster>> MapListAsync(string contextGuid, IEnumerable<BillMasterBusiness> source, List<BillMaster> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BillMasterBusiness>> MapListAsync(string contextGuid, IEnumerable<BillMaster> source, List<BillMasterBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<BillMaster, BillMasterBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<BillMaster, BillMasterBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BillMasterBusiness, BillMaster> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public BillMasterPersistenceRepository(ContextHolder db)
		: base(db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\BillMasterPersistenceRepository.cs", 38);
		}
	}
}
