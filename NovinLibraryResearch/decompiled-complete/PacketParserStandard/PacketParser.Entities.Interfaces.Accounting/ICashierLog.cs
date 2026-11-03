using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ICashierLog : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid SanadGuid { get; set; }

	EnLog SanadType { get; set; }

	string Title { get; set; }

	DateTime DateM { get; set; }

	Guid CashierGuid { get; set; }

	byte[]? OldSerializedData { get; set; }

	byte[]? NewSerializedData { get; set; }

	ENAction Action { get; set; }

	EnSystem System { get; set; }

	DateTime? DateUtc { get; set; }

	short StatusId { get; set; }

	ReturnedState Level { get; set; }

	ENSource Source { get; set; }

	string ApplicationVersion { get; set; }
}
