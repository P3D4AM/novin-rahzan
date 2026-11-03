using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Filters;

namespace EntityCache.Core.Repository;

public interface IGardeshTafsilRepository : IRepository<GardeshTafsilBusiness>, INoGuidRepository<GardeshTafsilBusiness>, IDisposable
{
	Task<IEnumerable<GardeshTafsilBusiness>> GetAllAsync(GardeshTafsiliFilter filter, EnSystem system, string tranName, CancellationToken token);

	Task<IEnumerable<GardeshTafsilBusiness>> GetAllFromVSanadDetAsync(List<Guid> listTafsilGuid, EnSystem system, int top, DateTime? date1, DateTime? date2, List<Guid> listMoeinGuid, Guid? kolGuid, bool? status, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> kolGuid, IEnumerable<Guid> moeinGuid, IEnumerable<Guid?> tafsilGuid, short tryCount, string tranName, CancellationToken token);

	Task<decimal> GardeshBallanceAsync(Guid? tafsilGuid, Guid? moeinGuid, DateTime? date1, DateTime? date2, string tranName, CancellationToken token);

	Task<GardeshTafsilBusiness> GetLastTransactionAsync(Guid tafsilGuid, EnSystem system, string tranName, CancellationToken token);
}
