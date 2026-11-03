using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Report;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Sanad;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class SanadStatusReportCacheRepository : SyncCacheRepository<SanadStatusReportBusiness>, ISanadStatusReportRepository, ISyncRepository<SanadStatusReportBusiness>, IEditableDelivery, IRepository<SanadStatusReportBusiness>, INoGuidRepository<SanadStatusReportBusiness>, IDisposable
{
	private ISanadStatusReportRepository _dbRepository;

	public override IItemAsyncMapper<SanadStatusReportBusiness, SanadStatusReportBusiness> Mapper => SanadStatusReportMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SanadStatusReportBusiness> TimingCache { get; set; }

	public SanadStatusReportCacheRepository(ISanadStatusReportRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<SanadStatusReportBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, SanadStatusReportBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<SanadStatusReportBusiness> GetAsync(int id, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(id, token, tranName);
	}

	public Task<IEnumerable<SanadStatusReportBusiness>> GetAllAsync(Document? asType, bool? status, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(asType, status, token, tranName);
	}

	public Task<int> GetNextIdAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextIdAsync(token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Sanad\\SanadStatusReportCacheRepository.cs", 43);
		}
	}

	public Task<IEnumerable<SanadStatusReportErrorTimeModel>> GetAllErrorTimeAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllErrorTimeAsync(token, tranName);
	}

	public Task<int> GetMinErrorTimeAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetMinErrorTimeAsync(token, tranName);
	}

	public Task<SanadStatusReportBusiness> GetAsync(string name, Document asType, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(name, asType, token, tranName);
	}

	public Task<List<SanadStatusReportBusiness>> IsDuplicateCodeAsync(long minCode, long maxCode, Document asType, Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.IsDuplicateCodeAsync(minCode, maxCode, asType, guid, token, tranName);
	}

	public Task<IEnumerable<SanadStatusReportForceUpdateModel>> GetAllForceUpdateAsync(DateTime dateMoasser, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllForceUpdateAsync(dateMoasser, token, tranName);
	}
}
