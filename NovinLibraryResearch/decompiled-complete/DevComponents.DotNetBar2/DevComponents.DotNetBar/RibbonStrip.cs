using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
public class RibbonStrip : ItemControl
{
	private const int _൯ = 29;

	private RibbonStripContainerItem _0D70;

	private RibbonTabItemGroupCollection _0D71;

	private int _0D72 = 14;

	private bool _0D73;

	private Font _0D74;

	private Font _0D75;

	private int _0D76;

	private bool _0D77;

	private bool _0D78 = true;

	private int _0D79 = 46;

	private Rectangle ൺ = Rectangle.Empty;

	private Rectangle ൻ = Rectangle.Empty;

	private Rectangle ർ = Rectangle.Empty;

	private Rectangle[] ൽ;

	private Font ൾ;

	private bool શ = true;

	private ElementStyle ൿ = new ElementStyle();

	private bool _0D80 = true;

	private string _0D81 = "";

	private bool _0D82;

	private MarkupLinkClickEventHandler _0D83;

	private _0CB4 _0D84;

	internal Rectangle අ = Rectangle.Empty;

	private bool _ආ;

	private bool ඇ = true;

	private bool ඈ;

	private IKeyTipsControl ඉ;

	private bool _ඊ;

	private bool උ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool CanSupportGlass
	{
		get
		{
			return _0D82;
		}
		set
		{
			_0D82 = value;
		}
	}

	internal override bool ষ
	{
		get
		{
			bool flag = base.ষ;
			if (!_0D82 || !flag)
			{
				return false;
			}
			Form val = ((Control)this).FindForm();
			if (val is OfficeForm)
			{
				return false;
			}
			if (val is RibbonForm)
			{
				return ((RibbonForm)(object)val).IsGlassEnabled;
			}
			return flag;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
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
			_0D84 = null;
			if (ሒ.መ(ref _0D81))
			{
				_0D84 = ሒ.ሕ(_0D81);
				if (_0D84 != null)
				{
					_0D84.ವ += ඌ;
				}
				අ = Rectangle.Empty;
				((Control)this).Invalidate();
			}
		}
	}

	internal _0CB4 _0D62 => _0D84;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether KeyTips functionality is enabled.")]
	public bool KeyTipsEnabled
	{
		get
		{
			return _0D80;
		}
		set
		{
			_0D80 = value;
		}
	}

	[DefaultValue(true)]
	[Category("Customization")]
	[Browsable(true)]
	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	public bool CanCustomize
	{
		get
		{
			return શ;
		}
		set
		{
			શ = value;
		}
	}

	[Description("Indicates explicit height of the caption provided by control")]
	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Appearance")]
	public int CaptionHeight
	{
		get
		{
			return _0D76;
		}
		set
		{
			_0D76 = value;
			_0D70.NeedRecalcSize = true;
		}
	}

	internal bool _0D63
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item.Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	internal Rectangle[] _0D64
	{
		get
		{
			return ൽ;
		}
		set
		{
			ൽ = value;
		}
	}

	internal Rectangle _0D65
	{
		get
		{
			return ൺ;
		}
		set
		{
			ൺ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Rectangle QuickToolbarBounds
	{
		get
		{
			return ൻ;
		}
		set
		{
			ൻ = value;
		}
	}

	internal Rectangle _൦
	{
		get
		{
			return ർ;
		}
		set
		{
			ർ = value;
		}
	}

	internal SystemCaptionItem _൧ => _0D70.SystemCaptionItem;

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether custom caption line provided by the control is visible.")]
	[Browsable(true)]
	public bool CaptionVisible
	{
		get
		{
			return _0D77;
		}
		set
		{
			_0D77 = value;
			ඍ();
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Description("")]
	public Font CaptionFont
	{
		get
		{
			return ൾ;
		}
		set
		{
			ൾ = value;
			_0D70.NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(46)]
	[Category("Layout")]
	[Description("Indicates indent of the ribbon strip.")]
	[Browsable(true)]
	public int RibbonStripIndent
	{
		get
		{
			return _0D79;
		}
		set
		{
			_0D79 = value;
			RecalcLayout();
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Groups")]
	[Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
	[DefaultValue(14)]
	[Browsable(true)]
	public int TabGroupHeight
	{
		get
		{
			return _0D72;
		}
		set
		{
			_0D72 = value;
			_0D70.NeedRecalcSize = true;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
	[DefaultValue(false)]
	[Category("Tab Groups")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public bool TabGroupsVisible
	{
		get
		{
			return _0D73;
		}
		set
		{
			_0D73 = value;
			_0D70.NeedRecalcSize = true;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Category("Tab Groups")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Editor("DevComponents.DotNetBar.Design.RibbonTabItemGroupCollectionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public RibbonTabItemGroupCollection TabGroups => _0D71;

	internal eDotNetBarStyle _൨ => _0D70.EffectiveStyle;

	[Browsable(true)]
	[Description("Specifies the visual style of the control.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle Style
	{
		get
		{
			return _0D70.Style;
		}
		set
		{
			base.ColorScheme.ߕ = value;
			_0D70.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => _0D70.RibbonStripContainer.SubItems;

	[Browsable(false)]
	public RibbonTabItem SelectedRibbonTabItem
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item is RibbonTabItem && ((RibbonTabItem)item).Checked)
				{
					return (RibbonTabItem)item;
				}
			}
			return null;
		}
	}

	internal bool _൩
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return (int)val.WindowState == 2;
			}
			return false;
		}
	}

	internal bool _൪
	{
		get
		{
			return _ආ;
		}
		set
		{
			if (_ආ != value)
			{
				_ආ = value;
				((Control)this).Invalidate();
				Form val = ((Control)this).FindForm();
				if (val != null)
				{
					((Control)val).Invalidate();
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Tab Groups")]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	public Font DefaultGroupFont
	{
		get
		{
			return _0D74;
		}
		set
		{
			_0D74 = value;
			if (((Component)(object)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	internal bool _൫
	{
		get
		{
			return _0D78;
		}
		set
		{
			_0D78 = value;
		}
	}

	internal bool _൬
	{
		get
		{
			return ඇ;
		}
		set
		{
			ඇ = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IgnoreF10Key
	{
		get
		{
			return _ඊ;
		}
		set
		{
			_ඊ = value;
		}
	}

	internal bool _൭
	{
		get
		{
			if (ඩ() is ApplicationButton { BackstageTab: not null, Expanded: not false, BackstageTabEnabled: not false })
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsInKeyTipsMode => ඈ | ShowKeyTips;

	internal bool _൮ => උ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection QuickToolbarItems => _0D70.CaptionContainer.SubItems;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public GenericItemContainer CaptionContainerItem => _0D70.CaptionContainer;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public GenericItemContainer StripContainerItem => _0D70.RibbonStripContainer;

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
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

	public RibbonStrip()
	{
		((Control)this).SetStyle((ControlStyles)4096, true);
		_0D70 = new RibbonStripContainerItem(this);
		_0D70.GlobalItem = false;
		_0D70.ContainerControl = this;
		_0D70.Displayed = true;
		_0D70.SetOwner(this);
		_0D70.Style = eDotNetBarStyle.Office2003;
		SetBaseItemContainer(_0D70);
		base.ColorScheme.ߕ = eDotNetBarStyle.Office2003;
		((Control)this).AutoSize = true;
		_0D71 = new RibbonTabItemGroupCollection();
		_0D71._၉ = this;
		_0D70.SystemCaptionItem.Click += ණ;
	}

	protected override void Dispose(bool disposing)
	{
		_0D70.SystemCaptionItem.Click -= ණ;
		base.Dispose(disposing);
	}

	private void ඌ(object _٠, EventArgs ؿ)
	{
		if (_٠ is _0CFE obj && _0D83 != null)
		{
			_0D83(this, new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	private void ඍ()
	{
		_0D70.NeedRecalcSize = true;
		RecalcLayout();
	}

	internal override ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = base._0A92(_٥);
		if (((Control)this).Parent is RibbonControl ribbonControl)
		{
			itemPaintArgs.ControlExpanded = ribbonControl.Expanded || ribbonControl.IsPopupMode;
		}
		return itemPaintArgs;
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Invalid comparison between Unknown and I4
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).Msg == 132)
		{
			int x = _0EFE._0F0B(((Message)(ref m)).LParam);
			int y = _0EFE._0F0D(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (ষ && CaptionVisible)
			{
				Rectangle rectangle = new Rectangle(((Control)this).Width - BarFunctions.CaptionButtonSize.Width * 3, 0, BarFunctions.CaptionButtonSize.Width * 3, BarFunctions.CaptionButtonSize.Height + 6);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (BarFunctions.IsWindows7 && _൩)
				{
					Rectangle[] array = ൽ;
					if (array != null)
					{
						Rectangle[] array2 = array;
						foreach (Rectangle rectangle2 in array2)
						{
							if (rectangle2.Contains(pt))
							{
								((Message)(ref m)).Result = new IntPtr(-1);
								return;
							}
						}
					}
				}
			}
			else if (CaptionVisible && !_൩ && new Rectangle(0, 0, ((Control)this).Width, Dpi.Height4).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			eDotNetBarStyle eDotNetBarStyle2 = _൨;
			if (_0D77 && (eDotNetBarStyle2 == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(eDotNetBarStyle2)) && ((pt.X < Dpi.Width28 && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - Dpi.Width28 && (int)((Control)this).RightToLeft == 1)) && pt.Y < Dpi.Height28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if (_0D77 && eDotNetBarStyle2 == eDotNetBarStyle.Windows7 && ((pt.X < Dpi.Width28 && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - Dpi.Width28 && (int)((Control)this).RightToLeft == 1)) && pt.Y < Dpi.Height28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		base.WndProc(ref m);
	}

	protected override void ClearBackground(ItemPaintArgs pa)
	{
		if (ষ)
		{
			if (((Control)this).FindForm() is RibbonForm ribbonForm)
			{
				pa.Graphics.Clear(Color.Transparent);
				pa.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, ribbonForm.Ủ - 1), (CombineMode)4);
			}
			base.ClearBackground(pa);
			pa.Graphics.ResetClip();
		}
		else
		{
			base.ClearBackground(pa);
		}
	}

	protected override Rectangle GetPaintControlBackgroundRectangle()
	{
		if (ষ && (_൨ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_൨)))
		{
			Rectangle paintControlBackgroundRectangle = base.GetPaintControlBackgroundRectangle();
			paintControlBackgroundRectangle.Y += 29;
			paintControlBackgroundRectangle.Height -= 29;
			return paintControlBackgroundRectangle;
		}
		return base.GetPaintControlBackgroundRectangle();
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		bool flag = false;
		eDotNetBarStyle eDotNetBarStyle2 = _൨;
		bool flag2 = ষ;
		RibbonForm ribbonForm = ((Control)this).FindForm() as RibbonForm;
		if (flag2 && ribbonForm != null)
		{
			pa.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, ribbonForm.Ủ - (((eDotNetBarStyle2 != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(eDotNetBarStyle2)) || !pa.GlassEnabled) ? 1 : 29)), (CombineMode)4);
			flag = true;
		}
		if (flag2 && (eDotNetBarStyle2 == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(eDotNetBarStyle2)))
		{
			ElementStyle backgroundStyle = GetBackgroundStyle();
			if (backgroundStyle != null)
			{
				Rectangle rectangle = new Rectangle(0, 27, ((Control)this).Width, ((Control)this).Height + 8);
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.AddEllipse(rectangle);
					PathGradientBrush val2 = new PathGradientBrush(val);
					try
					{
						val2.CenterColor = backgroundStyle.BackColor;
						val2.SurroundColors = new Color[1] { backgroundStyle.BackColor2 };
						val2.CenterPoint = new PointF(rectangle.Width / 2, rectangle.Bottom - 14);
						Blend val3 = new Blend();
						val3.Factors = new float[3] { 0f, 0.85f, 1f };
						val3.Positions = new float[3] { 0f, 0.65f, 1f };
						val2.Blend = val3;
						pa.Graphics.FillPath((Brush)(object)val2, val);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else
		{
			base.PaintControlBackground(pa);
		}
		if (flag)
		{
			pa.Graphics.ResetClip();
		}
		ൻ = Rectangle.Empty;
		ൺ = Rectangle.Empty;
		ർ = Rectangle.Empty;
		BaseRenderer renderer = GetRenderer();
		RibbonControl ribbonControl = ((Control)this).Parent as RibbonControl;
		if (renderer != null && ribbonControl != null)
		{
			RibbonControlRendererEventArgs ribbonControlRendererEventArgs = new RibbonControlRendererEventArgs(pa.Graphics, ((Control)this).Parent as RibbonControl, pa.GlassEnabled);
			ribbonControlRendererEventArgs.က = pa;
			renderer.DrawRibbonControlBackground(ribbonControlRendererEventArgs);
			if (((Control)ribbonControl).BackgroundImage != null && !_൭)
			{
				Rectangle rectangle2 = ඎ(ribbonForm);
				BarFunctions._0982(pa.Graphics, rectangle2, ((Control)ribbonControl).BackgroundImage, ribbonControl.BackgroundImagePosition, 255);
			}
			if (_0D77)
			{
				renderer.DrawQuickAccessToolbarBackground(new RibbonControlRendererEventArgs(pa.Graphics, ((Control)this).Parent as RibbonControl, pa.GlassEnabled));
			}
		}
		if (_0D73)
		{
			ඐ(pa);
		}
		if (renderer != null && _0D77)
		{
			RibbonControlRendererEventArgs ribbonControlRendererEventArgs2 = new RibbonControlRendererEventArgs(pa.Graphics, ((Control)this).Parent as RibbonControl, pa.GlassEnabled);
			ribbonControlRendererEventArgs2.က = pa;
			renderer.DrawRibbonFormCaptionText(ribbonControlRendererEventArgs2);
		}
		if (_089F.फ())
		{
			pa.Graphics.Clear(Color.White);
			TextDrawing.DrawString(pa.Graphics, "Trial Version Expired :-(", ((Control)this).Font, Color.FromArgb(128, Color.Black), ((Control)this).ClientRectangle, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
	}

	private Rectangle ඎ(RibbonForm ඏ)
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (ඏ != null)
		{
			clientRectangle.Inflate(ඏ.ủ - 1, 0);
		}
		return clientRectangle;
	}

	internal void ඐ(ItemPaintArgs _0BD2)
	{
		if (_0D71.Count == 0 || _ආ)
		{
			return;
		}
		RibbonTabItemGroup ribbonTabItemGroup = null;
		Rectangle भ = Rectangle.Empty;
		Rectangle ඓ = Rectangle.Empty;
		int num = 0;
		eDotNetBarStyle eDotNetBarStyle2 = _൨;
		if (_0D77)
		{
			num = ((eDotNetBarStyle2 != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(eDotNetBarStyle2)) ? (num + ((_0BD2.GlassEnabled && eDotNetBarStyle2 == eDotNetBarStyle.Windows7) ? 4 : 3)) : (num + 1));
		}
		foreach (RibbonTabItemGroup tabGroup in TabGroups)
		{
			tabGroup.DisplayPositions.Clear();
		}
		foreach (BaseItem item in Items)
		{
			if (_089F.फ())
			{
				return;
			}
			if (item is RibbonTabItem)
			{
				RibbonTabItem ribbonTabItem = item as RibbonTabItem;
				if (ribbonTabItem.Group != ribbonTabItemGroup)
				{
					if (ribbonTabItem.Visible)
					{
						if (ribbonTabItemGroup != null)
						{
							එ(_0BD2, ribbonTabItemGroup, भ, ඓ);
						}
						ribbonTabItemGroup = ribbonTabItem.Group;
						if (ribbonTabItemGroup != null)
						{
							ribbonTabItemGroup.DisplayPositions.Clear();
							भ = new Rectangle(ribbonTabItem.DisplayRectangle.X, num, ribbonTabItem.DisplayRectangle.Width, (eDotNetBarStyle2 == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(eDotNetBarStyle2)) ? ribbonTabItem.DisplayRectangle.Y : (ribbonTabItem.DisplayRectangle.Y - 4));
							ඓ = new Rectangle(ribbonTabItem.DisplayRectangle.X, num, ribbonTabItem.DisplayRectangle.Width, ribbonTabItem.DisplayRectangle.Bottom - (StyleManager.IsMetro(eDotNetBarStyle2) ? 1 : _0D72));
						}
					}
				}
				else if (ribbonTabItemGroup != null)
				{
					if (ribbonTabItem.Visible)
					{
						भ.Width += ribbonTabItem.DisplayRectangle.Right - भ.Right;
						ඓ.Width += ribbonTabItem.DisplayRectangle.Right - ඓ.Right;
					}
				}
				else
				{
					भ = Rectangle.Empty;
					ඓ = Rectangle.Empty;
				}
			}
			else if (ribbonTabItemGroup != null)
			{
				එ(_0BD2, ribbonTabItemGroup, भ, ඓ);
			}
		}
		if (ribbonTabItemGroup != null)
		{
			එ(_0BD2, ribbonTabItemGroup, भ, ඓ);
		}
	}

	private void එ(ItemPaintArgs _0BD2, RibbonTabItemGroup ඒ, Rectangle भ, Rectangle ඓ)
	{
		if ((!_0D77 || ൻ.IsEmpty || !भ.IntersectsWith(ൻ)) && !ർ.IntersectsWith(ඓ))
		{
			eDotNetBarStyle eDotNetBarStyle2 = _൨;
			if (eDotNetBarStyle2 == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(eDotNetBarStyle2))
			{
				ඒ.DisplayPositions.Add(new Rectangle(भ.X, भ.Y, भ.Width + 1, भ.Height));
			}
			else
			{
				ඒ.DisplayPositions.Add(भ);
			}
			if (BarFunctions.IsOffice2007Style(eDotNetBarStyle2))
			{
				RibbonTabGroupRendererEventArgs e = new RibbonTabGroupRendererEventArgs(_0BD2.Graphics, ඒ, भ, ඓ, ඔ(), _0BD2, eDotNetBarStyle2);
				BaseRenderer renderer = GetRenderer();
				renderer.DrawRibbonTabGroup(e);
			}
			else
			{
				ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(ඒ.Style, _0BD2.Graphics, भ);
				ElementStyleDisplay.Paint(e2);
				ElementStyleDisplay.PaintText(e2, ඒ.GroupTitle, ඔ());
			}
		}
	}

	private Font ඔ()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (_0D74 == null)
		{
			try
			{
				if (_0D75 == null)
				{
					_0D75 = new Font(((Control)this).Font.Name, ((Control)this).Font.SizeInPoints - 1f);
				}
			}
			catch
			{
				return ((Control)this).Font;
			}
			return _0D75;
		}
		return _0D74;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefaultGroupFont()
	{
		TypeDescriptor.GetProperties(this)["DefaultGroupFont"].SetValue(this, null);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		base.OnFontChanged(e);
		_0D74 = null;
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (base.BackgroundStyle.Custom)
		{
			return base.GetBackgroundStyle();
		}
		return ൿ;
	}

	internal void ඕ()
	{
		ൿ.SetColorScheme(GetColorScheme());
		if (((Control)this).Parent is RibbonControl)
		{
			RibbonPredefinedColorSchemes._20F9(ൿ, (RibbonControl)(object)((Control)this).Parent);
		}
	}

	internal ElementStyle ඖ()
	{
		return GetBackgroundStyle();
	}

	internal Rectangle _0D97()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (_0D73)
		{
			itemContainerRectangle.Y += _0D72 + 1;
		}
		if (_0D77)
		{
			itemContainerRectangle.Y += ක();
		}
		if (_0D77 && _0D79 > 0)
		{
			eDotNetBarStyle eDotNetBarStyle2 = _൨;
			if (eDotNetBarStyle2 != eDotNetBarStyle.Office2010 && eDotNetBarStyle2 != eDotNetBarStyle.Windows7 && !StyleManager.IsMetro(eDotNetBarStyle2))
			{
				if ((int)((Control)this).RightToLeft == 0)
				{
					itemContainerRectangle.X += _0D79;
				}
				itemContainerRectangle.Width -= _0D79;
			}
		}
		return itemContainerRectangle;
	}

	internal Rectangle _0D98()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (ষ)
		{
			itemContainerRectangle.Y += 3;
		}
		return new Rectangle(itemContainerRectangle.X, itemContainerRectangle.Y, itemContainerRectangle.Width, ක());
	}

	private int _0D99()
	{
		if (_0D76 == 0)
		{
			int num = 16;
			if (StyleManager.Style == eStyle.OfficeMobile2014)
			{
				num = 20;
			}
			if (ষ && _൨ != eDotNetBarStyle.Office2010 && _൨ != eDotNetBarStyle.Windows7 && !StyleManager.IsMetro(_൨))
			{
				num = num;
			}
			else if (!ষ && _൨ == eDotNetBarStyle.Office2010)
			{
				num -= 2;
			}
			else if (!ষ && _൨ == eDotNetBarStyle.Windows7)
			{
				num -= 2;
			}
			else if (!ষ && StyleManager.IsMetro(_൨))
			{
				num -= 2;
			}
			return num;
		}
		return _0D76;
	}

	internal int ක()
	{
		return Dpi.Height(_0D99() + ((!_0D73) ? _0D72 : 0)) + Dpi.CaptionVerticalPadding;
	}

	internal int ඛ()
	{
		return Dpi.Height(_0D99() + _0D72 + ((_൨ == eDotNetBarStyle.Office2010 || _൨ == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(_൨)) ? 0 : 0));
	}

	internal int ග()
	{
		if (_൨ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_൨))
		{
			return ((Control)this).Height;
		}
		return ඛ();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		ඕ();
		base.OnPaint(e);
	}

	protected override void RecalcSize()
	{
		ඕ();
		base.RecalcSize();
	}

	public override int GetAutoSizeHeight()
	{
		int num = base.GetAutoSizeHeight();
		if (BarFunctions.IsOffice2007Style(_൨))
		{
			BaseRenderer renderer = GetRenderer();
			if (renderer is Office2007Renderer)
			{
				num += ((Office2007Renderer)renderer).ColorTable.RibbonControl.CornerSize;
			}
		}
		return num;
	}

	protected override bool _੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		displayRectangle.Location = ((Control)this).PointToScreen(displayRectangle.Location);
		RibbonControl ribbonControl = ((Control)this).Parent as RibbonControl;
		if (((Control)this).Parent is RibbonControl)
		{
			displayRectangle = ((Control)this).Parent.DisplayRectangle;
			displayRectangle.Location = ((Control)this).Parent.PointToScreen(displayRectangle.Location);
			if (ribbonControl.SelectedRibbonTabItem != null && ribbonControl.SelectedRibbonTabItem.Panel != null && ((IKeyTipsControl)ribbonControl.SelectedRibbonTabItem.Panel).ShowKeyTips)
			{
				return false;
			}
		}
		if (ribbonControl != null && !ribbonControl.MouseWheelTabScrollEnabled)
		{
			return false;
		}
		Point mousePosition = Control.MousePosition;
		bool flag = true;
		if (ribbonControl != null)
		{
			flag = ribbonControl.Expanded;
		}
		bool flag2 = true;
		Form val = ((Control)this).FindForm();
		if (val != null && !BarFunctions.IsFormActive(val))
		{
			flag2 = false;
		}
		if (flag2 && displayRectangle.Contains(mousePosition) && !ShowKeyTips && flag && Items.Count > 0)
		{
			IntPtr intPtr = _089F.WindowFromPoint(new _089F.POINT(mousePosition));
			Control val2 = Control.FromChildHandle(intPtr);
			if (val2 == null)
			{
				val2 = Control.FromHandle(intPtr);
			}
			if (val2 is RibbonBar || val2 is RibbonStrip || val2 is RibbonControl || val2 is RibbonPanel)
			{
				RibbonTabItem selectedRibbonTabItem = SelectedRibbonTabItem;
				int num = 0;
				int num2 = Items.Count - 1;
				int num3 = 1;
				if ((_੦.ToInt64() & 0x80000000u) > 0)
				{
					num3 = -1;
					num2 = 0;
				}
				if (selectedRibbonTabItem != null)
				{
					num = Items.IndexOf(selectedRibbonTabItem) + num3;
					if (num3 < 0 && num < 0)
					{
						return false;
					}
					if (num == Items.Count)
					{
						num = 0;
					}
					else if (num < 0)
					{
						num = Items.Count - 1;
					}
				}
				int num4 = num - num3;
				do
				{
					num4 += num3;
					if (num4 < 0 || num4 > Items.Count - 1)
					{
						break;
					}
					if (Items[num4] is RibbonTabItem && Items[num4].Visible && Items[num4].Enabled)
					{
						((RibbonTabItem)Items[num4]).Checked = true;
						return true;
					}
				}
				while (num4 != num2);
			}
		}
		return false;
	}

	private ArrayList ඝ()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		ArrayList arrayList = new ArrayList();
		if (!(((Control)this).Parent is RibbonControl))
		{
			return arrayList;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			ඝ(val, arrayList);
		}
		RibbonControl ribbonControl = ((Control)this).Parent as RibbonControl;
		if (!ribbonControl.Expanded && SelectedRibbonTabItem != null && SelectedRibbonTabItem.Panel != null && ((Control)SelectedRibbonTabItem.Panel).Parent != ribbonControl)
		{
			ඝ((Control)(object)SelectedRibbonTabItem.Panel, arrayList);
		}
		return arrayList;
	}

	private void ඝ(Control _0BDE, ArrayList ඞ)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (_0BDE is RibbonBar)
		{
			ඞ.Add(_0BDE);
		}
		foreach (Control item in (ArrangedElementCollection)_0BDE.Controls)
		{
			Control val = item;
			ඝ(val, ඞ);
		}
	}

	public override ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(GetBaseItemContainer(), ItemName, arrayList);
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			BarFunctions.GetSubItemsByName(ribbonControl.Ꮮ.GetBaseItemContainer(), ItemName, arrayList);
		}
		ArrayList arrayList2 = ඝ();
		foreach (RibbonBar item in arrayList2)
		{
			BarFunctions.GetSubItemsByName(item.GetBaseItemContainer(), ItemName, arrayList);
		}
		if (ribbonControl != null && ribbonControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByName(ribbonControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType);
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			BarFunctions.GetSubItemsByNameAndType(ribbonControl.Ꮮ.GetBaseItemContainer(), ItemName, arrayList, itemType);
		}
		ArrayList arrayList2 = ඝ();
		foreach (RibbonBar item in arrayList2)
		{
			BarFunctions.GetSubItemsByNameAndType(item.GetBaseItemContainer(), ItemName, arrayList, itemType);
		}
		if (ribbonControl != null && ribbonControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(ribbonControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			BarFunctions.GetSubItemsByNameAndType(ribbonControl.Ꮮ.GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		}
		ArrayList arrayList2 = ඝ();
		foreach (RibbonBar item in arrayList2)
		{
			BarFunctions.GetSubItemsByNameAndType(item.GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		}
		if (ribbonControl != null && ribbonControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(ribbonControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType, useGlobalName);
		}
		return arrayList;
	}

	public override BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(GetBaseItemContainer(), ItemName);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			subItemByName = BarFunctions.GetSubItemByName(ribbonControl.Ꮮ.GetBaseItemContainer(), ItemName);
			if (subItemByName != null)
			{
				return subItemByName;
			}
		}
		ArrayList arrayList = ඝ();
		foreach (RibbonBar item in arrayList)
		{
			subItemByName = ((!item.OverflowState || item.ჽ == null) ? BarFunctions.GetSubItemByName(item.GetBaseItemContainer(), ItemName) : BarFunctions.GetSubItemByName(item.ჽ.GetBaseItemContainer(), ItemName));
			if (subItemByName != null)
			{
				return subItemByName;
			}
		}
		if (ribbonControl != null && ribbonControl.GlobalContextMenuBar != null)
		{
			return BarFunctions.GetSubItemByName(ribbonControl.GlobalContextMenuBar.ItemsContainer, ItemName);
		}
		return null;
	}

	internal void ඟ(RibbonBar ච)
	{
		if (!ඈ)
		{
			ඈ = true;
		}
		if (ඉ != ((Control)ච).Parent && ((Control)ච).Parent is RibbonPanel)
		{
			if (ඉ != null)
			{
				ඉ.ShowKeyTips = false;
			}
			ඉ = ((Control)ච).Parent as IKeyTipsControl;
			if (ඉ != null)
			{
				ඉ.ShowKeyTips = true;
			}
		}
	}

	protected override void OnShowKeyTipsChanged()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			ribbonControl.Ꮮ.ShowKeyTips = ShowKeyTips;
		}
		base.OnShowKeyTipsChanged();
		if (ShowKeyTips && !ඈ)
		{
			ඥ();
		}
	}

	internal void ඡ(SuperTabControl ජ)
	{
		if (ඉ == ජ || ඉ == ජ.TabStrip)
		{
			ඉ = null;
		}
	}

	protected override bool _੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Invalid comparison between Unknown and I4
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected I4, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Invalid comparison between Unknown and I4
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Invalid comparison between Unknown and I4
		int num = _0EFE._0F11(_੦);
		if (num == 27)
		{
			GetRibbonControl()?._᱖();
		}
		if (ඈ)
		{
			if (base.HasRegisteredPopups)
			{
				base._੪(_0A65, _੦, _੧);
				if (!base.HasRegisteredPopups)
				{
					ඦ();
				}
			}
			Keys val = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
			if ((int)val == 0)
			{
				val = (Keys)num;
			}
			if ((int)val == 27)
			{
				bool flag = false;
				if (!ShowKeyTips && ඉ != null && ඉ is RibbonPanel)
				{
					flag = true;
				}
				ඦ();
				if (flag)
				{
					ඥ();
				}
				return base._੪(_0A65, _੦, _੧);
			}
			char c = ඣ((int)val);
			if (c == '\0')
			{
				ඦ();
				return true;
			}
			if ((int)val == 32 || (int)val == 40 || (int)val == 39 || (int)val == 9)
			{
				ඦ();
				return false;
			}
			if (((Control)this).ProcessMnemonic(c))
			{
				return true;
			}
			return true;
		}
		if (num == 112 && (int)Control.ModifierKeys == 131072)
		{
			Form f = ((Control)this).FindForm();
			if (BarFunctions.IsFormActive(f))
			{
				RibbonControl ribbonControl = GetRibbonControl();
				if (ribbonControl.AutoKeyboardExpand)
				{
					ribbonControl.Expanded = !ribbonControl.Expanded;
				}
			}
		}
		return base._੪(_0A65, _੦, _੧);
	}

	private char ඣ(int _੬)
	{
		char[] array = new char[1];
		byte[] array2 = new byte[1];
		try
		{
			array2[0] = Convert.ToByte(_੬);
			Encoding.Default.GetDecoder().GetChars(array2, 0, 1, array, 0);
		}
		catch (Exception)
		{
			return '\0';
		}
		return array[0];
	}

	protected override bool ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		int num = _0EFE._0F11(_੦);
		if (num == 18 || (num == 121 && !_ඊ))
		{
			bool flag = true;
			if (ඈ)
			{
				if (num == 18 || (num == 121 && !_ඊ))
				{
					flag = false;
				}
				ඦ();
			}
			else
			{
				Form val = ((Control)this).FindForm();
				if (val == Form.ActiveForm)
				{
					flag = false;
					ඤ();
				}
			}
			if (!flag)
			{
				return true;
			}
		}
		return base.ੳ(_0A65, _੦, _੧);
	}

	private void ඤ()
	{
		if (ඩ() is ApplicationButton { BackstageTab: not null, Expanded: not false } applicationButton)
		{
			if (ඉ != null)
			{
				ඉ.ShowKeyTips = false;
			}
			ඉ = applicationButton.BackstageTab.TabStrip;
			if (!ඉ.ShowKeyTips)
			{
				ඉ.ShowKeyTips = true;
			}
		}
		else
		{
			ඥ();
		}
	}

	private void ඥ()
	{
		if (_0D80)
		{
			ඈ = true;
			ShowKeyTips = true;
			SetupActiveWindowTimer();
		}
	}

	private void ඦ()
	{
		if (ඈ)
		{
			ReleaseActiveWindowTimer();
			if (!ShowKeyTips && ඉ != null)
			{
				ඉ.ShowKeyTips = false;
				ඉ = null;
			}
			ShowKeyTips = false;
		}
		ඈ = false;
	}

	public void ExitKeyTipsMode()
	{
		ඦ();
	}

	internal void ට(BaseItem _06EC)
	{
		if (ShowKeyTips || (ඉ != null && ඉ.ShowKeyTips))
		{
			ShowKeyTips = false;
			ඦ();
		}
	}

	protected override bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (ShowKeyTips || (ඉ != null && ඉ.ShowKeyTips))
		{
			ShowKeyTips = false;
			ඦ();
		}
		GetRibbonControl()?._᱗(hWnd, wParam, lParam);
		return base.OnSysMouseDown(hWnd, wParam, lParam);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		if (ඉ != null)
		{
			if (ඉ.ProcessMnemonicEx(charCode))
			{
				ඦ();
				return true;
			}
			return false;
		}
		if (!ඈ && (Control.ModifierKeys & 0x40000) != 262144)
		{
			return false;
		}
		return base.ProcessMnemonic(charCode);
	}

	public override bool ProcessMnemonicEx(char charCode)
	{
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		BaseItem baseItem = null;
		if (_0D77)
		{
			baseItem = GetItemForMnemonic(_0D70.CaptionContainer, charCode, deepScan: false, stackKeys: true);
			if (baseItem != null && ProcessItemMnemonicKey(baseItem))
			{
				ඦ();
				return true;
			}
		}
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon && ribbonControl.Ꮮ.ProcessMnemonicEx(charCode))
		{
			ඦ();
			return true;
		}
		if (baseItem == null)
		{
			baseItem = GetItemForMnemonic(GetBaseItemContainer(), charCode, deepScan: true, stackKeys: true);
		}
		if (baseItem is RibbonTabItem && baseItem.Visible)
		{
			if (!ඈ)
			{
				ඥ();
			}
			ShowKeyTips = false;
			if (baseItem != SelectedRibbonTabItem)
			{
				baseItem.RaiseClick();
			}
			else if (SelectedRibbonTabItem != null && ribbonControl != null && !ribbonControl.Expanded && !ribbonControl.IsPopupMode)
			{
				ribbonControl.ᱚ(SelectedRibbonTabItem);
			}
			if (SelectedRibbonTabItem != null && _0D80)
			{
				if (ඉ != null)
				{
					ඉ.ShowKeyTips = false;
				}
				ඉ = SelectedRibbonTabItem.Panel;
				if (ඉ != null)
				{
					ඉ.ShowKeyTips = true;
				}
			}
			return true;
		}
		if (baseItem != null && ProcessItemMnemonicKey(baseItem))
		{
			ShowKeyTips = false;
			if (baseItem is ApplicationButton)
			{
				ApplicationButton applicationButton = (ApplicationButton)baseItem;
				if (applicationButton.BackstageTab != null)
				{
					if (ඉ != null)
					{
						ඉ.ShowKeyTips = false;
					}
					ඉ = applicationButton.BackstageTab.TabStrip;
					if (!ඉ.ShowKeyTips)
					{
						ඉ.ShowKeyTips = true;
					}
				}
			}
			return true;
		}
		if (ඈ && SelectedRibbonTabItem != null && SelectedRibbonTabItem.Panel != null)
		{
			Control panel = (Control)(object)SelectedRibbonTabItem.Panel;
			foreach (Control item in (ArrangedElementCollection)panel.Controls)
			{
				Control val = item;
				if (val is RibbonBar && val.Visible && Control.IsMnemonic(charCode, val.Text))
				{
					ShowKeyTips = false;
					((RibbonBar)(object)val).ShowKeyTips = true;
					if (ඉ != null)
					{
						ඉ.ShowKeyTips = false;
					}
					ඉ = (IKeyTipsControl)val;
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnActiveWindowChanged()
	{
		ඦ();
		base.OnActiveWindowChanged();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return ((ContainerControl)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return base.ProcessDialogKey(keyData);
	}

	protected override void OnPopupItemRightClick(BaseItem item)
	{
		GetRibbonControl()?.ᐆ(item, ᱩ: false);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_0D84 != null)
		{
			_0D84.ભ((Control)(object)this);
		}
		base.OnMouseLeave(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Invalid comparison between Unknown and I4
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Invalid comparison between Unknown and I4
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		MethodInvoker val = null;
		උ = false;
		if (_0D77)
		{
			උ = HitTestCaption(new Point(e.X, e.Y));
			if ((int)e.Button == 2097152 && උ)
			{
				Form val2 = ((Control)this).FindForm();
				if (val2 != null)
				{
					Point mousePosition = Control.MousePosition;
					byte[] bytes = BitConverter.GetBytes(mousePosition.X);
					byte[] bytes2 = BitConverter.GetBytes(mousePosition.Y);
					byte[] value = new byte[4]
					{
						bytes[0],
						bytes[1],
						bytes2[0],
						bytes2[1]
					};
					int lParam = BitConverter.ToInt32(value, 0);
					((Control)this).Capture = false;
					_089F.SendMessage(((Control)val2).Handle, 274, _089F.TrackPopupMenu(_089F.GetSystemMenu(((Control)val2).Handle, bRevert: false), 256u, Control.MousePosition.X, Control.MousePosition.Y, 0, ((Control)val2).Handle, IntPtr.Zero), lParam);
					if (val == null)
					{
						val = (MethodInvoker)delegate
						{
							උ = false;
						};
					}
					BarUtilities.InvokeDelayed(val, 500);
					return;
				}
			}
			if (_0D84 != null)
			{
				_0D84.ग़((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		if ((int)e.Button == 2097152 && QuickToolbarBounds.Contains(e.X, e.Y))
		{
			GetRibbonControl()?.ᱨ(this, e.X, e.Y);
		}
		else if ((int)e.Button == 1048576 && _0D77 && _0D73 && !_0D70.RibbonStripContainer.DisplayRectangle.Contains(e.X, e.Y))
		{
			foreach (RibbonTabItemGroup tabGroup in TabGroups)
			{
				if (!tabGroup.Visible)
				{
					continue;
				}
				foreach (Rectangle displayPosition in tabGroup.DisplayPositions)
				{
					if (displayPosition.Contains(e.X, e.Y))
					{
						if (!tabGroup.IsTabFromGroupSelected)
						{
							tabGroup.SelectFirstTab();
							return;
						}
						break;
					}
				}
			}
		}
		base.OnMouseDown(e);
	}

	private RibbonTabItemGroup ඨ(Point _065C)
	{
		if (_0D77 && _0D73 && !_0D70.RibbonStripContainer.DisplayRectangle.Contains(_065C.X, _065C.Y))
		{
			foreach (RibbonTabItemGroup tabGroup in TabGroups)
			{
				if (!tabGroup.Visible)
				{
					continue;
				}
				foreach (Rectangle displayPosition in tabGroup.DisplayPositions)
				{
					if (displayPosition.Contains(_065C.X, _065C.Y))
					{
						return tabGroup;
					}
				}
			}
		}
		return null;
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_0D77)
		{
			if (_0D84 != null)
			{
				_0D84.ફ((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		උ = false;
		base.OnMouseUp(e);
	}

	internal BaseItem ඩ()
	{
		if (!CaptionVisible)
		{
			return null;
		}
		BaseItem captionContainerItem = CaptionContainerItem;
		if ((_൨ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_൨)) && Items.Count > 0)
		{
			foreach (BaseItem item in Items)
			{
				if (item is ApplicationButton)
				{
					return item;
				}
			}
		}
		if (captionContainerItem.SubItems.Count > 0 && captionContainerItem.SubItems[0] is ApplicationButton)
		{
			return captionContainerItem.SubItems[0];
		}
		if (captionContainerItem.SubItems.Count > 0 && captionContainerItem.SubItems[0] is ButtonItem && ((ButtonItem)captionContainerItem.SubItems[0]).HotTrackingStyle == eHotTrackingStyle.Image)
		{
			return captionContainerItem.SubItems[0];
		}
		return null;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (_0D77)
		{
			if ((int)e.Button == 1048576 && උ && HitTestCaption(new Point(e.X, e.Y)))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && ((int)val.WindowState == 0 || BarFunctions.IsWindows7))
				{
					if (ඩ() is PopupItem { Expanded: not false } popupItem)
					{
						popupItem.Expanded = false;
					}
					Point mousePosition = Control.MousePosition;
					byte[] bytes = BitConverter.GetBytes(mousePosition.X);
					byte[] bytes2 = BitConverter.GetBytes(mousePosition.Y);
					byte[] value = new byte[4]
					{
						bytes[0],
						bytes[1],
						bytes2[0],
						bytes2[1]
					};
					int lParam = BitConverter.ToInt32(value, 0);
					((Control)this).Capture = false;
					_089F.SendMessage(((Control)val).Handle, 274, 61458, lParam);
					උ = false;
					return;
				}
			}
			if (_0D84 != null)
			{
				_0D84.મ((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		base.OnMouseMove(e);
	}

	protected override void InternalOnClick(MouseButtons mb, Point mousePos)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (_0D77)
		{
			MouseEventArgs ؿ = new MouseEventArgs(mb, 0, mousePos.X, mousePos.Y, 0);
			ؿ = ඪ(ؿ);
			mousePos = new Point(ؿ.X, ؿ.Y);
		}
		base.InternalOnClick(mb, mousePos);
	}

	private MouseEventArgs ඪ(MouseEventArgs ؿ)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		if (ؿ.Y <= 6)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && (int)val.WindowState == 2 && val is RibbonForm)
			{
				if (ؿ.X > 4)
				{
					ؿ = ((ؿ.X < ((Control)this).Width - 6) ? new MouseEventArgs(ؿ.Button, ؿ.Clicks, ؿ.X, CaptionContainerItem.TopInternal + 5, ؿ.Delta) : new MouseEventArgs(ؿ.Button, ؿ.Clicks, _൧.DisplayRectangle.Right - 4, _൧.DisplayRectangle.Top + 4, ؿ.Delta));
				}
				else
				{
					BaseItem baseItem = ඩ();
					if (baseItem != null)
					{
						ؿ = new MouseEventArgs(ؿ.Button, ؿ.Clicks, baseItem.LeftInternal + 1, baseItem.TopInternal + 1, ؿ.Delta);
					}
				}
			}
		}
		return ؿ;
	}

	protected override void OnClick(EventArgs e)
	{
		if (_0D77 && _0D84 != null)
		{
			_0D84.દ((Control)(object)this);
		}
		base.OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		if (_0D77)
		{
			Point p = ((Control)this).PointToClient(Control.MousePosition);
			if (HitTestCaption(p))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && val.MaximizeBox && ((int)val.FormBorderStyle == 4 || (int)val.FormBorderStyle == 6))
				{
					if ((int)val.WindowState == 0)
					{
						_089F.PostMessage(((Control)val).Handle, 274, 61488, 0);
					}
					else if ((int)val.WindowState == 2)
					{
						_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
					}
				}
			}
		}
		base.OnDoubleClick(e);
	}

	protected bool HitTestCaption(Point p)
	{
		if (ൺ.Contains(p) && ൽ != null)
		{
			Rectangle[] array = ൽ;
			foreach (Rectangle rectangle in array)
			{
				if (rectangle.Contains(p))
				{
					return true;
				}
			}
		}
		if (BarFunctions.IsOffice2010Style(_൨) && ((Control)this).ClientRectangle.Contains(p) && HitTest(p.X, p.Y) == null && ඨ(p) == null)
		{
			return true;
		}
		return false;
	}

	private void ණ(object _٠, EventArgs ؿ)
	{
		SystemCaptionItem systemCaptionItem = _٠ as SystemCaptionItem;
		Form val = ((Control)this).FindForm();
		if (val != null && systemCaptionItem._1C7E == systemCaptionItem.ᲀ)
		{
			if (systemCaptionItem._1C7E == _1886._1C8C)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61472, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8F)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61488, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8D)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
			}
			else if (systemCaptionItem._1C7E == _1886.ᆤ)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61536, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8E)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61824, 0);
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		base.OnResize(e);
		if (!_0D77)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			if ((int)val.WindowState == 2 || (int)val.WindowState == 1)
			{
				_0D70.SystemCaptionItem.RestoreEnabled = true;
			}
			else
			{
				_0D70.SystemCaptionItem.RestoreEnabled = false;
			}
		}
	}

	protected override void છ(Graphics _٥)
	{
		if (ShowKeyTips)
		{
			KeyTipsRendererEventArgs ؿ = new KeyTipsRendererEventArgs(_٥, Rectangle.Empty, "", GetKeyTipFont(), null);
			BaseRenderer renderer = GetRenderer();
			જ(_0D70.RibbonStripContainer, renderer, ؿ);
			if (_0D77)
			{
				જ(_0D70.CaptionContainer, renderer, ؿ);
			}
		}
	}

	protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Rectangle keyTipRectangle = base.GetKeyTipRectangle(g, item, font, keyTip);
		if (QuickToolbarItems.Contains(item))
		{
			keyTipRectangle.Y += 4;
		}
		return keyTipRectangle;
	}

	internal void ర(bool ఱ)
	{
		if (_0D70.RibbonStripContainer == null)
		{
			return;
		}
		bool flag = false;
		try
		{
			if (Items.Contains("dotnetbarsysiconitem"))
			{
				Items.Remove("dotnetbarsysiconitem");
				flag = true;
			}
			if (Items.Contains("dotnetbarsysmenuitem"))
			{
				Items.Remove("dotnetbarsysmenuitem");
				flag = true;
			}
			if (ఱ && flag)
			{
				RecalcLayout();
			}
		}
		catch (Exception)
		{
		}
	}

	internal void ల(Form ళ, bool ఱ)
	{
		ర(ఱ);
		if (ళ != null)
		{
			MDISystemItem mDISystemItem = new MDISystemItem("dotnetbarsysmenuitem");
			if (!ళ.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!ళ.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			if (!ళ.MaximizeBox)
			{
				mDISystemItem.RestoreEnabled = false;
			}
			mDISystemItem.ItemAlignment = eItemAlignment.Far;
			mDISystemItem.Click += ఴ;
			Items.Add(mDISystemItem);
			if (ఱ)
			{
				RecalcLayout();
			}
		}
	}

	private void ఴ(object _٠, EventArgs ؿ)
	{
		MDISystemItem mDISystemItem = _٠ as MDISystemItem;
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val = val.ActiveMdiChild;
		}
		if (val == null)
		{
			ర(ఱ: true);
		}
		else if (mDISystemItem._1C7E == _1886._1C8C)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61472, 0);
		}
		else if (mDISystemItem._1C7E == _1886._1C8D)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
		}
		else if (mDISystemItem._1C7E == _1886.ᆤ)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61536, 0);
		}
		else if (mDISystemItem._1C7E == _1886.Ა)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61504, 0);
		}
	}

	internal void ඬ(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseDown(ؿ);
	}

	internal void ත(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseMove(ؿ);
	}

	internal void ථ(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseUp(ؿ);
	}

	internal void ද(EventArgs ؿ)
	{
		((Control)this).OnClick(ؿ);
	}

	[CompilerGenerated]
	private void ධ()
	{
		උ = false;
	}
}
