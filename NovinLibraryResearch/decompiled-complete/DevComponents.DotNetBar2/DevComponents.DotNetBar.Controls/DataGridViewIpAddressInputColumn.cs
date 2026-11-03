using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxItem(false)]
[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "IpAddressInput.ico")]
public class DataGridViewIpAddressInputColumn : DataGridViewTextBoxColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<CancelEventArgs> _៦;

	private EventHandler<EventArgs> _181E;

	private EventHandler<EventArgs> _181F;

	private EventHandler<CancelEventArgs> _៧;

	private IpAddressInput __2B43;

	private Bitmap __2AFE;

	private bool __2B23 = true;

	[Browsable(false)]
	internal IpAddressInput _2B42 => __2B43;

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	public bool AllowEmptyState
	{
		get
		{
			return __2B43.AllowEmptyState;
		}
		set
		{
			__2B43.AllowEmptyState = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Free-Text")]
	[Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
	public bool AutoOffFreeTextEntry
	{
		get
		{
			return __2B43.AutoOffFreeTextEntry;
		}
		set
		{
			__2B43.AutoOffFreeTextEntry = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	[DefaultValue(false)]
	public bool AutoOverwrite
	{
		get
		{
			return __2B43.AutoOverwrite;
		}
		set
		{
			__2B43.AutoOverwrite = value;
		}
	}

	[Category("Free-Text")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether free text entries are attempted to be auto-resolved to IP address as host/domain names.")]
	public bool AutoResolveFreeTextEntries
	{
		get
		{
			return __2B43.AutoResolveFreeTextEntries;
		}
		set
		{
			__2B43.AutoResolveFreeTextEntries = value;
		}
	}

	[Browsable(false)]
	public Color BackColor
	{
		get
		{
			return ((Control)__2B43).BackColor;
		}
		set
		{
			((Control)__2B43).BackColor = value;
		}
	}

	[Description("Gets or sets control background style.")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	public ElementStyle BackgroundStyle => __2B43.BackgroundStyle;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => __2B43.ButtonClear;

	[Browsable(true)]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => __2B43.ButtonCustom;

	[Browsable(true)]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	public InputButtonSettings ButtonCustom2 => __2B43.ButtonCustom2;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[Category("Buttons")]
	public InputButtonSettings ButtonDropDown => __2B43.ButtonDropDown;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control will be displayed for the current cell only.")]
	[Browsable(true)]
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
				((Control)__2B43).Invalidate();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when the drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return __2B43.DropDownControl;
		}
		set
		{
			__2B43.DropDownControl = value;
		}
	}

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public SubItemsCollection DropDownItems => __2B43.DropDownItems;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the control can respond to user interaction.")]
	public bool Enabled
	{
		get
		{
			return ((Control)__2B43).Enabled;
		}
		set
		{
			((Control)__2B43).Enabled = value;
		}
	}

	[Description("Indicates color used as background color to highlight the text box when it has input focus and FocusHighlight is enabled.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Color FocusHighlightColor
	{
		get
		{
			return __2B43.FocusHighlightColor;
		}
		set
		{
			__2B43.FocusHighlightColor = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight the text box when it has input focus.")]
	[Category("Appearance")]
	[DefaultValue(false)]
	public bool FocusHighlightEnabled
	{
		get
		{
			return __2B43.FocusHighlightEnabled;
		}
		set
		{
			__2B43.FocusHighlightEnabled = value;
		}
	}

	[Browsable(false)]
	public Color ForeColor
	{
		get
		{
			return ((Control)__2B43).ForeColor;
		}
		set
		{
			((Control)__2B43).ForeColor = value;
		}
	}

	[DefaultValue(false)]
	[Category("Free-Text")]
	[Description("Indicates whether control input is in free-text input mode.")]
	[Browsable(true)]
	public bool FreeTextEntryMode
	{
		get
		{
			return __2B43.FreeTextEntryMode;
		}
		set
		{
			__2B43.FreeTextEntryMode = value;
		}
	}

	[Description("Indicates the Input Method Editor (IME) mode of the control.")]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)__2B43).ImeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)__2B43).ImeMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return __2B43.IsInputReadOnly;
		}
		set
		{
			__2B43.IsInputReadOnly = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	public bool LockUpdateChecked
	{
		get
		{
			return __2B43.LockUpdateChecked;
		}
		set
		{
			__2B43.LockUpdateChecked = value;
		}
	}

	[Description("Indicates the control's elements are aligned to support locales using right-to-left fonts.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	public RightToLeft RightToLeft
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)__2B43).RightToLeft;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)__2B43).RightToLeft = value;
		}
	}

	[DefaultValue(".")]
	[Description("List of characters that when pressed would select next input field.")]
	[Category("Behavior")]
	public string SelectNextInputCharacters
	{
		get
		{
			return __2B43.SelectNextInputCharacters;
		}
		set
		{
			__2B43.SelectNextInputCharacters = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	public bool ShowCheckBox
	{
		get
		{
			return __2B43.ShowCheckBox;
		}
		set
		{
			__2B43.ShowCheckBox = value;
		}
	}

	[Browsable(false)]
	public string Text
	{
		get
		{
			return ((Control)__2B43).Text;
		}
		set
		{
			((Control)__2B43).Text = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates watermark text color.")]
	[Category("Appearance")]
	public Color WatermarkColor
	{
		get
		{
			return __2B43.WatermarkColor;
		}
		set
		{
			__2B43.WatermarkColor = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	[Browsable(true)]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return __2B43.WatermarkEnabled;
		}
		set
		{
			__2B43.WatermarkEnabled = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates watermark font.")]
	[Browsable(true)]
	public Font WatermarkFont
	{
		get
		{
			return __2B43.WatermarkFont;
		}
		set
		{
			__2B43.WatermarkFont = value;
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string WatermarkText
	{
		get
		{
			return __2B43.WatermarkText;
		}
		set
		{
			__2B43.WatermarkText = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a IpAddressInput Cell is painted.")]
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

	public DataGridViewIpAddressInputColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewIpAddressInputCell();
		__2B43 = new IpAddressInput();
		__2B43.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewIpAddressBorderKey;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		__2B43.ResetBackgroundStyle();
		__2B43.BackgroundStyle.Class = "TextBoxBorder";
		__2B43.BackgroundStyle.CornerType = eCornerType.Square;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return __2B43.ShouldSerializeFocusHighlightColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetFocusHighlightColor()
	{
		__2B43.ResetFocusHighlightColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return __2B43.ShouldSerializeWatermarkColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		__2B43.ResetWatermarkColor();
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
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewIpAddressInputColumn dataGridViewIpAddressInputColumn = ((DataGridViewColumn)this).Clone() as DataGridViewIpAddressInputColumn;
		if (dataGridViewIpAddressInputColumn != null)
		{
			dataGridViewIpAddressInputColumn.AllowEmptyState = AllowEmptyState;
			dataGridViewIpAddressInputColumn.AutoOffFreeTextEntry = AutoOffFreeTextEntry;
			dataGridViewIpAddressInputColumn.AutoOverwrite = AutoOverwrite;
			dataGridViewIpAddressInputColumn.AutoResolveFreeTextEntries = AutoResolveFreeTextEntries;
			dataGridViewIpAddressInputColumn.DropDownControl = DropDownControl;
			dataGridViewIpAddressInputColumn.Enabled = Enabled;
			dataGridViewIpAddressInputColumn.FocusHighlightColor = FocusHighlightColor;
			dataGridViewIpAddressInputColumn.FocusHighlightEnabled = FocusHighlightEnabled;
			dataGridViewIpAddressInputColumn.FreeTextEntryMode = FreeTextEntryMode;
			dataGridViewIpAddressInputColumn.ImeMode = ImeMode;
			dataGridViewIpAddressInputColumn.IsInputReadOnly = IsInputReadOnly;
			dataGridViewIpAddressInputColumn.LockUpdateChecked = LockUpdateChecked;
			dataGridViewIpAddressInputColumn.RightToLeft = RightToLeft;
			dataGridViewIpAddressInputColumn.SelectNextInputCharacters = SelectNextInputCharacters;
			dataGridViewIpAddressInputColumn.ShowCheckBox = ShowCheckBox;
			dataGridViewIpAddressInputColumn.WatermarkColor = WatermarkColor;
			dataGridViewIpAddressInputColumn.WatermarkEnabled = WatermarkEnabled;
			dataGridViewIpAddressInputColumn.WatermarkFont = WatermarkFont;
			dataGridViewIpAddressInputColumn.WatermarkText = WatermarkText;
			dataGridViewIpAddressInputColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
			dataGridViewIpAddressInputColumn.BackgroundStyle.ApplyStyle(_2B42.BackgroundStyle);
			dataGridViewIpAddressInputColumn.BackgroundStyle.Class = _2B42.BackgroundStyle.Class;
			_2B42.ButtonClear.ᡄ(dataGridViewIpAddressInputColumn._2B42.ButtonClear);
			_2B42.ButtonDropDown.ᡄ(dataGridViewIpAddressInputColumn._2B42.ButtonDropDown);
			_2B42.ButtonCustom.ᡄ(dataGridViewIpAddressInputColumn._2B42.ButtonCustom);
			_2B42.ButtonCustom2.ᡄ(dataGridViewIpAddressInputColumn._2B42.ButtonCustom2);
		}
		return dataGridViewIpAddressInputColumn;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)__2B43).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
