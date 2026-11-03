using PacketParser;

namespace EntityCache.Business.Accounting.TTMS;

public class TTMSDetailAggregateDetailBusiness
{
	public decimal Sum { get; set; }

	public decimal Tax { get; set; }

	public decimal TotalSum { get; set; }

	public string SumForDisplay => Sum.NpToString("N0", 0);

	public string TaxForDisplay => Tax.NpToString("N0", 0);

	public string TotalSumForDisplay => TotalSum.NpToString("N0", 0);
}
