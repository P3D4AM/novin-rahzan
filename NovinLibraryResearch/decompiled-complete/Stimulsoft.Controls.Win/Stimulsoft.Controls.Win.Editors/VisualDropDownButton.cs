using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualDropDownButton : VisualButton
{
	protected override void PaintButtonBackground(PaintInfo p, Office2007ButtonItemStateColorTable ct)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		base.PaintButtonBackground(p, ct);
		if (base.Text.Length == 0 && base.Image == null)
		{
			SolidBrush val = new SolidBrush(ct.Text);
			try
			{
				p.Graphics.FillPolygon((Brush)(object)val, ButtonItemPainter.GetExpandPolygon(base.RenderBounds, ePopupSide.Default));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
