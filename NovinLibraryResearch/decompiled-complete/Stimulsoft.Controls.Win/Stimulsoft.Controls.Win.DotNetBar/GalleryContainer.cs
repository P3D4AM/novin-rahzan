using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.ScrollBar;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[ProvideProperty("GalleryGroup", typeof(BaseItem))]
public class GalleryContainer : ItemContainer, IExtenderProvider
{
	private class GroupDisplayOrderComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			if (x is GalleryGroup && y is GalleryGroup)
			{
				return ((GalleryGroup)x).DisplayOrder - ((GalleryGroup)y).DisplayOrder;
			}
			return new CaseInsensitiveComparer().Compare(x, y);
		}
	}

	private bool m_EnableGalleryPopup = true;

	private ButtonItem m_ScrollUp;

	private ButtonItem m_ScrollDown;

	private ButtonItem m_PopupGallery;

	private Size m_ScrollButtonSize = new Size(15, 20);

	private int m_ScrollButtonContentSpacing = 1;

	private Size m_DefaultSize = Size.Empty;

	private Point m_ScrollPosition = Point.Empty;

	private Size m_PopupGallerySize = Size.Empty;

	private ScrollBarCore m_ScrollBar;

	private bool m_PopupGalleryStyle;

	private new Rectangle m_ViewRectangle = Rectangle.Empty;

	private Size m_RecommendedSize = Size.Empty;

	private bool m_StretchGallery;

	internal bool SystemGallery;

	private bool m_IncrementalSizing = true;

	private GalleryGroupCollection m_Groups;

	private bool m_ScrollAnimation = true;

	private ThreadUIOperation m_ThreadUI;

	private bool m_PopupUsesStandardScrollbars = true;

	private bool m_MouseDownOnScrollBar;

	private bool _SettingScrollPosition;

	private Bitmap m_StateBitmap;

	private int m_AnimationEndPos;

	private int m_AnimationCurrentPos;

	private int m_AnimationStep;

	internal bool PopupGalleryStyle
	{
		get
		{
			return m_PopupGalleryStyle;
		}
		set
		{
			m_PopupGalleryStyle = value;
			if (m_ScrollBar != null)
			{
				m_ScrollBar.Dispose();
				m_ScrollBar = null;
			}
			if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars)
			{
				m_ScrollBar = new ScrollBarCore();
				m_ScrollBar.ValueChanged += PopupScrollBar_ValueChanged;
			}
		}
	}

	internal Size RecommendedSize
	{
		get
		{
			return m_RecommendedSize;
		}
		set
		{
			m_RecommendedSize = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle PopupGalleryButtonBounds => m_PopupGallery.DisplayRectangle;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ButtonItem PopupGalleryItem => m_PopupGallery;

	[Browsable(false)]
	[DefaultValue(eOrientation.Horizontal)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Category("Layout")]
	public override eOrientation LayoutOrientation
	{
		get
		{
			return base.LayoutOrientation;
		}
		set
		{
			base.LayoutOrientation = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override eHorizontalItemsAlignment HorizontalItemAlignment
	{
		get
		{
			return base.HorizontalItemAlignment;
		}
		set
		{
			base.HorizontalItemAlignment = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override eVerticalItemsAlignment VerticalItemAlignment
	{
		get
		{
			return base.VerticalItemAlignment;
		}
		set
		{
			base.VerticalItemAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Layout")]
	[Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
	public override bool MultiLine
	{
		get
		{
			return base.MultiLine;
		}
		set
		{
			base.MultiLine = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool ResizeItemsToFit
	{
		get
		{
			return base.ResizeItemsToFit;
		}
		set
		{
			base.ResizeItemsToFit = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether Gallery when on popup is using standard scrollbars to scroll the content.")]
	public bool PopupUsesStandardScrollbars
	{
		get
		{
			return m_PopupUsesStandardScrollbars;
		}
		set
		{
			m_PopupUsesStandardScrollbars = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Layout")]
	[Description("Indicates whether gallery is using incremental sizing when stretched.")]
	public bool IncrementalSizing
	{
		get
		{
			return m_IncrementalSizing;
		}
		set
		{
			m_IncrementalSizing = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether Gallery width is determined based on the Ribbon container size.")]
	public bool StretchGallery
	{
		get
		{
			return m_StretchGallery;
		}
		set
		{
			if (m_StretchGallery == value)
			{
				return;
			}
			m_StretchGallery = value;
			if (ContainerControl is RibbonBar ribbonBar)
			{
				if (m_StretchGallery)
				{
					if (ribbonBar.GalleryStretch == null)
					{
						ribbonBar.GalleryStretch = this;
					}
				}
				else if (ribbonBar.GalleryStretch == this)
				{
					ribbonBar.GalleryStretch = null;
				}
			}
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection PopupGalleryItems => m_PopupGallery.SubItems;

	[Browsable(true)]
	[Category("Layout")]
	[Description("Indicates the default size of the gallery.")]
	public Size DefaultSize
	{
		get
		{
			return m_DefaultSize;
		}
		set
		{
			m_DefaultSize = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[Category("Layout")]
	[Description("Indicates the default size of the gallery when gallery is displayed on the popup menu.")]
	public Size PopupGallerySize
	{
		get
		{
			return m_PopupGallerySize;
		}
		set
		{
			m_PopupGallerySize = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Gallery")]
	[Description("Indicates whether gallery can be displayed on the popup.")]
	public bool EnableGalleryPopup
	{
		get
		{
			return m_EnableGalleryPopup;
		}
		set
		{
			m_EnableGalleryPopup = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	internal ScrollBarCore ScrollBarControl => m_ScrollBar;

	internal bool IsAtMinimumSize
	{
		get
		{
			if (m_IncrementalSizing && SubItems.Count > 0)
			{
				return WidthInternal - SubItems[0].WidthInternal <= base.MinimumSize.Width;
			}
			return WidthInternal <= base.MinimumSize.Width;
		}
	}

	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			base.Bounds = value;
			RepositionButtons(DisplayRectangle);
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			base.Expanded = value;
			if (!value && m_PopupGallery.Expanded)
			{
				m_PopupGallery.Expanded = false;
			}
		}
	}

	[Browsable(false)]
	public bool IsGalleryPopupOpen => m_PopupGallery.Expanded;

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
	public override string KeyTips
	{
		get
		{
			return base.KeyTips;
		}
		set
		{
			base.KeyTips = value;
		}
	}

	private bool IsAnimationEnabled
	{
		get
		{
			if (DesignMode || !m_ScrollAnimation || !Displayed || TextDrawing.UseTextRenderer || NativeFunctions.IsTerminalSession())
			{
				return false;
			}
			return m_ScrollAnimation;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Animation")]
	[Description("Indicates whether scroll animation is enabled.")]
	public bool ScrollAnimation
	{
		get
		{
			return m_ScrollAnimation;
		}
		set
		{
			m_ScrollAnimation = value;
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.GalleryGroupCollectionEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Groups")]
	[Browsable(false)]
	[Description("Gets the collection of GalleryGroup objects associated with this gallery.")]
	public GalleryGroupCollection GalleryGroups => m_Groups;

	[Description("Occurs when Gallery popup item is about to open.")]
	public event DotNetBarManager.PopupOpenEventHandler GalleryPopupOpen;

	[Description("Occurs just before Gallery popup window is shown.")]
	public event EventHandler GalleryPopupShowing;

	[Description("Occurs before the Gallery popup item is closed")]
	public event EventHandler GalleryPopupClose;

	[Description("Occurs after popup item has been closed.")]
	public event EventHandler GalleryPopupFinalized;

	public GalleryContainer()
	{
		m_DefaultSize = GetDefaultSize();
		m_PopupGallerySize = GetDefaultPopupSize();
		m_Groups = new GalleryGroupCollection();
		m_Groups.Owner = this;
		MultiLine = true;
		CreateButtons();
		base.MinimumSize = new Size(42 + m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing, m_DefaultSize.Height);
		m_ThreadUI = new ThreadUIOperation(RecordStartState, UpdateThreadedUI, CleanupState);
	}

	private Size GetDefaultSize()
	{
		return new Size(140 + m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing, m_ScrollButtonSize.Height * 3 - 2);
	}

	private Size GetDefaultPopupSize()
	{
		return Size.Empty;
	}

	public override BaseItem Copy()
	{
		GalleryContainer galleryContainer = new GalleryContainer();
		CopyToItem(galleryContainer);
		return galleryContainer;
	}

	protected override void CopyToItem(BaseItem c)
	{
		GalleryContainer galleryContainer = c as GalleryContainer;
		galleryContainer.DefaultSize = DefaultSize;
		galleryContainer.PopupGallerySize = PopupGallerySize;
		galleryContainer.StretchGallery = StretchGallery;
		galleryContainer.PopupUsesStandardScrollbars = PopupUsesStandardScrollbars;
		foreach (BaseItem popupGalleryItem in PopupGalleryItems)
		{
			galleryContainer.PopupGalleryItems.Add(popupGalleryItem.Copy());
		}
		base.CopyToItem((BaseItem)galleryContainer);
		foreach (GalleryGroup galleryGroup3 in GalleryGroups)
		{
			GalleryGroup galleryGroup2 = new GalleryGroup();
			galleryGroup2.Name = galleryGroup3.Name;
			galleryGroup2.Text = galleryGroup3.Text;
			galleryGroup2.DisplayOrder = galleryGroup3.DisplayOrder;
			galleryContainer.GalleryGroups.Add(galleryGroup2);
			foreach (BaseItem item in galleryGroup3.Items)
			{
				galleryContainer.SetGalleryGroup(galleryContainer.SubItems[item.Name], galleryGroup2);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (m_ScrollBar != null)
		{
			m_ScrollBar.Dispose();
			m_ScrollBar = null;
		}
		m_ThreadUI.Stop();
		base.Dispose(disposing);
	}

	private void CreateButtons()
	{
		m_ScrollUp = new ButtonItem("sysgalleryscrollup");
		m_ScrollUp.Text = "<expand direction=\"top\"/>";
		m_ScrollUp.Style = Style;
		m_ScrollUp.ColorTable = eButtonColor.OrangeWithBackground;
		m_ScrollUp.SetParent(this);
		m_ScrollUp.Click += ScrollUp_Click;
		m_ScrollUp.ClickAutoRepeat = true;
		m_ScrollUp.ClickRepeatInterval = 200;
		m_ScrollUp.CanCustomize = false;
		m_ScrollUp.AutoCollapseOnClick = false;
		m_ScrollDown = new ButtonItem("sysgalleryscrolldown");
		m_ScrollDown.Text = "<expand direction=\"bottom\"/>";
		m_ScrollDown.Style = Style;
		m_ScrollDown.ColorTable = eButtonColor.OrangeWithBackground;
		m_ScrollDown.SetParent(this);
		m_ScrollDown.Click += ScrollDown_Click;
		m_ScrollDown.ClickAutoRepeat = true;
		m_ScrollDown.ClickRepeatInterval = 200;
		m_ScrollDown.CanCustomize = false;
		m_ScrollDown.AutoCollapseOnClick = false;
		m_PopupGallery = new ButtonItem("sysgallerypopup");
		m_PopupGallery.Text = "<expand direction=\"popup\"/>";
		m_PopupGallery.ButtonStyle = eButtonStyle.TextOnlyAlways;
		m_PopupGallery.Style = Style;
		m_PopupGallery.ColorTable = eButtonColor.OrangeWithBackground;
		m_PopupGallery.SetParent(this);
		m_PopupGallery.Click += PopupGallery_Click;
		m_PopupGallery.CanCustomize = false;
		m_PopupGallery.PopupOpen += PopupGallery_PopupOpen;
		m_PopupGallery.PopupShowing += PopupGallery_PopupShowing;
		m_PopupGallery.PopupClose += PopupGallery_PopupClose;
		m_PopupGallery.PopupFinalized += PopupGallery_PopupFinalized;
	}

	private void PopupGallery_PopupShowing(object sender, EventArgs e)
	{
		OnGalleryPopupShowing(sender, e);
	}

	private void PopupGallery_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		OnGalleryPopupOpen(sender, e);
	}

	private void PopupGallery_Click(object sender, EventArgs e)
	{
		PopupGallery();
	}

	private Size GetPopupGallerySize(Point screenPos)
	{
		Size result = Size.Empty;
		if (DisplayRectangle.Width < DefaultSize.Width || m_ViewRectangle.Size.IsEmpty)
		{
			int num = DefaultSize.Width;
			int num2 = 0;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem.Visible)
				{
					num2 += subItem.WidthInternal + base.ItemSpacing;
					if (num2 > num)
					{
						break;
					}
				}
			}
			if (num2 > 0)
			{
				num = num2;
				if (m_PopupUsesStandardScrollbars)
				{
					num += m_ScrollButtonContentSpacing + m_ScrollButtonSize.Width;
				}
			}
			result = new Size(num, Math.Max(DefaultSize.Height, m_ViewRectangle.Height));
		}
		else
		{
			result = new Size(DisplayRectangle.Width, m_ViewRectangle.Height);
		}
		ScreenInformation screenInformation = null;
		screenInformation = BarFunctions.ScreenFromPoint(screenPos);
		if (m_Groups.Count > 0 && screenInformation != null && screenInformation.WorkingArea.Bottom - screenPos.Y > result.Height)
		{
			result.Height = screenInformation.WorkingArea.Bottom - screenPos.Y;
		}
		if (screenInformation != null)
		{
			int num3 = 16;
			num3 += PopupGalleryItems.Count * 26;
			int num4 = 0;
			int num5 = 0;
			if (screenPos.Y + result.Height > screenInformation.WorkingArea.Bottom - num3)
			{
				num4 = screenPos.Y + result.Height - (screenInformation.WorkingArea.Bottom - num3);
			}
			if (screenPos.X + result.Width > screenInformation.WorkingArea.Right - num3)
			{
				num5 = screenPos.X + result.Width - (screenInformation.WorkingArea.Right - num3);
			}
			if (result.Width - num5 > base.MinimumSize.Width)
			{
				result.Width -= num5;
			}
			else
			{
				result.Width = base.MinimumSize.Width + m_ScrollButtonContentSpacing + m_ScrollButtonSize.Width;
			}
			if (result.Height - num4 > base.MinimumSize.Height)
			{
				result.Height -= num4;
			}
			else
			{
				result.Height = base.MinimumSize.Height;
			}
		}
		return result;
	}

	private void ClearPopupGalleryItems()
	{
		if (DesignMode)
		{
			return;
		}
		foreach (BaseItem subItem in m_PopupGallery.SubItems)
		{
			if (subItem is GalleryContainer { SystemGallery: not false } galleryContainer)
			{
				m_PopupGallery.SubItems.Remove(galleryContainer);
				galleryContainer.Dispose();
				break;
			}
		}
	}

	private void PopupGallery_PopupFinalized(object sender, EventArgs e)
	{
		OnGalleryPopupFinalized(sender, e);
		ClearPopupGalleryItems();
	}

	private void PopupGallery_PopupClose(object sender, EventArgs e)
	{
		OnGalleryPopupClose(sender, e);
		RestoreGalleryItems();
	}

	private void RestoreGalleryItems()
	{
	}

	private void ScrollUp_Click(object sender, EventArgs e)
	{
		ScrollUp();
	}

	private void ScrollDown_Click(object sender, EventArgs e)
	{
		ScrollDown();
	}

	internal bool ScrollHitTest(int x, int y)
	{
		if (m_ScrollUp.Enabled && m_ScrollUp.DisplayRectangle.Contains(x, y))
		{
			return true;
		}
		if (m_ScrollDown.Enabled && m_ScrollDown.DisplayRectangle.Contains(x, y))
		{
			return true;
		}
		if (m_ScrollBar != null && m_ScrollBar.Visible)
		{
			if (m_ScrollBar.ThumbIncreaseRectangle.Contains(x, y))
			{
				return true;
			}
			if (m_ScrollBar.ThumbDecreaseRectangle.Contains(x, y))
			{
				return true;
			}
		}
		return false;
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (base.IsDisposed)
		{
			return;
		}
		m_MouseDownOnScrollBar = false;
		if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars)
		{
			m_ScrollBar.MouseDown(objArg);
			if (m_ScrollBar.DisplayRectangle.Contains(objArg.X, objArg.Y))
			{
				m_MouseDownOnScrollBar = true;
			}
		}
		base.InternalMouseDown(objArg);
		if (!DesignMode)
		{
			return;
		}
		if (m_ScrollUp.Enabled && m_ScrollUp.DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			ScrollUp();
		}
		else if (m_ScrollDown.Enabled && m_ScrollDown.DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			ScrollDown();
		}
		else if (m_PopupGallery.Enabled && m_PopupGallery.DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			PopupGallery();
		}
		else if (m_ScrollBar != null && m_ScrollBar.Visible)
		{
			if (m_ScrollBar.ThumbIncreaseRectangle.Contains(objArg.X, objArg.Y))
			{
				ScrollDown();
			}
			else if (m_ScrollBar.ThumbDecreaseRectangle.Contains(objArg.X, objArg.Y))
			{
				ScrollUp();
			}
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (m_PopupGalleryStyle && !base.IsDisposed && m_PopupUsesStandardScrollbars)
		{
			m_ScrollBar.MouseMove(objArg);
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars && !base.IsDisposed)
		{
			m_ScrollBar.MouseUp(objArg);
			if (m_ScrollBar.DisplayRectangle.Contains(objArg.X, objArg.Y) || m_MouseDownOnScrollBar)
			{
				m_MouseDownOnScrollBar = false;
				return;
			}
		}
		base.InternalMouseUp(objArg);
	}

	public override void InternalMouseLeave()
	{
		if (m_PopupGalleryStyle && !base.IsDisposed && m_PopupUsesStandardScrollbars)
		{
			m_ScrollBar.MouseLeave();
		}
		base.InternalMouseLeave();
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		if (IsGalleryPopupOpen)
		{
			m_PopupGallery.InternalKeyDown(objArg);
		}
		else
		{
			base.InternalKeyDown(objArg);
		}
	}

	protected override void OnHotSubItemChanged(BaseItem newValue, BaseItem oldValue)
	{
		base.OnHotSubItemChanged(newValue, oldValue);
		if (newValue != null && newValue != oldValue && !GetClientRectangle().Contains(newValue.Bounds))
		{
			EnsureVisible(newValue);
		}
	}

	private Rectangle GetClientRectangle()
	{
		Rectangle displayRectangle = DisplayRectangle;
		if (m_ScrollBar != null)
		{
			displayRectangle.Width -= m_ScrollBar.DisplayRectangle.Width;
		}
		if (m_PopupGalleryStyle && !m_PopupUsesStandardScrollbars)
		{
			if (m_ScrollUp != null)
			{
				displayRectangle.Y += m_ScrollUp.HeightInternal;
				displayRectangle.Height -= m_ScrollUp.HeightInternal;
			}
			if (m_ScrollDown != null)
			{
				displayRectangle.Height -= m_ScrollDown.HeightInternal;
			}
		}
		else if (m_ScrollUp != null)
		{
			displayRectangle.Width -= m_ScrollUp.WidthInternal;
		}
		return displayRectangle;
	}

	protected virtual void OnGalleryPopupOpen(object sender, PopupOpenEventArgs e)
	{
		if (this.GalleryPopupOpen != null)
		{
			this.GalleryPopupOpen(sender, e);
		}
	}

	protected virtual void OnGalleryPopupShowing(object sender, EventArgs e)
	{
		if (this.GalleryPopupShowing != null)
		{
			this.GalleryPopupShowing(sender, e);
		}
	}

	protected virtual void OnGalleryPopupClose(object sender, EventArgs e)
	{
		if (this.GalleryPopupClose != null)
		{
			this.GalleryPopupClose(sender, e);
		}
	}

	protected virtual void OnGalleryPopupFinalized(object sender, EventArgs e)
	{
		if (this.GalleryPopupFinalized != null)
		{
			this.GalleryPopupFinalized(sender, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDefaultSize()
	{
		return m_DefaultSize != GetDefaultSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefaultSize()
	{
		TypeDescriptor.GetProperties(this)["DefaultSize"].SetValue(this, GetDefaultSize());
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePopupGallerySize()
	{
		return m_PopupGallerySize != GetDefaultPopupSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPopupGallerySize()
	{
		TypeDescriptor.GetProperties(this)["PopupGallerySize"].SetValue(this, GetDefaultPopupSize());
	}

	public void EnsureVisible(BaseItem item)
	{
		if (item == null)
		{
			throw new NullReferenceException("Item must be non null value.");
		}
		int y = GetScrollPosition().Y;
		Rectangle clientRectangle = GetClientRectangle();
		if (!clientRectangle.IsEmpty && (item.TopInternal + y < clientRectangle.Y || item.Bounds.Bottom > clientRectangle.Bottom))
		{
			int num = clientRectangle.Top - item.TopInternal + y;
			if (item.Bounds.Bottom > clientRectangle.Bottom)
			{
				num = -(item.Bounds.Bottom - clientRectangle.Y - clientRectangle.Height) + y;
			}
			if (Math.Abs(num) <= 4)
			{
				num = 0;
			}
			SetScrollPosition(num);
		}
	}

	public void ScrollDown()
	{
		SetScrollPosition(GetNextScrollLine());
	}

	public void ScrollUp()
	{
		SetScrollPosition(GetPreviousScrollLine());
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (!DesignMode && (!m_PopupGalleryStyle || !m_PopupUsesStandardScrollbars))
		{
			if (m_ScrollUp.DisplayRectangle.Contains(x, y) && m_ScrollUp.Visible)
			{
				return m_ScrollUp;
			}
			if (m_ScrollDown.DisplayRectangle.Contains(x, y) && m_ScrollDown.Visible)
			{
				return m_ScrollDown;
			}
			if (m_EnableGalleryPopup && m_PopupGallery.DisplayRectangle.Contains(x, y))
			{
				return m_PopupGallery;
			}
		}
		else if (!DesignMode && m_PopupGalleryStyle && m_PopupUsesStandardScrollbars && !SystemGallery && BaseItem.IsOnPopup(this) && m_ScrollBar != null && (m_ScrollBar.DisplayRectangle.Contains(x, y) || m_ScrollBar.IsMouseDown))
		{
			return this;
		}
		if (m_MouseDownOnScrollBar)
		{
			return this;
		}
		return base.ItemAtLocation(x, y);
	}

	private void PopupScrollBar_ValueChanged(object sender, EventArgs e)
	{
		if (!_SettingScrollPosition)
		{
			SetScrollPosition(-m_ScrollBar.Value);
		}
	}

	public void SetScrollPosition(int y)
	{
		if (m_ScrollPosition.Y != y)
		{
			_SettingScrollPosition = true;
			int y2 = y - m_ScrollPosition.Y;
			int y3 = m_ScrollPosition.Y;
			m_ScrollPosition.Y = y;
			OffsetSubItems(new Point(0, y2));
			if (!m_PopupGalleryStyle || !m_PopupUsesStandardScrollbars)
			{
				EnableScrollButtons(DisplayRectangle);
			}
			if (IsAnimationEnabled)
			{
				StartAnimation(y3, y);
			}
			else
			{
				Refresh();
			}
			if (m_ScrollBar != null && m_ScrollBar.Value != -y)
			{
				m_ScrollBar.Value = -y;
			}
			_SettingScrollPosition = false;
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		m_ThreadUI.StartReadOperation();
		try
		{
			if (m_StateBitmap != null)
			{
				base.PaintBackground(p);
				Graphics graphics = p.Graphics;
				Rectangle clipRectangle = GetClipRectangle();
				Region clip = graphics.Clip;
				graphics.SetClip(clipRectangle, (CombineMode)1);
				p.Graphics.DrawImage((Image)(object)m_StateBitmap, clipRectangle.X, clipRectangle.Y + m_AnimationCurrentPos);
				if (clip != null)
				{
					graphics.Clip = clip;
				}
				else
				{
					graphics.ResetClip();
				}
			}
			else
			{
				base.Paint(p);
			}
		}
		finally
		{
			m_ThreadUI.EndReadOperation();
		}
		if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars)
		{
			if (m_ScrollBar.ParentControl == null)
			{
				m_ScrollBar.ParentControl = p.ContainerControl;
			}
			m_ScrollBar.Paint(p);
			return;
		}
		if (m_ScrollUp.Visible)
		{
			m_ScrollUp.Paint(p);
		}
		if (m_ScrollDown.Visible)
		{
			m_ScrollDown.Paint(p);
		}
		if (m_EnableGalleryPopup)
		{
			m_PopupGallery.Paint(p);
		}
	}

	private Point GetScrollPosition()
	{
		return m_ScrollPosition;
	}

	private Size GetLayoutSize()
	{
		if (!m_RecommendedSize.IsEmpty && m_RecommendedSize.Width >= base.MinimumSize.Width && m_RecommendedSize.Height >= base.MinimumSize.Height)
		{
			return m_RecommendedSize;
		}
		Size defaultSize = m_DefaultSize;
		if (PopupGalleryStyle && !SystemGallery)
		{
			if (base.MinimumSize.Width > defaultSize.Width)
			{
				defaultSize.Width = base.MinimumSize.Width;
			}
			if (base.MinimumSize.Height > defaultSize.Height)
			{
				defaultSize.Height = base.MinimumSize.Height;
			}
		}
		return defaultSize;
	}

	protected override Rectangle GetLayoutRectangle(Rectangle bounds)
	{
		Rectangle result = new Rectangle(bounds.Location, GetLayoutSize());
		if (m_PopupUsesStandardScrollbars || !m_PopupGalleryStyle)
		{
			result.Width -= m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing;
			if (IsRightToLeft)
			{
				result.X += m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing;
			}
		}
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(base.BackgroundStyle, out dispose);
		result.X += ElementStyleLayout.LeftWhiteSpace(elementStyle);
		result.Y += ElementStyleLayout.TopWhiteSpace(elementStyle);
		result.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(elementStyle);
		result.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle);
		result.Offset(GetScrollPosition());
		if (dispose)
		{
			elementStyle.Dispose();
		}
		return result;
	}

	protected override Rectangle LayoutItems(Rectangle bounds)
	{
		m_ViewRectangle = base.LayoutItems(bounds);
		if (m_ViewRectangle.Height < Math.Abs(m_ScrollPosition.Y))
		{
			SetScrollPosition(0);
		}
		Size layoutSize = GetLayoutSize();
		if (((SystemGallery && !EnableGalleryPopup) || base.IsOnMenu) && layoutSize.Height > m_ViewRectangle.Height)
		{
			layoutSize.Height = m_ViewRectangle.Height;
		}
		if (m_IncrementalSizing && !PopupGalleryStyle)
		{
			layoutSize.Width = m_ViewRectangle.Width + (m_ScrollButtonSize.Width + m_ScrollButtonContentSpacing);
		}
		Rectangle rectangle = new Rectangle(bounds.Location, layoutSize);
		int num = m_ScrollButtonSize.Height * 2 - 1;
		if (m_EnableGalleryPopup)
		{
			num += m_ScrollButtonSize.Height - 1;
		}
		rectangle.Height = Math.Max(num, rectangle.Height);
		RepositionButtons(rectangle);
		if (m_PopupGalleryStyle && m_PopupUsesStandardScrollbars)
		{
			if (m_ViewRectangle.Bottom - rectangle.Bottom > 0)
			{
				m_ScrollBar.Enabled = true;
				m_ScrollBar.Maximum = m_ViewRectangle.Bottom - rectangle.Bottom;
				m_ScrollBar.Minimum = 0;
				m_ScrollBar.SmallChange = GetScrollLineHeight();
				m_ScrollBar.LargeChange = (int)((float)m_ScrollBar.Maximum * ((float)rectangle.Height / (float)m_ViewRectangle.Height));
			}
			else
			{
				m_ScrollBar.Enabled = false;
			}
		}
		else
		{
			EnableScrollButtons(rectangle);
		}
		return rectangle;
	}

	protected override Size GetEmptyDesignTimeSize()
	{
		if (m_PopupGalleryStyle && !SystemGallery)
		{
			Size minimumSize = base.MinimumSize;
			if (!minimumSize.IsEmpty)
			{
				return minimumSize;
			}
		}
		return base.GetEmptyDesignTimeSize();
	}

	private void EnableScrollButtons(Rectangle displayRect)
	{
		if (GetScrollPosition().Y == 0)
		{
			m_ScrollUp.Enabled = false;
		}
		else
		{
			m_ScrollUp.Enabled = true;
		}
		if (m_PopupGalleryStyle && !m_PopupUsesStandardScrollbars)
		{
			m_ScrollUp.Visible = m_ScrollUp.Enabled;
			m_ScrollUp.Displayed = m_ScrollUp.Enabled;
		}
		if (m_ViewRectangle.Bottom + GetScrollPosition().Y > displayRect.Bottom)
		{
			m_ScrollDown.Enabled = true;
		}
		else
		{
			m_ScrollDown.Enabled = false;
		}
		if (m_PopupGalleryStyle && !m_PopupUsesStandardScrollbars)
		{
			m_ScrollDown.Visible = m_ScrollDown.Enabled;
			m_ScrollDown.Displayed = m_ScrollDown.Enabled;
		}
	}

	protected override void OnExternalSizeChange()
	{
		if (PopupGalleryStyle)
		{
			RepositionButtons(DisplayRectangle);
		}
		base.OnExternalSizeChange();
	}

	private void RepositionButtons(Rectangle r)
	{
		int num = r.Right - m_ScrollButtonSize.Width;
		int y = r.Y;
		if (IsRightToLeft)
		{
			num = r.X + m_ScrollButtonSize.Width;
		}
		if (!m_ViewRectangle.IsEmpty)
		{
			if (m_ViewRectangle.Y != r.Y)
			{
				m_ViewRectangle.Y = r.Y;
			}
			if (m_ViewRectangle.X != r.X)
			{
				m_ViewRectangle.X = r.X;
			}
		}
		if (m_PopupGalleryStyle && m_ScrollBar != null)
		{
			bool dispose = false;
			ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(base.BackgroundStyle, out dispose);
			num -= ElementStyleLayout.RightWhiteSpace(elementStyle);
			y += ElementStyleLayout.TopWhiteSpace(elementStyle);
			r.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle);
			m_ScrollBar.DisplayRectangle = new Rectangle(num, y, m_ScrollButtonSize.Width, r.Height);
			if (dispose)
			{
				elementStyle.Dispose();
			}
			return;
		}
		if (m_PopupGalleryStyle && !m_PopupUsesStandardScrollbars)
		{
			Size size = new Size(r.Width, 12);
			m_ScrollUp.RecalcSize();
			m_ScrollUp.Size = size;
			m_ScrollUp.SetDisplayRectangle(new Rectangle(r.X, r.Y, size.Width, size.Height));
			m_ScrollDown.RecalcSize();
			m_ScrollDown.Size = size;
			m_ScrollDown.SetDisplayRectangle(new Rectangle(r.X, r.Bottom - size.Height, size.Width, size.Height));
			return;
		}
		Rectangle displayRectangle = new Rectangle(num, y, m_ScrollButtonSize.Width, m_ScrollButtonSize.Height);
		m_ScrollUp.Displayed = true;
		m_ScrollUp.RecalcSize();
		m_ScrollUp.Size = displayRectangle.Size;
		m_ScrollUp.SetDisplayRectangle(displayRectangle);
		if (m_EnableGalleryPopup)
		{
			y = r.Bottom - m_ScrollButtonSize.Height;
			displayRectangle = new Rectangle(num, y, m_ScrollButtonSize.Width, m_ScrollButtonSize.Height);
			m_PopupGallery.Displayed = true;
			m_PopupGallery.RecalcSize();
			m_PopupGallery.Size = displayRectangle.Size;
			m_PopupGallery.SetDisplayRectangle(displayRectangle);
			displayRectangle.Offset(0, -(m_ScrollButtonSize.Height - 1));
		}
		else
		{
			m_PopupGallery.Displayed = false;
			y = r.Bottom - m_ScrollButtonSize.Height;
			displayRectangle = new Rectangle(num, y, m_ScrollButtonSize.Width, m_ScrollButtonSize.Height);
		}
		m_ScrollDown.Displayed = true;
		m_ScrollDown.RecalcSize();
		m_ScrollDown.Size = displayRectangle.Size;
		m_ScrollDown.SetDisplayRectangle(displayRectangle);
	}

	protected override void OnLeftLocationChanged(int oldValue)
	{
		base.OnLeftLocationChanged(oldValue);
		RepositionButtons(DisplayRectangle);
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		base.OnTopLocationChanged(oldValue);
		RepositionButtons(DisplayRectangle);
	}

	protected override void OnStyleChanged()
	{
		m_ScrollUp.Style = Style;
		m_ScrollDown.Style = Style;
		m_PopupGallery.Style = Style;
		if (BarFunctions.IsOffice2010Style(Style))
		{
			m_ScrollUp.Shape = new RoundRectangleShapeDescriptor();
			m_ScrollDown.Shape = new RoundRectangleShapeDescriptor();
			m_PopupGallery.Shape = new RoundRectangleShapeDescriptor();
		}
		else
		{
			m_ScrollUp.Shape = null;
			m_ScrollDown.Shape = null;
			m_PopupGallery.Shape = null;
		}
		base.OnStyleChanged();
	}

	private int GetScrollLineHeight()
	{
		int num = DisplayRectangle.Bottom + GetScrollPosition().Y;
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible && subItem.TopInternal > num)
			{
				return subItem.HeightInternal;
			}
		}
		return 24;
	}

	private int GetNextScrollLine()
	{
		bool flag = false;
		int num = 0;
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible)
			{
				if (flag && subItem.TopInternal > num)
				{
					return GetScrollForItem(subItem);
				}
				if (!flag && subItem.TopInternal >= TopInternal)
				{
					flag = true;
					num = subItem.TopInternal;
				}
			}
		}
		return GetScrollPosition().Y;
	}

	private int GetPreviousScrollLine()
	{
		if (GetScrollPosition().Y == 0)
		{
			return 0;
		}
		int num = SubItems.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			BaseItem baseItem = SubItems[num2];
			if (baseItem.Visible && baseItem.TopInternal < TopInternal)
			{
				return GetScrollForItem(baseItem);
			}
		}
		return 0;
	}

	private int GetScrollForItem(BaseItem item)
	{
		int num = DisplayRectangle.Top - item.TopInternal + GetScrollPosition().Y;
		if (Math.Abs(num) <= 4)
		{
			num = 0;
		}
		return num;
	}

	public void PopupGallery()
	{
		if (m_PopupGallery.Expanded)
		{
			m_PopupGallery.Expanded = false;
		}
		else if (!DesignMode)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			Point point = val.PointToScreen(DisplayRectangle.Location);
			GalleryContainer galleryContainer = null;
			if (m_Groups.Count > 0)
			{
				galleryContainer = new GalleryContainer();
				galleryContainer.DefaultSize = DefaultSize;
				galleryContainer.StretchGallery = StretchGallery;
				galleryContainer.MultiLine = false;
				galleryContainer.LayoutOrientation = eOrientation.Vertical;
				foreach (BaseItem popupGalleryItem in PopupGalleryItems)
				{
					galleryContainer.PopupGalleryItems.Add(popupGalleryItem.Copy());
				}
				ArrangePopupGroups(galleryContainer);
			}
			else
			{
				galleryContainer = Copy() as GalleryContainer;
			}
			galleryContainer.EnableGalleryPopup = false;
			galleryContainer.SystemGallery = true;
			galleryContainer.IncrementalSizing = false;
			galleryContainer.PopupUsesStandardScrollbars = m_PopupUsesStandardScrollbars;
			if (PopupGallerySize.IsEmpty)
			{
				galleryContainer.DefaultSize = GetPopupGallerySize(point);
			}
			else
			{
				galleryContainer.DefaultSize = PopupGallerySize;
			}
			galleryContainer.BackgroundStyle.Reset();
			galleryContainer.PopupGalleryStyle = true;
			m_PopupGallery.SubItems.Insert(0, galleryContainer);
			m_PopupGallery.PopupMenu(point);
		}
		else
		{
			m_PopupGallery.Expanded = true;
		}
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		if (!SystemGallery && BaseItem.IsOnPopup(this))
		{
			SetScrollPosition(0);
			PopupGalleryStyle = true;
		}
		base.OnContainerChanged(objOldContainer);
	}

	private void ArrangePopupGroups(GalleryContainer gc)
	{
		ArrayList sortedGroupList = GetSortedGroupList();
		foreach (GalleryGroup item3 in sortedGroupList)
		{
			if (item3.Items.Count != 0)
			{
				LabelItem item = CreateLabelForGroup(item3);
				gc.SubItems.Add(item);
				ItemContainer item2 = CreateContainerFormGroup(item3);
				gc.SubItems.Add(item2);
			}
		}
	}

	private ItemContainer CreateContainerFormGroup(GalleryGroup g)
	{
		ItemContainer itemContainer = new ItemContainer();
		itemContainer.LayoutOrientation = eOrientation.Horizontal;
		itemContainer.MultiLine = true;
		itemContainer.Name = "container_" + g.Name;
		foreach (BaseItem item in g.Items)
		{
			itemContainer.SubItems.Add(item.Copy());
		}
		return itemContainer;
	}

	private LabelItem CreateLabelForGroup(GalleryGroup g)
	{
		LabelItem labelItem = new LabelItem("label_" + g.Name, g.Text);
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			labelItem.BackColor = colorTable.Gallery.GroupLabelBackground;
			labelItem.ForeColor = colorTable.Gallery.GroupLabelText;
			labelItem.SingleLineColor = colorTable.Gallery.GroupLabelBorder;
			labelItem.BorderType = eBorderType.SingleLine;
			labelItem.BorderSide = eBorderSide.Bottom;
			labelItem.PaddingTop = 1;
			labelItem.PaddingLeft = 10;
			labelItem.PaddingBottom = 1;
		}
		return labelItem;
	}

	private ArrayList GetSortedGroupList()
	{
		ArrayList arrayList = new ArrayList(m_Groups.Count);
		foreach (GalleryGroup group in m_Groups)
		{
			arrayList.Add(group);
		}
		arrayList.Sort(new GroupDisplayOrderComparer());
		return arrayList;
	}

	protected override InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		InsertPosition insertPosition = DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
		if (insertPosition == null && m_PopupGallery.Expanded)
		{
			insertPosition = m_PopupGallery.GetInsertPosition(pScreen, dragItem);
		}
		return insertPosition;
	}

	protected override IContentLayout GetContentLayout()
	{
		SerialContentLayoutManager serialContentLayoutManager = base.GetContentLayout() as SerialContentLayoutManager;
		if (serialContentLayoutManager != null)
		{
			serialContentLayoutManager.HorizontalFitContainerHeight = false;
		}
		return serialContentLayoutManager;
	}

	private void StartAnimation(int start, int end)
	{
		m_AnimationEndPos = end;
		m_AnimationCurrentPos = start;
		m_AnimationStep = Math.Max(8, Math.Abs(end - start) / 3);
		if (end < start)
		{
			m_AnimationStep *= -1;
		}
		m_ThreadUI.Start();
	}

	private void RecordStartState(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		m_StateBitmap = new Bitmap(m_ViewRectangle.Width, m_ViewRectangle.Height, (PixelFormat)925707);
		Graphics val = Graphics.FromImage((Image)(object)m_StateBitmap);
		BufferedBitmap bufferedBitmap = new BufferedBitmap(val, new Rectangle(0, 0, m_ViewRectangle.Width, m_ViewRectangle.Height));
		Graphics graphics = bufferedBitmap.Graphics;
		Color color = Color.Empty;
		try
		{
			bool dispose = false;
			ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(base.BackgroundStyle, out dispose);
			color = ((!elementStyle.BackColor.IsEmpty) ? elementStyle.BackColor : ((elementStyle.BackColorBlend.Count <= 0 || elementStyle.BackColorBlend[0].Color.IsEmpty) ? Color.WhiteSmoke : elementStyle.BackColorBlend[0].Color));
			if (!color.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(color);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, 0, 0, ((Image)m_StateBitmap).Width, ((Image)m_StateBitmap).Height);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			object containerControl = ContainerControl;
			Control val3 = (Control)((containerControl is Control) ? containerControl : null);
			bool flag = false;
			ItemPaintArgs itemPaintArgs = null;
			if (val3 is ItemControl)
			{
				flag = ((ItemControl)(object)val3).AntiAlias;
				itemPaintArgs = ((ItemControl)(object)val3).GetItemPaintArgs(graphics);
			}
			else if (val3 is Bar)
			{
				flag = ((Bar)(object)val3).AntiAlias;
				itemPaintArgs = ((Bar)(object)val3).GetItemPaintArgs(graphics);
			}
			else if (val3 is ButtonX)
			{
				flag = ((ButtonX)(object)val3).AntiAlias;
				itemPaintArgs = ((ButtonX)(object)val3).GetItemPaintArgs(graphics);
			}
			else if (val3 is MenuPanel)
			{
				flag = ((MenuPanel)(object)val3).AntiAlias;
				itemPaintArgs = ((MenuPanel)(object)val3).GetItemPaintArgs(graphics);
			}
			Rectangle clipRectangle = GetClipRectangle();
			Matrix val4 = new Matrix();
			val4.Translate((float)(-(clipRectangle.X + m_ScrollPosition.X)), (float)(-(clipRectangle.Y + m_ScrollPosition.Y)), (MatrixOrder)1);
			graphics.Transform = val4;
			if (flag)
			{
				graphics.SmoothingMode = (SmoothingMode)4;
				graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			if (itemPaintArgs == null)
			{
				((Image)m_StateBitmap).Dispose();
				m_StateBitmap = null;
			}
			ItemDisplay itemDisplay = GetItemDisplay();
			itemDisplay.Paint(this, itemPaintArgs);
			bufferedBitmap.Render(val);
			if (dispose)
			{
				elementStyle.Dispose();
			}
		}
		finally
		{
			graphics = null;
			bufferedBitmap.Dispose();
			val.Dispose();
		}
		if (!color.IsEmpty)
		{
			m_StateBitmap.MakeTransparent(color);
		}
	}

	private void CleanupState(object sender, EventArgs e)
	{
		if (m_StateBitmap != null)
		{
			((Image)m_StateBitmap).Dispose();
			m_StateBitmap = null;
		}
	}

	private void UpdateThreadedUI(object sender, EventArgs e)
	{
		m_AnimationCurrentPos += m_AnimationStep;
		if ((m_AnimationCurrentPos <= m_AnimationEndPos && m_AnimationEndPos < 0 && m_AnimationStep < 0) || (m_AnimationCurrentPos >= m_AnimationEndPos && m_AnimationEndPos >= 0 && m_AnimationStep > 0) || (m_AnimationCurrentPos >= m_AnimationEndPos && m_AnimationEndPos < 0 && m_AnimationStep > 0) || (m_AnimationCurrentPos <= m_AnimationEndPos && m_AnimationEndPos >= 0 && m_AnimationStep < 0))
		{
			m_ThreadUI.Stop();
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			val.Invalidate(DisplayRectangle);
		}
	}

	protected override void OnDisplayedChanged()
	{
		if (!Displayed && !SystemGallery && PopupGalleryStyle)
		{
			SetScrollPosition(0);
		}
		base.OnDisplayedChanged();
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		base.OnVisibleChanged(newValue);
		if (!newValue)
		{
			m_ThreadUI.Stop();
		}
	}

	public bool CanExtend(object extendee)
	{
		if (!(extendee is BaseItem value))
		{
			return false;
		}
		return SubItems.Contains(value);
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.GalleryGroupTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public GalleryGroup GetGalleryGroup(BaseItem item)
	{
		if (item == null)
		{
			return null;
		}
		return GetItemGalleryGroup(item);
	}

	public void SetGalleryGroup(BaseItem item, GalleryGroup group)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item argument cannot be null.");
		}
		if (group == null)
		{
			GetItemGalleryGroup(item)?.Items.Remove(item);
			return;
		}
		GetItemGalleryGroup(item)?.Items.Remove(item);
		group.Items.Add(item);
	}

	private GalleryGroup GetItemGalleryGroup(BaseItem item)
	{
		foreach (GalleryGroup group in m_Groups)
		{
			if (group.Items.Contains(item))
			{
				return group;
			}
		}
		return null;
	}
}
