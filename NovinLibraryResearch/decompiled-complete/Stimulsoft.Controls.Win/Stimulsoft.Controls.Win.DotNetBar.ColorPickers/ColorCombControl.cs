using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.ColorPickers;

[ToolboxItem(false)]
[DefaultEvent("SelectedColorChanged")]
[DefaultProperty("SelectedColor")]
[ToolboxBitmap(typeof(ColorCombControl), "ColorPickerItem.ColorCombControl.ico")]
public class ColorCombControl : Control
{
	private CombCell[] m_ColorCombs = new CombCell[144];

	private const float YOffset = 0.824f;

	private float[] arrXOffset = new float[6] { -0.5f, -1f, -0.5f, 0.5f, 1f, 0.5f };

	private float[] arrYOffset = new float[6] { 0.824f, 0f, -0.824f, -0.824f, 0f, 0.824f };

	private int CombDepth = 7;

	private int m_MouseOverIndex = -1;

	private int m_SelectedIndex = -1;

	private Container components;

	private bool m_AntiAlias = true;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return m_AntiAlias;
		}
		set
		{
			if (m_AntiAlias != value)
			{
				m_AntiAlias = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public Color MouseOverColor
	{
		get
		{
			if (m_MouseOverIndex < 0)
			{
				return Color.Empty;
			}
			return m_ColorCombs[m_MouseOverIndex].Color;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectedColor
	{
		get
		{
			if (m_SelectedIndex < 0)
			{
				return Color.Empty;
			}
			return m_ColorCombs[m_SelectedIndex].Color;
		}
		set
		{
			if (value == Color.Empty && m_SelectedIndex >= 0)
			{
				m_ColorCombs[m_SelectedIndex].Selected = false;
				m_SelectedIndex = -1;
				((Control)this).Invalidate();
				OnSelectedColorChanged(EventArgs.Empty);
				return;
			}
			int combColorIndex = GetCombColorIndex(value);
			if (combColorIndex != m_SelectedIndex)
			{
				if (m_SelectedIndex >= 0)
				{
					m_ColorCombs[m_SelectedIndex].Selected = false;
				}
				m_SelectedIndex = combColorIndex;
				if (m_SelectedIndex >= 0)
				{
					m_ColorCombs[m_SelectedIndex].Selected = true;
				}
				((Control)this).Invalidate();
				OnSelectedColorChanged(EventArgs.Empty);
			}
		}
	}

	public event EventHandler SelectedColorChanged;

	public event EventHandler MouseOverColorChanged;

	protected virtual void OnSelectedColorChanged(EventArgs e)
	{
		this.SelectedColorChanged?.Invoke(this, e);
	}

	public ColorCombControl()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < m_ColorCombs.Length; i++)
		{
			m_ColorCombs[i] = new CombCell();
		}
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

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}

	private int GetCellSize(int height)
	{
		int num = height / (2 * CombDepth - 1) + 1;
		if ((int)Math.Floor((double)num / 2.0) * 2 < num)
		{
			num--;
		}
		return num;
	}

	private void InitializeColorComb()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(-8, -8);
		clientRectangle.Height -= GetCellSize(Math.Min(clientRectangle.Height, clientRectangle.Width));
		if (clientRectangle.Height < clientRectangle.Width)
		{
			clientRectangle.Inflate(-(clientRectangle.Width - clientRectangle.Height) / 2, 0);
		}
		else
		{
			clientRectangle.Inflate(0, -(clientRectangle.Height - clientRectangle.Width) / 2);
		}
		int cellSize = GetCellSize(clientRectangle.Height);
		clientRectangle.Height -= cellSize * 2;
		int num = (clientRectangle.Left + clientRectangle.Right) / 2;
		int num2 = (clientRectangle.Top + clientRectangle.Bottom) / 2;
		m_ColorCombs[0].Color = Color.White;
		m_ColorCombs[0].SetPosition(num, num2, cellSize);
		int num3 = 1;
		for (int i = 1; i < CombDepth; i++)
		{
			float num4 = num + cellSize * i;
			float num5 = num2;
			for (int j = 0; j < CombDepth - 1; j++)
			{
				int num6 = (int)((float)cellSize * arrXOffset[j]);
				int num7 = (int)((float)cellSize * arrYOffset[j]);
				for (int k = 0; k < i; k++)
				{
					float angleFromPoint = GetAngleFromPoint(num4 - (float)num, num5 - (float)num2);
					double l = 0.936 * (double)(CombDepth - i) / (double)CombDepth + 0.12;
					m_ColorCombs[num3].Color = GetRGBFromHLSExtend(angleFromPoint, l, 1.0);
					m_ColorCombs[num3].SetPosition(num4, num5, cellSize);
					num3++;
					num4 += (float)num6;
					num5 += (float)num7;
				}
			}
		}
		m_ColorCombs[num3].Color = Color.Black;
		num3++;
		m_ColorCombs[num3].Color = Color.White;
		int num8 = 15;
		int num9 = 255 - num8;
		num = clientRectangle.X + cellSize * 3;
		num2 = clientRectangle.Bottom + cellSize;
		for (int m = 0; m < 15; m++)
		{
			Color color = Color.FromArgb(num9, num9, num9);
			m_ColorCombs[num3].Color = color;
			m_ColorCombs[num3].SetPosition(num, num2, cellSize);
			num += cellSize;
			num3++;
			if (m == 7)
			{
				num = clientRectangle.X + (int)((double)cellSize * 3.5);
				num2 += (int)((float)cellSize * 0.824f);
			}
			num9 -= num8;
		}
		m_ColorCombs[num3].Color = Color.Black;
	}

	private Color GetRGBFromHLSExtend(double H, double L, double S)
	{
		int red;
		int green;
		int blue;
		if (S == 0.0)
		{
			red = (green = (blue = (int)(L * 255.0)));
		}
		else
		{
			float num = ((!(L <= 0.5)) ? ((float)(L + S - L * S)) : ((float)(L + L * S)));
			float rm = (float)(2.0 * L - (double)num);
			red = GetRGBFromHue(rm, num, (float)(H + 120.0));
			green = GetRGBFromHue(rm, num, (float)H);
			blue = GetRGBFromHue(rm, num, (float)(H - 120.0));
		}
		return Color.FromArgb(red, green, blue);
	}

	private float GetAngleFromPoint(float nX, float nY)
	{
		double num = Math.Atan2(nY, nX);
		return (float)(num * 180.0 / Math.PI);
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

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (m_AntiAlias)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			graphics.FillRectangle((Brush)(object)val, -1, -1, ((Control)this).Width + 1, ((Control)this).Height + 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(e);
		}
		CombCell[] colorCombs = m_ColorCombs;
		foreach (CombCell combCell in colorCombs)
		{
			combCell.Draw(graphics);
		}
		if (m_MouseOverIndex >= 0)
		{
			m_ColorCombs[m_MouseOverIndex].Draw(graphics);
		}
		if (m_SelectedIndex >= 0)
		{
			m_ColorCombs[m_SelectedIndex].Draw(graphics);
		}
		((Control)this).OnPaint(e);
	}

	protected override void OnResize(EventArgs e)
	{
		InitializeColorComb();
		((Control)this).OnResize(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		int cellAt = GetCellAt(e.X, e.Y);
		SetMouseOver(cellAt);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			if (m_SelectedIndex >= 0)
			{
				m_ColorCombs[m_SelectedIndex].Selected = false;
				((Control)this).Invalidate(m_ColorCombs[m_SelectedIndex].Bounds);
			}
			m_SelectedIndex = -1;
			if (m_MouseOverIndex >= 0)
			{
				m_SelectedIndex = m_MouseOverIndex;
				m_ColorCombs[m_SelectedIndex].Selected = true;
				((Control)this).Invalidate(m_ColorCombs[m_SelectedIndex].Bounds);
				OnSelectedColorChanged(EventArgs.Empty);
			}
		}
		((Control)this).OnMouseDown(e);
	}

	private int GetCombColorIndex(Color value)
	{
		for (int i = 0; i < m_ColorCombs.Length; i++)
		{
			if (m_ColorCombs[i].Color.Equals((object?)value))
			{
				return i;
			}
		}
		return -1;
	}

	protected virtual void OnMouseOverColorChanged(EventArgs e)
	{
		this.MouseOverColorChanged?.Invoke(this, e);
	}

	private void SetMouseOver(int newMouseOver)
	{
		if (newMouseOver != m_MouseOverIndex)
		{
			if (m_MouseOverIndex >= 0)
			{
				m_ColorCombs[m_MouseOverIndex].MouseOver = false;
				((Control)this).Invalidate(m_ColorCombs[m_MouseOverIndex].Bounds);
			}
			m_MouseOverIndex = newMouseOver;
			if (m_MouseOverIndex >= 0)
			{
				m_ColorCombs[m_MouseOverIndex].MouseOver = true;
				((Control)this).Invalidate(m_ColorCombs[m_MouseOverIndex].Bounds);
			}
			OnMouseOverColorChanged(EventArgs.Empty);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		SetMouseOver(-1);
	}

	private int GetCellAt(int x, int y)
	{
		for (int i = 0; i < m_ColorCombs.Length; i++)
		{
			if (m_ColorCombs[i].Bounds.Contains(x, y))
			{
				return i;
			}
		}
		return -1;
	}
}
