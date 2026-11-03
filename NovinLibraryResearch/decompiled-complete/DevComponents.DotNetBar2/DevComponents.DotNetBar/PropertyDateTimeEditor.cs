using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar;

public class PropertyDateTimeEditor : PropertyValueEditor
{
	private class _2EBD : DateTimeInput, IPropertyValueEditor
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
				else if (base.Value != (DateTime)value)
				{
					base.Value = (DateTime)value;
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

	public eDateTimePickerFormat Format = eDateTimePickerFormat.Short;

	public string CustomFormat = "";

	public bool AllowEmptyState;

	public bool ShowDropDownButton = true;

	public DateTime MinDate = DateTimeGroup.MinDateTime;

	public DateTime MaxDate = DateTimeGroup.MaxDateTime;

	public PropertyDateTimeEditor(eDateTimePickerFormat format)
	{
		Format = format;
	}

	public PropertyDateTimeEditor(DateTime minDate, DateTime maxDate)
	{
		MinDate = minDate;
		MaxDate = maxDate;
	}

	public PropertyDateTimeEditor(eDateTimePickerFormat format, DateTime minDate, DateTime maxDate)
	{
		Format = format;
		MinDate = minDate;
		MaxDate = maxDate;
	}

	public PropertyDateTimeEditor(eDateTimePickerFormat format, string customFormat, bool allowEmptyState, bool showDropDownButton, DateTime minDate, DateTime maxDate)
	{
		Format = format;
		CustomFormat = customFormat;
		AllowEmptyState = allowEmptyState;
		ShowDropDownButton = showDropDownButton;
		MinDate = minDate;
		MaxDate = maxDate;
	}

	public PropertyDateTimeEditor(eDateTimePickerFormat format, bool allowEmptyState)
	{
		Format = format;
		AllowEmptyState = allowEmptyState;
	}

	public PropertyDateTimeEditor(string customFormat)
	{
		CustomFormat = customFormat;
	}

	public PropertyDateTimeEditor(string customFormat, bool allowEmptyState)
	{
		CustomFormat = customFormat;
		AllowEmptyState = allowEmptyState;
	}

	public PropertyDateTimeEditor()
	{
	}

	public override IPropertyValueEditor CreateEditor(PropertyDescriptor propertyDescriptor, object targetObject)
	{
		if ((object)propertyDescriptor.PropertyType != typeof(DateTime) && (object)propertyDescriptor.PropertyType != typeof(DateTime?))
		{
			throw new InvalidOperationException("PropertyDateTimeEditor works only with DateTime type properties");
		}
		_2EBD _2EBD = new _2EBD();
		_2EBD._181B = 1;
		if (!string.IsNullOrEmpty(CustomFormat))
		{
			_2EBD.Format = eDateTimePickerFormat.Custom;
			_2EBD.CustomFormat = CustomFormat;
		}
		else
		{
			_2EBD.Format = Format;
		}
		_2EBD.AllowEmptyState = AllowEmptyState;
		_2EBD.BackgroundStyle.Class = "";
		_2EBD.BackgroundStyle.BackColor = Color.White;
		_2EBD.MinDate = MinDate;
		_2EBD.MaxDate = MaxDate;
		if (AllowEmptyState)
		{
			_2EBD.ButtonClear.Visible = true;
			_2EBD.ButtonDropDown.DisplayPosition = 1;
		}
		((Control)_2EBD).Height = Dpi.Height14;
		_2EBD.ButtonDropDown.Visible = ShowDropDownButton;
		return _2EBD;
	}
}
