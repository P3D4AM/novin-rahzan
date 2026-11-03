using System.Collections.Generic;
using EntityCache.Business;
using PacketParser.BusinessLayer.Models.Product;

namespace EntityCache.Models.Accounting.Prd;

public class ProductPriceReportContainer
{
	public ProductCompactBusiness Product { get; set; }

	public List<PriceRelationModel> PriceRelationList { get; set; }
}
