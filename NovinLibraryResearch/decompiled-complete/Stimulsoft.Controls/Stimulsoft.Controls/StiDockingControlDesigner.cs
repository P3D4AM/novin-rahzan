using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls;

internal class StiDockingControlDesigner : ParentControlDesigner
{
	private StiDockingControl dockingControl;

	public override SelectionRules SelectionRules => (SelectionRules)0;

	public override bool CanBeParentedTo(IDesigner parentDesigner)
	{
		return parentDesigner.Component is StiDockingPanel;
	}

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		dockingControl = component as StiDockingControl;
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		Pen val = new Pen(SystemColors.ControlDark);
		try
		{
			val.DashStyle = (DashStyle)1;
			pe.Graphics.DrawRectangle(val, 0, 0, ((Control)dockingControl).Width - 1, ((Control)dockingControl).Height - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
