using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar;

public class PropertyDoubleEditor : PropertyValueEditor
{
	private class _2EC0 : DoubleInput, IPropertyValueEditor
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
				return base.ValueObject;
			}
			set
			{
				if (value == null)
				{
					base.ValueObject = null;
				}
				else
				{
					base.Value = (double)value;
				}
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
			_2EBF(e);
			base.OnValueChanged(e);
		}

		private void _2EBF(EventArgs ؿ)
		{
			_2EBE?.Invoke(this, ؿ);
		}
	}

	public bool ShowUpDownButton = true;

	public string DisplayFormat = "";

	public double MinValue = double.MinValue;

	public double MaxValue = double.MaxValue;

	public bool AllowEmptyState;

	public PropertyDoubleEditor()
	{
	}

	public PropertyDoubleEditor(bool showUpDownButton)
	{
		ShowUpDownButton = showUpDownButton;
	}

	public PropertyDoubleEditor(double minValue, double maxValue)
	{
		MinValue = minValue;
		MaxValue = maxValue;
	}

	public PropertyDoubleEditor(bool showUpDownButton, double minValue, double maxValue)
	{
		ShowUpDownButton = showUpDownButton;
		MinValue = minValue;
		MaxValue = maxValue;
	}

	public PropertyDoubleEditor(bool showUpDownButton, string displayFormat, double minValue, double maxValue)
	{
		ShowUpDownButton = showUpDownButton;
		DisplayFormat = displayFormat;
		MinValue = minValue;
		MaxValue = maxValue;
	}

	public PropertyDoubleEditor(bool showUpDownButton, string displayFormat)
	{
		ShowUpDownButton = showUpDownButton;
		DisplayFormat = displayFormat;
	}

	public override IPropertyValueEditor CreateEditor(PropertyDescriptor propertyDescriptor, object targetObject)
	{
		if ((object)propertyDescriptor.PropertyType != typeof(double) && (object)propertyDescriptor.PropertyType != typeof(double?))
		{
			throw new InvalidOperationException("PropertyDateTimeEditor works only with double type properties");
		}
		_2EC0 _2EC0 = new _2EC0();
		_2EC0._181B = 1;
		if (!string.IsNullOrEmpty(DisplayFormat))
		{
			_2EC0.DisplayFormat = DisplayFormat;
		}
		_2EC0.ShowUpDown = ShowUpDownButton;
		((Control)_2EC0).Height = Dpi.Height14;
		_2EC0.BackgroundStyle.Class = "";
		_2EC0.BackgroundStyle.BackColor = Color.White;
		_2EC0.MinValue = MinValue;
		_2EC0.MaxValue = MaxValue;
		_2EC0.AllowEmptyState = AllowEmptyState;
		return _2EC0;
	}
}
