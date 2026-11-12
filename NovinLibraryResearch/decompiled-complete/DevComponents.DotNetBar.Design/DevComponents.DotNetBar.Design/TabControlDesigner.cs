using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class TabControlDesigner : TabStripDesigner
{
	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(base.AssociatedComponents);
			if (((ControlDesigner)this).Control is TabControl tabControl)
			{
				foreach (TabItem tab in tabControl.Tabs)
				{
					arrayList.Add(tab);
				}
			}
			return arrayList;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates the index of selected tab.")]
	[Browsable(true)]
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

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether tabs are visible")]
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

	public TabControlDesigner()
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
		TabControl tabControl = component as TabControl;
		if (((ComponentDesigner)this).Inherited)
		{
			if (!tabControl.TabsVisible)
			{
				tabControl.TabsVisible = true;
			}
			if (tabControl.SelectedPanel != null)
			{
				((Control)tabControl.SelectedPanel).BringToFront();
			}
		}
	}

	protected override void CreateNewTab(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (!(((ControlDesigner)this).Control is TabControl tabControl) || designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			TabItem tabItem = designerHost.CreateComponent(typeof(TabItem)) as TabItem;
			tabItem.Text = tabItem.Name;
			TabControlPanel tabControlPanel = designerHost.CreateComponent(typeof(TabControlPanel)) as TabControlPanel;
			tabControl.ApplyDefaultPanelStyle(tabControlPanel);
			tabItem.AttachedControl = (Control)(object)tabControlPanel;
			tabControlPanel.TabItem = tabItem;
			tabControl.Tabs.Add(tabItem);
			((Control)tabControl).Controls.Add((Control)(object)tabControlPanel);
			((Control)tabControlPanel).Dock = (DockStyle)5;
			((Control)tabControlPanel).SendToBack();
			tabControl.RecalcLayout();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(tabControlPanel, null);
				componentChangeService.OnComponentChanged(tabControlPanel, null, null, null);
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
	}

	public override bool CanParent(Control c)
	{
		if (c is TabControlPanel)
		{
			return true;
		}
		return false;
	}

	protected override void InternalComponentRemoving(ComponentEventArgs e)
	{
		if (m_InternalRemove)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		if (e.Component is TabControlPanel && ((ControlDesigner)this).Control != null && ((ControlDesigner)this).Control.Controls.Contains((Control)/*isinst with value type is only supported in some contexts*/))
		{
			try
			{
				TabControlPanel tabControlPanel = e.Component as TabControlPanel;
				if (tabControlPanel.TabItem != null)
				{
					TabControl tabControl = ((ControlDesigner)this).Control as TabControl;
					tabControl.Tabs.Remove(tabControlPanel.TabItem);
					designerHost.DestroyComponent(tabControlPanel.TabItem);
				}
				return;
			}
			finally
			{
				m_InternalRemove = false;
			}
		}
		if (e.Component is TabItem && ((ControlDesigner)this).Control != null && ((TabControl)(object)((ControlDesigner)this).Control).Tabs.Contains(e.Component as TabItem))
		{
			try
			{
				TabItem tabItem = e.Component as TabItem;
				if (tabItem.AttachedControl != null && ((ControlDesigner)this).Control.Controls.Contains(tabItem.AttachedControl))
				{
					TabControl tabControl2 = ((ControlDesigner)this).Control as TabControl;
					((Control)tabControl2).Controls.Remove(tabItem.AttachedControl);
					designerHost.DestroyComponent((IComponent)tabItem.AttachedControl);
				}
				return;
			}
			finally
			{
				m_InternalRemove = false;
			}
		}
		if (e.Component != ((ControlDesigner)this).Control)
		{
			return;
		}
		m_InternalRemove = true;
		try
		{
			TabControl tabControl3 = ((ControlDesigner)this).Control as TabControl;
			TabItem[] array = new TabItem[tabControl3.Tabs.Count];
			tabControl3.Tabs.CopyTo(array, 0);
			TabItem[] array2 = array;
			foreach (TabItem component in array2)
			{
				designerHost.DestroyComponent(component);
			}
		}
		finally
		{
			m_InternalRemove = false;
		}
	}

	protected override TabStrip GetTabStrip()
	{
		if (((ControlDesigner)this).Control == null || !(((ControlDesigner)this).Control is TabControl))
		{
			return null;
		}
		return ((TabControl)(object)((ControlDesigner)this).Control).TabStrip;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		base.PreFilterProperties(properties);
		properties["SelectedTabIndex"] = TypeDescriptor.CreateProperty(typeof(TabControlDesigner), (PropertyDescriptor)properties["SelectedTabIndex"], new BrowsableAttribute(browsable: true), new CategoryAttribute("Behavior"));
		properties["TabsVisible"] = TypeDescriptor.CreateProperty(typeof(TabControlDesigner), (PropertyDescriptor)properties["TabsVisible"], new BrowsableAttribute(browsable: true), new CategoryAttribute("Behavior"));
	}
}
