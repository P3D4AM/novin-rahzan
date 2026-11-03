using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors;

public class VisualCheckBox : VisualToggleButton
{
	private Size _ᢌ = new Size(13, 13);

	public override void PerformLayout(PaintInfo pi)
	{
		base.Size = new Size(Dpi.Width(_ᢌ.Width + 2), Dpi.Height(_ᢌ.Height));
		base.CalculatedSize = base.Size;
		base.PerformLayout(pi);
	}

	protected override void OnPaint(PaintInfo p)
	{
		Graphics graphics = p.Graphics;
		Size size = Dpi.Size(_ᢌ);
		Rectangle ᢑ = new Rectangle(base.RenderBounds.X + 1, base.RenderBounds.Y + (base.RenderBounds.Height - size.Height) / 2, size.Width, size.Height);
		ᢏ ᢏ = ᣓ._191F(null);
		Office2007CheckBoxColorTable office2007CheckBoxColorTable = ᢍ();
		Office2007CheckBoxStateColorTable ዅ = office2007CheckBoxColorTable.Default;
		if (!GetIsEnabled(p))
		{
			ዅ = office2007CheckBoxColorTable.Disabled;
		}
		else if (base.IsMouseDown)
		{
			ዅ = office2007CheckBoxColorTable.Pressed;
		}
		else if (base.IsMouseOver)
		{
			ዅ = office2007CheckBoxColorTable.MouseOver;
		}
		ᢏ.ᢓ(graphics, ᢑ, ዅ, (CheckState)(Checked ? 1 : 0));
		base.OnPaint(p);
	}

	private Office2007CheckBoxColorTable ᢍ()
	{
		if (ဣ() is Office2007Renderer office2007Renderer)
		{
			return office2007Renderer.ColorTable.CheckBoxItem;
		}
		return null;
	}

	private BaseRenderer ဣ()
	{
		return GlobalManager.Renderer;
	}
}
