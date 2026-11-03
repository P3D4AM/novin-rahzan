using System;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Product;

namespace PacketParser.Entities;

public class WebProductPriceRelation : IProductPriceRelation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[JsonIgnore]
	public bool Status { get; set; }

	public Guid ProductGuid { get; set; }

	public decimal Price { get; set; }

	public decimal Quantity { get; set; }

	public decimal Profit { get; set; }

	public decimal KasrRial { get; set; }

	public Guid PriceLevelGuid { get; set; }

	public EnPriceLevelType Type { get; set; }

	public Guid? UnitGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }
}
