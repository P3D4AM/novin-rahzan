using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiSeriesTopN : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiTopNMode Mode { get; set; }

	int Count { get; set; }

	bool ShowOthers { get; set; }

	string OthersText { get; set; }
}
