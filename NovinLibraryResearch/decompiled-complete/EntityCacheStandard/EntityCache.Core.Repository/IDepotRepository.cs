using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IDepotRepository : ISyncRepository<DepotBusiness>, IEditableDelivery, IRepository<DepotBusiness>, INoGuidRepository<DepotBusiness>, IDisposable
{
	Task<DepotBusiness> GetByNameAsync(string name, CancellationToken token, string tranName);
}
