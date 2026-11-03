using System;

namespace Stimulsoft.Report;

[Flags]
public enum StiStyleComponentPlacement
{
	None = 0,
	ReportTitle = 1,
	ReportSummary = 2,
	PageHeader = 4,
	PageFooter = 8,
	GroupHeader = 0x10,
	GroupFooter = 0x20,
	Header = 0x40,
	Footer = 0x80,
	ColumnHeader = 0x100,
	ColumnFooter = 0x200,
	Data = 0x400,
	DataEvenStyle = 0x800,
	DataOddStyle = 0x1000,
	Table = 0x2000,
	Hierarchical = 0x4000,
	Child = 0x8000,
	Empty = 0x10000,
	Overlay = 0x20000,
	Panel = 0x40000,
	Page = 0x80000,
	AllExeptStyles = 0xFE7FF
}
