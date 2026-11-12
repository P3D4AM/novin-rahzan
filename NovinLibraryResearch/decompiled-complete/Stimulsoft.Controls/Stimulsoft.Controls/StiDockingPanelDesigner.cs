using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiDockingPanelDesigner : ParentControlDesigner
{
	private DesignerVerb addControl;

	private StiDockingPanel dockingPanel;

	protected override bool DrawGrid => false;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[1] { addControl });

	public override bool CanBeParentedTo(IDesigner parentDesigner)
	{
		return parentDesigner.Component is Form;
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 513)
		{
			Point point = ((Control)dockingPanel).PointToClient(Cursor.Position);
			StiDockingControl dockingControlAt = dockingPanel.GetDockingControlAt(point.X, point.Y);
			if (dockingControlAt != null)
			{
				dockingPanel.SelectedControl = dockingControlAt;
				ArrayList arrayList = new ArrayList();
				arrayList.Add(dockingControlAt);
				ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
				selectionService.SetSelectedComponents(arrayList);
				dockingPanel.IsSelected = true;
				((Control)dockingPanel).Invalidate();
			}
		}
		if (((Message)(ref msg)).Msg == 512)
		{
			Point point2 = ((Control)dockingPanel).PointToClient(Cursor.Position);
			if (dockingPanel.TabsBounds.Contains(point2.X, point2.Y))
			{
				dockingPanel.InvalidateTabs();
			}
		}
		if (((Message)(ref msg)).Msg == 675)
		{
			dockingPanel.InvalidateTabs();
		}
		((ControlDesigner)this).WndProc(ref msg);
	}

	protected override bool GetHitTest(Point point)
	{
		Rectangle rectangle = new Rectangle(0, ((Control)dockingPanel).ClientRectangle.Height - 22, ((Control)dockingPanel).ClientRectangle.Width, 22);
		return rectangle.Contains(((Control)dockingPanel).PointToClient(point));
	}

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		dockingPanel = (StiDockingPanel)component;
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		selectionService.SelectionChanged += OnSelectionChanged;
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
	}

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		ICollection selectedComponents = selectionService.GetSelectedComponents();
		foreach (Control item in selectedComponents)
		{
			Control val = item;
			if (val == dockingPanel)
			{
				dockingPanel.IsSelected = true;
				((Control)dockingPanel).Invalidate();
				return;
			}
		}
		dockingPanel.IsSelected = false;
		((Control)dockingPanel).Invalidate();
	}

	private void OnAddControl(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add Control");
		StiDockingControl stiDockingControl = (StiDockingControl)designerHost.CreateComponent(typeof(StiDockingControl));
		((Control)dockingPanel).Controls.Add((Control)(object)stiDockingControl);
		((Control)stiDockingControl).Dock = (DockStyle)5;
		dockingPanel.SelectedControl = stiDockingControl;
		designerTransaction.Commit();
	}

	public StiDockingPanelDesigner()
	{
		addControl = new DesignerVerb("Add Control", OnAddControl);
	}
}
