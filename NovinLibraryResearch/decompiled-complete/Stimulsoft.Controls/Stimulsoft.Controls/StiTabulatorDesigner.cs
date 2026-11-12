using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiTabulatorDesigner : ParentControlDesigner
{
	private DesignerVerb addControl;

	private StiTabulator tabControl;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[1] { addControl });

	protected override bool DrawGrid => false;

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		tabControl = (StiTabulator)(object)((ControlDesigner)this).Control;
	}

	private void OnAddControl(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add Page");
		StiTabulatorPage stiTabulatorPage = (StiTabulatorPage)designerHost.CreateComponent(typeof(StiTabulatorPage));
		((Control)tabControl).Controls.Add((Control)(object)stiTabulatorPage);
		stiTabulatorPage.Dock = (DockStyle)5;
		((Control)tabControl).Invalidate();
		designerTransaction.Commit();
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 513)
		{
			Point p = ((Control)tabControl).PointToClient(Cursor.Position);
			StiTabulatorPage tabPageAtPoint = tabControl.GetTabPageAtPoint(p);
			if (tabPageAtPoint != null)
			{
				tabControl.SelectedTab = tabPageAtPoint;
				ArrayList arrayList = new ArrayList();
				arrayList.Add(tabPageAtPoint);
				ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
				selectionService.SetSelectedComponents(arrayList);
			}
		}
		((ControlDesigner)this).WndProc(ref msg);
	}

	protected override bool GetHitTest(Point point)
	{
		StiTabulatorPage tabPageAtPoint = tabControl.GetTabPageAtPoint(((Control)tabControl).PointToClient(point));
		return tabPageAtPoint != null;
	}

	public StiTabulatorDesigner()
	{
		addControl = new DesignerVerb("Add Page", OnAddControl);
	}
}
