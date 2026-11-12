using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Design;

public class MetroTabPanelDesigner : PanelControlDesigner
{
	public override SelectionRules SelectionRules => (SelectionRules)(-1073741824);

	public override DesignerVerbCollection Verbs => new DesignerVerbCollection();

	protected override void SetDesignTimeDefaults()
	{
	}

	protected override void DrawBorder(Graphics g)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (!(((ControlDesigner)this).Control is MetroTabPanel metroTabPanel))
		{
			return;
		}
		Rectangle clientRectangle = ((Control)metroTabPanel).ClientRectangle;
		Pen val = new Pen(Color.WhiteSmoke, 1f);
		try
		{
			val.DashStyle = (DashStyle)1;
			clientRectangle.Width--;
			clientRectangle.Height--;
			g.DrawRectangle(val, clientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
