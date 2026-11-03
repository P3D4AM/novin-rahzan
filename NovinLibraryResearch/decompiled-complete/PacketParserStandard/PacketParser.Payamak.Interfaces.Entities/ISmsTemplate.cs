using System;

namespace PacketParser.Payamak.Interfaces.Entities;

public interface ISmsTemplate
{
	int Id { get; set; }

	string Title { get; set; }

	string Content { get; set; }

	DateTime DateRegister { get; set; }

	string GroupName { get; set; }

	int Count { get; set; }

	int TopSt { get; set; }

	int EndSt { get; set; }

	bool Status { get; set; }
}
