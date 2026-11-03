using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.TaxService;
using PacketParser.Records.TaxService;

namespace PacketParser.TaxService;

public interface ITaxApiConnection : INpDisposable, IDisposable
{
	WebDepartment DepInfo { get; }

	WebUser UserInfo { get; }

	string TaxClientId { get; }

	CancellationToken Token { get; }

	event Action<object, string> OnDisposed;

	Task<ReturnedSaveFuncInfoWithValue<TaxInvoiceTrackingEventArgs>> TrackingOrderAsync(InvoiceTrackingEventArgs tracking, ConnectionData connection);

	Task<ReturnedSaveFuncInfoWithValue<TaxInvoiceResultEventArgs>> SendAsync(WebOrder order, int orderId);

	Task<TaxEconomicCodeRecord> GetEconomicCodeInformationAsync(string clientId);
}
