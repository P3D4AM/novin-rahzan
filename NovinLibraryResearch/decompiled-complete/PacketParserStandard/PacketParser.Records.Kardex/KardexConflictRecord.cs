using System;
using PacketParser.Entities;

namespace PacketParser.Records.Kardex;

public class KardexConflictRecord
{
	public string ProductName { get; set; }

	public string ProductCode { get; set; }

	public Guid ProductGuid { get; set; }

	public Document SanadType { get; set; }

	public string SanadTypeForDisplay => SanadType.GetDisplay().GetTranslate();

	public Guid SanadGuid { get; set; }

	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public decimal TrustUnitPrice { get; set; }

	public decimal CurrentUnitPrice { get; set; }
}
