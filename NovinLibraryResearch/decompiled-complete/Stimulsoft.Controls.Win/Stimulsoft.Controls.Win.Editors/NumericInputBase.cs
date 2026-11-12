using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxItem(false)]
[DefaultEvent("ValueChanged")]
public abstract class NumericInputBase : VisualControlBase, ICommandSource
{
	private VisualInputGroup _InputGroup;

	private ButtonItem _PopupItem;

	private static string _DropDownControlContainerName = "sysPopupControlContainer";

	private static string _DropDownItemContainerName = "sysPopupItemContainer";

	private bool _ShowCheckBox;

	private bool _LockUpdateChecked = true;

	private bool _ShowUpDown;

	private InputButtonSettings _ButtonDropDown;

	private InputButtonSettings _ButtonClear;

	private InputButtonSettings _ButtonFreeText;

	private InputButtonSettings _ButtonCalculator;

	private string _DisplayFormat = "";

	private Calculator _DropDownCalculator;

	private bool _IsCalculatorDisplayed;

	private Control _PreviousDropDownControlParent;

	private DateTime _CloseTime = DateTime.MinValue;

	private Control _DropDownControl;

	private ICommand _Command;

	private object _CommandParameter;

	private Keys _FreeTextEntryToggleKey;

	private bool _AutoResolveFreeTextEntries = true;

	private bool _AutoOffFreeTextEntry;

	private bool _FreeTextEntryMode;

	internal FreeTextEntryBox _FreeTextEntryBox;

	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only. When set to true the input part of the control becomes read-only and does not allow the typing. However, drop-down and up/down buttons part if visible still allows user to possibly change the value of the control through the method you can provide on drop-down or through up/down buttons. Use this property to allow change of the value through drop-down or up/down buttons only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return _InputGroup.IsReadOnly;
		}
		set
		{
			_InputGroup.IsReadOnly = value;
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
			if (_InputGroup.Items[0] is LockUpdateCheckBox)
			{
				return (LockUpdateCheckBox)_InputGroup.Items[0];
			}
			return null;
		}
	}

	[DefaultValue(true)]
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

	[DefaultValue(false)]
	public bool ShowUpDown
	{
		get
		{
			return _ShowUpDown;
		}
		set
		{
			_ShowUpDown = value;
			OnShowUpDownChanged();
		}
	}

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

	[Category("Buttons")]
	[Description("Describes the settings for the button that displays popup calculator.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCalculator => _ButtonCalculator;

	[DefaultValue("")]
	[Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
	public string DisplayFormat
	{
		get
		{
			return _DisplayFormat;
		}
		set
		{
			if (value != null)
			{
				string text = 12.ToString(value);
				_DisplayFormat = value;
				OnDisplayFormatChanged();
				((Control)this).Invalidate();
				RootVisualItem.InvalidateArrange();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	public bool AllowEmptyState
	{
		get
		{
			return _InputGroup.AllowEmptyState;
		}
		set
		{
			_InputGroup.AllowEmptyState = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsEmpty
	{
		get
		{
			return _InputGroup.IsEmpty;
		}
		set
		{
			_InputGroup.IsEmpty = value;
		}
	}

	protected override bool EatMenuFilterKeyDown
	{
		get
		{
			if (_IsCalculatorDisplayed && _DropDownCalculator != null)
			{
				return false;
			}
			return base.EatMenuFilterKeyDown;
		}
	}

	protected bool IsCalculatorDisplayed => _IsCalculatorDisplayed;

	[Browsable(true)]
	[DefaultValue(eHorizontalAlignment.Right)]
	[Category("Appearance")]
	[Description("Indicates alignment of input fields inside of the control.")]
	public override eHorizontalAlignment InputHorizontalAlignment
	{
		get
		{
			return base.InputHorizontalAlignment;
		}
		set
		{
			base.InputHorizontalAlignment = value;
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
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	public bool AutoOverwrite
	{
		get
		{
			return _InputGroup.AutoOverwrite;
		}
		set
		{
			_InputGroup.AutoOverwrite = value;
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

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Free-Text")]
	[Description("")]
	public Keys FreeTextEntryToggleKey
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _FreeTextEntryToggleKey;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_FreeTextEntryToggleKey = value;
		}
	}

	[DefaultValue(true)]
	[Category("Free-Text")]
	[Description("Indicates whether free text entries are attempted to be auto-resolved to dates like Today to today's date or Now to date and time now etc.")]
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

	public event EventHandler ValueChanged;

	public event EventHandler ValueObjectChanged;

	public event CancelEventHandler ButtonClearClick;

	public event CancelEventHandler ButtonDropDownClick;

	public event EventHandler LockUpdateChanged;

	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value control expects.")]
	[Category("Free-Text")]
	public event FreeTextEntryConversionEventHandler ConvertFreeTextEntry;

	public event CancelEventHandler ButtonFreeTextClick;

	public NumericInputBase()
	{
		_ButtonClear = new InputButtonSettings(this);
		_ButtonDropDown = new InputButtonSettings(this);
		_ButtonFreeText = new InputButtonSettings(this);
		_ButtonCalculator = new InputButtonSettings(this);
		_InputGroup = RootVisualItem as VisualInputGroup;
		_InputGroup.AutoOverwrite = false;
		InputHorizontalAlignment = eHorizontalAlignment.Right;
	}

	protected virtual void UpdateInputFieldAlignment()
	{
	}

	protected override void OnInputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
	{
		UpdateInputFieldAlignment();
		base.OnInputButtonSettingsChanged(inputButtonSettings);
	}

	protected override void OnInputHorizontalAlignmentChanged(eHorizontalAlignment oldValue, eHorizontalAlignment newValue)
	{
		UpdateInputFieldAlignment();
		base.OnInputHorizontalAlignmentChanged(oldValue, newValue);
	}

	protected virtual void OnShowCheckBoxChanged()
	{
		UpdateInputFieldAlignment();
		LockUpdateCheckBox lockUpdateCheckBox = LockUpdateCheckBox;
		if (lockUpdateCheckBox != null)
		{
			_InputGroup.Items.Remove(lockUpdateCheckBox);
			lockUpdateCheckBox.CheckedChanged -= LockCheckedChanged;
		}
		if (ShowCheckBox && !(_InputGroup.Items[0] is LockUpdateCheckBox))
		{
			lockUpdateCheckBox = new LockUpdateCheckBox();
			lockUpdateCheckBox.ItemType = eSystemItemType.SystemButton;
			lockUpdateCheckBox.CheckedChanged += LockCheckedChanged;
			_InputGroup.Items.Insert(0, lockUpdateCheckBox);
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
		if (_FreeTextEntryBox != null)
		{
			((TextBoxBase)_FreeTextEntryBox).ReadOnly = !LockUpdateChecked;
		}
	}

	protected override void RecreateButtons()
	{
		base.RecreateButtons();
		OnShowUpDownChanged();
		OnShowCheckBoxChanged();
	}

	protected virtual void OnShowUpDownChanged()
	{
		UpdateInputFieldAlignment();
		for (int num = _InputGroup.Items.Count - 1; num >= 0; num--)
		{
			if (_InputGroup.Items[num] is VisualUpDownButton)
			{
				VisualUpDownButton visualUpDownButton = (VisualUpDownButton)_InputGroup.Items[num];
				_InputGroup.Items.RemoveAt(num);
				break;
			}
		}
		if (ShowUpDown)
		{
			VisualUpDownButton visualUpDownButton2 = new VisualUpDownButton();
			visualUpDownButton2.Alignment = eItemAlignment.Right;
			visualUpDownButton2.AutoChange = eUpDownButtonAutoChange.FocusedItem;
			visualUpDownButton2.ItemType = eSystemItemType.SystemButton;
			visualUpDownButton2.Enabled = LockUpdateChecked;
			_InputGroup.Items.Add(visualUpDownButton2);
		}
	}

	public abstract void DecreaseValue();

	public abstract void IncreaseValue();

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
		else if (buttonSettings == _ButtonCalculator)
		{
			if (buttonSettings.Image == null)
			{
				visualButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.Calculator.png");
			}
			visualButton.Checked = buttonSettings.Checked;
		}
		return visualItem;
	}

	protected abstract void OnDisplayFormatChanged();

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
		if (_ButtonCalculator.Visible)
		{
			VisualItem visualItem4 = CreateButton(_ButtonCalculator);
			if (_ButtonCalculator.ItemReference != null)
			{
				_ButtonCalculator.ItemReference.Click -= CalculatorButtonClick;
			}
			_ButtonCalculator.ItemReference = visualItem4;
			visualItem4.Click += CalculatorButtonClick;
			sortedList.Add(_ButtonCalculator, visualItem4);
		}
		return sortedList;
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (_IsCalculatorDisplayed && _DropDownCalculator != null)
		{
			_DropDownCalculator.ProcessKeyPress(e);
		}
		base.OnKeyPress(e);
	}

	private void CalculatorButtonClick(object sender, EventArgs e)
	{
		if (_CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 150.0)
		{
			_CloseTime = DateTime.MinValue;
			return;
		}
		if (_DropDownCalculator == null)
		{
			_DropDownCalculator = new Calculator();
			((Control)_DropDownCalculator).AutoSize = true;
			_DropDownCalculator.FocusButtonsOnMouseDown = false;
			_DropDownCalculator.DisplayVisible = false;
			_DropDownCalculator.ValueChanged += DropDownCalculatorValueChanged;
			OnDropDownCalculatorCreated(_DropDownCalculator);
		}
		UpdateCalculatorValue(_DropDownCalculator);
		_IsCalculatorDisplayed = true;
		ShowDropDown((Control)(object)_DropDownCalculator);
	}

	protected virtual void UpdateCalculatorValue(Calculator calculator)
	{
	}

	protected virtual void OnDropDownCalculatorCreated(Calculator calculator)
	{
	}

	protected virtual void DropDownCalculatorValueChanged(object sender, ValueChangedEventArgs e)
	{
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
		ShowDropDown(_DropDownControl);
	}

	private void ShowDropDown(Control dropDownControl)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (dropDownControl == null && _PopupItem.SubItems.Count == 0)
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
		if (dropDownControl != null)
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
		_PreviousDropDownControlParent = dropDownControl.Parent;
		controlContainerItem.Control = dropDownControl;
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
		if (_PopupItem.SubItems[_DropDownItemContainerName] is ItemContainer itemContainer)
		{
			if (itemContainer.SubItems[_DropDownControlContainerName] is ControlContainerItem { Control: var control } controlContainerItem)
			{
				controlContainerItem.Control = null;
				itemContainer.SubItems.Remove(controlContainerItem);
				if (control != null)
				{
					control.Parent = _PreviousDropDownControlParent;
					_PreviousDropDownControlParent = null;
				}
			}
			_PopupItem.SubItems.Remove(itemContainer);
		}
		_IsCalculatorDisplayed = false;
	}

	private void ClearButtonClick(object sender, EventArgs e)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonClearClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			IsEmpty = true;
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

	protected virtual void OnValueChanged(EventArgs e)
	{
		if (!IsInitializing)
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, e);
			}
			if (this.ValueObjectChanged != null)
			{
				this.ValueObjectChanged(this, e);
			}
			ExecuteCommand();
		}
	}

	protected override PopupItem CreatePopupItem()
	{
		ButtonItem buttonItem = new ButtonItem("sysPopupProvider");
		buttonItem.PopupFinalized += DropDownPopupClose;
		_PopupItem = buttonItem;
		return buttonItem;
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

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if ((int)_FreeTextEntryToggleKey != 0 && keyData == _FreeTextEntryToggleKey)
		{
			FreeTextEntryMode = !FreeTextEntryMode;
			if (FreeTextEntryMode && _FreeTextEntryBox != null)
			{
				((Control)_FreeTextEntryBox).Focus();
			}
			return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
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
				if (FocusHighlightEnabled)
				{
					freeTextBox.BackColor = FocusHighlightColor;
				}
			}
			else if (FocusHighlightEnabled)
			{
				Control freeTextBox2 = GetFreeTextBox();
				freeTextBox2.BackColor = SystemColors.Window;
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
			((Control)freeTextEntryBox).Text = ((Control)this).Text;
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
			_FreeTextEntryBox.IncreaseValue += FreeTextIncreaseValue;
			_FreeTextEntryBox.DecreaseValue += FreeTextDecreaseValue;
			((TextBoxBase)_FreeTextEntryBox).BorderStyle = (BorderStyle)0;
			((TextBoxBase)_FreeTextEntryBox).ReadOnly = !LockUpdateChecked;
			if (InputHorizontalAlignment == eHorizontalAlignment.Right)
			{
				((TextBox)_FreeTextEntryBox).TextAlign = (HorizontalAlignment)1;
			}
			else if (InputHorizontalAlignment == eHorizontalAlignment.Center)
			{
				((TextBox)_FreeTextEntryBox).TextAlign = (HorizontalAlignment)2;
			}
			else if (InputHorizontalAlignment == eHorizontalAlignment.Left)
			{
				((TextBox)_FreeTextEntryBox).TextAlign = (HorizontalAlignment)0;
			}
			((Control)this).Controls.Add((Control)(object)_FreeTextEntryBox);
		}
		return (Control)(object)_FreeTextEntryBox;
	}

	private void FreeTextDecreaseValue(object sender, EventArgs e)
	{
		ApplyFreeTextValue();
		DecreaseValue();
	}

	private void FreeTextIncreaseValue(object sender, EventArgs e)
	{
		ApplyFreeTextValue();
		IncreaseValue();
	}

	private void RevertFreeTextValue(object sender, EventArgs e)
	{
		if (_FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Text = ((Control)this).Text;
		}
	}

	protected virtual void OnConvertFreeTextEntry(FreeTextEntryConversionEventArgs e)
	{
		this.ConvertFreeTextEntry?.Invoke(this, e);
	}

	private void ApplyFreeTextValue(object sender, EventArgs e)
	{
		ApplyFreeTextValue();
	}

	protected virtual void ApplyFreeTextValue()
	{
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
}
