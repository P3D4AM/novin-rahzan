using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.ColorPickers;

internal class CombCell
{
	private Color m_Color = Color.Empty;

	private Point[] m_HexagonPoints = new Point[6];

	private const float Tan30 = 0.57735026f;

	private bool m_MouseOver;

	private bool m_Selected;

	private Rectangle m_Bounds = Rectangle.Empty;

	public Color Color
	{
		get
		{
			return m_Color;
		}
		set
		{
			m_Color = value;
		}
	}

	public Rectangle Bounds => m_Bounds;

	public bool MouseOver
	{
		get
		{
			return m_MouseOver;
		}
		set
		{
			m_MouseOver = value;
		}
	}

	public bool Selected
	{
		get
		{
			return m_Selected;
		}
		set
		{
			m_Selected = value;
		}
	}

	public void SetPosition(float x, float y, int nWidth)
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		float num = (float)nWidth * 0.57735026f;
		m_HexagonPoints[0] = new Point((int)Math.Floor(x - (float)(nWidth / 2)), (int)Math.Floor(y - num / 2f) - 1);
		m_HexagonPoints[1] = new Point((int)Math.Floor(x), (int)Math.Floor(y - (float)(nWidth / 2)) - 1);
		m_HexagonPoints[2] = new Point((int)Math.Floor(x + (float)(nWidth / 2)), (int)Math.Floor(y - num / 2f) - 1);
		m_HexagonPoints[3] = new Point((int)Math.Floor(x + (float)(nWidth / 2)), (int)Math.Floor(y + num / 2f) + 1);
		m_HexagonPoints[4] = new Point((int)Math.Floor(x), (int)Math.Floor(y + (float)(nWidth / 2)) + 1);
		m_HexagonPoints[5] = new Point((int)Math.Floor(x - (float)(nWidth / 2)), (int)Math.Floor(y + num / 2f) + 1);
		GraphicsPath val = new GraphicsPath();
		val.AddPolygon(m_HexagonPoints);
		m_Bounds = Rectangle.Round(val.GetBounds());
		m_Bounds.Inflate(2, 2);
	}

	public void Draw(Graphics g)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		GraphicsPath val = new GraphicsPath();
		val.AddPolygon(m_HexagonPoints);
		val.CloseAllFigures();
		SolidBrush val2 = new SolidBrush(m_Color);
		try
		{
			g.FillPath((Brush)(object)val2, val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (m_MouseOver || m_Selected)
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)4;
			Pen val3 = new Pen(Color.FromArgb(41, 92, 150), 2f);
			try
			{
				g.DrawPath(val3, val);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Pen val4 = new Pen(Color.FromArgb(149, 178, 239), 1f);
			try
			{
				g.DrawPath(val4, val);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			g.SmoothingMode = smoothingMode;
		}
		val.Dispose();
	}

	public void Draw(Graphics g, Color c)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddPolygon(m_HexagonPoints);
		SolidBrush val2 = new SolidBrush(c);
		try
		{
			g.FillPath((Brush)(object)val2, val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		val.Dispose();
	}
}
