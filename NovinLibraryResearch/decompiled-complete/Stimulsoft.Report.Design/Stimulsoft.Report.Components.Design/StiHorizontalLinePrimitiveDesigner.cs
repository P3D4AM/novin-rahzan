using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiHorizontalLinePrimitiveDesigner : StiComponentDesigner
{
	public override StiAction GetActionFromPoint(double x, double y, StiComponent component, bool forGetDesigner = false)
	{
		StiHorizontalLinePrimitive stiHorizontalLinePrimitive = component as StiHorizontalLinePrimitive;
		double num = component.Page.Unit.ConvertFromHInches(3f + stiHorizontalLinePrimitive.Size);
		RectangleD clientRectangle = component.ClientRectangle;
		StiAction stiAction = ((clientRectangle.Y - num < y && clientRectangle.Y + num > y && clientRectangle.Right - num > x && clientRectangle.X + num < x) ? StiAction.Move : StiActionUtils.PointInRect(component.Page.Unit.ConvertFromHInches(4.0), x, y, component.ClientRectangle, component.SelectRectangle, component.IsSelected, component.Locked || component.Inherited || !StiRestrictionsHelper.IsAllowChangePosition(component)));
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
		StiHorizontalLinePrimitive stiHorizontalLinePrimitive = comp as StiHorizontalLinePrimitive;
		double num = comp.Page.Unit.ConvertFromHInches(3f + stiHorizontalLinePrimitive.Size);
		double num2 = designer.XToPage(x);
		double num3 = designer.YToPage(y);
		RectangleD rectangleD = comp.ComponentToPage(comp.ClientRectangle);
		StiAction stiAction;
		if (rectangleD.Y - num < num3 && rectangleD.Y + num > num3 && rectangleD.Right - num > num2 && rectangleD.X + num < num2)
		{
			stiAction = StiAction.Move;
		}
		else
		{
			stiAction = base.PointInRect(designer, comp, x, y);
			switch (stiAction)
			{
			case StiAction.SizeLeftTop:
			case StiAction.SizeLeftBottom:
				stiAction = StiAction.SizeLeft;
				break;
			case StiAction.SizeRightTop:
			case StiAction.SizeRightBottom:
				stiAction = StiAction.SizeRight;
				break;
			case StiAction.SizeTop:
			case StiAction.SizeBottom:
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

	public StiHorizontalLinePrimitiveDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
