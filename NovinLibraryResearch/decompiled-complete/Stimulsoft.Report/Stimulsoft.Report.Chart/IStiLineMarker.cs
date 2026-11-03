using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiLineMarker : IStiMarker, ICloneable, IStiFontIconsSeries, IStiJsonReportObject, IStiDefault
{
	double Step { get; set; }
}
