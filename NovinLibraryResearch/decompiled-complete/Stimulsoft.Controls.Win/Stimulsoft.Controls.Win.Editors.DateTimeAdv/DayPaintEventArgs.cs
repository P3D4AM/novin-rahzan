using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class DayPaintEventArgs : EventArgs
{
	public readonly Graphics Graphics;

	public eDayPaintParts RenderParts = eDayPaintParts.All;

	internal DayLabel _Item;

	internal ItemPaintArgs _ItemPaintArgs;

	public DayPaintEventArgs(ItemPaintArgs p, DayLabel item)
	{
		Graphics = p.Graphics;
		_ItemPaintArgs = p;
		_Item = item;
	}

	public void PaintBackground()
	{
		_Item.PaintBackground(_ItemPaintArgs);
	}

	public void PaintText()
	{
		_Item.PaintText(_ItemPaintArgs, null, Color.Empty, _Item.TextAlign);
	}

	public void PaintText(Color textColor)
	{
		_Item.PaintText(_ItemPaintArgs, null, textColor, _Item.TextAlign);
	}

	public void PaintText(Color textColor, eLabelPartAlignment textAlign)
	{
		_Item.PaintText(_ItemPaintArgs, null, textColor, textAlign);
	}

	public void PaintText(Color textColor, Font textFont)
	{
		_Item.PaintText(_ItemPaintArgs, textFont, textColor, _Item.TextAlign);
	}

	public void PaintText(Color textColor, Font textFont, eLabelPartAlignment textAlign)
	{
		_Item.PaintText(_ItemPaintArgs, textFont, textColor, textAlign);
	}

	public void PaintImage()
	{
		_Item.PaintImage(_ItemPaintArgs, _Item.Image, _Item.ImageAlign);
	}

	public void PaintImage(eLabelPartAlignment imageAlign)
	{
		_Item.PaintImage(_ItemPaintArgs, _Item.Image, imageAlign);
	}
}
