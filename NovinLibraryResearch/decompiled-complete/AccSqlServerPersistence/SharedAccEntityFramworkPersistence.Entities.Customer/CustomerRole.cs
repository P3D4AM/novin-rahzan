using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;

namespace SharedAccEntityFramworkPersistence.Entities.Customer;

[Table("Acc_CustomerRole")]
public class CustomerRole : ICustomerRole, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
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

	public bool Status { get; set; }

	[MaxLength(500)]
	public string? Name { get; set; }

	[MaxLength(500)]
	public string? WordPressName { get; set; }

	public string? Description { get; set; }

	public Guid? PriceTypeGuid { get; set; }

	public string? MetaData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public ProductPriceLevel ProductPriceLevel { get; set; }

	public ICollection<AccSqlServerPersistence.Entities.Customer> Customers { get; set; }
}
