using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Controls.Win.DotNetBar.Ribbon;

[ToolboxItem(false)]
public class RibbonClientPanel : PanelControl
{
	private bool m_IsShadowEnabled = true;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether panel automatically provides shadows for child controls.")]
	public bool IsShadowEnabled
	{
		get
		{
			return m_IsShadowEnabled;
		}
		set
		{
			if (m_IsShadowEnabled != value)
			{
				m_IsShadowEnabled = value;
				((Control)this).Invalidate();
			}
		}
	}

	protected override void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		base.PaintInnerContent(e, style, paintText);
		if (!m_IsShadowEnabled)
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
					shadowPaintInfo.Rectangle = new Rectangle(val.Bounds.X, val.Bounds.Y + groupPanel.GetInternalClientRectangle().Y / 2, val.Bounds.Width, val.Bounds.Height - groupPanel.GetInternalClientRectangle().Y / 2);
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
