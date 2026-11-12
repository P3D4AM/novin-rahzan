using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiTabControlDesigner : ParentControlDesigner
{
	private DesignerVerb addControl;

	private StiTabControl tabControl;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[1] { addControl });

	protected override bool DrawGrid => false;

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		tabControl = (StiTabControl)(object)((ControlDesigner)this).Control;
	}

	private void OnAddControl(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add Page");
		StiTabPage stiTabPage = (StiTabPage)designerHost.CreateComponent(typeof(StiTabPage));
		((Control)tabControl).Controls.Add((Control)(object)stiTabPage);
		stiTabPage.Dock = (DockStyle)5;
		((Control)tabControl).Invalidate();
		designerTransaction.Commit();
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 513)
		{
			Point p = ((Control)tabControl).PointToClient(Cursor.Position);
			StiTabPage tabPageAtPoint = tabControl.GetTabPageAtPoint(p);
			if (tabPageAtPoint != null)
			{
				tabControl.SelectedTab = tabPageAtPoint;
				ArrayList arrayList = new ArrayList();
				arrayList.Add(tabPageAtPoint);
				ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
				selectionService.SetSelectedComponents(arrayList);
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
		Rectangle rectangle = ((!tabControl.PositionAtBottom) ? new Rectangle(0, 0, ((Control)tabControl).ClientRectangle.Width, 22) : new Rectangle(0, ((Control)tabControl).ClientRectangle.Height - 22, ((Control)tabControl).ClientRectangle.Width, 22));
		StiTabPage tabPageAtPoint = tabControl.GetTabPageAtPoint(((Control)tabControl).PointToClient(point));
		return tabPageAtPoint != null;
	}

	public StiTabControlDesigner()
	{
		addControl = new DesignerVerb("Add Page", OnAddControl);
	}
}
