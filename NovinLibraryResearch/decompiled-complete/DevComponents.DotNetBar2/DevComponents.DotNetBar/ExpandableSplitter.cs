using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.ExpandableSplitterDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("ExpandedChanged")]
[ComVisible(false)]
public class ExpandableSplitter : Splitter, ሎ
{
	private int ᇰ = 100;

	private Control ⷐ;

	private bool Ӫ = true;

	private eSplitterStyle Ӷ;

	private bool ⷑ = true;

	private eShortcut _27C5;

	private bool _২;

	private bool ݡ;

	private bool ݠ;

	private bool ⷒ;

	private Point ၽ = Point.Empty;

	private ColorScheme լ;

	private eColorSchemePart ދ = eColorSchemePart.None;

	private Color ތ = Color.Empty;

	private eColorSchemePart ލ = eColorSchemePart.None;

	private int ގ;

	private Color ⷓ = Color.Empty;

	private eColorSchemePart ⷔ = eColorSchemePart.None;

	private Color չ = Color.Empty;

	private eColorSchemePart պ = eColorSchemePart.None;

	private Color ⷕ = Color.Empty;

	private eColorSchemePart ⷖ = eColorSchemePart.None;

	private Color _2DD7 = Color.Empty;

	private eColorSchemePart ⷘ = eColorSchemePart.None;

	private Color ⷙ = Color.Empty;

	private eColorSchemePart ⷚ = eColorSchemePart.None;

	private Color ⷛ = Color.Empty;

	private eColorSchemePart ⷜ = eColorSchemePart.None;

	private int ⷝ;

	private Color ⷞ = Color.Empty;

	private eColorSchemePart _2DDF = eColorSchemePart.None;

	private Color _2DE0 = Color.Empty;

	private eColorSchemePart _2DE1 = eColorSchemePart.None;

	private Color _2DE2 = Color.Empty;

	private eColorSchemePart _2DE3 = eColorSchemePart.None;

	private Color _2DE4 = Color.Empty;

	private eColorSchemePart _2DE5 = eColorSchemePart.None;

	private _2DEF _2DE6;

	private _2DF3 _2DE7 = new _2DF3();

	private bool _2DE8 = true;

	private bool _2DE9;

	private ExpandChangeEventHandler ℏ;

	private ExpandChangeEventHandler _1C2B;

	[DefaultValue(true)]
	[Category("Expand")]
	[Browsable(true)]
	[Description("Indicates whether expandable control ExpandableControl assigned to this splitter is expaned or not. Default value is true.")]
	public bool Expanded
	{
		get
		{
			return Ӫ;
		}
		set
		{
			if (Ӫ != value)
			{
				_211E(value, eEventSource.Code);
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates whether Click event is triggering expand/collapse of the splitter. Default value is true.")]
	[Category("Expand")]
	[DefaultValue(true)]
	public bool ExpandActionClick
	{
		get
		{
			return _2DE8;
		}
		set
		{
			_2DE8 = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether DoubleClick event is triggering expand/collapse of the splitter. Default value is false.")]
	[Browsable(true)]
	[Category("Expand")]
	public bool ExpandActionDoubleClick
	{
		get
		{
			return _2DE9;
		}
		set
		{
			_2DE9 = value;
		}
	}

	[Category("Expand")]
	[Description("Indicates whether splitter will act as expandable splitter. Default value is true.")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool Expandable
	{
		get
		{
			return ⷑ;
		}
		set
		{
			ⷑ = value;
			((Control)this).Refresh();
		}
	}

	[DefaultValue(null)]
	[Description("Indicates control that will be expanded/collapsed by the splitter.")]
	[Browsable(true)]
	[Category("Expand")]
	public Control ExpandableControl
	{
		get
		{
			return ⷐ;
		}
		set
		{
			if (value != this)
			{
				ⷐ = value;
				_2DEE();
			}
		}
	}

	[Description("Indicates visual style of the control.")]
	[DefaultValue(eSplitterStyle.Office2003)]
	[Category("Appearance")]
	[Browsable(true)]
	public eSplitterStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			Ӷ = value;
			ߓ();
		}
	}

	[DefaultValue(100)]
	[Category("Expand")]
	[Description("Indicates animation time in milliseconds, default value is 100.")]
	[Browsable(true)]
	public int AnimationTime
	{
		get
		{
			return ᇰ;
		}
		set
		{
			if (ᇰ >= 0)
			{
				ᇰ = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates shortcut key to expand/collapse splitter.")]
	[Category("Expand")]
	[DefaultValue(eShortcut.None)]
	public eShortcut Shortcut
	{
		get
		{
			return _27C5;
		}
		set
		{
			_27C5 = value;
			if (_27C5 != 0 && ((Control)this).IsHandleCreated)
			{
				_2818();
			}
		}
	}

	[Description("Gets or sets the background color for UI element.")]
	[Category("Colors")]
	[Browsable(true)]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	public Color BackColor
	{
		get
		{
			return _065A(((Control)this).BackColor, ދ);
		}
		set
		{
			if (ދ != eColorSchemePart.None && !value.IsEmpty)
			{
				BackColorSchemePart = eColorSchemePart.None;
			}
			((Control)this).BackColor = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart BackColorSchemePart
	{
		get
		{
			return ދ;
		}
		set
		{
			ދ = value;
			ߓ();
		}
	}

	[Category("Colors")]
	[Browsable(true)]
	[Description("Gets or sets the target gradient background color for UI element.")]
	public Color BackColor2
	{
		get
		{
			return _065A(ތ, ލ);
		}
		set
		{
			if (ލ != eColorSchemePart.None && !value.IsEmpty)
			{
				BackColor2SchemePart = eColorSchemePart.None;
			}
			ތ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart BackColor2SchemePart
	{
		get
		{
			return ލ;
		}
		set
		{
			ލ = value;
			ߓ();
		}
	}

	[Description("Gets or sets the background gradient angle.")]
	[DefaultValue(0)]
	[Browsable(true)]
	[Category("Background")]
	public int BackColorGradientAngle
	{
		get
		{
			return ގ;
		}
		set
		{
			if (ގ != value)
			{
				ގ = value;
				ߓ();
			}
		}
	}

	[Category("Colors")]
	[Description("Indicates expand part fill color.")]
	[Browsable(true)]
	public Color ExpandFillColor
	{
		get
		{
			return _065A(ⷓ, ⷔ);
		}
		set
		{
			if (ⷔ != eColorSchemePart.None && !value.IsEmpty)
			{
				ExpandFillColorSchemePart = eColorSchemePart.None;
			}
			ⷓ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart ExpandFillColorSchemePart
	{
		get
		{
			return ⷔ;
		}
		set
		{
			ⷔ = value;
			ߓ();
		}
	}

	[Category("Colors")]
	[Browsable(true)]
	[Description("Indicates expand part line color.")]
	public Color ExpandLineColor
	{
		get
		{
			return _065A(չ, պ);
		}
		set
		{
			if (պ != eColorSchemePart.None && !value.IsEmpty)
			{
				ExpandLineColorSchemePart = eColorSchemePart.None;
			}
			չ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart ExpandLineColorSchemePart
	{
		get
		{
			return պ;
		}
		set
		{
			պ = value;
			ߓ();
		}
	}

	[Category("Colors")]
	[Description("Indicates expand part line color.")]
	[Browsable(true)]
	public Color GripDarkColor
	{
		get
		{
			return _065A(ⷕ, ⷖ);
		}
		set
		{
			if (ⷖ != eColorSchemePart.None && !value.IsEmpty)
			{
				GripDarkColorSchemePart = eColorSchemePart.None;
			}
			ⷕ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart GripDarkColorSchemePart
	{
		get
		{
			return ⷖ;
		}
		set
		{
			ⷖ = value;
			ߓ();
		}
	}

	[Category("Colors")]
	[Description("Indicates expand part line color.")]
	[Browsable(true)]
	public Color GripLightColor
	{
		get
		{
			return _065A(_2DD7, ⷘ);
		}
		set
		{
			if (ⷘ != eColorSchemePart.None && !value.IsEmpty)
			{
				GripLightColorSchemePart = eColorSchemePart.None;
			}
			_2DD7 = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart GripLightColorSchemePart
	{
		get
		{
			return ⷘ;
		}
		set
		{
			ⷘ = value;
			ߓ();
		}
	}

	[Description("Gets or sets the background color for UI element when mouse is over the element.")]
	[Browsable(true)]
	[Category("Colors")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	public Color HotBackColor
	{
		get
		{
			return _065A(ⷙ, ⷚ);
		}
		set
		{
			if (ⷚ != eColorSchemePart.None && !value.IsEmpty)
			{
				HotBackColorSchemePart = eColorSchemePart.None;
			}
			ⷙ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotBackColorSchemePart
	{
		get
		{
			return ⷚ;
		}
		set
		{
			ⷚ = value;
			ߓ();
		}
	}

	[Category("Colors")]
	[Browsable(true)]
	[Description("Gets or sets the target gradient background color for UI element when mouse is over the element.")]
	public Color HotBackColor2
	{
		get
		{
			return _065A(ⷛ, ⷜ);
		}
		set
		{
			if (ⷜ != eColorSchemePart.None && !value.IsEmpty)
			{
				HotBackColor2SchemePart = eColorSchemePart.None;
			}
			ⷛ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotBackColor2SchemePart
	{
		get
		{
			return ⷜ;
		}
		set
		{
			ⷜ = value;
			ߓ();
		}
	}

	[Category("Background")]
	[Description("Gets or sets the background gradient angle when mouse is over the element.")]
	[Browsable(true)]
	[DefaultValue(0)]
	public int HotBackColorGradientAngle
	{
		get
		{
			return ⷝ;
		}
		set
		{
			if (ⷝ != value)
			{
				ⷝ = value;
				ߓ();
			}
		}
	}

	[Browsable(true)]
	[Category("Colors")]
	[Description("Indicates expand part fill color when mouse is over the element.")]
	public Color HotExpandFillColor
	{
		get
		{
			return _065A(ⷞ, _2DDF);
		}
		set
		{
			if (_2DDF != eColorSchemePart.None && !value.IsEmpty)
			{
				HotExpandFillColorSchemePart = eColorSchemePart.None;
			}
			ⷞ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotExpandFillColorSchemePart
	{
		get
		{
			return _2DDF;
		}
		set
		{
			_2DDF = value;
			ߓ();
		}
	}

	[Category("Colors")]
	[Browsable(true)]
	[Description("Indicates expand part line color when mouse is over the element.")]
	public Color HotExpandLineColor
	{
		get
		{
			return _065A(_2DE0, _2DE1);
		}
		set
		{
			if (_2DE1 != eColorSchemePart.None && !value.IsEmpty)
			{
				HotExpandLineColorSchemePart = eColorSchemePart.None;
			}
			_2DE0 = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotExpandLineColorSchemePart
	{
		get
		{
			return _2DE1;
		}
		set
		{
			_2DE1 = value;
			ߓ();
		}
	}

	[Category("Colors")]
	[Description("Indicates expand part line color when mouse is over the element.")]
	[Browsable(true)]
	public Color HotGripDarkColor
	{
		get
		{
			return _065A(_2DE2, _2DE3);
		}
		set
		{
			if (_2DE3 != eColorSchemePart.None && !value.IsEmpty)
			{
				HotGripDarkColorSchemePart = eColorSchemePart.None;
			}
			_2DE2 = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotGripDarkColorSchemePart
	{
		get
		{
			return _2DE3;
		}
		set
		{
			_2DE3 = value;
			ߓ();
		}
	}

	[Category("Colors")]
	[Description("Indicates grip part light color when mouse is over the element.")]
	[Browsable(true)]
	public Color HotGripLightColor
	{
		get
		{
			return _065A(_2DE4, _2DE5);
		}
		set
		{
			if (_2DE5 != eColorSchemePart.None && !value.IsEmpty)
			{
				HotGripLightColorSchemePart = eColorSchemePart.None;
			}
			_2DE4 = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public eColorSchemePart HotGripLightColorSchemePart
	{
		get
		{
			return _2DE5;
		}
		set
		{
			_2DE5 = value;
			ߓ();
		}
	}

	bool ሎ.প
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return val.Modal;
			}
			return false;
		}
	}

	public event ExpandChangeEventHandler ExpandedChanging
	{
		add
		{
			ExpandChangeEventHandler expandChangeEventHandler = ℏ;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Combine(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref ℏ, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
		remove
		{
			ExpandChangeEventHandler expandChangeEventHandler = ℏ;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Remove(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref ℏ, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
	}

	public event ExpandChangeEventHandler ExpandedChanged
	{
		add
		{
			ExpandChangeEventHandler expandChangeEventHandler = _1C2B;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Combine(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref _1C2B, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
		remove
		{
			ExpandChangeEventHandler expandChangeEventHandler = _1C2B;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Remove(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref _1C2B, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
	}

	public ExpandableSplitter()
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		լ = new ColorScheme(eDotNetBarStyle.Office2003);
		ApplyStyle(eSplitterStyle.Office2003);
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		((Control)this).Dispose(disposing);
	}

	private _2DEF _2DEA()
	{
		if (_2DE6 == null)
		{
			switch (Ӷ)
			{
			case eSplitterStyle.Office2003:
			case eSplitterStyle.Office2007:
				_2DE6 = new _2DFE();
				break;
			case eSplitterStyle.Mozilla:
				_2DE6 = new _2DFF();
				break;
			}
		}
		return _2DE6;
	}

	private _2DF3 _2DEB(Graphics _٥)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		_2DE7.ܓ = _٥;
		_2DE7._2DF4 = _2517();
		_2DE7._2DF5 = ⷑ;
		_2DE7._2DF6 = Ӫ;
		_2DE7._2DF7 = ((Control)this).Dock;
		_2DE7.ᆽ = ((Control)this).DisplayRectangle;
		return _2DE7;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		_2DEA().Paint(_2DEB(e.Graphics));
	}

	private void ᱞ()
	{
		if (ExpandableControl == null)
		{
			return;
		}
		if (Expanded)
		{
			if (!ExpandableControl.Visible)
			{
				if (AnimationTime == 0 || ((Component)this).DesignMode)
				{
					if (((Component)this).DesignMode)
					{
						TypeDescriptor.GetProperties(ExpandableControl)["Visible"].SetValue(ExpandableControl, true);
					}
					ExpandableControl.Visible = true;
				}
				else
				{
					Rectangle rectStart = _2DEC(ExpandableControl, _2DED: false);
					Rectangle bounds = ExpandableControl.Bounds;
					BarFunctions.AnimateControl(ExpandableControl, show: true, ᇰ, rectStart, bounds);
				}
			}
		}
		else if (ExpandableControl.Visible || !((Control)this).IsHandleCreated)
		{
			if (AnimationTime == 0 || ((Component)this).DesignMode || !((Control)this).IsHandleCreated)
			{
				if (((Component)this).DesignMode)
				{
					TypeDescriptor.GetProperties(ExpandableControl)["Visible"].SetValue(ExpandableControl, false);
				}
				ExpandableControl.Visible = false;
			}
			else
			{
				Rectangle bounds2 = ExpandableControl.Bounds;
				Rectangle rectEnd = _2DEC(ExpandableControl, _2DED: false);
				BarFunctions.AnimateControl(ExpandableControl, show: false, ᇰ, bounds2, rectEnd);
				ExpandableControl.Visible = false;
				ExpandableControl.Bounds = bounds2;
			}
		}
		((Control)this).Refresh();
	}

	private Rectangle _2DEC(Control _0BDE, bool _2DED)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Invalid comparison between Unknown and I4
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		Rectangle result = Rectangle.Empty;
		DockStyle val = _2120(_0BDE);
		if (!_2DED)
		{
			if ((int)val == 3)
			{
				result = new Rectangle(_0BDE.Left, _0BDE.Top, 0, _0BDE.Height);
			}
			else if ((int)val == 4)
			{
				result = new Rectangle(_0BDE.Right, _0BDE.Top, 0, _0BDE.Height);
			}
			else if ((int)val == 1)
			{
				result = new Rectangle(_0BDE.Left, _0BDE.Top, _0BDE.Width, 0);
			}
			else if ((int)val == 2)
			{
				result = new Rectangle(_0BDE.Left, _0BDE.Bottom, _0BDE.Width, 0);
			}
		}
		else if ((int)val == 3)
		{
			result = new Rectangle(_0BDE.Left, _0BDE.Top, _0BDE.Width, _0BDE.Height);
		}
		else if ((int)val == 4)
		{
			result = new Rectangle(_0BDE.Left - _0BDE.Width, _0BDE.Top, _0BDE.Width, _0BDE.Height);
		}
		else if ((int)val == 1)
		{
			result = new Rectangle(_0BDE.Left, _0BDE.Top, _0BDE.Width, _0BDE.Height);
		}
		else if ((int)val == 2)
		{
			result = new Rectangle(_0BDE.Left, _0BDE.Top - _0BDE.Height, _0BDE.Width, _0BDE.Height);
		}
		return result;
	}

	private DockStyle _2120(Control _0BDE)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if ((int)_0BDE.Dock == 0)
		{
			if (_0BDE.Right >= ((Control)this).Left)
			{
				if (_0BDE.Left <= ((Control)this).Right)
				{
					if (_0BDE.Bottom >= ((Control)this).Top)
					{
						if (_0BDE.Top <= ((Control)this).Bottom)
						{
							return (DockStyle)3;
						}
						return (DockStyle)2;
					}
					return (DockStyle)1;
				}
				return (DockStyle)4;
			}
			return (DockStyle)3;
		}
		if ((int)_0BDE.Dock == 5)
		{
			return (DockStyle)3;
		}
		return _0BDE.Dock;
	}

	private void _2DEE()
	{
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		_2818();
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		((Control)this).OnHandleDestroyed(e);
		_2819();
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		if (Expandable && ݡ && !ⷒ && _2DE8)
		{
			_211E(!Expanded, eEventSource.Mouse);
		}
		ⷒ = false;
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		((Control)this).OnDoubleClick(e);
		if (Expandable && ݡ && _2DE9)
		{
			_211E(!Expanded, eEventSource.Mouse);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (ݠ)
		{
			((Splitter)this).OnMouseDown(e);
			return;
		}
		ⷒ = false;
		ၽ = new Point(e.X, e.Y);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		ݠ = false;
		((Splitter)this).OnMouseUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		ݡ = true;
		((Control)this).Refresh();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		ݡ = false;
		((Control)this).Refresh();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		((Splitter)this).OnMouseMove(e);
		if (!((Control)this).DisplayRectangle.Contains(e.X, e.Y) && !ݠ && (int)e.Button == 1048576)
		{
			ݠ = true;
			ⷒ = true;
			((Control)this).OnMouseDown(new MouseEventArgs(e.Button, 1, ၽ.X, ၽ.Y, e.Delta));
		}
	}

	private void _2125(ExpandedChangeEventArgs ؿ)
	{
		if (ℏ != null)
		{
			ℏ(this, ؿ);
		}
	}

	private void Ω(ExpandedChangeEventArgs ؿ)
	{
		if (_1C2B != null)
		{
			_1C2B(this, ؿ);
		}
	}

	private void _211E(bool _211F, eEventSource ڃ)
	{
		ExpandedChangeEventArgs expandedChangeEventArgs = new ExpandedChangeEventArgs(ڃ, _211F);
		_2125(expandedChangeEventArgs);
		if (!expandedChangeEventArgs.Cancel)
		{
			Ӫ = _211F;
			ᱞ();
			Ω(expandedChangeEventArgs);
		}
	}

	private void ߓ()
	{
		_2DE6 = null;
		((Control)this).Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		((Control)this).BackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		ތ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandFillColor()
	{
		ⷓ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetExpandLineColor()
	{
		չ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGripDarkColor()
	{
		ⷕ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGripLightColor()
	{
		_2DD7 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotBackColor()
	{
		ⷙ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotBackColor2()
	{
		ⷛ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotExpandFillColor()
	{
		ⷞ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotExpandLineColor()
	{
		_2DE0 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotGripDarkColor()
	{
		_2DE2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetHotGripLightColor()
	{
		_2DE4 = Color.Empty;
	}

	private Color _065A(Color _065B, eColorSchemePart _065C)
	{
		if (_065C == eColorSchemePart.None || _065C == eColorSchemePart.Custom)
		{
			return _065B;
		}
		ColorScheme colorScheme = GetColorScheme();
		if (colorScheme == null)
		{
			return _065B;
		}
		return (Color)colorScheme.GetType().GetProperty(_065C.ToString()).GetValue(colorScheme, null);
	}

	private ColorScheme GetColorScheme()
	{
		if (Style == eSplitterStyle.Office2007 && GlobalManager.Renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
		}
		return լ;
	}

	private _2DF8 _2517()
	{
		_2DF8 obj = new _2DF8();
		if (ݡ && !HotBackColor.IsEmpty)
		{
			obj.ᖳ = HotBackColor;
			obj._2DF9 = HotBackColor2;
			obj._2DFA = HotBackColorGradientAngle;
			obj._2DFD = HotExpandFillColor;
			obj._2DFC = HotExpandLineColor;
			obj._2DFB = HotGripDarkColor;
			obj._26D9 = HotGripLightColor;
		}
		else
		{
			obj.ᖳ = BackColor;
			obj._2DF9 = BackColor2;
			obj._2DFA = BackColorGradientAngle;
			obj._2DFD = ExpandFillColor;
			obj._2DFC = ExpandLineColor;
			obj._2DFB = GripDarkColor;
			obj._26D9 = GripLightColor;
		}
		return obj;
	}

	public void ApplyStyle(eSplitterStyle style)
	{
		switch (style)
		{
		case eSplitterStyle.Office2003:
			լ = new ColorScheme(eDotNetBarStyle.Office2003);
			BackColorSchemePart = eColorSchemePart.PanelBackground;
			BackColor2SchemePart = eColorSchemePart.PanelBorder;
			GripLightColorSchemePart = eColorSchemePart.BarBackground;
			GripDarkColorSchemePart = eColorSchemePart.ItemText;
			ExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
			ExpandLineColorSchemePart = eColorSchemePart.ItemText;
			HotBackColorSchemePart = eColorSchemePart.ItemPressedBackground;
			HotBackColor2SchemePart = eColorSchemePart.ItemPressedBackground2;
			HotGripLightColorSchemePart = eColorSchemePart.BarBackground;
			HotGripDarkColorSchemePart = eColorSchemePart.PanelBorder;
			HotExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
			HotExpandLineColorSchemePart = eColorSchemePart.ItemText;
			break;
		case eSplitterStyle.Office2007:
			լ = new ColorScheme(eDotNetBarStyle.Office2007);
			BackColorSchemePart = eColorSchemePart.PanelBackground;
			BackColor2SchemePart = eColorSchemePart.PanelBorder;
			GripLightColorSchemePart = eColorSchemePart.BarBackground;
			GripDarkColorSchemePart = eColorSchemePart.ItemText;
			ExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
			ExpandLineColorSchemePart = eColorSchemePart.ItemText;
			HotBackColorSchemePart = eColorSchemePart.ItemPressedBackground;
			HotBackColor2SchemePart = eColorSchemePart.ItemPressedBackground2;
			HotGripLightColorSchemePart = eColorSchemePart.BarBackground;
			HotGripDarkColorSchemePart = eColorSchemePart.PanelBorder;
			HotExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
			HotExpandLineColorSchemePart = eColorSchemePart.ItemText;
			break;
		case eSplitterStyle.Mozilla:
			լ = new ColorScheme(eDotNetBarStyle.VS2005);
			BackColorSchemePart = eColorSchemePart.None;
			BackColor = SystemColors.ControlLight;
			BackColor2SchemePart = eColorSchemePart.None;
			BackColor2 = Color.Empty;
			GripLightColorSchemePart = eColorSchemePart.MenuBackground;
			GripDarkColorSchemePart = eColorSchemePart.ItemPressedBorder;
			ExpandFillColorSchemePart = eColorSchemePart.ItemPressedBackground;
			ExpandLineColorSchemePart = eColorSchemePart.ItemPressedBorder;
			HotBackColorSchemePart = eColorSchemePart.ItemCheckedBackground;
			HotBackColor2 = Color.Empty;
			HotBackColor2SchemePart = eColorSchemePart.None;
			HotGripLightColorSchemePart = eColorSchemePart.MenuBackground;
			HotGripDarkColorSchemePart = eColorSchemePart.ItemPressedBorder;
			HotExpandFillColorSchemePart = eColorSchemePart.ItemPressedBackground;
			HotExpandLineColorSchemePart = eColorSchemePart.ItemPressedBorder;
			break;
		}
	}

	private bool _0A64(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ._੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੤
		return this._0A64(_0A65, _੦, _੧);
	}

	private bool _੩(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected I4, but got Unknown
		int num = _0EFE._0F11(_੦);
		if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
		{
			int _੬ = Control.ModifierKeys | num;
			if (_੫((eShortcut)_੬))
			{
				return true;
			}
		}
		return false;
	}

	bool ሎ._੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੩
		return this._੩(_0A65, _੦, _੧);
	}

	private bool _੫(eShortcut _੬)
	{
		if (_੬ == _27C5 && Expandable)
		{
			_211E(!Expanded, eEventSource.Keyboard);
		}
		return false;
	}

	private bool _੮(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ሏ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੮
		return this._੮(_0A65, _੦, _੧);
	}

	private bool _੯(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ሐ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੯
		return this._੯(_0A65, _੦, _੧);
	}

	private bool _0A70(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected I4, but got Unknown
		if (!((Component)this).DesignMode)
		{
			int num = _0EFE._0F11(_੦);
			if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
			{
				int _੬ = Control.ModifierKeys | num;
				if (_੫((eShortcut)_੬))
				{
					return true;
				}
			}
		}
		return false;
	}

	bool ሎ._0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੰ
		return this._0A70(_0A65, _੦, _੧);
	}

	private bool ੲ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੲ
		return this.ੲ(_0A65, _੦, _੧);
	}

	private void _2818()
	{
		if (!_২ && !((Component)this).DesignMode && _27C5 != 0)
		{
			_1BE6._1BEA(this);
			_২ = true;
		}
	}

	private void _2819()
	{
		if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
	}
}
