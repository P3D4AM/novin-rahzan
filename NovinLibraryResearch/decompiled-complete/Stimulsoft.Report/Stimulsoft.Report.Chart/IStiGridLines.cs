using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiGridLines : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiGridLinesCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	Color Color { get; set; }

	Color MinorColor { get; set; }

	StiPenStyle Style { get; set; }

	StiPenStyle MinorStyle { get; set; }

	bool Visible { get; set; }

	bool MinorVisible { get; set; }

	int MinorCount { get; set; }

	IStiArea Area { get; set; }
}
