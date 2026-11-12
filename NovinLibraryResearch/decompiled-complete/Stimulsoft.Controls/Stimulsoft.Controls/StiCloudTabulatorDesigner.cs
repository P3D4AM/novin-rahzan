using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiCloudTabulatorDesigner : ParentControlDesigner
{
	private DesignerVerb addControl;

	private StiCloudTabulator tabControl;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[1] { addControl });

	protected override bool DrawGrid => false;

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		tabControl = (StiCloudTabulator)(object)((ControlDesigner)this).Control;
	}

	private void OnAddControl(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add Page");
		StiCloudTabulatorPage stiCloudTabulatorPage = (StiCloudTabulatorPage)(object)designerHost.CreateComponent(typeof(StiCloudTabulatorPage));
		((Control)tabControl).Controls.Add((Control)(object)stiCloudTabulatorPage);
		stiCloudTabulatorPage.Dock = (DockStyle)5;
		((Control)tabControl).Invalidate();
		designerTransaction.Commit();
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 513)
		{
			Point p = ((Control)tabControl).PointToClient(Cursor.Position);
			StiCloudTabulatorPage tabPageAtPoint = tabControl.GetTabPageAtPoint(p);
			if (tabPageAtPoint != null)
			{
				tabControl.SelectedTab = tabPageAtPoint;
				ArrayList selectedComponents = new ArrayList { tabPageAtPoint };
				ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
				selectionService.SetSelectedComponents(selectedComponents);
			}
		}
		if (((Message)(ref msg)).Msg == 512)
		{
			((Control)tabControl).Invalidate();
		}
		if (((Message)(ref msg)).Msg == 675)
		{
			((Control)tabControl).Invalidate();
		}
		((ControlDesigner)this).WndProc(ref msg);
	}

	protected override bool GetHitTest(Point point)
	{
		return tabControl.GetTabPageAtPoint(((Control)tabControl).PointToClient(point)) != null;
	}

	public StiCloudTabulatorDesigner()
	{
		addControl = new DesignerVerb("Add Page", OnAddControl);
	}
}
