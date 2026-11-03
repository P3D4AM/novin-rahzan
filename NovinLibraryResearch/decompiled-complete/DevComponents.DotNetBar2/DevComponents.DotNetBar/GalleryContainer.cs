using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.ScrollBar;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[ProvideProperty("GalleryGroup", typeof(BaseItem))]
[DesignTimeVisible(false)]
[Designer("DevComponents.DotNetBar.Design.GalleryContainerDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class GalleryContainer : ItemContainer, IExtenderProvider
{
	private class _2499 : IComparer
	{
		private int _1C24(object و, object ى)
		{
			if (و is GalleryGroup && ى is GalleryGroup)
			{
				return ((GalleryGroup)و).DisplayOrder - ((GalleryGroup)ى).DisplayOrder;
			}
			return new CaseInsensitiveComparer().Compare(و, ى);
		}

		int IComparer.Compare(object و, object ى)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ᰤ
			return this._1C24(و, ى);
		}
	}

	private bool ᥨ = true;

	private ButtonItem ᥩ;

	private ButtonItem ᥪ;

	private ButtonItem ᥫ;

	private Size ᥬ = new Size(15, 20);

	private int ᥭ = 1;

	private Size ሧ = Size.Empty;

	private Point _196E = Point.Empty;

	private Size _196F = Size.Empty;

	private ScrollBarCore ᇫ;

	private bool ᥰ;

	private new Rectangle _0ABB = Rectangle.Empty;

	private Size ᥱ = Size.Empty;

	private bool ᥲ;

	internal bool ᥳ;

	private bool ᥴ = true;

	private GalleryGroupCollection _0D71;

	private bool _1975 = true;

	private ᦫ _1976;

	private bool _1977 = true;

	private DotNetBarManager.PopupOpenEventHandler _1978;

	private EventHandler _1979;

	private EventHandler _197A;

	private EventHandler _197B;

	private bool _197C;

	private bool __197D;

	private Bitmap _197E;

	private int _197F;

	private int ᦀ;

	private int ᦁ;

	internal bool ᥣ
	{
		get
		{
			return ᥰ;
		}
		set
		{
			ᥰ = value;
			if (ᇫ != null)
			{
				ᇫ.Dispose();
				ᇫ = null;
			}
			if (ᥰ && _1977)
			{
				ᇫ = new ScrollBarCore();
				ᇫ.ValueChanged += ᦒ;
			}
		}
	}

	internal Size ᥤ
	{
		get
		{
			return ᥱ;
		}
		set
		{
			ᥱ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Rectangle PopupGalleryButtonBounds => ᥫ.DisplayRectangle;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ButtonItem PopupGalleryItem => ᥫ;

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Layout")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[DefaultValue(true)]
	[Category("Layout")]
	[Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
	[Browsable(true)]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether Gallery when on popup is using standard scrollbars to scroll the content.")]
	public bool PopupUsesStandardScrollbars
	{
		get
		{
			return _1977;
		}
		set
		{
			_1977 = value;
		}
	}

	[Description("Indicates whether gallery is using incremental sizing when stretched.")]
	[Category("Layout")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool IncrementalSizing
	{
		get
		{
			return ᥴ;
		}
		set
		{
			ᥴ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Category("Layout")]
	[DefaultValue(false)]
	[Description("Indicates whether Gallery width is determined based on the Ribbon container size.")]
	[Browsable(true)]
	public bool StretchGallery
	{
		get
		{
			return ᥲ;
		}
		set
		{
			if (ᥲ == value)
			{
				return;
			}
			ᥲ = value;
			if (ContainerControl is RibbonBar ribbonBar)
			{
				if (ᥲ)
				{
					if (ribbonBar.ჷ == null)
					{
						ribbonBar.ჷ = this;
					}
				}
				else if (ribbonBar.ჷ == this)
				{
					ribbonBar.ჷ = null;
				}
			}
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection PopupGalleryItems => ᥫ.SubItems;

	[Description("Indicates the default size of the gallery.")]
	[Category("Layout")]
	[Browsable(true)]
	public Size DefaultSize
	{
		get
		{
			return ሧ;
		}
		set
		{
			ሧ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Category("Layout")]
	[Description("Indicates the default size of the gallery when gallery is displayed on the popup menu.")]
	[Browsable(true)]
	public Size PopupGallerySize
	{
		get
		{
			return _196F;
		}
		set
		{
			_196F = value;
		}
	}

	[Description("Indicates whether gallery can be displayed on the popup.")]
	[Browsable(true)]
	[Category("Gallery")]
	[DefaultValue(true)]
	public bool EnableGalleryPopup
	{
		get
		{
			return ᥨ;
		}
		set
		{
			ᥨ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	internal ScrollBarCore ᥥ => ᇫ;

	internal bool ᥦ
	{
		get
		{
			if (ᥴ && SubItems.Count > 0)
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
			ᦗ(DisplayRectangle);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DefaultValue(false)]
	public override bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			base.Expanded = value;
			if (!value && ᥫ.Expanded)
			{
				ᥫ.Expanded = false;
			}
		}
	}

	[Browsable(false)]
	public bool IsGalleryPopupOpen => ᥫ.Expanded;

	[DefaultValue("")]
	[Category("Appearance")]
	[Browsable(true)]
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

	private bool ᥧ
	{
		get
		{
			if (DesignMode || !_1975 || !Displayed || TextDrawing.UseTextRenderer || _089F.ष())
			{
				return false;
			}
			return _1975;
		}
	}

	[Description("Indicates whether scroll animation is enabled.")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Animation")]
	public bool ScrollAnimation
	{
		get
		{
			return _1975;
		}
		set
		{
			_1975 = value;
		}
	}

	[Category("Groups")]
	[Browsable(false)]
	[Description("Gets the collection of GalleryGroup objects associated with this gallery.")]
	[Editor("DevComponents.DotNetBar.Design.GalleryGroupCollectionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public GalleryGroupCollection GalleryGroups => _0D71;

	[Description("Occurs when Gallery popup item is about to open.")]
	public event DotNetBarManager.PopupOpenEventHandler GalleryPopupOpen
	{
		add
		{
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler = _1978;
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler2;
			do
			{
				popupOpenEventHandler2 = popupOpenEventHandler;
				DotNetBarManager.PopupOpenEventHandler value2 = (DotNetBarManager.PopupOpenEventHandler)Delegate.Combine(popupOpenEventHandler2, value);
				popupOpenEventHandler = Interlocked.CompareExchange(ref _1978, value2, popupOpenEventHandler2);
			}
			while ((object)popupOpenEventHandler != popupOpenEventHandler2);
		}
		remove
		{
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler = _1978;
			DotNetBarManager.PopupOpenEventHandler popupOpenEventHandler2;
			do
			{
				popupOpenEventHandler2 = popupOpenEventHandler;
				DotNetBarManager.PopupOpenEventHandler value2 = (DotNetBarManager.PopupOpenEventHandler)Delegate.Remove(popupOpenEventHandler2, value);
				popupOpenEventHandler = Interlocked.CompareExchange(ref _1978, value2, popupOpenEventHandler2);
			}
			while ((object)popupOpenEventHandler != popupOpenEventHandler2);
		}
	}

	[Description("Occurs just before Gallery popup window is shown.")]
	public event EventHandler GalleryPopupShowing
	{
		add
		{
			EventHandler eventHandler = _1979;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1979, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _1979;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1979, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before the Gallery popup item is closed")]
	public event EventHandler GalleryPopupClose
	{
		add
		{
			EventHandler eventHandler = _197A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _197A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _197A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _197A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after popup item has been closed.")]
	public event EventHandler GalleryPopupFinalized
	{
		add
		{
			EventHandler eventHandler = _197B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _197B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _197B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _197B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public GalleryContainer()
	{
		ሧ = ᦂ();
		_196F = ᦃ();
		_0D71 = new GalleryGroupCollection();
		_0D71._၉ = this;
		MultiLine = true;
		ᦄ();
		base.MinimumSize = new Size(Dpi.Width(42 + ᥬ.Width + ᥭ), Dpi.Height(ሧ.Height));
		_1976 = new ᦫ(ᦣ, ᦥ, ᦤ);
	}

	private Size ᦂ()
	{
		return new Size(140 + ᥬ.Width + ᥭ, ᥬ.Height * 3 - 2);
	}

	private Size ᦃ()
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
		foreach (BaseItem subItem in galleryContainer.SubItems)
		{
			subItem.CollapseAllBeforeRaisingClick = false;
		}
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
		ᥩ.Click -= ᦎ;
		ᥪ.Click -= ᦏ;
		ᥫ.Click -= ᦇ;
		ᥫ.PopupOpen -= ᦆ;
		ᥫ.PopupShowing -= ᦅ;
		ᥫ.PopupClose -= ᦌ;
		ᥫ.PopupFinalized -= ᦋ;
		if (ᇫ != null)
		{
			ᇫ.Dispose();
			ᇫ = null;
		}
		_1976.ᦲ();
		base.Dispose(disposing);
	}

	private void ᦄ()
	{
		ᥩ = new ButtonItem("sysgalleryscrollup");
		ᥩ.Text = "<expand direction=\"top\"/>";
		ᥩ.Style = Style;
		ᥩ.ColorTable = eButtonColor.OrangeWithBackground;
		ᥩ.SetParent(this);
		ᥩ.Click += ᦎ;
		ᥩ.ClickAutoRepeat = true;
		ᥩ.ClickRepeatInterval = 200;
		ᥩ.CanCustomize = false;
		ᥩ.AutoCollapseOnClick = false;
		ᥪ = new ButtonItem("sysgalleryscrolldown");
		ᥪ.Text = "<expand direction=\"bottom\"/>";
		ᥪ.Style = Style;
		ᥪ.ColorTable = eButtonColor.OrangeWithBackground;
		ᥪ.SetParent(this);
		ᥪ.Click += ᦏ;
		ᥪ.ClickAutoRepeat = true;
		ᥪ.ClickRepeatInterval = 200;
		ᥪ.CanCustomize = false;
		ᥪ.AutoCollapseOnClick = false;
		ᥫ = new ButtonItem("sysgallerypopup");
		ᥫ.Text = "<expand direction=\"popup\"/>";
		ᥫ.ButtonStyle = eButtonStyle.TextOnlyAlways;
		ᥫ.Style = Style;
		ᥫ.ColorTable = eButtonColor.OrangeWithBackground;
		ᥫ.SetParent(this);
		ᥫ.Click += ᦇ;
		ᥫ.CanCustomize = false;
		ᥫ.PopupOpen += ᦆ;
		ᥫ.PopupShowing += ᦅ;
		ᥫ.PopupClose += ᦌ;
		ᥫ.PopupFinalized += ᦋ;
	}

	private void ᦅ(object _٠, EventArgs ؿ)
	{
		OnGalleryPopupShowing(_٠, ؿ);
	}

	private void ᦆ(object _٠, PopupOpenEventArgs ؿ)
	{
		OnGalleryPopupOpen(_٠, ؿ);
	}

	private void ᦇ(object _٠, EventArgs ؿ)
	{
		PopupGallery();
	}

	private Size ᦈ(Point ᦉ)
	{
		Size result = Size.Empty;
		if (DisplayRectangle.Width < DefaultSize.Width || _0ABB.Size.IsEmpty)
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
				if (_1977)
				{
					num += Dpi.Width(ᥭ + ᥬ.Width);
				}
			}
			result = new Size(num, Math.Max(DefaultSize.Height, _0ABB.Height));
		}
		else
		{
			result = new Size(DisplayRectangle.Width, _0ABB.Height);
		}
		ঊ ঊ2 = null;
		ঊ2 = BarFunctions.ॹ(ᦉ);
		if (_0D71.Count > 0 && ঊ2 != null && ঊ2.ঌ.Bottom - ᦉ.Y > result.Height)
		{
			result.Height = ঊ2.ঌ.Bottom - ᦉ.Y;
		}
		if (ঊ2 != null)
		{
			int num3 = 16;
			num3 += PopupGalleryItems.Count * 26;
			int num4 = 0;
			int num5 = 0;
			if (ᦉ.Y + result.Height > ঊ2.ঌ.Bottom - num3)
			{
				num4 = ᦉ.Y + result.Height - (ঊ2.ঌ.Bottom - num3);
			}
			if (ᦉ.X + result.Width > ঊ2.ঌ.Right - num3)
			{
				num5 = ᦉ.X + result.Width - (ঊ2.ঌ.Right - num3);
			}
			if (result.Width - num5 > base.MinimumSize.Width)
			{
				result.Width -= num5;
			}
			else
			{
				result.Width = base.MinimumSize.Width + Dpi.Width(ᥭ + ᥬ.Width);
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

	private void ᦊ()
	{
		if (DesignMode)
		{
			return;
		}
		foreach (BaseItem subItem in ᥫ.SubItems)
		{
			if (subItem is GalleryContainer { ᥳ: not false } galleryContainer)
			{
				ᥫ.SubItems.Remove(galleryContainer);
				galleryContainer.Dispose();
				break;
			}
		}
	}

	private void ᦋ(object _٠, EventArgs ؿ)
	{
		OnGalleryPopupFinalized(_٠, ؿ);
		ᦊ();
	}

	private void ᦌ(object _٠, EventArgs ؿ)
	{
		OnGalleryPopupClose(_٠, ؿ);
		ᦍ();
	}

	private void ᦍ()
	{
	}

	private void ᦎ(object _٠, EventArgs ؿ)
	{
		ScrollUp();
	}

	private void ᦏ(object _٠, EventArgs ؿ)
	{
		ScrollDown();
	}

	internal bool ᦐ(int و, int ى)
	{
		if (ᥩ.Enabled && ᥩ.DisplayRectangle.Contains(و, ى))
		{
			return true;
		}
		if (ᥪ.Enabled && ᥪ.DisplayRectangle.Contains(و, ى))
		{
			return true;
		}
		if (ᇫ != null && ᇫ.Visible)
		{
			if (ᇫ.ThumbIncreaseRectangle.Contains(و, ى))
			{
				return true;
			}
			if (ᇫ.ThumbDecreaseRectangle.Contains(و, ى))
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
		_197C = false;
		if (ᥰ && _1977)
		{
			ᇫ.MouseDown(objArg);
			if (ᇫ.DisplayRectangle.Contains(objArg.X, objArg.Y))
			{
				_197C = true;
			}
		}
		base.InternalMouseDown(objArg);
		if (!DesignMode)
		{
			return;
		}
		if (ᥩ.Enabled && ᥩ.DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			ScrollUp();
		}
		else if (ᥪ.Enabled && ᥪ.DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			ScrollDown();
		}
		else if (ᥫ.Enabled && ᥫ.DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			PopupGallery();
		}
		else if (ᇫ != null && ᇫ.Visible)
		{
			if (ᇫ.ThumbIncreaseRectangle.Contains(objArg.X, objArg.Y))
			{
				ScrollDown();
			}
			else if (ᇫ.ThumbDecreaseRectangle.Contains(objArg.X, objArg.Y))
			{
				ScrollUp();
			}
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (ᥰ && !base.IsDisposed && _1977)
		{
			ᇫ.MouseMove(objArg);
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		if (ᥰ && _1977 && !base.IsDisposed)
		{
			ᇫ.MouseUp(objArg);
			if (ᇫ.DisplayRectangle.Contains(objArg.X, objArg.Y) || _197C)
			{
				_197C = false;
				return;
			}
		}
		base.InternalMouseUp(objArg);
	}

	public override void InternalMouseLeave()
	{
		if (ᥰ && !base.IsDisposed && _1977)
		{
			ᇫ.MouseLeave();
		}
		base.InternalMouseLeave();
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		if (IsGalleryPopupOpen)
		{
			ᥫ.InternalKeyDown(objArg);
		}
		else
		{
			base.InternalKeyDown(objArg);
		}
	}

	protected override void OnHotSubItemChanged(BaseItem newValue, BaseItem oldValue)
	{
		base.OnHotSubItemChanged(newValue, oldValue);
		if (newValue != null && newValue != oldValue && !ᦑ().Contains(newValue.Bounds))
		{
			EnsureVisible(newValue);
		}
	}

	private Rectangle ᦑ()
	{
		Rectangle displayRectangle = DisplayRectangle;
		if (ᇫ != null)
		{
			displayRectangle.Width -= ᇫ.DisplayRectangle.Width;
		}
		if (ᥰ && !_1977)
		{
			if (ᥩ != null)
			{
				displayRectangle.Y += ᥩ.HeightInternal;
				displayRectangle.Height -= ᥩ.HeightInternal;
			}
			if (ᥪ != null)
			{
				displayRectangle.Height -= ᥪ.HeightInternal;
			}
		}
		else if (ᥩ != null)
		{
			displayRectangle.Width -= ᥩ.WidthInternal;
		}
		return displayRectangle;
	}

	protected virtual void OnGalleryPopupOpen(object sender, PopupOpenEventArgs e)
	{
		if (_1978 != null)
		{
			_1978(sender, e);
		}
	}

	protected virtual void OnGalleryPopupShowing(object sender, EventArgs e)
	{
		if (_1979 != null)
		{
			_1979(sender, e);
		}
	}

	protected virtual void OnGalleryPopupClose(object sender, EventArgs e)
	{
		if (_197A != null)
		{
			_197A(sender, e);
		}
	}

	protected virtual void OnGalleryPopupFinalized(object sender, EventArgs e)
	{
		if (_197B != null)
		{
			_197B(sender, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDefaultSize()
	{
		return ሧ != ᦂ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefaultSize()
	{
		TypeDescriptor.GetProperties(this)["DefaultSize"].SetValue(this, ᦂ());
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePopupGallerySize()
	{
		return _196F != ᦃ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPopupGallerySize()
	{
		TypeDescriptor.GetProperties(this)["PopupGallerySize"].SetValue(this, ᦃ());
	}

	public void EnsureVisible(BaseItem item)
	{
		if (item == null)
		{
			throw new NullReferenceException("Item must be non null value.");
		}
		int y = ᦔ().Y;
		Rectangle rectangle = ᦑ();
		if (!rectangle.IsEmpty && (item.TopInternal + y < rectangle.Y || item.Bounds.Bottom > rectangle.Bottom))
		{
			int num = rectangle.Top - item.TopInternal + y;
			if (item.Bounds.Bottom > rectangle.Bottom)
			{
				num = -(item.Bounds.Bottom - rectangle.Y - rectangle.Height) + y;
			}
			if (Math.Abs(num) <= 4)
			{
				num = 0;
			}
			ᦓ(num);
		}
	}

	public void ScrollDown()
	{
		ᦓ(ᦙ());
	}

	public void ScrollUp()
	{
		ᦓ(ᦚ());
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (!DesignMode && (!ᥰ || !_1977))
		{
			if (ᥩ.DisplayRectangle.Contains(x, y) && ᥩ.Visible)
			{
				return ᥩ;
			}
			if (ᥪ.DisplayRectangle.Contains(x, y) && ᥪ.Visible)
			{
				return ᥪ;
			}
			if (ᥨ && ᥫ.DisplayRectangle.Contains(x, y))
			{
				return ᥫ;
			}
		}
		else if (!DesignMode && ᥰ && _1977 && !ᥳ && BaseItem.IsOnPopup(this) && ᇫ != null && (ᇫ.DisplayRectangle.Contains(x, y) || ᇫ.IsMouseDown))
		{
			return this;
		}
		if (_197C)
		{
			return this;
		}
		return base.ItemAtLocation(x, y);
	}

	private void ᦒ(object _٠, EventArgs ؿ)
	{
		if (!__197D)
		{
			ᦓ(-ᇫ.Value);
		}
	}

	private void ᦓ(int ى)
	{
		if (_196E.Y != ى)
		{
			__197D = true;
			int y = ى - _196E.Y;
			int y2 = _196E.Y;
			_196E.Y = ى;
			ᆀ(new Point(0, y));
			if (!ᥰ || !_1977)
			{
				ᦖ(DisplayRectangle);
			}
			if (ᥧ)
			{
				ᦡ(y2, ى);
			}
			else
			{
				Refresh();
			}
			if (ᇫ != null && ᇫ.Value != -ى)
			{
				ᇫ.Value = -ى;
			}
			__197D = false;
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		_1976.ᦳ();
		try
		{
			if (_197E != null)
			{
				base.PaintBackground(p);
				Graphics graphics = p.Graphics;
				Rectangle clipRectangle = GetClipRectangle();
				Region clip = graphics.Clip;
				graphics.SetClip(clipRectangle, (CombineMode)1);
				p.Graphics.DrawImage((Image)(object)_197E, clipRectangle.X, clipRectangle.Y + ᦀ);
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
			_1976.ᦴ();
		}
		if (ᥰ && _1977)
		{
			if (ᇫ.ParentControl == null)
			{
				ᇫ.ParentControl = p.ContainerControl;
			}
			ᇫ.Paint(p);
			return;
		}
		if (ᥩ.Visible)
		{
			ᥩ.Paint(p);
		}
		if (ᥪ.Visible)
		{
			ᥪ.Paint(p);
		}
		if (ᥨ)
		{
			ᥫ.Paint(p);
		}
	}

	private Point ᦔ()
	{
		return _196E;
	}

	private Size ᦕ()
	{
		if (!ᥱ.IsEmpty && ᥱ.Width >= base.MinimumSize.Width && ᥱ.Height >= base.MinimumSize.Height)
		{
			return ᥱ;
		}
		Size size = ሧ;
		if (ᥣ && !ᥳ)
		{
			if (base.MinimumSize.Width > size.Width)
			{
				size.Width = base.MinimumSize.Width;
			}
			if (base.MinimumSize.Height > size.Height)
			{
				size.Height = base.MinimumSize.Height;
			}
		}
		return Dpi.Size(size);
	}

	protected override Rectangle GetLayoutRectangle(Rectangle bounds)
	{
		Rectangle result = new Rectangle(bounds.Location, ᦕ());
		if (_1977 || !ᥰ)
		{
			result.Width -= Dpi.Width(ᥬ.Width + ᥭ);
			if (IsRightToLeft)
			{
				result.X += Dpi.Width(ᥬ.Width + ᥭ);
			}
		}
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(base.BackgroundStyle, out ല);
		result.X += ᝈ.ᝐ(elementStyle);
		result.Y += ᝈ._1753(elementStyle);
		result.Width -= ᝈ.ᝍ(elementStyle);
		result.Height -= ᝈ.ᝏ(elementStyle);
		result.Offset(ᦔ());
		if (ല)
		{
			elementStyle.Dispose();
		}
		return result;
	}

	protected override Rectangle LayoutItems(Rectangle bounds)
	{
		_0ABB = base.LayoutItems(bounds);
		if (_0ABB.Height < Math.Abs(_196E.Y))
		{
			ᦓ(0);
		}
		Size size = ᦕ();
		if (((ᥳ && !EnableGalleryPopup) || base.IsOnMenu) && size.Height > _0ABB.Height)
		{
			size.Height = _0ABB.Height;
		}
		if (ᥴ && !ᥣ)
		{
			size.Width = _0ABB.Width + Dpi.Width(ᥬ.Width + ᥭ);
		}
		Rectangle rectangle = new Rectangle(bounds.Location, size);
		int num = Dpi.Height(ᥬ.Height * 2) - 1;
		if (ᥨ)
		{
			num += Dpi.Height(ᥬ.Height) - 1;
		}
		rectangle.Height = Math.Max(num, rectangle.Height);
		ᦗ(rectangle);
		if (ᥰ && _1977)
		{
			if (_0ABB.Bottom - rectangle.Bottom > 0)
			{
				ᇫ.Enabled = true;
				ᇫ.Maximum = _0ABB.Bottom - rectangle.Bottom;
				ᇫ.Minimum = 0;
				ᇫ.SmallChange = ᦘ();
				ᇫ.LargeChange = (int)((float)ᇫ.Maximum * ((float)rectangle.Height / (float)_0ABB.Height));
			}
			else
			{
				ᇫ.Enabled = false;
			}
		}
		else
		{
			ᦖ(rectangle);
		}
		return rectangle;
	}

	protected override Size GetEmptyDesignTimeSize()
	{
		if (ᥰ && !ᥳ)
		{
			Size minimumSize = base.MinimumSize;
			if (!minimumSize.IsEmpty)
			{
				return minimumSize;
			}
		}
		return base.GetEmptyDesignTimeSize();
	}

	private void ᦖ(Rectangle ᕍ)
	{
		if (ᦔ().Y == 0)
		{
			ᥩ.Enabled = false;
		}
		else
		{
			ᥩ.Enabled = true;
		}
		if (ᥰ && !_1977)
		{
			ᥩ.Visible = ᥩ.Enabled;
			ᥩ.Displayed = ᥩ.Enabled;
		}
		if (_0ABB.Bottom + ᦔ().Y > ᕍ.Bottom)
		{
			ᥪ.Enabled = true;
		}
		else
		{
			ᥪ.Enabled = false;
		}
		if (ᥰ && !_1977)
		{
			ᥪ.Visible = ᥪ.Enabled;
			ᥪ.Displayed = ᥪ.Enabled;
		}
	}

	protected override void OnExternalSizeChange()
	{
		if (ᥣ)
		{
			ᦗ(DisplayRectangle);
		}
		base.OnExternalSizeChange();
	}

	private void ᦗ(Rectangle ल)
	{
		int num = ल.Right - Dpi.Width(ᥬ.Width);
		int y = ल.Y;
		if (IsRightToLeft)
		{
			num = ल.X + Dpi.Width(ᥬ.Width);
		}
		if (!_0ABB.IsEmpty)
		{
			if (_0ABB.Y != ल.Y)
			{
				_0ABB.Y = ल.Y;
			}
			if (_0ABB.X != ल.X)
			{
				_0ABB.X = ल.X;
			}
		}
		if (ᥰ && ᇫ != null)
		{
			bool ല = false;
			ElementStyle elementStyle = ElementStyleDisplay.റ(base.BackgroundStyle, out ല);
			num -= ᝈ._1752(elementStyle);
			y += ᝈ._1753(elementStyle);
			ल.Height -= ᝈ.ᝏ(elementStyle);
			ᇫ.DisplayRectangle = new Rectangle(num, y, Dpi.Width(ᥬ.Width), ल.Height);
			if (ല)
			{
				elementStyle.Dispose();
			}
			return;
		}
		if (ᥰ && !_1977)
		{
			Size size = new Size(ल.Width, Dpi.Height12);
			ᥩ.RecalcSize();
			ᥩ.Size = size;
			ᥩ.SetDisplayRectangle(new Rectangle(ल.X, ल.Y, size.Width, size.Height));
			ᥪ.RecalcSize();
			ᥪ.Size = size;
			ᥪ.SetDisplayRectangle(new Rectangle(ल.X, ल.Bottom - size.Height, size.Width, size.Height));
			return;
		}
		Rectangle displayRectangle = new Rectangle(num, y, Dpi.Width(ᥬ.Width), Dpi.Height(ᥬ.Height));
		ᥩ.Displayed = true;
		ᥩ.RecalcSize();
		ᥩ.Size = displayRectangle.Size;
		ᥩ.SetDisplayRectangle(displayRectangle);
		if (ᥨ)
		{
			y = ल.Bottom - Dpi.Height(ᥬ.Height);
			displayRectangle = new Rectangle(num, y, Dpi.Width(ᥬ.Width), Dpi.Height(ᥬ.Height));
			ᥫ.Displayed = true;
			ᥫ.RecalcSize();
			ᥫ.Size = displayRectangle.Size;
			ᥫ.SetDisplayRectangle(displayRectangle);
			displayRectangle.Offset(0, -(Dpi.Height(ᥬ.Height) - 1));
		}
		else
		{
			ᥫ.Displayed = false;
			y = ल.Bottom - Dpi.Height(ᥬ.Height);
			displayRectangle = new Rectangle(num, y, Dpi.Width(ᥬ.Width), Dpi.Height(ᥬ.Height));
		}
		ᥪ.Displayed = true;
		ᥪ.RecalcSize();
		ᥪ.Size = displayRectangle.Size;
		ᥪ.SetDisplayRectangle(displayRectangle);
	}

	protected override void OnLeftLocationChanged(int oldValue)
	{
		base.OnLeftLocationChanged(oldValue);
		ᦗ(DisplayRectangle);
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		base.OnTopLocationChanged(oldValue);
		ᦗ(DisplayRectangle);
	}

	protected override void OnStyleChanged()
	{
		ᥩ.Style = Style;
		ᥪ.Style = Style;
		ᥫ.Style = Style;
		if (BarFunctions.IsOffice2010Style(Style))
		{
			ᥩ.Shape = new RoundRectangleShapeDescriptor();
			ᥪ.Shape = new RoundRectangleShapeDescriptor();
			ᥫ.Shape = new RoundRectangleShapeDescriptor();
		}
		else
		{
			ᥩ.Shape = null;
			ᥪ.Shape = null;
			ᥫ.Shape = null;
		}
		base.OnStyleChanged();
	}

	private int ᦘ()
	{
		int num = DisplayRectangle.Bottom + ᦔ().Y;
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible && subItem.TopInternal > num)
			{
				return subItem.HeightInternal;
			}
		}
		return 24;
	}

	private int ᦙ()
	{
		bool flag = false;
		int num = 0;
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible)
			{
				if (flag && subItem.TopInternal > num)
				{
					return ᦛ(subItem);
				}
				if (!flag && subItem.TopInternal >= TopInternal)
				{
					flag = true;
					num = subItem.TopInternal;
				}
			}
		}
		return ᦔ().Y;
	}

	private int ᦚ()
	{
		if (ᦔ().Y == 0)
		{
			return 0;
		}
		int num = SubItems.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			BaseItem baseItem = SubItems[num2];
			if (baseItem.Visible && baseItem.TopInternal < TopInternal)
			{
				return ᦛ(baseItem);
			}
		}
		return 0;
	}

	private int ᦛ(BaseItem _06EC)
	{
		int num = DisplayRectangle.Top - _06EC.TopInternal + ᦔ().Y;
		if (Math.Abs(num) <= 4)
		{
			num = 0;
		}
		return num;
	}

	public void PopupGallery()
	{
		if (ᥫ.Expanded)
		{
			ᥫ.Expanded = false;
		}
		else if (!DesignMode)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			Point point = val.PointToScreen(DisplayRectangle.Location);
			GalleryContainer galleryContainer = null;
			if (_0D71.Count > 0)
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
				ᦜ(galleryContainer);
			}
			else
			{
				galleryContainer = Copy() as GalleryContainer;
			}
			galleryContainer.EnableGalleryPopup = false;
			galleryContainer.ᥳ = true;
			galleryContainer.IncrementalSizing = false;
			galleryContainer.PopupUsesStandardScrollbars = _1977;
			if (PopupGallerySize.IsEmpty)
			{
				galleryContainer.DefaultSize = ᦈ(point);
			}
			else
			{
				galleryContainer.DefaultSize = PopupGallerySize;
			}
			galleryContainer.BackgroundStyle.Reset();
			galleryContainer.ᥣ = true;
			ᥫ.SubItems.Insert(0, galleryContainer);
			ᥫ.PopupMenu(point);
		}
		else
		{
			ᥫ.Expanded = true;
		}
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		if (!ᥳ && BaseItem.IsOnPopup(this))
		{
			ᦓ(0);
			ᥣ = true;
		}
		base.OnContainerChanged(objOldContainer);
	}

	private void ᦜ(GalleryContainer ᦝ)
	{
		ArrayList arrayList = ᦠ();
		foreach (GalleryGroup item3 in arrayList)
		{
			if (item3.Items.Count != 0)
			{
				LabelItem item = ᦟ(item3);
				ᦝ.SubItems.Add(item);
				ItemContainer item2 = ᦞ(item3);
				ᦝ.SubItems.Add(item2);
			}
		}
	}

	private ItemContainer ᦞ(GalleryGroup _٥)
	{
		ItemContainer itemContainer = new ItemContainer();
		itemContainer.LayoutOrientation = eOrientation.Horizontal;
		itemContainer.MultiLine = true;
		itemContainer.Name = "container_" + _٥.Name;
		foreach (BaseItem item in _٥.Items)
		{
			itemContainer.SubItems.Add(item.Copy());
		}
		return itemContainer;
	}

	private LabelItem ᦟ(GalleryGroup _٥)
	{
		LabelItem labelItem = new LabelItem("label_" + _٥.Name, _٥.Text);
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

	private ArrayList ᦠ()
	{
		ArrayList arrayList = new ArrayList(_0D71.Count);
		foreach (GalleryGroup item in _0D71)
		{
			arrayList.Add(item);
		}
		arrayList.Sort(new _2499());
		return arrayList;
	}

	protected override InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		InsertPosition insertPosition = DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
		if (insertPosition == null && ᥫ.Expanded)
		{
			insertPosition = ᥫ.GetInsertPosition(pScreen, dragItem);
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

	private void ᦡ(int _0C73, int ᦢ)
	{
		_197F = ᦢ;
		ᦀ = _0C73;
		ᦁ = Math.Max(8, Math.Abs(ᦢ - _0C73) / 3);
		if (ᦢ < _0C73)
		{
			ᦁ *= -1;
		}
		_1976.ᦱ();
	}

	private void ᦣ(object _٠, EventArgs ؿ)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		_197E = new Bitmap(_0ABB.Width, _0ABB.Height, (PixelFormat)925707);
		Graphics val = Graphics.FromImage((Image)(object)_197E);
		BufferedBitmap bufferedBitmap = new BufferedBitmap(val, new Rectangle(0, 0, _0ABB.Width, _0ABB.Height));
		Graphics graphics = bufferedBitmap.Graphics;
		Color color = Color.Empty;
		try
		{
			bool ല = false;
			ElementStyle elementStyle = ElementStyleDisplay.റ(base.BackgroundStyle, out ല);
			color = ((!elementStyle.BackColor.IsEmpty) ? elementStyle.BackColor : ((elementStyle.BackColorBlend.Count <= 0 || elementStyle.BackColorBlend[0].Color.IsEmpty) ? Color.WhiteSmoke : elementStyle.BackColorBlend[0].Color));
			if (!color.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(color);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, 0, 0, ((Image)_197E).Width, ((Image)_197E).Height);
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
				itemPaintArgs = ((ItemControl)(object)val3)._0A92(graphics);
			}
			else if (val3 is Bar)
			{
				flag = ((Bar)(object)val3).AntiAlias;
				itemPaintArgs = ((Bar)(object)val3)._0A92(graphics);
			}
			else if (val3 is ButtonX)
			{
				flag = ((ButtonX)(object)val3).AntiAlias;
				itemPaintArgs = ((ButtonX)(object)val3)._0A92(graphics);
			}
			else if (val3 is MenuPanel)
			{
				flag = ((MenuPanel)(object)val3).AntiAlias;
				itemPaintArgs = ((MenuPanel)(object)val3)._0A92(graphics);
			}
			Rectangle clipRectangle = GetClipRectangle();
			Matrix val4 = new Matrix();
			val4.Translate((float)(-(clipRectangle.X + _196E.X)), (float)(-(clipRectangle.Y + _196E.Y)), (MatrixOrder)1);
			graphics.Transform = val4;
			if (flag)
			{
				graphics.SmoothingMode = (SmoothingMode)4;
				graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			if (itemPaintArgs == null)
			{
				((Image)_197E).Dispose();
				_197E = null;
			}
			_249A _249A2 = ᆆ();
			_249A2._139A(this, itemPaintArgs);
			bufferedBitmap.Render(val);
			if (ല)
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
			_197E.MakeTransparent(color);
		}
	}

	private void ᦤ(object _٠, EventArgs ؿ)
	{
		if (_197E != null)
		{
			((Image)_197E).Dispose();
			_197E = null;
		}
	}

	private void ᦥ(object _٠, EventArgs ؿ)
	{
		ᦀ += ᦁ;
		if ((ᦀ <= _197F && _197F < 0 && ᦁ < 0) || (ᦀ >= _197F && _197F >= 0 && ᦁ > 0) || (ᦀ >= _197F && _197F < 0 && ᦁ > 0) || (ᦀ <= _197F && _197F >= 0 && ᦁ < 0))
		{
			_1976.ᦲ();
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
		if (!Displayed && !ᥳ && ᥣ)
		{
			ᦓ(0);
		}
		base.OnDisplayedChanged();
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		base.OnVisibleChanged(newValue);
		if (!newValue)
		{
			_1976.ᦲ();
		}
	}

	protected override void ScaleItem(SizeF factor)
	{
		base.ScaleItem(factor);
		if (ᥫ != null)
		{
			ᥫ.NotifyScaleItem(factor);
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

	[Editor("DevComponents.DotNetBar.Design.GalleryGroupTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public GalleryGroup GetGalleryGroup(BaseItem item)
	{
		if (item == null)
		{
			return null;
		}
		return ᦦ(item);
	}

	public void SetGalleryGroup(BaseItem item, GalleryGroup group)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item argument cannot be null.");
		}
		if (group == null)
		{
			ᦦ(item)?.Items.Remove(item);
			return;
		}
		ᦦ(item)?.Items.Remove(item);
		group.Items.Add(item);
	}

	private GalleryGroup ᦦ(BaseItem _06EC)
	{
		foreach (GalleryGroup item in _0D71)
		{
			if (item.Items.Contains(_06EC))
			{
				return item;
			}
		}
		return null;
	}
}
