using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroFormRenderer : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		MetroAppForm metroAppForm = renderingInfo.Control as MetroAppForm;
		BorderOverlay borderOverlay = renderingInfo.Control as BorderOverlay;
		if (metroAppForm == null && borderOverlay != null)
		{
			metroAppForm = ((Control)borderOverlay).Parent as MetroAppForm;
		}
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		MetroAppFormColorTable metroAppForm2 = renderingInfo.ColorTable.MetroAppForm;
		Thickness borderThickness = metroAppForm.BorderThickness;
		BorderColors borderColor = metroAppForm.BorderColor;
		if (borderThickness.IsZero && borderColor.IsEmpty)
		{
			borderThickness = (metroAppForm.IsGlassEnabled ? metroAppForm2.BorderThickness : metroAppForm2.BorderPlainThickness);
			borderColor = (metroAppForm.IsActive ? metroAppForm2.BorderColors : metroAppForm2.BorderColorsInactive);
		}
		if (borderOverlay == null)
		{
			SolidBrush val = new SolidBrush(((Control)metroAppForm).BackColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, new Rectangle(0, 0, ((Control)metroAppForm).Width, ((Control)metroAppForm).Height));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (!borderThickness.IsZero && !borderColor.IsEmpty)
		{
			RectangleF bounds = new RectangleF(0f, 0f, ((Control)metroAppForm).Width, ((Control)metroAppForm).Height);
			DrawingHelpers.DrawBorder(graphics, bounds, borderThickness, borderColor);
		}
	}
}
