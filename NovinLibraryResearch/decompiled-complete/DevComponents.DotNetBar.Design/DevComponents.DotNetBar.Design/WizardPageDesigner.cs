using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class WizardPageDesigner : ScrollableControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[7]
			{
				new DesignerVerb("Create Inner Page", CreateInnerPage),
				new DesignerVerb("Create Welcome Page", CreateWelcomePage),
				new DesignerVerb("Create Outer Page", CreateOuterPage),
				new DesignerVerb("Delete Page", DeletePage),
				new DesignerVerb("Next Page", NextPage),
				new DesignerVerb("Previous Page", PreviousPage),
				new DesignerVerb("Goto Page/Change Order", GotoPage)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public override SelectionRules SelectionRules => (SelectionRules)int.MinValue;

	private Wizard GetWizard()
	{
		WizardPage wizardPage = ((ControlDesigner)this).Control as WizardPage;
		return ((Control)wizardPage).Parent as Wizard;
	}

	private void GotoPage(object sender, EventArgs e)
	{
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		WizardDesigner.GotoPage(GetWizard(), cc, ss);
	}

	private void CreateInnerPage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		Wizard wizard = GetWizard();
		WizardDesigner.CreatePage(wizard, innerPage: true, dh, cc, ss, wizard.ButtonStyle);
	}

	private void CreateWelcomePage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		Wizard wizard = GetWizard();
		WizardDesigner.CreateWelcomePage(wizard, dh, cc, ss, wizard.ButtonStyle);
	}

	private void CreateOuterPage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		Wizard wizard = GetWizard();
		WizardDesigner.CreatePage(wizard, innerPage: false, dh, cc, ss, wizard.ButtonStyle);
	}

	private void DeletePage(object sender, EventArgs e)
	{
		WizardPage page = ((ControlDesigner)this).Control as WizardPage;
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		WizardDesigner.DeletePage(page, dh, cc);
	}

	private void NextPage(object sender, EventArgs e)
	{
		WizardDesigner.SelectNextPage(GetWizard());
	}

	private void PreviousPage(object sender, EventArgs e)
	{
		WizardDesigner.SelectPreviousPage(GetWizard());
	}
}
