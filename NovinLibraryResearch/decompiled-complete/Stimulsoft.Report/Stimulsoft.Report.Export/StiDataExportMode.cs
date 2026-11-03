using System;

namespace Stimulsoft.Report.Export;

[Flags]
public enum StiDataExportMode
{
	Data = 1,
	Headers = 2,
	DataAndHeaders = 3,
	Footers = 4,
	HeadersFooters = 6,
	DataAndHeadersFooters = 7,
	AllBands = 0xF
}
