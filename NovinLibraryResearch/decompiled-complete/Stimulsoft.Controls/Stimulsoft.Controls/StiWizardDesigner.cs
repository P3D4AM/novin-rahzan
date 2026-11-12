using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiWizardDesigner : ParentControlDesigner
{
	private DesignerVerb addControl;

	private StiWizard wizardControl;

	private const int WM_LBUTTONDOWN = 513;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[1] { addControl });

	protected override bool DrawGrid => false;

	public override bool CanParent(Control control)
	{
		return control is StiWizardPage;
	}

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		wizardControl = (StiWizard)(object)((ControlDesigner)this).Control;
	}

	private void OnAddControl(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Add Page");
		StiWizardPage stiWizardPage = (StiWizardPage)designerHost.CreateComponent(typeof(StiWizardPage));
		((Control)wizardControl).Controls.Add((Control)(object)stiWizardPage);
		stiWizardPage.Dock = (DockStyle)5;
		((Control)wizardControl).Invalidate();
		designerTransaction.Commit();
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 513)
		{
			Point pos = ((Control)wizardControl).PointToClient(Cursor.Position);
			StiWizardPage wizardPageAtPoint = wizardControl.GetWizardPageAtPoint(pos);
			if (wizardPageAtPoint != null)
			{
				wizardControl.SelectedPage = wizardPageAtPoint;
				ArrayList arrayList = new ArrayList();
				arrayList.Add(wizardPageAtPoint);
				ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
				selectionService.SetSelectedComponents(arrayList);
				return;
			}
		}
		((ControlDesigner)this).WndProc(ref msg);
	}

	protected override bool GetHitTest(Point point)
	{
		StiWizardPage wizardPageAtPoint = wizardControl.GetWizardPageAtPoint(((Control)wizardControl).PointToClient(point));
		return wizardPageAtPoint != null;
	}

	public StiWizardDesigner()
	{
		addControl = new DesignerVerb("Add Page", OnAddControl);
	}
}
