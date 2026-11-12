using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls;

internal class StiToolButtonDesigner : ParentControlDesigner
{
	private StiToolButton toolButton;

	private DesignerVerb moveLeft;

	private DesignerVerb moveRight;

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new DesignerVerb[2] { moveLeft, moveRight });

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		toolButton = component as StiToolButton;
	}

	private void OnMoveRight(object sender, EventArgs e)
	{
		int childIndex = ((Control)toolButton).Parent.Controls.GetChildIndex((Control)(object)toolButton);
		if (childIndex > 0)
		{
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Move Left");
			componentChangeService.OnComponentChanging(((Control)toolButton).Parent, TypeDescriptor.GetProperties(((Control)toolButton).Parent)["Controls"]);
			((Control)toolButton).Parent.Controls.SetChildIndex((Control)(object)toolButton, childIndex - 1);
			componentChangeService.OnComponentChanged(((Control)toolButton).Parent, TypeDescriptor.GetProperties(((Control)toolButton).Parent)["Controls"], null, null);
			designerTransaction.Commit();
			((Control)toolButton).Parent.Invalidate();
		}
	}

	private void OnMoveLeft(object sender, EventArgs e)
	{
		int childIndex = ((Control)toolButton).Parent.Controls.GetChildIndex((Control)(object)toolButton);
		if (childIndex < ((ArrangedElementCollection)((Control)toolButton).Parent.Controls).Count - 1)
		{
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Move Right");
			componentChangeService.OnComponentChanging(((Control)toolButton).Parent, TypeDescriptor.GetProperties(((Control)toolButton).Parent)["Controls"]);
			((Control)toolButton).Parent.Controls.SetChildIndex((Control)(object)toolButton, childIndex + 1);
			componentChangeService.OnComponentChanged(((Control)toolButton).Parent, TypeDescriptor.GetProperties(((Control)toolButton).Parent)["Controls"], null, null);
			designerTransaction.Commit();
			((Control)toolButton).Parent.Invalidate();
		}
	}

	public StiToolButtonDesigner()
	{
		moveLeft = new DesignerVerb("Move Left", OnMoveLeft);
		moveRight = new DesignerVerb("Move Right", OnMoveRight);
	}
}
