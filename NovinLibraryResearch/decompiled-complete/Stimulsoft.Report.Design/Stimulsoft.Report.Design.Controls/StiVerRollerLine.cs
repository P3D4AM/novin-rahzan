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
internal class StiVerRollerLine : StiRollerLine
{
	public virtual void OnScroll(object sender, ScrollEventArgs se)
	{
		Draw(null, Rectangle.Empty);
	}

	private int GetVerStartPos()
	{
		Rectangle pageViewLocation = StiPageViewHelper.GetPageViewLocation(base.Page);
		pageViewLocation.Y += (int)Math.Ceiling(20.0 * StiScale.Y);
		pageViewLocation.Y += base.PageView.ScrollTop;
		pageViewLocation.Height--;
		StiMargins pageViewMargins = StiPageViewHelper.GetPageViewMargins(base.Page);
		return pageViewLocation.Y + (int)Math.Round(pageViewMargins.Top);
	}

	public override void DrawLine(Graphics g, Rectangle clipRect)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
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
			int verStartPos = GetVerStartPos();
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
			pageViewLocation.Y += (int)Math.Ceiling(20.0 * StiScale.Y);
			pageViewLocation.Y += base.PageView.ScrollTop;
			pageViewLocation.Height--;
			StiMargins pageViewMargins = StiPageViewHelper.GetPageViewMargins(base.Page);
			Rectangle rectangle = new Rectangle(3, pageViewLocation.Y, (int)Math.Ceiling(14.0 * StiScale.X), (int)Math.Round(pageViewMargins.Top));
			Rectangle rectangle2 = new Rectangle(3, pageViewLocation.Y + (int)Math.Round(pageViewMargins.Top), (int)Math.Ceiling(14.0 * StiScale.X), pageViewLocation.Height - (int)Math.Round(pageViewMargins.Top) - (int)Math.Round(pageViewMargins.Bottom));
			Rectangle rectangle3 = new Rectangle(3, pageViewLocation.Bottom - (int)Math.Round(pageViewMargins.Bottom), (int)Math.Ceiling(14.0 * StiScale.X), (int)Math.Round(pageViewMargins.Bottom));
			rectangle2.Height++;
			rectangle3.Y++;
			rectangle3.Height--;
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
				clip.Y++;
				clip.Height--;
				clip.X = rectangle2.X;
				clip.Width = rectangle2.Width;
				g.SetClip(clip);
				g.FillRectangle(val8, rollerRect);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
			g.Restore(val7);
			double num = base.Factor / 10.0;
			double num2 = verStartPos;
			int num3 = 0;
			int num4 = 0;
			while (num2 < (double)rectangle2.Bottom)
			{
				if (num3 == 10)
				{
					num3 = 0;
				}
				if (base.Page.Zoom >= 0.5 && num3 == 5)
				{
					g.DrawLine(Pens.DimGray, (float)(rectangle2.Left + 2), (float)num2, (float)(rectangle2.Right - 3), (float)num2);
				}
				if (num3 == 0 && base.Page.Zoom >= 0.5)
				{
					Rectangle rectangle7 = new Rectangle(rectangle2.X, (int)(num2 + 1.0 - 50.0), rectangle2.Width, 100);
					if (rectangle7.IntersectsWith(clipRect))
					{
						TextRenderer.DrawText((IDeviceContext)(object)g, ((double)num4 * base.RollerStep).ToString(), val, rectangle7, Color.DimGray, (TextFormatFlags)5);
					}
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
			rectangle.Y += (int)Math.Ceiling(30.0 * StiScale.X);
			return new Rectangle(((Control)this).Left + (int)Math.Ceiling(2.0 * StiScale.X), rectangle.Top - ((ScrollBar)base.PageView.VScrollBar).Value, (int)Math.Ceiling(14.0 * StiScale.X), rectangle.Height);
		}
		int y = ((Control)this).PointToClient(Cursor.Position).Y;
		return new Rectangle(((Control)this).Left + (int)Math.Ceiling(2.0 * StiScale.X), y - 1, (int)Math.Ceiling(14.0 * StiScale.X), (int)Math.Ceiling(5.0 * StiScale.Y));
	}

	public override void DisposeRes()
	{
		((Component)(object)this).Dispose();
		designer = null;
	}

	public StiVerRollerLine(StiDesignerControl designer, StiPage page, StiPageViewControl pageView)
		: base(designer, page, pageView)
	{
		((Control)this).Width = (int)Math.Ceiling(20.0 * StiScale.X);
	}
}
