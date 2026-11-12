using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

internal class StiDockingManagerDesigner : ComponentDesigner
{
	private StiDockingManager dockingManager;

	private DesignerVerb addToLeft;

	private DesignerVerb addToRight;

	private DesignerVerb addToTop;

	private DesignerVerb addToBottom;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[4] { addToLeft, addToRight, addToTop, addToBottom });

	public override void Initialize(IComponent component)
	{
		((ComponentDesigner)this).Initialize(component);
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		dockingManager = (StiDockingManager)component;
		StiDockingManager stiDockingManager = dockingManager;
		IComponent rootComponent = designerHost.RootComponent;
		stiDockingManager.ParentForm = (Form)((rootComponent is Form) ? rootComponent : null);
	}

	private void OnAddToLeft(object sender, EventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add to left");
		StiDockingPanel stiDockingPanel = (StiDockingPanel)designerHost.CreateComponent(typeof(StiDockingPanel));
		((Control)(Form)designerHost.RootComponent).Controls.Add((Control)(object)stiDockingPanel);
		dockingManager.DockedPanels.Add(stiDockingPanel);
		stiDockingPanel.Manager = dockingManager;
		((Control)stiDockingPanel).Dock = (DockStyle)3;
		designerTransaction.Commit();
	}

	private void OnAddToRight(object sender, EventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add to right");
		StiDockingPanel stiDockingPanel = (StiDockingPanel)designerHost.CreateComponent(typeof(StiDockingPanel));
		((Control)(Form)designerHost.RootComponent).Controls.Add((Control)(object)stiDockingPanel);
		dockingManager.DockedPanels.Add(stiDockingPanel);
		stiDockingPanel.Manager = dockingManager;
		((Control)stiDockingPanel).Dock = (DockStyle)4;
		designerTransaction.Commit();
	}

	private void OnAddToTop(object sender, EventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add to top");
		StiDockingPanel stiDockingPanel = (StiDockingPanel)designerHost.CreateComponent(typeof(StiDockingPanel));
		((Control)(Form)designerHost.RootComponent).Controls.Add((Control)(object)stiDockingPanel);
		dockingManager.DockedPanels.Add(stiDockingPanel);
		stiDockingPanel.Manager = dockingManager;
		((Control)stiDockingPanel).Dock = (DockStyle)1;
		designerTransaction.Commit();
	}

	private void OnAddToBottom(object sender, EventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add to bottom");
		StiDockingPanel stiDockingPanel = (StiDockingPanel)designerHost.CreateComponent(typeof(StiDockingPanel));
		((Control)(Form)designerHost.RootComponent).Controls.Add((Control)(object)stiDockingPanel);
		dockingManager.DockedPanels.Add(stiDockingPanel);
		stiDockingPanel.Manager = dockingManager;
		((Control)stiDockingPanel).Dock = (DockStyle)2;
		designerTransaction.Commit();
	}

	public StiDockingManagerDesigner()
	{
		addToLeft = new DesignerVerb("Add to left", OnAddToLeft);
		addToRight = new DesignerVerb("Add to right", OnAddToRight);
		addToTop = new DesignerVerb("Add to top", OnAddToTop);
		addToBottom = new DesignerVerb("Add to bottom", OnAddToBottom);
	}
}
