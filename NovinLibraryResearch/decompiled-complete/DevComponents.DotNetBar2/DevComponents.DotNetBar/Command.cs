using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Ribbon;

namespace DevComponents.DotNetBar;

[DefaultEvent("Executed")]
[ToolboxBitmap(typeof(Command), "Command.ico")]
[DesignTimeVisible(true)]
[ToolboxItem(true)]
public class Command : Component, ICommand
{
	private EventHandler ᚃ;

	private CancelEventHandler ᚄ;

	private string _ኚ;

	private bool _ᚅ;

	private bool _ᚆ;

	private bool _ᚇ;

	private bool _ᚈ;

	private bool _ᚉ;

	private bool _ᚊ;

	private Image _ነ;

	private bool _ᚋ;

	private Image _ᚌ;

	private bool _ᚍ;

	private bool _Ԋ = true;

	private bool _ᚎ;

	private string _ᚏ = "";

	[Description("Indicates Text that is assigned to all command sources that are using this command and have Text property.")]
	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string Text
	{
		get
		{
			return _ኚ;
		}
		set
		{
			_ኚ = value;
			_ᚅ = true;
			OnTextChanged();
		}
	}

	[Description("Indicates value for the Checked property that is assigned to the command subscribers using this command and have Checked property.")]
	public bool Checked
	{
		get
		{
			return _ᚆ;
		}
		set
		{
			_ᚆ = value;
			_ᚇ = true;
			OnCheckedChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsSyncingCommand
	{
		get
		{
			return _ᚈ;
		}
		private set
		{
			_ᚈ = value;
		}
	}

	[Description("Indicates value for the Visible property that is assigned to the command subscribers using this command and have Visible property.")]
	public bool Visible
	{
		get
		{
			return _ᚉ;
		}
		set
		{
			_ᚉ = value;
			_ᚊ = true;
			OnVisibleChanged();
		}
	}

	[Localizable(true)]
	[Description("Indicates image that is assigned to the command subscribers using this command and have Image property.")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			_ነ = value;
			_ᚋ = true;
			OnImageChanged();
		}
	}

	[Localizable(true)]
	[Description("Indicates small image that is assigned to the command subscribers using this command and have ImageSmall property.")]
	public Image ImageSmall
	{
		get
		{
			return _ᚌ;
		}
		set
		{
			_ᚌ = value;
			_ᚍ = true;
			OnImageSmallChanged();
		}
	}

	[Description("Indicates value for Enabled property assigned to the command subscribers using this command and have Enabled property.")]
	public bool Enabled
	{
		get
		{
			return _Ԋ;
		}
		set
		{
			_Ԋ = value;
			_ᚎ = true;
			OnEnabledChanged();
		}
	}

	[Category("Design")]
	[Description("Indicates the name used to identify node.")]
	[Browsable(false)]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				_ᚏ = Site.Name;
			}
			return _ᚏ;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				_ᚏ = "";
			}
			else
			{
				_ᚏ = value;
			}
		}
	}

	public event EventHandler Executed
	{
		add
		{
			EventHandler eventHandler = ᚃ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᚃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᚃ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᚃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event CancelEventHandler PreviewExecuted
	{
		add
		{
			CancelEventHandler cancelEventHandler = ᚄ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref ᚄ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = ᚄ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref ᚄ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public Command(IContainer container)
		: this()
	{
		container.Add(this);
	}

	public Command(IContainer container, EventHandler commandExecutedEventHandler)
		: this()
	{
		container.Add(this);
		Executed += commandExecutedEventHandler;
	}

	public Command(EventHandler commandExecutedEventHandler)
		: this()
	{
		Executed += commandExecutedEventHandler;
	}

	public Command()
	{
	}

	public virtual void Execute()
	{
		Execute(null);
	}

	public virtual void Execute(ICommandSource commandSource)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnPreviewExecuted(commandSource, cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			OnExecuted(commandSource, new EventArgs());
		}
	}

	protected virtual void OnExecuted(ICommandSource commandSource, EventArgs e)
	{
		ᚃ?.Invoke(commandSource, e);
	}

	protected virtual void OnPreviewExecuted(ICommandSource commandSource, CancelEventArgs e)
	{
		ᚄ?.Invoke(commandSource, e);
	}

	protected virtual void OnTextChanged()
	{
		SetTextProperty();
	}

	protected virtual void SetTextProperty()
	{
		IsSyncingCommand = true;
		try
		{
			ArrayList subscribers = GetSubscribers();
			ArrayList arrayList = new ArrayList(subscribers.Count);
			string ኚ = _ኚ;
			if (GetDesignMode())
			{
				foreach (object item in subscribers)
				{
					if (IsTextPropertyChanged(item, ኚ))
					{
						SetPropertyValue(item, "Text", ኚ);
					}
					else
					{
						arrayList.Add(item);
					}
				}
			}
			else
			{
				foreach (object item2 in subscribers)
				{
					if (IsTextPropertyChanged(item2, ኚ))
					{
						SetTextProperty(item2, ኚ);
					}
					else
					{
						arrayList.Add(item2);
					}
				}
			}
			foreach (object item3 in arrayList)
			{
				subscribers.Remove(item3);
			}
			RecalcLayout(subscribers);
		}
		finally
		{
			IsSyncingCommand = false;
		}
	}

	private bool IsTextPropertyChanged(object item, string text)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		if (item is ComboBoxItem)
		{
			ComboBoxItem comboBoxItem = item as ComboBoxItem;
			if ((int)comboBoxItem.DropDownStyle == 2)
			{
				if (comboBoxItem.SelectedItem is ComboBoxItem)
				{
					return ((ComboBoxItem)comboBoxItem.SelectedItem).Text != text;
				}
				if (comboBoxItem.SelectedItem != null)
				{
					return comboBoxItem.SelectedItem.ToString() != text;
				}
				return ((Control)comboBoxItem.ComboBoxEx).Text != text;
			}
			return comboBoxItem.Text != text;
		}
		if (item is BaseItem)
		{
			return ((BaseItem)item).Text != text;
		}
		if (item is TabItem)
		{
			return ((TabItem)item).Text != text;
		}
		if (item is Control)
		{
			return ((Control)item).Text != text;
		}
		return GetPropertyValue(item, "Text") != text;
	}

	protected virtual void SetTextProperty(object item, string text)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (item is ComboBoxItem)
		{
			ComboBoxItem comboBoxItem = item as ComboBoxItem;
			if ((int)comboBoxItem.DropDownStyle == 2)
			{
				if (((Control)comboBoxItem.ComboBoxEx).Text != text)
				{
					if (text == "" || text == null)
					{
						comboBoxItem.SelectedIndex = -1;
					}
					else
					{
						comboBoxItem.SelectedIndex = ((ComboBox)comboBoxItem.ComboBoxEx).FindString(text);
					}
				}
			}
			else
			{
				comboBoxItem.Text = text;
			}
		}
		else if (item is BaseItem)
		{
			((BaseItem)item).Text = text;
		}
		else if (item is TabItem)
		{
			((TabItem)item).Text = text;
		}
		else if (item is Control)
		{
			((Control)item).Text = text;
		}
		else
		{
			SetPropertyValue(item, "Text", text);
		}
	}

	private void RecalcLayout(ArrayList list)
	{
		if (!CommandManager.AutoUpdateLayout)
		{
			return;
		}
		ArrayList arrayList = new ArrayList(list.Count);
		foreach (object item in list)
		{
			if (item is BaseItem)
			{
				object containerControl = ((BaseItem)item).ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (BarFunctions.IsHandleValid(val) && !arrayList.Contains(val))
				{
					InvokeRecalcLayout(val);
					arrayList.Add(val);
				}
			}
		}
	}

	private void InvokeRecalcLayout(Control control)
	{
		if (control is Bar)
		{
			((Bar)(object)control).RecalcLayout();
		}
		else if (control is RibbonBar && ((Control)(RibbonBar)(object)control).Parent is RibbonPanel && !((RibbonPanel)(object)((Control)(RibbonBar)(object)control).Parent).DefaultLayout)
		{
			((RibbonBar)(object)control).RecalcLayout();
			((Control)(RibbonPanel)(object)((Control)(RibbonBar)(object)control).Parent).PerformLayout();
		}
		else if (control is ItemControl)
		{
			((ItemControl)(object)control).RecalcLayout();
		}
		else if (control is BaseItemControl)
		{
			((BaseItemControl)(object)control).RecalcLayout();
		}
		else if (control is MenuPanel)
		{
			((MenuPanel)(object)control).RecalcLayout();
		}
		else if (control is ExplorerBar)
		{
			((ExplorerBar)(object)control).RecalcLayout();
		}
		else if (control is SideBar)
		{
			((SideBar)(object)control).RecalcLayout();
		}
	}

	private bool GetDesignMode()
	{
		if (Site != null)
		{
			return Site.DesignMode;
		}
		return false;
	}

	protected virtual void SetPropertyValue(object item, string propertyName, object value)
	{
		if (CommandManager.UseReflection)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
			properties.Find(propertyName, ignoreCase: false)?.SetValue(item, value);
		}
	}

	protected virtual object GetPropertyValue(object item, string propertyName)
	{
		if (!CommandManager.UseReflection)
		{
			return null;
		}
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
		return properties.Find(propertyName, ignoreCase: false)?.GetValue(item);
	}

	public void SetValue(string propertyName, object value)
	{
		IsSyncingCommand = true;
		try
		{
			ArrayList subscribers = GetSubscribers();
			Type type = null;
			if (value != null)
			{
				type = value.GetType();
			}
			ArrayList arrayList = new ArrayList(subscribers.Count);
			foreach (object item in subscribers)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
				PropertyDescriptor propertyDescriptor = properties.Find(propertyName, ignoreCase: false);
				if (propertyDescriptor != null && ((object)type == null || (object)propertyDescriptor.PropertyType == type))
				{
					propertyDescriptor.SetValue(item, value);
					arrayList.Add(item);
				}
			}
			RecalcLayout(arrayList);
		}
		finally
		{
			IsSyncingCommand = false;
		}
	}

	private ArrayList GetSubscribers()
	{
		return CommandManager.GetSubscribers(this);
	}

	public bool ShouldSerializeText()
	{
		return _ᚅ;
	}

	public void ResetText()
	{
		_ᚅ = false;
		_ኚ = null;
	}

	protected virtual void OnCheckedChanged()
	{
		SetCheckedProperty();
	}

	protected virtual void SetCheckedProperty()
	{
		IsSyncingCommand = true;
		try
		{
			ArrayList subscribers = GetSubscribers();
			bool ᚆ = _ᚆ;
			foreach (object item in subscribers)
			{
				SetCheckedProperty(item, ᚆ);
			}
		}
		finally
		{
			IsSyncingCommand = false;
		}
	}

	protected virtual void SetCheckedProperty(object item, bool check)
	{
		if (item is ButtonItem)
		{
			((ButtonItem)item).Checked = check;
		}
		else if (item is ButtonX)
		{
			((ButtonX)item).Checked = check;
		}
		else if (item is CheckBoxItem)
		{
			((CheckBoxItem)item).Checked = check;
		}
		else if (item is SwitchButtonItem)
		{
			((SwitchButtonItem)item).Value = check;
		}
		else
		{
			SetPropertyValue(item, "Checked", check);
		}
	}

	public bool ShouldSerializeChecked()
	{
		return _ᚇ;
	}

	public void ResetChecked()
	{
		_ᚇ = false;
		_ᚆ = false;
	}

	protected virtual void OnVisibleChanged()
	{
		SetVisibleProperty();
	}

	protected virtual void SetVisibleProperty()
	{
		ArrayList subscribers = GetSubscribers();
		bool ᚉ = _ᚉ;
		foreach (object item in subscribers)
		{
			SetVisibleProperty(item, ᚉ);
		}
	}

	protected virtual void SetVisibleProperty(object item, bool visible)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		ArrayList subscribers = GetSubscribers();
		if (base.DesignMode)
		{
			SetPropertyValue(item, "Visible", visible);
		}
		else if (item is BaseItem)
		{
			((BaseItem)item).Visible = visible;
		}
		else if (item is Control)
		{
			((Control)item).Visible = visible;
		}
		else
		{
			SetPropertyValue(item, "Visible", visible);
		}
		RecalcLayout(subscribers);
	}

	public bool ShouldSerializeVisible()
	{
		return _ᚊ;
	}

	public void ResetVisible()
	{
		_ᚊ = false;
		_ᚉ = false;
	}

	protected virtual void OnImageChanged()
	{
		SetImageProperty();
	}

	protected virtual void SetImageProperty()
	{
		IsSyncingCommand = true;
		try
		{
			ArrayList subscribers = GetSubscribers();
			Image ነ = _ነ;
			if (GetDesignMode())
			{
				foreach (object item in subscribers)
				{
					if (item is ButtonItem)
					{
						ButtonItem buttonItem = item as ButtonItem;
						bool flag = false;
						if (buttonItem.ContainerControl is RibbonStrip)
						{
							RibbonStrip ribbonStrip = buttonItem.ContainerControl as RibbonStrip;
							if (((Control)ribbonStrip).Parent is RibbonControl && ((RibbonControl)(object)((Control)ribbonStrip).Parent).QuickToolbarItems.Contains(buttonItem))
							{
								flag = true;
							}
						}
						else if (buttonItem.ContainerControl is Ꮮ)
						{
							flag = true;
						}
						if (flag && ነ != null && (ነ.Width > 16 || ነ.Height > 16))
						{
							buttonItem.UseSmallImage = true;
							if (buttonItem.ImageSmall == null)
							{
								TypeDescriptor.GetProperties(buttonItem)["ImageFixedSize"].SetValue(buttonItem, new Size(16, 16));
							}
						}
					}
					SetPropertyValue(item, "Image", ነ);
				}
			}
			else
			{
				foreach (object item2 in subscribers)
				{
					SetImageProperty(item2, ነ);
				}
			}
			RecalcLayout(subscribers);
		}
		finally
		{
			IsSyncingCommand = false;
		}
	}

	protected virtual void SetImageProperty(object item, Image image)
	{
		if (item is ButtonItem)
		{
			((ButtonItem)item).Image = image;
		}
		else if (item is ExplorerBarGroupItem)
		{
			((ExplorerBarGroupItem)item).Image = image;
		}
		else if (item is LabelItem)
		{
			((LabelItem)item).Image = image;
		}
		else if (item is SideBarPanelItem)
		{
			((SideBarPanelItem)item).Image = image;
		}
		else if (item is TabItem)
		{
			((TabItem)item).Image = image;
		}
		else if (item is ButtonX)
		{
			((ButtonX)item).Image = image;
		}
		else if (item is LabelX)
		{
			((LabelX)item).Image = image;
		}
		else if (item is ReflectionImage)
		{
			((ReflectionImage)item).Image = image;
		}
		else if (item is BubbleButton)
		{
			((BubbleButton)item).Image = image;
		}
		else
		{
			SetPropertyValue(item, "Image", image);
		}
	}

	public bool ShouldSerializeImage()
	{
		return _ᚋ;
	}

	public void ResetImage()
	{
		_ᚋ = false;
		_ነ = null;
	}

	protected virtual void OnImageSmallChanged()
	{
		SetImageSmallProperty();
	}

	protected virtual void SetImageSmallProperty()
	{
		IsSyncingCommand = true;
		try
		{
			ArrayList subscribers = GetSubscribers();
			Image ᚌ = _ᚌ;
			foreach (object item in subscribers)
			{
				SetImageSmallProperty(item, ᚌ);
			}
			RecalcLayout(subscribers);
		}
		finally
		{
			IsSyncingCommand = false;
		}
	}

	protected virtual void SetImageSmallProperty(object item, Image image)
	{
		if (item is ButtonItem)
		{
			((ButtonItem)item).ImageSmall = image;
		}
		else
		{
			SetPropertyValue(item, "ImageSmall", image);
		}
	}

	public bool ShouldSerializeImageSmall()
	{
		return _ᚍ;
	}

	public void ResetImageSmall()
	{
		_ᚍ = false;
		_ᚌ = null;
	}

	protected virtual void OnEnabledChanged()
	{
		SetEnabledProperty();
	}

	protected virtual void SetEnabledProperty()
	{
		IsSyncingCommand = true;
		try
		{
			ArrayList subscribers = GetSubscribers();
			bool ԋ = _Ԋ;
			foreach (object item in subscribers)
			{
				SetEnabledProperty(item, ԋ);
			}
		}
		finally
		{
			IsSyncingCommand = false;
		}
	}

	protected virtual void SetEnabledProperty(object item, bool enabled)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (item is BaseItem)
		{
			((BaseItem)item).Enabled = enabled;
		}
		else if (item is Control)
		{
			((Control)item).Enabled = enabled;
		}
		else
		{
			SetPropertyValue(item, "Enabled", enabled);
		}
	}

	public bool ShouldSerializeEnabled()
	{
		return _ᚎ;
	}

	public void ResetEnabled()
	{
		_ᚎ = false;
		_Ԋ = false;
	}

	public virtual void CommandSourceRegistered(ICommandSource source)
	{
		if (source != null && !GetDesignMode())
		{
			if (_ᚎ)
			{
				SetEnabledProperty(source, _Ԋ);
			}
			if (_ᚇ)
			{
				SetCheckedProperty(source, _ᚆ);
			}
			if (_ᚋ)
			{
				SetImageProperty(source, _ነ);
			}
			if (_ᚍ)
			{
				SetImageSmallProperty(source, _ᚌ);
			}
			if (_ᚅ)
			{
				SetTextProperty(source, _ኚ);
			}
			if (_ᚊ)
			{
				SetVisibleProperty(source, _ᚉ);
			}
		}
	}

	public virtual void CommandSourceUnregistered(ICommandSource source)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			CommandManager.UnRegisterCommand(this);
		}
		base.Dispose(disposing);
	}
}
