using System;

namespace PacketParser.Payamak.Entities;

public class BaseSmsInBox
{
	public int ID { get; set; }

	public string Message { get; set; }

	public string Number { get; set; }

	public string FixMessage { get; set; }

	public DateTime DateM { get; set; }

	public string DateSH => DateM.MiladiToCalture();

	public int CustomerID { get; set; }

	public string CustomerName { get; set; }

	public string RStr1 { get; set; }

	public string RStr2 { get; set; }

	public string RStr3 { get; set; }

	public string RStr4 { get; set; }

	public string RStr5 { get; set; }

	public int NetAccountId { get; set; }

	public bool ST { get; set; }
}
