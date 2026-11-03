using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "IntegerInput.ico")]
[ToolboxItem(false)]
public class DataGridViewIntegerInputColumn : DataGridViewTextBoxColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<CancelEventArgs> _៦;

	private EventHandler<EventArgs> _181E;

	private EventHandler<EventArgs> _181F;

	private EventHandler<CancelEventArgs> _៧;

	private EventHandler<CancelEventArgs> _17FC;

	private EventHandler<FreeTextEntryConversionEventArgs> _17FB;

	private IntegerInput __2B39;

	private Bitmap __2AFE;

	private bool __2B23 = true;

	internal IntegerInput _2B38 => __2B39;

	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Browsable(true)]
	public ElementStyle BackgroundStyle => __2B39.BackgroundStyle;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	public InputButtonSettings ButtonClear => __2B39.ButtonClear;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => __2B39.ButtonCustom;

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public InputButtonSettings ButtonCustom2 => __2B39.ButtonCustom2;

	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public InputButtonSettings ButtonDropDown => __2B39.ButtonDropDown;

	[Browsable(true)]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	public InputButtonSettings ButtonFreeText => __2B39.ButtonFreeText;

	[Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
	[Browsable(true)]
	[DefaultValue("")]
	public string DisplayFormat
	{
		get
		{
			return __2B39.DisplayFormat;
		}
		set
		{
			__2B39.DisplayFormat = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control will be displayed for the current cell only.")]
	[DefaultValue(true)]
	public bool DisplayControlForCurrentCellOnly
	{
		get
		{
			return __2B23;
		}
		set
		{
			if (__2B23 != value)
			{
				__2B23 = value;
				((Control)__2B39).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the control can respond to user interaction.")]
	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return ((Control)__2B39).Enabled;
		}
		set
		{
			((Control)__2B39).Enabled = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
	[DefaultValue(1)]
	public int Increment
	{
		get
		{
			return __2B39.Increment;
		}
		set
		{
			__2B39.Increment = value;
		}
	}

	[DefaultValue(eHorizontalAlignment.Right)]
	[Description("Indicates alignment of input fields inside of the control.")]
	[Category("Appearance")]
	[Browsable(true)]
	public eHorizontalAlignment InputHorizontalAlignment
	{
		get
		{
			return __2B39.InputHorizontalAlignment;
		}
		set
		{
			__2B39.InputHorizontalAlignment = value;
		}
	}

	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool LockUpdateChecked
	{
		get
		{
			return __2B39.LockUpdateChecked;
		}
		set
		{
			__2B39.LockUpdateChecked = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates maximum value that can be entered.")]
	[DefaultValue(int.MaxValue)]
	public int MaxValue
	{
		get
		{
			return __2B39.MaxValue;
		}
		set
		{
			__2B39.MaxValue = value;
		}
	}

	[DefaultValue(int.MinValue)]
	[Browsable(true)]
	[Description("Indicates minimum value that can be entered.")]
	public int MinValue
	{
		get
		{
			return __2B39.MinValue;
		}
		set
		{
			__2B39.MinValue = value;
		}
	}

	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool ShowCheckBox
	{
		get
		{
			return __2B39.ShowCheckBox;
		}
		set
		{
			__2B39.ShowCheckBox = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	public bool ShowUpDown
	{
		get
		{
			return __2B39.ShowUpDown;
		}
		set
		{
			__2B39.ShowUpDown = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before an IntegerInput Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint
	{
		add
		{
			EventHandler<BeforeCellPaintEventArgs> eventHandler = _2AF8;
			EventHandler<BeforeCellPaintEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BeforeCellPaintEventArgs> value2 = (EventHandler<BeforeCellPaintEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AF8, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<BeforeCellPaintEventArgs> eventHandler = _2AF8;
			EventHandler<BeforeCellPaintEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BeforeCellPaintEventArgs> value2 = (EventHandler<BeforeCellPaintEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AF8, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.")]
	public event EventHandler<CancelEventArgs> ButtonClearClick
	{
		add
		{
			EventHandler<CancelEventArgs> eventHandler = _៦;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៦, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<CancelEventArgs> eventHandler = _៦;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៦, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when ButtonCustom control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustomClick
	{
		add
		{
			EventHandler<EventArgs> eventHandler = _181E;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = _181E;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when ButtonCustom2 control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustom2Click
	{
		add
		{
			EventHandler<EventArgs> eventHandler = _181F;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = _181F;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Drop-Down button that shows calendar is clicked and allows you to cancel showing of the popup.")]
	public event EventHandler<CancelEventArgs> ButtonDropDownClick
	{
		add
		{
			EventHandler<CancelEventArgs> eventHandler = _៧;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៧, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<CancelEventArgs> eventHandler = _៧;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៧, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Free-Text button is clicked and allows you to cancel its default action.")]
	public event EventHandler<CancelEventArgs> ButtonFreeTextClick
	{
		add
		{
			EventHandler<CancelEventArgs> eventHandler = _17FC;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17FC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<CancelEventArgs> eventHandler = _17FC;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17FC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value the control expects.")]
	public event EventHandler<FreeTextEntryConversionEventArgs> ConvertFreeTextEntry
	{
		add
		{
			EventHandler<FreeTextEntryConversionEventArgs> eventHandler = _17FB;
			EventHandler<FreeTextEntryConversionEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<FreeTextEntryConversionEventArgs> value2 = (EventHandler<FreeTextEntryConversionEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17FB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<FreeTextEntryConversionEventArgs> eventHandler = _17FB;
			EventHandler<FreeTextEntryConversionEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<FreeTextEntryConversionEventArgs> value2 = (EventHandler<FreeTextEntryConversionEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17FB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public DataGridViewIntegerInputColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewIntegerInputCell();
		__2B39 = new IntegerInput();
		__2B39.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewNumericBorderKey;
		((Control)__2B39).Visible = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		__2B39.ResetBackgroundStyle();
		__2B39.BackgroundStyle.Class = "TextBoxBorder";
		__2B39.BackgroundStyle.CornerType = eCornerType.Square;
	}

	internal void _2B31(object _٠, CancelEventArgs ؿ)
	{
		if (_៦ != null)
		{
			_៦(this, ؿ);
		}
	}

	internal void _2B32(object _٠, EventArgs ؿ)
	{
		if (_181E != null)
		{
			_181E(this, ؿ);
		}
	}

	internal void _2B33(object _٠, EventArgs ؿ)
	{
		if (_181F != null)
		{
			_181F(this, ؿ);
		}
	}

	internal void _2B34(object _٠, CancelEventArgs ؿ)
	{
		if (_៧ != null)
		{
			_៧(this, ؿ);
		}
	}

	internal void _2B35(object _٠, CancelEventArgs ؿ)
	{
		if (_17FC != null)
		{
			_17FC(this, ؿ);
		}
	}

	internal void _2B36(object _٠, FreeTextEntryConversionEventArgs ؿ)
	{
		if (_17FB != null)
		{
			_17FB(this, ؿ);
		}
	}

	internal Bitmap _2B01(Rectangle _2AE9)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (__2AFE == null || ((Image)__2AFE).Width != _2AE9.Width || ((Image)__2AFE).Height < _2AE9.Height)
		{
			if (__2AFE != null)
			{
				((Image)__2AFE).Dispose();
			}
			__2AFE = new Bitmap(_2AE9.Width, _2AE9.Height);
		}
		return __2AFE;
	}

	internal void _2B02(int _2AEA, int _21A5)
	{
		if (_2AF8 != null)
		{
			_2AF8(this, new BeforeCellPaintEventArgs(_2AEA, _21A5));
		}
	}

	public override object Clone()
	{
		DataGridViewIntegerInputColumn dataGridViewIntegerInputColumn = ((DataGridViewColumn)this).Clone() as DataGridViewIntegerInputColumn;
		if (dataGridViewIntegerInputColumn != null)
		{
			dataGridViewIntegerInputColumn.DisplayFormat = DisplayFormat;
			dataGridViewIntegerInputColumn.Enabled = Enabled;
			dataGridViewIntegerInputColumn.Increment = Increment;
			dataGridViewIntegerInputColumn.InputHorizontalAlignment = InputHorizontalAlignment;
			dataGridViewIntegerInputColumn.LockUpdateChecked = LockUpdateChecked;
			dataGridViewIntegerInputColumn.MaxValue = MaxValue;
			dataGridViewIntegerInputColumn.MinValue = MinValue;
			dataGridViewIntegerInputColumn.ShowCheckBox = ShowCheckBox;
			dataGridViewIntegerInputColumn.ShowUpDown = ShowUpDown;
			dataGridViewIntegerInputColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
			dataGridViewIntegerInputColumn.BackgroundStyle.ApplyStyle(_2B38.BackgroundStyle);
			dataGridViewIntegerInputColumn.BackgroundStyle.Class = _2B38.BackgroundStyle.Class;
			_2B38.ButtonClear.ᡄ(dataGridViewIntegerInputColumn._2B38.ButtonClear);
			_2B38.ButtonDropDown.ᡄ(dataGridViewIntegerInputColumn._2B38.ButtonDropDown);
			_2B38.ButtonFreeText.ᡄ(dataGridViewIntegerInputColumn._2B38.ButtonFreeText);
			_2B38.ButtonCustom.ᡄ(dataGridViewIntegerInputColumn._2B38.ButtonCustom);
			_2B38.ButtonCustom2.ᡄ(dataGridViewIntegerInputColumn._2B38.ButtonCustom2);
		}
		return dataGridViewIntegerInputColumn;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)__2B39).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
