using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

public class VisualLabel : VisualItem
{
	private Padding _ᢾ = new Padding(0);

	private string _ኚ = "";

	public Padding TextPadding
	{
		get
		{
			return _ᢾ;
		}
		set
		{
			_ᢾ = value;
			InvalidateArrange();
		}
	}

	public string Text
	{
		get
		{
			return _ኚ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_ኚ != value)
			{
				_ኚ = value;
				InvalidateArrange();
			}
		}
	}

	public override void PerformLayout(PaintInfo p)
	{
		Size size = Size.Empty;
		Graphics graphics = p.Graphics;
		Font defaultFont = p.DefaultFont;
		eTextFormat format = eTextFormat.LeftAndRightPadding;
		if (_ኚ.Length > 0)
		{
			if (Text.Trim().Length == 0)
			{
				size = TextDrawing.MeasureString(graphics, Text.Replace(' ', '-'), defaultFont, 0, format);
				size.Width = size.Width;
			}
			else
			{
				size = TextDrawing.MeasureString(graphics, Text, defaultFont, 0, format);
			}
		}
		size.Height += _ᢾ.Vertical;
		size.Width += _ᢾ.Horizontal;
		base.Size = size;
		base.PerformLayout(p);
	}

	protected override void OnPaint(PaintInfo p)
	{
		Graphics graphics = p.Graphics;
		Font defaultFont = p.DefaultFont;
		Rectangle renderBounds = base.RenderBounds;
		eTextFormat format = eTextFormat.NoPadding;
		Color color = p.ForeColor;
		renderBounds.X += _ᢾ.Left;
		renderBounds.Y += _ᢾ.Top;
		if (!GetIsEnabled(p))
		{
			color = p.DisabledForeColor;
		}
		if (_ኚ.Length > 0)
		{
			TextDrawing.DrawString(graphics, _ኚ, defaultFont, color, renderBounds, format);
		}
		base.OnPaint(p);
	}
}
