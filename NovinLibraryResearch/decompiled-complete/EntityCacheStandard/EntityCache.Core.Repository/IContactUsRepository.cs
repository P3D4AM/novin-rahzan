using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IContactUsRepository : ISyncRepository<ContactUsBusiness>, IEditableDelivery, IRepository<ContactUsBusiness>, INoGuidRepository<ContactUsBusiness>, IDisposable
{
	Task<IEnumerable<ContactUsBusiness>> GetAllAsync(TypeContact type, bool? status, CancellationToken token, string tranName);
}
