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

public class ChequeMOperationCacheRepository : SyncCacheRepository<ChequeMOperationBusiness>, IChequeMOperationRepository, ISyncRepository<ChequeMOperationBusiness>, IEditableDelivery, IRepository<ChequeMOperationBusiness>, INoGuidRepository<ChequeMOperationBusiness>, IDisposable
{
	private IChequeMOperationRepository dbRepository;

	public override IItemAsyncMapper<ChequeMOperationBusiness, ChequeMOperationBusiness> Mapper => ChequeMOperationMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ChequeMOperationBusiness> TimingCache { get; set; }

	public ChequeMOperationCacheRepository(IChequeMOperationRepository dbRepository, int CachingTime)
		: base((ISyncRepository<ChequeMOperationBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, ChequeMOperationBusiness>(base.ContextGuid, Mapper, CachingTime);
		this.dbRepository = dbRepository;
	}

	public Task<IEnumerable<ChequeMOperationBusiness>> GetAllAsync(ChequeMOperationFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<List<MoghayeratChequeAvalDoreModel>> GetAllAvalDoreMoghyerat(Guid chequeMojodGuid, Guid chequeVagozarGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAvalDoreMoghyerat(chequeMojodGuid, chequeVagozarGuid, token, tranName);
	}

	public Task<List<ChequeMInstantModel>> GetAllChequeMInstantAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllChequeMInstantAsync(dateM, token, tranName);
	}

	public Task<IEnumerable<ChequeMOperationReportBusiness>> GetAllReportAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllReportAsync(chequeGuid, token, tranName);
	}

	public Task<short> GetNextOrdinalAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetNextOrdinalAsync(chequeGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> IncreaseOrinalAsync(Guid chequeGuid, short orinal, CancellationToken token, string tranName)
	{
		return dbRepository.IncreaseOrinalAsync(chequeGuid, orinal, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveByChequeGuidAsync(Guid chequeGuid, short tryCount, CancellationToken token, string tranName)
	{
		return dbRepository.RemoveByChequeGuidAsync(chequeGuid, tryCount, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		return dbRepository.SetSanadNoAsync(guid, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateDataBaseOperationsAsync(List<Tuple<Guid, short>> tuples, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateDataBaseOperationsAsync(tuples, token, tranName);
	}
}
