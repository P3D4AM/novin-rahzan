using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebOrderCompact : Serializable<WebOrderCompact>, INpDisposable, IDisposable
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public DateTime DateInsert { get; set; }

	public string Description { get; set; }

	public decimal SumService { get; set; }

	public decimal SumKala { get; set; }

	public decimal Discount { get; set; }

	public Guid TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public string UserAccName { get; set; }

	public string Visitor1Name { get; set; }

	public string Vistor2Name { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public string ShortDeliveryMessage { get; set; }

	public string DeliveryMessage { get; set; }

	public ENSource Source { get; set; }

	public bool? DeliveryStatus { get; set; }

	public decimal TotalSum { get; set; }

	public Document FactorType { get; set; }
}
