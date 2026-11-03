using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_Message_ReadSaleRepository : IRepository<Scale_Message_ReadSaleBusiness>, INoGuidRepository<Scale_Message_ReadSaleBusiness>, IDisposable
{
}
