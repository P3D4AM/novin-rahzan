using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[Designer("DevComponents.DotNetBar.Design.RadialMenuItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class RadialMenuItem : BaseItem
{
	private EventHandler _0E65;

	private Size _ᨀ = Size.Empty;

	private Size _ᢧ = Size.Empty;

	private GraphicsPath __248D;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ;

	private Image _ነ;

	private Image _ᶘ;

	private Rectangle __248E;

	private Rectangle __248F;

	private int __2490;

	private int __2491;

	private bool _ᣅ = true;

	private bool _ᆿ;

	private bool __2492 = true;

	private bool _ᚆ;

	private Point __2493 = Point.Empty;

	private EventHandler _2494;

	private EventHandler _2495;

	private int __2496;

	private Color _ኩ = Color.Empty;

	private Color _ኪ = Color.Empty;

	private Color _ካ = Color.Empty;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public GraphicsPath DisplayPath
	{
		get
		{
			return __248D;
		}
		internal set
		{
			if (value != __248D)
			{
				GraphicsPath oldValue = __248D;
				__248D = value;
				OnDisplayPathChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[DefaultValue("")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
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

	[Description("Indicates the size of the symbol in points.")]
	[DefaultValue(0f)]
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

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates image displayed on the item.")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (value != _ነ)
			{
				Image ነ = _ነ;
				_ነ = value;
				OnImageChanged(ነ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates disabled image displayed on the item when Enabled is set to false.")]
	[DefaultValue(null)]
	public Image DisabledImage
	{
		get
		{
			return _ᶘ;
		}
		set
		{
			if (value != _ᶘ)
			{
				Image ᶘ = _ᶘ;
				_ᶘ = value;
				OnDisabledImageChanged(ᶘ, value);
			}
		}
	}

	internal Rectangle _248B
	{
		get
		{
			return __248E;
		}
		set
		{
			__248E = value;
		}
	}

	internal Rectangle _248C
	{
		get
		{
			return __248F;
		}
		set
		{
			__248F = value;
		}
	}

	internal int ᨕ
	{
		get
		{
			return __2490;
		}
		set
		{
			__2490 = value;
		}
	}

	internal int ᨖ
	{
		get
		{
			return __2491;
		}
		set
		{
			__2491 = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether text on item is visible")]
	[DefaultValue(true)]
	public bool TextVisible
	{
		get
		{
			return _ᣅ;
		}
		set
		{
			if (value != _ᣅ)
			{
				bool ᣅ = _ᣅ;
				_ᣅ = value;
				OnTextVisibleChanged(ᣅ, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return _ᆿ;
		}
		internal set
		{
			_ᆿ = value;
			OnIsMouseOverChanged();
		}
	}

	[Description("Indicates whether item changes its appearance when mouse is over it or pressed")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool TracksMouse
	{
		get
		{
			return __2492;
		}
		set
		{
			if (value != __2492)
			{
				bool oldValue = __2492;
				__2492 = value;
				OnTracksMouseChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether item is in checked state.")]
	public bool Checked
	{
		get
		{
			return _ᚆ;
		}
		set
		{
			if (value != _ᚆ)
			{
				bool ᚆ = _ᚆ;
				_ᚆ = value;
				OnCheckedChanged(ᚆ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Specifies optional text offset.")]
	public Point TextOffset
	{
		get
		{
			return __2493;
		}
		set
		{
			if (value != __2493)
			{
				Point oldValue = __2493;
				__2493 = value;
				OnTextOffsetChanged(oldValue, value);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(false)]
	[Browsable(false)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			if (base.Expanded != value)
			{
				if (value)
				{
					OnBeforeMenuOpened(EventArgs.Empty);
				}
				else
				{
					OnBeforeMenuClosed(EventArgs.Empty);
				}
				base.Expanded = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override object ContainerControl
	{
		get
		{
			return GetContainerControl(checkIsContainer: false);
		}
		set
		{
			base.ContainerControl = value;
		}
	}

	[Category("Appearance")]
	[Description("Specifies explicit circular menu type diameter. Applies to circular menu type only.")]
	[DefaultValue(0)]
	public int CircularMenuDiameter
	{
		get
		{
			return __2496;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value != __2496)
			{
				int oldValue = __2496;
				__2496 = value;
				OnCircularMenuDiameterChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates background color of the circular menu item type.")]
	public Color CircularBackColor
	{
		get
		{
			return _ኩ;
		}
		set
		{
			_ኩ = value;
			Invalidate();
		}
	}

	[Description("Indicates text color of the circular menu item type.")]
	[Category("Appearance")]
	public Color CircularForeColor
	{
		get
		{
			return _ኪ;
		}
		set
		{
			_ኪ = value;
			Invalidate();
		}
	}

	[Description("Indicates border color of the circular menu item type.")]
	[Category("Appearance")]
	public Color CircularBorderColor
	{
		get
		{
			return _ካ;
		}
		set
		{
			_ካ = value;
			Invalidate();
		}
	}

	[Description("Occurs after Checked property has changed.")]
	public event EventHandler CheckedChanged
	{
		add
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before radial menu item is expanded and its sub-items displayed.")]
	public event EventHandler BeforeMenuOpened
	{
		add
		{
			EventHandler eventHandler = _2494;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2494, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2494;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2494, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before radial menu item is closed.")]
	public event EventHandler BeforeMenuClosed
	{
		add
		{
			EventHandler eventHandler = _2495;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2495, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2495;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2495, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (__248D != null)
		{
			__248D.Dispose();
			__248D = null;
		}
		base.Dispose(disposing);
	}

	protected virtual void OnCheckedChanged(EventArgs e)
	{
		_0E65?.Invoke(this, e);
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Expected O, but got Unknown
		Image val = ((Enabled || _ᶘ == null) ? _ነ : _ᶘ);
		if (string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(SymbolRealized) && val == null)
		{
			return;
		}
		if ((eRadialMenuType)p.ContextData2 == eRadialMenuType.Circular)
		{
			_2497(p);
			return;
		}
		Graphics graphics = p.Graphics;
		RadialMenuColorTable radialMenuColorTable = RadialMenuContainer.ጧ();
		RadialMenuColorTable radialMenuColorTable2 = (RadialMenuColorTable)p.ContextData;
		Color color = ColorScheme.GetColor(2840474);
		Color color2 = Color.FromArgb(72, color);
		if (!Enabled && (!radialMenuColorTable2.RadialMenuItemDisabledForeground.IsEmpty || (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuItemDisabledForeground.IsEmpty)))
		{
			if (!radialMenuColorTable2.RadialMenuItemDisabledForeground.IsEmpty)
			{
				color = radialMenuColorTable2.RadialMenuItemDisabledForeground;
			}
			else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuItemDisabledForeground.IsEmpty)
			{
				color = radialMenuColorTable.RadialMenuItemDisabledForeground;
			}
		}
		else if (_ᆿ && __2492 && (!radialMenuColorTable2.RadialMenuItemMouseOverForeground.IsEmpty || (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuItemMouseOverForeground.IsEmpty)))
		{
			if (!radialMenuColorTable2.RadialMenuItemMouseOverForeground.IsEmpty)
			{
				color = radialMenuColorTable2.RadialMenuItemMouseOverForeground;
			}
			else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuItemMouseOverForeground.IsEmpty)
			{
				color = radialMenuColorTable.RadialMenuItemMouseOverForeground;
			}
		}
		else if (!radialMenuColorTable2.RadialMenuItemForeground.IsEmpty)
		{
			color = radialMenuColorTable2.RadialMenuItemForeground;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuItemForeground.IsEmpty)
		{
			color = radialMenuColorTable.RadialMenuItemForeground;
		}
		if (!radialMenuColorTable2.RadialMenuItemMouseOverBackground.IsEmpty)
		{
			color2 = radialMenuColorTable2.RadialMenuItemMouseOverBackground;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.RadialMenuItemMouseOverBackground.IsEmpty)
		{
			color2 = radialMenuColorTable.RadialMenuItemMouseOverBackground;
		}
		Size size = Size.Empty;
		Size empty = Size.Empty;
		if (!string.IsNullOrEmpty(__0EA6))
		{
			size = _ᨀ;
		}
		else if (val != null)
		{
			size = val.Size;
			if (Parent is RadialMenuContainer)
			{
				RadialMenuContainer radialMenuContainer = (RadialMenuContainer)Parent;
				size.Width = (int)((float)size.Width * radialMenuContainer.ScaleFactor.Width);
				size.Height = (int)((float)size.Height * radialMenuContainer.ScaleFactor.Height);
			}
		}
		empty = size;
		if (!string.IsNullOrEmpty(Text) && _ᣅ)
		{
			empty.Width = Math.Max(empty.Width, _ᢧ.Width);
			empty.Height += _ᢧ.Height + 1;
		}
		empty.Width += 2;
		empty.Height += 2;
		if (_ᆿ && __2492)
		{
			SolidBrush val2 = new SolidBrush(color2);
			try
			{
				graphics.FillPath((Brush)(object)val2, __248D);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle rectangle = __248E;
		rectangle.Width -= empty.Width;
		rectangle.Height -= empty.Height;
		rectangle.Inflate(-2, -2);
		if (rectangle.Width < __248F.Width)
		{
			rectangle.Inflate((__248F.Width - rectangle.Width) / 2, 0);
		}
		if (rectangle.Height < __248F.Height)
		{
			rectangle.Inflate(0, (__248F.Height - rectangle.Height) / 2);
		}
		Point point = new Point(rectangle.X + (int)((double)(rectangle.Width / 2) * Math.Cos((double)(__2490 + __2491 / 2) * (Math.PI / 180.0))), rectangle.Y + (int)((double)(rectangle.Height / 2) * Math.Sin((double)(__2490 + __2491 / 2) * (Math.PI / 180.0))));
		point.Offset(rectangle.Width / 2, rectangle.Height / 2);
		if (!string.IsNullOrEmpty(__0EA6))
		{
			TextDrawing.DrawStringLegacy(graphics, __0EA6, Symbols.GetFont(SymbolSize, SymbolSet), color, new Rectangle(point.X + empty.Width / 2, point.Y + 2, 0, 0), eTextFormat.HorizontalCenter);
		}
		else if (val != null)
		{
			if (graphics.DpiX != 96f || graphics.DpiY != 96f)
			{
				float num = graphics.DpiX / 96f;
				float num2 = graphics.DpiY / 96f;
				graphics.DrawImage(val, (float)(point.X + (empty.Width - size.Width) / 2), (float)point.Y, (float)val.Width * num, (float)val.Height * num2);
			}
			else
			{
				graphics.DrawImage(val, point.X + (empty.Width - size.Width) / 2, point.Y, val.Width, val.Height);
			}
		}
		if (!string.IsNullOrEmpty(Text) && _ᣅ)
		{
			TextDrawing.DrawString(graphics, _2498(Text), p.Font, color, point.X + empty.Width / 2 + __2493.X, point.Y + size.Height + 3 + __2493.Y, eTextFormat.HorizontalCenter);
		}
		if (!_ᚆ)
		{
			return;
		}
		Pen val3 = new Pen(color, 2f);
		try
		{
			graphics.DrawArc(val3, __248E, (float)__2490, (float)__2491);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	protected override void OnClick()
	{
		base.OnClick();
		ExecuteCommand();
	}

	private void _2497(ItemPaintArgs _065C)
	{
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Expected O, but got Unknown
		Graphics graphics = _065C.Graphics;
		RadialMenuColorTable radialMenuColorTable = RadialMenuContainer.ጧ();
		RadialMenuColorTable radialMenuColorTable2 = (RadialMenuColorTable)_065C.ContextData;
		Color color = ColorScheme.GetColor(13913902);
		Color color2 = Color.White;
		Color color3 = Color.White;
		if (!_ኪ.IsEmpty)
		{
			color2 = _ኪ;
		}
		else if (!radialMenuColorTable2.CircularForeColor.IsEmpty)
		{
			color2 = radialMenuColorTable2.CircularForeColor;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.CircularForeColor.IsEmpty)
		{
			color2 = radialMenuColorTable.CircularForeColor;
		}
		if (!_ኩ.IsEmpty)
		{
			color = _ኩ;
		}
		else if (!radialMenuColorTable2.CircularBackColor.IsEmpty)
		{
			color = radialMenuColorTable2.CircularBackColor;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.CircularBackColor.IsEmpty)
		{
			color = radialMenuColorTable.CircularBackColor;
		}
		if (!_ካ.IsEmpty)
		{
			color3 = _ካ;
		}
		else if (!radialMenuColorTable2.CircularBorderColor.IsEmpty)
		{
			color3 = radialMenuColorTable2.CircularBorderColor;
		}
		else if (radialMenuColorTable != null && !radialMenuColorTable.CircularBorderColor.IsEmpty)
		{
			color3 = radialMenuColorTable.CircularBorderColor;
		}
		Color color4 = Color.FromArgb(192, color);
		if (__2492 && _ᆿ)
		{
			color = color4;
		}
		Size size = Size.Empty;
		if (!string.IsNullOrEmpty(__0EA6))
		{
			size = new Size(_ᨀ.Height, _ᨀ.Height);
		}
		else if (_ነ != null)
		{
			size = _ነ.Size;
		}
		if (size.Width > size.Height)
		{
			size.Height = size.Width;
		}
		else if (size.Height > size.Width)
		{
			size.Width = size.Height;
		}
		size.Width = (int)Math.Sqrt(Math.Pow(size.Width, 2.0) + Math.Pow(size.Height, 2.0));
		size.Height = size.Width;
		size.Width++;
		size.Height++;
		int num = Dpi.Width(__2496);
		if (num > 0)
		{
			size.Width = num;
			size.Height = num;
		}
		Rectangle rectangle = __248E;
		rectangle.Inflate(-1, -1);
		rectangle.Inflate(-size.Width, -size.Height);
		if (rectangle.Width < __248F.Width)
		{
			rectangle.Inflate((__248F.Width - rectangle.Width) / 2, 0);
		}
		if (rectangle.Height < __248F.Height)
		{
			rectangle.Inflate(0, (__248F.Height - rectangle.Height) / 2);
		}
		Point location = new Point(rectangle.X + (int)((double)(rectangle.Width / 2) * Math.Cos((double)(__2490 + __2491 / 2) * (Math.PI / 180.0))), rectangle.Y + (int)((double)(rectangle.Height / 2) * Math.Sin((double)(__2490 + __2491 / 2) * (Math.PI / 180.0))));
		location.Offset(rectangle.Width / 2, rectangle.Height / 2);
		Rectangle rectangle2 = new Rectangle(location, Size.Empty);
		rectangle2.Inflate(size);
		rectangle2.Inflate(-1, -1);
		if (color.A < byte.MaxValue)
		{
			graphics.FillEllipse(Brushes.White, rectangle2);
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			graphics.FillEllipse((Brush)(object)val, rectangle2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		rectangle2.Inflate(1, 1);
		Pen val2 = new Pen(color3, 3f);
		try
		{
			graphics.DrawEllipse(val2, rectangle2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (!string.IsNullOrEmpty(__0EA6))
		{
			TextDrawing.DrawStringLegacy(graphics, __0EA6, Symbols.GetFont(SymbolSize, SymbolSet), color2, new RectangleF(rectangle2.X + rectangle2.Width / 2, rectangle2.Y + rectangle2.Height / 2 + 2, 0f, 0f), eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter);
		}
		else if (_ነ != null)
		{
			ᶣ.ᶤ(graphics, _ነ, rectangle2);
		}
	}

	public override void RecalcSize()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		_ᨀ = Size.Empty;
		_ᢧ = Size.Empty;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || !BarFunctions.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = BarFunctions.CreateGraphics(val);
		if (val2 == null)
		{
			return;
		}
		try
		{
			if (!string.IsNullOrEmpty(__0EA6))
			{
				Font font = Symbols.GetFont(_ຨ, _ວ);
				_ᨀ = TextDrawing.MeasureStringLegacy(val2, __0EA6, Symbols.GetFont(SymbolSize, SymbolSet), Size.Empty, eTextFormat.Default);
				int num = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
				_ᨀ.Height -= num;
			}
			if (!string.IsNullOrEmpty(Text) && _ᣅ)
			{
				string text = _2498(Text);
				_ᢧ = TextDrawing.MeasureString(val2, text, val.Font, Size.Empty, eTextFormat.Default);
				_ᢧ.Width += 2;
			}
		}
		finally
		{
			val2.Dispose();
		}
		base.RecalcSize();
	}

	private string _2498(string _06EA)
	{
		if (_06EA.Contains("\\r\\n"))
		{
			_06EA = _06EA.Replace("\\r\\n", "\r\n");
		}
		return _06EA;
	}

	protected virtual void OnDisplayPathChanged(GraphicsPath oldValue, GraphicsPath newValue)
	{
		if (oldValue != null && oldValue != newValue)
		{
			oldValue.Dispose();
		}
	}

	public override BaseItem Copy()
	{
		RadialMenuItem radialMenuItem = new RadialMenuItem();
		CopyToItem(radialMenuItem);
		return radialMenuItem;
	}

	internal void _0EBA(RadialMenuItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		base.CopyToItem(copy);
		RadialMenuItem radialMenuItem = copy as RadialMenuItem;
		radialMenuItem.Checked = _ᚆ;
		radialMenuItem.Symbol = _ລ;
		radialMenuItem.SymbolSet = _ວ;
		radialMenuItem.SymbolSize = _ຨ;
		radialMenuItem.TextVisible = _ᣅ;
		radialMenuItem.TracksMouse = __2492;
		radialMenuItem.CircularBackColor = _ኩ;
		radialMenuItem.CircularBorderColor = _ካ;
		radialMenuItem.CircularForeColor = _ኪ;
		radialMenuItem.CircularMenuDiameter = __2496;
		radialMenuItem.Image = _ነ;
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
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnImageChanged(Image oldValue, Image newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnDisabledImageChanged(Image oldValue, Image newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnTextVisibleChanged(bool oldValue, bool newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (!_ᆿ)
		{
			IsMouseOver = true;
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseLeave()
	{
		if (_ᆿ)
		{
			IsMouseOver = false;
		}
		base.InternalMouseLeave();
	}

	protected virtual void OnIsMouseOverChanged()
	{
		Refresh();
	}

	protected virtual void OnTracksMouseChanged(bool oldValue, bool newValue)
	{
	}

	protected internal override void OnExpandChange()
	{
		NeedRecalcSize = true;
		if (!Expanded && base.HotSubItem != null)
		{
			base.HotSubItem.InternalMouseLeave();
		}
		base.OnExpandChange();
	}

	protected virtual void OnCheckedChanged(bool oldValue, bool newValue)
	{
		OnCheckedChanged(EventArgs.Empty);
		Refresh();
	}

	protected internal override void OnSubItemExpandChange(BaseItem item)
	{
		if (Parent != null)
		{
			Parent.OnSubItemExpandChange(item);
		}
		base.OnSubItemExpandChange(item);
	}

	protected virtual void OnTextOffsetChanged(Point oldValue, Point newValue)
	{
		Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextOffset()
	{
		return !__2493.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextOffset()
	{
		TextOffset = Point.Empty;
	}

	protected virtual void OnBeforeMenuOpened(EventArgs e)
	{
		_2494?.Invoke(this, e);
	}

	protected virtual void OnBeforeMenuClosed(EventArgs e)
	{
		_2495?.Invoke(this, e);
	}

	protected virtual void OnCircularMenuDiameterChanged(int oldValue, int newValue)
	{
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCircularBackColor()
	{
		return !_ኩ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCircularBackColor()
	{
		CircularBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCircularForeColor()
	{
		return !_ኪ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCircularForeColor()
	{
		CircularForeColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCircularBorderColor()
	{
		return !_ካ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCircularBorderColor()
	{
		CircularBorderColor = Color.Empty;
	}
}
