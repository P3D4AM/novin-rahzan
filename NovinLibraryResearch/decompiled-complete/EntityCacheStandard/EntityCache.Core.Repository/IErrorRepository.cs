using System;
using PacketParser.Entities;

namespace EntityCache.Core.Repository;

public interface IErrorRepository : IRepository<WebErrorLog>, INoGuidRepository<WebErrorLog>, IDisposable
{
}
