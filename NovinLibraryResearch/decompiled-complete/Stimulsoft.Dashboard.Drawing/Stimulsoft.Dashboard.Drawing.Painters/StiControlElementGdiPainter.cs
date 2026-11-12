using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public abstract class StiControlElementGdiPainter : StiElementGdiPainter
{
	public override void PaintBackground(Graphics g, RectangleD rect, IStiElement element)
	{
		if (!(element is IStiBackColor stiBackColor) || StiDashboardExpressionHelper.GetBackColor(element, stiBackColor.BackColor) == Color.Transparent)
		{
			double num = StiElementScale.Factor(element);
			StiCornerRadius cornerRadius = (element as IStiCornerRadius)?.CornerRadius;
			StiSimpleBorder border = (element as IStiSimpleBorder)?.Border;
			StiRoundedBackgroundGdiPainter.PaintBackground(g, rect.ToRectangleF(), border, cornerRadius, GetBackColor(element as IStiControlElement), StiButtonShapeType.Rectangle, element.IsEnabled, (float)num);
		}
		else
		{
			base.PaintBackground(g, rect, element);
		}
	}

	public virtual Font GetFont(IStiControlElement element)
	{
		return StiDashboardStyleHelper.GetFont(element);
	}

	public virtual Color GetForeColor(IStiControlElement element)
	{
		return StiDashboardStyleHelper.GetForeColor(element);
	}

	public virtual Color GetBackColor(IStiControlElement element)
	{
		return StiDashboardStyleHelper.GetBackColor(element);
	}

	public virtual Color GetSelectedForeColor(IStiControlElement element)
	{
		return StiDashboardStyleHelper.GetSelectedForeColor(element);
	}

	public virtual Color GetSelectedBackColor(IStiControlElement element)
	{
		return StiDashboardStyleHelper.GetSelectedBackColor(element);
	}

	public virtual Color GetGlyphColor(IStiControlElement element)
	{
		return StiDashboardStyleHelper.GetGlyphColor(element);
	}

	public virtual Color GetSeparatorColor(IStiControlElement element)
	{
		return StiDashboardStyleHelper.GetSeparatorColor(element);
	}

	public virtual string Format(IStiControlElement element, object value)
	{
		CultureInfo culture = StiCultureHelper.Set(element.Report);
		try
		{
			if (value is DateTime && !(element.TextFormat is StiDateFormatService))
			{
				return ((DateTime)value).ToShortDateString();
			}
			if (StiExtValueConverter.IsEligable(value, element?.Report?.Culture))
			{
				return StiExtValueConverter.Convert(value, element?.Report?.Culture);
			}
			return element.TextFormat.Format(value);
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	public virtual void PaintButton(Graphics g, RectangleF rect, IStiControlElement element, float scale)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		RectangleF rectangleF = new RectangleF(rect.Right - 20f * scale, rect.Y, 20f * scale, rect.Height);
		float num = rectangleF.X + rectangleF.Width / 2f;
		float num2 = rectangleF.Y + rectangleF.Height / 2f;
		GraphicsPath val = new GraphicsPath();
		try
		{
			float num3 = 2f * scale;
			float num4 = 3f * scale;
			val.AddLine(num - num4, num2 - num3, num - num4, num2 - scale);
			val.AddLine(num - num4, num2 - scale, num, num2 + num3);
			val.AddLine(num, num2 + num3, num + num4, num2 - scale);
			val.AddLine(num + num4, num2 - scale, num + num4, num2 - num3);
			Pen val2 = new Pen(GetGlyphColor(element), 2f);
			try
			{
				val2.StartCap = (LineCap)0;
				val2.EndCap = (LineCap)0;
				g.SmoothingMode = (SmoothingMode)4;
				g.SetClip(new RectangleF(num - num4, num2 - num3, 7f * scale, 6f * scale));
				g.DrawPath(val2, val);
				g.ResetClip();
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override void PaintCheckBoxTitle(Graphics g, RectangleD rect, IStiElement element)
	{
		if (element is StiListBoxElement || element is StiTreeViewElement)
		{
			base.PaintCheckBoxTitle(g, rect, element);
		}
	}
}
