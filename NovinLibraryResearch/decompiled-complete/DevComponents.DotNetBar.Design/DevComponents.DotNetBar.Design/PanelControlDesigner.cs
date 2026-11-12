using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class PanelControlDesigner : ScrollableControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[3]
			{
				new DesignerVerb("Apply Panel Style", ApplyPanelStyle),
				new DesignerVerb("Apply Button Style", ApplyButtonStyle),
				new DesignerVerb("Apply Label Style", ApplyLabelStyle)
			};
			return new DesignerVerbCollection(value);
		}
	}

	private void ApplyPanelStyle(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PanelControl panelControl)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			panelControl.ApplyPanelStyle();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
		}
	}

	private void ApplyButtonStyle(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PanelControl panelControl)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			panelControl.ApplyButtonStyle();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
		}
	}

	private void ApplyLabelStyle(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PanelControl panelControl)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			panelControl.ApplyLabelStyle();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	protected virtual void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is PanelControl panelControl)
		{
			panelControl.ApplyPanelStyle();
			panelControl.CanvasColor = SystemColors.Control;
		}
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		if (((ComponentDesigner)this).Component is PanelControl panelControl && panelControl.Style.Border == eStyleBorderType.None)
		{
			DrawBorder(pe.Graphics);
		}
		((ParentControlDesigner)this).OnPaintAdornments(pe);
	}

	protected virtual void DrawBorder(Graphics g)
	{
		PanelControl panelControl = ((ControlDesigner)this).Control as PanelControl;
		Color controlDarkDark = SystemColors.ControlDarkDark;
		Rectangle clientRectangle = ((ControlDesigner)this).Control.ClientRectangle;
		Color backColor = panelControl.Style.BackColor;
		Helpers.DrawDesignTimeSelection(g, clientRectangle, backColor, controlDarkDark, 1);
	}
}
