using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls;

internal class StiTabPageDesigner : ParentControlDesigner
{
	private StiTabPage tabPage;

	private DesignerVerb moveLeft;

	private DesignerVerb moveRight;

	public override SelectionRules SelectionRules => (SelectionRules)0;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[2] { moveLeft, moveRight });

	public override bool CanBeParentedTo(IDesigner parentDesigner)
	{
		return parentDesigner.Component is StiTabControl;
	}

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		tabPage = component as StiTabPage;
	}

	private void OnMoveLeft(object sender, EventArgs e)
	{
		int childIndex = ((Control)tabPage).Parent.Controls.GetChildIndex((Control)(object)tabPage);
		if (childIndex > 0)
		{
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Move Left");
			componentChangeService.OnComponentChanging(((Control)tabPage).Parent, TypeDescriptor.GetProperties(((Control)tabPage).Parent)["Controls"]);
			((Control)tabPage).Parent.Controls.SetChildIndex((Control)(object)tabPage, childIndex - 1);
			componentChangeService.OnComponentChanged(((Control)tabPage).Parent, TypeDescriptor.GetProperties(((Control)tabPage).Parent)["Controls"], null, null);
			designerTransaction.Commit();
			((Control)tabPage).Parent.Invalidate();
		}
	}

	private void OnMoveRight(object sender, EventArgs e)
	{
		int childIndex = ((Control)tabPage).Parent.Controls.GetChildIndex((Control)(object)tabPage);
		if (childIndex < ((ArrangedElementCollection)((Control)tabPage).Parent.Controls).Count - 1)
		{
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Move Right");
			componentChangeService.OnComponentChanging(((Control)tabPage).Parent, TypeDescriptor.GetProperties(((Control)tabPage).Parent)["Controls"]);
			((Control)tabPage).Parent.Controls.SetChildIndex((Control)(object)tabPage, childIndex + 1);
			componentChangeService.OnComponentChanged(((Control)tabPage).Parent, TypeDescriptor.GetProperties(((Control)tabPage).Parent)["Controls"], null, null);
			designerTransaction.Commit();
			((Control)tabPage).Parent.Invalidate();
		}
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (tabPage != null)
		{
			Pen val = new Pen(SystemColors.ControlDark);
			try
			{
				val.DashStyle = (DashStyle)1;
				pe.Graphics.DrawRectangle(val, 0, 0, ((Control)tabPage).Width - 1, ((Control)tabPage).Height - 1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public StiTabPageDesigner()
	{
		moveLeft = new DesignerVerb("Move Left", OnMoveLeft);
		moveRight = new DesignerVerb("Move Right", OnMoveRight);
	}
}
