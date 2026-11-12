using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiPointPrimitiveDesigner : StiComponentDesigner
{
	public override StiAction GetActionFromPoint(double x, double y, StiComponent component, bool forGetDesigner = false)
	{
		return StiAction.None;
	}

	public override StiAction PointInRect(IStiDesignerBase designer, StiComponent comp, int x, int y)
	{
		return StiAction.None;
	}

	public StiPointPrimitiveDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
