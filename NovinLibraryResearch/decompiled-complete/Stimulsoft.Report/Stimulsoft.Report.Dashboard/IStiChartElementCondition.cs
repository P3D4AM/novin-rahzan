using System.Drawing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiChartElementCondition
{
	StiChartConditionalField Field { get; set; }

	string KeyValueMeter { get; set; }

	StiFilterDataType DataType { get; set; }

	StiFilterCondition Condition { get; set; }

	string Value { get; set; }

	Color Color { get; set; }

	StiMarkerType MarkerType { get; set; }

	float MarkerAngle { get; set; }

	bool IsExpression { get; set; }
}
