using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class GroupPanelDesigner : ScrollableControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[1]
			{
				new DesignerVerb("Reset Style", ResetStyle)
			};
			return new DesignerVerbCollection(value);
		}
	}

	private void ResetStyle(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is GroupPanel groupPanel)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			groupPanel.SetDefaultPanelStyle();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is GroupPanel groupPanel)
		{
			groupPanel.SetDefaultPanelStyle();
		}
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		PanelControl panelControl = ((ComponentDesigner)this).Component as PanelControl;
		if (panelControl.Style.Border == eStyleBorderType.None)
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

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132 && ((ControlDesigner)this).Control is GroupPanel groupPanel)
		{
			int x = WinApi.LOWORD(((Message)(ref m)).LParam);
			int y = WinApi.HIWORD(((Message)(ref m)).LParam);
			if (((Control)groupPanel).PointToClient(new Point(x, y)).Y < 0)
			{
				((Message)(ref m)).Result = new IntPtr(1);
				return;
			}
		}
		((ScrollableControlDesigner)this).WndProc(ref m);
	}
}
