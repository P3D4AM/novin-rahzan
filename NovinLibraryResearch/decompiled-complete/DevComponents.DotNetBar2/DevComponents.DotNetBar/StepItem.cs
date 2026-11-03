using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[DefaultEvent("Click")]
[ToolboxItem(false)]
public class StepItem : BaseItem
{
	private const int ᬧ = 4;

	private Size _ᬫ = Size.Empty;

	private GraphicsPath __24A0;

	private bool __24A1;

	private int _ᗶ;

	private int _ᗵ = 100;

	private int _ᗸ;

	private Color __0EA4 = Color.Empty;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ = 13f;

	private Image _ነ;

	private bool __24A2;

	private Size _ᑀ = Size.Empty;

	private bool __0605 = true;

	private bool _ኗ;

	private bool __24A3;

	private int _ᒘ = 4;

	private Padding _ᬩ = new Padding(4);

	private Color[] __24A4;

	private Color[] __137D;

	private eButtonTextAlignment _ᬭ;

	private Color __0CD1 = Color.Empty;

	private bool __0EB9 = true;

	internal Size ᬦ => _ᬫ;

	[Browsable(false)]
	public GraphicsPath ItemPath
	{
		get
		{
			return __24A0;
		}
		internal set
		{
			if (__24A0 != null)
			{
				__24A0.Dispose();
			}
			__24A0 = value;
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Gets or sets the minimum value of the range of the control.")]
	public int Minimum
	{
		get
		{
			return _ᗶ;
		}
		set
		{
			if (value != _ᗶ)
			{
				int ᗶ = _ᗶ;
				_ᗶ = value;
				OnMinimumChanged(ᗶ, value);
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(100)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	public int Maximum
	{
		get
		{
			return _ᗵ;
		}
		set
		{
			if (value != _ᗵ)
			{
				int ᗵ = _ᗵ;
				_ᗵ = value;
				OnMaximumChanged(ᗵ, value);
			}
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[Description("Gets or sets the current position of the progress bar.")]
	[DefaultValue(0)]
	public int Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			if (value != _ᗸ)
			{
				int ᗸ = _ᗸ;
				_ᗸ = value;
				OnValueChanged(ᗸ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of the Symbol.")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			Refresh();
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[DefaultValue("")]
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

	[DefaultValue(12f)]
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

	[DefaultValue(null)]
	[Description("Indicates the image that is displayed next to the item text label.")]
	[Category("Appearance")]
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

	internal bool _249E
	{
		get
		{
			return __24A1;
		}
		set
		{
			__24A1 = value;
		}
	}

	internal bool _249F
	{
		get
		{
			return __24A2;
		}
		set
		{
			__24A2 = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates minimum size of the item")]
	public Size MinimumSize
	{
		get
		{
			return _ᑀ;
		}
		set
		{
			if (value != _ᑀ)
			{
				Size ᑀ = _ᑀ;
				_ᑀ = value;
				OnMinimumSizeChanged(ᑀ, value);
			}
		}
	}

	[Description("Specifies whether item changes its appearance when mouse is moved over the item")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool HotTracking
	{
		get
		{
			return __0605;
		}
		set
		{
			if (value != __0605)
			{
				bool oldValue = __0605;
				__0605 = value;
				OnHotTrackingChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return _ኗ;
		}
		internal set
		{
			_ኗ = value;
		}
	}

	[Browsable(false)]
	public bool IsMouseDown
	{
		get
		{
			return __24A3;
		}
		internal set
		{
			__24A3 = value;
		}
	}

	[Description("Indicates the spacing between image and text.")]
	[Category("Appearance")]
	[DefaultValue(4)]
	public int ImageTextSpacing
	{
		get
		{
			return _ᒘ;
		}
		set
		{
			if (value != _ᒘ)
			{
				int ᒘ = _ᒘ;
				_ᒘ = value;
				OnImageTextSpacingChanged(ᒘ, value);
			}
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Appearance")]
	[Description("Gets or sets padding around content of the item.")]
	public Padding Padding => _ᬩ;

	[Description("Indicates the array of colors that when set are used to draw the current progress, i.e. Value>Minimum")]
	[DefaultValue(null)]
	[TypeConverter(typeof(ArrayConverter))]
	[Category("Appearance")]
	public Color[] ProgressColors
	{
		get
		{
			return __24A4;
		}
		set
		{
			if (__24A4 != value)
			{
				__24A4 = value;
				Refresh();
			}
		}
	}

	[TypeConverter(typeof(ArrayConverter))]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates the array of colors that when set are used to draw the background of the item.")]
	public Color[] BackColors
	{
		get
		{
			return __137D;
		}
		set
		{
			if (__137D != value)
			{
				__137D = value;
				Refresh();
			}
		}
	}

	[Description("Indicates text alignment.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eButtonTextAlignment.Left)]
	public eButtonTextAlignment TextAlignment
	{
		get
		{
			return _ᬭ;
		}
		set
		{
			_ᬭ = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DefaultValue("")]
	[Description("The text contained in the item.")]
	[DevCoBrowsable(true)]
	[Localizable(true)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[Description("Indicates color of text.")]
	[Category("Columns")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			Refresh();
		}
	}

	protected override bool IsMarkupSupported => __0EB9;

	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	public StepItem()
		: this("", "")
	{
	}

	public StepItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public StepItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		MouseUpNotification = true;
		_ᬩ.PropertyChanged += _24A6;
	}

	public override BaseItem Copy()
	{
		StepItem stepItem = new StepItem(m_Name);
		CopyToItem(stepItem);
		return stepItem;
	}

	internal void _0EBA(StepItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		StepItem stepItem = copy as StepItem;
		stepItem.Symbol = _ລ;
		stepItem.SymbolSet = _ວ;
		stepItem.SymbolColor = __0EA4;
		stepItem.SymbolSize = _ຨ;
		base.CopyToItem(stepItem);
	}

	protected override void Dispose(bool disposing)
	{
		if (__24A0 != null)
		{
			__24A0.Dispose();
			__24A0 = null;
		}
		base.Dispose(disposing);
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			StepItemRendererEventArgs stepItemRendererEventArgs = new StepItemRendererEventArgs(this, p.Graphics);
			stepItemRendererEventArgs.က = p;
			ໝ.DrawStepItem(stepItemRendererEventArgs);
		}
		else
		{
			_24A7 _24A7 = ᣓ._1929(this);
			if (_24A7 != null)
			{
				StepItemRendererEventArgs stepItemRendererEventArgs2 = new StepItemRendererEventArgs(this, p.Graphics);
				stepItemRendererEventArgs2.က = p;
				_24A7.Paint(stepItemRendererEventArgs2);
			}
		}
		if (DesignMode && Focused)
		{
			Rectangle ल = Rectangle.Round(__24A0.GetBounds());
			ल.Inflate(-1, -1);
			_1A6B._1A6C(p.Graphics, ल, p.Colors.ItemDesignTimeBorder);
		}
		DrawInsertMarker(p.Graphics);
	}

	public override void RecalcSize()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Invalid comparison between Unknown and I4
		_໘(null);
		Size size = Size.Empty;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.Disposing || val.IsDisposed)
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
			if (!string.IsNullOrEmpty(_ລ))
			{
				_ᬫ = _᧗(val2);
				size = _ᬫ;
			}
			else if (_ነ != null)
			{
				size = _ነ.Size;
			}
			if (!string.IsNullOrEmpty(Text))
			{
				Size size2 = _20B1._20B2(this, val2, 0, val.Font, eTextFormat.Default, (int)val.RightToLeft == 1);
				size.Width += size2.Width;
				size.Height = Math.Max(size.Height, size2.Height);
				if (_ነ != null || !string.IsNullOrEmpty(_ລ))
				{
					size.Width += Dpi.Width(_ᒘ);
				}
			}
			else if (string.IsNullOrEmpty(_ລ) && _ነ == null)
			{
				size = new Size(Dpi.Width16, Dpi.Height16);
			}
			size.Width += _24A5();
			if (!__24A1 && !__24A2)
			{
				size.Width += Dpi.Width(_24A5());
			}
			size.Width += Dpi.Width(_ᬩ.Horizontal);
			size.Height += Dpi.Height(_ᬩ.Vertical);
			base.RecalcSize();
		}
		finally
		{
			val2.Dispose();
		}
		if (!_ᑀ.IsEmpty)
		{
			if (size.Width < _ᑀ.Width)
			{
				size.Width = _ᑀ.Width;
			}
			if (size.Height < _ᑀ.Height)
			{
				size.Height = _ᑀ.Height;
			}
		}
		m_Rect.Size = size;
	}

	internal int _24A5()
	{
		if (Parent is StepItemContainer)
		{
			return ((StepItemContainer)Parent).PointerSize;
		}
		return 10;
	}

	private Size _᧗(Graphics _٥)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Size empty = Size.Empty;
		if (_٥ == null || string.IsNullOrEmpty(_ລ))
		{
			return empty;
		}
		Font font = Symbols.GetFont(SymbolSize, SymbolSet);
		empty = TextDrawing.MeasureString(_٥, "\uf00a", font);
		int num = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
		empty.Height -= num;
		return empty;
	}

	private Font _໘(ItemPaintArgs _0BD2)
	{
		Font val = null;
		if (_0BD2 != null)
		{
			val = _0BD2.Font;
		}
		if (val == null)
		{
			Control val2 = null;
			if (_0BD2 != null)
			{
				val2 = _0BD2.ContainerControl;
			}
			if (val2 == null)
			{
				object containerControl = ContainerControl;
				val2 = (Control)((containerControl is Control) ? containerControl : null);
			}
			val = ((val2 == null || val2.Font == null) ? SystemFonts.DefaultFont : val2.Font);
		}
		return val;
	}

	protected virtual void OnMinimumChanged(int oldValue, int newValue)
	{
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnMaximumChanged(int oldValue, int newValue)
	{
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnValueChanged(int oldValue, int newValue)
	{
		OnAppearanceChanged();
		Refresh();
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

	protected virtual void OnMinimumSizeChanged(Size oldValue, Size newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMinimumSize()
	{
		return !_ᑀ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMinimumSize()
	{
		Size = Size.Empty;
	}

	protected virtual void OnHotTrackingChanged(bool oldValue, bool newValue)
	{
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!DesignMode)
		{
			_ኗ = true;
			if (GetEnabled() && __0605)
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (!DesignMode)
		{
			_ኗ = false;
			__24A3 = false;
			if (GetEnabled() && __0605)
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if ((int)objArg.Button == 1048576 && !DesignMode)
		{
			__24A3 = true;
			if (GetEnabled() && __0605)
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		if (__24A3 && !DesignMode)
		{
			__24A3 = false;
			if (GetEnabled() && __0605)
			{
				Refresh();
			}
		}
	}

	protected virtual void OnImageTextSpacingChanged(int oldValue, int newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePadding()
	{
		if (_ᬩ.Bottom == 4 && _ᬩ.Top == 4 && _ᬩ.Left == 4)
		{
			return _ᬩ.Right != 4;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPadding()
	{
		_ᬩ.All = 4;
	}

	private void _24A6(object _٠, PropertyChangedEventArgs ؿ)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !__0CD1.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}
}
