using System;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IFareOrderCredit : IRepository<FareOrderCreditBusiness>, INoGuidRepository<FareOrderCreditBusiness>, IDisposable
{
}
