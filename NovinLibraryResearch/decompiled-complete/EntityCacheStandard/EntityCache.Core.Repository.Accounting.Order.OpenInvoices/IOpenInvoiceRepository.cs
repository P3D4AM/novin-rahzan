using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OpenInvoices;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting.Order.OpenInvoices;

public interface IOpenInvoiceRepository : IRepository<OpenInvoiceBusiness>, INoGuidRepository<OpenInvoiceBusiness>, IDisposable, IEditableDelivery
{
	Task<int> GetNextNumberAsync(CancellationToken token, string tranName);

	Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName);

	Task<OpenInvoiceBusiness> GetByOrderGuidAsync(Guid orderGuid, CancellationToken token, string tranName);
}
