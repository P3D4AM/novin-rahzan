using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_ScaleWeightRepository : IRepository<Scale_ScaleWeightBusiness>, INoGuidRepository<Scale_ScaleWeightBusiness>, IDisposable
{
}
