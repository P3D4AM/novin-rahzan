using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxBitmap(typeof(DotNetBarManager), "IpAddressInput.ico")]
[ToolboxItem(false)]
public class IpAddressInput : VisualControlBase, ICommandSource
{
	private IpAddressGroup _IpGroup;

	private InputButtonSettings _ButtonDropDown;

	private InputButtonSettings _ButtonClear;

	private InputButtonSettings _ButtonFreeText;

	private Control _DropDownControl;

	private ButtonItem _PopupItem;

	private Control _PreviousDropDownControlParent;

	private static string _DropDownControlContainerName = "sysPopupControlContainer";

	private static string _DropDownItemContainerName = "sysPopupItemContainer";

	private DateTime _CloseTime = DateTime.MinValue;

	private bool _ShowCheckBox;

	private bool _LockUpdateChecked;

	private string _Value;

	private bool _AutoResolveFreeTextEntries = true;

	private bool _AutoOffFreeTextEntry;

	private bool _FreeTextEntryMode;

	private FreeTextEntryBox _FreeTextEntryBox;

	private ICommand _Command;

	private object _CommandParameter;

	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => _ButtonDropDown;

	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => _ButtonClear;

	[Category("Buttons")]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonFreeText => _ButtonFreeText;

	[DefaultValue(false)]
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	public bool AutoOverwrite
	{
		get
		{
			return _IpGroup.AutoOverwrite;
		}
		set
		{
			_IpGroup.AutoOverwrite = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	public bool AllowEmptyState
	{
		get
		{
			return _IpGroup.AllowEmptyState;
		}
		set
		{
			_IpGroup.AllowEmptyState = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return _DropDownControl;
		}
		set
		{
			_DropDownControl = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection DropDownItems => _PopupItem.SubItems;

	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return _IpGroup.IsReadOnly;
		}
		set
		{
			_IpGroup.IsReadOnly = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	public bool ShowCheckBox
	{
		get
		{
			return _ShowCheckBox;
		}
		set
		{
			_ShowCheckBox = value;
			OnShowCheckBoxChanged();
		}
	}

	private LockUpdateCheckBox LockUpdateCheckBox
	{
		get
		{
			if (_IpGroup.Items[0] is LockUpdateCheckBox)
			{
				return (LockUpdateCheckBox)_IpGroup.Items[0];
			}
			return null;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	public bool LockUpdateChecked
	{
		get
		{
			return _LockUpdateChecked;
		}
		set
		{
			if (_LockUpdateChecked != value)
			{
				_LockUpdateChecked = value;
				LockUpdateCheckBox lockUpdateCheckBox = LockUpdateCheckBox;
				if (lockUpdateCheckBox != null)
				{
					lockUpdateCheckBox.Checked = _LockUpdateChecked;
				}
			}
		}
	}

	[DefaultValue(".")]
	[Category("Behavior")]
	[Description("List of characters that when pressed would select next input field.")]
	public string SelectNextInputCharacters
	{
		get
		{
			return _IpGroup.SelectNextInputCharacters;
		}
		set
		{
			if (_IpGroup.SelectNextInputCharacters != value)
			{
				_IpGroup.SelectNextInputCharacters = value;
			}
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the IP Address value.")]
	[Bindable(true)]
	public string Value
	{
		get
		{
			return _IpGroup.Value;
		}
		set
		{
			_Value = value;
			InternalValueChanged();
		}
	}

	protected override bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused && (_FreeTextEntryBox == null || !((Control)_FreeTextEntryBox).Focused))
			{
				return _IpGroup.IsEmpty;
			}
			return false;
		}
	}

	[DefaultValue(true)]
	[Category("Free-Text")]
	[Description("Indicates whether free text entries are attempted to be auto-resolved to IP address as host/domain names.")]
	public bool AutoResolveFreeTextEntries
	{
		get
		{
			return _AutoResolveFreeTextEntries;
		}
		set
		{
			_AutoResolveFreeTextEntries = value;
		}
	}

	[DefaultValue(false)]
	[Category("Free-Text")]
	[Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
	public bool AutoOffFreeTextEntry
	{
		get
		{
			return _AutoOffFreeTextEntry;
		}
		set
		{
			_AutoOffFreeTextEntry = value;
		}
	}

	[DefaultValue(false)]
	[Category("Free-Text")]
	[Description("Indicates whether control input is in free-text input mode.")]
	public bool FreeTextEntryMode
	{
		get
		{
			return _FreeTextEntryMode;
		}
		set
		{
			_FreeTextEntryMode = value;
			OnFreeTextEntryModeChanged();
		}
	}

	protected override bool SupportsFreeTextEntry => true;

	protected override bool IsFreeTextEntryVisible
	{
		get
		{
			if (_FreeTextEntryMode)
			{
				return base.IsKeyboardFocusWithin;
			}
			return false;
		}
	}

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	public Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	ICommand ICommandSource.Command
	{
		get
		{
			return _Command;
		}
		set
		{
			bool flag = false;
			if (_Command != value)
			{
				flag = true;
			}
			if (_Command != null)
			{
				CommandManager.UnRegisterCommandSource(this, _Command);
			}
			_Command = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public object CommandParameter
	{
		get
		{
			return _CommandParameter;
		}
		set
		{
			_CommandParameter = value;
		}
	}

	public event CancelEventHandler ButtonClearClick;

	public event CancelEventHandler ButtonDropDownClick;

	public event EventHandler LockUpdateChanged;

	public event EventHandler ValueChanged;

	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value control expects.")]
	[Category("Free-Text")]
	public event FreeTextEntryConversionEventHandler ConvertFreeTextEntry;

	public event CancelEventHandler ButtonFreeTextClick;

	public IpAddressInput()
	{
		_ButtonClear = new InputButtonSettings(this);
		_ButtonDropDown = new InputButtonSettings(this);
		_ButtonFreeText = new InputButtonSettings(this);
		_IpGroup = RootVisualItem as IpAddressGroup;
	}

	protected override VisualItem CreateRootVisual()
	{
		IpAddressGroup ipAddressGroup = new IpAddressGroup();
		ipAddressGroup.SelectNextInputCharacters = ".";
		ipAddressGroup.ValueChanged += InputItemValueChanged;
		return ipAddressGroup;
	}

	private void InputItemValueChanged(object sender, EventArgs e)
	{
		if (_Value != _IpGroup.Value)
		{
			Value = _IpGroup.Value;
		}
	}

	public virtual void Copy()
	{
		if (_IpGroup != null)
		{
			Clipboard.SetText(((Control)this).Text);
		}
	}

	public virtual void Paste()
	{
		if (_IpGroup != null)
		{
			((Control)this).Text = Clipboard.GetText();
		}
	}

	public virtual void Cut()
	{
		if (_IpGroup != null)
		{
			Copy();
			if (AllowEmptyState)
			{
				((Control)this).Text = null;
			}
		}
	}

	protected override VisualItem CreateButton(InputButtonSettings buttonSettings)
	{
		VisualItem visualItem = null;
		if (buttonSettings == _ButtonDropDown)
		{
			visualItem = new VisualDropDownButton();
			ApplyButtonSettings(buttonSettings, visualItem as VisualButton);
		}
		else
		{
			visualItem = base.CreateButton(buttonSettings);
		}
		VisualButton visualButton = visualItem as VisualButton;
		visualButton.ClickAutoRepeat = false;
		if (buttonSettings == _ButtonClear)
		{
			if (buttonSettings.Image == null)
			{
				visualButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.DateReset.png");
			}
		}
		else if (buttonSettings == _ButtonFreeText)
		{
			if (buttonSettings.Image == null)
			{
				visualButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.FreeText.png");
			}
			visualButton.Checked = buttonSettings.Checked;
		}
		return visualItem;
	}

	protected override PopupItem CreatePopupItem()
	{
		ButtonItem buttonItem = new ButtonItem("sysPopupProvider");
		buttonItem.PopupFinalized += DropDownPopupClose;
		_PopupItem = buttonItem;
		return buttonItem;
	}

	protected override SortedList CreateSortedButtonList()
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		SortedList sortedList = base.CreateSortedButtonList();
		if (_ButtonClear.Visible)
		{
			VisualItem visualItem = CreateButton(_ButtonClear);
			if (_ButtonClear.ItemReference != null)
			{
				_ButtonClear.ItemReference.Click -= ClearButtonClick;
			}
			_ButtonClear.ItemReference = visualItem;
			visualItem.Click += ClearButtonClick;
			sortedList.Add(_ButtonClear, visualItem);
		}
		if (_ButtonDropDown.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ButtonDropDown);
			if (_ButtonDropDown.ItemReference != null)
			{
				_ButtonDropDown.ItemReference.MouseDown -= new MouseEventHandler(DropDownButtonMouseDown);
			}
			_ButtonDropDown.ItemReference = visualItem2;
			visualItem2.MouseDown += new MouseEventHandler(DropDownButtonMouseDown);
			sortedList.Add(_ButtonDropDown, visualItem2);
		}
		if (_ButtonFreeText.Visible)
		{
			VisualItem visualItem3 = CreateButton(_ButtonFreeText);
			if (_ButtonFreeText.ItemReference != null)
			{
				_ButtonFreeText.ItemReference.Click -= FreeTextButtonClick;
			}
			_ButtonFreeText.ItemReference = visualItem3;
			visualItem3.Click += FreeTextButtonClick;
			sortedList.Add(_ButtonFreeText, visualItem3);
		}
		return sortedList;
	}

	private void DropDownButtonMouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576 || (_CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 150.0))
		{
			_CloseTime = DateTime.MinValue;
		}
		else
		{
			ShowDropDown();
		}
	}

	public void ShowDropDown()
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if (_DropDownControl == null && _PopupItem.SubItems.Count == 0)
		{
			return;
		}
		if (_PopupItem.Expanded)
		{
			_PopupItem.Expanded = false;
			return;
		}
		ControlContainerItem controlContainerItem = null;
		ItemContainer itemContainer = null;
		if (_DropDownControl != null)
		{
			itemContainer = new ItemContainer();
			itemContainer.Name = _DropDownItemContainerName;
			controlContainerItem = new ControlContainerItem(_DropDownControlContainerName);
			itemContainer.SubItems.Add(controlContainerItem);
			_PopupItem.SubItems.Insert(0, itemContainer);
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonDropDownClick(cancelEventArgs);
		if (cancelEventArgs.Cancel || _PopupItem.SubItems.Count == 0)
		{
			if (itemContainer != null)
			{
				_PopupItem.SubItems.Remove(itemContainer);
			}
			return;
		}
		_PreviousDropDownControlParent = _DropDownControl.Parent;
		controlContainerItem.Control = _DropDownControl;
		_PopupItem.SetDisplayRectangle(((Control)this).ClientRectangle);
		if ((int)((Control)this).RightToLeft == 0)
		{
			Point point = new Point(((Control)this).Width - _PopupItem.PopupSize.Width, ((Control)this).Height);
			ScreenInformation screenInformation = BarFunctions.ScreenFromControl((Control)(object)this);
			Point point2 = ((Control)this).PointToScreen(point);
			if (screenInformation != null && screenInformation.WorkingArea.X > point2.X)
			{
				point.X = 0;
			}
			_PopupItem.PopupLocation = point;
		}
		_PopupItem.Expanded = !_PopupItem.Expanded;
	}

	public void CloseDropDown()
	{
		if (_PopupItem.Expanded)
		{
			_PopupItem.Expanded = false;
		}
	}

	private void DropDownPopupClose(object sender, EventArgs e)
	{
		_CloseTime = DateTime.Now;
		if (!(_PopupItem.SubItems[_DropDownItemContainerName] is ItemContainer itemContainer))
		{
			return;
		}
		if (itemContainer.SubItems[_DropDownControlContainerName] is ControlContainerItem controlContainerItem)
		{
			controlContainerItem.Control = null;
			itemContainer.SubItems.Remove(controlContainerItem);
			if (_DropDownControl != null)
			{
				_DropDownControl.Parent = _PreviousDropDownControlParent;
				_PreviousDropDownControlParent = null;
			}
		}
		_PopupItem.SubItems.Remove(itemContainer);
	}

	private void ClearButtonClick(object sender, EventArgs e)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonClearClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			_IpGroup.Value = null;
		}
	}

	protected virtual void OnButtonClearClick(CancelEventArgs e)
	{
		if (this.ButtonClearClick != null)
		{
			this.ButtonClearClick(this, e);
		}
	}

	protected virtual void OnButtonDropDownClick(CancelEventArgs e)
	{
		if (this.ButtonDropDownClick != null)
		{
			this.ButtonDropDownClick(this, e);
		}
	}

	protected virtual void OnShowCheckBoxChanged()
	{
		LockUpdateCheckBox lockUpdateCheckBox = LockUpdateCheckBox;
		if (lockUpdateCheckBox != null)
		{
			_IpGroup.Items.Remove(lockUpdateCheckBox);
			lockUpdateCheckBox.CheckedChanged -= LockCheckedChanged;
		}
		if (ShowCheckBox && !(_IpGroup.Items[0] is LockUpdateCheckBox))
		{
			lockUpdateCheckBox = new LockUpdateCheckBox();
			lockUpdateCheckBox.CheckedChanged += LockCheckedChanged;
			_IpGroup.Items.Insert(0, lockUpdateCheckBox);
			lockUpdateCheckBox.Checked = _LockUpdateChecked;
		}
	}

	private void LockCheckedChanged(object sender, EventArgs e)
	{
		LockUpdateCheckBox lockUpdateCheckBox = LockUpdateCheckBox;
		if (lockUpdateCheckBox != null)
		{
			_LockUpdateChecked = lockUpdateCheckBox.Checked;
		}
		OnLockUpdateChanged(e);
	}

	protected virtual void OnLockUpdateChanged(EventArgs e)
	{
		if (this.LockUpdateChanged != null)
		{
			this.LockUpdateChanged(this, e);
		}
	}

	private void InternalValueChanged()
	{
		if (_Value != _IpGroup.Value)
		{
			_IpGroup.Value = _Value;
			ExecuteCommand();
		}
		((Control)this).Text = _Value;
		if (FreeTextEntryMode && _FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Text = _Value;
		}
		OnValueChanged(EventArgs.Empty);
	}

	protected virtual void OnValueChanged(EventArgs eventArgs)
	{
		this.ValueChanged?.Invoke(this, eventArgs);
	}

	private void FreeTextButtonClick(object sender, EventArgs e)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonFreeTextClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			FreeTextEntryMode = !FreeTextEntryMode;
			_ButtonFreeText.Checked = FreeTextEntryMode;
			if (FreeTextEntryMode && _FreeTextEntryBox != null && !((Control)_FreeTextEntryBox).Focused)
			{
				((Control)_FreeTextEntryBox).Focus();
			}
		}
	}

	protected virtual void OnButtonFreeTextClick(CancelEventArgs e)
	{
		this.ButtonFreeTextClick?.Invoke(this, e);
	}

	private void OnFreeTextEntryModeChanged()
	{
		if (!_FreeTextEntryMode)
		{
			if (_FreeTextEntryBox != null)
			{
				_FreeTextEntryBox.ApplyValue -= ApplyFreeTextValue;
				_FreeTextEntryBox.RevertValue -= RevertFreeTextValue;
				((Control)_FreeTextEntryBox).LostFocus -= FreeTextLostFocus;
				((Control)this).Controls.Remove((Control)(object)_FreeTextEntryBox);
				((Component)(object)_FreeTextEntryBox).Dispose();
				_FreeTextEntryBox = null;
			}
		}
		else
		{
			UpdateFreeTextBoxVisibility();
		}
		if (_ButtonFreeText != null)
		{
			_ButtonFreeText.Checked = _FreeTextEntryMode;
		}
	}

	protected override void OnIsKeyboardFocusWithinChanged()
	{
		if (_FreeTextEntryMode)
		{
			UpdateFreeTextBoxVisibility();
			if (base.IsKeyboardFocusWithin)
			{
				Control freeTextBox = GetFreeTextBox();
				if (!freeTextBox.Focused)
				{
					freeTextBox.Focus();
				}
			}
		}
		base.OnIsKeyboardFocusWithinChanged();
	}

	private void UpdateFreeTextBoxVisibility()
	{
		FreeTextEntryBox freeTextEntryBox = (FreeTextEntryBox)(object)GetFreeTextBox();
		if (base.IsKeyboardFocusWithin)
		{
			((Control)freeTextEntryBox).Visible = true;
			((Control)freeTextEntryBox).Text = Value;
			RootVisualItem.InvalidateArrange();
			((Control)this).Invalidate();
		}
		else if (((Control)freeTextEntryBox).Visible)
		{
			if (((Control)freeTextEntryBox).Focused)
			{
				freeTextEntryBox.HideOnLostFocus();
			}
			else
			{
				((Control)freeTextEntryBox).Visible = false;
			}
			RootVisualItem.InvalidateArrange();
			((Control)this).Invalidate();
		}
	}

	protected override Control GetFreeTextBox()
	{
		if (_FreeTextEntryBox == null)
		{
			_FreeTextEntryBox = new FreeTextEntryBox();
			_FreeTextEntryBox.ApplyValue += ApplyFreeTextValue;
			_FreeTextEntryBox.RevertValue += RevertFreeTextValue;
			((Control)_FreeTextEntryBox).LostFocus += FreeTextLostFocus;
			((TextBoxBase)_FreeTextEntryBox).BorderStyle = (BorderStyle)0;
			((Control)this).Controls.Add((Control)(object)_FreeTextEntryBox);
		}
		return (Control)(object)_FreeTextEntryBox;
	}

	private void RevertFreeTextValue(object sender, EventArgs e)
	{
		if (_FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Text = Value;
		}
	}

	protected virtual void OnConvertFreeTextEntry(FreeTextEntryConversionEventArgs e)
	{
		this.ConvertFreeTextEntry?.Invoke(this, e);
	}

	private void ApplyFreeTextValue(object sender, EventArgs e)
	{
		if (_FreeTextEntryBox == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(((Control)_FreeTextEntryBox).Text))
		{
			Value = null;
			return;
		}
		if (_IpGroup.IsValueValid(((Control)_FreeTextEntryBox).Text) && _AutoResolveFreeTextEntries)
		{
			Value = ((Control)_FreeTextEntryBox).Text;
			return;
		}
		FreeTextEntryConversionEventArgs freeTextEntryConversionEventArgs = new FreeTextEntryConversionEventArgs(((Control)_FreeTextEntryBox).Text);
		OnConvertFreeTextEntry(freeTextEntryConversionEventArgs);
		if (freeTextEntryConversionEventArgs.IsValueConverted)
		{
			if (freeTextEntryConversionEventArgs.ControlValue is string && _IpGroup.IsValueValid((string)freeTextEntryConversionEventArgs.ControlValue))
			{
				Value = (string)freeTextEntryConversionEventArgs.ControlValue;
				return;
			}
			throw new ArgumentException("ControlValue assigned is not recognized as valid IP value.");
		}
		string text = null;
		if (_AutoResolveFreeTextEntries)
		{
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(((Control)_FreeTextEntryBox).Text);
				if (hostAddresses != null && hostAddresses.Length != 0)
				{
					text = hostAddresses[0].ToString();
					if (!_IpGroup.IsValueValid(text))
					{
						text = null;
					}
				}
			}
			catch
			{
				text = null;
			}
		}
		Value = text;
	}

	private void FreeTextLostFocus(object sender, EventArgs e)
	{
		if (_AutoOffFreeTextEntry && !base.IsKeyboardFocusWithin)
		{
			FreeTextEntryMode = false;
		}
	}

	protected override void HideFreeTextBoxEntry()
	{
		if (_FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Visible = false;
		}
	}

	protected virtual void ExecuteCommand()
	{
		if (_Command != null)
		{
			CommandManager.ExecuteCommand(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
