using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiPictorialArea : IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	bool Actual { get; set; }
}
