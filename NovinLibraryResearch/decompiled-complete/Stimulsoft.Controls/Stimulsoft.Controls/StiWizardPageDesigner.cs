using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiWizardPageDesigner : ParentControlDesigner
{
	private StiWizardPage tabPage;

	public override SelectionRules SelectionRules => (SelectionRules)0;

	public override bool CanBeParentedTo(IDesigner parentDesigner)
	{
		return parentDesigner.Component is StiWizard;
	}

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		tabPage = component as StiWizardPage;
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (tabPage == null)
		{
			return;
		}
		Pen val = new Pen(SystemColors.ControlDark);
		try
		{
			val.DashStyle = (DashStyle)1;
			pe.Graphics.DrawRectangle(val, 2, 2, ((Control)tabPage).Width - 5, ((Control)tabPage).Height - 5);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
