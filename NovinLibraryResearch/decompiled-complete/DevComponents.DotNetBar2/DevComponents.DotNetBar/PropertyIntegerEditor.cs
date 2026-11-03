using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar;

public class PropertyIntegerEditor : PropertyValueEditor
{
	private class _2EC1 : IntegerInput, IPropertyValueEditor
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
					base.Value = (int)value;
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

	public int MinValue = int.MinValue;

	public int MaxValue = int.MaxValue;

	public bool AllowEmptyState;

	public PropertyIntegerEditor()
	{
	}

	public PropertyIntegerEditor(bool showUpDownButton)
	{
		ShowUpDownButton = showUpDownButton;
	}

	public PropertyIntegerEditor(int minValue, int maxValue)
	{
		MinValue = minValue;
		MaxValue = maxValue;
	}

	public PropertyIntegerEditor(bool showUpDownButton, int minValue, int maxValue)
	{
		ShowUpDownButton = showUpDownButton;
		MinValue = minValue;
		MaxValue = maxValue;
	}

	public PropertyIntegerEditor(bool showUpDownButton, string displayFormat, int minValue, int maxValue)
	{
		ShowUpDownButton = showUpDownButton;
		DisplayFormat = displayFormat;
		MinValue = minValue;
		MaxValue = maxValue;
	}

	public PropertyIntegerEditor(bool showUpDownButton, string displayFormat)
	{
		ShowUpDownButton = showUpDownButton;
		DisplayFormat = displayFormat;
	}

	public override IPropertyValueEditor CreateEditor(PropertyDescriptor propertyDescriptor, object targetObject)
	{
		if ((object)propertyDescriptor.PropertyType != typeof(int) && (object)propertyDescriptor.PropertyType != typeof(int?) && (object)propertyDescriptor.PropertyType != typeof(byte) && (object)propertyDescriptor.PropertyType != typeof(byte?))
		{
			throw new InvalidOperationException("PropertyIntegerEditor works only with int or byte type properties");
		}
		_2EC1 _2EC1 = new _2EC1();
		_2EC1._181B = 1;
		if (!string.IsNullOrEmpty(DisplayFormat))
		{
			_2EC1.DisplayFormat = DisplayFormat;
		}
		_2EC1.ShowUpDown = ShowUpDownButton;
		((Control)_2EC1).Height = Dpi.Height14;
		_2EC1.BackgroundStyle.Class = "";
		_2EC1.BackgroundStyle.BackColor = Color.White;
		_2EC1.AllowEmptyState = AllowEmptyState;
		if ((object)propertyDescriptor.PropertyType == typeof(byte) || (object)propertyDescriptor.PropertyType == typeof(byte?))
		{
			_2EC1.MinValue = Math.Max(0, MinValue);
			_2EC1.MaxValue = Math.Min(255, MaxValue);
		}
		else
		{
			_2EC1.MinValue = MinValue;
			_2EC1.MaxValue = MaxValue;
		}
		return _2EC1;
	}
}
