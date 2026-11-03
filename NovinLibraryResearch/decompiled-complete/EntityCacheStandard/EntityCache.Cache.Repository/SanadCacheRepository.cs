using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class SanadCacheRepository : SyncCacheRepository<SanadBusiness>, ISanadRepository, ISyncRepository<SanadBusiness>, IEditableDelivery, IRepository<SanadBusiness>, INoGuidRepository<SanadBusiness>, IDisposable
{
	private ISanadRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SanadBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<SanadBusiness, SanadBusiness> Mapper => SanadMapper.Instance;

	public SanadCacheRepository(ISanadRepository dbRepository, int CachingTime)
		: base((ISyncRepository<SanadBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, SanadBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<SanadBusiness> GetAsync(int SanadNO, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(SanadNO, token, tranName);
	}

	public Task<SanadSt> GetSanadStBySanadNoAsync(int SanadNO, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetSanadStBySanadNoAsync(SanadNO, token, tranName);
	}

	public Task<int> UpdateVSanadDetailAsync(short mode, Guid asGuid, int sanadNo, int oldSanadNo, string oldDateSh, Guid cashierGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.UpdateVSanadDetailAsync(mode, asGuid, sanadNo, oldSanadNo, oldDateSh, cashierGuid, tryCount, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SanadSortAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SanadSortAsync(token, tranName);
	}

	public new Task<ReturnedSaveFuncInfo> SaveAsync(SanadBusiness Sanad, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SaveAsync(Sanad, tryCount, token, tranName);
	}

	public Task<int> NextIdAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.NextIdAsync(token, tranName);
	}

	public new Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveAsync(guid, tryCount, token, tranName);
	}

	public Task<int> GetSanadNoByGuidAsync(Guid Guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetSanadNoByGuidAsync(Guid, token, tranName);
	}

	public Task<bool> IsVSanadDetailAsync(int SanadNo, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.IsVSanadDetailAsync(SanadNo, token, tranName);
	}

	public Task<SanadSt> GetSanadStByGuidTypeAsync(Guid SanadGuid, Document SanadType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetSanadStByGuidTypeAsync(SanadGuid, SanadType, token, tranName);
	}

	public Task<SanadBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(guid, token, tranName);
	}

	public Task<IEnumerable<SanadReportBusiness>> GetAllAsync(SanadFilter filter, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<List<SanadDetReportBusiness>> GetAllRoznameSanadDetAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllRoznameSanadDetAsync(date1, date2, tranName, token);
	}

	public Task<List<SanadDetRoznameKolBusiness>> GetAllSanadDetRoznameKolAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllSanadDetRoznameKolAsync(date1, date2, tranName, token);
	}

	public Task<List<MoeinTafsilReportModel>> GetMoein2TafsilsRptAsync(DateTime date1, DateTime date2, Guid moeinGuid, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetMoein2TafsilsRptAsync(date1, date2, moeinGuid, tranName, token);
	}

	public Task<List<MoeinTafsilReportModel>> GetTafsil2MoeinsRptAsync(DateTime date1, DateTime date2, Guid tafsilGuid, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetTafsil2MoeinsRptAsync(date1, date2, tafsilGuid, tranName, token);
	}

	public Task<List<SanadDetRoznameKolBusiness>> GetTestTarazNameAsync(DateTime date, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetTestTarazNameAsync(date, tranName, token);
	}

	public override Task<SanadBusiness> GetAsync(Guid Guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(Guid, token, tranName);
	}

	public Task<SanadBusiness> GetAsync(Guid SanadGuid, Document SanadType, bool LoadDets, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(SanadGuid, SanadType, LoadDets, token, tranName);
	}

	public Task<SanadBusiness> GetFromDbAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName)
	{
		return dbRepository.GetFromDbAsync(sanadGuid, sanadType, token, tranName);
	}

	public Task<List<SanadDetReportBusiness>> GetAllGardeshSanadAsync(ENHesabLevel level, DateTime? date1, DateTime? date2, long? code1, long? code2, long? codeMoein, Guid? kolGuid, string tranName, int? maxCount, short maxParalelThreads, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllGardeshSanadAsync(level, date1, date2, code1, code2, codeMoein, kolGuid, tranName, maxCount, maxParalelThreads, token);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid asGuid, Document asType, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveAsync(asGuid, asType, tryCount, token, tranName);
	}

	public Task<List<MoeinTafsilReportModel>> GetTafsilTajmeeReportAsync(DateTime date1, DateTime date2, string moeinCode1, string moeinCode2, string tafsilCode1, string tafsilCode2, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetTafsilTajmeeReportAsync(date1, date2, moeinCode1, moeinCode2, tafsilCode1, tafsilCode2, token, tranName);
	}

	public Task<List<SanadDetReportBusiness>> GetMoeinTafsilGardeshAsync(DateTime d1, DateTime d2, Guid tafsilGuid, string moeinCode1, string moeinCode2, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetMoeinTafsilGardeshAsync(d1, d2, tafsilGuid, moeinCode1, moeinCode2, tranName, token);
	}

	public Task<Guid> AsId_To_AsGuidAsync(int asId, Document asType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.AsId_To_AsGuidAsync(asId, asType, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeSanadNoAsync(List<int> oldSanadNo, int newSanadNo, string description, DateTime date, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.ChangeSanadNoAsync(oldSanadNo, newSanadNo, description, date, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeSanadStatusAsync(string sansdNos, SanadSt st, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.ChangeSanadStatusAsync(sansdNos, st, tryCount, token, tranName);
	}

	public Task<int> CountOfAutoSanadWithDiffrentDateAsync(int sanadNo, DateTime dateM, Guid asGuid1, short asType1, Guid asGuid2, short asType2, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CountOfAutoSanadWithDiffrentDateAsync(sanadNo, dateM, asGuid1, asType1, asGuid2, asType2, token, tranName);
	}

	public Task<bool> CheckGhestAsync(Guid sanadGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CheckGhestAsync(sanadGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(string caller, SanadBusiness item, EnSystem system, int oldSanadNo, CancellationToken token, string tranName, IEnumerable<ViewSanadDetailCoverBusiness> lst, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SaveAsync(caller, item, system, oldSanadNo, token, tranName, lst, autoSanadItemsForSave);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Sanad\\SanadCacheRepository.cs", 222);
		}
	}

	public Task<decimal> GetBeforeAccountAsync(Guid tafsilGuid, Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		return dbRepository.GetBeforeAccountAsync(tafsilGuid, asGuid, asType, token, tranName);
	}

	public Task<SanadBusiness> GetEkhtetamieSanadAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetEkhtetamieSanadAsync(token, tranName);
	}

	public Task<List<Sanad_DetBusiness>> GetSanadTempAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetSanadTempAsync(token, tranName);
	}

	public Task<Guid> GetGuidBySanadNoAsync(int sanadNo, CancellationToken token, string tranName)
	{
		return dbRepository.GetGuidBySanadNoAsync(sanadNo, token, tranName);
	}

	public Task<DateTime?> GetSanadBargashtDateAsync(decimal price, Guid debitTafsilGuid, Guid creditTafsilGuid, int sanadNo, CancellationToken token, string tranName)
	{
		return dbRepository.GetSanadBargashtDateAsync(price, debitTafsilGuid, creditTafsilGuid, sanadNo, token, tranName);
	}

	public Task<List<Sanad_DetBusiness>> GetSanadTemporaryAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetSanadTemporaryAsync(token, tranName);
	}

	public Task<IEnumerable<SanadGhestViewModel>> GetAllSanadGhestAsync(Guid sanadGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllSanadGhestAsync(sanadGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveDuplicateSanadNoAsync(CancellationToken token, string tranName)
	{
		return dbRepository.RemoveDuplicateSanadNoAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		return dbRepository.ChangeStatusIdAsync(guid, statusId, token, tranName);
	}
}
