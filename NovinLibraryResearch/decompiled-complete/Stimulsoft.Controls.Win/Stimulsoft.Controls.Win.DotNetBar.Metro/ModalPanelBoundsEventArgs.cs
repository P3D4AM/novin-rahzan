using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

public class ModalPanelBoundsEventArgs : EventArgs
{
	public Rectangle ModalPanelBounds = Rectangle.Empty;

	public ModalPanelBoundsEventArgs(Rectangle modalPanelBounds)
	{
		ModalPanelBounds = modalPanelBounds;
	}
}
