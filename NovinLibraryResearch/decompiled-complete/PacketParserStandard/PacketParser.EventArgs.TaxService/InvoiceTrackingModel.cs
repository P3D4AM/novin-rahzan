using System.Collections.Generic;

namespace PacketParser.EventArgs.TaxService;

public class InvoiceTrackingModel
{
	public string ConfirmationReferenceId { get; set; }

	public bool Success { get; set; }

	public List<InvoiceTrackingResultModel> Error { get; set; }
}
