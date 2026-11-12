using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2010AppButtonPainter : Office2007ButtonItemPainter
{
	public override Office2007ButtonItemColorTable GetColorTable(ButtonItem button, eButtonContainer buttonCont)
	{
		Office2007ColorTable colorTable = base.ColorTable;
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = null;
		if (button.CustomColorName != "")
		{
			office2007ButtonItemColorTable = colorTable.ApplicationButtonColors[button.CustomColorName];
		}
		if (office2007ButtonItemColorTable == null)
		{
			office2007ButtonItemColorTable = colorTable.ApplicationButtonColors[button.GetColorTableName()];
		}
		if (office2007ButtonItemColorTable == null && colorTable.ApplicationButtonColors.Count > 0)
		{
			office2007ButtonItemColorTable = colorTable.ApplicationButtonColors[0];
		}
		if (office2007ButtonItemColorTable == null)
		{
			office2007ButtonItemColorTable = Office2010ColorTable.GetFallBackAppButtonColorTable();
		}
		return office2007ButtonItemColorTable;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, CompositeImage image)
	{
		Rectangle displayRectangle = button.DisplayRectangle;
		displayRectangle.Inflate(-1, -1);
		displayRectangle.Width--;
		Office2007ButtonItemColorTable colorTable = GetColorTable(button, eButtonContainer.RibbonStrip);
		Color color = Color.FromArgb(32, Color.Black);
		if (colorTable.Default != null && colorTable.Default.OuterBorder != null && !colorTable.Default.OuterBorder.IsEmpty)
		{
			color = colorTable.Default.OuterBorder.Start;
		}
		if (pa.GlassEnabled)
		{
			ThemeTextFormat tf = (ThemeTextFormat)1048613;
			Office2007RibbonControlPainter.PaintShadowTextOnGlass(pa.Graphics, button.Text, pa.Font, displayRectangle, tf, textColor, color, copySourceBackground: true);
			return;
		}
		eTextFormat format = eTextFormat.HidePrefix | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
		displayRectangle.Offset(1, 1);
		TextDrawing.DrawString(pa.Graphics, button.Text, pa.Font, color, displayRectangle, format);
		displayRectangle.Offset(-1, -1);
		TextDrawing.DrawString(pa.Graphics, button.Text, pa.Font, textColor, displayRectangle, format);
	}

	protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		return base.GetTextColor(button, pa, GetColorTable(button, eButtonContainer.RibbonStrip));
	}

	public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle imagebounds)
	{
		if (string.IsNullOrEmpty(button.Text))
		{
			Rectangle displayRectangle = button.DisplayRectangle;
			displayRectangle.X += (displayRectangle.Width - imagebounds.Width) / 2;
			displayRectangle.Y += (displayRectangle.Height - imagebounds.Height) / 2;
			displayRectangle.Width = imagebounds.Width;
			displayRectangle.Height = imagebounds.Height;
			base.PaintButtonImage(button, pa, image, displayRectangle);
		}
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		Office2007ButtonItemColorTable colorTable = GetColorTable(button, eButtonContainer.RibbonStrip);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = colorTable.Default;
		if (button.IsMouseDown)
		{
			office2007ButtonItemStateColorTable = colorTable.Pressed;
		}
		else if (button.IsMouseOver)
		{
			office2007ButtonItemStateColorTable = colorTable.MouseOver;
		}
		Rectangle displayRectangle = button.DisplayRectangle;
		displayRectangle.Width--;
		Graphics graphics = pa.Graphics;
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(displayRectangle, 2, 2, 0, 0);
		try
		{
			DisplayHelp.FillPath(graphics, roundedRectanglePath, office2007ButtonItemStateColorTable.Background);
			if (office2007ButtonItemStateColorTable.BottomBackgroundHighlight != null && !office2007ButtonItemStateColorTable.BottomBackgroundHighlight.IsEmpty)
			{
				Rectangle rectangle = new Rectangle(displayRectangle.X - 12, displayRectangle.Y + displayRectangle.Height / 2 - 4, displayRectangle.Width + 24, displayRectangle.Height + 4);
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.AddEllipse(rectangle);
					PathGradientBrush val2 = new PathGradientBrush(val);
					try
					{
						val2.CenterColor = office2007ButtonItemStateColorTable.BottomBackgroundHighlight.Start;
						val2.SurroundColors = new Color[1] { office2007ButtonItemStateColorTable.BottomBackgroundHighlight.End };
						val2.CenterPoint = new PointF(rectangle.X + rectangle.Width / 2, displayRectangle.Bottom - 1);
						graphics.FillRectangle((Brush)(object)val2, displayRectangle);
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
			if (office2007ButtonItemStateColorTable.InnerBorder != null && !office2007ButtonItemStateColorTable.InnerBorder.IsEmpty)
			{
				Rectangle bounds = displayRectangle;
				bounds.Y++;
				bounds.Width--;
				bounds.X++;
				DisplayHelp.DrawGradientRectangle(graphics, bounds, office2007ButtonItemStateColorTable.InnerBorder, 1);
				Pen val3 = new Pen(ColorScheme.GetColor(10, 16777215));
				try
				{
					graphics.DrawLine(val3, bounds.X + 1, bounds.Y + 1, bounds.X + 1, bounds.Bottom - 1);
					graphics.DrawLine(val3, bounds.Right - 2, bounds.Y + 1, bounds.Right - 2, bounds.Bottom - 1);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			DisplayHelp.DrawGradientPathBorder(graphics, roundedRectanglePath, office2007ButtonItemStateColorTable.OuterBorder, 1);
		}
		finally
		{
			((IDisposable)roundedRectanglePath)?.Dispose();
		}
	}
}
