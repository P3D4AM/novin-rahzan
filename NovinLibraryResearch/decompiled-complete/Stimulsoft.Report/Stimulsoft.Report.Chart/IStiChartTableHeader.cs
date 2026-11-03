using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiChartTableHeader : ICloneable, IStiJsonReportObject, IStiDefault
{
	string TextAfter { get; set; }

	StiBrush Brush { get; set; }

	Font Font { get; set; }

	Color TextColor { get; set; }

	bool WordWrap { get; set; }

	string Format { get; set; }
}
