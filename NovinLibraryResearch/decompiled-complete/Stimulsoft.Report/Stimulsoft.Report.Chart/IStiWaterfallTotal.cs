using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiWaterfallTotal : ICloneable, IStiJsonReportObject
{
	string Text { get; set; }

	bool Visible { get; set; }
}
