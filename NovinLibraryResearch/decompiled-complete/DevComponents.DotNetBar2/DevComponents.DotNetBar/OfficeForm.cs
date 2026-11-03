using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class OfficeForm : RibbonForm
{
	private int ᗀ = 2;

	private int ன = 5;

	private GenericItemContainer Ẻ;

	private ColorScheme լ;

	private LabelItem ẻ;

	private SystemCaptionItem Ẽ;

	private SystemCaptionItem _0B82;

	private Timer ẽ;

	private Font ൾ;

	private string _0D81 = "";

	private MarkupLinkClickEventHandler _0D83;

	private bool _Ế;

	private bool _ế;

	private BufferedBitmap Ề;

	private Point ề = Point.Empty;

	private Timer _Ể;

	private DateTime _ể = DateTime.MinValue;

	private bool _Ễ;

	private bool _Ꮄ;

	private Timer ễ;

	private Point _Ệ = Point.Empty;

	private bool _ệ = true;

	private _20EF _Ỉ;

	private bool ե = true;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	[Category("Appearance")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Description("Indicates text displayed in form caption instead of the Form.Text property.")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Browsable(true)]
	public string TitleText
	{
		get
		{
			return _0D81;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_0D81 = value;
			if (value == "")
			{
				ẻ.Text = ((Control)this).Text;
			}
			else
			{
				ẻ.Text = value;
			}
			if (((Control)this).IsHandleCreated)
			{
				_0BE5();
			}
			InvalidateNonClient(invalidateForm: false);
		}
	}

	[DefaultValue(null)]
	[Category("Caption")]
	[Description("Indicates font for the form caption text when CaptionVisible=true.")]
	[Browsable(true)]
	public Font CaptionFont
	{
		get
		{
			return ൾ;
		}
		set
		{
			ൾ = value;
			Ị();
		}
	}

	protected override bool EnableGlassExtend => false;

	protected virtual bool UseCornerSize => true;

	private bool ẹ
	{
		get
		{
			return _Ễ;
		}
		set
		{
			if (_Ễ != value)
			{
				_Ễ = value;
				if (!_Ễ)
				{
					Ố();
					Ẻ.InternalMouseLeave();
				}
				else
				{
					Ỏ();
				}
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	public bool HideMdiSystemMenu
	{
		get
		{
			return _ệ;
		}
		set
		{
			_ệ = value;
		}
	}

	[Browsable(true)]
	public Icon Icon
	{
		get
		{
			return ((Form)this).Icon;
		}
		set
		{
			((Form)this).Icon = value;
			Ẽ.Icon = value;
			Ợ();
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	public override bool AutoScroll
	{
		get
		{
			return ((Form)this).AutoScroll;
		}
		set
		{
			((Form)this).AutoScroll = value;
		}
	}

	protected internal bool CloseEnabled
	{
		get
		{
			return _0B82.CloseEnabled;
		}
		set
		{
			if (_0B82.CloseEnabled != value)
			{
				_0B82.CloseEnabled = value;
				OnCloseEnabledChanged();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates the Office 2007 Renderer global Color Table.")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eOffice2007ColorScheme Office2007ColorTable
	{
		get
		{
			if (GetRenderer() is Office2007Renderer office2007Renderer)
			{
				return office2007Renderer.ColorTable.InitialColorScheme;
			}
			return eOffice2007ColorScheme.Blue;
		}
		set
		{
			RibbonPredefinedColorSchemes.ChangeOffice2007ColorTable(value);
		}
	}

	protected override bool PaintClientBorder => false;

	protected override bool PaintRibbonCaption => false;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting form caption.")]
	[Browsable(true)]
	public bool CaptionAntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			if (ե != value)
			{
				ե = value;
			}
		}
	}

	[DefaultValue(eRenderMode.Global)]
	[Browsable(false)]
	public eRenderMode RenderMode
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
				((Control)this).Invalidate(true);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public BaseRenderer Renderer
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

	[Localizable(true)]
	[Description("Indicates tooltip for the form system icon.")]
	[DefaultValue("")]
	[Browsable(true)]
	public string IconTooltip
	{
		get
		{
			return Ẽ.Tooltip;
		}
		set
		{
			Ẽ.Tooltip = value;
		}
	}

	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	public OfficeForm()
	{
		_ = GlobalManager.Renderer;
		ᗀ = SystemInformation.Border3DSize.Width + _089F.ࢠ;
		((ScrollableControl)this).DockPadding.All = 0;
		Ẻ = new GenericItemContainer();
		Ẻ.GlobalItem = false;
		Ẻ.ContainerControl = this;
		Ẻ.WrapItems = false;
		Ẻ._0DBE = false;
		Ẻ._0DBF = false;
		Ẻ.Stretch = true;
		Ẻ.Displayed = true;
		Ẻ.SystemContainer = true;
		Ẻ.PaddingTop = 0;
		Ẻ.PaddingBottom = 0;
		Ẻ.PaddingLeft = 2;
		Ẻ.ItemSpacing = 1;
		Ẻ.SetOwner(this);
		Ẻ.Style = eDotNetBarStyle.StyleManagerControlled;
		Ẻ.ව = eContainerVerticalAlignment.Top;
		Ẽ = new SystemCaptionItem();
		Ẽ.GlobalItem = false;
		Ẽ.Enabled = false;
		Ẻ.SubItems.Add(Ẽ);
		Ẽ.IsSystemIcon = true;
		Ẽ.Icon = Icon;
		ẻ = CreateTitleLabel();
		Ẻ.SubItems.Add(ẻ);
		CreateAdditionalCaptionItems(Ẻ);
		_0B82 = new SystemCaptionItem();
		_0B82.GlobalItem = false;
		_0B82.Click += Ổ;
		_0B82.MouseEnter += ổ;
		Ẻ.SubItems.Add(_0B82);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			ộ();
			Ố();
			_0B82.Click -= Ổ;
			_0B82.MouseEnter -= ổ;
			ẻ.MarkupLinkClick -= ỉ;
			Ẻ.Dispose();
		}
		base.Dispose(disposing);
	}

	protected virtual void CreateAdditionalCaptionItems(GenericItemContainer captionContainer)
	{
	}

	protected virtual LabelItem CreateTitleLabel()
	{
		LabelItem labelItem = new LabelItem();
		labelItem.GlobalItem = false;
		labelItem.Font = SystemFonts.CaptionFont;
		labelItem.Stretch = true;
		labelItem.TextLineAlignment = (StringAlignment)1;
		labelItem.Text = ((Control)this).Text;
		labelItem.PaddingLeft = 3;
		labelItem.PaddingRight = 3;
		labelItem.MarkupLinkClick += ỉ;
		return labelItem;
	}

	private void ỉ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		OnTitleTextMarkupLinkClick(ؿ);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		base.StyleManagerStyleChanged(newStyle);
		_0BE5();
		InvalidateNonClient(invalidateForm: true);
	}

	protected override bool SetClientSizeCoreCustom(int x, int y)
	{
		base.ứ = true;
		AdjustBounds(ref x, ref y);
		return true;
	}

	protected virtual void OnTitleTextMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		_0D83?.Invoke(this, e);
	}

	protected override bool IsCustomFormStyleEnabled()
	{
		if (base.IsCustomFormStyleEnabled())
		{
			if (_0EFE.ষ && EnableGlass && !((Component)(object)this).DesignMode)
			{
				return ((Form)this).IsMdiChild;
			}
			return true;
		}
		return false;
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (EnableGlass && _0EFE.ষ)
		{
			if (((Control)this).IsHandleCreated)
			{
				UpdateFormGlassState();
			}
			else if (_Ế)
			{
				_ế = true;
			}
		}
		_Ế = ((Form)this).IsMdiChild;
	}

	private void Ị()
	{
		if (ẻ != null)
		{
			if (ൾ != null)
			{
				ẻ.Font = ൾ;
			}
			else
			{
				ẻ.Font = SystemFonts.CaptionFont;
			}
			((Control)this).Invalidate(Ẻ.DisplayRectangle);
		}
		InvalidateNonClient(invalidateForm: false);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Form)this).OnFontChanged(e);
		InvalidateNonClient(invalidateForm: false);
	}

	protected override bool WindowsMessageSetIcon(ref Message m)
	{
		Ợ();
		return base.WindowsMessageSetIcon(ref m);
	}

	public void InvalidateNonClient(bool invalidateForm)
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			_089F.RECT lprcUpdate = new _089F.RECT(0, 0, ((Control)this).Width, Ẻ.HeightInternal);
			if (invalidateForm)
			{
				lprcUpdate = new _089F.RECT(0, 0, ((Control)this).Width, ((Control)this).Height);
			}
			_089F.RedrawWindow(((Control)this).Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}

	protected override Rectangle ReduceDisplayRectangle(Rectangle r)
	{
		return r;
	}

	protected override void AdjustBounds(ref int width, ref int height, BoundsSpecified specified)
	{
	}

	protected override bool WindowsMessageDwmCompositionChanged(ref Message m)
	{
		bool result = base.WindowsMessageDwmCompositionChanged(ref m);
		UpdateFormControlStyle();
		return result;
	}

	protected override bool WindowsMessageNCCalcSize(ref Message m)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		if (_0B82 != null)
		{
			_0B82.ᱳ = (int)((Form)this).FormBorderStyle == 5 || (int)((Form)this).FormBorderStyle == 6;
		}
		return true;
	}

	protected override bool WindowsMessageNCActivate(ref Message m)
	{
		if (((Message)(ref m)).WParam != IntPtr.Zero)
		{
			base.Ứ = true;
		}
		else
		{
			base.Ứ = false;
			_ể = DateTime.MinValue;
		}
		ị();
		BaseWndProc(ref m);
		Ọ();
		if (base.NonClientPaintEnabled)
		{
			PaintNonClientAreaBuffered();
			Redraw();
		}
		return false;
	}

	private void ị()
	{
		if (Ề != null)
		{
			Ề.Dispose();
			Ề = null;
		}
		if (Ẻ.DisplayRectangle.Width > 0 && Ẻ.DisplayRectangle.Height > 0 && ((Control)this).IsHandleCreated && !((Control)this).IsDisposed)
		{
			IntPtr windowDC = _0EFE.GetWindowDC(((Control)this).Handle);
			try
			{
				Ề = new BufferedBitmap(windowDC, new Rectangle(ề.X, ề.Y, Ẻ.DisplayRectangle.Width, Ẻ.DisplayRectangle.Height));
			}
			finally
			{
				_0EFE.ReleaseDC(((Control)this).Handle, windowDC);
			}
			ợ(Ề.Graphics);
		}
	}

	private void Ọ()
	{
		if (Ề != null)
		{
			IntPtr windowDC = _0EFE.GetWindowDC(((Control)this).Handle);
			Graphics val = Graphics.FromHdc(windowDC);
			try
			{
				Ề.Render(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			_0EFE.ReleaseDC(((Control)this).Handle, windowDC);
			Ề.Dispose();
			Ề = null;
		}
	}

	protected override Rectangle GetClientRectangle(Rectangle rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		Ἇ ἇ = Ꮛ();
		if ((int)base.WindowState == 1 && ((Form)this).IsMdiChild)
		{
			return new Rectangle(rect.X + ἇ.ἒ, rect.Y + ἇ.ἐ, rect.Width - (ἇ.ἒ + ἇ.ἓ), 0);
		}
		return new Rectangle(rect.X + ἇ.ἒ, rect.Y + ἇ.ἐ, rect.Width - (ἇ.ἒ + ἇ.ἓ), rect.Height - (ἇ.ἐ + ἇ.ἑ));
	}

	protected override GraphicsPath GetFormPath(Rectangle bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = bounds;
		if (!UseCornerSize || (base.TopLeftCornerSize == 0 && base.TopRightCornerSize == 0))
		{
			val.AddRectangle(rectangle);
			return val;
		}
		ᒇ ᒇ2 = ElementStyleDisplay.സ(rectangle, ன, eCornerArc.TopLeft);
		val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		ᒇ2 = ElementStyleDisplay.സ(rectangle, ன, eCornerArc.TopRight);
		val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		val.AddLine(rectangle.Right, rectangle.Bottom - 3, rectangle.Right, rectangle.Bottom);
		val.AddLine(rectangle.Right, rectangle.Bottom, rectangle.Right - 3, rectangle.Bottom);
		val.AddLine(rectangle.X + 3, rectangle.Bottom, rectangle.X, rectangle.Bottom);
		val.AddLine(rectangle.X, rectangle.Bottom, rectangle.X, rectangle.Bottom - 3);
		val.CloseAllFigures();
		return val;
	}

	private Point ọ()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		if (((Control)this).IsDisposed)
		{
			return Point.Empty;
		}
		Point result = ((Control)this).PointToClient(Control.MousePosition);
		if ((int)base.WindowState != 1)
		{
			result.X += Ẻ.PaddingLeft;
			result.Y += Ẻ.HeightInternal;
		}
		return result;
	}

	private void Ỏ()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_Ể == null)
		{
			_Ể = new Timer();
			_Ể.Interval = 300;
			_Ể.Tick += ỏ;
			_Ể.Start();
		}
	}

	private void ỏ(object _٠, EventArgs ؿ)
	{
		Point pt = ọ();
		if (!Ẻ.Bounds.Contains(pt))
		{
			ẹ = false;
		}
	}

	private void Ố()
	{
		Timer ể = _Ể;
		_Ể = null;
		if (ể != null)
		{
			ể.Stop();
			ể.Tick -= ỏ;
			((Component)(object)ể).Dispose();
		}
	}

	protected override bool WindowsMessageNCMouseMove(ref Message m)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Invalid comparison between Unknown and I4
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Invalid comparison between Unknown and I4
		Point point = ọ();
		Ẻ.InternalMouseMove(new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0));
		ẹ = true;
		if (Ẽ.Tooltip != "" && Ẻ.HotSubItem == Ẽ)
		{
			ố();
		}
		if ((int)Control.MouseButtons == 1048576)
		{
			BaseItem baseItem = Ẻ.ItemAtLocation(point.X, point.Y);
			if (baseItem == ẻ && Ẻ.ItemAtLocation(_Ệ.X, _Ệ.Y) == baseItem)
			{
				if ((int)base.WindowState == 0 || ((int)base.WindowState == 1 && ((Form)this).IsMdiChild))
				{
					byte[] bytes = BitConverter.GetBytes(point.X);
					byte[] bytes2 = BitConverter.GetBytes(point.Y);
					byte[] value = new byte[4]
					{
						bytes[0],
						bytes[1],
						bytes2[0],
						bytes2[1]
					};
					int lParam = BitConverter.ToInt32(value, 0);
					((Control)this).Capture = false;
					PopupManager.CloseAllPopups();
					_089F.SendMessage(((Control)this).Handle, 274, 61458, lParam);
					return false;
				}
				if ((int)base.WindowState == 2 && BarFunctions.IsWindows7 && _Ꮄ)
				{
					if (_ể != DateTime.MinValue)
					{
						if (DateTime.Now.Subtract(_ể).TotalSeconds > 0.0)
						{
							base.WindowState = (FormWindowState)0;
							_ể = DateTime.MinValue;
						}
					}
					else
					{
						_ể = DateTime.Now;
					}
				}
			}
		}
		return true;
	}

	protected override void OnActivated(EventArgs e)
	{
		_Ꮄ = true;
		base.OnActivated(e);
	}

	protected override bool WindowsMessageNCMouseLeave(ref Message m)
	{
		ồ();
		Ẻ.InternalMouseLeave();
		ẹ = false;
		return true;
	}

	private void ố()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (ễ != null)
		{
			ễ.Stop();
			ễ.Start();
			return;
		}
		ễ = new Timer();
		ễ.Interval = Math.Max(500, SystemInformation.MouseHoverTime);
		ễ.Tick += Ồ;
		ễ.Start();
	}

	private void Ồ(object _٠, EventArgs ؿ)
	{
		ễ.Stop();
		if (Ẽ != null && Ẽ.Tooltip != "" && Ẻ != null && Ẻ.HotSubItem == Ẽ)
		{
			Ẻ.InternalMouseHover();
		}
		ồ();
	}

	private void ồ()
	{
		Timer val = ễ;
		if (ễ != null)
		{
			ễ = null;
			val.Stop();
			val.Tick -= Ồ;
		}
	}

	protected override bool WindowsMessageNCHitTest(ref Message m)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		int x = _0EFE._0F0B(((Message)(ref m)).LParam);
		int y = _0EFE._0F0D(((Message)(ref m)).LParam);
		Point pt = ((Control)this).PointToClient(new Point(x, y));
		if ((int)base.WindowState != 1)
		{
			pt.X += ᗀ;
			pt.Y += ᗀ + SystemInformation.CaptionHeight;
		}
		int num = ᗀ;
		int num2 = 2;
		if (IsSizable)
		{
			if (((Form)this).RightToLeftLayout && (int)((Control)this).RightToLeft == 1)
			{
				if (new Rectangle(((Control)this).Width - (ன + num2), 0, ன + num2, ன + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(13);
					return false;
				}
				if (new Rectangle(0, 0, ன + num2, ன + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(14);
					return false;
				}
				if (new Rectangle(((Control)this).Width - (ன + num2), ((Control)this).Height - (ன + num2), ன + num2, ன + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(16);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - (ன + num2), ன + num2, ன + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(17);
					return false;
				}
				if (new Rectangle(((Control)this).Width - num, 0, num, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(10);
					return false;
				}
				if (new Rectangle(0, 0, num, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(11);
					return false;
				}
			}
			else
			{
				if (new Rectangle(0, 0, ன + num2, ன + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(13);
					return false;
				}
				if (new Rectangle(((Control)this).Width - (ன + num2), 0, ன + num2, ன + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(14);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - ன, ன + num2, ன + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(16);
					return false;
				}
				if (new Rectangle(((Control)this).Width - ன, ((Control)this).Height - ன, ன + num2, ன + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(17);
					return false;
				}
				if (new Rectangle(0, 0, ((Control)this).Width, num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(12);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - num, ((Control)this).Width, num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(15);
					return false;
				}
				if (new Rectangle(0, 0, num, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(10);
					return false;
				}
				if (new Rectangle(((Control)this).Width - num, 0, num, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(11);
					return false;
				}
			}
		}
		BaseItem baseItem = Ẻ.ItemAtLocation(pt.X, pt.Y);
		if (baseItem == ẻ)
		{
			((Message)(ref m)).Result = new IntPtr(2);
			return false;
		}
		if (baseItem == Ẽ)
		{
			((Message)(ref m)).Result = new IntPtr(5);
			return false;
		}
		if (baseItem == _0B82)
		{
			switch (_0B82.ᱽ(pt.X, pt.Y))
			{
			case _1886._1C8C:
				((Message)(ref m)).Result = new IntPtr(8);
				break;
			case _1886._1C8F:
				((Message)(ref m)).Result = new IntPtr(9);
				break;
			case _1886.ᆤ:
				((Message)(ref m)).Result = new IntPtr(20);
				break;
			case _1886._1C8D:
				((Message)(ref m)).Result = new IntPtr(9);
				break;
			case _1886._1C8E:
				((Message)(ref m)).Result = new IntPtr(21);
				break;
			}
			return false;
		}
		if (baseItem is ButtonItem)
		{
			((Message)(ref m)).Result = new IntPtr(19);
			return false;
		}
		return true;
	}

	protected override bool WindowsMessageNCLButtonDown(ref Message m)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		int num = _0EFE._0F11(((Message)(ref m)).WParam);
		if (num == 12 || num == 14)
		{
			return true;
		}
		Point point = (_Ệ = ọ());
		Ẻ.InternalMouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		BaseItem baseItem = Ẻ.ItemAtLocation(point.X, point.Y);
		if (baseItem != null && (baseItem == _0B82 || baseItem == ẻ || (point.X >= _0B82.DisplayRectangle.X && point.X <= _0B82.DisplayRectangle.Right)))
		{
			if (!base.Ứ)
			{
				((Form)this).Activate();
			}
			return false;
		}
		return true;
	}

	protected override bool WindowsMessageNCLButtonUp(ref Message m)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		Point point = ọ();
		BaseItem baseItem = Ẻ.ItemAtLocation(point.X, point.Y);
		Ẻ.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		if (point == _Ệ && !string.IsNullOrEmpty(TitleText))
		{
			Ẻ.InternalClick((MouseButtons)1048576, point);
		}
		if (baseItem == _0B82)
		{
			return false;
		}
		return true;
	}

	private void Ổ(object _٠, EventArgs ؿ)
	{
		SystemCaptionItem systemCaptionItem = _٠ as SystemCaptionItem;
		if (systemCaptionItem.ᲀ == _1886._1C8C)
		{
			_089F.PostMessage(((Control)this).Handle, 274, 61472, 0);
			Ẻ.InternalMouseLeave();
		}
		else if (systemCaptionItem.ᲀ == _1886._1C8F)
		{
			_089F.PostMessage(((Control)this).Handle, 274, 61488, 0);
			Ẻ.InternalMouseLeave();
		}
		else if (systemCaptionItem.ᲀ == _1886._1C8D)
		{
			_089F.PostMessage(((Control)this).Handle, 274, 61728, 0);
			Ẻ.InternalMouseLeave();
		}
		else if (systemCaptionItem.ᲀ == _1886.ᆤ)
		{
			_089F.PostMessage(((Control)this).Handle, 274, 61536, 0);
			Ẻ.InternalMouseLeave();
		}
		else if (systemCaptionItem.ᲀ == _1886._1C8E)
		{
			_089F.PostMessage(((Control)this).Handle, 274, 61824, 0);
			Ẻ.InternalMouseLeave();
		}
	}

	private void ổ(object _٠, EventArgs ؿ)
	{
		if (ẽ == null)
		{
			Ỗ();
		}
	}

	private void Ỗ()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		ẽ = new Timer();
		ẽ.Interval = 200;
		ẽ.Tick += ỗ;
		ẽ.Start();
	}

	private void ỗ(object _٠, EventArgs ؿ)
	{
		Point pt = ọ();
		if (_0B82 != null && !_0B82.DisplayRectangle.Contains(pt))
		{
			ẽ.Enabled = false;
			Ộ();
			_0B82.InternalMouseLeave();
		}
	}

	private void Ộ()
	{
		if (ẽ != null)
		{
			ẽ.Enabled = false;
			ẽ.Tick -= ỗ;
			((Component)(object)ẽ).Dispose();
			ẽ = null;
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible)
		{
			Ộ();
			Ố();
		}
		((Form)this).OnVisibleChanged(e);
	}

	protected override void OnClosed(EventArgs e)
	{
		Ộ();
		((Form)this).OnClosed(e);
	}

	protected override void OnResize(EventArgs e)
	{
		_0BE5();
		base.OnResize(e);
	}

	internal void _0BE5()
	{
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Invalid comparison between Unknown and I4
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Invalid comparison between Unknown and I4
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Invalid comparison between Unknown and I4
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Invalid comparison between Unknown and I4
		if (!((Control)this).IsHandleCreated)
		{
			return;
		}
		if (Ẻ != null)
		{
			Ἇ ἇ = Ꮛ();
			Ẻ.LeftInternal = 1;
			Ẻ.TopInternal = 0;
			Ẻ.PaddingLeft = Dpi.DescaleWidth(ἇ.ἒ) - 1;
			Ẻ.PaddingRight = Dpi.DescaleWidth(ἇ.ἓ);
			Ẻ.PaddingTop = Dpi.DescaleHeight(ἇ.ἑ) / 2 + 1;
			Ẻ.WidthInternal = ((Control)this).Width - 1;
			Ẻ.HeightInternal = ἇ.ἐ - ἇ.ἑ / 2;
			Ẻ.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
			Ẻ._0DD8 = false;
			if (ẻ != null)
			{
				ẻ.Height = Dpi.DescaleHeight(Ẻ.HeightInternal - ἇ.ἑ - 2);
				if (_0D81 != "")
				{
					ẻ.Width = 0;
				}
				else
				{
					ẻ.Width = 16;
				}
				ẻ.TextLineAlignment = (StringAlignment)1;
			}
			Ẻ.RecalcSize();
			Ẻ.HeightInternal = ἇ.ἐ;
			if (Ẽ != null)
			{
				Ẽ.TopInternal += (Ẻ.HeightInternal - Ẽ.HeightInternal - ἇ.ἑ * 2) / 2 + 1;
			}
			ẻ.TopInternal += Dpi.Height1;
			if ((int)base.WindowState == 2)
			{
				if (Ẽ != null)
				{
					Ẽ.TopInternal += Dpi.Height2;
				}
				ẻ.TopInternal += Dpi.Height2;
			}
			if ((int)((Form)this).FormBorderStyle != 1)
			{
				_0B82.TopInternal -= Dpi.Height1;
			}
		}
		if ((int)base.WindowState == 2 || (int)base.WindowState == 1)
		{
			_0B82.RestoreEnabled = true;
		}
		else
		{
			_0B82.RestoreEnabled = false;
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		Ẻ.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnDeactivate(EventArgs e)
	{
		_Ꮄ = false;
		base.OnDeactivate(e);
		Ộ();
		Ẻ.InternalMouseLeave();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		UpdateFormText();
	}

	public void UpdateFormText()
	{
		bool flag = false;
		if (ẻ != null && _0D81 == "")
		{
			flag = ẻ.Text != ((Control)this).Text;
			ẻ.Text = ((Control)this).Text;
		}
		if (flag)
		{
			if (((Form)this).IsMdiChild && ((Form)this).MdiParent is RibbonForm)
			{
				((RibbonForm)(object)((Form)this).MdiParent).ἄ();
			}
			InvalidateNonClient(invalidateForm: false);
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		if (e.Control is MdiClient)
		{
			e.Control.ControlRemoved -= new ControlEventHandler(ở);
			e.Control.ControlAdded -= new ControlEventHandler(Ỡ);
			e.Control.HandleCreated -= Ở;
			ộ();
		}
		base.OnControlRemoved(e);
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		if (e.Control is MdiClient)
		{
			e.Control.ControlRemoved += new ControlEventHandler(ở);
			e.Control.ControlAdded += new ControlEventHandler(Ỡ);
			e.Control.HandleCreated += Ở;
			Ớ((MdiClient)e.Control);
		}
		base.OnControlAdded(e);
	}

	private void ộ()
	{
		if (_Ỉ != null)
		{
			_Ỉ._20F0 -= Ờ;
			((NativeWindow)_Ỉ).ReleaseHandle();
			_Ỉ = null;
		}
	}

	private void Ớ(MdiClient ớ)
	{
		if (((Control)ớ).IsHandleCreated)
		{
			ờ(ớ);
		}
	}

	private void Ờ(object _٠, EventArgs ؿ)
	{
		_20EF obj = _٠ as _20EF;
		if (_ệ)
		{
			obj._20F3 = true;
		}
	}

	private void ờ(MdiClient ᖲ)
	{
		ộ();
		_Ỉ = new _20EF();
		_Ỉ._20F0 += Ờ;
		((NativeWindow)_Ỉ).AssignHandle(((Control)ᖲ).Handle);
	}

	private void Ở(object _٠, EventArgs ؿ)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		ờ((MdiClient)_٠);
	}

	private void ở(object _٠, ControlEventArgs ؿ)
	{
		ؿ.Control.Resize -= ỡ;
	}

	private void Ỡ(object _٠, ControlEventArgs ؿ)
	{
		ؿ.Control.Resize += ỡ;
	}

	protected override void OnMdiChildActivate(EventArgs e)
	{
		UpdateFormText();
		base.OnMdiChildActivate(e);
	}

	private void ỡ(object _٠, EventArgs ؿ)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		ڣ((MethodInvoker)delegate
		{
			UpdateFormText();
		});
	}

	protected override void OnStyleChanged(EventArgs e)
	{
		Ợ();
		InvalidateNonClient(invalidateForm: false);
		base.OnStyleChanged(e);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		Ị();
		((Control)this).OnSystemColorsChanged(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		Ợ();
		if (_ế)
		{
			_ế = false;
			UpdateFormGlassState();
		}
		base.OnHandleCreated(e);
	}

	protected override void OnRenderFormIconChanged(bool oldValue, bool newValue)
	{
		Ợ();
		base.OnRenderFormIconChanged(oldValue, newValue);
	}

	protected override void OnRenderFormTextChanged(bool oldValue, bool newValue)
	{
		Ợ();
		base.OnRenderFormTextChanged(oldValue, newValue);
	}

	private void Ợ()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Invalid comparison between Unknown and I4
		if (_0B82 != null)
		{
			_0B82.MinimizeVisible = ((Form)this).MinimizeBox;
			_0B82.RestoreMaximizeVisible = ((Form)this).MaximizeBox;
			_0B82.Visible = ((Form)this).ControlBox;
			Ẽ.Visible = ((Form)this).ControlBox;
			_0B82.HelpVisible = ((Form)this).HelpButton;
		}
		if (Ẽ != null)
		{
			Ẽ.Visible = ((Form)this).ShowIcon && base.RenderFormIcon;
		}
		if ((int)((Form)this).FormBorderStyle == 3)
		{
			Ẽ.Visible = false;
		}
		if (ẻ != null)
		{
			ẻ.IsRendered = base.RenderFormText;
		}
		if (((Control)this).IsHandleCreated)
		{
			_0BE5();
		}
	}

	protected virtual void OnCloseEnabledChanged()
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			IntPtr handle = ((Control)this).Handle;
			IntPtr systemMenu = _089F.GetSystemMenu(handle, bRevert: false);
			uint num = 0u;
			num = ((!CloseEnabled) ? 1u : 4294967294u);
			_0EFE.EnableMenuItem(systemMenu, 61536u, num);
		}
	}

	protected override bool WindowsMessageNCPaint(ref Message m)
	{
		((Message)(ref m)).Result = IntPtr.Zero;
		if (((Control)this).IsHandleCreated && !((Control)this).IsDisposed && ((Control)this).Handle != IntPtr.Zero && base.EnableCustomStyle && base.NonClientPaintEnabled)
		{
			PaintNonClientAreaBuffered();
		}
		return false;
	}

	protected virtual void PaintNonClientAreaBuffered()
	{
		IntPtr windowDC = _0EFE.GetWindowDC(((Control)this).Handle);
		try
		{
			Graphics val = Graphics.FromHdc(windowDC);
			try
			{
				PaintNonClientAreaBuffered(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			_0EFE.ReleaseDC(((Control)this).Handle, windowDC);
		}
	}

	protected virtual void PaintNonClientAreaBuffered(Graphics targetGraphics)
	{
		BufferedBitmap bufferedBitmap = new BufferedBitmap(targetGraphics, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
		try
		{
			bufferedBitmap.Graphics.SetClip(GetClientRectangle(new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)), (CombineMode)4);
			PaintNonClientArea(bufferedBitmap.Graphics);
			bufferedBitmap.Render(targetGraphics, GetClientRectangle(new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)));
		}
		finally
		{
			bufferedBitmap.Dispose();
		}
	}

	protected virtual Rectangle GetInnerFormBounds()
	{
		Rectangle result = new Rectangle(1, 1, ((Control)this).Width - 3, ((Control)this).Height - 1);
		if (((Form)this).RightToLeftLayout)
		{
			result = new Rectangle(1, 1, ((Control)this).Width - 2, ((Control)this).Height - 1);
		}
		return result;
	}

	protected virtual void PaintNonClientArea(Graphics g)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		Ἇ ἇ = Ꮛ();
		PaintFormBorder(g, ἇ.ἒ, borderOnly: false);
		Rectangle innerFormBounds = GetInnerFormBounds();
		if (ன > 1 && UseCornerSize)
		{
			ன--;
		}
		GraphicsPath formPath = GetFormPath(innerFormBounds);
		try
		{
			if (ன > 0 && UseCornerSize)
			{
				ன++;
			}
			Region val = new Region();
			val.MakeEmpty();
			val.Union(formPath);
			formPath.Widen(SystemPens.Control);
			new Region(formPath);
			val.Union(formPath);
			g.SetClip(val, (CombineMode)0);
			ợ(g);
		}
		finally
		{
			((IDisposable)formPath)?.Dispose();
		}
		g.ResetClip();
	}

	private void ợ(Graphics _٥)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		ItemPaintArgs itemPaintArgs = _0A92(_٥);
		SmoothingMode smoothingMode = _٥.SmoothingMode;
		_ = _٥.TextRenderingHint;
		if (ե)
		{
			_٥.SmoothingMode = (SmoothingMode)4;
		}
		if (itemPaintArgs.ໝ != null)
		{
			if (itemPaintArgs.ໝ is Office2007Renderer)
			{
				if (base.Ứ)
				{
					ẻ.ForeColor = ((Office2007Renderer)itemPaintArgs.ໝ).ColorTable.Form.Active.CaptionText;
				}
				else
				{
					ẻ.ForeColor = ((Office2007Renderer)itemPaintArgs.ໝ).ColorTable.Form.Inactive.CaptionText;
				}
			}
			itemPaintArgs.ໝ.DrawFormCaptionBackground(new FormCaptionRendererEventArgs(_٥, Ẻ.DisplayRectangle, (Form)(object)this));
		}
		int borderSize = 0;
		if ((((Control)this).BackgroundImage != null && base.BackgroundImageUnderCaption) || StyleManager.IsMetro(StyleManager.Style))
		{
			Ἇ ἇ = Ꮛ();
			borderSize = ἇ.ἒ;
		}
		PaintNonClientBackgroundImage(_٥, ((Control)this).ClientRectangle);
		if ((((Control)this).BackgroundImage != null && base.BackgroundImageUnderCaption) || StyleManager.IsMetro(StyleManager.Style))
		{
			PaintFormBorder(_٥, borderSize, borderOnly: true);
		}
		Ẻ.Paint(itemPaintArgs);
		_٥.SmoothingMode = smoothingMode;
	}

	private ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(null, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		return itemPaintArgs;
	}

	protected virtual ColorScheme GetColorScheme()
	{
		if (լ == null)
		{
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			return new ColorScheme(eDotNetBarStyle.Office2007);
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
		return ਜ;
	}

	[CompilerGenerated]
	private void Ụ()
	{
		UpdateFormText();
	}
}
