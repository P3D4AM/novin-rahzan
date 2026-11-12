using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class PanelExDesigner : ScrollableControlDesigner
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
		if (((ControlDesigner)this).Control is PanelEx panelEx)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			panelEx.ApplyPanelStyle();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
		}
	}

	private void ApplyButtonStyle(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PanelEx panelEx)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			panelEx.ApplyButtonStyle();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
		}
	}

	private void ApplyLabelStyle(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is PanelEx panelEx)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			panelEx.ApplyLabelStyle();
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
		if (((ControlDesigner)this).Control is PanelEx panelEx)
		{
			panelEx.ApplyPanelStyle();
			panelEx.CanvasColor = SystemColors.Control;
			panelEx.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		}
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		if (((ComponentDesigner)this).Component is PanelEx panelEx && panelEx.Style.Border == eBorderType.None)
		{
			DrawBorder(pe.Graphics);
		}
		((ParentControlDesigner)this).OnPaintAdornments(pe);
	}

	private void DrawBorder(Graphics g)
	{
		PanelEx panelEx = ((ControlDesigner)this).Control as PanelEx;
		Color controlDarkDark = SystemColors.ControlDarkDark;
		Rectangle clientRectangle = ((ControlDesigner)this).Control.ClientRectangle;
		Color color = panelEx.Style.BackColor1.Color;
		Helpers.DrawDesignTimeSelection(g, clientRectangle, color, controlDarkDark, 1);
	}
}
