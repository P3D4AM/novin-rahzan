using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Models.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting.DepotReceipt;

public interface IDepotReceiptRepository : IRepository<DepotReceiptBusiness>, INoGuidRepository<DepotReceiptBusiness>, IDisposable
{
	Task<int> GetNextNumberAsync(EnDepotReceiptType type, CancellationToken token, string tranName);

	Task<Guid> GetGuidById2Async(EnDepotReceiptType type, int id2, CancellationToken token, string tranName);

	Task<IEnumerable<DepotReceiptReportBusiness>> GetAllAsync(DepotReceiptFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateRefrenceGuidAsync(List<SanadAttchmentModel> model, CancellationToken token, string tranName);

	Task<List<Guid>> GetAllByOrderGuidAsync(Guid orderGuid, CancellationToken token, string tranName);
}
