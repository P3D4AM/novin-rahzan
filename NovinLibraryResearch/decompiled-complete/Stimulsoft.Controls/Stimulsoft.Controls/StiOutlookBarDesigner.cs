using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiOutlookBarDesigner : ParentControlDesigner
{
	private DesignerVerb addPanel;

	private StiOutlookBar outlookBar;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[1] { addPanel });

	protected override bool DrawGrid => false;

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		outlookBar = (StiOutlookBar)(object)((ControlDesigner)this).Control;
	}

	private void OnAddPanel(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add Panel");
		StiOutlookPanel stiOutlookPanel = (StiOutlookPanel)designerHost.CreateComponent(typeof(StiOutlookPanel));
		((Control)outlookBar).Controls.Add((Control)(object)stiOutlookPanel);
		((Control)outlookBar).Controls.SetChildIndex((Control)(object)stiOutlookPanel, 0);
		((Control)stiOutlookPanel).Dock = (DockStyle)1;
		designerTransaction.Commit();
	}

	public StiOutlookBarDesigner()
	{
		addPanel = new DesignerVerb("&Add Panel", OnAddPanel);
	}
}
