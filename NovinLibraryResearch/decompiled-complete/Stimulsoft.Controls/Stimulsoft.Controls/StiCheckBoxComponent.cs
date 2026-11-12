using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiCheckBox), "Toolbox.StiCheckBox.bmp")]
public sealed class StiCheckBoxComponent : Control
{
	public StiCheckValueStyle CheckStyleForTrue { get; set; }

	public StiCheckValueStyle CheckStyleForFalse { get; set; }

	public bool Checked { get; set; }

	public StiBrush TextBrush { get; set; } = new StiEmptyBrush();


	public Color ContourColor { get; set; } = Color.Transparent;


	public double SizeValue { get; set; }

	public event EventHandler CheckedChanged;

	private void DrawCheckStyle(Graphics g, RectangleD rect)
	{
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		StiCheckValueStyle stiCheckValueStyle = (Checked ? CheckStyleForTrue : CheckStyleForFalse);
		if (stiCheckValueStyle == StiCheckValueStyle.None)
		{
			return;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		GraphicsState val = g.Save();
		g.SetClip(rect.ToRectangleF(), (CombineMode)1);
		StringFormat val2 = new StringFormat();
		try
		{
			char c = ' ';
			string text = null;
			switch (stiCheckValueStyle)
			{
			case StiCheckValueStyle.Cross:
				c = 'û';
				text = "Wingdings";
				break;
			case StiCheckValueStyle.Check:
				c = 'ü';
				text = "Wingdings";
				break;
			case StiCheckValueStyle.CrossRectangle:
				c = 'ý';
				text = "Wingdings";
				break;
			case StiCheckValueStyle.CheckRectangle:
				c = 'þ';
				text = "Wingdings";
				break;
			case StiCheckValueStyle.CrossCircle:
				c = 'V';
				text = "Wingdings 2";
				break;
			case StiCheckValueStyle.DotCircle:
				c = '¤';
				text = "Wingdings";
				break;
			case StiCheckValueStyle.DotRectangle:
				c = '©';
				text = "Wingdings 2";
				break;
			case StiCheckValueStyle.NoneCircle:
				c = '¡';
				text = "Wingdings";
				break;
			case StiCheckValueStyle.NoneRectangle:
				c = '\u00a8';
				text = "Wingdings";
				break;
			}
			try
			{
				FontFamily val3 = new FontFamily(text);
				try
				{
					if (val3.GetName(0) == text)
					{
						GraphicsPath val4 = new GraphicsPath();
						try
						{
							val4.AddString(c.ToString(), val3, 0, Math.Min((float)rect.Height, (float)rect.Width), new RectangleF(0f, 0f, (float)rect.Width, (float)rect.Height), val2);
							RectangleF bounds = val4.GetBounds();
							float num = ((float)rect.Width - bounds.Width) / 2f - bounds.Left;
							float num2 = ((float)rect.Height - bounds.Height) / 2f - bounds.Top;
							g.TranslateTransform((float)rect.X + num, (float)rect.Y + num2);
							Brush brush = StiBrush.GetBrush(TextBrush, bounds);
							try
							{
								g.FillPath(brush, val4);
							}
							finally
							{
								((IDisposable)brush)?.Dispose();
							}
							Pen val5 = new Pen(ContourColor);
							try
							{
								val5.Width = (float)SizeValue;
								g.DrawPath(val5, val4);
							}
							finally
							{
								((IDisposable)val5)?.Dispose();
							}
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			catch
			{
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		g.Restore(val);
		g.SmoothingMode = smoothingMode;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		RectangleD rect = new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height);
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			graphics.FillRectangle((Brush)(object)val, rect.ToRectangle());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		DrawCheckStyle(graphics, rect);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		((Control)this).OnMouseClick(e);
		Checked = !Checked;
		this.CheckedChanged?.Invoke(this, EventArgs.Empty);
		((Control)this).Invalidate();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		Keys keyCode = e.KeyCode;
		if ((int)keyCode == 13 || (int)keyCode == 32)
		{
			e.Handled = true;
			Checked = !Checked;
			this.CheckedChanged?.Invoke(this, EventArgs.Empty);
			((Control)this).Invalidate();
		}
	}

	public StiCheckBoxComponent()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)8192, false);
		((Control)this).SetStyle((ControlStyles)4, false);
	}
}
