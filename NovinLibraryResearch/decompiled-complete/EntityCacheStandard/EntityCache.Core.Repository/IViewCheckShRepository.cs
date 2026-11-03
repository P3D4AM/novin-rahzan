using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IViewCheckShRepository : ISyncRepository<ViewCheckShBusiness>, IEditableDelivery, IRepository<ViewCheckShBusiness>, INoGuidRepository<ViewCheckShBusiness>, IDisposable
{
	Task<IEnumerable<ViewCheckShBusiness>> GetAllAsync(ENCheckSHStatus? status, DateTime? date1, DateTime? date2, CancellationToken token, string tranName);

	Task<bool> CheckSHA2re_ISEnteghaliAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ElamPardakhtAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> EbtalAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> BargashtAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> PardakhtShode2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> Bargashty2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> Ebtal2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(Guid chequeGuid, ENCheckSHStatus status, CancellationToken token, string tranName);

	Task<IEnumerable<ChequeSHSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName);

	new void ResetCache();
}
