using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class OrderFilter
{
	public Document FactorType { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? MoeinGuid { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public bool? IsSent { get; set; }

	public decimal? SumFrom { get; set; }

	public decimal? SumTo { get; set; }

	public decimal? DiscountFrom { get; set; }

	public decimal? DiscountTo { get; set; }

	public decimal? FareFrom { get; set; }

	public decimal? FareTo { get; set; }

	public decimal? VisitorPriceFrom { get; set; }

	public decimal? VisitorPriceTo { get; set; }

	public decimal? TaxFrom { get; set; }

	public decimal? TaxTo { get; set; }

	public string Description { get; set; } = "";


	public bool? IsTemp { get; set; }

	public bool? Status { get; set; } = true;


	public string Search { get; set; } = "";


	public bool? IsLast30 { get; set; }

	public Guid? VisitorTafsilGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public decimal? Id2From { get; set; }

	public decimal? Id2To { get; set; }

	public string Numbers { get; set; } = "";


	public Guid? CustomerGroupGuid { get; set; }

	public List<ENSource> Sources { get; set; }

	public string HostRefrences { get; set; } = "";


	public List<Guid> SelectedOrdersGuid { get; set; }

	public List<ServerStatusForClient> ServerStatuses { get; set; }

	public bool? SanadAffective { get; set; }

	public bool? KardexAffective { get; set; }

	public EnTaxResult? TaxResult { get; set; }

	public int? StatusId { get; set; }

	public string SellerOrderNumber { get; set; }

	public List<Guid> AreaGuidList { get; set; }
}
