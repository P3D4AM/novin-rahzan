using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DepotReceipt;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;

[Table("Acc_DepotReceiptDetail")]
public class DepotReceiptDetail : IDepotReceiptDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public decimal Quantity { get; set; }

	public string? Description { get; set; }

	public Guid? V2Guid { get; set; }

	public decimal? V2Quantity { get; set; }

	public int Ordinal { get; set; }

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

	public Guid MasterGuid { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public string? MetaData { get; set; }

	public virtual Product ProductItem { get; set; }

	public virtual Depot DepotItem { get; set; }

	public virtual ProductVahed VahedItem { get; set; }

	public virtual DepotReceipt MasterItem { get; set; }

	public virtual ViewSanadDetail ReferenceItem { get; set; }
}
