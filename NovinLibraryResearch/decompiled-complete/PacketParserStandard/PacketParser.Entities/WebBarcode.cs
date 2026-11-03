using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebBarcode : Serializable<WebBarcode>, IBarcode, IHasChangeStatus
{
	public string Name { get; set; } = "";


	public string BarCode { get; set; } = "";


	public string Description { get; set; } = "";


	public DateTime CreateDate { get; set; } = DateTime.Now;


	[Obsolete("", true)]
	public DateTime ExpireDate { get; set; } = DateTime.MaxValue;


	[Obsolete("", true)]
	public short DepotId { get; set; }

	[Obsolete("", true)]
	public EnPriceType PriceType { get; set; } = EnPriceType.sell1;


	public Guid? PriceTypeGuid { get; set; }

	public decimal Price { get; set; }

	public BarcodePriceType Type { get; set; } = BarcodePriceType.ComboPrice;


	public decimal Quantity { get; set; } = 1m;


	public short PriceT { get; set; }

	public Guid? DepotGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public short Ordinal { get; set; }
}
