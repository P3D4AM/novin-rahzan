using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

[ToolboxBitmap(typeof(DotNetBarManager), "IpAddressInput.ico")]
[Designer("DevComponents.DotNetBar.Design.IpAddressInputDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
public class IpAddressInput : VisualControlBase, ICommandSource
{
	private CancelEventHandler _៦;

	private CancelEventHandler _៧;

	private EventHandler _៩;

	private EventHandler ᗲ;

	private IpAddressGroup __2B3A;

	private InputButtonSettings __17F1;

	private InputButtonSettings __17F2;

	private InputButtonSettings __17F5;

	private Control __22C3;

	private ButtonItem _០;

	private Control __22C0;

	private static string __22BA = "sysPopupControlContainer";

	private static string __22BB = "sysPopupItemContainer";

	private DateTime __17F4 = DateTime.MinValue;

	private bool __17EB;

	private bool __17F0;

	private string _ᗸ;

	private FreeTextEntryConversionEventHandler _17FB;

	private CancelEventHandler _17FC;

	private bool __17FD = true;

	private bool __17FE;

	private bool __17FF;

	private _232C __1800;

	private ICommand _૦;

	private object _૧;

	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => __17F1;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[Category("Buttons")]
	public InputButtonSettings ButtonClear => __17F2;

	[Category("Buttons")]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonFreeText => __17F5;

	[DefaultValue(false)]
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	public bool AutoOverwrite
	{
		get
		{
			return __2B3A.AutoOverwrite;
		}
		set
		{
			__2B3A.AutoOverwrite = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	public bool AllowEmptyState
	{
		get
		{
			return __2B3A.AllowEmptyState;
		}
		set
		{
			__2B3A.AllowEmptyState = value;
			((Control)this).Invalidate();
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

	[Description("Indicates whether input part of the control is read-only.")]
	[DefaultValue(false)]
	public bool IsInputReadOnly
	{
		get
		{
			return __2B3A.IsReadOnly;
		}
		set
		{
			__2B3A.IsReadOnly = value;
			if (__1800 != null)
			{
				((TextBoxBase)__1800).ReadOnly = value;
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
			if (__2B3A.Items[0] is LockUpdateCheckBox)
			{
				return (LockUpdateCheckBox)__2B3A.Items[0];
			}
			return null;
		}
	}

	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	[DefaultValue(false)]
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

	[DefaultValue(".")]
	[Category("Behavior")]
	[Description("List of characters that when pressed would select next input field.")]
	public string SelectNextInputCharacters
	{
		get
		{
			return __2B3A.SelectNextInputCharacters;
		}
		set
		{
			if (__2B3A.SelectNextInputCharacters != value)
			{
				__2B3A.SelectNextInputCharacters = value;
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
			return __2B3A.Value;
		}
		set
		{
			_ᗸ = value;
			_2B3B();
		}
	}

	protected override bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused && (__1800 == null || !((Control)__1800).Focused))
			{
				return __2B3A.IsEmpty;
			}
			return false;
		}
	}

	[Description("Indicates whether free text entries are attempted to be auto-resolved to IP address as host/domain names.")]
	[DefaultValue(true)]
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

	[DefaultValue(false)]
	[Description("Indicates whether control input is in free-text input mode.")]
	[Category("Free-Text")]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Localizable(true)]
	[TypeConverter(typeof(StringConverter))]
	[Category("Commands")]
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

	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value control expects.")]
	[Category("Free-Text")]
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

	public IpAddressInput()
	{
		__17F2 = new InputButtonSettings(this);
		__17F1 = new InputButtonSettings(this);
		__17F5 = new InputButtonSettings(this);
		__2B3A = RootVisualItem as IpAddressGroup;
	}

	protected override VisualItem CreateRootVisual()
	{
		IpAddressGroup ipAddressGroup = new IpAddressGroup();
		ipAddressGroup.SelectNextInputCharacters = ".";
		ipAddressGroup.ValueChanged += _22B9;
		return ipAddressGroup;
	}

	private void _22B9(object _٠, EventArgs ؿ)
	{
		if (_ᗸ != __2B3A.Value)
		{
			Value = __2B3A.Value;
		}
	}

	public virtual void Copy()
	{
		if (__2B3A != null)
		{
			Clipboard.SetText(((Control)this).Text);
		}
	}

	public virtual void Paste()
	{
		if (__2B3A != null)
		{
			((Control)this).Text = Clipboard.GetText();
		}
	}

	public virtual void Cut()
	{
		if (__2B3A != null)
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
		return visualItem;
	}

	protected override PopupItem CreatePopupItem()
	{
		ButtonItem buttonItem = new ButtonItem("sysPopupProvider");
		buttonItem.PopupFinalized += _180E;
		_០ = buttonItem;
		return buttonItem;
	}

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
		return sortedList;
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
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if (__22C3 == null && _០.SubItems.Count == 0)
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
		if (__22C3 != null)
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
		__22C0 = __22C3.Parent;
		controlContainerItem.Control = __22C3;
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
		if (!(_០.SubItems[__22BB] is ItemContainer itemContainer))
		{
			return;
		}
		if (itemContainer.SubItems[__22BA] is ControlContainerItem controlContainerItem)
		{
			controlContainerItem.Control = null;
			itemContainer.SubItems.Remove(controlContainerItem);
			if (__22C3 != null)
			{
				__22C3.Parent = __22C0;
				__22C0 = null;
			}
		}
		_០.SubItems.Remove(itemContainer);
	}

	private void _180F(object _٠, EventArgs ؿ)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonClearClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			__2B3A.Value = null;
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

	protected virtual void OnShowCheckBoxChanged()
	{
		LockUpdateCheckBox lockUpdateCheckBox = _17DE;
		if (lockUpdateCheckBox != null)
		{
			__2B3A.Items.Remove(lockUpdateCheckBox);
			lockUpdateCheckBox.CheckedChanged -= _180A;
		}
		if (ShowCheckBox && !(__2B3A.Items[0] is LockUpdateCheckBox))
		{
			lockUpdateCheckBox = new LockUpdateCheckBox();
			lockUpdateCheckBox.CheckedChanged += _180A;
			__2B3A.Items.Insert(0, lockUpdateCheckBox);
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
	}

	private void _2B3B()
	{
		if (_ᗸ != __2B3A.Value)
		{
			__2B3A.Value = _ᗸ;
			ExecuteCommand();
		}
		((Control)this).Text = _ᗸ;
		if (FreeTextEntryMode && __1800 != null)
		{
			((Control)__1800).Text = _ᗸ;
		}
		OnValueChanged(EventArgs.Empty);
	}

	protected virtual void OnValueChanged(EventArgs eventArgs)
	{
		ᗲ?.Invoke(this, eventArgs);
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
			}
		}
		base.OnIsKeyboardFocusWithinChanged();
	}

	private void _᠓()
	{
		_232C _232C2 = (_232C)(object)GetFreeTextBox();
		if (base.IsKeyboardFocusWithin)
		{
			((Control)_232C2).Visible = true;
			((Control)_232C2).Text = Value;
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
			((TextBoxBase)__1800).BorderStyle = (BorderStyle)0;
			((TextBoxBase)__1800).ReadOnly = IsInputReadOnly;
			((Control)this).Controls.Add((Control)(object)__1800);
		}
		return (Control)(object)__1800;
	}

	private void _᠔(object _٠, EventArgs ؿ)
	{
		if (__1800 != null)
		{
			((Control)__1800).Text = Value;
		}
	}

	protected virtual void OnConvertFreeTextEntry(FreeTextEntryConversionEventArgs e)
	{
		_17FB?.Invoke(this, e);
	}

	private void _᠗(object _٠, EventArgs ؿ)
	{
		if (__1800 == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(((Control)__1800).Text))
		{
			Value = null;
			return;
		}
		if (__2B3A._2B3D(((Control)__1800).Text) && __17FD)
		{
			Value = ((Control)__1800).Text;
			return;
		}
		FreeTextEntryConversionEventArgs freeTextEntryConversionEventArgs = new FreeTextEntryConversionEventArgs(((Control)__1800).Text);
		OnConvertFreeTextEntry(freeTextEntryConversionEventArgs);
		if (freeTextEntryConversionEventArgs.IsValueConverted)
		{
			if (freeTextEntryConversionEventArgs.ControlValue is string && __2B3A._2B3D((string)freeTextEntryConversionEventArgs.ControlValue))
			{
				Value = (string)freeTextEntryConversionEventArgs.ControlValue;
				return;
			}
			throw new ArgumentException("ControlValue assigned is not recognized as valid IP value.");
		}
		string text = null;
		if (__17FD)
		{
			try
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(((Control)__1800).Text);
				if (hostAddresses != null && hostAddresses.Length > 0)
				{
					text = hostAddresses[0].ToString();
					if (!__2B3A._2B3D(text))
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
}
