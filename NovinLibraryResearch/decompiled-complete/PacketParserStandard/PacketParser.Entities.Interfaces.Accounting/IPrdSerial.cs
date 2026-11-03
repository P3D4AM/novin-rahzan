using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IPrdSerial : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime InsertDate { get; set; }

	string Serial { get; set; }

	string GarrantyCode { get; set; }

	DateTime? StartDate { get; set; }

	DateTime? EndDate { get; set; }

	Guid ProductGuid { get; set; }

	bool IsTransfer { get; set; }

	string TechnicalInfo1 { get; set; }

	string TechnicalInfo2 { get; set; }

	string TechnicalInfo3 { get; set; }
}
