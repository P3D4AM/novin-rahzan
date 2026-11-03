using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Presentation;

namespace DevComponents.DotNetBar.Rendering;

internal class ᯉ : ᯈ, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void Paint(QatCustomizeItemRendererEventArgs e)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		QatCustomizeItem customizeItem = e.CustomizeItem;
		Rectangle displayRectangle = customizeItem.DisplayRectangle;
		Region val = null;
		if (graphics.Clip != null)
		{
			val = graphics.Clip;
		}
		graphics.SetClip(customizeItem.DisplayRectangle, (CombineMode)1);
		Office2007ButtonItemColorTable colorTable = GetColorTable();
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = colorTable.Default;
		if (customizeItem.Expanded)
		{
			office2007ButtonItemStateColorTable = colorTable.Expanded;
		}
		else if (customizeItem.IsMouseOver)
		{
			office2007ButtonItemStateColorTable = colorTable.MouseOver;
		}
		if (StyleManager.IsMetro(customizeItem.EffectiveStyle))
		{
			ᯊ.ᯓ(graphics, office2007ButtonItemStateColorTable, displayRectangle, RoundRectangleShapeDescriptor.RectangleShape);
		}
		else
		{
			ᯊ.ᯓ(graphics, office2007ButtonItemStateColorTable, displayRectangle, RoundRectangleShapeDescriptor.RoundCorner2);
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		Color expandBackground = office2007ButtonItemStateColorTable.ExpandBackground;
		Color expandLight = office2007ButtonItemStateColorTable.ExpandLight;
		ᙝ ᙝ = new ᙝ();
		ᙔ border = new ᙔ(expandLight, 1);
		ᙔ border2 = new ᙔ(expandBackground, 1);
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(0, 0), new ᙞ(Dpi.Width4, 0), border2));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(0, Dpi.Height1), new ᙞ(Dpi.Width4, Dpi.Height1), border));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(0, Dpi.Height3), new ᙞ(Dpi.Width4, Dpi.Height3), border2));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(Dpi.Width4, Dpi.Height3), new ᙞ(Dpi.Width2, Dpi.Height5), border2));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(Dpi.Width2, Dpi.Height5), new ᙞ(0, Dpi.Height3), border2));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(Dpi.Width1, Dpi.Height4), new ᙞ(Dpi.Width3, Dpi.Height4), border2));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(Dpi.Width4, Dpi.Height4), new ᙞ(Dpi.Width2, Dpi.Height6), border));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(Dpi.Width2, Dpi.Height6), new ᙞ(0, Dpi.Height4), border));
		Rectangle bounds = new Rectangle(displayRectangle.X + (displayRectangle.Width - Dpi.Width5) / 2, displayRectangle.Y + (displayRectangle.Height - Dpi.Height7) / 2, Dpi.Width5, Dpi.Height7);
		ᙰ p = new ᙰ(graphics, bounds);
		ᙝ.Paint(p);
		graphics.SmoothingMode = smoothingMode;
		if (val != null)
		{
			graphics.Clip = val;
			val.Dispose();
		}
		else
		{
			graphics.ResetClip();
		}
	}

	protected virtual Office2007ButtonItemColorTable GetColorTable()
	{
		Office2007ColorTable office2007ColorTable = ᘽ;
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = null;
		eButtonColor eButtonColor = eButtonColor.Orange;
		office2007ButtonItemColorTable = office2007ColorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor)];
		if (office2007ButtonItemColorTable == null)
		{
			return office2007ColorTable.ButtonItemColors[0];
		}
		return office2007ButtonItemColorTable;
	}
}
