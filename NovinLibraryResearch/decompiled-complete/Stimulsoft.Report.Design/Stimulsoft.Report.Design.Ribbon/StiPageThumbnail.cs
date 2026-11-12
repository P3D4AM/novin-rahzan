using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Ribbon;

[ToolboxItem(false)]
public class StiPageThumbnail : Control
{
	public Image Image { get; set; }

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width - 2, ((Control)this).Height - 2);
		ShadowPaintInfo info = new ShadowPaintInfo
		{
			Graphics = e.Graphics,
			Rectangle = rectangle,
			Size = 2
		};
		ShadowPainter.Paint(info);
		e.Graphics.DrawImage(Image, new Rectangle(0, 0, Image.Width, Image.Height));
	}
}
