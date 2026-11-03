using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.NavigationPanePanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class NavigationPanePanel : PanelEx
{
	private const string ᑙ = "Drop controls on this panel to associate them with current selection";

	private BaseItem _0AF1;

	[DevCoBrowsable(false)]
	[Browsable(false)]
	public BaseItem ParentItem
	{
		get
		{
			return _0AF1;
		}
		set
		{
			_0AF1 = value;
			if (_0AF1 != null && ((Control)this).Parent is NavigationPane && _0AF1 is ButtonItem && ((ButtonItem)_0AF1).Checked)
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
