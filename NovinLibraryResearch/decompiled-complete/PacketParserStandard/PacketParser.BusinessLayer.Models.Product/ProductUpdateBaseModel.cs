using System;
using System.Collections.Generic;

namespace PacketParser.BusinessLayer.Models.Product;

public class ProductUpdateBaseModel
{
	public Guid ProductGuid { get; set; }

	public List<ProductUpdateDetailModel> DetailList { get; set; }
}
