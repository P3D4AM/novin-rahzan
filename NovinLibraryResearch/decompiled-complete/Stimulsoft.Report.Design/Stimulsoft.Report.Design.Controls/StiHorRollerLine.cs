using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
internal class StiHorRollerLine : StiRollerLine
{
	public virtual void OnScroll(object sender, ScrollEventArgs se)
	{
		Draw(null, Rectangle.Empty);
	}

	internal int GetHorStartPos()
	{
		return (int)Math.Ceiling(10.0 * StiScale.X) + (int)(base.Page.Unit.ConvertToHInches(base.Page.Margins.Left) * base.Page.Zoom * StiScale.Factor) + base.PageView.ScrollLeft;
	}

	public override void DrawLine(Graphics g, Rectangle clipRect)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		if (base.Page == null)
		{
			return;
		}
		if (clipRect.IsEmpty)
		{
			clipRect = ((Control)this).ClientRectangle;
		}
		else if (!clipRect.IntersectsWith(((Control)this).ClientRectangle))
		{
			return;
		}
		g.SetClip(clipRect);
		Font val = new Font("Arial", 6f);
		try
		{
			int horStartPos = GetHorStartPos();
			Brush val2 = (Brush)new SolidBrush(StiGuiHelper.GetColorRulerBackground());
			try
			{
				g.FillRectangle(val2, clipRect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Rectangle pageViewLocation = StiPageViewHelper.GetPageViewLocation(base.Page);
			pageViewLocation.X += base.PageView.ScrollLeft;
			pageViewLocation.Width--;
			StiMargins pageViewMargins = StiPageViewHelper.GetPageViewMargins(base.Page);
			Rectangle rectangle = new Rectangle(pageViewLocation.X, 3, (int)Math.Round(pageViewMargins.Left), (int)Math.Ceiling(14.0 * StiScale.Y));
			Rectangle rectangle2 = new Rectangle(pageViewLocation.X + (int)Math.Round(pageViewMargins.Left), 3, pageViewLocation.Width - (int)Math.Round(pageViewMargins.Left) - (int)Math.Round(pageViewMargins.Right), (int)Math.Ceiling(14.0 * StiScale.Y));
			Rectangle rectangle3 = new Rectangle(pageViewLocation.Right - (int)Math.Round(pageViewMargins.Right), 3, (int)Math.Round(pageViewMargins.Right), (int)Math.Ceiling(14.0 * StiScale.Y));
			rectangle2.Width++;
			rectangle3.X++;
			rectangle3.Width--;
			Rectangle rectangle4 = rectangle;
			Rectangle rectangle5 = rectangle2;
			Rectangle rectangle6 = rectangle3;
			rectangle.Inflate(-1, -1);
			rectangle2.Inflate(-1, -1);
			rectangle3.Inflate(-1, -1);
			rectangle.Width++;
			rectangle.Height++;
			rectangle2.Width++;
			rectangle2.Height++;
			rectangle3.Width++;
			rectangle3.Height++;
			Brush val3 = (Brush)new SolidBrush(StiGuiHelper.GetColorRulerLightBackground());
			try
			{
				Brush val4 = (Brush)new SolidBrush(StiGuiHelper.GetColorRulerDarkBackground());
				try
				{
					Pen val5 = new Pen(StiGuiHelper.GetColorRulerLightBorder(), 1f);
					try
					{
						Pen val6 = new Pen(StiGuiHelper.GetColorRulerDarkBorder(), 1f);
						try
						{
							g.FillRectangle(val4, rectangle);
							g.FillRectangle(val4, rectangle3);
							g.FillRectangle(val3, rectangle2);
							g.DrawRectangle(val6, rectangle4);
							g.DrawRectangle(val6, rectangle6);
							g.DrawRectangle(val5, rectangle5);
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Rectangle rollerRect = GetRollerRect();
			GraphicsState val7 = g.Save();
			Brush val8 = (Brush)new SolidBrush(StiGuiHelper.GetColorRulerSelectionBackground());
			try
			{
				Rectangle clip = pageViewLocation;
				clip.X++;
				clip.Width--;
				clip.Y = rectangle2.Top;
				clip.Height = rectangle2.Height;
				g.SetClip(clip);
				g.FillRectangle(val8, rollerRect);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
			g.Restore(val7);
			double num = base.Factor / 10.0;
			double num2 = horStartPos;
			int num3 = 0;
			int num4 = 0;
			while (num2 < (double)rectangle2.Right)
			{
				if (num3 == 10)
				{
					num3 = 0;
				}
				if (base.Page.Zoom >= 0.5 && num3 == 5)
				{
					g.DrawLine(Pens.DimGray, (float)num2, (float)(rectangle2.Top + 2), (float)num2, (float)(rectangle2.Bottom - 3));
				}
				if (num3 == 0 && base.Page.Zoom >= 0.5)
				{
					Rectangle rectangle7 = new Rectangle((int)(num2 + 1.0 - 50.0), rectangle2.Top, 100, rectangle2.Height);
					TextRenderer.DrawText((IDeviceContext)(object)g, ((double)num4 * base.RollerStep).ToString(), val, rectangle7, Color.DimGray, (TextFormatFlags)5);
					num4++;
				}
				num2 += num;
				num3++;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.ResetClip();
	}

	public override Rectangle GetRollerRect()
	{
		if (designer?.SelectTool?.Designer == null)
		{
			return Rectangle.Empty;
		}
		RectangleD actionRectangle = designer.SelectTool.GetActionRectangle();
		if (!actionRectangle.IsEmpty)
		{
			Rectangle rectangle = StiConvert.RectangleToScreen(designer, actionRectangle);
			rectangle.X += (int)Math.Ceiling(10.0 * StiScale.X);
			return new Rectangle(rectangle.Left - ((ScrollBar)base.PageView.HScrollBar).Value, ((Control)this).Top + (int)Math.Ceiling(2.0 * StiScale.Y), rectangle.Width, (int)Math.Ceiling(14.0 * StiScale.Y));
		}
		int x = ((Control)this).PointToClient(Cursor.Position).X;
		return new Rectangle(x - 1, ((Control)this).Top + (int)Math.Ceiling(2.0 * StiScale.Y), (int)Math.Ceiling(5.0 * StiScale.X), (int)Math.Ceiling(14.0 * StiScale.Y));
	}

	public override void DisposeRes()
	{
		((Component)(object)this).Dispose();
		designer = null;
	}

	public StiHorRollerLine(StiDesignerControl designer, StiPage page, StiPageViewControl pageView)
		: base(designer, page, pageView)
	{
		((Control)this).Height = (int)Math.Ceiling(20.0 * StiScale.Y);
	}
}
