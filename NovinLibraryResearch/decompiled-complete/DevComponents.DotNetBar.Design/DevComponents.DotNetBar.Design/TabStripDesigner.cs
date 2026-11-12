using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class TabStripDesigner : ParentControlDesigner
{
	protected bool m_InternalRemove;

	private TabItem m_SelectItem;

	private bool m_IgnoreMouseUp;

	private bool m_SelectTabControl;

	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[3]
			{
				new DesignerVerb("Next Tab", SelectNextTab),
				new DesignerVerb("Previous Tab", SelectPreviousTab),
				new DesignerVerb("Create New Tab", CreateNewTab)
			};
			return new DesignerVerbCollection(value);
		}
	}

	[Category("Mdi Support")]
	[DefaultValue(false)]
	[Description("Indicates whether Tab-Strip control provides Tabbed MDI Child form support.")]
	[Browsable(true)]
	public bool MdiTabbedDocuments
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["MdiTabbedDocuments"];
		}
		set
		{
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			DesignerTransaction designerTransaction = null;
			if (designerHost != null)
			{
				designerTransaction = designerHost.CreateTransaction();
			}
			try
			{
				((ComponentDesigner)this).ShadowProperties["MdiTabbedDocuments"] = value;
				if (((ControlDesigner)this).Control is TabStrip component && designerHost != null && !designerHost.Loading)
				{
					if (value)
					{
						TypeDescriptor.GetProperties(component)["MdiForm"].SetValue(component, null);
						IComponent rootComponent = designerHost.RootComponent;
						Form value2 = (Form)((rootComponent is Form) ? rootComponent : null);
						TypeDescriptor.GetProperties(component)["MdiForm"].SetValue(component, value2);
					}
					else
					{
						TypeDescriptor.GetProperties(component)["MdiForm"].SetValue(component, null);
					}
				}
			}
			catch
			{
				designerTransaction?.Cancel();
				throw;
			}
			finally
			{
				if (designerTransaction != null && !designerTransaction.Canceled)
				{
					designerTransaction.Commit();
				}
			}
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			TabStrip tabStrip = GetTabStrip();
			if (tabStrip != null)
			{
				foreach (TabItem tab in tabStrip.Tabs)
				{
					arrayList.Add(tab);
				}
			}
			return arrayList;
		}
	}

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		if (component.Site.DesignMode)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += OnSelectionChanged;
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving += OnComponentRemoving;
			}
			TabStrip tabStrip = GetTabStrip();
			if (tabStrip != null)
			{
				tabStrip.TabMoved += TabMoved;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null)
		{
			selectionService.SelectionChanged -= OnSelectionChanged;
		}
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoving -= OnComponentRemoving;
		}
		((ParentControlDesigner)this).Dispose(disposing);
	}

	protected virtual void CreateNewTab(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (!(((ControlDesigner)this).Control is TabStrip tabStrip) || designerHost == null)
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
			tabStrip.Tabs.Add(tabItem);
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
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

	private void SelectNextTab(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabStrip tabStrip && tabStrip.SelectedTabIndex < tabStrip.Tabs.Count - 1)
		{
			TypeDescriptor.GetProperties(tabStrip)["SelectedTabIndex"].SetValue(tabStrip, tabStrip.SelectedTabIndex + 1);
		}
	}

	private void SelectPreviousTab(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabStrip { SelectedTabIndex: >0 } tabStrip)
		{
			TypeDescriptor.GetProperties(tabStrip)["SelectedTabIndex"].SetValue(tabStrip, tabStrip.SelectedTabIndex - 1);
		}
	}

	private void TabMoved(object sender, TabStripTabMovedEventArgs e)
	{
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			componentChangeService.OnComponentChanging(((ControlDesigner)this).Control, null);
			componentChangeService.OnComponentChanged(((ControlDesigner)this).Control, null, null, null);
		}
	}

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		TabStrip tabStrip = GetTabStrip();
		if (tabStrip != null && !((Control)tabStrip).IsDisposed)
		{
			ISelectionService selectionService = (ISelectionService)sender;
			if (selectionService != null && selectionService.PrimarySelection != ((ControlDesigner)this).Control && selectionService.PrimarySelection is TabItem && tabStrip.Tabs.Contains(selectionService.PrimarySelection as TabItem))
			{
				tabStrip.DesignTimeSelection = selectionService.PrimarySelection as TabItem;
			}
			else
			{
				tabStrip.DesignTimeSelection = null;
			}
		}
	}

	private void OnComponentRemoving(object sender, ComponentEventArgs e)
	{
		InternalComponentRemoving(e);
	}

	protected virtual void InternalComponentRemoving(ComponentEventArgs e)
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
		TabStrip tabStrip = GetTabStrip();
		if (tabStrip == null || ((Control)tabStrip).IsDisposed)
		{
			return;
		}
		if (e.Component is TabItem && ((ControlDesigner)this).Control != null && tabStrip.Tabs.Contains(e.Component as TabItem))
		{
			try
			{
				TabItem tabItem = e.Component as TabItem;
				if (tabItem.AttachedControl != null && ((ControlDesigner)this).Control.Controls.Contains(tabItem.AttachedControl))
				{
					((ControlDesigner)this).Control.Controls.Remove(tabItem.AttachedControl);
					designerHost.DestroyComponent((IComponent)tabItem.AttachedControl);
				}
				tabStrip.Tabs.Remove(tabItem);
				tabStrip.RecalcSize();
				((Control)tabStrip).Refresh();
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
			TabStrip tabStrip2 = ((ControlDesigner)this).Control as TabStrip;
			TabItem[] array = new TabItem[tabStrip2.Tabs.Count];
			tabStrip2.Tabs.CopyTo(array, 0);
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

	protected virtual TabStrip GetTabStrip()
	{
		return ((ControlDesigner)this).Control as TabStrip;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		if (((ControlDesigner)this).Control is TabStrip)
		{
			properties["MdiTabbedDocuments"] = TypeDescriptor.CreateProperty(typeof(TabStripDesigner), (PropertyDescriptor)properties["MdiTabbedDocuments"], new DefaultValueAttribute(value: false));
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ComponentDesigner)this).Component != null && ((ComponentDesigner)this).Component.Site != null && ((ComponentDesigner)this).Component.Site.DesignMode)
		{
			if (((ControlDesigner)this).Control is TabStrip tabStrip)
			{
				tabStrip.Style = eTabStripStyle.VS2005;
			}
			CreateNewTab(null, null);
		}
	}

	protected override void WndProc(ref Message m)
	{
		Control control = ((ControlDesigner)this).Control;
		if (control == null || control.IsDisposed)
		{
			((ControlDesigner)this).WndProc(ref m);
			return;
		}
		switch (((Message)(ref m)).Msg)
		{
		case 513:
		case 516:
			if (OnMouseDown(ref m))
			{
				return;
			}
			break;
		case 514:
		case 517:
			if (OnMouseUp(ref m))
			{
				return;
			}
			break;
		case 512:
			if (OnMouseMove(ref m))
			{
				return;
			}
			break;
		case 675:
			if (OnMouseLeave(ref m))
			{
				return;
			}
			break;
		case 1125:
			if (OnProcessPendingSelection(ref m))
			{
				return;
			}
			break;
		}
		((ControlDesigner)this).WndProc(ref m);
	}

	protected virtual bool OnMouseDown(ref Message m)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		TabStrip tabStrip = GetTabStrip();
		if (tabStrip == null || ((Control)tabStrip).IsDisposed || ((ControlDesigner)this).Control.IsDisposed)
		{
			return false;
		}
		Point point = ((Control)tabStrip).PointToClient(Control.MousePosition);
		MouseEventArgs val = new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0);
		TabItem selectedTab = tabStrip.SelectedTab;
		tabStrip.InternalOnMouseDown(val);
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		TabItem tabItem = null;
		if (selectionService != null && selectionService.PrimarySelection is TabItem)
		{
			tabItem = selectionService.PrimarySelection as TabItem;
		}
		if (tabStrip.SelectedTab != null && tabItem != tabStrip.SelectedTab && tabStrip.HitTest(val.X, val.Y) != null)
		{
			ArrayList arrayList = new ArrayList(1);
			arrayList.Add(tabStrip.SelectedTab);
			selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
			m_IgnoreMouseUp = true;
			if (((Message)(ref m)).Msg == 516)
			{
				((ControlDesigner)this).OnContextMenu(Control.MousePosition.X, Control.MousePosition.Y);
			}
			return true;
		}
		if (tabStrip.SelectedTab == selectedTab && tabStrip.HitTest(val.X, val.Y) != null)
		{
			if (((Message)(ref m)).Msg == 516)
			{
				((ControlDesigner)this).OnContextMenu(Control.MousePosition.X, Control.MousePosition.Y);
			}
			return true;
		}
		return false;
	}

	protected virtual bool OnMouseUp(ref Message m)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		TabStrip tabStrip = GetTabStrip();
		if (tabStrip == null || ((Control)tabStrip).IsDisposed || ((ControlDesigner)this).Control.IsDisposed)
		{
			return false;
		}
		Point point = ((Control)tabStrip).PointToClient(Control.MousePosition);
		MouseEventArgs e = new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0);
		tabStrip.InternalOnMouseUp(e);
		if (m_IgnoreMouseUp)
		{
			m_IgnoreMouseUp = false;
			return true;
		}
		if (m_SelectTabControl)
		{
			m_SelectTabControl = false;
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null && selectionService.PrimarySelection != ((ControlDesigner)this).Control && (object)selectionService.PrimarySelection.GetType() != ((object)((ControlDesigner)this).Control).GetType())
			{
				ArrayList arrayList = new ArrayList(1);
				arrayList.Add(((ControlDesigner)this).Control);
				selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
			}
		}
		return false;
	}

	protected virtual bool OnMouseMove(ref Message m)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		TabStrip tabStrip = GetTabStrip();
		if (tabStrip == null || ((Control)tabStrip).IsDisposed || ((ControlDesigner)this).Control.IsDisposed)
		{
			return false;
		}
		Point pt = ((Control)tabStrip).PointToClient(Control.MousePosition);
		MouseEventArgs e = new MouseEventArgs((MouseButtons)((((Message)(ref m)).WParam.ToInt32() == 1) ? 1048576 : 0), 0, pt.X, pt.Y, 0);
		tabStrip.InternalOnMouseMove(e);
		if (((Control)tabStrip).DisplayRectangle.Contains(pt))
		{
			return true;
		}
		return false;
	}

	protected virtual bool OnMouseLeave(ref Message m)
	{
		TabStrip tabStrip = GetTabStrip();
		if (tabStrip == null || ((Control)tabStrip).IsDisposed || ((ControlDesigner)this).Control.IsDisposed)
		{
			return false;
		}
		tabStrip.InternalOnMouseLeave(new EventArgs());
		return false;
	}

	protected virtual bool OnProcessPendingSelection(ref Message m)
	{
		return ProcessPendingSelection();
	}

	protected virtual bool ProcessPendingSelection()
	{
		TabStrip tabStrip = GetTabStrip();
		if (tabStrip == null || ((Control)tabStrip).IsDisposed || ((ControlDesigner)this).Control.IsDisposed)
		{
			return false;
		}
		if (m_SelectItem != null)
		{
			tabStrip.DesignTimeSelection = m_SelectItem;
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService == null)
			{
				return false;
			}
			ArrayList arrayList = new ArrayList(1);
			arrayList.Add(m_SelectItem);
			selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
			m_SelectItem = null;
			if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
			{
				componentChangeService.OnComponentChanging(((ControlDesigner)this).Control, null);
				componentChangeService.OnComponentChanged(((ControlDesigner)this).Control, null, null, null);
			}
			return true;
		}
		return false;
	}
}
