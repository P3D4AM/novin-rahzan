using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
public class TabSystemBox
{
	public Rectangle DisplayRectangle = Rectangle.Empty;

	private Rectangle m_CloseRect = Rectangle.Empty;

	private Rectangle m_ForwardRect = Rectangle.Empty;

	private Rectangle m_BackRect = Rectangle.Empty;

	public int DefaultWidth = 43;

	private bool m_BackEnabled = true;

	private bool m_ForwardEnabled = true;

	private bool m_CloseVisible = true;

	private eMouseState m_CloseMouseState;

	private eMouseState m_BackMouseState;

	private eMouseState m_ForwardMouseState;

	private TabStrip m_Parent;

	protected ToolTip m_ToolTipWnd;

	private Timer m_ClickTimer;

	private bool m_ScrollBack;

	private bool m_ScrollForward;

	public bool Visible = true;

	public bool CloseVisible
	{
		get
		{
			return m_CloseVisible;
		}
		set
		{
			m_CloseVisible = value;
			if (m_CloseVisible)
			{
				DefaultWidth = 43;
			}
			else
			{
				DefaultWidth = 32;
			}
		}
	}

	public Rectangle ForwardRect => m_ForwardRect;

	public Rectangle BackRect => m_BackRect;

	public bool ForwardEnabled
	{
		get
		{
			return m_ForwardEnabled;
		}
		set
		{
			if (m_ForwardEnabled != value)
			{
				m_ForwardEnabled = value;
				if (!m_ForwardEnabled)
				{
					DestroyTimer();
				}
			}
		}
	}

	public bool BackEnabled
	{
		get
		{
			return m_BackEnabled;
		}
		set
		{
			if (m_BackEnabled != value)
			{
				m_BackEnabled = value;
				if (!m_BackEnabled)
				{
					DestroyTimer();
				}
			}
		}
	}

	public event EventHandler Back;

	public event EventHandler Forward;

	public event EventHandler Close;

	public TabSystemBox(TabStrip parent)
	{
		m_Parent = parent;
	}

	public void Dispose()
	{
		DestroyTimer();
	}

	private void DestroyTimer()
	{
		m_ScrollBack = false;
		m_ScrollForward = false;
		if (m_ClickTimer != null)
		{
			m_ClickTimer.Stop();
			m_ClickTimer.Enabled = false;
			((Component)(object)m_ClickTimer).Dispose();
			m_ClickTimer = null;
		}
	}

	public void MouseLeave()
	{
		HideToolTip();
		if (!DisplayRectangle.IsEmpty && (m_CloseMouseState != 0 || m_BackMouseState != 0 || m_ForwardMouseState != 0))
		{
			ResetButtonMouseState();
			((Control)m_Parent).Invalidate(DisplayRectangle, false);
		}
	}

	public void MouseHover()
	{
		HideToolTip();
		_ = DisplayRectangle.IsEmpty;
	}

	public void MouseMove(MouseEventArgs e)
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Invalid comparison between Unknown and I4
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Invalid comparison between Unknown and I4
		if (DisplayRectangle.IsEmpty)
		{
			return;
		}
		bool flag = false;
		if (m_CloseMouseState != 0 || m_BackMouseState != 0 || m_ForwardMouseState != 0)
		{
			ResetButtonMouseState();
			flag = true;
		}
		if (!DisplayRectangle.Contains(e.X, e.Y))
		{
			if (flag)
			{
				((Control)m_Parent).Invalidate(DisplayRectangle, false);
			}
			return;
		}
		if (m_CloseRect.Contains(e.X, e.Y))
		{
			if (CloseVisible && m_Parent.SelectedTab != null)
			{
				if ((int)e.Button == 1048576)
				{
					m_CloseMouseState = eMouseState.Down;
				}
				else
				{
					m_CloseMouseState = eMouseState.Hot;
				}
				flag = true;
			}
		}
		else if (m_BackRect.Contains(e.X, e.Y))
		{
			if (BackEnabled)
			{
				if ((int)e.Button == 1048576)
				{
					m_BackMouseState = eMouseState.Down;
				}
				else
				{
					m_BackMouseState = eMouseState.Hot;
				}
				flag = true;
			}
		}
		else if (m_ForwardRect.Contains(e.X, e.Y) && ForwardEnabled)
		{
			if ((int)e.Button == 1048576)
			{
				m_ForwardMouseState = eMouseState.Down;
			}
			else
			{
				m_ForwardMouseState = eMouseState.Hot;
			}
			flag = true;
		}
		if (flag)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(2, 2);
			((Control)m_Parent).Invalidate(displayRectangle, false);
		}
	}

	public void MouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		if ((int)e.Button != 1048576 || DisplayRectangle.IsEmpty)
		{
			return;
		}
		if (CloseVisible && m_CloseRect.Contains(e.X, e.Y) && m_Parent.SelectedTab != null)
		{
			m_CloseMouseState = eMouseState.Down;
		}
		else if (BackEnabled && m_BackRect.Contains(e.X, e.Y))
		{
			m_BackMouseState = eMouseState.Down;
			if (m_Parent.TabScrollAutoRepeat)
			{
				m_ScrollBack = true;
				m_ScrollForward = false;
				if (m_ClickTimer == null)
				{
					m_ClickTimer = new Timer();
				}
				m_ClickTimer.Interval = m_Parent.TabScrollRepeatInterval;
				m_ClickTimer.Tick += ScrollClickTimer;
				m_ClickTimer.Start();
			}
		}
		else if (ForwardEnabled && m_ForwardRect.Contains(e.X, e.Y))
		{
			m_ForwardMouseState = eMouseState.Down;
			if (m_Parent.TabScrollAutoRepeat)
			{
				m_ScrollForward = true;
				m_ScrollBack = false;
				if (m_ClickTimer == null)
				{
					m_ClickTimer = new Timer();
				}
				m_ClickTimer.Interval = m_Parent.TabScrollRepeatInterval;
				m_ClickTimer.Tick += ScrollClickTimer;
				m_ClickTimer.Start();
			}
		}
		if (m_CloseMouseState != 0 || m_BackMouseState != 0 || m_ForwardMouseState != 0)
		{
			((Control)m_Parent).Invalidate(DisplayRectangle, false);
		}
	}

	private void ScrollClickTimer(object sender, EventArgs e)
	{
		if (m_ScrollBack)
		{
			if (this.Back != null)
			{
				this.Back(this, new EventArgs());
			}
		}
		else if (m_ScrollForward)
		{
			if (this.Forward != null)
			{
				this.Forward(this, new EventArgs());
			}
		}
		else if (m_ClickTimer != null)
		{
			m_ClickTimer.Stop();
		}
	}

	public void MouseUp(MouseEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		DestroyTimer();
		if ((int)e.Button != 1048576 || DisplayRectangle.IsEmpty)
		{
			return;
		}
		bool flag = false;
		if (m_CloseMouseState != 0 || m_BackMouseState != 0 || m_ForwardMouseState != 0)
		{
			flag = true;
		}
		if (CloseVisible && m_CloseRect.Contains(e.X, e.Y) && m_Parent.SelectedTab != null)
		{
			if (m_CloseMouseState == eMouseState.Down && this.Close != null)
			{
				this.Close(this, new EventArgs());
			}
			ResetButtonMouseState();
		}
		else if (BackEnabled && m_BackRect.Contains(e.X, e.Y))
		{
			if (m_BackMouseState == eMouseState.Down && this.Back != null)
			{
				this.Back(this, new EventArgs());
			}
			ResetButtonMouseState();
			m_BackMouseState = eMouseState.Hot;
		}
		else if (ForwardEnabled && m_ForwardRect.Contains(e.X, e.Y))
		{
			if (m_ForwardMouseState == eMouseState.Down && this.Forward != null)
			{
				this.Forward(this, new EventArgs());
			}
			ResetButtonMouseState();
			m_ForwardMouseState = eMouseState.Hot;
		}
		if (flag || m_CloseMouseState != 0 || m_BackMouseState != 0 || m_ForwardMouseState != 0)
		{
			((Control)m_Parent).Invalidate(DisplayRectangle, false);
		}
	}

	public void MouseWheel(MouseEventArgs e)
	{
		if (m_Parent.VisibleTabCount <= 0)
		{
			return;
		}
		if (e.Delta > 0)
		{
			if (this.Back != null && BackEnabled)
			{
				this.Back(this, new EventArgs());
			}
		}
		else if (this.Forward != null && ForwardEnabled)
		{
			this.Forward(this, new EventArgs());
		}
	}

	private void ResetButtonMouseState()
	{
		m_CloseMouseState = eMouseState.None;
		m_BackMouseState = eMouseState.None;
		m_ForwardMouseState = eMouseState.None;
	}

	public void Paint(Graphics g)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		if (DisplayRectangle.IsEmpty)
		{
			return;
		}
		int x = DisplayRectangle.X;
		int y = DisplayRectangle.Y;
		Color color = g.GetNearestColor(ControlPaint.Light(m_Parent.ColorScheme.TabItemText));
		if ((double)Math.Abs(color.GetBrightness() - m_Parent.ColorScheme.TabBackground.GetBrightness()) <= 0.2)
		{
			color = (((double)m_Parent.ColorScheme.TabBackground.GetBrightness() < 0.5) ? ControlPaint.Light(m_Parent.ColorScheme.TabBackground) : ControlPaint.Dark(m_Parent.ColorScheme.TabBackground));
		}
		SolidBrush val = null;
		Pen val2 = new Pen(color, 1f);
		if (BackEnabled || ForwardEnabled)
		{
			val = new SolidBrush(color);
		}
		if (m_Parent.TabAlignment == eTabStripAlignment.Left || m_Parent.TabAlignment == eTabStripAlignment.Right)
		{
			y += (DisplayRectangle.Height - (m_CloseVisible ? 32 : 18)) / 2;
			x += (DisplayRectangle.Width - 9) / 2;
			Point[] array = new Point[3];
			array[0].X = x + 4;
			array[0].Y = y;
			array[1].X = x;
			array[1].Y = y + 4;
			array[2].X = x + 8;
			array[2].Y = y + 4;
			m_BackRect = new Rectangle(x - 3, y - 5, 14, 14);
			if (BackEnabled)
			{
				if (m_BackMouseState == eMouseState.Hot)
				{
					DrawBackgroundHot(g, m_BackRect);
				}
				else if (m_BackMouseState == eMouseState.Down)
				{
					DrawBackgroundPressed(g, m_BackRect);
				}
				g.FillPolygon((Brush)(object)val, array);
			}
			g.DrawPolygon(val2, array);
			y += 14;
			array[0].X = x;
			array[0].Y = y;
			array[1].X = x + 8;
			array[1].Y = y;
			array[2].X = x + 4;
			array[2].Y = y + 4;
			m_ForwardRect = new Rectangle(x - 3, y - 5, 14, 14);
			if (ForwardEnabled)
			{
				if (m_ForwardMouseState == eMouseState.Hot)
				{
					DrawBackgroundHot(g, m_ForwardRect);
				}
				else if (m_ForwardMouseState == eMouseState.Down)
				{
					DrawBackgroundPressed(g, m_ForwardRect);
				}
				g.FillPolygon((Brush)(object)val, array);
			}
			g.DrawPolygon(val2, array);
			y += 13;
			if (CloseVisible)
			{
				m_CloseRect = new Rectangle(x - 3, y - 3, 14, 14);
				if (CloseVisible)
				{
					if (m_CloseMouseState == eMouseState.Hot)
					{
						DrawBackgroundHot(g, m_CloseRect);
					}
					else if (m_CloseMouseState == eMouseState.Down)
					{
						DrawBackgroundPressed(g, m_CloseRect);
					}
				}
				g.DrawLine(val2, x + 1, y, x + 7, y + 6);
				g.DrawLine(val2, x + 1, y + 1, x + 7, y + 7);
				g.DrawLine(val2, x + 1, y + 6, x + 7, y);
				g.DrawLine(val2, x + 1, y + 7, x + 7, y + 1);
			}
		}
		else
		{
			x += (DisplayRectangle.Width - (m_CloseVisible ? 32 : 18)) / 2;
			y += (DisplayRectangle.Height - 9) / 2;
			Point[] array2 = new Point[3];
			array2[0].X = x + 4;
			array2[0].Y = y;
			array2[1].X = x + 4;
			array2[1].Y = y + 8;
			array2[2].X = x;
			array2[2].Y = y + 4;
			m_BackRect = new Rectangle(x - 5, y - 3, 14, 14);
			if (BackEnabled)
			{
				if (m_BackMouseState == eMouseState.Hot)
				{
					DrawBackgroundHot(g, m_BackRect);
				}
				else if (m_BackMouseState == eMouseState.Down)
				{
					DrawBackgroundPressed(g, m_BackRect);
				}
				g.FillPolygon((Brush)(object)val, array2);
			}
			g.DrawPolygon(val2, array2);
			x += 14;
			array2[0].X = x;
			array2[0].Y = y;
			array2[1].X = x;
			array2[1].Y = y + 8;
			array2[2].X = x + 4;
			array2[2].Y = y + 4;
			m_ForwardRect = new Rectangle(x - 5, y - 3, 14, 14);
			if (ForwardEnabled)
			{
				if (m_ForwardMouseState == eMouseState.Hot)
				{
					DrawBackgroundHot(g, m_ForwardRect);
				}
				else if (m_ForwardMouseState == eMouseState.Down)
				{
					DrawBackgroundPressed(g, m_ForwardRect);
				}
				g.FillPolygon((Brush)(object)val, array2);
			}
			g.DrawPolygon(val2, array2);
			x += 13;
			if (CloseVisible)
			{
				m_CloseRect = new Rectangle(x - 4, y - 3, 14, 14);
				if (CloseVisible)
				{
					if (m_CloseMouseState == eMouseState.Hot)
					{
						DrawBackgroundHot(g, m_CloseRect);
					}
					else if (m_CloseMouseState == eMouseState.Down)
					{
						DrawBackgroundPressed(g, m_CloseRect);
					}
				}
				g.DrawLine(val2, x, y + 1, x + 6, y + 7);
				g.DrawLine(val2, x + 1, y + 1, x + 7, y + 7);
				g.DrawLine(val2, x + 6, y + 1, x, y + 7);
				g.DrawLine(val2, x + 7, y + 1, x + 1, y + 7);
			}
		}
		val2.Dispose();
		if (val != null)
		{
			((Brush)val).Dispose();
		}
	}

	private void DrawBackgroundHot(Graphics g, Rectangle r)
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		Color color = m_Parent.ColorScheme.TabItemHotBorder;
		Color color2 = m_Parent.ColorScheme.TabItemHotBackground;
		Color color3 = m_Parent.ColorScheme.TabItemHotBackground2;
		if ((m_Parent.Style == eTabStripStyle.Office2003 || m_Parent.Style == eTabStripStyle.VS2005 || m_Parent.Style == eTabStripStyle.VS2005Document || m_Parent.Style == eTabStripStyle.VS2005Dock) && color2.IsEmpty && color3.IsEmpty)
		{
			ColorScheme colorScheme = ((m_Parent.Style != eTabStripStyle.Office2003) ? new ColorScheme(eDotNetBarStyle.VS2005) : new ColorScheme(eDotNetBarStyle.Office2003));
			color2 = colorScheme.ItemHotBackground;
			color3 = colorScheme.ItemHotBackground2;
			color = colorScheme.ItemHotBorder;
		}
		if (m_Parent.Style == eTabStripStyle.OneNote || m_Parent.Style == eTabStripStyle.Office2007Document || m_Parent.Style == eTabStripStyle.Office2007Dock || m_Parent.Style == eTabStripStyle.Office2003 || m_Parent.Style == eTabStripStyle.VS2005 || m_Parent.Style == eTabStripStyle.VS2005Document || m_Parent.Style == eTabStripStyle.VS2005Dock)
		{
			if (color3.IsEmpty)
			{
				if (!color2.IsEmpty)
				{
					SolidBrush val = new SolidBrush(color2);
					try
					{
						g.FillRectangle((Brush)(object)val, r);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				LinearGradientBrush val2 = new LinearGradientBrush(r, color2, color3, (float)m_Parent.ColorScheme.TabItemHotBackgroundGradientAngle);
				try
				{
					g.FillRectangle((Brush)(object)val2, r);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			Pen val3 = new Pen(color, 1f);
			try
			{
				g.DrawRectangle(val3, r);
				return;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Color nearestColor = g.GetNearestColor(ControlPaint.LightLight(m_Parent.ColorScheme.TabBackground));
		BarFunctions.DrawBorder3D(g, r, (Border3DStyle)4, (Border3DSide)15, nearestColor, bFillInner: false);
	}

	private void DrawBackgroundPressed(Graphics g, Rectangle r)
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		Color color = m_Parent.ColorScheme.TabItemHotBorder;
		Color color2 = m_Parent.ColorScheme.TabItemHotBackground;
		Color color3 = m_Parent.ColorScheme.TabItemHotBackground2;
		if ((m_Parent.Style == eTabStripStyle.Office2003 || m_Parent.Style == eTabStripStyle.VS2005 || m_Parent.Style == eTabStripStyle.VS2005Document || m_Parent.Style == eTabStripStyle.VS2005Dock) && color2.IsEmpty && color3.IsEmpty)
		{
			ColorScheme colorScheme = ((m_Parent.Style != eTabStripStyle.Office2003) ? new ColorScheme(eDotNetBarStyle.VS2005) : new ColorScheme(eDotNetBarStyle.Office2003));
			color2 = colorScheme.ItemPressedBackground;
			color3 = colorScheme.ItemPressedBackground2;
			color = colorScheme.ItemPressedBorder;
		}
		if (m_Parent.Style == eTabStripStyle.OneNote || m_Parent.Style == eTabStripStyle.Office2007Document || m_Parent.Style == eTabStripStyle.Office2007Dock || m_Parent.Style == eTabStripStyle.Office2003 || m_Parent.Style == eTabStripStyle.VS2005 || m_Parent.Style == eTabStripStyle.VS2005 || m_Parent.Style == eTabStripStyle.VS2005Document)
		{
			if (color3.IsEmpty)
			{
				if (!color2.IsEmpty)
				{
					SolidBrush val = new SolidBrush(color2);
					try
					{
						g.FillRectangle((Brush)(object)val, r);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				LinearGradientBrush val2 = new LinearGradientBrush(r, color3, color2, (float)m_Parent.ColorScheme.TabItemHotBackgroundGradientAngle);
				try
				{
					g.FillRectangle((Brush)(object)val2, r);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			Pen val3 = new Pen(color, 1f);
			try
			{
				g.DrawRectangle(val3, r);
				return;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Color nearestColor = g.GetNearestColor(ControlPaint.LightLight(m_Parent.ColorScheme.TabBackground));
		BarFunctions.DrawBorder3D(g, r, (Border3DStyle)2, (Border3DSide)15, nearestColor, bFillInner: false);
	}

	private void HideToolTip()
	{
		if (m_ToolTipWnd != null)
		{
			((Control)m_ToolTipWnd).Hide();
			((Component)(object)m_ToolTipWnd).Dispose();
			m_ToolTipWnd = null;
		}
	}
}
