using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class SymbolElement : MarkupElement
{
	private enum eSymbol
	{
		XInCircle
	}

	private Size _DefaultSize = new Size(16, 16);

	private eSymbol _Symbol;

	public override bool CanStartNewLine => false;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = new Rectangle(Point.Empty, _DefaultSize);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}

	public override void Render(MarkupDrawContext d)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		Rectangle bounds = base.Bounds;
		bounds.Offset(d.Offset);
		if (!d.ClipRectangle.IsEmpty && !bounds.IntersectsWith(d.ClipRectangle))
		{
			return;
		}
		Graphics graphics = d.Graphics;
		Color currentForeColor = d.CurrentForeColor;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)4;
		if (_Symbol == eSymbol.XInCircle)
		{
			Rectangle rectangle = new Rectangle(bounds.X + (bounds.Width - 14) / 2, bounds.Y + (bounds.Height - 14) / 2, 14, 14);
			Pen val = new Pen(currentForeColor, 1.51f);
			try
			{
				graphics.DrawEllipse(val, rectangle);
				graphics.DrawLine(val, rectangle.X + 4, rectangle.Y + 4, rectangle.X + 10, rectangle.Y + 10);
				graphics.DrawLine(val, rectangle.X + 10, rectangle.Y + 4, rectangle.X + 4, rectangle.Y + 10);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		graphics.SmoothingMode = smoothingMode;
		base.RenderBounds = bounds;
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		_Symbol = eSymbol.XInCircle;
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "type")
			{
				string text = reader.Value.ToLower();
				if (text == "xincircle")
				{
					_Symbol = eSymbol.XInCircle;
				}
				break;
			}
		}
	}
}
