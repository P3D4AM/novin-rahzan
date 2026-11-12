using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing;

public sealed class StiPenStyleHelper
{
	public static int GetMaxWidthOfPenStyles(Graphics g, Font font)
	{
		Array values = Enum.GetValues(typeof(StiPenStyle));
		int num = 0;
		for (int i = 0; i < values.Length; i++)
		{
			string text = ((StiPenStyle)values.GetValue(i)).ToString();
			string text2 = StiLocalization.Get("PropertyEnum", "StiPenStyle" + text);
			if (text2 != null)
			{
				text = text2;
			}
			int val = (int)g.MeasureString(text, font).Width;
			num = Math.Max(num, val);
		}
		return num + (int)Math.Ceiling(60.0 * StiScale.X);
	}

	public static void DrawPenStyle(DrawItemEventArgs e)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		Rectangle rectangle = new Rectangle(bounds.X + (int)Math.Ceiling(2.0 * StiScale.X), bounds.Y + (int)Math.Ceiling(2.0 * StiScale.Y), (int)Math.Ceiling(52.0 * StiScale.X), (int)Math.Ceiling(14.0 * StiScale.Y));
		bounds.Width--;
		StiControlPaint.DrawItemBackground(graphics, bounds, e.State);
		Array values = Enum.GetValues(typeof(StiPenStyle));
		Pen val = new Pen(Color.Black, 2f);
		try
		{
			StiPenStyle stiPenStyle = StiPenStyle.Solid;
			if (e.Index != -1)
			{
				stiPenStyle = (StiPenStyle)values.GetValue(e.Index);
			}
			val.DashStyle = StiPenUtils.GetPenStyle(stiPenStyle);
			graphics.FillRectangle(Brushes.White, rectangle);
			int num = bounds.Top + bounds.Height / 2;
			switch (stiPenStyle)
			{
			case StiPenStyle.Double:
				val.Width = (int)(1.0 * StiScale.Y);
				graphics.DrawLine(val, (int)Math.Ceiling(2.0 * StiScale.X), num - (int)Math.Ceiling(1.0 * StiScale.Y), (int)Math.Ceiling(54.0 * StiScale.X), num - (int)Math.Ceiling(1.0 * StiScale.Y));
				graphics.DrawLine(val, (int)Math.Ceiling(2.0 * StiScale.X), num + (int)Math.Ceiling(1.0 * StiScale.Y), (int)Math.Ceiling(54.0 * StiScale.X), num + (int)Math.Ceiling(1.0 * StiScale.Y));
				break;
			default:
				graphics.DrawLine(val, (int)Math.Ceiling(2.0 * StiScale.X), num, (int)Math.Ceiling(54.0 * StiScale.X), num);
				break;
			case StiPenStyle.None:
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		graphics.DrawRectangle(Pens.Black, rectangle);
		Font val2 = new Font("Arial", 8f);
		try
		{
			StringFormat val3 = new StringFormat();
			try
			{
				val3.LineAlignment = (StringAlignment)1;
				string text = ((StiPenStyle)values.GetValue(e.Index)).ToString();
				string text2 = StiLocalization.Get("PropertyEnum", "StiPenStyle" + text);
				if (text2 != null)
				{
					text = text2;
				}
				e.Graphics.DrawString(text, val2, Brushes.Black, (RectangleF)new Rectangle((int)Math.Ceiling(55.0 * StiScale.X), bounds.Top + (int)Math.Ceiling(4.0 * StiScale.Y), bounds.Width - (int)Math.Ceiling(18.0 * StiScale.X), (int)Math.Ceiling(10.0 * StiScale.Y)), val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
