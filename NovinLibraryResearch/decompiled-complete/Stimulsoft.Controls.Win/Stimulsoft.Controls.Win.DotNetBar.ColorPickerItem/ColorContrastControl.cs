using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.ColorPickerItem;

[ToolboxItem(false)]
internal class ColorContrastControl : Control
{
	private Container components;

	private Color m_SelectedColor = Color.White;

	private Bitmap m_BlendBitmap;

	private double m_SelectedLuminance = -1.0;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectedColor
	{
		get
		{
			if (m_SelectedLuminance >= 0.0)
			{
				double H = 0.0;
				double S = 0.0;
				double L = 0.0;
				GetHSLFromRGB(m_SelectedColor, ref H, ref S, ref L);
				return GetColorFromHSL(H, S, m_SelectedLuminance);
			}
			return m_SelectedColor;
		}
		set
		{
			m_SelectedColor = value;
			double H = 0.0;
			double S = 0.0;
			double L = 0.0;
			GetHSLFromRGB(m_SelectedColor, ref H, ref S, ref L);
			if (m_SelectedLuminance < 0.0)
			{
				m_SelectedLuminance = L;
			}
			CreateBlendBitmap();
			((Control)this).Invalidate();
		}
	}

	public event EventHandler SelectedColorChanged;

	protected virtual void OnSelectedColorChanged(EventArgs e)
	{
		this.SelectedColorChanged?.Invoke(this, e);
	}

	public ColorContrastControl()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		if (((Control)this).BackColor == Color.Transparent)
		{
			((Control)this).OnPaintBackground(e);
		}
		else
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (m_BlendBitmap != null)
		{
			graphics.DrawImageUnscaled((Image)(object)m_BlendBitmap, 0, 0);
		}
		if (m_SelectedLuminance >= 0.0)
		{
			int num = (int)((double)((Control)this).ClientRectangle.Height * (1.0 - m_SelectedLuminance));
			int num2 = ((Image)m_BlendBitmap).Width + StiScale2.I4;
			GraphicsPath val2 = new GraphicsPath();
			val2.AddLine(num2, num, num2 + StiScale2.I7, num - StiScale2.I4);
			val2.AddLine(num2 + StiScale2.I7, num - StiScale2.I4, num2 + StiScale2.I7, num + StiScale2.I4);
			val2.CloseAllFigures();
			SolidBrush val3 = new SolidBrush(Color.Black);
			try
			{
				graphics.FillPath((Brush)(object)val3, val2);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			val2.Dispose();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		CreateBlendBitmap();
		((Control)this).OnResize(e);
	}

	private void CreateBlendBitmap()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Bitmap val = new Bitmap(StiScale2.XXI(12.0), clientRectangle.Height, (PixelFormat)137224);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.FillRectangle(SystemBrushes.Control, clientRectangle);
			Color selectedColor = m_SelectedColor;
			int r = selectedColor.R;
			int g = selectedColor.G;
			int b = selectedColor.B;
			int i = StiScale2.I4;
			int width = StiScale2.XXI(12.0);
			int x = clientRectangle.X;
			int num = clientRectangle.Y;
			double H = 0.0;
			double S = 0.0;
			double L = 0.0;
			GetHSLFromRGB(m_SelectedColor, ref H, ref S, ref L);
			for (int j = clientRectangle.Y; j < clientRectangle.Height; j += i)
			{
				double l = 1.0 - (double)j / (double)clientRectangle.Height;
				Color colorFromHSL = GetColorFromHSL(H, S, l);
				SolidBrush val3 = new SolidBrush(colorFromHSL);
				try
				{
					val2.FillRectangle((Brush)(object)val3, new Rectangle(x, num, width, i));
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				num += i;
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (m_BlendBitmap != null)
		{
			((Image)m_BlendBitmap).Dispose();
		}
		m_BlendBitmap = val;
	}

	private int GetRGBFromHue(float rm1, float rm2, float rh)
	{
		if (rh > 360f)
		{
			rh -= 360f;
		}
		else if (rh < 0f)
		{
			rh += 360f;
		}
		if (rh < 60f)
		{
			rm1 += (rm2 - rm1) * rh / 60f;
		}
		else if (rh < 180f)
		{
			rm1 = rm2;
		}
		else if (rh < 240f)
		{
			rm1 += (rm2 - rm1) * (240f - rh) / 60f;
		}
		return (int)(rm1 * 255f);
	}

	internal static Color GetColorFromHSL(double H, double S, double L)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		if (L == 0.0)
		{
			num = (num2 = (num3 = 0.0));
		}
		else if (S == 0.0)
		{
			num = (num2 = (num3 = L));
		}
		else
		{
			double num4 = ((L <= 0.5) ? (L * (1.0 + S)) : (L + S - L * S));
			double num5 = 2.0 * L - num4;
			double[] array = new double[3]
			{
				H + 1.0 / 3.0,
				H,
				H - 1.0 / 3.0
			};
			double[] array2 = new double[3];
			for (int i = 0; i < 3; i++)
			{
				if (array[i] < 0.0)
				{
					array[i] += 1.0;
				}
				if (array[i] > 1.0)
				{
					array[i] -= 1.0;
				}
				if (6.0 * array[i] < 1.0)
				{
					array2[i] = num5 + (num4 - num5) * array[i] * 6.0;
				}
				else if (2.0 * array[i] < 1.0)
				{
					array2[i] = num4;
				}
				else if (3.0 * array[i] < 2.0)
				{
					array2[i] = num5 + (num4 - num5) * (2.0 / 3.0 - array[i]) * 6.0;
				}
				else
				{
					array2[i] = num5;
				}
			}
			num = array2[0];
			num2 = array2[1];
			num3 = array2[2];
		}
		return Color.FromArgb((int)(255.0 * num), (int)(255.0 * num2), (int)(255.0 * num3));
	}

	internal static void GetHSLFromRGB(Color color, ref double H, ref double S, ref double L)
	{
		H = (double)color.GetHue() / 360.0;
		L = color.GetBrightness();
		S = color.GetSaturation();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		double luminance = 1.0 - (double)e.Y / (double)((Control)this).ClientRectangle.Height;
		SetLuminance(luminance);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && e.Y >= 0 && e.Y < ((Control)this).ClientRectangle.Bottom)
		{
			double luminance = 1.0 - (double)e.Y / (double)((Control)this).ClientRectangle.Height;
			SetLuminance(luminance);
		}
		((Control)this).OnMouseMove(e);
	}

	internal void SetLuminance(double d)
	{
		m_SelectedLuminance = d;
		((Control)this).Invalidate();
		if (this.SelectedColorChanged != null)
		{
			this.SelectedColorChanged(this, new EventArgs());
		}
	}

	private void InitializeComponent()
	{
		((Control)this).Name = "ColorContrastControl";
		((Control)this).Size = new System.Drawing.Size(24, 248);
	}
}
