using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.App;

public interface IStiCheckBoxUI : IStiDashboardElementStyle, IStiCornerRadius
{
	string Text { get; set; }

	StiCheckState Checked { get; set; }
}
