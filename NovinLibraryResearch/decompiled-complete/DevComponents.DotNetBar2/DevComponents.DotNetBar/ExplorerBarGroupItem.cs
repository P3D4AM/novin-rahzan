using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.ExplorerBarGroupItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class ExplorerBarGroupItem : ImageItem, IDesignTimeProvider
{
	private const int _1CD2 = 25;

	private const string _193D = "Right-click header and choose one of Create commands or use SubItems collection to create new buttons. You can also drag & drop controls on this panel.";

	private Rectangle _1CD3 = Rectangle.Empty;

	private bool _1CD4;

	private int ᇬ = 2;

	private bool ݡ;

	private bool ݠ;

	private Rectangle _1944 = Rectangle.Empty;

	private ItemStyleMapper _᥇;

	private ItemStyleMapper _᥈;

	private ItemStyleMapper _0599;

	private ElementStyle ᄈ;

	private ElementStyle _1CD5;

	private ElementStyle ᇭ;

	private eExplorerBarStockStyle ᇮ = eExplorerBarStockStyle.Custom;

	private bool _1CD6 = true;

	private Point _0AC2;

	private Image _0E66;

	private int դ = -1;

	private Image ຂ;

	private int _1CD7 = 4;

	private bool _1CD8;

	private Color ղ = Color.DarkGray;

	private Color մ = Color.White;

	private Color _1CD9 = SystemColors.ActiveCaption;

	private Color _1CDA = Color.DarkGray;

	private Color _1CDB = Color.White;

	private Color _1CDC = SystemColors.ActiveCaption;

	private bool _1CDD = true;

	private bool _1CDE = true;

	private bool _0B84 = true;

	private ShadowPaintInfo Ꮻ;

	private bool _1CDF;

	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DefaultValue(false)]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			m_ThemeAware = value;
			if (m_SubItems == null)
			{
				return;
			}
			foreach (BaseItem subItem in m_SubItems)
			{
				if (!(subItem is ButtonItem))
				{
					subItem.ThemeAware = value;
				}
			}
		}
	}

	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether expand button is visible.")]
	public bool ExpandButtonVisible
	{
		get
		{
			return _1CD6;
		}
		set
		{
			_1CD6 = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets a value indicating whether group is expanded or not.")]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			base.Expanded = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether drop shadow is displayed when non-themed display is used.")]
	[Category("Appearance")]
	[Browsable(true)]
	public bool DropShadow
	{
		get
		{
			return _0B84;
		}
		set
		{
			_0B84 = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets a normal item style.")]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TitleStyle => ᄈ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets a mouse over item style.")]
	public ElementStyle TitleHotStyle => _1CD5;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Style")]
	[DevCoBrowsable(true)]
	[Description("Gets or sets group background style.")]
	public ElementStyle BackStyle => ᇭ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("This property is obsolete. Use TitleStyle property instead")]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ItemStyleMapper HeaderStyle => _᥇;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Obsolete("This property is obsolete. Use TitleHotStyle property instead")]
	public ItemStyleMapper HeaderHotStyle => _᥈;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Obsolete("This property is obsolete. Use BackStyle property instead")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ItemStyleMapper BackgroundStyle => _0599;

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Determines whether clicking the header of the control expands the item.")]
	[DefaultValue(true)]
	public bool HeaderExpands
	{
		get
		{
			return _1CDD;
		}
		set
		{
			_1CDD = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DefaultValue(eExplorerBarStockStyle.Custom)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Applies the stock style to the object.")]
	[Category("Style")]
	public eExplorerBarStockStyle StockStyle
	{
		get
		{
			return ᇮ;
		}
		set
		{
			ᇮ = value;
			if (ᇮ == eExplorerBarStockStyle.Custom)
			{
				return;
			}
			BarFunctions.SetExplorerBarStyle(this, ᇮ);
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is ButtonItem)
				{
					BarFunctions.SetExplorerBarStyle(subItem as ButtonItem, ᇮ);
				}
			}
			if (ᇮ == eExplorerBarStockStyle.BlueSpecial || ᇮ == eExplorerBarStockStyle.OliveGreenSpecial || ᇮ == eExplorerBarStockStyle.SilverSpecial)
			{
				XPSpecialGroup = true;
			}
			else if (ᇮ != eExplorerBarStockStyle.SystemColors)
			{
				XPSpecialGroup = false;
			}
			Refresh();
		}
	}

	[Description("Determines the display of the item when shown.")]
	[Category("Appearance")]
	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public override eDotNetBarStyle Style
	{
		get
		{
			return base.Style;
		}
		set
		{
			base.Style = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Style")]
	[Description("Specifies expand button border color.")]
	public Color ExpandBorderColor
	{
		get
		{
			return ղ;
		}
		set
		{
			ղ = value;
			OnAppearanceChanged();
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies expand button back color.")]
	public Color ExpandBackColor
	{
		get
		{
			return մ;
		}
		set
		{
			մ = value;
			OnAppearanceChanged();
		}
	}

	[Description("Specifies expand button fore color.")]
	[Category("Style")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color ExpandForeColor
	{
		get
		{
			return _1CD9;
		}
		set
		{
			_1CD9 = value;
			OnAppearanceChanged();
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[Description("Specifies hot expand button border color.")]
	[DevCoBrowsable(true)]
	public Color ExpandHotBorderColor
	{
		get
		{
			return _1CDA;
		}
		set
		{
			_1CDA = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies hot expand button back color.")]
	[Category("Style")]
	public Color ExpandHotBackColor
	{
		get
		{
			return _1CDB;
		}
		set
		{
			_1CDB = value;
			OnAppearanceChanged();
		}
	}

	[Description("Specifies hot expand button fore color.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Style")]
	public Color ExpandHotForeColor
	{
		get
		{
			return _1CDC;
		}
		set
		{
			_1CDC = value;
			OnAppearanceChanged();
		}
	}

	[DefaultValue(eItemAlignment.Near)]
	[Category("Appearance")]
	[Description("Determines alignment of the item inside the container.")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[DefaultValue(false)]
	[DevCoBrowsable(false)]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	[Category("Appearance")]
	[Browsable(false)]
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

	[DefaultValue(true)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Category("Behavior")]
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

	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
	[Browsable(false)]
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
	[Category("Design")]
	[DefaultValue("")]
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

	[DefaultValue("")]
	[Description("Indicates description of the item that is displayed during design.")]
	[Category("Design")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
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

	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Browsable(false)]
	[Description("Indicates whether this item is beginning of the group.")]
	public override bool BeginGroup
	{
		get
		{
			return base.BeginGroup;
		}
		set
		{
			base.BeginGroup = value;
		}
	}

	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DefaultValue(true)]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection of sub items.")]
	[DevCoBrowsable(false)]
	[Editor("DevComponents.DotNetBar.Design.ExplorerBarGroupItemEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Data")]
	[Browsable(true)]
	public override SubItemsCollection SubItems => base.SubItems;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Rectangle PanelRect => _1944;

	internal bool _193C => base.IsThemed;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImageList ImageList
	{
		get
		{
			if (GetOwner() is IOwner owner)
			{
				return owner.ImagesMedium;
			}
			return null;
		}
	}

	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Description("The image that will be displayed on the face of the item.")]
	[Browsable(true)]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			NeedRecalcSize = true;
			_0E66 = value;
			OnImageChanged();
			Refresh();
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether XP themed special group colors are used for drawing.")]
	public bool XPSpecialGroup
	{
		get
		{
			return _1CD8;
		}
		set
		{
			_1CD8 = value;
			Refresh();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates margin in pixels between the edge of the container and the items contained inside of it")]
	[DefaultValue(4)]
	public int SubItemsMargin
	{
		get
		{
			return _1CD7;
		}
		set
		{
			_1CD7 = value;
		}
	}

	[Description("Indicates whether text on sub items is wrapped on new line if it cannot fit the space available.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool WordWrapSubItems
	{
		get
		{
			return _1CDE;
		}
		set
		{
			_1CDE = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is ButtonItem)
				{
					((ButtonItem)subItem)._ງ = _1CDE;
				}
			}
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(-1)]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed on the face of the item.")]
	[TypeConverter(typeof(ImageIndexConverter))]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			ຂ = null;
			if (դ != value)
			{
				դ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ImageIndex");
				}
				if (m_Parent != null)
				{
					OnImageChanged();
					NeedRecalcSize = true;
					Refresh();
				}
			}
		}
	}

	public ExplorerBarGroupItem()
		: this("", "")
	{
	}

	public ExplorerBarGroupItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public ExplorerBarGroupItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		m_IsContainer = true;
		m_AllowOnlyOneSubItemExpanded = false;
		ᄈ = new ElementStyle();
		ᄈ.StyleChanged += ઐ;
		_1CD5 = new ElementStyle();
		_1CD5.StyleChanged += ઐ;
		ᇭ = new ElementStyle();
		ᇭ.StyleChanged += ઐ;
		_᥇ = new ItemStyleMapper(ᄈ);
		_᥈ = new ItemStyleMapper(_1CD5);
		_0599 = new ItemStyleMapper(ᇭ);
		SubItemsImageSize = new Size(12, 12);
		ImageSize = new Size(12, 12);
		BarFunctions.SetExplorerBarStyle(this, ᇮ);
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.ᆟ(ref _0E66);
			BarUtilities.ᆟ(ref ຂ);
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		ExplorerBarGroupItem explorerBarGroupItem = new ExplorerBarGroupItem();
		CopyToItem(explorerBarGroupItem);
		return explorerBarGroupItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		ExplorerBarGroupItem objCopy = copy as ExplorerBarGroupItem;
		base.CopyToItem(objCopy);
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		ઐ();
	}

	internal void ઐ()
	{
		OnAppearanceChanged();
	}

	public override void RecalcSize()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = Graphics.FromHwnd(val.Handle);
		SizeF empty = SizeF.Empty;
		Image val3 = _0EC5();
		string text = m_Text;
		if (text == "")
		{
			text = " ";
		}
		int num = Dpi.Width(ᇬ);
		int num2 = m_Rect.Width - num * 2;
		int num3 = Dpi.Width(25);
		if (_1CD6)
		{
			num2 -= num3;
		}
		if (val3 != null)
		{
			num2 -= val3.Width + num * 2;
		}
		if (num2 <= 0)
		{
			num2 = 1;
		}
		bool disposeFont = false;
		Font font = GetFont(out disposeFont);
		empty = TextDrawing.MeasureString(val2, text, font, num2, ᄈ.ݽ);
		if (disposeFont)
		{
			font.Dispose();
		}
		val2.Dispose();
		m_Rect.Height = Dpi.Height23;
		if (m_Rect.Height < (int)empty.Height + num * 2)
		{
			m_Rect.Height = (int)empty.Height + num * 2;
		}
		if (val3 != null)
		{
			int height = m_Rect.Height;
			if (val3.Height > m_Rect.Height)
			{
				m_Rect.Height = val3.Height + num * 2;
			}
			_1944 = new Rectangle(0, m_Rect.Height - height, m_Rect.Width, height);
		}
		else
		{
			_1944 = new Rectangle(0, 0, m_Rect.Width, m_Rect.Height);
		}
		if (_1CD6)
		{
			_1CD3 = new Rectangle(_1944.Right - num3, _1944.Y, num3, _1944.Height);
		}
		if (Expanded)
		{
			if (m_SubItems != null)
			{
				int num4 = m_Rect.Bottom + 1;
				int leftInternal = m_Rect.Left + _1CD7;
				int num5 = -1;
				foreach (BaseItem subItem in m_SubItems)
				{
					num5++;
					if (!subItem.Visible)
					{
						subItem.Displayed = false;
						continue;
					}
					subItem.WidthInternal = m_Rect.Width - _1CD7 * 2;
					subItem.RecalcSize();
					subItem.WidthInternal = m_Rect.Width - _1CD7 * 2;
					if (subItem.BeginGroup)
					{
						num4 += Dpi.Height3;
					}
					subItem.TopInternal = num4;
					subItem.LeftInternal = leftInternal;
					num4 += subItem.HeightInternal;
					subItem.Displayed = true;
				}
				m_Rect.Height = num4 - m_Rect.Top + Dpi.Height2;
			}
		}
		else
		{
			foreach (BaseItem subItem2 in m_SubItems)
			{
				subItem2.Displayed = false;
			}
		}
		if (Expanded && DesignMode && SubItems.Count == 0 && Parent != null && Parent.SubItems.Count == 1)
		{
			m_Rect.Height += 64;
		}
		base.RecalcSize();
	}

	private Image _0EC5()
	{
		if (_0E66 != null)
		{
			return _0E66;
		}
		if (դ >= 0)
		{
			return _0EC9(դ);
		}
		return null;
	}

	private Image _0EC9(int _0ECA)
	{
		if (_0ECA >= 0)
		{
			IOwner owner = null;
			Bar bar = null;
			if (owner == null)
			{
				owner = GetOwner() as IOwner;
			}
			if (bar == null)
			{
				bar = ContainerControl as Bar;
			}
			if (owner != null)
			{
				try
				{
					if (owner.ImagesMedium != null)
					{
						if (ຂ == null)
						{
							ຂ = owner.ImagesMedium.Images[_0ECA];
						}
						return ຂ;
					}
				}
				catch (Exception)
				{
					return null;
				}
			}
		}
		return null;
	}

	private ShadowPaintInfo ᱍ()
	{
		if (Ꮻ == null)
		{
			Ꮻ = new ShadowPaintInfo();
		}
		Ꮻ.Size = 3;
		return Ꮻ;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Expected O, but got Unknown
		if (DisplayRectangle.Width <= 0 || DisplayRectangle.Height <= 0 || _1CDF)
		{
			return;
		}
		_1CDF = true;
		try
		{
			if (base.IsThemed)
			{
				_0DF4(pa);
				DrawInsertMarker(pa.Graphics);
			}
			else
			{
				if (SuspendLayout)
				{
					return;
				}
				Graphics graphics = pa.Graphics;
				if (m_NeedRecalcSize)
				{
					RecalcSize();
				}
				Rectangle rectangle = _1944;
				rectangle.Offset(m_Rect.X, m_Rect.Y);
				bool disposeFont = false;
				Font font = GetFont(out disposeFont);
				_ = ContainerControl;
				Image val = _0EC5();
				Rectangle rectangle2 = rectangle;
				int num = Dpi.Width(ᇬ);
				rectangle2.X += num;
				rectangle2.X += num * 2;
				rectangle2.Width -= num * 3;
				if (val != null)
				{
					rectangle2.Width -= val.Width + num;
					rectangle2.X += val.Width + num;
				}
				if (_1CD6)
				{
					rectangle2.Width -= Dpi.Width(25);
				}
				ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo();
				elementStyleDisplayInfo.Bounds = rectangle;
				elementStyleDisplayInfo.Graphics = graphics;
				if (ݡ)
				{
					elementStyleDisplayInfo.Style = _1CD5;
				}
				else
				{
					elementStyleDisplayInfo.Style = ᄈ;
				}
				ElementStyleDisplay.Paint(elementStyleDisplayInfo);
				if (elementStyleDisplayInfo.Style.Font != null)
				{
					font = elementStyleDisplayInfo.Style.Font;
				}
				if (pa.RightToLeft)
				{
					TextDrawing.DrawString(graphics, m_Text, font, elementStyleDisplayInfo.Style.TextColor, _1CE0(_1944, rectangle2), elementStyleDisplayInfo.Style.ݽ | eTextFormat.RightToLeft);
				}
				else
				{
					TextDrawing.DrawString(graphics, m_Text, font, elementStyleDisplayInfo.Style.TextColor, rectangle2, elementStyleDisplayInfo.Style.ݽ);
				}
				if (_1CD6)
				{
					Rectangle ल = _1CD3;
					ल.Offset(m_Rect.X, m_Rect.Y);
					if (pa.RightToLeft)
					{
						_102B(pa, _1CE0(m_Rect, ल), ݡ, ݠ, m_Expanded);
					}
					else
					{
						_102B(pa, ल, ݡ, ݠ, m_Expanded);
					}
				}
				int cornerDiameter = ᄈ.CornerDiameter;
				Rectangle bounds = new Rectangle(m_Rect.X, _1944.Bottom + m_Rect.Y, m_Rect.Width, m_Rect.Height - _1944.Bottom);
				if (bounds.Width > 0 && bounds.Height > 0)
				{
					elementStyleDisplayInfo.Style = ᇭ;
					elementStyleDisplayInfo.Bounds = bounds;
					ElementStyleDisplay.Paint(elementStyleDisplayInfo);
					if (_0B84)
					{
						ShadowPaintInfo shadowPaintInfo = ᱍ();
						shadowPaintInfo.Rectangle = new Rectangle(m_Rect.X, m_Rect.Y + _1944.Top + cornerDiameter, m_Rect.Width, m_Rect.Height - _1944.Top - cornerDiameter);
						shadowPaintInfo.Graphics = graphics;
						ShadowPainter.Paint(shadowPaintInfo);
					}
				}
				else if (_0B84)
				{
					rectangle.Y += cornerDiameter;
					rectangle.Height -= cornerDiameter;
					ShadowPaintInfo shadowPaintInfo2 = ᱍ();
					shadowPaintInfo2.Rectangle = rectangle;
					shadowPaintInfo2.Graphics = graphics;
					ShadowPainter.Paint(shadowPaintInfo2);
				}
				if (val != null)
				{
					if (pa.RightToLeft)
					{
						graphics.DrawImage(val, _1CE0(_1944, new Rectangle(rectangle.Left + num, rectangle.Bottom - num - val.Height, val.Width, val.Height)));
					}
					else
					{
						graphics.DrawImage(val, rectangle.Left + num, rectangle.Bottom - num - val.Height, val.Width, val.Height);
					}
				}
				if (Focused)
				{
					if (DesignMode)
					{
						Rectangle ल2 = rectangle;
						ल2.Inflate(-1, -1);
						if (pa.RightToLeft)
						{
							ल2 = _1CE0(_1944, ल2);
						}
						_1A6B._1A6C(graphics, rectangle, pa.Colors.ItemDesignTimeBorder);
					}
					else
					{
						Rectangle rectangle3 = rectangle2;
						rectangle3.Inflate(0, -1);
						rectangle3.Width -= 2;
						rectangle3.X -= 2;
						if (pa.RightToLeft)
						{
							rectangle3 = _1CE0(_1944, rectangle3);
						}
						ControlPaint.DrawFocusRectangle(graphics, rectangle3);
					}
				}
				if ((Expanded || (Parent is ExplorerBarContainerItem && ((ExplorerBarContainerItem)Parent)._ሑ)) && m_SubItems != null)
				{
					rectangle = new Rectangle(m_Rect.X, m_Rect.Y + rectangle.Height + 1, m_Rect.Width, m_Rect.Height - rectangle.Height - 1);
					for (int i = 0; i < m_SubItems.Count; i++)
					{
						BaseItem baseItem = m_SubItems[i];
						if (!baseItem.Displayed || !baseItem.Visible)
						{
							continue;
						}
						if (baseItem.BeginGroup)
						{
							Pen val2 = new Pen(pa.Colors.ItemSeparator, 1f);
							try
							{
								graphics.DrawLine(val2, baseItem.LeftInternal + 2, baseItem.TopInternal - 2, baseItem.DisplayRectangle.Right - 4, baseItem.TopInternal - 2);
							}
							finally
							{
								((IDisposable)val2)?.Dispose();
							}
						}
						baseItem.Paint(pa);
					}
				}
				if (disposeFont)
				{
					font.Dispose();
				}
				DrawInsertMarker(pa.Graphics);
				ᥓ(pa);
			}
		}
		finally
		{
			_1CDF = false;
		}
	}

	private Rectangle _1CE0(Rectangle ശ, Rectangle ल)
	{
		return new Rectangle(ശ.Right - (ल.Width + (ल.X - ശ.X)), ल.Y, ल.Width, ल.Height);
	}

	private void _0DF4(ItemPaintArgs _0BD2)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		Bitmap val = new Bitmap(DisplayRectangle.Width, DisplayRectangle.Height, _0BD2.Graphics);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		_0A7E obj = _0BD2._0A7E;
		Rectangle rectangle = _1944;
		_0FAD _070B = _0FAD._0FB5;
		_0FBA _0FCF = _0FBA._0FCF;
		if (_1CD8)
		{
			_070B = _0FAD._0FB9;
		}
		obj._0EE5(val2, _070B, _0FCF, rectangle);
		Rectangle ल = new Rectangle(0, _1944.Bottom, m_Rect.Width + (_1CD8 ? 0 : 0), m_Rect.Height - _1944.Bottom);
		if (ल.Width > 0 && ल.Height > 0)
		{
			_0FAD _070B2;
			_0FBA య;
			if (_1CD8)
			{
				_070B2 = _0FAD._0FB6;
				య = _0FBA._0FD0;
			}
			else
			{
				_070B2 = _0FAD._0FB2;
				య = _0FBA._0FC8;
			}
			obj._0EE5(val2, _070B2, య, ल);
		}
		Image val3 = _0EC5();
		Rectangle rectangle2 = rectangle;
		int num = Dpi.Width(ᇬ);
		rectangle2.X += num;
		rectangle2.X += num * 2;
		rectangle2.Width -= num * 5;
		if (val3 != null)
		{
			rectangle2.Width -= val3.Width + num;
			rectangle2.X += val3.Width + num;
		}
		if (_1CD6)
		{
			rectangle2.Width -= Dpi.Width(25);
		}
		bool disposeFont = false;
		Font font = GetFont(out disposeFont);
		ElementStyle elementStyle = ᄈ;
		if (ݡ)
		{
			elementStyle = _1CD5;
		}
		if (elementStyle.Font != null)
		{
			font = elementStyle.Font;
		}
		if (_0BD2.RightToLeft)
		{
			TextDrawing.DrawStringLegacy(val2, m_Text, font, elementStyle.TextColor, _1CE0(_1944, rectangle2), elementStyle.ݽ | eTextFormat.RightToLeft);
		}
		else
		{
			TextDrawing.DrawStringLegacy(val2, m_Text, font, elementStyle.TextColor, rectangle2, elementStyle.ݽ);
		}
		if (_1CD6)
		{
			Rectangle ल2 = _1CD3;
			if (_1CD8)
			{
				if (Expanded)
				{
					_070B = _0FAD._0FB7;
					_0FCF = ((!_1CD4) ? _0FBA._0FD1 : _0FBA._0FD2);
				}
				else
				{
					_070B = _0FAD._0FB8;
					_0FCF = ((!_1CD4) ? _0FBA._0FD4 : _0FBA._0FD5);
				}
			}
			else if (Expanded)
			{
				_070B = _0FAD._0FB3;
				_0FCF = ((!_1CD4) ? _0FBA._0FC9 : _0FBA._0FCA);
			}
			else
			{
				_070B = _0FAD._0FB4;
				_0FCF = ((!_1CD4) ? _0FBA._0FCC : _0FBA._0FCD);
			}
			if (_0BD2.RightToLeft)
			{
				obj._0EE5(val2, _070B, _0FCF, _1CE0(m_Rect, ल2));
			}
			else
			{
				obj._0EE5(val2, _070B, _0FCF, ल2);
			}
		}
		if (val3 != null)
		{
			if (_0BD2.RightToLeft)
			{
				val2.DrawImage(val3, _1CE0(_1944, new Rectangle(rectangle.Left + num, rectangle.Bottom - num - val3.Height, val3.Width, val3.Height)));
			}
			else
			{
				val2.DrawImage(val3, rectangle.Left + num, rectangle.Bottom - num - val3.Height, val3.Width, val3.Height);
			}
		}
		if (Focused)
		{
			if (DesignMode)
			{
				Rectangle ल3 = rectangle;
				ल3.Inflate(-1, -1);
				if (_0BD2.RightToLeft)
				{
					ल3 = _1CE0(_1944, ल3);
				}
				_1A6B._1A6C(val2, rectangle, _0BD2.Colors.ItemDesignTimeBorder);
			}
			else
			{
				Rectangle rectangle3 = rectangle2;
				rectangle3.Inflate(0, -1);
				rectangle3.Width -= 2;
				rectangle3.X -= 2;
				if (_0BD2.RightToLeft)
				{
					rectangle3 = _1CE0(_1944, rectangle3);
				}
				ControlPaint.DrawFocusRectangle(val2, rectangle3);
			}
		}
		val2.Dispose();
		val2 = _0BD2.Graphics;
		val2.DrawImage((Image)(object)val, DisplayRectangle.X, DisplayRectangle.Y, ((Image)val).Width, ((Image)val).Height);
		((Image)val).Dispose();
		if (disposeFont)
		{
			font.Dispose();
		}
		if ((Expanded || (Parent is ExplorerBarContainerItem && ((ExplorerBarContainerItem)Parent)._ሑ)) && m_SubItems != null)
		{
			rectangle = new Rectangle(m_Rect.X, m_Rect.Y + rectangle.Height + 1, m_Rect.Width, m_Rect.Height - rectangle.Height - 1);
			for (int i = 0; i < m_SubItems.Count; i++)
			{
				BaseItem baseItem = m_SubItems[i];
				if (baseItem.Displayed && baseItem.Visible)
				{
					baseItem.Paint(_0BD2);
				}
			}
		}
		ᥓ(_0BD2);
	}

	private void ᥓ(ItemPaintArgs _0BD2)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		if (m_SubItems != null && m_SubItems.Count == 0 && DesignMode)
		{
			Rectangle rect = m_Rect;
			if (!_1944.IsEmpty)
			{
				rect.Y += _1944.Height;
				rect.Height -= _1944.Height;
			}
			string text = "Right-click header and choose one of Create commands or use SubItems collection to create new buttons. You can also drag & drop controls on this panel.";
			rect.Inflate(-1, -1);
			eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			Font val = new Font(SystemFonts.DefaultFont.FontFamily, 7f);
			TextDrawing.DrawString(_0BD2.Graphics, text, val, SystemColors.ControlText, rect, format);
			val.Dispose();
		}
	}

	private void _1CE1()
	{
		if (!_1CD6)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			_0A7E obj = null;
			bool flag = false;
			if (val is ል)
			{
				obj = ((ል)val)._0A7E;
			}
			else
			{
				flag = true;
				obj = new _0A7E(val);
			}
			_0FAD obj2 = _0FAD._0FB3;
			_0FBA _0FC9 = _0FBA._0FC9;
			Rectangle ल = _1CD3;
			ल.Offset(m_Rect.X, m_Rect.Y);
			if (_1CD8)
			{
				if (Expanded)
				{
					obj2 = _0FAD._0FB7;
					_0FC9 = ((!_1CD4) ? _0FBA._0FD1 : _0FBA._0FD2);
				}
				else
				{
					obj2 = _0FAD._0FB8;
					_0FC9 = ((!_1CD4) ? _0FBA._0FD4 : _0FBA._0FD5);
				}
			}
			else if (Expanded)
			{
				obj2 = _0FAD._0FB3;
				_0FC9 = ((!_1CD4) ? _0FBA._0FC9 : _0FBA._0FCA);
			}
			else
			{
				obj2 = _0FAD._0FB4;
				_0FC9 = ((!_1CD4) ? _0FBA._0FCC : _0FBA._0FCD);
			}
			obj._0EE5(val2, obj2, _0FC9, ल);
			if (flag)
			{
				obj.Dispose();
			}
		}
		finally
		{
			if (val2 != null)
			{
				val2.SmoothingMode = (SmoothingMode)0;
				val2.TextRenderingHint = (TextRenderingHint)0;
				val2.Dispose();
			}
		}
	}

	private void _102B()
	{
		if (_1CD6)
		{
			Rectangle rectangle = _1CD3;
			rectangle.Offset(m_Rect.X, m_Rect.Y);
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Invalidate(rectangle);
				val.Update();
			}
		}
	}

	private void _102B(ItemPaintArgs _0BD2, Rectangle ल, bool _1CE2, bool _1CE3, bool ᚤ)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		if (_0BD2.ContainerControl is ExplorerBar explorerBar && ((explorerBar.GroupButtonExpandNormal != null && !ᚤ) || (explorerBar.GroupButtonCollapseNormal != null && ᚤ)))
		{
			Image val = explorerBar.GroupButtonExpandNormal;
			if (ᚤ)
			{
				val = explorerBar.GroupButtonCollapseNormal;
			}
			if (_1CE3)
			{
				if (!ᚤ && explorerBar.GroupButtonExpandPressed != null)
				{
					val = explorerBar.GroupButtonExpandPressed;
				}
				else if (ᚤ && explorerBar.GroupButtonCollapsePressed != null)
				{
					val = explorerBar.GroupButtonCollapsePressed;
				}
			}
			else if (_1CE2)
			{
				if (!ᚤ && explorerBar.GroupButtonExpandHot != null)
				{
					val = explorerBar.GroupButtonExpandHot;
				}
				else if (ᚤ && explorerBar.GroupButtonCollapseHot != null)
				{
					val = explorerBar.GroupButtonCollapseHot;
				}
			}
			Rectangle rectangle = ल;
			rectangle.Y += (rectangle.Height - val.Height) / 2;
			graphics.DrawImageUnscaled(val, rectangle);
			return;
		}
		int width = Dpi.Width16;
		Pen val2 = new Pen(_1CE2 ? _1CDA : ղ, 1f);
		try
		{
			if (ल.Width > width)
			{
				ल.Offset((ल.Width - width) / 2, 0);
			}
			if (ल.Height > width)
			{
				ल.Offset(0, (ल.Height - width) / 2);
			}
			ल.Width = width;
			ल.Height = width;
			Brush val3 = (Brush)new SolidBrush(_1CE2 ? _1CDB : մ);
			graphics.FillEllipse(val3, ल);
			val3.Dispose();
			graphics.DrawEllipse(val2, ल);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		Pen val4 = new Pen(_1CE2 ? _1CDC : _1CD9, (float)Dpi.Width1);
		try
		{
			if (ᚤ)
			{
				Point point = new Point(ल.X + width / 2, ल.Y + Dpi.Height4);
				graphics.DrawLine(val4, point.X, point.Y, point.X - Dpi.Width3, point.Y + Dpi.Height3);
				graphics.DrawLine(val4, point.X, point.Y, point.X + Dpi.Width3, point.Y + Dpi.Height3);
				graphics.DrawLine(val4, point.X, point.Y + Dpi.Height1, point.X - Dpi.Width3, point.Y + Dpi.Height4);
				graphics.DrawLine(val4, point.X, point.Y + Dpi.Height1, point.X + Dpi.Width3, point.Y + Dpi.Height4);
				point.Y += Dpi.Height4;
				graphics.DrawLine(val4, point.X, point.Y, point.X - Dpi.Width3, point.Y + Dpi.Height3);
				graphics.DrawLine(val4, point.X, point.Y, point.X + Dpi.Width3, point.Y + Dpi.Height3);
				graphics.DrawLine(val4, point.X, point.Y + Dpi.Height1, point.X - Dpi.Width3, point.Y + Dpi.Height4);
				graphics.DrawLine(val4, point.X, point.Y + Dpi.Height1, point.X + Dpi.Width3, point.Y + Dpi.Height4);
			}
			else
			{
				Point point2 = new Point(ल.X + width / 2, ल.Y + Dpi.Height8);
				graphics.DrawLine(val4, point2.X, point2.Y, point2.X - Dpi.Width3, point2.Y - Dpi.Height3);
				graphics.DrawLine(val4, point2.X, point2.Y, point2.X + Dpi.Width3, point2.Y - Dpi.Height3);
				graphics.DrawLine(val4, point2.X, point2.Y - Dpi.Height1, point2.X - Dpi.Width3, point2.Y - Dpi.Height4);
				graphics.DrawLine(val4, point2.X, point2.Y - Dpi.Height1, point2.X + Dpi.Width3, point2.Y - Dpi.Height4);
				point2.Y += Dpi.Height4;
				graphics.DrawLine(val4, point2.X, point2.Y, point2.X - Dpi.Width3, point2.Y - Dpi.Height3);
				graphics.DrawLine(val4, point2.X, point2.Y, point2.X + Dpi.Width3, point2.Y - Dpi.Height3);
				graphics.DrawLine(val4, point2.X, point2.Y - Dpi.Height1, point2.X - Dpi.Width3, point2.Y - Dpi.Height4);
				graphics.DrawLine(val4, point2.X, point2.Y - Dpi.Height1, point2.X + Dpi.Width3, point2.Y - Dpi.Height4);
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	public override void SubItemSizeChanged(BaseItem objChildItem)
	{
		NeedRecalcSize = true;
	}

	private void ᥕ(Graphics _٥, Rectangle भ, Color _0BDE, bool ᥖ)
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		Point[] array = new Point[3];
		if (ᥖ)
		{
			array[0].X = भ.Left + (भ.Width - 9) / 2;
			array[0].Y = भ.Top + भ.Height / 2 + 1;
			array[1].X = array[0].X + 8;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 4;
			array[2].Y = array[0].Y - 5;
		}
		else
		{
			array[0].X = भ.Left + (भ.Width - 7) / 2;
			array[0].Y = भ.Top + (भ.Height - 4) / 2;
			array[1].X = array[0].X + 7;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 3;
			array[2].Y = array[0].Y + 4;
		}
		_٥.FillPolygon((Brush)new SolidBrush(_0BDE), array);
	}

	protected virtual Font GetFont(out bool disposeFont)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		disposeFont = false;
		if (TitleStyle.Font != null)
		{
			return TitleStyle.Font;
		}
		Font val = null;
		object containerControl = ContainerControl;
		Control val2 = (Control)((containerControl is Control) ? containerControl : null);
		if (val2 != null)
		{
			val = val2.Font;
		}
		else
		{
			val = SystemFonts.DefaultFont;
			disposeFont = true;
		}
		if (!val.Bold)
		{
			val = new Font(val, (FontStyle)1);
			disposeFont = true;
		}
		return val;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && (Math.Abs(objArg.X - _0AC2.X) >= 4 || Math.Abs(objArg.Y - _0AC2.Y) >= 4) && ContainerControl is ExplorerBar { AllowUserCustomize: not false } && CanCustomize)
		{
			BaseItem hotSubItem = m_HotSubItem;
			if (hotSubItem != null && hotSubItem.CanCustomize && !hotSubItem.SystemItem && GetOwner() is IOwner { DragItem: null } owner)
			{
				owner.StartItemDrag(hotSubItem);
				return;
			}
		}
		Rectangle rectangle = _1944;
		rectangle.Offset(m_Rect.Location);
		if (rectangle.Contains(objArg.X, objArg.Y))
		{
			if (!ݡ)
			{
				ݡ = true;
				if (_1CDD)
				{
					Cursor = Cursors.Hand;
				}
				Refresh();
			}
		}
		else if (ݡ)
		{
			if (_1CDD)
			{
				Cursor = Cursors.Default;
			}
			ݡ = false;
			Refresh();
		}
		Rectangle rectangle2 = _1CD3;
		rectangle2.Offset(m_Rect.Location);
		if (_1CD6 && rectangle2.Contains(objArg.X, objArg.Y))
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				m_HotSubItem = null;
			}
			_1CD4 = true;
			_102B();
		}
		else
		{
			if (_1CD4)
			{
				_1CD4 = false;
				_102B();
			}
			base.InternalMouseMove(objArg);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		Cursor = Cursors.Default;
		base.InternalMouseLeave();
		ݡ = false;
		ݠ = false;
		if (_1CD4)
		{
			_1CD4 = false;
		}
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Invalid comparison between Unknown and I4
		Rectangle rectangle = _1944;
		rectangle.Offset(m_Rect.Location);
		_0AC2 = new Point(objArg.X, objArg.Y);
		if (DesignMode)
		{
			if (ItemAtLocation(objArg.X, objArg.Y) == null && !rectangle.Contains(objArg.X, objArg.Y) && Expanded)
			{
				if (GetOwner() is IOwner owner)
				{
					owner.SetFocusItem(null);
				}
			}
			else if (!rectangle.Contains(objArg.X, objArg.Y))
			{
				base.InternalMouseDown(objArg);
			}
			return;
		}
		Rectangle rectangle2 = _1CD3;
		rectangle2.Offset(m_Rect.X, m_Rect.Y);
		if (_1CD6 && (int)objArg.Button == 1048576 && rectangle2.Contains(objArg.X, objArg.Y))
		{
			_1CE4();
			return;
		}
		base.InternalMouseDown(objArg);
		if ((int)objArg.Button == 1048576 && rectangle.Contains(objArg.X, objArg.Y))
		{
			ݠ = true;
			if (_1CDD)
			{
				Expanded = !Expanded;
			}
			else
			{
				Refresh();
			}
		}
	}

	internal void _1CE4()
	{
		Expanded = !Expanded;
	}

	protected internal override void OnExpandChange()
	{
		base.OnExpandChange();
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible)
			{
				subItem.Displayed = Expanded;
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576)
		{
			ݠ = false;
			Refresh();
		}
		base.InternalMouseUp(objArg);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		base.InternalClick(mb, mpos);
		if (GetOwner() is IOwner)
		{
			_ = ((IOwner)GetOwner()).DragInProgress;
		}
	}

	protected internal override void OnSubItemsClear()
	{
		base.OnSubItemsClear();
		NeedRecalcSize = true;
		if (DesignMode)
		{
			Refresh();
		}
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		int num = m_Rect.Top - oldValue;
		if (m_SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			if (subItem.Visible)
			{
				subItem.TopInternal += num;
			}
		}
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (ContainerControl is ExplorerBar)
		{
			ExplorerBar explorerBar = ContainerControl as ExplorerBar;
			ᄈ.SetColorScheme(explorerBar.ColorScheme);
			_1CD5.SetColorScheme(explorerBar.ColorScheme);
			ᇭ.SetColorScheme(explorerBar.ColorScheme);
		}
	}

	protected internal override void OnVisibleChanged(bool bVisible)
	{
		base.OnVisibleChanged(bVisible);
		if (bVisible)
		{
			return;
		}
		foreach (BaseItem subItem in SubItems)
		{
			subItem.Displayed = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public bool ShouldSerializeExpandBorderColor()
	{
		if (ղ != Color.DarkGray)
		{
			return ᇮ == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	public bool ShouldSerializeExpandBackColor()
	{
		if (մ != Color.White)
		{
			return ᇮ == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool ShouldSerializeExpandForeColor()
	{
		if (_1CD9 != SystemColors.ActiveCaption)
		{
			return ᇮ == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandHotBorderColor()
	{
		if (_1CDA != Color.DarkGray)
		{
			return ᇮ == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public bool ShouldSerializeExpandHotBackColor()
	{
		if (_1CDB != Color.White)
		{
			return ᇮ == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	public bool ShouldSerializeExpandHotForeColor()
	{
		if (_1CDC != SystemColors.ActiveCaption)
		{
			return ᇮ == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("stockstyle", XmlConvert.ToString((int)ᇮ));
		itemXmlElement.SetAttribute("expvisible", XmlConvert.ToString(_1CD6));
		itemXmlElement.SetAttribute("subitemsmargin", XmlConvert.ToString(_1CD7));
		itemXmlElement.SetAttribute("xpspecial", XmlConvert.ToString(_1CD8));
		itemXmlElement.SetAttribute("expborder", BarFunctions.ColorToString(ղ));
		itemXmlElement.SetAttribute("expbc", BarFunctions.ColorToString(մ));
		itemXmlElement.SetAttribute("expfc", BarFunctions.ColorToString(_1CD9));
		itemXmlElement.SetAttribute("exphborder", BarFunctions.ColorToString(_1CDA));
		itemXmlElement.SetAttribute("exphbc", BarFunctions.ColorToString(_1CDB));
		itemXmlElement.SetAttribute("exphfc", BarFunctions.ColorToString(_1CDC));
		itemXmlElement.SetAttribute("headerexp", XmlConvert.ToString(_1CDD));
		itemXmlElement.SetAttribute("expanded", XmlConvert.ToString(m_Expanded));
		if (ᇮ == eExplorerBarStockStyle.Custom)
		{
			XmlElement xmlElement = itemXmlElement.OwnerDocument.CreateElement("backstyle");
			itemXmlElement.AppendChild(xmlElement);
			ሊ(ᇭ, xmlElement);
		}
		if (ᇮ == eExplorerBarStockStyle.Custom)
		{
			XmlElement xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("headerhotstyle");
			itemXmlElement.AppendChild(xmlElement2);
			ሊ(_1CD5, xmlElement2);
		}
		if (ᇮ == eExplorerBarStockStyle.Custom)
		{
			XmlElement xmlElement3 = itemXmlElement.OwnerDocument.CreateElement("headerstyle");
			itemXmlElement.AppendChild(xmlElement3);
			ሊ(ᄈ, xmlElement3);
		}
		if (դ != -1)
		{
			itemXmlElement.SetAttribute("imageindex", XmlConvert.ToString(դ));
		}
		else if (_0E66 != null)
		{
			XmlElement xmlElement4 = itemXmlElement.OwnerDocument.CreateElement("image");
			itemXmlElement.AppendChild(xmlElement4);
			BarFunctions.SerializeImage(_0E66, xmlElement4);
		}
	}

	private void ሊ(ElementStyle _0657, XmlElement ላ)
	{
		ElementSerializer.Serialize(_0657, ላ);
	}

	private void ሌ(ElementStyle _0657, XmlElement ላ)
	{
		ElementSerializer.Deserialize(_0657, ላ);
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		ᇮ = (eExplorerBarStockStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("stockstyle"));
		_1CD6 = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("expvisible"));
		_1CD7 = XmlConvert.ToInt32(itemXmlElement.GetAttribute("subitemsmargin"));
		_1CD8 = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("xpspecial"));
		ղ = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("expborder"));
		մ = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("expbc"));
		_1CD9 = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("expfc"));
		_1CDA = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("exphborder"));
		_1CDB = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("exphbc"));
		_1CDC = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("exphfc"));
		_1CDD = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("headerexp"));
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			switch (childNode.Name)
			{
			case "backstyle":
				ሌ(ᇭ, childNode);
				break;
			case "headerhotstyle":
				ሌ(_1CD5, childNode);
				break;
			case "headerstyle":
				ሌ(ᄈ, childNode);
				break;
			case "image":
				_0E66 = BarFunctions.DeserializeImage(childNode);
				break;
			}
		}
		if (itemXmlElement.HasAttribute("imageindex"))
		{
			ImageIndex = XmlConvert.ToInt32(itemXmlElement.GetAttribute("imageindex"));
		}
		RefreshImageSize();
		Expanded = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("expanded"));
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		NeedRecalcSize = true;
		if (item is ButtonItem)
		{
			((ButtonItem)item)._ງ = _1CDE;
		}
		base.OnItemAdded(item);
		if (!Expanded)
		{
			if (!item.Displayed)
			{
				item.Displayed = true;
			}
			item.Displayed = false;
		}
		if (item is ButtonItem)
		{
			ButtonItem buttonItem = item as ButtonItem;
			buttonItem.ThemeAware = false;
			if (buttonItem.ForeColor.IsEmpty && buttonItem.HotForeColor.IsEmpty)
			{
				BarFunctions.SetExplorerBarStyle(item as ButtonItem, ᇮ);
			}
		}
		item.NeedRecalcSize = true;
		if (DesignMode)
		{
			Refresh();
		}
	}

	protected internal override void OnBeforeItemRemoved(BaseItem item)
	{
		if (item is ButtonItem)
		{
			((ButtonItem)item)._ງ = false;
		}
		base.OnBeforeItemRemoved(item);
	}

	public override void ShowToolTip()
	{
		if (DesignMode || !Visible || !Displayed)
		{
			return;
		}
		IOwner owner = GetOwner() as IOwner;
		if (owner != null && !owner.ShowToolTips)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return;
		}
		Rectangle rectangle = _1944;
		rectangle.Offset(m_Rect.X, m_Rect.Y);
		if (!rectangle.Contains(val.PointToClient(Control.MousePosition)))
		{
			return;
		}
		OnTooltip(bShow: true);
		if (m_Tooltip != "")
		{
			if (m_ToolTipWnd == null)
			{
				m_ToolTipWnd = new ToolTip();
			}
			m_ToolTipWnd.Text = m_Tooltip;
			if (owner != null && owner.ShowShortcutKeysInToolTips && Shortcuts.Count > 0)
			{
				ToolTip toolTipWnd = m_ToolTipWnd;
				toolTipWnd.Text = toolTipWnd.Text + " (" + base.ShortcutString + ")";
			}
			GetIOwnerItemEvents()?.InvokeToolTipShowing(this, new EventArgs());
			m_ToolTipWnd.ShowToolTip();
		}
	}

	public override void Refresh()
	{
		if (SuspendLayout)
		{
			return;
		}
		if (!Expanded)
		{
			base.Refresh();
		}
		else
		{
			if ((!Visible && !base.IsOnCustomizeMenu) || !Displayed)
			{
				return;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val == null || !BaseItem.IsHandleValid(val) || val is ᆓ)
			{
				return;
			}
			if (m_NeedRecalcSize)
			{
				RecalcSize();
				if (m_Parent != null)
				{
					m_Parent.SubItemSizeChanged(this);
				}
			}
			Rectangle rectangle = _1944;
			rectangle.Inflate(4, 4);
			val.Invalidate(m_Rect, false);
		}
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	private InsertPosition ᆉ(Point ॺ, BaseItem ᆊ)
	{
		return GetContainerInsertPosition(ॺ, ᆊ);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point ॺ, BaseItem ᆊ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆉ
		return this.ᆉ(ॺ, ᆊ);
	}

	private void ᆋ(int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆋ
		this.ᆋ(ᆌ, ᆍ);
	}

	private void ᆎ(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.InsertItemAt(this, ਤ, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆎ
		this.ᆎ(ਤ, ᆌ, ᆍ);
	}

	private int _෭(BaseItem _෮)
	{
		int result = -1;
		int num = _෮.SubItems.Count - 1;
		while (num >= 0 && _෮.SubItems[num].SystemItem)
		{
			result = num;
			num--;
		}
		return result;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void SetDesignTimeDefaults(ButtonItem button, eExplorerBarStockStyle e)
	{
		BarFunctions.SetExplorerBarStyle(button, e);
		button.Text = "New Button";
		button.ButtonStyle = eButtonStyle.ImageAndText;
		button.ImagePosition = eImagePosition.Left;
		button.HotTrackingStyle = eHotTrackingStyle.None;
		button.HotFontUnderline = true;
		button.Cursor = Cursors.Hand;
	}

	public void SetDefaultAppearance()
	{
		StockStyle = eExplorerBarStockStyle.SystemColors;
		BackStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
		BackStyle.BorderLeft = eStyleBorderType.Solid;
		BackStyle.BorderRight = eStyleBorderType.Solid;
		BackStyle.BorderTop = eStyleBorderType.Solid;
		BackStyle.BorderBottom = eStyleBorderType.Solid;
		BackStyle.BorderLeftColor = SystemColors.Window;
		BackStyle.BorderRightColor = SystemColors.Window;
		BackStyle.BorderTopColor = SystemColors.Window;
		BackStyle.BorderBottomColor = SystemColors.Window;
		BackStyle.BorderLeftWidth = 1;
		BackStyle.BorderRightWidth = 1;
		BackStyle.BorderTopWidth = 1;
		BackStyle.BorderBottomWidth = 1;
		ExpandBackColor = SystemColors.Window;
		ExpandBorderColor = SystemColors.InactiveCaption;
		ExpandForeColor = SystemColors.Highlight;
		ExpandHotBackColor = SystemColors.Window;
		ExpandHotForeColor = SystemColors.ActiveCaption;
		ExpandHotBorderColor = SystemColors.ActiveCaption;
		TitleHotStyle.BackColor = SystemColors.Window;
		TitleHotStyle.BackColor2 = SystemColors.InactiveCaption;
		TitleHotStyle.TextColor = SystemColors.ActiveCaption;
		TitleStyle.BackColor = SystemColors.Window;
		TitleStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
		TitleStyle.TextColor = SystemColors.ControlText;
		ThemeAware = true;
	}
}
