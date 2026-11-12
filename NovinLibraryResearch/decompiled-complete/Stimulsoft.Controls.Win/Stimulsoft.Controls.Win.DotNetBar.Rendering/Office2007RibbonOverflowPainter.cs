using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007RibbonOverflowPainter : Office2007ButtonItemPainter
{
	public override Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		Rectangle empty = Rectangle.Empty;
		bool flag = IsOnMenu(button, pa);
		if (image != null)
		{
			empty.Width = image.Width + 16;
			empty.Height = image.Height + 16;
			empty.X = button.DisplayRectangle.X + (button.DisplayRectangle.Width - empty.Width) / 2;
			empty.Y = button.DisplayRectangle.Y + 3;
		}
		return empty;
	}

	public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle imagebounds)
	{
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		if (imagebounds.Width <= 0 || imagebounds.Height <= 0)
		{
			return;
		}
		if (button is RibbonOverflowButtonItem { RibbonBar: not null } ribbonOverflowButtonItem)
		{
			ElementStyle paintBackgroundStyle = ribbonOverflowButtonItem.RibbonBar.GetPaintBackgroundStyle();
			ElementStyle titleStyle = ribbonOverflowButtonItem.RibbonBar.TitleStyle;
			int num = 3;
			if (paintBackgroundStyle.BackColorBlend.Count > 0)
			{
				DisplayHelp.FillRoundedRectangle(pa.Graphics, imagebounds, num, paintBackgroundStyle.BackColorBlend[0].Color, paintBackgroundStyle.BackColorBlend[paintBackgroundStyle.BackColorBlend.Count - 1].Color, ribbonOverflowButtonItem.RibbonBar.BackgroundStyle.BackColorGradientAngle);
			}
			else
			{
				DisplayHelp.FillRoundedRectangle(pa.Graphics, imagebounds, num, paintBackgroundStyle.BackColor, paintBackgroundStyle.BackColor2, paintBackgroundStyle.BackColorGradientAngle);
			}
			if (!button.Expanded)
			{
				DisplayHelp.FillRectangle(pa.Graphics, new Rectangle(imagebounds.X + 1, imagebounds.Bottom - 8, imagebounds.Width - 2, 7), titleStyle.BackColor, titleStyle.BackColor2, titleStyle.BackColorGradientAngle);
			}
			if (!paintBackgroundStyle.BorderColor.IsEmpty)
			{
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.AddLine(imagebounds.X, imagebounds.Bottom - 8, imagebounds.Right, imagebounds.Bottom - 8);
					ElementStyleDisplay.AddCornerArc(val, imagebounds, num, eCornerArc.BottomRight);
					ElementStyleDisplay.AddCornerArc(val, imagebounds, num, eCornerArc.BottomLeft);
					val.CloseAllFigures();
					SolidBrush val2 = new SolidBrush(Color.FromArgb(192, paintBackgroundStyle.BorderColor));
					try
					{
						pa.Graphics.FillPath((Brush)(object)val2, val);
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
			DisplayHelp.DrawRoundGradientRectangle(pa.Graphics, imagebounds, paintBackgroundStyle.BorderColor, paintBackgroundStyle.BorderColor2, paintBackgroundStyle.BorderGradientAngle, 1, num);
			imagebounds.X += (imagebounds.Width - image.Width) / 2;
			imagebounds.Y += 4;
			imagebounds.Width = image.Width;
			imagebounds.Height = image.Height;
			image.DrawImage(pa.Graphics, imagebounds);
		}
		else
		{
			base.PaintButtonImage(button, pa, image, imagebounds);
		}
	}

	protected override Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, CompositeImage image)
	{
		Rectangle textRectangle = base.GetTextRectangle(button, pa, stringFormat, image);
		textRectangle.Offset(0, 12);
		return textRectangle;
	}

	protected override void PaintState(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r, bool isMouseDown)
	{
	}
}
