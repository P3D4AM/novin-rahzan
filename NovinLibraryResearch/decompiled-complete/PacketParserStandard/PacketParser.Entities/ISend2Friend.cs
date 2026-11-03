using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ISend2Friend : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid CustomerGuid { get; set; }

	DateTime DateM { get; set; }

	string DateSh { get; }

	string Email { get; set; }

	string Family { get; set; }

	short Gender { get; set; }

	string Ip { get; set; }

	string Message { get; set; }

	string Mob { get; set; }

	string Name { get; set; }

	long SmsId { get; set; }

	short SmsSt { get; set; }

	int Type { get; set; }
}
