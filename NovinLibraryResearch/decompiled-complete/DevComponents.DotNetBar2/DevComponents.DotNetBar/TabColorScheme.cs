using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
public class TabColorScheme
{
	private EventHandler ᗆ;

	private eTabStripStyle Ӷ = eTabStripStyle.OneNote;

	private Color _1CFC = Color.Empty;

	private bool _1CFD;

	private Color _1CFE = Color.Empty;

	private bool _1CFF;

	private int ᴀ = 90;

	private Color ᴁ = Color.Empty;

	private bool ᴂ;

	private Color ᴃ = Color.Empty;

	private bool ᴄ;

	private Color ᴅ = Color.Empty;

	private bool ᴆ;

	private int ᴇ = 90;

	private Color ᴈ = Color.Empty;

	private bool ᴉ;

	private Color ᴊ = Color.Empty;

	private bool ᴋ;

	private Color ᴌ = Color.Empty;

	private bool ᴍ;

	private Color ᴎ = Color.Empty;

	private bool ᴏ;

	private Color ᴐ = Color.Empty;

	private bool ᴑ;

	private Color ᴒ = Color.Empty;

	private bool ᴓ;

	private int ᴔ = 90;

	private BackgroundColorBlendCollection ᴕ = new BackgroundColorBlendCollection();

	private Color ᴖ = Color.Empty;

	private bool ᴗ;

	private Color ᴘ = Color.Empty;

	private bool ᴙ;

	private Color ᴚ = Color.Empty;

	private bool ᴛ;

	private Color ᴜ = Color.Empty;

	private bool ᴝ;

	private Color ᴞ = Color.Empty;

	private bool ᴟ;

	private Color ᴠ = Color.Empty;

	private bool ᴡ;

	private int ᴢ = 90;

	private BackgroundColorBlendCollection ᴣ = new BackgroundColorBlendCollection();

	private Color ᴤ = Color.Empty;

	private bool ᴥ;

	private Color ᴦ = Color.Empty;

	private bool ᴧ;

	private Color ᴨ = Color.Empty;

	private bool ᴩ;

	private Color ᴪ = Color.Empty;

	private bool ᴫ;

	private Color ᴬ = Color.Empty;

	private bool ᴭ;

	private Color ᴮ = Color.Empty;

	private bool ᴯ;

	private int ᴰ = 90;

	private BackgroundColorBlendCollection ᴱ = new BackgroundColorBlendCollection();

	private Color ᴲ = Color.Empty;

	private bool ᴳ;

	private Color ᴴ = Color.Empty;

	private bool ᴵ;

	private Color ᴶ = Color.Empty;

	private bool ᴷ;

	private bool ᴸ;

	private bool ᴹ;

	private ᵋ _ᴺ;

	private Image _ᴻ;

	private Color ᴼ = Color.Empty;

	private bool ᴽ;

	internal ᵋ ᳺ
	{
		get
		{
			return _ᴺ;
		}
		set
		{
			_ᴺ = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[Description("Indicates style that color scheme represents.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(eTabStripStyle.OneNote)]
	public eTabStripStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			if (Ӷ != value)
			{
				Ӷ = value;
				Refresh();
			}
		}
	}

	internal bool _1CFB
	{
		get
		{
			return ᴸ;
		}
		set
		{
			ᴸ = value;
			Refresh();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool SchemeChanged
	{
		get
		{
			if (!_1CFF && !_1CFD && !ᴂ && !ᴓ && !ᴑ && !ᴋ && !ᴏ && !ᴍ && !ᴗ && !ᴡ && !ᴟ && !ᴙ && !ᴝ && !ᴛ && !ᴥ && !ᴯ && !ᴭ && !ᴧ && !ᴫ && !ᴩ && !ᴳ && !ᴵ && !ᴷ)
			{
				return ᴹ;
			}
			return true;
		}
	}

	[Category("Tab Control Colors")]
	[Description("Indicates tab-strip background image.")]
	[DefaultValue(null)]
	public Image TabBackgroundImage
	{
		get
		{
			return _ᴻ;
		}
		set
		{
			_ᴻ = value;
		}
	}

	[Description("Specifies the background color of the tab control.")]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Browsable(true)]
	public Color TabBackground
	{
		get
		{
			return _1CFC;
		}
		set
		{
			if (_1CFC != value)
			{
				_1CFC = value;
				_1CFD = true;
				InvokeColorChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Browsable(true)]
	[Description("Specifies the target gradient background color of the tab control.")]
	public Color TabBackground2
	{
		get
		{
			return _1CFE;
		}
		set
		{
			if (_1CFE != value)
			{
				_1CFE = value;
				_1CFF = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	[Category("Tab Control Colors")]
	public int TabBackgroundGradientAngle
	{
		get
		{
			return ᴀ;
		}
		set
		{
			ᴀ = value;
			InvokeColorChanged();
			ᴹ = true;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Browsable(true)]
	[Description("Specifies the border color of the tab control.")]
	public Color TabBorder
	{
		get
		{
			return ᴁ;
		}
		set
		{
			if (ᴁ != value)
			{
				ᴁ = value;
				ᴂ = true;
				InvokeColorChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Browsable(true)]
	[Description("Specifies the background color of the tab panel.")]
	public Color TabPanelBackground
	{
		get
		{
			return ᴃ;
		}
		set
		{
			if (ᴃ != value)
			{
				ᴃ = value;
				ᴄ = true;
				InvokeColorChanged();
			}
		}
	}

	[Description("Specifies the target gradient background color of the tab panel.")]
	[Category("Tab Control Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color TabPanelBackground2
	{
		get
		{
			return ᴅ;
		}
		set
		{
			if (ᴅ != value)
			{
				ᴅ = value;
				ᴆ = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the gradient angle.")]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[DefaultValue(90)]
	public int TabPanelBackgroundGradientAngle
	{
		get
		{
			return ᴇ;
		}
		set
		{
			ᴇ = value;
			InvokeColorChanged();
			ᴹ = true;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Browsable(true)]
	[Description("Specifies the border color of the tab panel.")]
	public Color TabPanelBorder
	{
		get
		{
			return ᴈ;
		}
		set
		{
			if (ᴈ != value)
			{
				ᴈ = value;
				ᴉ = true;
				InvokeColorChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the border color of the tab item.")]
	[Browsable(true)]
	[Category("Tab Control Colors")]
	public Color TabItemBorder
	{
		get
		{
			return ᴊ;
		}
		set
		{
			if (ᴊ != value)
			{
				ᴊ = value;
				ᴋ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Browsable(true)]
	[Description("Specifies the light border color of the tab item.")]
	[DevCoBrowsable(true)]
	public Color TabItemBorderLight
	{
		get
		{
			return ᴌ;
		}
		set
		{
			if (ᴌ != value)
			{
				ᴌ = value;
				ᴍ = true;
				InvokeColorChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the dark border color of the tab item.")]
	[Browsable(true)]
	public Color TabItemBorderDark
	{
		get
		{
			return ᴎ;
		}
		set
		{
			if (ᴎ != value)
			{
				ᴎ = value;
				ᴏ = true;
				InvokeColorChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the background color of the tab item.")]
	[Category("Tab Control Colors")]
	[Browsable(true)]
	public Color TabItemBackground
	{
		get
		{
			return ᴐ;
		}
		set
		{
			if (ᴐ != value)
			{
				ᴐ = value;
				ᴑ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Description("Specifies the target gradient background color of the tab item.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color TabItemBackground2
	{
		get
		{
			return ᴒ;
		}
		set
		{
			if (ᴒ != value)
			{
				ᴒ = value;
				ᴓ = true;
				InvokeColorChanged();
			}
		}
	}

	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	[Category("Tab Control Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int TabItemBackgroundGradientAngle
	{
		get
		{
			return ᴔ;
		}
		set
		{
			ᴔ = value;
			InvokeColorChanged();
			ᴹ = true;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection that defines the multicolor gradient background.")]
	[Browsable(true)]
	public BackgroundColorBlendCollection TabItemBackgroundColorBlend => ᴕ;

	[Description("Specifies the text of the tab item.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	public Color TabItemText
	{
		get
		{
			return ᴖ;
		}
		set
		{
			if (ᴖ != value)
			{
				ᴖ = value;
				ᴗ = true;
				InvokeColorChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	[Description("Specifies the border color of the tab item when mouse is over it.")]
	[Browsable(true)]
	public Color TabItemHotBorder
	{
		get
		{
			return ᴘ;
		}
		set
		{
			if (ᴘ != value)
			{
				ᴘ = value;
				ᴙ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Description("Specifies the light border color of the tab item when mouse is over it.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color TabItemHotBorderLight
	{
		get
		{
			return ᴚ;
		}
		set
		{
			if (ᴚ != value)
			{
				ᴚ = value;
				ᴛ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Description("Specifies the dark border color of the tab item when mouse is over it.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color TabItemHotBorderDark
	{
		get
		{
			return ᴜ;
		}
		set
		{
			if (ᴜ != value)
			{
				ᴜ = value;
				ᴝ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Browsable(true)]
	[Description("Specifies the background color of the tab item when mouse is over it.")]
	[DevCoBrowsable(true)]
	public Color TabItemHotBackground
	{
		get
		{
			return ᴞ;
		}
		set
		{
			if (ᴞ != value)
			{
				ᴞ = value;
				ᴟ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Description("Specifies the target gradient background color of the tab item when mouse is over it.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color TabItemHotBackground2
	{
		get
		{
			return ᴠ;
		}
		set
		{
			if (ᴠ != value)
			{
				ᴠ = value;
				ᴡ = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(90)]
	[Description("Specifies the gradient angle.")]
	[Category("Tab Control Colors")]
	public int TabItemHotBackgroundGradientAngle
	{
		get
		{
			return ᴢ;
		}
		set
		{
			ᴢ = value;
			InvokeColorChanged();
			ᴹ = true;
		}
	}

	[Description("Collection that defines the multicolor gradient background.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public BackgroundColorBlendCollection TabItemHotBackgroundColorBlend => ᴣ;

	[Description("Specifies the text color of the tab item when mouse is over it.")]
	[Category("Tab Control Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color TabItemHotText
	{
		get
		{
			return ᴤ;
		}
		set
		{
			if (ᴤ != value)
			{
				ᴤ = value;
				ᴥ = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the border color of the tab item when selected.")]
	[Category("Tab Control Colors")]
	public Color TabItemSelectedBorder
	{
		get
		{
			return ᴦ;
		}
		set
		{
			if (ᴦ != value)
			{
				ᴦ = value;
				ᴧ = true;
				InvokeColorChanged();
			}
		}
	}

	[Description("Specifies the light border color of the tab item when selected.")]
	[Browsable(true)]
	[Category("Tab Control Colors")]
	[DevCoBrowsable(true)]
	public Color TabItemSelectedBorderLight
	{
		get
		{
			return ᴨ;
		}
		set
		{
			if (ᴨ != value)
			{
				ᴨ = value;
				ᴩ = true;
				InvokeColorChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Specifies the dark border color of the tab item when selected.")]
	[Category("Tab Control Colors")]
	public Color TabItemSelectedBorderDark
	{
		get
		{
			return ᴪ;
		}
		set
		{
			if (ᴪ != value)
			{
				ᴪ = value;
				ᴫ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the background color of the tab item when selected.")]
	public Color TabItemSelectedBackground
	{
		get
		{
			return ᴬ;
		}
		set
		{
			if (ᴬ != value)
			{
				ᴬ = value;
				ᴭ = true;
				InvokeColorChanged();
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the target gradient background color of the tab item when selected.")]
	[Category("Tab Control Colors")]
	[DevCoBrowsable(true)]
	public Color TabItemSelectedBackground2
	{
		get
		{
			return ᴮ;
		}
		set
		{
			if (ᴮ != value)
			{
				ᴮ = value;
				ᴯ = true;
				InvokeColorChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(90)]
	[Description("Specifies the gradient angle.")]
	[Category("Tab Control Colors")]
	public int TabItemSelectedBackgroundGradientAngle
	{
		get
		{
			return ᴰ;
		}
		set
		{
			ᴰ = value;
			InvokeColorChanged();
			ᴹ = true;
		}
	}

	[Description("Collection that defines the multicolor gradient background.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public BackgroundColorBlendCollection TabItemSelectedBackgroundColorBlend => ᴱ;

	[Description("Specifies the text color of the tab item when selected.")]
	[Category("Tab Control Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color TabItemSelectedText
	{
		get
		{
			return ᴲ;
		}
		set
		{
			if (ᴲ != value)
			{
				ᴲ = value;
				ᴳ = true;
				InvokeColorChanged();
			}
		}
	}

	[Description("Specifies the tab item separator color.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Control Colors")]
	public Color TabItemSeparator
	{
		get
		{
			return ᴴ;
		}
		set
		{
			if (ᴴ != value)
			{
				ᴴ = value;
				ᴵ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Description("Specifies the tab item separator shadow color.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color TabItemSeparatorShade
	{
		get
		{
			return ᴶ;
		}
		set
		{
			if (ᴶ != value)
			{
				ᴶ = value;
				ᴷ = true;
				InvokeColorChanged();
			}
		}
	}

	[Category("Tab Control Colors")]
	[Browsable(true)]
	[Description("Specifies foreground color of tab-strip navigation box.")]
	[DevCoBrowsable(true)]
	public Color NavigationBoxForeColor
	{
		get
		{
			return ᴼ;
		}
		set
		{
			if (ᴼ != value)
			{
				ᴼ = value;
				ᴽ = true;
				InvokeColorChanged();
			}
		}
	}

	public event EventHandler ColorChanged
	{
		add
		{
			EventHandler eventHandler = ᗆ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗆ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗆ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗆ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public TabColorScheme()
	{
		Refresh();
	}

	public TabColorScheme(eTabStripStyle style)
	{
		Ӷ = style;
		Refresh();
	}

	public void Refresh()
	{
		_ᴻ = null;
		if (ᴸ)
		{
			ᵄ();
			return;
		}
		switch (Ӷ)
		{
		case eTabStripStyle.Metro:
			if (StyleManager.IsVisualStudio2012(StyleManager.Style))
			{
				ᴾ();
			}
			else
			{
				ᴿ();
			}
			break;
		case eTabStripStyle.Flat:
			ᵁ();
			break;
		case eTabStripStyle.Office2003:
			ᵂ();
			break;
		case eTabStripStyle.Office2007Document:
		case eTabStripStyle.Office2007Dock:
			ᵊ();
			break;
		case eTabStripStyle.VS2005:
			ᵆ(ᵇ: false);
			break;
		case eTabStripStyle.VS2005Dock:
			ᵆ(ᵇ: true);
			break;
		case eTabStripStyle.SimulatedTheme:
			ᵆ(ᵇ: true);
			ᵀ();
			break;
		case eTabStripStyle.RoundHeader:
			ᵈ();
			break;
		case eTabStripStyle.VS2005Document:
			ᵃ();
			break;
		default:
			ᵅ();
			break;
		}
		if (!ᴗ && !ᴐ.IsEmpty && (double)Math.Abs(ᴖ.GetBrightness() - ᴐ.GetBrightness()) <= 0.2)
		{
			ᴖ = (((double)ᴖ.GetBrightness() < 0.5) ? ControlPaint.Light(ᴖ) : ControlPaint.Dark(ᴖ));
		}
		if (!ᴳ && !ᴬ.IsEmpty && (double)Math.Abs(ᴲ.GetBrightness() - ᴬ.GetBrightness()) <= 0.2)
		{
			ᴲ = (((double)ᴲ.GetBrightness() < 0.5) ? ControlPaint.Light(ᴲ) : ControlPaint.Dark(ᴲ));
		}
	}

	private void ᴾ()
	{
		Office2007TabColorTable office2007TabColorTable = null;
		Office2007ColorTable office2007ColorTable = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			office2007TabColorTable = ((_ᴺ != ᵋ.ᵌ || office2007ColorTable.DockTabStrip == null) ? office2007ColorTable.TabControl : office2007ColorTable.DockTabStrip);
		}
		if (office2007TabColorTable == null)
		{
			ᵅ();
			return;
		}
		if (!ᴵ)
		{
			ᴴ = Color.Empty;
		}
		if (!ᴷ)
		{
			ᴶ = Color.Empty;
		}
		if (!_1CFD)
		{
			if (office2007TabColorTable.TabBackground.Start.IsEmpty)
			{
				_1CFC = office2007ColorTable.Form.BackColor;
			}
			else
			{
				_1CFC = office2007TabColorTable.TabBackground.Start;
			}
		}
		if (!_1CFF)
		{
			_1CFE = office2007TabColorTable.TabBackground.End;
		}
		ᴀ = office2007TabColorTable.TabBackground.GradientAngle;
		if (!ᴂ)
		{
			ᴁ = office2007TabColorTable.TabPanelBorder;
		}
		if (!ᴋ)
		{
			ᴊ = Color.Empty;
		}
		if (!ᴍ)
		{
			ᴌ = Color.Empty;
		}
		if (!ᴏ)
		{
			ᴎ = Color.Empty;
		}
		if (!ᴑ)
		{
			ᴐ = Color.Empty;
		}
		if (!ᴓ)
		{
			ᴒ = Color.Empty;
		}
		if (!ᴗ)
		{
			if (office2007TabColorTable.TabBackground.Start.IsEmpty)
			{
				ᴖ = office2007ColorTable.Form.TextColor;
			}
			else
			{
				ᴖ = office2007TabColorTable.Default.Text;
			}
		}
		if (!ᴙ)
		{
			ᴘ = Color.Empty;
		}
		if (!ᴛ)
		{
			ᴚ = Color.Empty;
		}
		if (!ᴝ)
		{
			ᴜ = Color.Empty;
		}
		if (!ᴟ && office2007TabColorTable.MouseOver.TopBackground != null)
		{
			ᴞ = office2007TabColorTable.MouseOver.TopBackground.Start;
		}
		if (!ᴡ)
		{
			ᴠ = Color.Empty;
		}
		if (!ᴥ)
		{
			ᴤ = office2007TabColorTable.MouseOver.Text;
		}
		if (!ᴧ)
		{
			if (office2007TabColorTable.Selected.OuterBorder.IsEmpty)
			{
				ᴦ = ᴖ;
			}
			else
			{
				ᴦ = office2007TabColorTable.Selected.OuterBorder;
			}
		}
		if (!ᴩ)
		{
			ᴨ = Color.Empty;
		}
		if (!ᴫ)
		{
			ᴪ = Color.Empty;
		}
		if (!ᴭ && office2007TabColorTable.Selected.TopBackground != null)
		{
			ᴬ = office2007TabColorTable.Selected.TopBackground.Start;
		}
		if (!ᴯ)
		{
			ᴮ = Color.Empty;
		}
		if (!ᴳ)
		{
			ᴲ = office2007TabColorTable.Selected.Text;
		}
		if (!ᴄ)
		{
			ᴃ = _1CFC;
		}
		if (!ᴆ)
		{
			ᴅ = _1CFE;
		}
		if (!ᴉ)
		{
			ᴈ = ᴁ;
		}
	}

	private void ᴿ()
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
			ᵅ();
			return;
		}
		if (!ᴵ)
		{
			ᴴ = Color.Empty;
		}
		if (!ᴷ)
		{
			ᴶ = Color.Empty;
		}
		if (!_1CFD)
		{
			if (office2007TabColorTable.TabBackground.Start.IsEmpty)
			{
				_1CFC = office2007ColorTable.Form.BackColor;
			}
			else
			{
				_1CFC = office2007TabColorTable.TabBackground.Start;
			}
		}
		if (!_1CFF)
		{
			_1CFE = office2007TabColorTable.TabBackground.End;
		}
		ᴀ = office2007TabColorTable.TabBackground.GradientAngle;
		if (!ᴂ)
		{
			ᴁ = office2007TabColorTable.TabPanelBorder;
		}
		if (!ᴋ)
		{
			ᴊ = Color.Empty;
		}
		if (!ᴍ)
		{
			ᴌ = Color.Empty;
		}
		if (!ᴏ)
		{
			ᴎ = Color.Empty;
		}
		if (!ᴑ)
		{
			ᴐ = Color.Empty;
		}
		if (!ᴓ)
		{
			ᴒ = Color.Empty;
		}
		if (!ᴗ)
		{
			if (office2007TabColorTable.TabBackground.Start.IsEmpty)
			{
				ᴖ = office2007ColorTable.Form.TextColor;
			}
			else
			{
				ᴖ = office2007TabColorTable.Default.Text;
			}
		}
		if (!ᴙ)
		{
			ᴘ = Color.Empty;
		}
		if (!ᴛ)
		{
			ᴚ = Color.Empty;
		}
		if (!ᴝ)
		{
			ᴜ = Color.Empty;
		}
		if (!ᴟ)
		{
			ᴞ = Color.Empty;
		}
		if (!ᴡ)
		{
			ᴠ = Color.Empty;
		}
		if (!ᴥ)
		{
			ᴤ = office2007TabColorTable.MouseOver.Text;
		}
		if (!ᴧ)
		{
			if (office2007TabColorTable.Selected.OuterBorder.IsEmpty)
			{
				ᴦ = ᴖ;
			}
			else
			{
				ᴦ = office2007TabColorTable.Selected.OuterBorder;
			}
		}
		if (!ᴩ)
		{
			ᴨ = Color.Empty;
		}
		if (!ᴫ)
		{
			ᴪ = Color.Empty;
		}
		if (!ᴭ)
		{
			ᴬ = _1CFC;
		}
		if (!ᴯ)
		{
			ᴮ = Color.Empty;
		}
		if (!ᴳ)
		{
			ᴲ = office2007TabColorTable.Selected.Text;
		}
		if (!ᴄ)
		{
			ᴃ = _1CFC;
		}
		if (!ᴆ)
		{
			ᴅ = _1CFE;
		}
		if (!ᴉ)
		{
			ᴈ = ᴁ;
		}
	}

	private void ᵀ()
	{
		if (BarFunctions._0945 == _0883._0884)
		{
			if (!ᴫ)
			{
				ᴪ = SystemColors.Highlight;
			}
			if (!ᴩ)
			{
				ᴨ = Color.FromArgb(128, SystemColors.Highlight);
			}
		}
		else
		{
			if (!ᴫ)
			{
				ᴪ = _065A("E68B2C");
			}
			if (!ᴩ)
			{
				ᴨ = _065A("FFC73C");
			}
		}
		if (!ᴝ)
		{
			ᴜ = ᴪ;
		}
		if (!ᴛ)
		{
			ᴚ = ᴨ;
		}
		if (!ᴙ)
		{
			ᴘ = ᴊ;
		}
		if (!ᴍ)
		{
			ᴌ = SystemColors.ControlLightLight;
		}
		if (!ᴏ)
		{
			ᴎ = SystemColors.Control;
		}
	}

	private void ᵁ()
	{
		if (!ᴵ)
		{
			ᴴ = SystemColors.ControlDark;
		}
		if (!ᴷ)
		{
			ᴶ = Color.Empty;
		}
		if (!_1CFD)
		{
			_1CFC = ᕭ();
		}
		if (!_1CFF)
		{
			_1CFE = Color.Empty;
		}
		if (!ᴂ)
		{
			ᴁ = SystemColors.ControlDark;
		}
		if (!ᴋ)
		{
			ᴊ = Color.Empty;
		}
		if (!ᴍ)
		{
			ᴌ = Color.Empty;
		}
		if (!ᴏ)
		{
			ᴎ = Color.Empty;
		}
		if (!ᴑ)
		{
			ᴐ = Color.Empty;
		}
		if (!ᴓ)
		{
			ᴒ = Color.Empty;
		}
		if (!ᴗ)
		{
			ᴖ = SystemColors.ControlDark;
		}
		if (!ᴙ)
		{
			ᴘ = Color.Empty;
		}
		if (!ᴛ)
		{
			ᴚ = Color.Empty;
		}
		if (!ᴝ)
		{
			ᴜ = Color.Empty;
		}
		if (!ᴟ)
		{
			ᴞ = Color.Empty;
		}
		if (!ᴡ)
		{
			ᴠ = Color.Empty;
		}
		if (!ᴥ)
		{
			ᴤ = Color.Empty;
		}
		if (!ᴧ)
		{
			ᴦ = SystemColors.ControlText;
		}
		if (!ᴩ)
		{
			ᴨ = SystemColors.ControlLightLight;
		}
		if (!ᴫ)
		{
			ᴪ = Color.Empty;
		}
		if (!ᴭ)
		{
			ᴬ = SystemColors.Control;
		}
		if (!ᴯ)
		{
			ᴮ = Color.Empty;
		}
		if (!ᴳ)
		{
			ᴲ = SystemColors.ControlText;
		}
		if (!ᴄ)
		{
			ᴃ = ᴬ;
		}
		if (!ᴆ)
		{
			ᴅ = Color.Empty;
		}
		if (!ᴉ)
		{
			ᴈ = SystemColors.ControlDark;
		}
	}

	private Color ᕭ()
	{
		Color result = ControlPaint.Light(SystemColors.Control);
		switch (BarFunctions._0945)
		{
		case _0883._0885:
			result = Color.FromArgb(255, 251, 233);
			break;
		case _0883._0886:
			result = Color.FromArgb(255, 251, 233);
			break;
		case _0883._0887:
			result = Color.FromArgb(251, 250, 255);
			break;
		}
		return result;
	}

	private void ᵂ()
	{
		ColorScheme colorScheme = new ColorScheme();
		colorScheme.ߕ = eDotNetBarStyle.Office2003;
		if (!ᴵ)
		{
			ᴴ = colorScheme.ItemSeparator;
		}
		if (!ᴷ)
		{
			ᴶ = colorScheme.ItemSeparatorShade;
		}
		if (!_1CFD)
		{
			_1CFC = colorScheme.BarBackground;
		}
		if (!_1CFF)
		{
			_1CFE = colorScheme.BarBackground2;
		}
		if (!ᴂ)
		{
			ᴁ = Color.Empty;
		}
		if (!ᴋ)
		{
			ᴊ = Color.Empty;
		}
		if (!ᴍ)
		{
			ᴌ = Color.Empty;
		}
		if (!ᴏ)
		{
			ᴎ = Color.Empty;
		}
		if (!ᴑ)
		{
			ᴐ = Color.Empty;
		}
		if (!ᴓ)
		{
			ᴒ = Color.Empty;
		}
		if (!ᴗ)
		{
			ᴖ = SystemColors.ControlText;
		}
		if (!ᴙ)
		{
			ᴘ = Color.Empty;
		}
		if (!ᴛ)
		{
			ᴚ = Color.Empty;
		}
		if (!ᴝ)
		{
			ᴜ = Color.Empty;
		}
		if (!ᴟ)
		{
			ᴞ = Color.Empty;
		}
		if (!ᴡ)
		{
			ᴠ = Color.Empty;
		}
		if (!ᴥ)
		{
			ᴤ = Color.Empty;
		}
		if (!ᴧ)
		{
			ᴦ = colorScheme.ItemPressedBorder;
		}
		if (!ᴩ)
		{
			ᴨ = colorScheme.ItemPressedBorder;
		}
		if (!ᴫ)
		{
			ᴪ = Color.Empty;
		}
		if (!ᴭ)
		{
			ᴬ = colorScheme.ItemPressedBackground;
		}
		if (!ᴯ)
		{
			ᴮ = colorScheme.ItemPressedBackground2;
		}
		if (!ᴳ)
		{
			ᴲ = colorScheme.ItemPressedText;
		}
		if (!ᴄ)
		{
			ᴃ = _1CFE;
		}
		if (!ᴆ)
		{
			ᴅ = _1CFC;
		}
		if (!ᴉ)
		{
			ᴈ = ᴦ;
		}
	}

	private void ᵃ()
	{
		ColorScheme colorScheme = new ColorScheme();
		colorScheme.ߕ = eDotNetBarStyle.VS2005;
		if (!ᴵ)
		{
			ᴴ = colorScheme.ItemSeparator;
		}
		if (!ᴷ)
		{
			ᴶ = colorScheme.ItemSeparatorShade;
		}
		if (!_1CFD)
		{
			_1CFC = colorScheme.BarBackground;
		}
		if (!_1CFF)
		{
			_1CFE = Color.Empty;
		}
		if (!ᴂ)
		{
			ᴁ = Color.Empty;
		}
		if (!ᴋ)
		{
			ᴊ = _065A("ACA899");
		}
		if (!ᴍ)
		{
			ᴌ = Color.White;
		}
		if (!ᴏ)
		{
			ᴎ = _065A("ECE9D8");
		}
		if (!ᴑ)
		{
			ᴐ = _065A("FDFCFB");
		}
		if (!ᴓ)
		{
			ᴒ = _065A("F1EFE2");
		}
		if (!ᴗ)
		{
			ᴖ = SystemColors.ControlText;
		}
		if (!ᴙ)
		{
			ᴘ = Color.Empty;
		}
		if (!ᴛ)
		{
			ᴚ = Color.Empty;
		}
		if (!ᴝ)
		{
			ᴜ = Color.Empty;
		}
		if (!ᴟ)
		{
			ᴞ = Color.Empty;
		}
		if (!ᴡ)
		{
			ᴠ = Color.Empty;
		}
		if (!ᴥ)
		{
			ᴤ = Color.Empty;
		}
		if (!ᴧ)
		{
			ᴦ = _065A("7F9DB9");
		}
		if (!ᴩ)
		{
			ᴨ = Color.Empty;
		}
		if (!ᴫ)
		{
			ᴪ = Color.Empty;
		}
		if (!ᴭ)
		{
			ᴬ = Color.White;
		}
		if (!ᴯ)
		{
			ᴮ = Color.Empty;
		}
		if (!ᴳ)
		{
			ᴲ = SystemColors.ControlText;
		}
		if (!ᴄ)
		{
			ᴃ = _1CFE;
		}
		if (!ᴆ)
		{
			ᴅ = _1CFC;
		}
		if (!ᴉ)
		{
			ᴈ = ᴦ;
		}
	}

	private void ᵄ()
	{
		if (!ᴵ)
		{
			ᴴ = SystemColors.ControlDark;
		}
		if (!ᴷ)
		{
			ᴶ = Color.Empty;
		}
		if (!_1CFD)
		{
			_1CFC = Color.Empty;
		}
		if (!_1CFF)
		{
			_1CFE = Color.Empty;
		}
		if (!ᴂ)
		{
			ᴁ = Color.Empty;
		}
		if (!ᴋ)
		{
			ᴊ = Color.Empty;
		}
		if (!ᴍ)
		{
			ᴌ = Color.Empty;
		}
		if (!ᴏ)
		{
			ᴎ = Color.Empty;
		}
		if (!ᴑ)
		{
			ᴐ = Color.Empty;
		}
		if (!ᴓ)
		{
			ᴒ = Color.Empty;
		}
		if (!ᴗ)
		{
			ᴖ = SystemColors.ControlText;
		}
		if (!ᴄ)
		{
			ᴃ = SystemColors.Control;
		}
		if (!ᴆ)
		{
			ᴅ = Color.Empty;
		}
		if (!ᴉ)
		{
			ᴈ = Color.Empty;
		}
		if (!ᴙ)
		{
			ᴘ = Color.Empty;
		}
		if (!ᴛ)
		{
			ᴚ = Color.Empty;
		}
		if (!ᴝ)
		{
			ᴜ = Color.Empty;
		}
		if (!ᴟ)
		{
			ᴞ = Color.Empty;
		}
		if (!ᴡ)
		{
			ᴠ = Color.Empty;
		}
		if (!ᴥ)
		{
			ᴤ = Color.Empty;
		}
		if (!ᴧ)
		{
			ᴦ = SystemColors.ControlText;
		}
		if (!ᴩ)
		{
			ᴨ = SystemColors.ControlLightLight;
		}
		if (!ᴫ)
		{
			ᴪ = Color.Empty;
		}
		if (!ᴭ)
		{
			ᴬ = SystemColors.Control;
		}
		if (!ᴯ)
		{
			ᴮ = Color.Empty;
		}
		if (!ᴳ)
		{
			ᴲ = SystemColors.ControlText;
		}
	}

	private void ᵅ()
	{
		_0883 obj = BarFunctions._0945;
		if (!ᴵ)
		{
			ᴴ = Color.Empty;
		}
		if (!ᴷ)
		{
			ᴶ = Color.Empty;
		}
		ColorScheme colorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
		switch (obj)
		{
		case _0883._0885:
			if (!_1CFD)
			{
				_1CFC = Color.FromArgb(196, 218, 250);
			}
			if (!_1CFF)
			{
				_1CFE = Color.FromArgb(253, 254, 255);
			}
			if (!ᴂ)
			{
				ᴁ = Color.Empty;
			}
			if (!ᴋ)
			{
				ᴊ = Color.FromArgb(0, 53, 154);
			}
			if (!ᴍ)
			{
				ᴌ = Color.White;
			}
			if (!ᴏ)
			{
				ᴎ = Color.FromArgb(117, 166, 241);
			}
			if (!ᴑ)
			{
				ᴐ = Color.FromArgb(236, 243, 252);
			}
			if (!ᴓ)
			{
				ᴒ = Color.FromArgb(191, 214, 246);
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = ᴒ;
			}
			if (!ᴆ)
			{
				ᴅ = Color.White;
			}
			if (!ᴉ)
			{
				ᴈ = Color.Empty;
			}
			if (!ᴙ)
			{
				ᴘ = Color.FromArgb(0, 0, 128);
			}
			if (!ᴛ)
			{
				ᴚ = Color.FromArgb(0, 0, 128);
			}
			if (!ᴝ)
			{
				ᴜ = Color.FromArgb(0, 0, 128);
			}
			if (!ᴟ)
			{
				ᴞ = Color.FromArgb(255, 249, 237);
			}
			if (!ᴡ)
			{
				ᴠ = Color.FromArgb(255, 240, 199);
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = colorScheme.BarDockedBorder;
			}
			if (!ᴩ)
			{
				ᴨ = Color.White;
			}
			if (!ᴫ)
			{
				ᴪ = ᴨ;
			}
			if (!ᴭ)
			{
				ᴬ = colorScheme.BarBackground;
			}
			if (!ᴯ)
			{
				ᴮ = colorScheme.BarBackground2;
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		case _0883._0887:
			if (!_1CFD)
			{
				_1CFC = Color.FromArgb(243, 243, 247);
			}
			if (!_1CFF)
			{
				_1CFE = Color.FromArgb(226, 226, 235);
			}
			if (!ᴂ)
			{
				ᴁ = Color.Empty;
			}
			if (!ᴋ)
			{
				ᴊ = Color.FromArgb(118, 116, 146);
			}
			if (!ᴍ)
			{
				ᴌ = Color.White;
			}
			if (!ᴏ)
			{
				ᴎ = Color.FromArgb(186, 185, 206);
			}
			if (!ᴑ)
			{
				ᴐ = Color.FromArgb(243, 243, 247);
			}
			if (!ᴓ)
			{
				ᴒ = Color.FromArgb(215, 215, 228);
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = Color.FromArgb(215, 215, 228);
			}
			if (!ᴆ)
			{
				ᴅ = Color.White;
			}
			if (!ᴉ)
			{
				ᴈ = Color.Empty;
			}
			if (!ᴙ)
			{
				ᴘ = Color.FromArgb(75, 75, 111);
			}
			if (!ᴛ)
			{
				ᴚ = Color.FromArgb(75, 75, 111);
			}
			if (!ᴝ)
			{
				ᴜ = Color.FromArgb(75, 75, 111);
			}
			if (!ᴟ)
			{
				ᴞ = Color.FromArgb(255, 249, 237);
			}
			if (!ᴡ)
			{
				ᴠ = Color.FromArgb(255, 240, 199);
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = colorScheme.BarDockedBorder;
			}
			if (!ᴩ)
			{
				ᴨ = Color.White;
			}
			if (!ᴫ)
			{
				ᴪ = ᴨ;
			}
			if (!ᴭ)
			{
				ᴬ = colorScheme.BarBackground;
			}
			if (!ᴯ)
			{
				ᴮ = colorScheme.BarBackground;
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		case _0883._0886:
			if (!_1CFD)
			{
				_1CFC = Color.FromArgb(242, 241, 228);
			}
			if (!_1CFF)
			{
				_1CFE = Color.FromArgb(230, 236, 209);
			}
			if (!ᴂ)
			{
				ᴁ = Color.Empty;
			}
			if (!ᴋ)
			{
				ᴊ = Color.FromArgb(96, 119, 107);
			}
			if (!ᴍ)
			{
				ᴌ = Color.White;
			}
			if (!ᴏ)
			{
				ᴎ = Color.FromArgb(176, 194, 140);
			}
			if (!ᴑ)
			{
				ᴐ = Color.FromArgb(244, 247, 236);
			}
			if (!ᴓ)
			{
				ᴒ = Color.FromArgb(221, 229, 192);
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = Color.FromArgb(221, 229, 192);
			}
			if (!ᴆ)
			{
				ᴅ = Color.White;
			}
			if (!ᴉ)
			{
				ᴈ = Color.Empty;
			}
			if (!ᴙ)
			{
				ᴘ = Color.FromArgb(63, 93, 56);
			}
			if (!ᴛ)
			{
				ᴚ = Color.FromArgb(63, 93, 56);
			}
			if (!ᴝ)
			{
				ᴜ = Color.FromArgb(63, 93, 56);
			}
			if (!ᴟ)
			{
				ᴞ = Color.FromArgb(255, 249, 237);
			}
			if (!ᴡ)
			{
				ᴠ = Color.FromArgb(255, 240, 199);
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = colorScheme.BarDockedBorder;
			}
			if (!ᴩ)
			{
				ᴨ = Color.White;
			}
			if (!ᴫ)
			{
				ᴪ = ᴨ;
			}
			if (!ᴭ)
			{
				ᴬ = colorScheme.BarBackground;
			}
			if (!ᴯ)
			{
				ᴮ = colorScheme.BarBackground;
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		default:
			if (!_1CFD)
			{
				_1CFC = colorScheme.BarBackground;
			}
			if (!_1CFF)
			{
				_1CFE = colorScheme.BarBackground2;
			}
			if (!ᴂ)
			{
				ᴁ = Color.Empty;
			}
			if (!ᴋ)
			{
				ᴊ = SystemColors.ControlDark;
			}
			if (!ᴍ)
			{
				ᴌ = Color.White;
			}
			if (!ᴏ)
			{
				ᴎ = SystemColors.ControlDarkDark;
			}
			if (!ᴑ)
			{
				ᴐ = _1CFE;
			}
			if (!ᴓ)
			{
				ᴒ = _1CFC;
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = _1CFE;
			}
			if (!ᴆ)
			{
				ᴅ = ControlPaint.LightLight(_1CFE);
			}
			if (!ᴉ)
			{
				ᴈ = Color.Empty;
			}
			if (!ᴙ)
			{
				ᴘ = colorScheme.BarDockedBorder;
			}
			if (!ᴛ)
			{
				ᴚ = colorScheme.BarDockedBorder;
			}
			if (!ᴝ)
			{
				ᴜ = colorScheme.BarDockedBorder;
			}
			if (!ᴟ)
			{
				ᴞ = colorScheme.ItemHotBackground;
			}
			if (!ᴡ)
			{
				ᴠ = colorScheme.ItemHotBackground2;
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = SystemColors.ControlDark;
			}
			if (!ᴩ)
			{
				ᴨ = SystemColors.ControlLightLight;
			}
			if (!ᴫ)
			{
				ᴪ = ᴨ;
			}
			if (!ᴭ)
			{
				ᴬ = colorScheme.ItemPressedBackground2;
			}
			if (!ᴯ)
			{
				ᴮ = colorScheme.ItemPressedBackground;
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		}
		if (!ᴄ)
		{
			ᴃ = ᴮ;
		}
		if (!ᴆ)
		{
			ᴅ = ᴬ;
		}
		if (!ᴉ)
		{
			ᴈ = ᴦ;
		}
	}

	private void ᵆ(bool ᵇ)
	{
		_0883 obj = BarFunctions._0945;
		ColorScheme colorScheme = new ColorScheme(eDotNetBarStyle.VS2005);
		switch (obj)
		{
		case _0883._0885:
			if (!_1CFD)
			{
				_1CFC = _065A("F4F2E8");
			}
			if (!_1CFF)
			{
				_1CFE = Color.Empty;
			}
			if (!ᴂ)
			{
				ᴁ = _065A("ACA899");
			}
			if (!ᴋ)
			{
				if (ᵇ)
				{
					ᴊ = _065A("ACA899");
				}
				else
				{
					ᴊ = Color.Empty;
				}
			}
			if (!ᴍ)
			{
				ᴌ = Color.Empty;
			}
			if (!ᴏ)
			{
				ᴎ = Color.Empty;
			}
			if (!ᴑ)
			{
				ᴐ = _065A("F4F2E8");
			}
			if (!ᴓ)
			{
				ᴒ = Color.Empty;
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = ᴐ;
			}
			if (!ᴆ)
			{
				ᴅ = Color.White;
			}
			if (!ᴉ)
			{
				ᴈ = ᴁ;
			}
			if (!ᴙ)
			{
				ᴘ = Color.Empty;
			}
			if (!ᴛ)
			{
				ᴚ = Color.Empty;
			}
			if (!ᴝ)
			{
				ᴜ = Color.Empty;
			}
			if (!ᴟ)
			{
				ᴞ = Color.Empty;
			}
			if (!ᴡ)
			{
				ᴠ = Color.Empty;
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = _065A("ACA899");
			}
			if (!ᴩ)
			{
				ᴨ = Color.Empty;
			}
			if (!ᴫ)
			{
				ᴪ = Color.Empty;
			}
			if (!ᴭ)
			{
				ᴬ = Color.White;
			}
			if (!ᴯ)
			{
				ᴮ = Color.Empty;
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		case _0883._0887:
			if (!_1CFD)
			{
				_1CFC = _065A("E7E7EF");
			}
			if (!_1CFF)
			{
				_1CFE = Color.Empty;
			}
			if (!ᴂ)
			{
				ᴁ = _065A("9D9DA1");
			}
			if (!ᴋ)
			{
				if (ᵇ)
				{
					ᴊ = ᴁ;
				}
				else
				{
					ᴊ = Color.Empty;
				}
			}
			if (!ᴍ)
			{
				ᴌ = Color.Empty;
			}
			if (!ᴏ)
			{
				ᴎ = Color.Empty;
			}
			if (!ᴑ)
			{
				ᴐ = _065A("E7E7EF");
			}
			if (!ᴓ)
			{
				ᴒ = Color.Empty;
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = ᴐ;
			}
			if (!ᴆ)
			{
				ᴅ = Color.White;
			}
			if (!ᴉ)
			{
				ᴈ = ᴊ;
			}
			if (!ᴙ)
			{
				ᴘ = Color.Empty;
			}
			if (!ᴛ)
			{
				ᴚ = Color.Empty;
			}
			if (!ᴝ)
			{
				ᴜ = Color.Empty;
			}
			if (!ᴟ)
			{
				ᴞ = Color.Empty;
			}
			if (!ᴡ)
			{
				ᴠ = Color.Empty;
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = ᴁ;
			}
			if (!ᴩ)
			{
				ᴨ = Color.Empty;
			}
			if (!ᴫ)
			{
				ᴪ = Color.Empty;
			}
			if (!ᴭ)
			{
				ᴬ = Color.White;
			}
			if (!ᴯ)
			{
				ᴮ = Color.Empty;
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		case _0883._0886:
			if (!_1CFD)
			{
				_1CFC = _065A("F4F2E9");
			}
			if (!_1CFF)
			{
				_1CFE = Color.Empty;
			}
			if (!ᴂ)
			{
				ᴁ = _065A("ACA899");
			}
			if (!ᴋ)
			{
				if (ᵇ)
				{
					ᴊ = ᴁ;
				}
				else
				{
					ᴊ = Color.Empty;
				}
			}
			if (!ᴍ)
			{
				ᴌ = Color.Empty;
			}
			if (!ᴏ)
			{
				ᴎ = Color.Empty;
			}
			if (!ᴑ)
			{
				ᴐ = _065A("F4F2E9");
			}
			if (!ᴓ)
			{
				ᴒ = Color.Empty;
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = ᴐ;
			}
			if (!ᴆ)
			{
				ᴅ = Color.White;
			}
			if (!ᴉ)
			{
				ᴈ = ᴁ;
			}
			if (!ᴙ)
			{
				ᴘ = Color.Empty;
			}
			if (!ᴛ)
			{
				ᴚ = Color.Empty;
			}
			if (!ᴝ)
			{
				ᴜ = Color.Empty;
			}
			if (!ᴟ)
			{
				ᴞ = Color.Empty;
			}
			if (!ᴡ)
			{
				ᴠ = Color.Empty;
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = _065A("ACA899");
			}
			if (!ᴩ)
			{
				ᴨ = Color.Empty;
			}
			if (!ᴫ)
			{
				ᴪ = Color.Empty;
			}
			if (!ᴭ)
			{
				ᴬ = Color.White;
			}
			if (!ᴯ)
			{
				ᴮ = Color.Empty;
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		default:
			if (!_1CFD)
			{
				_1CFC = colorScheme.BarBackground;
			}
			if (!_1CFF)
			{
				_1CFE = Color.Empty;
			}
			if (!ᴂ)
			{
				ᴁ = colorScheme.BarBackground2;
			}
			if (!ᴋ)
			{
				if (ᵇ)
				{
					ᴊ = ᴁ;
				}
				else
				{
					ᴊ = Color.Empty;
				}
			}
			if (!ᴍ)
			{
				ᴌ = Color.Empty;
			}
			if (!ᴏ)
			{
				ᴎ = Color.Empty;
			}
			if (!ᴑ)
			{
				ᴐ = _1CFC;
			}
			if (!ᴓ)
			{
				ᴒ = Color.Empty;
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = ᴐ;
			}
			if (!ᴆ)
			{
				ᴅ = ControlPaint.LightLight(ᴐ);
			}
			if (!ᴉ)
			{
				ᴈ = Color.Empty;
			}
			if (!ᴙ)
			{
				ᴘ = Color.Empty;
			}
			if (!ᴛ)
			{
				ᴚ = Color.Empty;
			}
			if (!ᴝ)
			{
				ᴜ = Color.Empty;
			}
			if (!ᴟ)
			{
				ᴞ = Color.Empty;
			}
			if (!ᴡ)
			{
				ᴠ = Color.Empty;
			}
			if (!ᴥ)
			{
				ᴤ = Color.Empty;
			}
			if (!ᴧ)
			{
				ᴦ = colorScheme.BarFloatingBorder;
			}
			if (!ᴩ)
			{
				ᴨ = Color.Empty;
			}
			if (!ᴫ)
			{
				ᴪ = Color.Empty;
			}
			if (!ᴭ)
			{
				ᴬ = ControlPaint.LightLight(colorScheme.BarBackground);
			}
			if (!ᴯ)
			{
				ᴮ = Color.Empty;
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		}
		if (!ᴵ)
		{
			ᴴ = ᴦ;
		}
		if (!ᴷ)
		{
			ᴶ = ᴨ;
		}
	}

	private void ᵈ()
	{
		_0883 obj = BarFunctions._0945;
		if (!ᴵ)
		{
			ᴴ = Color.Empty;
		}
		if (!ᴷ)
		{
			ᴶ = Color.Empty;
		}
		ColorScheme colorScheme = new ColorScheme(eDotNetBarStyle.VS2005);
		switch (obj)
		{
		case _0883._0885:
			if (!_1CFD)
			{
				_1CFC = _065A("F9F8F4");
			}
			if (!_1CFF)
			{
				_1CFE = Color.Empty;
			}
			if (!ᴂ)
			{
				ᴁ = SystemColors.ControlDarkDark;
			}
			if (!ᴋ)
			{
				ᴊ = SystemColors.ControlDarkDark;
			}
			if (!ᴍ)
			{
				ᴌ = Color.Empty;
			}
			if (!ᴏ)
			{
				ᴎ = Color.Empty;
			}
			if (!ᴑ)
			{
				ᴐ = _065A("ECE9D7");
			}
			if (!ᴓ)
			{
				ᴒ = _065A("FEFDFD");
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = Color.White;
			}
			if (!ᴆ)
			{
				ᴅ = ᴐ;
			}
			if (!ᴉ)
			{
				ᴈ = ᴁ;
			}
			if (!ᴙ)
			{
				ᴘ = Color.Empty;
			}
			if (!ᴛ)
			{
				ᴚ = Color.Empty;
			}
			if (!ᴝ)
			{
				ᴜ = Color.Empty;
			}
			if (!ᴟ)
			{
				ᴞ = Color.Empty;
			}
			if (!ᴡ)
			{
				ᴠ = Color.Empty;
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = ᴊ;
			}
			if (!ᴩ)
			{
				ᴨ = Color.Empty;
			}
			if (!ᴫ)
			{
				ᴪ = Color.Empty;
			}
			if (!ᴭ)
			{
				ᴬ = _065A("0266FB");
			}
			if (!ᴯ)
			{
				ᴮ = Color.FromArgb(45, ᴬ);
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		case _0883._0887:
			if (!_1CFD)
			{
				_1CFC = SystemColors.Control;
			}
			if (!_1CFF)
			{
				_1CFE = Color.Empty;
			}
			if (!ᴂ)
			{
				ᴁ = SystemColors.ControlDarkDark;
			}
			if (!ᴋ)
			{
				ᴊ = SystemColors.ControlDarkDark;
			}
			if (!ᴍ)
			{
				ᴌ = Color.Empty;
			}
			if (!ᴏ)
			{
				ᴎ = Color.Empty;
			}
			if (!ᴑ)
			{
				ᴐ = _065A("F2F2F7");
			}
			if (!ᴓ)
			{
				ᴒ = _065A("FEFEFE");
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = Color.White;
			}
			if (!ᴆ)
			{
				ᴅ = ᴐ;
			}
			if (!ᴉ)
			{
				ᴈ = ᴊ;
			}
			if (!ᴙ)
			{
				ᴘ = Color.Empty;
			}
			if (!ᴛ)
			{
				ᴚ = Color.Empty;
			}
			if (!ᴝ)
			{
				ᴜ = Color.Empty;
			}
			if (!ᴟ)
			{
				ᴞ = Color.Empty;
			}
			if (!ᴡ)
			{
				ᴠ = Color.Empty;
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = ᴊ;
			}
			if (!ᴩ)
			{
				ᴨ = Color.Empty;
			}
			if (!ᴫ)
			{
				ᴪ = Color.Empty;
			}
			if (!ᴭ)
			{
				ᴬ = SystemColors.Highlight;
			}
			if (!ᴯ)
			{
				ᴮ = Color.FromArgb(45, ᴬ);
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		case _0883._0886:
			if (!_1CFD)
			{
				_1CFC = SystemColors.Control;
			}
			if (!_1CFF)
			{
				_1CFE = Color.Empty;
			}
			if (!ᴂ)
			{
				ᴁ = SystemColors.ControlDarkDark;
			}
			if (!ᴋ)
			{
				ᴊ = SystemColors.ControlDarkDark;
			}
			if (!ᴍ)
			{
				ᴌ = Color.Empty;
			}
			if (!ᴏ)
			{
				ᴎ = Color.Empty;
			}
			if (!ᴑ)
			{
				ᴐ = _065A("ECE9D7");
			}
			if (!ᴓ)
			{
				ᴒ = _065A("FEFDFD");
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = Color.White;
			}
			if (!ᴆ)
			{
				ᴅ = ᴐ;
			}
			if (!ᴉ)
			{
				ᴈ = ᴁ;
			}
			if (!ᴙ)
			{
				ᴘ = Color.Empty;
			}
			if (!ᴛ)
			{
				ᴚ = Color.Empty;
			}
			if (!ᴝ)
			{
				ᴜ = Color.Empty;
			}
			if (!ᴟ)
			{
				ᴞ = Color.Empty;
			}
			if (!ᴡ)
			{
				ᴠ = Color.Empty;
			}
			if (!ᴥ)
			{
				ᴤ = SystemColors.ControlText;
			}
			if (!ᴧ)
			{
				ᴦ = _065A("ACA899");
			}
			if (!ᴩ)
			{
				ᴨ = Color.Empty;
			}
			if (!ᴫ)
			{
				ᴪ = Color.Empty;
			}
			if (!ᴭ)
			{
				ᴬ = SystemColors.Highlight;
			}
			if (!ᴯ)
			{
				ᴮ = Color.FromArgb(45, ᴬ);
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		default:
			if (!_1CFD)
			{
				_1CFC = SystemColors.Control;
			}
			if (!_1CFF)
			{
				_1CFE = Color.Empty;
			}
			if (!ᴂ)
			{
				ᴁ = SystemColors.ControlDarkDark;
			}
			if (!ᴋ)
			{
				ᴊ = SystemColors.ControlDarkDark;
			}
			if (!ᴍ)
			{
				ᴌ = Color.Empty;
			}
			if (!ᴏ)
			{
				ᴎ = Color.Empty;
			}
			if (!ᴑ)
			{
				ᴐ = ControlPaint.Light(colorScheme.BarBackground2);
			}
			if (!ᴓ)
			{
				ᴒ = ControlPaint.Light(colorScheme.BarBackground);
			}
			if (!ᴗ)
			{
				ᴖ = SystemColors.ControlText;
			}
			if (!ᴄ)
			{
				ᴃ = ControlPaint.LightLight(ᴐ);
			}
			if (!ᴆ)
			{
				ᴅ = ᴐ;
			}
			if (!ᴉ)
			{
				ᴈ = ᴁ;
			}
			if (!ᴙ)
			{
				ᴘ = Color.Empty;
			}
			if (!ᴛ)
			{
				ᴚ = Color.Empty;
			}
			if (!ᴝ)
			{
				ᴜ = Color.Empty;
			}
			if (!ᴟ)
			{
				ᴞ = Color.Empty;
			}
			if (!ᴡ)
			{
				ᴠ = Color.Empty;
			}
			if (!ᴥ)
			{
				ᴤ = Color.Empty;
			}
			if (!ᴧ)
			{
				ᴦ = ᴊ;
			}
			if (!ᴩ)
			{
				ᴨ = Color.Empty;
			}
			if (!ᴫ)
			{
				ᴪ = Color.Empty;
			}
			if (!ᴭ)
			{
				ᴬ = SystemColors.Highlight;
			}
			if (!ᴯ)
			{
				ᴮ = Color.FromArgb(45, ᴬ);
			}
			if (!ᴳ)
			{
				ᴲ = SystemColors.ControlText;
			}
			break;
		}
	}

	private Color _065A(string ᵉ)
	{
		if (ᵉ == "" || ᵉ == null)
		{
			return Color.Empty;
		}
		return Color.FromArgb(Convert.ToInt32(ᵉ.Substring(0, 2), 16), Convert.ToInt32(ᵉ.Substring(2, 2), 16), Convert.ToInt32(ᵉ.Substring(4, 2), 16));
	}

	private void ᵊ()
	{
		Office2007TabColorTable office2007TabColorTable = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			office2007TabColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.TabControl;
		}
		if (office2007TabColorTable == null)
		{
			ᵅ();
			return;
		}
		_ᴻ = office2007TabColorTable.TabBackgroundImage;
		if (!ᴵ)
		{
			ᴴ = Color.Empty;
		}
		if (!ᴷ)
		{
			ᴶ = Color.Empty;
		}
		if (!_1CFD)
		{
			_1CFC = office2007TabColorTable.TabBackground.Start;
		}
		if (!_1CFF)
		{
			_1CFE = office2007TabColorTable.TabBackground.End;
		}
		if (!ᴂ)
		{
			ᴁ = Color.Empty;
		}
		if (!ᴋ)
		{
			ᴊ = office2007TabColorTable.Default.OuterBorder;
		}
		if (!ᴍ)
		{
			ᴌ = office2007TabColorTable.Default.InnerBorder;
		}
		if (!ᴏ)
		{
			ᴎ = Color.Empty;
		}
		if (!ᴑ)
		{
			ᴐ = office2007TabColorTable.Default.TopBackground.Start;
		}
		if (!ᴓ)
		{
			ᴒ = office2007TabColorTable.Default.BottomBackground.End;
		}
		if (!ᴑ)
		{
			ᴕ.Clear();
			ᴕ.Add(new BackgroundColorBlend(office2007TabColorTable.Default.TopBackground.Start, 0f));
			ᴕ.Add(new BackgroundColorBlend(office2007TabColorTable.Default.TopBackground.End, 0.45f));
			ᴕ.Add(new BackgroundColorBlend(office2007TabColorTable.Default.BottomBackground.Start, 0.45f));
			ᴕ.Add(new BackgroundColorBlend(office2007TabColorTable.Default.BottomBackground.End, 1f));
		}
		if (!ᴗ)
		{
			ᴖ = office2007TabColorTable.Default.Text;
		}
		if (!ᴄ)
		{
			ᴃ = office2007TabColorTable.TabPanelBackground.Start;
		}
		if (!ᴆ)
		{
			ᴅ = office2007TabColorTable.TabPanelBackground.End;
		}
		if (!ᴉ)
		{
			ᴈ = office2007TabColorTable.TabPanelBorder;
		}
		if (!ᴙ)
		{
			ᴘ = office2007TabColorTable.MouseOver.OuterBorder;
		}
		if (!ᴛ)
		{
			ᴚ = office2007TabColorTable.MouseOver.InnerBorder;
		}
		if (!ᴝ)
		{
			ᴜ = Color.Empty;
		}
		if (!ᴟ)
		{
			ᴞ = office2007TabColorTable.MouseOver.TopBackground.Start;
		}
		if (!ᴡ)
		{
			ᴠ = office2007TabColorTable.MouseOver.BottomBackground.End;
		}
		if (!ᴟ)
		{
			ᴣ.Clear();
			ᴣ.Add(new BackgroundColorBlend(office2007TabColorTable.MouseOver.TopBackground.Start, 0f));
			ᴣ.Add(new BackgroundColorBlend(office2007TabColorTable.MouseOver.TopBackground.End, 0.45f));
			ᴣ.Add(new BackgroundColorBlend(office2007TabColorTable.MouseOver.BottomBackground.Start, 0.45f));
			ᴣ.Add(new BackgroundColorBlend(office2007TabColorTable.MouseOver.BottomBackground.End, 1f));
		}
		if (!ᴥ)
		{
			ᴤ = office2007TabColorTable.MouseOver.Text;
		}
		if (!ᴧ)
		{
			ᴦ = office2007TabColorTable.Selected.OuterBorder;
		}
		if (!ᴩ)
		{
			ᴨ = office2007TabColorTable.Selected.InnerBorder;
		}
		if (!ᴫ)
		{
			ᴪ = Color.Empty;
		}
		if (!ᴭ)
		{
			ᴬ = office2007TabColorTable.Selected.TopBackground.Start;
		}
		if (!ᴯ)
		{
			ᴮ = office2007TabColorTable.Selected.BottomBackground.End;
		}
		if (!ᴭ)
		{
			ᴱ.Clear();
			ᴱ.Add(new BackgroundColorBlend(office2007TabColorTable.Selected.TopBackground.Start, 0f));
			ᴱ.Add(new BackgroundColorBlend(office2007TabColorTable.Selected.TopBackground.End, 0.45f));
			ᴱ.Add(new BackgroundColorBlend(office2007TabColorTable.Selected.BottomBackground.Start, 0.45f));
			ᴱ.Add(new BackgroundColorBlend(office2007TabColorTable.Selected.BottomBackground.End, 1f));
		}
		if (!ᴳ)
		{
			ᴲ = office2007TabColorTable.Selected.Text;
		}
		if (!ᴄ)
		{
			ᴃ = office2007TabColorTable.TabPanelBackground.Start;
		}
		if (!ᴆ)
		{
			ᴅ = office2007TabColorTable.TabPanelBackground.End;
		}
		if (!ᴉ)
		{
			ᴈ = office2007TabColorTable.TabPanelBorder;
		}
	}

	protected virtual void InvokeColorChanged()
	{
		if (ᗆ != null)
		{
			ᗆ(this, new EventArgs());
		}
	}

	public void ResetChangedFlag()
	{
		_1CFD = false;
		_1CFF = false;
		ᴂ = false;
		ᴑ = false;
		ᴓ = false;
		ᴋ = false;
		ᴏ = false;
		ᴍ = false;
		ᴗ = false;
		ᴟ = false;
		ᴡ = false;
		ᴙ = false;
		ᴝ = false;
		ᴛ = false;
		ᴥ = false;
		ᴭ = false;
		ᴯ = false;
		ᴧ = false;
		ᴫ = false;
		ᴩ = false;
		ᴳ = false;
		ᴵ = false;
		ᴷ = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabBackground()
	{
		return _1CFD;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabBackground2()
	{
		return _1CFF;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabBorder()
	{
		return ᴂ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabPanelBackground()
	{
		return ᴄ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabPanelBackground2()
	{
		return ᴆ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabPanelBorder()
	{
		return ᴉ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBorder()
	{
		return ᴋ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBorderLight()
	{
		return ᴍ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBorderDark()
	{
		return ᴏ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBackground()
	{
		return ᴑ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemBackground2()
	{
		return ᴓ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemText()
	{
		return ᴗ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBorder()
	{
		return ᴙ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBorderLight()
	{
		return ᴛ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBorderDark()
	{
		return ᴝ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBackground()
	{
		return ᴟ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotBackground2()
	{
		return ᴡ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemHotText()
	{
		return ᴥ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBorder()
	{
		return ᴧ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBorderLight()
	{
		return ᴩ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBorderDark()
	{
		return ᴫ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBackground()
	{
		return ᴭ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedBackground2()
	{
		return ᴯ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSelectedText()
	{
		return ᴳ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSeparator()
	{
		return ᴵ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabItemSeparatorShade()
	{
		return ᴷ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNavigationBoxForeColor()
	{
		return ᴽ;
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
