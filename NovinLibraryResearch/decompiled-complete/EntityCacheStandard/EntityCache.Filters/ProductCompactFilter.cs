using System;
using System.Collections.Generic;
using System.ComponentModel;
using EntityCache.Business;
using PacketParser.Entities;

namespace EntityCache.Filters;

public class ProductCompactFilter
{
	private ProductSearchOption? productSearchOption;

	[DefaultValue(false)]
	public bool LoadBarcodeInsteadOfCode { get; set; }

	public string SearchText { get; set; } = "";


	public OrderBusiness Order { get; set; }

	public List<Guid> GroupGuids { get; set; }

	public List<Guid> CashierAccessGroupGuids { get; set; }

	public bool? Status { get; set; } = true;


	public SearchLocation? SearchLocation { get; set; }

	public ProductSearchOption? ProductSearchOption
	{
		get
		{
			return productSearchOption;
		}
		set
		{
			productSearchOption = value;
		}
	}

	public ProductSort? Sort { get; set; }

	public bool? HasAmount { get; set; }

	public bool? UnderOrderPoint { get; set; }

	public bool? IsTax { get; set; }

	public Guid? MasterGuid { get; set; }

	public bool LoadJustParents { get; set; } = true;


	public int? MaxCount { get; set; }

	public List<EnProductKind> KindList { get; set; }

	public DateTime? CreationDate { get; set; }

	public bool? HasMultiVahed { get; set; }

	public bool? HasDiscount { get; set; }

	public Guid? TaxClassGuid { get; set; }

	public bool? HasOrderQuantity { get; set; }

	public bool? IsSpecial { get; set; }
}
