using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiToolBarDesigner : ParentControlDesigner
{
	private DesignerVerb addToolButton;

	private DesignerVerb addSeparator;

	private StiToolBar toolBar;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[2] { addToolButton, addSeparator });

	protected override bool DrawGrid => false;

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		toolBar = (StiToolBar)(object)((ControlDesigner)this).Control;
	}

	private void OnAddToolButton(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add ToolButton");
		StiToolButton stiToolButton = (StiToolButton)designerHost.CreateComponent(typeof(StiToolButton));
		((Control)stiToolButton).Dock = (DockStyle)3;
		((Control)toolBar).Controls.Add((Control)(object)stiToolButton);
		((Control)toolBar).Controls.SetChildIndex((Control)(object)stiToolButton, 0);
		designerTransaction.Commit();
	}

	private void OnAddSeparator(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add Separator");
		StiToolButton stiToolButton = (StiToolButton)designerHost.CreateComponent(typeof(StiToolButton));
		((Control)stiToolButton).Dock = (DockStyle)3;
		stiToolButton.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton).Width = 8;
		((Control)toolBar).Controls.Add((Control)(object)stiToolButton);
		((Control)toolBar).Controls.SetChildIndex((Control)(object)stiToolButton, 0);
		designerTransaction.Commit();
	}

	public StiToolBarDesigner()
	{
		addToolButton = new DesignerVerb("Add ToolButton", OnAddToolButton);
		addSeparator = new DesignerVerb("Add Separator", OnAddSeparator);
	}
}
