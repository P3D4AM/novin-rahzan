using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.ColorTables;

namespace DevComponents.DotNetBar.Metro.Rendering;

internal class Ꭰ : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		MetroAppForm metroAppForm = renderingInfo.Control as MetroAppForm;
		_285A _285A = renderingInfo.Control as _285A;
		if (metroAppForm == null && _285A != null)
		{
			metroAppForm = ((Control)_285A).Parent as MetroAppForm;
		}
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		MetroAppFormColorTable metroAppForm2 = renderingInfo.ColorTable.MetroAppForm;
		Thickness _2530 = metroAppForm.BorderThickness;
		BorderColors ዾ = metroAppForm.BorderColor;
		if (_2530.IsZero && ዾ.IsEmpty)
		{
			_2530 = (metroAppForm.IsGlassEnabled ? metroAppForm2.BorderThickness : metroAppForm2.BorderPlainThickness);
			ዾ = (metroAppForm.IsActive ? metroAppForm2.BorderColors : metroAppForm2.BorderColorsInactive);
		}
		if (_285A == null)
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
		if (!_2530.IsZero && !ዾ.IsEmpty)
		{
			RectangleF ശ = new RectangleF(0f, 0f, ((Control)metroAppForm).Width, ((Control)metroAppForm).Height);
			_252F._२(graphics, ശ, _2530, ዾ);
		}
	}
}
