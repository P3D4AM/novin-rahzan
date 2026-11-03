using System;
using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Filters;

public class ProductSerialFilter
{
	public bool AddNewSerialOnSell { get; set; }

	public Guid ProductGuid { get; set; } = Guid.Empty;


	public Guid? ProductGroupGuid { get; set; }

	public Guid? DepotGuid { get; set; }

	public List<IPrdSerial> CurrentSerial { get; set; }

	public int Count { get; set; }

	public bool? Status { get; set; }

	public string Serial { get; set; } = "";


	public Document AsType { get; set; }

	public Guid DetailGuid { get; set; } = Guid.Empty;


	public bool? IsMojod { get; set; }

	public DateTime? DateM { get; set; }

	public DateTime? GarrantyStartDate { get; set; }

	public DateTime? GarrantyEndDate { get; set; }
}
