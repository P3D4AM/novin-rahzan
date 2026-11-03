using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public abstract class StiControlElementStyle : StiElementStyle
{
	public abstract string LocalizedName { get; }

	public virtual Color BackColor { get; set; } = StiElementConsts.BackgroundColor;


	public virtual Color ForeColor { get; set; } = StiElementConsts.ForegroundColor;


	public virtual Color GlyphColor { get; set; } = Color.DimGray;


	public virtual Color SeparatorColor { get; set; } = Color.LightGray;


	public virtual Color SelectedBackColor { get; set; } = StiColor.Get("3498db");


	public virtual Color SelectedForeColor { get; set; } = Color.White;


	public virtual Color SelectedGlyphColor { get; set; } = Color.White;


	public virtual Color HotBackColor { get; set; } = StiColorUtils.Light(Color.LightGray, (byte)15);


	public virtual Color HotForeColor { get; set; } = StiElementConsts.ForegroundColor;


	public virtual Color HotGlyphColor { get; set; } = StiElementConsts.ForegroundColor;


	public virtual Color HotSelectedBackColor { get; set; } = StiColorUtils.Light("3498db", (byte)30);


	public virtual Color HotSelectedForeColor { get; set; } = Color.White;


	public virtual Color HotSelectedGlyphColor { get; set; } = Color.White;


	public Font Font { get; set; } = new Font("Arial", 8f);


	public void DrawStyleForGallery(Graphics g, Rectangle rect)
	{
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		g.SetClip(rect);
		g.FillRectangle(Brushes.White, rect);
		rect.Inflate((int)(-10.0 * StiScale.Factor), (int)(-10.0 * StiScale.Factor));
		int num = StiScale.XXI(30.0);
		rect = new Rectangle(rect.X, rect.Y + (rect.Height - num) / 2, rect.Width, num);
		StiDrawing.FillRectangle(g, BackColor, rect);
		RectangleF rectangleF = new RectangleF(rect.Right - rect.Height, rect.Y, rect.Height, rect.Height);
		rectangleF.Inflate((int)(-3.0 * StiScale.Factor), (int)(-3.0 * StiScale.Factor));
		StiDrawing.FillRectangle(g, SelectedBackColor, rectangleF);
		g.DrawRectangle(Pens.LightGray, rect);
		PaintButton(g, rectangleF);
		Font val = new Font("Arial", 8f);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				val2.FormatFlags = (StringFormatFlags)4096;
				val2.Alignment = (StringAlignment)0;
				val2.LineAlignment = (StringAlignment)1;
				Rectangle rectangle = new Rectangle(StiScale.I6 + rect.X, rect.Y, rect.Width - StiScale.XXI(26.0), rect.Height);
				StiDrawing.DrawString(g, LocalizedName, val, ForeColor, rectangle, val2);
				g.ResetClip();
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PaintButton(Graphics g, RectangleF rect)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		float height = rect.Height;
		RectangleF rectangleF = new RectangleF(rect.Right - height, rect.Y, height, rect.Height);
		float num = rectangleF.X + rectangleF.Width / 2f;
		float num2 = rectangleF.Y + rectangleF.Height / 2f;
		GraphicsPath val = new GraphicsPath();
		try
		{
			int i = StiScale.I1;
			int i2 = StiScale.I2;
			int i3 = StiScale.I3;
			val.AddLine(num - (float)i3, num2 - (float)i2, num - (float)i3, num2 - (float)i);
			val.AddLine(num - (float)i3, num2 - (float)i, num, num2 + (float)i2);
			val.AddLine(num, num2 + (float)i2, num + (float)i3, num2 - (float)i);
			val.AddLine(num + (float)i3, num2 - (float)i, num + (float)i3, num2 - (float)i2);
			Pen val2 = new Pen(GlyphColor, 2f);
			try
			{
				val2.StartCap = (LineCap)0;
				val2.EndCap = (LineCap)0;
				g.SmoothingMode = (SmoothingMode)4;
				g.SetClip(new RectangleF(num - (float)i3, num2 - (float)i2, StiScale.I7, StiScale.I6));
				g.DrawPath(val2, val);
				g.ResetClip();
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
