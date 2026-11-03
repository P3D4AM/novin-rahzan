using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiInterlacing : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiInterlacingCoreXF Core { get; set; }

	bool AllowApplyStyle { get; set; }

	StiBrush InterlacedBrush { get; set; }

	bool Visible { get; set; }

	IStiArea Area { get; set; }
}
