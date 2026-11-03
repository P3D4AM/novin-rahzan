using System;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class KardexFilter
{
	public Guid? MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? PrdGuid { get; set; }

	public Guid? DepotGuid { get; set; }

	public Document? FactorType { get; set; }

	public Guid? PrdGroupGuid { get; set; }

	public string Description { get; set; } = "";


	public Guid? UserGuid { get; set; }

	public string Serial { get; set; } = "";


	public DateTime? Date1 { get; set; }

	public DateTime? Date2 { get; set; }

	public decimal? UnitPrice1 { get; set; }

	public ENNumberSelectionType UnitPrice1Type { get; set; } = ENNumberSelectionType.MoreThan;


	public decimal? UnitPrice2 { get; set; }

	public ENNumberSelectionType UnitPrice2Type { get; set; } = ENNumberSelectionType.LessThan;


	public short? CountForDisplay { get; set; }

	public IOrder Order { get; set; }

	public bool? KardexAffective { get; set; } = true;

}
