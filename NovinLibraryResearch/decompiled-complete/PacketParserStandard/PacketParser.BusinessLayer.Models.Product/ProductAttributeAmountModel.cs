using System.Collections.Generic;

namespace PacketParser.BusinessLayer.Models.Product;

public class ProductAttributeAmountModel
{
	public string RowAttributeValue { get; set; }

	public List<ProductAttributeAmountDetailModel> ColumnValues { get; set; }
}
