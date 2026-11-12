using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Controls;

public class StiCustomItem : ButtonItem
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsOnGallery { get; set; }

	protected override void Invalidate()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			val.Invalidate(DisplayRectangle);
		}
	}

	public override void Paint(ItemPaintArgs e)
	{
		if (e.IsOnMenu || IsOnGallery)
		{
			PaintItem(e);
		}
		else
		{
			base.Paint(e);
		}
	}

	private void PaintItem(ItemPaintArgs e)
	{
		ButtonItemPainter buttonItemPainter = PainterFactory.CreateButtonPainter(this);
		Rectangle mouseOverRectangle = buttonItemPainter.GetMouseOverRectangle(this, e, null);
		if (Checked && buttonItemPainter is Office2003ButtonItemPainter office2003ButtonItemPainter)
		{
			office2003ButtonItemPainter.PaintButtonCheckBackground(this, e, mouseOverRectangle);
		}
		if (base.IsMouseOver)
		{
			buttonItemPainter.PaintButtonMouseOver(this, e, null, mouseOverRectangle);
		}
	}
}
