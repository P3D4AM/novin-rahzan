using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository;

public interface ISanadRepository : ISyncRepository<SanadBusiness>, IEditableDelivery, IRepository<SanadBusiness>, INoGuidRepository<SanadBusiness>, IDisposable
{
	Task<SanadBusiness> GetAsync(int sanadNo, CancellationToken token, string tranName);

	Task<SanadSt> GetSanadStBySanadNoAsync(int SanadNO, CancellationToken token, string tranName);

	Task<int> UpdateVSanadDetailAsync(short mode, Guid asGuid, int sanadNo, int oldSanadNo, string oldDateSh, Guid cashierGuid, short tryCount, CancellationToken token, string tranName);

	Task<int> GetSanadNoByGuidAsync(Guid Guid, CancellationToken token, string tranName);

	Task<Guid> GetGuidBySanadNoAsync(int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SanadSortAsync(CancellationToken token, string tranName);

	Task<int> NextIdAsync(CancellationToken token, string tranName);

	Task<bool> IsVSanadDetailAsync(int SanadNo, CancellationToken token, string tranName);

	Task<SanadSt> GetSanadStByGuidTypeAsync(Guid SanadGuid, Document SanadType, CancellationToken token, string tranName);

	Task<SanadBusiness> GetAsync(Guid SanadGuid, Document SanadType, bool LoadDets, CancellationToken token, string tranName);

	Task<IEnumerable<SanadReportBusiness>> GetAllAsync(SanadFilter filter, CancellationToken token, string tranName);

	Task<List<SanadDetReportBusiness>> GetAllRoznameSanadDetAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token);

	Task<List<SanadDetRoznameKolBusiness>> GetAllSanadDetRoznameKolAsync(DateTime date1, DateTime date2, string tranName, CancellationToken token);

	Task<List<MoeinTafsilReportModel>> GetMoein2TafsilsRptAsync(DateTime date1, DateTime date2, Guid moeinGuid, string tranName, CancellationToken token);

	Task<List<MoeinTafsilReportModel>> GetTafsil2MoeinsRptAsync(DateTime date1, DateTime date2, Guid tafsilGuid, string tranName, CancellationToken token);

	Task<List<SanadDetRoznameKolBusiness>> GetTestTarazNameAsync(DateTime date, string tranName, CancellationToken token);

	Task<SanadBusiness> GetFromDbAsync(Guid sanadGuid, Document sanadType, CancellationToken token, string tranName);

	Task<List<SanadDetReportBusiness>> GetAllGardeshSanadAsync(ENHesabLevel level, DateTime? date1, DateTime? date2, long? code1, long? code2, long? codeMoein, Guid? kolGuid, string tranName, int? maxCount, short maxParalelThreads, CancellationToken token);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid asGuid, Document asType, short tryCount, CancellationToken token, string tranName);

	Task<List<MoeinTafsilReportModel>> GetTafsilTajmeeReportAsync(DateTime date1, DateTime date2, string moeinCode1, string moeinCode2, string tafsilCode1, string tafsilCode2, CancellationToken token, string tranName);

	Task<List<SanadDetReportBusiness>> GetMoeinTafsilGardeshAsync(DateTime d1, DateTime d2, Guid tafsilGuid, string moeinCode1, string moeinCode2, string tranName, CancellationToken token);

	Task<Guid> AsId_To_AsGuidAsync(int asId, Document asType, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeSanadNoAsync(List<int> oldSanadNo, int newSanadNo, string description, DateTime date, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeSanadStatusAsync(string sansdNos, SanadSt st, short tryCount, CancellationToken token, string tranName);

	Task<int> CountOfAutoSanadWithDiffrentDateAsync(int sanadNo, DateTime dateM, Guid asGuid1, short asType1, Guid asGuid2, short asType2, CancellationToken token, string tranName);

	Task<bool> CheckGhestAsync(Guid sanadGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(string caller, SanadBusiness item, EnSystem system, int oldSanadNo, CancellationToken token, string tranName, IEnumerable<ViewSanadDetailCoverBusiness> lst, params Tuple<Document, Guid>[] autoSanadItemsForSave);

	Task<decimal> GetBeforeAccountAsync(Guid tafsilGuid, Guid asGuid, Document asType, CancellationToken token, string tranName);

	Task<SanadBusiness> GetEkhtetamieSanadAsync(CancellationToken token, string tranName);

	Task<List<Sanad_DetBusiness>> GetSanadTempAsync(CancellationToken token, string tranName);

	Task<DateTime?> GetSanadBargashtDateAsync(decimal price, Guid debitTafsilGuid, Guid creditTafsilGuid, int sanadNo, CancellationToken token, string tranName);

	Task<List<Sanad_DetBusiness>> GetSanadTemporaryAsync(CancellationToken token, string tranName);

	Task<IEnumerable<SanadGhestViewModel>> GetAllSanadGhestAsync(Guid sanadGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveDuplicateSanadNoAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName);
}
