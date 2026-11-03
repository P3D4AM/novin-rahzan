using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Aghsat;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Aghsat;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Filters.Configs;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Aghsat;

public class AghsatMCacheRepository : SyncCacheRepository<AghsatMBusiness>, IAghsatMRepository, ISyncRepository<AghsatMBusiness>, IEditableDelivery, IRepository<AghsatMBusiness>, INoGuidRepository<AghsatMBusiness>, IDisposable
{
	private bool _isDisposed;

	private IAghsatMRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	protected override TimingCache<Guid, AghsatMBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<AghsatMBusiness, AghsatMBusiness> Mapper => AghsatMMapper.Instance;

	public AghsatMCacheRepository(IAghsatMRepository dbRepository, int cachingTime)
		: base((ISyncRepository<AghsatMBusiness>)dbRepository, cachingTime, cachingEnable: true)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AghsatMBusiness>(base.ContextGuid, Mapper, cachingTime);
	}

	public Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.NextId2Async(token, tranName);
	}

	public Task<IEnumerable<AghsatMBusiness>> GetAllByGroupGuidAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllByGroupGuidAsync(groupGuid, token, tranName);
	}

	public Task<List<AghsatMBusiness>> GetAllNotPayedAsync(Guid? hGuid, HesabType? hesabType, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllNotPayedAsync(hGuid, hesabType, token, tranName);
	}

	public Task<IEnumerable<AghsatMBusiness>> GetAllAsync(CancellationToken token, AghsatMFilter filter, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(token, filter, tranName);
	}

	public Task<AghsatMBusiness> GetAsync(int id2, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAsync(id2, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Aghsat\\AghsatMCacheRepository.cs", 73);
		}
	}

	public Task<int> GetCountByHGuidAsync(Guid tafsilGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetCountByHGuidAsync(tafsilGuid, token, tranName);
	}

	public Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllReportAsync(filter, token, tranName);
	}

	public Task<IEnumerable<AghsatMSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		return _dbRepository.GetSummaryAsync(date1, date2, token, tranName);
	}

	public Task<IEnumerable<AghsatMDelayModel>> GetAllDelayedInstallmentsAsync(AghsatMDelayedConfig config, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllDelayedInstallmentsAsync(config, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateReceptionInfoAsync(Guid? receptionGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.UpdateReceptionInfoAsync(receptionGuid, token, tranName);
	}
}
