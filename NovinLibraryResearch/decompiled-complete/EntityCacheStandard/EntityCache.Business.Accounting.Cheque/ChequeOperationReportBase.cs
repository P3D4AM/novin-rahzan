using System;
using PacketParser;

namespace EntityCache.Business.Accounting.Cheque;

public abstract class ChequeOperationReportBase
{
	public Guid Guid { get; set; }

	public Guid GuidCheque { get; set; }

	public decimal Price { get; set; }

	public string TafsilName { get; set; }

	public string Number { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public int SanadNO { get; set; }

	public short Ordinal { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public abstract string OldStateStr { get; }

	public abstract string NewStateStr { get; }
}
