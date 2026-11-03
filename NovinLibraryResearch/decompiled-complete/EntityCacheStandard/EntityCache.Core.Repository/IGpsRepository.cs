using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IGpsRepository : ISyncRepository<GPSBusiness>, IEditableDelivery, IRepository<GPSBusiness>, INoGuidRepository<GPSBusiness>, IDisposable
{
	Task<List<GPSBusiness>> GetAllNotSentAsync(int count, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAllAsync(IEnumerable<Guid> res, short tryCount, CancellationToken token, string tranName);
}
