using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_ProductGroup")]
public class ProductGroup : Serializable<IPrdGroup>, IPrdGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public int Level { get; set; }

	[Required]
	public string? Slug { get; set; }

	public long Code { get; set; }

	[Required]
	public string? Description { get; set; }

	public long FullCode { get; set; }

	public string? HtmlFull { get; set; }

	public string? HtmlQuick { get; set; }

	[Required]
	public string? Name { get; set; }

	[Required]
	[MaxLength(200)]
	public string? ShortName { get; set; }

	[Required]
	public string? Pic { get; set; }

	public bool Special { get; set; }

	public bool Status { get; set; }

	public string? TechInfoData { get; set; }

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

	public EnProductType Type { get; set; }

	public Guid? ParentGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public ICollection<ProductGroupRelation> ProductGroupRelations { get; set; }

	public ICollection<ProductGroupSelectedDetails> ProductGroupSelectedDetails { get; set; }

	public ICollection<Placement> Placements { get; set; }
}
