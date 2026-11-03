using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart;

public interface IStiChartFilter : ICloneable
{
	StiFilterCondition Condition { get; set; }

	StiFilterDataType DataType { get; set; }

	StiFilterItem Item { get; set; }

	string Value { get; set; }
}
