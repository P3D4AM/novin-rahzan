using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

[ToolboxItem(false)]
[DefaultEvent("ValueChanged")]
public abstract class NumericInputBase : VisualControlBase, ICommandSource
{
	private VisualInputGroup __22B8;

	private ButtonItem _០;

	private static string __22BA = "sysPopupControlContainer";

	private static string __22BB = "sysPopupItemContainer";

	private EventHandler ᗲ;

	private EventHandler _៤;

	private CancelEventHandler _៦;

	private CancelEventHandler _៧;

	private EventHandler _៩;

	private bool __17EB;

	private bool __17F0 = true;

	private bool __17EC;

	private InputButtonSettings __17F1;

	private InputButtonSettings __17F2;

	private InputButtonSettings __17F5;

	private InputButtonSettings __22BC;

	private string __22BD = "";

	private Calculator __22BE;

	private bool __22BF;

	private Control __22C0;

	private DateTime __17F4 = DateTime.MinValue;

	private bool __22C1;

	private eTriggerType __22C2;

	private Control __22C3;

	private ICommand _૦;

	private object _૧;

	private Keys __22C4;

	private FreeTextEntryConversionEventHandler _17FB;

	private CancelEventHandler _17FC;

	private bool __17FD = true;

	private bool __17FE;

	private bool __17FF;

	internal _232C __1800;

	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only. When set to true the input part of the control becomes read-only and does not allow the typing. However, drop-down and up/down buttons part if visible still allows user to possibly change the value of the control through the method you can provide on drop-down or through up/down buttons. Use this property to allow change of the value through drop-down or up/down buttons only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return __22B8.IsReadOnly;
		}
		set
		{
			__22B8.IsReadOnly = value;
			if (__1800 != null)
			{
				((TextBoxBase)__1800).ReadOnly = value || !LockUpdateChecked;
			}
		}
	}

	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	[DefaultValue(false)]
	public bool ShowCheckBox
	{
		get
		{
			return __17EB;
		}
		set
		{
			__17EB = value;
			OnShowCheckBoxChanged();
		}
	}

	private LockUpdateCheckBox _17DE
	{
		get
		{
			if (__22B8.Items[0] is LockUpdateCheckBox)
			{
				return (LockUpdateCheckBox)__22B8.Items[0];
			}
			return null;
		}
	}

	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	[DefaultValue(true)]
	public bool LockUpdateChecked
	{
		get
		{
			return __17F0;
		}
		set
		{
			if (__17F0 != value)
			{
				__17F0 = value;
				LockUpdateCheckBox lockUpdateCheckBox = _17DE;
				if (lockUpdateCheckBox != null)
				{
					lockUpdateCheckBox.Checked = __17F0;
				}
			}
		}
	}

	[DefaultValue(false)]
	public bool ShowUpDown
	{
		get
		{
			return __17EC;
		}
		set
		{
			__17EC = value;
			OnShowUpDownChanged();
		}
	}

	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => __17F1;

	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => __17F2;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	public InputButtonSettings ButtonFreeText => __17F5;

	[Description("Describes the settings for the button that displays popup calculator.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCalculator => __22BC;

	[DefaultValue("")]
	[Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
	public string DisplayFormat
	{
		get
		{
			return __22BD;
		}
		set
		{
			if (value != null)
			{
				12.ToString(value);
				__22BD = value;
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
			return __22B8.AllowEmptyState;
		}
		set
		{
			__22B8.AllowEmptyState = value;
			((Control)this).Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			return __22B8.IsEmpty;
		}
		set
		{
			bool flag = __22B8.IsEmpty != value;
			__22B8.IsEmpty = value;
			if (flag)
			{
				OnValueObjectChanged(EventArgs.Empty);
				ExecuteCommand();
			}
		}
	}

	protected override bool EatMenuFilterKeyDown
	{
		get
		{
			if (__22BF && __22BE != null)
			{
				return false;
			}
			return base.EatMenuFilterKeyDown;
		}
	}

	protected bool IsCalculatorDisplayed => __22BF;

	[Category("Behavior")]
	[Description("Specifies the behavior of ValueChanged event triggering.")]
	[DefaultValue(eTriggerType.Immediate)]
	public eTriggerType ValueChangedTrigger
	{
		get
		{
			return __22C2;
		}
		set
		{
			__22C2 = value;
		}
	}

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
			return __22C3;
		}
		set
		{
			__22C3 = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection DropDownItems => _០.SubItems;

	[DefaultValue(false)]
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	public bool AutoOverwrite
	{
		get
		{
			return __22B8.AutoOverwrite;
		}
		set
		{
			__22B8.AutoOverwrite = value;
		}
	}

	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	[DefaultValue(null)]
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
	ICommand ICommandSource.ᒍ
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
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

	[Localizable(true)]
	[TypeConverter(typeof(StringConverter))]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
		}
	}

	[Description("")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Free-Text")]
	public Keys FreeTextEntryToggleKey
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return __22C4;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			__22C4 = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether free text entries are attempted to be auto-resolved to dates like Today to today's date or Now to date and time now etc.")]
	[Category("Free-Text")]
	public bool AutoResolveFreeTextEntries
	{
		get
		{
			return __17FD;
		}
		set
		{
			__17FD = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
	[Category("Free-Text")]
	public bool AutoOffFreeTextEntry
	{
		get
		{
			return __17FE;
		}
		set
		{
			__17FE = value;
		}
	}

	[Category("Free-Text")]
	[DefaultValue(false)]
	[Description("Indicates whether control input is in free-text input mode.")]
	public bool FreeTextEntryMode
	{
		get
		{
			return __17FF;
		}
		set
		{
			__17FF = value;
			_᠒();
		}
	}

	protected override bool SupportsFreeTextEntry => true;

	protected override bool IsFreeTextEntryVisible
	{
		get
		{
			if (__17FF)
			{
				return base.IsKeyboardFocusWithin;
			}
			return false;
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ValueObjectChanged
	{
		add
		{
			EventHandler eventHandler = _៤;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៤, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _៤;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៤, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event CancelEventHandler ButtonClearClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _៦;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៦, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _៦;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៦, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public event CancelEventHandler ButtonDropDownClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _៧;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៧, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _៧;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៧, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public event EventHandler LockUpdateChanged
	{
		add
		{
			EventHandler eventHandler = _៩;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៩, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _៩;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៩, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Free-Text")]
	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value control expects.")]
	public event FreeTextEntryConversionEventHandler ConvertFreeTextEntry
	{
		add
		{
			FreeTextEntryConversionEventHandler freeTextEntryConversionEventHandler = _17FB;
			FreeTextEntryConversionEventHandler freeTextEntryConversionEventHandler2;
			do
			{
				freeTextEntryConversionEventHandler2 = freeTextEntryConversionEventHandler;
				FreeTextEntryConversionEventHandler value2 = (FreeTextEntryConversionEventHandler)Delegate.Combine(freeTextEntryConversionEventHandler2, value);
				freeTextEntryConversionEventHandler = Interlocked.CompareExchange(ref _17FB, value2, freeTextEntryConversionEventHandler2);
			}
			while ((object)freeTextEntryConversionEventHandler != freeTextEntryConversionEventHandler2);
		}
		remove
		{
			FreeTextEntryConversionEventHandler freeTextEntryConversionEventHandler = _17FB;
			FreeTextEntryConversionEventHandler freeTextEntryConversionEventHandler2;
			do
			{
				freeTextEntryConversionEventHandler2 = freeTextEntryConversionEventHandler;
				FreeTextEntryConversionEventHandler value2 = (FreeTextEntryConversionEventHandler)Delegate.Remove(freeTextEntryConversionEventHandler2, value);
				freeTextEntryConversionEventHandler = Interlocked.CompareExchange(ref _17FB, value2, freeTextEntryConversionEventHandler2);
			}
			while ((object)freeTextEntryConversionEventHandler != freeTextEntryConversionEventHandler2);
		}
	}

	public event CancelEventHandler ButtonFreeTextClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _17FC;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _17FC, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _17FC;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _17FC, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public NumericInputBase()
	{
		__17F2 = new InputButtonSettings(this);
		__17F1 = new InputButtonSettings(this);
		__17F5 = new InputButtonSettings(this);
		__22BC = new InputButtonSettings(this);
		__22B8 = RootVisualItem as VisualInputGroup;
		__22B8.AutoOverwrite = false;
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
		LockUpdateCheckBox lockUpdateCheckBox = _17DE;
		if (lockUpdateCheckBox != null)
		{
			__22B8.Items.Remove(lockUpdateCheckBox);
			lockUpdateCheckBox.CheckedChanged -= _180A;
		}
		if (ShowCheckBox && !(__22B8.Items[0] is LockUpdateCheckBox))
		{
			lockUpdateCheckBox = new LockUpdateCheckBox();
			lockUpdateCheckBox.ᡇ = _1885._1886;
			lockUpdateCheckBox.CheckedChanged += _180A;
			__22B8.Items.Insert(0, lockUpdateCheckBox);
			lockUpdateCheckBox.Checked = __17F0;
		}
	}

	private void _180A(object _٠, EventArgs ؿ)
	{
		LockUpdateCheckBox lockUpdateCheckBox = _17DE;
		if (lockUpdateCheckBox != null)
		{
			__17F0 = lockUpdateCheckBox.Checked;
		}
		OnLockUpdateChanged(ؿ);
	}

	protected virtual void OnLockUpdateChanged(EventArgs e)
	{
		if (_៩ != null)
		{
			_៩(this, e);
		}
		if (__1800 != null)
		{
			((TextBoxBase)__1800).ReadOnly = !LockUpdateChecked;
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
		for (int num = __22B8.Items.Count - 1; num >= 0; num--)
		{
			if (__22B8.Items[num] is VisualUpDownButton)
			{
				_ = (VisualUpDownButton)__22B8.Items[num];
				__22B8.Items.RemoveAt(num);
				break;
			}
		}
		if (ShowUpDown)
		{
			VisualUpDownButton visualUpDownButton = new VisualUpDownButton();
			visualUpDownButton.Alignment = eItemAlignment.Right;
			visualUpDownButton.AutoChange = eUpDownButtonAutoChange.FocusedItem;
			visualUpDownButton.ᡇ = _1885._1886;
			visualUpDownButton.Enabled = LockUpdateChecked;
			__22B8.Items.Add(visualUpDownButton);
		}
	}

	public abstract void DecreaseValue();

	public abstract void IncreaseValue();

	protected override VisualItem CreateButton(InputButtonSettings buttonSettings)
	{
		VisualItem visualItem = null;
		if (buttonSettings == __17F1)
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
		if (buttonSettings == __17F2)
		{
			if (buttonSettings.Image == null && string.IsNullOrEmpty(buttonSettings.Text))
			{
				visualButton.Symbol = "\uf00d";
			}
		}
		else if (buttonSettings == __17F5)
		{
			if (buttonSettings.Image == null && string.IsNullOrEmpty(buttonSettings.Text))
			{
				visualButton.Symbol = "\uf040";
			}
			visualButton.Checked = buttonSettings.Checked;
		}
		else if (buttonSettings == __22BC)
		{
			if (buttonSettings.Image == null && string.IsNullOrEmpty(buttonSettings.Text))
			{
				visualButton.Symbol = "\uf1ec";
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
		if (__17F2.Visible)
		{
			VisualItem visualItem = CreateButton(__17F2);
			if (__17F2.ItemReference != null)
			{
				__17F2.ItemReference.Click -= _180F;
			}
			__17F2.ItemReference = visualItem;
			visualItem.Click += _180F;
			sortedList.Add(__17F2, visualItem);
		}
		if (__17F1.Visible)
		{
			VisualItem visualItem2 = CreateButton(__17F1);
			if (__17F1.ItemReference != null)
			{
				__17F1.ItemReference.MouseDown -= new MouseEventHandler(_180C);
			}
			__17F1.ItemReference = visualItem2;
			visualItem2.MouseDown += new MouseEventHandler(_180C);
			sortedList.Add(__17F1, visualItem2);
		}
		if (__17F5.Visible)
		{
			VisualItem visualItem3 = CreateButton(__17F5);
			if (__17F5.ItemReference != null)
			{
				__17F5.ItemReference.Click -= _᠑;
			}
			__17F5.ItemReference = visualItem3;
			visualItem3.Click += _᠑;
			sortedList.Add(__17F5, visualItem3);
		}
		if (__22BC.Visible)
		{
			VisualItem visualItem4 = CreateButton(__22BC);
			if (__22BC.ItemReference != null)
			{
				__22BC.ItemReference.Click -= _22C5;
			}
			__22BC.ItemReference = visualItem4;
			visualItem4.Click += _22C5;
			sortedList.Add(__22BC, visualItem4);
		}
		return sortedList;
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (__22BF && __22BE != null)
		{
			__22BE.ᡡ(e);
		}
		base.OnKeyPress(e);
	}

	private void _22C5(object _٠, EventArgs ؿ)
	{
		if (__17F4 != DateTime.MinValue && DateTime.Now.Subtract(__17F4).TotalMilliseconds < 150.0)
		{
			__17F4 = DateTime.MinValue;
			return;
		}
		if (__22BE == null)
		{
			__22BE = new Calculator();
			((Control)__22BE).AutoSize = true;
			__22BE._22E8 = false;
			__22BE.DisplayVisible = false;
			__22BE.ValueChanged += DropDownCalculatorValueChanged;
			((Control)__22BE).Scale(Dpi.Factor);
			OnDropDownCalculatorCreated(__22BE);
		}
		UpdateCalculatorValue(__22BE);
		__22BF = true;
		_22C6((Control)(object)__22BE);
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

	private void _180C(object _٠, MouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)ؿ.Button != 1048576 || (__17F4 != DateTime.MinValue && DateTime.Now.Subtract(__17F4).TotalMilliseconds < 150.0))
		{
			__17F4 = DateTime.MinValue;
		}
		else
		{
			ShowDropDown();
		}
	}

	public void ShowDropDown()
	{
		_22C6(__22C3);
	}

	private void _22C6(Control _22C7)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (_22C7 == null && _០.SubItems.Count == 0)
		{
			return;
		}
		if (_០.Expanded)
		{
			_០.Expanded = false;
			return;
		}
		ControlContainerItem controlContainerItem = null;
		ItemContainer itemContainer = null;
		if (_22C7 != null)
		{
			itemContainer = new ItemContainer();
			itemContainer.Name = __22BB;
			controlContainerItem = new ControlContainerItem(__22BA);
			itemContainer.SubItems.Add(controlContainerItem);
			_០.SubItems.Insert(0, itemContainer);
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonDropDownClick(cancelEventArgs);
		if (cancelEventArgs.Cancel || _០.SubItems.Count == 0)
		{
			if (itemContainer != null)
			{
				_០.SubItems.Remove(itemContainer);
			}
			return;
		}
		__22C0 = _22C7.Parent;
		controlContainerItem.Control = _22C7;
		_០.SetDisplayRectangle(((Control)this).ClientRectangle);
		if ((int)((Control)this).RightToLeft == 0)
		{
			Point point = new Point(((Control)this).Width - _០.PopupSize.Width, ((Control)this).Height);
			ঊ ঊ = BarFunctions.ॻ((Control)(object)this);
			Point point2 = ((Control)this).PointToScreen(point);
			if (ঊ != null && ঊ.ঌ.X > point2.X)
			{
				point.X = 0;
			}
			_០.PopupLocation = point;
		}
		_០.Expanded = !_០.Expanded;
	}

	public void CloseDropDown()
	{
		if (_០.Expanded)
		{
			_០.Expanded = false;
		}
	}

	private void _180E(object _٠, EventArgs ؿ)
	{
		__17F4 = DateTime.Now;
		if (_០.SubItems[__22BB] is ItemContainer itemContainer)
		{
			if (itemContainer.SubItems[__22BA] is ControlContainerItem { Control: var control } controlContainerItem)
			{
				controlContainerItem.Control = null;
				itemContainer.SubItems.Remove(controlContainerItem);
				if (control != null)
				{
					control.Parent = __22C0;
					__22C0 = null;
				}
			}
			_០.SubItems.Remove(itemContainer);
		}
		__22BF = false;
	}

	private void _180F(object _٠, EventArgs ؿ)
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
		if (_៦ != null)
		{
			_៦(this, e);
		}
	}

	protected virtual void OnButtonDropDownClick(CancelEventArgs e)
	{
		if (_៧ != null)
		{
			_៧(this, e);
		}
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		if (__22C2 == eTriggerType.Immediate)
		{
			OnValueChangedDirect(e);
		}
		else
		{
			__22C1 = true;
		}
	}

	protected virtual void OnValueChangedDirect(EventArgs e)
	{
		if (!IsInitializing)
		{
			if (ᗲ != null)
			{
				ᗲ(this, e);
			}
			OnValueObjectChanged(e);
			ExecuteCommand();
			__22C1 = false;
		}
	}

	protected virtual void OnValueObjectChanged(EventArgs e)
	{
		if (_៤ != null)
		{
			_៤(this, e);
		}
	}

	protected override PopupItem CreatePopupItem()
	{
		ButtonItem buttonItem = new ButtonItem("sysPopupProvider");
		buttonItem.PopupFinalized += _180E;
		_០ = buttonItem;
		return buttonItem;
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null)
		{
			CommandManager._૭(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if ((int)__22C4 != 0 && keyData == __22C4)
		{
			FreeTextEntryMode = !FreeTextEntryMode;
			if (FreeTextEntryMode && __1800 != null)
			{
				((Control)__1800).Focus();
			}
			return true;
		}
		if ((int)keyData == 13 && __22C2 == eTriggerType.LostFocusAndEnterKey && __22C1)
		{
			OnValueChangedDirect(EventArgs.Empty);
			return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void _᠑(object _٠, EventArgs ؿ)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonFreeTextClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			FreeTextEntryMode = !FreeTextEntryMode;
			__17F5.Checked = FreeTextEntryMode;
			if (FreeTextEntryMode && __1800 != null && !((Control)__1800).Focused)
			{
				((Control)__1800).Focus();
			}
		}
	}

	protected virtual void OnButtonFreeTextClick(CancelEventArgs e)
	{
		_17FC?.Invoke(this, e);
	}

	private void _᠒()
	{
		if (!__17FF)
		{
			if (__1800 != null)
			{
				__1800._232E -= _᠗;
				__1800._232D -= _᠔;
				((Control)__1800).LostFocus -= _᠘;
				((Control)this).Controls.Remove((Control)(object)__1800);
				((Component)(object)__1800).Dispose();
				__1800 = null;
			}
		}
		else
		{
			_᠓();
		}
		if (__17F5 != null)
		{
			__17F5.Checked = __17FF;
		}
	}

	protected override void OnIsKeyboardFocusWithinChanged()
	{
		if (__17FF)
		{
			_᠓();
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
		if (__22C1 && !base.IsKeyboardFocusWithin)
		{
			OnValueChangedDirect(EventArgs.Empty);
		}
		base.OnIsKeyboardFocusWithinChanged();
	}

	private void _᠓()
	{
		_232C _232C2 = (_232C)(object)GetFreeTextBox();
		if (base.IsKeyboardFocusWithin)
		{
			((Control)_232C2).Visible = true;
			((Control)_232C2).Text = ((Control)this).Text;
			RootVisualItem.InvalidateArrange();
			((Control)this).Invalidate();
		}
		else if (((Control)_232C2).Visible)
		{
			if (((Control)_232C2).Focused)
			{
				_232C2._2334();
			}
			else
			{
				((Control)_232C2).Visible = false;
			}
			RootVisualItem.InvalidateArrange();
			((Control)this).Invalidate();
		}
	}

	protected override Control GetFreeTextBox()
	{
		if (__1800 == null)
		{
			__1800 = new _232C();
			__1800._232E += _᠗;
			__1800._232D += _᠔;
			((Control)__1800).LostFocus += _᠘;
			__1800._232F += _22C9;
			__1800._2330 += _22C8;
			((TextBoxBase)__1800).BorderStyle = (BorderStyle)0;
			((TextBoxBase)__1800).ReadOnly = !LockUpdateChecked || IsInputReadOnly;
			if (InputHorizontalAlignment == eHorizontalAlignment.Right)
			{
				((TextBox)__1800).TextAlign = (HorizontalAlignment)1;
			}
			else if (InputHorizontalAlignment == eHorizontalAlignment.Center)
			{
				((TextBox)__1800).TextAlign = (HorizontalAlignment)2;
			}
			else if (InputHorizontalAlignment == eHorizontalAlignment.Left)
			{
				((TextBox)__1800).TextAlign = (HorizontalAlignment)0;
			}
			((Control)this).Controls.Add((Control)(object)__1800);
		}
		return (Control)(object)__1800;
	}

	private void _22C8(object _٠, EventArgs ؿ)
	{
		ApplyFreeTextValue();
		DecreaseValue();
	}

	private void _22C9(object _٠, EventArgs ؿ)
	{
		ApplyFreeTextValue();
		IncreaseValue();
	}

	private void _᠔(object _٠, EventArgs ؿ)
	{
		if (__1800 != null)
		{
			((Control)__1800).Text = ((Control)this).Text;
		}
	}

	protected virtual void OnConvertFreeTextEntry(FreeTextEntryConversionEventArgs e)
	{
		_17FB?.Invoke(this, e);
	}

	private void _᠗(object _٠, EventArgs ؿ)
	{
		ApplyFreeTextValue();
	}

	protected virtual void ApplyFreeTextValue()
	{
	}

	private void _᠘(object _٠, EventArgs ؿ)
	{
		if (__17FE && !base.IsKeyboardFocusWithin)
		{
			FreeTextEntryMode = false;
		}
	}

	protected override void HideFreeTextBoxEntry()
	{
		if (__1800 != null)
		{
			((Control)__1800).Visible = false;
		}
	}
}
