using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Metro.ColorTables;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Metro.Rendering;

internal class ᣕ : ᯊ
{
	private static readonly int _2214 = 2;

	public override Office2007ButtonItemColorTable GetColorTable(ButtonItem button, _21F0 buttonCont)
	{
		Office2007ColorTable office2007ColorTable = base.ᘽ;
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = null;
		if (button.CustomColorName != "")
		{
			office2007ButtonItemColorTable = office2007ColorTable.ApplicationButtonColors[button.CustomColorName];
		}
		if (office2007ButtonItemColorTable == null)
		{
			office2007ButtonItemColorTable = office2007ColorTable.ApplicationButtonColors[button.ຽ()];
		}
		if (office2007ButtonItemColorTable == null && office2007ColorTable.ApplicationButtonColors.Count > 0)
		{
			office2007ButtonItemColorTable = office2007ColorTable.ApplicationButtonColors[0];
		}
		if (office2007ButtonItemColorTable == null)
		{
			office2007ButtonItemColorTable = _2215._2216();
		}
		return office2007ButtonItemColorTable;
	}

	protected override Rectangle GetDisplayRectangle(ButtonItem button)
	{
		Rectangle displayRectangle = button.DisplayRectangle;
		if (button is ApplicationButton && ((ApplicationButton)button)._1BF2)
		{
			displayRectangle.Width += 4;
			displayRectangle.Height++;
		}
		return displayRectangle;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, ᇎ image)
	{
		Rectangle displayRectangle = GetDisplayRectangle(button);
		displayRectangle.Inflate(-1, -1);
		displayRectangle.Width--;
		if (pa.ContainerControl.FindForm() is RibbonForm)
		{
			displayRectangle.Width -= _2214 + 2;
		}
		if (button is ApplicationButton && ((ApplicationButton)button)._1BF2)
		{
			displayRectangle.Height--;
			displayRectangle.Width += 3;
		}
		eTextFormat format = eTextFormat.HidePrefix | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
		TextDrawing.DrawString(pa.Graphics, button.Text, pa.Font, textColor, displayRectangle, format);
	}

	protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		return base.GetTextColor(button, pa, GetColorTable(button, _21F0._1BF3));
	}

	public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle imagebounds)
	{
		if (string.IsNullOrEmpty(button.Text))
		{
			Rectangle displayRectangle = GetDisplayRectangle(button);
			displayRectangle.X += (displayRectangle.Width - imagebounds.Width) / 2;
			displayRectangle.Y += (displayRectangle.Height - imagebounds.Height) / 2;
			displayRectangle.Width = imagebounds.Width;
			displayRectangle.Height = imagebounds.Height;
			base.PaintButtonImage(button, pa, image, displayRectangle);
		}
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		Office2007ButtonItemColorTable colorTable = GetColorTable(button, _21F0._1BF3);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = colorTable.Default;
		if (button.IsMouseDown)
		{
			office2007ButtonItemStateColorTable = colorTable.Pressed;
		}
		else if (button.IsMouseOver)
		{
			office2007ButtonItemStateColorTable = colorTable.MouseOver;
		}
		Rectangle displayRectangle = GetDisplayRectangle(button);
		displayRectangle.Width--;
		Graphics graphics = pa.Graphics;
		if (pa.ContainerControl.FindForm() is RibbonForm)
		{
			displayRectangle.Width -= _2214;
		}
		DisplayHelp.FillRectangle(graphics, displayRectangle, office2007ButtonItemStateColorTable.Background);
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
			Rectangle rectangle2 = displayRectangle;
			rectangle2.Inflate(-1, -1);
			GraphicsPath val3 = new GraphicsPath();
			try
			{
				val3.AddRectangle(rectangle2);
				DisplayHelp.DrawGradientPath(graphics, val3, rectangle2, office2007ButtonItemStateColorTable.InnerBorder, 1);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (office2007ButtonItemStateColorTable.OuterBorder != null && !office2007ButtonItemStateColorTable.OuterBorder.IsEmpty)
		{
			DisplayHelp.DrawRectangle(graphics, office2007ButtonItemStateColorTable.OuterBorder.Start, displayRectangle);
		}
	}
}
