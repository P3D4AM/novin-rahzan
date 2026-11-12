using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.ColorPickers;

[ToolboxItem(false)]
[DefaultEvent("SelectedColorChanged")]
[DefaultProperty("SelectedColor")]
[ToolboxBitmap(typeof(CustomColorBlender), "ColorPickerItem.CustomColorBlender.ico")]
public class CustomColorBlender : Control
{
	private Bitmap m_ColorBlendBitmap;

	private Point m_SelectedPoint = new Point(-1, -1);

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectedColor
	{
		get
		{
			if (m_SelectedPoint.X < 0 || m_SelectedPoint.Y < 0 || m_ColorBlendBitmap == null)
			{
				return Color.Empty;
			}
			return m_ColorBlendBitmap.GetPixel(m_SelectedPoint.X, m_SelectedPoint.Y);
		}
		set
		{
			if (value.Equals((object?)Color.Transparent) || value.IsEmpty)
			{
				if (!m_SelectedPoint.IsEmpty)
				{
					m_SelectedPoint = new Point(-1, -1);
					((Control)this).Invalidate();
					OnSelectedColorChanged(EventArgs.Empty);
				}
				return;
			}
			Point point = FindColorPoint(value);
			if (m_SelectedPoint != point)
			{
				m_SelectedPoint = point;
				SetSelectedPoint(m_SelectedPoint);
				((Control)this).Invalidate();
				OnSelectedColorChanged(EventArgs.Empty);
			}
		}
	}

	public event EventHandler SelectedColorChanged;

	protected virtual void OnSelectedColorChanged(EventArgs e)
	{
		this.SelectedColorChanged?.Invoke(this, e);
	}

	public CustomColorBlender()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)16, true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && m_ColorBlendBitmap != null)
		{
			((Image)m_ColorBlendBitmap).Dispose();
			m_ColorBlendBitmap = null;
		}
		((Control)this).Dispose(disposing);
	}

	private void CreateBlendBitmap()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		int num = 6;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		int num2 = clientRectangle.Width / num;
		int num3 = 127;
		int num4 = clientRectangle.X;
		Bitmap val = new Bitmap(clientRectangle.Width, clientRectangle.Height, (PixelFormat)137224);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.FillRectangle(SystemBrushes.Control, clientRectangle);
			for (int i = 0; i < num; i++)
			{
				int num5 = 1;
				int num6 = 255 / (num2 / num5);
				if (num6 < 1)
				{
					num5 = num2 / 255;
					num6 = 1;
				}
				int num7 = 1;
				int num8 = num3 / (clientRectangle.Height / num7);
				if (num8 < 1)
				{
					num7 = clientRectangle.Height / num3;
					num8 = 1;
				}
				int num9 = num4;
				int num10 = clientRectangle.Y;
				int num11 = 0;
				int num12 = 0;
				int num13 = 0;
				int num14 = 0;
				int num15 = 0;
				int num16 = 0;
				int num17 = 0;
				int num18 = 0;
				int num19 = 0;
				switch (i)
				{
				case 0:
					num11 = 255;
					num12 = 0;
					num13 = 0;
					num15 = num6;
					break;
				case 1:
					num11 = 255;
					num12 = 255;
					num13 = 0;
					num14 = -num6;
					break;
				case 2:
					num11 = 0;
					num12 = 255;
					num13 = 0;
					num16 = num6;
					break;
				case 3:
					num11 = 0;
					num12 = 255;
					num13 = 255;
					num15 = -num6;
					break;
				case 4:
					num11 = 0;
					num12 = 0;
					num13 = 255;
					num14 = num6;
					break;
				case 5:
					num11 = 255;
					num12 = 0;
					num13 = 255;
					num16 = -num6;
					break;
				}
				for (int j = 0; j < num2; j += num5)
				{
					int red = num11;
					int green = num12;
					int blue = num13;
					num17 = 127 - num11;
					num18 = 127 - num12;
					num19 = 127 - num13;
					for (int k = clientRectangle.Y; k < clientRectangle.Height; k += num7)
					{
						SolidBrush val3 = new SolidBrush(Color.FromArgb(red, green, blue));
						try
						{
							val2.FillRectangle((Brush)(object)val3, new Rectangle(num9, num10, num5, num7));
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
						num10 += num7;
						red = num11 + (int)((float)num17 * ((float)k / (float)clientRectangle.Height));
						green = num12 + (int)((float)num18 * ((float)k / (float)clientRectangle.Height));
						blue = num13 + (int)((float)num19 * ((float)k / (float)clientRectangle.Height));
					}
					num9 += num5;
					num10 = clientRectangle.Y;
					num11 += num14;
					num12 += num15;
					num13 += num16;
				}
				num4 = num9;
				if (i == 5)
				{
					break;
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (m_ColorBlendBitmap != null)
		{
			((Image)m_ColorBlendBitmap).Dispose();
		}
		m_ColorBlendBitmap = val;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		if (!((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, -1, -1, ((Control)this).Width + 1, ((Control)this).Height + 1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (m_ColorBlendBitmap == null)
		{
			return;
		}
		graphics.DrawImageUnscaled((Image)(object)m_ColorBlendBitmap, 0, 0);
		if (m_SelectedPoint.X >= 0 && m_SelectedPoint.Y >= 0)
		{
			Color white = Color.White;
			SolidBrush val2 = new SolidBrush(white);
			try
			{
				Rectangle rectangle = new Rectangle(m_SelectedPoint.X - 2, m_SelectedPoint.Y - 9, 3, 5);
				graphics.FillRectangle((Brush)(object)val2, rectangle);
				rectangle.Offset(0, 10);
				graphics.FillRectangle((Brush)(object)val2, rectangle);
				rectangle = new Rectangle(m_SelectedPoint.X - 8, m_SelectedPoint.Y - 3, 5, 3);
				graphics.FillRectangle((Brush)(object)val2, rectangle);
				rectangle.Offset(10, 0);
				graphics.FillRectangle((Brush)(object)val2, rectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		((Control)this).OnPaint(e);
	}

	private void SetSelectedPoint(Point p)
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (m_ColorBlendBitmap != null)
		{
			clientRectangle.Width = ((Image)m_ColorBlendBitmap).Width;
			clientRectangle.Height = ((Image)m_ColorBlendBitmap).Height;
		}
		if (p.X < 0)
		{
			p.X = 0;
		}
		if (p.Y < 0)
		{
			p.Y = 0;
		}
		if (p.X > clientRectangle.Right)
		{
			p.X = clientRectangle.Right - 1;
		}
		if (p.Y > clientRectangle.Bottom)
		{
			p.Y = clientRectangle.Bottom - 1;
		}
		if (p != m_SelectedPoint)
		{
			if (m_SelectedPoint.X >= 0 && m_SelectedPoint.Y >= 0)
			{
				Rectangle rectangle = new Rectangle(m_SelectedPoint, Size.Empty);
				rectangle.Inflate(10, 10);
				((Control)this).Invalidate(rectangle);
			}
			m_SelectedPoint = p;
			if (m_SelectedPoint.X >= 0 && m_SelectedPoint.Y >= 0)
			{
				Rectangle rectangle2 = new Rectangle(m_SelectedPoint, Size.Empty);
				rectangle2.Inflate(10, 10);
				((Control)this).Invalidate(rectangle2);
			}
			OnSelectedColorChanged(EventArgs.Empty);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		SetSelectedPoint(new Point(e.X, e.Y));
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Control)this).OnMouseMove(e);
		if ((int)e.Button == 1048576)
		{
			int num = e.X;
			int num2 = e.Y;
			if (num < 0)
			{
				num = 0;
			}
			if (num >= ((Control)this).ClientRectangle.Width)
			{
				num = ((Control)this).ClientRectangle.Width - 1;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 >= ((Control)this).ClientRectangle.Height)
			{
				num2 = ((Control)this).ClientRectangle.Height - 1;
			}
			SetSelectedPoint(new Point(num, num2));
		}
	}

	protected override void OnResize(EventArgs e)
	{
		CreateBlendBitmap();
		((Control)this).OnResize(e);
	}

	private Point FindColorPoint(Color value)
	{
		if (m_ColorBlendBitmap == null || value.IsEmpty || value == Color.Transparent)
		{
			return new Point(-1, -1);
		}
		for (int i = 0; i < ((Image)m_ColorBlendBitmap).Width; i++)
		{
			for (int j = 0; j < ((Image)m_ColorBlendBitmap).Height; j++)
			{
				Color pixel = m_ColorBlendBitmap.GetPixel(i, j);
				if (IsEqualColor(pixel, value))
				{
					return new Point(i, j);
				}
			}
		}
		return new Point(-1, -1);
	}

	private bool IsEqualColor(Color value1, Color value2)
	{
		byte b = 2;
		return Math.Abs(value1.R - value2.R) < b && Math.Abs(value1.G - value2.G) < b && Math.Abs(value1.B - value2.B) < b && Math.Abs(value1.A - value2.A) < b;
	}
}
