using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiXAxis : IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault
{
	bool ShowEdgeValues { get; set; }

	StiShowXAxis ShowXAxis { get; set; }

	IStiAxisDateTimeStep DateTimeStep { get; set; }
}
