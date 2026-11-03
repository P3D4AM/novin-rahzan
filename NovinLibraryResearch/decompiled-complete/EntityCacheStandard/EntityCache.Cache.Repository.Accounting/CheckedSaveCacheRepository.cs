using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class CheckedSaveCacheRepository : CacheRepository<CheckedSaveBusiness>, ICheckedSaveRepository, IRepository<CheckedSaveBusiness>, INoGuidRepository<CheckedSaveBusiness>, IDisposable
{
	private ICheckedSaveRepository dbRepository;

	public override IItemAsyncMapper<CheckedSaveBusiness, CheckedSaveBusiness> Mapper => CheckedSaveMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CheckedSaveBusiness> TimingCache { get; set; }

	public new bool IsDisposed { get; private set; }

	public CheckedSaveCacheRepository(ICheckedSaveRepository dbRepository, int CachingTime)
		: base((IRepository<CheckedSaveBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CheckedSaveBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<IEnumerable<CheckedSaveBusiness>> GetAllAsync(checkedTablReportType reportType, Guid hesabGuid, bool? status, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(reportType, hesabGuid, status, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid TafsilGuid, DateTime d1, DateTime d2, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.DeleteAsync(reportType, TafsilGuid, d1, d2, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid hesabGuid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.DeleteAsync(reportType, hesabGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid hesabGuid, Guid detailGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.DeleteAsync(reportType, hesabGuid, detailGuid, tryCount, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\CheckedSaveCacheRepository.cs", 66);
		}
	}
}
