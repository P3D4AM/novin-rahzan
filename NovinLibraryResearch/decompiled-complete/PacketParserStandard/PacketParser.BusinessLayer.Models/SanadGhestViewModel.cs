using System;

namespace PacketParser.BusinessLayer.Models;

public class SanadGhestViewModel
{
	public Guid TafsilGuid { get; set; }

	public long TafsilCode { get; set; }

	public string TafsilName { get; set; }

	public int AghsatCount { get; set; }

	public decimal TotalAghsatSum { get; set; }

	public decimal SanadDebitSum { get; set; }

	public decimal SanadCreditSum { get; set; }
}
