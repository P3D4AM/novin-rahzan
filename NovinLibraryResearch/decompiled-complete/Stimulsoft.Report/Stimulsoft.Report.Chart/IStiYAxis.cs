using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiYAxis : IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault
{
	StiShowYAxis ShowYAxis { get; set; }
}
