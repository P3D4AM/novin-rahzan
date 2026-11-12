using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2003RibbonTabItemPainter : Office2003ButtonItemPainter
{
	public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
	{
		PaintButtonCheck(button, pa, image, r);
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		if (IsOnMenu(button, pa))
		{
			base.PaintButtonBackground(button, pa, image);
		}
		else
		{
			PaintButtonCheck(button, pa, image, button.DisplayRectangle);
		}
	}

	public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
	{
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		if (IsOnMenu(button, pa))
		{
			base.PaintButtonCheck(button, pa, image, r);
		}
		else
		{
			if (!button.IsMouseOver && !button.Checked)
			{
				return;
			}
			Color color = pa.Colors.BarBackground2;
			Color color2 = pa.Colors.BarBackground;
			Color barFloatingBorder = pa.Colors.BarFloatingBorder;
			int num = pa.Colors.BarBackgroundGradientAngle;
			if (button.IsMouseOver && !button.Checked)
			{
				color = pa.Colors.ItemHotBackground2;
				color2 = pa.Colors.ItemHotBackground;
				num = pa.Colors.ItemHotBackgroundGradientAngle;
			}
			else if (button.Checked)
			{
				color = Color.White;
				color2 = pa.Colors.BarBackground;
				num = pa.Colors.ItemPressedBackgroundGradientAngle;
			}
			Color color3 = ControlPaint.LightLight(color2);
			Color color4 = Color.FromArgb(100, ControlPaint.Dark(color));
			int num2 = 4;
			Graphics graphics = pa.Graphics;
			GraphicsPath tabPath = GetTabPath(r, num2);
			tabPath.CloseAllFigures();
			LinearGradientBrush val = DisplayHelp.CreateLinearGradientBrush(r, color, color2, num);
			try
			{
				val.GammaCorrection = true;
				graphics.FillPath((Brush)(object)val, tabPath);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			tabPath.Dispose();
			GraphicsPath val2 = (tabPath = GetTabPathLight(r, num2 - 1));
			try
			{
				Pen val3 = new Pen(color4, 1f);
				try
				{
					graphics.DrawPath(val3, tabPath);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			GraphicsPath val4 = (tabPath = GetTabPathDark(r, num2 - 1));
			try
			{
				Pen val5 = new Pen(color4, 1f);
				try
				{
					graphics.DrawPath(val5, tabPath);
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
			GraphicsPath val6 = (tabPath = GetTabPath(r, num2));
			try
			{
				Pen val7 = new Pen(barFloatingBorder, 1f);
				try
				{
					graphics.DrawPath(val7, tabPath);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
	}

	private GraphicsPath GetTabPathLight(Rectangle r, int cornerDiameter)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		r.X++;
		r.Y++;
		r.Width -= 2;
		r.Height--;
		GraphicsPath val = new GraphicsPath();
		val.AddLine(r.X, r.Bottom, r.X, r.Y + cornerDiameter);
		ArcData cornerArc = ElementStyleDisplay.GetCornerArc(r, cornerDiameter, eCornerArc.TopLeft);
		val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		val.AddLine(r.X + cornerDiameter, r.Y, r.Right - cornerDiameter, r.Y);
		return val;
	}

	private GraphicsPath GetTabPathDark(Rectangle r, int cornerDiameter)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		r.X++;
		r.Y++;
		r.Width -= 3;
		r.Height--;
		GraphicsPath val = new GraphicsPath();
		ArcData cornerArc = ElementStyleDisplay.GetCornerArc(r, cornerDiameter, eCornerArc.TopRight);
		val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		val.AddLine(r.Right, r.Y + cornerDiameter, r.Right, r.Bottom);
		return val;
	}

	private GraphicsPath GetTabPath(Rectangle r, int cornerDiameter)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		r.Width--;
		GraphicsPath val = new GraphicsPath();
		val.AddLine(r.X, r.Bottom, r.X, r.Y + cornerDiameter);
		ArcData cornerArc = ElementStyleDisplay.GetCornerArc(r, cornerDiameter, eCornerArc.TopLeft);
		val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		val.AddLine(r.X + cornerDiameter, r.Y, r.Right - cornerDiameter, r.Y);
		cornerArc = ElementStyleDisplay.GetCornerArc(r, cornerDiameter, eCornerArc.TopRight);
		val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		val.AddLine(r.Right, r.Y + cornerDiameter, r.Right, r.Bottom);
		return val;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, CompositeImage image)
	{
		base.PaintButtonText(button, pa, textColor, image);
	}
}
