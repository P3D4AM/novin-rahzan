using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class BindingNavigatorExDesigner : BarDesigner
{
	private static readonly string[] ItemPropertyNames = new string[8] { "MovePreviousButton", "MoveFirstButton", "MoveNextButton", "MoveLastButton", "AddNewRecordButton", "DeleteButton", "PositionTextBox", "CountLabel" };

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		base.InitializeNewComponent(defaultValues);
		BindingNavigatorEx bindingNavigatorEx = (BindingNavigatorEx)((ComponentDesigner)this).Component;
		IDesignerHost host = (IDesignerHost)((ComponentDesigner)this).Component.Site.GetService(typeof(IDesignerHost));
		((Control)bindingNavigatorEx).Dock = (DockStyle)1;
		bindingNavigatorEx.SuspendLayout();
		bindingNavigatorEx.AddDefaultItems();
		SiteItems(host, bindingNavigatorEx.Items);
		RaiseItemsChanged();
		bindingNavigatorEx.ResumeLayout();
		bindingNavigatorEx.RecalcLayout();
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoved += ComponentRemoved;
			componentChangeService.ComponentChanged += ComponentChanged;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoved -= ComponentRemoved;
				componentChangeService.ComponentChanged -= ComponentChanged;
			}
		}
		base.Dispose(disposing);
	}

	private void ComponentChanged(object sender, ComponentChangedEventArgs e)
	{
		BindingNavigatorEx bindingNavigatorEx = (BindingNavigatorEx)((ComponentDesigner)this).Component;
		if (e.Component != null && e.Component == bindingNavigatorEx.CountLabel && e.Member != null && e.Member.Name == "Text")
		{
			bindingNavigatorEx.CountLabelFormat = bindingNavigatorEx.CountLabel.Text;
		}
	}

	private new void ComponentRemoved(object sender, ComponentEventArgs e)
	{
		if (e.Component is BaseItem baseItem)
		{
			BindingNavigatorEx bindingNavigatorEx = (BindingNavigatorEx)((ComponentDesigner)this).Component;
			if (baseItem == bindingNavigatorEx.MoveFirstButton)
			{
				bindingNavigatorEx.MoveFirstButton = null;
			}
			else if (baseItem == bindingNavigatorEx.MovePreviousButton)
			{
				bindingNavigatorEx.MovePreviousButton = null;
			}
			else if (baseItem == bindingNavigatorEx.MoveNextButton)
			{
				bindingNavigatorEx.MoveNextButton = null;
			}
			else if (baseItem == bindingNavigatorEx.MoveLastButton)
			{
				bindingNavigatorEx.MoveLastButton = null;
			}
			else if (baseItem == bindingNavigatorEx.PositionTextBox)
			{
				bindingNavigatorEx.PositionTextBox = null;
			}
			else if (baseItem == bindingNavigatorEx.CountLabel)
			{
				bindingNavigatorEx.CountLabel = null;
			}
			else if (baseItem == bindingNavigatorEx.AddNewRecordButton)
			{
				bindingNavigatorEx.AddNewRecordButton = null;
			}
			else if (baseItem == bindingNavigatorEx.DeleteButton)
			{
				bindingNavigatorEx.DeleteButton = null;
			}
		}
	}

	private void RaiseItemsChanged()
	{
		BindingNavigatorEx component = (BindingNavigatorEx)((ComponentDesigner)this).Component;
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService == null)
		{
			return;
		}
		MemberDescriptor member = TypeDescriptor.GetProperties(component)["Items"];
		componentChangeService.OnComponentChanging(component, member);
		componentChangeService.OnComponentChanged(component, member, null, null);
		string[] itemPropertyNames = ItemPropertyNames;
		foreach (string name in itemPropertyNames)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)[name];
			if (propertyDescriptor != null)
			{
				componentChangeService.OnComponentChanging(component, propertyDescriptor);
				componentChangeService.OnComponentChanged(component, propertyDescriptor, null, null);
			}
		}
	}

	private void SiteItems(IDesignerHost host, SubItemsCollection items)
	{
		foreach (BaseItem item in items)
		{
			SiteItem(host, item);
		}
	}

	private void SiteItem(IDesignerHost host, BaseItem item)
	{
		host.Container.Add(item, GetUniqueSiteName(host, item.Name));
		item.Name = item.Site.Name;
		if (item.SubItems.Count > 0)
		{
			SiteItems(host, item.SubItems);
		}
	}

	internal static string GetUniqueSiteName(IDesignerHost host, string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		INameCreationService nameCreationService = (INameCreationService)host.GetService(typeof(INameCreationService));
		if (nameCreationService == null)
		{
			return null;
		}
		if (host.Container.Components[name] == null)
		{
			if (!nameCreationService.IsValidName(name))
			{
				return null;
			}
			return name;
		}
		string text = name;
		int num = 1;
		while (!nameCreationService.IsValidName(text))
		{
			text = name + num.ToString(CultureInfo.InvariantCulture);
			num++;
		}
		return text;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		base.PreFilterProperties(properties);
		RemoveDescriptors(properties, new string[33]
		{
			"AlwaysDisplayDockTab", "AlwaysDisplayKeyAccelerators", "AutoCreateCaptionMenu", "AutoHide", "AutoHideAnimationTime", "AutoHideTabTextAlwaysVisible", "AutoSyncBarCaption", "BarType", "CanAutoHide", "CanCustomize",
			"CanDockBottom", "CanDockDocument", "CanDockLeft", "CanDockRight", "CanDockTab", "CanDockTop", "CanDockHide", "CanReorderTabs", "CanUndock", "CaptionHeight",
			"CloseSingleTab", "DisplayMoreItemsOnMenu", "DockOrientation", "DockTabAlignment", "DockTabCloseButtonVisible", "DockTabStripHeight", "EqualButtonSize", "LayoutType", "MenuBar", "SaveLayoutChanges",
			"SelectedDockTab", "TabNavigation", "WrapItemsDock"
		});
	}

	private void RemoveDescriptors(IDictionary properties, string[] propNames)
	{
		foreach (string key in propNames)
		{
			if (properties.Contains(key))
			{
				properties.Remove(key);
			}
		}
	}

	protected override void PreFilterEvents(IDictionary events)
	{
		RemoveDescriptors(events, new string[14]
		{
			"AutoHideChanged", "AutoHideDisplay", "BarDock", "BarUndock", "BeforeAutoHideDisplayed", "BeforeAutoHideHidden", "BeforeDockTabDisplayed", "Closing", "DefinitionLoaded", "DeserializeItem",
			"DockTabChange", "DockTabClosed", "DockTabClosing", "SerializeItem"
		});
		((ComponentDesigner)this).PreFilterEvents(events);
	}
}
