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
using PacketParser.Filters.Configs;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class ViewCheckMCacheRepository : CacheRepository<ViewCheckMBusiness>, IViewCheckMRepository, ISyncRepository<ViewCheckMBusiness>, IEditableDelivery, IRepository<ViewCheckMBusiness>, INoGuidRepository<ViewCheckMBusiness>, IDisposable
{
	private new IViewCheckMRepository DbRepository;

	private ConcurrentDictionary<string, Guid> CacheName = new ConcurrentDictionary<string, Guid>();

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ViewCheckMBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<ViewCheckMBusiness, ViewCheckMBusiness> Mapper => ViewChechMMapper.Instance;

	public ViewCheckMCacheRepository(IRepository<ViewCheckMBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = (IViewCheckMRepository)dbRepository;
		TimingCache = new TimingCache<Guid, ViewCheckMBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ViewCheckMCacheRepository.cs", 45);
		}
	}

	public Task<IEnumerable<ViewCheckMBusiness>> GetAllAsync(ENUMCheck? status, Guid? pardazandeGuid, DateTime? dateSarresid1, DateTime? dateSarresid2, CancellationToken token, string tranName)
	{
		return DbRepository.GetAllAsync(status, pardazandeGuid, dateSarresid1, dateSarresid2, token, tranName);
	}

	public Task<bool> CheckMA2re_ISEnteghaliAsync(Guid guid, CancellationToken token, string tranName)
	{
		return DbRepository.CheckMA2re_ISEnteghaliAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> VagozarKardanAsync(List<ViewCheckMBusiness> checkList, Guid guid, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.VagozarKardanAsync(checkList, guid, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> BargashtZadanAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.BargashtZadanAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> NaghdKardanAsync(List<ViewCheckMBusiness> checkList, Guid guid, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.NaghdKardanAsync(checkList, guid, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> EbtalAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		return DbRepository.EbtalAsync(checkList, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> VosolKardanAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.VosolKardanAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> OdatBargashtiAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.OdatBargashtiAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> Bargashty2GhablAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.Bargashty2GhablAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> Vagozar2MojoodAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.Vagozar2MojoodAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> Naghd2MojoodAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.Naghd2MojoodAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> Batel2MojoodAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		return DbRepository.Batel2MojoodAsync(checkList, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> Vosol2VagozarAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		return DbRepository.Vosol2VagozarAsync(checkList, dateM, sanadNo, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ElamTahvilAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		return DbRepository.ElamTahvilAsync(checkList, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		return DbRepository.SetDeliveryAsync(del, token, tryCount, tranName);
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return DbRepository.GetAllNotSentAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		return DbRepository.SetSaveResult(item, token, tryCount, tranName);
	}

	public override Task<ViewCheckMBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return DbRepository.GetAsync(guid, token, tranName);
	}

	public Task<Guid> GetBankVagozarGuidAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		return DbRepository.GetBankVagozarGuidAsync(chequeGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(ViewChequeMForceUpdateConfig config, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ViewCheckMCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				ReturnedSaveFuncInfo ret = await DbRepository.ForceUpdateChecqueStatusAsync(config, token, tranName);
				if (ret.HasError)
				{
					return ret;
				}
				ViewCheckMBusiness viewCheckMBusiness = await GetAsync(config.ChequeGuid, token, tranName);
				if (viewCheckMBusiness != null)
				{
					await TimingCache.UpdateTimeAsync(viewCheckMBusiness.Guid, viewCheckMBusiness, tranName);
				}
				return ret;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ViewCheckMCacheRepository.cs", 98);
				return new ReturnedSaveFuncInfo(ex2, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ViewCheckMCacheRepository.cs", 99);
			}
		}
	}

	public Task<IEnumerable<ChequeMSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		return DbRepository.GetSummaryAsync(date1, date2, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		return DbRepository.UpdateServerStatusAsync(guid, status, token, tranName);
	}
}
