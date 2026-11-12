using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualLabel : VisualItem
{
	private Padding _TextPadding = new Padding(0);

	private string _Text = "";

	public Padding TextPadding
	{
		get
		{
			return _TextPadding;
		}
		set
		{
			_TextPadding = value;
			InvalidateArrange();
		}
	}

	public string Text
	{
		get
		{
			return _Text;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_Text != value)
			{
				_Text = value;
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
		if (_Text.Length > 0)
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
		size.Height += _TextPadding.Vertical;
		size.Width += _TextPadding.Horizontal;
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
		renderBounds.X += _TextPadding.Left;
		renderBounds.Y += _TextPadding.Top;
		if (!GetIsEnabled(p))
		{
			color = p.DisabledForeColor;
		}
		if (_Text.Length > 0)
		{
			TextDrawing.DrawString(graphics, _Text, defaultFont, color, renderBounds, format);
		}
		base.OnPaint(p);
	}
}
