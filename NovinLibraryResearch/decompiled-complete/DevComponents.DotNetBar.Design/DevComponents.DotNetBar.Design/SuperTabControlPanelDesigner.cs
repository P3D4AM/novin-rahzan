using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class SuperTabControlPanelDesigner : PanelControlDesigner
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

	protected override void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is PanelControl panelControl)
		{
			panelControl.ApplyLabelStyle();
			((Control)panelControl).Text = "";
		}
	}

	protected virtual void CreateNewTab(object sender, EventArgs e)
	{
		if (!(((ControlDesigner)this).Control is SuperTabControlPanel superTabControlPanel) || !(((Control)superTabControlPanel).Parent is SuperTabControl))
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (!(((Control)superTabControlPanel).Parent is SuperTabControl superTabControl) || designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			SuperTabItem superTabItem = designerHost.CreateComponent(typeof(SuperTabItem)) as SuperTabItem;
			SuperTabControlPanel superTabControlPanel2 = designerHost.CreateComponent(typeof(SuperTabControlPanel)) as SuperTabControlPanel;
			if (superTabItem != null && superTabControlPanel2 != null)
			{
				superTabItem.Text = superTabItem.Name;
				superTabControl.CreateTab(superTabItem, superTabControlPanel2, -1);
				superTabControl.SelectedTab = superTabItem;
			}
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(superTabControlPanel2, null);
				componentChangeService.OnComponentChanged(superTabControlPanel2, null, null, null);
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
		if (((ControlDesigner)this).Control is SuperTabControlPanel superTabControlPanel && ((Control)superTabControlPanel).Parent is SuperTabControl)
		{
			SuperTabControl superTabControl = ((Control)superTabControlPanel).Parent as SuperTabControl;
			superTabControl.SelectNextTab();
		}
	}

	private void SelectPreviousTab(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SuperTabControlPanel superTabControlPanel && ((Control)superTabControlPanel).Parent is SuperTabControl)
		{
			SuperTabControl superTabControl = ((Control)superTabControlPanel).Parent as SuperTabControl;
			superTabControl.SelectPreviousTab();
		}
	}
}
