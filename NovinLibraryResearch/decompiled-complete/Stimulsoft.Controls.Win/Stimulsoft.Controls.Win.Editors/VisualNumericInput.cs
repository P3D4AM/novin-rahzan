using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualNumericInput : VisualInputBase
{
	private bool _MouseWheelValueChangeEnabled = true;

	private bool _AllowEmptyState = true;

	private bool _AllowsNegativeValue = true;

	private bool _LeadingZero;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse wheel increases or decreases the input value when used.")]
	public bool MouseWheelValueChangeEnabled
	{
		get
		{
			return _MouseWheelValueChangeEnabled;
		}
		set
		{
			_MouseWheelValueChangeEnabled = value;
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

	public bool AllowsNegativeValue
	{
		get
		{
			return _AllowsNegativeValue;
		}
		set
		{
			_AllowsNegativeValue = value;
		}
	}

	protected virtual bool IsLeadingZero => _LeadingZero;

	public event EventHandler ValueChanged;

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if (((int)e.KeyCode == 109 || (int)e.KeyCode == 189) && AllowsNegativeValue && base.InputStack.Length > 0)
		{
			if (!IsEmpty)
			{
				NegateValue();
			}
			e.Handled = true;
		}
		base.OnKeyDown(e);
	}

	protected override bool OnCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		if (!IsReadOnly)
		{
			if ((int)keyData == 38)
			{
				IncreaseValue();
				return true;
			}
			if ((int)keyData == 40)
			{
				DecreaseValue();
				return true;
			}
		}
		return base.OnCmdKey(ref msg, keyData);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (_MouseWheelValueChangeEnabled && !IsReadOnly)
		{
			if (e.Delta > 0)
			{
				IncreaseValue();
			}
			else
			{
				DecreaseValue();
			}
		}
		base.OnMouseWheel(e);
	}

	public virtual void DecreaseValue()
	{
		InputComplete(sendNotification: false);
		ResetInputPosition();
	}

	public virtual void IncreaseValue()
	{
		InputComplete(sendNotification: false);
		ResetInputPosition();
	}

	protected virtual void NegateValue()
	{
	}

	protected override bool AcceptKeyPress(KeyPressEventArgs e)
	{
		if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (AllowsNegativeValue && e.KeyChar == '-' && (IsEmpty || (!IsEmpty && (base.InputStack == "" || base.InputStack == "0")))))
		{
			e.Handled = true;
			return true;
		}
		return false;
	}

	public override void PerformLayout(PaintInfo p)
	{
		Size empty = Size.Empty;
		Graphics graphics = p.Graphics;
		Font defaultFont = p.DefaultFont;
		eTextFormat format = eTextFormat.NoPadding;
		string measureString = GetMeasureString();
		empty = TextDrawing.MeasureString(graphics, measureString, defaultFont, 0, format);
		base.Size = empty;
		base.PerformLayout(p);
	}

	protected virtual string GetMeasureString()
	{
		return "";
	}

	protected override void OnPaint(PaintInfo p)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Font defaultFont = p.DefaultFont;
		Color color = p.ForeColor;
		if (!GetIsEnabled(p))
		{
			color = p.DisabledForeColor;
		}
		eTextFormat format = eTextFormat.NoPadding;
		if (base.IsFocused)
		{
			if (p.Colors.Highlight.IsEmpty)
			{
				graphics.FillRectangle(SystemBrushes.Highlight, base.RenderBounds);
			}
			else
			{
				SolidBrush val = new SolidBrush(p.Colors.Highlight);
				try
				{
					graphics.FillRectangle((Brush)(object)val, base.RenderBounds);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			color = (p.Colors.HighlightText.IsEmpty ? SystemColors.HighlightText : p.Colors.HighlightText);
		}
		if (!IsEmpty || !AllowEmptyState)
		{
			string renderString = GetRenderString();
			TextDrawing.DrawString(graphics, renderString, defaultFont, color, base.RenderBounds, format);
		}
		base.OnPaint(p);
	}

	protected virtual string GetRenderString()
	{
		return "";
	}

	protected override bool SetInputStack(string s)
	{
		s = ProcessNewInputStack(s);
		return base.SetInputStack(s);
	}

	protected virtual string ProcessNewInputStack(string s)
	{
		if (base.InputStack == "0" && s != "0" && s.StartsWith("0"))
		{
			s = s.Substring(1);
			SetInputPosition(base.InputPosition - 1);
			_LeadingZero = true;
		}
		else
		{
			_LeadingZero = false;
		}
		return s;
	}

	protected override void ResetInputStack()
	{
		_LeadingZero = false;
		base.ResetInputStack();
	}

	protected virtual void OnValueChanged()
	{
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, new EventArgs());
		}
	}

	protected override void OnInputLostFocus()
	{
		if (base.InputStack == "-")
		{
			OnClear();
		}
		else
		{
			ResetInputStack();
		}
		base.OnInputLostFocus();
	}
}
