using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiAxisRange : ICloneable, IStiJsonReportObject, IStiDefault
{
	double Minimum { get; set; }

	double Maximum { get; set; }

	bool Auto { get; set; }
}
