using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting;

public interface ITafsilNumberRepository : INoGuidRepository<TafsilNumberBusiness>, IDisposable
{
	Task<List<TafsilNumberBusiness>> GetAllAsync(Guid tafsilGuid, CancellationToken token, string tranName);

	Task<List<TafsilNumberReportBusiness>> GetAllReportAsync(CancellationToken token, string tranName);

	Task<List<TafsilNumberBusiness>> GetInfoAsync(long number, bool? status, CancellationToken token, string tranName);

	Task<List<string>> GetTitleListAsync(CancellationToken token, string tranName);

	Task<List<TafsilNumberBusiness>> GetInfoByCalcNumberAsync(long number, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(List<Guid> tafsilGuids, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid tafsilGuid, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(Guid tafsilGuid, long Number, short tryCount, CancellationToken token, string tranName);

	Task<List<TafsilNumberBusiness>> GetAllAsync(List<Guid> tafsilGuids, CancellationToken token, string tranName);
}
