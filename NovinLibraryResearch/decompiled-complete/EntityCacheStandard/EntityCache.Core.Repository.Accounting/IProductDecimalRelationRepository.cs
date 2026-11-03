using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting;

public interface IProductDecimalRelationRepository : INoGuidRepository<ProductDecimalRelationBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(CancellationToken token, string tranName);
}
