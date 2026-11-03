using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.LabelXDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[ComVisible(false)]
[ToolboxBitmap(typeof(LabelX), "Controls.LabelX.ico")]
public class LabelX : BaseItemControl, ICommandSource
{
	private LabelItem _2289;

	private bool _228A = true;

	private Size ᒏ = Size.Empty;

	private MarkupLinkClickEventHandler Ӟ;

	private ICommand _૦;

	private object _૧;

	[Category("Appearance")]
	[Description("Specifies spacing between label image and text.")]
	[DefaultValue(2)]
	public int ImageTextSpacing
	{
		get
		{
			return _2289.ImageTextSpacing;
		}
		set
		{
			_2289.ImageTextSpacing = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected string PlainText => _2289.PlainText;

	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool EnableMarkup
	{
		get
		{
			return _2289.EnableMarkup;
		}
		set
		{
			_2289.EnableMarkup = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether control displays focus cues when focused.")]
	[Category("Behavior")]
	public override bool FocusCuesEnabled
	{
		get
		{
			return base.FocusCuesEnabled;
		}
		set
		{
			base.FocusCuesEnabled = value;
		}
	}

	[Description("Specifies border sides that are displayed.")]
	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(eBorderSide.All)]
	public eBorderSide BorderSide
	{
		get
		{
			return _2289.BorderSide;
		}
		set
		{
			_2289.BorderSide = value;
			InvalidateAutoSize();
		}
	}

	[Description("Indicates the type of the border drawn around the label.")]
	[Browsable(false)]
	[DefaultValue(eBorderType.None)]
	[Category("Appearance")]
	public eBorderType BorderType
	{
		get
		{
			return _2289.BorderType;
		}
		set
		{
			_2289.BorderType = value;
			InvalidateAutoSize();
		}
	}

	[DefaultValue(null)]
	[Description("The image that will be displayed on the face of the item.")]
	[Category("Appearance")]
	[Browsable(true)]
	public Image Image
	{
		get
		{
			return _2289.Image;
		}
		set
		{
			_2289.Image = value;
			InvalidateAutoSize();
		}
	}

	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[DefaultValue(eImagePosition.Left)]
	[Browsable(true)]
	public eImagePosition ImagePosition
	{
		get
		{
			return _2289.ImagePosition;
		}
		set
		{
			_2289.ImagePosition = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[Description("Indicates border line color when border is single line.")]
	[Category("Appearance")]
	public Color SingleLineColor
	{
		get
		{
			return _2289.SingleLineColor;
		}
		set
		{
			_2289.SingleLineColor = value;
		}
	}

	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[Description("The text contained in the item.")]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Indicates text alignment.")]
	[Category("Layout")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	public StringAlignment TextAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _2289.TextAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_2289.TextAlignment = value;
			RecalcLayout();
		}
	}

	[Description("Indicates text line alignment.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Layout")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StringAlignment TextLineAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _2289.TextLineAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_2289.TextLineAlignment = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[DefaultValue(false)]
	[Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
	public bool WordWrap
	{
		get
		{
			return _2289.WordWrap;
		}
		set
		{
			_2289.WordWrap = value;
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool TabStop
	{
		get
		{
			return ((Control)this).TabStop;
		}
		set
		{
			((Control)this).TabStop = value;
		}
	}

	[Category("Layout")]
	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates left padding in pixels.")]
	public int PaddingLeft
	{
		get
		{
			return _2289.PaddingLeft;
		}
		set
		{
			_2289.PaddingLeft = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[Description("Indicates right padding in pixels.")]
	[Category("Layout")]
	[DefaultValue(0)]
	public int PaddingRight
	{
		get
		{
			return _2289.PaddingRight;
		}
		set
		{
			_2289.PaddingRight = value;
			InvalidateAutoSize();
		}
	}

	[Description("Indicates top padding in pixels.")]
	[Category("Layout")]
	[Browsable(true)]
	[DefaultValue(0)]
	public int PaddingTop
	{
		get
		{
			return _2289.PaddingTop;
		}
		set
		{
			_2289.PaddingTop = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates bottom padding in pixels.")]
	[Category("Layout")]
	public int PaddingBottom
	{
		get
		{
			return _2289.PaddingBottom;
		}
		set
		{
			_2289.PaddingBottom = value;
			InvalidateAutoSize();
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
	public bool UseMnemonic
	{
		get
		{
			return _228A;
		}
		set
		{
			_228A = value;
			InvalidateAutoSize();
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[Localizable(true)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	internal bool _2287
	{
		get
		{
			if (_2289 != null)
			{
				return _2289.TextMarkupBody != null;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public override bool AutoSize
	{
		get
		{
			return ((Control)this).AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				((Control)this).AutoSize = value;
				_21DF();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates text-orientation")]
	[DefaultValue(eOrientation.Horizontal)]
	public eOrientation TextOrientation
	{
		get
		{
			return _2289.TextOrientation;
		}
		set
		{
			_2289.TextOrientation = value;
			InvalidateAutoSize();
			_21DF();
		}
	}

	[Description("Indicates how vertical text is rotated when TextOrientation = Vertical.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool VerticalTextTopUp
	{
		get
		{
			return _2289.VerticalTextTopUp;
		}
		set
		{
			_2289.VerticalTextTopUp = value;
			if (TextOrientation == eOrientation.Vertical)
			{
				((Control)this).Invalidate();
			}
		}
	}

	internal LabelItem _2288 => _2289;

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the symbol displayed on face of the tile instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return _2289.Symbol;
		}
		set
		{
			_2289.Symbol = value;
		}
	}

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _2289.SymbolSet;
		}
		set
		{
			_2289.SymbolSet = value;
		}
	}

	[DefaultValue(0f)]
	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return _2289.SymbolSize;
		}
		set
		{
			_2289.SymbolSize = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of the symbol.")]
	public Color SymbolColor
	{
		get
		{
			return _2289.SymbolColor;
		}
		set
		{
			_2289.SymbolColor = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether label text is rendered using bold font unless the Font property is specifically set to different font.")]
	public bool FontBold
	{
		get
		{
			return _2289.FontBold;
		}
		set
		{
			if (_2289.FontBold != value)
			{
				_2289.FontBold = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	[DefaultValue(null)]
	public Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
		}
	}

	ICommand ICommandSource.ᒍ
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[Localizable(true)]
	[Category("Commands")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
		}
	}

	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	public LabelX()
	{
		_2289 = new LabelItem();
		_2289.Style = eDotNetBarStyle.Office2007;
		_2289.MarkupLinkClick += _228B;
		FocusCuesEnabled = false;
		HostItem = _2289;
		TabStop = false;
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (((Control)this).AutoSize)
		{
			_21DF();
		}
		RecalcLayout();
		((Control)this).OnHandleCreated(e);
	}

	protected override void RecalcSize()
	{
		_2289._19CA = true;
		_2289.Width = Dpi.DescaleWidth(_2289.Bounds.Width);
		_2289.Height = Dpi.DescaleHeight(_2289.Bounds.Height);
		_2289._19CA = false;
		base.RecalcSize();
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		_2289.BackColor = ((Control)this).BackColor;
		((Control)this).OnBackColorChanged(e);
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		if (((Control)this).ForeColor == SystemColors.ControlText)
		{
			_2289.ForeColor = Color.Empty;
		}
		else
		{
			_2289.ForeColor = ((Control)this).ForeColor;
		}
		((Control)this).OnForeColorChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSingleLineColor()
	{
		return _2289.ShouldSerializeSingleLineColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSingleLineColor()
	{
		_2289.ResetSingleLineColor();
	}

	private void _228B(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		OnMarkupLinkClick(ؿ);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	protected void InvalidateAutoSize()
	{
		ᒏ = Size.Empty;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_228A)
		{
			_2289.ᧄ = true;
		}
		else
		{
			_2289.ᧄ = false;
		}
		if (_2289.NeedRecalcSize)
		{
			_2289.RecalcSize();
		}
		base.OnPaint(e);
	}

	private bool _228C()
	{
		if (!((Control)this).Enabled || !((Control)this).Visible)
		{
			return false;
		}
		return true;
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (!UseMnemonic || !Control.IsMnemonic(charCode, ((Control)this).Text) || !_228C() || (int)Control.ModifierKeys != 262144 || _2289.IsUsingTextMarkup)
		{
			return false;
		}
		Control parent = ((Control)this).Parent;
		if (parent != null && parent.SelectNextControl((Control)(object)this, true, false, true, false) && !parent.ContainsFocus)
		{
			parent.Focus();
		}
		return true;
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!ᒏ.IsEmpty && (proposedSize.Width == ᒏ.Width || proposedSize.Width == 0 || ((Control)this).AutoSize))
		{
			return ᒏ;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this) || (((Component)(object)this).DesignMode && !((Control)this).AutoSize))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		if (((Control)this).Text.Length == 0)
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		int width = _2289.Width;
		int height = _2289.Height;
		_2289._19CA = true;
		_2289.Width = 0;
		_2289.Height = 0;
		if (((proposedSize.Width > 0 && proposedSize.Width < 500000) || ((Control)this).MaximumSize.Width > 0) && _2289.TextMarkupBody != null)
		{
			if (TextOrientation == eOrientation.Horizontal)
			{
				_2289._᧖((((Control)this).MaximumSize.Width > 0) ? ((Control)this).MaximumSize.Width : proposedSize.Width);
			}
			else
			{
				_2289._᧖((((Control)this).MaximumSize.Height > 0) ? ((Control)this).MaximumSize.Height : proposedSize.Height);
			}
		}
		else
		{
			_2289.RecalcSize();
			if (WordWrap && _2289.WidthInternal > ((Control)this).MaximumSize.Width && ((Control)this).MaximumSize.Width > 0)
			{
				_2289.Height = 0;
				_2289.Width = Dpi.DescaleWidth(((Control)this).MaximumSize.Width);
				_2289.RecalcSize();
			}
			else if (WordWrap && _2289.WidthInternal > proposedSize.Width && proposedSize.Width > 0)
			{
				_2289.Height = 0;
				_2289.Width = Dpi.DescaleWidth(proposedSize.Width);
				_2289.RecalcSize();
			}
		}
		Size size = _2289.Size;
		size.Height += 2;
		if (TextOrientation == eOrientation.Vertical)
		{
			size.Width += Dpi.Width2;
		}
		if (((Control)this).Font.Size > 13f)
		{
			size.Height += 2;
		}
		_2289.Width = width;
		_2289.Height = height;
		_2289._19CA = false;
		ᒏ = size;
		return ᒏ;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size empty = Size.Empty;
			if (width != ᒏ.Width && width > 0)
			{
				InvalidateAutoSize();
				empty = ((Control)this).GetPreferredSize(new Size(width, 0));
			}
			else
			{
				empty = ((Control)this).PreferredSize;
			}
			width = empty.Width;
			height = empty.Height;
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	private void _21DF()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		InvalidateAutoSize();
		base.OnFontChanged(e);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		InvalidateAutoSize();
		base.OnTextChanged(e);
		_21DF();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return _2289.ShouldSerializeSymbolColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		_2289.ResetSymbolColor();
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null)
		{
			CommandManager._૭(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
