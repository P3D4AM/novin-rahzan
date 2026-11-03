using System;
using EntityCache.Business.OrderDepot.OpenInvoices;

namespace EntityCache.Core.Repository.Accounting.Order.OpenInvoices;

public interface IOpenInvoiceOrderRelationRepository : INoGuidRepository<OpenInvoiceOrderRelationBusiness>, IDisposable
{
}
