using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IProductVahedRepository : ISyncRepository<PrdVahedBusiness>, IEditableDelivery, IRepository<PrdVahedBusiness>, INoGuidRepository<PrdVahedBusiness>, IDisposable
{
	Task<PrdVahedBusiness> GetAsync(string name, CancellationToken token, string tranName);
}
