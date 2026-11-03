using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Dashboard;

public interface IStiShapeElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval
{
	StiShapeTypeService ShapeType { get; set; }

	Color Stroke { get; set; }

	StiBrush Fill { get; set; }

	float Size { get; set; }
}
