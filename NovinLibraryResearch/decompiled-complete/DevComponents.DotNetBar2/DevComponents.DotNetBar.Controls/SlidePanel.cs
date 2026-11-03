using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Animation;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(CircularProgress), "Controls.SlidePanel.ico")]
[Designer("DevComponents.DotNetBar.Design.SlidePanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class SlidePanel : UserControl
{
	internal const int _2365 = 500;

	private EventHandler _2366;

	private eSlideSide __2367;

	private bool __20D1 = true;

	private bool __2368;

	private Rectangle __2369 = Rectangle.Empty;

	private bool __236A;

	private _1A18 __19EC;

	private bool __236B;

	private SliderButton __236C;

	private int __20D7 = 500;

	private bool __236D = true;

	private Size __236E = SliderButton._23C5;

	private Size __236F = SliderButton._23C7;

	private ElementStyle __2370;

	private bool __2371;

	private Color __0EA4 = Color.Empty;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ = 10f;

	[DefaultValue(eSlideSide.Left)]
	[Category("Behavior")]
	[Description("Indicates side panel slides into.")]
	public eSlideSide SlideSide
	{
		get
		{
			return __2367;
		}
		set
		{
			if (value != __2367)
			{
				eSlideSide oldValue = __2367;
				__2367 = value;
				OnSlideSideChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	[Description("Indicates whether panel is open. When this property is changed panel will slide in or out of the view.")]
	public bool IsOpen
	{
		get
		{
			return __20D1;
		}
		set
		{
			if ((!__2368 || __19EC == null) && !__236A && value != __20D1)
			{
				bool oldValue = __20D1;
				__20D1 = value;
				OnIsOpenChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public bool UsesBlockingAnimation
	{
		get
		{
			return __2368;
		}
		set
		{
			__2368 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Rectangle OpenBounds
	{
		get
		{
			return __2369;
		}
		set
		{
			__2369 = value;
			if (!((Control)this).Visible && ((Control)this).Bounds.Size != __2369.Size)
			{
				((Control)this).Bounds = _2377(((Control)this).Parent, __2369);
			}
		}
	}

	internal bool _2364 => __236B;

	[Category("Behavior")]
	[Description("Indicates animation duration time in milliseconds. Setting this property to 0 will disable slide animation.")]
	[DefaultValue(500)]
	public int AnimationTime
	{
		get
		{
			return __20D7;
		}
		set
		{
			if (value != __20D7)
			{
				int oldValue = __20D7;
				__20D7 = value;
				OnAnimationTimeChanged(oldValue, value);
			}
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether slide out button is shown when panel is out of the view and which allows panel to be shown.")]
	[DefaultValue(true)]
	public bool SlideOutButtonVisible
	{
		get
		{
			return __236D;
		}
		set
		{
			if (value != __236D)
			{
				bool oldValue = __236D;
				__236D = value;
				OnSlideOutButtonVisibleChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates slide-out buttton size in default state.")]
	public Size SlideOutButtonSize
	{
		get
		{
			return __236E;
		}
		set
		{
			if (value != __236E)
			{
				Size oldValue = __236E;
				__236E = value;
				OnSlideOutButtonSizeChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates active (mouse over) slide out button size.")]
	public Size SlideOutActiveButtonSize
	{
		get
		{
			return __236F;
		}
		set
		{
			if (value != __236F)
			{
				Size oldValue = __236F;
				__236F = value;
				OnSlideOutActiveButtonSizeChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates slide-out button style.")]
	[DefaultValue(null)]
	public ElementStyle SlideOutButtonStyle
	{
		get
		{
			return __2370;
		}
		set
		{
			if (value != __2370)
			{
				ElementStyle oldValue = __2370;
				__2370 = value;
				OnSlideOutButtonStyleChanged(oldValue, value);
			}
		}
	}

	internal bool Ԭ
	{
		get
		{
			return __2371;
		}
		set
		{
			if (value != __2371)
			{
				bool oldValue = __2371;
				__2371 = value;
				OnCenterContentChanged(oldValue, value);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Bindable(true)]
	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((UserControl)this).Text;
		}
		set
		{
			((UserControl)this).Text = value;
		}
	}

	[Description("Indicates color of the Symbol displayed on slideout button.")]
	[Category("Appearance")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates the symbol displayed on face of the slideout button.")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string Symbol
	{
		get
		{
			return _ລ;
		}
		set
		{
			if (value != _ລ)
			{
				string ລ = _ລ;
				_ລ = value;
				OnSymbolChanged(ລ, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(eSymbolSet.Awesome)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ວ;
		}
		set
		{
			if (_ວ != value)
			{
				eSymbolSet ວ = _ວ;
				_ວ = value;
				OnSymbolSetChanged(ວ, value);
			}
		}
	}

	[DefaultValue(10f)]
	[Description("Indicates the size of the symbol in points.")]
	[Category("Appearance")]
	public float SymbolSize
	{
		get
		{
			return _ຨ;
		}
		set
		{
			if (value != _ຨ)
			{
				float ຨ = _ຨ;
				_ຨ = value;
				OnSymbolSizeChanged(ຨ, value);
			}
		}
	}

	[Description("Occurs when IsOpen property value has changed, i.e. slide-panel is shown or hidden.")]
	public event EventHandler IsOpenChanged
	{
		add
		{
			EventHandler eventHandler = _2366;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2366, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2366;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2366, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnIsOpenChanged(EventArgs e)
	{
		_2366?.Invoke(this, e);
	}

	protected override void Dispose(bool disposing)
	{
		_2374();
		((ContainerControl)this).Dispose(disposing);
	}

	protected virtual void OnSlideSideChanged(eSlideSide oldValue, eSlideSide newValue)
	{
		if (!IsOpen)
		{
			WaitForCurrentAnimationFinish();
			((Control)this).Bounds = _2377(((Control)this).Parent, __2369);
			if (__236D)
			{
				_2376();
			}
		}
	}

	protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
	{
		if (!newValue)
		{
			_2375();
		}
		else
		{
			_2372();
		}
		OnIsOpenChanged(EventArgs.Empty);
	}

	private void _2372()
	{
		_1A18 obj = __19EC;
		if (obj != null)
		{
			obj.Stop();
			WaitForCurrentAnimationFinish();
		}
		Rectangle bounds = ((Control)this).Bounds;
		Rectangle rectangle = __2369;
		if (__236C != null)
		{
			((Control)__236C).Visible = false;
		}
		if (__20D7 > 0)
		{
			__236B = true;
			((Control)this).Visible = true;
			_20EE obj2 = new _20EE(new ခ(this, "Bounds", bounds, rectangle), AnimationEasing.EaseOutExpo, __20D7);
			obj2.AnimationCompleted += ચ;
			obj2.Start();
			__19EC = obj2;
		}
		else
		{
			((Control)this).Bounds = rectangle;
		}
		if (__236D && __236C != null)
		{
			_2374();
		}
	}

	internal void _2373()
	{
		_1A18 obj = __19EC;
		__19EC = null;
		if (obj != null)
		{
			obj.Stop();
			obj.Dispose();
		}
	}

	public void WaitForCurrentAnimationFinish()
	{
		if (__236A)
		{
			return;
		}
		__236A = true;
		try
		{
			if (__19EC == null)
			{
				return;
			}
			DateTime now = DateTime.Now;
			while (__19EC != null)
			{
				Application.DoEvents();
				if (DateTime.Now.Subtract(now).TotalMilliseconds > (double)((float)__20D7 * 1.5f))
				{
					_2373();
					break;
				}
			}
		}
		finally
		{
			__236A = false;
		}
	}

	private void _2374()
	{
		if (__236C != null)
		{
			Control parent = ((Control)__236C).Parent;
			if (parent != null)
			{
				parent.Resize -= _0A76;
				parent.Controls.Remove((Control)(object)__236C);
			}
			((Component)(object)__236C).Dispose();
			__236C = null;
		}
	}

	private void _2375()
	{
		_1A18 obj = __19EC;
		if (obj != null)
		{
			obj.Stop();
			WaitForCurrentAnimationFinish();
		}
		Rectangle bounds = ((Control)this).Bounds;
		Rectangle rectangle = _2377();
		if (__20D7 > 0 && ((Control)this).Visible)
		{
			__236B = true;
			_20EE obj2 = new _20EE(new ခ(this, "Bounds", bounds, rectangle), AnimationEasing.EaseOutExpo, __20D7);
			obj2.AnimationCompleted += ચ;
			obj2.Start();
			__19EC = obj2;
		}
		else
		{
			((Control)this).Bounds = rectangle;
		}
		__2369 = bounds;
		if (__236D)
		{
			_2376();
		}
	}

	private void ચ(object _٠, EventArgs ؿ)
	{
		_1A18 obj = __19EC;
		__19EC = null;
		obj?.Dispose();
		if (!__20D1)
		{
			((Control)this).Visible = false;
		}
		__236B = false;
	}

	public void SlideOutOfViewSilent(Control parent)
	{
		__2369 = ((Control)this).Bounds;
		((Control)this).Bounds = _2377(parent);
		__20D1 = false;
	}

	private void _2376()
	{
		_2374();
		Control parent = ((Control)this).Parent;
		if (parent != null)
		{
			__236C = new SliderButton(this);
			if (__2370 != null)
			{
				__236C.Style = __2370;
			}
			else
			{
				__236C.Style = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
			}
			Size activeSliderSize = Dpi.Size(__236F);
			Size sliderSize = Dpi.Size(__236E);
			if (__2367 == eSlideSide.Top || __2367 == eSlideSide.Bottom)
			{
				activeSliderSize = new Size(activeSliderSize.Height, activeSliderSize.Width);
				sliderSize = new Size(sliderSize.Height, sliderSize.Width);
			}
			__236C.ActiveSliderSize = activeSliderSize;
			__236C.SliderSize = sliderSize;
			__236C.Symbol = _ລ;
			__236C.SymbolColor = __0EA4;
			__236C.SymbolSet = _ວ;
			__236C.SymbolSize = _ຨ;
			parent.Controls.Add((Control)(object)__236C);
			parent.Controls.SetChildIndex((Control)(object)__236C, 0);
			parent.Resize += _0A76;
		}
	}

	private void _0A76(object _٠, EventArgs ؿ)
	{
		if (__236C != null)
		{
			__236C._23C9();
		}
	}

	private Rectangle _2377()
	{
		return _2377(((Control)this).Parent);
	}

	private Rectangle _2377(Control ԟ)
	{
		return _2377(ԟ, ((Control)this).Bounds);
	}

	private Rectangle _2377(Control ԟ, Rectangle _2378)
	{
		if (ԟ == null)
		{
			return _2378;
		}
		Rectangle result = _2378;
		if (__2367 == eSlideSide.Left)
		{
			result.X = -result.Width;
		}
		else if (__2367 == eSlideSide.Right)
		{
			result.X = ԟ.Width;
		}
		else if (__2367 == eSlideSide.Top)
		{
			result.Y = -result.Height;
		}
		else if (__2367 == eSlideSide.Bottom)
		{
			result.Y = ԟ.Height;
		}
		return result;
	}

	protected virtual void OnAnimationTimeChanged(int oldValue, int newValue)
	{
	}

	protected virtual void OnSlideOutButtonVisibleChanged(bool oldValue, bool newValue)
	{
		if (!__20D1)
		{
			if (newValue)
			{
				_2376();
			}
			else
			{
				_2374();
			}
		}
	}

	protected virtual void OnSlideOutButtonSizeChanged(Size oldValue, Size newValue)
	{
		if (__236C != null)
		{
			__236C.SliderSize = newValue;
			__236C._23C9();
		}
	}

	public bool ShouldSerializeSlideOutButtonSize()
	{
		return __236E != SliderButton._23C5;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSlideOutButtonSize()
	{
		SlideOutButtonSize = SliderButton._23C5;
	}

	protected virtual void OnSlideOutActiveButtonSizeChanged(Size oldValue, Size newValue)
	{
		if (__236C != null)
		{
			__236C.ActiveSliderSize = newValue;
			__236C._23C9();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSlideOutActiveButtonSize()
	{
		return __236F != SliderButton._23C7;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSlideOutActiveButtonSize()
	{
		SlideOutActiveButtonSize = SliderButton._23C7;
	}

	protected virtual void OnSlideOutButtonStyleChanged(ElementStyle oldValue, ElementStyle newValue)
	{
		if (newValue != null && __236C != null)
		{
			__236C.Style = newValue;
		}
	}

	protected virtual void OnCenterContentChanged(bool oldValue, bool newValue)
	{
		if (newValue)
		{
			_237A();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		if (__2371)
		{
			_237A();
		}
		((UserControl)this).OnResize(e);
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (__2371)
		{
			_2379(e.Control);
		}
		((Control)this).OnControlAdded(e);
	}

	private void _2379(Control _06EC)
	{
		Point point = new Point(Math.Max(0, (((Control)this).Width - _06EC.Width) / 2), Math.Max(0, (((Control)this).Height - _06EC.Height) / 2));
		if (_06EC.Location != point)
		{
			_06EC.Location = point;
		}
	}

	private void _237A()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			_2379(val);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return !__0EA4.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		SymbolColor = Color.Empty;
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			__0EA6 = "";
		}
		else
		{
			__0EA6 = Symbols.GetSymbol(newValue);
		}
		((Control)this).Invalidate();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		((Control)this).Invalidate();
		((Control)this).Refresh();
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		((Control)this).Invalidate();
	}
}
