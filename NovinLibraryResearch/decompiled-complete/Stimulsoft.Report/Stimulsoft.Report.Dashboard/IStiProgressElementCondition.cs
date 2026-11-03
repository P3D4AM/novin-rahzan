using System.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiProgressElementCondition
{
	StiFilterCondition Condition { get; set; }

	StiProgressFieldCondition Field { get; set; }

	string Value { get; set; }

	StiProgressConditionPermissions Permissions { get; set; }

	Color TextColor { get; set; }

	Color Color { get; set; }

	Color TrackColor { get; set; }

	Font Font { get; set; }
}
