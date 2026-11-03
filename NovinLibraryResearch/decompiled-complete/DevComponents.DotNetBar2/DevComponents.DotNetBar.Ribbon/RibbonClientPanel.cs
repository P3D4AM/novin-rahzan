using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Ribbon;

[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.RibbonClientPanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class RibbonClientPanel : PanelControl
{
	private bool _28A0 = true;

	[DefaultValue(true)]
	[Description("Indicates whether panel automatically provides shadows for child controls.")]
	[Browsable(true)]
	[Category("Appearance")]
	public bool IsShadowEnabled
	{
		get
		{
			return _28A0;
		}
		set
		{
			if (_28A0 != value)
			{
				_28A0 = value;
				((Control)this).Invalidate();
			}
		}
	}

	protected override void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		base.PaintInnerContent(e, style, paintText);
		if (!_28A0)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		ShadowPaintInfo shadowPaintInfo = new ShadowPaintInfo();
		shadowPaintInfo.Graphics = graphics;
		shadowPaintInfo.Size = 6;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val.Visible && (!(val.BackColor == Color.Transparent) || val is GroupPanel))
			{
				if (val is GroupPanel)
				{
					GroupPanel groupPanel = val as GroupPanel;
					shadowPaintInfo.Rectangle = new Rectangle(val.Bounds.X, val.Bounds.Y + groupPanel._28A9().Y / 2, val.Bounds.Width, val.Bounds.Height - groupPanel._28A9().Y / 2);
				}
				else
				{
					shadowPaintInfo.Rectangle = val.Bounds;
				}
				ShadowPainter.Paint2(shadowPaintInfo);
			}
		}
	}
}
