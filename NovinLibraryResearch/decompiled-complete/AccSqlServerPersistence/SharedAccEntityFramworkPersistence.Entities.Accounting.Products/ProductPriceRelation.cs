using System;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Product;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Products;

[Table("Acc_ProductPriceRelation")]
public class ProductPriceRelation : IProductPriceRelation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[NotMapped]
	private DateTime _modified;

	public Guid Guid { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	[NotMapped]
	public bool Status { get; set; }

	public Guid ProductGuid { get; set; }

	public decimal Price { get; set; }

	public decimal Quantity { get; set; }

	public decimal Profit { get; set; }

	public decimal KasrRial { get; set; }

	public Guid PriceLevelGuid { get; set; }

	public Guid? UnitGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public EnPriceLevelType Type { get; set; }

	public virtual Product ProductItem { get; set; }

	public virtual ProductPriceLevel PriceLevelItem { get; set; }

	public virtual ProductVahed UnitItem { get; set; }
}
