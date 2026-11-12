using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualCheckBox : VisualToggleButton
{
	private Size _DefaultSize = new Size(13, 13);

	public override void PerformLayout(PaintInfo pi)
	{
		base.Size = new Size(_DefaultSize.Width + 2, _DefaultSize.Height);
		base.CalculatedSize = base.Size;
		base.PerformLayout(pi);
	}

	protected override void OnPaint(PaintInfo p)
	{
		Graphics graphics = p.Graphics;
		Rectangle checkBoxPosition = new Rectangle(base.RenderBounds.X + 1, base.RenderBounds.Y + (base.RenderBounds.Height - _DefaultSize.Height) / 2, _DefaultSize.Width, _DefaultSize.Height);
		Office2007CheckBoxItemPainter office2007CheckBoxItemPainter = PainterFactory.CreateCheckBoxItemPainter(null);
		Office2007CheckBoxColorTable checkBoxColorTable = GetCheckBoxColorTable();
		Office2007CheckBoxStateColorTable ct = checkBoxColorTable.Default;
		if (!GetIsEnabled(p))
		{
			ct = checkBoxColorTable.Disabled;
		}
		else if (base.IsMouseDown)
		{
			ct = checkBoxColorTable.Pressed;
		}
		else if (base.IsMouseOver)
		{
			ct = checkBoxColorTable.MouseOver;
		}
		office2007CheckBoxItemPainter.PaintCheckBox(graphics, checkBoxPosition, ct, (CheckState)(Checked ? 1 : 0));
		base.OnPaint(p);
	}

	private Office2007CheckBoxColorTable GetCheckBoxColorTable()
	{
		if (GetRenderer() is Office2007Renderer office2007Renderer)
		{
			return office2007Renderer.ColorTable.CheckBoxItem;
		}
		return null;
	}

	private BaseRenderer GetRenderer()
	{
		return GlobalManager.Renderer;
	}
}
