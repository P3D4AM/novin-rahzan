using System.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiTableElementCondition
{
	string[] KeyDataFieldMeters { get; set; }

	string[] KeyDestinationMeters { get; set; }

	StiFilterDataType DataType { get; set; }

	StiFilterCondition Condition { get; set; }

	string Value { get; set; }

	StiTableConditionPermissions Permissions { get; set; }

	Font Font { get; set; }

	Color ForeColor { get; set; }

	Color BackColor { get; set; }

	bool IsExpression { get; set; }
}
