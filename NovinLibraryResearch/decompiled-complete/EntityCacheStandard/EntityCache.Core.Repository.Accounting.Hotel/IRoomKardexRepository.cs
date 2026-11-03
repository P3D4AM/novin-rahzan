using System;
using EntityCache.Business.Accounting.Hotel;

namespace EntityCache.Core.Repository.Accounting.Hotel;

public interface IRoomKardexRepository : IRepository<RoomKardexBusiness>, INoGuidRepository<RoomKardexBusiness>, IDisposable
{
}
