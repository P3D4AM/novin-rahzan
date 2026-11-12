using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class PageSliderPageDesigner : ScrollableControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[4]
			{
				new DesignerVerb("Move Forward", MovePageForward),
				new DesignerVerb("Move Backward", MovePageBackward),
				new DesignerVerb("Make First", MakeFirstPage),
				new DesignerVerb("Make Last", MakeLastPage)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public override SelectionRules SelectionRules => (SelectionRules)int.MinValue;

	private void MovePageTo(PageSliderPage page, PageSlider slider, int newIndex)
	{
		IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		((ComponentDesigner)this).GetService(typeof(ISelectionService));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Moving page");
		componentChangeService?.OnComponentChanging(slider, TypeDescriptor.GetProperties(slider)["Controls"]);
		((Control)slider).Controls.SetChildIndex((Control)(object)page, newIndex);
		componentChangeService?.OnComponentChanged(slider, TypeDescriptor.GetProperties(slider)["Controls"], null, null);
		designerTransaction.Commit();
	}

	private void MovePageForward(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PageSliderPage pageSliderPage && ((Control)pageSliderPage).Parent is PageSlider pageSlider)
		{
			int num = ((Control)pageSlider).Controls.IndexOf((Control)(object)pageSliderPage);
			num = ((num < pageSlider.PageCount - 1) ? (num + 1) : 0);
			MovePageTo(pageSliderPage, pageSlider, num);
		}
	}

	private void MovePageBackward(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PageSliderPage pageSliderPage && ((Control)pageSliderPage).Parent is PageSlider pageSlider)
		{
			int num = ((Control)pageSlider).Controls.IndexOf((Control)(object)pageSliderPage);
			num = ((num <= 0) ? (pageSlider.PageCount - 1) : (num - 1));
			MovePageTo(pageSliderPage, pageSlider, num);
		}
	}

	private void MakeFirstPage(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PageSliderPage pageSliderPage && ((Control)pageSliderPage).Parent is PageSlider slider)
		{
			MovePageTo(pageSliderPage, slider, 0);
		}
	}

	private void MakeLastPage(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PageSliderPage pageSliderPage && ((Control)pageSliderPage).Parent is PageSlider pageSlider)
		{
			MovePageTo(pageSliderPage, pageSlider, pageSlider.PageCount - 1);
		}
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		if (((ComponentDesigner)this).Component is PageSliderPage)
		{
			DrawBorder(pe.Graphics);
		}
		((ParentControlDesigner)this).OnPaintAdornments(pe);
	}

	private void DrawBorder(Graphics g)
	{
		_ = ((ControlDesigner)this).Control;
		Color controlDarkDark = SystemColors.ControlDarkDark;
		Rectangle clientRectangle = ((ControlDesigner)this).Control.ClientRectangle;
		Color empty = Color.Empty;
		Helpers.DrawDesignTimeSelection(g, clientRectangle, empty, controlDarkDark, 1);
	}
}
