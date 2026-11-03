using System;
using EntityCache.Business.OrderDepot.OpenInvoices;

namespace EntityCache.Core.Repository.Accounting.Order.OpenInvoices;

public interface IOpenInvoicePlacementRelationRepository : INoGuidRepository<OpenInvoicePlacementRelationBusiness>, IDisposable
{
}
