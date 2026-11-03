using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.ScrollBar;

namespace DevComponents.DotNetBar.Controls;

internal interface _2273
{
	bool _2274 { get; }
}
internal delegate void _1DDE(object sender, _1DDF e);
internal class _1DDF : EventArgs
{
	public Graphics ܓ;

	public Rectangle ঋ = Rectangle.Empty;

	public _1DDF(Graphics g, Rectangle r)
	{
		ܓ = g;
		ঋ = r;
	}
}
internal class _225D : VScrollBar, _2263._2262
{
	private _2263 _225F;

	private bool _2260 = true;

	protected override CreateParams CreateParams
	{
		get
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Invalid comparison between Unknown and I4
			CreateParams createParams = ((VScrollBar)this).CreateParams;
			if (_2260)
			{
				createParams.ClassName = null;
				createParams.Style = 33554432;
				createParams.Style |= 0x44000000;
				if (((Control)this).Visible)
				{
					createParams.Style |= 0x10000000;
				}
				if (!((Control)this).Enabled)
				{
					createParams.Style |= 0x8000000;
				}
				if ((int)((Control)this).RightToLeft == 1)
				{
					createParams.ExStyle |= 0x2000;
					createParams.ExStyle |= 0x1000;
					createParams.ExStyle |= 0x4000;
				}
			}
			return createParams;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether custom styling (Office 2007 style) is enabled.")]
	public bool _225E
	{
		get
		{
			return _2260;
		}
		set
		{
			if (_2260 != value)
			{
				_2260 = value;
				if (((Control)this).IsHandleCreated)
				{
					((Control)this).RecreateHandle();
				}
			}
		}
	}

	public bool ᘾ
	{
		get
		{
			return _225F.ᘾ;
		}
		set
		{
			_225F.ᘾ = value;
		}
	}

	public _225D()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		_225F = new _2263((ScrollBar)(object)this);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, false);
	}

	protected override void Dispose(bool disposing)
	{
		_225F.Dispose();
		((Control)this).Dispose(disposing);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		_225F._226B();
		((ScrollBar)this).OnHandleCreated(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (_2260)
		{
			_225F.ሏ(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_2260)
		{
			_225F._2270(e);
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		if (_2260)
		{
			_225F._226C(e);
		}
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_2260)
		{
			_225F._226D(e);
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_2260)
		{
			_225F._1DD2();
			_225F.ሐ(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_2260)
		{
			_225F.ઘ(e);
		}
		((Control)this).OnPaint(e);
	}

	protected override void NotifyInvalidate(Rectangle invalidatedArea)
	{
		if (_2260)
		{
			_225F._226E(invalidatedArea);
		}
		((Control)this).NotifyInvalidate(invalidatedArea);
	}

	protected override void OnResize(EventArgs e)
	{
		_1DD2();
		((Control)this).OnResize(e);
	}

	public void _1DD2()
	{
		_225F._1DD2();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (((Control)this).Parent is DataGridView)
		{
			_225F._2265.ᘄ = true;
			_225F._2265.ᘃ = false;
		}
		else
		{
			_225F._2265.ᘄ = false;
			_225F._2265.ᘃ = true;
		}
		((Control)this).OnParentChanged(e);
	}

	private void _2261(int ل, ScrollEventType ᗧ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if ((int)ᗧ == 1 && ((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange - ل < 21)
		{
			ل = ((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange + 1;
			ᗧ = (ScrollEventType)7;
		}
		int value = ((ScrollBar)this).Value;
		((ScrollBar)this).Value = ل;
		ScrollEventArgs val = new ScrollEventArgs(ᗧ, value, ل, (ScrollOrientation)1);
		((ScrollBar)this).OnScroll(val);
	}

	void _2263._2262.ᘩ(int ل, ScrollEventType ᗧ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ≡
		this._2261(ل, ᗧ);
	}
}
internal class _2263 : IDisposable
{
	internal interface _2262
	{
		void ᘩ(int ل, ScrollEventType ᗧ);
	}

	private ScrollBarCore _2266;

	private ScrollBar _2267;

	private _2262 _2268;

	private bool _2269;

	private ColorScheme լ;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private bool _2264 => _2269;

	public eRenderMode ԧ
	{
		get
		{
			return ԁ;
		}
		set
		{
			if (ԁ != value)
			{
				ԁ = value;
				((Control)_2267).Invalidate(true);
			}
		}
	}

	public BaseRenderer ໝ
	{
		get
		{
			return ਜ;
		}
		set
		{
			ਜ = value;
		}
	}

	internal ScrollBarCore _2265 => _2266;

	public bool ᘾ
	{
		get
		{
			return _2266.IsAppScrollBarStyle;
		}
		set
		{
			_2266.IsAppScrollBarStyle = value;
		}
	}

	public _2263(ScrollBar sb)
	{
		_2267 = sb;
		_2268 = (_2262)_2267;
		_2269 = _2267 is VScrollBar;
		_2266 = new ScrollBarCore((Control)(object)_2267, isPassive: false);
		_2266.ValueChanged += _2271;
		if (_2267 is HScrollBar)
		{
			_2266.Orientation = eOrientation.Horizontal;
		}
		else
		{
			_2266.Orientation = eOrientation.Vertical;
		}
		_2266.Minimum = _2267.Minimum;
		_2266.Maximum = _2267.Maximum;
		_2266.Value = _2267.Value;
		_2266.Enabled = ((Control)_2267).Enabled;
		((Control)_2267).EnabledChanged += _226A;
	}

	private void _226A(object _٠, EventArgs ؿ)
	{
		_2266.Enabled = ((Control)_2267).Enabled;
	}

	internal void _226B()
	{
		_1DD2();
	}

	internal void _1DD2()
	{
		Rectangle rectangle = new Rectangle(0, 0, ((Control)_2267).Width, ((Control)_2267).Height);
		if (_2266.Minimum != _2267.Minimum)
		{
			_2266.Minimum = _2267.Minimum;
		}
		if (_2266.Maximum != _2267.Maximum)
		{
			_2266.Maximum = _2267.Maximum;
		}
		if (_2266.SmallChange != _2267.SmallChange)
		{
			_2266.SmallChange = _2267.SmallChange;
		}
		if (_2266.LargeChange != _2267.LargeChange)
		{
			_2266.LargeChange = _2267.LargeChange;
		}
		if (_2266.Value != _2267.Value)
		{
			_2266.Value = _2267.Value;
		}
		if (rectangle != _2266.DisplayRectangle)
		{
			_2266.DisplayRectangle = rectangle;
		}
	}

	internal void _226C(EventArgs ؿ)
	{
		if (((Control)_2267).Capture)
		{
			((Control)_2267).Capture = false;
		}
	}

	internal void _226D(EventArgs ؿ)
	{
		_2266.MouseLeave();
	}

	internal void ሐ(MouseEventArgs ؿ)
	{
		_2266.MouseMove(ؿ);
	}

	internal void _226E(Rectangle _226F)
	{
		_2266.ᘝ();
	}

	internal void ሏ(MouseEventArgs ؿ)
	{
		_2266.MouseDown(ؿ);
	}

	internal void _2270(MouseEventArgs ؿ)
	{
		_2266.MouseUp(ؿ);
	}

	private void _2271(object _٠, EventArgs ؿ)
	{
		ᘩ(_2266.Value);
	}

	private void ᘩ(int _1A7F)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		if (_2267.Value != _1A7F || _2267.Value == _2266.ᘬ())
		{
			ScrollEventType val = (ScrollEventType)1;
			if (_2266.ᘅ == ScrollBarCore.ᘱ.ᘳ)
			{
				val = (ScrollEventType)0;
			}
			else if (_2266.ᘅ == ScrollBarCore.ᘱ.ᘴ)
			{
				val = (ScrollEventType)5;
			}
			else if (_2266.ᘅ == ScrollBarCore.ᘱ.ᘵ)
			{
				val = ((_1A7F <= _2267.Value) ? ((ScrollEventType)2) : ((ScrollEventType)3));
			}
			if ((int)val == 1 && _2267.Value == _1A7F && _2267.Value == _2266.ᘬ())
			{
				val = (ScrollEventType)7;
			}
			_2268.ᘩ(_1A7F, val);
		}
	}

	public void Dispose()
	{
		_2266.ᘝ();
	}

	internal void ઘ(PaintEventArgs ؿ)
	{
		Graphics graphics = ؿ.Graphics;
		_1DD2();
		_2266.Paint(_0A92(graphics));
	}

	private ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(_2267 as IOwner, (Control)(object)_2267, _٥, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = false;
		return itemPaintArgs;
	}

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		if (լ == null)
		{
			լ = new ColorScheme(eDotNetBarStyle.Office2007);
		}
		return լ;
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (ԁ == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (ԁ == eRenderMode.Custom && ਜ != null)
		{
			return ਜ;
		}
		if (ਛ == null)
		{
			ਛ = new Office2007Renderer();
		}
		return ਛ;
	}
}
internal class _2272 : HScrollBar, _2263._2262
{
	private _2263 _225F;

	private bool _2260 = true;

	protected override CreateParams CreateParams
	{
		get
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Invalid comparison between Unknown and I4
			CreateParams createParams = ((HScrollBar)this).CreateParams;
			if (_2260)
			{
				createParams.ClassName = null;
				createParams.Style = 33554432;
				createParams.Style |= 0x44000000;
				if (((Control)this).Visible)
				{
					createParams.Style |= 0x10000000;
				}
				if (!((Control)this).Enabled)
				{
					createParams.Style |= 0x8000000;
				}
				if ((int)((Control)this).RightToLeft == 1)
				{
					createParams.ExStyle |= 0x2000;
					createParams.ExStyle |= 0x1000;
					createParams.ExStyle |= 0x4000;
				}
			}
			return createParams;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether custom styling (Office 2007 style) is enabled.")]
	public bool _225E
	{
		get
		{
			return _2260;
		}
		set
		{
			if (_2260 != value)
			{
				_2260 = value;
				if (((Control)this).IsHandleCreated)
				{
					((Control)this).RecreateHandle();
				}
			}
		}
	}

	public bool ᘾ
	{
		get
		{
			return _225F.ᘾ;
		}
		set
		{
			_225F.ᘾ = value;
		}
	}

	public _2272()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		_225F = new _2263((ScrollBar)(object)this);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, false);
	}

	protected override void Dispose(bool disposing)
	{
		_225F.Dispose();
		((Control)this).Dispose(disposing);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		_225F._226B();
		((ScrollBar)this).OnHandleCreated(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (_2260)
		{
			_225F.ሏ(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_2260)
		{
			_225F._2270(e);
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		if (_2260)
		{
			_225F._226C(e);
		}
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_2260)
		{
			_225F._226D(e);
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_2260)
		{
			_225F.ሐ(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_2260)
		{
			_225F.ઘ(e);
		}
		((Control)this).OnPaint(e);
	}

	protected override void NotifyInvalidate(Rectangle invalidatedArea)
	{
		if (_2260)
		{
			_225F._226E(invalidatedArea);
		}
		((Control)this).NotifyInvalidate(invalidatedArea);
	}

	public void _1DD2()
	{
		_225F._1DD2();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (((Control)this).Parent is DataGridView)
		{
			_225F._2265.ᘄ = true;
			_225F._2265.ᘃ = false;
		}
		else
		{
			_225F._2265.ᘄ = false;
			_225F._2265.ᘃ = true;
		}
		((Control)this).OnParentChanged(e);
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		_1DD2();
	}

	private void _2261(int ل, ScrollEventType ᗧ)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		if (ل != ((ScrollBar)this).Value)
		{
			int value = ((ScrollBar)this).Value;
			((ScrollBar)this).Value = ل;
			ScrollEventArgs val = new ScrollEventArgs(ᗧ, value, ل, (ScrollOrientation)0);
			((ScrollBar)this).OnScroll(val);
		}
	}

	void _2263._2262.ᘩ(int ل, ScrollEventType ᗧ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ≡
		this._2261(ل, ᗧ);
	}
}
internal class _26B0 : IDisposable
{
	private ToolStripControlHost __26B5;

	private _26BE __26B6;

	private Padding _ᬩ = Padding.Empty;

	private Padding __0AE5 = new Padding(1, 1, 1, 1);

	private ToolStripDropDownClosedEventHandler m__19E5;

	private ToolStripDropDownClosingEventHandler m__0B4C;

	public Rectangle ঋ => ((Control)__26B6).Bounds;

	public bool _2285
	{
		get
		{
			return ((ToolStripDropDown)__26B6).AutoClose;
		}
		set
		{
			((ToolStripDropDown)__26B6).AutoClose = value;
		}
	}

	public Rectangle _26B1
	{
		get
		{
			return __26B6._26B1;
		}
		set
		{
			__26B6._26B1 = value;
		}
	}

	public bool ᆒ
	{
		get
		{
			if (__26B6 == null || !((ToolStripDropDown)__26B6).Visible)
			{
				return false;
			}
			return true;
		}
	}

	public Control ᘵ
	{
		get
		{
			if (__26B5 == null)
			{
				return null;
			}
			return __26B5.Control;
		}
	}

	public Padding ᙟ
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _ᬩ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_ᬩ = value;
		}
	}

	public Padding _26B2
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return __0AE5;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			__0AE5 = value;
		}
	}

	public bool _26B3
	{
		get
		{
			if (__26B6 == null)
			{
				return false;
			}
			return __26B6._26B3;
		}
		set
		{
			if (__26B6 != null)
			{
				__26B6._26B3 = value;
			}
		}
	}

	public bool _26B4
	{
		get
		{
			if (__26B6 == null)
			{
				return false;
			}
			return __26B6._26B4;
		}
		set
		{
			if (__26B6 != null)
			{
				__26B6._26B4 = value;
			}
		}
	}

	public event ToolStripDropDownClosedEventHandler _19E5
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ToolStripDropDownClosedEventHandler val = this.m__19E5;
			ToolStripDropDownClosedEventHandler val2;
			do
			{
				val2 = val;
				ToolStripDropDownClosedEventHandler value2 = (ToolStripDropDownClosedEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m__19E5, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ToolStripDropDownClosedEventHandler val = this.m__19E5;
			ToolStripDropDownClosedEventHandler val2;
			do
			{
				val2 = val;
				ToolStripDropDownClosedEventHandler value2 = (ToolStripDropDownClosedEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m__19E5, value2, val2);
			}
			while (val != val2);
		}
	}

	public event ToolStripDropDownClosingEventHandler _0B4C
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ToolStripDropDownClosingEventHandler val = this.m__0B4C;
			ToolStripDropDownClosingEventHandler val2;
			do
			{
				val2 = val;
				ToolStripDropDownClosingEventHandler value2 = (ToolStripDropDownClosingEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m__0B4C, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ToolStripDropDownClosingEventHandler val = this.m__0B4C;
			ToolStripDropDownClosingEventHandler val2;
			do
			{
				val2 = val;
				ToolStripDropDownClosingEventHandler value2 = (ToolStripDropDownClosingEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m__0B4C, value2, val2);
			}
			while (val != val2);
		}
	}

	public _26B0()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		_26B8();
	}

	protected virtual void OnClosed(ToolStripDropDownClosedEventArgs e)
	{
		ToolStripDropDownClosedEventHandler val = this._19E5;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	protected virtual void OnClosing(ToolStripDropDownClosingEventArgs e)
	{
		ToolStripDropDownClosingEventHandler val = this._0B4C;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	private void _26B7(Control ॼ)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_26B8();
		if (ॼ != ᘵ)
		{
			_26BD();
		}
		if (__26B5 == null)
		{
			__26B5 = new ToolStripControlHost(ॼ);
			((ToolStripItem)__26B5).AutoSize = false;
			((ToolStripItem)__26B5).Padding = ᙟ;
			((ToolStripItem)__26B5).Margin = _26B2;
		}
		((ToolStripItem)__26B5).Size = ॼ.Size;
		((ToolStrip)__26B6).Items.Clear();
		_26BE obj = __26B6;
		Padding padding = (((Control)__26B6).Margin = Padding.Empty);
		((Control)obj).Padding = padding;
		((ToolStrip)__26B6).Items.Add((ToolStripItem)(object)__26B5);
	}

	private void _26B8()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		if (__26B6 == null)
		{
			__26B6 = new _26BE();
			((ToolStripDropDown)__26B6).Closed += new ToolStripDropDownClosedEventHandler(_258C);
			((ToolStripDropDown)__26B6).Closing += new ToolStripDropDownClosingEventHandler(_26B9);
		}
	}

	private void _258C(object _٠, ToolStripDropDownClosedEventArgs ؿ)
	{
		OnClosed(ؿ);
	}

	private void _26B9(object _٠, ToolStripDropDownClosingEventArgs ؿ)
	{
		OnClosing(ؿ);
	}

	public void _26BA(Control ॼ, int و, int ى)
	{
		_26BA(ॼ, و, ى, ePopupResizeEdge.None);
	}

	public void _26BA(Control ॼ, int و, int ى, ePopupResizeEdge _26BB)
	{
		_26BA(ॼ, و, ى, -1, -1, _26BB);
	}

	public void _26BA(Control ॼ, int و, int ى, int _९, int _0970, ePopupResizeEdge _26BB)
	{
		_ = ॼ.Size;
		_26B7(ॼ);
		__26B6._26BF = _26BB;
		__26B6._26BA(و, ى, _९, _0970);
		ॼ.Focus();
	}

	public void _26BC()
	{
		if (__26B6 != null && ((ToolStripDropDown)__26B6).Visible)
		{
			((Control)__26B6).Hide();
		}
	}

	private void _26BD()
	{
		if (__26B5 != null)
		{
			if (__26B6 != null)
			{
				((ToolStrip)__26B6).Items.Clear();
			}
			((Component)(object)__26B5).Dispose();
			__26B5 = null;
		}
	}

	public void Dispose()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		_26BD();
		if (__26B6 != null)
		{
			((ToolStripDropDown)__26B6).Closed -= new ToolStripDropDownClosedEventHandler(_258C);
			((ToolStripDropDown)__26B6).Closing -= new ToolStripDropDownClosingEventHandler(_26B9);
			((Component)(object)__26B6).Dispose();
			__26B6 = null;
		}
	}
}
internal class _26BE : ToolStripDropDown
{
	private struct _26D8
	{
		public Color ᖳ;

		public Color _26D9;

		public Color _26DA;

		public _26D8(Color backColor, Color gripLightColor, Color gripColor)
		{
			ᖳ = backColor;
			_26D9 = gripLightColor;
			_26DA = gripColor;
		}
	}

	private ePopupResizeEdge __26C2;

	private Rectangle __26C3 = Rectangle.Empty;

	private bool __26C4;

	private bool __26C5;

	private bool __26C6;

	private static readonly Size _26C7 = new Size(16, 16);

	private _26D8 __26C8;

	private bool __26C9;

	private Rectangle __26CA = Rectangle.Empty;

	private _26DB __26CB;

	private bool __137B = true;

	public bool _26B3
	{
		get
		{
			return __26C9;
		}
		set
		{
			__26C9 = value;
		}
	}

	public Rectangle _26B1
	{
		get
		{
			return __26CA;
		}
		set
		{
			__26CA = value;
		}
	}

	public ePopupResizeEdge _26BF
	{
		get
		{
			return __26C2;
		}
		set
		{
			if (value != __26C2)
			{
				__26C2 = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool _26B4
	{
		get
		{
			return __137B;
		}
		set
		{
			__137B = value;
		}
	}

	public Rectangle _26C0
	{
		get
		{
			return __26C3;
		}
		private set
		{
			__26C3 = value;
		}
	}

	private bool _26C1
	{
		get
		{
			if (_26BF != ePopupResizeEdge.BottomLeft)
			{
				return _26BF == ePopupResizeEdge.BottomRight;
			}
			return true;
		}
	}

	public _26BE()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).AutoSize = false;
		((Control)this).Padding = Padding.Empty;
		((Control)this).Margin = Padding.Empty;
	}

	protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
	{
		Control val = _26D0();
		if (val != null)
		{
			val.SizeChanged -= _26CC;
		}
		((ToolStripDropDown)this).OnClosing(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		_26C0 = Rectangle.Empty;
		Size size = Dpi.Size(_26C7);
		if (_26D1(ePopupResizeEdge.BottomLeft))
		{
			_26D8 _26D8 = _2517();
			SolidBrush val = new SolidBrush(_26D8.ᖳ);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, 1, ((Control)this).Height - size.Height, ((Control)this).Width - 2, size.Height - 1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			_26C0 = new Rectangle(1, ((Control)this).Height - size.Height, size.Width, size.Height);
			_26E2._26E3(e.Graphics, _26C0, _26D8._26D9, _26D8._26DA, _20B4: true);
		}
		else if (_26D1(ePopupResizeEdge.BottomRight))
		{
			_26D8 _26D82 = _2517();
			SolidBrush val2 = new SolidBrush(_26D82.ᖳ);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, 1, ((Control)this).Height - size.Height, ((Control)this).Width - 2, size.Height - 1);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			_26C0 = new Rectangle(((Control)this).Width - size.Width - 1, ((Control)this).Height - size.Height, size.Width, size.Height);
			_26E2._26E3(e.Graphics, _26C0, _26D82._26D9, _26D82._26DA, _20B4: false);
		}
		((ToolStrip)this).OnPaint(e);
	}

	private _26D8 _2517()
	{
		return __26C8;
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		if (!__26C5)
		{
			_26CF();
			__26C9 = true;
		}
	}

	protected void _26CC(object _٠, EventArgs ؿ)
	{
		if (!__26C4)
		{
			_26CE(-1);
		}
	}

	public void _26BA(int و, int ى)
	{
		_26BA(و, ى, -1, -1);
	}

	public void _26BA(int و, int ى, int _९, int _0970)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			__26C8 = new _26D8(colorTable.Form.BackColor, colorTable.LegacyColors.SplitterBackground2, colorTable.LegacyColors.BarStripeColor);
		}
		else
		{
			__26C8 = new _26D8(SystemColors.ButtonFace, Color.White, SystemColors.ControlDark);
		}
		Control val = _26D0();
		if (val == null)
		{
			return;
		}
		__26C4 = true;
		__26C5 = true;
		((Control)this).Size = new Size(1, 1);
		((ToolStripDropDown)this).Show(و, ى);
		__26C4 = false;
		__26C5 = false;
		_26CE(_९);
		if (__137B && _26C1)
		{
			if (__26CB == null)
			{
				ButtonItem buttonItem = new ButtonItem();
				buttonItem.Symbol = "\uf00d";
				buttonItem.SymbolSize = 8f;
				buttonItem.Style = eDotNetBarStyle.StyleManagerControlled;
				buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
				buttonItem.LeftInternal = 1;
				buttonItem.Click += _26CD;
				__26CB = new _26DB(buttonItem, (Control)(object)this);
				buttonItem.RecalcSize();
				buttonItem.TopInternal = ((Control)this).Height - buttonItem.HeightInternal - 1;
			}
		}
		else if (__26CB != null)
		{
			__26CB.Dispose();
			__26CB = null;
		}
		if (__26C6)
		{
			_26CF();
		}
		if (ى > ((Control)this).Top && ى <= ((Control)this).Bottom)
		{
			if (__26CA.IsEmpty)
			{
				((Control)this).Top = ى - ((Control)this).Height - ((_0970 != -1) ? _0970 : 0);
			}
			else
			{
				((Control)this).Top = __26CA.Y - ((Control)this).Height;
			}
		}
		val.SizeChanged += _26CC;
	}

	private void _26CD(object _٠, EventArgs ؿ)
	{
		((ToolStripDropDown)this).Close((ToolStripDropDownCloseReason)4);
	}

	protected void _26CE(int ᖩ)
	{
		if (__26C5)
		{
			return;
		}
		__26C4 = true;
		try
		{
			Rectangle bounds = ((Control)this).Bounds;
			bounds.Size = _26D3(ᖩ);
			if (!_26D1(ePopupResizeEdge.None) && ᖩ > 0 && bounds.Width - 2 > ᖩ && !_26D1(ePopupResizeEdge.Right))
			{
				bounds.X -= bounds.Width - 2 - ᖩ;
			}
			((Control)this).Bounds = bounds;
		}
		finally
		{
			__26C4 = false;
		}
	}

	protected void _26CF()
	{
		if (__26C4)
		{
			return;
		}
		__26C5 = true;
		try
		{
			Control val = _26D0();
			if (val != null)
			{
				Rectangle bounds = val.Bounds;
				if (_26D1(ePopupResizeEdge.TopLeft) || _26D1(ePopupResizeEdge.TopRight))
				{
					Size size = _26C7;
					bounds.Location = new Point(1, Dpi.Height(size.Height));
				}
				else
				{
					bounds.Location = new Point(1, 1);
				}
				bounds.Width = ((Control)this).ClientRectangle.Width - 2;
				bounds.Height = ((Control)this).ClientRectangle.Height - 2;
				if (_26C1)
				{
					int height = bounds.Height;
					Size size2 = _26C7;
					bounds.Height = height - Dpi.Height(size2.Height);
				}
				if (bounds.Size != val.Size)
				{
					val.Size = bounds.Size;
				}
				if (bounds.Location != val.Location)
				{
					val.Location = bounds.Location;
				}
				if (__26CB != null)
				{
					__26CB._0CCA.TopInternal = ((Control)this).Height - __26CB._0CCA.HeightInternal - 1;
				}
			}
		}
		finally
		{
			__26C5 = false;
		}
	}

	public Control _26D0()
	{
		if (((ArrangedElementCollection)((ToolStrip)this).Items).Count > 0)
		{
			ToolStripItem obj = ((ToolStrip)this).Items[0];
			ToolStripControlHost val = (ToolStripControlHost)(object)((obj is ToolStripControlHost) ? obj : null);
			if (val != null)
			{
				return val.Control;
			}
		}
		return null;
	}

	public bool _26D1(ePopupResizeEdge _26D2)
	{
		return (_26BF & _26D2) == _26D2;
	}

	protected Size _26D3(int ᖩ)
	{
		Size result = Size.Empty;
		__26C6 = false;
		Control val = _26D0();
		if (val != null)
		{
			if (_26D1(ePopupResizeEdge.TopLeft) || _26D1(ePopupResizeEdge.TopRight))
			{
				val.Location = new Point(1, Dpi.Height16);
			}
			else
			{
				val.Location = new Point(1, 1);
			}
			result = ((Control)this).SizeFromClientSize(val.Size);
			if (ᖩ > 0 && result.Width < ᖩ)
			{
				result.Width = ᖩ;
				__26C6 = true;
			}
		}
		if (_26C1)
		{
			result.Height += Dpi.Height16;
		}
		result.Width += 2;
		result.Height += 2;
		return result;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	protected override void WndProc(ref Message m)
	{
		if (!_26D4(ref m, _26D5: false))
		{
			((ToolStripDropDown)this).WndProc(ref m);
		}
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public bool _26D4(ref Message Ⴘ)
	{
		return _26D4(ref Ⴘ, _26D5: true);
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	private bool _26D4(ref Message Ⴘ, bool _26D5)
	{
		if (_26BF != 0)
		{
			if (((Message)(ref Ⴘ)).Msg == 132)
			{
				return _26D7(ref Ⴘ, _26D5);
			}
			if (((Message)(ref Ⴘ)).Msg == 36)
			{
				return _26D6(ref Ⴘ);
			}
		}
		return false;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	private bool _26D6(ref Message Ⴘ)
	{
		Control val = _26D0();
		if (val != null)
		{
			_0EFE.MINMAXINFO mINMAXINFO = (_0EFE.MINMAXINFO)Marshal.PtrToStructure(((Message)(ref Ⴘ)).LParam, typeof(_0EFE.MINMAXINFO));
			if (val.MaximumSize.Width != 0)
			{
				mINMAXINFO.maxTrackSize.Width = val.MaximumSize.Width;
			}
			if (val.MaximumSize.Height != 0)
			{
				mINMAXINFO.maxTrackSize.Height = val.MaximumSize.Height;
			}
			mINMAXINFO.minTrackSize = new Size(32, 32);
			if (val.MinimumSize.Width > mINMAXINFO.minTrackSize.Width)
			{
				mINMAXINFO.minTrackSize.Width = val.MinimumSize.Width;
			}
			if (val.MinimumSize.Height > mINMAXINFO.minTrackSize.Height)
			{
				mINMAXINFO.minTrackSize.Height = val.MinimumSize.Height;
			}
			Marshal.StructureToPtr((object)mINMAXINFO, ((Message)(ref Ⴘ)).LParam, fDeleteOld: false);
		}
		return true;
	}

	private bool _26D7(ref Message Ⴘ, bool _26D5)
	{
		Point pt = ((Control)this).PointToClient(new Point(_0EFE._0F0B(((Message)(ref Ⴘ)).LParam), _0EFE._0F0D(((Message)(ref Ⴘ)).LParam)));
		IntPtr intPtr = new IntPtr(-1);
		if (_26C0.Contains(pt))
		{
			if (_26D1(ePopupResizeEdge.BottomLeft))
			{
				((Message)(ref Ⴘ)).Result = (_26D5 ? intPtr : ((IntPtr)16L));
				return true;
			}
			if (_26D1(ePopupResizeEdge.BottomRight))
			{
				((Message)(ref Ⴘ)).Result = (_26D5 ? intPtr : ((IntPtr)17L));
				return true;
			}
			if (_26D1(ePopupResizeEdge.TopLeft))
			{
				((Message)(ref Ⴘ)).Result = (_26D5 ? intPtr : ((IntPtr)13L));
				return true;
			}
			if (_26D1(ePopupResizeEdge.TopRight))
			{
				((Message)(ref Ⴘ)).Result = (_26D5 ? intPtr : ((IntPtr)14L));
				return true;
			}
		}
		else
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			if (pt.X > clientRectangle.Right - 3 && pt.X <= clientRectangle.Right && _26D1(ePopupResizeEdge.Right))
			{
				((Message)(ref Ⴘ)).Result = (_26D5 ? intPtr : ((IntPtr)11L));
				return true;
			}
			if (pt.Y > clientRectangle.Bottom - 3 && pt.Y <= clientRectangle.Bottom && _26D1(ePopupResizeEdge.Bottom))
			{
				((Message)(ref Ⴘ)).Result = (_26D5 ? intPtr : ((IntPtr)15L));
				return true;
			}
			if (pt.X > -1 && pt.X < 3 && _26D1(ePopupResizeEdge.Left))
			{
				((Message)(ref Ⴘ)).Result = (_26D5 ? intPtr : ((IntPtr)10L));
				return true;
			}
			if (pt.Y > -1 && pt.Y < 3 && _26D1(ePopupResizeEdge.Top))
			{
				((Message)(ref Ⴘ)).Result = (_26D5 ? intPtr : ((IntPtr)12L));
				return true;
			}
		}
		return false;
	}
}
internal struct _2AD2
{
	public bool _2AD3;

	public int _2AD4;

	public int _2AD5;

	public IntPtr _2AD6;

	public IntPtr _2AD7;
}
internal class _2AD8
{
	[DllImport("user32.dll")]
	public static extern IntPtr CreateIconIndirect(ref _2AD2 icon);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetIconInfo(IntPtr hIcon, ref _2AD2 pIconInfo);

	[DllImport("gdi32.dll")]
	public static extern bool DeleteObject(IntPtr handle);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern bool DestroyIcon(IntPtr handle);

	public static Cursor _2AD9(Bitmap _2ADA, int _2AD4, int _2AD5, bool _2ADB = true)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		IntPtr intPtr = (_2ADB ? _2ADC((Image)(object)_2ADA, 32, 32).GetHicon() : _2ADA.GetHicon());
		_2AD2 pIconInfo = default(_2AD2);
		GetIconInfo(intPtr, ref pIconInfo);
		pIconInfo._2AD4 = _2AD4;
		pIconInfo._2AD5 = _2AD5;
		pIconInfo._2AD3 = false;
		IntPtr intPtr2 = CreateIconIndirect(ref pIconInfo);
		if (pIconInfo._2AD7 != IntPtr.Zero)
		{
			DeleteObject(pIconInfo._2AD7);
		}
		if (pIconInfo._2AD6 != IntPtr.Zero)
		{
			DeleteObject(pIconInfo._2AD6);
		}
		if (intPtr != IntPtr.Zero)
		{
			DestroyIcon(intPtr);
		}
		Cursor val = new Cursor(intPtr2);
		val.Tag = (_2ADB ? new Size(32, 32) : ((Image)_2ADA).Size);
		return val;
	}

	public static Bitmap _2ADC(Image ແ, int _2ADD, int _2ADE)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		double num = Math.Min((double)_2ADE / (double)ແ.Height, (double)_2ADD / (double)ແ.Width);
		int num2 = (int)((double)ແ.Width * num);
		int num3 = (int)((double)ແ.Height * num);
		Bitmap val = new Bitmap(num2, num3);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.DrawImage(ແ, 0, 0, num2, num3);
			return val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static Bitmap _2ADF(Control _0BDE, Color _2AE0)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		Bitmap val = new Bitmap(_0BDE.Width, _0BDE.Height);
		_0BDE.DrawToBitmap(val, new Rectangle(0, 0, _0BDE.Width, _0BDE.Height));
		if (!_2AE0.IsEmpty)
		{
			val.MakeTransparent(_2AE0);
		}
		return val;
	}

	public static Bitmap _2AE1(Bitmap _2AE2, Bitmap _2AE3, Point _2AE4)
	{
		Graphics val = Graphics.FromImage((Image)(object)_2AE2);
		try
		{
			val.DrawImage((Image)(object)_2AE3, new Rectangle(_2AE4, ((Image)_2AE3).Size));
			return _2AE2;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
