using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Filters.Khazane;

namespace EntityCache.Core.Repository.Accounting.Aghsat;

public interface IAghsatShRepository : ISyncRepository<AghsatShBusiness>, IEditableDelivery, IRepository<AghsatShBusiness>, INoGuidRepository<AghsatShBusiness>, IDisposable
{
	Task<int> NextId2Async(CancellationToken token, string tranName);

	Task<IEnumerable<AghsatShBusiness>> GetAllAsync(AghsatShFilter filter, CancellationToken token, string tranName);

	Task<AghsatShBusiness> GetAsync(int id2, CancellationToken token, string tranName);

	Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName);

	Task<List<AghsatSHSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdatePardakhtInfoAsync(Guid? pardakhtGuid, CancellationToken token, string tranName);
}
