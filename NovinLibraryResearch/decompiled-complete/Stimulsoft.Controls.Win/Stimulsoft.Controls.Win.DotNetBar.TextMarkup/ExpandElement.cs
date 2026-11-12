using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class ExpandElement : MarkupElement
{
	private enum eExpandDirection
	{
		Left,
		Right,
		Top,
		Bottom,
		Default,
		PopupDropDown
	}

	private Size m_DefaultSize = new Size((int)Math.Ceiling(5.0 * StiScale2.X), (int)Math.Ceiling(4.0 * StiScale2.X));

	private eExpandDirection m_Direction = eExpandDirection.Default;

	public override bool CanStartNewLine => false;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = new Rectangle(Point.Empty, m_DefaultSize);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}

	public override void Render(MarkupDrawContext d)
	{
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		Rectangle bounds = base.Bounds;
		bounds.Offset(d.Offset);
		if (!d.ClipRectangle.IsEmpty && !bounds.IntersectsWith(d.ClipRectangle))
		{
			return;
		}
		Graphics graphics = d.Graphics;
		Color currentForeColor = d.CurrentForeColor;
		eExpandDirection eExpandDirection = eExpandDirection.Bottom;
		if (m_Direction != eExpandDirection.Default)
		{
			eExpandDirection = m_Direction;
		}
		if (d.ContextObject is ButtonItem && m_Direction == eExpandDirection.Default)
		{
			ButtonItem buttonItem = d.ContextObject as ButtonItem;
			if (buttonItem.IsOnMenu)
			{
				eExpandDirection = eExpandDirection.Right;
				if ((buttonItem.PopupSide == ePopupSide.Default && d.RightToLeft) || buttonItem.PopupSide == ePopupSide.Left)
				{
					eExpandDirection = eExpandDirection.Left;
				}
			}
			else if (buttonItem.PopupSide == ePopupSide.Default)
			{
				eExpandDirection = eExpandDirection.Bottom;
			}
			else if (buttonItem.PopupSide == ePopupSide.Left)
			{
				eExpandDirection = eExpandDirection.Left;
			}
			else if (buttonItem.PopupSide == ePopupSide.Right)
			{
				eExpandDirection = eExpandDirection.Right;
			}
			else if (buttonItem.PopupSide == ePopupSide.Bottom)
			{
				eExpandDirection = eExpandDirection.Bottom;
			}
			else if (buttonItem.PopupSide == ePopupSide.Top)
			{
				eExpandDirection = eExpandDirection.Top;
			}
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)3;
		Rectangle r = bounds;
		switch (eExpandDirection)
		{
		case eExpandDirection.Bottom:
		case eExpandDirection.PopupDropDown:
			r.Offset(0, 1);
			break;
		case eExpandDirection.Top:
			r.Offset(0, -1);
			break;
		case eExpandDirection.Left:
			r.Offset(1, 0);
			break;
		case eExpandDirection.Right:
			r.Offset(-1, 0);
			break;
		}
		Point[] expandPolygon = GetExpandPolygon(r, eExpandDirection);
		expandPolygon = GetExpandPolygon(bounds, eExpandDirection);
		SolidBrush val = new SolidBrush(currentForeColor);
		try
		{
			graphics.FillPolygon((Brush)(object)val, expandPolygon);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (eExpandDirection == eExpandDirection.PopupDropDown)
		{
			Pen val2 = new Pen(currentForeColor, 1f);
			try
			{
				graphics.DrawLine(val2, bounds.X, bounds.Y - 2, bounds.Right - 1, bounds.Y - 2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		graphics.SmoothingMode = smoothingMode;
		base.RenderBounds = bounds;
	}

	private Point[] GetExpandPolygon(Rectangle r, eExpandDirection direction)
	{
		double factor = StiScale2.Factor;
		Point[] array = new Point[3];
		switch (direction)
		{
		case eExpandDirection.Right:
			array[0].X = r.Left + 1;
			array[0].Y = r.Top + (r.Height - m_DefaultSize.Height) / 2 - 1;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + 6;
			array[2].X = array[0].X + 3;
			array[2].Y = array[0].Y + 3;
			break;
		case eExpandDirection.Left:
			array[0].X = r.Left + 3;
			array[0].Y = r.Top + (r.Height - m_DefaultSize.Height) / 2 - 1;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + 6;
			array[2].X = array[0].X - 3;
			array[2].Y = array[0].Y + 3;
			break;
		case eExpandDirection.Top:
			array[0].X = r.Left - 1;
			array[0].Y = r.Top + (r.Height - m_DefaultSize.Height) / 2 + m_DefaultSize.Height;
			array[1].X = array[0].X + (int)Math.Ceiling(6.0 * StiScale2.X);
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + (int)Math.Ceiling(3.0 * StiScale2.X);
			array[2].Y = array[0].Y - (int)Math.Ceiling(4.0 * StiScale2.Y);
			break;
		case eExpandDirection.Bottom:
		case eExpandDirection.PopupDropDown:
			array[0].X = r.Left;
			array[0].Y = r.Top + (r.Height - m_DefaultSize.Height) / 2 + 1;
			array[1].X = array[0].X + 5;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 2;
			array[2].Y = array[0].Y + 3;
			break;
		}
		return array;
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		m_Direction = eExpandDirection.Default;
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "direction")
			{
				switch (reader.Value.ToLower())
				{
				case "left":
					m_Direction = eExpandDirection.Left;
					break;
				case "right":
					m_Direction = eExpandDirection.Right;
					break;
				case "top":
					m_Direction = eExpandDirection.Top;
					break;
				case "bottom":
					m_Direction = eExpandDirection.Bottom;
					break;
				case "popup":
					m_Direction = eExpandDirection.PopupDropDown;
					break;
				}
				break;
			}
		}
	}
}
