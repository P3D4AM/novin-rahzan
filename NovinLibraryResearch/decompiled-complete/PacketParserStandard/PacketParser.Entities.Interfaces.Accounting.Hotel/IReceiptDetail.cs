using System;

namespace PacketParser.Entities.Interfaces.Accounting.Hotel;

public interface IReceiptDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	DateTime StartDate { get; set; }

	DateTime EndDate { get; set; }

	decimal Tax { get; set; }

	decimal TaxPercent { get; set; }

	decimal Avarez { get; set; }

	decimal AvarezPercent { get; set; }

	decimal Discount { get; set; }

	decimal DiscountPercent { get; set; }

	decimal ExtraPrice { get; set; }

	decimal Penalty { get; set; }

	decimal PricePerNight { get; set; }

	int ExtraCount { get; set; }
}
