using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class TabControlPanelDesigner : PanelExDesigner
{
	public override SelectionRules SelectionRules => (SelectionRules)(-1073741824);

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

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		base.InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private new void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is PanelEx panelEx)
		{
			panelEx.ApplyLabelStyle();
			((Control)panelEx).Text = "";
		}
	}

	protected virtual void CreateNewTab(object sender, EventArgs e)
	{
		if (!(((ControlDesigner)this).Control is TabControlPanel tabControlPanel) || !(((Control)tabControlPanel).Parent is TabControl))
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (!(((Control)tabControlPanel).Parent is TabControl tabControl) || designerHost == null)
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
			TabControlPanel tabControlPanel2 = designerHost.CreateComponent(typeof(TabControlPanel)) as TabControlPanel;
			tabControl.ApplyDefaultPanelStyle(tabControlPanel2);
			tabItem.AttachedControl = (Control)(object)tabControlPanel2;
			tabControlPanel2.TabItem = tabItem;
			tabControl.Tabs.Add(tabItem);
			((Control)tabControl).Controls.Add((Control)(object)tabControlPanel2);
			((Control)tabControlPanel2).Dock = (DockStyle)5;
			((Control)tabControlPanel2).SendToBack();
			tabControl.RecalcLayout();
			tabControl.SelectedTab = tabItem;
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(tabControlPanel2, null);
				componentChangeService.OnComponentChanged(tabControlPanel2, null, null, null);
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

	private void SelectNextTab(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabControlPanel tabControlPanel && ((Control)tabControlPanel).Parent is TabControl)
		{
			TabControl tabControl = ((Control)tabControlPanel).Parent as TabControl;
			tabControl.SelectNextTab();
		}
	}

	private void SelectPreviousTab(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is TabControlPanel tabControlPanel && ((Control)tabControlPanel).Parent is TabControl)
		{
			TabControl tabControl = ((Control)tabControlPanel).Parent as TabControl;
			tabControl.SelectPreviousTab();
		}
	}
}
