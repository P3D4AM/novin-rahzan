using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.ScrollBar;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
public class ScrollbarControl : ControlWithBackgroundStyle
{
	private const int ᗙ = 400;

	private HScrollBarAdv __059E;

	private VScrollBarAdv __059D;

	private Control __059F;

	private eScrollBarAppearance _ז;

	private bool _ᗛ;

	private Control __1BA7;

	private bool _ᗜ;

	private bool _ᗝ;

	[Description("Gets or sets the scroll-bar visual style.")]
	[Category("Appearance")]
	[DefaultValue(eScrollBarAppearance.Default)]
	public eScrollBarAppearance ScrollBarAppearance
	{
		get
		{
			return _ז;
		}
		set
		{
			_ז = value;
			ف();
		}
	}

	internal Control _1BA6
	{
		get
		{
			return __1BA7;
		}
		set
		{
			if (value != __1BA7)
			{
				if (!(value is IScrollBarOverrideSupport))
				{
					throw new ArgumentException("ScrollOverrideControl must implement IScrollBarOverrideSupport interface.");
				}
				Control oldValue = __1BA7;
				__1BA7 = value;
				OnScrollOverrideControlChanged(oldValue, value);
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
		__059E = new HScrollBarAdv();
		((Control)__059E).Visible = false;
		__059E.Appearance = _ז;
		__059E.Scroll += new ScrollEventHandler(ڪ);
		__059D = new VScrollBarAdv();
		((Control)__059D).Visible = false;
		__059D.Appearance = _ז;
		__059D.Scroll += new ScrollEventHandler(ک);
		((Control)this).Controls.Add((Control)(object)__059E);
		((Control)this).Controls.Add((Control)(object)__059D);
		__059F = new Control();
		__059F.Visible = false;
		__059F.BackColor = SystemColors.Window;
		((Control)this).Controls.Add(__059F);
	}

	private void ف()
	{
		if (__059D != null)
		{
			__059D.Appearance = _ז;
		}
		if (__059E != null)
		{
			__059E.Appearance = _ז;
		}
	}

	private void _1BA8()
	{
		if (__1BA7 != null)
		{
			__1BA7.Bounds = _1BA9();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		_1BA8();
		UpdateScrollBars();
		((Control)this).OnResize(e);
	}

	protected override void OnVisualPropertyChanged()
	{
		base.OnVisualPropertyChanged();
		_1BA8();
		UpdateScrollBars();
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).ScaleControl(factor, specified);
		_1BA8();
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
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		MethodInvoker val = null;
		MethodInvoker val2 = null;
		if (!((Control)this).IsHandleCreated || _ᗛ)
		{
			return;
		}
		_ᗛ = true;
		try
		{
			Control val3 = __1BA7;
			if (val3 == null)
			{
				if (((Control)__059E).Visible)
				{
					((Control)__059E).Visible = false;
				}
				if (((Control)__059D).Visible)
				{
					((Control)__059D).Visible = false;
				}
				return;
			}
			_0EFE.SCROLLBARINFO psbi = default(_0EFE.SCROLLBARINFO);
			psbi.cbSize = Marshal.SizeOf((object)psbi);
			_0EFE.GetScrollBarInfo(val3.Handle, 4294967291u, ref psbi);
			if (psbi.rgstate[0] != 32768)
			{
				Rectangle rectangle = psbi.rcScrollBar.ToRectangle();
				rectangle.Location = ((Control)this).PointToClient(rectangle.Location);
				Rectangle bounds = __1BA7.Bounds;
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
				if (((Control)__059D).Bounds != rectangle)
				{
					((Control)__059D).Bounds = rectangle;
					ᗩ();
				}
				if (!((Control)__059D).Visible)
				{
					((Control)__059D).Visible = true;
					((Control)__059D).Refresh();
					if (val == null)
					{
						val = (MethodInvoker)delegate
						{
							ᗩ();
						};
					}
					InvokeDelayed(val, 400);
				}
				if (psbi.rgstate[0] == 1)
				{
					((Control)__059D).Enabled = false;
				}
				else if (!((Control)__059D).Enabled)
				{
					((Control)__059D).Enabled = true;
				}
			}
			else if (((Control)__059D).Visible)
			{
				((Control)__059D).Visible = false;
			}
			psbi = default(_0EFE.SCROLLBARINFO);
			psbi.cbSize = Marshal.SizeOf((object)psbi);
			_0EFE.GetScrollBarInfo(val3.Handle, 4294967290u, ref psbi);
			if (psbi.rgstate[0] != 32768)
			{
				Rectangle rectangle2 = psbi.rcScrollBar.ToRectangle();
				rectangle2.Location = ((Control)this).PointToClient(rectangle2.Location);
				Rectangle bounds2 = __1BA7.Bounds;
				if (!bounds2.Contains(rectangle2) && rectangle2.Bottom > bounds2.Bottom)
				{
					rectangle2.Y = bounds2.Bottom - rectangle2.Height;
				}
				if (((Control)__059D).Visible && rectangle2.Width == bounds2.Width)
				{
					rectangle2.Width -= ((Control)__059D).Width;
				}
				if (((Control)__059E).Bounds != rectangle2)
				{
					((Control)__059E).Bounds = rectangle2;
					ᗪ();
				}
				if (!((Control)__059E).Visible)
				{
					((Control)__059E).Visible = true;
					((Control)__059E).Refresh();
					if (val2 == null)
					{
						val2 = (MethodInvoker)delegate
						{
							ᗪ();
						};
					}
					InvokeDelayed(val2, 400);
				}
				if (psbi.rgstate[0] == 1)
				{
					((Control)__059E).Enabled = false;
				}
				else if (!((Control)__059E).Enabled)
				{
					((Control)__059E).Enabled = true;
				}
			}
			else if (((Control)__059E).Visible)
			{
				((Control)__059E).Visible = false;
			}
			if (((Control)__059E).Visible && ((Control)__059D).Visible)
			{
				__059F.Bounds = new Rectangle(((Control)__059D).Left, ((Control)__059D).Bounds.Bottom, ((Control)__059D).Width, ((Control)__059E).Height);
				__059F.Visible = true;
			}
			else
			{
				__059F.Visible = false;
			}
		}
		finally
		{
			_ᗛ = false;
		}
	}

	private Rectangle _1BA9()
	{
		return GetContentRectangle();
	}

	protected virtual void OnScrollOverrideControlChanged(Control oldValue, Control newValue)
	{
		if (oldValue != null)
		{
			IScrollBarOverrideSupport scrollBarOverrideSupport = (IScrollBarOverrideSupport)newValue;
			scrollBarOverrideSupport.NonClientSizeChanged -= ᖤ;
			scrollBarOverrideSupport.ScrollBarValueChanged -= ᗡ;
			scrollBarOverrideSupport.ControlMoved -= _1BAA;
			((Control)this).Controls.Remove(oldValue);
		}
		if (newValue != null)
		{
			IScrollBarOverrideSupport scrollBarOverrideSupport2 = (IScrollBarOverrideSupport)newValue;
			scrollBarOverrideSupport2.NonClientSizeChanged += ᖤ;
			scrollBarOverrideSupport2.ScrollBarValueChanged += ᗡ;
			scrollBarOverrideSupport2.ControlMoved += _1BAA;
			((Control)this).Controls.Add(newValue);
		}
		_1BA8();
		if (((Control)this).IsHandleCreated)
		{
			UpdateScrollBars();
		}
	}

	private void _1BAA(object _٠, EventArgs ؿ)
	{
		_1BA8();
	}

	private void ک(object _٠, ScrollEventArgs ؿ)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if (ؿ.NewValue != ؿ.OldValue || (int)ؿ.Type == 8)
		{
			_ᗜ = true;
			_0EFE.SendMessage(__1BA7.Handle, 277, _0EFE._0F0E(ᗦ(ؿ.Type), ؿ.NewValue), IntPtr.Zero);
			_ᗜ = false;
			if ((int)ؿ.Type == 8)
			{
				ᗩ();
			}
		}
	}

	private void ڪ(object _٠, ScrollEventArgs ؿ)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if (ؿ.NewValue != ؿ.OldValue || (int)ؿ.Type == 8)
		{
			_ᗝ = true;
			_0EFE.SendMessage(__1BA7.Handle, 276, _0EFE._0F0E(ᗨ(ؿ.Type), ؿ.NewValue), IntPtr.Zero);
			_ᗝ = false;
			if ((int)ؿ.Type == 8)
			{
				ᗪ();
			}
		}
	}

	private static int ᗦ(ScrollEventType ᗧ)
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
		if ((int)ᗧ == 8)
		{
			return 8;
		}
		if ((int)ᗧ == 6)
		{
			return 6;
		}
		if ((int)ᗧ == 2)
		{
			return 2;
		}
		if ((int)ᗧ == 3)
		{
			return 3;
		}
		if ((int)ᗧ == 7)
		{
			return 7;
		}
		if ((int)ᗧ == 0)
		{
			return 0;
		}
		if ((int)ᗧ == 1)
		{
			return 1;
		}
		if ((int)ᗧ == 4)
		{
			return 4;
		}
		if ((int)ᗧ == 5)
		{
			return 5;
		}
		return 0;
	}

	private static int ᗨ(ScrollEventType ᗧ)
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
		if ((int)ᗧ == 8)
		{
			return 8;
		}
		if ((int)ᗧ == 6)
		{
			return 6;
		}
		if ((int)ᗧ == 2)
		{
			return 2;
		}
		if ((int)ᗧ == 3)
		{
			return 3;
		}
		if ((int)ᗧ == 7)
		{
			return 7;
		}
		if ((int)ᗧ == 0)
		{
			return 0;
		}
		if ((int)ᗧ == 1)
		{
			return 1;
		}
		if ((int)ᗧ == 4)
		{
			return 4;
		}
		if ((int)ᗧ == 5)
		{
			return 5;
		}
		return 0;
	}

	private void ᗩ()
	{
		if (_ᗜ)
		{
			return;
		}
		_0EFE.SCROLLINFO lpsi = default(_0EFE.SCROLLINFO);
		lpsi.cbSize = Marshal.SizeOf((object)lpsi);
		lpsi.fMask = _0EFE._0F15._0F16 | _0EFE._0F15._0F17 | _0EFE._0F15._0F18;
		if (_0EFE.GetScrollInfo(__1BA7.Handle, _0EFE.SBOrientation.SB_VERT, ref lpsi))
		{
			if (__059D.Minimum != lpsi.nMin)
			{
				__059D.Minimum = lpsi.nMin;
			}
			if (__059D.Maximum != lpsi.nMax)
			{
				__059D.Maximum = lpsi.nMax;
			}
			if (__059D.Value != lpsi.nPos)
			{
				__059D.Value = lpsi.nPos;
			}
			if (__059D.LargeChange != lpsi.nPage)
			{
				__059D.LargeChange = lpsi.nPage;
			}
		}
	}

	private void ᗪ()
	{
		if (_ᗝ)
		{
			return;
		}
		_0EFE.SCROLLINFO lpsi = default(_0EFE.SCROLLINFO);
		lpsi.cbSize = Marshal.SizeOf((object)lpsi);
		lpsi.fMask = _0EFE._0F15._0F1B;
		if (_0EFE.GetScrollInfo(__1BA7.Handle, _0EFE.SBOrientation.SB_HORZ, ref lpsi))
		{
			if (__059E.Minimum != lpsi.nMin)
			{
				__059E.Minimum = lpsi.nMin;
			}
			if (__059E.Maximum != lpsi.nMax)
			{
				__059E.Maximum = lpsi.nMax;
			}
			if (__059E.Value != lpsi.nPos)
			{
				__059E.Value = lpsi.nPos;
			}
			if (__059E.LargeChange != lpsi.nPage)
			{
				__059E.LargeChange = lpsi.nPage;
			}
		}
	}

	private void ᗡ(object _٠, ScrollValueChangedEventArgs ؿ)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		MethodInvoker val = null;
		if ((ؿ.ScrollChange & eScrollBarScrollChange.Vertical) == eScrollBarScrollChange.Vertical)
		{
			ᗩ();
		}
		if ((ؿ.ScrollChange & eScrollBarScrollChange.MouseWheel) == eScrollBarScrollChange.MouseWheel)
		{
			ᗩ();
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					ᗩ();
				};
			}
			InvokeDelayed(val, 400);
		}
		if ((ؿ.ScrollChange & eScrollBarScrollChange.Horizontal) == eScrollBarScrollChange.Horizontal)
		{
			ᗪ();
		}
		UpdateScrollBars();
	}

	private void ᖤ(object _٠, EventArgs ؿ)
	{
		UpdateScrollBars();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		UpdateScrollBars();
		((Control)this).OnHandleCreated(e);
	}

	internal static eScrollBarScrollChange _1BAB(int ᘮ)
	{
		return ᘮ switch
		{
			277 => eScrollBarScrollChange.Vertical, 
			276 => eScrollBarScrollChange.Horizontal, 
			522 => eScrollBarScrollChange.MouseWheel, 
			_ => throw new ArgumentException("Message not recognized msg=" + ᘮ), 
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
		val.Tick += ڥ;
		val.Start();
	}

	private void ڥ(object _٠, EventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)_٠;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		if (!((Control)this).IsDisposed)
		{
			val2.Invoke();
		}
	}

	[CompilerGenerated]
	private void _1BAC()
	{
		UpdateScrollBars();
	}

	[CompilerGenerated]
	private void _1BAD()
	{
		ᗩ();
	}

	[CompilerGenerated]
	private void ᮮ()
	{
		ᗪ();
	}

	[CompilerGenerated]
	private void ᮯ()
	{
		ᗩ();
	}
}
