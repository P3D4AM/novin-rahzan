using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot;
using PacketParser.Entities;

namespace EntityCache.Core.Repository;

public interface IOrderAbadRepository : IRepository<OrderAbadBusiness>, INoGuidRepository<OrderAbadBusiness>, IDisposable
{
	Task<OrderAbadBusiness> GetAsync(Guid DetailGuid, Document OrderType, CancellationToken token, string tranName);
}
