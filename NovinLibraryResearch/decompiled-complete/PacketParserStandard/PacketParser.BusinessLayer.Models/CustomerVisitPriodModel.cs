using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class CustomerVisitPriodModel
{
	public Guid TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public long TafsilCode { get; set; }

	public DateTime LastVisitDateM { get; set; }

	public string LastVisitDateSh => LastVisitDateM.MiladiToCalture();

	public DateTime NextVisitDateM { get; set; }

	public string NextVisitDateSh => NextVisitDateM.MiladiToCalture();

	public EnVisitPeriod PriodType { get; set; }

	public string PriodTypeStr => PriodType.GetDisplay();

	public string PriodForDisplay => $"{PriodTypeStr} ({Term})";

	public int Term { get; set; }

	public decimal Account { get; set; }

	public string TafsilNumber { get; set; }
}
