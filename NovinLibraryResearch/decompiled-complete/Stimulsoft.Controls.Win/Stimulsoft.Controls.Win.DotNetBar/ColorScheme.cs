using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ColorScheme
{
	private Color m_MenuBarBackground;

	private bool m_MenuBarBackgroundCustom;

	private Color m_MenuBarBackground2 = Color.Empty;

	private bool m_MenuBarBackground2Custom;

	private int m_MenuBarBackgroundGradientAngle = 90;

	private Color m_BarBackground;

	private bool m_BarBackgroundCustom;

	private Color m_BarBackground2 = Color.Empty;

	private bool m_BarBackground2Custom;

	private int m_BarBackgroundGradientAngle = 90;

	private Color m_BarCaptionBackground;

	private bool m_BarCaptionBackgroundCustom;

	private Color m_BarCaptionBackground2 = Color.Empty;

	private bool m_BarCaptionBackground2Custom;

	private int m_BarCaptionBackgroundGradientAngle = 90;

	private Color m_BarCaptionText;

	private bool m_BarCaptionTextCustom;

	private Color m_BarCaptionInactiveBackground;

	private bool m_BarCaptionInactiveBackgroundCustom;

	private Color m_BarCaptionInactiveBackground2 = Color.Empty;

	private bool m_BarCaptionInactiveBackground2Custom;

	private int m_BarCaptionInactiveBackgroundGAngle = 90;

	private Color m_BarCaptionInactiveText;

	private bool m_BarCaptionInactiveTextCustom;

	private Color m_BarPopupBackground;

	private bool m_BarPopupBackgroundCustom;

	private Color m_BarPopupBorder;

	private bool m_BarPopupBorderCustom;

	private Color m_BarDockedBorder;

	private bool m_BarDockedBorderCustom;

	private Color m_BarStripeColor;

	private bool m_BarStripeColorCustom;

	private Color m_BarFloatingBorder;

	private bool m_BarFloatingBorderCustom;

	private Color m_ItemBackground;

	private bool m_ItemBackgroundCustom;

	private Color m_ItemBackground2;

	private bool m_ItemBackground2Custom;

	private int m_ItemBackgroundGradientAngle = 90;

	private Color m_ItemText;

	private bool m_ItemTextCustom;

	private Color m_ItemDisabledBackground;

	private bool m_ItemDisabledBackgroundCustom;

	private Color m_ItemDisabledText;

	private bool m_ItemDisabledTextCustom;

	private Color m_ItemHotBackground;

	private bool m_ItemHotBackgroundCustom;

	private Color m_ItemHotBackground2 = Color.Empty;

	private bool m_ItemHotBackground2Custom;

	private int m_ItemHotBackgroundGradientAngle = 90;

	private Color m_ItemHotText;

	private bool m_ItemHotTextCustom;

	private Color m_ItemHotBorder;

	private bool m_ItemHotBorderCustom;

	private Color m_ItemPressedBackground;

	private bool m_ItemPressedBackgroundCustom;

	private Color m_ItemPressedBackground2 = Color.Empty;

	private bool m_ItemPressedBackground2Custom;

	private int m_ItemPressedBackgroundGradientAngle = 90;

	private Color m_ItemPressedText;

	private bool m_ItemPressedTextCustom;

	private Color m_ItemPressedBorder;

	private bool m_ItemPressedBorderCustom;

	private Color m_ItemSeparator;

	private bool m_ItemSeparatorCustom;

	private Color m_ItemSeparatorShade = Color.Empty;

	private bool m_ItemSeparatorShadeCustom;

	private Color m_ItemExpandedBackground;

	private bool m_ItemExpandedBackgroundCustom;

	private Color m_ItemExpandedBackground2 = Color.Empty;

	private bool m_ItemExpandedBackground2Custom;

	private int m_ItemExpandedBackgroundGradientAngle = 90;

	private Color m_ItemExpandedText;

	private bool m_ItemExpandedTextCustom;

	private Color m_ItemExpandedShadow;

	private bool m_ItemExpandedShadowCustom;

	private Color m_ItemExpandedBorder;

	private bool m_ItemExpandedBorderCustom;

	private Color m_ItemCheckedBackground;

	private bool m_ItemCheckedBackgroundCustom;

	private int m_ItemCheckedBackgroundGradientAngle = 90;

	private Color m_ItemCheckedBackground2 = Color.Empty;

	private bool m_ItemCheckedBackground2Custom;

	private Color m_ItemCheckedBorder;

	private bool m_ItemCheckedBorderCustom;

	private Color m_ItemCheckedText;

	private bool m_ItemCheckedTextCustom;

	private Color m_MenuBorder;

	private bool m_MenuBorderCustom;

	private Color m_MenuBackground;

	private bool m_MenuBackgroundCustom;

	private Color m_MenuBackground2 = Color.Empty;

	private bool m_MenuBackground2Custom;

	private int m_MenuBackgroundGradientAngle;

	private Color m_MenuSide;

	private bool m_MenuSideCustom;

	private Color m_MenuSide2 = Color.Empty;

	private bool m_MenuSide2Custom;

	private int m_MenuSideGradientAngle;

	private Color m_MenuUnusedBackground;

	private bool m_MenuUnusedBackgroundCustom;

	private Color m_MenuUnusedSide;

	private bool m_MenuUnusedSideCustom;

	private Color m_MenuUnusedSide2 = Color.Empty;

	private bool m_MenuUnusedSide2Custom;

	private int m_MenuUnusedSideGradientAngle;

	private Color m_ItemDesignTimeBorder;

	private bool m_ItemDesignTimeBorderCustom;

	private Color m_CustomizeBackground = Color.Empty;

	private bool m_CustomizeBackgroundCustom;

	private Color m_CustomizeBackground2 = Color.Empty;

	private bool m_CustomizeBackground2Custom;

	private int m_CustomizeBackgroundGradientAngle = 90;

	private Color m_CustomizeText = Color.Empty;

	private bool m_CustomizeTextCustom;

	private Color m_PanelBackground = Color.Empty;

	private bool m_PanelBackgroundCustom;

	private Color m_PanelBackground2 = Color.Empty;

	private bool m_PanelBackground2Custom;

	private int m_PanelBackgroundGradientAngle = 90;

	private Color m_PanelText = Color.Empty;

	private bool m_PanelTextCustom;

	private Color m_PanelBorder = Color.Empty;

	private bool m_PanelBorderCustom;

	private Color m_SplitterBackground = Color.Empty;

	private bool m_SplitterBackgroundCustom;

	private Color m_SplitterBackground2 = Color.Empty;

	private bool m_SplitterBackground2Custom;

	private int m_SplitterBackgroundGradientAngle = 90;

	private Color m_SplitterText = Color.Empty;

	private bool m_SplitterTextCustom;

	private Color m_SplitterBorder = Color.Empty;

	private bool m_SplitterBorderCustom;

	private Color m_ExplorerBarBackground = Color.Empty;

	private bool m_ExplorerBarBackgroundCustom;

	private Color m_ExplorerBarBackground2 = Color.Empty;

	private bool m_ExplorerBarBackground2Custom;

	private int m_ExplorerBarBackgroundGradientAngle = 90;

	internal bool _DesignTimeSchemeChanged;

	private eDotNetBarStyle m_Style;

	private Color m_DockSiteBackColor = Color.Empty;

	private bool m_DockSiteBackColorCustom;

	private Color m_DockSiteBackColor2 = Color.Empty;

	private bool m_DockSiteBackColor2Custom;

	private int m_DockSiteBackColorGradientAngle;

	private ePredefinedColorScheme m_PredefinedColorScheme;

	private Color m_MdiSystemItemForeground = Color.Empty;

	private Image _AutoHidePanelBackgroundImage;

	private Color _AutoHidePanelBackground = Color.Empty;

	private Color _AutoHidePanelBackground2 = Color.Empty;

	private Color _AutoHideTabBackground = Color.Empty;

	private Color _AutoHideTabBackground2 = Color.Empty;

	private int _AutoHideTabBackgroundGradientAngle = 90;

	private Color _AutoHideTabText = Color.Empty;

	private Color _AutoHideTabBorder = Color.Empty;

	private Color _AutoHideSelectedTabBackground = Color.Empty;

	private Color _AutoHideSelectedTabBackground2 = Color.Empty;

	private Color _AutoHideSelectedTabText = Color.Empty;

	private Color _AutoHideSelectedTabBorder = Color.Empty;

	internal eDotNetBarStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			if (m_Style != value)
			{
				m_Style = value;
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies starting color of dock site.")]
	public Color DockSiteBackColor
	{
		get
		{
			return m_DockSiteBackColor;
		}
		set
		{
			if (m_DockSiteBackColor != value)
			{
				m_DockSiteBackColor = value;
				m_DockSiteBackColorCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies ending gradient color of dock site.")]
	public Color DockSiteBackColor2
	{
		get
		{
			return m_DockSiteBackColor2;
		}
		set
		{
			if (m_DockSiteBackColor2 != value)
			{
				m_DockSiteBackColor2 = value;
				m_DockSiteBackColor2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(0)]
	public int DockSiteBackColorGradientAngle
	{
		get
		{
			return m_DockSiteBackColorGradientAngle;
		}
		set
		{
			m_DockSiteBackColorGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the menu bar background color.")]
	public Color MenuBarBackground
	{
		get
		{
			return m_MenuBarBackground;
		}
		set
		{
			if (m_MenuBarBackground != value)
			{
				m_MenuBarBackground = value;
				m_MenuBarBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the target menu bar gradient background color.")]
	public Color MenuBarBackground2
	{
		get
		{
			return m_MenuBarBackground2;
		}
		set
		{
			if (m_MenuBarBackground2 != value)
			{
				m_MenuBarBackground2 = value;
				m_MenuBarBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int MenuBarBackgroundGradientAngle
	{
		get
		{
			return m_MenuBarBackgroundGradientAngle;
		}
		set
		{
			m_MenuBarBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the background color for the bar when floating or when docked.")]
	public Color BarBackground
	{
		get
		{
			return m_BarBackground;
		}
		set
		{
			if (m_BarBackground != value)
			{
				m_BarBackground = value;
				m_BarBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the target gradient background color for the bar when floating or when docked.")]
	public Color BarBackground2
	{
		get
		{
			return m_BarBackground2;
		}
		set
		{
			if (m_BarBackground2 != value)
			{
				m_BarBackground2 = value;
				m_BarBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int BarBackgroundGradientAngle
	{
		get
		{
			return m_BarBackgroundGradientAngle;
		}
		set
		{
			m_BarBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the background color for the bar Caption.")]
	public Color BarCaptionBackground
	{
		get
		{
			return m_BarCaptionBackground;
		}
		set
		{
			if (m_BarCaptionBackground != value)
			{
				m_BarCaptionBackground = value;
				m_BarCaptionBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the target bar gradient background color for the bar Caption.")]
	public Color BarCaptionBackground2
	{
		get
		{
			return m_BarCaptionBackground2;
		}
		set
		{
			if (m_BarCaptionBackground2 != value)
			{
				m_BarCaptionBackground2 = value;
				m_BarCaptionBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int BarCaptionBackgroundGradientAngle
	{
		get
		{
			return m_BarCaptionBackgroundGradientAngle;
		}
		set
		{
			m_BarCaptionBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the color for text of the Caption.")]
	public Color BarCaptionText
	{
		get
		{
			return m_BarCaptionText;
		}
		set
		{
			if (m_BarCaptionText != value)
			{
				m_BarCaptionText = value;
				m_BarCaptionTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the Bar Caption inactive (lost focus) background color.")]
	public Color BarCaptionInactiveBackground
	{
		get
		{
			return m_BarCaptionInactiveBackground;
		}
		set
		{
			if (m_BarCaptionInactiveBackground != value)
			{
				m_BarCaptionInactiveBackground = value;
				m_BarCaptionInactiveBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the target background gradient Bar Caption inactive (lost focus) color.")]
	public Color BarCaptionInactiveBackground2
	{
		get
		{
			return m_BarCaptionInactiveBackground2;
		}
		set
		{
			if (m_BarCaptionInactiveBackground2 != value)
			{
				m_BarCaptionInactiveBackground2 = value;
				m_BarCaptionInactiveBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	internal int BarCaptionInactiveBackgroundGAngle
	{
		get
		{
			return m_BarCaptionInactiveBackgroundGAngle;
		}
		set
		{
			m_BarCaptionInactiveBackgroundGAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the Bar inactive (lost focus) text color.")]
	public Color BarCaptionInactiveText
	{
		get
		{
			return m_BarCaptionInactiveText;
		}
		set
		{
			if (m_BarCaptionInactiveText != value)
			{
				m_BarCaptionInactiveText = value;
				m_BarCaptionInactiveTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the background color for popup bars.")]
	public Color BarPopupBackground
	{
		get
		{
			return m_BarPopupBackground;
		}
		set
		{
			if (m_BarPopupBackground != value)
			{
				m_BarPopupBackground = value;
				m_BarPopupBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the border color for popup bars.")]
	public Color BarPopupBorder
	{
		get
		{
			return m_BarPopupBorder;
		}
		set
		{
			if (m_BarPopupBorder != value)
			{
				m_BarPopupBorder = value;
				m_BarPopupBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the border color for docked bars.")]
	public Color BarDockedBorder
	{
		get
		{
			return m_BarDockedBorder;
		}
		set
		{
			if (m_BarDockedBorder != value)
			{
				m_BarDockedBorder = value;
				m_BarDockedBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the color of the grab handle stripes.")]
	public Color BarStripeColor
	{
		get
		{
			return m_BarStripeColor;
		}
		set
		{
			if (m_BarStripeColor != value)
			{
				m_BarStripeColor = value;
				m_BarStripeColorCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Description("Specifies the border color for floating bars.")]
	public Color BarFloatingBorder
	{
		get
		{
			return m_BarFloatingBorder;
		}
		set
		{
			if (m_BarFloatingBorder != value)
			{
				m_BarFloatingBorder = value;
				m_BarFloatingBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the item background color.")]
	public Color ItemBackground
	{
		get
		{
			return m_ItemBackground;
		}
		set
		{
			if (m_ItemBackground != value)
			{
				m_ItemBackground = value;
				m_ItemBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the target item background gradient color.")]
	public Color ItemBackground2
	{
		get
		{
			return m_ItemBackground2;
		}
		set
		{
			if (m_ItemBackground2 != value)
			{
				m_ItemBackground2 = value;
				m_ItemBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int ItemBackgroundGradientAngle
	{
		get
		{
			return m_ItemBackgroundGradientAngle;
		}
		set
		{
			m_ItemBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the item text color.")]
	public Color ItemText
	{
		get
		{
			return m_ItemText;
		}
		set
		{
			if (m_ItemText != value)
			{
				m_ItemText = value;
				m_ItemTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the background color for the item that is disabled.")]
	public Color ItemDisabledBackground
	{
		get
		{
			return m_ItemDisabledBackground;
		}
		set
		{
			if (m_ItemDisabledBackground != value)
			{
				m_ItemDisabledBackground = value;
				m_ItemDisabledBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the text color for the item that is disabled.")]
	public Color ItemDisabledText
	{
		get
		{
			return m_ItemDisabledText;
		}
		set
		{
			if (m_ItemDisabledText != value)
			{
				m_ItemDisabledText = value;
				m_ItemDisabledTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the background color when mouse is over the item.")]
	public Color ItemHotBackground
	{
		get
		{
			return m_ItemHotBackground;
		}
		set
		{
			if (m_ItemHotBackground != value)
			{
				m_ItemHotBackground = value;
				m_ItemHotBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the target gradient background color when mouse is over the item.")]
	public Color ItemHotBackground2
	{
		get
		{
			return m_ItemHotBackground2;
		}
		set
		{
			if (m_ItemHotBackground2 != value)
			{
				m_ItemHotBackground2 = value;
				m_ItemHotBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int ItemHotBackgroundGradientAngle
	{
		get
		{
			return m_ItemHotBackgroundGradientAngle;
		}
		set
		{
			m_ItemHotBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the text color when mouse is over the item.")]
	public Color ItemHotText
	{
		get
		{
			return m_ItemHotText;
		}
		set
		{
			if (m_ItemHotText != value)
			{
				m_ItemHotText = value;
				m_ItemHotTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the border color when mouse is over the item.")]
	public Color ItemHotBorder
	{
		get
		{
			return m_ItemHotBorder;
		}
		set
		{
			if (m_ItemHotBorder != value)
			{
				m_ItemHotBorder = value;
				m_ItemHotBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the background color when item is pressed.")]
	public Color ItemPressedBackground
	{
		get
		{
			return m_ItemPressedBackground;
		}
		set
		{
			if (m_ItemPressedBackground != value)
			{
				m_ItemPressedBackground = value;
				m_ItemPressedBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the target gradient background color when item is pressed.")]
	public Color ItemPressedBackground2
	{
		get
		{
			return m_ItemPressedBackground2;
		}
		set
		{
			if (m_ItemPressedBackground2 != value)
			{
				m_ItemPressedBackground2 = value;
				m_ItemPressedBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int ItemPressedBackgroundGradientAngle
	{
		get
		{
			return m_ItemPressedBackgroundGradientAngle;
		}
		set
		{
			m_ItemPressedBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the text color when item is pressed.")]
	public Color ItemPressedText
	{
		get
		{
			return m_ItemPressedText;
		}
		set
		{
			if (m_ItemPressedText != value)
			{
				m_ItemPressedText = value;
				m_ItemPressedTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the border color when item is pressed.")]
	public Color ItemPressedBorder
	{
		get
		{
			return m_ItemPressedBorder;
		}
		set
		{
			if (m_ItemPressedBorder != value)
			{
				m_ItemPressedBorder = value;
				m_ItemPressedBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the color for the item group separator.")]
	public Color ItemSeparator
	{
		get
		{
			return m_ItemSeparator;
		}
		set
		{
			if (m_ItemSeparator != value)
			{
				m_ItemSeparator = value;
				m_ItemSeparatorCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the color for the item group separator shade.")]
	public Color ItemSeparatorShade
	{
		get
		{
			return m_ItemSeparatorShade;
		}
		set
		{
			if (m_ItemSeparatorShade != value)
			{
				m_ItemSeparatorShade = value;
				m_ItemSeparatorShadeCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the background color for the shadow of expanded item.")]
	public Color ItemExpandedShadow
	{
		get
		{
			return m_ItemExpandedShadow;
		}
		set
		{
			if (m_ItemExpandedShadow != value)
			{
				m_ItemExpandedShadow = value;
				m_ItemExpandedShadowCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the background color for the expanded item.")]
	public Color ItemExpandedBackground
	{
		get
		{
			return m_ItemExpandedBackground;
		}
		set
		{
			if (m_ItemExpandedBackground != value)
			{
				m_ItemExpandedBackground = value;
				m_ItemExpandedBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the target gradient background color for the expanded item.")]
	public Color ItemExpandedBackground2
	{
		get
		{
			return m_ItemExpandedBackground2;
		}
		set
		{
			if (m_ItemExpandedBackground2 != value)
			{
				m_ItemExpandedBackground2 = value;
				m_ItemExpandedBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int ItemExpandedBackgroundGradientAngle
	{
		get
		{
			return m_ItemExpandedBackgroundGradientAngle;
		}
		set
		{
			m_ItemExpandedBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the text color for the expanded item.")]
	public Color ItemExpandedText
	{
		get
		{
			return m_ItemExpandedText;
		}
		set
		{
			if (m_ItemExpandedText != value)
			{
				m_ItemExpandedText = value;
				m_ItemExpandedTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the border color for the expanded item.")]
	public Color ItemExpandedBorder
	{
		get
		{
			return m_ItemExpandedBorder;
		}
		set
		{
			if (m_ItemExpandedBorder != value)
			{
				m_ItemExpandedBorder = value;
				m_ItemExpandedBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the background color for the checked item.")]
	public Color ItemCheckedBackground
	{
		get
		{
			return m_ItemCheckedBackground;
		}
		set
		{
			if (m_ItemCheckedBackground != value)
			{
				m_ItemCheckedBackground = value;
				m_ItemCheckedBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the target gradient background color for the checked item.")]
	public Color ItemCheckedBackground2
	{
		get
		{
			return m_ItemCheckedBackground2;
		}
		set
		{
			if (m_ItemCheckedBackground2 != value)
			{
				m_ItemCheckedBackground2 = value;
				m_ItemCheckedBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int ItemCheckedBackgroundGradientAngle
	{
		get
		{
			return m_ItemCheckedBackgroundGradientAngle;
		}
		set
		{
			m_ItemCheckedBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the border color for the checked item.")]
	public Color ItemCheckedBorder
	{
		get
		{
			return m_ItemCheckedBorder;
		}
		set
		{
			if (m_ItemCheckedBorder != value)
			{
				m_ItemCheckedBorder = value;
				m_ItemCheckedBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the text color for the checked item.")]
	public Color ItemCheckedText
	{
		get
		{
			return m_ItemCheckedText;
		}
		set
		{
			if (m_ItemCheckedText != value)
			{
				m_ItemCheckedText = value;
				m_ItemCheckedTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Special Item Colors")]
	[Description("Specifies the customize item background color.")]
	public Color CustomizeBackground
	{
		get
		{
			return m_CustomizeBackground;
		}
		set
		{
			if (m_CustomizeBackground != value)
			{
				m_CustomizeBackground = value;
				m_CustomizeBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Special Item Colors")]
	[Description("Specifies the customize item target gradient background color.")]
	public Color CustomizeBackground2
	{
		get
		{
			return m_CustomizeBackground2;
		}
		set
		{
			if (m_CustomizeBackground2 != value)
			{
				m_CustomizeBackground2 = value;
				m_CustomizeBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Special Item Colors")]
	[Description("Specifies the customize item background color gradient angle.")]
	[DefaultValue(90)]
	public int CustomizeBackgroundGradientAngle
	{
		get
		{
			return m_CustomizeBackgroundGradientAngle;
		}
		set
		{
			m_CustomizeBackgroundGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Special Item Colors")]
	[Description("Specifies the customize item text color.")]
	public Color CustomizeText
	{
		get
		{
			return m_CustomizeText;
		}
		set
		{
			if (m_CustomizeText != value)
			{
				m_CustomizeText = value;
				m_CustomizeTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the color of the menu border.")]
	public Color MenuBorder
	{
		get
		{
			return m_MenuBorder;
		}
		set
		{
			if (m_MenuBorder != value)
			{
				m_MenuBorder = value;
				m_MenuBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the background color of the menu.")]
	public Color MenuBackground
	{
		get
		{
			return m_MenuBackground;
		}
		set
		{
			if (m_MenuBackground != value)
			{
				m_MenuBackground = value;
				m_MenuBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the target gradient background color of the menu.")]
	public Color MenuBackground2
	{
		get
		{
			return m_MenuBackground2;
		}
		set
		{
			if (m_MenuBackground2 != value)
			{
				m_MenuBackground2 = value;
				m_MenuBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the angle of the gradient fill for the menu background.")]
	[DefaultValue(0)]
	public int MenuBackgroundGradientAngle
	{
		get
		{
			return m_MenuBackgroundGradientAngle;
		}
		set
		{
			if (m_MenuBackgroundGradientAngle != value)
			{
				m_MenuBackgroundGradientAngle = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the background color of the menu part (left side)  that is showing the images.")]
	public Color MenuSide
	{
		get
		{
			return m_MenuSide;
		}
		set
		{
			if (m_MenuSide != value)
			{
				m_MenuSide = value;
				m_MenuSideCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the target gradient background color of the menu part (left side)  that is showing the images.")]
	public Color MenuSide2
	{
		get
		{
			return m_MenuSide2;
		}
		set
		{
			if (m_MenuSide2 != value)
			{
				m_MenuSide2 = value;
				m_MenuSide2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the angle of the gradient fill for the menu part (left side) that is showing the images.")]
	[DefaultValue(0)]
	public int MenuSideGradientAngle
	{
		get
		{
			return m_MenuSideGradientAngle;
		}
		set
		{
			if (m_MenuSideGradientAngle != value)
			{
				m_MenuSideGradientAngle = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the background color for the items that were not recently used.")]
	public Color MenuUnusedBackground
	{
		get
		{
			return m_MenuUnusedBackground;
		}
		set
		{
			if (m_MenuUnusedBackground != value)
			{
				m_MenuUnusedBackground = value;
				m_MenuUnusedBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the side bar color for the items that were not recently used.")]
	public Color MenuUnusedSide
	{
		get
		{
			return m_MenuUnusedSide;
		}
		set
		{
			if (m_MenuUnusedSide != value)
			{
				m_MenuUnusedSide = value;
				m_MenuUnusedSideCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the target gradient side bar color for the items that were not recently used.")]
	public Color MenuUnusedSide2
	{
		get
		{
			return m_MenuUnusedSide2;
		}
		set
		{
			if (m_MenuUnusedSide2 != value)
			{
				m_MenuUnusedSide2 = value;
				m_MenuUnusedSide2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Menu Colors")]
	[Description("Specifies the angle of the gradient fill for the menu part (left side) that is showing the images.")]
	[DefaultValue(0)]
	public int MenuUnusedSideGradientAngle
	{
		get
		{
			return m_MenuUnusedSideGradientAngle;
		}
		set
		{
			if (m_MenuUnusedSideGradientAngle != value)
			{
				m_MenuUnusedSideGradientAngle = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Design Colors")]
	[Description("Specifies the border color for focused design-time item.")]
	public Color ItemDesignTimeBorder
	{
		get
		{
			return m_ItemDesignTimeBorder;
		}
		set
		{
			if (m_ItemDesignTimeBorder != value)
			{
				m_ItemDesignTimeBorder = value;
				m_ItemDesignTimeBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Color Scheme")]
	[Description("Indicates predefined color scheme. By default DotNetBar will automatically change and generate color scheme depending on system colors.")]
	[DefaultValue(ePredefinedColorScheme.AutoGenerated)]
	public ePredefinedColorScheme PredefinedColorScheme
	{
		get
		{
			return m_PredefinedColorScheme;
		}
		set
		{
			if (m_PredefinedColorScheme != value)
			{
				m_PredefinedColorScheme = value;
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies the background color of the panel.")]
	public Color PanelBackground
	{
		get
		{
			return m_PanelBackground;
		}
		set
		{
			if (m_PanelBackground != value)
			{
				m_PanelBackground = value;
				m_PanelBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies the target background gradient color of the panel.")]
	public Color PanelBackground2
	{
		get
		{
			return m_PanelBackground2;
		}
		set
		{
			if (m_PanelBackground2 != value)
			{
				m_PanelBackground2 = value;
				m_PanelBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies the angle of the gradient fill for the panel background.")]
	[DefaultValue(90)]
	public int PanelBackgroundGradientAngle
	{
		get
		{
			return m_PanelBackgroundGradientAngle;
		}
		set
		{
			if (m_PanelBackgroundGradientAngle != value)
			{
				m_PanelBackgroundGradientAngle = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies border color of the panel.")]
	public Color PanelBorder
	{
		get
		{
			return m_PanelBorder;
		}
		set
		{
			if (m_PanelBorder != value)
			{
				m_PanelBorder = value;
				m_PanelBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies color of the text on the panel.")]
	public Color PanelText
	{
		get
		{
			return m_PanelText;
		}
		set
		{
			if (m_PanelText != value)
			{
				m_PanelText = value;
				m_PanelTextCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Explorer Bar Colors")]
	[Description("Specifies the background color of the explorer bar.")]
	public Color ExplorerBarBackground
	{
		get
		{
			return m_ExplorerBarBackground;
		}
		set
		{
			if (m_ExplorerBarBackground != value)
			{
				m_ExplorerBarBackground = value;
				m_ExplorerBarBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Explorer Bar Colors")]
	[Description("Specifies target gradient background color of the explorer bar.")]
	public Color ExplorerBarBackground2
	{
		get
		{
			return m_ExplorerBarBackground2;
		}
		set
		{
			if (m_ExplorerBarBackground2 != value)
			{
				m_ExplorerBarBackground2 = value;
				m_ExplorerBarBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Explorer Bar Colors")]
	[Description("Specifies the angle of the gradient fill for the explorer bar background.")]
	[DefaultValue(90)]
	public int ExplorerBarBackgroundGradientAngle
	{
		get
		{
			return m_ExplorerBarBackgroundGradientAngle;
		}
		set
		{
			if (m_ExplorerBarBackgroundGradientAngle != value)
			{
				m_ExplorerBarBackgroundGradientAngle = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the foreground color of MDI System Item buttons.")]
	public Color MdiSystemItemForeground
	{
		get
		{
			return m_MdiSystemItemForeground;
		}
		set
		{
			m_MdiSystemItemForeground = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Splitter Colors")]
	[Description("Specifies the background color of the Splitter.")]
	public Color SplitterBackground
	{
		get
		{
			return m_SplitterBackground;
		}
		set
		{
			if (m_SplitterBackground != value)
			{
				m_SplitterBackground = value;
				m_SplitterBackgroundCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Splitter Colors")]
	[Description("Specifies the target background gradient color of the Splitter.")]
	public Color SplitterBackground2
	{
		get
		{
			return m_SplitterBackground2;
		}
		set
		{
			if (m_SplitterBackground2 != value)
			{
				m_SplitterBackground2 = value;
				m_SplitterBackground2Custom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Splitter Colors")]
	[Description("Specifies the angle of the gradient fill for the Splitter background.")]
	[DefaultValue(90)]
	public int SplitterBackgroundGradientAngle
	{
		get
		{
			return m_SplitterBackgroundGradientAngle;
		}
		set
		{
			if (m_SplitterBackgroundGradientAngle != value)
			{
				m_SplitterBackgroundGradientAngle = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Splitter Colors")]
	[Description("Specifies border color of the Splitter.")]
	public Color SplitterBorder
	{
		get
		{
			return m_SplitterBorder;
		}
		set
		{
			if (m_SplitterBorder != value)
			{
				m_SplitterBorder = value;
				m_SplitterBorderCustom = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Splitter Colors")]
	[Description("Specifies color of the text on the Splitter.")]
	public Color SplitterText
	{
		get
		{
			return m_SplitterText;
		}
		set
		{
			if (m_SplitterText != value)
			{
				m_SplitterText = value;
				m_SplitterTextCustom = true;
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool SchemeChanged
	{
		get
		{
			if (!m_BarCaptionBackgroundCustom && !m_BarCaptionBackground2Custom && !m_BarStripeColorCustom && !m_BarCaptionInactiveBackgroundCustom && !m_BarCaptionInactiveBackground2Custom && !m_BarCaptionInactiveTextCustom && !m_BarCaptionTextCustom && !m_BarFloatingBorderCustom && !m_BarPopupBackgroundCustom && !m_BarDockedBorderCustom && !m_BarPopupBorderCustom && !m_BarBackgroundCustom && !m_BarBackground2Custom && !m_ItemBackgroundCustom && !m_ItemCheckedBackgroundCustom && !m_ItemCheckedBackground2Custom && !m_ItemCheckedBorderCustom && !m_ItemCheckedTextCustom && !m_ItemDisabledBackgroundCustom && !m_ItemDisabledTextCustom && !m_ItemExpandedShadowCustom && !m_ItemExpandedBackgroundCustom && !m_ItemExpandedBackground2Custom && !m_ItemExpandedTextCustom && !m_ItemExpandedBorderCustom && !m_ItemHotBackgroundCustom && !m_ItemHotBackground2Custom && !m_ItemHotBorderCustom && !m_ItemHotTextCustom && !m_ItemPressedBackgroundCustom && !m_ItemPressedBackground2Custom && !m_ItemPressedBorderCustom && !m_ItemPressedTextCustom && !m_ItemSeparatorCustom && !m_ItemTextCustom && !m_MenuBackgroundCustom && !m_MenuBackground2Custom && !m_MenuBarBackgroundCustom && !m_MenuBarBackground2Custom && !m_MenuBorderCustom && !m_MenuSideCustom && !m_MenuSide2Custom && !m_MenuUnusedBackgroundCustom && !m_MenuUnusedSideCustom && !m_MenuUnusedSide2Custom && !m_ItemDesignTimeBorderCustom && !m_CustomizeBackgroundCustom && !m_CustomizeBackground2Custom && !m_DockSiteBackColorCustom && !m_DockSiteBackColor2Custom && m_MdiSystemItemForeground.IsEmpty)
			{
				return m_PredefinedColorScheme != ePredefinedColorScheme.AutoGenerated;
			}
			return true;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates auto-hide panel background image if any.")]
	[Category("Auto-Hide Panel")]
	public Image AutoHidePanelBackgroundImage
	{
		get
		{
			return _AutoHidePanelBackgroundImage;
		}
		set
		{
			_AutoHidePanelBackgroundImage = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide panel background.")]
	public Color AutoHidePanelBackground
	{
		get
		{
			return _AutoHidePanelBackground;
		}
		set
		{
			_AutoHidePanelBackground = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide panel background.")]
	public Color AutoHidePanelBackground2
	{
		get
		{
			return _AutoHidePanelBackground2;
		}
		set
		{
			_AutoHidePanelBackground2 = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of the auto-hide tab.")]
	public Color AutoHideTabBackground
	{
		get
		{
			return _AutoHideTabBackground;
		}
		set
		{
			_AutoHideTabBackground = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide tab.")]
	public Color AutoHideTabBackground2
	{
		get
		{
			return _AutoHideTabBackground2;
		}
		set
		{
			_AutoHideTabBackground2 = value;
		}
	}

	[DefaultValue(90)]
	[Category("Auto-Hide Panel")]
	[Description("Gets or sets the auto-hide tab background gradient angle.")]
	public int AutoHideTabBackgroundGradientAngle
	{
		get
		{
			return _AutoHideTabBackgroundGradientAngle;
		}
		set
		{
			_AutoHideTabBackgroundGradientAngle = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide tab text.")]
	public Color AutoHideTabText
	{
		get
		{
			return _AutoHideTabText;
		}
		set
		{
			_AutoHideTabText = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide tab border.")]
	public Color AutoHideTabBorder
	{
		get
		{
			return _AutoHideTabBorder;
		}
		set
		{
			_AutoHideTabBorder = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of the auto-hide tab.")]
	public Color AutoHideSelectedTabBackground
	{
		get
		{
			return _AutoHideSelectedTabBackground;
		}
		set
		{
			_AutoHideSelectedTabBackground = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide tab.")]
	public Color AutoHideSelectedTabBackground2
	{
		get
		{
			return _AutoHideSelectedTabBackground2;
		}
		set
		{
			_AutoHideSelectedTabBackground2 = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide tab text.")]
	public Color AutoHideSelectedTabText
	{
		get
		{
			return _AutoHideSelectedTabText;
		}
		set
		{
			_AutoHideSelectedTabText = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of tab border.")]
	public Color AutoHideSelectedTabBorder
	{
		get
		{
			return _AutoHideSelectedTabBorder;
		}
		set
		{
			_AutoHideSelectedTabBorder = value;
		}
	}

	public ColorScheme()
	{
		Refresh(null, bSystemColorEvent: false);
	}

	public ColorScheme(Graphics graphics)
	{
		Refresh(graphics, bSystemColorEvent: false);
	}

	public ColorScheme(eDotNetBarStyle style)
	{
		m_Style = style;
		Refresh(null, bSystemColorEvent: true);
	}

	internal void SwitchStyle(eDotNetBarStyle style)
	{
		if (m_Style != style)
		{
			m_Style = style;
			Refresh(null, bSystemColorEvent: true);
		}
	}

	public void SetStyle()
	{
		Refresh(null, bSystemColorEvent: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDockSiteBackColor()
	{
		return m_DockSiteBackColorCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDockSiteBackColor2()
	{
		return m_DockSiteBackColor2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBarBackground()
	{
		return m_MenuBarBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBarBackground2()
	{
		return m_MenuBarBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarBackground()
	{
		return m_BarBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarBackground2()
	{
		return m_BarBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionBackground()
	{
		return m_BarCaptionBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionBackground2()
	{
		return m_BarCaptionBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionText()
	{
		return m_BarCaptionTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionInactiveBackground()
	{
		return m_BarCaptionInactiveBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionInactiveBackground2()
	{
		return m_BarCaptionInactiveBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionInactiveText()
	{
		return m_BarCaptionInactiveTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarPopupBackground()
	{
		return m_BarPopupBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarPopupBorder()
	{
		return m_BarPopupBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarDockedBorder()
	{
		return m_BarDockedBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarStripeColor()
	{
		return m_BarStripeColorCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarFloatingBorder()
	{
		return m_BarFloatingBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemBackground()
	{
		return m_ItemBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemBackground2()
	{
		return m_ItemBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemText()
	{
		return m_ItemTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemDisabledBackground()
	{
		return m_ItemDisabledBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemDisabledText()
	{
		return m_ItemDisabledTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemHotBackground()
	{
		return m_ItemHotBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemHotBackground2()
	{
		return m_ItemHotBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemHotText()
	{
		return m_ItemHotTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemHotBorder()
	{
		return m_ItemHotBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemPressedBackground()
	{
		return m_ItemPressedBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemPressedBackground2()
	{
		return m_ItemPressedBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemPressedText()
	{
		return m_ItemPressedTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemPressedBorder()
	{
		return m_ItemPressedBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemSeparator()
	{
		return m_ItemSeparatorCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemSeparatorShade()
	{
		return m_ItemSeparatorShadeCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedShadow()
	{
		return m_ItemExpandedShadowCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedBackground()
	{
		return m_ItemExpandedBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedBackground2()
	{
		return m_ItemExpandedBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedText()
	{
		return m_ItemExpandedTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedBorder()
	{
		return m_ItemExpandedBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemCheckedBackground()
	{
		return m_ItemCheckedBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemCheckedBackground2()
	{
		return m_ItemCheckedBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemCheckedBorder()
	{
		return m_ItemCheckedBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemCheckedText()
	{
		return m_ItemCheckedTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCustomizeBackground()
	{
		return m_CustomizeBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCustomizeBackground2()
	{
		return m_CustomizeBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCustomizeText()
	{
		return m_CustomizeTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBorder()
	{
		return m_MenuBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBackground()
	{
		return m_MenuBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBackground2()
	{
		return m_MenuBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuSide()
	{
		return m_MenuSideCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuSide2()
	{
		return m_MenuSide2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuUnusedBackground()
	{
		return m_MenuUnusedBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuUnusedSide()
	{
		return m_MenuUnusedSideCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuUnusedSide2()
	{
		return m_MenuUnusedSide2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemDesignTimeBorder()
	{
		return m_ItemDesignTimeBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelBackground()
	{
		return m_PanelBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelBackground2()
	{
		return m_PanelBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelBorder()
	{
		return m_PanelBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelText()
	{
		return m_PanelTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExplorerBarBackground()
	{
		return m_ExplorerBarBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExplorerBarBackground2()
	{
		return m_ExplorerBarBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMdiSystemItemForeground()
	{
		return !m_MdiSystemItemForeground.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMdiSystemItemForeground()
	{
		MdiSystemItemForeground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSplitterBackground()
	{
		return m_SplitterBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSplitterBackground2()
	{
		return m_SplitterBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSplitterBorder()
	{
		return m_SplitterBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSplitterText()
	{
		return m_SplitterTextCustom;
	}

	public void Refresh()
	{
		Refresh(null, bSystemColorEvent: false);
	}

	public void ResetChangedFlag()
	{
		m_BarCaptionBackgroundCustom = false;
		m_BarCaptionBackground2Custom = false;
		m_BarStripeColorCustom = false;
		m_BarCaptionInactiveBackgroundCustom = false;
		m_BarCaptionInactiveBackground2Custom = false;
		m_BarCaptionInactiveTextCustom = false;
		m_BarCaptionTextCustom = false;
		m_BarFloatingBorderCustom = false;
		m_BarPopupBackgroundCustom = false;
		m_BarPopupBorderCustom = false;
		m_BarDockedBorderCustom = false;
		m_BarBackgroundCustom = false;
		m_BarBackground2Custom = false;
		m_ItemBackgroundCustom = false;
		m_ItemBackground2Custom = false;
		m_ItemCheckedBackgroundCustom = false;
		m_ItemCheckedBackground2Custom = false;
		m_ItemCheckedBorderCustom = false;
		m_ItemCheckedTextCustom = false;
		m_ItemDisabledBackgroundCustom = false;
		m_ItemDisabledTextCustom = false;
		m_ItemExpandedShadowCustom = false;
		m_ItemExpandedBackgroundCustom = false;
		m_ItemExpandedBackground2Custom = false;
		m_ItemExpandedTextCustom = false;
		m_ItemExpandedBorderCustom = false;
		m_ItemHotBackgroundCustom = false;
		m_ItemHotBackground2Custom = false;
		m_ItemHotBorderCustom = false;
		m_ItemHotTextCustom = false;
		m_ItemPressedBackgroundCustom = false;
		m_ItemPressedBackground2Custom = false;
		m_ItemPressedBorderCustom = false;
		m_ItemPressedTextCustom = false;
		m_ItemSeparatorCustom = false;
		m_ItemSeparatorShadeCustom = false;
		m_ItemTextCustom = false;
		m_MenuBackgroundCustom = false;
		m_MenuBackground2Custom = false;
		m_MenuBarBackgroundCustom = false;
		m_MenuBarBackground2Custom = false;
		m_MenuBorderCustom = false;
		m_MenuSideCustom = false;
		m_MenuSide2Custom = false;
		m_MenuUnusedBackgroundCustom = false;
		m_MenuUnusedSideCustom = false;
		m_MenuUnusedSide2Custom = false;
		m_ItemDesignTimeBorderCustom = false;
		m_CustomizeBackgroundCustom = false;
		m_CustomizeBackground2Custom = false;
		m_CustomizeTextCustom = false;
		m_PanelBackgroundCustom = false;
		m_PanelBackground2Custom = false;
		m_PanelBorderCustom = false;
		m_PanelTextCustom = false;
		m_SplitterBackgroundCustom = false;
		m_SplitterBackground2Custom = false;
		m_SplitterBorderCustom = false;
		m_SplitterTextCustom = false;
		m_ExplorerBarBackgroundCustom = false;
		m_ExplorerBarBackground2Custom = false;
		m_MdiSystemItemForeground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHidePanelBackground()
	{
		return !_AutoHidePanelBackground.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHidePanelBackground()
	{
		AutoHidePanelBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHidePanelBackground2()
	{
		return !_AutoHidePanelBackground2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHidePanelBackground2()
	{
		AutoHidePanelBackground2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideTabBackground()
	{
		return !_AutoHideTabBackground.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideTabBackground()
	{
		AutoHideTabBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideTabBackground2()
	{
		return !_AutoHideTabBackground2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideTabBackground2()
	{
		AutoHideTabBackground2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideTabText()
	{
		return !_AutoHideTabText.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideTabText()
	{
		AutoHideTabText = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideTabBorder()
	{
		return !_AutoHideTabBorder.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideTabBorder()
	{
		AutoHideTabBorder = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideSelectedTabBackground()
	{
		return !_AutoHideSelectedTabBackground.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideSelectedTabBackground()
	{
		AutoHideSelectedTabBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideSelectedTabBackground2()
	{
		return !_AutoHideSelectedTabBackground2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideSelectedTabBackground2()
	{
		AutoHideSelectedTabBackground2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideSelectedTabText()
	{
		return !_AutoHideSelectedTabText.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideSelectedTabText()
	{
		AutoHideSelectedTabText = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideSelectedTabBorder()
	{
		return !_AutoHideSelectedTabBorder.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideSelectedTabBorder()
	{
		AutoHideSelectedTabBorder = Color.Empty;
	}

	public void Refresh(Graphics graphics, bool bSystemColorEvent)
	{
		if (!bSystemColorEvent)
		{
			ResetChangedFlag();
		}
		if (m_PredefinedColorScheme == ePredefinedColorScheme.Blue2003)
		{
			SchemeXpBlue2003();
		}
		else if (m_PredefinedColorScheme == ePredefinedColorScheme.OliveGreen2003)
		{
			SchemeXpOliveGreen2003();
		}
		else if (m_PredefinedColorScheme == ePredefinedColorScheme.Silver2003)
		{
			SchemeXpSilver2003();
		}
		else if (m_PredefinedColorScheme == ePredefinedColorScheme.SystemColors)
		{
			GenerateScheme();
		}
		else if (Style == eDotNetBarStyle.Office2007)
		{
			SchemeOffice2007();
		}
		else
		{
			switch (BarFunctions.WinXPColorScheme)
			{
			case eWinXPColorScheme.Blue:
				if (Style == eDotNetBarStyle.Office2003)
				{
					SchemeXpBlue2003();
				}
				else if (Style == eDotNetBarStyle.VS2005)
				{
					SchemeXpBlueVS2005();
				}
				else
				{
					SchemeXpBlue();
				}
				break;
			case eWinXPColorScheme.Silver:
				if (Style == eDotNetBarStyle.Office2003)
				{
					SchemeXpSilver2003();
				}
				else if (Style == eDotNetBarStyle.VS2005)
				{
					SchemeXpSilverVS2005();
				}
				else
				{
					SchemeXpSilver();
				}
				break;
			case eWinXPColorScheme.OliveGreen:
				if (Style == eDotNetBarStyle.Office2003)
				{
					SchemeXpOliveGreen2003();
				}
				else if (Style == eDotNetBarStyle.VS2005)
				{
					SchemeXpOliveGreenVS2005();
				}
				else
				{
					SchemeXpOliveGreen();
				}
				break;
			default:
				GenerateScheme();
				break;
			case eWinXPColorScheme.Undetermined:
				GenerateScheme();
				break;
			}
		}
		bool flag = false;
		if (graphics == null)
		{
			IntPtr desktopWindow = NativeFunctions.GetDesktopWindow();
			if (desktopWindow != IntPtr.Zero)
			{
				graphics = Graphics.FromHwnd(desktopWindow);
				flag = true;
			}
		}
		try
		{
			if (graphics != null)
			{
				if (!m_BarCaptionBackground.IsSystemColor && !m_BarCaptionBackground.IsEmpty)
				{
					m_BarCaptionBackground = graphics.GetNearestColor(m_BarCaptionBackground);
				}
				if (!m_BarCaptionBackground2.IsSystemColor && !m_BarCaptionBackground2.IsEmpty)
				{
					m_BarCaptionBackground2 = graphics.GetNearestColor(m_BarCaptionBackground2);
				}
				if (!m_BarStripeColor.IsSystemColor && !m_BarStripeColor.IsEmpty)
				{
					m_BarStripeColor = graphics.GetNearestColor(m_BarStripeColor);
				}
				if (!m_BarCaptionInactiveBackground.IsSystemColor && !m_BarCaptionInactiveBackground.IsEmpty)
				{
					m_BarCaptionInactiveBackground = graphics.GetNearestColor(m_BarCaptionInactiveBackground);
				}
				if (!m_BarCaptionInactiveBackground2.IsSystemColor && !m_BarCaptionInactiveBackground2.IsEmpty)
				{
					m_BarCaptionInactiveBackground2 = graphics.GetNearestColor(m_BarCaptionInactiveBackground2);
				}
				if (!m_BarCaptionInactiveText.IsSystemColor && !m_BarCaptionInactiveText.IsEmpty)
				{
					m_BarCaptionInactiveText = graphics.GetNearestColor(m_BarCaptionInactiveText);
				}
				if (!m_BarCaptionText.IsSystemColor && !m_BarCaptionText.IsEmpty)
				{
					m_BarCaptionText = graphics.GetNearestColor(m_BarCaptionText);
				}
				if (!m_BarFloatingBorder.IsSystemColor && !m_BarFloatingBorder.IsEmpty)
				{
					m_BarFloatingBorder = graphics.GetNearestColor(m_BarFloatingBorder);
				}
				if (!m_BarPopupBackground.IsSystemColor && !m_BarPopupBackground.IsEmpty)
				{
					m_BarPopupBackground = graphics.GetNearestColor(m_BarPopupBackground);
				}
				if (!m_BarPopupBorder.IsSystemColor && !m_BarPopupBorder.IsEmpty)
				{
					m_BarPopupBorder = graphics.GetNearestColor(m_BarPopupBorder);
				}
				if (!m_BarDockedBorder.IsSystemColor && !m_BarDockedBorder.IsEmpty)
				{
					m_BarDockedBorder = graphics.GetNearestColor(m_BarDockedBorder);
				}
				if (!m_BarBackground.IsSystemColor && !m_BarBackground.IsEmpty)
				{
					m_BarBackground = graphics.GetNearestColor(m_BarBackground);
				}
				if (!m_BarBackground2.IsEmpty && !m_BarBackground2.IsSystemColor)
				{
					m_BarBackground2 = graphics.GetNearestColor(m_BarBackground2);
				}
				if (!m_ItemBackground.IsEmpty)
				{
					m_ItemBackground = graphics.GetNearestColor(m_ItemBackground);
				}
				if (!m_ItemBackground2.IsEmpty)
				{
					m_ItemBackground2 = graphics.GetNearestColor(m_ItemBackground2);
				}
				if (!m_ItemCheckedBackground.IsSystemColor)
				{
					m_ItemCheckedBackground = graphics.GetNearestColor(m_ItemCheckedBackground);
				}
				if (!m_ItemCheckedBackground2.IsEmpty && !m_ItemCheckedBackground2.IsSystemColor)
				{
					m_ItemCheckedBackground2 = graphics.GetNearestColor(m_ItemCheckedBackground2);
				}
				if (!m_ItemCheckedBorder.IsSystemColor)
				{
					m_ItemCheckedBorder = graphics.GetNearestColor(m_ItemCheckedBorder);
				}
				if (!m_ItemCheckedText.IsSystemColor)
				{
					m_ItemCheckedText = graphics.GetNearestColor(m_ItemCheckedText);
				}
				if (!m_ItemDisabledBackground.IsEmpty && !m_ItemDisabledBackground.IsSystemColor)
				{
					m_ItemDisabledBackground = graphics.GetNearestColor(m_ItemDisabledBackground);
				}
				if (!m_ItemDisabledText.IsSystemColor)
				{
					m_ItemDisabledText = graphics.GetNearestColor(m_ItemDisabledText);
				}
				if (!m_ItemExpandedShadow.IsSystemColor && !m_ItemExpandedShadow.IsEmpty)
				{
					m_ItemExpandedShadow = graphics.GetNearestColor(m_ItemExpandedShadow);
				}
				if (!m_ItemExpandedBackground.IsSystemColor)
				{
					m_ItemExpandedBackground = graphics.GetNearestColor(m_ItemExpandedBackground);
				}
				if (!m_ItemExpandedText.IsSystemColor)
				{
					m_ItemExpandedText = graphics.GetNearestColor(m_ItemExpandedText);
				}
				if (!m_ItemExpandedBorder.IsSystemColor)
				{
					m_ItemExpandedBorder = graphics.GetNearestColor(m_ItemExpandedBorder);
				}
				if (!m_ItemHotBackground.IsSystemColor)
				{
					m_ItemHotBackground = graphics.GetNearestColor(m_ItemHotBackground);
				}
				if (!m_ItemHotBackground2.IsEmpty && !m_ItemHotBackground2.IsSystemColor)
				{
					m_ItemHotBackground2 = graphics.GetNearestColor(m_ItemHotBackground2);
				}
				if (!m_ItemHotBorder.IsSystemColor)
				{
					m_ItemHotBorder = graphics.GetNearestColor(m_ItemHotBorder);
				}
				if (!m_ItemHotText.IsSystemColor)
				{
					m_ItemHotText = graphics.GetNearestColor(m_ItemHotText);
				}
				if (!m_ItemPressedBackground.IsSystemColor)
				{
					m_ItemPressedBackground = graphics.GetNearestColor(m_ItemPressedBackground);
				}
				if (!m_ItemPressedBackground2.IsEmpty && !m_ItemPressedBackground2.IsSystemColor)
				{
					m_ItemPressedBackground2 = graphics.GetNearestColor(m_ItemPressedBackground2);
				}
				if (!m_ItemPressedBorder.IsSystemColor)
				{
					m_ItemPressedBorder = graphics.GetNearestColor(m_ItemPressedBorder);
				}
				if (!m_ItemPressedText.IsSystemColor)
				{
					m_ItemPressedText = graphics.GetNearestColor(m_ItemPressedText);
				}
				if (!m_ItemSeparator.IsSystemColor)
				{
					m_ItemSeparator = graphics.GetNearestColor(m_ItemSeparator);
				}
				if (!m_ItemSeparatorShade.IsSystemColor && !m_ItemSeparatorShade.IsEmpty)
				{
					m_ItemSeparatorShade = graphics.GetNearestColor(m_ItemSeparatorShade);
				}
				if (!m_ItemText.IsSystemColor)
				{
					m_ItemText = graphics.GetNearestColor(m_ItemText);
				}
				if (!m_MenuBackground.IsSystemColor)
				{
					m_MenuBackground = graphics.GetNearestColor(m_MenuBackground);
				}
				if (!m_MenuBarBackground.IsSystemColor)
				{
					m_MenuBarBackground = graphics.GetNearestColor(m_MenuBarBackground);
				}
				if (!m_MenuBarBackground2.IsEmpty && !m_MenuBarBackground2.IsSystemColor)
				{
					m_MenuBarBackground2 = graphics.GetNearestColor(m_MenuBarBackground2);
				}
				if (!m_MenuBorder.IsSystemColor)
				{
					m_MenuBorder = graphics.GetNearestColor(m_MenuBorder);
				}
				if (!m_MenuSide.IsSystemColor)
				{
					m_MenuSide = graphics.GetNearestColor(m_MenuSide);
				}
				if (!m_MenuUnusedBackground.IsSystemColor)
				{
					m_MenuUnusedBackground = graphics.GetNearestColor(m_MenuUnusedBackground);
				}
				if (!m_MenuUnusedSide.IsSystemColor)
				{
					m_MenuUnusedSide = graphics.GetNearestColor(m_MenuUnusedSide);
				}
				if (!m_ItemDesignTimeBorder.IsSystemColor)
				{
					m_ItemDesignTimeBorder = graphics.GetNearestColor(m_ItemDesignTimeBorder);
				}
				if (!m_CustomizeBackground.IsEmpty && !m_CustomizeBackground.IsSystemColor)
				{
					m_CustomizeBackground = graphics.GetNearestColor(m_CustomizeBackground);
				}
				if (!m_CustomizeBackground2.IsEmpty && !m_CustomizeBackground2.IsSystemColor)
				{
					m_CustomizeBackground2 = graphics.GetNearestColor(m_CustomizeBackground2);
				}
				if (!m_CustomizeText.IsEmpty && !m_CustomizeText.IsSystemColor)
				{
					m_CustomizeText = graphics.GetNearestColor(m_CustomizeText);
				}
			}
		}
		finally
		{
			if (flag)
			{
				graphics.Dispose();
			}
		}
	}

	private void GenerateScheme()
	{
		if (m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005)
		{
			GenerateScheme2003();
		}
		else
		{
			GenerateSchemeDefault();
		}
	}

	private void GenerateSchemeDefault()
	{
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = ColorFunctions.MenuFocusBorderColor();
		}
		m_BarCaptionBackground2 = Color.Empty;
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = SystemColors.ControlDark;
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = SystemColors.Control;
		}
		m_BarCaptionInactiveBackground2 = Color.Empty;
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = SystemColors.ControlText;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = SystemColors.ActiveCaptionText;
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = ColorFunctions.MenuFocusBorderColor();
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = SystemColors.Control;
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = ColorFunctions.MenuFocusBorderColor();
		}
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = ColorFunctions.CheckBoxBackColor();
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = SystemColors.Highlight;
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = ColorFunctions.HoverBackColor();
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = SystemColors.Highlight;
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = ColorFunctions.PressedBackColor();
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = SystemColors.Highlight;
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = SystemColors.ControlDarkDark;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = SystemColors.ControlDark;
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = ColorFunctions.MenuBackColor();
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = SystemColors.Control;
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = ColorFunctions.SideRecentlyBackColor();
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = SystemColors.Highlight;
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackground2Custom)
		{
			m_MenuBarBackground2 = Color.Empty;
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.Empty;
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.Empty;
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.Empty;
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.Empty;
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.Empty;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = Color.Empty;
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.Empty;
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = Color.Empty;
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.Empty;
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = Color.Empty;
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = Color.Empty;
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = Color.Empty;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = ControlPaint.Dark(m_BarBackground);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = ControlPaint.DarkDark(m_BarBackground);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = ControlPaint.LightLight(m_BarBackground);
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = ControlPaint.DarkDark(m_BarBackground);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = ControlPaint.Dark(m_BarBackground);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = ControlPaint.DarkDark(m_BarBackground);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = ControlPaint.LightLight(m_BarBackground);
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = ControlPaint.DarkDark(m_BarBackground);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = m_MenuBarBackground;
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = m_ItemSeparator;
		}
	}

	private void GenerateScheme2003()
	{
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = ColorFunctions.MenuFocusBorderColor();
		}
		if (!m_BarCaptionBackground2Custom)
		{
			m_BarCaptionBackground2 = ControlPaint.Light(m_BarCaptionBackground);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = SystemColors.ControlDark;
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = SystemColors.Control;
		}
		m_BarCaptionInactiveBackground2 = Color.Empty;
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = SystemColors.ControlText;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = SystemColors.ActiveCaptionText;
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = ColorFunctions.MenuFocusBorderColor();
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = SystemColors.Control;
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = ColorFunctions.MenuFocusBorderColor();
		}
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = ColorFunctions.MenuBackColor();
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = ColorFunctions.CheckBoxBackColor();
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = SystemColors.Highlight;
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = Color.Empty;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = ColorFunctions.HoverBackColor3();
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = SystemColors.Highlight;
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = ColorFunctions.HoverBackColor2();
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = SystemColors.Highlight;
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = SystemColors.ControlDarkDark;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = SystemColors.ControlDark;
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = ColorFunctions.MenuBackColor();
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = SystemColors.Control;
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = ColorFunctions.MenuBackColor();
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = ColorFunctions.SideRecentlyBackColor();
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = SystemColors.Highlight;
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = SystemColors.Control;
		}
		if (!m_MenuBarBackground2Custom)
		{
			m_MenuBarBackground2 = Color.Empty;
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = ColorFunctions.HoverBackColor2();
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = ColorFunctions.HoverBackColor3();
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.Empty;
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.Empty;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = SystemColors.Control;
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = SystemColors.ControlLightLight;
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = SystemColors.Control;
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = ColorFunctions.MenuBackColor();
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = SystemColors.ControlDark;
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = ColorFunctions.MenuFocusBorderColor();
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = SystemColors.ControlText;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = m_BarBackground2;
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = ControlPaint.Dark(m_BarBackground2);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = ControlPaint.LightLight(m_BarBackground);
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = ControlPaint.DarkDark(m_BarBackground2);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = m_BarBackground2;
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = ControlPaint.Dark(m_BarBackground2);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = ControlPaint.LightLight(m_BarBackground);
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = ControlPaint.DarkDark(m_BarBackground2);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = m_MenuBarBackground;
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = m_ItemSeparator;
		}
	}

	private void SchemeXpBlue()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = Color.FromArgb(239, 237, 222);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = Color.FromArgb(191, 188, 177);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = Color.FromArgb(172, 168, 153);
		}
		m_BarCaptionBackground2 = Color.Empty;
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = SystemColors.Control;
		}
		m_BarCaptionInactiveBackground2 = Color.Empty;
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = SystemColors.ControlText;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.FromArgb(64, 64, 64);
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = Color.FromArgb(172, 168, 153);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = Color.FromArgb(252, 252, 249);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = Color.FromArgb(138, 134, 122);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = Color.FromArgb(225, 230, 232);
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = SystemColors.Highlight;
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = Color.FromArgb(239, 237, 222);
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = Color.FromArgb(193, 210, 238);
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = SystemColors.Highlight;
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = Color.FromArgb(152, 181, 226);
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = SystemColors.Highlight;
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = Color.FromArgb(73, 73, 73);
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = Color.FromArgb(197, 194, 184);
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = Color.FromArgb(252, 252, 249);
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = SystemColors.Control;
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = Color.FromArgb(239, 237, 222);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = Color.FromArgb(230, 227, 210);
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = SystemColors.Highlight;
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackground2Custom)
		{
			m_MenuBarBackground2 = Color.Empty;
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.Empty;
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.Empty;
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.Empty;
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.Empty;
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.Empty;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = Color.Empty;
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.Empty;
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = Color.Empty;
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.Empty;
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = Color.Empty;
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = Color.Empty;
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = Color.Empty;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = Color.FromArgb(89, 135, 214);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = Color.FromArgb(3, 56, 148);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.White;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = Color.FromArgb(0, 45, 150);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = Color.FromArgb(89, 135, 214);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = Color.FromArgb(3, 56, 148);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.White;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = Color.FromArgb(0, 45, 150);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(123, 162, 231);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(99, 117, 214);
		}
	}

	private void SchemeXpOliveGreen()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = Color.FromArgb(239, 237, 222);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = Color.FromArgb(191, 188, 177);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = Color.FromArgb(153, 153, 153);
		}
		m_BarCaptionBackground2 = Color.Empty;
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = SystemColors.Control;
		}
		m_BarCaptionInactiveBackground2 = Color.Empty;
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = SystemColors.ControlText;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.FromArgb(64, 64, 64);
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = Color.FromArgb(153, 153, 153);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = Color.FromArgb(252, 252, 249);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = Color.FromArgb(138, 134, 122);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = Color.FromArgb(234, 235, 223);
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = SystemColors.Highlight;
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = Color.FromArgb(239, 237, 222);
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = Color.FromArgb(206, 209, 195);
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = SystemColors.Highlight;
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = Color.FromArgb(201, 208, 184);
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = SystemColors.Highlight;
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = Color.FromArgb(102, 102, 102);
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = Color.FromArgb(197, 194, 184);
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = Color.FromArgb(252, 252, 249);
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = SystemColors.Control;
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = Color.FromArgb(239, 237, 222);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = Color.FromArgb(230, 227, 210);
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = SystemColors.Highlight;
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackground2Custom)
		{
			m_MenuBarBackground2 = Color.Empty;
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.Empty;
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.Empty;
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.Empty;
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.Empty;
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.Empty;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = Color.Empty;
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.Empty;
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = Color.Empty;
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.Empty;
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = Color.Empty;
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = Color.Empty;
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = Color.Empty;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = Color.FromArgb(175, 192, 130);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = Color.FromArgb(99, 122, 68);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.White;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = Color.FromArgb(96, 128, 88);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = Color.FromArgb(175, 192, 130);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = Color.FromArgb(99, 122, 68);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.White;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = Color.FromArgb(96, 128, 88);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(204, 217, 173);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(165, 189, 132);
		}
	}

	private void SchemeXpSilver()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = Color.FromArgb(229, 228, 232);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = Color.FromArgb(179, 179, 182);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = Color.FromArgb(157, 157, 161);
		}
		m_BarCaptionBackground2 = Color.Empty;
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = SystemColors.Control;
		}
		m_BarCaptionInactiveBackground2 = Color.Empty;
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = SystemColors.ControlText;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.FromArgb(53, 53, 53);
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = Color.FromArgb(157, 157, 161);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = Color.FromArgb(251, 250, 251);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = Color.FromArgb(126, 126, 129);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = Color.FromArgb(233, 234, 237);
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = SystemColors.Highlight;
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = Color.FromArgb(229, 228, 232);
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = Color.FromArgb(199, 199, 202);
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = Color.FromArgb(169, 171, 181);
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = Color.FromArgb(210, 211, 216);
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = SystemColors.Highlight;
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = SystemColors.ControlText;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = Color.FromArgb(186, 186, 189);
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = Color.FromArgb(251, 250, 251);
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = SystemColors.Control;
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = SystemColors.ControlDark;
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = Color.FromArgb(229, 228, 232);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = Color.FromArgb(217, 216, 220);
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = SystemColors.Highlight;
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackground2Custom)
		{
			m_MenuBarBackground2 = Color.Empty;
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.Empty;
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.Empty;
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.Empty;
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.Empty;
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.Empty;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = Color.Empty;
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.Empty;
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = Color.Empty;
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.Empty;
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = Color.Empty;
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = Color.Empty;
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = Color.Empty;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = Color.FromArgb(168, 167, 191);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = Color.FromArgb(112, 111, 145);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.White;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = Color.FromArgb(124, 124, 148);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = Color.FromArgb(168, 167, 191);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = Color.FromArgb(112, 111, 145);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.White;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = Color.FromArgb(124, 124, 148);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(196, 200, 212);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(177, 179, 200);
		}
	}

	private void SchemeXpBlue2003()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = Color.FromArgb(223, 237, 254);
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = Color.FromArgb(142, 179, 231);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = Color.FromArgb(39, 65, 118);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = GetColor(6721760);
		}
		if (!m_BarCaptionBackground2Custom)
		{
			m_BarCaptionBackground2 = Color.FromArgb(42, 102, 201);
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = GetColor(6724095);
		}
		if (!m_BarCaptionInactiveBackground2Custom)
		{
			m_BarCaptionInactiveBackground2 = GetColor(5602242);
		}
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = Color.White;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.White;
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = Color.FromArgb(42, 102, 201);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = Color.FromArgb(246, 246, 246);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = Color.FromArgb(0, 45, 150);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = Color.FromArgb(255, 213, 140);
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.FromArgb(255, 173, 85);
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = Color.FromArgb(0, 0, 128);
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = Color.FromArgb(141, 141, 141);
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = Color.Empty;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = Color.FromArgb(227, 239, 255);
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.FromArgb(147, 181, 231);
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = Color.FromArgb(255, 244, 204);
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.FromArgb(255, 208, 145);
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = Color.FromArgb(0, 0, 128);
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = Color.FromArgb(254, 142, 75);
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.FromArgb(255, 207, 139);
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = Color.FromArgb(0, 0, 128);
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = SystemColors.ControlText;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = Color.FromArgb(106, 140, 203);
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.FromArgb(241, 249, 255);
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = Color.FromArgb(246, 246, 246);
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = Color.FromArgb(158, 190, 245);
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = Color.FromArgb(0, 45, 150);
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = Color.FromArgb(227, 239, 255);
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.FromArgb(135, 173, 228);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = Color.FromArgb(203, 221, 246);
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.FromArgb(121, 161, 220);
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = Color.FromArgb(0, 0, 128);
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = Color.FromArgb(59, 97, 156);
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = Color.FromArgb(158, 190, 245);
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.FromArgb(195, 218, 249);
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = Color.FromArgb(117, 166, 241);
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = Color.FromArgb(0, 53, 145);
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = SystemColors.ControlText;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = Color.FromArgb(89, 135, 214);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = Color.FromArgb(3, 56, 148);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.White;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = Color.FromArgb(0, 45, 150);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = Color.FromArgb(89, 135, 214);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = Color.FromArgb(3, 56, 148);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.White;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = Color.FromArgb(0, 45, 150);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(123, 162, 231);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(99, 117, 214);
		}
	}

	private void SchemeXpOliveGreen2003()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = Color.FromArgb(244, 247, 222);
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = Color.FromArgb(183, 198, 145);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = Color.FromArgb(81, 94, 51);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = Color.FromArgb(183, 198, 145);
		}
		if (!m_BarCaptionBackground2Custom)
		{
			m_BarCaptionBackground2 = Color.FromArgb(116, 134, 94);
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = GetColor(13753270);
		}
		if (!m_BarCaptionInactiveBackground2Custom)
		{
			m_BarCaptionInactiveBackground2 = Color.FromArgb(116, 134, 94);
		}
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = Color.White;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.White;
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = Color.FromArgb(116, 134, 94);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = Color.FromArgb(244, 244, 238);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = Color.FromArgb(117, 141, 94);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = Color.FromArgb(255, 213, 140);
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.FromArgb(255, 173, 85);
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = Color.FromArgb(63, 93, 56);
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = Color.FromArgb(141, 141, 141);
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = Color.Empty;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = Color.FromArgb(236, 240, 213);
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.FromArgb(194, 206, 159);
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = Color.FromArgb(255, 244, 204);
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.FromArgb(255, 208, 145);
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = Color.FromArgb(63, 93, 56);
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = Color.FromArgb(254, 142, 75);
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.FromArgb(255, 207, 139);
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = Color.FromArgb(63, 93, 56);
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = SystemColors.ControlText;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = Color.FromArgb(96, 128, 88);
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.FromArgb(244, 247, 222);
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = Color.FromArgb(244, 244, 238);
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = Color.FromArgb(217, 217, 168);
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = Color.FromArgb(117, 141, 94);
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = Color.FromArgb(255, 255, 237);
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.FromArgb(184, 199, 146);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = Color.FromArgb(230, 230, 239);
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.FromArgb(164, 180, 120);
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = Color.FromArgb(63, 93, 56);
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = Color.FromArgb(96, 128, 88);
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = Color.FromArgb(217, 217, 167);
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.FromArgb(242, 240, 228);
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = Color.FromArgb(176, 194, 140);
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = Color.FromArgb(96, 119, 107);
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = SystemColors.ControlText;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = Color.FromArgb(175, 192, 130);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = Color.FromArgb(99, 122, 68);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.White;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = Color.FromArgb(96, 128, 88);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = Color.FromArgb(175, 192, 130);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = Color.FromArgb(99, 122, 68);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.White;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = Color.FromArgb(96, 128, 88);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(204, 217, 173);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(165, 189, 132);
		}
	}

	private void SchemeXpSilver2003()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = Color.FromArgb(243, 244, 250);
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = Color.FromArgb(153, 151, 181);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = Color.FromArgb(84, 84, 117);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = Color.FromArgb(153, 151, 181);
		}
		if (!m_BarCaptionBackground2Custom)
		{
			m_BarCaptionBackground2 = Color.FromArgb(122, 121, 153);
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = GetColor(10921655);
		}
		if (!m_BarCaptionInactiveBackground2Custom)
		{
			m_BarCaptionInactiveBackground2 = Color.FromArgb(122, 121, 153);
		}
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = Color.White;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.White;
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = Color.FromArgb(122, 121, 153);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = Color.FromArgb(253, 250, 255);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = Color.FromArgb(124, 124, 148);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = Color.FromArgb(255, 213, 140);
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.FromArgb(255, 173, 85);
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = Color.FromArgb(75, 75, 111);
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = Color.FromArgb(141, 141, 141);
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = Color.Empty;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = Color.FromArgb(232, 233, 241);
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.FromArgb(177, 176, 198);
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = Color.FromArgb(255, 244, 204);
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.FromArgb(255, 208, 145);
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = Color.FromArgb(75, 75, 111);
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = Color.FromArgb(254, 142, 75);
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.FromArgb(255, 207, 139);
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = Color.FromArgb(75, 75, 111);
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = SystemColors.ControlText;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = Color.FromArgb(110, 109, 143);
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.White;
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = Color.FromArgb(253, 250, 255);
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = Color.FromArgb(215, 215, 229);
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = Color.FromArgb(124, 124, 148);
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = Color.FromArgb(249, 249, 255);
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.FromArgb(159, 157, 185);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = Color.FromArgb(215, 215, 226);
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.FromArgb(128, 126, 158);
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = Color.FromArgb(84, 84, 117);
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = Color.FromArgb(124, 124, 148);
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = Color.FromArgb(215, 215, 229);
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.FromArgb(243, 243, 247);
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = Color.FromArgb(179, 178, 200);
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = Color.FromArgb(118, 116, 146);
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = SystemColors.ControlText;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = Color.FromArgb(168, 167, 191);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = Color.FromArgb(112, 111, 145);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.White;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = Color.FromArgb(124, 124, 148);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = Color.FromArgb(168, 167, 191);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = Color.FromArgb(112, 111, 145);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.White;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = Color.FromArgb(124, 124, 148);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(196, 200, 212);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(177, 179, 200);
		}
	}

	private void SchemeXpBlueVS2005()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = Color.FromArgb(251, 250, 247);
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = Color.FromArgb(181, 181, 154);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = Color.FromArgb(193, 190, 179);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = Color.FromArgb(0, 84, 227);
		}
		if (!m_BarCaptionBackground2Custom)
		{
			m_BarCaptionBackground2 = Color.FromArgb(60, 148, 254);
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = GetColor(13420474);
		}
		if (!m_BarCaptionInactiveBackground2Custom)
		{
			m_BarCaptionInactiveBackground2 = Color.Empty;
		}
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = Color.Black;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.White;
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = Color.FromArgb(146, 143, 130);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = Color.FromArgb(252, 252, 249);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = Color.FromArgb(138, 134, 122);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = Color.FromArgb(225, 230, 232);
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = Color.FromArgb(49, 106, 197);
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = Color.FromArgb(180, 177, 163);
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = Color.Empty;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = Color.FromArgb(251, 250, 247);
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.Empty;
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = Color.FromArgb(193, 210, 238);
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.Empty;
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = Color.FromArgb(49, 106, 197);
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = Color.FromArgb(152, 181, 226);
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.Empty;
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = Color.FromArgb(75, 75, 111);
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = SystemColors.ControlText;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = Color.FromArgb(197, 194, 184);
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.Empty;
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = Color.FromArgb(252, 252, 249);
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = Color.FromArgb(229, 229, 215);
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = Color.FromArgb(138, 134, 122);
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = m_MenuBackground;
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.FromArgb(186, 186, 160);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = m_MenuBackground;
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = ControlPaint.Light(Color.FromArgb(186, 186, 160));
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = Color.Black;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = Color.FromArgb(146, 146, 118);
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = Color.FromArgb(229, 229, 215);
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.FromArgb(250, 250, 247);
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = Color.FromArgb(239, 238, 235);
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = Color.FromArgb(152, 152, 126);
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = SystemColors.ControlText;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = GetColor(12566434);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = Color.FromArgb(152, 152, 126);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.Black;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = Color.FromArgb(82, 82, 79);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = GetColor(12566434);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = Color.FromArgb(152, 152, 126);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.Black;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = Color.FromArgb(82, 82, 79);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(123, 162, 231);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(99, 117, 214);
		}
	}

	private void SchemeXpOliveGreenVS2005()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = GetColor(16448246);
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = GetColor(15592155);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = GetColor(12697267);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = GetColor(9150825);
		}
		if (!m_BarCaptionBackground2Custom)
		{
			m_BarCaptionBackground2 = GetColor(12964257);
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = GetColor(13420474);
		}
		if (!m_BarCaptionInactiveBackground2Custom)
		{
			m_BarCaptionInactiveBackground2 = Color.Empty;
		}
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = Color.Black;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.White;
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = GetColor(9604994);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = GetColor(16579833);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = GetColor(9078394);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = GetColor(11978381);
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = GetColor(9674864);
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = GetColor(11841955);
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = Color.Empty;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = GetColor(16382452);
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = Color.Empty;
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = GetColor(11978381);
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = Color.Empty;
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = GetColor(9674864);
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = GetColor(9674864);
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = Color.Empty;
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = GetColor(9674864);
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = Color.White;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = GetColor(12960440);
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.Empty;
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = GetColor(16579833);
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = GetColor(15526360);
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = GetColor(9078394);
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = GetColor(16579833);
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = GetColor(15592155);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = m_MenuSide;
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = ControlPaint.Light(m_MenuSide2);
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = Color.Black;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = GetColor(15723998);
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = GetColor(15526360);
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = GetColor(16447990);
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = GetColor(15724267);
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = GetColor(11315353);
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = SystemColors.ControlText;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = GetColor(15987434);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = GetColor(15000277);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.Black;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = GetColor(11315353);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = GetColor(15987434);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = GetColor(15000277);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.Black;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = GetColor(11315353);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(204, 217, 173);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(165, 189, 132);
		}
	}

	private void SchemeXpSilverVS2005()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = GetColor(15987962);
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = GetColor(10065845);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = GetColor(5526645);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = GetColor(10526394);
		}
		if (!m_BarCaptionBackground2Custom)
		{
			m_BarCaptionBackground2 = GetColor(14803692);
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = GetColor(13553358);
		}
		if (!m_BarCaptionInactiveBackground2Custom)
		{
			m_BarCaptionInactiveBackground2 = GetColor(15921910);
		}
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = Color.Black;
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = Color.Black;
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = GetColor(8026521);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = GetColor(16644863);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = GetColor(8158356);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = GetColor(16766348);
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = GetColor(16756053);
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = GetColor(4934511);
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = SystemColors.ControlText;
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = GetColor(9276813);
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = Color.Empty;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = GetColor(15264241);
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = GetColor(12237261);
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = SystemColors.ControlText;
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = GetColor(16774348);
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = GetColor(16765073);
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = GetColor(4934511);
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = SystemColors.ControlText;
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = GetColor(16683342);
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = GetColor(16765838);
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = GetColor(4934511);
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = Color.Black;
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = GetColor(7237007);
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.Empty;
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = SystemColors.ControlText;
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = GetColor(16644863);
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = GetColor(14145509);
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = GetColor(8158356);
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = GetColor(16382463);
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = GetColor(10460601);
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = m_MenuSide;
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = ControlPaint.Light(m_MenuSide2);
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = Color.Black;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = GetColor(8158356);
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = GetColor(14145509);
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = GetColor(15987703);
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = GetColor(11776712);
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = GetColor(7960468);
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = SystemColors.ControlText;
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = GetColor(15658734);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = GetColor(16777215);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = Color.Black;
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = GetColor(10329505);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = GetColor(15658734);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = GetColor(16777215);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = Color.Black;
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = GetColor(10329505);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(196, 200, 212);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(177, 179, 200);
		}
	}

	private void SchemeOffice2007()
	{
		if (!m_BarBackgroundCustom)
		{
			m_BarBackground = GetColor(14938111);
		}
		if (!m_BarBackground2Custom)
		{
			m_BarBackground2 = GetColor(11128574);
		}
		if (!m_BarStripeColorCustom)
		{
			m_BarStripeColor = GetColor(7314905);
		}
		if (!m_BarCaptionBackgroundCustom)
		{
			m_BarCaptionBackground = GetColor(6656975);
		}
		if (!m_BarCaptionBackground2Custom)
		{
			m_BarCaptionBackground2 = GetColor(3630240);
		}
		if (!m_BarCaptionInactiveBackgroundCustom)
		{
			m_BarCaptionInactiveBackground = GetColor(14938111);
		}
		if (!m_BarCaptionInactiveBackground2Custom)
		{
			m_BarCaptionInactiveBackground2 = GetColor(11522815);
		}
		if (!m_BarCaptionInactiveTextCustom)
		{
			m_BarCaptionInactiveText = GetColor(538482);
		}
		if (!m_BarCaptionTextCustom)
		{
			m_BarCaptionText = GetColor(16777215);
		}
		if (!m_BarFloatingBorderCustom)
		{
			m_BarFloatingBorder = GetColor(3630240);
		}
		if (!m_BarPopupBackgroundCustom)
		{
			m_BarPopupBackground = GetColor(16185078);
		}
		if (!m_BarPopupBorderCustom)
		{
			m_BarPopupBorder = GetColor(6656975);
		}
		if (!m_ItemBackgroundCustom)
		{
			m_ItemBackground = Color.Empty;
		}
		if (!m_ItemBackground2Custom)
		{
			m_ItemBackground2 = Color.Empty;
		}
		if (!m_ItemCheckedBackgroundCustom)
		{
			m_ItemCheckedBackground = GetColor(16569720);
		}
		if (!m_ItemCheckedBackground2Custom)
		{
			m_ItemCheckedBackground2 = GetColor(16500815);
		}
		if (!m_ItemCheckedBorderCustom)
		{
			m_ItemCheckedBorder = GetColor(12276995);
		}
		if (!m_ItemCheckedTextCustom)
		{
			m_ItemCheckedText = GetColor(0);
		}
		if (!m_ItemDisabledBackgroundCustom)
		{
			m_ItemDisabledBackground = Color.Empty;
		}
		if (!m_ItemDisabledTextCustom)
		{
			m_ItemDisabledText = GetColor(9276813);
		}
		if (!m_ItemExpandedShadowCustom)
		{
			m_ItemExpandedShadow = Color.Empty;
		}
		if (!m_ItemExpandedBackgroundCustom)
		{
			m_ItemExpandedBackground = GetColor(14938110);
		}
		if (!m_ItemExpandedBackground2Custom)
		{
			m_ItemExpandedBackground2 = GetColor(10076144);
		}
		if (!m_ItemExpandedTextCustom)
		{
			m_ItemExpandedText = GetColor(0);
		}
		if (!m_ItemHotBackgroundCustom)
		{
			m_ItemHotBackground = GetColor(16774604);
		}
		if (!m_ItemHotBackground2Custom)
		{
			m_ItemHotBackground2 = GetColor(16767861);
		}
		if (!m_ItemHotBorderCustom)
		{
			m_ItemHotBorder = GetColor(16760169);
		}
		if (!m_ItemHotTextCustom)
		{
			m_ItemHotText = GetColor(0);
		}
		if (!m_ItemPressedBackgroundCustom)
		{
			m_ItemPressedBackground = GetColor(16553789);
		}
		if (!m_ItemPressedBackground2Custom)
		{
			m_ItemPressedBackground2 = GetColor(16758878);
		}
		if (!m_ItemPressedBorderCustom)
		{
			m_ItemPressedBorder = GetColor(16485436);
		}
		if (!m_ItemPressedTextCustom)
		{
			m_ItemPressedText = GetColor(0);
		}
		if (!m_ItemSeparatorCustom)
		{
			m_ItemSeparator = Color.FromArgb(80, GetColor(10143487));
		}
		if (!m_ItemSeparatorShadeCustom)
		{
			m_ItemSeparatorShade = Color.FromArgb(250, GetColor(16777215));
		}
		if (!m_ItemTextCustom)
		{
			m_ItemText = GetColor(0);
		}
		if (!m_MenuBackgroundCustom)
		{
			m_MenuBackground = GetColor(16185078);
		}
		if (!m_MenuBackground2Custom)
		{
			m_MenuBackground2 = Color.Empty;
		}
		if (!m_MenuBarBackgroundCustom)
		{
			m_MenuBarBackground = GetColor(12573695);
		}
		if (!m_MenuBorderCustom)
		{
			m_MenuBorder = GetColor(6656975);
		}
		if (!m_ItemExpandedBorderCustom)
		{
			m_ItemExpandedBorder = m_MenuBorder;
		}
		if (!m_MenuSideCustom)
		{
			m_MenuSide = GetColor(15331054);
		}
		if (!m_MenuSide2Custom)
		{
			m_MenuSide2 = Color.Empty;
		}
		if (!m_MenuUnusedBackgroundCustom)
		{
			m_MenuUnusedBackground = m_MenuBackground;
		}
		if (!m_MenuUnusedSideCustom)
		{
			m_MenuUnusedSide = GetColor(14342874);
		}
		if (!m_MenuUnusedSide2Custom)
		{
			m_MenuUnusedSide2 = Color.Empty;
		}
		if (!m_ItemDesignTimeBorderCustom)
		{
			m_ItemDesignTimeBorder = Color.Black;
		}
		if (!m_BarDockedBorderCustom)
		{
			m_BarDockedBorder = GetColor(7314905);
		}
		if (!m_DockSiteBackColorCustom)
		{
			m_DockSiteBackColor = GetColor(12573695);
		}
		if (!m_DockSiteBackColor2Custom)
		{
			m_DockSiteBackColor2 = Color.Empty;
		}
		if (!m_CustomizeBackgroundCustom)
		{
			m_CustomizeBackground = GetColor(14149887);
		}
		if (!m_CustomizeBackground2Custom)
		{
			m_CustomizeBackground2 = GetColor(7314905);
		}
		if (!m_CustomizeTextCustom)
		{
			m_CustomizeText = GetColor(0);
		}
		if (!m_PanelBackgroundCustom)
		{
			m_PanelBackground = GetColor(14938111);
		}
		if (!m_PanelBackground2Custom)
		{
			m_PanelBackground2 = GetColor(11522815);
		}
		if (!m_PanelTextCustom)
		{
			m_PanelText = GetColor(538482);
		}
		if (!m_PanelBorderCustom)
		{
			m_PanelBorder = GetColor(6656975);
		}
		if (!m_SplitterBackgroundCustom)
		{
			m_SplitterBackground = GetColor(14938111);
		}
		if (!m_SplitterBackground2Custom)
		{
			m_SplitterBackground2 = GetColor(11522815);
		}
		if (!m_SplitterTextCustom)
		{
			m_SplitterText = GetColor(538482);
		}
		if (!m_SplitterBorderCustom)
		{
			m_SplitterBorder = GetColor(6656975);
		}
		if (!m_ExplorerBarBackgroundCustom)
		{
			m_ExplorerBarBackground = Color.FromArgb(196, 200, 212);
		}
		if (!m_ExplorerBarBackground2Custom)
		{
			m_ExplorerBarBackground2 = Color.FromArgb(177, 179, 200);
		}
	}

	public static Color GetColor(string rgbHex)
	{
		if (string.IsNullOrEmpty(rgbHex))
		{
			return Color.Empty;
		}
		if (rgbHex.Length == 8)
		{
			return Color.FromArgb(Convert.ToInt32(rgbHex.Substring(0, 2), 16), Convert.ToInt32(rgbHex.Substring(2, 2), 16), Convert.ToInt32(rgbHex.Substring(4, 2), 16), Convert.ToInt32(rgbHex.Substring(6, 2), 16));
		}
		return Color.FromArgb(Convert.ToInt32(rgbHex.Substring(0, 2), 16), Convert.ToInt32(rgbHex.Substring(2, 2), 16), Convert.ToInt32(rgbHex.Substring(4, 2), 16));
	}

	public static Color GetColor(int rgb)
	{
		if (rgb == -1)
		{
			return Color.Empty;
		}
		return Color.FromArgb((rgb & 0xFF0000) >> 16, (rgb & 0xFF00) >> 8, rgb & 0xFF);
	}

	public static Color GetColor(int alpha, int rgb)
	{
		if (rgb == -1)
		{
			return Color.Empty;
		}
		return Color.FromArgb(alpha, (rgb & 0xFF0000) >> 16, (rgb & 0xFF00) >> 8, rgb & 0xFF);
	}

	public void Serialize(XmlElement xmlElem)
	{
		if (m_BarBackgroundCustom)
		{
			xmlElem.SetAttribute("barback", BarFunctions.ColorToString(m_BarBackground));
		}
		if (m_BarBackground2Custom)
		{
			xmlElem.SetAttribute("barback2", BarFunctions.ColorToString(m_BarBackground2));
		}
		if (m_BarBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("barbackga", m_BarBackgroundGradientAngle.ToString());
		}
		if (m_BarStripeColorCustom)
		{
			xmlElem.SetAttribute("barstripeclr", BarFunctions.ColorToString(m_BarStripeColor));
		}
		if (m_BarCaptionBackgroundCustom)
		{
			xmlElem.SetAttribute("barcapback", BarFunctions.ColorToString(m_BarCaptionBackground));
		}
		if (m_BarCaptionBackground2Custom)
		{
			xmlElem.SetAttribute("barcapback2", BarFunctions.ColorToString(m_BarCaptionBackground2));
		}
		if (m_BarCaptionBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("barcapbackga", m_BarCaptionBackgroundGradientAngle.ToString());
		}
		if (m_BarCaptionInactiveBackgroundCustom)
		{
			xmlElem.SetAttribute("barcapiback", BarFunctions.ColorToString(m_BarCaptionInactiveBackground));
		}
		if (m_BarCaptionInactiveBackground2Custom)
		{
			xmlElem.SetAttribute("barcapiback2", BarFunctions.ColorToString(m_BarCaptionInactiveBackground2));
		}
		if (m_BarCaptionInactiveBackgroundGAngle != 90)
		{
			xmlElem.SetAttribute("barcapibackga", m_BarCaptionInactiveBackgroundGAngle.ToString());
		}
		if (m_BarDockedBorderCustom)
		{
			xmlElem.SetAttribute("bardockborder", BarFunctions.ColorToString(m_BarDockedBorder));
		}
		if (m_BarCaptionInactiveTextCustom)
		{
			xmlElem.SetAttribute("barcapitext", BarFunctions.ColorToString(m_BarCaptionInactiveText));
		}
		if (m_BarCaptionTextCustom)
		{
			xmlElem.SetAttribute("barcaptext", BarFunctions.ColorToString(m_BarCaptionText));
		}
		if (m_BarFloatingBorderCustom)
		{
			xmlElem.SetAttribute("barfloatb", BarFunctions.ColorToString(m_BarFloatingBorder));
		}
		if (m_BarPopupBackgroundCustom)
		{
			xmlElem.SetAttribute("barpopupback", BarFunctions.ColorToString(m_BarPopupBackground));
		}
		if (m_BarPopupBorderCustom)
		{
			xmlElem.SetAttribute("barpopupb", BarFunctions.ColorToString(m_BarPopupBorder));
		}
		if (m_ItemBackgroundCustom)
		{
			xmlElem.SetAttribute("itemback", BarFunctions.ColorToString(m_ItemBackground));
		}
		if (m_ItemBackground2Custom)
		{
			xmlElem.SetAttribute("itemback2", BarFunctions.ColorToString(m_ItemBackground2));
		}
		if (m_ItemBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("itembackga", m_ItemBackgroundGradientAngle.ToString());
		}
		if (m_ItemCheckedBackgroundCustom)
		{
			xmlElem.SetAttribute("itemchkback", BarFunctions.ColorToString(m_ItemCheckedBackground));
		}
		if (m_ItemCheckedBackground2Custom)
		{
			xmlElem.SetAttribute("itemchkback2", BarFunctions.ColorToString(m_ItemCheckedBackground2));
		}
		if (m_ItemCheckedBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("itemchkbackga", m_ItemCheckedBackgroundGradientAngle.ToString());
		}
		if (m_ItemCheckedBorderCustom)
		{
			xmlElem.SetAttribute("itemchkb", BarFunctions.ColorToString(m_ItemCheckedBorder));
		}
		if (m_ItemCheckedTextCustom)
		{
			xmlElem.SetAttribute("itemchktext", BarFunctions.ColorToString(m_ItemCheckedText));
		}
		if (m_ItemDisabledBackgroundCustom && !m_ItemDisabledBackground.IsEmpty)
		{
			xmlElem.SetAttribute("itemdisback", BarFunctions.ColorToString(m_ItemDisabledBackground));
		}
		if (m_ItemDisabledTextCustom)
		{
			xmlElem.SetAttribute("itemdistext", BarFunctions.ColorToString(m_ItemDisabledText));
		}
		if (m_ItemExpandedShadowCustom)
		{
			xmlElem.SetAttribute("itemexpshadow", BarFunctions.ColorToString(m_ItemExpandedShadow));
		}
		if (m_ItemExpandedBackgroundCustom)
		{
			xmlElem.SetAttribute("itemexpback", BarFunctions.ColorToString(m_ItemExpandedBackground));
		}
		if (m_ItemExpandedBackground2Custom)
		{
			xmlElem.SetAttribute("itemexpback2", BarFunctions.ColorToString(m_ItemExpandedBackground2));
		}
		if (m_ItemExpandedBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("itemexpbackga", m_ItemExpandedBackgroundGradientAngle.ToString());
		}
		if (m_ItemExpandedTextCustom)
		{
			xmlElem.SetAttribute("itemexptext", BarFunctions.ColorToString(m_ItemExpandedText));
		}
		if (m_ItemExpandedBorderCustom)
		{
			xmlElem.SetAttribute("itemexpborder", BarFunctions.ColorToString(m_ItemExpandedBorder));
		}
		if (m_ItemHotBackgroundCustom)
		{
			xmlElem.SetAttribute("itemhotback", BarFunctions.ColorToString(m_ItemHotBackground));
		}
		if (m_ItemHotBackground2Custom)
		{
			xmlElem.SetAttribute("itemhotback2", BarFunctions.ColorToString(m_ItemHotBackground2));
		}
		if (m_ItemHotBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("itemhotbackga", m_ItemHotBackgroundGradientAngle.ToString());
		}
		if (m_ItemHotBorderCustom)
		{
			xmlElem.SetAttribute("itemhotb", BarFunctions.ColorToString(m_ItemHotBorder));
		}
		if (m_ItemHotTextCustom)
		{
			xmlElem.SetAttribute("itemhottext", BarFunctions.ColorToString(m_ItemHotText));
		}
		if (m_ItemPressedBackgroundCustom)
		{
			xmlElem.SetAttribute("itempressback", BarFunctions.ColorToString(m_ItemPressedBackground));
		}
		if (m_ItemPressedBackground2Custom)
		{
			xmlElem.SetAttribute("itempressback2", BarFunctions.ColorToString(m_ItemPressedBackground2));
		}
		if (m_ItemPressedBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("itempressbackga", m_ItemPressedBackgroundGradientAngle.ToString());
		}
		if (m_ItemPressedBorderCustom)
		{
			xmlElem.SetAttribute("itempressb", BarFunctions.ColorToString(m_ItemPressedBorder));
		}
		if (m_ItemPressedTextCustom)
		{
			xmlElem.SetAttribute("itempresstext", BarFunctions.ColorToString(m_ItemPressedText));
		}
		if (m_ItemSeparatorCustom)
		{
			xmlElem.SetAttribute("itemsep", BarFunctions.ColorToString(m_ItemSeparator));
		}
		if (m_ItemTextCustom)
		{
			xmlElem.SetAttribute("itemtext", BarFunctions.ColorToString(m_ItemText));
		}
		if (m_MenuBackgroundCustom)
		{
			xmlElem.SetAttribute("menuback", BarFunctions.ColorToString(m_MenuBackground));
		}
		if (m_MenuBackground2Custom)
		{
			xmlElem.SetAttribute("menuback2", BarFunctions.ColorToString(m_MenuBackground2));
		}
		if (m_MenuBackgroundGradientAngle != 0)
		{
			xmlElem.SetAttribute("menubackga", m_MenuBackgroundGradientAngle.ToString());
		}
		if (m_MenuBarBackgroundCustom)
		{
			xmlElem.SetAttribute("menubarback", BarFunctions.ColorToString(m_MenuBarBackground));
		}
		if (m_MenuBarBackground2Custom)
		{
			xmlElem.SetAttribute("menubarback2", BarFunctions.ColorToString(m_MenuBarBackground2));
		}
		if (m_MenuBarBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("menubarbackga", m_MenuBarBackgroundGradientAngle.ToString());
		}
		if (m_MenuBorderCustom)
		{
			xmlElem.SetAttribute("menub", BarFunctions.ColorToString(m_MenuBorder));
		}
		if (m_MenuSideCustom)
		{
			xmlElem.SetAttribute("menuside", BarFunctions.ColorToString(m_MenuSide));
		}
		if (m_MenuSide2Custom)
		{
			xmlElem.SetAttribute("menuside2", BarFunctions.ColorToString(m_MenuSide2));
		}
		if (m_MenuSideGradientAngle != 0)
		{
			xmlElem.SetAttribute("menusidega", m_MenuSideGradientAngle.ToString());
		}
		if (m_MenuUnusedBackgroundCustom)
		{
			xmlElem.SetAttribute("menuuback", BarFunctions.ColorToString(m_MenuUnusedBackground));
		}
		if (m_MenuUnusedSideCustom)
		{
			xmlElem.SetAttribute("menuuside", BarFunctions.ColorToString(m_MenuUnusedSide));
		}
		if (m_MenuUnusedSide2Custom)
		{
			xmlElem.SetAttribute("menuuside2", BarFunctions.ColorToString(m_MenuUnusedSide2));
		}
		if (m_MenuSideGradientAngle != 0)
		{
			xmlElem.SetAttribute("menuusidega", m_MenuSideGradientAngle.ToString());
		}
		if (m_ItemDesignTimeBorderCustom)
		{
			xmlElem.SetAttribute("menudtb", BarFunctions.ColorToString(m_ItemDesignTimeBorder));
		}
		if (m_CustomizeBackgroundCustom)
		{
			xmlElem.SetAttribute("customback", BarFunctions.ColorToString(m_CustomizeBackground));
		}
		if (m_CustomizeBackground2Custom)
		{
			xmlElem.SetAttribute("customback2", BarFunctions.ColorToString(m_CustomizeBackground2));
		}
		if (m_CustomizeTextCustom)
		{
			xmlElem.SetAttribute("customtext", BarFunctions.ColorToString(m_CustomizeText));
		}
		if (m_CustomizeBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("custombackga", m_CustomizeBackgroundGradientAngle.ToString());
		}
		if (m_PanelBackgroundCustom)
		{
			xmlElem.SetAttribute("panelback", BarFunctions.ColorToString(m_PanelBackground));
		}
		if (m_PanelBackground2Custom)
		{
			xmlElem.SetAttribute("panelback2", BarFunctions.ColorToString(m_PanelBackground2));
		}
		if (m_PanelBorderCustom)
		{
			xmlElem.SetAttribute("panelborder", BarFunctions.ColorToString(m_PanelBorder));
		}
		if (m_PanelTextCustom)
		{
			xmlElem.SetAttribute("paneltext", BarFunctions.ColorToString(m_PanelText));
		}
		if (m_PanelBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("panelbackga", m_PanelBackgroundGradientAngle.ToString());
		}
		if (m_SplitterBackgroundCustom)
		{
			xmlElem.SetAttribute("splitterback", BarFunctions.ColorToString(m_SplitterBackground));
		}
		if (m_SplitterBackground2Custom)
		{
			xmlElem.SetAttribute("splitterback2", BarFunctions.ColorToString(m_SplitterBackground2));
		}
		if (m_SplitterBorderCustom)
		{
			xmlElem.SetAttribute("splitterborder", BarFunctions.ColorToString(m_SplitterBorder));
		}
		if (m_SplitterTextCustom)
		{
			xmlElem.SetAttribute("splittertext", BarFunctions.ColorToString(m_SplitterText));
		}
		if (m_SplitterBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("splitterbackga", m_SplitterBackgroundGradientAngle.ToString());
		}
		if (m_ExplorerBarBackgroundCustom)
		{
			xmlElem.SetAttribute("exbarback", BarFunctions.ColorToString(m_ExplorerBarBackground));
		}
		if (m_ExplorerBarBackground2Custom)
		{
			xmlElem.SetAttribute("exbarback2", BarFunctions.ColorToString(m_ExplorerBarBackground2));
		}
		if (m_ExplorerBarBackgroundGradientAngle != 90)
		{
			xmlElem.SetAttribute("exbarbackga", m_ExplorerBarBackgroundGradientAngle.ToString());
		}
		if (m_DockSiteBackColorCustom)
		{
			xmlElem.SetAttribute("dsback", BarFunctions.ColorToString(m_DockSiteBackColor));
		}
		if (m_DockSiteBackColor2Custom)
		{
			xmlElem.SetAttribute("dsback2", BarFunctions.ColorToString(m_DockSiteBackColor2));
		}
		if (m_DockSiteBackColorGradientAngle != 0)
		{
			xmlElem.SetAttribute("dsbackga", m_DockSiteBackColorGradientAngle.ToString());
		}
		if (!m_MdiSystemItemForeground.IsEmpty)
		{
			xmlElem.SetAttribute("mdisystemitemforeground", BarFunctions.ColorToString(m_MdiSystemItemForeground));
		}
		if (m_PredefinedColorScheme != 0)
		{
			xmlElem.SetAttribute("predefcolorscheme", XmlConvert.ToString((int)m_PredefinedColorScheme));
		}
	}

	public void Deserialize(XmlElement xmlElem)
	{
		if (xmlElem.HasAttribute("predefcolorscheme"))
		{
			m_PredefinedColorScheme = (ePredefinedColorScheme)XmlConvert.ToInt32(xmlElem.GetAttribute("predefcolorscheme"));
		}
		Refresh();
		if (xmlElem.HasAttribute("barback"))
		{
			m_BarBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("barback"));
			m_BarBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("barback2"))
		{
			m_BarBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("barback2"));
			m_BarBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("barbackga"))
		{
			m_BarBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("barbackga"));
		}
		if (xmlElem.HasAttribute("barstripeclr"))
		{
			m_BarStripeColor = BarFunctions.ColorFromString(xmlElem.GetAttribute("barstripeclr"));
			m_BarStripeColorCustom = true;
		}
		if (xmlElem.HasAttribute("barcapback"))
		{
			m_BarCaptionBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapback"));
			m_BarCaptionBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("barcapback2"))
		{
			m_BarCaptionBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapback2"));
			m_BarCaptionBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("barcapbackga"))
		{
			m_BarCaptionBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("barcapbackga"));
		}
		if (xmlElem.HasAttribute("barcapiback"))
		{
			m_BarCaptionInactiveBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapiback"));
			m_BarCaptionInactiveBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("barcapiback2"))
		{
			m_BarCaptionInactiveBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapiback2"));
			m_BarCaptionInactiveBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("barcapibackga"))
		{
			m_BarCaptionInactiveBackgroundGAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("barcapibackga"));
		}
		if (xmlElem.HasAttribute("barcapitext"))
		{
			m_BarCaptionInactiveText = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapitext"));
			m_BarCaptionInactiveTextCustom = true;
		}
		if (xmlElem.HasAttribute("barcaptext"))
		{
			m_BarCaptionText = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcaptext"));
			m_BarCaptionTextCustom = true;
		}
		if (xmlElem.HasAttribute("barfloatb"))
		{
			m_BarFloatingBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("barfloatb"));
			m_BarFloatingBorderCustom = true;
		}
		if (xmlElem.HasAttribute("bardockborder"))
		{
			m_BarDockedBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("bardockborder"));
			m_BarDockedBorderCustom = true;
		}
		if (xmlElem.HasAttribute("barpopupback"))
		{
			m_BarPopupBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("barpopupback"));
			m_BarPopupBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("barpopupb"))
		{
			m_BarPopupBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("barpopupb"));
			m_BarPopupBorderCustom = true;
		}
		if (xmlElem.HasAttribute("itemback"))
		{
			m_ItemBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemback"));
			m_ItemBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("itemback2"))
		{
			m_ItemBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemback2"));
			m_ItemBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("itembackga"))
		{
			m_ItemBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("itembackga"));
		}
		if (xmlElem.HasAttribute("itemchkback"))
		{
			m_ItemCheckedBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemchkback"));
			m_ItemCheckedBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("itemchkback2"))
		{
			m_ItemCheckedBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemchkback2"));
			m_ItemCheckedBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("itemchkbackga"))
		{
			m_ItemCheckedBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("itemchkbackga"));
		}
		if (xmlElem.HasAttribute("itemchkb"))
		{
			m_ItemCheckedBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemchkb"));
			m_ItemCheckedBorderCustom = true;
		}
		if (xmlElem.HasAttribute("itemchktext"))
		{
			m_ItemCheckedText = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemchktext"));
			m_ItemCheckedTextCustom = true;
		}
		if (xmlElem.HasAttribute("itemdisback"))
		{
			m_ItemDisabledBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemdisback"));
			m_ItemDisabledBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("itemdistext"))
		{
			m_ItemDisabledText = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemdistext"));
			m_ItemDisabledTextCustom = true;
		}
		if (xmlElem.HasAttribute("itemexpshadow"))
		{
			m_ItemExpandedShadow = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexpshadow"));
			m_ItemExpandedShadowCustom = true;
		}
		if (xmlElem.HasAttribute("itemexpback"))
		{
			m_ItemExpandedBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexpback"));
			m_ItemExpandedBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("itemexpback2"))
		{
			m_ItemExpandedBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexpback2"));
			m_ItemExpandedBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("itemexpbackga"))
		{
			m_ItemExpandedBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("itemexpbackga"));
		}
		if (xmlElem.HasAttribute("itemexptext"))
		{
			m_ItemExpandedText = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexptext"));
			m_ItemExpandedTextCustom = true;
		}
		if (xmlElem.HasAttribute("itemexpborder"))
		{
			m_ItemExpandedBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexpborder"));
			m_ItemExpandedBorderCustom = true;
		}
		if (xmlElem.HasAttribute("itemhotback"))
		{
			m_ItemHotBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemhotback"));
			m_ItemHotBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("itemhotback2"))
		{
			m_ItemHotBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemhotback2"));
			m_ItemHotBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("itemhotbackga"))
		{
			m_ItemHotBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("itemhotbackga"));
		}
		if (xmlElem.HasAttribute("itemhotb"))
		{
			m_ItemHotBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemhotb"));
			m_ItemHotBorderCustom = true;
		}
		if (xmlElem.HasAttribute("itemhottext"))
		{
			m_ItemHotText = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemhottext"));
			m_ItemHotTextCustom = true;
		}
		if (xmlElem.HasAttribute("itempressback"))
		{
			m_ItemPressedBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("itempressback"));
			m_ItemPressedBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("itempressback2"))
		{
			m_ItemPressedBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itempressback2"));
			m_ItemPressedBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("itempressbackga"))
		{
			m_ItemPressedBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("itempressbackga"));
		}
		if (xmlElem.HasAttribute("itempressb"))
		{
			m_ItemPressedBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("itempressb"));
			m_ItemPressedBorderCustom = true;
		}
		if (xmlElem.HasAttribute("itempresstext"))
		{
			m_ItemPressedText = BarFunctions.ColorFromString(xmlElem.GetAttribute("itempresstext"));
			m_ItemPressedTextCustom = true;
		}
		if (xmlElem.HasAttribute("itemsep"))
		{
			m_ItemSeparator = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemsep"));
			m_ItemSeparatorCustom = true;
		}
		if (xmlElem.HasAttribute("itemtext"))
		{
			m_ItemText = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemtext"));
			m_ItemTextCustom = true;
		}
		if (xmlElem.HasAttribute("menuback"))
		{
			m_MenuBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuback"));
			m_MenuBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("menuback2"))
		{
			m_MenuBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuback2"));
			m_MenuBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("menubackga"))
		{
			m_MenuBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("menubackga"));
		}
		if (xmlElem.HasAttribute("menubarback"))
		{
			m_MenuBarBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("menubarback"));
			m_MenuBarBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("menubarback2"))
		{
			m_MenuBarBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("menubarback2"));
			m_MenuBarBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("menubarbackga"))
		{
			m_MenuBarBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("menubarbackga"));
		}
		else
		{
			m_MenuBarBackgroundGradientAngle = 90;
		}
		if (xmlElem.HasAttribute("menub"))
		{
			m_MenuBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("menub"));
			m_MenuBorderCustom = true;
		}
		if (xmlElem.HasAttribute("menuside"))
		{
			m_MenuSide = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuside"));
			m_MenuSideCustom = true;
		}
		if (xmlElem.HasAttribute("menuside2"))
		{
			m_MenuSide2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuside2"));
			m_MenuSide2Custom = true;
		}
		if (xmlElem.HasAttribute("menusidega"))
		{
			m_MenuSideGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("menusidega"));
		}
		if (xmlElem.HasAttribute("menuuback"))
		{
			m_MenuUnusedBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuuback"));
			m_MenuUnusedBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("menuuside"))
		{
			m_MenuUnusedSide = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuuside"));
			m_MenuUnusedSideCustom = true;
		}
		if (xmlElem.HasAttribute("menuuside2"))
		{
			m_MenuUnusedSide2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuuside2"));
			m_MenuUnusedSide2Custom = true;
		}
		if (xmlElem.HasAttribute("menuusidega"))
		{
			m_MenuUnusedSideGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("menuusidega"));
		}
		if (xmlElem.HasAttribute("menudtb"))
		{
			m_ItemDesignTimeBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("menudtb"));
			m_ItemDesignTimeBorderCustom = true;
		}
		if (xmlElem.HasAttribute("customback"))
		{
			m_CustomizeBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("customback"));
			m_CustomizeBackgroundCustom = true;
		}
		if (xmlElem.HasAttribute("customback2"))
		{
			m_CustomizeBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("customback2"));
			m_CustomizeBackground2Custom = true;
		}
		if (xmlElem.HasAttribute("customtext"))
		{
			m_CustomizeText = BarFunctions.ColorFromString(xmlElem.GetAttribute("customtext"));
			m_CustomizeTextCustom = true;
		}
		if (xmlElem.HasAttribute("custombackga"))
		{
			m_CustomizeBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("custombackga"));
		}
		if (xmlElem.HasAttribute("panelback"))
		{
			m_PanelBackgroundCustom = true;
			m_PanelBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("panelback"));
		}
		if (xmlElem.HasAttribute("panelback2"))
		{
			m_PanelBackground2Custom = true;
			m_PanelBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("panelback2"));
		}
		if (xmlElem.HasAttribute("panelborder"))
		{
			m_PanelBorderCustom = true;
			m_PanelBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("panelborder"));
		}
		if (xmlElem.HasAttribute("paneltext"))
		{
			m_PanelTextCustom = true;
			m_PanelText = BarFunctions.ColorFromString(xmlElem.GetAttribute("paneltext"));
		}
		if (xmlElem.HasAttribute("panelbackga"))
		{
			m_PanelBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("panelbackga"));
		}
		if (xmlElem.HasAttribute("splitterback"))
		{
			m_SplitterBackgroundCustom = true;
			m_SplitterBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("Splitterback"));
		}
		if (xmlElem.HasAttribute("splitterback2"))
		{
			m_SplitterBackground2Custom = true;
			m_SplitterBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("Splitterback2"));
		}
		if (xmlElem.HasAttribute("splitterborder"))
		{
			m_SplitterBorderCustom = true;
			m_SplitterBorder = BarFunctions.ColorFromString(xmlElem.GetAttribute("Splitterborder"));
		}
		if (xmlElem.HasAttribute("splittertext"))
		{
			m_SplitterTextCustom = true;
			m_SplitterText = BarFunctions.ColorFromString(xmlElem.GetAttribute("Splittertext"));
		}
		if (xmlElem.HasAttribute("splitterbackga"))
		{
			m_SplitterBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("Splitterbackga"));
		}
		if (xmlElem.HasAttribute("exbarback"))
		{
			m_ExplorerBarBackgroundCustom = true;
			m_ExplorerBarBackground = BarFunctions.ColorFromString(xmlElem.GetAttribute("exbarback"));
		}
		if (xmlElem.HasAttribute("exbarback2"))
		{
			m_ExplorerBarBackground2Custom = true;
			m_ExplorerBarBackground2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("exbarback2"));
		}
		if (xmlElem.HasAttribute("exbarbackga"))
		{
			m_ExplorerBarBackgroundGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("exbarbackga"));
		}
		if (xmlElem.HasAttribute("dsback"))
		{
			m_DockSiteBackColorCustom = true;
			m_DockSiteBackColor = BarFunctions.ColorFromString(xmlElem.GetAttribute("dsback"));
		}
		if (xmlElem.HasAttribute("dsback2"))
		{
			m_DockSiteBackColor2Custom = true;
			m_DockSiteBackColor2 = BarFunctions.ColorFromString(xmlElem.GetAttribute("dsback2"));
		}
		if (xmlElem.HasAttribute("dsbackga"))
		{
			m_DockSiteBackColorGradientAngle = XmlConvert.ToInt32(xmlElem.GetAttribute("dsbackga"));
		}
		if (xmlElem.HasAttribute("mdisystemitemforeground"))
		{
			m_MdiSystemItemForeground = BarFunctions.ColorFromString(xmlElem.GetAttribute("mdisystemitemforeground"));
		}
		else
		{
			m_MdiSystemItemForeground = Color.Empty;
		}
	}
}
