using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiProgressBar : Control
{
	private const int BarLength = 50;

	private const int BarStep = 10;

	private Timer timer;

	private int maximum = 100;

	private int minimum;

	private int _value;

	public int Maximum
	{
		get
		{
			return maximum;
		}
		set
		{
			maximum = value;
		}
	}

	public int Minimum
	{
		get
		{
			return minimum;
		}
		set
		{
			minimum = value;
		}
	}

	public int Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			((Control)this).Invalidate();
		}
	}

	public void StartMarquee()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		timer = new Timer();
		timer.Interval = 50;
		timer.Tick += OnTimerTick;
		timer.Enabled = true;
	}

	public void StopMarquee()
	{
		if (timer != null)
		{
			timer.Enabled = false;
		}
	}

	internal void UpdateMarguee()
	{
		Value += 10;
		if (Value > ((Control)this).Width)
		{
			Value = 0;
		}
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			Draw(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawBar(Graphics g, Rectangle rect)
	{
		for (int i = rect.X; i < rect.Right; i += 10)
		{
			Rectangle rectangle = new Rectangle(i, rect.Y, 8, rect.Height);
			g.FillRectangle(SystemBrushes.Highlight, rectangle);
		}
	}

	private void Draw()
	{
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			Draw(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void Draw(Graphics gg)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Bitmap val = new Bitmap(clientRectangle.Width, clientRectangle.Height, (PixelFormat)2498570);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				SolidBrush val3 = new SolidBrush(((Control)this).BackColor);
				try
				{
					val2.FillRectangle((Brush)(object)val3, clientRectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				ControlPaint.DrawBorder3D(val2, clientRectangle, (Border3DStyle)2);
				clientRectangle.Inflate(-2, -2);
				val2.SetClip(clientRectangle);
				int value = Value;
				float num = (float)clientRectangle.Width / (float)Maximum;
				int width = (int)((float)value * num);
				if (timer != null && timer.Enabled)
				{
					Rectangle rect = new Rectangle(Value, clientRectangle.Y, 50, clientRectangle.Height);
					DrawBar(val2, rect);
					int num2 = clientRectangle.Right - rect.Right;
					if (num2 < 0)
					{
						rect.X = -(rect.Width + num2);
						DrawBar(val2, rect);
					}
				}
				else
				{
					DrawBar(val2, new Rectangle(clientRectangle.X, clientRectangle.Y, width, clientRectangle.Height));
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			gg.DrawImageUnscaled((Image)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		Draw(e.Graphics);
	}

	private void OnTimerTick(object sender, EventArgs e)
	{
		UpdateMarguee();
	}

	public StiProgressBar()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
	}
}
