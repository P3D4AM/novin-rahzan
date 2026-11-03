using System;

namespace PacketParser.BusinessLayer.Models;

public class MoeinTafsilReportModel
{
	public Guid MoeinGuid { get; set; }

	public string MoeinName { get; set; }

	public long MoeinCode { get; set; }

	public decimal MoeinAccount { get; set; }

	public Guid TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public long TafsilCode { get; set; }

	public decimal TafsilAccount { get; set; }

	public decimal Debit_FD { get; set; }

	public decimal Credit_FD { get; set; }

	public int Count_FD { get; set; }

	public decimal Debit_TD { get; set; }

	public decimal Credit_TD { get; set; }

	public int Count_TD { get; set; }

	public decimal Debit_ED { get; set; }

	public decimal Credit_ED { get; set; }

	public int Count_ED { get; set; }
}
