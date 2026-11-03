using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_CustomerTitle")]
public class CustomerTitle : ICusTitle, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Name { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public ICollection<Customer> Customers { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }
}
