using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar;

public class PropertySliderEditor : PropertyValueEditor
{
	private class _2EC8 : Slider, IPropertyValueEditor
	{
		private EventHandler _2EBE;

		public Font EditorFont
		{
			get
			{
				return ((Control)this).Font;
			}
			set
			{
				((Control)this).Font = value;
			}
		}

		public bool IsEditorFocused => ((Control)this).Focused;

		public object EditValue
		{
			get
			{
				return base.Value;
			}
			set
			{
				if (value is int)
				{
					base.Value = (int)value;
				}
				else if (value is double)
				{
					base.Value = (int)(double)value;
				}
				else if (value is float)
				{
					base.Value = (int)(float)value;
				}
				else if (value is decimal)
				{
					base.Value = (int)(decimal)value;
				}
				else if (value is byte)
				{
					base.Value = (byte)value;
				}
				else if (value is long)
				{
					base.Value = (int)(long)value;
				}
				else if (value is float)
				{
					base.Value = (int)(float)value;
				}
				((Control)this).Text = base.Value.ToString();
			}
		}

		public event EventHandler EditValueChanged
		{
			add
			{
				EventHandler eventHandler = _2EBE;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref _2EBE, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = _2EBE;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref _2EBE, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public void FocusEditor()
		{
			((Control)this).Focus();
		}

		protected override void OnValueChanged(EventArgs e)
		{
			((Control)this).Text = base.Value.ToString();
			_2EBF(e);
			base.OnValueChanged(e);
		}

		private void _2EBF(EventArgs ؿ)
		{
			_2EBE?.Invoke(this, ؿ);
		}
	}

	public int MinValue;

	public int MaxValue = 100;

	public bool LabelVisible = true;

	public int LabelWidth = 22;

	public Color TextColor = Color.Black;

	public PropertySliderEditor(int minValue, int maxValue)
	{
		MinValue = minValue;
		MaxValue = maxValue;
	}

	public PropertySliderEditor(int minValue, int maxValue, Color textColor)
	{
		MinValue = minValue;
		MaxValue = maxValue;
		TextColor = textColor;
	}

	public PropertySliderEditor(int minValue, int maxValue, bool labelVisible, int labelWidth, Color textColor)
	{
		MinValue = minValue;
		MaxValue = maxValue;
		LabelVisible = labelVisible;
		LabelWidth = labelWidth;
		TextColor = textColor;
	}

	public PropertySliderEditor(int minValue, int maxValue, bool labelVisible)
	{
		MinValue = minValue;
		MaxValue = maxValue;
		LabelVisible = labelVisible;
	}

	public PropertySliderEditor(int minValue, int maxValue, bool labelVisible, int labelWidth)
	{
		MinValue = minValue;
		MaxValue = maxValue;
		LabelVisible = labelVisible;
		LabelWidth = labelWidth;
	}

	public override IPropertyValueEditor CreateEditor(PropertyDescriptor propertyDescriptor, object targetObject)
	{
		_2EC8 _2EC8 = new _2EC8();
		_2EC8.Style = eDotNetBarStyle.StyleManagerControlled;
		_2EC8.LabelVisible = LabelVisible;
		_2EC8.LabelWidth = LabelWidth;
		_2EC8.Minimum = MinValue;
		_2EC8.Maximum = MaxValue;
		((Control)_2EC8).Height = Dpi.Height16;
		_2EC8.FocusCuesEnabled = false;
		if (!TextColor.IsEmpty)
		{
			_2EC8.TextColor = TextColor;
		}
		return _2EC8;
	}
}
