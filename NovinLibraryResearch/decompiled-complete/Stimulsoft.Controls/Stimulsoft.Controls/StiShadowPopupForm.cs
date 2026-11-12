using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

public class StiShadowPopupForm : StiPopupBaseForm
{
	protected override bool Layered => false;

	protected override bool HookMouseButtonsMessages => false;

	protected override void OnPaint(PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		StiShadow.DrawShadow(graphics, clientRectangle);
	}

	public StiShadowPopupForm()
		: this(null)
	{
	}

	public StiShadowPopupForm(Control parentControl)
		: base(parentControl)
	{
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).BackColor = Color.Transparent;
	}
}
