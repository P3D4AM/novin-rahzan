using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IDescRepository : ISyncRepository<DescBusiness>, IEditableDelivery, IRepository<DescBusiness>, INoGuidRepository<DescBusiness>, IDisposable
{
	Task<IEnumerable<DescBusiness>> GetAllAsync(DescSection section, bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<DescBusiness>> GetAllAsync(string search, DescSection section, CancellationToken token, bool? status, string tranName);
}
