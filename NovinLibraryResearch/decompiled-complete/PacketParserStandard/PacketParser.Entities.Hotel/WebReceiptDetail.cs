using System;
using System.Collections.Generic;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;

namespace PacketParser.Entities.Hotel;

public class WebReceiptDetail : Serializable<WebReceiptDetail>, IReceiptDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public decimal Tax { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal Avarez { get; set; }

	public decimal AvarezPercent { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPercent { get; set; }

	public decimal ExtraPrice { get; set; }

	public decimal Penalty { get; set; }

	public decimal PricePerNight { get; set; }

	public int ExtraCount { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid RoomGuid { get; set; }

	public List<WebProductCompact> ServiceList { get; set; }
}
