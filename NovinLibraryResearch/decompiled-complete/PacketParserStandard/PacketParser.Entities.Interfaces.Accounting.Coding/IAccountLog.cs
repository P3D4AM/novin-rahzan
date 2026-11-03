using System;

namespace PacketParser.Entities.Interfaces.Accounting.Coding;

public interface IAccountLog : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid MoeinGuid { get; set; }

	Guid? TafsilGuid { get; set; }

	Guid AsGuid { get; set; }

	Document AsType { get; set; }

	DateTime Date { get; set; }

	decimal NewAccount { get; set; }

	decimal OldAccount { get; set; }

	string DebuggerInfo { get; set; }

	ENAction Action { get; set; }
}
