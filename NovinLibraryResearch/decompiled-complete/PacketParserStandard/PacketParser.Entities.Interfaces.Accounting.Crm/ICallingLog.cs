using System;

namespace PacketParser.Entities.Interfaces.Accounting.Crm;

public interface ICallingLog : IHasModified, IHasStatus
{
	string CallerIdNumber { get; set; }

	string Uuid { get; set; }

	string? MasterUuid { get; set; }

	short Rings { get; set; }

	string DialedNumber { get; set; }

	DateTime? PickUpDateM { get; set; }

	DateTime? HangUpDateM { get; set; }

	DateTime DateM { get; set; }

	Guid? CoGuid { get; set; }

	Guid? ExGuid { get; set; }

	new EnumNotification Status { get; set; }

	short StatusId { get; set; }

	string Message { get; set; }

	bool IsIncoming { get; set; }

	bool IsCompleteCall { get; set; }

	string Description { get; set; }

	int Version { get; set; }

	long CalcedNumber { get; }

	DateTime? DateUtc { get; set; }
}
