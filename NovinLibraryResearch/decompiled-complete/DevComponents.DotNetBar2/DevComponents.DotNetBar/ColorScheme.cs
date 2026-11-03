using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar;

public class ColorScheme
{
	private Color ߗ;

	private bool ߘ;

	private Color ߙ = Color.Empty;

	private bool ߚ;

	private int ߛ = 90;

	private Color ߜ;

	private bool ߝ;

	private Color ߞ = Color.Empty;

	private bool ߟ;

	private int ߠ = 90;

	private Color ߡ;

	private bool ߢ;

	private Color ߣ = Color.Empty;

	private bool ߤ;

	private int ߥ = 90;

	private Color ߦ;

	private bool ߧ;

	private Color ߨ;

	private bool ߩ;

	private Color ߪ = Color.Empty;

	private bool _07EB;

	private int _07EC = 90;

	private Color _07ED;

	private bool _07EE;

	private Color _07EF;

	private bool _07F0;

	private Color _07F1;

	private bool _07F2;

	private Color _07F3;

	private bool ߴ;

	private Color ߵ;

	private bool _07F6;

	private Color _07F7;

	private bool _07F8;

	private Color _07F9;

	private bool ߺ;

	private Color _07FB;

	private bool _07FC;

	private int _07FD = 90;

	private Color _07FE;

	private bool _07FF;

	private Color ࠀ;

	private bool ࠁ;

	private Color ࠂ;

	private bool ࠃ;

	private Color ࠄ;

	private bool ࠅ;

	private Color ࠆ = Color.Empty;

	private bool ࠇ;

	private int ࠈ = 90;

	private Color ࠉ;

	private bool ࠊ;

	private Color ࠋ;

	private bool ࠌ;

	private Color ࠍ;

	private bool ࠎ;

	private Color ࠏ = Color.Empty;

	private bool ࠐ;

	private int ࠑ = 90;

	private Color ࠒ;

	private bool ࠓ;

	private Color ࠔ;

	private bool ࠕ;

	private Color _0816;

	private bool _0817;

	private Color _0818 = Color.Empty;

	private bool _0819;

	private Color ࠚ;

	private bool _081B;

	private Color _081C = Color.Empty;

	private bool _081D;

	private int _081E = 90;

	private Color _081F;

	private bool _0820;

	private Color _0821;

	private bool _0822;

	private Color _0823;

	private bool ࠤ;

	private Color _0825;

	private bool _0826;

	private int _0827 = 90;

	private Color ࠨ = Color.Empty;

	private bool _0829;

	private Color _082A;

	private bool _082B;

	private Color _082C;

	private bool _082D;

	private Color _082E;

	private bool _082F;

	private Color _0830;

	private bool _0831;

	private Color _0832 = Color.Empty;

	private bool _0833;

	private int _0834;

	private Color _0835;

	private bool _0836;

	private Color _0837 = Color.Empty;

	private bool _0838;

	private int _0839;

	private Color _083A;

	private bool _083B;

	private Color _083C;

	private bool _083D;

	private Color _083E = Color.Empty;

	private bool _083F;

	private int ࡀ;

	private Color ࡁ;

	private bool ࡂ;

	private Color ࡃ = Color.Empty;

	private bool ࡄ;

	private Color ࡅ = Color.Empty;

	private bool ࡆ;

	private int ࡇ = 90;

	private Color ࡈ = Color.Empty;

	private bool ࡉ;

	private Color ࡊ = Color.Empty;

	private bool ࡋ;

	private Color ࡌ = Color.Empty;

	private bool ࡍ;

	private int ࡎ = 90;

	private Color ࡏ = Color.Empty;

	private bool ࡐ;

	private Color ࡑ = Color.Empty;

	private bool ࡒ;

	private Color ࡓ = Color.Empty;

	private bool ࡔ;

	private Color ࡕ = Color.Empty;

	private bool ࡖ;

	private int ࡗ = 90;

	private Color ࡘ = Color.Empty;

	private bool _0859;

	private Color _085A = Color.Empty;

	private bool _085B;

	private Color _085C = Color.Empty;

	private bool _085D;

	private Color _085E = Color.Empty;

	private bool _085F;

	private int ࡠ = 90;

	internal bool _ࡡ;

	private eDotNetBarStyle Ӷ;

	private Color ࡢ = Color.Empty;

	private bool ࡣ;

	private Color ࡤ = Color.Empty;

	private bool ࡥ;

	private int ࡦ;

	private ePredefinedColorScheme ࡧ;

	private Color ࡨ = Color.Empty;

	private Image _ࡩ;

	private Color _ࡪ = Color.Empty;

	private Color __086B = Color.Empty;

	private Color __086C = Color.Empty;

	private Color __086D = Color.Empty;

	private int __086E = 90;

	private Color __086F = Color.Empty;

	private Color __0870 = Color.Empty;

	private Color __0871 = Color.Empty;

	private Color __0872 = Color.Empty;

	private Color __0873 = Color.Empty;

	private Color __0874 = Color.Empty;

	internal eDotNetBarStyle ߕ
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

	[Browsable(true)]
	[Description("Specifies starting color of dock site.")]
	[Category("Panel Colors")]
	[DevCoBrowsable(true)]
	public Color DockSiteBackColor
	{
		get
		{
			return ࡢ;
		}
		set
		{
			if (ࡢ != value)
			{
				ࡢ = value;
				ࡣ = true;
			}
		}
	}

	[Category("Panel Colors")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies ending gradient color of dock site.")]
	public Color DockSiteBackColor2
	{
		get
		{
			return ࡤ;
		}
		set
		{
			if (ࡤ != value)
			{
				ࡤ = value;
				ࡥ = true;
			}
		}
	}

	[Description("Specifies the gradient angle.")]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Browsable(true)]
	public int DockSiteBackColorGradientAngle
	{
		get
		{
			return ࡦ;
		}
		set
		{
			ࡦ = value;
		}
	}

	[Description("Specifies the menu bar background color.")]
	[Category("Bar Colors")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color MenuBarBackground
	{
		get
		{
			return ߗ;
		}
		set
		{
			if (ߗ != value)
			{
				ߗ = value;
				ߘ = true;
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the target menu bar gradient background color.")]
	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	public Color MenuBarBackground2
	{
		get
		{
			return ߙ;
		}
		set
		{
			if (ߙ != value)
			{
				ߙ = value;
				ߚ = true;
			}
		}
	}

	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	[Category("Bar Colors")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public int MenuBarBackgroundGradientAngle
	{
		get
		{
			return ߛ;
		}
		set
		{
			ߛ = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Bar Colors")]
	[Browsable(true)]
	[Description("Specifies the background color for the bar when floating or when docked.")]
	public Color BarBackground
	{
		get
		{
			return ߜ;
		}
		set
		{
			if (ߜ != value)
			{
				ߜ = value;
				ߝ = true;
			}
		}
	}

	[Description("Specifies the target gradient background color for the bar when floating or when docked.")]
	[Category("Bar Colors")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color BarBackground2
	{
		get
		{
			return ߞ;
		}
		set
		{
			if (ߞ != value)
			{
				ߞ = value;
				ߟ = true;
			}
		}
	}

	[Description("Specifies the gradient angle.")]
	[Category("Bar Colors")]
	[DevCoBrowsable(true)]
	[DefaultValue(90)]
	[Browsable(true)]
	public int BarBackgroundGradientAngle
	{
		get
		{
			return ߠ;
		}
		set
		{
			ߠ = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the background color for the bar Caption.")]
	[Category("Bar Colors")]
	public Color BarCaptionBackground
	{
		get
		{
			return ߡ;
		}
		set
		{
			if (ߡ != value)
			{
				ߡ = value;
				ߢ = true;
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
			return ߣ;
		}
		set
		{
			if (ߣ != value)
			{
				ߣ = value;
				ߤ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	[Browsable(true)]
	[Category("Bar Colors")]
	public int BarCaptionBackgroundGradientAngle
	{
		get
		{
			return ߥ;
		}
		set
		{
			ߥ = value;
		}
	}

	[Category("Bar Colors")]
	[Description("Specifies the color for text of the Caption.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color BarCaptionText
	{
		get
		{
			return ߦ;
		}
		set
		{
			if (ߦ != value)
			{
				ߦ = value;
				ߧ = true;
			}
		}
	}

	[Category("Bar Colors")]
	[DevCoBrowsable(true)]
	[Description("Specifies the Bar Caption inactive (lost focus) background color.")]
	[Browsable(true)]
	public Color BarCaptionInactiveBackground
	{
		get
		{
			return ߨ;
		}
		set
		{
			if (ߨ != value)
			{
				ߨ = value;
				ߩ = true;
			}
		}
	}

	[Category("Bar Colors")]
	[Browsable(true)]
	[Description("Specifies the target background gradient Bar Caption inactive (lost focus) color.")]
	[DevCoBrowsable(true)]
	public Color BarCaptionInactiveBackground2
	{
		get
		{
			return ߪ;
		}
		set
		{
			if (ߪ != value)
			{
				ߪ = value;
				_07EB = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(90)]
	[Browsable(true)]
	[Description("Specifies the gradient angle.")]
	[Category("Bar Colors")]
	internal int ߖ
	{
		get
		{
			return _07EC;
		}
		set
		{
			_07EC = value;
		}
	}

	[Description("Specifies the Bar inactive (lost focus) text color.")]
	[Category("Bar Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color BarCaptionInactiveText
	{
		get
		{
			return _07ED;
		}
		set
		{
			if (_07ED != value)
			{
				_07ED = value;
				_07EE = true;
			}
		}
	}

	[Category("Bar Colors")]
	[Description("Specifies the background color for popup bars.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color BarPopupBackground
	{
		get
		{
			return _07EF;
		}
		set
		{
			if (_07EF != value)
			{
				_07EF = value;
				_07F0 = true;
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the border color for popup bars.")]
	[Category("Bar Colors")]
	[DevCoBrowsable(true)]
	public Color BarPopupBorder
	{
		get
		{
			return _07F1;
		}
		set
		{
			if (_07F1 != value)
			{
				_07F1 = value;
				_07F2 = true;
			}
		}
	}

	[Category("Bar Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Specifies the border color for docked bars.")]
	public Color BarDockedBorder
	{
		get
		{
			return _07F3;
		}
		set
		{
			if (_07F3 != value)
			{
				_07F3 = value;
				ߴ = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the color of the grab handle stripes.")]
	[Category("Bar Colors")]
	public Color BarStripeColor
	{
		get
		{
			return ߵ;
		}
		set
		{
			if (ߵ != value)
			{
				ߵ = value;
				_07F6 = true;
			}
		}
	}

	[Category("Bar Colors")]
	[Description("Specifies the border color for floating bars.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color BarFloatingBorder
	{
		get
		{
			return _07F7;
		}
		set
		{
			if (_07F7 != value)
			{
				_07F7 = value;
				_07F8 = true;
			}
		}
	}

	[Category("Item Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Specifies the item background color.")]
	public Color ItemBackground
	{
		get
		{
			return _07F9;
		}
		set
		{
			if (_07F9 != value)
			{
				_07F9 = value;
				ߺ = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the target item background gradient color.")]
	[Category("Item Colors")]
	public Color ItemBackground2
	{
		get
		{
			return _07FB;
		}
		set
		{
			if (_07FB != value)
			{
				_07FB = value;
				_07FC = true;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(90)]
	[DevCoBrowsable(true)]
	[Description("Specifies the gradient angle.")]
	[Category("Item Colors")]
	public int ItemBackgroundGradientAngle
	{
		get
		{
			return _07FD;
		}
		set
		{
			_07FD = value;
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the item text color.")]
	[Category("Item Colors")]
	[Browsable(true)]
	public Color ItemText
	{
		get
		{
			return _07FE;
		}
		set
		{
			if (_07FE != value)
			{
				_07FE = value;
				_07FF = true;
			}
		}
	}

	[Browsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the background color for the item that is disabled.")]
	[DevCoBrowsable(true)]
	public Color ItemDisabledBackground
	{
		get
		{
			return ࠀ;
		}
		set
		{
			if (ࠀ != value)
			{
				ࠀ = value;
				ࠁ = true;
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the text color for the item that is disabled.")]
	[Category("Item Colors")]
	[DevCoBrowsable(true)]
	public Color ItemDisabledText
	{
		get
		{
			return ࠂ;
		}
		set
		{
			if (ࠂ != value)
			{
				ࠂ = value;
				ࠃ = true;
			}
		}
	}

	[Category("Item Colors")]
	[Description("Specifies the background color when mouse is over the item.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color ItemHotBackground
	{
		get
		{
			return ࠄ;
		}
		set
		{
			if (ࠄ != value)
			{
				ࠄ = value;
				ࠅ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the target gradient background color when mouse is over the item.")]
	public Color ItemHotBackground2
	{
		get
		{
			return ࠆ;
		}
		set
		{
			if (ࠆ != value)
			{
				ࠆ = value;
				ࠇ = true;
			}
		}
	}

	[Browsable(true)]
	[Category("Item Colors")]
	[DefaultValue(90)]
	[Description("Specifies the gradient angle.")]
	[DevCoBrowsable(true)]
	public int ItemHotBackgroundGradientAngle
	{
		get
		{
			return ࠈ;
		}
		set
		{
			ࠈ = value;
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
			return ࠉ;
		}
		set
		{
			if (ࠉ != value)
			{
				ࠉ = value;
				ࠊ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Browsable(true)]
	[Description("Specifies the border color when mouse is over the item.")]
	public Color ItemHotBorder
	{
		get
		{
			return ࠋ;
		}
		set
		{
			if (ࠋ != value)
			{
				ࠋ = value;
				ࠌ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the background color when item is pressed.")]
	[Category("Item Colors")]
	[Browsable(true)]
	public Color ItemPressedBackground
	{
		get
		{
			return ࠍ;
		}
		set
		{
			if (ࠍ != value)
			{
				ࠍ = value;
				ࠎ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the target gradient background color when item is pressed.")]
	[Category("Item Colors")]
	[Browsable(true)]
	public Color ItemPressedBackground2
	{
		get
		{
			return ࠏ;
		}
		set
		{
			if (ࠏ != value)
			{
				ࠏ = value;
				ࠐ = true;
			}
		}
	}

	[Category("Item Colors")]
	[Description("Specifies the gradient angle.")]
	[DefaultValue(90)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public int ItemPressedBackgroundGradientAngle
	{
		get
		{
			return ࠑ;
		}
		set
		{
			ࠑ = value;
		}
	}

	[Category("Item Colors")]
	[Description("Specifies the text color when item is pressed.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color ItemPressedText
	{
		get
		{
			return ࠒ;
		}
		set
		{
			if (ࠒ != value)
			{
				ࠒ = value;
				ࠓ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the border color when item is pressed.")]
	public Color ItemPressedBorder
	{
		get
		{
			return ࠔ;
		}
		set
		{
			if (ࠔ != value)
			{
				ࠔ = value;
				ࠕ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the color for the item group separator.")]
	[Category("Item Colors")]
	[Browsable(true)]
	public Color ItemSeparator
	{
		get
		{
			return _0816;
		}
		set
		{
			if (_0816 != value)
			{
				_0816 = value;
				_0817 = true;
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the color for the item group separator shade.")]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	public Color ItemSeparatorShade
	{
		get
		{
			return _0818;
		}
		set
		{
			if (_0818 != value)
			{
				_0818 = value;
				_0819 = true;
			}
		}
	}

	[Category("Item Colors")]
	[Description("Specifies the background color for the shadow of expanded item.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color ItemExpandedShadow
	{
		get
		{
			return _0821;
		}
		set
		{
			if (_0821 != value)
			{
				_0821 = value;
				_0822 = true;
			}
		}
	}

	[Description("Specifies the background color for the expanded item.")]
	[Category("Item Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color ItemExpandedBackground
	{
		get
		{
			return ࠚ;
		}
		set
		{
			if (ࠚ != value)
			{
				ࠚ = value;
				_081B = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the target gradient background color for the expanded item.")]
	[Browsable(true)]
	[Category("Item Colors")]
	public Color ItemExpandedBackground2
	{
		get
		{
			return _081C;
		}
		set
		{
			if (_081C != value)
			{
				_081C = value;
				_081D = true;
			}
		}
	}

	[DefaultValue(90)]
	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the gradient angle.")]
	[Browsable(true)]
	public int ItemExpandedBackgroundGradientAngle
	{
		get
		{
			return _081E;
		}
		set
		{
			_081E = value;
		}
	}

	[Description("Specifies the text color for the expanded item.")]
	[Category("Item Colors")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color ItemExpandedText
	{
		get
		{
			return _081F;
		}
		set
		{
			if (_081F != value)
			{
				_081F = value;
				_0820 = true;
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
			return _0823;
		}
		set
		{
			if (_0823 != value)
			{
				_0823 = value;
				ࠤ = true;
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
			return _0825;
		}
		set
		{
			if (_0825 != value)
			{
				_0825 = value;
				_0826 = true;
			}
		}
	}

	[Description("Specifies the target gradient background color for the checked item.")]
	[Category("Item Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color ItemCheckedBackground2
	{
		get
		{
			return ࠨ;
		}
		set
		{
			if (ࠨ != value)
			{
				ࠨ = value;
				_0829 = true;
			}
		}
	}

	[Category("Item Colors")]
	[Description("Specifies the gradient angle.")]
	[DevCoBrowsable(true)]
	[DefaultValue(90)]
	[Browsable(true)]
	public int ItemCheckedBackgroundGradientAngle
	{
		get
		{
			return _0827;
		}
		set
		{
			_0827 = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the border color for the checked item.")]
	[Category("Item Colors")]
	public Color ItemCheckedBorder
	{
		get
		{
			return _082A;
		}
		set
		{
			if (_082A != value)
			{
				_082A = value;
				_082B = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Item Colors")]
	[Description("Specifies the text color for the checked item.")]
	[Browsable(true)]
	public Color ItemCheckedText
	{
		get
		{
			return _082C;
		}
		set
		{
			if (_082C != value)
			{
				_082C = value;
				_082D = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Specifies the customize item background color.")]
	[Category("Special Item Colors")]
	public Color CustomizeBackground
	{
		get
		{
			return ࡃ;
		}
		set
		{
			if (ࡃ != value)
			{
				ࡃ = value;
				ࡄ = true;
			}
		}
	}

	[Category("Special Item Colors")]
	[Browsable(true)]
	[Description("Specifies the customize item target gradient background color.")]
	[DevCoBrowsable(true)]
	public Color CustomizeBackground2
	{
		get
		{
			return ࡅ;
		}
		set
		{
			if (ࡅ != value)
			{
				ࡅ = value;
				ࡆ = true;
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the customize item background color gradient angle.")]
	[DefaultValue(90)]
	[Category("Special Item Colors")]
	[DevCoBrowsable(true)]
	public int CustomizeBackgroundGradientAngle
	{
		get
		{
			return ࡇ;
		}
		set
		{
			ࡇ = value;
		}
	}

	[Category("Special Item Colors")]
	[Description("Specifies the customize item text color.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color CustomizeText
	{
		get
		{
			return ࡈ;
		}
		set
		{
			if (ࡈ != value)
			{
				ࡈ = value;
				ࡉ = true;
			}
		}
	}

	[Category("Menu Colors")]
	[Description("Specifies the color of the menu border.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color MenuBorder
	{
		get
		{
			return _082E;
		}
		set
		{
			if (_082E != value)
			{
				_082E = value;
				_082F = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Specifies the background color of the menu.")]
	[Category("Menu Colors")]
	public Color MenuBackground
	{
		get
		{
			return _0830;
		}
		set
		{
			if (_0830 != value)
			{
				_0830 = value;
				_0831 = true;
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the target gradient background color of the menu.")]
	[Category("Menu Colors")]
	[DevCoBrowsable(true)]
	public Color MenuBackground2
	{
		get
		{
			return _0832;
		}
		set
		{
			if (_0832 != value)
			{
				_0832 = value;
				_0833 = true;
			}
		}
	}

	[Category("Menu Colors")]
	[Browsable(true)]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Description("Specifies the angle of the gradient fill for the menu background.")]
	public int MenuBackgroundGradientAngle
	{
		get
		{
			return _0834;
		}
		set
		{
			if (_0834 != value)
			{
				_0834 = value;
			}
		}
	}

	[Description("Specifies the background color of the menu part (left side)  that is showing the images.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Menu Colors")]
	public Color MenuSide
	{
		get
		{
			return _0835;
		}
		set
		{
			if (_0835 != value)
			{
				_0835 = value;
				_0836 = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the target gradient background color of the menu part (left side)  that is showing the images.")]
	[Category("Menu Colors")]
	public Color MenuSide2
	{
		get
		{
			return _0837;
		}
		set
		{
			if (_0837 != value)
			{
				_0837 = value;
				_0838 = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Description("Specifies the angle of the gradient fill for the menu part (left side) that is showing the images.")]
	[Category("Menu Colors")]
	public int MenuSideGradientAngle
	{
		get
		{
			return _0839;
		}
		set
		{
			if (_0839 != value)
			{
				_0839 = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Specifies the background color for the items that were not recently used.")]
	[Category("Menu Colors")]
	[DevCoBrowsable(true)]
	public Color MenuUnusedBackground
	{
		get
		{
			return _083A;
		}
		set
		{
			if (_083A != value)
			{
				_083A = value;
				_083B = true;
			}
		}
	}

	[Category("Menu Colors")]
	[Description("Specifies the side bar color for the items that were not recently used.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color MenuUnusedSide
	{
		get
		{
			return _083C;
		}
		set
		{
			if (_083C != value)
			{
				_083C = value;
				_083D = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the target gradient side bar color for the items that were not recently used.")]
	[Category("Menu Colors")]
	public Color MenuUnusedSide2
	{
		get
		{
			return _083E;
		}
		set
		{
			if (_083E != value)
			{
				_083E = value;
				_083F = true;
			}
		}
	}

	[DefaultValue(0)]
	[Description("Specifies the angle of the gradient fill for the menu part (left side) that is showing the images.")]
	[Category("Menu Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int MenuUnusedSideGradientAngle
	{
		get
		{
			return ࡀ;
		}
		set
		{
			if (ࡀ != value)
			{
				ࡀ = value;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Design Colors")]
	[Browsable(true)]
	[Description("Specifies the border color for focused design-time item.")]
	public Color ItemDesignTimeBorder
	{
		get
		{
			return ࡁ;
		}
		set
		{
			if (ࡁ != value)
			{
				ࡁ = value;
				ࡂ = true;
			}
		}
	}

	[DefaultValue(ePredefinedColorScheme.AutoGenerated)]
	[DevCoBrowsable(true)]
	[Description("Indicates predefined color scheme. By default DotNetBar will automatically change and generate color scheme depending on system colors.")]
	[Browsable(true)]
	[Category("Color Scheme")]
	public ePredefinedColorScheme PredefinedColorScheme
	{
		get
		{
			return ࡧ;
		}
		set
		{
			if (ࡧ != value)
			{
				ࡧ = value;
				Refresh();
			}
		}
	}

	[Category("Panel Colors")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Specifies the background color of the panel.")]
	public Color PanelBackground
	{
		get
		{
			return ࡊ;
		}
		set
		{
			if (ࡊ != value)
			{
				ࡊ = value;
				ࡋ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Description("Specifies the target background gradient color of the panel.")]
	[Browsable(true)]
	public Color PanelBackground2
	{
		get
		{
			return ࡌ;
		}
		set
		{
			if (ࡌ != value)
			{
				ࡌ = value;
				ࡍ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Browsable(true)]
	[DefaultValue(90)]
	[Description("Specifies the angle of the gradient fill for the panel background.")]
	public int PanelBackgroundGradientAngle
	{
		get
		{
			return ࡎ;
		}
		set
		{
			if (ࡎ != value)
			{
				ࡎ = value;
			}
		}
	}

	[Category("Panel Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Specifies border color of the panel.")]
	public Color PanelBorder
	{
		get
		{
			return ࡑ;
		}
		set
		{
			if (ࡑ != value)
			{
				ࡑ = value;
				ࡒ = true;
			}
		}
	}

	[Description("Specifies color of the text on the panel.")]
	[DevCoBrowsable(true)]
	[Category("Panel Colors")]
	[Browsable(true)]
	public Color PanelText
	{
		get
		{
			return ࡏ;
		}
		set
		{
			if (ࡏ != value)
			{
				ࡏ = value;
				ࡐ = true;
			}
		}
	}

	[Category("Explorer Bar Colors")]
	[Description("Specifies the background color of the explorer bar.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color ExplorerBarBackground
	{
		get
		{
			return _085C;
		}
		set
		{
			if (_085C != value)
			{
				_085C = value;
				_085D = true;
			}
		}
	}

	[Description("Specifies target gradient background color of the explorer bar.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Explorer Bar Colors")]
	public Color ExplorerBarBackground2
	{
		get
		{
			return _085E;
		}
		set
		{
			if (_085E != value)
			{
				_085E = value;
				_085F = true;
			}
		}
	}

	[Category("Explorer Bar Colors")]
	[Description("Specifies the angle of the gradient fill for the explorer bar background.")]
	[DevCoBrowsable(true)]
	[DefaultValue(90)]
	[Browsable(true)]
	public int ExplorerBarBackgroundGradientAngle
	{
		get
		{
			return ࡠ;
		}
		set
		{
			if (ࡠ != value)
			{
				ࡠ = value;
			}
		}
	}

	[Description("Specifies the foreground color of MDI System Item buttons.")]
	[Category("Item Colors")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color MdiSystemItemForeground
	{
		get
		{
			return ࡨ;
		}
		set
		{
			ࡨ = value;
		}
	}

	[Category("Splitter Colors")]
	[Description("Specifies the background color of the Splitter.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color SplitterBackground
	{
		get
		{
			return ࡓ;
		}
		set
		{
			if (ࡓ != value)
			{
				ࡓ = value;
				ࡔ = true;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the target background gradient color of the Splitter.")]
	[Browsable(true)]
	[Category("Splitter Colors")]
	public Color SplitterBackground2
	{
		get
		{
			return ࡕ;
		}
		set
		{
			if (ࡕ != value)
			{
				ࡕ = value;
				ࡖ = true;
			}
		}
	}

	[DefaultValue(90)]
	[Description("Specifies the angle of the gradient fill for the Splitter background.")]
	[Browsable(true)]
	[Category("Splitter Colors")]
	[DevCoBrowsable(true)]
	public int SplitterBackgroundGradientAngle
	{
		get
		{
			return ࡗ;
		}
		set
		{
			if (ࡗ != value)
			{
				ࡗ = value;
			}
		}
	}

	[Description("Specifies border color of the Splitter.")]
	[Category("Splitter Colors")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color SplitterBorder
	{
		get
		{
			return _085A;
		}
		set
		{
			if (_085A != value)
			{
				_085A = value;
				_085B = true;
			}
		}
	}

	[Description("Specifies color of the text on the Splitter.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Splitter Colors")]
	public Color SplitterText
	{
		get
		{
			return ࡘ;
		}
		set
		{
			if (ࡘ != value)
			{
				ࡘ = value;
				_0859 = true;
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool SchemeChanged
	{
		get
		{
			if (!ߢ && !ߤ && !_07F6 && !ߩ && !_07EB && !_07EE && !ߧ && !_07F8 && !_07F0 && !ߴ && !_07F2 && !ߝ && !ߟ && !ߺ && !_0826 && !_0829 && !_082B && !_082D && !ࠁ && !ࠃ && !_0822 && !_081B && !_081D && !_0820 && !ࠤ && !ࠅ && !ࠇ && !ࠌ && !ࠊ && !ࠎ && !ࠐ && !ࠕ && !ࠓ && !_0817 && !_07FF && !_0831 && !_0833 && !ߘ && !ߚ && !_082F && !_0836 && !_0838 && !_083B && !_083D && !_083F && !ࡂ && !ࡄ && !ࡆ && !ࡣ && !ࡥ && ࡨ.IsEmpty)
			{
				return ࡧ != ePredefinedColorScheme.AutoGenerated;
			}
			return true;
		}
	}

	[Description("Indicates auto-hide panel background image if any.")]
	[DefaultValue(null)]
	[Category("Auto-Hide Panel")]
	public Image AutoHidePanelBackgroundImage
	{
		get
		{
			return _ࡩ;
		}
		set
		{
			_ࡩ = value;
		}
	}

	[Description("Indicates color of auto-hide panel background.")]
	[Category("Auto-Hide Panel")]
	public Color AutoHidePanelBackground
	{
		get
		{
			return _ࡪ;
		}
		set
		{
			_ࡪ = value;
		}
	}

	[Description("Indicates color of auto-hide panel background.")]
	[Category("Auto-Hide Panel")]
	public Color AutoHidePanelBackground2
	{
		get
		{
			return __086B;
		}
		set
		{
			__086B = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of the auto-hide tab.")]
	public Color AutoHideTabBackground
	{
		get
		{
			return __086C;
		}
		set
		{
			__086C = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide tab.")]
	public Color AutoHideTabBackground2
	{
		get
		{
			return __086D;
		}
		set
		{
			__086D = value;
		}
	}

	[Description("Gets or sets the auto-hide tab background gradient angle.")]
	[Category("Auto-Hide Panel")]
	[DefaultValue(90)]
	public int AutoHideTabBackgroundGradientAngle
	{
		get
		{
			return __086E;
		}
		set
		{
			__086E = value;
		}
	}

	[Description("Indicates color of auto-hide tab text.")]
	[Category("Auto-Hide Panel")]
	public Color AutoHideTabText
	{
		get
		{
			return __086F;
		}
		set
		{
			__086F = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of auto-hide tab border.")]
	public Color AutoHideTabBorder
	{
		get
		{
			return __0870;
		}
		set
		{
			__0870 = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of the auto-hide tab.")]
	public Color AutoHideSelectedTabBackground
	{
		get
		{
			return __0871;
		}
		set
		{
			__0871 = value;
		}
	}

	[Description("Indicates color of auto-hide tab.")]
	[Category("Auto-Hide Panel")]
	public Color AutoHideSelectedTabBackground2
	{
		get
		{
			return __0872;
		}
		set
		{
			__0872 = value;
		}
	}

	[Description("Indicates color of auto-hide tab text.")]
	[Category("Auto-Hide Panel")]
	public Color AutoHideSelectedTabText
	{
		get
		{
			return __0873;
		}
		set
		{
			__0873 = value;
		}
	}

	[Category("Auto-Hide Panel")]
	[Description("Indicates color of tab border.")]
	public Color AutoHideSelectedTabBorder
	{
		get
		{
			return __0874;
		}
		set
		{
			__0874 = value;
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
		Ӷ = style;
		Refresh(null, bSystemColorEvent: true);
	}

	public ColorScheme(eColorSchemeStyle cs)
	{
		eDotNetBarStyle ӷ = eDotNetBarStyle.Office2003;
		switch (cs)
		{
		case eColorSchemeStyle.VS2005:
			ӷ = eDotNetBarStyle.VS2005;
			break;
		case eColorSchemeStyle.Office2007:
			ӷ = eDotNetBarStyle.Office2007;
			break;
		}
		Ӷ = ӷ;
		Refresh(null, bSystemColorEvent: true);
	}

	internal void _0875(eDotNetBarStyle _0657)
	{
		if (Ӷ != _0657)
		{
			Ӷ = _0657;
			Refresh(null, bSystemColorEvent: true);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDockSiteBackColor()
	{
		return ࡣ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDockSiteBackColor2()
	{
		return ࡥ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBarBackground()
	{
		return ߘ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBarBackground2()
	{
		return ߚ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarBackground()
	{
		return ߝ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarBackground2()
	{
		return ߟ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionBackground()
	{
		return ߢ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionBackground2()
	{
		return ߤ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionText()
	{
		return ߧ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionInactiveBackground()
	{
		return ߩ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionInactiveBackground2()
	{
		return _07EB;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarCaptionInactiveText()
	{
		return _07EE;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarPopupBackground()
	{
		return _07F0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarPopupBorder()
	{
		return _07F2;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarDockedBorder()
	{
		return ߴ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarStripeColor()
	{
		return _07F6;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBarFloatingBorder()
	{
		return _07F8;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemBackground()
	{
		return ߺ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemBackground2()
	{
		return _07FC;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemText()
	{
		return _07FF;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemDisabledBackground()
	{
		return ࠁ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemDisabledText()
	{
		return ࠃ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemHotBackground()
	{
		return ࠅ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemHotBackground2()
	{
		return ࠇ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemHotText()
	{
		return ࠊ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemHotBorder()
	{
		return ࠌ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemPressedBackground()
	{
		return ࠎ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemPressedBackground2()
	{
		return ࠐ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemPressedText()
	{
		return ࠓ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemPressedBorder()
	{
		return ࠕ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemSeparator()
	{
		return _0817;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemSeparatorShade()
	{
		return _0819;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedShadow()
	{
		return _0822;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedBackground()
	{
		return _081B;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedBackground2()
	{
		return _081D;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedText()
	{
		return _0820;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemExpandedBorder()
	{
		return ࠤ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemCheckedBackground()
	{
		return _0826;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemCheckedBackground2()
	{
		return _0829;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemCheckedBorder()
	{
		return _082B;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemCheckedText()
	{
		return _082D;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCustomizeBackground()
	{
		return ࡄ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCustomizeBackground2()
	{
		return ࡆ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCustomizeText()
	{
		return ࡉ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBorder()
	{
		return _082F;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBackground()
	{
		return _0831;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuBackground2()
	{
		return _0833;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuSide()
	{
		return _0836;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuSide2()
	{
		return _0838;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuUnusedBackground()
	{
		return _083B;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuUnusedSide()
	{
		return _083D;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMenuUnusedSide2()
	{
		return _083F;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeItemDesignTimeBorder()
	{
		return ࡂ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelBackground()
	{
		return ࡋ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelBackground2()
	{
		return ࡍ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelBorder()
	{
		return ࡒ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelText()
	{
		return ࡐ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExplorerBarBackground()
	{
		return _085D;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExplorerBarBackground2()
	{
		return _085F;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMdiSystemItemForeground()
	{
		return !ࡨ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMdiSystemItemForeground()
	{
		MdiSystemItemForeground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSplitterBackground()
	{
		return ࡔ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSplitterBackground2()
	{
		return ࡖ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSplitterBorder()
	{
		return _085B;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSplitterText()
	{
		return _0859;
	}

	public void Refresh()
	{
		Refresh(null, bSystemColorEvent: false);
	}

	public void ResetChangedFlag()
	{
		ߢ = false;
		ߤ = false;
		_07F6 = false;
		ߩ = false;
		_07EB = false;
		_07EE = false;
		ߧ = false;
		_07F8 = false;
		_07F0 = false;
		_07F2 = false;
		ߴ = false;
		ߝ = false;
		ߟ = false;
		ߺ = false;
		_07FC = false;
		_0826 = false;
		_0829 = false;
		_082B = false;
		_082D = false;
		ࠁ = false;
		ࠃ = false;
		_0822 = false;
		_081B = false;
		_081D = false;
		_0820 = false;
		ࠤ = false;
		ࠅ = false;
		ࠇ = false;
		ࠌ = false;
		ࠊ = false;
		ࠎ = false;
		ࠐ = false;
		ࠕ = false;
		ࠓ = false;
		_0817 = false;
		_0819 = false;
		_07FF = false;
		_0831 = false;
		_0833 = false;
		ߘ = false;
		ߚ = false;
		_082F = false;
		_0836 = false;
		_0838 = false;
		_083B = false;
		_083D = false;
		_083F = false;
		ࡂ = false;
		ࡄ = false;
		ࡆ = false;
		ࡉ = false;
		ࡋ = false;
		ࡍ = false;
		ࡒ = false;
		ࡐ = false;
		ࡔ = false;
		ࡖ = false;
		_085B = false;
		_0859 = false;
		_085D = false;
		_085F = false;
		ࡨ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHidePanelBackground()
	{
		return !_ࡪ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHidePanelBackground()
	{
		AutoHidePanelBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHidePanelBackground2()
	{
		return !__086B.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHidePanelBackground2()
	{
		AutoHidePanelBackground2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideTabBackground()
	{
		return !__086C.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideTabBackground()
	{
		AutoHideTabBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideTabBackground2()
	{
		return !__086D.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideTabBackground2()
	{
		AutoHideTabBackground2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideTabText()
	{
		return !__086F.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideTabText()
	{
		AutoHideTabText = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideTabBorder()
	{
		return !__0870.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideTabBorder()
	{
		AutoHideTabBorder = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideSelectedTabBackground()
	{
		return !__0871.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideSelectedTabBackground()
	{
		AutoHideSelectedTabBackground = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideSelectedTabBackground2()
	{
		return !__0872.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideSelectedTabBackground2()
	{
		AutoHideSelectedTabBackground2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideSelectedTabText()
	{
		return !__0873.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAutoHideSelectedTabText()
	{
		AutoHideSelectedTabText = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAutoHideSelectedTabBorder()
	{
		return !__0874.IsEmpty;
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
		if (ࡧ == ePredefinedColorScheme.Blue2003)
		{
			_087C();
		}
		else if (ࡧ == ePredefinedColorScheme.OliveGreen2003)
		{
			_087D();
		}
		else if (ࡧ == ePredefinedColorScheme.Silver2003)
		{
			_087E();
		}
		else if (ࡧ == ePredefinedColorScheme.SystemColors)
		{
			_0876();
		}
		else if (ߕ == eDotNetBarStyle.Office2007)
		{
			_0882();
		}
		else
		{
			switch (BarFunctions._0945)
			{
			case _0883._0885:
				if (ߕ == eDotNetBarStyle.Office2003)
				{
					_087C();
				}
				else if (ߕ == eDotNetBarStyle.VS2005)
				{
					_087F();
				}
				else
				{
					_0879();
				}
				break;
			case _0883._0887:
				if (ߕ == eDotNetBarStyle.Office2003)
				{
					_087E();
				}
				else if (ߕ == eDotNetBarStyle.VS2005)
				{
					_0881();
				}
				else
				{
					_087B();
				}
				break;
			case _0883._0886:
				if (ߕ == eDotNetBarStyle.Office2003)
				{
					_087D();
				}
				else if (ߕ == eDotNetBarStyle.VS2005)
				{
					_0880();
				}
				else
				{
					_087A();
				}
				break;
			default:
				_0876();
				break;
			case _0883._0884:
				_0876();
				break;
			}
		}
		bool flag = false;
		if (graphics == null)
		{
			IntPtr desktopWindow = _089F.GetDesktopWindow();
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
				if (!ߡ.IsSystemColor && !ߡ.IsEmpty)
				{
					ߡ = graphics.GetNearestColor(ߡ);
				}
				if (!ߣ.IsSystemColor && !ߣ.IsEmpty)
				{
					ߣ = graphics.GetNearestColor(ߣ);
				}
				if (!ߵ.IsSystemColor && !ߵ.IsEmpty)
				{
					ߵ = graphics.GetNearestColor(ߵ);
				}
				if (!ߨ.IsSystemColor && !ߨ.IsEmpty)
				{
					ߨ = graphics.GetNearestColor(ߨ);
				}
				if (!ߪ.IsSystemColor && !ߪ.IsEmpty)
				{
					ߪ = graphics.GetNearestColor(ߪ);
				}
				if (!_07ED.IsSystemColor && !_07ED.IsEmpty)
				{
					_07ED = graphics.GetNearestColor(_07ED);
				}
				if (!ߦ.IsSystemColor && !ߦ.IsEmpty)
				{
					ߦ = graphics.GetNearestColor(ߦ);
				}
				if (!_07F7.IsSystemColor && !_07F7.IsEmpty)
				{
					_07F7 = graphics.GetNearestColor(_07F7);
				}
				if (!_07EF.IsSystemColor && !_07EF.IsEmpty)
				{
					_07EF = graphics.GetNearestColor(_07EF);
				}
				if (!_07F1.IsSystemColor && !_07F1.IsEmpty)
				{
					_07F1 = graphics.GetNearestColor(_07F1);
				}
				if (!_07F3.IsSystemColor && !_07F3.IsEmpty)
				{
					_07F3 = graphics.GetNearestColor(_07F3);
				}
				if (!ߜ.IsSystemColor && !ߜ.IsEmpty)
				{
					ߜ = graphics.GetNearestColor(ߜ);
				}
				if (!ߞ.IsEmpty && !ߞ.IsSystemColor)
				{
					ߞ = graphics.GetNearestColor(ߞ);
				}
				if (!_07F9.IsEmpty)
				{
					_07F9 = graphics.GetNearestColor(_07F9);
				}
				if (!_07FB.IsEmpty)
				{
					_07FB = graphics.GetNearestColor(_07FB);
				}
				if (!_0825.IsSystemColor)
				{
					_0825 = graphics.GetNearestColor(_0825);
				}
				if (!ࠨ.IsEmpty && !ࠨ.IsSystemColor)
				{
					ࠨ = graphics.GetNearestColor(ࠨ);
				}
				if (!_082A.IsSystemColor)
				{
					_082A = graphics.GetNearestColor(_082A);
				}
				if (!_082C.IsSystemColor)
				{
					_082C = graphics.GetNearestColor(_082C);
				}
				if (!ࠀ.IsEmpty && !ࠀ.IsSystemColor)
				{
					ࠀ = graphics.GetNearestColor(ࠀ);
				}
				if (!ࠂ.IsSystemColor)
				{
					ࠂ = graphics.GetNearestColor(ࠂ);
				}
				if (!_0821.IsSystemColor && !_0821.IsEmpty)
				{
					_0821 = graphics.GetNearestColor(_0821);
				}
				if (!ࠚ.IsSystemColor)
				{
					ࠚ = graphics.GetNearestColor(ࠚ);
				}
				if (!_081F.IsSystemColor)
				{
					_081F = graphics.GetNearestColor(_081F);
				}
				if (!_0823.IsSystemColor)
				{
					_0823 = graphics.GetNearestColor(_0823);
				}
				if (!ࠄ.IsSystemColor)
				{
					ࠄ = graphics.GetNearestColor(ࠄ);
				}
				if (!ࠆ.IsEmpty && !ࠆ.IsSystemColor)
				{
					ࠆ = graphics.GetNearestColor(ࠆ);
				}
				if (!ࠋ.IsSystemColor)
				{
					ࠋ = graphics.GetNearestColor(ࠋ);
				}
				if (!ࠉ.IsSystemColor)
				{
					ࠉ = graphics.GetNearestColor(ࠉ);
				}
				if (!ࠍ.IsSystemColor)
				{
					ࠍ = graphics.GetNearestColor(ࠍ);
				}
				if (!ࠏ.IsEmpty && !ࠏ.IsSystemColor)
				{
					ࠏ = graphics.GetNearestColor(ࠏ);
				}
				if (!ࠔ.IsSystemColor)
				{
					ࠔ = graphics.GetNearestColor(ࠔ);
				}
				if (!ࠒ.IsSystemColor)
				{
					ࠒ = graphics.GetNearestColor(ࠒ);
				}
				if (!_0816.IsSystemColor)
				{
					_0816 = graphics.GetNearestColor(_0816);
				}
				if (!_0818.IsSystemColor && !_0818.IsEmpty)
				{
					_0818 = graphics.GetNearestColor(_0818);
				}
				if (!_07FE.IsSystemColor)
				{
					_07FE = graphics.GetNearestColor(_07FE);
				}
				if (!_0830.IsSystemColor)
				{
					_0830 = graphics.GetNearestColor(_0830);
				}
				if (!ߗ.IsSystemColor)
				{
					ߗ = graphics.GetNearestColor(ߗ);
				}
				if (!ߙ.IsEmpty && !ߙ.IsSystemColor)
				{
					ߙ = graphics.GetNearestColor(ߙ);
				}
				if (!_082E.IsSystemColor)
				{
					_082E = graphics.GetNearestColor(_082E);
				}
				if (!_0835.IsSystemColor)
				{
					_0835 = graphics.GetNearestColor(_0835);
				}
				if (!_083A.IsSystemColor)
				{
					_083A = graphics.GetNearestColor(_083A);
				}
				if (!_083C.IsSystemColor)
				{
					_083C = graphics.GetNearestColor(_083C);
				}
				if (!ࡁ.IsSystemColor)
				{
					ࡁ = graphics.GetNearestColor(ࡁ);
				}
				if (!ࡃ.IsEmpty && !ࡃ.IsSystemColor)
				{
					ࡃ = graphics.GetNearestColor(ࡃ);
				}
				if (!ࡅ.IsEmpty && !ࡅ.IsSystemColor)
				{
					ࡅ = graphics.GetNearestColor(ࡅ);
				}
				if (!ࡈ.IsEmpty && !ࡈ.IsSystemColor)
				{
					ࡈ = graphics.GetNearestColor(ࡈ);
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

	private void _0876()
	{
		if (Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005)
		{
			_0878();
		}
		else
		{
			_0877();
		}
	}

	private void _0877()
	{
		if (!ߢ)
		{
			ߡ = ColorFunctions.MenuFocusBorderColor();
		}
		ߣ = Color.Empty;
		if (!_07F6)
		{
			ߵ = SystemColors.ControlDark;
		}
		if (!ߩ)
		{
			ߨ = SystemColors.Control;
		}
		ߪ = Color.Empty;
		if (!_07EE)
		{
			_07ED = SystemColors.ControlText;
		}
		if (!ߧ)
		{
			ߦ = SystemColors.ActiveCaptionText;
		}
		if (!_07F8)
		{
			_07F7 = ColorFunctions.MenuFocusBorderColor();
		}
		if (!_07F0)
		{
			_07EF = SystemColors.Control;
		}
		if (!_07F2)
		{
			_07F1 = ColorFunctions.MenuFocusBorderColor();
		}
		if (!ߝ)
		{
			ߜ = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = ColorFunctions.CheckBoxBackColor();
		}
		if (!_082B)
		{
			_082A = SystemColors.Highlight;
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = SystemColors.ControlDark;
		}
		if (!_0822)
		{
			_0821 = SystemColors.ControlDark;
		}
		if (!_081B)
		{
			ࠚ = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = ColorFunctions.HoverBackColor();
		}
		if (!ࠌ)
		{
			ࠋ = SystemColors.Highlight;
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = ColorFunctions.PressedBackColor();
		}
		if (!ࠕ)
		{
			ࠔ = SystemColors.Highlight;
		}
		if (!ࠓ)
		{
			ࠒ = SystemColors.ControlDarkDark;
		}
		if (!_0817)
		{
			_0816 = SystemColors.ControlDark;
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = ColorFunctions.MenuBackColor();
		}
		if (!ߘ)
		{
			ߗ = SystemColors.Control;
		}
		if (!_082F)
		{
			_082E = SystemColors.ControlDark;
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = ColorFunctions.SideRecentlyBackColor();
		}
		if (!ࡂ)
		{
			ࡁ = SystemColors.Highlight;
		}
		if (!ߟ)
		{
			ߞ = Color.Empty;
		}
		if (!ߚ)
		{
			ߙ = Color.Empty;
		}
		if (!ࠇ)
		{
			ࠆ = Color.Empty;
		}
		if (!ࠐ)
		{
			ࠏ = Color.Empty;
		}
		if (!_081D)
		{
			_081C = Color.Empty;
		}
		if (!_0829)
		{
			ࠨ = Color.Empty;
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!_0838)
		{
			_0837 = Color.Empty;
		}
		if (!_083F)
		{
			_083E = Color.Empty;
		}
		if (!ߴ)
		{
			_07F3 = Color.Empty;
		}
		if (!_0819)
		{
			_0818 = Color.Empty;
		}
		if (!ࡣ)
		{
			ࡢ = Color.Empty;
		}
		if (!ࡥ)
		{
			ࡤ = Color.Empty;
		}
		if (!ࡄ)
		{
			ࡃ = Color.Empty;
		}
		if (!ࡆ)
		{
			ࡅ = Color.Empty;
		}
		if (!ࡉ)
		{
			ࡈ = Color.Empty;
		}
		if (!ࡋ)
		{
			ࡊ = ControlPaint.Dark(ߜ);
		}
		if (!ࡍ)
		{
			ࡌ = ControlPaint.DarkDark(ߜ);
		}
		if (!ࡐ)
		{
			ࡏ = ControlPaint.LightLight(ߜ);
		}
		if (!ࡒ)
		{
			ࡑ = ControlPaint.DarkDark(ߜ);
		}
		if (!ࡔ)
		{
			ࡓ = ControlPaint.Dark(ߜ);
		}
		if (!ࡖ)
		{
			ࡕ = ControlPaint.DarkDark(ߜ);
		}
		if (!_0859)
		{
			ࡘ = ControlPaint.LightLight(ߜ);
		}
		if (!_085B)
		{
			_085A = ControlPaint.DarkDark(ߜ);
		}
		if (!_085D)
		{
			_085C = ߗ;
		}
		if (!_085F)
		{
			_085E = _0816;
		}
	}

	private void _0878()
	{
		if (!ߢ)
		{
			ߡ = ColorFunctions.MenuFocusBorderColor();
		}
		if (!ߤ)
		{
			ߣ = ControlPaint.Light(ߡ);
		}
		if (!_07F6)
		{
			ߵ = SystemColors.ControlDark;
		}
		if (!ߩ)
		{
			ߨ = SystemColors.Control;
		}
		ߪ = Color.Empty;
		if (!_07EE)
		{
			_07ED = SystemColors.ControlText;
		}
		if (!ߧ)
		{
			ߦ = SystemColors.ActiveCaptionText;
		}
		if (!_07F8)
		{
			_07F7 = ColorFunctions.MenuFocusBorderColor();
		}
		if (!_07F0)
		{
			_07EF = SystemColors.Control;
		}
		if (!_07F2)
		{
			_07F1 = ColorFunctions.MenuFocusBorderColor();
		}
		if (!ߝ)
		{
			ߜ = ColorFunctions.MenuBackColor();
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = ColorFunctions.CheckBoxBackColor();
		}
		if (!_0829)
		{
			ࠨ = Color.Empty;
		}
		if (!_082B)
		{
			_082A = SystemColors.Highlight;
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = SystemColors.ControlDark;
		}
		if (!_0822)
		{
			_0821 = Color.Empty;
		}
		if (!_081B)
		{
			ࠚ = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = ColorFunctions.HoverBackColor3();
		}
		if (!ࠌ)
		{
			ࠋ = SystemColors.Highlight;
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = ColorFunctions.HoverBackColor2();
		}
		if (!ࠕ)
		{
			ࠔ = SystemColors.Highlight;
		}
		if (!ࠓ)
		{
			ࠒ = SystemColors.ControlDarkDark;
		}
		if (!_0817)
		{
			_0816 = SystemColors.ControlDark;
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = ColorFunctions.MenuBackColor();
		}
		if (!ߘ)
		{
			ߗ = SystemColors.Control;
		}
		if (!_082F)
		{
			_082E = SystemColors.ControlDark;
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = ColorFunctions.MenuBackColor();
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = ColorFunctions.SideRecentlyBackColor();
		}
		if (!ࡂ)
		{
			ࡁ = SystemColors.Highlight;
		}
		if (!ߟ)
		{
			ߞ = SystemColors.Control;
		}
		if (!ߚ)
		{
			ߙ = Color.Empty;
		}
		if (!ࠇ)
		{
			ࠆ = ColorFunctions.HoverBackColor2();
		}
		if (!ࠐ)
		{
			ࠏ = ColorFunctions.HoverBackColor3();
		}
		if (!_081D)
		{
			_081C = Color.Empty;
		}
		if (!_0829)
		{
			ࠨ = Color.Empty;
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!_0838)
		{
			_0837 = ColorFunctions.ToolMenuFocusBackColor();
		}
		if (!_083F)
		{
			_083E = Color.Empty;
		}
		if (!ߴ)
		{
			_07F3 = SystemColors.Control;
		}
		if (!_0819)
		{
			_0818 = SystemColors.ControlLightLight;
		}
		if (!ࡣ)
		{
			ࡢ = SystemColors.Control;
		}
		if (!ࡥ)
		{
			ࡤ = ColorFunctions.MenuBackColor();
		}
		if (!ࡄ)
		{
			ࡃ = SystemColors.ControlDark;
		}
		if (!ࡆ)
		{
			ࡅ = ColorFunctions.MenuFocusBorderColor();
		}
		if (!ࡉ)
		{
			ࡈ = SystemColors.ControlText;
		}
		if (!ࡋ)
		{
			ࡊ = ߞ;
		}
		if (!ࡍ)
		{
			ࡌ = ControlPaint.Dark(ߞ);
		}
		if (!ࡐ)
		{
			ࡏ = ControlPaint.LightLight(ߜ);
		}
		if (!ࡒ)
		{
			ࡑ = ControlPaint.DarkDark(ߞ);
		}
		if (!ࡔ)
		{
			ࡓ = ߞ;
		}
		if (!ࡖ)
		{
			ࡕ = ControlPaint.Dark(ߞ);
		}
		if (!_0859)
		{
			ࡘ = ControlPaint.LightLight(ߜ);
		}
		if (!_085B)
		{
			_085A = ControlPaint.DarkDark(ߞ);
		}
		if (!_085D)
		{
			_085C = ߗ;
		}
		if (!_085F)
		{
			_085E = _0816;
		}
	}

	private void _0879()
	{
		if (!ߝ)
		{
			ߜ = Color.FromArgb(239, 237, 222);
		}
		if (!_07F6)
		{
			ߵ = Color.FromArgb(191, 188, 177);
		}
		if (!ߢ)
		{
			ߡ = Color.FromArgb(172, 168, 153);
		}
		ߣ = Color.Empty;
		if (!ߩ)
		{
			ߨ = SystemColors.Control;
		}
		ߪ = Color.Empty;
		if (!_07EE)
		{
			_07ED = SystemColors.ControlText;
		}
		if (!ߧ)
		{
			ߦ = Color.FromArgb(64, 64, 64);
		}
		if (!_07F8)
		{
			_07F7 = Color.FromArgb(172, 168, 153);
		}
		if (!_07F0)
		{
			_07EF = Color.FromArgb(252, 252, 249);
		}
		if (!_07F2)
		{
			_07F1 = Color.FromArgb(138, 134, 122);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = Color.FromArgb(225, 230, 232);
		}
		if (!_082B)
		{
			_082A = SystemColors.Highlight;
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = SystemColors.ControlDark;
		}
		if (!_0822)
		{
			_0821 = SystemColors.ControlDark;
		}
		if (!_081B)
		{
			ࠚ = Color.FromArgb(239, 237, 222);
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = Color.FromArgb(193, 210, 238);
		}
		if (!ࠌ)
		{
			ࠋ = SystemColors.Highlight;
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = Color.FromArgb(152, 181, 226);
		}
		if (!ࠕ)
		{
			ࠔ = SystemColors.Highlight;
		}
		if (!ࠓ)
		{
			ࠒ = Color.FromArgb(73, 73, 73);
		}
		if (!_0817)
		{
			_0816 = Color.FromArgb(197, 194, 184);
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = Color.FromArgb(252, 252, 249);
		}
		if (!ߘ)
		{
			ߗ = SystemColors.Control;
		}
		if (!_082F)
		{
			_082E = SystemColors.ControlDark;
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = Color.FromArgb(239, 237, 222);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = Color.FromArgb(230, 227, 210);
		}
		if (!ࡂ)
		{
			ࡁ = SystemColors.Highlight;
		}
		if (!ߟ)
		{
			ߞ = Color.Empty;
		}
		if (!ߚ)
		{
			ߙ = Color.Empty;
		}
		if (!ࠇ)
		{
			ࠆ = Color.Empty;
		}
		if (!ࠐ)
		{
			ࠏ = Color.Empty;
		}
		if (!_081D)
		{
			_081C = Color.Empty;
		}
		if (!_0829)
		{
			ࠨ = Color.Empty;
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!_0838)
		{
			_0837 = Color.Empty;
		}
		if (!_083F)
		{
			_083E = Color.Empty;
		}
		if (!ߴ)
		{
			_07F3 = Color.Empty;
		}
		if (!_0819)
		{
			_0818 = Color.Empty;
		}
		if (!ࡣ)
		{
			ࡢ = Color.Empty;
		}
		if (!ࡥ)
		{
			ࡤ = Color.Empty;
		}
		if (!ࡄ)
		{
			ࡃ = Color.Empty;
		}
		if (!ࡆ)
		{
			ࡅ = Color.Empty;
		}
		if (!ࡉ)
		{
			ࡈ = Color.Empty;
		}
		if (!ࡋ)
		{
			ࡊ = Color.FromArgb(89, 135, 214);
		}
		if (!ࡍ)
		{
			ࡌ = Color.FromArgb(3, 56, 148);
		}
		if (!ࡐ)
		{
			ࡏ = Color.White;
		}
		if (!ࡒ)
		{
			ࡑ = Color.FromArgb(0, 45, 150);
		}
		if (!ࡔ)
		{
			ࡓ = Color.FromArgb(89, 135, 214);
		}
		if (!ࡖ)
		{
			ࡕ = Color.FromArgb(3, 56, 148);
		}
		if (!_0859)
		{
			ࡘ = Color.White;
		}
		if (!_085B)
		{
			_085A = Color.FromArgb(0, 45, 150);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(123, 162, 231);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(99, 117, 214);
		}
	}

	private void _087A()
	{
		if (!ߝ)
		{
			ߜ = Color.FromArgb(239, 237, 222);
		}
		if (!_07F6)
		{
			ߵ = Color.FromArgb(191, 188, 177);
		}
		if (!ߢ)
		{
			ߡ = Color.FromArgb(153, 153, 153);
		}
		ߣ = Color.Empty;
		if (!ߩ)
		{
			ߨ = SystemColors.Control;
		}
		ߪ = Color.Empty;
		if (!_07EE)
		{
			_07ED = SystemColors.ControlText;
		}
		if (!ߧ)
		{
			ߦ = Color.FromArgb(64, 64, 64);
		}
		if (!_07F8)
		{
			_07F7 = Color.FromArgb(153, 153, 153);
		}
		if (!_07F0)
		{
			_07EF = Color.FromArgb(252, 252, 249);
		}
		if (!_07F2)
		{
			_07F1 = Color.FromArgb(138, 134, 122);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = Color.FromArgb(234, 235, 223);
		}
		if (!_082B)
		{
			_082A = SystemColors.Highlight;
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = SystemColors.ControlDark;
		}
		if (!_0822)
		{
			_0821 = SystemColors.ControlDark;
		}
		if (!_081B)
		{
			ࠚ = Color.FromArgb(239, 237, 222);
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = Color.FromArgb(206, 209, 195);
		}
		if (!ࠌ)
		{
			ࠋ = SystemColors.Highlight;
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = Color.FromArgb(201, 208, 184);
		}
		if (!ࠕ)
		{
			ࠔ = SystemColors.Highlight;
		}
		if (!ࠓ)
		{
			ࠒ = Color.FromArgb(102, 102, 102);
		}
		if (!_0817)
		{
			_0816 = Color.FromArgb(197, 194, 184);
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = Color.FromArgb(252, 252, 249);
		}
		if (!ߘ)
		{
			ߗ = SystemColors.Control;
		}
		if (!_082F)
		{
			_082E = SystemColors.ControlDark;
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = Color.FromArgb(239, 237, 222);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = Color.FromArgb(230, 227, 210);
		}
		if (!ࡂ)
		{
			ࡁ = SystemColors.Highlight;
		}
		if (!ߟ)
		{
			ߞ = Color.Empty;
		}
		if (!ߚ)
		{
			ߙ = Color.Empty;
		}
		if (!ࠇ)
		{
			ࠆ = Color.Empty;
		}
		if (!ࠐ)
		{
			ࠏ = Color.Empty;
		}
		if (!_081D)
		{
			_081C = Color.Empty;
		}
		if (!_0829)
		{
			ࠨ = Color.Empty;
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!_0838)
		{
			_0837 = Color.Empty;
		}
		if (!_083F)
		{
			_083E = Color.Empty;
		}
		if (!ߴ)
		{
			_07F3 = Color.Empty;
		}
		if (!_0819)
		{
			_0818 = Color.Empty;
		}
		if (!ࡣ)
		{
			ࡢ = Color.Empty;
		}
		if (!ࡥ)
		{
			ࡤ = Color.Empty;
		}
		if (!ࡄ)
		{
			ࡃ = Color.Empty;
		}
		if (!ࡆ)
		{
			ࡅ = Color.Empty;
		}
		if (!ࡉ)
		{
			ࡈ = Color.Empty;
		}
		if (!ࡋ)
		{
			ࡊ = Color.FromArgb(175, 192, 130);
		}
		if (!ࡍ)
		{
			ࡌ = Color.FromArgb(99, 122, 68);
		}
		if (!ࡐ)
		{
			ࡏ = Color.White;
		}
		if (!ࡒ)
		{
			ࡑ = Color.FromArgb(96, 128, 88);
		}
		if (!ࡔ)
		{
			ࡓ = Color.FromArgb(175, 192, 130);
		}
		if (!ࡖ)
		{
			ࡕ = Color.FromArgb(99, 122, 68);
		}
		if (!_0859)
		{
			ࡘ = Color.White;
		}
		if (!_085B)
		{
			_085A = Color.FromArgb(96, 128, 88);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(204, 217, 173);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(165, 189, 132);
		}
	}

	private void _087B()
	{
		if (!ߝ)
		{
			ߜ = Color.FromArgb(229, 228, 232);
		}
		if (!_07F6)
		{
			ߵ = Color.FromArgb(179, 179, 182);
		}
		if (!ߢ)
		{
			ߡ = Color.FromArgb(157, 157, 161);
		}
		ߣ = Color.Empty;
		if (!ߩ)
		{
			ߨ = SystemColors.Control;
		}
		ߪ = Color.Empty;
		if (!_07EE)
		{
			_07ED = SystemColors.ControlText;
		}
		if (!ߧ)
		{
			ߦ = Color.FromArgb(53, 53, 53);
		}
		if (!_07F8)
		{
			_07F7 = Color.FromArgb(157, 157, 161);
		}
		if (!_07F0)
		{
			_07EF = Color.FromArgb(251, 250, 251);
		}
		if (!_07F2)
		{
			_07F1 = Color.FromArgb(126, 126, 129);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = Color.FromArgb(233, 234, 237);
		}
		if (!_082B)
		{
			_082A = SystemColors.Highlight;
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = SystemColors.ControlDark;
		}
		if (!_0822)
		{
			_0821 = SystemColors.ControlDark;
		}
		if (!_081B)
		{
			ࠚ = Color.FromArgb(229, 228, 232);
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = Color.FromArgb(199, 199, 202);
		}
		if (!ࠌ)
		{
			ࠋ = Color.FromArgb(169, 171, 181);
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = Color.FromArgb(210, 211, 216);
		}
		if (!ࠕ)
		{
			ࠔ = SystemColors.Highlight;
		}
		if (!ࠓ)
		{
			ࠒ = SystemColors.ControlText;
		}
		if (!_0817)
		{
			_0816 = Color.FromArgb(186, 186, 189);
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = Color.FromArgb(251, 250, 251);
		}
		if (!ߘ)
		{
			ߗ = SystemColors.Control;
		}
		if (!_082F)
		{
			_082E = SystemColors.ControlDark;
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = Color.FromArgb(229, 228, 232);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = Color.FromArgb(217, 216, 220);
		}
		if (!ࡂ)
		{
			ࡁ = SystemColors.Highlight;
		}
		if (!ߟ)
		{
			ߞ = Color.Empty;
		}
		if (!ߚ)
		{
			ߙ = Color.Empty;
		}
		if (!ࠇ)
		{
			ࠆ = Color.Empty;
		}
		if (!ࠐ)
		{
			ࠏ = Color.Empty;
		}
		if (!_081D)
		{
			_081C = Color.Empty;
		}
		if (!_0829)
		{
			ࠨ = Color.Empty;
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!_0838)
		{
			_0837 = Color.Empty;
		}
		if (!_083F)
		{
			_083E = Color.Empty;
		}
		if (!ߴ)
		{
			_07F3 = Color.Empty;
		}
		if (!_0819)
		{
			_0818 = Color.Empty;
		}
		if (!ࡣ)
		{
			ࡢ = Color.Empty;
		}
		if (!ࡥ)
		{
			ࡤ = Color.Empty;
		}
		if (!ࡄ)
		{
			ࡃ = Color.Empty;
		}
		if (!ࡆ)
		{
			ࡅ = Color.Empty;
		}
		if (!ࡉ)
		{
			ࡈ = Color.Empty;
		}
		if (!ࡋ)
		{
			ࡊ = Color.FromArgb(168, 167, 191);
		}
		if (!ࡍ)
		{
			ࡌ = Color.FromArgb(112, 111, 145);
		}
		if (!ࡐ)
		{
			ࡏ = Color.White;
		}
		if (!ࡒ)
		{
			ࡑ = Color.FromArgb(124, 124, 148);
		}
		if (!ࡔ)
		{
			ࡓ = Color.FromArgb(168, 167, 191);
		}
		if (!ࡖ)
		{
			ࡕ = Color.FromArgb(112, 111, 145);
		}
		if (!_0859)
		{
			ࡘ = Color.White;
		}
		if (!_085B)
		{
			_085A = Color.FromArgb(124, 124, 148);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(196, 200, 212);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(177, 179, 200);
		}
	}

	private void _087C()
	{
		if (!ߝ)
		{
			ߜ = Color.FromArgb(223, 237, 254);
		}
		if (!ߟ)
		{
			ߞ = Color.FromArgb(142, 179, 231);
		}
		if (!_07F6)
		{
			ߵ = Color.FromArgb(39, 65, 118);
		}
		if (!ߢ)
		{
			ߡ = GetColor(6721760);
		}
		if (!ߤ)
		{
			ߣ = Color.FromArgb(42, 102, 201);
		}
		if (!ߩ)
		{
			ߨ = GetColor(6724095);
		}
		if (!_07EB)
		{
			ߪ = GetColor(5602242);
		}
		if (!_07EE)
		{
			_07ED = Color.White;
		}
		if (!ߧ)
		{
			ߦ = Color.White;
		}
		if (!_07F8)
		{
			_07F7 = Color.FromArgb(42, 102, 201);
		}
		if (!_07F0)
		{
			_07EF = Color.FromArgb(246, 246, 246);
		}
		if (!_07F2)
		{
			_07F1 = Color.FromArgb(0, 45, 150);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = Color.FromArgb(255, 213, 140);
		}
		if (!_0829)
		{
			ࠨ = Color.FromArgb(255, 173, 85);
		}
		if (!_082B)
		{
			_082A = Color.FromArgb(0, 0, 128);
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = Color.FromArgb(141, 141, 141);
		}
		if (!_0822)
		{
			_0821 = Color.Empty;
		}
		if (!_081B)
		{
			ࠚ = Color.FromArgb(227, 239, 255);
		}
		if (!_081D)
		{
			_081C = Color.FromArgb(147, 181, 231);
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = Color.FromArgb(255, 244, 204);
		}
		if (!ࠇ)
		{
			ࠆ = Color.FromArgb(255, 208, 145);
		}
		if (!ࠌ)
		{
			ࠋ = Color.FromArgb(0, 0, 128);
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = Color.FromArgb(254, 142, 75);
		}
		if (!ࠐ)
		{
			ࠏ = Color.FromArgb(255, 207, 139);
		}
		if (!ࠕ)
		{
			ࠔ = Color.FromArgb(0, 0, 128);
		}
		if (!ࠓ)
		{
			ࠒ = SystemColors.ControlText;
		}
		if (!_0817)
		{
			_0816 = Color.FromArgb(106, 140, 203);
		}
		if (!_0819)
		{
			_0818 = Color.FromArgb(241, 249, 255);
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = Color.FromArgb(246, 246, 246);
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!ߘ)
		{
			ߗ = Color.FromArgb(158, 190, 245);
		}
		if (!_082F)
		{
			_082E = Color.FromArgb(0, 45, 150);
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = Color.FromArgb(227, 239, 255);
		}
		if (!_0838)
		{
			_0837 = Color.FromArgb(135, 173, 228);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = Color.FromArgb(203, 221, 246);
		}
		if (!_083F)
		{
			_083E = Color.FromArgb(121, 161, 220);
		}
		if (!ࡂ)
		{
			ࡁ = Color.FromArgb(0, 0, 128);
		}
		if (!ߴ)
		{
			_07F3 = Color.FromArgb(59, 97, 156);
		}
		if (!ࡣ)
		{
			ࡢ = Color.FromArgb(158, 190, 245);
		}
		if (!ࡥ)
		{
			ࡤ = Color.FromArgb(195, 218, 249);
		}
		if (!ࡄ)
		{
			ࡃ = Color.FromArgb(117, 166, 241);
		}
		if (!ࡆ)
		{
			ࡅ = Color.FromArgb(0, 53, 145);
		}
		if (!ࡉ)
		{
			ࡈ = SystemColors.ControlText;
		}
		if (!ࡋ)
		{
			ࡊ = Color.FromArgb(89, 135, 214);
		}
		if (!ࡍ)
		{
			ࡌ = Color.FromArgb(3, 56, 148);
		}
		if (!ࡐ)
		{
			ࡏ = Color.White;
		}
		if (!ࡒ)
		{
			ࡑ = Color.FromArgb(0, 45, 150);
		}
		if (!ࡔ)
		{
			ࡓ = Color.FromArgb(89, 135, 214);
		}
		if (!ࡖ)
		{
			ࡕ = Color.FromArgb(3, 56, 148);
		}
		if (!_0859)
		{
			ࡘ = Color.White;
		}
		if (!_085B)
		{
			_085A = Color.FromArgb(0, 45, 150);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(123, 162, 231);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(99, 117, 214);
		}
	}

	private void _087D()
	{
		if (!ߝ)
		{
			ߜ = Color.FromArgb(244, 247, 222);
		}
		if (!ߟ)
		{
			ߞ = Color.FromArgb(183, 198, 145);
		}
		if (!_07F6)
		{
			ߵ = Color.FromArgb(81, 94, 51);
		}
		if (!ߢ)
		{
			ߡ = Color.FromArgb(183, 198, 145);
		}
		if (!ߤ)
		{
			ߣ = Color.FromArgb(116, 134, 94);
		}
		if (!ߩ)
		{
			ߨ = GetColor(13753270);
		}
		if (!_07EB)
		{
			ߪ = Color.FromArgb(116, 134, 94);
		}
		if (!_07EE)
		{
			_07ED = Color.White;
		}
		if (!ߧ)
		{
			ߦ = Color.White;
		}
		if (!_07F8)
		{
			_07F7 = Color.FromArgb(116, 134, 94);
		}
		if (!_07F0)
		{
			_07EF = Color.FromArgb(244, 244, 238);
		}
		if (!_07F2)
		{
			_07F1 = Color.FromArgb(117, 141, 94);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = Color.FromArgb(255, 213, 140);
		}
		if (!_0829)
		{
			ࠨ = Color.FromArgb(255, 173, 85);
		}
		if (!_082B)
		{
			_082A = Color.FromArgb(63, 93, 56);
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = Color.FromArgb(141, 141, 141);
		}
		if (!_0822)
		{
			_0821 = Color.Empty;
		}
		if (!_081B)
		{
			ࠚ = Color.FromArgb(236, 240, 213);
		}
		if (!_081D)
		{
			_081C = Color.FromArgb(194, 206, 159);
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = Color.FromArgb(255, 244, 204);
		}
		if (!ࠇ)
		{
			ࠆ = Color.FromArgb(255, 208, 145);
		}
		if (!ࠌ)
		{
			ࠋ = Color.FromArgb(63, 93, 56);
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = Color.FromArgb(254, 142, 75);
		}
		if (!ࠐ)
		{
			ࠏ = Color.FromArgb(255, 207, 139);
		}
		if (!ࠕ)
		{
			ࠔ = Color.FromArgb(63, 93, 56);
		}
		if (!ࠓ)
		{
			ࠒ = SystemColors.ControlText;
		}
		if (!_0817)
		{
			_0816 = Color.FromArgb(96, 128, 88);
		}
		if (!_0819)
		{
			_0818 = Color.FromArgb(244, 247, 222);
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = Color.FromArgb(244, 244, 238);
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!ߘ)
		{
			ߗ = Color.FromArgb(217, 217, 168);
		}
		if (!_082F)
		{
			_082E = Color.FromArgb(117, 141, 94);
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = Color.FromArgb(255, 255, 237);
		}
		if (!_0838)
		{
			_0837 = Color.FromArgb(184, 199, 146);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = Color.FromArgb(230, 230, 239);
		}
		if (!_083F)
		{
			_083E = Color.FromArgb(164, 180, 120);
		}
		if (!ࡂ)
		{
			ࡁ = Color.FromArgb(63, 93, 56);
		}
		if (!ߴ)
		{
			_07F3 = Color.FromArgb(96, 128, 88);
		}
		if (!ࡣ)
		{
			ࡢ = Color.FromArgb(217, 217, 167);
		}
		if (!ࡥ)
		{
			ࡤ = Color.FromArgb(242, 240, 228);
		}
		if (!ࡄ)
		{
			ࡃ = Color.FromArgb(176, 194, 140);
		}
		if (!ࡆ)
		{
			ࡅ = Color.FromArgb(96, 119, 107);
		}
		if (!ࡉ)
		{
			ࡈ = SystemColors.ControlText;
		}
		if (!ࡋ)
		{
			ࡊ = Color.FromArgb(175, 192, 130);
		}
		if (!ࡍ)
		{
			ࡌ = Color.FromArgb(99, 122, 68);
		}
		if (!ࡐ)
		{
			ࡏ = Color.White;
		}
		if (!ࡒ)
		{
			ࡑ = Color.FromArgb(96, 128, 88);
		}
		if (!ࡔ)
		{
			ࡓ = Color.FromArgb(175, 192, 130);
		}
		if (!ࡖ)
		{
			ࡕ = Color.FromArgb(99, 122, 68);
		}
		if (!_0859)
		{
			ࡘ = Color.White;
		}
		if (!_085B)
		{
			_085A = Color.FromArgb(96, 128, 88);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(204, 217, 173);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(165, 189, 132);
		}
	}

	private void _087E()
	{
		if (!ߝ)
		{
			ߜ = Color.FromArgb(243, 244, 250);
		}
		if (!ߟ)
		{
			ߞ = Color.FromArgb(153, 151, 181);
		}
		if (!_07F6)
		{
			ߵ = Color.FromArgb(84, 84, 117);
		}
		if (!ߢ)
		{
			ߡ = Color.FromArgb(153, 151, 181);
		}
		if (!ߤ)
		{
			ߣ = Color.FromArgb(122, 121, 153);
		}
		if (!ߩ)
		{
			ߨ = GetColor(10921655);
		}
		if (!_07EB)
		{
			ߪ = Color.FromArgb(122, 121, 153);
		}
		if (!_07EE)
		{
			_07ED = Color.White;
		}
		if (!ߧ)
		{
			ߦ = Color.White;
		}
		if (!_07F8)
		{
			_07F7 = Color.FromArgb(122, 121, 153);
		}
		if (!_07F0)
		{
			_07EF = Color.FromArgb(253, 250, 255);
		}
		if (!_07F2)
		{
			_07F1 = Color.FromArgb(124, 124, 148);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = Color.FromArgb(255, 213, 140);
		}
		if (!_0829)
		{
			ࠨ = Color.FromArgb(255, 173, 85);
		}
		if (!_082B)
		{
			_082A = Color.FromArgb(75, 75, 111);
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = Color.FromArgb(141, 141, 141);
		}
		if (!_0822)
		{
			_0821 = Color.Empty;
		}
		if (!_081B)
		{
			ࠚ = Color.FromArgb(232, 233, 241);
		}
		if (!_081D)
		{
			_081C = Color.FromArgb(177, 176, 198);
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = Color.FromArgb(255, 244, 204);
		}
		if (!ࠇ)
		{
			ࠆ = Color.FromArgb(255, 208, 145);
		}
		if (!ࠌ)
		{
			ࠋ = Color.FromArgb(75, 75, 111);
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = Color.FromArgb(254, 142, 75);
		}
		if (!ࠐ)
		{
			ࠏ = Color.FromArgb(255, 207, 139);
		}
		if (!ࠕ)
		{
			ࠔ = Color.FromArgb(75, 75, 111);
		}
		if (!ࠓ)
		{
			ࠒ = SystemColors.ControlText;
		}
		if (!_0817)
		{
			_0816 = Color.FromArgb(110, 109, 143);
		}
		if (!_0819)
		{
			_0818 = Color.White;
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = Color.FromArgb(253, 250, 255);
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!ߘ)
		{
			ߗ = Color.FromArgb(215, 215, 229);
		}
		if (!_082F)
		{
			_082E = Color.FromArgb(124, 124, 148);
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = Color.FromArgb(249, 249, 255);
		}
		if (!_0838)
		{
			_0837 = Color.FromArgb(159, 157, 185);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = Color.FromArgb(215, 215, 226);
		}
		if (!_083F)
		{
			_083E = Color.FromArgb(128, 126, 158);
		}
		if (!ࡂ)
		{
			ࡁ = Color.FromArgb(84, 84, 117);
		}
		if (!ߴ)
		{
			_07F3 = Color.FromArgb(124, 124, 148);
		}
		if (!ࡣ)
		{
			ࡢ = Color.FromArgb(215, 215, 229);
		}
		if (!ࡥ)
		{
			ࡤ = Color.FromArgb(243, 243, 247);
		}
		if (!ࡄ)
		{
			ࡃ = Color.FromArgb(179, 178, 200);
		}
		if (!ࡆ)
		{
			ࡅ = Color.FromArgb(118, 116, 146);
		}
		if (!ࡉ)
		{
			ࡈ = SystemColors.ControlText;
		}
		if (!ࡋ)
		{
			ࡊ = Color.FromArgb(168, 167, 191);
		}
		if (!ࡍ)
		{
			ࡌ = Color.FromArgb(112, 111, 145);
		}
		if (!ࡐ)
		{
			ࡏ = Color.White;
		}
		if (!ࡒ)
		{
			ࡑ = Color.FromArgb(124, 124, 148);
		}
		if (!ࡔ)
		{
			ࡓ = Color.FromArgb(168, 167, 191);
		}
		if (!ࡖ)
		{
			ࡕ = Color.FromArgb(112, 111, 145);
		}
		if (!_0859)
		{
			ࡘ = Color.White;
		}
		if (!_085B)
		{
			_085A = Color.FromArgb(124, 124, 148);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(196, 200, 212);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(177, 179, 200);
		}
	}

	private void _087F()
	{
		if (!ߝ)
		{
			ߜ = Color.FromArgb(251, 250, 247);
		}
		if (!ߟ)
		{
			ߞ = Color.FromArgb(181, 181, 154);
		}
		if (!_07F6)
		{
			ߵ = Color.FromArgb(193, 190, 179);
		}
		if (!ߢ)
		{
			ߡ = Color.FromArgb(0, 84, 227);
		}
		if (!ߤ)
		{
			ߣ = Color.FromArgb(60, 148, 254);
		}
		if (!ߩ)
		{
			ߨ = GetColor(13420474);
		}
		if (!_07EB)
		{
			ߪ = Color.Empty;
		}
		if (!_07EE)
		{
			_07ED = Color.Black;
		}
		if (!ߧ)
		{
			ߦ = Color.White;
		}
		if (!_07F8)
		{
			_07F7 = Color.FromArgb(146, 143, 130);
		}
		if (!_07F0)
		{
			_07EF = Color.FromArgb(252, 252, 249);
		}
		if (!_07F2)
		{
			_07F1 = Color.FromArgb(138, 134, 122);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = Color.FromArgb(225, 230, 232);
		}
		if (!_0829)
		{
			ࠨ = Color.Empty;
		}
		if (!_082B)
		{
			_082A = Color.FromArgb(49, 106, 197);
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = Color.FromArgb(180, 177, 163);
		}
		if (!_0822)
		{
			_0821 = Color.Empty;
		}
		if (!_081B)
		{
			ࠚ = Color.FromArgb(251, 250, 247);
		}
		if (!_081D)
		{
			_081C = Color.Empty;
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = Color.FromArgb(193, 210, 238);
		}
		if (!ࠇ)
		{
			ࠆ = Color.Empty;
		}
		if (!ࠌ)
		{
			ࠋ = Color.FromArgb(49, 106, 197);
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = Color.FromArgb(152, 181, 226);
		}
		if (!ࠐ)
		{
			ࠏ = Color.Empty;
		}
		if (!ࠕ)
		{
			ࠔ = Color.FromArgb(75, 75, 111);
		}
		if (!ࠓ)
		{
			ࠒ = SystemColors.ControlText;
		}
		if (!_0817)
		{
			_0816 = Color.FromArgb(197, 194, 184);
		}
		if (!_0819)
		{
			_0818 = Color.Empty;
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = Color.FromArgb(252, 252, 249);
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!ߘ)
		{
			ߗ = Color.FromArgb(229, 229, 215);
		}
		if (!_082F)
		{
			_082E = Color.FromArgb(138, 134, 122);
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = _0830;
		}
		if (!_0838)
		{
			_0837 = Color.FromArgb(186, 186, 160);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = _0830;
		}
		if (!_083F)
		{
			_083E = ControlPaint.Light(Color.FromArgb(186, 186, 160));
		}
		if (!ࡂ)
		{
			ࡁ = Color.Black;
		}
		if (!ߴ)
		{
			_07F3 = Color.FromArgb(146, 146, 118);
		}
		if (!ࡣ)
		{
			ࡢ = Color.FromArgb(229, 229, 215);
		}
		if (!ࡥ)
		{
			ࡤ = Color.FromArgb(250, 250, 247);
		}
		if (!ࡄ)
		{
			ࡃ = Color.FromArgb(239, 238, 235);
		}
		if (!ࡆ)
		{
			ࡅ = Color.FromArgb(152, 152, 126);
		}
		if (!ࡉ)
		{
			ࡈ = SystemColors.ControlText;
		}
		if (!ࡋ)
		{
			ࡊ = GetColor(12566434);
		}
		if (!ࡍ)
		{
			ࡌ = Color.FromArgb(152, 152, 126);
		}
		if (!ࡐ)
		{
			ࡏ = Color.Black;
		}
		if (!ࡒ)
		{
			ࡑ = Color.FromArgb(82, 82, 79);
		}
		if (!ࡔ)
		{
			ࡓ = GetColor(12566434);
		}
		if (!ࡖ)
		{
			ࡕ = Color.FromArgb(152, 152, 126);
		}
		if (!_0859)
		{
			ࡘ = Color.Black;
		}
		if (!_085B)
		{
			_085A = Color.FromArgb(82, 82, 79);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(123, 162, 231);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(99, 117, 214);
		}
	}

	private void _0880()
	{
		if (!ߝ)
		{
			ߜ = GetColor(16448246);
		}
		if (!ߟ)
		{
			ߞ = GetColor(15592155);
		}
		if (!_07F6)
		{
			ߵ = GetColor(12697267);
		}
		if (!ߢ)
		{
			ߡ = GetColor(9150825);
		}
		if (!ߤ)
		{
			ߣ = GetColor(12964257);
		}
		if (!ߩ)
		{
			ߨ = GetColor(13420474);
		}
		if (!_07EB)
		{
			ߪ = Color.Empty;
		}
		if (!_07EE)
		{
			_07ED = Color.Black;
		}
		if (!ߧ)
		{
			ߦ = Color.White;
		}
		if (!_07F8)
		{
			_07F7 = GetColor(9604994);
		}
		if (!_07F0)
		{
			_07EF = GetColor(16579833);
		}
		if (!_07F2)
		{
			_07F1 = GetColor(9078394);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = GetColor(11978381);
		}
		if (!_0829)
		{
			ࠨ = Color.Empty;
		}
		if (!_082B)
		{
			_082A = GetColor(9674864);
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = GetColor(11841955);
		}
		if (!_0822)
		{
			_0821 = Color.Empty;
		}
		if (!_081B)
		{
			ࠚ = GetColor(16382452);
		}
		if (!_081D)
		{
			_081C = Color.Empty;
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = GetColor(11978381);
		}
		if (!ࠇ)
		{
			ࠆ = Color.Empty;
		}
		if (!ࠌ)
		{
			ࠋ = GetColor(9674864);
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = GetColor(9674864);
		}
		if (!ࠐ)
		{
			ࠏ = Color.Empty;
		}
		if (!ࠕ)
		{
			ࠔ = GetColor(9674864);
		}
		if (!ࠓ)
		{
			ࠒ = Color.White;
		}
		if (!_0817)
		{
			_0816 = GetColor(12960440);
		}
		if (!_0819)
		{
			_0818 = Color.Empty;
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = GetColor(16579833);
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!ߘ)
		{
			ߗ = GetColor(15526360);
		}
		if (!_082F)
		{
			_082E = GetColor(9078394);
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = GetColor(16579833);
		}
		if (!_0838)
		{
			_0837 = GetColor(15592155);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = _0835;
		}
		if (!_083F)
		{
			_083E = ControlPaint.Light(_0837);
		}
		if (!ࡂ)
		{
			ࡁ = Color.Black;
		}
		if (!ߴ)
		{
			_07F3 = GetColor(15723998);
		}
		if (!ࡣ)
		{
			ࡢ = GetColor(15526360);
		}
		if (!ࡥ)
		{
			ࡤ = GetColor(16447990);
		}
		if (!ࡄ)
		{
			ࡃ = GetColor(15724267);
		}
		if (!ࡆ)
		{
			ࡅ = GetColor(11315353);
		}
		if (!ࡉ)
		{
			ࡈ = SystemColors.ControlText;
		}
		if (!ࡋ)
		{
			ࡊ = GetColor(15987434);
		}
		if (!ࡍ)
		{
			ࡌ = GetColor(15000277);
		}
		if (!ࡐ)
		{
			ࡏ = Color.Black;
		}
		if (!ࡒ)
		{
			ࡑ = GetColor(11315353);
		}
		if (!ࡔ)
		{
			ࡓ = GetColor(15987434);
		}
		if (!ࡖ)
		{
			ࡕ = GetColor(15000277);
		}
		if (!_0859)
		{
			ࡘ = Color.Black;
		}
		if (!_085B)
		{
			_085A = GetColor(11315353);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(204, 217, 173);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(165, 189, 132);
		}
	}

	private void _0881()
	{
		if (!ߝ)
		{
			ߜ = GetColor(15987962);
		}
		if (!ߟ)
		{
			ߞ = GetColor(10065845);
		}
		if (!_07F6)
		{
			ߵ = GetColor(5526645);
		}
		if (!ߢ)
		{
			ߡ = GetColor(10526394);
		}
		if (!ߤ)
		{
			ߣ = GetColor(14803692);
		}
		if (!ߩ)
		{
			ߨ = GetColor(13553358);
		}
		if (!_07EB)
		{
			ߪ = GetColor(15921910);
		}
		if (!_07EE)
		{
			_07ED = Color.Black;
		}
		if (!ߧ)
		{
			ߦ = Color.Black;
		}
		if (!_07F8)
		{
			_07F7 = GetColor(8026521);
		}
		if (!_07F0)
		{
			_07EF = GetColor(16644863);
		}
		if (!_07F2)
		{
			_07F1 = GetColor(8158356);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = GetColor(16766348);
		}
		if (!_0829)
		{
			ࠨ = GetColor(16756053);
		}
		if (!_082B)
		{
			_082A = GetColor(4934511);
		}
		if (!_082D)
		{
			_082C = SystemColors.ControlText;
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = GetColor(9276813);
		}
		if (!_0822)
		{
			_0821 = Color.Empty;
		}
		if (!_081B)
		{
			ࠚ = GetColor(15264241);
		}
		if (!_081D)
		{
			_081C = GetColor(12237261);
		}
		if (!_0820)
		{
			_081F = SystemColors.ControlText;
		}
		if (!ࠅ)
		{
			ࠄ = GetColor(16774348);
		}
		if (!ࠇ)
		{
			ࠆ = GetColor(16765073);
		}
		if (!ࠌ)
		{
			ࠋ = GetColor(4934511);
		}
		if (!ࠊ)
		{
			ࠉ = SystemColors.ControlText;
		}
		if (!ࠎ)
		{
			ࠍ = GetColor(16683342);
		}
		if (!ࠐ)
		{
			ࠏ = GetColor(16765838);
		}
		if (!ࠕ)
		{
			ࠔ = GetColor(4934511);
		}
		if (!ࠓ)
		{
			ࠒ = Color.Black;
		}
		if (!_0817)
		{
			_0816 = GetColor(7237007);
		}
		if (!_0819)
		{
			_0818 = Color.Empty;
		}
		if (!_07FF)
		{
			_07FE = SystemColors.ControlText;
		}
		if (!_0831)
		{
			_0830 = GetColor(16644863);
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!ߘ)
		{
			ߗ = GetColor(14145509);
		}
		if (!_082F)
		{
			_082E = GetColor(8158356);
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = GetColor(16382463);
		}
		if (!_0838)
		{
			_0837 = GetColor(10460601);
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = _0835;
		}
		if (!_083F)
		{
			_083E = ControlPaint.Light(_0837);
		}
		if (!ࡂ)
		{
			ࡁ = Color.Black;
		}
		if (!ߴ)
		{
			_07F3 = GetColor(8158356);
		}
		if (!ࡣ)
		{
			ࡢ = GetColor(14145509);
		}
		if (!ࡥ)
		{
			ࡤ = GetColor(15987703);
		}
		if (!ࡄ)
		{
			ࡃ = GetColor(11776712);
		}
		if (!ࡆ)
		{
			ࡅ = GetColor(7960468);
		}
		if (!ࡉ)
		{
			ࡈ = SystemColors.ControlText;
		}
		if (!ࡋ)
		{
			ࡊ = GetColor(15658734);
		}
		if (!ࡍ)
		{
			ࡌ = GetColor(16777215);
		}
		if (!ࡐ)
		{
			ࡏ = Color.Black;
		}
		if (!ࡒ)
		{
			ࡑ = GetColor(10329505);
		}
		if (!ࡔ)
		{
			ࡓ = GetColor(15658734);
		}
		if (!ࡖ)
		{
			ࡕ = GetColor(16777215);
		}
		if (!_0859)
		{
			ࡘ = Color.Black;
		}
		if (!_085B)
		{
			_085A = GetColor(10329505);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(196, 200, 212);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(177, 179, 200);
		}
	}

	private void _0882()
	{
		if (!ߝ)
		{
			ߜ = GetColor(14938111);
		}
		if (!ߟ)
		{
			ߞ = GetColor(11128574);
		}
		if (!_07F6)
		{
			ߵ = GetColor(7314905);
		}
		if (!ߢ)
		{
			ߡ = GetColor(6656975);
		}
		if (!ߤ)
		{
			ߣ = GetColor(3630240);
		}
		if (!ߩ)
		{
			ߨ = GetColor(14938111);
		}
		if (!_07EB)
		{
			ߪ = GetColor(11522815);
		}
		if (!_07EE)
		{
			_07ED = GetColor(538482);
		}
		if (!ߧ)
		{
			ߦ = GetColor(16777215);
		}
		if (!_07F8)
		{
			_07F7 = GetColor(3630240);
		}
		if (!_07F0)
		{
			_07EF = GetColor(16185078);
		}
		if (!_07F2)
		{
			_07F1 = GetColor(6656975);
		}
		if (!ߺ)
		{
			_07F9 = Color.Empty;
		}
		if (!_07FC)
		{
			_07FB = Color.Empty;
		}
		if (!_0826)
		{
			_0825 = GetColor(16569720);
		}
		if (!_0829)
		{
			ࠨ = GetColor(16500815);
		}
		if (!_082B)
		{
			_082A = GetColor(12276995);
		}
		if (!_082D)
		{
			_082C = GetColor(0);
		}
		if (!ࠁ)
		{
			ࠀ = Color.Empty;
		}
		if (!ࠃ)
		{
			ࠂ = GetColor(9276813);
		}
		if (!_0822)
		{
			_0821 = Color.Empty;
		}
		if (!_081B)
		{
			ࠚ = GetColor(14938110);
		}
		if (!_081D)
		{
			_081C = GetColor(10076144);
		}
		if (!_0820)
		{
			_081F = GetColor(0);
		}
		if (!ࠅ)
		{
			ࠄ = GetColor(16774604);
		}
		if (!ࠇ)
		{
			ࠆ = GetColor(16767861);
		}
		if (!ࠌ)
		{
			ࠋ = GetColor(16760169);
		}
		if (!ࠊ)
		{
			ࠉ = GetColor(0);
		}
		if (!ࠎ)
		{
			ࠍ = GetColor(16553789);
		}
		if (!ࠐ)
		{
			ࠏ = GetColor(16758878);
		}
		if (!ࠕ)
		{
			ࠔ = GetColor(16485436);
		}
		if (!ࠓ)
		{
			ࠒ = GetColor(0);
		}
		if (!_0817)
		{
			_0816 = Color.FromArgb(80, GetColor(10143487));
		}
		if (!_0819)
		{
			_0818 = Color.FromArgb(250, GetColor(16777215));
		}
		if (!_07FF)
		{
			_07FE = GetColor(0);
		}
		if (!_0831)
		{
			_0830 = GetColor(16185078);
		}
		if (!_0833)
		{
			_0832 = Color.Empty;
		}
		if (!ߘ)
		{
			ߗ = GetColor(12573695);
		}
		if (!_082F)
		{
			_082E = GetColor(6656975);
		}
		if (!ࠤ)
		{
			_0823 = _082E;
		}
		if (!_0836)
		{
			_0835 = GetColor(15331054);
		}
		if (!_0838)
		{
			_0837 = Color.Empty;
		}
		if (!_083B)
		{
			_083A = _0830;
		}
		if (!_083D)
		{
			_083C = GetColor(14342874);
		}
		if (!_083F)
		{
			_083E = Color.Empty;
		}
		if (!ࡂ)
		{
			ࡁ = Color.Black;
		}
		if (!ߴ)
		{
			_07F3 = GetColor(7314905);
		}
		if (!ࡣ)
		{
			ࡢ = GetColor(12573695);
		}
		if (!ࡥ)
		{
			ࡤ = Color.Empty;
		}
		if (!ࡄ)
		{
			ࡃ = GetColor(14149887);
		}
		if (!ࡆ)
		{
			ࡅ = GetColor(7314905);
		}
		if (!ࡉ)
		{
			ࡈ = GetColor(0);
		}
		if (!ࡋ)
		{
			ࡊ = GetColor(14938111);
		}
		if (!ࡍ)
		{
			ࡌ = GetColor(11522815);
		}
		if (!ࡐ)
		{
			ࡏ = GetColor(538482);
		}
		if (!ࡒ)
		{
			ࡑ = GetColor(6656975);
		}
		if (!ࡔ)
		{
			ࡓ = GetColor(14938111);
		}
		if (!ࡖ)
		{
			ࡕ = GetColor(11522815);
		}
		if (!_0859)
		{
			ࡘ = GetColor(538482);
		}
		if (!_085B)
		{
			_085A = GetColor(6656975);
		}
		if (!_085D)
		{
			_085C = Color.FromArgb(196, 200, 212);
		}
		if (!_085F)
		{
			_085E = Color.FromArgb(177, 179, 200);
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
		if (ߝ)
		{
			xmlElem.SetAttribute("barback", BarFunctions.ColorToString(ߜ));
		}
		if (ߟ)
		{
			xmlElem.SetAttribute("barback2", BarFunctions.ColorToString(ߞ));
		}
		if (ߠ != 90)
		{
			xmlElem.SetAttribute("barbackga", ߠ.ToString());
		}
		if (_07F6)
		{
			xmlElem.SetAttribute("barstripeclr", BarFunctions.ColorToString(ߵ));
		}
		if (ߢ)
		{
			xmlElem.SetAttribute("barcapback", BarFunctions.ColorToString(ߡ));
		}
		if (ߤ)
		{
			xmlElem.SetAttribute("barcapback2", BarFunctions.ColorToString(ߣ));
		}
		if (ߥ != 90)
		{
			xmlElem.SetAttribute("barcapbackga", ߥ.ToString());
		}
		if (ߩ)
		{
			xmlElem.SetAttribute("barcapiback", BarFunctions.ColorToString(ߨ));
		}
		if (_07EB)
		{
			xmlElem.SetAttribute("barcapiback2", BarFunctions.ColorToString(ߪ));
		}
		if (_07EC != 90)
		{
			xmlElem.SetAttribute("barcapibackga", _07EC.ToString());
		}
		if (ߴ)
		{
			xmlElem.SetAttribute("bardockborder", BarFunctions.ColorToString(_07F3));
		}
		if (_07EE)
		{
			xmlElem.SetAttribute("barcapitext", BarFunctions.ColorToString(_07ED));
		}
		if (ߧ)
		{
			xmlElem.SetAttribute("barcaptext", BarFunctions.ColorToString(ߦ));
		}
		if (_07F8)
		{
			xmlElem.SetAttribute("barfloatb", BarFunctions.ColorToString(_07F7));
		}
		if (_07F0)
		{
			xmlElem.SetAttribute("barpopupback", BarFunctions.ColorToString(_07EF));
		}
		if (_07F2)
		{
			xmlElem.SetAttribute("barpopupb", BarFunctions.ColorToString(_07F1));
		}
		if (ߺ)
		{
			xmlElem.SetAttribute("itemback", BarFunctions.ColorToString(_07F9));
		}
		if (_07FC)
		{
			xmlElem.SetAttribute("itemback2", BarFunctions.ColorToString(_07FB));
		}
		if (_07FD != 90)
		{
			xmlElem.SetAttribute("itembackga", _07FD.ToString());
		}
		if (_0826)
		{
			xmlElem.SetAttribute("itemchkback", BarFunctions.ColorToString(_0825));
		}
		if (_0829)
		{
			xmlElem.SetAttribute("itemchkback2", BarFunctions.ColorToString(ࠨ));
		}
		if (_0827 != 90)
		{
			xmlElem.SetAttribute("itemchkbackga", _0827.ToString());
		}
		if (_082B)
		{
			xmlElem.SetAttribute("itemchkb", BarFunctions.ColorToString(_082A));
		}
		if (_082D)
		{
			xmlElem.SetAttribute("itemchktext", BarFunctions.ColorToString(_082C));
		}
		if (ࠁ && !ࠀ.IsEmpty)
		{
			xmlElem.SetAttribute("itemdisback", BarFunctions.ColorToString(ࠀ));
		}
		if (ࠃ)
		{
			xmlElem.SetAttribute("itemdistext", BarFunctions.ColorToString(ࠂ));
		}
		if (_0822)
		{
			xmlElem.SetAttribute("itemexpshadow", BarFunctions.ColorToString(_0821));
		}
		if (_081B)
		{
			xmlElem.SetAttribute("itemexpback", BarFunctions.ColorToString(ࠚ));
		}
		if (_081D)
		{
			xmlElem.SetAttribute("itemexpback2", BarFunctions.ColorToString(_081C));
		}
		if (_081E != 90)
		{
			xmlElem.SetAttribute("itemexpbackga", _081E.ToString());
		}
		if (_0820)
		{
			xmlElem.SetAttribute("itemexptext", BarFunctions.ColorToString(_081F));
		}
		if (ࠤ)
		{
			xmlElem.SetAttribute("itemexpborder", BarFunctions.ColorToString(_0823));
		}
		if (ࠅ)
		{
			xmlElem.SetAttribute("itemhotback", BarFunctions.ColorToString(ࠄ));
		}
		if (ࠇ)
		{
			xmlElem.SetAttribute("itemhotback2", BarFunctions.ColorToString(ࠆ));
		}
		if (ࠈ != 90)
		{
			xmlElem.SetAttribute("itemhotbackga", ࠈ.ToString());
		}
		if (ࠌ)
		{
			xmlElem.SetAttribute("itemhotb", BarFunctions.ColorToString(ࠋ));
		}
		if (ࠊ)
		{
			xmlElem.SetAttribute("itemhottext", BarFunctions.ColorToString(ࠉ));
		}
		if (ࠎ)
		{
			xmlElem.SetAttribute("itempressback", BarFunctions.ColorToString(ࠍ));
		}
		if (ࠐ)
		{
			xmlElem.SetAttribute("itempressback2", BarFunctions.ColorToString(ࠏ));
		}
		if (ࠑ != 90)
		{
			xmlElem.SetAttribute("itempressbackga", ࠑ.ToString());
		}
		if (ࠕ)
		{
			xmlElem.SetAttribute("itempressb", BarFunctions.ColorToString(ࠔ));
		}
		if (ࠓ)
		{
			xmlElem.SetAttribute("itempresstext", BarFunctions.ColorToString(ࠒ));
		}
		if (_0817)
		{
			xmlElem.SetAttribute("itemsep", BarFunctions.ColorToString(_0816));
		}
		if (_07FF)
		{
			xmlElem.SetAttribute("itemtext", BarFunctions.ColorToString(_07FE));
		}
		if (_0831)
		{
			xmlElem.SetAttribute("menuback", BarFunctions.ColorToString(_0830));
		}
		if (_0833)
		{
			xmlElem.SetAttribute("menuback2", BarFunctions.ColorToString(_0832));
		}
		if (_0834 != 0)
		{
			xmlElem.SetAttribute("menubackga", _0834.ToString());
		}
		if (ߘ)
		{
			xmlElem.SetAttribute("menubarback", BarFunctions.ColorToString(ߗ));
		}
		if (ߚ)
		{
			xmlElem.SetAttribute("menubarback2", BarFunctions.ColorToString(ߙ));
		}
		if (ߛ != 90)
		{
			xmlElem.SetAttribute("menubarbackga", ߛ.ToString());
		}
		if (_082F)
		{
			xmlElem.SetAttribute("menub", BarFunctions.ColorToString(_082E));
		}
		if (_0836)
		{
			xmlElem.SetAttribute("menuside", BarFunctions.ColorToString(_0835));
		}
		if (_0838)
		{
			xmlElem.SetAttribute("menuside2", BarFunctions.ColorToString(_0837));
		}
		if (_0839 != 0)
		{
			xmlElem.SetAttribute("menusidega", _0839.ToString());
		}
		if (_083B)
		{
			xmlElem.SetAttribute("menuuback", BarFunctions.ColorToString(_083A));
		}
		if (_083D)
		{
			xmlElem.SetAttribute("menuuside", BarFunctions.ColorToString(_083C));
		}
		if (_083F)
		{
			xmlElem.SetAttribute("menuuside2", BarFunctions.ColorToString(_083E));
		}
		if (_0839 != 0)
		{
			xmlElem.SetAttribute("menuusidega", _0839.ToString());
		}
		if (ࡂ)
		{
			xmlElem.SetAttribute("menudtb", BarFunctions.ColorToString(ࡁ));
		}
		if (ࡄ)
		{
			xmlElem.SetAttribute("customback", BarFunctions.ColorToString(ࡃ));
		}
		if (ࡆ)
		{
			xmlElem.SetAttribute("customback2", BarFunctions.ColorToString(ࡅ));
		}
		if (ࡉ)
		{
			xmlElem.SetAttribute("customtext", BarFunctions.ColorToString(ࡈ));
		}
		if (ࡇ != 90)
		{
			xmlElem.SetAttribute("custombackga", ࡇ.ToString());
		}
		if (ࡋ)
		{
			xmlElem.SetAttribute("panelback", BarFunctions.ColorToString(ࡊ));
		}
		if (ࡍ)
		{
			xmlElem.SetAttribute("panelback2", BarFunctions.ColorToString(ࡌ));
		}
		if (ࡒ)
		{
			xmlElem.SetAttribute("panelborder", BarFunctions.ColorToString(ࡑ));
		}
		if (ࡐ)
		{
			xmlElem.SetAttribute("paneltext", BarFunctions.ColorToString(ࡏ));
		}
		if (ࡎ != 90)
		{
			xmlElem.SetAttribute("panelbackga", ࡎ.ToString());
		}
		if (ࡔ)
		{
			xmlElem.SetAttribute("splitterback", BarFunctions.ColorToString(ࡓ));
		}
		if (ࡖ)
		{
			xmlElem.SetAttribute("splitterback2", BarFunctions.ColorToString(ࡕ));
		}
		if (_085B)
		{
			xmlElem.SetAttribute("splitterborder", BarFunctions.ColorToString(_085A));
		}
		if (_0859)
		{
			xmlElem.SetAttribute("splittertext", BarFunctions.ColorToString(ࡘ));
		}
		if (ࡗ != 90)
		{
			xmlElem.SetAttribute("splitterbackga", ࡗ.ToString());
		}
		if (_085D)
		{
			xmlElem.SetAttribute("exbarback", BarFunctions.ColorToString(_085C));
		}
		if (_085F)
		{
			xmlElem.SetAttribute("exbarback2", BarFunctions.ColorToString(_085E));
		}
		if (ࡠ != 90)
		{
			xmlElem.SetAttribute("exbarbackga", ࡠ.ToString());
		}
		if (ࡣ)
		{
			xmlElem.SetAttribute("dsback", BarFunctions.ColorToString(ࡢ));
		}
		if (ࡥ)
		{
			xmlElem.SetAttribute("dsback2", BarFunctions.ColorToString(ࡤ));
		}
		if (ࡦ != 0)
		{
			xmlElem.SetAttribute("dsbackga", ࡦ.ToString());
		}
		if (!ࡨ.IsEmpty)
		{
			xmlElem.SetAttribute("mdisystemitemforeground", BarFunctions.ColorToString(ࡨ));
		}
		if (ࡧ != 0)
		{
			xmlElem.SetAttribute("predefcolorscheme", XmlConvert.ToString((int)ࡧ));
		}
	}

	public void Deserialize(XmlElement xmlElem)
	{
		if (xmlElem.HasAttribute("predefcolorscheme"))
		{
			ࡧ = (ePredefinedColorScheme)XmlConvert.ToInt32(xmlElem.GetAttribute("predefcolorscheme"));
		}
		Refresh();
		if (xmlElem.HasAttribute("barback"))
		{
			ߜ = BarFunctions.ColorFromString(xmlElem.GetAttribute("barback"));
			ߝ = true;
		}
		if (xmlElem.HasAttribute("barback2"))
		{
			ߞ = BarFunctions.ColorFromString(xmlElem.GetAttribute("barback2"));
			ߟ = true;
		}
		if (xmlElem.HasAttribute("barbackga"))
		{
			ߠ = XmlConvert.ToInt32(xmlElem.GetAttribute("barbackga"));
		}
		if (xmlElem.HasAttribute("barstripeclr"))
		{
			ߵ = BarFunctions.ColorFromString(xmlElem.GetAttribute("barstripeclr"));
			_07F6 = true;
		}
		if (xmlElem.HasAttribute("barcapback"))
		{
			ߡ = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapback"));
			ߢ = true;
		}
		if (xmlElem.HasAttribute("barcapback2"))
		{
			ߣ = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapback2"));
			ߤ = true;
		}
		if (xmlElem.HasAttribute("barcapbackga"))
		{
			ߥ = XmlConvert.ToInt32(xmlElem.GetAttribute("barcapbackga"));
		}
		if (xmlElem.HasAttribute("barcapiback"))
		{
			ߨ = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapiback"));
			ߩ = true;
		}
		if (xmlElem.HasAttribute("barcapiback2"))
		{
			ߪ = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapiback2"));
			_07EB = true;
		}
		if (xmlElem.HasAttribute("barcapibackga"))
		{
			_07EC = XmlConvert.ToInt32(xmlElem.GetAttribute("barcapibackga"));
		}
		if (xmlElem.HasAttribute("barcapitext"))
		{
			_07ED = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcapitext"));
			_07EE = true;
		}
		if (xmlElem.HasAttribute("barcaptext"))
		{
			ߦ = BarFunctions.ColorFromString(xmlElem.GetAttribute("barcaptext"));
			ߧ = true;
		}
		if (xmlElem.HasAttribute("barfloatb"))
		{
			_07F7 = BarFunctions.ColorFromString(xmlElem.GetAttribute("barfloatb"));
			_07F8 = true;
		}
		if (xmlElem.HasAttribute("bardockborder"))
		{
			_07F3 = BarFunctions.ColorFromString(xmlElem.GetAttribute("bardockborder"));
			ߴ = true;
		}
		if (xmlElem.HasAttribute("barpopupback"))
		{
			_07EF = BarFunctions.ColorFromString(xmlElem.GetAttribute("barpopupback"));
			_07F0 = true;
		}
		if (xmlElem.HasAttribute("barpopupb"))
		{
			_07F1 = BarFunctions.ColorFromString(xmlElem.GetAttribute("barpopupb"));
			_07F2 = true;
		}
		if (xmlElem.HasAttribute("itemback"))
		{
			_07F9 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemback"));
			ߺ = true;
		}
		if (xmlElem.HasAttribute("itemback2"))
		{
			_07FB = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemback2"));
			_07FC = true;
		}
		if (xmlElem.HasAttribute("itembackga"))
		{
			_07FD = XmlConvert.ToInt32(xmlElem.GetAttribute("itembackga"));
		}
		if (xmlElem.HasAttribute("itemchkback"))
		{
			_0825 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemchkback"));
			_0826 = true;
		}
		if (xmlElem.HasAttribute("itemchkback2"))
		{
			ࠨ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemchkback2"));
			_0829 = true;
		}
		if (xmlElem.HasAttribute("itemchkbackga"))
		{
			_0827 = XmlConvert.ToInt32(xmlElem.GetAttribute("itemchkbackga"));
		}
		if (xmlElem.HasAttribute("itemchkb"))
		{
			_082A = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemchkb"));
			_082B = true;
		}
		if (xmlElem.HasAttribute("itemchktext"))
		{
			_082C = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemchktext"));
			_082D = true;
		}
		if (xmlElem.HasAttribute("itemdisback"))
		{
			ࠀ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemdisback"));
			ࠁ = true;
		}
		if (xmlElem.HasAttribute("itemdistext"))
		{
			ࠂ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemdistext"));
			ࠃ = true;
		}
		if (xmlElem.HasAttribute("itemexpshadow"))
		{
			_0821 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexpshadow"));
			_0822 = true;
		}
		if (xmlElem.HasAttribute("itemexpback"))
		{
			ࠚ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexpback"));
			_081B = true;
		}
		if (xmlElem.HasAttribute("itemexpback2"))
		{
			_081C = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexpback2"));
			_081D = true;
		}
		if (xmlElem.HasAttribute("itemexpbackga"))
		{
			_081E = XmlConvert.ToInt32(xmlElem.GetAttribute("itemexpbackga"));
		}
		if (xmlElem.HasAttribute("itemexptext"))
		{
			_081F = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexptext"));
			_0820 = true;
		}
		if (xmlElem.HasAttribute("itemexpborder"))
		{
			_0823 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemexpborder"));
			ࠤ = true;
		}
		if (xmlElem.HasAttribute("itemhotback"))
		{
			ࠄ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemhotback"));
			ࠅ = true;
		}
		if (xmlElem.HasAttribute("itemhotback2"))
		{
			ࠆ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemhotback2"));
			ࠇ = true;
		}
		if (xmlElem.HasAttribute("itemhotbackga"))
		{
			ࠈ = XmlConvert.ToInt32(xmlElem.GetAttribute("itemhotbackga"));
		}
		if (xmlElem.HasAttribute("itemhotb"))
		{
			ࠋ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemhotb"));
			ࠌ = true;
		}
		if (xmlElem.HasAttribute("itemhottext"))
		{
			ࠉ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemhottext"));
			ࠊ = true;
		}
		if (xmlElem.HasAttribute("itempressback"))
		{
			ࠍ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itempressback"));
			ࠎ = true;
		}
		if (xmlElem.HasAttribute("itempressback2"))
		{
			ࠏ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itempressback2"));
			ࠐ = true;
		}
		if (xmlElem.HasAttribute("itempressbackga"))
		{
			ࠑ = XmlConvert.ToInt32(xmlElem.GetAttribute("itempressbackga"));
		}
		if (xmlElem.HasAttribute("itempressb"))
		{
			ࠔ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itempressb"));
			ࠕ = true;
		}
		if (xmlElem.HasAttribute("itempresstext"))
		{
			ࠒ = BarFunctions.ColorFromString(xmlElem.GetAttribute("itempresstext"));
			ࠓ = true;
		}
		if (xmlElem.HasAttribute("itemsep"))
		{
			_0816 = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemsep"));
			_0817 = true;
		}
		if (xmlElem.HasAttribute("itemtext"))
		{
			_07FE = BarFunctions.ColorFromString(xmlElem.GetAttribute("itemtext"));
			_07FF = true;
		}
		if (xmlElem.HasAttribute("menuback"))
		{
			_0830 = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuback"));
			_0831 = true;
		}
		if (xmlElem.HasAttribute("menuback2"))
		{
			_0832 = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuback2"));
			_0833 = true;
		}
		if (xmlElem.HasAttribute("menubackga"))
		{
			_0834 = XmlConvert.ToInt32(xmlElem.GetAttribute("menubackga"));
		}
		if (xmlElem.HasAttribute("menubarback"))
		{
			ߗ = BarFunctions.ColorFromString(xmlElem.GetAttribute("menubarback"));
			ߘ = true;
		}
		if (xmlElem.HasAttribute("menubarback2"))
		{
			ߙ = BarFunctions.ColorFromString(xmlElem.GetAttribute("menubarback2"));
			ߚ = true;
		}
		if (xmlElem.HasAttribute("menubarbackga"))
		{
			ߛ = XmlConvert.ToInt32(xmlElem.GetAttribute("menubarbackga"));
		}
		else
		{
			ߛ = 90;
		}
		if (xmlElem.HasAttribute("menub"))
		{
			_082E = BarFunctions.ColorFromString(xmlElem.GetAttribute("menub"));
			_082F = true;
		}
		if (xmlElem.HasAttribute("menuside"))
		{
			_0835 = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuside"));
			_0836 = true;
		}
		if (xmlElem.HasAttribute("menuside2"))
		{
			_0837 = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuside2"));
			_0838 = true;
		}
		if (xmlElem.HasAttribute("menusidega"))
		{
			_0839 = XmlConvert.ToInt32(xmlElem.GetAttribute("menusidega"));
		}
		if (xmlElem.HasAttribute("menuuback"))
		{
			_083A = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuuback"));
			_083B = true;
		}
		if (xmlElem.HasAttribute("menuuside"))
		{
			_083C = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuuside"));
			_083D = true;
		}
		if (xmlElem.HasAttribute("menuuside2"))
		{
			_083E = BarFunctions.ColorFromString(xmlElem.GetAttribute("menuuside2"));
			_083F = true;
		}
		if (xmlElem.HasAttribute("menuusidega"))
		{
			ࡀ = XmlConvert.ToInt32(xmlElem.GetAttribute("menuusidega"));
		}
		if (xmlElem.HasAttribute("menudtb"))
		{
			ࡁ = BarFunctions.ColorFromString(xmlElem.GetAttribute("menudtb"));
			ࡂ = true;
		}
		if (xmlElem.HasAttribute("customback"))
		{
			ࡃ = BarFunctions.ColorFromString(xmlElem.GetAttribute("customback"));
			ࡄ = true;
		}
		if (xmlElem.HasAttribute("customback2"))
		{
			ࡅ = BarFunctions.ColorFromString(xmlElem.GetAttribute("customback2"));
			ࡆ = true;
		}
		if (xmlElem.HasAttribute("customtext"))
		{
			ࡈ = BarFunctions.ColorFromString(xmlElem.GetAttribute("customtext"));
			ࡉ = true;
		}
		if (xmlElem.HasAttribute("custombackga"))
		{
			ࡇ = XmlConvert.ToInt32(xmlElem.GetAttribute("custombackga"));
		}
		if (xmlElem.HasAttribute("panelback"))
		{
			ࡋ = true;
			ࡊ = BarFunctions.ColorFromString(xmlElem.GetAttribute("panelback"));
		}
		if (xmlElem.HasAttribute("panelback2"))
		{
			ࡍ = true;
			ࡌ = BarFunctions.ColorFromString(xmlElem.GetAttribute("panelback2"));
		}
		if (xmlElem.HasAttribute("panelborder"))
		{
			ࡒ = true;
			ࡑ = BarFunctions.ColorFromString(xmlElem.GetAttribute("panelborder"));
		}
		if (xmlElem.HasAttribute("paneltext"))
		{
			ࡐ = true;
			ࡏ = BarFunctions.ColorFromString(xmlElem.GetAttribute("paneltext"));
		}
		if (xmlElem.HasAttribute("panelbackga"))
		{
			ࡎ = XmlConvert.ToInt32(xmlElem.GetAttribute("panelbackga"));
		}
		if (xmlElem.HasAttribute("splitterback"))
		{
			ࡔ = true;
			ࡓ = BarFunctions.ColorFromString(xmlElem.GetAttribute("Splitterback"));
		}
		if (xmlElem.HasAttribute("splitterback2"))
		{
			ࡖ = true;
			ࡕ = BarFunctions.ColorFromString(xmlElem.GetAttribute("Splitterback2"));
		}
		if (xmlElem.HasAttribute("splitterborder"))
		{
			_085B = true;
			_085A = BarFunctions.ColorFromString(xmlElem.GetAttribute("Splitterborder"));
		}
		if (xmlElem.HasAttribute("splittertext"))
		{
			_0859 = true;
			ࡘ = BarFunctions.ColorFromString(xmlElem.GetAttribute("Splittertext"));
		}
		if (xmlElem.HasAttribute("splitterbackga"))
		{
			ࡗ = XmlConvert.ToInt32(xmlElem.GetAttribute("Splitterbackga"));
		}
		if (xmlElem.HasAttribute("exbarback"))
		{
			_085D = true;
			_085C = BarFunctions.ColorFromString(xmlElem.GetAttribute("exbarback"));
		}
		if (xmlElem.HasAttribute("exbarback2"))
		{
			_085F = true;
			_085E = BarFunctions.ColorFromString(xmlElem.GetAttribute("exbarback2"));
		}
		if (xmlElem.HasAttribute("exbarbackga"))
		{
			ࡠ = XmlConvert.ToInt32(xmlElem.GetAttribute("exbarbackga"));
		}
		if (xmlElem.HasAttribute("dsback"))
		{
			ࡣ = true;
			ࡢ = BarFunctions.ColorFromString(xmlElem.GetAttribute("dsback"));
		}
		if (xmlElem.HasAttribute("dsback2"))
		{
			ࡥ = true;
			ࡤ = BarFunctions.ColorFromString(xmlElem.GetAttribute("dsback2"));
		}
		if (xmlElem.HasAttribute("dsbackga"))
		{
			ࡦ = XmlConvert.ToInt32(xmlElem.GetAttribute("dsbackga"));
		}
		if (xmlElem.HasAttribute("mdisystemitemforeground"))
		{
			ࡨ = BarFunctions.ColorFromString(xmlElem.GetAttribute("mdisystemitemforeground"));
		}
		else
		{
			ࡨ = Color.Empty;
		}
	}
}
