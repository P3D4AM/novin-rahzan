namespace PacketParser.BusinessLayer.Models.Product;

public class ProductAttributeAmountDetailModel
{
	public string ColumnAttributeValue { get; set; }

	public decimal Amount { get; set; }

	public string AmountForDisplay => Amount.NpToString("N0", 0);
}
