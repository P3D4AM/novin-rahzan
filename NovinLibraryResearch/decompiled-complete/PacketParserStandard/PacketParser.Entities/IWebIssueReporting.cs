using System;

namespace PacketParser.Entities;

public interface IWebIssueReporting
{
	Guid Guid { get; set; }

	ENSource Source { get; set; }

	string Version { get; set; }

	DateTime InsertDate { get; set; }

	string Title { get; set; }

	string Discription { get; set; }

	Guid CashierGuid { get; set; }

	EnIssueReportingLevel Level { get; set; }

	EnIssueReportingStatus Status { get; set; }

	string CashierName { get; set; }
}
