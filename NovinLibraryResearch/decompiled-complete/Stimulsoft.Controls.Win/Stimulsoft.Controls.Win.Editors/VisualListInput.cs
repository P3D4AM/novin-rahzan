using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualListInput : VisualInputBase
{
	private bool _AllowEmptyState = true;

	public override bool IsEmpty
	{
		get
		{
			return GetInputStringValue().Length == 0;
		}
		set
		{
			if (value != IsEmpty && value && _AllowEmptyState)
			{
				ResetValue();
			}
		}
	}

	public bool AllowEmptyState
	{
		get
		{
			return _AllowEmptyState;
		}
		set
		{
			_AllowEmptyState = value;
		}
	}

	protected override bool OnCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 38)
		{
			SelectPrevious();
			return true;
		}
		if ((int)keyData == 40)
		{
			SelectNext();
			return true;
		}
		return base.OnCmdKey(ref msg, keyData);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (!IsReadOnly)
		{
			if (e.Delta > 0)
			{
				SelectNext();
			}
			else
			{
				SelectPrevious();
			}
		}
		base.OnMouseWheel(e);
	}

	protected override bool ValidateNewInputStack(string s)
	{
		if (s.Length == 0 && !_AllowEmptyState)
		{
			return false;
		}
		return base.ValidateNewInputStack(s);
	}

	public virtual void SelectNext()
	{
		if (!IsReadOnly)
		{
			ResetInputStack();
			InputComplete(sendNotification: false);
		}
	}

	public virtual void SelectPrevious()
	{
		if (!IsReadOnly)
		{
			ResetInputStack();
			InputComplete(sendNotification: false);
		}
	}

	protected override void ResetValue()
	{
		if (SetInputStack(""))
		{
			SetInputPosition(base.InputStack.Length);
		}
		base.ResetValue();
	}

	public override void PerformLayout(PaintInfo p)
	{
		Size empty = Size.Empty;
		Graphics graphics = p.Graphics;
		Font defaultFont = p.DefaultFont;
		eTextFormat format = eTextFormat.NoPadding;
		string measureString = GetMeasureString();
		empty = TextDrawing.MeasureString(graphics, measureString, defaultFont, 0, format);
		empty.Width++;
		base.Size = empty;
		base.PerformLayout(p);
	}

	protected virtual string GetMeasureString()
	{
		string text = GetRenderString();
		if (text.Length == 0)
		{
			text = "T";
		}
		return text;
	}

	protected override void OnPaint(PaintInfo p)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Font defaultFont = p.DefaultFont;
		Color color = p.ForeColor;
		if (!GetIsEnabled(p))
		{
			color = p.DisabledForeColor;
		}
		eTextFormat format = eTextFormat.NoPadding;
		string renderString = GetRenderString();
		Rectangle renderBounds = base.RenderBounds;
		Region val = null;
		if (base.IsFocused && base.InputStack.Length > 0 && base.InputStack.Length < renderString.Length)
		{
			Size size = TextDrawing.MeasureString(graphics, renderString.Substring(0, base.InputStack.Length), defaultFont);
			val = graphics.Clip;
			Rectangle rectangle = renderBounds;
			if (base.IsRightToLeft)
			{
				rectangle.X += rectangle.Width - size.Width;
				rectangle.Width = size.Width;
				renderBounds.Width -= size.Width;
			}
			else
			{
				rectangle.Width = size.Width;
				renderBounds.X += size.Width;
				renderBounds.Width -= size.Width;
			}
			graphics.SetClip(rectangle, (CombineMode)1);
			TextDrawing.DrawString(graphics, renderString, defaultFont, color, base.RenderBounds, format);
			graphics.Clip = val;
			graphics.SetClip(renderBounds, (CombineMode)1);
		}
		if (base.IsFocused)
		{
			if (p.Colors.Highlight.IsEmpty)
			{
				graphics.FillRectangle(SystemBrushes.Highlight, renderBounds);
			}
			else
			{
				SolidBrush val2 = new SolidBrush(p.Colors.Highlight);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, renderBounds);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			color = (p.Colors.HighlightText.IsEmpty ? SystemColors.HighlightText : p.Colors.HighlightText);
		}
		if (!IsEmpty)
		{
			TextDrawing.DrawString(graphics, renderString, defaultFont, color, base.RenderBounds, format);
		}
		if (val != null)
		{
			graphics.Clip = val;
		}
		base.OnPaint(p);
	}

	protected virtual string GetRenderString()
	{
		return GetInputStringValue();
	}
}
