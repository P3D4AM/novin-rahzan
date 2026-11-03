using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface ICustomerTitleRepository : ISyncRepository<CusTitleBusiness>, IEditableDelivery, IRepository<CusTitleBusiness>, INoGuidRepository<CusTitleBusiness>, IDisposable
{
	Task<CusTitleBusiness> GetAsync(string name, CancellationToken token, string tranName);
}
