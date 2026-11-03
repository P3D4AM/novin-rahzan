using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;

namespace EntityCache.Core.Repository;

public interface IBarcodeRepository : INoGuidRepository<BarcodeBusiness>, IDisposable
{
	Task<BarcodeBusiness> GetAsync(string barcode, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsyncByPrdGuid(Guid prdGuid, short tryCount, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfo> RemoveAsync(string barCode, short tryCount, string tranName, CancellationToken token);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(List<Guid> prdGuid, short tryCount, string tranName, CancellationToken token);

	Task<List<BarcodeBusiness>> GetByPrdGuidAsync(List<Guid> prdGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> FillBarcodesAsync(CancellationToken token, string tranName);

	Task<IEnumerable<BarcodeModel>> GetAllConflictsAsync(CancellationToken token, string tranName);
}
