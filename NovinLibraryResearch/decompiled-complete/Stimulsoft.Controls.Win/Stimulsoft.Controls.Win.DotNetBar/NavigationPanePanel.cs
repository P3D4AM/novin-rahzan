using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class NavigationPanePanel : PanelEx
{
	private BaseItem m_ParentItem;

	private const string INFO_TEXT = "Drop controls on this panel to associate them with current selection";

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public BaseItem ParentItem
	{
		get
		{
			return m_ParentItem;
		}
		set
		{
			m_ParentItem = value;
			if (m_ParentItem != null && ((Control)this).Parent is NavigationPane && m_ParentItem is ButtonItem && ((ButtonItem)m_ParentItem).Checked)
			{
				((Control)this).Visible = true;
				((Control)this).BringToFront();
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		base.OnPaint(e);
		if (((Component)(object)this).DesignMode && ((ArrangedElementCollection)((Control)this).Controls).Count == 0 && ((Control)this).Text == "")
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
			Font val = new Font(((Control)this).Font, (FontStyle)1);
			TextDrawing.DrawString(e.Graphics, "Drop controls on this panel to associate them with current selection", val, ControlPaint.Dark(base.Style.BackColor1.Color), clientRectangle, format);
			val.Dispose();
		}
	}
}
