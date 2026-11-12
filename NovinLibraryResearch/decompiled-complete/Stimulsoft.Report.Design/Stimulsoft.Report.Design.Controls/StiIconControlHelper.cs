using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Design.Controls;

public static class StiIconControlHelper
{
	public static void PanelRootPainter(object sender, PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Control val = (Control)((sender is Control) ? sender : null);
		Rectangle clientRectangle = val.ClientRectangle;
		StiDrawing.FillRectangle(graphics, StiUX.Background, clientRectangle);
		Color color = (StiUX.IsDark ? StiUX.InputBorder : Color.FromArgb(170, 170, 170));
		StiDrawing.DrawRectangle(graphics, color, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
	}
}
