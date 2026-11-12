using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiRectanglePrimitiveDesigner : StiComponentDesigner
{
	public override StiAction GetActionFromPoint(double x, double y, StiComponent component, bool forGetDesigner = false)
	{
		StiRectanglePrimitive stiRectanglePrimitive = component as StiRectanglePrimitive;
		double num = component.Page.Unit.ConvertFromHInches(3f + stiRectanglePrimitive.Size);
		RectangleD clientRectangle = component.ClientRectangle;
		RectangleD clientRectangle2 = component.ClientRectangle;
		clientRectangle.Inflate(num, num);
		clientRectangle2.Inflate(0.0 - num, 0.0 - num);
		if (clientRectangle.Contains(x, y) && !clientRectangle2.Contains(x, y))
		{
			StiAction stiAction = StiActionUtils.PointInRect(component.Page.Unit.ConvertFromHInches(4.0), x, y, component.ClientRectangle, component.SelectRectangle, component.IsSelected, component.Locked || component.Inherited || !StiRestrictionsHelper.IsAllowChangePosition(component));
			if (stiAction == StiAction.None)
			{
				stiAction = StiAction.Move;
			}
			switch (stiAction)
			{
			case StiAction.Move:
				if (!StiRestrictionsHelper.IsAllowMove(component) && !forGetDesigner)
				{
					return StiAction.None;
				}
				return stiAction;
			case StiAction.Select:
				if (!StiRestrictionsHelper.IsAllowSelect(component))
				{
					return StiAction.None;
				}
				return stiAction;
			default:
				if (!StiRestrictionsHelper.IsAllowResize(component))
				{
					return StiAction.None;
				}
				return stiAction;
			}
		}
		return StiAction.None;
	}

	public override StiAction PointInRect(IStiDesignerBase designer, StiComponent comp, int x, int y)
	{
		if (!StiRestrictionsHelper.IsAllowChangePosition(comp))
		{
			return StiAction.None;
		}
		StiRectanglePrimitive stiRectanglePrimitive = comp as StiRectanglePrimitive;
		double num = comp.Page.Unit.ConvertFromHInches(3f + stiRectanglePrimitive.Size);
		double x2 = designer.XToPage(x);
		double y2 = designer.YToPage(y);
		RectangleD clientRectangle = comp.ClientRectangle;
		RectangleD clientRectangle2 = comp.ClientRectangle;
		clientRectangle.Inflate(num, num);
		clientRectangle2.Inflate(0.0 - num, 0.0 - num);
		if (clientRectangle.Contains(x2, y2) && !clientRectangle2.Contains(x2, y2))
		{
			StiAction stiAction = base.PointInRect(designer, comp, x, y);
			if (stiAction == StiAction.None)
			{
				stiAction = StiAction.Move;
			}
			switch (stiAction)
			{
			case StiAction.Move:
				if (!StiRestrictionsHelper.IsAllowMove(comp))
				{
					return StiAction.None;
				}
				return stiAction;
			case StiAction.Select:
				if (!StiRestrictionsHelper.IsAllowSelect(comp))
				{
					return StiAction.None;
				}
				return stiAction;
			default:
				if (!StiRestrictionsHelper.IsAllowResize(comp))
				{
					return StiAction.None;
				}
				return stiAction;
			}
		}
		return StiAction.None;
	}

	public StiRectanglePrimitiveDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
