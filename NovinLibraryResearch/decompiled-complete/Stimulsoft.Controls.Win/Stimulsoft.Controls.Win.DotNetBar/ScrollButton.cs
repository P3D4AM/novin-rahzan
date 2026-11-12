using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
internal class ScrollButton : UserControl
{
	private bool m_MouseOver;

	private bool m_MouseDown;

	private eOrientation m_Orientation;

	private eItemAlignment m_ButtonAlignment;

	private Timer m_ScrollTimer;

	public ColorScheme _Scheme;

	private ThemeScrollBar m_ThemeScrollBar;

	public bool UseThemes;

	public bool UseTimer = true;

	public bool StandardButton;

	public eOrientation Orientation
	{
		get
		{
			return m_Orientation;
		}
		set
		{
			if (m_Orientation != value)
			{
				m_Orientation = value;
				((Control)this).Refresh();
			}
		}
	}

	public eItemAlignment ButtonAlignment
	{
		get
		{
			return m_ButtonAlignment;
		}
		set
		{
			if (m_ButtonAlignment != value)
			{
				m_ButtonAlignment = value;
				((Control)this).Refresh();
			}
		}
	}

	private ThemeScrollBar ThemeScrollBar
	{
		get
		{
			if (m_ThemeScrollBar == null)
			{
				m_ThemeScrollBar = new ThemeScrollBar((Control)(object)this);
			}
			return m_ThemeScrollBar;
		}
	}

	public ScrollButton()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).TabStop = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		if (StandardButton)
		{
			PaintStandardButton(e);
			return;
		}
		if (m_MouseOver)
		{
			if (_Scheme == null)
			{
				SolidBrush val = new SolidBrush(ColorFunctions.HoverBackColor(graphics));
				try
				{
					graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				NativeFunctions.DrawRectangle(graphics, SystemPens.Highlight, ((Control)this).ClientRectangle);
			}
			else
			{
				if (!_Scheme.ItemHotBackground2.IsEmpty)
				{
					LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(((Control)this).ClientRectangle, _Scheme.ItemHotBackground, _Scheme.ItemHotBackground2, _Scheme.ItemHotBackgroundGradientAngle);
					graphics.FillRectangle((Brush)(object)val2, ((Control)this).ClientRectangle);
					((Brush)val2).Dispose();
				}
				else
				{
					SolidBrush val3 = new SolidBrush(_Scheme.ItemHotBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val3, ((Control)this).DisplayRectangle);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				NativeFunctions.DrawRectangle(graphics, new Pen(_Scheme.ItemHotBorder), ((Control)this).ClientRectangle);
			}
		}
		else if (_Scheme == null)
		{
			SolidBrush val4 = new SolidBrush(SystemColors.Control);
			try
			{
				graphics.FillRectangle((Brush)(object)val4, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			NativeFunctions.DrawRectangle(graphics, SystemPens.ControlDark, ((Control)this).ClientRectangle);
		}
		else if (_Scheme.ItemBackground.IsEmpty)
		{
			SolidBrush val5 = new SolidBrush(_Scheme.BarBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val5, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		else if (_Scheme.ItemBackground2.IsEmpty)
		{
			SolidBrush val6 = new SolidBrush(_Scheme.ItemBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val6, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		else
		{
			LinearGradientBrush val7 = BarFunctions.CreateLinearGradientBrush(((Control)this).DisplayRectangle, _Scheme.ItemBackground, _Scheme.ItemBackground2, _Scheme.ItemBackgroundGradientAngle);
			try
			{
				graphics.FillRectangle((Brush)(object)val7, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (m_Orientation == eOrientation.Horizontal)
		{
			if (m_ButtonAlignment == eItemAlignment.Far)
			{
				Point[] array = new Point[3];
				array[0].X = ((Control)this).ClientRectangle.Width / 2 - 2;
				array[0].Y = (((Control)this).ClientRectangle.Height - 8) / 2;
				array[1].X = array[0].X;
				array[1].Y = array[0].Y + 8;
				array[2].X = array[0].X + 4;
				array[2].Y = array[0].Y + 4;
				if (_Scheme == null)
				{
					graphics.FillPolygon(SystemBrushes.ControlText, array);
				}
				else
				{
					graphics.FillPolygon((Brush)new SolidBrush(_Scheme.ItemText), array);
				}
			}
			else
			{
				Point[] array2 = new Point[3];
				array2[0].X = ((Control)this).ClientRectangle.Width / 2 + 2;
				array2[0].Y = (((Control)this).ClientRectangle.Height - 8) / 2;
				array2[1].X = array2[0].X;
				array2[1].Y = array2[0].Y + 8;
				array2[2].X = array2[0].X - 4;
				array2[2].Y = array2[0].Y + 4;
				if (_Scheme == null)
				{
					graphics.FillPolygon(SystemBrushes.ControlText, array2);
				}
				else
				{
					graphics.FillPolygon((Brush)new SolidBrush(_Scheme.ItemText), array2);
				}
			}
		}
		else if (m_ButtonAlignment == eItemAlignment.Far)
		{
			Point[] array3 = new Point[3];
			array3[0].X = ((Control)this).ClientRectangle.Width / 2 - 3;
			array3[0].Y = (((Control)this).ClientRectangle.Height - 4) / 2;
			array3[1].X = array3[0].X + 7;
			array3[1].Y = array3[0].Y;
			array3[2].X = array3[0].X + 3;
			array3[2].Y = array3[0].Y + 4;
			if (_Scheme == null)
			{
				graphics.FillPolygon(SystemBrushes.ControlText, array3);
			}
			else
			{
				graphics.FillPolygon((Brush)new SolidBrush(_Scheme.ItemText), array3);
			}
		}
		else
		{
			Point[] array4 = new Point[3];
			array4[0].X = ((Control)this).ClientRectangle.Width / 2 - 3;
			array4[0].Y = (((Control)this).ClientRectangle.Height + 3) / 2;
			array4[1].X = array4[0].X + 7;
			array4[1].Y = array4[0].Y;
			array4[2].X = array4[0].X + 3;
			array4[2].Y = array4[0].Y - 4;
			if (_Scheme == null)
			{
				graphics.FillPolygon(SystemBrushes.ControlText, array4);
			}
			else
			{
				graphics.FillPolygon((Brush)new SolidBrush(_Scheme.ItemText), array4);
			}
		}
	}

	private void PaintStandardButton(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		if (UseThemes)
		{
			ThemeScrollBar themeScrollBar = ThemeScrollBar;
			ThemeScrollBarParts arrowBtn = ThemeScrollBarParts.ArrowBtn;
			ThemeScrollBarStates arrowBtnUpNormal = ThemeScrollBarStates.ArrowBtnUpNormal;
			arrowBtnUpNormal = (m_MouseOver ? ((m_Orientation == eOrientation.Vertical) ? ((m_ButtonAlignment != 0) ? ThemeScrollBarStates.ArrowBtnDownHot : ThemeScrollBarStates.ArrowBtnUpHot) : ((m_ButtonAlignment != 0) ? ThemeScrollBarStates.ArrowBtnRightHot : ThemeScrollBarStates.ArrowBtnLeftHot)) : ((m_Orientation == eOrientation.Vertical) ? ((m_ButtonAlignment != 0) ? ThemeScrollBarStates.ArrowBtnDownNormal : ThemeScrollBarStates.ArrowBtnUpNormal) : ((m_ButtonAlignment != 0) ? ThemeScrollBarStates.ArrowBtnRightNormal : ThemeScrollBarStates.ArrowBtnLeftNormal)));
			themeScrollBar.DrawBackground(graphics, arrowBtn, arrowBtnUpNormal, ((Control)this).DisplayRectangle);
			return;
		}
		if (m_MouseDown)
		{
			BarFunctions.DrawBorder3D(graphics, ((Control)this).DisplayRectangle, (Border3DStyle)8);
		}
		else
		{
			BarFunctions.DrawBorder3D(graphics, ((Control)this).DisplayRectangle, (Border3DStyle)5);
		}
		if (m_Orientation == eOrientation.Vertical)
		{
			PaintArrow(graphics, ((Control)this).DisplayRectangle, SystemColors.ControlText, m_ButtonAlignment == eItemAlignment.Near);
		}
	}

	private void PaintArrow(Graphics g, Rectangle rect, Color c, bool up)
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		Point[] array = new Point[3];
		if (up)
		{
			array[0].X = rect.Left + (rect.Width - 9) / 2;
			array[0].Y = rect.Top + rect.Height / 2 + 1;
			array[1].X = array[0].X + 8;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 4;
			array[2].Y = array[0].Y - 5;
		}
		else
		{
			array[0].X = rect.Left + (rect.Width - 7) / 2;
			array[0].Y = rect.Top + (rect.Height - 4) / 2;
			array[1].X = array[0].X + 7;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 3;
			array[2].Y = array[0].Y + 4;
		}
		g.FillPolygon((Brush)new SolidBrush(c), array);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			((Control)this).Refresh();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (m_ScrollTimer != null)
		{
			m_ScrollTimer.Stop();
			((Component)(object)m_ScrollTimer).Dispose();
			m_ScrollTimer = null;
		}
		if (m_MouseOver)
		{
			m_MouseOver = false;
			((Control)this).Refresh();
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		if (UseTimer)
		{
			SetupScrollTimer();
		}
	}

	private void SetupScrollTimer()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (m_ScrollTimer == null)
		{
			m_ScrollTimer = new Timer();
			m_ScrollTimer.Interval = 200;
			m_ScrollTimer.Tick += ScrollTimerTick;
			m_ScrollTimer.Start();
		}
	}

	private void ScrollTimerTick(object sender, EventArgs e)
	{
		((Control)this).InvokeOnClick((Control)(object)this, new EventArgs());
	}

	protected override void Dispose(bool disposing)
	{
		if (m_ScrollTimer != null)
		{
			m_ScrollTimer.Stop();
			((Component)(object)m_ScrollTimer).Dispose();
			m_ScrollTimer = null;
		}
		((ContainerControl)this).Dispose(disposing);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		DisposeThemes();
		((Control)this).OnHandleDestroyed(e);
	}

	private void DisposeThemes()
	{
		if (m_ThemeScrollBar != null)
		{
			m_ThemeScrollBar.Dispose();
			m_ThemeScrollBar = null;
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
		}
		else if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
			return;
		}
		((UserControl)this).WndProc(ref m);
	}

	private void RefreshThemes()
	{
		if (m_ThemeScrollBar != null)
		{
			m_ThemeScrollBar.Dispose();
			m_ThemeScrollBar = new ThemeScrollBar((Control)(object)this);
		}
	}
}
