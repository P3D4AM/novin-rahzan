using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(true)]
[ToolboxBitmap(typeof(Command), "Command.ico")]
[DefaultEvent("Executed")]
public class Command : Component, ICommand
{
	private string _Text;

	private bool _TextSet;

	private bool _Checked;

	private bool _CheckedSet;

	private bool _Visible;

	private bool _VisibleSet;

	private Image _Image;

	private bool _ImageSet;

	private Image _ImageSmall;

	private bool _ImageSmallSet;

	private bool _Enabled = true;

	private bool _EnabledSet;

	private string _Name = "";

	[Localizable(true)]
	[Description("Indicates Text that is assigned to all command sources that are using this command and have Text property.")]
	public string Text
	{
		get
		{
			return _Text;
		}
		set
		{
			_Text = value;
			_TextSet = true;
			OnTextChanged();
		}
	}

	[Description("Indicates value for the Checked property that is assigned to the command subscribers using this command and have Checked property.")]
	public bool Checked
	{
		get
		{
			return _Checked;
		}
		set
		{
			_Checked = value;
			_CheckedSet = true;
			OnCheckedChanged();
		}
	}

	[Description("Indicates value for the Visible property that is assigned to the command subscribers using this command and have Visible property.")]
	public bool Visible
	{
		get
		{
			return _Visible;
		}
		set
		{
			_Visible = value;
			_VisibleSet = true;
			OnVisibleChanged();
		}
	}

	[Description("Indicates image that is assigned to the command subscribers using this command and have Image property.")]
	[Localizable(true)]
	public Image Image
	{
		get
		{
			return _Image;
		}
		set
		{
			_Image = value;
			_ImageSet = true;
			OnImageChanged();
		}
	}

	[Description("Indicates small image that is assigned to the command subscribers using this command and have ImageSmall property.")]
	[Localizable(true)]
	public Image ImageSmall
	{
		get
		{
			return _ImageSmall;
		}
		set
		{
			_ImageSmall = value;
			_ImageSmallSet = true;
			OnImageSmallChanged();
		}
	}

	[Description("Indicates value for Enabled property assigned to the command subscribers using this command and have Enabled property.")]
	public bool Enabled
	{
		get
		{
			return _Enabled;
		}
		set
		{
			_Enabled = value;
			_EnabledSet = true;
			OnEnabledChanged();
		}
	}

	[Browsable(false)]
	[Category("Design")]
	[Description("Indicates the name used to identify node.")]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				_Name = Site.Name;
			}
			return _Name;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				_Name = "";
			}
			else
			{
				_Name = value;
			}
		}
	}

	public event EventHandler Executed;

	public event CancelEventHandler PreviewExecuted;

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
		this.Executed?.Invoke(commandSource, e);
	}

	protected virtual void OnPreviewExecuted(ICommandSource commandSource, CancelEventArgs e)
	{
		this.PreviewExecuted?.Invoke(commandSource, e);
	}

	protected virtual void OnTextChanged()
	{
		SetTextProperty();
	}

	protected virtual void SetTextProperty()
	{
		ArrayList subscribers = GetSubscribers();
		ArrayList arrayList = new ArrayList(subscribers.Count);
		string text = _Text;
		if (GetDesignMode())
		{
			foreach (object item in subscribers)
			{
				if (IsTextPropertyChanged(item, text))
				{
					SetPropertyValue(item, "Text", text);
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
				if (IsTextPropertyChanged(item2, text))
				{
					SetTextProperty(item2, text);
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
		return GetPropertyValue(item, "Text") as string != text;
	}

	protected virtual void SetTextProperty(object item, string text)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
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
			if (propertyDescriptor != null && (type == null || propertyDescriptor.PropertyType == type))
			{
				propertyDescriptor.SetValue(item, value);
				arrayList.Add(item);
			}
		}
		RecalcLayout(arrayList);
	}

	private ArrayList GetSubscribers()
	{
		return CommandManager.GetSubscribers(this);
	}

	public bool ShouldSerializeText()
	{
		return _TextSet;
	}

	public void ResetText()
	{
		_TextSet = false;
		_Text = null;
	}

	protected virtual void OnCheckedChanged()
	{
		SetCheckedProperty();
	}

	protected virtual void SetCheckedProperty()
	{
		ArrayList subscribers = GetSubscribers();
		bool @checked = _Checked;
		foreach (object item in subscribers)
		{
			SetCheckedProperty(item, @checked);
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
		return _CheckedSet;
	}

	public void ResetChecked()
	{
		_CheckedSet = false;
		_Checked = false;
	}

	protected virtual void OnVisibleChanged()
	{
		SetVisibleProperty();
	}

	protected virtual void SetVisibleProperty()
	{
		ArrayList subscribers = GetSubscribers();
		bool visible = _Visible;
		foreach (object item in subscribers)
		{
			SetVisibleProperty(item, visible);
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
		return _VisibleSet;
	}

	public void ResetVisible()
	{
		_VisibleSet = false;
		_Visible = false;
	}

	protected virtual void OnImageChanged()
	{
		SetImageProperty();
	}

	protected virtual void SetImageProperty()
	{
		ArrayList subscribers = GetSubscribers();
		Image image = _Image;
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
					else if (buttonItem.ContainerControl is QatToolbar)
					{
						flag = true;
					}
					if (flag && image != null && (image.Width > 16 || image.Height > 16))
					{
						buttonItem.UseSmallImage = true;
						if (buttonItem.ImageSmall == null)
						{
							TypeDescriptor.GetProperties(buttonItem)["ImageFixedSize"].SetValue(buttonItem, new Size(16, 16));
						}
					}
				}
				SetPropertyValue(item, "Image", image);
			}
		}
		else
		{
			foreach (object item2 in subscribers)
			{
				SetImageProperty(item2, image);
			}
		}
		RecalcLayout(subscribers);
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
		else
		{
			SetPropertyValue(item, "Image", image);
		}
	}

	public bool ShouldSerializeImage()
	{
		return _ImageSet;
	}

	public void ResetImage()
	{
		_ImageSet = false;
		_Image = null;
	}

	protected virtual void OnImageSmallChanged()
	{
		SetImageSmallProperty();
	}

	protected virtual void SetImageSmallProperty()
	{
		ArrayList subscribers = GetSubscribers();
		Image imageSmall = _ImageSmall;
		foreach (object item in subscribers)
		{
			SetImageSmallProperty(item, imageSmall);
		}
		RecalcLayout(subscribers);
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
		return _ImageSmallSet;
	}

	public void ResetImageSmall()
	{
		_ImageSmallSet = false;
		_ImageSmall = null;
	}

	protected virtual void OnEnabledChanged()
	{
		SetEnabledProperty();
	}

	protected virtual void SetEnabledProperty()
	{
		ArrayList subscribers = GetSubscribers();
		bool enabled = _Enabled;
		foreach (object item in subscribers)
		{
			SetEnabledProperty(item, enabled);
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
		return _EnabledSet;
	}

	public void ResetEnabled()
	{
		_EnabledSet = false;
		_Enabled = false;
	}

	public virtual void CommandSourceRegistered(ICommandSource source)
	{
		if (source != null && !GetDesignMode())
		{
			if (_EnabledSet)
			{
				SetEnabledProperty(source, _Enabled);
			}
			if (_CheckedSet)
			{
				SetCheckedProperty(source, _Checked);
			}
			if (_ImageSet)
			{
				SetImageProperty(source, _Image);
			}
			if (_ImageSmallSet)
			{
				SetImageSmallProperty(source, _ImageSmall);
			}
			if (_TextSet)
			{
				SetTextProperty(source, _Text);
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
