using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(RibbonBar), "Ribbon.RibbonBar.ico")]
[ToolboxItem(false)]
public class RibbonBar : ItemControl
{
	[CompilerGenerated]
	private MouseEventHandler m_DialogLauncherMouseDown;

	private ItemContainer m_ItemContainer;

	private Rectangle m_TitleRectangle = Rectangle.Empty;

	private ElementStyle m_TitleStyle;

	private ElementStyle m_TitleStyleMouseOver;

	private SimpleElement m_TitleElement = new SimpleElement();

	private bool m_DialogLauncherVisible;

	private Rectangle m_DialogLauncherRect = Rectangle.Empty;

	private Image m_DialogLauncherButton;

	private Image m_DialogLauncherMouseOverButton;

	private bool m_MouseOverDialogLauncher;

	private bool m_AutoOverflow = true;

	private RibbonOverflowButtonItem m_OverflowButton;

	private RibbonBar m_OverflowRibbonBar;

	private string m_OverflowButtonText = "";

	private Image m_OverflowButtonImage;

	private int m_ResizeOrderIndex;

	private bool m_SuspendRecalcSize;

	private int m_MaximumOverflowTextLength = 25;

	private Size m_BeforeOverflowSize = Size.Empty;

	private bool m_MouseOver;

	private ElementStyle m_BackgroundMouseOverStyle = new ElementStyle();

	private eRibbonTitlePosition m_TitlePosition = eRibbonTitlePosition.Bottom;

	private bool m_TitleVisible = true;

	private bool m_EnableDragDrop;

	private ElementStyle m_DefaultBackgroundStyle = new ElementStyle();

	private ElementStyle m_DefaultBackgroundMouseOverStyle = new ElementStyle();

	private ElementStyle m_DefaultTitleStyle = new ElementStyle();

	private ElementStyle m_DefaultTitleStyleMouseOver = new ElementStyle();

	private bool m_CanCustomize = true;

	private ArrayList m_AutoSizeBagList = new ArrayList();

	private bool m_AutoSizeItems = true;

	private Size m_FullItemsSize = Size.Empty;

	private Size m_LastReducedSize = Size.Empty;

	private string m_DialogLauncherAccessibleName = "";

	private bool m_IsOverflowRibbon;

	private GalleryContainer m_GalleryStretch;

	private Timer m_MouseOverTimer;

	private bool m_BackgroundHoverEnabled = true;

	private bool m_UseSpecKeyTipsPositioning;

	private bool hotDragDrop;

	public ButtonItem LatestMouseOverItem;

	private ButtonItem _QatButtonParent;

	private bool _IsOnQat;

	private eVerticalItemsAlignment m_OriginalVerticalItemAlign;

	private eOrientation m_OriginalLayoutOrientation;

	private eHorizontalItemsAlignment m_OriginalHorizontalItemAlign;

	private RibbonBar _OverflowParent;

	private string _DialogLauncherKeyTip = "";

	private Bitmap m_FadeImageState;

	private int m_FadeDirection = 1;

	private int m_FadeAlpha;

	private bool m_FadeAnimation;

	private ReaderWriterLock m_FadeImageLock = new ReaderWriterLock();

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool HotDragDrop
	{
		get
		{
			return hotDragDrop;
		}
		set
		{
			hotDragDrop = value;
		}
	}

	[Browsable(false)]
	internal ButtonItem QatButtonParent
	{
		get
		{
			return _QatButtonParent;
		}
		set
		{
			_QatButtonParent = value;
		}
	}

	[Browsable(false)]
	internal bool IsOnQat
	{
		get
		{
			return _IsOnQat;
		}
		set
		{
			_IsOnQat = value;
		}
	}

	[DefaultValue(false)]
	[Category("KeyTips")]
	[Description("Indicates whether Office 2007 Design Guidelines specification for positioning KeyTips is used.")]
	public bool UseSpecKeyTipsPositioning
	{
		get
		{
			return m_UseSpecKeyTipsPositioning;
		}
		set
		{
			m_UseSpecKeyTipsPositioning = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Accessibility")]
	[Description("Indicates Accessible name for the Dialog Launcher button.")]
	public string DialogLauncherAccessibleName
	{
		get
		{
			return m_DialogLauncherAccessibleName;
		}
		set
		{
			m_DialogLauncherAccessibleName = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Indicates mouse over background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundMouseOverStyle => m_BackgroundMouseOverStyle;

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates image that is used as dialog luncher button in ribbon title bar.")]
	public Image DialogLauncherButton
	{
		get
		{
			return m_DialogLauncherButton;
		}
		set
		{
			m_DialogLauncherButton = value;
			OnDialogLuncherButtonChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates image that is used as dialog launcher button when mouse is over the button.")]
	public Image DialogLauncherMouseOverButton
	{
		get
		{
			return m_DialogLauncherMouseOverButton;
		}
		set
		{
			m_DialogLauncherMouseOverButton = value;
		}
	}

	[DefaultValue(25)]
	[Category("Overflow")]
	[Browsable(true)]
	[Description("Indicates maximum text length for automatic overflow button text.")]
	public int MaximumOverflowTextLength
	{
		get
		{
			return m_MaximumOverflowTextLength;
		}
		set
		{
			m_MaximumOverflowTextLength = value;
		}
	}

	[Browsable(true)]
	[Category("Overflow")]
	[DefaultValue(0)]
	[Description("Indicates resize order index of the control.")]
	public int ResizeOrderIndex
	{
		get
		{
			return m_ResizeOrderIndex;
		}
		set
		{
			m_ResizeOrderIndex = value;
		}
	}

	public bool AutoOverflowEnabled
	{
		get
		{
			return m_AutoOverflow;
		}
		set
		{
			if (m_AutoOverflow != value)
			{
				m_AutoOverflow = value;
				OnAutoOverflowChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates text for overflow button that is created when ribbon bar size is reduced so it cannot display all its content")]
	[Category("Overflow")]
	public string OverflowButtonText
	{
		get
		{
			return m_OverflowButtonText;
		}
		set
		{
			m_OverflowButtonText = value;
			OnOverflowButtonChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates image for overflow button that is created when ribbon bar size is reduced so it cannot display all its content")]
	[Category("Overflow")]
	public Image OverflowButtonImage
	{
		get
		{
			return m_OverflowButtonImage;
		}
		set
		{
			m_OverflowButtonImage = value;
			OnOverflowButtonChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether dialog launcher button is visible in title of the ribbon.")]
	public bool DialogLauncherVisible
	{
		get
		{
			return m_DialogLauncherVisible;
		}
		set
		{
			m_DialogLauncherVisible = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[DefaultValue(eOrientation.Horizontal)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public virtual eOrientation LayoutOrientation
	{
		get
		{
			return m_ItemContainer.LayoutOrientation;
		}
		set
		{
			m_ItemContainer.LayoutOrientation = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(1)]
	[Category("Layout")]
	[Description("Indicates spacing in pixels between items.")]
	public int ItemSpacing
	{
		get
		{
			return m_ItemContainer.ItemSpacing;
		}
		set
		{
			m_ItemContainer.ItemSpacing = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Layout")]
	public bool ResizeItemsToFit
	{
		get
		{
			return m_ItemContainer.ResizeItemsToFit;
		}
		set
		{
			m_ItemContainer.ResizeItemsToFit = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eHorizontalItemsAlignment.Left)]
	[Category("Layout")]
	[Description("Indicates item alignment when container is in horizontal layout.")]
	[DevCoBrowsable(true)]
	public eHorizontalItemsAlignment HorizontalItemAlignment
	{
		get
		{
			return m_ItemContainer.HorizontalItemAlignment;
		}
		set
		{
			m_ItemContainer.HorizontalItemAlignment = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eVerticalItemsAlignment.Top)]
	[Category("Layout")]
	[Description("Indicates item item vertical alignment.")]
	[DevCoBrowsable(true)]
	public eVerticalItemsAlignment VerticalItemAlignment
	{
		get
		{
			return m_ItemContainer.VerticalItemAlignment;
		}
		set
		{
			m_ItemContainer.VerticalItemAlignment = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items
	{
		get
		{
			if (OverflowState && !((Component)(object)this).DesignMode)
			{
				return m_OverflowRibbonBar.Items;
			}
			return m_ItemContainer.SubItems;
		}
	}

	internal Rectangle DialogLauncherRect => m_DialogLauncherRect;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether ribbon bar can be customized by end user i.e. added to Quick Access Toolbar.")]
	public virtual bool CanCustomize
	{
		get
		{
			return m_CanCustomize;
		}
		set
		{
			m_CanCustomize = value;
		}
	}

	internal GalleryContainer GalleryStretch
	{
		get
		{
			return m_GalleryStretch;
		}
		set
		{
			m_GalleryStretch = value;
		}
	}

	internal Size BeforeOverflowSize => m_BeforeOverflowSize;

	internal Size LastReducedSize => m_LastReducedSize;

	internal bool IsItemsAutoSizeActive => m_AutoSizeBagList.Count > 0;

	internal bool IsOverflowRibbon
	{
		get
		{
			return m_IsOverflowRibbon;
		}
		set
		{
			m_IsOverflowRibbon = value;
		}
	}

	internal RibbonBar OverflowParent
	{
		get
		{
			return _OverflowParent;
		}
		set
		{
			_OverflowParent = value;
		}
	}

	internal RibbonBar OverflowRibbonBar => m_OverflowRibbonBar;

	[Browsable(false)]
	public bool OverflowState
	{
		get
		{
			if (m_OverflowButton != null)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether ButtonItem objects hosted on control are resized to reduce the space consumed by ribbon bar when required.")]
	public bool AutoSizeItems
	{
		get
		{
			return m_AutoSizeItems;
		}
		set
		{
			m_AutoSizeItems = value;
		}
	}

	public override Rectangle DisplayRectangle
	{
		get
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			if (!m_TitleRectangle.IsEmpty && m_TitleVisible)
			{
				if (m_TitlePosition == eRibbonTitlePosition.Top)
				{
					clientRectangle.Y += m_TitleRectangle.Height;
					clientRectangle.Height -= m_TitleRectangle.Height;
				}
				else
				{
					clientRectangle.Height -= m_TitleRectangle.Height;
				}
			}
			return clientRectangle;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_ItemContainer.Style;
		}
		set
		{
			m_ItemContainer.Style = value;
			base.ColorScheme.Style = m_ItemContainer.EffectiveStyle;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	internal eDotNetBarStyle EffectiveStyle => m_ItemContainer.EffectiveStyle;

	protected override bool NeedsTopLevelKeyTipCanvasParent
	{
		get
		{
			if (DialogLauncherVisible && DialogLauncherKeyTip != "")
			{
				return true;
			}
			return false;
		}
	}

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates KeyTip for the dialog launcher button.")]
	public string DialogLauncherKeyTip
	{
		get
		{
			return _DialogLauncherKeyTip;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_DialogLauncherKeyTip = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether ribbon bar title is visible")]
	public bool TitleVisible
	{
		get
		{
			return m_TitleVisible;
		}
		set
		{
			m_TitleVisible = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Specifies the style of the title of the control")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TitleStyle => m_TitleStyle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Specifies the style of the title of the control when mouse is over the control.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TitleStyleMouseOver => m_TitleStyleMouseOver;

	internal Rectangle TitleRectangle => m_TitleElement.Bounds;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether control changes its background when mouse is over the control.")]
	public bool BackgroundHoverEnabled
	{
		get
		{
			return m_BackgroundHoverEnabled;
		}
		set
		{
			m_BackgroundHoverEnabled = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
	public override bool AllowExternalDrop
	{
		get
		{
			return base.AllowExternalDrop;
		}
		set
		{
			base.AllowExternalDrop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
	public override bool UseNativeDragDrop
	{
		get
		{
			return base.UseNativeDragDrop;
		}
		set
		{
			base.UseNativeDragDrop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Specifies whether automatic drag & drop support is enabled.")]
	public virtual bool EnableDragDrop
	{
		get
		{
			return m_EnableDragDrop;
		}
		set
		{
			m_EnableDragDrop = value;
		}
	}

	[Description("Occurs when dialog launcher button in title bar is clicked. Use DialogLauncherVisible property to show the button in title bar.")]
	public event EventHandler LaunchDialog;

	[Description("Occurs when overflow button for control is created because control size is below the minimum size required to display complete content of the control.")]
	public event OverflowButtonEventHandler OverflowButtonSetup;

	[Description("Occurs after overflow button setup is complete and all items contained by this control are moved to it.")]
	public event OverflowButtonEventHandler OverflowButtonSetupComplete;

	[Description("Occurs before overflow button is destroyed.")]
	public event OverflowButtonEventHandler OverflowButtonDestroy;

	[Description("")]
	public event EventHandler DialogLauncherMouseEnter;

	[Description("Occurs when mouse leaves dialog launcher button.")]
	public event EventHandler DialogLauncherMouseLeave;

	[Description("Occurs when mouse hovers over the dialog launcher button.")]
	public event EventHandler DialogLauncherMouseHover;

	[Description("Occurs when mouse is pressed over the dialog launcher button.")]
	public event MouseEventHandler DialogLauncherMouseDown
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_DialogLauncherMouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_DialogLauncherMouseDown, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_DialogLauncherMouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_DialogLauncherMouseDown, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler StartDragDrop;

	public void InvokeStartDragDrop(object item)
	{
		this.StartDragDrop(item, EventArgs.Empty);
	}

	public RibbonBar()
	{
		m_ItemContainer = new ItemContainer();
		m_ItemContainer.GlobalItem = false;
		m_ItemContainer.ContainerControl = this;
		m_ItemContainer.Stretch = false;
		m_ItemContainer.Displayed = true;
		m_ItemContainer.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.Style = eDotNetBarStyle.StyleManagerControlled;
		m_ItemContainer.SetOwner(this);
		m_ItemContainer.SetSystemContainer(b: true);
		SetBaseItemContainer(m_ItemContainer);
		DragDropSupport = true;
		m_TitleStyle = new ElementStyle();
		m_TitleStyle.SetColorScheme(GetColorScheme());
		m_TitleStyle.StyleChanged += ElementStyleChanged;
		m_TitleStyleMouseOver = new ElementStyle();
		m_TitleStyleMouseOver.SetColorScheme(GetColorScheme());
		m_TitleStyleMouseOver.StyleChanged += ElementStyleChanged;
		m_BackgroundMouseOverStyle.SetColorScheme(GetColorScheme());
		m_BackgroundMouseOverStyle.StyleChanged += VisualPropertyChanged;
		base.ItemAdded += RibbonBar_ItemAdded;
		base.ItemRemoved += RibbonBar_ItemRemoved;
	}

	protected override void Dispose(bool disposing)
	{
		DestroyMouseOverTimer();
		if (m_OverflowRibbonBar != null)
		{
			((Component)(object)m_OverflowRibbonBar).Dispose();
			m_OverflowRibbonBar = null;
		}
		if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref m_DialogLauncherButton);
			BarUtilities.DisposeImage(ref m_OverflowButtonImage);
			BarUtilities.DisposeImage(ref m_DialogLauncherMouseOverButton);
		}
		base.Dispose(disposing);
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		return (AccessibleObject)(object)new RibbonBarAccessibleObject(this);
	}

	protected override void OnItemClick(BaseItem item)
	{
		base.OnItemClick(item);
		if (IsOverflowRibbon && ((Control)this).Parent is MenuPanel && item != null && item.AutoCollapseOnClick && item is ButtonItem && !item.Name.StartsWith("sysgallery"))
		{
			ButtonItem buttonItem = item as ButtonItem;
			if (!buttonItem.AutoExpandOnClick)
			{
				CloseOverflowPopup();
			}
		}
	}

	public void CloseOverflowPopup()
	{
		if (IsOverflowRibbon)
		{
			if (((Control)this).Parent is MenuPanel && ((MenuPanel)(object)((Control)this).Parent).ParentItem is RibbonOverflowButtonItem { Expanded: not false } ribbonOverflowButtonItem)
			{
				ribbonOverflowButtonItem.Expanded = false;
			}
		}
		else if (OverflowState && m_OverflowButton != null && m_OverflowButton.Expanded)
		{
			m_OverflowButton.Expanded = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundMouseOverStyle()
	{
		m_BackgroundMouseOverStyle.StyleChanged -= VisualPropertyChanged;
		m_BackgroundMouseOverStyle = new ElementStyle();
		m_BackgroundMouseOverStyle.StyleChanged += VisualPropertyChanged;
		((Control)this).Invalidate();
	}

	protected virtual ElementStyle GetBackgroundMouseOverStyle()
	{
		if (!m_BackgroundHoverEnabled)
		{
			return null;
		}
		if (OverflowState && m_OverflowButton.Expanded)
		{
			return GetBackgroundStyle();
		}
		if (m_BackgroundMouseOverStyle.Custom)
		{
			ElementStyle elementStyle = m_DefaultBackgroundMouseOverStyle.Copy();
			elementStyle.ApplyStyle(m_BackgroundMouseOverStyle);
			return elementStyle;
		}
		return m_DefaultBackgroundMouseOverStyle;
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (OverflowState && m_OverflowButton.Expanded && GetRenderer() is Office2007Renderer office2007Renderer)
		{
			return RibbonPredefinedColorSchemes.StyleFromRibbonBarStateColorTable(office2007Renderer.ColorTable.RibbonBar.Expanded);
		}
		if (base.BackgroundStyle.Custom)
		{
			ElementStyle elementStyle = m_DefaultBackgroundStyle.Copy();
			elementStyle.ApplyStyle(base.BackgroundStyle);
			return elementStyle;
		}
		return m_DefaultBackgroundStyle;
	}

	private ElementStyle GetTitleStyle()
	{
		if (m_TitleStyle.Custom)
		{
			ElementStyle elementStyle = m_DefaultTitleStyle.Copy();
			elementStyle.ApplyStyle(m_TitleStyle);
			return elementStyle;
		}
		return m_DefaultTitleStyle;
	}

	private ElementStyle GetTitleMouseOverStyle()
	{
		if (!m_BackgroundHoverEnabled)
		{
			return null;
		}
		if (m_TitleStyleMouseOver.Custom)
		{
			ElementStyle elementStyle = m_DefaultTitleStyleMouseOver.Copy();
			elementStyle.ApplyStyle(m_TitleStyleMouseOver);
			return elementStyle;
		}
		return m_DefaultTitleStyleMouseOver;
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		if (GetDesignMode())
		{
			RecalcLayout();
		}
		else if (((Control)this).Parent != null && ((Component)(object)((Control)this).Parent).Site != null && ((Component)(object)((Control)this).Parent).Site.DesignMode)
		{
			RecalcLayout();
		}
	}

	private void OnAutoOverflowChanged()
	{
		if (m_OverflowButton != null)
		{
			DisposeOverflowButton();
		}
		RecalcLayout();
	}

	private void OnOverflowButtonChanged()
	{
		if (m_OverflowButton != null)
		{
			m_OverflowButton.Text = m_OverflowButtonText;
			m_OverflowButton.Image = m_OverflowButtonImage;
		}
	}

	public void DoLaunchDialog()
	{
		InvokeLaunchDialog(new EventArgs());
	}

	protected virtual void InvokeLaunchDialog(EventArgs e)
	{
		if (this.LaunchDialog != null)
		{
			this.LaunchDialog(this, e);
		}
	}

	protected override void OnPopupItemRightClick(BaseItem item)
	{
		if (item == null || item.CanCustomize)
		{
			GetRibbonControl()?.ShowCustomizeContextMenu(item, ribbonStrip: false);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Invalid comparison between Unknown and I4
		if (HotDragDrop)
		{
			foreach (ButtonItem item in Items)
			{
				if (item.ScreenRectangle.Contains(((Control)this).PointToScreen(new Point(e.X, e.Y))))
				{
					LatestMouseOverItem = item;
					break;
				}
			}
		}
		base.OnMouseDown(e);
		if (m_DialogLauncherRect.Contains(e.X, e.Y) && !m_DialogLauncherRect.IsEmpty && m_TitleVisible && !OverflowState)
		{
			((Control)this).Invalidate(m_DialogLauncherRect);
			OnDialogLauncherMouseDown(e);
		}
		if ((int)e.Button == 2097152)
		{
			GetRibbonControl()?.OnRibbonBarRightClick(this, e.X, e.Y);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		base.OnMouseUp(e);
		if (!m_DialogLauncherRect.Contains(e.X, e.Y) || (int)e.Button != 1048576)
		{
			return;
		}
		if (!m_DialogLauncherRect.IsEmpty)
		{
			((Control)this).Invalidate(m_DialogLauncherRect);
		}
		if (((Control)this).Parent is MenuPanel)
		{
			if (((MenuPanel)(object)((Control)this).Parent).ParentItem is PopupItem popupItem)
			{
				popupItem.ClosePopup();
			}
		}
		else if (((Control)this).Parent is Bar && ((Bar)(object)((Control)this).Parent).BarState == eBarState.Popup && ((Bar)(object)((Control)this).Parent).ParentItem is PopupItem popupItem2)
		{
			popupItem2.ClosePopup();
		}
		InvokeLaunchDialog(new EventArgs());
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		SetMouseOver(value: true);
		if (((m_TitleStyleMouseOver != null && m_TitleStyleMouseOver.Custom) || m_DefaultTitleStyleMouseOver.Custom) && m_TitleVisible && !OverflowState)
		{
			((Control)this).Invalidate(GetTitleRectangle());
		}
		ElementStyle backgroundMouseOverStyle = GetBackgroundMouseOverStyle();
		if (backgroundMouseOverStyle != null && backgroundMouseOverStyle.Custom)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (((Control)this).ClientRectangle.Contains(pt))
		{
			Form val = ((Control)this).FindForm();
			if (val != null && Form.ActiveForm == val)
			{
				CreateMouseOverTimer();
				return;
			}
		}
		SetMouseOver(value: false);
		SetMouseOverDialogLauncher(value: false);
		if (((m_TitleStyleMouseOver != null && m_TitleStyleMouseOver.Custom) || m_DefaultTitleStyleMouseOver.Custom) && m_TitleVisible && !OverflowState)
		{
			((Control)this).Invalidate(GetTitleRectangle());
		}
		ElementStyle backgroundMouseOverStyle = GetBackgroundMouseOverStyle();
		if (backgroundMouseOverStyle != null && backgroundMouseOverStyle.Custom)
		{
			((Control)this).Invalidate();
		}
	}

	private void CreateMouseOverTimer()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (m_MouseOverTimer == null)
		{
			m_MouseOverTimer = new Timer();
			m_MouseOverTimer.Interval = 500;
			m_MouseOverTimer.Tick += MouseOverTimer_Tick;
			m_MouseOverTimer.Enabled = true;
			m_MouseOverTimer.Start();
		}
	}

	private void MouseOverTimer_Tick(object sender, EventArgs e)
	{
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (!((Control)this).ClientRectangle.Contains(pt) || !((Control)this).Visible)
		{
			m_MouseOverTimer.Stop();
			m_MouseOverTimer.Enabled = false;
			SetMouseOver(value: false);
			DestroyMouseOverTimer();
		}
	}

	private void DestroyMouseOverTimer()
	{
		if (m_MouseOverTimer != null)
		{
			m_MouseOverTimer.Stop();
			((Component)(object)m_MouseOverTimer).Dispose();
			m_MouseOverTimer = null;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && HotDragDrop && !((Control)this).ClientRectangle.Contains(new Point(e.X, e.Y)) && LatestMouseOverItem != null)
		{
			ButtonItem latestMouseOverItem = LatestMouseOverItem;
			LatestMouseOverItem = null;
			if (this.StartDragDrop != null && latestMouseOverItem.Tag != null)
			{
				this.StartDragDrop(latestMouseOverItem, EventArgs.Empty);
			}
			LatestMouseOverItem = null;
		}
		base.OnMouseMove(e);
		if (m_DialogLauncherVisible)
		{
			if (m_DialogLauncherRect.Contains(e.X, e.Y))
			{
				SetMouseOverDialogLauncher(value: true);
			}
			else
			{
				SetMouseOverDialogLauncher(value: false);
			}
		}
	}

	protected override void InternalMouseMove(MouseEventArgs e)
	{
		if (m_TitleVisible && !m_TitleRectangle.IsEmpty && m_TitleRectangle.Contains(e.X, e.Y))
		{
			if (m_ItemContainer.HotSubItem != null)
			{
				m_ItemContainer.InternalMouseLeave();
			}
		}
		else
		{
			base.InternalMouseMove(e);
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		base.OnMouseHover(e);
		if (m_MouseOverDialogLauncher)
		{
			OnDialogLauncherMouseHover(new EventArgs());
		}
	}

	private void SetMouseOverDialogLauncher(bool value)
	{
		if (m_MouseOverDialogLauncher != value)
		{
			m_MouseOverDialogLauncher = value;
			((Control)this).Invalidate(m_DialogLauncherRect);
			if (value)
			{
				OnDialogLauncherMouseEnter(new EventArgs());
			}
			else
			{
				OnDialogLauncherMouseLeave(new EventArgs());
			}
		}
	}

	protected virtual void OnDialogLauncherMouseEnter(EventArgs e)
	{
		if (this.DialogLauncherMouseEnter != null)
		{
			this.DialogLauncherMouseEnter(this, e);
		}
	}

	protected virtual void OnDialogLauncherMouseLeave(EventArgs e)
	{
		if (this.DialogLauncherMouseLeave != null)
		{
			this.DialogLauncherMouseLeave(this, e);
		}
	}

	protected virtual void OnDialogLauncherMouseHover(EventArgs e)
	{
		if (this.DialogLauncherMouseHover != null)
		{
			this.DialogLauncherMouseHover(this, e);
		}
	}

	protected virtual void OnDialogLauncherMouseDown(MouseEventArgs e)
	{
		if (this.DialogLauncherMouseDown != null)
		{
			this.DialogLauncherMouseDown.Invoke((object)this, e);
		}
	}

	public override ArrayList GetItems(string ItemName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return base.GetItems(ItemName);
		}
		return ribbonControl.RibbonStrip.GetItems(ItemName);
	}

	public override ArrayList GetItems(string ItemName, Type itemType)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return base.GetItems(ItemName, itemType);
		}
		return ribbonControl.RibbonStrip.GetItems(ItemName, itemType);
	}

	public override BaseItem GetItem(string ItemName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return base.GetItem(ItemName);
		}
		return ribbonControl.RibbonStrip.GetItem(ItemName);
	}

	public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl == null)
		{
			return base.GetItems(ItemName, itemType, useGlobalName);
		}
		return ribbonControl.RibbonStrip.GetItems(ItemName, itemType, useGlobalName);
	}

	protected override void OnShowKeyTipsChanged()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ShowKeyTips)
		{
			ribbonControl.RibbonStrip.OnRibbonBarShowKeyTips(this);
		}
		base.OnShowKeyTipsChanged();
	}

	protected override BaseItem GetItemForMnemonic(BaseItem container, char charCode, bool deepScan, bool stackKeys)
	{
		BaseItem itemForMnemonic = base.GetItemForMnemonic(container, charCode, deepScan, stackKeys);
		if (itemForMnemonic == null && OverflowState)
		{
			itemForMnemonic = m_OverflowRibbonBar.GetItemForMnemonic(m_OverflowRibbonBar.m_ItemContainer, charCode, deepScan, stackKeys);
		}
		return itemForMnemonic;
	}

	protected override void OnTextChanged(EventArgs e)
	{
		m_TitleElement.Text = ((Control)this).Text;
		if (m_OverflowButton != null)
		{
			m_OverflowButton.Text = GetOverflowButtonText();
		}
		RecalcLayout();
		((Control)this).OnTextChanged(e);
	}

	private void OnDialogLuncherButtonChanged()
	{
		RecalcLayout();
	}

	private void InitDefaultStyles()
	{
		ColorScheme colorScheme = GetColorScheme();
		m_DefaultBackgroundStyle.SetColorScheme(colorScheme);
		m_DefaultBackgroundMouseOverStyle.SetColorScheme(colorScheme);
		m_DefaultTitleStyle.SetColorScheme(colorScheme);
		m_DefaultTitleStyle.HideMnemonic = true;
		m_DefaultTitleStyleMouseOver.SetColorScheme(colorScheme);
		m_DefaultTitleStyleMouseOver.HideMnemonic = true;
		RibbonPredefinedColorSchemes.ApplyRibbonBarElementStyle(m_DefaultBackgroundStyle, m_DefaultBackgroundMouseOverStyle, m_DefaultTitleStyle, m_DefaultTitleStyleMouseOver, this);
	}

	private void RibbonBar_ItemRemoved(object sender, ItemRemovedEventArgs e)
	{
		if (sender == m_GalleryStretch)
		{
			m_GalleryStretch = null;
		}
	}

	private void RibbonBar_ItemAdded(object sender, EventArgs e)
	{
		if (sender is GalleryContainer)
		{
			GalleryContainer galleryContainer = sender as GalleryContainer;
			if (galleryContainer.StretchGallery && m_GalleryStretch == null)
			{
				m_GalleryStretch = galleryContainer;
			}
		}
	}

	private Size GetItemContainerMinimumSize()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (m_TitleVisible)
		{
			Rectangle titleRectangle = GetTitleRectangle();
			if (m_TitlePosition == eRibbonTitlePosition.Top)
			{
				titleRectangle.Height++;
				itemContainerRectangle.Y += titleRectangle.Height;
				itemContainerRectangle.Height -= titleRectangle.Height;
			}
			else
			{
				titleRectangle.Height++;
				itemContainerRectangle.Height -= titleRectangle.Height;
			}
		}
		return itemContainerRectangle.Size;
	}

	protected override void RecalcSize()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Invalid comparison between Unknown and I4
		if (!BarFunctions.IsHandleValid((Control)(object)this) || m_SuspendRecalcSize)
		{
			return;
		}
		InitDefaultStyles();
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (m_TitleVisible)
		{
			SimpleElementLayoutInfo simpleElementLayoutInfo = new SimpleElementLayoutInfo();
			m_TitleElement.Image = m_DialogLauncherButton;
			if (m_DialogLauncherButton != null && m_DialogLauncherVisible)
			{
				m_TitleElement.ImageAlignment = eSimplePartAlignment.FarCenter;
			}
			simpleElementLayoutInfo.Element = m_TitleElement;
			simpleElementLayoutInfo.LayoutStyle = GetTitleStyle();
			simpleElementLayoutInfo.Font = ((Control)this).Font;
			simpleElementLayoutInfo.LeftToRight = (int)((Control)this).RightToLeft != 1;
			simpleElementLayoutInfo.VerticalPartAlignment = false;
			simpleElementLayoutInfo.Left = ((Control)this).ClientRectangle.X;
			simpleElementLayoutInfo.Top = ((Control)this).ClientRectangle.Y;
			m_TitleElement.FixedWidth = ((Control)this).ClientRectangle.Width;
			if (backgroundStyle != null)
			{
				if (backgroundStyle.PaintRightBorder)
				{
					m_TitleElement.FixedWidth -= backgroundStyle.BorderRightWidth;
				}
				if (backgroundStyle.PaintTopBorder)
				{
					simpleElementLayoutInfo.Top += backgroundStyle.BorderTopWidth;
				}
			}
			Graphics val = CreateGraphics();
			try
			{
				simpleElementLayoutInfo.Graphics = val;
				SimpleElementLayout.LayoutSimpleElement(simpleElementLayoutInfo);
				if (m_TitlePosition == eRibbonTitlePosition.Bottom)
				{
					simpleElementLayoutInfo.Top = ((Control)this).ClientRectangle.Bottom - m_TitleElement.Bounds.Height;
					if (backgroundStyle != null && backgroundStyle.PaintBottomBorder)
					{
						simpleElementLayoutInfo.Top -= backgroundStyle.BorderBottomWidth;
					}
					SimpleElementLayout.LayoutSimpleElement(simpleElementLayoutInfo);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			m_TitleRectangle = m_TitleElement.Bounds;
		}
		else
		{
			m_TitleRectangle = Rectangle.Empty;
		}
		RestoreAutoSizedItems();
		m_FullItemsSize = Size.Empty;
		if (((Control)this).Parent is RibbonPanel)
		{
			RibbonControl ribbonControl = GetRibbonControl();
			if (ribbonControl != null && !((Control)ribbonControl).AutoSize)
			{
				m_ItemContainer.MinimumSize = new Size(0, GetItemContainerMinimumSize().Height);
			}
		}
		else if (IsOverflowRibbon && !((Control)this).AutoSize)
		{
			m_ItemContainer.MinimumSize = new Size(0, GetItemContainerMinimumSize().Height);
		}
		else
		{
			m_ItemContainer.MinimumSize = Size.Empty;
		}
		base.RecalcSize();
		Size size = GetContentBasedSize();
		if (ResizeGallery(size.Width))
		{
			m_LastReducedSize = Size.Empty;
		}
		else
		{
			if (!AutoOverflowEnabled || ((Component)(object)this).DesignMode)
			{
				return;
			}
			if (size.Width > ((Control)this).Width)
			{
				if (m_AutoSizeBagList.Count == 0 && m_AutoSizeItems && ((Control)this).Width > 1 && !OverflowState)
				{
					AutoReduceItemSize(size.Width);
					base.RecalcSize();
					Size contentBasedSize = GetContentBasedSize();
					if (contentBasedSize.Width > ((Control)this).Width)
					{
						RestoreAutoSizedItems();
					}
					else
					{
						m_LastReducedSize = contentBasedSize;
						m_FullItemsSize = size;
						size = contentBasedSize;
					}
				}
				if (!OverflowState && size.Width > ((Control)this).Width)
				{
					m_BeforeOverflowSize = size;
					SetupOverflowButton();
					base.RecalcSize();
				}
			}
			else if (OverflowState)
			{
				if (m_BeforeOverflowSize.IsEmpty)
				{
					DisposeOverflowButton();
					base.RecalcSize();
					if (GetContentBasedSize().Width > ((Control)this).Width)
					{
						SetupOverflowButton();
						base.RecalcSize();
					}
				}
				else if (m_BeforeOverflowSize.Width <= ((Control)this).Width)
				{
					DisposeOverflowButton();
					base.RecalcSize();
					if (ResizeGallery(GetContentBasedSize().Width))
					{
						m_LastReducedSize = Size.Empty;
					}
				}
				else if (!m_LastReducedSize.IsEmpty && m_LastReducedSize.Width <= ((Control)this).Width)
				{
					DisposeOverflowButton();
					AutoReduceItemSize(m_LastReducedSize.Width);
					base.RecalcSize();
					if (ResizeGallery(GetContentBasedSize().Width))
					{
						m_LastReducedSize = Size.Empty;
					}
				}
			}
			else
			{
				m_LastReducedSize = Size.Empty;
			}
		}
	}

	private bool ResizeGallery(int contentBasedWidth)
	{
		if (m_GalleryStretch == null || !m_GalleryStretch.Visible)
		{
			return false;
		}
		int num = ((Control)this).Width - contentBasedWidth;
		if (m_GalleryStretch.DisplayRectangle.Width + num >= m_GalleryStretch.MinimumSize.Width)
		{
			int width = m_GalleryStretch.DisplayRectangle.Width;
			m_GalleryStretch.RecommendedSize = new Size(m_GalleryStretch.DisplayRectangle.Width + num, m_GalleryStretch.DefaultSize.Height);
			m_GalleryStretch.RecalcSize();
			int num2 = m_GalleryStretch.DisplayRectangle.Width - width;
			OffsetItems(m_GalleryStretch.Parent, m_GalleryStretch.Parent.SubItems.IndexOf(m_GalleryStretch) + 1, num2);
			return Math.Abs(num2) >= Math.Abs(num);
		}
		return false;
	}

	private void OffsetItems(BaseItem parent, int startOffsetIndex, int offset)
	{
		int count = parent.SubItems.Count;
		for (int i = startOffsetIndex; i < count; i++)
		{
			BaseItem baseItem = parent.SubItems[i];
			if (baseItem.Visible)
			{
				Rectangle bounds = baseItem.Bounds;
				bounds.Offset(offset, 0);
				baseItem.Bounds = bounds;
			}
		}
		parent.SetDisplayRectangle(new Rectangle(parent.DisplayRectangle.X, parent.DisplayRectangle.Y, parent.DisplayRectangle.Width + offset, parent.DisplayRectangle.Height));
	}

	private void AutoReduceItemSize(int contentBasedWidth)
	{
		if (m_AutoSizeBagList.Count <= 0)
		{
			if (m_GalleryStretch != null)
			{
				m_GalleryStretch.RecommendedSize = new Size(m_GalleryStretch.MinimumSize.Width, m_GalleryStretch.HeightInternal);
			}
			AutoReduceItemContainerSize(m_ItemContainer);
		}
	}

	private void AutoReduceItemContainerSize(ItemContainer cont)
	{
		double factor = StiScale2.Factor;
		if (cont.LayoutOrientation == eOrientation.Vertical)
		{
			foreach (BaseItem subItem in cont.SubItems)
			{
				if (subItem is ButtonItem { ButtonStyle: eButtonStyle.ImageAndText } buttonItem && (buttonItem.ImagePosition == eImagePosition.Left || buttonItem.ImagePosition == eImagePosition.Right))
				{
					BaseItemAutoSizeBag baseItemAutoSizeBag = AutoSizeBagFactory.CreateAutoSizeBag(buttonItem);
					baseItemAutoSizeBag.RecordSetting(buttonItem);
					bool globalItem = buttonItem.GlobalItem;
					buttonItem.GlobalItem = false;
					buttonItem.ButtonStyle = eButtonStyle.Default;
					buttonItem.ImageFixedSize = new Size((int)Math.Ceiling(16.0 * StiScale2.X), (int)Math.Ceiling(16.0 * StiScale2.Y));
					buttonItem.UseSmallImage = true;
					buttonItem.GlobalItem = globalItem;
					m_AutoSizeBagList.Add(baseItemAutoSizeBag);
				}
				else if (subItem is ItemContainer && !(subItem is GalleryContainer))
				{
					AutoReduceItemContainerSize(subItem as ItemContainer);
				}
			}
			return;
		}
		ArrayList arrayList = new ArrayList();
		foreach (BaseItem subItem2 in cont.SubItems)
		{
			if (subItem2 is ItemContainer && !(subItem2 is GalleryContainer))
			{
				AutoReduceItemContainerSize(subItem2 as ItemContainer);
			}
			else if (subItem2 is ButtonItem)
			{
				ButtonItem buttonItem2 = subItem2 as ButtonItem;
				if (buttonItem2.ImagePosition == eImagePosition.Top || buttonItem2.ImagePosition == eImagePosition.Bottom)
				{
					arrayList.Add(buttonItem2);
				}
			}
		}
		if (arrayList.Count <= 1)
		{
			return;
		}
		int num = arrayList.Count / 3;
		int num2 = arrayList.Count - num * 3;
		for (int num3 = arrayList.Count - 1; num3 >= num2; num3--)
		{
			ButtonItem buttonItem3 = arrayList[num3] as ButtonItem;
			BaseItemAutoSizeBag baseItemAutoSizeBag2 = AutoSizeBagFactory.CreateAutoSizeBag(buttonItem3);
			baseItemAutoSizeBag2.RecordSetting(buttonItem3);
			bool globalItem2 = buttonItem3.GlobalItem;
			buttonItem3.GlobalItem = false;
			buttonItem3.ImagePosition = eImagePosition.Left;
			buttonItem3.ButtonStyle = eButtonStyle.ImageAndText;
			buttonItem3.ImageFixedSize = new Size((int)Math.Ceiling(16.0 * StiScale2.X), (int)Math.Ceiling(16.0 * StiScale2.Y));
			if (buttonItem3.TextMarkupBody != null && buttonItem3.TextMarkupBody.HasExpandElement)
			{
				buttonItem3.Text = MarkupParser.RemoveExpand(buttonItem3.Text);
			}
			buttonItem3.GlobalItem = globalItem2;
			m_AutoSizeBagList.Add(baseItemAutoSizeBag2);
		}
		BaseItemAutoSizeBag baseItemAutoSizeBag3 = AutoSizeBagFactory.CreateAutoSizeBag(cont);
		m_AutoSizeBagList.Add(baseItemAutoSizeBag3);
		baseItemAutoSizeBag3.RecordSetting(cont);
		cont.LayoutOrientation = eOrientation.Vertical;
		cont.MultiLine = true;
	}

	internal void RestoreAutoSizedItems()
	{
		if (m_GalleryStretch != null)
		{
			m_GalleryStretch.RecommendedSize = Size.Empty;
		}
		if (m_AutoSizeBagList.Count != 0)
		{
			BaseItemAutoSizeBag[] array = new BaseItemAutoSizeBag[m_AutoSizeBagList.Count];
			m_AutoSizeBagList.CopyTo(array);
			BaseItemAutoSizeBag[] array2 = array;
			foreach (BaseItemAutoSizeBag baseItemAutoSizeBag in array2)
			{
				baseItemAutoSizeBag.RestoreSettings();
			}
			m_AutoSizeBagList.Clear();
		}
	}

	private RibbonBar CreateOverflowRibbon()
	{
		return CreateOverflowRibbon(createHandle: false);
	}

	internal RibbonBar CreateOverflowRibbon(bool createHandle)
	{
		RibbonBar ribbonBar = new RibbonBar();
		ribbonBar.ShortcutsEnabled = false;
		ribbonBar.AutoOverflowEnabled = false;
		ribbonBar.Style = Style;
		ribbonBar.BackgroundStyle.ApplyStyle(base.BackgroundStyle);
		ribbonBar.BackgroundMouseOverStyle.ApplyStyle(BackgroundMouseOverStyle);
		((Control)ribbonBar).Text = ((Control)this).Text;
		ribbonBar.TitleStyle.ApplyStyle(TitleStyle);
		ribbonBar.TitleStyleMouseOver.ApplyStyle(TitleStyleMouseOver);
		ribbonBar.LayoutOrientation = LayoutOrientation;
		CopyIOwnerEvents(ribbonBar);
		ribbonBar.DialogLauncherVisible = DialogLauncherVisible;
		ribbonBar.DialogLauncherMouseDown = this.DialogLauncherMouseDown;
		ribbonBar.DialogLauncherMouseEnter = this.DialogLauncherMouseEnter;
		ribbonBar.DialogLauncherMouseHover = this.DialogLauncherMouseHover;
		ribbonBar.DialogLauncherMouseLeave = this.DialogLauncherMouseLeave;
		ribbonBar.DialogLauncherButton = DialogLauncherButton;
		ribbonBar.DialogLauncherMouseOverButton = DialogLauncherMouseOverButton;
		ribbonBar.Images = Images;
		ribbonBar.ImagesLarge = ImagesLarge;
		ribbonBar.ImagesMedium = ImagesMedium;
		ribbonBar.FadeEffect = base.FadeEffect;
		ribbonBar.LaunchDialog = this.LaunchDialog;
		ribbonBar.ItemSpacing = ItemSpacing;
		ribbonBar.HorizontalItemAlignment = HorizontalItemAlignment;
		ribbonBar.IsOverflowRibbon = true;
		ribbonBar.AutoSizeItems = AutoSizeItems;
		ribbonBar.MaximumOverflowTextLength = MaximumOverflowTextLength;
		ribbonBar.VerticalItemAlignment = VerticalItemAlignment;
		if (createHandle && !BarFunctions.IsHandleValid((Control)(object)this))
		{
			((Control)this).CreateHandle();
			RecalcLayout();
		}
		if (!m_BeforeOverflowSize.IsEmpty)
		{
			Size beforeOverflowSize = m_BeforeOverflowSize;
			((Control)ribbonBar).Size = beforeOverflowSize;
		}
		else
		{
			((Control)ribbonBar).Size = GetFullContentBasedSize();
		}
		ribbonBar.TitleVisible = TitleVisible;
		((Control)ribbonBar).Visible = true;
		return ribbonBar;
	}

	private void SetupOverflowButton()
	{
		if (m_OverflowButton != null || m_ItemContainer.SubItems.Count == 0)
		{
			return;
		}
		m_SuspendRecalcSize = true;
		RibbonOverflowButtonItem ribbonOverflowButtonItem = new RibbonOverflowButtonItem();
		ribbonOverflowButtonItem.Name = "sysOverflowButton";
		ribbonOverflowButtonItem.Text = GetOverflowButtonText();
		ribbonOverflowButtonItem.RibbonBar = this;
		ribbonOverflowButtonItem.Image = GetOverflowButtonImage();
		ribbonOverflowButtonItem.ButtonStyle = eButtonStyle.ImageAndText;
		ribbonOverflowButtonItem.ImagePosition = eImagePosition.Top;
		ribbonOverflowButtonItem.AutoExpandOnClick = true;
		ribbonOverflowButtonItem.PopupType = ePopupType.Menu;
		ribbonOverflowButtonItem.ImagePaddingHorizontal = StiScale2.YYI(16.0);
		ribbonOverflowButtonItem.VerticalPadding = StiScale2.YYI(12.0);
		ribbonOverflowButtonItem.SubItemsExpandWidth = StiScale2.XXI(14.0);
		ribbonOverflowButtonItem.HotTrackingStyle = eHotTrackingStyle.None;
		ribbonOverflowButtonItem.PopupOpen += OverflowButton_PopupOpen;
		if (this.OverflowButtonSetup != null)
		{
			this.OverflowButtonSetup(this, new OverflowButtonEventArgs(ribbonOverflowButtonItem));
		}
		ArrayList arrayList = new ArrayList();
		m_ItemContainer.SubItems.CopyTo(arrayList);
		m_ItemContainer.SubItems.Clear();
		m_ItemContainer.SubItems.Add(ribbonOverflowButtonItem);
		m_OverflowRibbonBar = CreateOverflowRibbon();
		m_OverflowRibbonBar.OverflowParent = this;
		m_OriginalVerticalItemAlign = m_ItemContainer.VerticalItemAlignment;
		m_OriginalLayoutOrientation = m_ItemContainer.LayoutOrientation;
		m_OriginalHorizontalItemAlign = m_ItemContainer.HorizontalItemAlignment;
		m_ItemContainer.VerticalItemAlignment = eVerticalItemsAlignment.Top;
		m_ItemContainer.LayoutOrientation = eOrientation.Horizontal;
		m_ItemContainer.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		foreach (BaseItem item in arrayList)
		{
			m_OverflowRibbonBar.Items.Add(item);
			((IOwner)this).AddShortcutsFromItem(item);
		}
		ItemContainer itemContainer = new ItemContainer();
		itemContainer.Stretch = true;
		ControlContainerItem controlContainerItem = new ControlContainerItem();
		controlContainerItem.AllowItemResize = false;
		itemContainer.SubItems.Add(controlContainerItem);
		controlContainerItem.Control = (Control)(object)m_OverflowRibbonBar;
		ribbonOverflowButtonItem.SubItems.Add(itemContainer);
		m_ItemContainer.RefreshImageSize();
		m_OverflowButton = ribbonOverflowButtonItem;
		if (this.OverflowButtonSetupComplete != null)
		{
			this.OverflowButtonSetupComplete(this, new OverflowButtonEventArgs(m_OverflowButton));
		}
		m_SuspendRecalcSize = false;
	}

	protected override void OnClick(EventArgs e)
	{
		if (m_OverflowButton != null)
		{
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (!m_OverflowButton.DisplayRectangle.Contains(pt))
			{
				m_OverflowButton.Expanded = !m_OverflowButton.Expanded;
			}
		}
		base.OnClick(e);
	}

	private void OverflowButton_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		if (m_OverflowRibbonBar != null)
		{
			m_OverflowRibbonBar.BackgroundStyle.ApplyStyle(base.BackgroundStyle);
			m_OverflowRibbonBar.BackgroundMouseOverStyle.ApplyStyle(BackgroundMouseOverStyle);
			m_OverflowRibbonBar.TitleStyle.ApplyStyle(TitleStyle);
			m_OverflowRibbonBar.TitleStyleMouseOver.ApplyStyle(TitleStyleMouseOver);
			if (ShowKeyTips)
			{
				m_OverflowRibbonBar.ShowKeyTips = true;
			}
		}
	}

	private void DisposeOverflowButton()
	{
		if (m_OverflowButton == null)
		{
			return;
		}
		m_SuspendRecalcSize = true;
		if (this.OverflowButtonDestroy != null)
		{
			this.OverflowButtonDestroy(this, new OverflowButtonEventArgs(m_OverflowButton));
		}
		m_OverflowButton.PopupOpen -= OverflowButton_PopupOpen;
		m_ItemContainer.SubItems.Remove(m_OverflowButton);
		m_ItemContainer.VerticalItemAlignment = m_OriginalVerticalItemAlign;
		m_ItemContainer.LayoutOrientation = m_OriginalLayoutOrientation;
		m_ItemContainer.HorizontalItemAlignment = m_OriginalHorizontalItemAlign;
		ArrayList arrayList = new ArrayList();
		m_OverflowRibbonBar.Items.CopyTo(arrayList);
		m_OverflowRibbonBar.Items.Clear();
		m_OverflowButton.SubItems.Clear();
		foreach (BaseItem item in arrayList)
		{
			((IOwner)this).RemoveShortcutsFromItem(item);
			m_ItemContainer.SubItems.Add(item);
		}
		if (m_OverflowButtonImage == null)
		{
			m_OverflowButton.Image.Dispose();
			m_OverflowButton.Image = null;
		}
		m_OverflowButton.Dispose();
		((Component)(object)m_OverflowRibbonBar).Dispose();
		m_OverflowButton = null;
		m_OverflowRibbonBar = null;
		m_SuspendRecalcSize = false;
		m_BeforeOverflowSize = Size.Empty;
	}

	internal Image GetOverflowButtonImage()
	{
		if (m_OverflowButtonImage == null)
		{
			return (Image)(object)BarFunctions.LoadBitmap("SystemImages.RibbonOverflow.png");
		}
		return m_OverflowButtonImage;
	}

	private string GetOverflowButtonText()
	{
		string text = m_OverflowButtonText;
		if (text == "")
		{
			text = ((Control)this).Text;
			if (m_MaximumOverflowTextLength > 0 && text.Length >= m_MaximumOverflowTextLength)
			{
				text = text.Substring(0, m_MaximumOverflowTextLength) + "...";
			}
			else if (text.IndexOf(' ') > 0)
			{
				text += " <expand/>";
			}
		}
		return text;
	}

	protected override void OnResize(EventArgs e)
	{
		if (OverflowState)
		{
			m_OverflowButton.Bounds = GetItemContainerRectangle();
		}
		base.OnResize(e);
	}

	public void ResetCachedContentSize()
	{
		m_BeforeOverflowSize = Size.Empty;
	}

	internal Size GetContentBasedSize()
	{
		return GetControlSize(new Size(m_ItemContainer.WidthInternal, m_ItemContainer.HeightInternal));
	}

	private Size GetControlSize(Size innerContentSize)
	{
		ElementStyle backgroundStyle = GetBackgroundStyle();
		Size result = new Size(((innerContentSize.Width > 0) ? innerContentSize.Width : 32) + ElementStyleLayout.HorizontalStyleWhiteSpace(backgroundStyle), innerContentSize.Height + ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle));
		if (m_TitleVisible && !OverflowState)
		{
			result.Height += m_TitleRectangle.Height;
		}
		if (BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			result.Width++;
			result.Height += 4;
		}
		return result;
	}

	internal Size GetFullContentBasedSize()
	{
		if (!m_FullItemsSize.IsEmpty)
		{
			return GetControlSize(m_FullItemsSize);
		}
		return GetContentBasedSize();
	}

	public override void AutoSyncSize()
	{
		if (((Control)this).AutoSize && !OverflowState && !m_ItemContainer.NeedRecalcSize)
		{
			((Control)this).Height = GetContentBasedSize().Height;
		}
	}

	public override int GetAutoSizeHeight()
	{
		return GetContentBasedSize().Height;
	}

	public int GetAutoSizeWidth()
	{
		return GetContentBasedSize().Width;
	}

	protected override Rectangle GetItemContainerRectangle()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (m_TitleVisible && !OverflowState)
		{
			Rectangle titleRectangle = GetTitleRectangle();
			if (m_TitlePosition == eRibbonTitlePosition.Top)
			{
				titleRectangle.Height++;
				itemContainerRectangle.Y += titleRectangle.Height;
				itemContainerRectangle.Height -= titleRectangle.Height;
			}
			else
			{
				itemContainerRectangle.Height -= titleRectangle.Height;
			}
		}
		return itemContainerRectangle;
	}

	private Rectangle GetBackgroundRectangle()
	{
		return ((Control)this).ClientRectangle;
	}

	public Rectangle GetTitleRectangle()
	{
		return m_TitleRectangle;
	}

	protected override Font GetKeyTipFont()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			RibbonStrip ribbonStrip = ribbonControl.RibbonStrip;
			if (ribbonStrip.KeyTipsFont != null)
			{
				return ribbonStrip.KeyTipsFont;
			}
		}
		return base.GetKeyTipFont();
	}

	internal override void PaintContainerKeyTips(BaseItem container, BaseRenderer renderer, KeyTipsRendererEventArgs e)
	{
		base.PaintContainerKeyTips(container, renderer, e);
		if (DialogLauncherVisible && DialogLauncherKeyTip != "")
		{
			string keyTipsKeysStack = ((IKeyTipsControl)this).KeyTipsKeysStack;
			string dialogLauncherKeyTip = DialogLauncherKeyTip;
			if (!(keyTipsKeysStack != "") || dialogLauncherKeyTip.StartsWith(keyTipsKeysStack))
			{
				e.ReferenceObject = this;
				e.KeyTip = dialogLauncherKeyTip;
				Size keyTipsPadding = KeyTipsPainter.KeyTipsPadding;
				Size size = TextDrawing.MeasureString(e.Graphics, dialogLauncherKeyTip, e.Font);
				size.Width += keyTipsPadding.Width;
				size.Height += keyTipsPadding.Height;
				Rectangle bounds = new Rectangle(((Control)this).Width - size.Width, ((Control)this).Height + 4, size.Width, size.Height);
				e.Bounds = bounds;
				renderer.DrawKeyTips(e);
			}
		}
	}

	protected override Rectangle GetKeyTipCanvasBounds()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null)
		{
			Point point = ((Control)ribbonControl).PointToClient(((Control)this).PointToScreen(Point.Empty));
			return new Rectangle(point.X, point.Y - 8, ((Control)this).Bounds.Width + 12, ((Control)this).Bounds.Height + 20);
		}
		return new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
	}

	protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Size keyTipsPadding = KeyTipsPainter.KeyTipsPadding;
		Size size = TextDrawing.MeasureString(g, keyTip, font);
		size.Width += keyTipsPadding.Width;
		size.Height += keyTipsPadding.Height;
		Rectangle itemClientRectangle = GetItemClientRectangle(item);
		Rectangle result = new Rectangle(itemClientRectangle.Right - size.Width - 1, itemClientRectangle.Bottom - size.Height + size.Height / 3 + 8, size.Width, size.Height);
		if (item == m_OverflowButton)
		{
			result.Y = ((Control)this).Height - 2;
			result.X = itemClientRectangle.X + (itemClientRectangle.Width - size.Width - 2) / 2;
		}
		else if (m_UseSpecKeyTipsPositioning)
		{
			if ((double)item.HeightInternal * 1.2 >= (double)m_ItemContainer.HeightInternal || item is GalleryContainer)
			{
				result.Y = ((Control)this).Height - size.Height - 5;
				if (item is GalleryContainer)
				{
					result.X = itemClientRectangle.Right - (size.Width - 2) / 2;
				}
				else
				{
					result.X = itemClientRectangle.X + (itemClientRectangle.Width - size.Width - 2) / 2;
				}
			}
			else if ((double)itemClientRectangle.Bottom * 1.2 >= (double)m_ItemContainer.HeightInternal)
			{
				result.Y = ((Control)this).Height - size.Height - 5;
			}
			else if (item.Parent is ItemContainer && itemClientRectangle.Y >= 0 && itemClientRectangle.Y <= 12)
			{
				result.Y = 0;
			}
			else if (result.Bottom > ((Control)this).Height)
			{
				result.Y = ((Control)this).Height - size.Height;
			}
			else if ((double)itemClientRectangle.Y >= (double)((Control)this).Height * 0.25)
			{
				result.Y = (int)((double)((Control)this).Height * 0.4);
			}
		}
		else if (item is GalleryContainer)
		{
			result.X = itemClientRectangle.Right - (size.Width - 2) / 2;
		}
		return result;
	}

	private Rectangle GetItemClientRectangle(BaseItem item)
	{
		return item.DisplayRectangle;
	}

	protected override bool ProcessMnemonic(BaseItem container, char charCode)
	{
		if (((Control)this).Parent is IKeyTipsControl && !((IKeyTipsControl)((Control)this).Parent).ShowKeyTips)
		{
			return false;
		}
		bool flag = base.ProcessMnemonic(container, charCode);
		if (!flag && DialogLauncherVisible && DialogLauncherKeyTip != "")
		{
			string keyTipsKeysStack = ((IKeyTipsControl)this).KeyTipsKeysStack;
			string text = keyTipsKeysStack + charCode;
			text = text.ToUpper();
			if (DialogLauncherKeyTip.ToUpper() == text)
			{
				DoLaunchDialog();
				flag = true;
			}
			else if (DialogLauncherKeyTip.ToUpper().StartsWith(text))
			{
				((IKeyTipsControl)this).KeyTipsKeysStack += charCode.ToString().ToUpper();
			}
		}
		return flag;
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Invalid comparison between Unknown and I4
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Invalid comparison between Unknown and I4
		bool flag = m_MouseOver;
		bool flag2 = m_FadeImageState != null;
		if (flag2)
		{
			flag = false;
		}
		GraphicsPath val = null;
		ElementStyle elementStyle = GetBackgroundStyle();
		bool flag3 = flag2;
		try
		{
			if (elementStyle != null)
			{
				Rectangle backgroundRectangle = GetBackgroundRectangle();
				pa.Graphics.SetClip(backgroundRectangle, (CombineMode)0);
				ElementStyle backgroundMouseOverStyle = GetBackgroundMouseOverStyle();
				if (flag && elementStyle != null && backgroundMouseOverStyle != null && backgroundMouseOverStyle.Custom)
				{
					elementStyle = elementStyle.Copy();
					flag3 = true;
					elementStyle.ApplyStyle(backgroundMouseOverStyle);
				}
				ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(elementStyle, pa.Graphics, backgroundRectangle, EffectiveStyle == eDotNetBarStyle.Office2007);
				ElementStyleDisplay.Paint(elementStyleDisplayInfo);
				pa.Graphics.ResetClip();
				elementStyleDisplayInfo.Bounds = GetBackgroundRectangle();
				elementStyleDisplayInfo.Bounds.X--;
				elementStyleDisplayInfo.Bounds.Width++;
				val = ElementStyleDisplay.GetInsideClip(elementStyleDisplayInfo);
				elementStyleDisplayInfo.Bounds.X++;
				elementStyleDisplayInfo.Bounds.Width--;
			}
			if (val != null)
			{
				pa.Graphics.SetClip(val, (CombineMode)0);
			}
			m_DialogLauncherRect = Rectangle.Empty;
			if (m_TitleVisible && !OverflowState)
			{
				ElementStyle elementStyle2 = GetTitleStyle();
				ElementStyle titleMouseOverStyle = GetTitleMouseOverStyle();
				if (flag && elementStyle2 != null && titleMouseOverStyle != null && titleMouseOverStyle.Custom)
				{
					elementStyle2 = elementStyle2.Copy();
					elementStyle2.ApplyStyle(titleMouseOverStyle);
				}
				if (elementStyle2 != null)
				{
					SimpleNodeDisplayInfo simpleNodeDisplayInfo = new SimpleNodeDisplayInfo(elementStyle2, pa.Graphics, m_TitleElement, ((Control)this).Font, (int)((Control)this).RightToLeft == 1);
					if (m_DialogLauncherVisible)
					{
						if (m_DialogLauncherButton == null)
						{
							Rectangle textBounds = m_TitleElement.TextBounds;
							textBounds.Width -= m_TitleRectangle.Height;
							if ((int)((Control)this).RightToLeft == 1)
							{
								textBounds.X += m_TitleRectangle.Height;
							}
							simpleNodeDisplayInfo.TextBounds = textBounds;
						}
						else if (m_MouseOverDialogLauncher && m_DialogLauncherMouseOverButton != null)
						{
							m_TitleElement.Image = m_DialogLauncherMouseOverButton;
						}
						else
						{
							m_TitleElement.Image = m_DialogLauncherButton;
						}
					}
					SimpleNodeDisplay.Paint(simpleNodeDisplayInfo);
					if (m_DialogLauncherVisible && m_TitleElement.Image == null)
					{
						PaintDialogLauncher(pa);
					}
					else
					{
						m_DialogLauncherRect = m_TitleElement.ImageBounds;
					}
				}
			}
			pa.Graphics.ResetClip();
			m_FadeImageLock.AcquireReaderLock(-1);
			try
			{
				if (m_FadeImageState != null)
				{
					Graphics graphics = pa.Graphics;
					Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
					ColorMatrix val2 = new ColorMatrix();
					val2[3, 3] = (float)m_FadeAlpha / 255f;
					ImageAttributes val3 = new ImageAttributes();
					try
					{
						val3.SetColorMatrix(val2);
						graphics.DrawImage((Image)(object)m_FadeImageState, rectangle, 0, 0, rectangle.Width, rectangle.Height, (GraphicsUnit)2, val3);
						return;
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
			}
			finally
			{
				m_FadeImageLock.ReleaseReaderLock();
			}
		}
		finally
		{
			if (val != null)
			{
				val.Dispose();
			}
			if (flag3)
			{
				elementStyle?.Dispose();
			}
		}
	}

	private void PaintDialogLauncher(ItemPaintArgs pa)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Invalid comparison between Unknown and I4
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Invalid comparison between Unknown and I4
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Invalid comparison between Unknown and I4
		Rectangle rectangle = new Rectangle(m_TitleRectangle.Right - (m_TitleRectangle.Height - StiScale2.XXI(5.0)) - StiScale2.XXI(3.0), m_TitleRectangle.Y + StiScale2.YYI(2.0), m_TitleRectangle.Height - StiScale2.YYI(5.0), m_TitleRectangle.Height - StiScale2.YYI(5.0));
		if ((int)((Control)this).RightToLeft == 1)
		{
			rectangle.X = m_TitleRectangle.X;
		}
		Graphics graphics = pa.Graphics;
		if (BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			int num = Math.Min(StiScale2.XXI(15.0), m_TitleRectangle.Height - 1);
			rectangle = new Rectangle(m_TitleRectangle.Right - num - StiScale2.XXI(2.0), m_TitleRectangle.Y + (m_TitleRectangle.Height - num) / 2, num, num);
			if ((int)((Control)this).RightToLeft == 1)
			{
				rectangle.X = m_TitleRectangle.X + StiScale2.XXI(2.0);
			}
			RibbonBarRendererEventArgs ribbonBarRendererEventArgs = new RibbonBarRendererEventArgs(graphics, rectangle, this);
			if (m_MouseOverDialogLauncher)
			{
				if ((int)Control.MouseButtons == 1048576)
				{
					ribbonBarRendererEventArgs.Pressed = true;
				}
				else
				{
					ribbonBarRendererEventArgs.MouseOver = true;
				}
			}
			GetRenderer().DrawRibbonDialogLauncher(ribbonBarRendererEventArgs);
			m_DialogLauncherRect = rectangle;
			return;
		}
		if (base.AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)0;
		}
		LinearGradientBrush val = DisplayHelp.CreateLinearGradientBrush(rectangle, ControlPaint.LightLight(pa.Colors.BarBackground), pa.Colors.BarBackground, 90f);
		try
		{
			graphics.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Rectangle rectangle2 = new Rectangle(rectangle.X + (int)Math.Ceiling((float)(rectangle.Width - StiScale2.XXI(8.0)) / 2f), rectangle.Bottom - StiScale2.YYI(4.0), StiScale2.XXI(2.0), StiScale2.YYI(2.0));
		graphics.FillRectangle(SystemBrushes.ControlText, rectangle2);
		rectangle2.Offset(rectangle2.Width + StiScale2.XXI(1.0), 0);
		graphics.FillRectangle(SystemBrushes.ControlText, rectangle2);
		rectangle2.Offset(rectangle2.Width + StiScale2.XXI(1.0), 0);
		graphics.FillRectangle(SystemBrushes.ControlText, rectangle2);
		if (base.AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		m_DialogLauncherRect = rectangle;
	}

	public void ResetTitleStyle()
	{
		m_TitleStyle.StyleChanged -= ElementStyleChanged;
		m_TitleStyle = new ElementStyle();
		m_TitleStyle.StyleChanged += ElementStyleChanged;
		((Control)this).Invalidate();
	}

	public void ResetTitleStyleMouseOver()
	{
		m_TitleStyleMouseOver.StyleChanged -= ElementStyleChanged;
		m_TitleStyleMouseOver = new ElementStyle();
		m_TitleStyleMouseOver.StyleChanged += ElementStyleChanged;
		((Control)this).Invalidate();
	}

	private void ElementStyleChanged(object sender, EventArgs e)
	{
		if (((Component)(object)this).DesignMode)
		{
			RecalcLayout();
		}
	}

	private void SetMouseOver(bool value)
	{
		if (value == m_MouseOver)
		{
			return;
		}
		if (base.IsFadeEnabled && ((Control)this).Enabled && ((Control)this).Visible && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				m_FadeImageLock.AcquireReaderLock(-1);
				try
				{
					flag = m_FadeImageState == null;
				}
				finally
				{
					m_FadeImageLock.ReleaseReaderLock();
				}
				if (flag)
				{
					m_MouseOver = true;
					bool isReaderLockHeld = m_FadeImageLock.IsReaderLockHeld;
					LockCookie lockCookie = default(LockCookie);
					if (isReaderLockHeld)
					{
						lockCookie = m_FadeImageLock.UpgradeToWriterLock(-1);
					}
					else
					{
						m_FadeImageLock.AcquireWriterLock(-1);
					}
					try
					{
						m_FadeImageState = GetCurrentStateImage();
					}
					finally
					{
						if (isReaderLockHeld)
						{
							m_FadeImageLock.DowngradeFromWriterLock(ref lockCookie);
						}
						else
						{
							m_FadeImageLock.ReleaseWriterLock();
						}
					}
				}
			}
			catch (ApplicationException)
			{
				flag2 = true;
			}
			m_FadeDirection = 1;
			m_MouseOver = value;
			if (!flag2)
			{
				if (!m_MouseOver)
				{
					m_FadeDirection = -1;
				}
				else
				{
					m_FadeAlpha = 10;
				}
				FadeAnimator.Fade(this, OnFadeChanged);
				m_FadeAnimation = true;
			}
		}
		else
		{
			m_MouseOver = value;
		}
	}

	private void StopFade()
	{
		if (m_FadeAnimation)
		{
			m_FadeAnimation = false;
			FadeAnimator.StopFade(this, OnFadeChanged);
			bool flag = false;
			m_FadeImageLock.AcquireReaderLock(-1);
			try
			{
				flag = m_FadeImageState != null;
			}
			finally
			{
				m_FadeImageLock.ReleaseReaderLock();
			}
			if (flag)
			{
				DisposeFadeImage();
			}
			if (m_FadeAlpha > 230)
			{
				m_FadeAlpha = 255;
			}
			else if (m_FadeAlpha < 0)
			{
				m_FadeAlpha = 0;
			}
		}
	}

	private void DisposeFadeImage()
	{
		bool isReaderLockHeld = m_FadeImageLock.IsReaderLockHeld;
		LockCookie lockCookie = default(LockCookie);
		if (isReaderLockHeld)
		{
			lockCookie = m_FadeImageLock.UpgradeToWriterLock(-1);
		}
		else
		{
			m_FadeImageLock.AcquireWriterLock(-1);
		}
		try
		{
			((Image)m_FadeImageState).Dispose();
			m_FadeImageState = null;
		}
		finally
		{
			if (isReaderLockHeld)
			{
				m_FadeImageLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				m_FadeImageLock.ReleaseWriterLock();
			}
		}
	}

	private void OnFadeChanged(object sender, EventArgs e)
	{
		m_FadeAlpha += m_FadeDirection * 65;
		if ((m_FadeDirection < 0 && m_FadeAlpha <= 0) || (m_FadeDirection > 0 && m_FadeAlpha >= 255))
		{
			StopFade();
		}
		((Control)this).Invalidate();
	}

	private Bitmap GetCurrentStateImage()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = new Bitmap(((Control)this).Width, ((Control)this).Height, (PixelFormat)925707);
		val.MakeTransparent();
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			bool antiAlias = base.AntiAlias;
			ColorScheme colorScheme = GetColorScheme();
			if (antiAlias)
			{
				val2.SmoothingMode = (SmoothingMode)4;
				val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			ItemPaintArgs itemPaintArgs = GetItemPaintArgs(val2);
			PaintControlBackground(itemPaintArgs);
			return val;
		}
		finally
		{
			val2.Dispose();
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		StopFade();
		base.OnHandleDestroyed(e);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible)
		{
			StopFade();
			if (IsOverflowRibbon)
			{
				((IOwner)this).OnApplicationDeactivate();
			}
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}
}
