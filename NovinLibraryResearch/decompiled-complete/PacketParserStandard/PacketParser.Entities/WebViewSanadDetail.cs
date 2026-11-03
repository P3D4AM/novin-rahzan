using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebViewSanadDetail : IViewSanadDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public Guid? AsGuid { get; set; }

	public Document AsType { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public decimal Qt { get; set; }

	public Guid? DecimalGuid { get; set; }

	public string RowDesc { get; set; }

	public Guid KolGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public Guid? TafsilGuid4 { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public string ContextGuid { get; set; }

	public string Serial { get; set; }
}
