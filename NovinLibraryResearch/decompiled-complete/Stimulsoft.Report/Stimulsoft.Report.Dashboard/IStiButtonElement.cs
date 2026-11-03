using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiButtonElement : IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiCornerRadius, IStiSimpleShadow, IStiSimpleBorder
{
	string Text { get; set; }

	bool Checked { get; set; }

	StiButtonShapeType ShapeType { get; set; }

	StiButtonType Type { get; set; }

	StiIconAlignment IconAlignment { get; set; }

	StiButtonStretch Stretch { get; set; }

	StiBrush IconBrush { get; set; }

	IStiButtonElementIconSet GetIconSet();

	IStiButtonVisualStates GetVisualStates();
}
