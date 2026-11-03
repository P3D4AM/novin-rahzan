using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Configs;

namespace EntityCache.Core.Repository;

public interface IViewCheckMRepository : ISyncRepository<ViewCheckMBusiness>, IEditableDelivery, IRepository<ViewCheckMBusiness>, INoGuidRepository<ViewCheckMBusiness>, IDisposable
{
	Task<IEnumerable<ViewCheckMBusiness>> GetAllAsync(ENUMCheck? status, Guid? pardazandeGuid, DateTime? dateSarresid1, DateTime? dateSarresid2, CancellationToken token, string tranName);

	Task<bool> CheckMA2re_ISEnteghaliAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> VagozarKardanAsync(List<ViewCheckMBusiness> checkList, Guid guid, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> BargashtZadanAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> NaghdKardanAsync(List<ViewCheckMBusiness> checkList, Guid guid, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> EbtalAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> VosolKardanAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> OdatBargashtiAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> Bargashty2GhablAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> Vagozar2MojoodAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> Naghd2MojoodAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> Batel2MojoodAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<int>> Vosol2VagozarAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ElamTahvilAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName);

	Task<Guid> GetBankVagozarGuidAsync(Guid chequeGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(ViewChequeMForceUpdateConfig config, CancellationToken token, string tranName);

	Task<IEnumerable<ChequeMSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName);

	new void ResetCache();
}
