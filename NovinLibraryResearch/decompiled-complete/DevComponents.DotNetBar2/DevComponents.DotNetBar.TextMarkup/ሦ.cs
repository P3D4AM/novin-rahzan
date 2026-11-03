using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሦ : _0CBF
{
	private enum ሩ
	{
		_0749,
		ሪ,
		_074A,
		ራ,
		ሬ,
		ር
	}

	private Size ሧ = new Size(5, 4);

	private ሩ ຯ = ሩ.ሬ;

	public override bool CanStartNewLine => false;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = new Rectangle(Point.Empty, Dpi.Size(ሧ));
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}

	public override void Render(MarkupDrawContext d)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		Rectangle bounds = base.Bounds;
		bounds.Offset(d.Offset);
		if (!d.ClipRectangle.IsEmpty && !bounds.IntersectsWith(d.ClipRectangle))
		{
			return;
		}
		Graphics graphics = d.Graphics;
		Color currentForeColor = d.CurrentForeColor;
		ሩ ሩ = ሩ.ራ;
		if (ຯ != ሩ.ሬ)
		{
			ሩ = ຯ;
		}
		if (d.ContextObject is ButtonItem && ຯ == ሩ.ሬ)
		{
			ButtonItem buttonItem = d.ContextObject as ButtonItem;
			if (buttonItem.IsOnMenu)
			{
				ሩ = ሩ.ሪ;
				if ((buttonItem.PopupSide == ePopupSide.Default && d.RightToLeft) || buttonItem.PopupSide == ePopupSide.Left)
				{
					ሩ = ሩ._0749;
				}
			}
			else if (buttonItem.PopupSide == ePopupSide.Default)
			{
				ሩ = ሩ.ራ;
			}
			else if (buttonItem.PopupSide == ePopupSide.Left)
			{
				ሩ = ሩ._0749;
			}
			else if (buttonItem.PopupSide == ePopupSide.Right)
			{
				ሩ = ሩ.ሪ;
			}
			else if (buttonItem.PopupSide == ePopupSide.Bottom)
			{
				ሩ = ሩ.ራ;
			}
			else if (buttonItem.PopupSide == ePopupSide.Top)
			{
				ሩ = ሩ._074A;
			}
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)3;
		Rectangle ल = bounds;
		switch (ሩ)
		{
		case ሩ.ራ:
		case ሩ.ር:
			ल.Offset(0, 1);
			break;
		case ሩ._074A:
			ल.Offset(0, -1);
			break;
		case ሩ._0749:
			ल.Offset(1, 0);
			break;
		case ሩ.ሪ:
			ल.Offset(-1, 0);
			break;
		}
		Point[] array = ረ(ल, ሩ);
		array = ረ(bounds, ሩ);
		SolidBrush val = new SolidBrush(currentForeColor);
		try
		{
			graphics.FillPolygon((Brush)(object)val, array);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (ሩ == ሩ.ር)
		{
			Pen val2 = new Pen(currentForeColor, (float)Dpi.Height1);
			try
			{
				graphics.DrawLine(val2, bounds.X, bounds.Y - Dpi.Width2, bounds.Right - 1, bounds.Y - Dpi.Width2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		graphics.SmoothingMode = smoothingMode;
		base._0CC0 = bounds;
	}

	private Point[] ረ(Rectangle ल, ሩ _0C74)
	{
		Point[] array = new Point[3];
		int num = Dpi.Height(ሧ.Height);
		switch (_0C74)
		{
		case ሩ.ሪ:
			array[0].X = ल.Left + 1;
			array[0].Y = ल.Top + (ल.Height - num) / 2 - 1;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + Dpi.Height6;
			array[2].X = array[0].X + Dpi.Width3;
			array[2].Y = array[0].Y + Dpi.Height3;
			break;
		case ሩ._0749:
			array[0].X = ल.Left + Dpi.Width3;
			array[0].Y = ल.Top + (ल.Height - num) / 2 - 1;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + Dpi.Height6;
			array[2].X = array[0].X - Dpi.Width3;
			array[2].Y = array[0].Y + Dpi.Height3;
			break;
		case ሩ._074A:
			array[0].X = ल.Left - 1;
			array[0].Y = ल.Top + (ल.Height - num) / 2 + num;
			array[1].X = array[0].X + Dpi.Width6;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + Dpi.Width3;
			array[2].Y = array[0].Y - Dpi.Height4;
			break;
		case ሩ.ራ:
		case ሩ.ር:
			array[0].X = ल.Left;
			array[0].Y = ल.Top + (ल.Height - num) / 2 + 1;
			array[1].X = array[0].X + Dpi.Width5;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + Dpi.Width2;
			array[2].Y = array[0].Y + Dpi.Height3;
			break;
		}
		return array;
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		ຯ = ሩ.ሬ;
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "direction")
			{
				switch (reader.Value.ToLower())
				{
				case "left":
					ຯ = ሩ._0749;
					break;
				case "right":
					ຯ = ሩ.ሪ;
					break;
				case "top":
					ຯ = ሩ._074A;
					break;
				case "bottom":
					ຯ = ሩ.ራ;
					break;
				case "popup":
					ຯ = ሩ.ር;
					break;
				}
				break;
			}
		}
	}
}
