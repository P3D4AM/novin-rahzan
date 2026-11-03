using System.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard;

public interface IStiIndicatorElementCondition
{
	StiIndicatorFieldCondition Field { get; set; }

	StiFilterCondition Condition { get; set; }

	string Value { get; set; }

	StiIconAlignment IconAlignment { get; set; }

	StiIconAlignment TargetIconAlignment { get; set; }

	Color IconColor { get; set; }

	StiFontIcons Icon { get; set; }

	byte[] CustomIcon { get; set; }

	StiIndicatorConditionPermissions Permissions { get; set; }

	Font Font { get; set; }

	Color TextColor { get; set; }

	Color BackColor { get; set; }

	Color TargetIconColor { get; set; }

	StiFontIcons TargetIcon { get; set; }
}
