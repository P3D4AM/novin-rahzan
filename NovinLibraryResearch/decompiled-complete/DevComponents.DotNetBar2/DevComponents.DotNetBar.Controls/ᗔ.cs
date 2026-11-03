using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.ScrollBar;

namespace DevComponents.DotNetBar.Controls;

internal class ᗔ : IDisposable
{
	private const int ᗙ = 400;

	private HScrollBarAdv __059E;

	private VScrollBarAdv __059D;

	private Control __059F;

	private Control _ᗚ;

	private bool _ᗛ;

	private eScrollBarAppearance _ז;

	private bool _ᗜ;

	private bool _ᗝ;

	private bool _ᗞ;

	[Category("Appearance")]
	[DefaultValue(eScrollBarAppearance.Default)]
	[Description("Gets or sets the scroll-bar visual style.")]
	public eScrollBarAppearance ᗕ
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

	public VScrollBarAdv ᗖ => __059D;

	public HScrollBarAdv ᗗ => __059E;

	public bool ᗘ => _ᗞ;

	public ᗔ(Control parent)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		if (parent == null)
		{
			throw new ArgumentNullException("Parent cannot be null");
		}
		if (!(parent is IScrollBarOverrideSupport))
		{
			throw new ArgumentException("parent must implement IScrollBarOverrideSupport interface.");
		}
		_ᗚ = parent;
		__059E = new HScrollBarAdv();
		((Control)__059E).Visible = false;
		__059E.Appearance = _ז;
		__059E.Scroll += new ScrollEventHandler(ڪ);
		__059D = new VScrollBarAdv();
		((Control)__059D).Visible = false;
		__059D.Appearance = _ז;
		__059D.Scroll += new ScrollEventHandler(ک);
		__059F = new Control();
		__059F.Visible = false;
		__059F.BackColor = SystemColors.Window;
		if (_ᗚ.Parent != null)
		{
			ᗤ();
		}
		ᗟ();
		if (_ᗚ.IsHandleCreated)
		{
			UpdateScrollBars();
		}
	}

	private void ᗟ()
	{
		_ᗚ.ParentChanged += ᗣ;
		_ᗚ.HandleCreated += ᗠ;
		_ᗚ.Resize += _0A76;
		IScrollBarOverrideSupport scrollBarOverrideSupport = (IScrollBarOverrideSupport)_ᗚ;
		scrollBarOverrideSupport.NonClientSizeChanged += ᖤ;
		scrollBarOverrideSupport.ScrollBarValueChanged += ᗡ;
	}

	private void _0A76(object _٠, EventArgs ؿ)
	{
		UpdateScrollBars();
	}

	private void ᗠ(object _٠, EventArgs ؿ)
	{
		UpdateScrollBars();
	}

	private void ᖤ(object _٠, EventArgs ؿ)
	{
		UpdateScrollBars();
	}

	private void ᗡ(object _٠, ScrollValueChangedEventArgs ؿ)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		MethodInvoker val = null;
		MethodInvoker val2 = null;
		if ((ؿ.ScrollChange & eScrollBarScrollChange.Vertical) == eScrollBarScrollChange.Vertical)
		{
			ᗩ();
		}
		if ((ؿ.ScrollChange & eScrollBarScrollChange.MouseWheel) == eScrollBarScrollChange.MouseWheel)
		{
			if (((Control)__059D).Visible)
			{
				ᗩ();
				if (val == null)
				{
					val = (MethodInvoker)delegate
					{
						ᗩ();
					};
				}
				ڣ(val, 400);
			}
			else if (((Control)__059E).Visible)
			{
				ᗪ();
				if (val2 == null)
				{
					val2 = (MethodInvoker)delegate
					{
						ᗪ();
					};
				}
				ڣ(val2, 400);
			}
		}
		if ((ؿ.ScrollChange & eScrollBarScrollChange.Horizontal) == eScrollBarScrollChange.Horizontal)
		{
			ᗪ();
		}
		UpdateScrollBars();
	}

	private void ᗢ()
	{
		_ᗚ.ParentChanged -= ᗣ;
		_ᗚ.HandleCreated -= ᗠ;
		_ᗚ.Resize -= _0A76;
		IScrollBarOverrideSupport scrollBarOverrideSupport = (IScrollBarOverrideSupport)_ᗚ;
		scrollBarOverrideSupport.NonClientSizeChanged -= ᖤ;
		scrollBarOverrideSupport.ScrollBarValueChanged -= ᗡ;
	}

	private void ᗣ(object _٠, EventArgs ؿ)
	{
		ᗤ();
		UpdateScrollBars();
	}

	protected virtual void UpdateScrollBarsDelayed()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		ڣ((MethodInvoker)delegate
		{
			UpdateScrollBars();
		}, 400);
	}

	protected virtual void UpdateScrollBars()
	{
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		MethodInvoker val = null;
		MethodInvoker val2 = null;
		if (_ᗚ == null || _ᗚ.Parent == null || !_ᗚ.IsHandleCreated || _ᗛ)
		{
			return;
		}
		_ᗛ = true;
		try
		{
			Control ᗚ = _ᗚ;
			if (ᗚ == null || ((IScrollBarOverrideSupport)ᗚ).DesignMode)
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
			_0EFE.GetScrollBarInfo(ᗚ.Handle, 4294967291u, ref psbi);
			if (psbi.rgstate[0] != 32768)
			{
				Rectangle rectangle = psbi.rcScrollBar.ToRectangle();
				rectangle.Location = ᗚ.Parent.PointToClient(rectangle.Location);
				Rectangle bounds = ᗚ.Bounds;
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
					((Control)__059D).BringToFront();
					((Control)__059D).Refresh();
					if (val == null)
					{
						val = (MethodInvoker)delegate
						{
							ᗩ();
						};
					}
					ڣ(val, 400);
				}
				else
				{
					((Control)__059D).BringToFront();
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
			_0EFE.GetScrollBarInfo(ᗚ.Handle, 4294967290u, ref psbi);
			if (psbi.rgstate[0] != 32768)
			{
				Rectangle rectangle2 = psbi.rcScrollBar.ToRectangle();
				rectangle2.Location = ᗚ.Parent.PointToClient(rectangle2.Location);
				Rectangle bounds2 = ᗚ.Bounds;
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
					((Control)__059E).BringToFront();
					((Control)__059E).Refresh();
					if (val2 == null)
					{
						val2 = (MethodInvoker)delegate
						{
							ᗪ();
						};
					}
					ڣ(val2, 400);
				}
				else
				{
					((Control)__059E).BringToFront();
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
				__059F.BringToFront();
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

	private void ᗤ()
	{
		ᗥ();
		Control parent = _ᗚ.Parent;
		if (parent == null)
		{
			return;
		}
		if (parent is TableLayoutPanel)
		{
			while (parent is TableLayoutPanel)
			{
				parent = parent.Parent;
			}
		}
		if (parent != null)
		{
			parent.Controls.Add((Control)(object)__059D);
			parent.Controls.Add((Control)(object)__059E);
			parent.Controls.Add(__059F);
		}
	}

	private void ᗥ()
	{
		if (((Control)__059D).Parent != null)
		{
			((Control)__059D).Parent.Controls.Remove((Control)(object)__059D);
		}
		if (((Control)__059E).Parent != null)
		{
			((Control)__059E).Parent.Controls.Remove((Control)(object)__059E);
		}
		if (__059F.Parent != null)
		{
			__059F.Parent.Controls.Remove(__059F);
		}
		((Control)__059D).Visible = false;
		((Control)__059E).Visible = false;
		__059F.Visible = false;
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

	private void ک(object _٠, ScrollEventArgs ؿ)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		if (ؿ.NewValue != ؿ.OldValue || (int)ؿ.Type == 8)
		{
			_ᗜ = true;
			if ((int)ؿ.Type == 5)
			{
				_0EFE.SCROLLINFO si = default(_0EFE.SCROLLINFO);
				si.nTrackPos = ؿ.NewValue;
				si.nPos = ؿ.NewValue;
				si.fMask = _0EFE._0F15._0F18;
				si.cbSize = Marshal.SizeOf((object)si);
				_0EFE.SetScrollInfo(_ᗚ.Handle, _0EFE.SBOrientation.SB_VERT, ref si, redraw: false);
			}
			_0EFE.SendMessage(_ᗚ.Handle, 277, _0EFE._0F0E(ᗦ(ؿ.Type), ؿ.NewValue), IntPtr.Zero);
			_ᗜ = false;
			if ((int)ؿ.Type == 8)
			{
				ᗩ();
			}
		}
	}

	private void ڪ(object _٠, ScrollEventArgs ؿ)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		if (ؿ.NewValue != ؿ.OldValue || (int)ؿ.Type == 8)
		{
			_ᗝ = true;
			if ((int)ؿ.Type == 5)
			{
				_0EFE.SCROLLINFO si = default(_0EFE.SCROLLINFO);
				si.nTrackPos = ؿ.NewValue;
				si.nPos = ؿ.NewValue;
				si.fMask = _0EFE._0F15._0F18;
				si.cbSize = Marshal.SizeOf((object)si);
				_0EFE.SetScrollInfo(_ᗚ.Handle, _0EFE.SBOrientation.SB_HORZ, ref si, redraw: false);
			}
			_0EFE.SendMessage(_ᗚ.Handle, 276, _0EFE._0F0E(ᗨ(ؿ.Type), ؿ.NewValue), IntPtr.Zero);
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
		if (_0EFE.GetScrollInfo(_ᗚ.Handle, _0EFE.SBOrientation.SB_VERT, ref lpsi))
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
		if (_0EFE.GetScrollInfo(_ᗚ.Handle, _0EFE.SBOrientation.SB_HORZ, ref lpsi))
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

	protected void ڣ(MethodInvoker ڤ)
	{
		ڣ(ڤ, 10);
	}

	protected void ڣ(MethodInvoker ڤ, int ᗫ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		if (ᗫ <= 0)
		{
			ڤ.Invoke();
			return;
		}
		Timer val = new Timer();
		val = new Timer();
		val.Tag = ڤ;
		val.Interval = ᗫ;
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
		if (!ᗘ)
		{
			val2.Invoke();
		}
	}

	public void Dispose()
	{
		ᗢ();
		if (((Control)__059D).Parent == null)
		{
			((Component)(object)__059D).Dispose();
		}
		if (((Control)__059E).Parent == null)
		{
			((Component)(object)__059E).Dispose();
		}
		if (__059F.Parent == null)
		{
			((Component)(object)__059F).Dispose();
		}
		_ᗞ = true;
	}

	[CompilerGenerated]
	private void ᗬ()
	{
		ᗩ();
	}

	[CompilerGenerated]
	private void ᗭ()
	{
		ᗪ();
	}

	[CompilerGenerated]
	private void ᗮ()
	{
		UpdateScrollBars();
	}

	[CompilerGenerated]
	private void ᗯ()
	{
		ᗩ();
	}

	[CompilerGenerated]
	private void ᗰ()
	{
		ᗪ();
	}
}
