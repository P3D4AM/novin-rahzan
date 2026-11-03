using System;
using PacketParser.Payamak.Interfaces.Entities;

namespace PacketParser.Payamak.Entities;

public class BaseSmsTemplate : ISmsTemplate
{
	public int Id { get; set; }

	public string Title { get; set; }

	public string Content { get; set; }

	public DateTime DateRegister { get; set; }

	public string GroupName { get; set; }

	public int Count { get; set; }

	public int TopSt { get; set; }

	public int EndSt { get; set; }

	public bool Status { get; set; }
}
