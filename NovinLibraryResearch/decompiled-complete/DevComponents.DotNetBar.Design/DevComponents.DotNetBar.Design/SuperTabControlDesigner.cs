using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class SuperTabControlDesigner : SuperTabStripDesigner
{
	private SuperTabControl _TabControl;

	[Description("Indicates the index of selected tab.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public int SelectedTabIndex
	{
		get
		{
			return (int)((ComponentDesigner)this).ShadowProperties["SelectedTabIndex"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["SelectedTabIndex"] = value;
		}
	}

	[Description("Indicates whether tabs are visible")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool TabsVisible
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["TabsVisible"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["TabsVisible"] = value;
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(base.AssociatedComponents);
			foreach (BaseItem tab in _TabControl.Tabs)
			{
				arrayList.Add(tab);
			}
			return arrayList;
		}
	}

	internal bool IsBackstageTab
	{
		get
		{
			SuperTabControl superTabControl = (SuperTabControl)(object)((ControlDesigner)this).Control;
			if (superTabControl != null && superTabControl.TabStrip != null)
			{
				return superTabControl.TabStrip.ApplicationButton != null;
			}
			return false;
		}
	}

	public SuperTabControlDesigner()
	{
		TabsVisible = true;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (!component.Site.DesignMode)
		{
			return;
		}
		_TabControl = component as SuperTabControl;
		if (_TabControl == null)
		{
			return;
		}
		base.TabStrip = _TabControl.TabStrip;
		if (((ComponentDesigner)this).Inherited)
		{
			if (!_TabControl.TabsVisible)
			{
				_TabControl.TabsVisible = true;
			}
			if (_TabControl.SelectedPanel != null)
			{
				((Control)_TabControl.SelectedPanel).BringToFront();
			}
		}
		_TabControl.TabStrip.GetBaseItemContainer().SetDesignMode(b: true);
	}

	protected override BaseItem GetItemContainer()
	{
		return _TabControl.TabStrip.GetBaseItemContainer();
	}

	protected override Control GetItemContainerControl()
	{
		return (Control)(object)_TabControl.TabStrip;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		properties["SelectedTabIndex"] = TypeDescriptor.CreateProperty(typeof(SuperTabControlDesigner), (PropertyDescriptor)properties["SelectedTabIndex"], new BrowsableAttribute(browsable: true), new CategoryAttribute("Behavior"));
		properties["TabsVisible"] = TypeDescriptor.CreateProperty(typeof(SuperTabControlDesigner), (PropertyDescriptor)properties["TabsVisible"], new BrowsableAttribute(browsable: true), new CategoryAttribute("Behavior"));
	}

	protected override void PostFilterEvents(IDictionary events)
	{
		events.Remove("MouseUp");
		events.Remove("MouseDown");
		events.Remove("MouseMove");
		events.Remove("MouseClick");
		events.Remove("MouseDoubleClick");
		events.Remove("MouseEnter");
		events.Remove("MouseHover");
		events.Remove("MouseLeave");
		((ComponentDesigner)this).PostFilterEvents(events);
	}

	public override SuperTabItem CreateNewTab()
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		SuperTabItem superTabItem = null;
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			superTabItem = designerHost.CreateComponent(typeof(SuperTabItem)) as SuperTabItem;
			SuperTabControlPanel superTabControlPanel = designerHost.CreateComponent(typeof(SuperTabControlPanel)) as SuperTabControlPanel;
			if (superTabItem != null && superTabControlPanel != null)
			{
				superTabItem.Text = superTabItem.Name;
				_TabControl.CreateTab(superTabItem, superTabControlPanel, -1);
				_TabControl.SelectedTab = superTabItem;
			}
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(superTabControlPanel, null);
				componentChangeService.OnComponentChanged(superTabControlPanel, null, null, null);
			}
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
		return superTabItem;
	}

	protected override void SelectNextTab(object sender, EventArgs e)
	{
		if (_TabControl.SelectedTabIndex < _TabControl.Tabs.Count - 1)
		{
			_TabControl.SelectNextTab();
		}
	}

	protected override void SelectPreviousTab(object sender, EventArgs e)
	{
		if (_TabControl.SelectedTabIndex > 0)
		{
			_TabControl.SelectPreviousTab();
		}
	}

	protected SuperTabStrip GetTabStrip()
	{
		return _TabControl.TabStrip;
	}

	protected override IOwner GetIOwner()
	{
		return GetTabStrip();
	}

	public override bool CanParent(Control c)
	{
		return c is SuperTabControlPanel;
	}

	protected override void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
	{
		if (!(((ComponentDesigner)this).GetService(typeof(ISelectionService)) is ISelectionService selectionService) || selectionService.PrimarySelection != _TabControl)
		{
			return;
		}
		if (m_InsertItemTransaction == null && sender is IDesignerHost designerHost)
		{
			m_InsertItemTransaction = designerHost.CreateTransaction("Adding Item Clip");
		}
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService && e.Component is BaseItem)
		{
			componentChangeService.OnComponentChanging(_TabControl, null);
			_TabControl.Tabs.Add(e.Component as BaseItem);
			componentChangeService.OnComponentChanged(_TabControl, null, null, null);
			if (e.Component is SuperTabItem)
			{
				_TabControl.SelectedTab = e.Component as SuperTabItem;
			}
		}
		if (m_InsertItemTransaction != null)
		{
			m_InsertItemTransaction.Commit();
			m_InsertItemTransaction = null;
		}
		RecalcLayout();
	}

	protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (InternalRemove)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		if (e.Component is SuperTabControlPanel && ((Control)_TabControl).Controls.Contains((Control)/*isinst with value type is only supported in some contexts*/))
		{
			SuperTabControlPanel superTabControlPanel = e.Component as SuperTabControlPanel;
			((Control)_TabControl).Controls.Remove((Control)(object)superTabControlPanel);
			if (superTabControlPanel.TabItem != null)
			{
				if (_TabControl.Tabs.Contains(superTabControlPanel.TabItem))
				{
					_TabControl.Tabs.Remove(superTabControlPanel.TabItem);
				}
				if (!((ComponentDesigner)this).Inherited)
				{
					designerHost.DestroyComponent(superTabControlPanel.TabItem);
				}
			}
		}
		else if (e.Component is BaseItem && _TabControl.Tabs.Contains(e.Component as BaseItem))
		{
			if (!designerHost.TransactionDescription.StartsWith("Deleting"))
			{
				BaseItem baseItem = e.Component as BaseItem;
				_TabControl.Tabs.Remove(baseItem);
				if (baseItem is SuperTabItem { AttachedControl: not null } superTabItem && ((Control)_TabControl).Controls.Contains(superTabItem.AttachedControl))
				{
					((Control)_TabControl).Controls.Remove(superTabItem.AttachedControl);
					designerHost.DestroyComponent((IComponent)superTabItem.AttachedControl);
				}
			}
		}
		else
		{
			if (e.Component != ((ControlDesigner)this).Control)
			{
				return;
			}
			InternalRemove = true;
			try
			{
				for (int num = _TabControl.Tabs.Count - 1; num >= 0; num--)
				{
					designerHost.DestroyComponent(_TabControl.Tabs[num]);
				}
			}
			finally
			{
				InternalRemove = false;
			}
		}
	}

	protected override Point GetClientPoint(int x, int y)
	{
		return ((Control)_TabControl.TabStrip).PointToClient(new Point(x, y));
	}

	protected override void OnitemCreated(BaseItem item)
	{
		if (item is ButtonItem && IsBackstageTab)
		{
			ButtonItem buttonItem = (ButtonItem)item;
			buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
			buttonItem.ColorTable = eButtonColor.Blue;
			buttonItem.ImagePaddingVertical = 10;
			buttonItem.ImagePaddingHorizontal = 18;
			buttonItem.Stretch = true;
		}
		base.OnitemCreated(item);
	}
}
