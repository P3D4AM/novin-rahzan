using System;
using PacketParser;

namespace EntityCache.Business.Accounting.Reservation;

public class ReservationReportBusiness
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public Guid TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public Guid? OrderGuid { get; set; }

	public decimal OrderSum { get; set; }

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public decimal ReceptionSum { get; set; }

	public string Description { get; set; }

	public int Number { get; set; }
}
