using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_ReceiptHeaderRepository : IRepository<Scale_ReceiptHeaderBusiness>, INoGuidRepository<Scale_ReceiptHeaderBusiness>, IDisposable
{
}
