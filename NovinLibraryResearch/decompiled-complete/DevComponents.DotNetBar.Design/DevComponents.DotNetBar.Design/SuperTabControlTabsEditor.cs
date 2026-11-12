using System;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design;

public class SuperTabControlTabsEditor : SuperTabStripTabsEditor
{
	public SuperTabControlTabsEditor(Type type)
		: base(type)
	{
	}

	protected override object CreateInstance(Type itemType)
	{
		object obj = base.CreateInstance(itemType);
		if (obj is SuperTabItem)
		{
			SuperTabItem superTabItem = obj as SuperTabItem;
			superTabItem.Text = (string.IsNullOrEmpty(superTabItem.Name) ? "My Tab" : superTabItem.Name);
			CreateNewTab(superTabItem);
		}
		else if (obj is ButtonItem)
		{
			ButtonItem buttonItem = obj as ButtonItem;
			buttonItem.Text = (string.IsNullOrEmpty(buttonItem.Name) ? "My Button" : buttonItem.Name);
		}
		return obj;
	}

	private void CreateNewTab(SuperTabItem tab)
	{
		IDesignerHost designerHost = (IDesignerHost)((CollectionEditor)this).GetService(typeof(IDesignerHost));
		SuperTabControl superTabControl = ((CollectionEditor)this).Context.Instance as SuperTabControl;
		if (designerHost == null || superTabControl == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			IComponentChangeService componentChangeService = ((CollectionEditor)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((CollectionEditor)this).Context.Container, null);
			SuperTabControlPanel superTabControlPanel = designerHost.CreateComponent(typeof(SuperTabControlPanel)) as SuperTabControlPanel;
			if (tab != null && superTabControlPanel != null)
			{
				tab.AttachedControl = (Control)(object)superTabControlPanel;
				superTabControlPanel.TabItem = tab;
				((Control)superTabControl).Controls.Add((Control)(object)superTabControlPanel);
				superTabControl.Tabs.Add(tab);
				((Control)superTabControlPanel).Dock = (DockStyle)5;
				((Control)superTabControlPanel).BringToFront();
				superTabControl.ApplyPanelStyle(superTabControlPanel);
				superTabControl.SelectedTab = tab;
			}
			componentChangeService?.OnComponentChanged(((CollectionEditor)this).Context.Container, null, null, null);
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
	}
}
