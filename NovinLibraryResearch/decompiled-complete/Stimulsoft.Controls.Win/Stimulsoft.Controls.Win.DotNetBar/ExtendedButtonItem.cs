using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("Click")]
public class ExtendedButtonItem : ButtonItem
{
	private int imageSize = StiScale2.XXI(16.0);

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<string> AfterTexts { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Image> AfterImages { get; set; }

	public ExtendedButtonItem()
	{
		NeedRecalcSize = true;
	}

	public override void Paint(ItemPaintArgs p)
	{
		base.Paint(p);
		Font font = GetFont(p, isLayout: false);
		eTextFormat buttonStringFormat = p.ButtonStringFormat;
		List<Rectangle> list = new List<Rectangle>();
		List<Rectangle> list2 = new List<Rectangle>();
		if (AfterTexts == null || AfterImages == null)
		{
			return;
		}
		int num = Bounds.Right;
		for (int num2 = AfterTexts.Count - 1; num2 >= 0; num2--)
		{
			string text = AfterTexts[num2];
			SizeF sizeF = p.Graphics.MeasureString(text, font);
			num -= (int)sizeF.Width;
			Rectangle item = new Rectangle(num, Bounds.Y, (int)sizeF.Width, Bounds.Height);
			list2.Insert(0, item);
			num -= imageSize;
			Rectangle item2 = new Rectangle(num, Bounds.Y + (Bounds.Height - imageSize) / 2, imageSize, imageSize);
			list.Insert(0, item2);
			num -= StiScale2.I4;
		}
		for (int num3 = AfterTexts.Count - 1; num3 >= 0; num3--)
		{
			string text2 = AfterTexts[num3];
			Rectangle rectangle = list2[num3];
			TextDrawing.DrawString(p.Graphics, text2, font, Color.White, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), buttonStringFormat);
			Rectangle rectangle2 = list[num3];
			if (AfterImages != null)
			{
				p.Graphics.DrawImage(AfterImages[num3], new Rectangle(rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height));
			}
		}
	}

	public override void RecalcSize()
	{
		base.RecalcSize();
		if (AfterTexts == null || AfterImages == null)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		Graphics val2 = BarFunctions.CreateGraphics(val);
		List<Rectangle> list = new List<Rectangle>();
		List<Rectangle> list2 = new List<Rectangle>();
		int num = (int)val2.MeasureString(Text, val.Font).Width + imageSize + StiScale2.XXI(10.0);
		int num2 = Bounds.X + num;
		foreach (string afterText in AfterTexts)
		{
			int x = num2;
			Rectangle item = new Rectangle(x, Bounds.Y + (Bounds.Height - imageSize) / 2, imageSize, imageSize);
			list.Add(item);
			SizeF sizeF = val2.MeasureString(afterText, val.Font);
			int x2 = num2 + imageSize;
			Rectangle item2 = new Rectangle(x2, Bounds.Y, (int)sizeF.Width, Bounds.Height);
			list2.Add(item2);
			num2 += imageSize + (int)sizeF.Width + StiScale2.I4;
		}
		m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, num2 - m_Rect.X, m_Rect.Height);
	}
}
