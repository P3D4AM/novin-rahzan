using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.Helpers;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Controls;

public class DesktopAlertWindow : Form
{
	private class _2B87 : IComparer<Rectangle>
	{
		public int Compare(Rectangle x, Rectangle y)
		{
			return x.Y - y.Y;
		}
	}

	private class _2B88 : IComparer<Rectangle>
	{
		public int Compare(Rectangle x, Rectangle y)
		{
			return y.Y - x.Y;
		}
	}

	private bool _ὕ;

	private bool _ᜣ = true;

	private Rectangle __1F4E = Rectangle.Empty;

	private Rectangle _ὑ = Rectangle.Empty;

	private Rectangle __1378 = Rectangle.Empty;

	private Color __0EA4 = Color.Empty;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ = 22f;

	private Image _ነ;

	private long __2B74;

	private Action<long> __2B75;

	private bool _ݷ = true;

	private _0CB4 __1A56;

	private MarkupLinkClickEventHandler Ӟ;

	private static readonly Size _2B76 = new Size(360, 64);

	private Size __2B77 = _2B76;

	private int __2B69 = 200;

	private bool __2B78 = true;

	private int __2B6A = 6;

	private Timer __2279;

	private Control __2B79;

	private static Semaphore __2B7A = new Semaphore(1, 1);

	private static List<Rectangle> __2B7B = new List<Rectangle>();

	private static readonly _2B87 __2B7C = new _2B87();

	private static readonly _2B88 __2B7D = new _2B88();

	private bool __2B7E;

	private int __2B7F = 8;

	private eAlertPosition __2B67 = eAlertPosition.BottomRight;

	private bool __2B80 = true;

	private int __2B81 = 11;

	private bool __2B6B = true;

	private bool __137B = true;

	private eTextFormat ݽ => eTextFormat.EndEllipsis | eTextFormat.VerticalCenter | eTextFormat.WordBreak;

	[DefaultValue(true)]
	public override bool AutoSize
	{
		get
		{
			return _ᜣ;
		}
		set
		{
			_ᜣ = value;
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
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[DefaultValue("")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
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

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
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
	[Category("Appearance")]
	[DefaultValue(22f)]
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
	[Description("Indicates image displayed on alert.")]
	[Category("Appearance")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			_ነ = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DefaultValue(0)]
	public long AlertId
	{
		get
		{
			return __2B74;
		}
		set
		{
			__2B74 = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public Action<long> ClickAction
	{
		get
		{
			return __2B75;
		}
		set
		{
			__2B75 = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether text-markup can be used in Text property.")]
	public bool TextMarkupEnabled
	{
		get
		{
			return _ݷ;
		}
		set
		{
			if (value != _ݷ)
			{
				bool ݷ = _ݷ;
				_ݷ = value;
				OnTextMarkupEnabledChanged(ݷ, value);
			}
		}
	}

	internal _0CB4 _2B72 => __1A56;

	[Description("Indicates default alert size.")]
	[Category("Appearance")]
	public Size DefaultAlertSize
	{
		get
		{
			return __2B77;
		}
		set
		{
			if (value != __2B77)
			{
				Size ك = __2B77;
				__2B77 = value;
				_2B83(ك, value);
			}
		}
	}

	[DefaultValue(200)]
	[Browsable(true)]
	[Description("Gets or sets the total time in milliseconds alert animation takes.")]
	[Category("Behavior")]
	public int AlertAnimationDuration
	{
		get
		{
			return __2B69;
		}
		set
		{
			__2B69 = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether balloon will close automatically when user click the close button.")]
	[Category("Behavior")]
	public bool AutoClose
	{
		get
		{
			return __2B78;
		}
		set
		{
			__2B78 = value;
		}
	}

	[Description("Indicates time period in seconds after balloon closes automatically.")]
	[Category("Behavior")]
	[DefaultValue(6)]
	public int AutoCloseTimeOut
	{
		get
		{
			return __2B6A;
		}
		set
		{
			__2B6A = value;
			OnAutoCloseTimeOutChanged();
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public Control ReferenceControl
	{
		get
		{
			return __2B79;
		}
		set
		{
			__2B79 = value;
		}
	}

	protected override bool ShowWithoutActivation => true;

	[DefaultValue(8)]
	[Category("Behavior")]
	[Description("Indicates spacing between alerts on the screen")]
	public int AlertsSpacing
	{
		get
		{
			return __2B7F;
		}
		set
		{
			if (__2B7F != value)
			{
				int oldValue = __2B7F;
				__2B7F = value;
				OnAlertsSpacingChanged(value, oldValue);
			}
		}
	}

	[Description("Indicates the request screen position for the alert")]
	[Category("Behavior")]
	[DefaultValue(eAlertPosition.BottomRight)]
	public eAlertPosition AlertPosition
	{
		get
		{
			return __2B67;
		}
		set
		{
			if (__2B67 != value)
			{
				eAlertPosition oldValue = __2B67;
				__2B67 = value;
				OnAlertPositionChanged(value, oldValue);
			}
		}
	}

	[DefaultValue(true)]
	public bool TopMost
	{
		get
		{
			return __2B80;
		}
		set
		{
			if (__2B80 != value)
			{
				bool oldValue = __2B80;
				__2B80 = value;
				OnTopMostChanged(value, oldValue);
			}
		}
	}

	[Description("Indicates spacing in pixels between image and text")]
	[Category("Appearance")]
	[DefaultValue(11)]
	public int ImageTextPadding
	{
		get
		{
			return __2B81;
		}
		set
		{
			if (__2B81 != value)
			{
				int oldValue = __2B81;
				__2B81 = value;
				OnImageTextPaddingChanged(value, oldValue);
			}
		}
	}

	private bool _2B73 => true;

	[Category("Behavior")]
	[Description("Indicates whether alert plays exclamation sound when shown.")]
	[DefaultValue(true)]
	public bool PlaySound
	{
		get
		{
			return __2B6B;
		}
		set
		{
			if (__2B6B != value)
			{
				bool oldValue = __2B6B;
				__2B6B = value;
				OnPlaySoundChanged(value, oldValue);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether close button on alert is visible")]
	public bool CloseButtonVisible
	{
		get
		{
			return __137B;
		}
		set
		{
			if (__137B != value)
			{
				bool oldValue = __137B;
				__137B = value;
				OnCloseButtonVisibleChanged(value, oldValue);
			}
		}
	}

	[Description("Occurs when text markup link is clicked. Markup links can be created using a tag.")]
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

	public DesktopAlertWindow()
	{
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)73750, true);
		((Control)this).AccessibleRole = (AccessibleRole)8;
		((Form)this).ClientSize = _2B76;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(96f, 96f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)2;
		((Form)this).ControlBox = false;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Padding = new Padding(8);
		((Control)this).Font = new Font("Segoe UI", 10.125f);
	}

	protected override void Dispose(bool disposing)
	{
		_2286();
		((Form)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Invalid comparison between Unknown and I4
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Invalid comparison between Unknown and I4
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Color backColor = ((Control)this).BackColor;
		Color foreColor = ((Control)this).ForeColor;
		SolidBrush val = new SolidBrush(backColor);
		try
		{
			graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (__137B)
		{
			Font font = Symbols.GetFont(12f, eSymbolSet.Material);
			TextDrawing.DrawStringLegacy(graphics, "\ue14c", font, _ὕ ? _1A7B._᪃(foreColor) : foreColor, __1378, eTextFormat.NoClipping);
		}
		if (!string.IsNullOrEmpty(__0EA6))
		{
			Font font2 = Symbols.GetFont(_ຨ, _ວ);
			TextDrawing.DrawStringLegacy(graphics, __0EA6, font2, __0EA4.IsEmpty ? foreColor : __0EA4, __1F4E, eTextFormat.NoClipping | eTextFormat.VerticalCenter);
		}
		else if (_ነ != null)
		{
			graphics.DrawImage(_ነ, __1F4E);
		}
		Rectangle ὑ = _ὑ;
		int bottom = ὑ.Bottom;
		int bottom2 = ((Control)this).ClientRectangle.Bottom;
		Padding padding = ((Control)this).Padding;
		if (bottom > bottom2 - ((Padding)(ref padding)).Bottom)
		{
			int height = ὑ.Height;
			int bottom3 = ὑ.Bottom;
			int bottom4 = ((Control)this).ClientRectangle.Bottom;
			Padding padding2 = ((Control)this).Padding;
			ὑ.Height = height - (bottom3 - (bottom4 - ((Padding)(ref padding2)).Bottom));
		}
		eTextFormat eTextFormat = ݽ;
		if (__1A56 == null)
		{
			if ((int)((Control)this).RightToLeft == 1)
			{
				eTextFormat |= eTextFormat.RightToLeft;
			}
			if (_2B73)
			{
				TextRenderer.DrawText((IDeviceContext)(object)graphics, ((Control)this).Text, ((Control)this).Font, ὑ, foreColor, backColor, TextDrawing.GetTextFormatFlags(eTextFormat));
			}
			else
			{
				TextDrawing.DrawString(graphics, ((Control)this).Text, ((Control)this).Font, foreColor, ὑ, eTextFormat);
			}
		}
		else
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, ((Control)this).Font, foreColor, (int)((Control)this).RightToLeft == 1, ὑ, hotKeyPrefixVisible: true);
			Size size = __1A56.Bounds.Size;
			if (!size.IsEmpty && (eTextFormat & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter && ὑ.Height > size.Height)
			{
				ὑ.Y += (ὑ.Height - size.Height) / 2;
				ὑ.Height = size.Height;
			}
			__1A56._0CC6(ὑ, markupDrawContext);
			__1A56.Render(markupDrawContext);
		}
		((Form)this).OnPaint(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		if (__2B78 && __2279 != null)
		{
			__2279.Enabled = false;
		}
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (__1A56 != null)
		{
			__1A56.ભ((Control)(object)this);
		}
		if (__2B78 && __2279 != null)
		{
			__2279.Enabled = true;
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (__137B && __1378.Contains(e.Location))
		{
			_ὕ = true;
			((Control)this).Invalidate(__1378);
		}
		else if (_ὕ)
		{
			_ὕ = false;
			((Control)this).Invalidate(__1378);
		}
		if (__1A56 != null)
		{
			__1A56.મ((Control)(object)this, e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (__1A56 != null)
		{
			__1A56.ग़((Control)(object)this, e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (__1A56 != null)
		{
			__1A56.ફ((Control)(object)this, e);
		}
		if (_ὕ && (int)e.Button == 1048576)
		{
			HideAlert(eAlertClosureSource.CloseButton);
		}
		((Control)this).OnMouseUp(e);
	}

	public void PerformAutoSize()
	{
		Size ߒ = Dpi.Size(__2B77);
		if (!((Control)this).MinimumSize.IsEmpty)
		{
			ߒ = Dpi.Size(((Control)this).MinimumSize);
		}
		if (!((Control)this).IsHandleCreated)
		{
			((Control)this).CreateHandle();
		}
		ߒ = _2B82(ߒ);
		Size maximumSize = ((Control)this).MaximumSize;
		if (!maximumSize.IsEmpty)
		{
			if (maximumSize.Width > 0 && ߒ.Width > maximumSize.Width)
			{
				ߒ.Width = maximumSize.Width;
			}
			if (maximumSize.Height > 0 && ߒ.Height > maximumSize.Height)
			{
				ߒ.Height = maximumSize.Height;
			}
			ߒ = _2B82(ߒ);
		}
		((Form)this).Size = ߒ;
	}

	private Size _2B82(Size ߒ)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		Rectangle ὑ = new Rectangle(Point.Empty, ߒ);
		int x = ὑ.X;
		Padding padding = ((Control)this).Padding;
		ὑ.X = x + Dpi.Width(((Padding)(ref padding)).Left);
		int width = ὑ.Width;
		Padding padding2 = ((Control)this).Padding;
		ὑ.Width = width - Dpi.Width(((Padding)(ref padding2)).Horizontal);
		int y = ὑ.Y;
		Padding padding3 = ((Control)this).Padding;
		ὑ.Y = y + Dpi.Height(((Padding)(ref padding3)).Top);
		int height = ὑ.Height;
		Padding padding4 = ((Control)this).Padding;
		ὑ.Height = height - Dpi.Height(((Padding)(ref padding4)).Vertical);
		if (!string.IsNullOrEmpty(((Control)this).Text) || !string.IsNullOrEmpty(__0EA6) || _ነ != null)
		{
			Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
			try
			{
				if (__137B)
				{
					Font font = Symbols.GetFont(12f, eSymbolSet.Material);
					Size size = TextDrawing.MeasureString(val, "\ue14c", font);
					__1378 = new Rectangle(ὑ.Right - size.Width - Dpi.Width4, ὑ.Y + Dpi.Height4, size.Width, size.Height);
					ὑ.Width -= size.Width + Dpi.Width8;
				}
				__1F4E = Rectangle.Empty;
				if (!string.IsNullOrEmpty(__0EA6))
				{
					Size size2 = _᧗(val);
					size2.Width += Dpi.Width2;
					__1F4E = new Rectangle(ὑ.X, ὑ.Y, size2.Width, size2.Height);
					ὑ.X += size2.Width + __2B81;
					ὑ.Width -= size2.Width + __2B81;
					if (size2.Height > ὑ.Height)
					{
						ߒ.Height += size2.Height - ὑ.Height;
						ὑ.Height = size2.Height;
					}
					else
					{
						__1F4E.Height = ὑ.Height;
					}
				}
				else if (_ነ != null)
				{
					__1F4E = new Rectangle(ὑ.X, ὑ.Y, _ነ.Width, _ነ.Height);
					ὑ.X += _ነ.Width + __2B81;
					ὑ.Width -= _ነ.Width + __2B81;
					if (_ነ.Height > ὑ.Height)
					{
						ߒ.Height += _ነ.Height - ὑ.Height;
						ὑ.Height = _ነ.Height;
					}
				}
				_ὑ = Rectangle.Empty;
				if (__1A56 != null)
				{
					MarkupDrawContext markupDrawContext = ᛟ(val);
					__1A56.Measure(ὑ.Size, markupDrawContext);
					Size size3 = __1A56.Bounds.Size;
					__1A56._0CC6(new Rectangle(ὑ.Location, size3), markupDrawContext);
					if (size3.Width > ὑ.Width)
					{
						ߒ.Width += size3.Width - ὑ.Width;
						ὑ.Width = ߒ.Width;
					}
					if (size3.Height > ὑ.Height)
					{
						ߒ.Height += size3.Height - ὑ.Height;
						ὑ.Height = ߒ.Height;
					}
					_ὑ = ὑ;
				}
				else if (!string.IsNullOrEmpty(((Control)this).Text))
				{
					Size size4 = (_2B73 ? TextRenderer.MeasureText((IDeviceContext)(object)val, ((Control)this).Text, ((Control)this).Font, ὑ.Size, TextDrawing.GetTextFormatFlags(ݽ)) : TextDrawing.MeasureString(val, ((Control)this).Text, ((Control)this).Font, ߒ, ݽ));
					if (size4.Width > ὑ.Width)
					{
						ߒ.Width += size4.Width - ὑ.Width;
						ὑ.Width = ߒ.Width;
					}
					if (size4.Height > ὑ.Height)
					{
						ߒ.Height += size4.Height - ὑ.Height;
						ὑ.Height = ߒ.Height;
					}
					_ὑ = ὑ;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return ߒ;
	}

	private MarkupDrawContext ᛟ(Graphics _٥)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		return new MarkupDrawContext(_٥, ((Control)this).Font, ((Control)this).ForeColor, (int)((Control)this).RightToLeft == 1);
	}

	private Size _᧗(Graphics _٥)
	{
		Size empty = Size.Empty;
		if (_٥ == null || string.IsNullOrEmpty(_ລ))
		{
			return empty;
		}
		Font font = Symbols.GetFont(SymbolSize, SymbolSet);
		return TextDrawing.MeasureString(_٥, "\uf00a", font);
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

	protected override void OnTextChanged(EventArgs e)
	{
		ᛝ();
		((Form)this).OnTextChanged(e);
	}

	protected override void OnClick(EventArgs e)
	{
		if (__1A56 != null && __1A56.ಷ is _0CFE)
		{
			__1A56.દ((Control)(object)this);
			((Control)this).OnClick(e);
			return;
		}
		if (_ὕ)
		{
			((Control)this).OnClick(e);
			return;
		}
		if (__2B75 != null)
		{
			__2B75(__2B74);
			HideAlert(eAlertClosureSource.AlertClicked);
		}
		((Control)this).OnClick(e);
	}

	protected virtual void OnTextMarkupEnabledChanged(bool oldValue, bool newValue)
	{
		((Control)this).Invalidate();
	}

	private void ᛝ()
	{
		if (__1A56 != null)
		{
			__1A56.ವ -= TextMarkupLinkClick;
		}
		__1A56 = null;
		if (!_ݷ)
		{
			return;
		}
		string _06EA = ((Control)this).Text;
		if (ሒ.መ(ref _06EA))
		{
			__1A56 = ሒ.ሕ(_06EA);
			if (__1A56 != null)
			{
				__1A56.ವ += TextMarkupLinkClick;
			}
		}
	}

	protected virtual void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		Ӟ?.Invoke(this, e);
	}

	internal static Size _2036(DesktopAlertWindow _06EC, Graphics _٥, int ᜨ, Font _0EE7, eTextFormat _20B3, bool _20B4)
	{
		if (((Control)_06EC).Text == "" && _06EC._2B72 == null)
		{
			return Size.Empty;
		}
		Size empty = Size.Empty;
		if (_06EC._2B72 == null)
		{
			return TextDrawing.MeasureString(_٥, _192F.ಱ(((Control)_06EC).Text), _0EE7, ᜨ, _20B3);
		}
		Size availableSize = new Size(ᜨ, 1);
		if (ᜨ == 0)
		{
			availableSize.Width = 1600;
		}
		MarkupDrawContext markupDrawContext = new MarkupDrawContext(_٥, _0EE7, Color.Empty, rightToLeft: false);
		_06EC._2B72.Measure(availableSize, markupDrawContext);
		availableSize = _06EC._2B72.Bounds.Size;
		if (ᜨ != 0)
		{
			availableSize.Width = ᜨ;
		}
		markupDrawContext.RightToLeft = _20B4;
		_06EC._2B72._0CC6(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
		return _06EC._2B72.Bounds.Size;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDefaultAlertSize()
	{
		int width = __2B77.Width;
		Size size = _2B76;
		if (width == size.Width)
		{
			int height = __2B77.Height;
			Size size2 = _2B76;
			return height != size2.Height;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefaultAlertSize()
	{
		DefaultAlertSize = _2B76;
	}

	private void _2B83(Size ك, Size ل)
	{
		if (_ᜣ && ((Control)this).IsHandleCreated)
		{
			PerformAutoSize();
		}
	}

	protected void OnAutoCloseTimeOutChanged()
	{
		if (__2B6A > 0 && !((Component)this).DesignMode)
		{
			_2B84();
		}
		else
		{
			_2286();
		}
	}

	private void _2B84()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (__2279 == null)
		{
			__2279 = new Timer();
			__2279.Enabled = false;
			__2279.Tick += AutoCloseTimeOutEllapsed;
		}
		__2279.Interval = __2B6A * 1000;
		if (((Control)this).Visible)
		{
			__2279.Enabled = true;
			__2279.Start();
		}
	}

	protected virtual void AutoCloseTimeOutEllapsed(object sender, EventArgs e)
	{
		if (!((Control)this).IsDisposed)
		{
			_2286();
			HideAlert(eAlertClosureSource.Timeout);
			((Form)this).Close();
		}
	}

	private void _2286()
	{
		if (__2279 != null)
		{
			__2279.Enabled = false;
			__2279.Tick -= AutoCloseTimeOutEllapsed;
			((Component)(object)__2279).Dispose();
			__2279 = null;
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Form)this).OnVisibleChanged(e);
		if (((Control)this).Visible)
		{
			if (__2B6A > 0)
			{
				_2B84();
			}
			if (__2279 != null && !__2279.Enabled)
			{
				__2279.Enabled = true;
				__2279.Start();
			}
		}
		else if (__2279 != null && __2279.Enabled)
		{
			__2279.Stop();
			__2279.Enabled = false;
		}
	}

	private Rectangle _2B85()
	{
		Rectangle rectangle = ((Control)this).Bounds;
		ঊ ঊ = null;
		ঊ = ((__2B79 != null) ? BarFunctions.ॻ(__2B79) : BarFunctions._0944);
		if (__2B67 == eAlertPosition.BottomRight)
		{
			rectangle = new Rectangle(ঊ.ঌ.Right - ((Control)this).Width, ঊ.ঌ.Bottom - ((Control)this).Height - Dpi.Height12, ((Control)this).Width, ((Control)this).Height);
		}
		else if (__2B67 == eAlertPosition.BottomLeft)
		{
			rectangle = new Rectangle(ঊ.ঌ.Left, ঊ.ঌ.Bottom - ((Control)this).Height - Dpi.Height12, ((Control)this).Width, ((Control)this).Height);
		}
		else if (__2B67 == eAlertPosition.TopRight)
		{
			rectangle = new Rectangle(ঊ.ঌ.Right - ((Control)this).Width, ঊ.ঌ.Top + Dpi.Height12, ((Control)this).Width, ((Control)this).Height);
		}
		else if (__2B67 == eAlertPosition.TopLeft)
		{
			rectangle = new Rectangle(ঊ.ঌ.Left, ঊ.ঌ.Top + Dpi.Height12, ((Control)this).Width, ((Control)this).Height);
		}
		if (__2B7B.Count > 0)
		{
			if (__2B67 == eAlertPosition.TopLeft || __2B67 == eAlertPosition.TopRight)
			{
				__2B7B.Sort(__2B7C);
			}
			else
			{
				__2B7B.Sort(__2B7D);
			}
			Rectangle rectangle2 = rectangle;
			for (int i = 0; i < __2B7B.Count && __2B7B[i].IntersectsWith(rectangle); i++)
			{
				if (__2B67 == eAlertPosition.BottomLeft || __2B67 == eAlertPosition.BottomRight)
				{
					rectangle.Y -= __2B7B[i].Height + __2B7F;
				}
				else
				{
					rectangle.Y += __2B7B[i].Height + __2B7F;
				}
				if (!ঊ.ঌ.Contains(rectangle))
				{
					rectangle = rectangle2;
					break;
				}
				if (i == __2B7B.Count - 1)
				{
					break;
				}
			}
		}
		__2B7A.WaitOne();
		try
		{
			__2B7B.Add(rectangle);
			return rectangle;
		}
		finally
		{
			__2B7A.Release();
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		__2B7A.WaitOne();
		try
		{
			for (int num = __2B7B.Count - 1; num >= 0; num--)
			{
				if (__2B7B[num] == ((Control)this).Bounds)
				{
					__2B7B.RemoveAt(num);
					break;
				}
			}
		}
		finally
		{
			__2B7A.Release();
		}
		((Form)this).OnClosed(e);
	}

	public void Show(bool focusAlert)
	{
		PerformAutoSize();
		((Control)this).Bounds = _2B85();
		_ = ((Control)this).Bounds;
		DesktopAlert._2B70(this, EventArgs.Empty);
		if (((Control)this).IsDisposed)
		{
			return;
		}
		if (_2447())
		{
			try
			{
				__2B7E = true;
				switch (_2B86())
				{
				case eAlertAnimation.RightToLeft:
					_089F.AnimateWindow(((Control)this).Handle, __2B69, 262146);
					break;
				case eAlertAnimation.LeftToRight:
					_089F.AnimateWindow(((Control)this).Handle, __2B69, 262145);
					break;
				case eAlertAnimation.BottomToTop:
					_089F.AnimateWindow(((Control)this).Handle, __2B69, 262152);
					break;
				case eAlertAnimation.TopToBottom:
					_089F.AnimateWindow(((Control)this).Handle, __2B69, 262148);
					break;
				}
			}
			finally
			{
				__2B7E = false;
			}
		}
		((Control)this).Show();
		if (__2B80)
		{
			_089F.SetWindowPos(((Control)this).Handle, new IntPtr(-1), 0, 0, 0, 0, 19);
		}
		if (__2B6B)
		{
			SystemSounds.Beep.Play();
		}
	}

	private eAlertAnimation _2B86()
	{
		if (__2B67 == eAlertPosition.TopLeft || __2B67 == eAlertPosition.BottomLeft)
		{
			return eAlertAnimation.LeftToRight;
		}
		return eAlertAnimation.RightToLeft;
	}

	public void Show()
	{
		Show(focusAlert: false);
	}

	protected virtual void HideAlert(eAlertClosureSource source)
	{
		_2286();
		if (_2447())
		{
			_ = ((Control)this).Bounds;
			try
			{
				__2B7E = true;
				switch (_2B86())
				{
				case eAlertAnimation.RightToLeft:
					_089F.AnimateWindow(((Control)this).Handle, __2B69, 327681);
					break;
				case eAlertAnimation.LeftToRight:
					_089F.AnimateWindow(((Control)this).Handle, __2B69, 327682);
					break;
				case eAlertAnimation.BottomToTop:
					_089F.AnimateWindow(((Control)this).Handle, __2B69, 327684);
					break;
				case eAlertAnimation.TopToBottom:
					_089F.AnimateWindow(((Control)this).Handle, __2B69, 327688);
					break;
				}
			}
			finally
			{
				__2B7E = false;
			}
		}
		else
		{
			((Control)this).Hide();
		}
		((Form)this).Close();
		DesktopAlert._2B71(this, new AlertClosedEventArgs(source));
		__2B75 = null;
		((Component)this).Dispose();
	}

	public void Hide()
	{
		HideAlert(eAlertClosureSource.Timeout);
	}

	private bool _2447()
	{
		if (__2B69 > 0 && !((Component)this).DesignMode && !((Control)this).IsDisposed)
		{
			return true;
		}
		return false;
	}

	protected virtual void OnAlertsSpacingChanged(int newValue, int oldValue)
	{
		throw new NotImplementedException();
	}

	protected virtual void OnAlertPositionChanged(eAlertPosition newValue, eAlertPosition oldValue)
	{
	}

	protected virtual void OnTopMostChanged(bool newValue, bool oldValue)
	{
		if (((Control)this).IsHandleCreated && ((Control)this).Visible)
		{
			_089F.SetWindowPos(((Control)this).Handle, new IntPtr(newValue ? (-1) : (-2)), 0, 0, 0, 0, 19);
		}
	}

	protected virtual void OnImageTextPaddingChanged(int newValue, int oldValue)
	{
	}

	protected virtual void OnPlaySoundChanged(bool newValue, bool oldValue)
	{
	}

	protected virtual void OnCloseButtonVisibleChanged(bool newValue, bool oldValue)
	{
	}
}
