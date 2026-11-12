using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class TabColorScheme
{
	private eTabStripStyle m_Style = eTabStripStyle.OneNote;

	private Color m_TabBackground = Color.Empty;

	private bool m_TabBackgroundCustom;

	private Color m_TabBackground2 = Color.Empty;

	private bool m_TabBackground2Custom;

	private int m_TabBackgroundGradientAngle = 90;

	private Color m_TabBorder = Color.Empty;

	private bool m_TabBorderCustom;

	private Color m_TabPanelBackground = Color.Empty;

	private bool m_TabPanelBackgroundCustom;

	private Color m_TabPanelBackground2 = Color.Empty;

	private bool m_TabPanelBackground2Custom;

	private int m_TabPanelBackgroundGradientAngle = 90;

	private Color m_TabPanelBorder = Color.Empty;

	private bool m_TabPanelBorderCustom;

	private Color m_TabItemBorder = Color.Empty;

	private bool m_TabItemBorderCustom;

	private Color m_TabItemBorderLight = Color.Empty;

	private bool m_TabItemBorderLightCustom;

	private Color m_TabItemBorderDark = Color.Empty;

	private bool m_TabItemBorderDarkCustom;

	private Color m_TabItemBackground = Color.Empty;

	private bool m_TabItemBackgroundCustom;

	private Color m_TabItemBackground2 = Color.Empty;

	private bool m_TabItemBackground2Custom;

	private int m_TabItemBackgroundGradientAngle = 90;

	private BackgroundColorBlendCollection m_TabItemBackgroundColorBlend = new BackgroundColorBlendCollection();

	private Color m_TabItemText = Color.Empty;

	private bool m_TabItemTextCustom;

	private Color m_TabItemHotBorder = Color.Empty;

	private bool m_TabItemHotBorderCustom;

	private Color m_TabItemHotBorderLight = Color.Empty;

	private bool m_TabItemHotBorderLightCustom;

	private Color m_TabItemHotBorderDark = Color.Empty;

	private bool m_TabItemHotBorderDarkCustom;

	private Color m_TabItemHotBackground = Color.Empty;

	private bool m_TabItemHotBackgroundCustom;

	private Color m_TabItemHotBackground2 = Color.Empty;

	private bool m_TabItemHotBackground2Custom;

	private int m_TabItemHotBackgroundGradientAngle = 90;

	private BackgroundColorBlendCollection m_TabItemHotBackgroundColorBlend = new BackgroundColorBlendCollection();

	private Color m_TabItemHotText = Color.Empty;

	private bool m_TabItemHotTextCustom;

	private Color m_TabItemSelectedBorder = Color.Empty;

	private bool m_TabItemSelectedBorderCustom;

	private Color m_TabItemSelectedBorderLight = Color.Empty;

	private bool m_TabItemSelectedBorderLightCustom;

	private Color m_TabItemSelectedBorderDark = Color.Empty;

	private bool m_TabItemSelectedBorderDarkCustom;

	private Color m_TabItemSelectedBackground = Color.Empty;

	private bool m_TabItemSelectedBackgroundCustom;

	private Color m_TabItemSelectedBackground2 = Color.Empty;

	private bool m_TabItemSelectedBackground2Custom;

	private int m_TabItemSelectedBackgroundGradientAngle = 90;

	private BackgroundColorBlendCollection m_TabItemSelectedBackgroundColorBlend = new BackgroundColorBlendCollection();

	private Color m_TabItemSelectedText = Color.Empty;

	private bool m_TabItemSelectedTextCustom;

	private Color m_TabItemSeparator = Color.Empty;

	private bool m_TabItemSeparatorCustom;

	private Color m_TabItemSeparatorShade = Color.Empty;

	private bool m_TabItemSeparatorShadeCustom;

	private bool m_Themed;

	private bool m_OtherCustom;

	private Image _TabBackgroundImage;

	[DefaultValue(eTabStripStyle.OneNote)]
	[Browsable(false)]
	[Category("Appearance")]
	[Description("Indicates style that color scheme represents.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eTabStripStyle Style
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

	internal bool Themed
	{
		get
		{
			return m_Themed;
		}
		set
		{
			m_Themed = value;
			Refresh();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool SchemeChanged
	{
		get
		{
			if (!m_TabBackground2Custom && !m_TabBackgroundCustom && !m_TabBorderCustom && !m_TabItemBackground2Custom && !m_TabItemBackgroundCustom && !m_TabItemBorderCustom && !m_TabItemBorderDarkCustom && !m_TabItemBorderLightCustom && !m_TabItemTextCustom && !m_TabItemHotBackground2Custom && !m_TabItemHotBackgroundCustom && !m_TabItemHotBorderCustom && !m_TabItemHotBorderDarkCustom && !m_TabItemHotBorderLightCustom && !m_TabItemHotTextCustom && !m_TabItemSelectedBackground2Custom && !m_TabItemSelectedBackgroundCustom && !m_TabItemSelectedBorderCustom && !m_TabItemSelectedBorderDarkCustom && !m_TabItemSelectedBorderLightCustom && !m_TabItemSelectedTextCustom && !m_TabItemSeparatorCustom && !m_TabItemSeparatorShadeCustom)
			{
				return m_OtherCustom;
			}
			return true;
		}
	}

	[DefaultValue(null)]
	[Category("Tab Control Colors")]
	[Description("Indicates tab-strip background image.")]
	public Image TabBackgroundImage
	{
		get
		{
			return _TabBackgroundImage;
		}
		set
		{
			_TabBackgroundImage = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the background color of the tab control.")]
	public Color TabBackground
	{
		get
		{
			return m_TabBackground;
		}
		set
		{
			if (m_TabBackground != value)
			{
				m_TabBackground = value;
				m_TabBackgroundCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the target gradient background color of the tab control.")]
	public Color TabBackground2
	{
		get
		{
			return m_TabBackground2;
		}
		set
		{
			if (m_TabBackground2 != value)
			{
				m_TabBackground2 = value;
				m_TabBackground2Custom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int TabBackgroundGradientAngle
	{
		get
		{
			return m_TabBackgroundGradientAngle;
		}
		set
		{
			m_TabBackgroundGradientAngle = value;
			InvokeColorChanged();
			m_OtherCustom = true;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the border color of the tab control.")]
	public Color TabBorder
	{
		get
		{
			return m_TabBorder;
		}
		set
		{
			if (m_TabBorder != value)
			{
				m_TabBorder = value;
				m_TabBorderCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the background color of the tab panel.")]
	public Color TabPanelBackground
	{
		get
		{
			return m_TabPanelBackground;
		}
		set
		{
			if (m_TabPanelBackground != value)
			{
				m_TabPanelBackground = value;
				m_TabPanelBackgroundCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the target gradient background color of the tab panel.")]
	public Color TabPanelBackground2
	{
		get
		{
			return m_TabPanelBackground2;
		}
		set
		{
			if (m_TabPanelBackground2 != value)
			{
				m_TabPanelBackground2 = value;
				m_TabPanelBackground2Custom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int TabPanelBackgroundGradientAngle
	{
		get
		{
			return m_TabPanelBackgroundGradientAngle;
		}
		set
		{
			m_TabPanelBackgroundGradientAngle = value;
			InvokeColorChanged();
			m_OtherCustom = true;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the border color of the tab panel.")]
	public Color TabPanelBorder
	{
		get
		{
			return m_TabPanelBorder;
		}
		set
		{
			if (m_TabPanelBorder != value)
			{
				m_TabPanelBorder = value;
				m_TabPanelBorderCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the border color of the tab item.")]
	public Color TabItemBorder
	{
		get
		{
			return m_TabItemBorder;
		}
		set
		{
			if (m_TabItemBorder != value)
			{
				m_TabItemBorder = value;
				m_TabItemBorderCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the light border color of the tab item.")]
	public Color TabItemBorderLight
	{
		get
		{
			return m_TabItemBorderLight;
		}
		set
		{
			if (m_TabItemBorderLight != value)
			{
				m_TabItemBorderLight = value;
				m_TabItemBorderLightCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the dark border color of the tab item.")]
	public Color TabItemBorderDark
	{
		get
		{
			return m_TabItemBorderDark;
		}
		set
		{
			if (m_TabItemBorderDark != value)
			{
				m_TabItemBorderDark = value;
				m_TabItemBorderDarkCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the background color of the tab item.")]
	public Color TabItemBackground
	{
		get
		{
			return m_TabItemBackground;
		}
		set
		{
			if (m_TabItemBackground != value)
			{
				m_TabItemBackground = value;
				m_TabItemBackgroundCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the target gradient background color of the tab item.")]
	public Color TabItemBackground2
	{
		get
		{
			return m_TabItemBackground2;
		}
		set
		{
			if (m_TabItemBackground2 != value)
			{
				m_TabItemBackground2 = value;
				m_TabItemBackground2Custom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int TabItemBackgroundGradientAngle
	{
		get
		{
			return m_TabItemBackgroundGradientAngle;
		}
		set
		{
			m_TabItemBackgroundGradientAngle = value;
			InvokeColorChanged();
			m_OtherCustom = true;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection that defines the multicolor gradient background.")]
	public BackgroundColorBlendCollection TabItemBackgroundColorBlend => m_TabItemBackgroundColorBlend;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the text of the tab item.")]
	public Color TabItemText
	{
		get
		{
			return m_TabItemText;
		}
		set
		{
			if (m_TabItemText != value)
			{
				m_TabItemText = value;
				m_TabItemTextCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the border color of the tab item when mouse is over it.")]
	public Color TabItemHotBorder
	{
		get
		{
			return m_TabItemHotBorder;
		}
		set
		{
			if (m_TabItemHotBorder != value)
			{
				m_TabItemHotBorder = value;
				m_TabItemHotBorderCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the light border color of the tab item when mouse is over it.")]
	public Color TabItemHotBorderLight
	{
		get
		{
			return m_TabItemHotBorderLight;
		}
		set
		{
			if (m_TabItemHotBorderLight != value)
			{
				m_TabItemHotBorderLight = value;
				m_TabItemHotBorderLightCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the dark border color of the tab item when mouse is over it.")]
	public Color TabItemHotBorderDark
	{
		get
		{
			return m_TabItemHotBorderDark;
		}
		set
		{
			if (m_TabItemHotBorderDark != value)
			{
				m_TabItemHotBorderDark = value;
				m_TabItemHotBorderDarkCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the background color of the tab item when mouse is over it.")]
	public Color TabItemHotBackground
	{
		get
		{
			return m_TabItemHotBackground;
		}
		set
		{
			if (m_TabItemHotBackground != value)
			{
				m_TabItemHotBackground = value;
				m_TabItemHotBackgroundCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the target gradient background color of the tab item when mouse is over it.")]
	public Color TabItemHotBackground2
	{
		get
		{
			return m_TabItemHotBackground2;
		}
		set
		{
			if (m_TabItemHotBackground2 != value)
			{
				m_TabItemHotBackground2 = value;
				m_TabItemHotBackground2Custom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int TabItemHotBackgroundGradientAngle
	{
		get
		{
			return m_TabItemHotBackgroundGradientAngle;
		}
		set
		{
			m_TabItemHotBackgroundGradientAngle = value;
			InvokeColorChanged();
			m_OtherCustom = true;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection that defines the multicolor gradient background.")]
	public BackgroundColorBlendCollection TabItemHotBackgroundColorBlend => m_TabItemHotBackgroundColorBlend;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the text color of the tab item when mouse is over it.")]
	public Color TabItemHotText
	{
		get
		{
			return m_TabItemHotText;
		}
		set
		{
			if (m_TabItemHotText != value)
			{
				m_TabItemHotText = value;
				m_TabItemHotTextCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the border color of the tab item when selected.")]
	public Color TabItemSelectedBorder
	{
		get
		{
			return m_TabItemSelectedBorder;
		}
		set
		{
			if (m_TabItemSelectedBorder != value)
			{
				m_TabItemSelectedBorder = value;
				m_TabItemSelectedBorderCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the light border color of the tab item when selected.")]
	public Color TabItemSelectedBorderLight
	{
		get
		{
			return m_TabItemSelectedBorderLight;
		}
		set
		{
			if (m_TabItemSelectedBorderLight != value)
			{
				m_TabItemSelectedBorderLight = value;
				m_TabItemSelectedBorderLightCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the dark border color of the tab item when selected.")]
	public Color TabItemSelectedBorderDark
	{
		get
		{
			return m_TabItemSelectedBorderDark;
		}
		set
		{
			if (m_TabItemSelectedBorderDark != value)
			{
				m_TabItemSelectedBorderDark = value;
				m_TabItemSelectedBorderDarkCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the background color of the tab item when selected.")]
	public Color TabItemSelectedBackground
	{
		get
		{
			return m_TabItemSelectedBackground;
		}
		set
		{
			if (m_TabItemSelectedBackground != value)
			{
				m_TabItemSelectedBackground = value;
				m_TabItemSelectedBackgroundCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the target gradient background color of the tab item when selected.")]
	public Color TabItemSelectedBackground2
	{
		get
		{
			return m_TabItemSelectedBackground2;
		}
		set
		{
			if (m_TabItemSelectedBackground2 != value)
			{
				m_TabItemSelectedBackground2 = value;
				m_TabItemSelectedBackground2Custom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	public int TabItemSelectedBackgroundGradientAngle
	{
		get
		{
			return m_TabItemSelectedBackgroundGradientAngle;
		}
		set
		{
			m_TabItemSelectedBackgroundGradientAngle = value;
			InvokeColorChanged();
			m_OtherCustom = true;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection that defines the multicolor gradient background.")]
	public BackgroundColorBlendCollection TabItemSelectedBackgroundColorBlend => m_TabItemSelectedBackgroundColorBlend;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the text color of the tab item when selected.")]
	public Color TabItemSelectedText
	{
		get
		{
			return m_TabItemSelectedText;
		}
		set
		{
			if (m_TabItemSelectedText != value)
			{
				m_TabItemSelectedText = value;
				m_TabItemSelectedTextCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the tab item separator color.")]
	public Color TabItemSeparator
	{
		get
		{
			return m_TabItemSeparator;
		}
		set
		{
			if (m_TabItemSeparator != value)
			{
				m_TabItemSeparator = value;
				m_TabItemSeparatorCustom = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the tab item separator shadow color.")]
	public Color TabItemSeparatorShade
	{
		get
		{
			return m_TabItemSeparatorShade;
		}
		set
		{
			if (m_TabItemSeparatorShade != value)
			{
				m_TabItemSeparatorShade = value;
				m_TabItemSeparatorShadeCustom = true;
				InvokeColorChanged();
			}
		}
	}

	public event EventHandler ColorChanged;

	public TabColorScheme()
	{
		Refresh();
	}

	public TabColorScheme(eTabStripStyle style)
	{
		m_Style = style;
		Refresh();
	}

	public void Refresh()
	{
		_TabBackgroundImage = null;
		if (m_Themed)
		{
			GenerateThemedColorScheme();
			return;
		}
		switch (m_Style)
		{
		case eTabStripStyle.Metro:
			GenerateMetroColorScheme();
			break;
		case eTabStripStyle.Flat:
			GenerateFlatColorScheme();
			break;
		case eTabStripStyle.Office2003:
			GenerateOffice2003ColorScheme();
			break;
		case eTabStripStyle.Office2007Document:
		case eTabStripStyle.Office2007Dock:
			GenerateOffice2007DocumentColorScheme();
			break;
		case eTabStripStyle.VS2005:
			GenerateVS2005ColorScheme(dockStyle: false);
			break;
		case eTabStripStyle.VS2005Dock:
			GenerateVS2005ColorScheme(dockStyle: true);
			break;
		case eTabStripStyle.SimulatedTheme:
			GenerateVS2005ColorScheme(dockStyle: true);
			ApplySimulatedColorScheme();
			break;
		case eTabStripStyle.RoundHeader:
			GenerateRoundHeaderColorScheme();
			break;
		case eTabStripStyle.VS2005Document:
			GenerateVS2005DocumentColorScheme();
			break;
		default:
			GenerateOneNoteColorScheme();
			break;
		}
		if (!m_TabItemTextCustom && !m_TabItemBackground.IsEmpty && (double)Math.Abs(m_TabItemText.GetBrightness() - m_TabItemBackground.GetBrightness()) <= 0.2)
		{
			m_TabItemText = (((double)m_TabItemText.GetBrightness() < 0.5) ? ControlPaint.Light(m_TabItemText) : ControlPaint.Dark(m_TabItemText));
		}
		if (!m_TabItemSelectedTextCustom && !m_TabItemSelectedBackground.IsEmpty && (double)Math.Abs(m_TabItemSelectedText.GetBrightness() - m_TabItemSelectedBackground.GetBrightness()) <= 0.2)
		{
			m_TabItemSelectedText = (((double)m_TabItemSelectedText.GetBrightness() < 0.5) ? ControlPaint.Light(m_TabItemSelectedText) : ControlPaint.Dark(m_TabItemSelectedText));
		}
	}

	private void GenerateMetroColorScheme()
	{
		Office2007TabColorTable office2007TabColorTable = null;
		Office2007ColorTable office2007ColorTable = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			office2007TabColorTable = office2007ColorTable.TabControl;
		}
		if (office2007TabColorTable == null)
		{
			GenerateOneNoteColorScheme();
			return;
		}
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = Color.Empty;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = Color.Empty;
		}
		if (!m_TabBackgroundCustom)
		{
			if (office2007TabColorTable.TabBackground.Start.IsEmpty)
			{
				m_TabBackground = office2007ColorTable.Form.BackColor;
			}
			else
			{
				m_TabBackground = office2007TabColorTable.TabBackground.Start;
			}
		}
		if (!m_TabBackground2Custom)
		{
			m_TabBackground2 = office2007TabColorTable.TabBackground.End;
		}
		m_TabBackgroundGradientAngle = office2007TabColorTable.TabBackground.GradientAngle;
		if (!m_TabBorderCustom)
		{
			m_TabBorder = office2007TabColorTable.TabPanelBorder;
		}
		if (!m_TabItemBorderCustom)
		{
			m_TabItemBorder = Color.Empty;
		}
		if (!m_TabItemBorderLightCustom)
		{
			m_TabItemBorderLight = Color.Empty;
		}
		if (!m_TabItemBorderDarkCustom)
		{
			m_TabItemBorderDark = Color.Empty;
		}
		if (!m_TabItemBackgroundCustom)
		{
			m_TabItemBackground = Color.Empty;
		}
		if (!m_TabItemBackground2Custom)
		{
			m_TabItemBackground2 = Color.Empty;
		}
		if (!m_TabItemTextCustom)
		{
			if (office2007TabColorTable.TabBackground.Start.IsEmpty)
			{
				m_TabItemText = office2007ColorTable.Form.TextColor;
			}
			else
			{
				m_TabItemText = office2007TabColorTable.Default.Text;
			}
		}
		if (!m_TabItemHotBorderCustom)
		{
			m_TabItemHotBorder = Color.Empty;
		}
		if (!m_TabItemHotBorderLightCustom)
		{
			m_TabItemHotBorderLight = Color.Empty;
		}
		if (!m_TabItemHotBorderDarkCustom)
		{
			m_TabItemHotBorderDark = Color.Empty;
		}
		if (!m_TabItemHotBackgroundCustom)
		{
			m_TabItemHotBackground = Color.Empty;
		}
		if (!m_TabItemHotBackground2Custom)
		{
			m_TabItemHotBackground2 = Color.Empty;
		}
		if (!m_TabItemHotTextCustom)
		{
			m_TabItemHotText = office2007TabColorTable.MouseOver.Text;
		}
		if (!m_TabItemSelectedBorderCustom)
		{
			if (office2007TabColorTable.Selected.OuterBorder.IsEmpty)
			{
				m_TabItemSelectedBorder = m_TabItemText;
			}
			else
			{
				m_TabItemSelectedBorder = office2007TabColorTable.Selected.OuterBorder;
			}
		}
		if (!m_TabItemSelectedBorderLightCustom)
		{
			m_TabItemSelectedBorderLight = Color.Empty;
		}
		if (!m_TabItemSelectedBorderDarkCustom)
		{
			m_TabItemSelectedBorderDark = Color.Empty;
		}
		if (!m_TabItemSelectedBackgroundCustom)
		{
			m_TabItemSelectedBackground = Color.Empty;
		}
		if (!m_TabItemSelectedBackground2Custom)
		{
			m_TabItemSelectedBackground2 = Color.Empty;
		}
		if (!m_TabItemSelectedTextCustom)
		{
			m_TabItemSelectedText = office2007TabColorTable.Selected.Text;
		}
		if (!m_TabPanelBackgroundCustom)
		{
			m_TabPanelBackground = m_TabBackground;
		}
		if (!m_TabPanelBackground2Custom)
		{
			m_TabPanelBackground2 = m_TabBackground2;
		}
		if (!m_TabPanelBorderCustom)
		{
			m_TabPanelBorder = m_TabBorder;
		}
	}

	private void ApplySimulatedColorScheme()
	{
		if (BarFunctions.WinXPColorScheme == eWinXPColorScheme.Undetermined)
		{
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = SystemColors.Highlight;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.FromArgb(128, SystemColors.Highlight);
			}
		}
		else
		{
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = GetColor("E68B2C");
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = GetColor("FFC73C");
			}
		}
		if (!m_TabItemHotBorderDarkCustom)
		{
			m_TabItemHotBorderDark = m_TabItemSelectedBorderDark;
		}
		if (!m_TabItemHotBorderLightCustom)
		{
			m_TabItemHotBorderLight = m_TabItemSelectedBorderLight;
		}
		if (!m_TabItemHotBorderCustom)
		{
			m_TabItemHotBorder = m_TabItemBorder;
		}
		if (!m_TabItemBorderLightCustom)
		{
			m_TabItemBorderLight = SystemColors.ControlLightLight;
		}
		if (!m_TabItemBorderDarkCustom)
		{
			m_TabItemBorderDark = SystemColors.Control;
		}
	}

	private void GenerateFlatColorScheme()
	{
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = SystemColors.ControlDark;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = Color.Empty;
		}
		if (!m_TabBackgroundCustom)
		{
			m_TabBackground = GetBackColorFlat();
		}
		if (!m_TabBackground2Custom)
		{
			m_TabBackground2 = Color.Empty;
		}
		if (!m_TabBorderCustom)
		{
			m_TabBorder = SystemColors.ControlDark;
		}
		if (!m_TabItemBorderCustom)
		{
			m_TabItemBorder = Color.Empty;
		}
		if (!m_TabItemBorderLightCustom)
		{
			m_TabItemBorderLight = Color.Empty;
		}
		if (!m_TabItemBorderDarkCustom)
		{
			m_TabItemBorderDark = Color.Empty;
		}
		if (!m_TabItemBackgroundCustom)
		{
			m_TabItemBackground = Color.Empty;
		}
		if (!m_TabItemBackground2Custom)
		{
			m_TabItemBackground2 = Color.Empty;
		}
		if (!m_TabItemTextCustom)
		{
			m_TabItemText = SystemColors.ControlDark;
		}
		if (!m_TabItemHotBorderCustom)
		{
			m_TabItemHotBorder = Color.Empty;
		}
		if (!m_TabItemHotBorderLightCustom)
		{
			m_TabItemHotBorderLight = Color.Empty;
		}
		if (!m_TabItemHotBorderDarkCustom)
		{
			m_TabItemHotBorderDark = Color.Empty;
		}
		if (!m_TabItemHotBackgroundCustom)
		{
			m_TabItemHotBackground = Color.Empty;
		}
		if (!m_TabItemHotBackground2Custom)
		{
			m_TabItemHotBackground2 = Color.Empty;
		}
		if (!m_TabItemHotTextCustom)
		{
			m_TabItemHotText = Color.Empty;
		}
		if (!m_TabItemSelectedBorderCustom)
		{
			m_TabItemSelectedBorder = SystemColors.ControlText;
		}
		if (!m_TabItemSelectedBorderLightCustom)
		{
			m_TabItemSelectedBorderLight = SystemColors.ControlLightLight;
		}
		if (!m_TabItemSelectedBorderDarkCustom)
		{
			m_TabItemSelectedBorderDark = Color.Empty;
		}
		if (!m_TabItemSelectedBackgroundCustom)
		{
			m_TabItemSelectedBackground = SystemColors.Control;
		}
		if (!m_TabItemSelectedBackground2Custom)
		{
			m_TabItemSelectedBackground2 = Color.Empty;
		}
		if (!m_TabItemSelectedTextCustom)
		{
			m_TabItemSelectedText = SystemColors.ControlText;
		}
		if (!m_TabPanelBackgroundCustom)
		{
			m_TabPanelBackground = m_TabItemSelectedBackground;
		}
		if (!m_TabPanelBackground2Custom)
		{
			m_TabPanelBackground2 = Color.Empty;
		}
		if (!m_TabPanelBorderCustom)
		{
			m_TabPanelBorder = SystemColors.ControlDark;
		}
	}

	private Color GetBackColorFlat()
	{
		Color result = ControlPaint.Light(SystemColors.Control);
		switch (BarFunctions.WinXPColorScheme)
		{
		case eWinXPColorScheme.Blue:
			result = Color.FromArgb(255, 251, 233);
			break;
		case eWinXPColorScheme.OliveGreen:
			result = Color.FromArgb(255, 251, 233);
			break;
		case eWinXPColorScheme.Silver:
			result = Color.FromArgb(251, 250, 255);
			break;
		}
		return result;
	}

	private void GenerateOffice2003ColorScheme()
	{
		ColorScheme colorScheme = new ColorScheme();
		colorScheme.Style = eDotNetBarStyle.Office2003;
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = colorScheme.ItemSeparator;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = colorScheme.ItemSeparatorShade;
		}
		if (!m_TabBackgroundCustom)
		{
			m_TabBackground = colorScheme.BarBackground;
		}
		if (!m_TabBackground2Custom)
		{
			m_TabBackground2 = colorScheme.BarBackground2;
		}
		if (!m_TabBorderCustom)
		{
			m_TabBorder = Color.Empty;
		}
		if (!m_TabItemBorderCustom)
		{
			m_TabItemBorder = Color.Empty;
		}
		if (!m_TabItemBorderLightCustom)
		{
			m_TabItemBorderLight = Color.Empty;
		}
		if (!m_TabItemBorderDarkCustom)
		{
			m_TabItemBorderDark = Color.Empty;
		}
		if (!m_TabItemBackgroundCustom)
		{
			m_TabItemBackground = Color.Empty;
		}
		if (!m_TabItemBackground2Custom)
		{
			m_TabItemBackground2 = Color.Empty;
		}
		if (!m_TabItemTextCustom)
		{
			m_TabItemText = SystemColors.ControlText;
		}
		if (!m_TabItemHotBorderCustom)
		{
			m_TabItemHotBorder = Color.Empty;
		}
		if (!m_TabItemHotBorderLightCustom)
		{
			m_TabItemHotBorderLight = Color.Empty;
		}
		if (!m_TabItemHotBorderDarkCustom)
		{
			m_TabItemHotBorderDark = Color.Empty;
		}
		if (!m_TabItemHotBackgroundCustom)
		{
			m_TabItemHotBackground = Color.Empty;
		}
		if (!m_TabItemHotBackground2Custom)
		{
			m_TabItemHotBackground2 = Color.Empty;
		}
		if (!m_TabItemHotTextCustom)
		{
			m_TabItemHotText = Color.Empty;
		}
		if (!m_TabItemSelectedBorderCustom)
		{
			m_TabItemSelectedBorder = colorScheme.ItemPressedBorder;
		}
		if (!m_TabItemSelectedBorderLightCustom)
		{
			m_TabItemSelectedBorderLight = colorScheme.ItemPressedBorder;
		}
		if (!m_TabItemSelectedBorderDarkCustom)
		{
			m_TabItemSelectedBorderDark = Color.Empty;
		}
		if (!m_TabItemSelectedBackgroundCustom)
		{
			m_TabItemSelectedBackground = colorScheme.ItemPressedBackground;
		}
		if (!m_TabItemSelectedBackground2Custom)
		{
			m_TabItemSelectedBackground2 = colorScheme.ItemPressedBackground2;
		}
		if (!m_TabItemSelectedTextCustom)
		{
			m_TabItemSelectedText = colorScheme.ItemPressedText;
		}
		if (!m_TabPanelBackgroundCustom)
		{
			m_TabPanelBackground = m_TabBackground2;
		}
		if (!m_TabPanelBackground2Custom)
		{
			m_TabPanelBackground2 = m_TabBackground;
		}
		if (!m_TabPanelBorderCustom)
		{
			m_TabPanelBorder = m_TabItemSelectedBorder;
		}
	}

	private void GenerateVS2005DocumentColorScheme()
	{
		ColorScheme colorScheme = new ColorScheme();
		colorScheme.Style = eDotNetBarStyle.VS2005;
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = colorScheme.ItemSeparator;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = colorScheme.ItemSeparatorShade;
		}
		if (!m_TabBackgroundCustom)
		{
			m_TabBackground = colorScheme.BarBackground;
		}
		if (!m_TabBackground2Custom)
		{
			m_TabBackground2 = Color.Empty;
		}
		if (!m_TabBorderCustom)
		{
			m_TabBorder = Color.Empty;
		}
		if (!m_TabItemBorderCustom)
		{
			m_TabItemBorder = GetColor("ACA899");
		}
		if (!m_TabItemBorderLightCustom)
		{
			m_TabItemBorderLight = Color.White;
		}
		if (!m_TabItemBorderDarkCustom)
		{
			m_TabItemBorderDark = GetColor("ECE9D8");
		}
		if (!m_TabItemBackgroundCustom)
		{
			m_TabItemBackground = GetColor("FDFCFB");
		}
		if (!m_TabItemBackground2Custom)
		{
			m_TabItemBackground2 = GetColor("F1EFE2");
		}
		if (!m_TabItemTextCustom)
		{
			m_TabItemText = SystemColors.ControlText;
		}
		if (!m_TabItemHotBorderCustom)
		{
			m_TabItemHotBorder = Color.Empty;
		}
		if (!m_TabItemHotBorderLightCustom)
		{
			m_TabItemHotBorderLight = Color.Empty;
		}
		if (!m_TabItemHotBorderDarkCustom)
		{
			m_TabItemHotBorderDark = Color.Empty;
		}
		if (!m_TabItemHotBackgroundCustom)
		{
			m_TabItemHotBackground = Color.Empty;
		}
		if (!m_TabItemHotBackground2Custom)
		{
			m_TabItemHotBackground2 = Color.Empty;
		}
		if (!m_TabItemHotTextCustom)
		{
			m_TabItemHotText = Color.Empty;
		}
		if (!m_TabItemSelectedBorderCustom)
		{
			m_TabItemSelectedBorder = GetColor("7F9DB9");
		}
		if (!m_TabItemSelectedBorderLightCustom)
		{
			m_TabItemSelectedBorderLight = Color.Empty;
		}
		if (!m_TabItemSelectedBorderDarkCustom)
		{
			m_TabItemSelectedBorderDark = Color.Empty;
		}
		if (!m_TabItemSelectedBackgroundCustom)
		{
			m_TabItemSelectedBackground = Color.White;
		}
		if (!m_TabItemSelectedBackground2Custom)
		{
			m_TabItemSelectedBackground2 = Color.Empty;
		}
		if (!m_TabItemSelectedTextCustom)
		{
			m_TabItemSelectedText = SystemColors.ControlText;
		}
		if (!m_TabPanelBackgroundCustom)
		{
			m_TabPanelBackground = m_TabBackground2;
		}
		if (!m_TabPanelBackground2Custom)
		{
			m_TabPanelBackground2 = m_TabBackground;
		}
		if (!m_TabPanelBorderCustom)
		{
			m_TabPanelBorder = m_TabItemSelectedBorder;
		}
	}

	private void GenerateThemedColorScheme()
	{
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = SystemColors.ControlDark;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = Color.Empty;
		}
		if (!m_TabBackgroundCustom)
		{
			m_TabBackground = Color.Empty;
		}
		if (!m_TabBackground2Custom)
		{
			m_TabBackground2 = Color.Empty;
		}
		if (!m_TabBorderCustom)
		{
			m_TabBorder = Color.Empty;
		}
		if (!m_TabItemBorderCustom)
		{
			m_TabItemBorder = Color.Empty;
		}
		if (!m_TabItemBorderLightCustom)
		{
			m_TabItemBorderLight = Color.Empty;
		}
		if (!m_TabItemBorderDarkCustom)
		{
			m_TabItemBorderDark = Color.Empty;
		}
		if (!m_TabItemBackgroundCustom)
		{
			m_TabItemBackground = Color.Empty;
		}
		if (!m_TabItemBackground2Custom)
		{
			m_TabItemBackground2 = Color.Empty;
		}
		if (!m_TabItemTextCustom)
		{
			m_TabItemText = SystemColors.ControlText;
		}
		if (!m_TabPanelBackgroundCustom)
		{
			m_TabPanelBackground = SystemColors.Control;
		}
		if (!m_TabPanelBackground2Custom)
		{
			m_TabPanelBackground2 = Color.Empty;
		}
		if (!m_TabPanelBorderCustom)
		{
			m_TabPanelBorder = Color.Empty;
		}
		if (!m_TabItemHotBorderCustom)
		{
			m_TabItemHotBorder = Color.Empty;
		}
		if (!m_TabItemHotBorderLightCustom)
		{
			m_TabItemHotBorderLight = Color.Empty;
		}
		if (!m_TabItemHotBorderDarkCustom)
		{
			m_TabItemHotBorderDark = Color.Empty;
		}
		if (!m_TabItemHotBackgroundCustom)
		{
			m_TabItemHotBackground = Color.Empty;
		}
		if (!m_TabItemHotBackground2Custom)
		{
			m_TabItemHotBackground2 = Color.Empty;
		}
		if (!m_TabItemHotTextCustom)
		{
			m_TabItemHotText = Color.Empty;
		}
		if (!m_TabItemSelectedBorderCustom)
		{
			m_TabItemSelectedBorder = SystemColors.ControlText;
		}
		if (!m_TabItemSelectedBorderLightCustom)
		{
			m_TabItemSelectedBorderLight = SystemColors.ControlLightLight;
		}
		if (!m_TabItemSelectedBorderDarkCustom)
		{
			m_TabItemSelectedBorderDark = Color.Empty;
		}
		if (!m_TabItemSelectedBackgroundCustom)
		{
			m_TabItemSelectedBackground = SystemColors.Control;
		}
		if (!m_TabItemSelectedBackground2Custom)
		{
			m_TabItemSelectedBackground2 = Color.Empty;
		}
		if (!m_TabItemSelectedTextCustom)
		{
			m_TabItemSelectedText = SystemColors.ControlText;
		}
	}

	private void GenerateOneNoteColorScheme()
	{
		eWinXPColorScheme winXPColorScheme = BarFunctions.WinXPColorScheme;
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = Color.Empty;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = Color.Empty;
		}
		ColorScheme colorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
		switch (winXPColorScheme)
		{
		case eWinXPColorScheme.Blue:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = Color.FromArgb(196, 218, 250);
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.FromArgb(253, 254, 255);
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = Color.Empty;
			}
			if (!m_TabItemBorderCustom)
			{
				m_TabItemBorder = Color.FromArgb(0, 53, 154);
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.White;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.FromArgb(117, 166, 241);
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = Color.FromArgb(236, 243, 252);
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = Color.FromArgb(191, 214, 246);
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = m_TabItemBackground2;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = Color.White;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.FromArgb(0, 0, 128);
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.FromArgb(0, 0, 128);
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.FromArgb(0, 0, 128);
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.FromArgb(255, 249, 237);
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.FromArgb(255, 240, 199);
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = colorScheme.BarDockedBorder;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.White;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = m_TabItemSelectedBorderLight;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = colorScheme.BarBackground;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = colorScheme.BarBackground2;
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		case eWinXPColorScheme.Silver:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = Color.FromArgb(243, 243, 247);
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.FromArgb(226, 226, 235);
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = Color.Empty;
			}
			if (!m_TabItemBorderCustom)
			{
				m_TabItemBorder = Color.FromArgb(118, 116, 146);
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.White;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.FromArgb(186, 185, 206);
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = Color.FromArgb(243, 243, 247);
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = Color.FromArgb(215, 215, 228);
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = Color.FromArgb(215, 215, 228);
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = Color.White;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.FromArgb(75, 75, 111);
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.FromArgb(75, 75, 111);
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.FromArgb(75, 75, 111);
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.FromArgb(255, 249, 237);
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.FromArgb(255, 240, 199);
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = colorScheme.BarDockedBorder;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.White;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = m_TabItemSelectedBorderLight;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = colorScheme.BarBackground;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = colorScheme.BarBackground;
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		case eWinXPColorScheme.OliveGreen:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = Color.FromArgb(242, 241, 228);
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.FromArgb(230, 236, 209);
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = Color.Empty;
			}
			if (!m_TabItemBorderCustom)
			{
				m_TabItemBorder = Color.FromArgb(96, 119, 107);
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.White;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.FromArgb(176, 194, 140);
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = Color.FromArgb(244, 247, 236);
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = Color.FromArgb(221, 229, 192);
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = Color.FromArgb(221, 229, 192);
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = Color.White;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.FromArgb(63, 93, 56);
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.FromArgb(63, 93, 56);
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.FromArgb(63, 93, 56);
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.FromArgb(255, 249, 237);
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.FromArgb(255, 240, 199);
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = colorScheme.BarDockedBorder;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.White;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = m_TabItemSelectedBorderLight;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = colorScheme.BarBackground;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = colorScheme.BarBackground;
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		default:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = colorScheme.BarBackground;
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = colorScheme.BarBackground2;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = Color.Empty;
			}
			if (!m_TabItemBorderCustom)
			{
				m_TabItemBorder = SystemColors.ControlDark;
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.White;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = m_TabBackground2;
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = m_TabBackground;
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = m_TabBackground2;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = ControlPaint.LightLight(m_TabBackground2);
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = colorScheme.BarDockedBorder;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = colorScheme.BarDockedBorder;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = colorScheme.BarDockedBorder;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = colorScheme.ItemHotBackground;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = colorScheme.ItemHotBackground2;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = SystemColors.ControlDark;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = SystemColors.ControlLightLight;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = m_TabItemSelectedBorderLight;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = colorScheme.ItemPressedBackground2;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = colorScheme.ItemPressedBackground;
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		}
		if (!m_TabPanelBackgroundCustom)
		{
			m_TabPanelBackground = m_TabItemSelectedBackground2;
		}
		if (!m_TabPanelBackground2Custom)
		{
			m_TabPanelBackground2 = m_TabItemSelectedBackground;
		}
		if (!m_TabPanelBorderCustom)
		{
			m_TabPanelBorder = m_TabItemSelectedBorder;
		}
	}

	private void GenerateVS2005ColorScheme(bool dockStyle)
	{
		eWinXPColorScheme winXPColorScheme = BarFunctions.WinXPColorScheme;
		ColorScheme colorScheme = new ColorScheme(eDotNetBarStyle.VS2005);
		switch (winXPColorScheme)
		{
		case eWinXPColorScheme.Blue:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = GetColor("F4F2E8");
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.Empty;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = GetColor("ACA899");
			}
			if (!m_TabItemBorderCustom)
			{
				if (dockStyle)
				{
					m_TabItemBorder = GetColor("ACA899");
				}
				else
				{
					m_TabItemBorder = Color.Empty;
				}
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.Empty;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.Empty;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = GetColor("F4F2E8");
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = Color.Empty;
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = m_TabItemBackground;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = Color.White;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = m_TabBorder;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.Empty;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.Empty;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.Empty;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.Empty;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = GetColor("ACA899");
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.Empty;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = Color.Empty;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = Color.White;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = Color.Empty;
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		case eWinXPColorScheme.Silver:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = GetColor("E7E7EF");
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.Empty;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = GetColor("9D9DA1");
			}
			if (!m_TabItemBorderCustom)
			{
				if (dockStyle)
				{
					m_TabItemBorder = m_TabBorder;
				}
				else
				{
					m_TabItemBorder = Color.Empty;
				}
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.Empty;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.Empty;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = GetColor("E7E7EF");
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = Color.Empty;
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = m_TabItemBackground;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = Color.White;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = m_TabItemBorder;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.Empty;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.Empty;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.Empty;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.Empty;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = m_TabBorder;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.Empty;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = Color.Empty;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = Color.White;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = Color.Empty;
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		case eWinXPColorScheme.OliveGreen:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = GetColor("F4F2E9");
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.Empty;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = GetColor("ACA899");
			}
			if (!m_TabItemBorderCustom)
			{
				if (dockStyle)
				{
					m_TabItemBorder = m_TabBorder;
				}
				else
				{
					m_TabItemBorder = Color.Empty;
				}
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.Empty;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.Empty;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = GetColor("F4F2E9");
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = Color.Empty;
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = m_TabItemBackground;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = Color.White;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = m_TabBorder;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.Empty;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.Empty;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.Empty;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.Empty;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = GetColor("ACA899");
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.Empty;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = Color.Empty;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = Color.White;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = Color.Empty;
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		default:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = colorScheme.BarBackground;
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.Empty;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = colorScheme.BarBackground2;
			}
			if (!m_TabItemBorderCustom)
			{
				if (dockStyle)
				{
					m_TabItemBorder = m_TabBorder;
				}
				else
				{
					m_TabItemBorder = Color.Empty;
				}
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.Empty;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.Empty;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = m_TabBackground;
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = Color.Empty;
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = m_TabItemBackground;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = ControlPaint.LightLight(m_TabItemBackground);
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.Empty;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.Empty;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.Empty;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.Empty;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = Color.Empty;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = colorScheme.BarFloatingBorder;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.Empty;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = Color.Empty;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = ControlPaint.LightLight(colorScheme.BarBackground);
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = Color.Empty;
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		}
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = m_TabItemSelectedBorder;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = m_TabItemSelectedBorderLight;
		}
	}

	private void GenerateRoundHeaderColorScheme()
	{
		eWinXPColorScheme winXPColorScheme = BarFunctions.WinXPColorScheme;
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = Color.Empty;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = Color.Empty;
		}
		ColorScheme colorScheme = new ColorScheme(eDotNetBarStyle.VS2005);
		switch (winXPColorScheme)
		{
		case eWinXPColorScheme.Blue:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = GetColor("F9F8F4");
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.Empty;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBorderCustom)
			{
				m_TabItemBorder = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.Empty;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.Empty;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = GetColor("ECE9D7");
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = GetColor("FEFDFD");
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = Color.White;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = m_TabItemBackground;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = m_TabBorder;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.Empty;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.Empty;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.Empty;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.Empty;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = m_TabItemBorder;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.Empty;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = Color.Empty;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = GetColor("0266FB");
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = Color.FromArgb(45, m_TabItemSelectedBackground);
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		case eWinXPColorScheme.Silver:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = SystemColors.Control;
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.Empty;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBorderCustom)
			{
				m_TabItemBorder = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.Empty;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.Empty;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = GetColor("F2F2F7");
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = GetColor("FEFEFE");
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = Color.White;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = m_TabItemBackground;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = m_TabItemBorder;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.Empty;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.Empty;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.Empty;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.Empty;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = m_TabItemBorder;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.Empty;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = Color.Empty;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = SystemColors.Highlight;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = Color.FromArgb(45, m_TabItemSelectedBackground);
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		case eWinXPColorScheme.OliveGreen:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = SystemColors.Control;
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.Empty;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBorderCustom)
			{
				m_TabItemBorder = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.Empty;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.Empty;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = GetColor("ECE9D7");
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = GetColor("FEFDFD");
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = Color.White;
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = m_TabItemBackground;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = m_TabBorder;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.Empty;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.Empty;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.Empty;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.Empty;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = SystemColors.ControlText;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = GetColor("ACA899");
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.Empty;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = Color.Empty;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = SystemColors.Highlight;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = Color.FromArgb(45, m_TabItemSelectedBackground);
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		default:
			if (!m_TabBackgroundCustom)
			{
				m_TabBackground = SystemColors.Control;
			}
			if (!m_TabBackground2Custom)
			{
				m_TabBackground2 = Color.Empty;
			}
			if (!m_TabBorderCustom)
			{
				m_TabBorder = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBorderCustom)
			{
				m_TabItemBorder = SystemColors.ControlDarkDark;
			}
			if (!m_TabItemBorderLightCustom)
			{
				m_TabItemBorderLight = Color.Empty;
			}
			if (!m_TabItemBorderDarkCustom)
			{
				m_TabItemBorderDark = Color.Empty;
			}
			if (!m_TabItemBackgroundCustom)
			{
				m_TabItemBackground = ControlPaint.Light(colorScheme.BarBackground2);
			}
			if (!m_TabItemBackground2Custom)
			{
				m_TabItemBackground2 = ControlPaint.Light(colorScheme.BarBackground);
			}
			if (!m_TabItemTextCustom)
			{
				m_TabItemText = SystemColors.ControlText;
			}
			if (!m_TabPanelBackgroundCustom)
			{
				m_TabPanelBackground = ControlPaint.LightLight(m_TabItemBackground);
			}
			if (!m_TabPanelBackground2Custom)
			{
				m_TabPanelBackground2 = m_TabItemBackground;
			}
			if (!m_TabPanelBorderCustom)
			{
				m_TabPanelBorder = m_TabBorder;
			}
			if (!m_TabItemHotBorderCustom)
			{
				m_TabItemHotBorder = Color.Empty;
			}
			if (!m_TabItemHotBorderLightCustom)
			{
				m_TabItemHotBorderLight = Color.Empty;
			}
			if (!m_TabItemHotBorderDarkCustom)
			{
				m_TabItemHotBorderDark = Color.Empty;
			}
			if (!m_TabItemHotBackgroundCustom)
			{
				m_TabItemHotBackground = Color.Empty;
			}
			if (!m_TabItemHotBackground2Custom)
			{
				m_TabItemHotBackground2 = Color.Empty;
			}
			if (!m_TabItemHotTextCustom)
			{
				m_TabItemHotText = Color.Empty;
			}
			if (!m_TabItemSelectedBorderCustom)
			{
				m_TabItemSelectedBorder = m_TabItemBorder;
			}
			if (!m_TabItemSelectedBorderLightCustom)
			{
				m_TabItemSelectedBorderLight = Color.Empty;
			}
			if (!m_TabItemSelectedBorderDarkCustom)
			{
				m_TabItemSelectedBorderDark = Color.Empty;
			}
			if (!m_TabItemSelectedBackgroundCustom)
			{
				m_TabItemSelectedBackground = SystemColors.Highlight;
			}
			if (!m_TabItemSelectedBackground2Custom)
			{
				m_TabItemSelectedBackground2 = Color.FromArgb(45, m_TabItemSelectedBackground);
			}
			if (!m_TabItemSelectedTextCustom)
			{
				m_TabItemSelectedText = SystemColors.ControlText;
			}
			break;
		}
	}

	private Color GetColor(string rgbHex)
	{
		if (rgbHex == "" || rgbHex == null)
		{
			return Color.Empty;
		}
		return Color.FromArgb(Convert.ToInt32(rgbHex.Substring(0, 2), 16), Convert.ToInt32(rgbHex.Substring(2, 2), 16), Convert.ToInt32(rgbHex.Substring(4, 2), 16));
	}

	private void GenerateOffice2007DocumentColorScheme()
	{
		Office2007TabColorTable office2007TabColorTable = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			office2007TabColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.TabControl;
		}
		if (office2007TabColorTable == null)
		{
			GenerateOneNoteColorScheme();
			return;
		}
		_TabBackgroundImage = office2007TabColorTable.TabBackgroundImage;
		if (!m_TabItemSeparatorCustom)
		{
			m_TabItemSeparator = Color.Empty;
		}
		if (!m_TabItemSeparatorShadeCustom)
		{
			m_TabItemSeparatorShade = Color.Empty;
		}
		if (!m_TabBackgroundCustom)
		{
			m_TabBackground = office2007TabColorTable.TabBackground.Start;
		}
		if (!m_TabBackground2Custom)
		{
			m_TabBackground2 = office2007TabColorTable.TabBackground.End;
		}
		if (!m_TabBorderCustom)
		{
			m_TabBorder = Color.Empty;
		}
		if (!m_TabItemBorderCustom)
		{
			m_TabItemBorder = office2007TabColorTable.Default.OuterBorder;
		}
		if (!m_TabItemBorderLightCustom)
		{
			m_TabItemBorderLight = office2007TabColorTable.Default.InnerBorder;
		}
		if (!m_TabItemBorderDarkCustom)
		{
			m_TabItemBorderDark = Color.Empty;
		}
		if (!m_TabItemBackgroundCustom)
		{
			m_TabItemBackground = office2007TabColorTable.Default.TopBackground.Start;
		}
		if (!m_TabItemBackground2Custom)
		{
			m_TabItemBackground2 = office2007TabColorTable.Default.BottomBackground.End;
		}
		if (!m_TabItemBackgroundCustom)
		{
			m_TabItemBackgroundColorBlend.Clear();
			m_TabItemBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.Default.TopBackground.Start, 0f));
			m_TabItemBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.Default.TopBackground.End, 0.45f));
			m_TabItemBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.Default.BottomBackground.Start, 0.45f));
			m_TabItemBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.Default.BottomBackground.End, 1f));
		}
		if (!m_TabItemTextCustom)
		{
			m_TabItemText = office2007TabColorTable.Default.Text;
		}
		if (!m_TabPanelBackgroundCustom)
		{
			m_TabPanelBackground = office2007TabColorTable.TabPanelBackground.Start;
		}
		if (!m_TabPanelBackground2Custom)
		{
			m_TabPanelBackground2 = office2007TabColorTable.TabPanelBackground.End;
		}
		if (!m_TabPanelBorderCustom)
		{
			m_TabPanelBorder = office2007TabColorTable.TabPanelBorder;
		}
		if (!m_TabItemHotBorderCustom)
		{
			m_TabItemHotBorder = office2007TabColorTable.MouseOver.OuterBorder;
		}
		if (!m_TabItemHotBorderLightCustom)
		{
			m_TabItemHotBorderLight = office2007TabColorTable.MouseOver.InnerBorder;
		}
		if (!m_TabItemHotBorderDarkCustom)
		{
			m_TabItemHotBorderDark = Color.Empty;
		}
		if (!m_TabItemHotBackgroundCustom)
		{
			m_TabItemHotBackground = office2007TabColorTable.MouseOver.TopBackground.Start;
		}
		if (!m_TabItemHotBackground2Custom)
		{
			m_TabItemHotBackground2 = office2007TabColorTable.MouseOver.BottomBackground.End;
		}
		if (!m_TabItemHotBackgroundCustom)
		{
			m_TabItemHotBackgroundColorBlend.Clear();
			m_TabItemHotBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.MouseOver.TopBackground.Start, 0f));
			m_TabItemHotBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.MouseOver.TopBackground.End, 0.45f));
			m_TabItemHotBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.MouseOver.BottomBackground.Start, 0.45f));
			m_TabItemHotBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.MouseOver.BottomBackground.End, 1f));
		}
		if (!m_TabItemHotTextCustom)
		{
			m_TabItemHotText = office2007TabColorTable.MouseOver.Text;
		}
		if (!m_TabItemSelectedBorderCustom)
		{
			m_TabItemSelectedBorder = office2007TabColorTable.Selected.OuterBorder;
		}
		if (!m_TabItemSelectedBorderLightCustom)
		{
			m_TabItemSelectedBorderLight = office2007TabColorTable.Selected.InnerBorder;
		}
		if (!m_TabItemSelectedBorderDarkCustom)
		{
			m_TabItemSelectedBorderDark = Color.Empty;
		}
		if (!m_TabItemSelectedBackgroundCustom)
		{
			m_TabItemSelectedBackground = office2007TabColorTable.Selected.TopBackground.Start;
		}
		if (!m_TabItemSelectedBackground2Custom)
		{
			m_TabItemSelectedBackground2 = office2007TabColorTable.Selected.BottomBackground.End;
		}
		if (!m_TabItemSelectedBackgroundCustom)
		{
			m_TabItemSelectedBackgroundColorBlend.Clear();
			m_TabItemSelectedBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.Selected.TopBackground.Start, 0f));
			m_TabItemSelectedBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.Selected.TopBackground.End, 0.45f));
			m_TabItemSelectedBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.Selected.BottomBackground.Start, 0.45f));
			m_TabItemSelectedBackgroundColorBlend.Add(new BackgroundColorBlend(office2007TabColorTable.Selected.BottomBackground.End, 1f));
		}
		if (!m_TabItemSelectedTextCustom)
		{
			m_TabItemSelectedText = office2007TabColorTable.Selected.Text;
		}
		if (!m_TabPanelBackgroundCustom)
		{
			m_TabPanelBackground = office2007TabColorTable.TabPanelBackground.Start;
		}
		if (!m_TabPanelBackground2Custom)
		{
			m_TabPanelBackground2 = office2007TabColorTable.TabPanelBackground.End;
		}
		if (!m_TabPanelBorderCustom)
		{
			m_TabPanelBorder = office2007TabColorTable.TabPanelBorder;
		}
	}

	protected virtual void InvokeColorChanged()
	{
		if (this.ColorChanged != null)
		{
			this.ColorChanged(this, new EventArgs());
		}
	}

	public void ResetChangedFlag()
	{
		m_TabBackgroundCustom = false;
		m_TabBackground2Custom = false;
		m_TabBorderCustom = false;
		m_TabItemBackgroundCustom = false;
		m_TabItemBackground2Custom = false;
		m_TabItemBorderCustom = false;
		m_TabItemBorderDarkCustom = false;
		m_TabItemBorderLightCustom = false;
		m_TabItemTextCustom = false;
		m_TabItemHotBackgroundCustom = false;
		m_TabItemHotBackground2Custom = false;
		m_TabItemHotBorderCustom = false;
		m_TabItemHotBorderDarkCustom = false;
		m_TabItemHotBorderLightCustom = false;
		m_TabItemHotTextCustom = false;
		m_TabItemSelectedBackgroundCustom = false;
		m_TabItemSelectedBackground2Custom = false;
		m_TabItemSelectedBorderCustom = false;
		m_TabItemSelectedBorderDarkCustom = false;
		m_TabItemSelectedBorderLightCustom = false;
		m_TabItemSelectedTextCustom = false;
		m_TabItemSeparatorCustom = false;
		m_TabItemSeparatorShadeCustom = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabBackground()
	{
		return m_TabBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabBackground2()
	{
		return m_TabBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabBorder()
	{
		return m_TabBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabPanelBackground()
	{
		return m_TabPanelBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabPanelBackground2()
	{
		return m_TabPanelBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabPanelBorder()
	{
		return m_TabPanelBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBorder()
	{
		return m_TabItemBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBorderLight()
	{
		return m_TabItemBorderLightCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBorderDark()
	{
		return m_TabItemBorderDarkCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBackground()
	{
		return m_TabItemBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBackground2()
	{
		return m_TabItemBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemText()
	{
		return m_TabItemTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBorder()
	{
		return m_TabItemHotBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBorderLight()
	{
		return m_TabItemHotBorderLightCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBorderDark()
	{
		return m_TabItemHotBorderDarkCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBackground()
	{
		return m_TabItemHotBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBackground2()
	{
		return m_TabItemHotBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotText()
	{
		return m_TabItemHotTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBorder()
	{
		return m_TabItemSelectedBorderCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBorderLight()
	{
		return m_TabItemSelectedBorderLightCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBorderDark()
	{
		return m_TabItemSelectedBorderDarkCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBackground()
	{
		return m_TabItemSelectedBackgroundCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBackground2()
	{
		return m_TabItemSelectedBackground2Custom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedText()
	{
		return m_TabItemSelectedTextCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSeparator()
	{
		return m_TabItemSeparatorCustom;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSeparatorShade()
	{
		return m_TabItemSeparatorShadeCustom;
	}

	public static void ApplyPredefinedColor(TabItem item, eTabItemColor c)
	{
		GetPredefinedColors(c, out var color, out var color2);
		item.BackColor = color;
		item.BackColor2 = color2;
		item.BackColorGradientAngle = 90;
		if (c == eTabItemColor.Default)
		{
			item.TextColor = Color.Empty;
		}
		else
		{
			item.TextColor = Color.Black;
		}
	}

	public static void ApplyPredefinedColor(ISimpleTab item, eTabItemColor c)
	{
		GetPredefinedColors(c, out var color, out var color2);
		item.BackColor = color;
		item.BackColor2 = color2;
		item.BackColorGradientAngle = 90;
		item.TextColor = Color.Black;
		item.BorderColor = Color.Empty;
		item.DarkBorderColor = Color.FromArgb(190, Color.DimGray);
		item.LightBorderColor = Color.FromArgb(128, Color.White);
	}

	public static void GetPredefinedColors(eTabItemColor c, out Color color1, out Color color2)
	{
		switch (c)
		{
		case eTabItemColor.Apple:
			color1 = Color.FromArgb(232, 248, 224);
			color2 = Color.FromArgb(173, 231, 146);
			break;
		case eTabItemColor.Blue:
			color1 = Color.FromArgb(221, 230, 247);
			color2 = Color.FromArgb(138, 168, 228);
			break;
		case eTabItemColor.BlueMist:
			color1 = Color.FromArgb(227, 236, 243);
			color2 = Color.FromArgb(155, 187, 210);
			break;
		case eTabItemColor.Cyan:
			color1 = Color.FromArgb(227, 236, 243);
			color2 = Color.FromArgb(155, 187, 210);
			break;
		case eTabItemColor.Green:
			color1 = Color.FromArgb(234, 240, 226);
			color2 = Color.FromArgb(183, 201, 151);
			break;
		case eTabItemColor.Lemon:
			color1 = Color.FromArgb(252, 253, 215);
			color2 = Color.FromArgb(245, 249, 111);
			break;
		case eTabItemColor.Magenta:
			color1 = Color.FromArgb(243, 229, 236);
			color2 = Color.FromArgb(213, 164, 187);
			break;
		case eTabItemColor.Orange:
			color1 = Color.FromArgb(252, 233, 217);
			color2 = Color.FromArgb(246, 176, 120);
			break;
		case eTabItemColor.Purple:
			color1 = Color.FromArgb(234, 227, 245);
			color2 = Color.FromArgb(180, 158, 222);
			break;
		case eTabItemColor.PurpleMist:
			color1 = Color.FromArgb(232, 227, 234);
			color2 = Color.FromArgb(171, 156, 183);
			break;
		case eTabItemColor.Red:
			color1 = Color.FromArgb(249, 225, 226);
			color2 = Color.FromArgb(238, 149, 151);
			break;
		case eTabItemColor.Silver:
			color1 = Color.FromArgb(225, 225, 232);
			color2 = Color.FromArgb(149, 149, 170);
			break;
		case eTabItemColor.Tan:
			color1 = Color.FromArgb(248, 242, 226);
			color2 = Color.FromArgb(232, 209, 153);
			break;
		case eTabItemColor.Teal:
			color1 = Color.FromArgb(205, 236, 240);
			color2 = Color.FromArgb(78, 188, 202);
			break;
		case eTabItemColor.Yellow:
			color1 = Color.FromArgb(255, 244, 213);
			color2 = Color.FromArgb(255, 216, 105);
			break;
		default:
			color1 = Color.Empty;
			color2 = Color.Empty;
			break;
		}
	}
}
