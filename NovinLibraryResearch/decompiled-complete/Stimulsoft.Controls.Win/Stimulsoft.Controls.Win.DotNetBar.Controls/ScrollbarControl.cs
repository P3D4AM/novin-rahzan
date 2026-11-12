using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
public class ScrollbarControl : ControlWithBackgroundStyle
{
	private const int ScrollPositionUpdateDelay = 400;

	private HScrollBarAdv _HScrollBar;

	private VScrollBarAdv _VScrollBar;

	private Control _Thumb;

	private eScrollBarAppearance _ScrollBarAppearance;

	private bool _UpdatingScrollbars;

	private Control _ScrollOverrideControl;

	private bool _InternalVScrollPositionUpdated;

	private bool _InternalHScrollPositionUpdated;

	[DefaultValue(eScrollBarAppearance.Default)]
	[Category("Appearance")]
	[Description("Gets or sets the scroll-bar visual style.")]
	public eScrollBarAppearance ScrollBarAppearance
	{
		get
		{
			return _ScrollBarAppearance;
		}
		set
		{
			_ScrollBarAppearance = value;
			OnScrollBarAppearanceChanged();
		}
	}

	internal Control ScrollOverrideControl
	{
		get
		{
			return _ScrollOverrideControl;
		}
		set
		{
			if (value != _ScrollOverrideControl)
			{
				if (!(value is IScrollBarOverrideSupport))
				{
					throw new ArgumentException("ScrollOverrideControl must implement IScrollBarOverrideSupport interface.");
				}
				Control scrollOverrideControl = _ScrollOverrideControl;
				_ScrollOverrideControl = value;
				OnScrollOverrideControlChanged(scrollOverrideControl, value);
			}
		}
	}

	public ScrollbarControl()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)(0x2016 | DisplayHelp.DoubleBufferFlag), true);
		_HScrollBar = new HScrollBarAdv();
		((Control)_HScrollBar).Visible = false;
		_HScrollBar.Appearance = _ScrollBarAppearance;
		_HScrollBar.Scroll += new ScrollEventHandler(HScrollBarScroll);
		_VScrollBar = new VScrollBarAdv();
		((Control)_VScrollBar).Visible = false;
		_VScrollBar.Appearance = _ScrollBarAppearance;
		_VScrollBar.Scroll += new ScrollEventHandler(VScrollBarScroll);
		((Control)this).Controls.Add((Control)(object)_HScrollBar);
		((Control)this).Controls.Add((Control)(object)_VScrollBar);
		_Thumb = new Control();
		_Thumb.Visible = false;
		_Thumb.BackColor = SystemColors.Window;
		((Control)this).Controls.Add(_Thumb);
	}

	private void OnScrollBarAppearanceChanged()
	{
		if (_VScrollBar != null)
		{
			_VScrollBar.Appearance = _ScrollBarAppearance;
		}
		if (_HScrollBar != null)
		{
			_HScrollBar.Appearance = _ScrollBarAppearance;
		}
	}

	private void UpdateScrollOverrideControlBounds()
	{
		if (_ScrollOverrideControl != null)
		{
			_ScrollOverrideControl.Bounds = GetScrollOverrideControlBounds();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		UpdateScrollOverrideControlBounds();
		UpdateScrollBars();
		((Control)this).OnResize(e);
	}

	protected override void OnVisualPropertyChanged()
	{
		base.OnVisualPropertyChanged();
		UpdateScrollOverrideControlBounds();
		UpdateScrollBars();
	}

	protected virtual void UpdateScrollBarsDelayed()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		InvokeDelayed((MethodInvoker)delegate
		{
			UpdateScrollBars();
		}, 400);
	}

	protected virtual void UpdateScrollBars()
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		if (!((Control)this).IsHandleCreated || _UpdatingScrollbars)
		{
			return;
		}
		_UpdatingScrollbars = true;
		try
		{
			Control scrollOverrideControl = _ScrollOverrideControl;
			if (scrollOverrideControl == null)
			{
				if (((Control)_HScrollBar).Visible)
				{
					((Control)_HScrollBar).Visible = false;
				}
				if (((Control)_VScrollBar).Visible)
				{
					((Control)_VScrollBar).Visible = false;
				}
				return;
			}
			WinApi.SCROLLBARINFO psbi = default(WinApi.SCROLLBARINFO);
			psbi.cbSize = Marshal.SizeOf(psbi);
			WinApi.GetScrollBarInfo(scrollOverrideControl.Handle, 4294967291u, ref psbi);
			if (psbi.rgstate[0] != 32768)
			{
				Rectangle rectangle = psbi.rcScrollBar.ToRectangle();
				rectangle.Location = ((Control)this).PointToClient(rectangle.Location);
				Rectangle bounds = _ScrollOverrideControl.Bounds;
				if (!bounds.Contains(rectangle))
				{
					if (rectangle.Right > bounds.Right)
					{
						rectangle.X = bounds.Right - rectangle.Width;
					}
					else if (rectangle.X < bounds.X)
					{
						rectangle.X = 0;
					}
				}
				if (((Control)_VScrollBar).Bounds != rectangle)
				{
					((Control)_VScrollBar).Bounds = rectangle;
					UpdateVerticalScrollBarValues();
				}
				if (!((Control)_VScrollBar).Visible)
				{
					((Control)_VScrollBar).Visible = true;
					((Control)_VScrollBar).Refresh();
					InvokeDelayed((MethodInvoker)delegate
					{
						UpdateVerticalScrollBarValues();
					}, 400);
				}
				if (psbi.rgstate[0] == 1)
				{
					((Control)_VScrollBar).Enabled = false;
				}
				else if (!((Control)_VScrollBar).Enabled)
				{
					((Control)_VScrollBar).Enabled = true;
				}
			}
			else if (((Control)_VScrollBar).Visible)
			{
				((Control)_VScrollBar).Visible = false;
			}
			psbi = default(WinApi.SCROLLBARINFO);
			psbi.cbSize = Marshal.SizeOf(psbi);
			WinApi.GetScrollBarInfo(scrollOverrideControl.Handle, 4294967290u, ref psbi);
			if (psbi.rgstate[0] != 32768)
			{
				Rectangle rectangle2 = psbi.rcScrollBar.ToRectangle();
				rectangle2.Location = ((Control)this).PointToClient(rectangle2.Location);
				Rectangle bounds2 = _ScrollOverrideControl.Bounds;
				if (!bounds2.Contains(rectangle2) && rectangle2.Bottom > bounds2.Bottom)
				{
					rectangle2.Y = bounds2.Bottom - rectangle2.Height;
				}
				if (((Control)_VScrollBar).Visible && rectangle2.Width == bounds2.Width)
				{
					rectangle2.Width -= ((Control)_VScrollBar).Width;
				}
				if (((Control)_HScrollBar).Bounds != rectangle2)
				{
					((Control)_HScrollBar).Bounds = rectangle2;
					UpdateHorizontalScrollBarValues();
				}
				if (!((Control)_HScrollBar).Visible)
				{
					((Control)_HScrollBar).Visible = true;
					((Control)_HScrollBar).Refresh();
					InvokeDelayed((MethodInvoker)delegate
					{
						UpdateHorizontalScrollBarValues();
					}, 400);
				}
				if (psbi.rgstate[0] == 1)
				{
					((Control)_HScrollBar).Enabled = false;
				}
				else if (!((Control)_HScrollBar).Enabled)
				{
					((Control)_HScrollBar).Enabled = true;
				}
			}
			else if (((Control)_HScrollBar).Visible)
			{
				((Control)_HScrollBar).Visible = false;
			}
			if (((Control)_HScrollBar).Visible && ((Control)_VScrollBar).Visible)
			{
				_Thumb.Bounds = new Rectangle(((Control)_VScrollBar).Left, ((Control)_VScrollBar).Bounds.Bottom, ((Control)_VScrollBar).Width, ((Control)_HScrollBar).Height);
				_Thumb.Visible = true;
			}
			else
			{
				_Thumb.Visible = false;
			}
		}
		finally
		{
			_UpdatingScrollbars = false;
		}
	}

	private Rectangle GetScrollOverrideControlBounds()
	{
		return GetContentRectangle();
	}

	protected virtual void OnScrollOverrideControlChanged(Control oldValue, Control newValue)
	{
		if (oldValue != null)
		{
			IScrollBarOverrideSupport scrollBarOverrideSupport = (IScrollBarOverrideSupport)newValue;
			scrollBarOverrideSupport.NonClientSizeChanged -= NonClientSizeChanged;
			scrollBarOverrideSupport.ScrollBarValueChanged -= ControlScrollBarValueChanged;
			((Control)this).Controls.Remove(oldValue);
		}
		if (newValue != null)
		{
			IScrollBarOverrideSupport scrollBarOverrideSupport2 = (IScrollBarOverrideSupport)newValue;
			scrollBarOverrideSupport2.NonClientSizeChanged += NonClientSizeChanged;
			scrollBarOverrideSupport2.ScrollBarValueChanged += ControlScrollBarValueChanged;
			((Control)this).Controls.Add(newValue);
		}
		UpdateScrollOverrideControlBounds();
		if (((Control)this).IsHandleCreated)
		{
			UpdateScrollBars();
		}
	}

	private void VScrollBarScroll(object sender, ScrollEventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if (e.NewValue != e.OldValue || (int)e.Type == 8)
		{
			_InternalVScrollPositionUpdated = true;
			WinApi.SendMessage(_ScrollOverrideControl.Handle, 277, WinApi.CreateLParam(MapVScrollType(e.Type), e.NewValue), IntPtr.Zero);
			_InternalVScrollPositionUpdated = false;
			if ((int)e.Type == 8)
			{
				UpdateVerticalScrollBarValues();
			}
		}
	}

	private void HScrollBarScroll(object sender, ScrollEventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if (e.NewValue != e.OldValue || (int)e.Type == 8)
		{
			_InternalHScrollPositionUpdated = true;
			WinApi.SendMessage(_ScrollOverrideControl.Handle, 276, WinApi.CreateLParam(MapHScrollType(e.Type), e.NewValue), IntPtr.Zero);
			_InternalHScrollPositionUpdated = false;
			if ((int)e.Type == 8)
			{
				UpdateHorizontalScrollBarValues();
			}
		}
	}

	private static int MapVScrollType(ScrollEventType type)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		if ((int)type == 8)
		{
			return 8;
		}
		if ((int)type == 6)
		{
			return 6;
		}
		if ((int)type == 2)
		{
			return 2;
		}
		if ((int)type == 3)
		{
			return 3;
		}
		if ((int)type == 7)
		{
			return 7;
		}
		if ((int)type == 0)
		{
			return 0;
		}
		if ((int)type == 1)
		{
			return 1;
		}
		if ((int)type == 4)
		{
			return 4;
		}
		if ((int)type == 5)
		{
			return 5;
		}
		return 0;
	}

	private static int MapHScrollType(ScrollEventType type)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		if ((int)type == 8)
		{
			return 8;
		}
		if ((int)type == 6)
		{
			return 6;
		}
		if ((int)type == 2)
		{
			return 2;
		}
		if ((int)type == 3)
		{
			return 3;
		}
		if ((int)type == 7)
		{
			return 7;
		}
		if ((int)type == 0)
		{
			return 0;
		}
		if ((int)type == 1)
		{
			return 1;
		}
		if ((int)type == 4)
		{
			return 4;
		}
		if ((int)type == 5)
		{
			return 5;
		}
		return 0;
	}

	private void UpdateVerticalScrollBarValues()
	{
		if (_InternalVScrollPositionUpdated)
		{
			return;
		}
		WinApi.SCROLLINFO lpsi = default(WinApi.SCROLLINFO);
		lpsi.cbSize = Marshal.SizeOf(lpsi);
		lpsi.fMask = WinApi.ScrollInfoMask.SIF_RANGE | WinApi.ScrollInfoMask.SIF_PAGE | WinApi.ScrollInfoMask.SIF_POS;
		if (WinApi.GetScrollInfo(_ScrollOverrideControl.Handle, WinApi.SBOrientation.SB_VERT, ref lpsi))
		{
			if (_VScrollBar.Minimum != lpsi.nMin)
			{
				_VScrollBar.Minimum = lpsi.nMin;
			}
			if (_VScrollBar.Maximum != lpsi.nMax)
			{
				_VScrollBar.Maximum = lpsi.nMax;
			}
			if (_VScrollBar.Value != lpsi.nPos)
			{
				_VScrollBar.Value = lpsi.nPos;
			}
			if (_VScrollBar.LargeChange != lpsi.nPage)
			{
				_VScrollBar.LargeChange = lpsi.nPage;
			}
		}
	}

	private void UpdateHorizontalScrollBarValues()
	{
		if (_InternalHScrollPositionUpdated)
		{
			return;
		}
		WinApi.SCROLLINFO lpsi = default(WinApi.SCROLLINFO);
		lpsi.cbSize = Marshal.SizeOf(lpsi);
		lpsi.fMask = WinApi.ScrollInfoMask.SIF_ALL;
		if (WinApi.GetScrollInfo(_ScrollOverrideControl.Handle, WinApi.SBOrientation.SB_HORZ, ref lpsi))
		{
			if (_HScrollBar.Minimum != lpsi.nMin)
			{
				_HScrollBar.Minimum = lpsi.nMin;
			}
			if (_HScrollBar.Maximum != lpsi.nMax)
			{
				_HScrollBar.Maximum = lpsi.nMax;
			}
			if (_HScrollBar.Value != lpsi.nPos)
			{
				_HScrollBar.Value = lpsi.nPos;
			}
			if (_HScrollBar.LargeChange != lpsi.nPage)
			{
				_HScrollBar.LargeChange = lpsi.nPage;
			}
		}
	}

	private void ControlScrollBarValueChanged(object sender, ScrollValueChangedEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		if ((e.ScrollChange & eScrollBarScrollChange.Vertical) == eScrollBarScrollChange.Vertical)
		{
			UpdateVerticalScrollBarValues();
		}
		if ((e.ScrollChange & eScrollBarScrollChange.MouseWheel) == eScrollBarScrollChange.MouseWheel)
		{
			UpdateVerticalScrollBarValues();
			InvokeDelayed((MethodInvoker)delegate
			{
				UpdateVerticalScrollBarValues();
			}, 400);
		}
		if ((e.ScrollChange & eScrollBarScrollChange.Horizontal) == eScrollBarScrollChange.Horizontal)
		{
			UpdateHorizontalScrollBarValues();
		}
		UpdateScrollBars();
	}

	private void NonClientSizeChanged(object sender, EventArgs e)
	{
		UpdateScrollBars();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		UpdateScrollBars();
		((Control)this).OnHandleCreated(e);
	}

	internal static eScrollBarScrollChange MapMessageToScrollChange(int msg)
	{
		return msg switch
		{
			277 => eScrollBarScrollChange.Vertical, 
			276 => eScrollBarScrollChange.Horizontal, 
			522 => eScrollBarScrollChange.MouseWheel, 
			_ => throw new ArgumentException("Message not recognized msg=" + msg), 
		};
	}

	protected void InvokeDelayed(MethodInvoker method)
	{
		InvokeDelayed(method, 10);
	}

	protected void InvokeDelayed(MethodInvoker method, int delayInterval)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		if (delayInterval <= 0)
		{
			method.Invoke();
			return;
		}
		Timer val = new Timer();
		val = new Timer();
		val.Tag = method;
		val.Interval = delayInterval;
		val.Tick += DelayedInvokeTimerTick;
		val.Start();
	}

	private void DelayedInvokeTimerTick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)sender;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		if (!((Control)this).IsDisposed)
		{
			val2.Invoke();
		}
	}
}
