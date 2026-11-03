using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_ReceiptDetailRepository : IRepository<Scale_ReceiptDetailBusiness>, INoGuidRepository<Scale_ReceiptDetailBusiness>, IDisposable
{
}
