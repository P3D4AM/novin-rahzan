using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class TajmieeProductFilter
{
	public Document Type { get; set; }

	public DateTime? Date1 { get; set; }

	public DateTime? Date2 { get; set; }

	public decimal Discount1 { get; set; }

	public decimal Discount2 { get; set; }

	public decimal Sum1 { get; set; }

	public decimal Sum2 { get; set; }

	public decimal Price1 { get; set; }

	public decimal Price2 { get; set; }

	public decimal VisitorPrice1 { get; set; }

	public decimal VisitorPrice2 { get; set; }

	public string RowDescription { get; set; } = "";


	public string Description { get; set; } = "";


	public Guid? CashierGuid { get; set; }

	public int Number1 { get; set; }

	public int Number2 { get; set; }

	public List<int> SelectedNumbers { get; set; }

	public Guid? CustomerSelectedListGuid { get; set; }

	public Guid? ProductSelectedListGuid { get; set; }

	public Guid? CustomerGroupGuid { get; set; }

	public List<Guid> ProductGroupGuidList { get; set; }

	public decimal Tax1 { get; set; }

	public decimal Tax2 { get; set; }

	public decimal Avarez1 { get; set; }

	public decimal Avarez2 { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? VisitorGuid { get; set; }

	public int ShahrId { get; set; }

	public int NahiyeId { get; set; }

	public string MasirAsli { get; set; } = "";


	public string MasirFaree { get; set; } = "";


	public bool? KardexAffective { get; set; } = true;


	public Guid? DepotGuid { get; set; }

	public List<Document> DocumentList { get; set; }
}
