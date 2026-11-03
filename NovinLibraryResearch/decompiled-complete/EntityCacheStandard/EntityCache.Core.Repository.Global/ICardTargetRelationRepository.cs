using System;
using EntityCache.Business.Global;

namespace EntityCache.Core.Repository.Global;

public interface ICardTargetRelationRepository : INoGuidRepository<CardTargetRelationBusiness>, IDisposable
{
}
