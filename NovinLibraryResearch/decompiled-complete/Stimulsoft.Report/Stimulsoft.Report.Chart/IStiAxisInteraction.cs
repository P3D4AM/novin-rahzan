using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiAxisInteraction : ICloneable, IStiJsonReportObject, IStiDefault
{
	bool ShowScrollBar { get; set; }

	bool RangeScrollEnabled { get; set; }
}
