using System;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IHesabTypeRepository : IRepository<HesabTypeBusiness>, INoGuidRepository<HesabTypeBusiness>, IDisposable
{
}
