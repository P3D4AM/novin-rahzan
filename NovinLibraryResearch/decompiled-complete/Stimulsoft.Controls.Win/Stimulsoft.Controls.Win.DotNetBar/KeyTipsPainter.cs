using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public abstract class KeyTipsPainter
{
	public static Size KeyTipsPadding = new Size(6, 4);

	public abstract void PaintKeyTips(KeyTipsRendererEventArgs e);
}
