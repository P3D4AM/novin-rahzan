using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

public class ScrollBarCore : IDisposable
{
	internal enum eScrollPart
	{
		ThumbIncrease,
		ThumbDecrease,
		Track,
		Control,
		None
	}

	private Rectangle m_DisplayRectangle = Rectangle.Empty;

	private eOrientation m_Orientation = eOrientation.Vertical;

	private Size m_VThumbSize = new Size(15, 17);

	private Size m_HThumbSize = new Size(17, 15);

	private Rectangle m_ThumbDecrease = Rectangle.Empty;

	private Rectangle m_ThumbIncrease = Rectangle.Empty;

	private Rectangle m_Track = Rectangle.Empty;

	private eScrollPart m_MouseOverPart = eScrollPart.None;

	private Control m_ParentControl;

	private bool m_MouseDown;

	private Timer m_ClickTimer;

	private int m_MouseDownTrackOffset;

	private Bitmap m_CashedView;

	private bool m_ViewExpired = true;

	private bool m_PassiveScrollBar;

	private bool m_IsAppScrollBarStyle;

	private bool m_IsScrollBarParent;

	private bool m_IsAdvScrollBar;

	private int m_AutoClickCount;

	[CompilerGenerated]
	private ScrollEventHandler m_Scroll;

	private bool _IsPainting;

	private bool m_HasBorder = true;

	private bool m_SideBorderOnly;

	private int m_Minimum;

	private int m_Maximum;

	private int m_SmallChange = 1;

	private int m_LargeChange = 16;

	private int m_Value;

	private bool _RaiseEndScroll;

	private bool m_Visible = true;

	private bool m_Enabled = true;

	private bool IsFocused
	{
		get
		{
			if (m_IsAdvScrollBar && m_ParentControl.Focused)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsMouseDown => m_MouseDown;

	private ScrollOrientation ScrollOrientation
	{
		get
		{
			if (Orientation == eOrientation.Horizontal)
			{
				return (ScrollOrientation)0;
			}
			return (ScrollOrientation)1;
		}
	}

	internal bool HasBorder
	{
		get
		{
			return m_HasBorder;
		}
		set
		{
			m_HasBorder = value;
		}
	}

	internal bool SideBorderOnly
	{
		get
		{
			return m_SideBorderOnly;
		}
		set
		{
			m_SideBorderOnly = value;
		}
	}

	public Rectangle ThumbDecreaseRectangle
	{
		get
		{
			return m_ThumbDecrease;
		}
		set
		{
			m_ThumbDecrease = value;
			DisposeCashedView();
		}
	}

	public Rectangle ThumbIncreaseRectangle
	{
		get
		{
			return m_ThumbIncrease;
		}
		set
		{
			m_ThumbIncrease = value;
			DisposeCashedView();
		}
	}

	public Rectangle TrackRectangle
	{
		get
		{
			return m_Track;
		}
		set
		{
			m_Track = value;
			DisposeCashedView();
		}
	}

	public Rectangle DisplayRectangle
	{
		get
		{
			return m_DisplayRectangle;
		}
		set
		{
			m_DisplayRectangle = value;
			DisposeCashedView();
			UpdateLayout();
		}
	}

	public eOrientation Orientation
	{
		get
		{
			return m_Orientation;
		}
		set
		{
			m_Orientation = value;
			DisposeCashedView();
		}
	}

	public int Minimum
	{
		get
		{
			return m_Minimum;
		}
		set
		{
			m_Minimum = value;
			DisposeCashedView();
			UpdateLayout();
		}
	}

	public int Maximum
	{
		get
		{
			return m_Maximum;
		}
		set
		{
			m_Maximum = value;
			DisposeCashedView();
			UpdateLayout();
		}
	}

	public int SmallChange
	{
		get
		{
			return m_SmallChange;
		}
		set
		{
			m_SmallChange = value;
		}
	}

	public int LargeChange
	{
		get
		{
			return m_LargeChange;
		}
		set
		{
			m_LargeChange = value;
			DisposeCashedView();
			UpdateLayout();
		}
	}

	public int Value
	{
		get
		{
			return m_Value;
		}
		set
		{
			value = NormalizeValue(value);
			if (m_Value == value && (!m_IsScrollBarParent || m_IsAdvScrollBar))
			{
				return;
			}
			m_Value = value;
			if (!m_PassiveScrollBar)
			{
				DisposeCashedView();
				UpdateLayout();
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this, new EventArgs());
				}
				Invalidate();
			}
		}
	}

	internal eScrollPart MouseOverPart
	{
		get
		{
			return m_MouseOverPart;
		}
		set
		{
			m_MouseOverPart = value;
		}
	}

	public bool Visible
	{
		get
		{
			return m_Visible;
		}
		set
		{
			m_Visible = value;
		}
	}

	public Control ParentControl
	{
		get
		{
			return m_ParentControl;
		}
		set
		{
			m_ParentControl = value;
			if (m_ParentControl == null)
			{
				DisposeTimer();
			}
		}
	}

	public bool Enabled
	{
		get
		{
			return m_Enabled;
		}
		set
		{
			if (m_Enabled != value)
			{
				m_Enabled = value;
				Invalidate();
			}
		}
	}

	public bool IsAppScrollBarStyle
	{
		get
		{
			return m_IsAppScrollBarStyle;
		}
		set
		{
			m_IsAppScrollBarStyle = value;
			DisposeCashedView();
		}
	}

	public event EventHandler ValueChanged;

	public event ScrollEventHandler Scroll
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ScrollEventHandler val = this.m_Scroll;
			ScrollEventHandler val2;
			do
			{
				val2 = val;
				ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_Scroll, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ScrollEventHandler val = this.m_Scroll;
			ScrollEventHandler val2;
			do
			{
				val2 = val;
				ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_Scroll, value2, val2);
			}
			while (val != val2);
		}
	}

	public ScrollBarCore()
		: this(null)
	{
	}

	public ScrollBarCore(Control parentControl)
		: this(parentControl, isPassive: false)
	{
	}

	public ScrollBarCore(Control parentControl, bool isPassive)
	{
		m_ParentControl = parentControl;
		m_IsScrollBarParent = m_ParentControl is ScrollBar || m_ParentControl is ScrollBarAdv;
		m_IsAdvScrollBar = m_ParentControl is ScrollBarAdv;
		m_PassiveScrollBar = isPassive;
		DisposeCashedView();
	}

	internal void DisposeCashedView()
	{
		if (m_CashedView != null)
		{
			((Image)m_CashedView).Dispose();
			m_CashedView = null;
		}
	}

	private ScrollBarPainter GetScrollBarPainter()
	{
		ScrollBarPainter scrollBarPainter = null;
		if (StyleManager.Style == eStyle.Metro)
		{
			MetroScrollBarPainter metroScrollBarPainter = new MetroScrollBarPainter();
			metroScrollBarPainter.ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			metroScrollBarPainter.AppStyleScrollBar = m_IsAppScrollBarStyle;
			return metroScrollBarPainter;
		}
		Office2007ScrollBarPainter office2007ScrollBarPainter = new Office2007ScrollBarPainter();
		office2007ScrollBarPainter.ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		office2007ScrollBarPainter.AppStyleScrollBar = m_IsAppScrollBarStyle;
		return office2007ScrollBarPainter;
	}

	public void Paint(ItemPaintArgs p)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Invalid comparison between Unknown and I4
		if (_IsPainting)
		{
			return;
		}
		_IsPainting = true;
		try
		{
			if (m_DisplayRectangle.IsEmpty || m_DisplayRectangle.Width <= 0 || m_DisplayRectangle.Height <= 0 || p.Graphics == null)
			{
				return;
			}
			ScrollBarPainter scrollBarPainter = GetScrollBarPainter();
			if (m_CashedView == null)
			{
				try
				{
					m_CashedView = new Bitmap(m_DisplayRectangle.Width, m_DisplayRectangle.Height, p.Graphics);
				}
				catch
				{
					return;
				}
				m_CashedView.MakeTransparent();
				m_ViewExpired = true;
			}
			if (!m_ViewExpired)
			{
				Bitmap cashedView = m_CashedView;
				if (cashedView != null)
				{
					p.Graphics.DrawImageUnscaled((Image)(object)cashedView, m_DisplayRectangle.Location);
				}
				return;
			}
			Graphics val = Graphics.FromImage((Image)(object)m_CashedView);
			try
			{
				if (!m_DisplayRectangle.Location.IsEmpty)
				{
					val.TranslateTransform((float)(-m_DisplayRectangle.X), (float)(-m_DisplayRectangle.Y));
				}
				eScrollBarState state = ((!m_Enabled) ? eScrollBarState.Disabled : eScrollBarState.Normal);
				if (m_Enabled && (m_MouseOverPart != eScrollPart.None || IsFocused))
				{
					state = eScrollBarState.ControlMouseOver;
				}
				bool rtl = false;
				if (m_ParentControl != null)
				{
					rtl = (int)m_ParentControl.RightToLeft == 1;
				}
				scrollBarPainter.PaintBackground(val, m_DisplayRectangle, state, m_Orientation == eOrientation.Horizontal, m_SideBorderOnly, rtl);
				eScrollThumbPosition position = eScrollThumbPosition.Top;
				if (m_Orientation == eOrientation.Horizontal)
				{
					position = eScrollThumbPosition.Left;
				}
				if (m_MouseOverPart == eScrollPart.ThumbDecrease && m_Enabled)
				{
					state = ((!m_MouseDown) ? eScrollBarState.PartMouseOver : eScrollBarState.Pressed);
				}
				scrollBarPainter.PaintThumb(val, m_ThumbDecrease, position, state);
				position = eScrollThumbPosition.Bottom;
				if (m_MouseOverPart == eScrollPart.ThumbIncrease && m_Enabled)
				{
					state = ((!m_MouseDown) ? eScrollBarState.PartMouseOver : eScrollBarState.Pressed);
				}
				else if (m_MouseOverPart != eScrollPart.None && m_Enabled)
				{
					state = eScrollBarState.ControlMouseOver;
				}
				if (m_Orientation == eOrientation.Horizontal)
				{
					position = eScrollThumbPosition.Right;
				}
				scrollBarPainter.PaintThumb(val, m_ThumbIncrease, position, state);
				if (m_MouseOverPart == eScrollPart.Track && m_Enabled)
				{
					state = ((!m_MouseDown) ? eScrollBarState.PartMouseOver : eScrollBarState.Pressed);
				}
				else if ((m_MouseOverPart != eScrollPart.None || IsFocused) && m_Enabled)
				{
					state = eScrollBarState.ControlMouseOver;
				}
				if (m_Orientation == eOrientation.Horizontal)
				{
					scrollBarPainter.PaintTrackHorizontal(val, m_Track, state);
				}
				else
				{
					scrollBarPainter.PaintTrackVertical(val, m_Track, state);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			p.Graphics.DrawImageUnscaled((Image)(object)m_CashedView, m_DisplayRectangle.Location);
			m_ViewExpired = false;
		}
		finally
		{
			_IsPainting = false;
		}
	}

	public void MouseMove(MouseEventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (!m_Enabled)
		{
			return;
		}
		Point point = new Point(e.X, e.Y);
		if (m_MouseDown && m_MouseOverPart == eScrollPart.Track)
		{
			if (!m_PassiveScrollBar)
			{
				int value = ValueFromMouseCoordinates(point);
				SetValue(value, (ScrollEventType)5);
			}
		}
		else
		{
			if ((int)e.Button != 0 && m_MouseOverPart != eScrollPart.None)
			{
				return;
			}
			if (m_DisplayRectangle.Contains(point))
			{
				eScrollPart eScrollPart = eScrollPart.Control;
				if (m_ThumbDecrease.Contains(point))
				{
					eScrollPart = eScrollPart.ThumbDecrease;
				}
				else if (m_ThumbIncrease.Contains(point))
				{
					eScrollPart = eScrollPart.ThumbIncrease;
				}
				else if (m_Track.Contains(point))
				{
					eScrollPart = eScrollPart.Track;
				}
				if (m_MouseOverPart != eScrollPart)
				{
					m_MouseOverPart = eScrollPart;
					Invalidate();
				}
			}
			else if (m_MouseOverPart != eScrollPart.None)
			{
				m_MouseOverPart = eScrollPart.None;
				Invalidate();
			}
		}
	}

	private int ValueFromMouseCoordinates(Point p)
	{
		if (m_Orientation == eOrientation.Vertical)
		{
			int val = p.Y - m_MouseDownTrackOffset;
			val = Math.Max(val, m_ThumbDecrease.Bottom);
			val = Math.Min(val, m_ThumbIncrease.Y - GetTrackSize());
			val -= m_ThumbDecrease.Bottom;
			int num = GetAvailableTrackArea() - GetTrackSize();
			return (int)((float)(GetMaximumValue() - Minimum) * ((float)val / (float)num));
		}
		int val2 = p.X - m_MouseDownTrackOffset;
		val2 = Math.Max(val2, m_ThumbDecrease.Right);
		val2 = Math.Min(val2, m_ThumbIncrease.X - GetTrackSize());
		val2 -= m_ThumbDecrease.Right;
		int num2 = GetAvailableTrackArea() - GetTrackSize();
		return (int)((float)(GetMaximumValue() - Minimum) * ((float)val2 / (float)num2));
	}

	public void MouseLeave()
	{
		if (m_MouseOverPart != eScrollPart.None && (!m_MouseDown || (m_MouseOverPart != eScrollPart.Track && (m_MouseOverPart == eScrollPart.None || !m_PassiveScrollBar))))
		{
			m_MouseOverPart = eScrollPart.None;
			Invalidate();
		}
	}

	public void MouseDown(MouseEventArgs e)
	{
		if (m_MouseOverPart == eScrollPart.None)
		{
			return;
		}
		m_MouseDown = true;
		if (m_PassiveScrollBar)
		{
			m_ViewExpired = true;
			return;
		}
		if (m_Orientation == eOrientation.Vertical)
		{
			m_MouseDownTrackOffset = e.Y - m_Track.Y;
		}
		else
		{
			m_MouseDownTrackOffset = e.X - m_Track.X;
		}
		if (m_MouseOverPart == eScrollPart.ThumbDecrease)
		{
			SetValue(Value - m_SmallChange, (ScrollEventType)0);
		}
		else if (m_MouseOverPart == eScrollPart.ThumbIncrease)
		{
			SetValue(Value + m_SmallChange, (ScrollEventType)1);
		}
		else if (m_MouseOverPart == eScrollPart.Track)
		{
			Invalidate();
			if (m_ParentControl != null)
			{
				m_ParentControl.Capture = true;
			}
		}
		else if (m_MouseOverPart == eScrollPart.Control)
		{
			MousePageDown(new Point(e.X, e.Y));
		}
		SetupClickTimer();
	}

	private void MousePageDown(Point p)
	{
		if ((m_Orientation != eOrientation.Vertical || (p.Y >= m_Track.Y && p.Y <= m_Track.Bottom)) && (m_Orientation != 0 || (p.X >= m_Track.X && p.X <= m_Track.Right)))
		{
			return;
		}
		if ((m_Orientation == eOrientation.Vertical && m_Track.Y > p.Y) || (m_Orientation == eOrientation.Horizontal && m_Track.X > p.X))
		{
			SetValue(Value - LargeChange, (ScrollEventType)2);
		}
		else if (m_IsScrollBarParent)
		{
			if (Value + LargeChange > GetMaximumValue())
			{
				SetValue(GetMaximumValue(), (ScrollEventType)3);
			}
			else
			{
				SetValue(Value + LargeChange, (ScrollEventType)3);
			}
		}
		else
		{
			SetValue(Value + LargeChange, (ScrollEventType)3);
		}
	}

	public void MouseUp(MouseEventArgs e)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		if (m_ParentControl != null && m_ParentControl.Capture)
		{
			m_ParentControl.Capture = false;
		}
		if (m_MouseDown)
		{
			if (m_MouseOverPart != eScrollPart.None && _RaiseEndScroll)
			{
				if (m_MouseOverPart == eScrollPart.Track)
				{
					OnScroll(new ScrollEventArgs((ScrollEventType)4, Value, Value, ScrollOrientation));
				}
				OnScroll(new ScrollEventArgs((ScrollEventType)8, Value, Value, ScrollOrientation));
				_RaiseEndScroll = false;
			}
			DisposeTimer();
			m_MouseDown = false;
			Invalidate();
		}
		if (!DisplayRectangle.Contains(e.X, e.Y))
		{
			MouseLeave();
		}
	}

	private void DisposeTimer()
	{
		Timer clickTimer = m_ClickTimer;
		m_ClickTimer = null;
		if (clickTimer != null)
		{
			clickTimer.Enabled = false;
			clickTimer.Tick -= ClickTimer_Tick;
			((Component)(object)clickTimer).Dispose();
		}
	}

	private void SetupClickTimer()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		if (m_ClickTimer == null)
		{
			m_AutoClickCount = 0;
			m_ClickTimer = new Timer();
			m_ClickTimer.Interval = 100;
			m_ClickTimer.Tick += ClickTimer_Tick;
			m_ClickTimer.Enabled = true;
		}
	}

	private void ClickTimer_Tick(object sender, EventArgs e)
	{
		Control parentControl = m_ParentControl;
		if (m_MouseDown && m_MouseOverPart == eScrollPart.ThumbDecrease)
		{
			SetValue(Value - m_SmallChange, (ScrollEventType)0);
		}
		else if (m_MouseDown && m_MouseOverPart == eScrollPart.ThumbIncrease)
		{
			SetValue(Value + m_SmallChange, (ScrollEventType)1);
		}
		else if (m_MouseDown && m_MouseOverPart == eScrollPart.Control && parentControl != null)
		{
			Point point = parentControl.PointToClient(Control.MousePosition);
			if (m_Track.Contains(point))
			{
				DisposeTimer();
				m_MouseDown = false;
			}
			else
			{
				MousePageDown(point);
			}
		}
		m_AutoClickCount++;
		if (m_AutoClickCount > 4 && m_ClickTimer != null && m_ClickTimer.Interval > 20)
		{
			Timer clickTimer = m_ClickTimer;
			clickTimer.Interval -= 10;
		}
	}

	public void Invalidate()
	{
		m_ViewExpired = true;
		Control parentControl = m_ParentControl;
		if (parentControl != null && !m_PassiveScrollBar)
		{
			parentControl.Invalidate(m_DisplayRectangle);
			parentControl.Update();
		}
	}

	private void UpdateLayout()
	{
		if (!m_PassiveScrollBar)
		{
			Rectangle displayRectangle = m_DisplayRectangle;
			if (m_HasBorder)
			{
				displayRectangle.Inflate(-1, -1);
			}
			if (m_Orientation == eOrientation.Vertical)
			{
				m_ThumbDecrease = new Rectangle(displayRectangle.X, displayRectangle.Y, displayRectangle.Width, m_VThumbSize.Height);
				m_ThumbIncrease = new Rectangle(displayRectangle.X, displayRectangle.Bottom - m_VThumbSize.Height, displayRectangle.Width, m_VThumbSize.Height);
				int trackSize = GetTrackSize();
				m_Track = new Rectangle(displayRectangle.X, displayRectangle.Y + GetTrackPosition(), displayRectangle.Width, trackSize);
			}
			else
			{
				m_ThumbDecrease = new Rectangle(displayRectangle.X, displayRectangle.Y, m_HThumbSize.Width, displayRectangle.Height);
				m_ThumbIncrease = new Rectangle(displayRectangle.Right - m_HThumbSize.Width, displayRectangle.Y, m_HThumbSize.Width, displayRectangle.Height);
				int trackSize2 = GetTrackSize();
				m_Track = new Rectangle(displayRectangle.X + GetTrackPosition(), displayRectangle.Y, trackSize2, displayRectangle.Height);
			}
			if (!(m_ParentControl is ScrollBar))
			{
				Invalidate();
			}
		}
	}

	private int GetTrackSize()
	{
		int num = 0;
		int availableTrackArea = GetAvailableTrackArea();
		num = (int)((float)availableTrackArea * ((float)m_LargeChange / (float)(m_Maximum - m_Minimum + 1)));
		num = Math.Max(14, num);
		return Math.Min(num, availableTrackArea);
	}

	private int GetAvailableTrackArea()
	{
		Rectangle displayRectangle = m_DisplayRectangle;
		if (m_HasBorder)
		{
			displayRectangle.Inflate(-1, -1);
		}
		int height = displayRectangle.Height;
		height = ((m_Orientation != 0) ? (height - m_VThumbSize.Height * 2) : (displayRectangle.Width - m_HThumbSize.Width * 2));
		return Math.Max(height, 8);
	}

	private int GetTrackPosition()
	{
		int num = 0;
		try
		{
			int trackSize = GetTrackSize();
			int num2 = GetAvailableTrackArea() - trackSize;
			num = Math.Min(num2, Math.Max((int)((float)num2 * ((float)m_Value / (float)GetMaximumValue())), 0));
			num = ((m_Orientation != eOrientation.Vertical) ? (num + m_HThumbSize.Width) : (num + m_VThumbSize.Height));
		}
		catch
		{
		}
		return num;
	}

	private void SetValue(int value, ScrollEventType type)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		value = NormalizeValue(value);
		OnScroll(new ScrollEventArgs(type, m_Value, value, ScrollOrientation));
		Value = value;
		_RaiseEndScroll = true;
	}

	private int NormalizeValue(int value)
	{
		int num = (m_IsAdvScrollBar ? GetMaximumValue() : m_Maximum);
		if (value < m_Minimum)
		{
			value = m_Minimum;
		}
		if (value > num)
		{
			value = num;
		}
		return value;
	}

	private void OnScroll(ScrollEventArgs e)
	{
		ScrollEventHandler scroll = this.Scroll;
		if (scroll != null)
		{
			scroll.Invoke((object)this, e);
		}
	}

	internal int GetMaximumValue()
	{
		if (m_IsScrollBarParent)
		{
			return m_Maximum - m_LargeChange + 1;
		}
		return m_Maximum;
	}

	internal bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		if ((int)keyData == 34)
		{
			SetValue(Value + m_LargeChange, (ScrollEventType)3);
			return true;
		}
		if ((int)keyData == 33)
		{
			SetValue(Value - m_LargeChange, (ScrollEventType)3);
			return true;
		}
		if ((int)keyData == 38 || (int)keyData == 37)
		{
			SetValue(Value - m_SmallChange, (ScrollEventType)0);
			return true;
		}
		if ((int)keyData == 40 || (int)keyData == 39)
		{
			SetValue(Value + m_SmallChange, (ScrollEventType)1);
			return true;
		}
		if ((int)keyData == 36)
		{
			SetValue(m_Minimum, (ScrollEventType)6);
			return true;
		}
		if ((int)keyData == 35)
		{
			SetValue(m_Maximum, (ScrollEventType)7);
			return true;
		}
		return false;
	}

	public void Dispose()
	{
		DisposeTimer();
	}

	internal void InvalidateScrollBar()
	{
		DisposeCashedView();
		Control parentControl = m_ParentControl;
		if (parentControl != null)
		{
			parentControl.Invalidate();
		}
	}
}
