using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiOutlookPanelDesigner : ParentControlDesigner
{
	private DesignerVerb expand;

	private DesignerVerb collapse;

	private StiOutlookPanel outlookPanel;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[2] { expand, collapse });

	protected override bool DrawGrid => false;

	private void OnAddToolButton(object sender, EventArgs e)
	{
		StiToolButton stiToolButton = new StiToolButton();
		((Control)stiToolButton).Dock = (DockStyle)1;
		((Control)outlookPanel).Controls.Add((Control)(object)stiToolButton);
		((Control)outlookPanel).Invalidate();
	}

	private void OnExpand(object sender, EventArgs e)
	{
		outlookPanel.Collapsed = true;
		((Control)outlookPanel).Invalidate();
	}

	private void OnCollapse(object sender, EventArgs e)
	{
		outlookPanel.Collapsed = false;
		((Control)outlookPanel).Invalidate();
	}

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		outlookPanel = (StiOutlookPanel)(object)((ControlDesigner)this).Control;
	}

	public StiOutlookPanelDesigner()
	{
		expand = new DesignerVerb("Expand", OnExpand);
		collapse = new DesignerVerb("Collapse", OnCollapse);
	}
}
