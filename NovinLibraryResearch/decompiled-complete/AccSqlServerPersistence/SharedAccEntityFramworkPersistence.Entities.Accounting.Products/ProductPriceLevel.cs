using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Product;
using SharedAccEntityFramworkPersistence.Entities.Customer;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Products;

[Table("Acc_ProductPriceLevel")]
public class ProductPriceLevel : IProductPriceLevel, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[MaxLength(500)]
	[Required]
	public string? Name { get; set; }

	[Required]
	public string? Description { get; set; }

	public short Ordinal { get; set; }

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

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public EnPriceLevelType Type { get; set; }

	public decimal DiscountCoefficient { get; set; }

	public bool IsSystem { get; set; }

	public bool IsDefault { get; set; }

	public ICollection<ProductPriceRelation> ProductPriceRelation { get; set; }

	public ICollection<OrderDetail> OrderDetail { get; set; }

	public ICollection<Order> Order { get; set; }

	public ICollection<CustomerRole> CustomerRole { get; set; }
}
