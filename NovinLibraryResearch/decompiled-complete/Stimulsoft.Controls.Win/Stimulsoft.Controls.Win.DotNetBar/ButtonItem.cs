using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;
using Stimulsoft.Controls.Win.Events;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("Click")]
public class ButtonItem : PopupItem, IPersonalizedMenuItem
{
	public class ButtonItemAccessibleObject : ItemAccessibleObject
	{
		private ButtonItemPartAccessibleObject m_PushButtonPart;

		private ButtonItemPartAccessibleObject m_ExpandPart;

		internal ButtonItem ButtonItem => base.Owner as ButtonItem;

		private ButtonItemPartAccessibleObject AccessiblePushButtonPart
		{
			get
			{
				if (m_PushButtonPart == null)
				{
					m_PushButtonPart = new ButtonItemPartAccessibleObject(ButtonItem, pushButtonPart: true);
				}
				return m_PushButtonPart;
			}
		}

		private ButtonItemPartAccessibleObject AccessibleExpandPart
		{
			get
			{
				if (m_ExpandPart == null)
				{
					m_ExpandPart = new ButtonItemPartAccessibleObject(ButtonItem, pushButtonPart: false);
				}
				return m_ExpandPart;
			}
		}

		public override AccessibleRole Role
		{
			get
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				if (IsSplitButton)
				{
					return (AccessibleRole)20;
				}
				return base.Role;
			}
		}

		private bool IsSplitButton
		{
			get
			{
				ButtonItem buttonItem = ButtonItem;
				if (buttonItem.VisibleSubItems > 0 && !buttonItem.AutoExpandOnClick && !buttonItem.IsOnMenu && !buttonItem.IsOnMenuBar)
				{
					return true;
				}
				return false;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				if (IsSplitButton)
				{
					if (base.Owner != null && base.Owner.IsAccessible)
					{
						AccessibleStates val = (AccessibleStates)0;
						if (!base.Owner.Displayed || !base.Owner.Visible)
						{
							val = (AccessibleStates)(val | 0x8000);
						}
						else if (!base.Owner.GetEnabled())
						{
							return (AccessibleStates)1;
						}
						return (AccessibleStates)256;
					}
					return (AccessibleStates)1;
				}
				AccessibleStates val2 = base.State;
				if (ButtonItem.GetEnabled() && (ButtonItem.Checked || ButtonItem.IsMouseDown))
				{
					val2 = (AccessibleStates)(val2 | 8);
				}
				return val2;
			}
		}

		public override string DefaultAction
		{
			get
			{
				if (IsSplitButton)
				{
					return "";
				}
				return base.DefaultAction;
			}
		}

		public override string KeyboardShortcut => "";

		public ButtonItemAccessibleObject(BaseItem owner)
			: base(owner)
		{
		}

		public override int GetChildCount()
		{
			if (IsSplitButton)
			{
				return 2;
			}
			return base.GetChildCount();
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (IsSplitButton)
			{
				switch (iIndex)
				{
				case 0:
					return (AccessibleObject)(object)AccessiblePushButtonPart;
				case 1:
					return (AccessibleObject)(object)AccessibleExpandPart;
				}
			}
			return base.GetChild(iIndex);
		}

		public override AccessibleObject Navigate(AccessibleNavigation navdir)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (IsSplitButton)
			{
				return null;
			}
			return base.Navigate(navdir);
		}

		public override void DoDefaultAction()
		{
			if (!IsSplitButton)
			{
				base.DoDefaultAction();
			}
		}

		public override AccessibleObject GetSelected()
		{
			if (IsSplitButton)
			{
				if (ButtonItem.IsMouseOverExpand)
				{
					return (AccessibleObject)(object)AccessibleExpandPart;
				}
				if (ButtonItem.IsMouseOver)
				{
					return (AccessibleObject)(object)AccessiblePushButtonPart;
				}
				return null;
			}
			return base.GetSelected();
		}

		public override AccessibleObject HitTest(int x, int y)
		{
			if (IsSplitButton)
			{
				if (((AccessibleObject)AccessiblePushButtonPart).Bounds.Contains(x, y))
				{
					return (AccessibleObject)(object)AccessiblePushButtonPart;
				}
				if (((AccessibleObject)AccessibleExpandPart).Bounds.Contains(x, y))
				{
					return (AccessibleObject)(object)AccessibleExpandPart;
				}
				return null;
			}
			return base.HitTest(x, y);
		}
	}

	public class ButtonItemPartAccessibleObject : AccessibleObject
	{
		private ButtonItem m_Button;

		private bool m_PushButtonPart = true;

		public override string Name
		{
			get
			{
				if (!m_PushButtonPart)
				{
					return "Open";
				}
				if (m_Button == null)
				{
					return "";
				}
				if (m_Button.AccessibleName != "")
				{
					return m_Button.AccessibleName;
				}
				if (m_Button.Text != null)
				{
					return m_Button.Text.Replace("&", "");
				}
				return m_Button.Tooltip;
			}
			set
			{
				m_Button.AccessibleName = value;
			}
		}

		public override string Description
		{
			get
			{
				if (m_Button == null)
				{
					return "";
				}
				if (m_Button.AccessibleDescription != "")
				{
					return m_Button.AccessibleDescription;
				}
				return ((AccessibleObject)this).Name + " button";
			}
		}

		public override AccessibleRole Role
		{
			get
			{
				if (!m_PushButtonPart)
				{
					return (AccessibleRole)56;
				}
				return (AccessibleRole)62;
			}
		}

		public override AccessibleStates State
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				if (m_Button == null)
				{
					return (AccessibleStates)1;
				}
				AccessibleStates val = (AccessibleStates)0;
				if (!m_Button.IsAccessible)
				{
					return (AccessibleStates)1;
				}
				if (!m_Button.Displayed || !m_Button.Visible)
				{
					return (AccessibleStates)(val | 0x8000);
				}
				if (!m_Button.GetEnabled())
				{
					return (AccessibleStates)1;
				}
				if (m_PushButtonPart)
				{
					if (m_Button.IsMouseOver && !m_Button.IsMouseOverExpand)
					{
						return (AccessibleStates)(val | 0x84);
					}
					return (AccessibleStates)(val | 0x100000);
				}
				if (m_Button.Expanded || m_Button.IsMouseOverExpand)
				{
					val = (AccessibleStates)(val | 0x84);
				}
				if (m_Button.Expanded)
				{
					return (AccessibleStates)(val | 0x200);
				}
				return (AccessibleStates)(val | 0x400);
			}
		}

		public override AccessibleObject Parent => m_Button.AccessibleObject;

		public override Rectangle Bounds
		{
			get
			{
				object containerControl = m_Button.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				Rectangle empty = Rectangle.Empty;
				if (m_PushButtonPart)
				{
					Rectangle subItemsRect = m_Button.SubItemsRect;
					subItemsRect.Offset(m_Button.Bounds.Location);
					empty = m_Button.Bounds;
					if (subItemsRect.X == empty.X && subItemsRect.Y == empty.Y)
					{
						empty.X += subItemsRect.Width;
						empty.Width -= subItemsRect.Width;
					}
					else if (subItemsRect.X == empty.X && subItemsRect.Bottom == empty.Bottom && subItemsRect.Y > empty.Y)
					{
						empty.Y += subItemsRect.Height;
						empty.Height -= subItemsRect.Height;
					}
					else
					{
						empty.Width -= subItemsRect.Width;
					}
				}
				else
				{
					empty = m_Button.SubItemsRect;
					empty.Offset(m_Button.Bounds.Location);
				}
				if (val != null)
				{
					empty.Location = val.PointToScreen(empty.Location);
				}
				return empty;
			}
		}

		public override string DefaultAction
		{
			get
			{
				if (m_Button.AccessibleDefaultActionDescription != "")
				{
					return m_Button.AccessibleDefaultActionDescription;
				}
				if (m_PushButtonPart)
				{
					return "Press";
				}
				return "Open";
			}
		}

		public override string KeyboardShortcut => m_Button.ShortcutString;

		public ButtonItemPartAccessibleObject(ButtonItem owner, bool pushButtonPart)
		{
			m_Button = owner;
			m_PushButtonPart = pushButtonPart;
		}

		public override int GetChildCount()
		{
			if (m_PushButtonPart)
			{
				return 0;
			}
			return m_Button.SubItems.Count;
		}

		public override AccessibleObject GetChild(int iIndex)
		{
			if (m_Button == null || m_PushButtonPart)
			{
				return null;
			}
			return m_Button.SubItems[iIndex].AccessibleObject;
		}

		public override void DoDefaultAction()
		{
			if (m_Button != null)
			{
				if (m_PushButtonPart)
				{
					m_Button._AccessibleExpandAction = false;
				}
				else
				{
					m_Button._AccessibleExpandAction = true;
				}
				object containerControl = m_Button.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is MenuPanel && !(val is IAccessibilitySupport))
				{
					object containerControl2 = ((MenuPanel)(object)val).ParentItem.ContainerControl;
					val = (Control)((containerControl2 is Control) ? containerControl2 : null);
				}
				if (val is IAccessibilitySupport accessibilitySupport)
				{
					accessibilitySupport.DoDefaultActionItem = m_Button;
					NativeFunctions.PostMessage(val.Handle, 1131, IntPtr.Zero, IntPtr.Zero);
				}
				((AccessibleObject)this).DoDefaultAction();
			}
		}

		public override AccessibleObject GetSelected()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Invalid comparison between Unknown and I4
			if (m_Button == null || m_PushButtonPart)
			{
				return ((AccessibleObject)this).GetSelected();
			}
			foreach (BaseItem subItem in m_Button.SubItems)
			{
				if ((subItem.AccessibleObject.State & 0x80) == 128)
				{
					return subItem.AccessibleObject;
				}
			}
			return ((AccessibleObject)this).GetSelected();
		}

		public override AccessibleObject HitTest(int x, int y)
		{
			if (m_Button == null)
			{
				return ((AccessibleObject)this).HitTest(x, y);
			}
			Point point = new Point(x, y);
			foreach (BaseItem subItem in m_Button.SubItems)
			{
				object containerControl = subItem.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					Point pt = val.PointToClient(point);
					if (subItem.DisplayRectangle.Contains(pt))
					{
						return subItem.AccessibleObject;
					}
				}
			}
			return ((AccessibleObject)this).HitTest(x, y);
		}

		public override AccessibleObject Navigate(AccessibleNavigation navdir)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Invalid comparison between Unknown and I4
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Invalid comparison between Unknown and I4
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Invalid comparison between Unknown and I4
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Invalid comparison between Unknown and I4
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Invalid comparison between Unknown and I4
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Invalid comparison between Unknown and I4
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Invalid comparison between Unknown and I4
			if (m_Button == null || m_PushButtonPart)
			{
				return ((AccessibleObject)this).Navigate(navdir);
			}
			BaseItem baseItem = null;
			if ((int)navdir == 2 || (int)navdir == 4 || (int)navdir == 5)
			{
				if (m_Button.Parent != null)
				{
					BaseItem parent = m_Button.Parent;
					baseItem = GetFirstVisible(parent.SubItems, parent.SubItems.IndexOf(m_Button) + 1);
				}
			}
			else if ((int)navdir == 7)
			{
				baseItem = GetFirstVisible(m_Button.SubItems, 0);
			}
			else if ((int)navdir == 8)
			{
				baseItem = GetFirstVisibleReverse(m_Button.SubItems, m_Button.SubItems.Count - 1);
			}
			else if ((int)navdir == 1 || (int)navdir == 3 || (int)navdir == 6)
			{
				BaseItem parent2 = m_Button.Parent;
				baseItem = GetFirstVisibleReverse(m_Button.SubItems, parent2.SubItems.IndexOf(m_Button) - 1);
			}
			if (baseItem != null)
			{
				return baseItem.AccessibleObject;
			}
			return ((AccessibleObject)this).Navigate(navdir);
		}

		private BaseItem GetFirstVisible(SubItemsCollection col, int startIndex)
		{
			int count = col.Count;
			if (count == 0)
			{
				return null;
			}
			if (startIndex >= col.Count)
			{
				startIndex = col.Count - 1;
			}
			for (int i = startIndex; i < count; i++)
			{
				if (col[i].Visible)
				{
					return col[i];
				}
			}
			return null;
		}

		private BaseItem GetFirstVisibleReverse(SubItemsCollection col, int startIndex)
		{
			if (col.Count == 0)
			{
				return null;
			}
			if (startIndex >= col.Count)
			{
				startIndex = col.Count - 1;
			}
			for (int num = startIndex; num >= 0; num--)
			{
				if (col[num].Visible)
				{
					return col[num];
				}
			}
			return null;
		}
	}

	private const int STATEBUTTON_SPACING = 3;

	private Image m_Image;

	private Image m_ImageSmall;

	private int m_ImageIndex;

	private Image m_HoverImage;

	private int m_HoverImageIndex;

	private Image m_DisabledImage;

	private bool m_DisabledImageCustom;

	private int m_DisabledImageIndex;

	private Icon m_DisabledIcon;

	private Image m_PressedImage;

	private int m_PressedImageIndex;

	private eButtonStyle m_ButtonStyle;

	private eImagePosition m_ImagePosition;

	private Font m_Font;

	private string m_AlternateShortcutText;

	private bool m_MouseOver;

	private bool m_MouseOverExpand;

	private bool m_MouseDown;

	private bool m_Checked;

	private Rectangle m_ImageDrawRect;

	private Rectangle m_TextDrawRect;

	private Rectangle m_SubItemsRect;

	private int m_VerticalPadding;

	private int m_HorizontalPadding;

	private Color m_ForeColor = Color.Empty;

	private Color m_HotForeColor = Color.Empty;

	private bool m_HotFontUnderline;

	private bool m_HotFontBold;

	private bool m_FontBold;

	private bool m_FontItalic;

	private bool m_FontUnderline;

	private eMenuVisibility m_MenuVisibility;

	private bool m_RecentlyUsed;

	private eHotTrackingStyle m_HotTrackingStyle;

	private Image m_ImageCachedIdx;

	private ItemPaintArgs _ItemPaintArgs;

	private int m_SubItemsExpandWidth = 12;

	private string m_OptionGroup = "";

	private Size m_ImageSizeOverride = Size.Empty;

	internal bool _FitContainer;

	private Icon m_Icon;

	private bool m_AutoExpandOnClick;

	private eButtonColor m_ColorTable = eButtonColor.Orange;

	private string m_CashedColorName = "Orange";

	private int m_ImagePaddingHorizontal = (int)Math.Ceiling(8.0 * StiScale2.X);

	private int m_ImagePaddingVertical = (int)Math.Ceiling(6.0 * StiScale2.Y);

	private string m_CustomColorName = "";

	private Size m_FixedSize = Size.Empty;

	private bool m_SplitButton;

	private bool m_IgnoreAlpha;

	private int m_MouseOverFadeAlphaIncrement = 65;

	private int m_PulseFadeAlphaIncrement = 12;

	private bool m_Pulse;

	private int m_PulseBeats;

	private int m_PulseCount;

	private bool m_StopPulseOnMouseOver = true;

	private bool m_RibbonWordWrap = true;

	private bool m_AutoCheckOnClick;

	private bool m_UseSmallImage;

	internal bool _FixedSizeCenterText;

	private bool showMarker;

	private bool is20Button;

	private ButtonItemAccessibleObject _ButtonItemAccessibleObject;

	private string _Symbol = "";

	private float _SymbolSize;

	private Image _ImageAlt;

	private eButtonImageListSelection _ImageListSizeSelection = eButtonImageListSelection.NotSet;

	private Bitmap m_ImageState2;

	private int m_Direction = 1;

	private int m_Alpha;

	private bool m_FadeAnimation;

	private ReaderWriterLock m_FadeImageLock = new ReaderWriterLock();

	internal bool _FadeEnabled = true;

	private bool _AutoExpandMenuItem = true;

	private Font _MouseOverFont;

	private eColorSchemePart _ForeColorColorSchemePart = eColorSchemePart.None;

	private ShapeDescriptor _Shape;

	private bool _EnableMarkup = true;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowMarker
	{
		get
		{
			return showMarker;
		}
		set
		{
			if (showMarker != value)
			{
				showMarker = value;
				Invalidate();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool Is20Button
	{
		get
		{
			return is20Button;
		}
		set
		{
			is20Button = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsCenteredButton { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int DropArrowShift { get; set; }

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[DefaultValue(eImagePosition.Left)]
	public virtual eImagePosition ImagePosition
	{
		get
		{
			return m_ImagePosition;
		}
		set
		{
			if (m_ImagePosition != value)
			{
				m_ImagePosition = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "ImagePosition");
				}
				NeedRecalcSize = true;
				if (Parent != null)
				{
					Parent.NeedRecalcSize = true;
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates custom color table name for the button when Office 2007 style is used.")]
	public virtual string CustomColorName
	{
		get
		{
			return m_CustomColorName;
		}
		set
		{
			m_CustomColorName = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(eButtonColor.Orange)]
	[Category("Appearance")]
	[Description("Indicates predefined color of button when Office 2007 style is used.")]
	public virtual eButtonColor ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			if (m_ColorTable != value)
			{
				m_ColorTable = value;
				m_CashedColorName = Enum.GetName(typeof(eButtonColor), m_ColorTable);
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Determines the style of the button.")]
	[DefaultValue(eButtonStyle.Default)]
	public virtual eButtonStyle ButtonStyle
	{
		get
		{
			return m_ButtonStyle;
		}
		set
		{
			if (m_ButtonStyle != value)
			{
				m_ButtonStyle = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "ButtonStyle");
				}
				NeedRecalcSize = true;
				if (Displayed && m_Parent != null)
				{
					RecalcSize();
					m_Parent.SubItemSizeChanged(this);
				}
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the item will auto-expand (display pop-up menu or toolbar) when clicked.")]
	public virtual bool AutoExpandOnClick
	{
		get
		{
			return m_AutoExpandOnClick;
		}
		set
		{
			m_AutoExpandOnClick = value;
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SymbolTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return _Symbol;
		}
		set
		{
			if (value != _Symbol)
			{
				string symbol = _Symbol;
				_Symbol = value;
				OnSymbolChanged(symbol, value);
			}
		}
	}

	[DefaultValue(0f)]
	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return _SymbolSize;
		}
		set
		{
			if (value != _SymbolSize)
			{
				float symbolSize = _SymbolSize;
				_SymbolSize = value;
				OnSymbolSizeChanged(symbolSize, value);
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending.")]
	[DefaultValue(null)]
	public virtual Icon Icon
	{
		get
		{
			return m_Icon;
		}
		set
		{
			NeedRecalcSize = true;
			m_Icon = value;
			OnImageChanged();
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the Button image which is used when button background is black or very dark. Image is used when button text is white. This is provided for Metro style applications.")]
	[DefaultValue(null)]
	public Image ImageAlt
	{
		get
		{
			return _ImageAlt;
		}
		set
		{
			_ImageAlt = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed on the face of the item.")]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return m_Image;
		}
		set
		{
			NeedRecalcSize = true;
			m_Image = value;
			OnImageChanged();
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the small Button image used by Ribbon control when small image variant is needed")]
	[DefaultValue(null)]
	public virtual Image ImageSmall
	{
		get
		{
			return m_ImageSmall;
		}
		set
		{
			m_ImageSmall = value;
			if (!m_ImageSizeOverride.IsEmpty || UseSmallImageResolved)
			{
				NeedRecalcSize = true;
				OnAppearanceChanged();
				OnImageChanged();
				Refresh();
			}
		}
	}

	private bool UseSmallImageResolved => m_UseSmallImage;

	[DefaultValue(eButtonImageListSelection.NotSet)]
	[Category("Image")]
	[Description("Indicates image size that is used by the button when multiple ImageList controls are used as source for button image.")]
	public virtual eButtonImageListSelection ImageListSizeSelection
	{
		get
		{
			return _ImageListSizeSelection;
		}
		set
		{
			_ImageListSizeSelection = value;
			NeedRecalcSize = true;
			OnImageChanged();
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool UseSmallImage
	{
		get
		{
			return m_UseSmallImage;
		}
		set
		{
			if (m_UseSmallImage != value)
			{
				m_UseSmallImage = value;
				if (m_ImageSmall != null)
				{
					NeedRecalcSize = true;
					OnImageChanged();
					OnAppearanceChanged();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed on the face of the item.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public int ImageIndex
	{
		get
		{
			return m_ImageIndex;
		}
		set
		{
			if (m_ImageCachedIdx != null)
			{
				m_ImageCachedIdx.Dispose();
			}
			m_ImageCachedIdx = null;
			if (m_ImageIndex != value)
			{
				m_ImageIndex = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "ImageIndex");
				}
				if (m_Parent != null)
				{
					OnImageChanged();
					NeedRecalcSize = true;
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when mouse hovers over the item.")]
	[DefaultValue(null)]
	public virtual Image HoverImage
	{
		get
		{
			return m_HoverImage;
		}
		set
		{
			m_HoverImage = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed when mouse hovers over the item.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public virtual int HoverImageIndex
	{
		get
		{
			return m_HoverImageIndex;
		}
		set
		{
			if (m_HoverImageIndex != value)
			{
				m_HoverImageIndex = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HoverImageIndex");
				}
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when item is pressed.")]
	[DefaultValue(null)]
	public virtual Image PressedImage
	{
		get
		{
			return m_PressedImage;
		}
		set
		{
			m_PressedImage = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed when item is pressed.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public virtual int PressedImageIndex
	{
		get
		{
			return m_PressedImageIndex;
		}
		set
		{
			if (m_PressedImageIndex != value)
			{
				m_PressedImageIndex = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "PressedImageIndex");
				}
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when item is disabled.")]
	[DefaultValue(null)]
	public Image DisabledImage
	{
		get
		{
			if (!m_DisabledImageCustom)
			{
				return null;
			}
			return m_DisabledImage;
		}
		set
		{
			m_DisabledImage = value;
			if (value == null)
			{
				m_DisabledImageCustom = false;
			}
			else
			{
				m_DisabledImageCustom = true;
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed when item is disabled.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public int DisabledImageIndex
	{
		get
		{
			return m_DisabledImageIndex;
		}
		set
		{
			if (m_DisabledImageIndex != value)
			{
				if (m_DisabledImage != null)
				{
					m_DisabledImage.Dispose();
				}
				m_DisabledImage = null;
				m_DisabledImageIndex = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "DisabledImageIndex");
				}
				Refresh();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowExpandArrow { get; set; } = true;


	internal bool IgnoreAlpha
	{
		get
		{
			return m_IgnoreAlpha;
		}
		set
		{
			m_IgnoreAlpha = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	public Size ImageFixedSize
	{
		get
		{
			return m_ImageSizeOverride;
		}
		set
		{
			if (!LockImageState)
			{
				m_ImageSizeOverride = value;
				OnImageChanged();
				if (m_Parent != null && m_Parent is ImageItem)
				{
					((ImageItem)m_Parent).RefreshImageSize();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[Description("Indicates fixed size of the button.")]
	[Category("Layout")]
	public virtual Size FixedSize
	{
		get
		{
			return m_FixedSize;
		}
		set
		{
			if (value.Width < 0)
			{
				value.Width = 0;
			}
			if (value.Height < 0)
			{
				value.Height = 0;
			}
			m_FixedSize = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	private Size IconSize
	{
		get
		{
			double factor = StiScale2.Factor;
			Size result = new Size((int)Math.Ceiling(16.0 * factor), (int)Math.Ceiling(16.0 * factor));
			IBarImageSize barImageSize = null;
			if (_ItemPaintArgs != null)
			{
				barImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
			}
			if (barImageSize == null)
			{
				barImageSize = ContainerControl as IBarImageSize;
			}
			try
			{
				eBarImageSize imageListSize = GetImageListSize(barImageSize);
				if (barImageSize != null)
				{
					switch (imageListSize)
					{
					case eBarImageSize.Medium:
						result = new Size((int)Math.Ceiling(24.0 * factor), (int)Math.Ceiling(24.0 * factor));
						return result;
					case eBarImageSize.Large:
						result = new Size((int)Math.Ceiling(32.0 * factor), (int)Math.Ceiling(32.0 * factor));
						return result;
					case eBarImageSize.Default:
						break;
					default:
						goto end_IL_005a;
					}
				}
				if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != 0)
				{
					switch (((SideBarPanelItem)m_Parent).ItemImageSize)
					{
					case eBarImageSize.Medium:
						result = new Size((int)Math.Ceiling(24.0 * factor), (int)Math.Ceiling(24.0 * factor));
						return result;
					case eBarImageSize.Large:
						result = new Size((int)Math.Ceiling(32.0 * factor), (int)Math.Ceiling(32.0 * factor));
						return result;
					}
				}
				end_IL_005a:;
			}
			catch (Exception)
			{
			}
			return result;
		}
	}

	protected virtual bool IsFadeEnabled
	{
		get
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (DesignMode || m_HotTrackingStyle == eHotTrackingStyle.None || !_FadeEnabled || TextDrawing.UseTextRenderer || effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7 || effectiveStyle == eDotNetBarStyle.Metro)
			{
				return false;
			}
			if (WinApi.IsGlassEnabled && (Parent is CaptionItemContainer || (Parent is RibbonTabItemContainer && effectiveStyle == eDotNetBarStyle.Office2010)))
			{
				return false;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				if (val is ItemControl)
				{
					return ((ItemControl)(object)val).IsFadeEnabled;
				}
				if (val is Bar)
				{
					return ((Bar)(object)val).IsFadeEnabled;
				}
				if (val is ButtonX)
				{
					return ((ButtonX)(object)val).IsFadeEnabled;
				}
			}
			return false;
		}
	}

	protected virtual bool IsPulseEnabed => true;

	[Browsable(false)]
	public bool IsPulsing => m_Pulse;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether pulse effect started with Pulse method stops automatically when mouse moves over the button.")]
	public virtual bool StopPulseOnMouseOver
	{
		get
		{
			return m_StopPulseOnMouseOver;
		}
		set
		{
			m_StopPulseOnMouseOver = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(12)]
	[Category("Behavior")]
	[Description("Indicates pulse speed. The value must be greater than 0 and less than 128.")]
	public virtual int PulseSpeed
	{
		get
		{
			return m_PulseFadeAlphaIncrement;
		}
		set
		{
			if (value <= 0 || value >= 128)
			{
				throw new ArgumentOutOfRangeException("PulseSpeed value must be greater than 0 and less than 128");
			}
			m_PulseFadeAlphaIncrement = value;
		}
	}

	internal bool MouseIsOver
	{
		get
		{
			return m_MouseOver;
		}
		set
		{
			m_MouseOver = value;
		}
	}

	internal bool MouseIsOverExpand
	{
		get
		{
			return m_MouseOverExpand;
		}
		set
		{
			m_MouseOverExpand = value;
		}
	}

	internal bool MouseIsDown
	{
		get
		{
			return m_MouseDown;
		}
		set
		{
			m_MouseDown = value;
		}
	}

	internal bool ButtonIsExpanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			m_Expanded = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	[Description("Indicates whether button auto-expands on mouse hover when button is used as menu-item and displayed on menu.")]
	[Category("Behavior")]
	public bool AutoExpandMenuItem
	{
		get
		{
			return _AutoExpandMenuItem;
		}
		set
		{
			_AutoExpandMenuItem = value;
		}
	}

	protected virtual bool CanShowPopup
	{
		get
		{
			if (ShowSubItems || ShouldAutoExpandOnClick)
			{
				if (SubItems.Count <= 0)
				{
					return PopupType == ePopupType.Container;
				}
				return true;
			}
			return false;
		}
	}

	protected virtual bool ShouldAutoExpandOnClick => m_AutoExpandOnClick;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool ShowArrow { get; set; }

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
	public virtual bool AutoCheckOnClick
	{
		get
		{
			return m_AutoCheckOnClick;
		}
		set
		{
			m_AutoCheckOnClick = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[DefaultValue(false)]
	[Bindable(true)]
	public virtual bool Checked
	{
		get
		{
			return m_Checked;
		}
		set
		{
			if (m_Checked == value)
			{
				return;
			}
			if (value && m_OptionGroup.Length > 0 && Parent != null)
			{
				ButtonItem buttonItem = null;
				foreach (BaseItem subItem in Parent.SubItems)
				{
					if (subItem != this)
					{
						buttonItem = subItem as ButtonItem;
						if (buttonItem != null && buttonItem.OptionGroup == m_OptionGroup && buttonItem.Checked)
						{
							break;
						}
					}
				}
				OptionGroupChangingEventArgs optionGroupChangingEventArgs = new OptionGroupChangingEventArgs(buttonItem, this);
				InvokeOptionGroupChanging(optionGroupChangingEventArgs);
				if (optionGroupChangingEventArgs.Cancel)
				{
					return;
				}
			}
			m_Checked = value;
			if (ShouldSyncProperties)
			{
				BarFunctions.SyncProperty(this, "Checked");
			}
			OnCheckedChanged();
			if (Displayed)
			{
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Design")]
	[Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts")]
	[DefaultValue("")]
	[Localizable(true)]
	public virtual string AlternateShortCutText
	{
		get
		{
			return m_AlternateShortcutText;
		}
		set
		{
			m_AlternateShortcutText = value;
			OnAppearanceChanged();
		}
	}

	internal string DrawShortcutText
	{
		get
		{
			if (AlternateShortCutText != "")
			{
				return AlternateShortCutText;
			}
			return base.ShortcutString;
		}
	}

	internal Rectangle ImageDrawRect
	{
		get
		{
			return m_ImageDrawRect;
		}
		set
		{
			m_ImageDrawRect = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle SubItemsRect
	{
		get
		{
			return m_SubItemsRect;
		}
		set
		{
			m_SubItemsRect = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle TextDrawRect
	{
		get
		{
			return m_TextDrawRect;
		}
		set
		{
			m_TextDrawRect = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Gets or set the Group item belongs to. The groups allows a user to choose from mutually exclusive options within the group.")]
	[DefaultValue("")]
	public virtual string OptionGroup
	{
		get
		{
			return m_OptionGroup;
		}
		set
		{
			if (!(m_OptionGroup != value))
			{
				return;
			}
			m_OptionGroup = value;
			if (m_OptionGroup != "" && m_Checked && Parent != null)
			{
				foreach (BaseItem subItem in Parent.SubItems)
				{
					if (subItem != this && subItem is ButtonItem buttonItem && buttonItem.OptionGroup == m_OptionGroup && buttonItem.Checked)
					{
						Checked = false;
					}
				}
			}
			OnAppearanceChanged();
		}
	}

	internal eColorSchemePart ForeColorColorSchemePart
	{
		get
		{
			return _ForeColorColorSchemePart;
		}
		set
		{
			_ForeColorColorSchemePart = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The foreground color used to display text.")]
	public virtual Color ForeColor
	{
		get
		{
			return m_ForeColor;
		}
		set
		{
			if (m_ForeColor != value)
			{
				m_ForeColor = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "ForeColor");
				}
				if (Displayed)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The foreground color used to display text when mouse is over the item.")]
	public virtual Color HotForeColor
	{
		get
		{
			return m_HotForeColor;
		}
		set
		{
			if (m_HotForeColor != value)
			{
				m_HotForeColor = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HotForeColor");
				}
				if (Displayed && m_MouseOver)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies that text font is underlined when mouse is over the item.")]
	[DefaultValue(false)]
	public virtual bool HotFontUnderline
	{
		get
		{
			return m_HotFontUnderline;
		}
		set
		{
			if (m_HotFontUnderline != value)
			{
				DisposeMouseOverFont();
				m_HotFontUnderline = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HotFontUnderline");
				}
				if (Displayed && m_MouseOver)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies that text font is bold when mouse is over the item.")]
	[DefaultValue(false)]
	public virtual bool HotFontBold
	{
		get
		{
			return m_HotFontBold;
		}
		set
		{
			if (m_HotFontBold != value)
			{
				DisposeMouseOverFont();
				m_HotFontBold = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HotFontBold");
				}
				if (Displayed && m_MouseOver)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies whether the font used to draw the item text is bold.")]
	[DefaultValue(false)]
	public virtual bool FontBold
	{
		get
		{
			return m_FontBold;
		}
		set
		{
			if (m_FontBold != value)
			{
				m_FontBold = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "FontBold");
				}
				if (Displayed)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies whether the font used to draw the item text is italic.")]
	[DefaultValue(false)]
	public virtual bool FontItalic
	{
		get
		{
			return m_FontItalic;
		}
		set
		{
			if (m_FontItalic != value)
			{
				m_FontItalic = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "FontItalic");
				}
				if (Displayed)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies whether the font used to draw the item text is underlined.")]
	[DefaultValue(false)]
	public virtual bool FontUnderline
	{
		get
		{
			return m_FontUnderline;
		}
		set
		{
			if (m_FontUnderline != value)
			{
				m_FontUnderline = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "FontUnderline");
				}
				if (Displayed)
				{
					Refresh();
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates the width of the expand part of the button item.")]
	[DefaultValue(12)]
	public virtual int SubItemsExpandWidth
	{
		get
		{
			return m_SubItemsExpandWidth;
		}
		set
		{
			m_SubItemsExpandWidth = value;
			NeedRecalcSize = true;
			if (DesignMode)
			{
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ButtonItemEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Data")]
	[Description("Collection of sub items.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public override SubItemsCollection SubItems => base.SubItems;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether button appears as split button. Use AutoExpandOnClick=true if you want to make complete surface of the button display popup when clicked.")]
	public virtual bool SplitButton
	{
		get
		{
			return m_SplitButton;
		}
		set
		{
			m_SplitButton = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	[DefaultValue("")]
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

	[DefaultValue(0)]
	internal int VerticalPadding
	{
		get
		{
			return m_VerticalPadding;
		}
		set
		{
			m_VerticalPadding = value;
		}
	}

	[DefaultValue(0)]
	public int HorizontalPadding
	{
		get
		{
			return m_HorizontalPadding;
		}
		set
		{
			m_HorizontalPadding = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(8)]
	[Category("Layout")]
	[Description("Indicates amount of padding added horizontally to the button images when not on menus")]
	public virtual int ImagePaddingHorizontal
	{
		get
		{
			return m_ImagePaddingHorizontal;
		}
		set
		{
			if (!LockImageState)
			{
				m_ImagePaddingHorizontal = value;
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockImageState { get; set; }

	[Browsable(true)]
	[DefaultValue(6)]
	[Category("Layout")]
	[Description("Indicates amount of padding added vertically to the button images when not on menus")]
	public virtual int ImagePaddingVertical
	{
		get
		{
			return m_ImagePaddingVertical;
		}
		set
		{
			if (!LockImageState)
			{
				m_ImagePaddingVertical = value;
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(null)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShapeTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShapeStringConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	[MergableProperty(false)]
	public virtual ShapeDescriptor Shape
	{
		get
		{
			return _Shape;
		}
		set
		{
			if (_Shape != value)
			{
				_Shape = value;
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	[DefaultValue(eMenuVisibility.VisibleAlways)]
	public virtual eMenuVisibility MenuVisibility
	{
		get
		{
			return m_MenuVisibility;
		}
		set
		{
			if (m_MenuVisibility != value)
			{
				m_MenuVisibility = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "MenuVisibility");
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool RecentlyUsed
	{
		get
		{
			return m_RecentlyUsed;
		}
		set
		{
			if (m_RecentlyUsed != value)
			{
				m_RecentlyUsed = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "RecentlyUsed");
				}
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseOver => m_MouseOver;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseOverExpand => m_MouseOverExpand;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseDown => m_MouseDown;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	[DefaultValue(eHotTrackingStyle.Default)]
	public virtual eHotTrackingStyle HotTrackingStyle
	{
		get
		{
			return m_HotTrackingStyle;
		}
		set
		{
			if (m_HotTrackingStyle != value)
			{
				m_HotTrackingStyle = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HotTrackingStyle");
				}
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImageList ImageList
	{
		get
		{
			if (GetOwner() is IOwner owner)
			{
				return owner.Images;
			}
			return null;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Localizable(true)]
	[Category("Ribbon")]
	[Description("Indicates whether the button text is automatically wrapped over multiple lines when button is used on RibbonBar control.")]
	public virtual bool RibbonWordWrap
	{
		get
		{
			return m_RibbonWordWrap;
		}
		set
		{
			if (m_RibbonWordWrap != value)
			{
				m_RibbonWordWrap = value;
				OnAppearanceChanged();
			}
		}
	}

	protected override bool IsMarkupSupported => _EnableMarkup;

	internal bool IsCurrentSheduler { get; set; }

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
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

	public event EventHandler CheckedChanged;

	[Description("Occurs before an item in option group is checked and provides opportunity to cancel that.")]
	public event OptionGroupChangingEventHandler OptionGroupChanging;

	[Description("Occurs when text markup link is clicked. Markup links can be created using a tag.")]
	public event MarkupLinkClickEventHandler MarkupLinkClick;

	public void DrawMarker(Graphics g, Rectangle rect)
	{
		if (ShowMarker)
		{
			int num = StiScale2.XXI(7.0);
			int num2 = StiScale2.YYI(7.0);
			Rectangle rectangle = new Rectangle(rect.X + (rect.Width - num) / 2, rect.Bottom - num2 - StiScale2.I2, num, num2);
			g.FillEllipse(Brushes.Gray, rectangle);
			rectangle.Inflate(-1, -1);
			g.FillEllipse(Brushes.Gray, rectangle);
		}
	}

	public static Bitmap GetImage(string imageName, bool makeTransparent)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		Assembly assembly = typeof(ButtonItem).Assembly;
		Stream manifestResourceStream = assembly.GetManifestResourceStream(imageName);
		if (manifestResourceStream != null)
		{
			Bitmap val = new Bitmap(manifestResourceStream);
			if (makeTransparent)
			{
				MakeImageBackgroundAlphaZero(val);
			}
			return val;
		}
		throw new Exception($"Can't find image '{imageName}' in resources of '{assembly}' assembly");
	}

	public static void MakeImageBackgroundAlphaZero(Bitmap image)
	{
		Color pixel = image.GetPixel(0, ((Image)image).Height - 1);
		image.MakeTransparent();
		Color color = Color.FromArgb(0, pixel);
		image.SetPixel(0, ((Image)image).Height - 1, color);
	}

	public ButtonItem()
		: this("", "")
	{
	}

	public ButtonItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public ButtonItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		m_IsContainer = false;
		m_Image = null;
		m_ImageIndex = -1;
		m_HoverImage = null;
		m_HoverImageIndex = -1;
		m_DisabledImage = null;
		m_DisabledImageIndex = -1;
		m_PressedImage = null;
		m_PressedImageIndex = -1;
		m_MouseOver = false;
		m_MouseDown = false;
		m_ButtonStyle = eButtonStyle.Default;
		m_ImagePosition = eImagePosition.Left;
		m_Font = null;
		m_ImageDrawRect = Rectangle.Empty;
		m_TextDrawRect = Rectangle.Empty;
		m_SubItemsRect = Rectangle.Empty;
		m_VerticalPadding = 0;
		m_HorizontalPadding = 0;
		m_AlternateShortcutText = "";
	}

	public override BaseItem Copy()
	{
		ButtonItem buttonItem = new ButtonItem(m_Name);
		CopyToItem(buttonItem);
		return buttonItem;
	}

	internal void InternalCopyToItem(ButtonItem copy)
	{
		CopyToItem(copy);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		ButtonItem buttonItem = copy as ButtonItem;
		base.CopyToItem((BaseItem)buttonItem);
		if (m_Image != null)
		{
			buttonItem.Image = (Image)m_Image.Clone();
		}
		if (m_ImageSmall != null)
		{
			buttonItem.ImageSmall = (Image)m_ImageSmall.Clone();
		}
		if (m_HoverImage != null)
		{
			buttonItem.HoverImage = (Image)m_HoverImage.Clone();
		}
		if (m_PressedImage != null)
		{
			buttonItem.PressedImage = (Image)m_PressedImage.Clone();
		}
		if (m_DisabledImage != null && m_DisabledImageCustom)
		{
			buttonItem.DisabledImage = (Image)m_DisabledImage.Clone();
		}
		if (m_Icon != null)
		{
			object obj = m_Icon.Clone();
			buttonItem.Icon = (Icon)((obj is Icon) ? obj : null);
		}
		buttonItem.ButtonStyle = m_ButtonStyle;
		buttonItem.ImagePosition = m_ImagePosition;
		buttonItem.OptionGroup = m_OptionGroup;
		buttonItem.Checked = m_Checked;
		buttonItem.PopupType = PopupType;
		buttonItem.AlternateShortCutText = AlternateShortCutText;
		if (m_ImageIndex >= 0)
		{
			buttonItem.SetImageIndex(m_ImageIndex);
		}
		if (m_PressedImageIndex >= 0)
		{
			buttonItem.PressedImageIndex = m_PressedImageIndex;
		}
		if (m_HoverImageIndex >= 0)
		{
			buttonItem.HoverImageIndex = m_HoverImageIndex;
		}
		if (m_DisabledImageIndex >= 0)
		{
			buttonItem.DisabledImageIndex = m_DisabledImageIndex;
		}
		if (!m_ForeColor.IsEmpty)
		{
			buttonItem.ForeColor = m_ForeColor;
		}
		buttonItem.HotFontBold = m_HotFontBold;
		buttonItem.HotFontUnderline = m_HotFontUnderline;
		if (!m_HotForeColor.IsEmpty)
		{
			buttonItem.HotForeColor = m_HotForeColor;
		}
		buttonItem.HotTrackingStyle = m_HotTrackingStyle;
		buttonItem.MenuVisibility = m_MenuVisibility;
		buttonItem.AutoExpandOnClick = AutoExpandOnClick;
		buttonItem.AlternateShortCutText = AlternateShortCutText;
		buttonItem.SubItemsExpandWidth = SubItemsExpandWidth;
		buttonItem.CheckedChanged = this.CheckedChanged;
		buttonItem.OptionGroupChanging = this.OptionGroupChanging;
		if (!ImageFixedSize.IsEmpty)
		{
			buttonItem.ImageFixedSize = ImageFixedSize;
		}
		buttonItem.ImagePaddingHorizontal = m_ImagePaddingHorizontal;
		buttonItem.ImagePaddingVertical = m_ImagePaddingVertical;
		buttonItem.FixedSize = FixedSize;
		buttonItem.RibbonWordWrap = RibbonWordWrap;
		buttonItem.AutoCheckOnClick = AutoCheckOnClick;
		if (Shape != null)
		{
			buttonItem.Shape = Shape;
		}
		buttonItem.ColorTable = ColorTable;
		buttonItem.CustomColorName = CustomColorName;
		buttonItem.AutoExpandMenuItem = AutoExpandMenuItem;
		buttonItem.EnableMarkup = EnableMarkup;
		buttonItem.SplitButton = SplitButton;
	}

	protected override void Dispose(bool disposing)
	{
		StopFade();
		StopPulse();
		DisposeMouseOverFont();
		DisposedCachedImageListImage();
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.DisposeImage(ref m_Image);
			BarUtilities.DisposeImage(ref m_Icon);
			BarUtilities.DisposeImage(ref m_ImageSmall);
			BarUtilities.DisposeImage(ref m_HoverImage);
			BarUtilities.DisposeImage(ref m_DisabledImage);
			BarUtilities.DisposeImage(ref m_PressedImage);
			BarUtilities.DisposeImage(ref m_DisabledIcon);
		}
		base.Dispose(disposing);
	}

	private void DisposedCachedImageListImage()
	{
		if (m_ImageCachedIdx != null)
		{
			m_ImageCachedIdx.Dispose();
		}
		m_ImageCachedIdx = null;
	}

	protected override AccessibleObject CreateAccessibilityInstance()
	{
		if (_ButtonItemAccessibleObject == null)
		{
			_ButtonItemAccessibleObject = new ButtonItemAccessibleObject(this);
		}
		return (AccessibleObject)(object)_ButtonItemAccessibleObject;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("ImagePosition", XmlConvert.ToString((int)m_ImagePosition));
		itemXmlElement.SetAttribute("ButtonStyle", XmlConvert.ToString((int)m_ButtonStyle));
		itemXmlElement.SetAttribute("Checked", XmlConvert.ToString(m_Checked));
		itemXmlElement.SetAttribute("VerticalPadding", XmlConvert.ToString(m_VerticalPadding));
		itemXmlElement.SetAttribute("HorizontalPadding", XmlConvert.ToString(m_HorizontalPadding));
		itemXmlElement.SetAttribute("MenuVisibility", XmlConvert.ToString((int)m_MenuVisibility));
		itemXmlElement.SetAttribute("RecentlyUsed", XmlConvert.ToString(m_RecentlyUsed));
		if (m_AlternateShortcutText != "")
		{
			itemXmlElement.SetAttribute("AlternateShortcutText", m_AlternateShortcutText);
		}
		if (!m_ForeColor.IsEmpty)
		{
			itemXmlElement.SetAttribute("forecolor", BarFunctions.ColorToString(m_ForeColor));
		}
		if (m_HotTrackingStyle != 0)
		{
			itemXmlElement.SetAttribute("hottrack", XmlConvert.ToString((int)m_HotTrackingStyle));
		}
		if (m_HotFontBold)
		{
			itemXmlElement.SetAttribute("hotfb", XmlConvert.ToString(m_HotFontBold));
		}
		if (m_HotFontUnderline)
		{
			itemXmlElement.SetAttribute("hotfu", XmlConvert.ToString(m_HotFontUnderline));
		}
		if (!m_HotForeColor.IsEmpty)
		{
			itemXmlElement.SetAttribute("hotclr", BarFunctions.ColorToString(m_HotForeColor));
		}
		if (m_OptionGroup != "")
		{
			itemXmlElement.SetAttribute("optiongroup", m_OptionGroup);
		}
		if (m_FontBold)
		{
			itemXmlElement.SetAttribute("fontbold", XmlConvert.ToString(m_FontBold));
		}
		if (m_FontItalic)
		{
			itemXmlElement.SetAttribute("fontitalic", XmlConvert.ToString(m_FontItalic));
		}
		if (m_FontUnderline)
		{
			itemXmlElement.SetAttribute("fontunderline", XmlConvert.ToString(m_FontUnderline));
		}
		if (m_AutoExpandOnClick)
		{
			itemXmlElement.SetAttribute("autoexpandclick", XmlConvert.ToString(m_AutoExpandOnClick));
		}
		if (m_CustomColorName != "")
		{
			itemXmlElement.SetAttribute("CustomColorName", m_CustomColorName);
		}
		if (m_ColorTable != eButtonColor.Orange)
		{
			itemXmlElement.SetAttribute("ColorTable", Enum.GetName(m_ColorTable.GetType(), m_ColorTable));
		}
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (m_Image != null || m_ImageIndex >= 0 || m_HoverImage != null || m_HoverImageIndex >= 0 || m_DisabledImage != null || m_DisabledImageIndex >= 0 || m_PressedImage != null || m_PressedImageIndex >= 0 || m_Icon != null || m_ImageSmall != null)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			if (m_ImageIndex >= 0)
			{
				xmlElement.SetAttribute("imageindex", XmlConvert.ToString(m_ImageIndex));
			}
			if (m_HoverImageIndex >= 0)
			{
				xmlElement.SetAttribute("hoverimageindex", XmlConvert.ToString(m_HoverImageIndex));
			}
			if (m_DisabledImageIndex >= 0)
			{
				xmlElement.SetAttribute("disabledimageindex", XmlConvert.ToString(m_DisabledImageIndex));
			}
			if (m_PressedImageIndex >= 0)
			{
				xmlElement.SetAttribute("pressedimageindex", XmlConvert.ToString(m_PressedImageIndex));
			}
			if (m_Image != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "default");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_Image, xmlElement2);
			}
			if (m_ImageSmall != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("imagesmall");
				xmlElement2.SetAttribute("type", "small");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_ImageSmall, xmlElement2);
			}
			if (m_HoverImage != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "hover");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_HoverImage, xmlElement2);
			}
			if (m_DisabledImage != null && m_DisabledImageCustom)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "disabled");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_DisabledImage, xmlElement2);
			}
			if (m_PressedImage != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "pressed");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_PressedImage, xmlElement2);
			}
			if (m_Icon != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "icon");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeIcon(m_Icon, xmlElement2);
			}
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		m_ImagePosition = (eImagePosition)XmlConvert.ToInt32(itemXmlElement.GetAttribute("ImagePosition"));
		m_ButtonStyle = (eButtonStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("ButtonStyle"));
		m_Checked = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("Checked"));
		m_VerticalPadding = XmlConvert.ToInt32(itemXmlElement.GetAttribute("VerticalPadding"));
		m_HorizontalPadding = XmlConvert.ToInt32(itemXmlElement.GetAttribute("HorizontalPadding"));
		m_MenuVisibility = (eMenuVisibility)XmlConvert.ToInt32(itemXmlElement.GetAttribute("MenuVisibility"));
		m_RecentlyUsed = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("RecentlyUsed"));
		if (itemXmlElement.HasAttribute("forecolor"))
		{
			m_ForeColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("forecolor"));
		}
		else
		{
			m_ForeColor = Color.Empty;
		}
		if (itemXmlElement.HasAttribute("hottrack"))
		{
			m_HotTrackingStyle = (eHotTrackingStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("hottrack"));
		}
		else
		{
			m_HotTrackingStyle = eHotTrackingStyle.Default;
		}
		if (itemXmlElement.HasAttribute("hotclr"))
		{
			m_HotForeColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("hotclr"));
		}
		else
		{
			m_HotForeColor = Color.Empty;
		}
		if (itemXmlElement.HasAttribute("hotfb"))
		{
			m_HotFontBold = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("hotfb"));
		}
		else
		{
			m_HotFontBold = false;
		}
		if (itemXmlElement.HasAttribute("hotfu"))
		{
			m_HotFontUnderline = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("hotfu"));
		}
		else
		{
			m_HotFontUnderline = false;
		}
		if (itemXmlElement.HasAttribute("optiongroup"))
		{
			m_OptionGroup = itemXmlElement.GetAttribute("optiongroup");
		}
		else
		{
			m_OptionGroup = "";
		}
		if (itemXmlElement.HasAttribute("fontbold"))
		{
			m_FontBold = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("fontbold"));
		}
		else
		{
			m_FontBold = false;
		}
		if (itemXmlElement.HasAttribute("fontitalic"))
		{
			m_FontItalic = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("fontitalic"));
		}
		else
		{
			m_FontItalic = false;
		}
		if (itemXmlElement.HasAttribute("fontunderline"))
		{
			m_FontUnderline = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("fontunderline"));
		}
		else
		{
			m_FontUnderline = false;
		}
		if (itemXmlElement.HasAttribute("AlternateShortcutText"))
		{
			m_AlternateShortcutText = itemXmlElement.GetAttribute("AlternateShortcutText");
		}
		else
		{
			m_AlternateShortcutText = "";
		}
		if (itemXmlElement.HasAttribute("autoexpandclick"))
		{
			m_AutoExpandOnClick = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("autoexpandclick"));
		}
		else
		{
			m_AutoExpandOnClick = false;
		}
		if (itemXmlElement.HasAttribute("CustomColorName"))
		{
			m_CustomColorName = itemXmlElement.GetAttribute("CustomColorName");
		}
		else
		{
			m_CustomColorName = "";
		}
		if (itemXmlElement.HasAttribute("ColorTable"))
		{
			m_ColorTable = (eButtonColor)Enum.Parse(m_ColorTable.GetType(), itemXmlElement.GetAttribute("ColorTable"));
		}
		else
		{
			m_ColorTable = eButtonColor.Orange;
		}
		m_ImageIndex = -1;
		m_HoverImageIndex = -1;
		m_DisabledImageIndex = -1;
		m_PressedImageIndex = -1;
		m_Icon = null;
		m_Image = null;
		m_ImageSmall = null;
		m_HoverImage = null;
		m_DisabledImage = null;
		m_DisabledImageCustom = false;
		m_PressedImage = null;
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			if (!(childNode.Name == "images"))
			{
				continue;
			}
			if (childNode.HasAttribute("imageindex"))
			{
				m_ImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("imageindex"));
			}
			if (childNode.HasAttribute("hoverimageindex"))
			{
				m_HoverImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("hoverimageindex"));
			}
			if (childNode.HasAttribute("disabledimageindex"))
			{
				m_DisabledImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("disabledimageindex"));
			}
			if (childNode.HasAttribute("pressedimageindex"))
			{
				m_PressedImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("pressedimageindex"));
			}
			foreach (XmlElement childNode2 in childNode.ChildNodes)
			{
				switch (childNode2.GetAttribute("type"))
				{
				case "default":
					m_Image = BarFunctions.DeserializeImage(childNode2);
					m_ImageIndex = -1;
					break;
				case "icon":
					m_Icon = BarFunctions.DeserializeIcon(childNode2);
					m_ImageIndex = -1;
					break;
				case "hover":
					m_HoverImage = BarFunctions.DeserializeImage(childNode2);
					m_HoverImageIndex = -1;
					break;
				case "disabled":
					m_DisabledImage = BarFunctions.DeserializeImage(childNode2);
					m_DisabledImageIndex = -1;
					m_DisabledImageCustom = true;
					break;
				case "pressed":
					m_PressedImage = BarFunctions.DeserializeImage(childNode2);
					m_PressedImageIndex = -1;
					break;
				case "small":
					m_ImageSmall = BarFunctions.DeserializeImage(childNode2);
					break;
				}
			}
			break;
		}
		OnImageChanged();
	}

	internal virtual string GetColorTableName()
	{
		return m_CashedColorName;
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	internal void SetImageIndex(int iImageIndex)
	{
		m_ImageIndex = iImageIndex;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void OnImageChanged()
	{
		base.OnImageChanged();
		StopPulse();
		if (m_DisabledImage != null && !m_DisabledImageCustom)
		{
			m_DisabledImage.Dispose();
			m_DisabledImage = null;
		}
		if (m_DisabledIcon != null)
		{
			m_DisabledIcon.Dispose();
			m_DisabledIcon = null;
		}
		CompositeImage image = GetImage(ImageState.Default, Color.Black);
		if (image != null)
		{
			if (m_Image != null)
			{
				IBarImageSize barImageSize = null;
				if (_ItemPaintArgs != null)
				{
					barImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
				}
				if (barImageSize == null)
				{
					barImageSize = ContainerControl as IBarImageSize;
				}
				eBarImageSize imageListSize = GetImageListSize(barImageSize);
				if (barImageSize != null && imageListSize != 0)
				{
					if (imageListSize == eBarImageSize.Medium)
					{
						ImageSize = new Size(32, 32);
					}
					else
					{
						ImageSize = new Size(48, 48);
					}
				}
				else
				{
					ImageSize = new Size(image.Width, image.Height);
				}
			}
			else
			{
				ImageSize = new Size(image.Width, image.Height);
			}
			image.Dispose();
		}
		else
		{
			ImageSize = ImageItem._InitalImageSize;
		}
		if (m_Parent != null && m_Parent is ImageItem imageItem)
		{
			imageItem.RefreshSubItemImageSize();
		}
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		if (DesignMode || ((m_ImageIndex >= 0 || m_Icon != null) && ImageSize.Width == ImageItem._InitalImageSize.Width && ImageSize.Height == ImageItem._InitalImageSize.Height))
		{
			OnImageChanged();
		}
		base.OnContainerChanged(objOldContainer);
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		bool mouseOver = m_MouseOver;
		m_FadeImageLock.AcquireReaderLock(-1);
		try
		{
			if (m_ImageState2 != null)
			{
				m_MouseOver = false;
			}
			try
			{
				_ItemPaintArgs = p;
				if (EffectiveStyle == eDotNetBarStyle.Office2000 && !base.IsThemed)
				{
					PaintOffice(p);
				}
				else
				{
					RenderButton(p);
					if (m_ImageState2 != null && !m_IgnoreAlpha)
					{
						Graphics graphics = p.Graphics;
						Rectangle displayRectangle = DisplayRectangle;
						ColorMatrix val = new ColorMatrix();
						val[3, 3] = (float)m_Alpha / 255f;
						ImageAttributes val2 = new ImageAttributes();
						try
						{
							val2.SetColorMatrix(val);
							graphics.DrawImage((Image)(object)m_ImageState2, displayRectangle, 0, 0, displayRectangle.Width, displayRectangle.Height, (GraphicsUnit)2, val2);
							return;
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
				}
			}
			finally
			{
				_ItemPaintArgs = null;
				m_MouseOver = mouseOver;
			}
		}
		finally
		{
			m_FadeImageLock.ReleaseReaderLock();
		}
		DrawInsertMarker(p.Graphics);
	}

	protected virtual void RenderButton(ItemPaintArgs p)
	{
		BaseRenderer renderer = p.Renderer;
		if (renderer != null)
		{
			p.ButtonItemRendererEventArgs.Graphics = p.Graphics;
			p.ButtonItemRendererEventArgs.ButtonItem = this;
			p.ButtonItemRendererEventArgs.ItemPaintArgs = p;
			renderer.DrawButtonItem(p.ButtonItemRendererEventArgs);
		}
		else
		{
			ButtonItemPainter buttonItemPainter = PainterFactory.CreateButtonPainter(this);
			if (buttonItemPainter != null)
			{
				buttonItemPainter.PaintButton(this, p);
			}
			else if (EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				PaintOffice(p);
			}
			else
			{
				PaintDotNet(p);
			}
		}
		DrawMarker(p.Graphics, DisplayRectangle);
	}

	private void PaintDotNet(ItemPaintArgs pa)
	{
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_1a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a23: Expected O, but got Unknown
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Expected O, but got Unknown
		//IL_0fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faa: Expected O, but got Unknown
		//IL_0faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb1: Expected O, but got Unknown
		//IL_126e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1275: Expected O, but got Unknown
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Expected O, but got Unknown
		//IL_1b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b16: Expected O, but got Unknown
		//IL_14ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d5: Expected O, but got Unknown
		//IL_14f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f9: Expected O, but got Unknown
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2c: Expected O, but got Unknown
		//IL_0d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d33: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3a: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b27: Expected O, but got Unknown
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7f: Expected O, but got Unknown
		//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Expected O, but got Unknown
		//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Expected O, but got Unknown
		//IL_143e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1445: Expected O, but got Unknown
		//IL_0ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edd: Expected O, but got Unknown
		//IL_2000: Unknown result type (might be due to invalid IL or missing references)
		//IL_2007: Expected O, but got Unknown
		//IL_1b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b51: Expected O, but got Unknown
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Expected O, but got Unknown
		//IL_186d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1874: Expected O, but got Unknown
		//IL_11c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c9: Expected O, but got Unknown
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Expected O, but got Unknown
		//IL_17fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Expected O, but got Unknown
		//IL_17b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bf: Expected O, but got Unknown
		//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18af: Expected O, but got Unknown
		//IL_2670: Unknown result type (might be due to invalid IL or missing references)
		//IL_2677: Expected O, but got Unknown
		//IL_2645: Unknown result type (might be due to invalid IL or missing references)
		//IL_264c: Expected O, but got Unknown
		//IL_20e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ee: Expected O, but got Unknown
		//IL_2088: Unknown result type (might be due to invalid IL or missing references)
		//IL_208f: Expected O, but got Unknown
		bool isOnMenu = pa.IsOnMenu;
		bool isOnMenuBar = pa.IsOnMenuBar;
		bool isThemed = base.IsThemed;
		if (!isOnMenu && !isOnMenuBar && isThemed)
		{
			ThemedButtonItemPainter.PaintButton(this, pa);
			return;
		}
		bool flag = m_MouseOver;
		if (isOnMenu && Expanded && pa.ContainerControl != null && pa.ContainerControl.Parent != null && !pa.ContainerControl.Parent.Bounds.Contains(Control.MousePosition))
		{
			flag = true;
		}
		Graphics graphics = pa.Graphics;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rectangle2 = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
		Color empty = Color.Empty;
		empty = ((flag && !m_HotForeColor.IsEmpty) ? m_HotForeColor : ((!m_ForeColor.IsEmpty) ? m_ForeColor : ((flag && !m_Expanded && m_HotTrackingStyle != eHotTrackingStyle.Image) ? pa.Colors.ItemHotText : (m_Expanded ? pa.Colors.ItemExpandedText : ((!(isThemed && isOnMenuBar) || !(pa.Colors.ItemText == SystemColors.ControlText)) ? pa.Colors.ItemText : SystemColors.MenuText)))));
		Font val = null;
		eTextFormat eTextFormat2 = pa.ButtonStringFormat;
		CompositeImage image = GetImage();
		Size empty2 = Size.Empty;
		val = ((m_Font == null) ? GetFont(pa, isLayout: false) : m_Font);
		if (image != null)
		{
			empty2 = ImageSize;
			rectangle = ((isOnMenu || (m_ImagePosition != eImagePosition.Top && m_ImagePosition != eImagePosition.Bottom)) ? new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, m_ImageDrawRect.Width, m_ImageDrawRect.Height) : new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, rectangle2.Width, m_ImageDrawRect.Height));
			rectangle.Offset(rectangle2.Left, rectangle2.Top);
			rectangle.Offset((rectangle.Width - empty2.Width) / 2, (rectangle.Height - empty2.Height) / 2);
			rectangle.Width = empty2.Width;
			rectangle.Height = empty2.Height;
		}
		if (isOnMenu)
		{
			if (MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
			{
				if (!pa.Colors.MenuUnusedSide2.IsEmpty)
				{
					LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height), pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
					graphics.FillRectangle((Brush)(object)val2, m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height);
					((Brush)val2).Dispose();
				}
				else
				{
					SolidBrush val3 = new SolidBrush(pa.Colors.MenuUnusedSide);
					try
					{
						graphics.FillRectangle((Brush)(object)val3, m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
			}
			else if (!pa.Colors.MenuSide2.IsEmpty)
			{
				LinearGradientBrush val4 = BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height), pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
				graphics.FillRectangle((Brush)(object)val4, m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height);
				((Brush)val4).Dispose();
			}
			else
			{
				SolidBrush val5 = new SolidBrush(pa.Colors.MenuSide);
				try
				{
					graphics.FillRectangle((Brush)(object)val5, m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
		}
		else if (!pa.Colors.ItemBackground.IsEmpty)
		{
			if (pa.Colors.ItemBackground2.IsEmpty)
			{
				SolidBrush val6 = new SolidBrush(pa.Colors.ItemBackground);
				try
				{
					graphics.FillRectangle((Brush)(object)val6, m_Rect);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			else
			{
				LinearGradientBrush val7 = BarFunctions.CreateLinearGradientBrush(DisplayRectangle, pa.Colors.ItemBackground, pa.Colors.ItemBackground2, pa.Colors.ItemBackgroundGradientAngle);
				try
				{
					graphics.FillRectangle((Brush)(object)val7, DisplayRectangle);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
		}
		else if (!GetEnabled(pa.ContainerControl) && !pa.Colors.ItemDisabledBackground.IsEmpty)
		{
			SolidBrush val8 = new SolidBrush(pa.Colors.ItemDisabledBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val8, m_Rect);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		if (GetEnabled(pa.ContainerControl) || DesignMode)
		{
			if (m_Expanded && !isOnMenu)
			{
				if (pa.Colors.ItemExpandedBackground2.IsEmpty)
				{
					Rectangle rectangle3 = new Rectangle(rectangle2.Left, rectangle2.Top, rectangle2.Width, rectangle2.Height);
					if (pa.Colors.ItemExpandedShadow.IsEmpty)
					{
						rectangle3.Width -= 2;
					}
					SolidBrush val9 = new SolidBrush(pa.Colors.ItemExpandedBackground);
					try
					{
						graphics.FillRectangle((Brush)(object)val9, rectangle3);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val10 = BarFunctions.CreateLinearGradientBrush(new Rectangle(rectangle2.Left, rectangle2.Top, rectangle2.Width - 2, rectangle2.Height), pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
					Rectangle rectangle4 = new Rectangle(rectangle2.Left, rectangle2.Top, rectangle2.Width, rectangle2.Height);
					if (pa.Colors.ItemExpandedShadow.IsEmpty)
					{
						rectangle4.Width -= 2;
					}
					graphics.FillRectangle((Brush)(object)val10, rectangle4);
					((Brush)val10).Dispose();
				}
				Point[] array = ((m_Orientation != 0 || PopupSide != 0) ? new Point[5] : new Point[4]);
				array[0].X = rectangle2.Left;
				array[0].Y = rectangle2.Top + rectangle2.Height - 1;
				array[1].X = rectangle2.Left;
				array[1].Y = rectangle2.Top;
				if (m_Orientation == eOrientation.Horizontal)
				{
					array[2].X = rectangle2.Left + rectangle2.Width - 3;
				}
				else
				{
					array[2].X = rectangle2.Left + rectangle2.Width - 1;
				}
				array[2].Y = rectangle2.Top;
				if (m_Orientation == eOrientation.Horizontal)
				{
					array[3].X = rectangle2.Left + rectangle2.Width - 3;
				}
				else
				{
					array[3].X = rectangle2.Left + rectangle2.Width - 1;
				}
				array[3].Y = rectangle2.Top + rectangle2.Height - 1;
				if (m_Orientation == eOrientation.Vertical || PopupSide != 0)
				{
					array[4].X = rectangle2.Left;
					array[4].Y = rectangle2.Top + rectangle2.Height - 1;
				}
				if (!pa.Colors.ItemExpandedBorder.IsEmpty)
				{
					Pen val11 = new Pen(pa.Colors.ItemExpandedBorder, 1f);
					try
					{
						graphics.DrawLines(val11, array);
					}
					finally
					{
						((IDisposable)val11)?.Dispose();
					}
				}
				if (!pa.Colors.ItemExpandedShadow.IsEmpty && m_Orientation == eOrientation.Horizontal)
				{
					SolidBrush val12 = new SolidBrush(pa.Colors.ItemExpandedShadow);
					try
					{
						graphics.FillRectangle((Brush)(object)val12, rectangle2.Left + rectangle2.Width - 2, rectangle2.Top + 2, 2, rectangle2.Height - 2);
					}
					finally
					{
						((IDisposable)val12)?.Dispose();
					}
				}
			}
			if ((flag && m_HotTrackingStyle != eHotTrackingStyle.None) || (m_Expanded && !isOnMenu))
			{
				if (!m_Expanded || isOnMenu)
				{
					Rectangle rectangle5 = rectangle2;
					if (isOnMenu)
					{
						rectangle5 = new Rectangle(rectangle2.Left + 1, rectangle2.Top, rectangle2.Width - 2, rectangle2.Height);
					}
					if (DesignMode && Focused)
					{
						rectangle5 = m_Rect;
						rectangle5.Inflate(-1, -1);
						DesignTime.DrawDesignTimeSelection(graphics, rectangle5, pa.Colors.ItemDesignTimeBorder);
					}
					else if (m_MouseDown)
					{
						if (m_HotTrackingStyle == eHotTrackingStyle.Image)
						{
							rectangle5 = rectangle;
							rectangle5.Inflate(2, 2);
						}
						if (pa.Colors.ItemPressedBackground2.IsEmpty)
						{
							SolidBrush val13 = new SolidBrush(pa.Colors.ItemPressedBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val13, rectangle5);
							}
							finally
							{
								((IDisposable)val13)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val14 = BarFunctions.CreateLinearGradientBrush(rectangle5, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val14, rectangle5);
							((Brush)val14).Dispose();
						}
						Pen val15 = new Pen(pa.Colors.ItemPressedBorder, 1f);
						try
						{
							NativeFunctions.DrawRectangle(graphics, val15, rectangle5);
						}
						finally
						{
							((IDisposable)val15)?.Dispose();
						}
					}
					else if (m_HotTrackingStyle == eHotTrackingStyle.Image && !rectangle.IsEmpty)
					{
						Rectangle rectangle6 = rectangle;
						rectangle6.Inflate(2, 2);
						if (!pa.Colors.ItemHotBackground2.IsEmpty)
						{
							LinearGradientBrush val16 = BarFunctions.CreateLinearGradientBrush(rectangle6, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val16, rectangle6);
							((Brush)val16).Dispose();
						}
						else
						{
							SolidBrush val17 = new SolidBrush(pa.Colors.ItemHotBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val17, rectangle6);
							}
							finally
							{
								((IDisposable)val17)?.Dispose();
							}
						}
						Pen val18 = new Pen(pa.Colors.ItemHotBorder, 1f);
						try
						{
							NativeFunctions.DrawRectangle(graphics, val18, rectangle6);
						}
						finally
						{
							((IDisposable)val18)?.Dispose();
						}
					}
					else
					{
						if (!pa.Colors.ItemCheckedBackground2.IsEmpty && m_Checked)
						{
							LinearGradientBrush val19 = BarFunctions.CreateLinearGradientBrush(rectangle5, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val19, rectangle5);
							((Brush)val19).Dispose();
						}
						else if (!pa.Colors.ItemHotBackground2.IsEmpty)
						{
							LinearGradientBrush val20 = BarFunctions.CreateLinearGradientBrush(rectangle5, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val20, rectangle5);
							((Brush)val20).Dispose();
						}
						else
						{
							SolidBrush val21 = new SolidBrush(pa.Colors.ItemHotBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val21, rectangle5);
							}
							finally
							{
								((IDisposable)val21)?.Dispose();
							}
						}
						Pen val22 = new Pen(pa.Colors.ItemHotBorder, 1f);
						try
						{
							NativeFunctions.DrawRectangle(graphics, val22, rectangle5);
						}
						finally
						{
							((IDisposable)val22)?.Dispose();
						}
					}
				}
				if (image != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
				{
					if (m_MouseDown || (m_Checked && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog))
					{
						image.DrawImage(graphics, rectangle);
					}
					else if (NativeFunctions.ColorDepth >= 16 && EffectiveStyle != eDotNetBarStyle.Office2003)
					{
						ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
						{
							new float[5],
							new float[5],
							new float[5],
							new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f },
							new float[5]
						});
						ImageAttributes val23 = new ImageAttributes();
						val23.ClearColorKey();
						val23.SetColorMatrix(colorMatrix);
						rectangle.Offset(1, 1);
						image.DrawImage(graphics, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val23);
						rectangle.Offset(-2, -2);
						image.DrawImage(graphics, rectangle);
					}
					else
					{
						if (EffectiveStyle == eDotNetBarStyle.OfficeXP)
						{
							rectangle.Offset(-1, -1);
						}
						image.DrawImage(graphics, rectangle);
					}
				}
				if (isOnMenu && base.IsOnCustomizeMenu && m_Visible && !base.SystemItem)
				{
					Rectangle rectangle7 = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
					rectangle7.Inflate(-1, -1);
					Color color = pa.Colors.ItemCheckedBackground;
					if (flag && !pa.Colors.ItemHotBackground2.IsEmpty)
					{
						LinearGradientBrush val24 = BarFunctions.CreateLinearGradientBrush(rectangle7, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val24, rectangle7);
						((Brush)val24).Dispose();
					}
					else
					{
						if (flag)
						{
							color = pa.Colors.ItemHotBackground;
						}
						if (!pa.Colors.ItemCheckedBackground2.IsEmpty && !flag)
						{
							LinearGradientBrush val25 = BarFunctions.CreateLinearGradientBrush(rectangle7, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val25, rectangle7);
							((Brush)val25).Dispose();
						}
						else
						{
							SolidBrush val26 = new SolidBrush(color);
							graphics.FillRectangle((Brush)(object)val26, rectangle7);
							((Brush)val26).Dispose();
						}
					}
				}
			}
			else
			{
				if (image != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
				{
					if (!m_Checked || base.IsOnCustomizeMenu)
					{
						if (m_HotTrackingStyle != eHotTrackingStyle.Color)
						{
							image.DrawImage(graphics, rectangle);
						}
						else
						{
							ColorMatrix colorMatrix2 = new ColorMatrix(new float[5][]
							{
								new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
								new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
								new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
								new float[5] { 0f, 0f, 0f, 1f, 0f },
								new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
							});
							ImageAttributes val27 = new ImageAttributes();
							val27.SetColorMatrix(colorMatrix2);
							image.DrawImage(graphics, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val27);
						}
					}
					else
					{
						if (((m_Checked && isOnMenu) || !Expanded) && !base.IsOnCustomizeDialog)
						{
							Rectangle rectangle8;
							if (isOnMenu)
							{
								rectangle8 = new Rectangle(m_Rect.X + 1, m_Rect.Y, m_ImageDrawRect.Width - 2, m_Rect.Height);
							}
							else if (m_HotTrackingStyle == eHotTrackingStyle.Image)
							{
								rectangle8 = rectangle;
								rectangle8.Inflate(2, 2);
							}
							else
							{
								rectangle8 = m_Rect;
							}
							if (isOnMenu)
							{
								rectangle8.Inflate(-1, -1);
							}
							Color color2;
							if (flag && m_HotTrackingStyle != eHotTrackingStyle.None)
							{
								if (m_Checked && !pa.Colors.ItemCheckedBackground2.IsEmpty)
								{
									LinearGradientBrush val28 = BarFunctions.CreateLinearGradientBrush(rectangle8, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackgroundGradientAngle);
									graphics.FillRectangle((Brush)(object)val28, rectangle8);
									((Brush)val28).Dispose();
									color2 = Color.Empty;
								}
								else if (!pa.Colors.ItemHotBackground2.IsEmpty)
								{
									LinearGradientBrush val29 = BarFunctions.CreateLinearGradientBrush(rectangle8, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
									graphics.FillRectangle((Brush)(object)val29, rectangle8);
									((Brush)val29).Dispose();
									color2 = Color.Empty;
								}
								else
								{
									color2 = ControlPaint.Dark(pa.Colors.ItemHotBackground);
								}
							}
							else if (!pa.Colors.ItemCheckedBackground2.IsEmpty)
							{
								LinearGradientBrush val30 = BarFunctions.CreateLinearGradientBrush(rectangle8, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
								graphics.FillRectangle((Brush)(object)val30, rectangle8);
								((Brush)val30).Dispose();
								color2 = Color.Empty;
							}
							else
							{
								color2 = pa.Colors.ItemCheckedBackground;
							}
							if (!color2.IsEmpty)
							{
								SolidBrush val31 = new SolidBrush(color2);
								graphics.FillRectangle((Brush)(object)val31, rectangle8);
								((Brush)val31).Dispose();
							}
						}
						image.DrawImage(graphics, rectangle);
					}
				}
				if (isOnMenu && base.IsOnCustomizeMenu && m_Visible && !base.SystemItem)
				{
					Rectangle rectangle9 = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
					rectangle9.Inflate(-1, -1);
					if (pa.Colors.ItemCheckedBackground2.IsEmpty)
					{
						Color itemCheckedBackground = pa.Colors.ItemCheckedBackground;
						SolidBrush val32 = new SolidBrush(itemCheckedBackground);
						graphics.FillRectangle((Brush)(object)val32, rectangle9);
						((Brush)val32).Dispose();
					}
					else
					{
						LinearGradientBrush val33 = BarFunctions.CreateLinearGradientBrush(rectangle9, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val33, rectangle9);
						((Brush)val33).Dispose();
					}
				}
				else if (m_Checked && !isOnMenu && image == null)
				{
					Rectangle rect = m_Rect;
					Color color3;
					if (flag && m_HotTrackingStyle != eHotTrackingStyle.None)
					{
						if (!pa.Colors.ItemCheckedBackground2.IsEmpty)
						{
							LinearGradientBrush val34 = BarFunctions.CreateLinearGradientBrush(rect, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val34, rect);
							((Brush)val34).Dispose();
							color3 = Color.Empty;
						}
						else if (!pa.Colors.ItemHotBackground2.IsEmpty)
						{
							LinearGradientBrush val35 = BarFunctions.CreateLinearGradientBrush(rect, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val35, rect);
							((Brush)val35).Dispose();
							color3 = Color.Empty;
						}
						else
						{
							color3 = pa.Colors.ItemHotBackground;
						}
					}
					else if (!pa.Colors.ItemCheckedBackground2.IsEmpty)
					{
						LinearGradientBrush val36 = BarFunctions.CreateLinearGradientBrush(rect, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
						graphics.FillRectangle((Brush)(object)val36, rect);
						((Brush)val36).Dispose();
						color3 = Color.Empty;
					}
					else
					{
						color3 = pa.Colors.ItemCheckedBackground;
					}
					if (!color3.IsEmpty)
					{
						SolidBrush val37 = new SolidBrush(color3);
						graphics.FillRectangle((Brush)(object)val37, rect);
						((Brush)val37).Dispose();
					}
				}
			}
			if (isOnMenu && base.IsOnCustomizeMenu && m_Visible && !base.SystemItem)
			{
				Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
				r.Inflate(-1, -1);
				Color itemCheckedBorder = pa.Colors.ItemCheckedBorder;
				Pen val38 = new Pen(itemCheckedBorder, 1f);
				NativeFunctions.DrawRectangle(graphics, val38, r);
				val38.Dispose();
				val38 = new Pen(pa.Colors.ItemCheckedText);
				Point[] array2 = new Point[3];
				array2[0].X = r.Left + (r.Width - 5) / 2 - 1;
				array2[0].Y = r.Top + (r.Height - 6) / 2 + 3;
				array2[1].X = array2[0].X + 2;
				array2[1].Y = array2[0].Y + 2;
				array2[2].X = array2[1].X + 4;
				array2[2].Y = array2[1].Y - 4;
				graphics.DrawLines(val38, array2);
				array2[0].X++;
				array2[1].X++;
				array2[2].X++;
				graphics.DrawLines(val38, array2);
				val38.Dispose();
			}
			if (m_Checked && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
			{
				Rectangle rectangle10;
				if (isOnMenu)
				{
					rectangle10 = new Rectangle(m_Rect.X + 1, m_Rect.Y, m_ImageDrawRect.Width - 2, m_Rect.Height);
				}
				else if (m_HotTrackingStyle == eHotTrackingStyle.Image)
				{
					rectangle10 = rectangle;
					rectangle10.Inflate(2, 2);
				}
				else
				{
					rectangle10 = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
				}
				if (isOnMenu)
				{
					rectangle10.Inflate(-1, -1);
				}
				if (isOnMenu || !Expanded)
				{
					if (image == null || m_ButtonStyle == eButtonStyle.TextOnlyAlways)
					{
						if (flag)
						{
							if (m_Checked && !pa.Colors.ItemCheckedBackground2.IsEmpty)
							{
								LinearGradientBrush val39 = BarFunctions.CreateLinearGradientBrush(rectangle10, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackgroundGradientAngle);
								graphics.FillRectangle((Brush)(object)val39, rectangle10);
								((Brush)val39).Dispose();
							}
							else if (!pa.Colors.ItemHotBackground2.IsEmpty)
							{
								LinearGradientBrush val40 = BarFunctions.CreateLinearGradientBrush(rectangle10, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
								graphics.FillRectangle((Brush)(object)val40, rectangle10);
								((Brush)val40).Dispose();
							}
							else
							{
								SolidBrush val41 = new SolidBrush(pa.Colors.ItemHotBackground);
								try
								{
									graphics.FillRectangle((Brush)(object)val41, rectangle10);
								}
								finally
								{
									((IDisposable)val41)?.Dispose();
								}
							}
						}
						else if (pa.Colors.ItemCheckedBackground2.IsEmpty)
						{
							SolidBrush val42 = new SolidBrush(pa.Colors.ItemCheckedBackground);
							try
							{
								graphics.FillRectangle((Brush)(object)val42, rectangle10);
							}
							finally
							{
								((IDisposable)val42)?.Dispose();
							}
						}
						else
						{
							LinearGradientBrush val43 = BarFunctions.CreateLinearGradientBrush(rectangle10, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
							graphics.FillRectangle((Brush)(object)val43, rectangle10);
							((Brush)val43).Dispose();
						}
					}
					Color itemCheckedBorder2 = pa.Colors.ItemCheckedBorder;
					Pen val44 = new Pen(itemCheckedBorder2, 1f);
					NativeFunctions.DrawRectangle(graphics, val44, rectangle10);
					val44.Dispose();
				}
				if ((image == null || m_ButtonStyle == eButtonStyle.TextOnlyAlways) && isOnMenu)
				{
					Pen val45 = new Pen(pa.Colors.ItemCheckedText);
					Point[] array3 = new Point[3];
					array3[0].X = rectangle10.Left + (rectangle10.Width - 5) / 2 - 1;
					array3[0].Y = rectangle10.Top + (rectangle10.Height - 6) / 2 + 3;
					array3[1].X = array3[0].X + 2;
					array3[1].Y = array3[0].Y + 2;
					array3[2].X = array3[1].X + 4;
					array3[2].Y = array3[1].Y - 4;
					graphics.DrawLines(val45, array3);
					array3[0].X++;
					array3[1].X++;
					array3[2].X++;
					graphics.DrawLines(val45, array3);
					val45.Dispose();
				}
			}
		}
		else
		{
			if (isOnMenu && flag && m_HotTrackingStyle == eHotTrackingStyle.Default)
			{
				Rectangle r2 = new Rectangle(rectangle2.Left + 1, rectangle2.Top, rectangle2.Width - 2, rectangle2.Height);
				Pen val46 = new Pen(pa.Colors.ItemHotBorder, 1f);
				try
				{
					NativeFunctions.DrawRectangle(graphics, val46, r2);
				}
				finally
				{
					((IDisposable)val46)?.Dispose();
				}
			}
			if (m_Checked && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
			{
				Rectangle r3;
				if (isOnMenu)
				{
					r3 = new Rectangle(m_Rect.X + 1, m_Rect.Y, m_ImageDrawRect.Width - 2, m_Rect.Height);
				}
				else if (m_HotTrackingStyle == eHotTrackingStyle.Image)
				{
					r3 = rectangle;
					r3.Inflate(2, 2);
				}
				else
				{
					r3 = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
				}
				if (isOnMenu)
				{
					r3.Inflate(-1, -1);
				}
				if (isOnMenu || !Expanded)
				{
					Color itemDisabledText = pa.Colors.ItemDisabledText;
					Pen val47 = new Pen(itemDisabledText, 1f);
					NativeFunctions.DrawRectangle(graphics, val47, r3);
					val47.Dispose();
				}
				if ((image == null || m_ButtonStyle == eButtonStyle.TextOnlyAlways) && isOnMenu)
				{
					Pen val48 = new Pen(pa.Colors.ItemDisabledText);
					Point[] array4 = new Point[3];
					array4[0].X = r3.Left + (r3.Width - 5) / 2 - 1;
					array4[0].Y = r3.Top + (r3.Height - 6) / 2 + 3;
					array4[1].X = array4[0].X + 2;
					array4[1].Y = array4[0].Y + 2;
					array4[2].X = array4[1].X + 4;
					array4[2].Y = array4[1].Y - 4;
					graphics.DrawLines(val48, array4);
					array4[0].X++;
					array4[1].X++;
					array4[2].X++;
					graphics.DrawLines(val48, array4);
					val48.Dispose();
				}
			}
			empty = pa.Colors.ItemDisabledText;
			if (image != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
			{
				image.DrawImage(graphics, rectangle);
			}
		}
		if (isOnMenu || m_ButtonStyle != 0 || image == null || (!isOnMenu && (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)))
		{
			if (isOnMenu)
			{
				rectangle = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, rectangle2.Width - m_ImageDrawRect.Right - 26, m_TextDrawRect.Height);
			}
			else
			{
				rectangle = m_TextDrawRect;
				if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)
				{
					if (m_Orientation == eOrientation.Vertical)
					{
						rectangle = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, m_TextDrawRect.Width, m_TextDrawRect.Height);
					}
					else
					{
						rectangle = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, rectangle2.Width, m_TextDrawRect.Height);
						if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems)
						{
							rectangle.Width -= 10;
						}
					}
					eTextFormat2 |= eTextFormat.HorizontalCenter;
				}
				else if (isOnMenuBar && image == null)
				{
					eTextFormat2 |= eTextFormat.HorizontalCenter;
				}
				if (m_MouseDown && m_HotTrackingStyle != eHotTrackingStyle.Image)
				{
					empty = ((!m_ForeColor.IsEmpty) ? ControlPaint.Light(m_ForeColor) : pa.Colors.ItemPressedText);
				}
			}
			rectangle.Offset(rectangle2.Left, rectangle2.Top);
			if (m_Orientation == eOrientation.Vertical && !isOnMenu)
			{
				graphics.RotateTransform(90f);
				TextDrawing.DrawStringLegacy(graphics, m_Text, val, empty, new Rectangle(rectangle.Top, -rectangle.Right, rectangle.Height, rectangle.Width), eTextFormat2);
				graphics.ResetTransform();
			}
			else
			{
				if (rectangle.Right > m_Rect.Right)
				{
					rectangle.Width = m_Rect.Right - rectangle.Left;
				}
				TextDrawing.DrawString(graphics, m_Text, val, empty, rectangle, eTextFormat2);
				if (!DesignMode && Focused && !isOnMenu && !isOnMenuBar)
				{
					Rectangle rectangle11 = rectangle;
					ControlPaint.DrawFocusRectangle(graphics, rectangle11);
				}
			}
		}
		if (DrawShortcutText != "" && isOnMenu && !base.IsOnCustomizeDialog)
		{
			eTextFormat2 |= eTextFormat.HidePrefix | eTextFormat.Right;
			TextDrawing.DrawString(graphics, DrawShortcutText, val, empty, rectangle, eTextFormat2);
		}
		if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems)
		{
			if (isOnMenu)
			{
				Point[] array5 = new Point[3];
				array5[0].X = rectangle2.Left + rectangle2.Width - 12;
				array5[0].Y = rectangle2.Top + (rectangle2.Height - 8) / 2;
				array5[1].X = array5[0].X;
				array5[1].Y = array5[0].Y + 8;
				array5[2].X = array5[0].X + 4;
				array5[2].Y = array5[0].Y + 4;
				SolidBrush val49 = new SolidBrush(empty);
				try
				{
					graphics.FillPolygon((Brush)(object)val49, array5);
				}
				finally
				{
					((IDisposable)val49)?.Dispose();
				}
			}
			else if (!m_SubItemsRect.IsEmpty)
			{
				if (GetEnabled(pa.ContainerControl) && (flag || m_Checked) && !m_Expanded && m_HotTrackingStyle != eHotTrackingStyle.None && m_HotTrackingStyle != eHotTrackingStyle.Image)
				{
					if (m_Orientation == eOrientation.Horizontal)
					{
						Pen val50 = new Pen(pa.Colors.ItemHotBorder);
						try
						{
							graphics.DrawLine(val50, rectangle2.Left + m_SubItemsRect.Left, rectangle2.Top, rectangle2.Left + m_SubItemsRect.Left, rectangle2.Bottom - 1);
						}
						finally
						{
							((IDisposable)val50)?.Dispose();
						}
					}
					else
					{
						Pen val51 = new Pen(pa.Colors.ItemHotBorder);
						try
						{
							graphics.DrawLine(val51, rectangle2.Left, rectangle2.Top + m_SubItemsRect.Top, rectangle2.Right - 2, rectangle2.Top + m_SubItemsRect.Top);
						}
						finally
						{
							((IDisposable)val51)?.Dispose();
						}
					}
				}
				Point[] array6 = new Point[3];
				if (PopupSide == ePopupSide.Default)
				{
					if (m_Orientation == eOrientation.Horizontal)
					{
						array6[0].X = rectangle2.Left + m_SubItemsRect.Left + (m_SubItemsRect.Width - 5) / 2;
						array6[0].Y = rectangle2.Top + (m_SubItemsRect.Height - 3) / 2 + 1;
						array6[1].X = array6[0].X + 5;
						array6[1].Y = array6[0].Y;
						array6[2].X = array6[0].X + 2;
						array6[2].Y = array6[0].Y + 3;
					}
					else
					{
						array6[0].X = rectangle2.Left + (m_SubItemsRect.Width - 3) / 2 + 1;
						array6[0].Y = rectangle2.Top + m_SubItemsRect.Top + (m_SubItemsRect.Height - 5) / 2;
						array6[1].X = array6[0].X;
						array6[1].Y = array6[0].Y + 6;
						array6[2].X = array6[0].X - 3;
						array6[2].Y = array6[0].Y + 3;
					}
				}
				else
				{
					switch (PopupSide)
					{
					case ePopupSide.Left:
						array6[0].X = rectangle2.Left + m_SubItemsRect.Left + m_SubItemsRect.Width / 2;
						array6[0].Y = rectangle2.Top + m_SubItemsRect.Height / 2 - 3;
						array6[1].X = array6[0].X;
						array6[1].Y = array6[0].Y + 6;
						array6[2].X = array6[0].X + 3;
						array6[2].Y = array6[0].Y + 3;
						break;
					case ePopupSide.Right:
						array6[0].X = rectangle2.Left + m_SubItemsRect.Left + m_SubItemsRect.Width / 2 + 3;
						array6[0].Y = rectangle2.Top + m_SubItemsRect.Height / 2 - 3;
						array6[1].X = array6[0].X;
						array6[1].Y = array6[0].Y + 6;
						array6[2].X = array6[0].X - 3;
						array6[2].Y = array6[0].Y + 3;
						break;
					case ePopupSide.Top:
						array6[0].X = rectangle2.Left + m_SubItemsRect.Left + (m_SubItemsRect.Width - 5) / 2;
						array6[0].Y = rectangle2.Top + (m_SubItemsRect.Height - 3) / 2 + 4;
						array6[1].X = array6[0].X + 6;
						array6[1].Y = array6[0].Y;
						array6[2].X = array6[0].X + 3;
						array6[2].Y = array6[0].Y - 4;
						break;
					case ePopupSide.Bottom:
						array6[0].X = rectangle2.Left + m_SubItemsRect.Left + (m_SubItemsRect.Width - 5) / 2 + 1;
						array6[0].Y = rectangle2.Top + (m_SubItemsRect.Height - 3) / 2 + 1;
						array6[1].X = array6[0].X + 5;
						array6[1].Y = array6[0].Y;
						array6[2].X = array6[0].X + 2;
						array6[2].Y = array6[0].Y + 3;
						break;
					}
				}
				if (GetEnabled(pa.ContainerControl))
				{
					SolidBrush val52 = new SolidBrush(pa.Colors.ItemText);
					try
					{
						graphics.FillPolygon((Brush)(object)val52, array6);
					}
					finally
					{
						((IDisposable)val52)?.Dispose();
					}
				}
				else
				{
					SolidBrush val53 = new SolidBrush(pa.Colors.ItemDisabledText);
					try
					{
						graphics.FillPolygon((Brush)(object)val53, array6);
					}
					finally
					{
						((IDisposable)val53)?.Dispose();
					}
				}
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle r4 = rectangle2;
			r4.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, r4, pa.Colors.ItemDesignTimeBorder);
		}
		image?.Dispose();
	}

	private void PaintThemed(ItemPaintArgs pa)
	{
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		ThemeToolbar themeToolbar = pa.ThemeToolbar;
		ThemeToolbarParts part = ThemeToolbarParts.Button;
		ThemeToolbarStates themeToolbarStates = ThemeToolbarStates.Normal;
		eTextFormat eTextFormat2 = pa.ButtonStringFormat;
		Color controlText = SystemColors.ControlText;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rect = m_Rect;
		Font val = null;
		CompositeImage image = GetImage();
		val = ((m_Font == null) ? GetFont(pa, isLayout: false) : m_Font);
		bool flag = (SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems && !m_SubItemsRect.IsEmpty;
		if (flag)
		{
			part = ThemeToolbarParts.SplitButton;
		}
		if (image != null)
		{
			rectangle = ((m_ImagePosition != eImagePosition.Top && m_ImagePosition != eImagePosition.Bottom) ? new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, m_ImageDrawRect.Width, m_ImageDrawRect.Height) : new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, rect.Width, m_ImageDrawRect.Height));
			rectangle.Offset(rect.Left, rect.Top);
			rectangle.Offset((rectangle.Width - ImageSize.Width) / 2, (rectangle.Height - ImageSize.Height) / 2);
			rectangle.Width = ImageSize.Width;
			rectangle.Height = ImageSize.Height;
		}
		if (!GetEnabled(pa.ContainerControl))
		{
			themeToolbarStates = ThemeToolbarStates.Disabled;
			controlText = pa.Colors.ItemDisabledText;
		}
		else if (m_MouseDown)
		{
			themeToolbarStates = ThemeToolbarStates.Pressed;
			controlText = pa.Colors.ItemPressedText;
		}
		else if (m_MouseOver && m_Checked)
		{
			themeToolbarStates = ThemeToolbarStates.HotChecked;
			controlText = pa.Colors.ItemHotText;
		}
		else if (m_MouseOver || m_Expanded)
		{
			themeToolbarStates = ThemeToolbarStates.Hot;
			controlText = pa.Colors.ItemHotText;
		}
		else if (m_Checked)
		{
			themeToolbarStates = ThemeToolbarStates.Checked;
			controlText = pa.Colors.ItemCheckedText;
		}
		else
		{
			controlText = pa.Colors.ItemText;
		}
		Rectangle r = m_Rect;
		if (m_HotTrackingStyle == eHotTrackingStyle.Image && image != null)
		{
			r = rectangle;
			r.Inflate(3, 3);
		}
		else if (flag)
		{
			r.Width -= m_SubItemsRect.Width;
		}
		if (m_HotTrackingStyle != eHotTrackingStyle.None)
		{
			themeToolbar.DrawBackground(graphics, part, themeToolbarStates, r);
		}
		if (image != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
		{
			if (themeToolbarStates == ThemeToolbarStates.Normal && m_HotTrackingStyle == eHotTrackingStyle.Color)
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
					new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
					new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
				});
				ImageAttributes val2 = new ImageAttributes();
				val2.SetColorMatrix(colorMatrix);
				image.DrawImage(graphics, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val2);
			}
			else if (themeToolbarStates == ThemeToolbarStates.Normal && !image.IsIcon)
			{
				ImageAttributes val3 = new ImageAttributes();
				val3.SetGamma(0.7f, (ColorAdjustType)1);
				image.DrawImage(graphics, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val3);
			}
			else
			{
				image.DrawImage(graphics, rectangle);
			}
		}
		if (m_ButtonStyle == eButtonStyle.ImageAndText || m_ButtonStyle == eButtonStyle.TextOnlyAlways || image == null)
		{
			Rectangle rectangle2 = m_TextDrawRect;
			if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)
			{
				if (m_Orientation == eOrientation.Vertical)
				{
					rectangle2 = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, m_TextDrawRect.Width, m_TextDrawRect.Height);
				}
				else
				{
					rectangle2 = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, rect.Width, m_TextDrawRect.Height);
					if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems)
					{
						rectangle2.Width -= 10;
					}
				}
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			rectangle2.Offset(rect.Left, rect.Top);
			if (m_Orientation == eOrientation.Vertical)
			{
				graphics.RotateTransform(90f);
				TextDrawing.DrawStringLegacy(graphics, m_Text, val, controlText, new Rectangle(rectangle2.Top, -rectangle2.Right, rectangle2.Height, rectangle2.Width), eTextFormat2);
				graphics.ResetTransform();
			}
			else
			{
				if (rectangle2.Right > m_Rect.Right)
				{
					rectangle2.Width = m_Rect.Right - rectangle2.Left;
				}
				TextDrawing.DrawString(graphics, m_Text, val, controlText, rectangle2, eTextFormat2);
				if (!DesignMode && Focused && !pa.IsOnMenu && !pa.IsOnMenuBar)
				{
					Rectangle rectangle3 = rectangle2;
					ControlPaint.DrawFocusRectangle(graphics, rectangle3);
				}
			}
		}
		if (flag)
		{
			part = ThemeToolbarParts.SplitButtonDropDown;
			themeToolbarStates = (GetEnabled(pa.ContainerControl) ? ThemeToolbarStates.Normal : ThemeToolbarStates.Disabled);
			if (m_HotTrackingStyle != eHotTrackingStyle.None && m_HotTrackingStyle != eHotTrackingStyle.Image && GetEnabled(pa.ContainerControl))
			{
				if (m_Expanded || m_MouseDown)
				{
					themeToolbarStates = ThemeToolbarStates.Pressed;
				}
				else if (m_MouseOver && m_Checked)
				{
					themeToolbarStates = ThemeToolbarStates.HotChecked;
				}
				else if (m_Checked)
				{
					themeToolbarStates = ThemeToolbarStates.Checked;
				}
				else if (m_MouseOver)
				{
					themeToolbarStates = ThemeToolbarStates.Hot;
				}
			}
			if (m_Orientation == eOrientation.Horizontal)
			{
				Rectangle subItemsRect = m_SubItemsRect;
				subItemsRect.Offset(rect.X, rect.Y);
				themeToolbar.DrawBackground(graphics, part, themeToolbarStates, subItemsRect);
			}
			else
			{
				Rectangle subItemsRect2 = m_SubItemsRect;
				subItemsRect2.Offset(rect.X, rect.Y);
				themeToolbar.DrawBackground(graphics, part, themeToolbarStates, subItemsRect2);
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle r2 = rect;
			r2.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, r2, pa.Colors.ItemDesignTimeBorder);
		}
		image?.Dispose();
	}

	private void PaintOffice(ItemPaintArgs pa)
	{
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Expected O, but got Unknown
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Expected O, but got Unknown
		//IL_0eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec1: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rect = m_Rect;
		Rectangle empty = Rectangle.Empty;
		Color color = SystemColors.ControlText;
		Color baseColor = SystemColors.Control;
		if (m_Parent is GenericItemContainer && !((GenericItemContainer)m_Parent).BackColor.IsEmpty)
		{
			baseColor = ((GenericItemContainer)m_Parent).BackColor;
		}
		else if (m_Parent is SideBarPanelItem && !((SideBarPanelItem)m_Parent).BackgroundStyle.BackColor1.IsEmpty)
		{
			baseColor = ((SideBarPanelItem)m_Parent).BackgroundStyle.BackColor1.GetCompositeColor();
		}
		if (m_MouseOver && !m_HotForeColor.IsEmpty)
		{
			color = m_HotForeColor;
		}
		else if (!m_ForeColor.IsEmpty)
		{
			color = m_ForeColor;
		}
		Font val = null;
		bool isOnMenu = pa.IsOnMenu;
		bool flag = pa.ContainerControl is ButtonX;
		eTextFormat eTextFormat2 = pa.ButtonStringFormat;
		CompositeImage image = GetImage();
		val = ((m_Font == null) ? GetFont(pa, isLayout: false) : m_Font);
		if (image != null)
		{
			rectangle = ((isOnMenu || (m_ImagePosition != eImagePosition.Top && m_ImagePosition != eImagePosition.Bottom)) ? m_ImageDrawRect : new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y + 1, rect.Width, m_ImageDrawRect.Height));
			rectangle.Offset(rect.Left, rect.Top);
			rectangle.Offset((rectangle.Width - ImageSize.Width) / 2, (rectangle.Height - ImageSize.Height) / 2);
			rectangle.Width = ImageSize.Width;
			rectangle.Height = ImageSize.Height;
		}
		if (isOnMenu && !DesignMode && MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !RecentlyUsed)
		{
			graphics.FillRectangle((Brush)new SolidBrush(ColorFunctions.RecentlyUsedOfficeBackColor()), m_Rect);
		}
		else if (flag)
		{
			ButtonState val2 = (ButtonState)0;
			if (m_MouseDown)
			{
				val2 = (ButtonState)512;
			}
			else if (m_Checked || m_Expanded)
			{
				val2 = (ButtonState)1024;
			}
			ControlPaint.DrawButton(graphics, rect, val2);
		}
		if (GetEnabled(pa.ContainerControl) || DesignMode)
		{
			if (m_Expanded && !isOnMenu)
			{
				graphics.FillRectangle(SystemBrushes.Control, rect);
				BarFunctions.DrawBorder3D(graphics, rect, (Border3DStyle)2, (Border3DSide)2063, baseColor);
			}
			if ((m_MouseOver && m_HotTrackingStyle != eHotTrackingStyle.None) || m_Expanded || (m_Checked && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog))
			{
				if (isOnMenu || base.IsOnCustomizeDialog)
				{
					if (((m_MouseOver && m_HotTrackingStyle != eHotTrackingStyle.None) || m_Expanded) && (!m_MouseOver || !DesignMode || base.IsOnCustomizeDialog))
					{
						graphics.FillRectangle(SystemBrushes.Highlight, rect);
					}
				}
				else if (m_MouseDown || (m_Checked && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog) || (Expanded && (!ShowSubItems || IsOnMenuBar)))
				{
					if (m_SubItemsRect.IsEmpty)
					{
						if (!flag)
						{
							BarFunctions.DrawBorder3D(graphics, rect, (Border3DStyle)2, (Border3DSide)2063, baseColor);
							if (m_Checked && !m_MouseOver && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
							{
								Rectangle rectangle2 = rect;
								rectangle2.Inflate(-1, -1);
								graphics.FillRectangle(ColorFunctions.GetPushedBrush(this), rectangle2);
							}
						}
					}
					else if (!flag)
					{
						Rectangle rectangle3 = ((m_Orientation != 0) ? new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - m_SubItemsRect.Height) : new Rectangle(rect.X, rect.Y, rect.Width - m_SubItemsRect.Width, rect.Height));
						BarFunctions.DrawBorder3D(graphics, rectangle3, (Border3DStyle)2, (Border3DSide)2063, baseColor);
						if (!m_MouseOver)
						{
							rectangle3.Inflate(-1, -1);
							graphics.FillRectangle(ColorFunctions.GetPushedBrush(this), rectangle3);
						}
					}
				}
				else if (!DesignMode)
				{
					if (m_SubItemsRect.IsEmpty)
					{
						if (!flag)
						{
							BarFunctions.DrawBorder3D(graphics, rect, (Border3DStyle)4, (Border3DSide)2063, baseColor);
						}
					}
					else
					{
						Rectangle r = ((m_Orientation != 0) ? new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - m_SubItemsRect.Height) : new Rectangle(rect.X, rect.Y, rect.Width - m_SubItemsRect.Width, rect.Height));
						BarFunctions.DrawBorder3D(graphics, r, (Border3DStyle)4, (Border3DSide)2063, baseColor);
					}
				}
				if (image != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
				{
					empty = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, m_ImageDrawRect.Width, rect.Height);
					empty.Offset(rect.Left, rect.Top);
					if (isOnMenu)
					{
						if (m_Checked && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog)
						{
							BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)2, (Border3DSide)2063, baseColor);
							if (!m_MouseOver)
							{
								empty.Inflate(-1, -1);
								graphics.FillRectangle(ColorFunctions.GetPushedBrush(this), empty);
							}
						}
						else
						{
							BarFunctions.DrawBorder3D(graphics, empty, (Border3DStyle)4, (Border3DSide)2063, baseColor);
						}
					}
					else if (m_MouseDown)
					{
						rectangle.Offset(1, 1);
					}
					image.DrawImage(graphics, rectangle);
				}
				else if ((image == null || m_ButtonStyle == eButtonStyle.TextOnlyAlways) && m_Checked && !base.IsOnCustomizeMenu && !base.IsOnCustomizeDialog && isOnMenu)
				{
					empty = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, m_ImageDrawRect.Width, rect.Height);
					empty.Offset(rect.Left, rect.Top);
					DrawOfficeCheckBox(graphics, empty);
				}
			}
			else if (image != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
			{
				if (m_HotTrackingStyle != eHotTrackingStyle.Color)
				{
					image.DrawImage(graphics, rectangle);
				}
				else if (m_HotTrackingStyle == eHotTrackingStyle.Color)
				{
					ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
					{
						new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
						new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
						new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
						new float[5] { 0f, 0f, 0f, 1f, 0f },
						new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
					});
					ImageAttributes val3 = new ImageAttributes();
					val3.SetColorMatrix(colorMatrix);
					image.DrawImage(graphics, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val3);
				}
			}
			if (isOnMenu && base.IsOnCustomizeMenu && m_Visible && !base.SystemItem)
			{
				Rectangle r2 = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
				DrawOfficeCheckBox(graphics, r2);
			}
		}
		else
		{
			color = SystemColors.ControlDark;
			if (image != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
			{
				image.DrawImage(graphics, rectangle);
			}
		}
		if (isOnMenu || m_ButtonStyle != 0 || image == null || (!isOnMenu && (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)))
		{
			if (isOnMenu)
			{
				rectangle = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, rect.Width - m_ImageDrawRect.Right - 24, m_TextDrawRect.Height);
			}
			else
			{
				rectangle = m_TextDrawRect;
				if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)
				{
					if (m_Orientation == eOrientation.Horizontal)
					{
						rectangle = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, rect.Width, m_TextDrawRect.Height);
					}
					if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && m_Orientation == eOrientation.Horizontal && ShowSubItems)
					{
						rectangle.Width -= 12;
					}
					eTextFormat2 |= eTextFormat.HorizontalCenter;
				}
			}
			if (((m_MouseOver && m_HotTrackingStyle != eHotTrackingStyle.None) || m_Expanded) && isOnMenu && !DesignMode)
			{
				color = SystemColors.HighlightText;
			}
			rectangle.Offset(rect.Left, rect.Top);
			if (!isOnMenu && (m_MouseDown || (Expanded && IsOnMenuBar)))
			{
				rectangle.Offset(1, 1);
			}
			if (flag)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
				rectangle.Height--;
			}
			if (GetEnabled(pa.ContainerControl) || DesignMode)
			{
				if (m_Orientation == eOrientation.Vertical && !isOnMenu)
				{
					graphics.RotateTransform(90f);
					TextDrawing.DrawStringLegacy(graphics, m_Text, val, color, new Rectangle(rectangle.Top, -rectangle.Right, rectangle.Height, rectangle.Width), eTextFormat2);
					graphics.ResetTransform();
				}
				else
				{
					TextDrawing.DrawString(graphics, m_Text, val, color, rectangle, eTextFormat2);
					if (!DesignMode && Focused && !isOnMenu && !pa.IsOnMenuBar)
					{
						Rectangle rectangle4 = rectangle;
						ControlPaint.DrawFocusRectangle(graphics, rectangle4);
					}
				}
			}
			else if (m_Orientation == eOrientation.Vertical && !isOnMenu)
			{
				graphics.RotateTransform(90f);
				ControlPaint.DrawStringDisabled(graphics, m_Text, val, SystemColors.Control, (RectangleF)new Rectangle(rectangle.Top, -rectangle.Right, rectangle.Height, rectangle.Width), TextDrawing.GetStringFormat(eTextFormat2));
				graphics.ResetTransform();
			}
			else
			{
				ControlPaint.DrawStringDisabled(graphics, m_Text, val, SystemColors.Control, (RectangleF)rectangle, TextDrawing.GetStringFormat(eTextFormat2));
			}
		}
		if (DrawShortcutText != "" && isOnMenu)
		{
			eTextFormat2 |= eTextFormat.HidePrefix | eTextFormat.Right;
			if (GetEnabled(pa.ContainerControl) || DesignMode)
			{
				TextDrawing.DrawString(graphics, DrawShortcutText, val, color, rectangle, eTextFormat2);
			}
			else
			{
				ControlPaint.DrawStringDisabled(graphics, DrawShortcutText, val, SystemColors.Control, (RectangleF)rectangle, TextDrawing.GetStringFormat(eTextFormat2));
			}
		}
		if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems)
		{
			if (isOnMenu)
			{
				Point[] array = new Point[3];
				array[0].X = rect.Left + rect.Width - 12;
				array[0].Y = rect.Top + (rect.Height - 8) / 2;
				array[1].X = array[0].X;
				array[1].Y = array[0].Y + 8;
				array[2].X = array[0].X + 4;
				array[2].Y = array[0].Y + 4;
				SolidBrush val4 = new SolidBrush(color);
				try
				{
					graphics.FillPolygon((Brush)(object)val4, array);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else if (!m_SubItemsRect.IsEmpty)
			{
				if (m_MouseOver && !m_Expanded && m_HotTrackingStyle != eHotTrackingStyle.None)
				{
					Rectangle r3 = new Rectangle(m_SubItemsRect.X, m_SubItemsRect.Y, m_SubItemsRect.Width, m_SubItemsRect.Height);
					r3.Offset(rect.X, rect.Y);
					BarFunctions.DrawBorder3D(graphics, r3, (Border3DStyle)4, (Border3DSide)2063, baseColor);
				}
				else if (m_Expanded)
				{
					Rectangle r4 = new Rectangle(m_SubItemsRect.X, m_SubItemsRect.Y, m_SubItemsRect.Width, m_SubItemsRect.Height);
					r4.Offset(rect.X, rect.Y);
					BarFunctions.DrawBorder3D(graphics, r4, (Border3DStyle)2, (Border3DSide)2063, baseColor);
				}
				Point[] array2 = new Point[3];
				if (m_Orientation == eOrientation.Horizontal)
				{
					array2[0].X = rect.Left + m_SubItemsRect.Left + (m_SubItemsRect.Width - 5) / 2;
					array2[0].Y = rect.Top + (m_SubItemsRect.Height - 3) / 2 + 1;
					array2[1].X = array2[0].X + 5;
					array2[1].Y = array2[0].Y;
					array2[2].X = array2[0].X + 2;
					array2[2].Y = array2[0].Y + 3;
				}
				else
				{
					array2[0].X = rect.Left + (m_SubItemsRect.Width - 3) / 2 + 1;
					array2[0].Y = rect.Top + m_SubItemsRect.Top + (m_SubItemsRect.Height - 5) / 2;
					array2[1].X = array2[0].X;
					array2[1].Y = array2[0].Y + 6;
					array2[2].X = array2[0].X - 3;
					array2[2].Y = array2[0].Y + 3;
				}
				graphics.FillPolygon(SystemBrushes.ControlText, array2);
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle rectangle5 = rect;
			rectangle5.Inflate(-1, -1);
			graphics.DrawRectangle(new Pen(SystemColors.ControlText, 2f), rectangle5);
		}
		image?.Dispose();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Image GetItemImage()
	{
		return GetImage()?.Image;
	}

	internal CompositeImage GetImage(Color textColor)
	{
		if (!GetEnabled() && !base.IsOnCustomizeDialog)
		{
			return GetImage(ImageState.Disabled, textColor);
		}
		if (m_MouseDown || Checked || (Expanded && m_HotTrackingStyle == eHotTrackingStyle.Image))
		{
			return GetImage(ImageState.Pressed, textColor);
		}
		if (m_MouseOver && !m_MouseOverExpand && (EffectiveStyle != eDotNetBarStyle.Metro || !Expanded))
		{
			return GetImage(ImageState.Hover, textColor);
		}
		return GetImage(ImageState.Default, textColor);
	}

	internal CompositeImage GetImage()
	{
		return GetImage(Color.Black);
	}

	internal CompositeImage GetImage(ImageState state, Color textColor)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		Image val = null;
		if (state == ImageState.Disabled && (m_DisabledImage != null || m_DisabledImageIndex >= 0 || m_DisabledIcon != null || m_Image != null || m_ImageIndex >= 0 || m_Icon != null))
		{
			if (m_DisabledImage != null)
			{
				return new CompositeImage(m_DisabledImage, dispose: false, m_ImageSizeOverride);
			}
			if (m_DisabledIcon != null)
			{
				return new CompositeImage(m_DisabledIcon, dispose: false, m_ImageSizeOverride);
			}
			if (m_DisabledImageIndex >= 0)
			{
				DisposedCachedImageListImage();
				val = GetImageFromImageList(m_DisabledImageIndex);
				if (val != null)
				{
					return new CompositeImage(val, val != m_ImageCachedIdx, m_ImageSizeOverride);
				}
				return null;
			}
			CreateDisabledImage();
			if (m_DisabledImage != null)
			{
				return new CompositeImage(m_DisabledImage, dispose: false, m_ImageSizeOverride);
			}
			if (m_DisabledIcon != null)
			{
				return new CompositeImage(m_DisabledIcon, dispose: false, m_ImageSizeOverride);
			}
			return null;
		}
		if (m_Icon != null)
		{
			Size iconSize = IconSize;
			Icon val2 = null;
			try
			{
				val2 = new Icon(m_Icon, iconSize);
			}
			catch
			{
				val2 = null;
			}
			if (val2 == null)
			{
				return new CompositeImage(m_Icon, dispose: false, m_ImageSizeOverride);
			}
			return new CompositeImage(val2, dispose: true, m_ImageSizeOverride);
		}
		if (state == ImageState.Hover && (m_HoverImage != null || m_HoverImageIndex >= 0))
		{
			if (m_HoverImage != null)
			{
				return new CompositeImage(m_HoverImage, dispose: false, m_ImageSizeOverride);
			}
			if (m_HoverImageIndex >= 0)
			{
				DisposedCachedImageListImage();
				val = GetImageFromImageList(m_HoverImageIndex);
				if (val != null)
				{
					return new CompositeImage(val, val != m_ImageCachedIdx, m_ImageSizeOverride);
				}
				return null;
			}
		}
		if (state == ImageState.Pressed && (m_PressedImage != null || m_PressedImageIndex >= 0))
		{
			if (m_PressedImage != null)
			{
				return new CompositeImage(m_PressedImage, dispose: false, m_ImageSizeOverride);
			}
			if (m_PressedImageIndex >= 0)
			{
				DisposedCachedImageListImage();
				val = GetImageFromImageList(m_PressedImageIndex);
				if (val != null)
				{
					return new CompositeImage(val, val != m_ImageCachedIdx, m_ImageSizeOverride);
				}
				return null;
			}
		}
		if (m_ImageSmall != null && (UseSmallImageResolved || (m_ImageSizeOverride.Width == m_ImageSmall.Width && m_ImageSizeOverride.Height == m_ImageSmall.Height)))
		{
			return new CompositeImage(m_ImageSmall, dispose: false);
		}
		if (m_Image != null)
		{
			if (_ImageAlt != null && textColor == Color.White)
			{
				return new CompositeImage(_ImageAlt, dispose: false, m_ImageSizeOverride);
			}
			return new CompositeImage(m_Image, dispose: false, m_ImageSizeOverride);
		}
		if (m_ImageIndex >= 0)
		{
			if (m_DisabledImageIndex >= 0 || m_HoverImageIndex >= 0 || m_PressedImageIndex >= 0)
			{
				DisposedCachedImageListImage();
			}
			val = GetImageFromImageList(m_ImageIndex);
			if (val != null)
			{
				return new CompositeImage(val, val != m_ImageCachedIdx, m_ImageSizeOverride);
			}
		}
		return null;
	}

	private eBarImageSize GetImageListSize(IBarImageSize barImageSize)
	{
		eBarImageSize result = eBarImageSize.Default;
		if (barImageSize != null)
		{
			result = barImageSize.ImageSize;
		}
		if (_ImageListSizeSelection != eButtonImageListSelection.NotSet)
		{
			result = (eBarImageSize)_ImageListSizeSelection;
		}
		return result;
	}

	private Image GetImageFromImageList(int ImageIndex)
	{
		if (ImageIndex >= 0)
		{
			IOwner owner = null;
			IBarImageSize barImageSize = null;
			if (_ItemPaintArgs != null)
			{
				owner = _ItemPaintArgs.Owner;
				barImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
			}
			if (owner == null)
			{
				owner = GetOwner() as IOwner;
			}
			if (barImageSize == null)
			{
				barImageSize = ContainerControl as IBarImageSize;
			}
			if (owner != null)
			{
				try
				{
					eBarImageSize imageListSize = GetImageListSize(barImageSize);
					if (imageListSize != 0)
					{
						if (imageListSize == eBarImageSize.Medium && owner.ImagesMedium != null && !UseSmallImageResolved)
						{
							return owner.ImagesMedium.Images[ImageIndex];
						}
						if (imageListSize == eBarImageSize.Large && owner.ImagesLarge != null && !UseSmallImageResolved)
						{
							return owner.ImagesLarge.Images[ImageIndex];
						}
						if (owner.Images != null)
						{
							if (ImageIndex != m_ImageIndex)
							{
								return owner.Images.Images[ImageIndex];
							}
							if (m_ImageCachedIdx == null && ImageIndex < owner.Images.Images.Count)
							{
								m_ImageCachedIdx = owner.Images.Images[ImageIndex];
							}
						}
					}
					else if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != 0)
					{
						eBarImageSize itemImageSize = ((SideBarPanelItem)m_Parent).ItemImageSize;
						if (itemImageSize == eBarImageSize.Medium && owner.ImagesMedium != null)
						{
							return owner.ImagesMedium.Images[ImageIndex];
						}
						if (itemImageSize == eBarImageSize.Large && owner.ImagesLarge != null)
						{
							return owner.ImagesLarge.Images[ImageIndex];
						}
						if (owner.Images != null)
						{
							return owner.Images.Images[ImageIndex];
						}
					}
					else if (owner.Images != null)
					{
						if (ImageIndex == m_ImageIndex)
						{
							if (m_ImageCachedIdx == null)
							{
								m_ImageCachedIdx = owner.Images.Images[ImageIndex];
							}
							return m_ImageCachedIdx;
						}
						return owner.Images.Images[ImageIndex];
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageFixedSize()
	{
		ImageFixedSize = default(Size);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageFixedSize()
	{
		return !m_ImageSizeOverride.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedSize()
	{
		return !m_FixedSize.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFixedSize()
	{
		TypeDescriptor.GetProperties(this)["FixedSize"].SetValue(this, Size.Empty);
	}

	public override void RecalcSize()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		bool flag = false;
		int pulseBeats = m_PulseBeats;
		if (m_FadeAnimation)
		{
			StopFade();
			flag = m_Pulse;
			m_Pulse = false;
		}
		if (EffectiveStyle == eDotNetBarStyle.Office2000 && !base.IsThemed)
		{
			RecalcSizeOffice();
		}
		else
		{
			RecalcSizeDotNet();
		}
		base.RecalcSize();
		if (flag)
		{
			InvokeDelayed((MethodInvoker)delegate
			{
				Pulse(pulseBeats);
			}, 500);
		}
	}

	private void RecalcSizeDotNet()
	{
		ButtonItemLayout.LayoutButton(this);
	}

	private void RecalcSizeOffice()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = Graphics.FromHwnd(val.Handle);
		val2.PageUnit = (GraphicsUnit)2;
		m_TextDrawRect = Rectangle.Empty;
		m_ImageDrawRect = Rectangle.Empty;
		m_Rect = Rectangle.Empty;
		m_SubItemsRect = Rectangle.Empty;
		bool flag = false;
		if (GetImage(ImageState.Default, Color.Black) != null)
		{
			flag = true;
		}
		Size size = ((m_Parent == null) ? ImageSize : ((!(m_Parent is ImageItem imageItem)) ? ImageSize : ((flag && !base.IsOnMenu) ? ((Orientation != 0) ? new Size(imageItem.SubItemsImageSize.Width, ImageSize.Height) : new Size(ImageSize.Width, imageItem.SubItemsImageSize.Height)) : new Size(imageItem.SubItemsImageSize.Width, imageItem.SubItemsImageSize.Height))));
		Font val3 = null;
		val3 = ((m_Font == null) ? GetFont(null, isLayout: true) : m_Font);
		eTextFormat stringFormat = GetStringFormat();
		Size size2 = ((m_Orientation != eOrientation.Vertical || base.IsOnMenu) ? TextDrawing.MeasureString(val2, m_Text, val3, 0, stringFormat) : TextDrawing.MeasureStringLegacy(val2, m_Text, val3, Size.Empty, stringFormat));
		if (base.IsOnMenu)
		{
			size.Width += 4;
			if (size2.Height > size.Height)
			{
				m_Rect.Height = size2.Height + 4;
			}
			else
			{
				m_Rect.Height = size.Height + 4;
			}
			m_Rect.Height += m_VerticalPadding;
			if (base.IsOnCustomizeMenu)
			{
				m_ImageDrawRect = new Rectangle(m_Rect.Height, 0, m_Rect.Height, m_Rect.Height);
			}
			else
			{
				m_ImageDrawRect = new Rectangle(0, 0, size.Width, m_Rect.Height);
			}
			m_Rect.Width = size2.Width;
			if (DrawShortcutText != "")
			{
				Size size3 = TextDrawing.MeasureString(val2, DrawShortcutText, val3, 0, stringFormat);
				m_Rect.Width += size3.Width + 14;
			}
			m_TextDrawRect = new Rectangle(m_ImageDrawRect.Right + 2, (m_Rect.Height - size2.Height) / 2, m_Rect.Width, size2.Height);
			m_Rect.Width += m_ImageDrawRect.Right + 2 + 24;
			m_Rect.Width += m_HorizontalPadding;
		}
		else
		{
			if (m_Orientation == eOrientation.Horizontal && (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right))
			{
				size.Width += 10;
				if (size2.Height > size.Height)
				{
					m_Rect.Height = size2.Height + 6;
				}
				else
				{
					m_Rect.Height = size.Height + 6;
				}
				m_Rect.Height += m_VerticalPadding;
				m_ImageDrawRect = Rectangle.Empty;
				if (m_ButtonStyle != eButtonStyle.TextOnlyAlways && flag)
				{
					m_ImageDrawRect = new Rectangle(0, (m_Rect.Height - size.Height) / 2, size.Width, size.Height);
				}
				m_TextDrawRect = Rectangle.Empty;
				if (m_ButtonStyle != 0 || !flag)
				{
					if (m_ImageDrawRect.Right > 0)
					{
						m_Rect.Width = size2.Width + 1;
						m_TextDrawRect = new Rectangle(m_ImageDrawRect.Right - 2, 2, m_Rect.Width, m_Rect.Height - 4);
					}
					else
					{
						m_Rect.Width = size2.Width + 6;
						m_TextDrawRect = new Rectangle(3, 2, m_Rect.Width, m_Rect.Height - 4);
					}
				}
				m_Rect.Width += m_ImageDrawRect.Right;
				m_Rect.Width += m_HorizontalPadding;
			}
			else if (m_Orientation == eOrientation.Horizontal)
			{
				if (size2.Width > size.Width)
				{
					m_Rect.Width = size2.Width + 6;
				}
				else
				{
					m_Rect.Width = size.Width + 6;
				}
				m_Rect.Height = size.Height + size2.Height + 10;
				m_Rect.Width += m_HorizontalPadding;
				m_Rect.Height += m_VerticalPadding;
				if (m_ImagePosition == eImagePosition.Top)
				{
					m_ImageDrawRect = new Rectangle(0, m_VerticalPadding / 2, m_Rect.Width, size.Height + 2);
					m_TextDrawRect = new Rectangle((m_Rect.Width - size2.Width) / 2, m_ImageDrawRect.Bottom + 2, size2.Width, size2.Height + 5);
				}
				else
				{
					m_TextDrawRect = new Rectangle((m_Rect.Width - size2.Width) / 2, m_VerticalPadding / 2, size2.Width, size2.Height + 2);
					m_ImageDrawRect = new Rectangle(0, m_TextDrawRect.Bottom + 2, m_Rect.Width, size.Height + 5);
				}
			}
			else
			{
				if (size2.Height > size.Height && m_ButtonStyle != 0)
				{
					m_Rect.Width = size2.Height + 6;
				}
				else
				{
					m_Rect.Width = size.Width + 6;
				}
				m_Rect.Width += m_HorizontalPadding;
				if (m_ButtonStyle != 0 || !flag)
				{
					if (flag)
					{
						m_Rect.Height = size.Height + size2.Width + 12;
					}
					else
					{
						m_Rect.Height = size2.Width + 6;
					}
				}
				else
				{
					m_Rect.Height = size.Height + 6;
				}
				if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Left)
				{
					if (flag)
					{
						m_ImageDrawRect = new Rectangle(0, 0, m_Rect.Width, size.Height + 5);
					}
					m_TextDrawRect = new Rectangle((m_Rect.Width - size2.Height) / 2, m_ImageDrawRect.Bottom + 2, size2.Height, size2.Width + 5);
				}
				else
				{
					m_TextDrawRect = new Rectangle((m_Rect.Width - size2.Height) / 2, 0, size2.Height, size2.Width + 5);
					if (flag)
					{
						m_ImageDrawRect = new Rectangle(0, m_TextDrawRect.Bottom + 2, m_Rect.Width, size.Height + 5);
					}
				}
				m_Rect.Height += m_VerticalPadding;
			}
			if ((SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems && (!IsOnMenuBar || GetImage() != null))
			{
				if (m_Orientation == eOrientation.Horizontal)
				{
					m_SubItemsRect = new Rectangle(m_Rect.Right, 0, 12, m_Rect.Height);
					m_Rect.Width += m_SubItemsRect.Width;
				}
				else
				{
					m_SubItemsRect = new Rectangle(0, m_Rect.Bottom, m_Rect.Width, 12);
					m_Rect.Height += m_SubItemsRect.Height;
				}
			}
		}
		val2.Dispose();
		val = null;
	}

	private void DrawOfficeCheckBox(Graphics g, Rectangle r)
	{
		ControlPaint.DrawBorder3D(g, r, (Border3DStyle)2, (Border3DSide)2063);
		if (!m_MouseOver)
		{
			r.Inflate(-1, -1);
			g.FillRectangle((Brush)(object)ColorFunctions.GetPushedBrush(), r);
		}
		Point[] array = new Point[3];
		array[0].X = r.Left + (r.Width - 6) / 2;
		array[0].Y = r.Top + (r.Height - 6) / 2 + 3;
		array[1].X = array[0].X + 2;
		array[1].Y = array[0].Y + 2;
		array[2].X = array[1].X + 4;
		array[2].Y = array[1].Y - 4;
		g.DrawLines(SystemPens.ControlText, array);
		array[0].X++;
		array[1].X++;
		array[2].X++;
		g.DrawLines(SystemPens.ControlText, array);
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		base.OnItemAdded(item);
		OnAppearanceChanged();
	}

	protected override void OnExternalSizeChange()
	{
		base.OnExternalSizeChange();
		if (!m_FixedSize.IsEmpty)
		{
			if (m_FixedSize.Width > 1)
			{
				m_Rect.Width = m_FixedSize.Width;
			}
			if (m_FixedSize.Height > 1)
			{
				m_Rect.Height = m_FixedSize.Height;
			}
		}
		ButtonItemLayout.Arrange(this);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		if (m_Expanded)
		{
			Expanded = false;
			if (m_Parent != null)
			{
				m_Parent.AutoExpand = false;
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		base.InternalKeyDown(objArg);
		if (Expanded || objArg.Handled)
		{
			return;
		}
		if ((int)objArg.KeyCode == 13 || (int)objArg.KeyCode == 13 || ((int)objArg.KeyCode == 39 && base.IsOnMenu))
		{
			if (SubItems.Count > 0 && GetEnabled())
			{
				if (Expanded)
				{
					if (m_Parent != null)
					{
						m_Parent.AutoExpand = false;
					}
					Expanded = false;
				}
				else
				{
					if (m_Parent != null)
					{
						m_Parent.AutoExpand = true;
					}
					Expanded = true;
					if (PopupType == ePopupType.Menu && base.PopupControl is MenuPanel)
					{
						((MenuPanel)(object)base.PopupControl).SelectFirstItem();
					}
				}
				objArg.Handled = true;
				return;
			}
		}
		else if ((int)objArg.KeyCode == 27 && SubItems.Count > 0 && Expanded)
		{
			Expanded = false;
			if (m_Parent != null)
			{
				m_Parent.AutoExpand = false;
			}
			objArg.Handled = true;
			return;
		}
		base.InternalKeyDown(objArg);
	}

	private void SetMouseOver(bool value)
	{
		if (value == m_MouseOver)
		{
			return;
		}
		if (m_Pulse)
		{
			if (m_StopPulseOnMouseOver)
			{
				m_Pulse = false;
			}
			StopFade();
		}
		if ((IsFadeEnabled || m_Pulse) && GetEnabled() && Displayed && Visible && WidthInternal > 0 && HeightInternal > 0)
		{
			int direction = (value ? 1 : (-1));
			int initialAlpha = (value ? 10 : 255);
			m_MouseOver = value;
			StartFade(direction, initialAlpha);
		}
		else
		{
			m_MouseOver = value;
		}
	}

	private void StartFade(int direction, int initialAlpha)
	{
		bool flag = false;
		m_FadeImageLock.AcquireReaderLock(-1);
		try
		{
			flag = m_ImageState2 == null;
		}
		finally
		{
			m_FadeImageLock.ReleaseReaderLock();
		}
		if (flag)
		{
			bool mouseOver = m_MouseOver;
			m_MouseOver = true;
			try
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
					m_ImageState2 = GetCurrentStateImage();
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
			finally
			{
				m_MouseOver = mouseOver;
			}
		}
		m_Direction = direction;
		m_Alpha = initialAlpha;
		FadeAnimator.Fade(this, OnFadeChanged);
		m_FadeAnimation = true;
	}

	internal void StopFade()
	{
		if (m_FadeAnimation)
		{
			m_FadeAnimation = false;
			FadeAnimator.StopFade(this, OnFadeChanged);
			bool flag = false;
			m_FadeImageLock.AcquireReaderLock(-1);
			try
			{
				flag = m_ImageState2 != null;
			}
			finally
			{
				m_FadeImageLock.ReleaseReaderLock();
			}
			if (flag)
			{
				DisposeFadeImage();
			}
			if (m_Alpha > 230)
			{
				m_Alpha = 255;
			}
			else if (m_Alpha < 0)
			{
				m_Alpha = 0;
			}
			Invalidate();
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
			((Image)m_ImageState2).Dispose();
			m_ImageState2 = null;
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
		m_Alpha += m_Direction * (m_Pulse ? m_PulseFadeAlphaIncrement : m_MouseOverFadeAlphaIncrement);
		if ((m_Direction < 0 && m_Alpha <= 0) || (m_Direction > 0 && m_Alpha >= 255))
		{
			if (m_Pulse)
			{
				m_Direction *= -1;
				if (m_Alpha >= 255)
				{
					m_Alpha = 255;
				}
				else
				{
					m_Alpha = 0;
				}
				m_PulseCount++;
				if (m_PulseBeats > 0 && m_PulseCount > m_PulseBeats)
				{
					StopFade();
					StopPulse();
				}
			}
			else
			{
				StopFade();
			}
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null && BarFunctions.IsHandleValid(val))
		{
			try
			{
				val.Invalidate(DisplayRectangle);
			}
			catch (ObjectDisposedException)
			{
			}
			catch (Win32Exception)
			{
			}
		}
	}

	private Bitmap GetCurrentStateImage()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = new Bitmap(WidthInternal, HeightInternal, (PixelFormat)925707);
		val.MakeTransparent();
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			object containerControl = ContainerControl;
			Control val3 = (Control)((containerControl is Control) ? containerControl : null);
			bool flag = false;
			ItemPaintArgs itemPaintArgs = null;
			if (val3 is ItemControl)
			{
				flag = ((ItemControl)(object)val3).AntiAlias;
				itemPaintArgs = ((ItemControl)(object)val3).GetItemPaintArgs(val2);
			}
			else if (val3 is Bar)
			{
				flag = ((Bar)(object)val3).AntiAlias;
				itemPaintArgs = ((Bar)(object)val3).GetItemPaintArgs(val2);
			}
			else if (val3 is ButtonX)
			{
				flag = ((ButtonX)(object)val3).AntiAlias;
				itemPaintArgs = ((ButtonX)(object)val3).GetItemPaintArgs(val2);
			}
			Matrix val4 = new Matrix();
			val4.Translate((float)(-DisplayRectangle.X), (float)(-DisplayRectangle.Y), (MatrixOrder)1);
			val2.Transform = val4;
			val4.Dispose();
			val4 = null;
			if (flag)
			{
				val2.SmoothingMode = (SmoothingMode)4;
				val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			if (itemPaintArgs == null)
			{
				((Image)val).Dispose();
				return null;
			}
			Paint(itemPaintArgs);
			return val;
		}
		finally
		{
			val2.Dispose();
		}
	}

	public void Pulse()
	{
		Pulse(0);
	}

	public void Pulse(int pulseBeatCount)
	{
		if (DesignMode || m_HotTrackingStyle == eHotTrackingStyle.None || !IsPulseEnabed)
		{
			return;
		}
		m_PulseBeats = pulseBeatCount;
		m_PulseCount = 0;
		if (!m_Pulse)
		{
			m_Pulse = true;
			if (!m_MouseOver)
			{
				StartFade(1, 0);
			}
		}
	}

	public void StopPulse()
	{
		if (m_Pulse)
		{
			m_Pulse = false;
			if (m_MouseOver)
			{
				SetMouseOver(value: false);
			}
			StopFade();
		}
	}

	protected override void OnEnabledChanged()
	{
		if (!GetEnabled())
		{
			SetMouseOver(value: false);
			SetMouseDown(mouseDown: false);
		}
		base.OnEnabledChanged();
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		if (!DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			return;
		}
		bool flag = false;
		Rectangle totalSubItemsRect = GetTotalSubItemsRect();
		if (!totalSubItemsRect.IsEmpty && GetEnabled())
		{
			totalSubItemsRect.Offset(DisplayRectangle.Location);
			if (totalSubItemsRect.Contains(objArg.X, objArg.Y))
			{
				if (!m_MouseOverExpand)
				{
					m_MouseOverExpand = true;
					flag = true;
				}
			}
			else if (m_MouseOverExpand)
			{
				m_MouseOverExpand = false;
				flag = true;
			}
		}
		if (!m_MouseOver)
		{
			SetMouseOver(value: true);
			if (GetEnabled() || base.IsOnMenu)
			{
				flag = true;
			}
		}
		if (flag)
		{
			Refresh();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		m_MouseOverExpand = false;
		SetMouseOver(value: false);
		m_MouseDown = false;
		if (GetEnabled() || base.IsOnMenu)
		{
			Refresh();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseHover()
	{
		base.InternalMouseHover();
		if ((SubItems.Count > 0 || (PopupType == ePopupType.Container && !base.IsOnCustomizeMenu)) && base.IsOnMenu && ShowSubItems && AutoExpandMenuItem && !Expanded && GetEnabled())
		{
			Expanded = true;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		base.InternalMouseDown(objArg);
		ButtonMouseDown(objArg);
	}

	protected virtual void ButtonMouseDown(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button != 1048576 || !GetEnabled())
		{
			return;
		}
		m_MouseDown = true;
		if ((IsOnMenuBar || ShouldAutoExpandOnClick) && !DesignMode)
		{
			if (objArg.Clicks == 2)
			{
				return;
			}
			if (CanShowPopup)
			{
				if (Expanded)
				{
					Expanded = false;
					if (m_Parent != null)
					{
						m_Parent.AutoExpand = false;
					}
				}
				else
				{
					if (IsOnMenuBar && m_Parent != null)
					{
						m_Parent.AutoExpand = true;
					}
					Expanded = true;
				}
			}
			else
			{
				Refresh();
			}
		}
		else if ((IsOnMenuBar || ShouldAutoExpandOnClick) && DesignMode)
		{
			Expanded = !Expanded;
		}
		else if (!base.IsOnMenu)
		{
			if ((SubItems.Count > 0 || (PopupType == ePopupType.Container && !base.IsOnCustomizeMenu)) && ShowSubItems)
			{
				if (!m_Expanded)
				{
					Rectangle totalSubItemsRect = GetTotalSubItemsRect();
					totalSubItemsRect.Width += 2;
					totalSubItemsRect.Height += 2;
					totalSubItemsRect.Offset(m_Rect.X, m_Rect.Y);
					if (totalSubItemsRect.Contains(objArg.X, objArg.Y))
					{
						if (EffectiveStyle == eDotNetBarStyle.Office2000)
						{
							m_MouseDown = false;
						}
						Expanded = true;
					}
				}
				else
				{
					Expanded = false;
				}
			}
			Refresh();
		}
		else if ((SubItems.Count > 0 || (PopupType == ePopupType.Container && !base.IsOnCustomizeMenu)) && ShowSubItems)
		{
			if (!base.IsOnMenu || !Expanded)
			{
				Expanded = !m_Expanded;
			}
		}
		else
		{
			Refresh();
		}
	}

	internal override void DoAccesibleDefaultAction()
	{
		if (base.VisibleSubItems > 0 && (base.IsOnMenu || IsOnMenuBar || _AccessibleExpandAction || ShouldAutoExpandOnClick))
		{
			if (Expanded)
			{
				Expanded = false;
				if (Parent != null && IsOnMenuBar)
				{
					Parent.AutoExpand = false;
				}
			}
			else
			{
				if (IsOnMenuBar && Parent != null)
				{
					Parent.AutoExpand = true;
				}
				Expanded = true;
			}
			Refresh();
			_AccessibleExpandAction = false;
		}
		else
		{
			RaiseClick(eEventSource.Keyboard);
		}
	}

	internal bool GetShouldAutoExpandOnClick()
	{
		return ShouldAutoExpandOnClick;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		bool flag = true;
		Rectangle totalSubItemsRect = GetTotalSubItemsRect();
		totalSubItemsRect.Width += 2;
		totalSubItemsRect.Height += 2;
		totalSubItemsRect.Offset(m_Rect.X, m_Rect.Y);
		if ((int)objArg.Button == 1048576)
		{
			if (GetEnabled() && !DesignMode && m_MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !m_RecentlyUsed && base.IsOnMenu)
			{
				m_RecentlyUsed = true;
				for (BaseItem parent = Parent; parent != null; parent = parent.Parent)
				{
					if (parent is IPersonalizedMenuItem personalizedMenuItem)
					{
						personalizedMenuItem.RecentlyUsed = true;
					}
				}
			}
			if (!base.IsOnMenu && (SubItems.Count > 0 || PopupType == ePopupType.Container) && ShowSubItems && m_HotSubItem == null && !DesignMode && !IsOnMenuBar && !ShouldAutoExpandOnClick)
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val == null)
				{
					return;
				}
				val = null;
				if (!totalSubItemsRect.Contains(objArg.X, objArg.Y))
				{
					bool flag2 = true;
					if (PopupType == ePopupType.ToolBar || PopupType == ePopupType.Menu)
					{
						bool expanded = Expanded;
						base.InternalMouseUp(objArg);
						if (!expanded && Expanded)
						{
							flag2 = false;
						}
						flag = false;
					}
					if (flag2)
					{
						BaseItem.CollapseAll(this);
					}
				}
			}
		}
		if ((!base.IsOnMenu || Parent is ItemContainer) && (SubItems.Count > 0 || (PopupType == ePopupType.Container && !base.IsOnCustomizeMenu)) && ShowSubItems && totalSubItemsRect.Contains(objArg.X, objArg.Y))
		{
			flag = false;
		}
		if (flag)
		{
			base.InternalMouseUp(objArg);
		}
		if (m_MouseDown)
		{
			m_MouseDown = false;
			if (!base.IsOnMenu || !(Parent is ButtonItem))
			{
				Refresh();
			}
		}
	}

	internal void SetMouseDown(bool mouseDown)
	{
		m_MouseDown = mouseDown;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		base.InternalDoubleClick(mb, mpos);
		if (Expanded && IsOnMenuBar && m_Parent != null && !DesignMode && PopupType == ePopupType.Menu && (PersonalizedMenus == ePersonalizedMenus.Both || PersonalizedMenus == ePersonalizedMenus.DisplayOnClick) && base.PopupControl is MenuPanel)
		{
			((MenuPanel)(object)base.PopupControl).ExpandRecentlyUsed();
		}
	}

	private void CreateDisabledImage()
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Invalid comparison between Unknown and I4
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Invalid comparison between Unknown and I4
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Invalid comparison between Unknown and I4
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		if (m_Image == null && m_ImageIndex < 0 && m_Icon == null)
		{
			return;
		}
		if (m_DisabledImage != null)
		{
			m_DisabledImage.Dispose();
		}
		m_DisabledImage = null;
		if (m_DisabledIcon != null)
		{
			m_DisabledIcon.Dispose();
		}
		m_DisabledIcon = null;
		CompositeImage image = GetImage(ImageState.Default, Color.Black);
		if (image == null)
		{
			return;
		}
		if (GetOwner() is IOwner && ((IOwner)GetOwner()).DisabledImagesGrayScale)
		{
			if (image.IsIcon)
			{
				m_DisabledIcon = BarFunctions.CreateDisabledIcon(image.Icon);
			}
			else
			{
				ref Image disabledImage = ref m_DisabledImage;
				Image image2 = image.Image;
				disabledImage = (Image)(object)ImageHelper.CreateGrayScaleImage((image2 is Bitmap) ? image2 : null);
			}
		}
		if (m_DisabledIcon == null && m_DisabledImage == null)
		{
			PixelFormat val = (PixelFormat)2498570;
			if (!image.IsIcon && image.Image != null)
			{
				val = image.Image.PixelFormat;
			}
			if ((int)val == 196865 || (int)val == 197634 || (int)val == 198659)
			{
				val = (PixelFormat)2498570;
			}
			Bitmap val2 = new Bitmap(image.Width, image.Height, val);
			m_DisabledImage = (Image)new Bitmap(image.Width, image.Height, val);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			SolidBrush val4 = new SolidBrush(Color.White);
			try
			{
				val3.FillRectangle((Brush)(object)val4, 0, 0, image.Width, image.Height);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			image.DrawImage(val3, new Rectangle(0, 0, image.Width, image.Height));
			val3.Dispose();
			val3 = Graphics.FromImage(m_DisabledImage);
			val2.MakeTransparent(Color.White);
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if ((effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)) && NativeFunctions.ColorDepth >= 8)
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5],
					new float[5],
					new float[5],
					new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f },
					new float[5]
				});
				ImageAttributes val5 = new ImageAttributes();
				val5.ClearColorKey();
				val5.SetColorMatrix(colorMatrix);
				val3.DrawImage((Image)(object)val2, new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height), 0, 0, ((Image)val2).Width, ((Image)val2).Height, (GraphicsUnit)2, val5);
			}
			else
			{
				ControlPaint.DrawImageDisabled(val3, (Image)(object)val2, 0, 0, ColorFunctions.MenuBackColor(val3));
			}
			val3.Dispose();
			val3 = null;
			((Image)val2).Dispose();
			val2 = null;
			image.Dispose();
		}
	}

	private eTextFormat GetStringFormat()
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		eTextFormat2 = (_FitContainer ? (eTextFormat2 | eTextFormat.WordBreak) : (eTextFormat2 | eTextFormat.SingleLine));
		return eTextFormat2 | eTextFormat.VerticalCenter;
	}

	private ThemeTextFormat GetThemeTextFormat()
	{
		return (ThemeTextFormat)1310724;
	}

	internal virtual Font GetFont(ItemPaintArgs pa, bool isLayout)
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		Font val = null;
		if (pa != null)
		{
			val = pa.Font;
		}
		if (val == null)
		{
			Control val2 = null;
			if (pa != null)
			{
				val2 = pa.ContainerControl;
			}
			if (val2 == null)
			{
				object containerControl = ContainerControl;
				val2 = (Control)((containerControl is Control) ? containerControl : null);
			}
			if (val2 != null)
			{
				if (val2.Font != null)
				{
					val = val2.Font;
				}
			}
			else
			{
				val = SystemFonts.DefaultFont;
			}
		}
		if (m_MouseOver || (isLayout && (m_HotFontBold || m_HotFontUnderline)))
		{
			if (_MouseOverFont != null && val != null && val.FontFamily.Name == _MouseOverFont.FontFamily.Name && val.SizeInPoints == _MouseOverFont.SizeInPoints)
			{
				return _MouseOverFont;
			}
			try
			{
				Font val3 = null;
				FontStyle val4 = val.Style;
				if (m_HotFontBold)
				{
					val4 = (FontStyle)(val4 | 1);
				}
				if (m_HotFontUnderline)
				{
					val4 = (FontStyle)(val4 | 4);
				}
				val3 = new Font(val, val4);
				DisposeMouseOverFont();
				_MouseOverFont = val3;
				return _MouseOverFont;
			}
			catch
			{
			}
		}
		if (m_FontBold || m_FontItalic || m_FontUnderline)
		{
			FontStyle val5 = val.Style;
			if (m_FontBold)
			{
				val5 = (FontStyle)(val5 | 1);
			}
			if (m_FontItalic)
			{
				val5 = (FontStyle)(val5 | 2);
			}
			if (m_FontUnderline)
			{
				val5 = (FontStyle)(val5 | 4);
			}
			Font val6 = null;
			try
			{
				val6 = new Font(val, val5);
			}
			catch
			{
			}
			if (val6 != null)
			{
				val = val6;
			}
		}
		return val;
	}

	private void DisposeMouseOverFont()
	{
		if (_MouseOverFont != null)
		{
			_MouseOverFont.Dispose();
			_MouseOverFont = null;
		}
	}

	protected virtual void OnCheckedChanged()
	{
		ButtonItem oldItem = null;
		if (m_OptionGroup != "" && m_Checked && Parent != null)
		{
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this && subItem is ButtonItem buttonItem && buttonItem.OptionGroup == m_OptionGroup && buttonItem.Checked)
				{
					oldItem = buttonItem;
					buttonItem.Checked = false;
				}
			}
		}
		InvokeCheckedChanged(new CheckedChangedEventArgs(oldItem));
	}

	protected internal override void OnVisibleChanged(bool bVisible)
	{
		base.OnVisibleChanged(bVisible);
		if (!bVisible)
		{
			StopFade();
		}
		if (!bVisible && Checked && m_OptionGroup != "")
		{
			Checked = false;
			if (Parent == null)
			{
				return;
			}
			{
				foreach (BaseItem subItem in Parent.SubItems)
				{
					if (subItem != this && subItem.GetEnabled() && subItem.Visible && subItem is ButtonItem buttonItem && buttonItem.OptionGroup == m_OptionGroup)
					{
						buttonItem.Checked = true;
						break;
					}
				}
				return;
			}
		}
		if (!bVisible || !(m_OptionGroup != "") || base.VisibleSubItems > 1)
		{
			return;
		}
		bool flag = true;
		if (Parent != null)
		{
			foreach (BaseItem subItem2 in Parent.SubItems)
			{
				if (subItem2 != this && subItem2.GetEnabled() && subItem2.Visible && subItem2 is ButtonItem buttonItem2 && buttonItem2.OptionGroup == m_OptionGroup && buttonItem2.Checked)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			Checked = true;
		}
	}

	internal void SetChecked(bool b)
	{
		m_Checked = b;
	}

	protected virtual void InvokeCheckedChanged(CheckedChangedEventArgs e)
	{
		if (this.CheckedChanged != null)
		{
			this.CheckedChanged(this, e);
		}
		GetIOwnerItemEvents()?.InvokeCheckedChanged(this, e);
	}

	protected virtual void InvokeOptionGroupChanging(OptionGroupChangingEventArgs e)
	{
		if (this.OptionGroupChanging != null)
		{
			this.OptionGroupChanging(this, e);
		}
		if (!e.Cancel)
		{
			GetIOwnerItemEvents()?.InvokeOptionGroupChanging(this, e);
		}
	}

	protected override void OnClick()
	{
		if (m_AutoCheckOnClick && m_OptionGroup == "")
		{
			Checked = !Checked;
		}
		base.OnClick();
		if (m_OptionGroup != "" && !m_Checked)
		{
			Checked = true;
		}
		ExecuteCommand();
	}

	protected override void OnCommandChanged()
	{
		if (!DesignMode && Command == null)
		{
			Enabled = false;
		}
		base.OnCommandChanged();
	}

	protected override string GetTooltipShortcutString()
	{
		return DrawShortcutText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeForeColor()
	{
		if (m_ForeColor.IsEmpty)
		{
			return false;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeHotForeColor()
	{
		if (m_HotForeColor.IsEmpty)
		{
			return false;
		}
		return true;
	}

	internal Rectangle GetTotalSubItemsRect()
	{
		Rectangle subItemsRect = SubItemsRect;
		if (SplitButton && (ButtonStyle == eButtonStyle.ImageAndText || ImagePosition == eImagePosition.Top || ImagePosition == eImagePosition.Bottom) && (Image != null || ImageIndex >= 0) && Text.Length > 0)
		{
			Rectangle textDrawRect = TextDrawRect;
			textDrawRect.Inflate(0, -2);
			if (ImagePosition == eImagePosition.Left)
			{
				if (Orientation == eOrientation.Horizontal)
				{
					textDrawRect.X -= 3;
				}
				else
				{
					textDrawRect.Y -= 6;
				}
			}
			if (subItemsRect.IsEmpty)
			{
				subItemsRect = textDrawRect;
				if (m_ImagePosition == eImagePosition.Top)
				{
					subItemsRect.X = 0;
					subItemsRect.Width = m_Rect.Width;
					subItemsRect.Height = DisplayRectangle.Bottom - subItemsRect.Y;
				}
				else if (m_ImagePosition == eImagePosition.Bottom)
				{
					subItemsRect.X = 0;
					subItemsRect.Width = m_Rect.Width;
				}
			}
			else
			{
				subItemsRect = Rectangle.Union(subItemsRect, textDrawRect);
			}
			return subItemsRect;
		}
		return subItemsRect;
	}

	protected override void OnDisplayedChanged()
	{
		m_MouseDown = false;
		m_MouseOver = false;
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
}
