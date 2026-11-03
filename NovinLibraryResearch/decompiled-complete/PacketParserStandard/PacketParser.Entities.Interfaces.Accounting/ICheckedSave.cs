using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ICheckedSave : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid EntityGuid { get; set; }

	Guid DetailGuid { get; set; }

	bool IsDebit { get; set; }

	checkedTablReportType ReportType { get; set; }

	DateTime DateM { get; set; }
}
