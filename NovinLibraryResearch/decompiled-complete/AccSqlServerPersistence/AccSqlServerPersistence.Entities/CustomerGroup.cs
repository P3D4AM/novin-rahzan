using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_CustomerGroup")]
public class CustomerGroup : ICusGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Description { get; set; }

	[Required]
	public string Name { get; set; }

	public bool Special { get; set; }

	public bool Status { get; set; }

	public Guid MoeinDefaultGuid { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public ICollection<Customer> Customers { get; set; }

	public TafsilGroup TafsilGroupItems { get; set; }

	public Moein MoeinItems { get; set; }
}
