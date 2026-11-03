using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Cache.Repository.Accounting;

public interface IUserAccRepository : ISyncRepository<UserAccBusiness>, IEditableDelivery, IRepository<UserAccBusiness>, INoGuidRepository<UserAccBusiness>, IDisposable
{
	Task<UserAccBusiness> GetAsync(string name, CancellationToken token, string tranName);
}
