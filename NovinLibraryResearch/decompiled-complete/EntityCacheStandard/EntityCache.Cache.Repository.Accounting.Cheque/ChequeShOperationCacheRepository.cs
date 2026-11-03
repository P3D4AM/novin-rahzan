using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Cheque;
using EntityCache.Mapping.BusinessMappers.Accounting.Cheque;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Cheque;

public class ChequeShOperationCacheRepository : SyncCacheRepository<ChequeShOperationBusiness>, IChequeShOperationRepository, ISyncRepository<ChequeShOperationBusiness>, IEditableDelivery, IRepository<ChequeShOperationBusiness>, INoGuidRepository<ChequeShOperationBusiness>, IDisposable
{
	private IChequeShOperationRepository dbRepository;

	public override IItemAsyncMapper<ChequeShOperationBusiness, ChequeShOperationBusiness> Mapper => ChequeShOperationMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ChequeShOperationBusiness> TimingCache { get; set; }

	public ChequeShOperationCacheRepository(IChequeShOperationRepository dbRepository, int CachingTime)
		: base((ISyncRepository<ChequeShOperationBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, ChequeShOperationBusiness>(base.ContextGuid, Mapper, CachingTime);
		this.dbRepository = dbRepository;
	}

	public Task<IEnumerable<ChequeShOperationBusiness>> GetAllAsync(ChequeShOperationFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveByChequeGuidAsync(Guid chequeGuid, short tryCount, CancellationToken token, string tranName)
	{
		return dbRepository.RemoveByChequeGuidAsync(chequeGuid, tryCount, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		return dbRepository.SetSanadNoAsync(guid, sanadNo, token, tranName);
	}

	public Task<List<ChequeShInstantModel>> GetAllChequeShInstantAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllChequeShInstantAsync(dateM, token, tranName);
	}

	public Task<IEnumerable<ChequeShOperationReportBusiness>> GetAllReportAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllReportAsync(chequeGuid, token, tranName);
	}
}
