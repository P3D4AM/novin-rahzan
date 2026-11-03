using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_ViewSanadDetail")]
public class ViewSanadDetail : IViewSanadDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string? Description { get; set; }

	public Guid? AsGuid { get; set; }

	public Document AsType { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public decimal Qt { get; set; }

	public Guid? DecimalGuid { get; set; }

	public string? RowDesc { get; set; }

	public Guid KolGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public Guid? TafsilGuid4 { get; set; }

	public Guid CashierGuid { get; set; }

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

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public string Serial { get; set; }

	public string? MetaData { get; set; }

	public DecimalEntity DecimalItem { get; set; }

	public Kol KolItem { get; set; }

	public Moein MoeinItem { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Tafsil Tafsil2 { get; set; }

	public Tafsil Tafsil3 { get; set; }

	public Tafsil Tafsil4 { get; set; }

	public Cashier CashierItem { get; set; }

	public ICollection<DepotReceiptDetail> DepotReceiptDetail { get; set; }
}
