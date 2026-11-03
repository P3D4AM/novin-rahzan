using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Bill;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Bill;

internal class TempBillPersistenceRepository : PersistenceNoGuidRepository<TempBillBusiness, TempBill>, ITempBillRepository, INoGuidRepository<TempBillBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TempBillBusiness, TempBill>, IItemAsyncMapper<TempBill, TempBillBusiness>
	{
		public Task<TempBill> MapAsync(string contextGuid, TempBillBusiness source, TempBill destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TempBill();
				}
				destination.OrderGuid = source.OrderGuid;
				destination.PlacementGuid = source.PlacementGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\TempBillPersistenceRepository.cs", 61);
			}
			return Task.FromResult(destination);
		}

		public Task<TempBillBusiness> MapAsync(string contextGuid, TempBill source, TempBillBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TempBillBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.PlacementGuid = source.PlacementGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\TempBillPersistenceRepository.cs", 79);
			}
			return Task.FromResult(destination);
		}

		public Task<List<TempBill>> MapListAsync(string contextGuid, IEnumerable<TempBillBusiness> source, List<TempBill> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TempBillBusiness>> MapListAsync(string contextGuid, IEnumerable<TempBill> source, List<TempBillBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TempBill, TempBillBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TempBillBusiness, TempBill> MapToDatabase => Mapper;

	public TempBillPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\TempBillPersistenceRepository.cs", 36);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(TempBillBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TempBillBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
