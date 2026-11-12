using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class ItemContainer : ImageItem, IDesignTimeProvider, IBlockExtended, IBlock
{
	private eOrientation m_LayoutOrientation;

	private Size m_EmptyDesignTimeSize = new Size(24, 24);

	private bool m_SystemContainer;

	private int m_ItemSpacing = 1;

	private Rectangle m_DesignerSelectionRectangle = Rectangle.Empty;

	private bool m_ResizeItemsToFit = true;

	private eHorizontalItemsAlignment m_HorizontalItemAlignment;

	private eVerticalItemsAlignment m_VerticalItemAlignment;

	private Size m_MinimumSize = Size.Empty;

	private ElementStyle m_BackgroundStyle = new ElementStyle();

	private int m_BeginGroupSpacing;

	private bool m_MultiLine;

	private Size _FixedSize = Size.Empty;

	private Size _TitleSize = Size.Empty;

	private bool _FitOversizeItemIntoAvailableWidth;

	private bool _Offsetting;

	private SerialContentLayoutManager m_LayoutManager;

	private ItemDisplay m_ItemDisplay;

	private ElementStyle _TitleStyle;

	private string _TitleText = "";

	private eTitlePosition _TitlePosition;

	public Size MinimumSize
	{
		get
		{
			return m_MinimumSize;
		}
		set
		{
			m_MinimumSize = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets container background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => m_BackgroundStyle;

	[Browsable(true)]
	[DefaultValue(eHorizontalItemsAlignment.Left)]
	[Category("Layout")]
	[Description("Indicates item alignment when container is in horizontal layout.")]
	[DevCoBrowsable(true)]
	public virtual eHorizontalItemsAlignment HorizontalItemAlignment
	{
		get
		{
			return m_HorizontalItemAlignment;
		}
		set
		{
			m_HorizontalItemAlignment = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(eVerticalItemsAlignment.Top)]
	[Category("Layout")]
	[Description("Indicates item item vertical alignment.")]
	[DevCoBrowsable(true)]
	public virtual eVerticalItemsAlignment VerticalItemAlignment
	{
		get
		{
			return m_VerticalItemAlignment;
		}
		set
		{
			m_VerticalItemAlignment = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
	public virtual bool MultiLine
	{
		get
		{
			return m_MultiLine;
		}
		set
		{
			m_MultiLine = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
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
			return m_ItemSpacing;
		}
		set
		{
			if (m_ItemSpacing != value)
			{
				m_ItemSpacing = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
			}
		}
	}

	private bool IsGroupingContainer
	{
		get
		{
			if (BarFunctions.IsOffice2007StyleOnly(EffectiveStyle))
			{
				return BeginGroup;
			}
			return false;
		}
	}

	[Description("Indicates suggested fixed size of the container. Width or Height or both may be set to suggest fixed size for the container.")]
	[Category("Appearance")]
	public Size FixedSize
	{
		get
		{
			return _FixedSize;
		}
		set
		{
			if (value != _FixedSize)
			{
				Size fixedSize = _FixedSize;
				_FixedSize = value;
				OnFixedSizeChanged(fixedSize, value);
			}
		}
	}

	internal bool FitOversizeItemIntoAvailableWidth
	{
		get
		{
			return _FitOversizeItemIntoAvailableWidth;
		}
		set
		{
			_FitOversizeItemIntoAvailableWidth = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override eOrientation Orientation
	{
		get
		{
			return eOrientation.Horizontal;
		}
		set
		{
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eOrientation.Horizontal)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Category("Layout")]
	public virtual eOrientation LayoutOrientation
	{
		get
		{
			return m_LayoutOrientation;
		}
		set
		{
			m_LayoutOrientation = value;
			OnOrientationChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Layout")]
	[Description("Indicates whether items contained by container are resized to fit the container bounds. When container is in horizontal layout mode then all items will have the same height. When container is in vertical layout mode then all items will have the same width.")]
	public virtual bool ResizeItemsToFit
	{
		get
		{
			return m_ResizeItemsToFit;
		}
		set
		{
			m_ResizeItemsToFit = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			Point offset = new Point(value.X - m_Rect.X, value.Y - m_Rect.Y);
			base.Bounds = value;
			if (m_HorizontalItemAlignment != 0 || (m_LayoutManager != null && m_LayoutManager.RightToLeft))
			{
				LayoutItems(value);
			}
			else if (!offset.IsEmpty)
			{
				OffsetSubItems(offset);
			}
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
			if (!value)
			{
				BaseItem.CollapseSubItems(this);
			}
		}
	}

	[Browsable(false)]
	public bool SystemContainer => m_SystemContainer;

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Accessibility")]
	[Description("Gets or sets the accessible role of the item.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public override AccessibleRole AccessibleRole
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.AccessibleRole;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.AccessibleRole = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets title style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TitleStyle => _TitleStyle;

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates title text for the container.")]
	public string TitleText
	{
		get
		{
			return _TitleText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != _TitleText)
			{
				string titleText = _TitleText;
				_TitleText = value;
				OnTitleTextChanged(titleText, value);
			}
		}
	}

	[DefaultValue(eTitlePosition.Top)]
	[Category("Appearance")]
	[Description("Indicates the position of title text.")]
	public eTitlePosition TitlePosition
	{
		get
		{
			return _TitlePosition;
		}
		set
		{
			if (value != _TitlePosition)
			{
				eTitlePosition titlePosition = _TitlePosition;
				_TitlePosition = value;
				OnTitlePositionChanged(titlePosition, value);
			}
		}
	}

	[Browsable(false)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether item can be customized by user.")]
	public override bool CanCustomize
	{
		get
		{
			return base.CanCustomize;
		}
		set
		{
			base.CanCustomize = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates item category used to group similar items at design-time.")]
	public override string Category
	{
		get
		{
			return base.Category;
		}
		set
		{
			base.Category = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(600)]
	[Category("Behavior")]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Specifies the mouse cursor displayed when mouse is over the item.")]
	public override Cursor Cursor
	{
		get
		{
			return base.Cursor;
		}
		set
		{
			base.Cursor = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates description of the item that is displayed during design.")]
	public override string Description
	{
		get
		{
			return base.Description;
		}
		set
		{
			base.Description = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether is item enabled.")]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
	public override bool GlobalItem
	{
		get
		{
			return base.GlobalItem;
		}
		set
		{
			base.GlobalItem = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(eItemAlignment.Near)]
	[Category("Appearance")]
	[Description("Determines alignment of the item inside the container.")]
	public override eItemAlignment ItemAlignment
	{
		get
		{
			return base.ItemAlignment;
		}
		set
		{
			base.ItemAlignment = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Design")]
	[Description("Indicates list of shortcut keys for this item.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsDesigner, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShortcutsConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the text that is displayed when mouse hovers over the item.")]
	[Localizable(true)]
	public override string Tooltip
	{
		get
		{
			return base.Tooltip;
		}
		set
		{
			base.Tooltip = value;
		}
	}

	bool IBlockExtended.IsBlockElement => false;

	bool IBlockExtended.IsNewLineAfterElement => false;

	bool IBlockExtended.CanStartNewLine => true;

	[Browsable(false)]
	public bool IsBlockContainer => true;

	Rectangle IBlock.Bounds
	{
		get
		{
			return Bounds;
		}
		set
		{
			Bounds = value;
		}
	}

	bool IBlock.Visible
	{
		get
		{
			return Visible;
		}
		set
		{
			Visible = value;
		}
	}

	public ItemContainer()
	{
		m_IsContainer = true;
		AutoCollapseOnClick = true;
		AccessibleRole = (AccessibleRole)20;
		m_BackgroundStyle.StyleChanged += BackgroundStyleChanged;
		_TitleStyle = new ElementStyle();
		_TitleStyle.StyleChanged += TitleStyleChanged;
	}

	protected override void Dispose(bool disposing)
	{
		m_BackgroundStyle.StyleChanged -= BackgroundStyleChanged;
		m_BackgroundStyle.Dispose();
		base.Dispose(disposing);
	}

	private void MarginChanged(object sender, EventArgs e)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	private void PaddingChanged(object sender, EventArgs e)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	private void BackgroundStyleChanged(object sender, EventArgs e)
	{
		OnAppearanceChanged();
	}

	private bool ShouldSerializeMinimumSize()
	{
		return !m_MinimumSize.IsEmpty;
	}

	protected virtual Rectangle GetClipRectangle()
	{
		Rectangle displayRectangle = DisplayRectangle;
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(m_BackgroundStyle, out dispose);
		displayRectangle.X += ElementStyleLayout.LeftWhiteSpace(elementStyle);
		displayRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(elementStyle);
		displayRectangle.Y += ElementStyleLayout.TopWhiteSpace(elementStyle);
		displayRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle);
		if (dispose)
		{
			elementStyle.Dispose();
		}
		return displayRectangle;
	}

	internal void InternalPaintBackground(ItemPaintArgs p)
	{
		PaintBackground(p);
	}

	protected virtual void PaintBackground(ItemPaintArgs p)
	{
		m_BackgroundStyle.SetColorScheme(p.Colors);
		Graphics graphics = p.Graphics;
		BaseRenderer renderer = p.Renderer;
		if (renderer != null)
		{
			ItemContainerRendererEventArgs itemContainerRendererEventArgs = new ItemContainerRendererEventArgs(p.Graphics, this);
			itemContainerRendererEventArgs.ItemPaintArgs = p;
			renderer.DrawItemContainer(itemContainerRendererEventArgs);
		}
		else
		{
			ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(m_BackgroundStyle, graphics, DisplayRectangle));
		}
	}

	private void PaintTitle(ItemPaintArgs p)
	{
		if (string.IsNullOrEmpty(_TitleText))
		{
			return;
		}
		Rectangle bounds = new Rectangle(DisplayRectangle.X, DisplayRectangle.Y, DisplayRectangle.Width, _TitleSize.Height);
		if (_TitlePosition == eTitlePosition.Bottom)
		{
			bounds.Y = DisplayRectangle.Bottom - _TitleSize.Height;
		}
		Graphics graphics = p.Graphics;
		Font font = p.Font;
		if (_TitleStyle.Custom)
		{
			bool dispose = false;
			ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(_TitleStyle, out dispose);
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(elementStyle, graphics, bounds);
			ElementStyleDisplay.Paint(e);
			ElementStyleDisplay.PaintText(e, _TitleText, font);
			if (dispose)
			{
				elementStyle.Dispose();
			}
		}
		else
		{
			TextDrawing.DrawString(graphics, _TitleText, font, SystemColors.ControlText, bounds, eTextFormat.Default);
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Region val = null;
		bool flag = false;
		PaintBackground(p);
		if (IsGroupingContainer)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			val = graphics.Clip;
			graphics.SetClip(displayRectangle, (CombineMode)0);
			flag = true;
		}
		else
		{
			Rectangle clipRectangle = GetClipRectangle();
			val = graphics.Clip;
			graphics.SetClip(clipRectangle, (CombineMode)1);
			flag = true;
		}
		PaintTitle(p);
		ItemDisplay itemDisplay = GetItemDisplay();
		itemDisplay.Paint(this, p);
		if (flag)
		{
			if (val != null)
			{
				graphics.Clip = val;
			}
			else
			{
				graphics.ResetClip();
			}
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (DesignMode && !SystemContainer && p.DesignerSelection)
		{
			Rectangle displayRectangle2 = DisplayRectangle;
			Pen val2 = null;
			val2 = ((!Focused) ? new Pen(Color.FromArgb(80, Color.Black), 1f) : new Pen(Color.FromArgb(190, Color.Navy), 1f));
			val2.DashStyle = (DashStyle)2;
			DisplayHelp.DrawRoundedRectangle(graphics, val2, displayRectangle2, 3);
			val2.Dispose();
			val2 = null;
			Image val3 = (Image)(object)BarFunctions.LoadBitmap("SystemImages.AddMoreItemsContainer.png");
			if (Parent is ItemContainer && !((ItemContainer)Parent).SystemContainer)
			{
				ItemContainer itemContainer = Parent as ItemContainer;
				while (itemContainer != null && !itemContainer.SystemContainer)
				{
					if (new Rectangle(itemContainer.DisplayRectangle.Location, val3.Size).Contains(DisplayRectangle.Location))
					{
						if (displayRectangle2.X + val3.Width + 1 > DisplayRectangle.Right)
						{
							displayRectangle2.X = DisplayRectangle.X;
							if (displayRectangle2.Y + val3.Height >= DisplayRectangle.Bottom)
							{
								break;
							}
							displayRectangle2.Y += val3.Height + 1;
						}
						displayRectangle2.X += val3.Width + 1;
					}
					itemContainer = itemContainer.Parent as ItemContainer;
				}
			}
			m_DesignerSelectionRectangle = new Rectangle(displayRectangle2.X + 1, displayRectangle2.Y + 1, val3.Width, val3.Height);
			graphics.DrawImageUnscaled(val3, m_DesignerSelectionRectangle.Location);
		}
		DrawInsertMarker(p.Graphics);
	}

	protected virtual Size GetEmptyDesignTimeSize()
	{
		Size emptyDesignTimeSize = m_EmptyDesignTimeSize;
		if (m_LayoutOrientation == eOrientation.Horizontal)
		{
			emptyDesignTimeSize.Width += 12;
		}
		else
		{
			emptyDesignTimeSize.Height += 12;
		}
		return emptyDesignTimeSize;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedSize()
	{
		return !_FixedSize.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedSize()
	{
		TypeDescriptor.GetProperties(this)["FixedSize"].SetValue(this, Size.Empty);
	}

	protected virtual void OnFixedSizeChanged(Size oldValue, Size newValue)
	{
		if (oldValue.Width != newValue.Width || oldValue.Height != newValue.Height)
		{
			NeedRecalcSize = true;
			Refresh();
		}
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		if (SubItems.Count == 0)
		{
			if (DesignMode && !SystemContainer)
			{
				m_Rect.Size = GetEmptyDesignTimeSize();
			}
			else if (!_FixedSize.IsEmpty)
			{
				m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, (_FixedSize.Width > 0) ? _FixedSize.Width : m_MinimumSize.Width, (_FixedSize.Height > 0) ? _FixedSize.Height : m_MinimumSize.Height);
			}
			else if (m_MinimumSize.IsEmpty)
			{
				m_Rect = Rectangle.Empty;
			}
			else
			{
				m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, m_MinimumSize.Width, m_MinimumSize.Height);
			}
			base.RecalcSize();
		}
		else
		{
			m_Rect = LayoutItems(m_Rect);
			base.RecalcSize();
		}
	}

	private void LayoutTitle(Rectangle bounds)
	{
		_TitleSize = Size.Empty;
		if (string.IsNullOrEmpty(_TitleText))
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return;
		}
		if (_TitleStyle.Custom)
		{
			bool dispose = false;
			ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(_TitleStyle, out dispose);
			Font font = val.Font;
			if (elementStyle.Font != null)
			{
				font = elementStyle.Font;
			}
			Size size = Size.Empty;
			Graphics val2 = BarFunctions.CreateGraphics(val);
			try
			{
				size = TextDrawing.MeasureString(val2, _TitleText, font, bounds.Width - ElementStyleLayout.HorizontalStyleWhiteSpace(elementStyle), elementStyle.TextFormat);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (bounds.Width > 0)
			{
				_TitleSize.Width = bounds.Width;
			}
			else
			{
				_TitleSize.Width = size.Width;
			}
			_TitleSize.Height = size.Height + ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle);
			if (dispose)
			{
				elementStyle.Dispose();
			}
			return;
		}
		Font font2 = val.Font;
		Graphics val3 = BarFunctions.CreateGraphics(val);
		try
		{
			Size size2 = TextDrawing.MeasureString(val3, _TitleText, font2, bounds.Width);
			if (bounds.Width > 0)
			{
				_TitleSize.Width = bounds.Width;
			}
			else
			{
				_TitleSize.Width = size2.Width;
			}
			_TitleSize.Height = size2.Height + 1;
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	protected virtual Rectangle LayoutItems(Rectangle bounds)
	{
		IContentLayout contentLayout = GetContentLayout();
		BlockLayoutManager blockLayoutManager = GetBlockLayoutManager();
		LayoutTitle(bounds);
		Rectangle layoutRectangle = GetLayoutRectangle(bounds);
		BaseItem[] array = new BaseItem[SubItems.Count];
		if (_FitOversizeItemIntoAvailableWidth)
		{
			for (int i = 0; i < SubItems.Count; i++)
			{
				if (SubItems[i] is ButtonItem buttonItem)
				{
					buttonItem._FitContainer = true;
					buttonItem.WidthInternal = layoutRectangle.Width;
				}
				array[i] = SubItems[i];
			}
		}
		else
		{
			SubItems.CopyTo(array, 0);
		}
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(m_BackgroundStyle, out dispose);
		Rectangle containerBounds = layoutRectangle;
		IBlock[] contentBlocks = array;
		layoutRectangle = contentLayout.Layout(containerBounds, contentBlocks, blockLayoutManager);
		if (!SystemContainer)
		{
			layoutRectangle = (_FixedSize.IsEmpty ? new Rectangle(bounds.Location, layoutRectangle.Size) : new Rectangle(bounds.X, bounds.Y, (_FixedSize.Width > 0) ? _FixedSize.Width : layoutRectangle.Width, (_FixedSize.Height > 0) ? _FixedSize.Height : layoutRectangle.Height));
		}
		layoutRectangle.Height += _TitleSize.Height;
		if (m_MinimumSize.Width > 0 && m_MinimumSize.Width > layoutRectangle.Width)
		{
			layoutRectangle.Width = m_MinimumSize.Width;
		}
		else
		{
			layoutRectangle.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(elementStyle);
		}
		if (m_MinimumSize.Height > 0 && m_MinimumSize.Height > layoutRectangle.Height)
		{
			layoutRectangle.Height = m_MinimumSize.Height;
		}
		else
		{
			layoutRectangle.Height += ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle);
		}
		if (dispose)
		{
			elementStyle.Dispose();
		}
		return layoutRectangle;
	}

	protected virtual Rectangle GetLayoutRectangle(Rectangle bounds)
	{
		if (bounds.Width == 0)
		{
			bounds.Width = 16;
		}
		if (bounds.Height == 0)
		{
			bounds.Height = 16;
		}
		if (_FixedSize.Width > 0)
		{
			bounds.Width = _FixedSize.Width;
		}
		else if (m_MinimumSize.Width > 0)
		{
			bounds.Width = m_MinimumSize.Width;
		}
		if (_FixedSize.Height > 0)
		{
			bounds.Height = _FixedSize.Height;
		}
		else if (m_MinimumSize.Height > 0)
		{
			bounds.Height = m_MinimumSize.Height;
		}
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(m_BackgroundStyle, out dispose);
		bounds.X += ElementStyleLayout.LeftWhiteSpace(elementStyle);
		bounds.Y += ElementStyleLayout.TopWhiteSpace(elementStyle);
		bounds.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(elementStyle);
		bounds.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle);
		if (dispose)
		{
			elementStyle.Dispose();
		}
		if (!_TitleSize.IsEmpty)
		{
			if (_TitlePosition == eTitlePosition.Top)
			{
				bounds.Y += _TitleSize.Height;
				bounds.Height -= _TitleSize.Height;
			}
			else if (_TitlePosition == eTitlePosition.Bottom)
			{
				bounds.Height -= _TitleSize.Height;
			}
		}
		return bounds;
	}

	protected override void OnExternalSizeChange()
	{
		base.OnExternalSizeChange();
		if ((m_VerticalItemAlignment != 0 || IsRightToLeft) && SubItems.Count > 0 && !SuspendLayout)
		{
			LayoutItems(m_Rect);
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem item)
	{
		if (item is ButtonItem && _FitOversizeItemIntoAvailableWidth)
		{
			((ButtonItem)item)._FitContainer = false;
		}
		base.OnAfterItemRemoved(item);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (DesignMode && !SystemContainer)
		{
			BaseItem baseItem = ItemAtLocation(objArg.X, objArg.Y);
			if (baseItem == this && (!baseItem.DesignMode || baseItem.DesignTimeVisible) && GetOwner() is IOwner owner)
			{
				owner.SetFocusItem(this);
				return;
			}
		}
		else if (!DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			if (DesignMode)
			{
				if (GetOwner() is IOwner owner2)
				{
					owner2.SetFocusItem(null);
				}
				LeaveHotSubItem(null);
			}
			return;
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (!DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			LeaveHotSubItem(null);
			if (!SystemContainer)
			{
				return;
			}
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		if (DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			base.InternalMouseUp(objArg);
		}
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (DesignMode && !SystemContainer && !m_DesignerSelectionRectangle.IsEmpty && m_DesignerSelectionRectangle.Contains(x, y))
		{
			return this;
		}
		if (!Visible || !DisplayRectangle.Contains(x, y))
		{
			return null;
		}
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				if (subItem.IsContainer)
				{
					BaseItem baseItem2 = subItem.ItemAtLocation(x, y);
					if (baseItem2 != null)
					{
						return baseItem2;
					}
				}
				else if ((subItem.Visible || base.IsOnCustomizeMenu) && subItem.Displayed && subItem.DisplayRectangle.Contains(x, y) && (!DesignMode || DisplayRectangle.IntersectsWith(subItem.DisplayRectangle)))
				{
					return subItem;
				}
			}
		}
		if (DisplayRectangle.Contains(x, y) && !SystemContainer)
		{
			return this;
		}
		return null;
	}

	private void OnLineAlignmentChanged()
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	private void OnOrientationChanged()
	{
		NeedRecalcSize = true;
		if (m_LayoutManager != null)
		{
			m_LayoutManager.ContentOrientation = ((LayoutOrientation != 0) ? eContentOrientation.Vertical : eContentOrientation.Horizontal);
		}
		OnAppearanceChanged();
	}

	internal void OffsetSubItems(Point offset)
	{
		if (_Offsetting || offset.IsEmpty)
		{
			return;
		}
		try
		{
			_Offsetting = true;
			BaseItem[] array = new BaseItem[SubItems.Count];
			SubItems.CopyTo(array, 0);
			BaseItem[] array2 = array;
			foreach (IBlock block in array2)
			{
				Rectangle bounds = block.Bounds;
				bounds.Offset(offset);
				block.Bounds = bounds;
			}
		}
		finally
		{
			_Offsetting = false;
		}
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		base.OnTopLocationChanged(oldValue);
		Point offset = new Point(0, TopInternal - oldValue);
		OffsetSubItems(offset);
	}

	protected override void OnLeftLocationChanged(int oldValue)
	{
		base.OnLeftLocationChanged(oldValue);
		Point offset = new Point(LeftInternal - oldValue, 0);
		OffsetSubItems(offset);
	}

	public override BaseItem Copy()
	{
		ItemContainer itemContainer = new ItemContainer();
		CopyToItem(itemContainer);
		return itemContainer;
	}

	protected override void CopyToItem(BaseItem c)
	{
		ItemContainer itemContainer = c as ItemContainer;
		itemContainer.BackgroundStyle.ApplyStyle(m_BackgroundStyle);
		itemContainer.HorizontalItemAlignment = HorizontalItemAlignment;
		itemContainer.ItemSpacing = ItemSpacing;
		itemContainer.LayoutOrientation = LayoutOrientation;
		itemContainer.MinimumSize = MinimumSize;
		itemContainer.MultiLine = MultiLine;
		itemContainer.ResizeItemsToFit = ResizeItemsToFit;
		itemContainer.VerticalItemAlignment = VerticalItemAlignment;
		base.CopyToItem(itemContainer);
	}

	protected internal override void OnSubItemExpandChange(BaseItem item)
	{
		base.OnSubItemExpandChange(item);
		if (item.Expanded)
		{
			Expanded = true;
		}
	}

	protected internal override void OnSubItemsClear()
	{
		RefreshImageSize();
		base.OnSubItemsClear();
	}

	protected override void OnDisplayedChanged()
	{
		base.OnDisplayedChanged();
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible)
			{
				subItem.Displayed = Displayed;
			}
		}
		if (!Displayed)
		{
			InternalMouseLeave();
		}
	}

	internal void SetSystemContainer(bool b)
	{
		m_SystemContainer = b;
	}

	protected virtual IContentLayout GetContentLayout()
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Invalid comparison between Unknown and I4
		if (m_LayoutManager == null)
		{
			m_LayoutManager = new SerialContentLayoutManager();
			m_LayoutManager.BlockSpacing = 1;
			m_LayoutManager.FitContainerOversize = false;
			m_LayoutManager.FitContainer = false;
			m_LayoutManager.BeforeNewBlockLayout += LayoutManager_BeforeNewBlockLayout;
		}
		m_LayoutManager.MultiLine = m_MultiLine;
		m_LayoutManager.EvenHeight = m_ResizeItemsToFit;
		if (m_MinimumSize.Width > 0 && m_LayoutOrientation == eOrientation.Vertical)
		{
			m_LayoutManager.VerticalFitContainerWidth = true;
		}
		else
		{
			m_LayoutManager.VerticalFitContainerWidth = false;
		}
		if (m_MinimumSize.Height > 0 && m_LayoutOrientation == eOrientation.Horizontal)
		{
			m_LayoutManager.HorizontalFitContainerHeight = true;
		}
		else
		{
			m_LayoutManager.HorizontalFitContainerHeight = false;
		}
		m_LayoutManager.BlockLineAlignment = eContentVerticalAlignment.Top;
		m_LayoutManager.ContentAlignment = GetContentAlignment();
		m_LayoutManager.ContentVerticalAlignment = GetVerticalContentAlignment();
		m_LayoutManager.ContentOrientation = ((LayoutOrientation != 0) ? eContentOrientation.Vertical : eContentOrientation.Horizontal);
		m_LayoutManager.RightToLeft = false;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null && (int)val.RightToLeft == 1)
		{
			m_LayoutManager.RightToLeft = true;
		}
		else
		{
			m_LayoutManager.RightToLeft = false;
		}
		m_LayoutManager.BlockSpacing = m_ItemSpacing - (IsGroupingContainer ? 1 : 0);
		return m_LayoutManager;
	}

	private void LayoutManager_BeforeNewBlockLayout(object sender, LayoutManagerLayoutEventArgs e)
	{
		if (e.Block is BaseItem baseItem && IsGroupingContainer && !(baseItem is ItemContainer) && e.BlockVisibleIndex > 0)
		{
			if (m_LayoutOrientation == eOrientation.Horizontal)
			{
				e.CurrentPosition.X += m_BeginGroupSpacing;
			}
			else
			{
				e.CurrentPosition.Y += m_BeginGroupSpacing;
			}
		}
	}

	private eContentAlignment GetContentAlignment()
	{
		if (m_HorizontalItemAlignment == eHorizontalItemsAlignment.Left)
		{
			return eContentAlignment.Left;
		}
		if (m_HorizontalItemAlignment == eHorizontalItemsAlignment.Center)
		{
			return eContentAlignment.Center;
		}
		return eContentAlignment.Right;
	}

	private eContentVerticalAlignment GetVerticalContentAlignment()
	{
		if (m_VerticalItemAlignment == eVerticalItemsAlignment.Top)
		{
			return eContentVerticalAlignment.Top;
		}
		if (m_VerticalItemAlignment == eVerticalItemsAlignment.Middle)
		{
			return eContentVerticalAlignment.Middle;
		}
		if (m_VerticalItemAlignment == eVerticalItemsAlignment.Bottom)
		{
			return eContentVerticalAlignment.Bottom;
		}
		return eContentVerticalAlignment.Top;
	}

	private BlockLayoutManager GetBlockLayoutManager()
	{
		return new ItemBlockLayoutManager();
	}

	internal ItemDisplay GetItemDisplay()
	{
		if (m_ItemDisplay == null)
		{
			m_ItemDisplay = new ItemDisplay();
		}
		return m_ItemDisplay;
	}

	private void TitleStyleChanged(object sender, EventArgs e)
	{
		OnAppearanceChanged();
	}

	protected virtual void OnTitleTextChanged(string oldValue, string newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected virtual void OnTitlePositionChanged(eTitlePosition oldValue, eTitlePosition newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return GetContainerInsertPosition(pScreen, dragItem);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
	}

	internal bool SelectFirstItem()
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (!subItem.Visible || !subItem.Displayed)
			{
				continue;
			}
			if (subItem is ItemContainer)
			{
				if (((ItemContainer)subItem).SelectFirstItem())
				{
					SetHotSubItem(null);
					base.HotSubItem = subItem;
					return true;
				}
			}
			else if (CanGetMouseFocus(subItem))
			{
				SetHotSubItem(subItem);
				return true;
			}
		}
		return false;
	}

	private bool CanGetMouseFocus(BaseItem item)
	{
		if (item is LabelItem)
		{
			return false;
		}
		return true;
	}

	public void SetHotSubItem(BaseItem item)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		if (item == m_HotSubItem)
		{
			return;
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
			if (m_AutoExpand && m_HotSubItem.Expanded)
			{
				m_HotSubItem.Expanded = false;
			}
			base.HotSubItem = null;
		}
		if (item != null)
		{
			base.HotSubItem = item;
			m_HotSubItem.InternalMouseEnter();
			m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 1, m_HotSubItem.TopInternal + 1, 0));
			if (ContainerControl is IScrollableItemControl scrollableItemControl)
			{
				scrollableItemControl.KeyboardItemSelected(m_HotSubItem);
			}
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		base.InternalKeyDown(objArg);
		ContainerKeyboardNavigation.ProcessKeyDown(this, objArg);
	}
}
