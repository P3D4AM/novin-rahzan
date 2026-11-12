using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabItem : BaseItem
{
	private Icon _Icon;

	private Image _Image;

	private int _ImageIndex = -1;

	private Rectangle _ImageBounds;

	private eTabItemColor _PredefinedColor;

	private SuperTabItemColorTable _TabColor = new SuperTabItemColorTable();

	private bool _EnableMarkup = true;

	private eItemAlignment? _TextAlignment;

	private Rectangle _TextBounds;

	private Font _TabFont;

	private Font _SelectedTabFont;

	private bool _CloseButtonVisible = true;

	private bool _CloseButtonMouseOver;

	private bool _CloseButtonPressed;

	private Rectangle _CloseButtonBounds;

	private SuperTabStripItem _TabStripItem;

	private Control _AttachedControl;

	private SuperTabItemBaseDisplay _TabItemDisplay;

	private Font _DefaultSelectedTabFont;

	private bool _EnableImageAnimation;

	private bool _CanAnimateImage;

	private ImageAlignment _ImageAlignment = ImageAlignment.NotSet;

	private Padding _ImagePadding;

	private Size _FixedTabSize = Size.Empty;

	protected override bool IsMarkupSupported => _EnableMarkup;

	protected Font DefaultSelectedTabFont
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			if (_DefaultSelectedTabFont == null)
			{
				_DefaultSelectedTabFont = new Font(SystemFonts.CaptionFont, (FontStyle)1);
			}
			return _DefaultSelectedTabFont;
		}
		set
		{
			if (_DefaultSelectedTabFont != null)
			{
				_DefaultSelectedTabFont.Dispose();
			}
			_DefaultSelectedTabFont = value;
		}
	}

	internal bool CanAnimateImage
	{
		get
		{
			if (_EnableImageAnimation)
			{
				return _CanAnimateImage;
			}
			return false;
		}
	}

	internal Rectangle CloseButtonBounds
	{
		get
		{
			if (_CloseButtonBounds.IsEmpty)
			{
				_CloseButtonBounds = GetCloseButtonBounds();
			}
			return _CloseButtonBounds;
		}
	}

	internal bool CloseButtonMouseOver
	{
		get
		{
			return _CloseButtonMouseOver;
		}
		set
		{
			if (_CloseButtonMouseOver != value)
			{
				_CloseButtonMouseOver = value;
				Refresh();
			}
		}
	}

	internal bool CloseButtonPressed
	{
		get
		{
			return _CloseButtonPressed;
		}
		set
		{
			if (_CloseButtonPressed != value)
			{
				_CloseButtonPressed = value;
				Refresh();
			}
		}
	}

	internal ImageAlignment EffectiveImageAlignment
	{
		get
		{
			if (ImageAlignment != ImageAlignment.NotSet)
			{
				return ImageAlignment;
			}
			if (TabStripItem.ImageAlignment != ImageAlignment.NotSet)
			{
				return TabStripItem.ImageAlignment;
			}
			return ImageAlignment.MiddleLeft;
		}
	}

	internal Rectangle ImageBounds
	{
		get
		{
			if (_ImageBounds.IsEmpty)
			{
				_ImageBounds = GetImageBounds();
			}
			return _ImageBounds;
		}
	}

	internal bool IsVertical
	{
		get
		{
			if (!_TabStripItem.HorizontalText)
			{
				if (_TabStripItem.TabAlignment != 0)
				{
					return _TabStripItem.TabAlignment == eTabStripAlignment.Right;
				}
				return true;
			}
			return false;
		}
	}

	internal SuperTabItemBaseDisplay TabItemDisplay => _TabItemDisplay;

	internal SuperTabStrip TabStrip
	{
		get
		{
			if (TabStripItem == null)
			{
				return null;
			}
			return TabStripItem.TabStrip;
		}
	}

	internal Rectangle TextBounds
	{
		get
		{
			if (_TextBounds.IsEmpty)
			{
				_TextBounds = GetTextBounds();
			}
			return _TextBounds;
		}
	}

	public bool EndGroup { get; set; }

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether image animation is enabled.")]
	public bool EnableImageAnimation
	{
		get
		{
			return _EnableImageAnimation;
		}
		set
		{
			if (_EnableImageAnimation != value)
			{
				_EnableImageAnimation = value;
				RefreshOwner();
			}
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Contains user specified tab display colors.")]
	public SuperTabItemColorTable TabColor
	{
		get
		{
			return _TabColor;
		}
		set
		{
			if (!_TabColor.Equals(value))
			{
				if (_TabColor != null)
				{
					_TabColor.ColorTableChanged -= ColorTableColorTableChanged;
				}
				_TabColor = value;
				if (value != null)
				{
					_TabColor.ColorTableChanged += ColorTableColorTableChanged;
				}
				OnTabColorChanged();
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Close button on the tab is visible when SuperTabStrip.CloseButtonOnTabsVisible property is set to true.")]
	public bool CloseButtonVisible
	{
		get
		{
			return _CloseButtonVisible;
		}
		set
		{
			if (_CloseButtonVisible != value)
			{
				_CloseButtonVisible = value;
				RefreshOwner();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the tab is enabled.")]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			if (base.Enabled != value)
			{
				base.Enabled = value;
				if (_TabStripItem != null && !value && IsSelected && !DesignMode)
				{
					_TabStripItem.SelectNewTab(this);
				}
				RefreshOwner();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for the control's Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _EnableMarkup;
		}
		set
		{
			if (_EnableMarkup != value)
			{
				_EnableMarkup = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	public Size FixedTabSize
	{
		get
		{
			return _FixedTabSize;
		}
		set
		{
			value.Width = Math.Max(0, value.Width);
			value.Height = Math.Max(0, value.Height);
			if (_FixedTabSize != value)
			{
				_FixedTabSize = value;
				RefreshOwner();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates the tab icon. Icon has same functionality as Image except that it supports Alpha blending.")]
	public Icon Icon
	{
		get
		{
			return _Icon;
		}
		set
		{
			if (_Icon != value)
			{
				_Icon = value;
				RefreshOwner();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates the tab image.")]
	public Image Image
	{
		get
		{
			return _Image;
		}
		set
		{
			if (_Image != value)
			{
				_Image = value;
				OnImageChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(-1)]
	[Category("Appearance")]
	[Description("Indicates the tab image index")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	public int ImageIndex
	{
		get
		{
			return _ImageIndex;
		}
		set
		{
			if (_ImageIndex != value)
			{
				_ImageIndex = value;
				OnImageChanged();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImageList ImageList
	{
		get
		{
			if (_TabStripItem != null && _TabStripItem.TabStrip != null)
			{
				return _TabStripItem.TabStrip.ImageList;
			}
			return null;
		}
	}

	[DefaultValue(ImageAlignment.NotSet)]
	[Category("Appearance")]
	[Description("Indicates the alignment of the Image within the tab.")]
	public ImageAlignment ImageAlignment
	{
		get
		{
			return _ImageAlignment;
		}
		set
		{
			if (_ImageAlignment != value)
			{
				_ImageAlignment = value;
				RefreshOwner();
			}
		}
	}

	[Description("Indicates the spacing between content and edges of the Image")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Padding ImagePadding
	{
		get
		{
			if (_ImagePadding == null)
			{
				_ImagePadding = new Padding(0);
				UpdateChangeHandler(null, _ImagePadding);
			}
			return _ImagePadding;
		}
		set
		{
			if (_ImagePadding != value)
			{
				UpdateChangeHandler(_ImagePadding, value);
				_ImagePadding = value;
				RefreshOwner();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eTabItemColor.Default)]
	[Category("Style")]
	[Description("Indicates the predefined color for the tab.")]
	public eTabItemColor PredefinedColor
	{
		get
		{
			return _PredefinedColor;
		}
		set
		{
			_PredefinedColor = value;
			OnTabColorChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(null)]
	[Description("Indicates the selected tab Font")]
	public Font SelectedTabFont
	{
		get
		{
			return _SelectedTabFont;
		}
		set
		{
			if (_SelectedTabFont != value)
			{
				if (value != null)
				{
					DefaultSelectedTabFont = null;
				}
				_SelectedTabFont = value;
				RefreshOwner();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates text alignment.")]
	public eItemAlignment? TextAlignment
	{
		get
		{
			return _TextAlignment;
		}
		set
		{
			if (_TextAlignment != value)
			{
				_TextAlignment = value;
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(null)]
	[Description("Indicates the tab Font")]
	public Font TabFont
	{
		get
		{
			return _TabFont;
		}
		set
		{
			if (_TabFont != value)
			{
				_TabFont = value;
				RefreshOwner();
			}
		}
	}

	[Browsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("Indicates the tab text.")]
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

	[Browsable(false)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Behavior")]
	public Control AttachedControl
	{
		get
		{
			return _AttachedControl;
		}
		set
		{
			if (_AttachedControl is SuperTabControlPanel superTabControlPanel)
			{
				superTabControlPanel.PanelColorChanged -= ColorTableColorTableChanged;
			}
			_AttachedControl = value;
			if (!(_AttachedControl is SuperTabControlPanel superTabControlPanel2))
			{
				return;
			}
			superTabControlPanel2.TabItem = this;
			((Control)superTabControlPanel2).Dock = (DockStyle)5;
			superTabControlPanel2.PanelColorChanged += ColorTableColorTableChanged;
			if (_TabStripItem != null && _TabStripItem.TabStrip != null)
			{
				if (DesignMode)
				{
					((Control)_TabStripItem.TabStrip).Controls.Add((Control)(object)superTabControlPanel2);
				}
				((Control)superTabControlPanel2).BringToFront();
				((Control)superTabControlPanel2).Visible = _TabStripItem.SelectedTab == this;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			base.Bounds = value;
			_TextBounds = Rectangle.Empty;
			_CloseButtonBounds = Rectangle.Empty;
			_ImageBounds = Rectangle.Empty;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseOver
	{
		get
		{
			if (_TabStripItem != null)
			{
				return _TabStripItem.MouseOverTab == this;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsSelected
	{
		get
		{
			if (_TabStripItem != null)
			{
				return _TabStripItem.SelectedTab == this;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			if (_TabStripItem != null)
			{
				return _TabStripItem.TabAlignment;
			}
			return eTabStripAlignment.Top;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle ContentRectangle
	{
		get
		{
			if (_TabItemDisplay != null)
			{
				return _TabItemDisplay.GetContentRectangle();
			}
			return Rectangle.Empty;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SuperTabStripItem TabStripItem
	{
		get
		{
			return _TabStripItem;
		}
		set
		{
			_TabStripItem = value;
			SetOwner(value);
			if (_TabStripItem != null)
			{
				TabStyle = _TabStripItem.TabStyle;
				ContainerControl = _TabStripItem.ContainerControl;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eSuperTabStyle TabStyle
	{
		get
		{
			if (_TabStripItem != null)
			{
				return _TabStripItem.TabStyle;
			}
			return eSuperTabStyle.Office2007;
		}
		internal set
		{
			if (_TabItemDisplay != null)
			{
				_TabItemDisplay.Dispose();
			}
			switch (value)
			{
			case eSuperTabStyle.Office2007:
				_TabItemDisplay = new Office2007SuperTabItem(this);
				break;
			case eSuperTabStyle.Office2010BackstageBlue:
				_TabItemDisplay = new Office2010BackstageSuperTabItem(this);
				break;
			case eSuperTabStyle.OneNote2007:
				_TabItemDisplay = new OneNote2007SuperTabItem(this);
				break;
			case eSuperTabStyle.VisualStudio2008Dock:
				_TabItemDisplay = new VS2008DockSuperTabItem(this);
				break;
			case eSuperTabStyle.VisualStudio2008Document:
				_TabItemDisplay = new VS2008DocumentSuperTabItem(this);
				break;
			case eSuperTabStyle.WinMediaPlayer12:
				_TabItemDisplay = new WinMediaPlayer12SuperTabItem(this);
				break;
			}
		}
	}

	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			base.Visible = value;
			if (_TabStripItem != null)
			{
				if (!value && IsSelected && !DesignMode)
				{
					_TabStripItem.SelectNewTab(this);
				}
				RefreshOwner();
			}
		}
	}

	[Description("Occurs when text markup link is clicked.")]
	public event MarkupLinkClickEventHandler MarkupLinkClick;

	[Description("Occurs when the tab colors have changed.")]
	public event EventHandler<EventArgs> TabColorChanged;

	public SuperTabItem()
	{
		Visible = true;
		GlobalItem = false;
		Stretch = false;
		Displayed = true;
		HookEvents(hook: true);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabColor()
	{
		return !_TabColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabColor()
	{
		TabColor = new SuperTabItemColorTable();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return !_FixedTabSize.IsEmpty;
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetIcon()
	{
		Icon = null;
	}

	private void OnImageChanged()
	{
		if (_EnableImageAnimation && _TabItemDisplay != null)
		{
			_TabItemDisplay.StopImageAnimation();
		}
		Image image = GetImage();
		_CanAnimateImage = ImageAnimator.CanAnimate(image);
		RefreshOwner();
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageIndex()
	{
		ImageIndex = -1;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private bool ShouldSerializeImagePadding()
	{
		if (_ImagePadding != null)
		{
			return !_ImagePadding.IsEmpty;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetImagePadding()
	{
		ImagePadding = null;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedTabFont()
	{
		return _SelectedTabFont != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		SelectedTabFont = null;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabFont()
	{
		return _TabFont != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabFont()
	{
		TabFont = null;
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			base.TextChanged += SuperTabItemTextChanged;
			MarkupLinkClick += TextMarkupLinkClick;
			_TabColor.ColorTableChanged += ColorTableColorTableChanged;
			return;
		}
		base.TextChanged -= SuperTabItemTextChanged;
		MarkupLinkClick -= TextMarkupLinkClick;
		if (_TabColor != null)
		{
			_TabColor.ColorTableChanged -= ColorTableColorTableChanged;
		}
	}

	private void SuperTabItemTextChanged(object sender, EventArgs e)
	{
		NeedRecalcSize = true;
		Refresh();
		if (_TabStripItem != null)
		{
			_TabStripItem.NeedRecalcSize = true;
			_TabStripItem.Refresh();
		}
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (this.MarkupLinkClick != null)
		{
			this.MarkupLinkClick(this, e);
		}
	}

	private void ColorTableColorTableChanged(object sender, EventArgs e)
	{
		OnTabColorChanged();
	}

	protected void OnTabColorChanged()
	{
		if (this.TabColorChanged != null)
		{
			this.TabColorChanged(this, EventArgs.Empty);
		}
		Refresh();
	}

	public void Close()
	{
		_TabStripItem.CloseTab(this);
	}

	public Font GetTabFont()
	{
		object obj = _TabFont ?? _TabStripItem.TabFont;
		if (obj == null)
		{
			obj = SystemFonts.CaptionFont;
		}
		Font result = (Font)obj;
		if (IsSelected)
		{
			object obj2 = _SelectedTabFont ?? _TabStripItem.SelectedTabFont;
			if (obj2 == null)
			{
				obj2 = DefaultSelectedTabFont;
			}
			result = (Font)obj2;
		}
		return result;
	}

	internal SuperTabItemStateColorTable GetTabColorTable()
	{
		return _TabItemDisplay.GetTabColorTable();
	}

	internal SuperTabItemStateColorTable GetTabColorTable(eTabState tabState)
	{
		return _TabItemDisplay.GetTabColorTable(tabState);
	}

	internal SuperTabPanelItemColorTable GetPanelColorTable()
	{
		return _TabItemDisplay.GetPanelColorTable();
	}

	public eSuperTabArea GetTabAreaFromPoint(Point pt)
	{
		if (Bounds.Contains(pt))
		{
			if (CloseButtonBounds.Contains(pt))
			{
				return eSuperTabArea.InCloseBox;
			}
			if (ImageBounds.Contains(pt))
			{
				return eSuperTabArea.InImage;
			}
			return eSuperTabArea.InContent;
		}
		return eSuperTabArea.InNone;
	}

	public bool PointInTab(Point pt)
	{
		GraphicsPath tabItemPath = GetTabItemPath();
		try
		{
			if (tabItemPath.IsVisible(pt))
			{
				return true;
			}
		}
		finally
		{
			((IDisposable)tabItemPath)?.Dispose();
		}
		return false;
	}

	public GraphicsPath GetTabItemPath()
	{
		if (_TabItemDisplay == null)
		{
			return null;
		}
		return _TabItemDisplay.TabItemPath();
	}

	private Rectangle GetCloseButtonBounds()
	{
		if (!CloseButtonVisible || _TabStripItem == null || !_TabStripItem.CloseButtonOnTabsVisible)
		{
			return Rectangle.Empty;
		}
		Rectangle contentRectangle = TabItemDisplay.GetContentRectangle();
		Rectangle rectangle = contentRectangle;
		Size tabCloseButtonSize = _TabStripItem.TabCloseButtonSize;
		int tabHorizontalSpacing = _TabStripItem.TabHorizontalSpacing;
		if (IsVertical)
		{
			contentRectangle = ((TabStripItem.CloseButtonPosition != 0) ? new Rectangle(contentRectangle.X + (contentRectangle.Width - tabCloseButtonSize.Width) / 2 + 1, contentRectangle.Y + tabHorizontalSpacing, tabCloseButtonSize.Width, tabCloseButtonSize.Height) : new Rectangle(contentRectangle.X + (contentRectangle.Width - tabCloseButtonSize.Width) / 2 + 1, contentRectangle.Bottom - tabHorizontalSpacing - tabCloseButtonSize.Height - 1, tabCloseButtonSize.Width, tabCloseButtonSize.Height));
			if (TabStripItem.RotateVerticalText ^ (TabStripItem.TabAlignment == eTabStripAlignment.Right))
			{
				contentRectangle.X = rectangle.X + (rectangle.Right - contentRectangle.Right);
				contentRectangle.Y = rectangle.Y + (rectangle.Bottom - contentRectangle.Bottom);
			}
		}
		else if (_TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Left)
		{
			contentRectangle = new Rectangle(contentRectangle.X + tabHorizontalSpacing, contentRectangle.Y + (contentRectangle.Height - tabCloseButtonSize.Height) / 2, tabCloseButtonSize.Width, tabCloseButtonSize.Height);
			if (_TabStripItem.TabAlignment == eTabStripAlignment.Right && IsSelected)
			{
				contentRectangle.X += _TabStripItem.TabDisplay.SelectedPaddingWidth;
			}
		}
		else
		{
			contentRectangle = new Rectangle(contentRectangle.Right - tabHorizontalSpacing - tabCloseButtonSize.Width, contentRectangle.Y + (contentRectangle.Height - tabCloseButtonSize.Height) / 2 + 1, tabCloseButtonSize.Width, tabCloseButtonSize.Height);
			if (_TabStripItem.TabAlignment == eTabStripAlignment.Left && IsSelected)
			{
				contentRectangle.X -= _TabStripItem.TabDisplay.SelectedPaddingWidth;
			}
		}
		return _TabStripItem.OnGetCloseBounds(this, contentRectangle);
	}

	private Rectangle GetImageBounds()
	{
		CompositeImage tabImage = GetTabImage();
		if (tabImage == null)
		{
			return Rectangle.Empty;
		}
		Rectangle contentRectangle = TabItemDisplay.GetContentRectangle();
		contentRectangle = ProcessCloseBounds(contentRectangle);
		contentRectangle = (IsVertical ? CalcVerticalImageBounds(tabImage, contentRectangle) : CalcHorizontalImageBounds(tabImage, contentRectangle));
		return _TabStripItem.OnGetImageBounds(this, contentRectangle);
	}

	private Rectangle CalcVerticalImageBounds(CompositeImage image, Rectangle r)
	{
		int height = image.Height;
		int width = image.Width;
		Padding imagePadding = ImagePadding;
		Rectangle rectangle = r;
		switch (EffectiveImageAlignment)
		{
		case ImageAlignment.TopLeft:
			r.X += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			r.Y = r.Bottom - (width + imagePadding.Left);
			break;
		case ImageAlignment.TopCenter:
			r.X += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			r.Y += (r.Height - (width + imagePadding.Horizontal - TabStripItem.TabHorizontalSpacing)) / 2;
			break;
		case ImageAlignment.TopRight:
			r.X += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			r.Y += imagePadding.Right + TabStripItem.TabHorizontalSpacing;
			break;
		case ImageAlignment.MiddleLeft:
			r.X += (r.Width - (height + imagePadding.Vertical)) / 2 + imagePadding.Top;
			r.Y = r.Bottom - (width + imagePadding.Left);
			break;
		case ImageAlignment.MiddleCenter:
			r.X += (r.Width - (height + imagePadding.Vertical)) / 2 + imagePadding.Top;
			r.Y += (r.Height - (width + imagePadding.Horizontal - TabStripItem.TabHorizontalSpacing)) / 2;
			break;
		case ImageAlignment.MiddleRight:
			r.X += (r.Width - (height + imagePadding.Vertical)) / 2 + imagePadding.Top;
			r.Y += imagePadding.Right + TabStripItem.TabHorizontalSpacing;
			break;
		case ImageAlignment.BottomLeft:
			r.X = r.Right - (width + imagePadding.Right + TabStripItem.TabHorizontalSpacing);
			r.Y = r.Bottom - (width + imagePadding.Left);
			break;
		case ImageAlignment.BottomCenter:
			r.X = r.Right - (width + imagePadding.Right + TabStripItem.TabHorizontalSpacing);
			r.Y += (r.Height - (width + imagePadding.Horizontal - TabStripItem.TabHorizontalSpacing)) / 2;
			break;
		case ImageAlignment.BottomRight:
			r.X = r.Right - (width + imagePadding.Right + TabStripItem.TabHorizontalSpacing);
			r.Y += imagePadding.Right + TabStripItem.TabHorizontalSpacing;
			break;
		}
		if (TabStripItem.RotateVerticalText ^ (TabStripItem.TabAlignment == eTabStripAlignment.Right))
		{
			r.X = rectangle.X + (rectangle.Right - (r.X + width));
			r.Y = rectangle.Y + (rectangle.Bottom - (r.Y + height));
		}
		r.Width = width;
		r.Height = height;
		return r;
	}

	private Rectangle CalcHorizontalImageBounds(CompositeImage image, Rectangle r)
	{
		int width = image.Width;
		int height = image.Height;
		Padding imagePadding = ImagePadding;
		switch (EffectiveImageAlignment)
		{
		case ImageAlignment.TopLeft:
			r.X += imagePadding.Left;
			r.Y += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			break;
		case ImageAlignment.TopCenter:
			r.X += (r.Width - (width + imagePadding.Horizontal + TabStripItem.TabHorizontalSpacing)) / 2;
			r.Y += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			break;
		case ImageAlignment.TopRight:
			r.X = r.Right - width - imagePadding.Right - TabStripItem.TabHorizontalSpacing;
			r.Y += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			break;
		case ImageAlignment.MiddleLeft:
			r.X += imagePadding.Left;
			r.Y += (r.Height - (height + imagePadding.Vertical)) / 2 + imagePadding.Top;
			break;
		case ImageAlignment.MiddleCenter:
			r.X += (r.Width - (width + imagePadding.Horizontal + TabStripItem.TabHorizontalSpacing)) / 2;
			r.Y += (r.Height - (height + imagePadding.Vertical)) / 2 + imagePadding.Top;
			break;
		case ImageAlignment.MiddleRight:
			r.X = r.Right - width - imagePadding.Right - TabStripItem.TabHorizontalSpacing;
			r.Y += (r.Height - (height + imagePadding.Vertical)) / 2 + imagePadding.Top;
			break;
		case ImageAlignment.BottomLeft:
			r.X += imagePadding.Left;
			r.Y = r.Bottom - (height + imagePadding.Bottom + TabStripItem.TabVerticalSpacing);
			break;
		case ImageAlignment.BottomCenter:
			r.X += (r.Width - (width + imagePadding.Horizontal + TabStripItem.TabHorizontalSpacing)) / 2;
			r.Y = r.Bottom - (height + imagePadding.Bottom + TabStripItem.TabVerticalSpacing);
			break;
		case ImageAlignment.BottomRight:
			r.X = r.Right - width - imagePadding.Right - TabStripItem.TabHorizontalSpacing;
			r.Y = r.Bottom - (height + imagePadding.Bottom + TabStripItem.TabVerticalSpacing);
			break;
		}
		r.Width = width;
		r.Height = height;
		return r;
	}

	internal CompositeImage GetTabImage()
	{
		Image image = GetImage();
		if (image != null)
		{
			return new CompositeImage(image, dispose: false);
		}
		if (Icon == null)
		{
			return null;
		}
		return new CompositeImage(Icon, dispose: false, Icon.Size);
	}

	private Image GetImage()
	{
		if (_Image != null)
		{
			return _Image;
		}
		if (_ImageIndex >= 0 && TabStrip != null)
		{
			ImageList imageList = TabStrip.ImageList;
			if (imageList != null && _ImageIndex < imageList.Images.Count)
			{
				return imageList.Images[_ImageIndex];
			}
		}
		return null;
	}

	private Rectangle GetTextBounds()
	{
		Rectangle contentRectangle = TabItemDisplay.GetContentRectangle();
		contentRectangle = ProcessImageBounds(ProcessCloseBounds(contentRectangle));
		if (!IsVertical)
		{
			if (IsSelected && (_TabStripItem.TabAlignment == eTabStripAlignment.Left || _TabStripItem.TabAlignment == eTabStripAlignment.Right))
			{
				contentRectangle.Width -= _TabStripItem.TabDisplay.SelectedPaddingWidth;
			}
		}
		else if ((_TabStripItem.TabAlignment == eTabStripAlignment.Left && _TabStripItem.RotateVerticalText) || (_TabStripItem.TabAlignment == eTabStripAlignment.Right && !_TabStripItem.RotateVerticalText))
		{
			contentRectangle.X += _TabStripItem.TabHorizontalSpacing;
		}
		contentRectangle.Width -= _TabStripItem.TabHorizontalSpacing;
		_TabStripItem.OnGetTextBounds(this, contentRectangle);
		return contentRectangle;
	}

	private Rectangle ProcessCloseBounds(Rectangle r)
	{
		int tabHorizontalSpacing = _TabStripItem.TabHorizontalSpacing;
		Size size = default(Size);
		if (_TabStripItem.CloseButtonOnTabsVisible && CloseButtonVisible)
		{
			size += CloseButtonBounds.Size;
			size.Width += tabHorizontalSpacing;
			size.Height += tabHorizontalSpacing;
		}
		if (IsVertical)
		{
			Rectangle rectangle = r;
			if (TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Right)
			{
				r.Y += size.Width;
			}
			r.Height -= size.Width + tabHorizontalSpacing;
			if (TabStripItem.RotateVerticalText ^ (TabStripItem.TabAlignment == eTabStripAlignment.Right))
			{
				r.X = rectangle.X + (rectangle.Right - r.Right);
				r.Y = rectangle.Y + (rectangle.Bottom - r.Bottom);
			}
		}
		else
		{
			if (_TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Left)
			{
				r.X += size.Width;
			}
			r.X += tabHorizontalSpacing;
			r.Width -= size.Width + tabHorizontalSpacing;
			if (IsSelected && _TabStripItem.TabAlignment == eTabStripAlignment.Right)
			{
				r.X += _TabStripItem.TabDisplay.SelectedPaddingWidth;
			}
		}
		return r;
	}

	private Rectangle ProcessImageBounds(Rectangle r)
	{
		CompositeImage tabImage = GetTabImage();
		if (tabImage != null)
		{
			r = (IsVertical ? SubVerticalImageBounds(tabImage, r) : SubHorizontalImageBounds(tabImage, r));
		}
		return r;
	}

	private Rectangle SubHorizontalImageBounds(CompositeImage image, Rectangle r)
	{
		int num = image.Width + _TabStripItem.TabHorizontalSpacing + ImagePadding.Horizontal;
		int num2 = image.Height + _TabStripItem.TabVerticalSpacing + ImagePadding.Vertical;
		switch (EffectiveImageAlignment)
		{
		case ImageAlignment.TopLeft:
		case ImageAlignment.TopCenter:
		case ImageAlignment.TopRight:
			r.Y += num2;
			r.Height -= num2;
			break;
		case ImageAlignment.MiddleLeft:
			r.X += num;
			r.Width -= num;
			break;
		case ImageAlignment.MiddleRight:
			r.Width -= num;
			break;
		case ImageAlignment.BottomLeft:
		case ImageAlignment.BottomCenter:
		case ImageAlignment.BottomRight:
			r.Height -= num2;
			break;
		}
		return r;
	}

	private Rectangle SubVerticalImageBounds(CompositeImage image, Rectangle r)
	{
		int num = image.Width + _TabStripItem.TabHorizontalSpacing + ImagePadding.Horizontal;
		int num2 = image.Height + _TabStripItem.TabVerticalSpacing + ImagePadding.Vertical;
		Rectangle rectangle = r;
		switch (EffectiveImageAlignment)
		{
		case ImageAlignment.TopLeft:
		case ImageAlignment.TopCenter:
		case ImageAlignment.TopRight:
			r.X += num2;
			r.Width -= num2;
			break;
		case ImageAlignment.MiddleLeft:
			r.Height -= num;
			break;
		case ImageAlignment.MiddleRight:
			r.Y += num;
			r.Height -= num;
			break;
		case ImageAlignment.BottomLeft:
		case ImageAlignment.BottomCenter:
		case ImageAlignment.BottomRight:
			r.Width -= num2;
			break;
		}
		if (TabStripItem.RotateVerticalText ^ (TabStripItem.TabAlignment == eTabStripAlignment.Right))
		{
			r.X = rectangle.X + (rectangle.Right - r.Right);
			r.Y = rectangle.Y + (rectangle.Bottom - r.Bottom);
		}
		return r;
	}

	private void RefreshOwner()
	{
		if (_TabStripItem != null)
		{
			_TabStripItem.NeedRecalcSize = true;
			_TabStripItem.Refresh();
		}
		Refresh();
	}

	public override void Refresh()
	{
		if (SuspendLayout || !Visible || !Displayed || TabStripItem == null)
		{
			return;
		}
		object containerControl = TabStripItem.ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || !BaseItem.IsHandleValid(val))
		{
			return;
		}
		if (NeedRecalcSize)
		{
			if (Parent is ItemContainer)
			{
				Parent.RecalcSize();
			}
			else
			{
				RecalcSize();
				if (Parent != null)
				{
					Parent.SubItemSizeChanged(this);
				}
			}
		}
		Invalidate(val);
	}

	public override void Paint(ItemPaintArgs p)
	{
		if (_TabItemDisplay != null)
		{
			_TabItemDisplay.Paint(p);
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		CloseButtonMouseOver = CloseButtonBounds.Contains(objArg.Location);
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		Refresh();
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		CloseButtonMouseOver = false;
		CloseButtonPressed = false;
		Refresh();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if (CloseButtonMouseOver)
		{
			CloseButtonPressed = (objArg.Button & 0x100000) == 1048576;
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		CloseButtonPressed = false;
	}

	protected void UpdateChangeHandler(INotifyPropertyChanged oldValue, INotifyPropertyChanged newValue)
	{
		if (oldValue != null)
		{
			oldValue.PropertyChanged -= ValuePropertyChanged;
		}
		if (newValue != null)
		{
			newValue.PropertyChanged += ValuePropertyChanged;
		}
	}

	private void ValuePropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		RefreshOwner();
	}

	public override string ToString()
	{
		return Text;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !base.IsDisposed)
		{
			HookEvents(hook: false);
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		SuperTabItem superTabItem = new SuperTabItem();
		CopyToItem(superTabItem);
		return superTabItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		if (copy is SuperTabItem superTabItem)
		{
			base.CopyToItem(superTabItem);
			superTabItem.TabColor = _TabColor;
			superTabItem.CloseButtonVisible = _CloseButtonVisible;
			superTabItem.EnableImageAnimation = _EnableImageAnimation;
			superTabItem.EnableMarkup = _EnableMarkup;
			superTabItem.Icon = _Icon;
			superTabItem.Image = _Image;
			superTabItem.ImageIndex = _ImageIndex;
			superTabItem.PredefinedColor = _PredefinedColor;
			superTabItem.SelectedTabFont = _SelectedTabFont;
			superTabItem.TextAlignment = _TextAlignment;
			superTabItem.TabFont = _TabFont;
			superTabItem.AttachedControl = _AttachedControl;
		}
	}
}
