using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiVerticalLinePrimitiveDesigner : StiComponentDesigner
{
	public override StiAction GetActionFromPoint(double x, double y, StiComponent component, bool forGetDesigner = false)
	{
		StiVerticalLinePrimitive stiVerticalLinePrimitive = component as StiVerticalLinePrimitive;
		double num = component.Page.Unit.ConvertFromHInches(3f + stiVerticalLinePrimitive.Size);
		RectangleD clientRectangle = component.ClientRectangle;
		StiAction stiAction = StiAction.None;
		stiAction = ((clientRectangle.Y + num < y && clientRectangle.Bottom - num > y && clientRectangle.X - num < x && clientRectangle.X + num > x) ? StiAction.Move : StiActionUtils.PointInRect(component.Page.Unit.ConvertFromHInches(4.0), x, y, component.ClientRectangle, component.SelectRectangle, component.IsSelected, component.Locked || component.Inherited || !StiRestrictionsHelper.IsAllowChangePosition(component)));
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

	public override StiAction PointInRect(IStiDesignerBase designer, StiComponent comp, int x, int y)
	{
		StiVerticalLinePrimitive stiVerticalLinePrimitive = comp as StiVerticalLinePrimitive;
		double num = comp.Page.Unit.ConvertFromHInches(3f + stiVerticalLinePrimitive.Size);
		double num2 = designer.XToPage(x);
		double num3 = designer.YToPage(y);
		StiAction stiAction = StiAction.None;
		RectangleD rectangleD = comp.ComponentToPage(comp.ClientRectangle);
		if (rectangleD.Y + num < num3 && rectangleD.Bottom - num > num3 && rectangleD.X - num < num2 && rectangleD.X + num > num2)
		{
			stiAction = StiAction.Move;
		}
		else
		{
			stiAction = base.PointInRect(designer, comp, x, y);
			switch (stiAction)
			{
			case StiAction.SizeLeftTop:
			case StiAction.SizeRightTop:
				stiAction = StiAction.SizeTop;
				break;
			case StiAction.SizeLeftBottom:
			case StiAction.SizeRightBottom:
				stiAction = StiAction.SizeBottom;
				break;
			case StiAction.SizeLeft:
			case StiAction.SizeRight:
				stiAction = StiAction.Move;
				break;
			}
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

	public StiVerticalLinePrimitiveDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
