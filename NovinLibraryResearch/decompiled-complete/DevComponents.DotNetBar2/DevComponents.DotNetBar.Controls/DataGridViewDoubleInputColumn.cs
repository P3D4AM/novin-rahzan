using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "DoubleInput.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewDoubleInputColumn : DataGridViewTextBoxColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<CancelEventArgs> _៦;

	private EventHandler<EventArgs> _181E;

	private EventHandler<EventArgs> _181F;

	private EventHandler<CancelEventArgs> _៧;

	private EventHandler<CancelEventArgs> _17FC;

	private EventHandler<FreeTextEntryConversionEventArgs> _17FB;

	private DoubleInput __29C5;

	private Bitmap __2AFE;

	private bool __2B23 = true;

	internal DoubleInput _2B37 => __29C5;

	[Description("Gets or sets control background style.")]
	[Browsable(true)]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => __29C5.BackgroundStyle;

	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[Category("Buttons")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => __29C5.ButtonClear;

	[Browsable(true)]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	public InputButtonSettings ButtonCustom => __29C5.ButtonCustom;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[Browsable(true)]
	public InputButtonSettings ButtonCustom2 => __29C5.ButtonCustom2;

	[Browsable(true)]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	public InputButtonSettings ButtonDropDown => __29C5.ButtonDropDown;

	[Category("Buttons")]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonFreeText => __29C5.ButtonFreeText;

	[Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
	[Browsable(true)]
	[DefaultValue("")]
	public string DisplayFormat
	{
		get
		{
			return __29C5.DisplayFormat;
		}
		set
		{
			__29C5.DisplayFormat = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether the control will be displayed for the current cell only.")]
	[Browsable(true)]
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
				((Control)__29C5).Invalidate();
			}
		}
	}

	[Description("Indicates whether the control can respond to user interaction.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	public bool Enabled
	{
		get
		{
			return ((Control)__29C5).Enabled;
		}
		set
		{
			((Control)__29C5).Enabled = value;
		}
	}

	[Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
	[DefaultValue(1)]
	[Browsable(true)]
	public double Increment
	{
		get
		{
			return __29C5.Increment;
		}
		set
		{
			__29C5.Increment = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates alignment of input fields inside of the control.")]
	[DefaultValue(eHorizontalAlignment.Right)]
	[Category("Appearance")]
	public eHorizontalAlignment InputHorizontalAlignment
	{
		get
		{
			return __29C5.InputHorizontalAlignment;
		}
		set
		{
			__29C5.InputHorizontalAlignment = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	[DefaultValue(true)]
	public bool LockUpdateChecked
	{
		get
		{
			return __29C5.LockUpdateChecked;
		}
		set
		{
			__29C5.LockUpdateChecked = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates maximum value that can be entered.")]
	[DefaultValue(double.MaxValue)]
	public double MaxValue
	{
		get
		{
			return __29C5.MaxValue;
		}
		set
		{
			__29C5.MaxValue = value;
		}
	}

	[Description("Indicates minimum value that can be entered.")]
	[DefaultValue(double.MinValue)]
	[Browsable(true)]
	public double MinValue
	{
		get
		{
			return __29C5.MinValue;
		}
		set
		{
			__29C5.MinValue = value;
		}
	}

	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool ShowCheckBox
	{
		get
		{
			return __29C5.ShowCheckBox;
		}
		set
		{
			__29C5.ShowCheckBox = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	public bool ShowUpDown
	{
		get
		{
			return __29C5.ShowUpDown;
		}
		set
		{
			__29C5.ShowUpDown = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a DoubleInput Cell is painted.")]
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

	public DataGridViewDoubleInputColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewDoubleInputCell();
		__29C5 = new DoubleInput();
		__29C5.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewNumericBorderKey;
		((Control)__29C5).Visible = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		__29C5.ResetBackgroundStyle();
		__29C5.BackgroundStyle.Class = "TextBoxBorder";
		__29C5.BackgroundStyle.CornerType = eCornerType.Square;
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
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = ((DataGridViewColumn)this).Clone() as DataGridViewDoubleInputColumn;
		if (dataGridViewDoubleInputColumn != null)
		{
			dataGridViewDoubleInputColumn.DisplayFormat = DisplayFormat;
			dataGridViewDoubleInputColumn.Enabled = Enabled;
			dataGridViewDoubleInputColumn.Increment = Increment;
			dataGridViewDoubleInputColumn.InputHorizontalAlignment = InputHorizontalAlignment;
			dataGridViewDoubleInputColumn.LockUpdateChecked = LockUpdateChecked;
			dataGridViewDoubleInputColumn.MaxValue = MaxValue;
			dataGridViewDoubleInputColumn.MinValue = MinValue;
			dataGridViewDoubleInputColumn.ShowCheckBox = ShowCheckBox;
			dataGridViewDoubleInputColumn.ShowUpDown = ShowUpDown;
			dataGridViewDoubleInputColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
			dataGridViewDoubleInputColumn.BackgroundStyle.ApplyStyle(_2B37.BackgroundStyle);
			dataGridViewDoubleInputColumn.BackgroundStyle.Class = _2B37.BackgroundStyle.Class;
			_2B37.ButtonClear.ᡄ(dataGridViewDoubleInputColumn._2B37.ButtonClear);
			_2B37.ButtonDropDown.ᡄ(dataGridViewDoubleInputColumn._2B37.ButtonDropDown);
			_2B37.ButtonFreeText.ᡄ(dataGridViewDoubleInputColumn._2B37.ButtonFreeText);
			_2B37.ButtonCustom.ᡄ(dataGridViewDoubleInputColumn._2B37.ButtonCustom);
			_2B37.ButtonCustom2.ᡄ(dataGridViewDoubleInputColumn._2B37.ButtonCustom2);
		}
		return dataGridViewDoubleInputColumn;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)__29C5).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
