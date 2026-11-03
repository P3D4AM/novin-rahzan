using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv;

public class DayPaintEventArgs : EventArgs
{
	public readonly Graphics Graphics;

	public eDayPaintParts RenderParts = eDayPaintParts.All;

	internal DayLabel _ᕹ;

	internal ItemPaintArgs __0E83;

	public DayPaintEventArgs(ItemPaintArgs p, DayLabel item)
	{
		Graphics = p.Graphics;
		__0E83 = p;
		_ᕹ = item;
	}

	public void PaintBackground()
	{
		_ᕹ.ᯓ(__0E83);
	}

	public void PaintText()
	{
		_ᕹ._2422(__0E83, null, Color.Empty, _ᕹ.TextAlign);
	}

	public void PaintText(Color textColor)
	{
		_ᕹ._2422(__0E83, null, textColor, _ᕹ.TextAlign);
	}

	public void PaintText(Color textColor, eLabelPartAlignment textAlign)
	{
		_ᕹ._2422(__0E83, null, textColor, textAlign);
	}

	public void PaintText(Color textColor, Font textFont)
	{
		_ᕹ._2422(__0E83, textFont, textColor, _ᕹ.TextAlign);
	}

	public void PaintText(Color textColor, Font textFont, eLabelPartAlignment textAlign)
	{
		_ᕹ._2422(__0E83, textFont, textColor, textAlign);
	}

	public void PaintImage()
	{
		_ᕹ._25B3(__0E83, _ᕹ.Image, _ᕹ.ImageAlign);
	}

	public void PaintImage(eLabelPartAlignment imageAlign)
	{
		_ᕹ._25B3(__0E83, _ᕹ.Image, imageAlign);
	}
}
