using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiBandDesigner : StiComponentDesigner
{
	public override StiAction GetActionFromPoint(double x, double y, StiComponent component)
	{
		double factor = StiScale.Factor;
		StiBand stiBand = component as StiBand;
		double num = component.Page.Unit.ConvertFromHInches(10.0 * factor) / (component.Page.Zoom * factor) * 1.2;
		RectangleD rectangleD = component.SelectRectangle;
		RectangleD rectangleD2 = new RectangleD(rectangleD.Left + rectangleD.Width / 2.0 - num / 2.0, rectangleD.Bottom - num, num, num * 2.0);
		if (stiBand is StiPageFooterBand && StiOptions.Engine.DockPageFooterToBottom)
		{
			rectangleD = component.ClientRectangle;
			rectangleD2 = new RectangleD(rectangleD.Left + rectangleD.Width / 2.0 - num / 2.0, rectangleD.Top - num / 2.0, num, num);
		}
		if (stiBand.IsCross)
		{
			rectangleD2 = new RectangleD(component.ClientRectangle.Right - num, component.ClientRectangle.Top + component.ClientRectangle.Height / 2.0 - num / 2.0, num * 2.0, num);
		}
		if (rectangleD2.Contains(x, y))
		{
			if (component.IsCross)
			{
				return StiAction.SizeRight;
			}
			if (stiBand is StiPageFooterBand && StiOptions.Engine.DockPageFooterToBottom)
			{
				return StiAction.SizeTop;
			}
			return StiAction.SizeBottom;
		}
		if (rectangleD.Contains(x, y))
		{
			return StiAction.Move;
		}
		return StiAction.None;
	}

	public override StiAction PointInRect(IStiDesignerBase designer, StiComponent comp, int x, int y)
	{
		double factor = StiScale.Factor;
		StiBand stiBand = comp as StiBand;
		double num = comp.Page.Unit.ConvertFromHInches(4.0 * factor) / (comp.Page.Zoom * factor) * 1.2;
		RectangleD rect = comp.SelectRectangle;
		double num2 = designer.XToPage(x);
		double num3 = designer.YToPage(y);
		if (comp is StiTable)
		{
			double num4 = 0.0;
			if (comp.Report.Info.ShowHeaders)
			{
				num4 = comp.Page.Unit.ConvertFromHInches(stiBand.HeaderSize);
			}
			RectangleD displayRectangle = comp.GetDisplayRectangle();
			if (((num2 > displayRectangle.Left) & (num2 < displayRectangle.Right)) && ((num3 <= displayRectangle.Top + num4) & (num3 > displayRectangle.Top + num4 - num)))
			{
				return StiAction.SelectColumn;
			}
			if (((num2 > displayRectangle.Left - num) & (num2 < displayRectangle.Left)) && ((num3 > displayRectangle.Top) & (num3 < displayRectangle.Bottom)))
			{
				return StiAction.SelectRow;
			}
		}
		RectangleD rect2 = new RectangleD(rect.Left + rect.Width / 2.0 - num / 2.0, rect.Bottom - num, num, num * 2.0);
		if (stiBand is StiPageFooterBand && StiOptions.Engine.DockPageFooterToBottom)
		{
			rect = comp.ClientRectangle;
			rect2 = new RectangleD(rect.Left + rect.Width / 2.0 - num / 2.0, rect.Top - num / 2.0, num, num);
		}
		if (stiBand.IsCross)
		{
			rect2 = new RectangleD(comp.ClientRectangle.Right - num, comp.ClientRectangle.Top + comp.ClientRectangle.Height / 2.0 - num / 2.0, num * 2.0, num);
		}
		rect2 = comp.ComponentToPage(rect2);
		rect = comp.ComponentToPage(rect);
		if (rect2.Contains(num2, num3))
		{
			if (comp.IsCross)
			{
				return StiAction.SizeRight;
			}
			if (stiBand is StiPageFooterBand && StiOptions.Engine.DockPageFooterToBottom)
			{
				return StiAction.SizeTop;
			}
			return StiAction.SizeBottom;
		}
		if (rect.Contains(num2, num3))
		{
			return StiAction.Move;
		}
		return StiAction.None;
	}

	public StiBandDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
