using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class QatCustomizeItemRendererEventArgs : EventArgs
{
	public QatCustomizeItem CustomizeItem;

	public Graphics Graphics;

	public QatCustomizeItemRendererEventArgs(QatCustomizeItem customizeItem, Graphics g)
	{
		CustomizeItem = customizeItem;
		Graphics = g;
	}
}
