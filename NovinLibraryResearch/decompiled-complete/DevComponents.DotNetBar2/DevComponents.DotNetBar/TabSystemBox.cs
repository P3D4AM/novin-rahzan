using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
public class TabSystemBox
{
	public Rectangle DisplayRectangle = Rectangle.Empty;

	private Rectangle ᳪ = Rectangle.Empty;

	private Rectangle ᳫ = Rectangle.Empty;

	private Rectangle ᳬ = Rectangle.Empty;

	public int DefaultWidth = 43;

	private bool _1CED = true;

	private bool ᳮ = true;

	private bool ᱶ = true;

	private EventHandler ᳯ;

	private EventHandler ᳰ;

	private EventHandler ᆤ;

	private eMouseState ᳱ;

	private eMouseState ᳲ;

	private eMouseState ᳳ;

	private TabStrip ӭ;

	protected ToolTip m_ToolTipWnd;

	private Timer _৮;

	private bool _1CF4;

	private bool ᳵ;

	public bool Visible = true;

	public bool CloseVisible
	{
		get
		{
			return ᱶ;
		}
		set
		{
			ᱶ = value;
			if (ᱶ)
			{
				DefaultWidth = 43;
			}
			else
			{
				DefaultWidth = 32;
			}
		}
	}

	public Rectangle ForwardRect => ᳫ;

	public Rectangle BackRect => ᳬ;

	public bool ForwardEnabled
	{
		get
		{
			return ᳮ;
		}
		set
		{
			if (ᳮ != value)
			{
				ᳮ = value;
				if (!ᳮ)
				{
					ᆵ();
				}
			}
		}
	}

	public bool BackEnabled
	{
		get
		{
			return _1CED;
		}
		set
		{
			if (_1CED != value)
			{
				_1CED = value;
				if (!_1CED)
				{
					ᆵ();
				}
			}
		}
	}

	public event EventHandler Back
	{
		add
		{
			EventHandler eventHandler = ᳯ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᳯ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᳯ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᳯ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler Forward
	{
		add
		{
			EventHandler eventHandler = ᳰ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᳰ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᳰ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᳰ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler Close
	{
		add
		{
			EventHandler eventHandler = ᆤ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᆤ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᆤ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᆤ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public TabSystemBox(TabStrip parent)
	{
		ӭ = parent;
	}

	public void Dispose()
	{
		ᆵ();
	}

	private void ᆵ()
	{
		_1CF4 = false;
		ᳵ = false;
		if (_৮ != null)
		{
			_৮.Stop();
			_৮.Enabled = false;
			((Component)(object)_৮).Dispose();
			_৮ = null;
		}
	}

	public void MouseLeave()
	{
		_0C4B();
		if (!DisplayRectangle.IsEmpty && (ᳱ != 0 || ᳲ != 0 || ᳳ != 0))
		{
			_1CF7();
			((Control)ӭ).Invalidate(DisplayRectangle, false);
		}
	}

	public void MouseHover()
	{
		_0C4B();
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
		if (ᳱ != 0 || ᳲ != 0 || ᳳ != 0)
		{
			_1CF7();
			flag = true;
		}
		if (!DisplayRectangle.Contains(e.X, e.Y))
		{
			if (flag)
			{
				((Control)ӭ).Invalidate(DisplayRectangle, false);
			}
			return;
		}
		if (ᳪ.Contains(e.X, e.Y))
		{
			if (CloseVisible && ӭ.SelectedTab != null)
			{
				if ((int)e.Button == 1048576)
				{
					ᳱ = eMouseState.Down;
				}
				else
				{
					ᳱ = eMouseState.Hot;
				}
				flag = true;
			}
		}
		else if (ᳬ.Contains(e.X, e.Y))
		{
			if (BackEnabled)
			{
				if ((int)e.Button == 1048576)
				{
					ᳲ = eMouseState.Down;
				}
				else
				{
					ᳲ = eMouseState.Hot;
				}
				flag = true;
			}
		}
		else if (ᳫ.Contains(e.X, e.Y) && ForwardEnabled)
		{
			if ((int)e.Button == 1048576)
			{
				ᳳ = eMouseState.Down;
			}
			else
			{
				ᳳ = eMouseState.Hot;
			}
			flag = true;
		}
		if (flag)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(2, 2);
			((Control)ӭ).Invalidate(displayRectangle, false);
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
		if (CloseVisible && ᳪ.Contains(e.X, e.Y) && ӭ.SelectedTab != null)
		{
			ᳱ = eMouseState.Down;
		}
		else if (BackEnabled && ᳬ.Contains(e.X, e.Y))
		{
			ᳲ = eMouseState.Down;
			if (ӭ.TabScrollAutoRepeat)
			{
				_1CF4 = true;
				ᳵ = false;
				if (_৮ == null)
				{
					_৮ = new Timer();
				}
				_৮.Interval = ӭ.TabScrollRepeatInterval;
				_৮.Tick += ᳶ;
				_৮.Start();
			}
		}
		else if (ForwardEnabled && ᳫ.Contains(e.X, e.Y))
		{
			ᳳ = eMouseState.Down;
			if (ӭ.TabScrollAutoRepeat)
			{
				ᳵ = true;
				_1CF4 = false;
				if (_৮ == null)
				{
					_৮ = new Timer();
				}
				_৮.Interval = ӭ.TabScrollRepeatInterval;
				_৮.Tick += ᳶ;
				_৮.Start();
			}
		}
		if (ᳱ != 0 || ᳲ != 0 || ᳳ != 0)
		{
			((Control)ӭ).Invalidate(DisplayRectangle, false);
		}
	}

	private void ᳶ(object _٠, EventArgs ؿ)
	{
		if (_1CF4)
		{
			if (ᳯ != null)
			{
				ᳯ(this, new EventArgs());
			}
		}
		else if (ᳵ)
		{
			if (ᳰ != null)
			{
				ᳰ(this, new EventArgs());
			}
		}
		else if (_৮ != null)
		{
			_৮.Stop();
		}
	}

	public void MouseUp(MouseEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		ᆵ();
		if ((int)e.Button != 1048576 || DisplayRectangle.IsEmpty)
		{
			return;
		}
		bool flag = false;
		if (ᳱ != 0 || ᳲ != 0 || ᳳ != 0)
		{
			flag = true;
		}
		if (CloseVisible && ᳪ.Contains(e.X, e.Y) && ӭ.SelectedTab != null)
		{
			if (ᳱ == eMouseState.Down && ᆤ != null)
			{
				ᆤ(this, new EventArgs());
			}
			_1CF7();
		}
		else if (BackEnabled && ᳬ.Contains(e.X, e.Y))
		{
			if (ᳲ == eMouseState.Down && ᳯ != null)
			{
				ᳯ(this, new EventArgs());
			}
			_1CF7();
			ᳲ = eMouseState.Hot;
		}
		else if (ForwardEnabled && ᳫ.Contains(e.X, e.Y))
		{
			if (ᳳ == eMouseState.Down && ᳰ != null)
			{
				ᳰ(this, new EventArgs());
			}
			_1CF7();
			ᳳ = eMouseState.Hot;
		}
		if (flag || ᳱ != 0 || ᳲ != 0 || ᳳ != 0)
		{
			((Control)ӭ).Invalidate(DisplayRectangle, false);
		}
	}

	public void MouseWheel(MouseEventArgs e)
	{
		if (ӭ.ᓥ <= 0)
		{
			return;
		}
		if (e.Delta > 0)
		{
			if (ᳯ != null && BackEnabled)
			{
				ᳯ(this, new EventArgs());
			}
		}
		else if (ᳰ != null && ForwardEnabled)
		{
			ᳰ(this, new EventArgs());
		}
	}

	private void _1CF7()
	{
		ᳱ = eMouseState.None;
		ᳲ = eMouseState.None;
		ᳳ = eMouseState.None;
	}

	public void Paint(Graphics g)
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		if (DisplayRectangle.IsEmpty)
		{
			return;
		}
		int x = DisplayRectangle.X;
		int y = DisplayRectangle.Y;
		Color color = ӭ.ColorScheme.NavigationBoxForeColor;
		if (color.IsEmpty)
		{
			color = g.GetNearestColor(ControlPaint.Light(ӭ.ColorScheme.TabItemText));
			if ((double)Math.Abs(color.GetBrightness() - ӭ.ColorScheme.TabBackground.GetBrightness()) <= 0.2)
			{
				color = (((double)ӭ.ColorScheme.TabBackground.GetBrightness() < 0.5) ? ControlPaint.Light(ӭ.ColorScheme.TabBackground) : ControlPaint.Dark(ӭ.ColorScheme.TabBackground));
			}
		}
		SolidBrush val = null;
		Pen val2 = new Pen(color, (float)Dpi.Width1);
		if (BackEnabled || ForwardEnabled)
		{
			val = new SolidBrush(color);
		}
		if (ӭ.TabAlignment == eTabStripAlignment.Left || ӭ.TabAlignment == eTabStripAlignment.Right)
		{
			y += (DisplayRectangle.Height - (ᱶ ? Dpi.Width32 : Dpi.Width18)) / 2;
			x += (DisplayRectangle.Width - Dpi.Width9) / 2;
			Point[] array = new Point[3];
			array[0].X = x + Dpi.Width4;
			array[0].Y = y;
			array[1].X = x;
			array[1].Y = y + Dpi.Height4;
			array[2].X = x + Dpi.Width8;
			array[2].Y = y + Dpi.Height4;
			ᳬ = new Rectangle(x - Dpi.Width3, y - Dpi.Height5, Dpi.Width14, Dpi.Height14);
			if (BackEnabled)
			{
				if (ᳲ == eMouseState.Hot)
				{
					_1CF8(g, ᳬ);
				}
				else if (ᳲ == eMouseState.Down)
				{
					_1CF9(g, ᳬ);
				}
				g.FillPolygon((Brush)(object)val, array);
			}
			g.DrawPolygon(val2, array);
			y += Dpi.Height14;
			array[0].X = x;
			array[0].Y = y;
			array[1].X = x + Dpi.Width8;
			array[1].Y = y;
			array[2].X = x + Dpi.Width4;
			array[2].Y = y + Dpi.Height4;
			ᳫ = new Rectangle(x - Dpi.Width3, y - Dpi.Height5, Dpi.Width14, Dpi.Height14);
			if (ForwardEnabled)
			{
				if (ᳳ == eMouseState.Hot)
				{
					_1CF8(g, ᳫ);
				}
				else if (ᳳ == eMouseState.Down)
				{
					_1CF9(g, ᳫ);
				}
				g.FillPolygon((Brush)(object)val, array);
			}
			g.DrawPolygon(val2, array);
			y += Dpi.Height13;
			if (CloseVisible)
			{
				ᳪ = new Rectangle(x - Dpi.Width3, y - Dpi.Height3, Dpi.Width14, Dpi.Height14);
				if (CloseVisible)
				{
					if (ᳱ == eMouseState.Hot)
					{
						_1CF8(g, ᳪ);
					}
					else if (ᳱ == eMouseState.Down)
					{
						_1CF9(g, ᳪ);
					}
				}
				g.DrawLine(val2, x + Dpi.Width1, y, x + Dpi.Width7, y + Dpi.Height6);
				g.DrawLine(val2, x + Dpi.Width1, y + Dpi.Height1, x + Dpi.Width7, y + Dpi.Height7);
				g.DrawLine(val2, x + Dpi.Width1, y + Dpi.Height6, x + Dpi.Width7, y);
				g.DrawLine(val2, x + Dpi.Width1, y + Dpi.Height7, x + Dpi.Width7, y + Dpi.Height1);
			}
		}
		else
		{
			x += (DisplayRectangle.Width - (ᱶ ? Dpi.Width32 : Dpi.Width18)) / 2;
			y += (DisplayRectangle.Height - Dpi.Height9) / 2;
			Point[] array2 = new Point[3];
			array2[0].X = x + Dpi.Width4;
			array2[0].Y = y;
			array2[1].X = x + Dpi.Width4;
			array2[1].Y = y + Dpi.Height8;
			array2[2].X = x;
			array2[2].Y = y + Dpi.Height4;
			ᳬ = new Rectangle(x - Dpi.Width5, y - Dpi.Height3, Dpi.Width14, Dpi.Height14);
			if (BackEnabled)
			{
				if (ᳲ == eMouseState.Hot)
				{
					_1CF8(g, ᳬ);
				}
				else if (ᳲ == eMouseState.Down)
				{
					_1CF9(g, ᳬ);
				}
				g.FillPolygon((Brush)(object)val, array2);
			}
			g.DrawPolygon(val2, array2);
			x += Dpi.Width14;
			array2[0].X = x;
			array2[0].Y = y;
			array2[1].X = x;
			array2[1].Y = y + Dpi.Height8;
			array2[2].X = x + Dpi.Width4;
			array2[2].Y = y + Dpi.Height4;
			ᳫ = new Rectangle(x - Dpi.Width5, y - Dpi.Height3, Dpi.Width14, Dpi.Height14);
			if (ForwardEnabled)
			{
				if (ᳳ == eMouseState.Hot)
				{
					_1CF8(g, ᳫ);
				}
				else if (ᳳ == eMouseState.Down)
				{
					_1CF9(g, ᳫ);
				}
				g.FillPolygon((Brush)(object)val, array2);
			}
			g.DrawPolygon(val2, array2);
			x += Dpi.Width13;
			if (CloseVisible)
			{
				ᳪ = new Rectangle(x - Dpi.Width4, y - Dpi.Height3, Dpi.Width14, Dpi.Height14);
				if (CloseVisible)
				{
					if (ᳱ == eMouseState.Hot)
					{
						_1CF8(g, ᳪ);
					}
					else if (ᳱ == eMouseState.Down)
					{
						_1CF9(g, ᳪ);
					}
				}
				g.DrawLine(val2, x, y + Dpi.Height1, x + Dpi.Width6, y + Dpi.Height7);
				g.DrawLine(val2, x + Dpi.Width1, y + Dpi.Height1, x + Dpi.Width7, y + Dpi.Height7);
				g.DrawLine(val2, x + Dpi.Width6, y + Dpi.Height1, x, y + Dpi.Height7);
				g.DrawLine(val2, x + Dpi.Width7, y + Dpi.Height1, x + Dpi.Width1, y + Dpi.Height7);
			}
		}
		val2.Dispose();
		if (val != null)
		{
			((Brush)val).Dispose();
		}
	}

	private void _1CF8(Graphics _٥, Rectangle ल)
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		Color color = ӭ.ColorScheme.TabItemHotBorder;
		Color color2 = ӭ.ColorScheme.TabItemHotBackground;
		Color color3 = ӭ.ColorScheme.TabItemHotBackground2;
		if ((ӭ.Style == eTabStripStyle.Office2003 || ӭ.Style == eTabStripStyle.VS2005 || ӭ.Style == eTabStripStyle.VS2005Document || ӭ.Style == eTabStripStyle.VS2005Dock) && color2.IsEmpty && color3.IsEmpty)
		{
			ColorScheme colorScheme = ((ӭ.Style != eTabStripStyle.Office2003) ? new ColorScheme(eDotNetBarStyle.VS2005) : new ColorScheme(eDotNetBarStyle.Office2003));
			color2 = colorScheme.ItemHotBackground;
			color3 = colorScheme.ItemHotBackground2;
			color = colorScheme.ItemHotBorder;
		}
		if (ӭ.Style == eTabStripStyle.OneNote || ӭ.Style == eTabStripStyle.Office2007Document || ӭ.Style == eTabStripStyle.Office2007Dock || ӭ.Style == eTabStripStyle.Office2003 || ӭ.Style == eTabStripStyle.VS2005 || ӭ.Style == eTabStripStyle.VS2005Document || ӭ.Style == eTabStripStyle.VS2005Dock)
		{
			if (color3.IsEmpty)
			{
				if (!color2.IsEmpty)
				{
					SolidBrush val = new SolidBrush(color2);
					try
					{
						_٥.FillRectangle((Brush)(object)val, ल);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				LinearGradientBrush val2 = new LinearGradientBrush(ल, color2, color3, (float)ӭ.ColorScheme.TabItemHotBackgroundGradientAngle);
				try
				{
					_٥.FillRectangle((Brush)(object)val2, ल);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			Pen val3 = new Pen(color, 1f);
			try
			{
				_٥.DrawRectangle(val3, ल);
				return;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Color nearestColor = _٥.GetNearestColor(ControlPaint.LightLight(ӭ.ColorScheme.TabBackground));
		BarFunctions._८(_٥, ल, (Border3DStyle)4, (Border3DSide)15, nearestColor, ॳ: false);
	}

	private void _1CF9(Graphics _٥, Rectangle ल)
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		Color color = ӭ.ColorScheme.TabItemHotBorder;
		Color color2 = ӭ.ColorScheme.TabItemHotBackground;
		Color color3 = ӭ.ColorScheme.TabItemHotBackground2;
		if ((ӭ.Style == eTabStripStyle.Office2003 || ӭ.Style == eTabStripStyle.VS2005 || ӭ.Style == eTabStripStyle.VS2005Document || ӭ.Style == eTabStripStyle.VS2005Dock) && color2.IsEmpty && color3.IsEmpty)
		{
			ColorScheme colorScheme = ((ӭ.Style != eTabStripStyle.Office2003) ? new ColorScheme(eDotNetBarStyle.VS2005) : new ColorScheme(eDotNetBarStyle.Office2003));
			color2 = colorScheme.ItemPressedBackground;
			color3 = colorScheme.ItemPressedBackground2;
			color = colorScheme.ItemPressedBorder;
		}
		if (ӭ.Style == eTabStripStyle.OneNote || ӭ.Style == eTabStripStyle.Office2007Document || ӭ.Style == eTabStripStyle.Office2007Dock || ӭ.Style == eTabStripStyle.Office2003 || ӭ.Style == eTabStripStyle.VS2005 || ӭ.Style == eTabStripStyle.VS2005 || ӭ.Style == eTabStripStyle.VS2005Document)
		{
			if (color3.IsEmpty)
			{
				if (!color2.IsEmpty)
				{
					SolidBrush val = new SolidBrush(color2);
					try
					{
						_٥.FillRectangle((Brush)(object)val, ल);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				LinearGradientBrush val2 = new LinearGradientBrush(ल, color3, color2, (float)ӭ.ColorScheme.TabItemHotBackgroundGradientAngle);
				try
				{
					_٥.FillRectangle((Brush)(object)val2, ल);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			Pen val3 = new Pen(color, 1f);
			try
			{
				_٥.DrawRectangle(val3, ल);
				return;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Color nearestColor = _٥.GetNearestColor(ControlPaint.LightLight(ӭ.ColorScheme.TabBackground));
		BarFunctions._८(_٥, ल, (Border3DStyle)2, (Border3DSide)15, nearestColor, ॳ: false);
	}

	private void _0C4B()
	{
		if (m_ToolTipWnd != null)
		{
			((Control)m_ToolTipWnd).Hide();
			((Component)(object)m_ToolTipWnd).Dispose();
			m_ToolTipWnd = null;
		}
	}
}
