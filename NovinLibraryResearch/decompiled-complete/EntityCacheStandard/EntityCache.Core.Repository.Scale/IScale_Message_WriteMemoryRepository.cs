using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_Message_WriteMemoryRepository : IRepository<Scale_Message_WriteMemoryBusiness>, INoGuidRepository<Scale_Message_WriteMemoryBusiness>, IDisposable
{
}
