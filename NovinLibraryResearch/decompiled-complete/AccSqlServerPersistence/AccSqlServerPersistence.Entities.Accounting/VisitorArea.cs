using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_VisitorArea")]
public class VisitorArea : Serializable<IVisitorArea>, IVisitorArea, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	[Required]
	[Range(1, 10)]
	public short Level { get; set; }

	[Required]
	public string Name { get; set; }

	public Guid? ParentGuid { get; set; }

	[Required]
	public string Description { get; set; }

	[Required]
	public bool Status { get; set; }

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

	[Required]
	public long Code { get; set; }

	[Required]
	public long FullCode { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public ICollection<Customer> Customers { get; set; }

	public string? MetaData { get; set; }
}
