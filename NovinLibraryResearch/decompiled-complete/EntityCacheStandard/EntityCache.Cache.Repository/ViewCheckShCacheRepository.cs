using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class ViewCheckShCacheRepository : CacheRepository<ViewCheckShBusiness>, IViewCheckShRepository, ISyncRepository<ViewCheckShBusiness>, IEditableDelivery, IRepository<ViewCheckShBusiness>, INoGuidRepository<ViewCheckShBusiness>, IDisposable
{
	private IViewCheckShRepository Dbrepository;

	private ConcurrentDictionary<string, Guid> CacheName = new ConcurrentDictionary<string, Guid>();

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ViewCheckShBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<ViewCheckShBusiness, ViewCheckShBusiness> Mapper => ViewCheckShMapper.Instance;

	public ViewCheckShCacheRepository(IRepository<ViewCheckShBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		Dbrepository = (IViewCheckShRepository)dbRepository;
		TimingCache = new TimingCache<Guid, ViewCheckShBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ViewCheckShCacheRepository.cs", 43);
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		return Dbrepository.SetDeliveryAsync(del, token, tryCount, tranName);
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return Dbrepository.GetAllNotSentAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		return Dbrepository.SetSaveResult(item, token, tryCount, tranName);
	}

	public Task<IEnumerable<ViewCheckShBusiness>> GetAllAsync(ENCheckSHStatus? status, DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		return Dbrepository.GetAllAsync(status, date1, date2, token, tranName);
	}

	public Task<bool> CheckSHA2re_ISEnteghaliAsync(Guid guid, CancellationToken token, string tranName)
	{
		return Dbrepository.CheckSHA2re_ISEnteghaliAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ElamPardakhtAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return Dbrepository.ElamPardakhtAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> EbtalAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return Dbrepository.EbtalAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> BargashtAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return Dbrepository.BargashtAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> PardakhtShode2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return Dbrepository.PardakhtShode2PardakhtaniAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> Bargashty2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return Dbrepository.Bargashty2PardakhtaniAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> Ebtal2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, CancellationToken token, string tranName)
	{
		return Dbrepository.Ebtal2PardakhtaniAsync(checkList, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(Guid chequeGuid, ENCheckSHStatus status, CancellationToken token, string tranName)
	{
		return Dbrepository.ForceUpdateChecqueStatusAsync(chequeGuid, status, token, tranName);
	}

	public Task<IEnumerable<ChequeSHSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		return Dbrepository.GetSummaryAsync(date1, date2, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		return Dbrepository.UpdateServerStatusAsync(guid, status, token, tranName);
	}
}
