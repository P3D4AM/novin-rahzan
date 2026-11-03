using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Metro.Rendering;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

internal class ᣓ
{
	private static _192E ᣝ = new _192E();

	private static ᯊ ᣞ = new ᯊ();

	private static _2235 ᣟ = new _2235();

	private static _2206 ᣠ = new _2206();

	private static _24AD ᣡ = new _24AD();

	private static _24AF ᣢ = new _24AF();

	private static _24B3 ᣣ = new _24B3();

	private static _24B4 ᣤ = new _24B4();

	private static _21F8 ᣥ = new _21F8();

	private static _21EB ᣦ = new _21EB();

	private static ẓ ᣧ = new ẓ();

	private static ᣜ ᣨ = null;

	private static _24B8 ᣩ = new _24B8();

	private static _24BB ᣪ = new _24BB();

	private static _2211 ᣫ = new _2211();

	private static _20A9 ᣬ = new _20A9();

	private static ᯉ ᣭ = new ᯉ();

	private static ᢏ ᣮ = new ᢏ();

	private static _24BD ᣯ = new _24BD();

	private static _24BF ᣰ = new _24BF();

	private static _24C1 ᣱ = new _251D();

	private static _24DE ᣲ = new _2525();

	private static _24DF ᣳ = null;

	private static ᣒ ᣴ = null;

	private static _2212 _ᣵ = new _2212();

	private static _24E7 __18F6 = new _2526();

	private static _192F __18F7;

	private static _192F __18F8;

	private static _2209 __18F9 = new _2209();

	private static _220D __18FA = new _220D();

	private static ᣖ __18FB = null;

	private static _250D __18FC = null;

	private static _250D __18FD = null;

	private static _250D __18FE = null;

	private static _24B7 __18FF = null;

	private static ᣛ _ᤀ = null;

	private static ᣜ _ᤁ = null;

	private static _250E _ᤂ = null;

	private static _24A8 _ᤃ = null;

	private static _245F _ᤄ = null;

	private static _2511 _ᤅ = null;

	private static _2515 _ᤆ = null;

	private static _2516 _ᤇ = new _252E();

	public static _192F ᣔ
	{
		get
		{
			if (__18F7 == null)
			{
				__18F7 = new _2213();
			}
			return __18F7;
		}
	}

	public static _192F ᣕ
	{
		get
		{
			if (__18F8 == null)
			{
				__18F8 = new ᣕ();
			}
			return __18F8;
		}
	}

	private static ᣖ ᣖ
	{
		get
		{
			if (__18FB == null)
			{
				__18FB = new ᣖ();
			}
			return __18FB;
		}
	}

	private static _250D ᣗ
	{
		get
		{
			if (__18FC == null)
			{
				__18FC = new ᣗ();
			}
			return __18FC;
		}
	}

	private static _250D ᣘ
	{
		get
		{
			if (__18FD == null)
			{
				__18FD = new ᣘ();
			}
			return __18FD;
		}
	}

	private static _250D ᣙ
	{
		get
		{
			if (__18FE == null)
			{
				__18FE = new ᣙ();
			}
			return __18FE;
		}
	}

	private static _24B7 ᣚ
	{
		get
		{
			if (__18FF == null)
			{
				__18FF = new ᣚ();
			}
			return __18FF;
		}
	}

	private static _24B7 ᣛ
	{
		get
		{
			if (_ᤀ == null)
			{
				_ᤀ = new ᣛ();
			}
			return _ᤀ;
		}
	}

	private static _24B7 ᣜ
	{
		get
		{
			if (_ᤁ == null)
			{
				_ᤁ = new ᣜ();
			}
			return _ᤁ;
		}
	}

	public static _192F ᤈ(ButtonItem ᤉ)
	{
		if (ᤉ is RibbonTabItem)
		{
			return ᤌ((RibbonTabItem)ᤉ);
		}
		if (ᤉ is TabFormItem)
		{
			return ᤍ((TabFormItem)ᤉ);
		}
		if (ᤉ is NewTabFormItem)
		{
			return ᤎ((NewTabFormItem)ᤉ);
		}
		if (ᤉ is _220F)
		{
			_192F obj = ᤋ((_220F)ᤉ);
			if (obj != null)
			{
				return obj;
			}
		}
		eDotNetBarStyle effectiveStyle = ᤉ.EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2010 && ᤉ.ContainerState == eContainerState.NavigationPane)
		{
			return _ᣵ;
		}
		if (effectiveStyle == eDotNetBarStyle.Office2010 && ᤉ is ApplicationButton)
		{
			return ᣔ;
		}
		if (ᤉ is MetroAppButton || (ᤉ is ApplicationButton && StyleManager.IsMetro(effectiveStyle)))
		{
			return ᣕ;
		}
		if (BarFunctions.IsOffice2007Style(effectiveStyle))
		{
			if (StyleManager.IsMetro(effectiveStyle))
			{
				return ᣟ;
			}
			if (ᤉ.ContainerControl is RibbonBar)
			{
				return ᣞ;
			}
			return ᣞ;
		}
		return ᣝ;
	}

	public static _24DF ᤊ(ButtonItem ᤉ)
	{
		if (ᣳ == null)
		{
			ᣳ = new _24DF();
		}
		return ᣳ;
	}

	public static _192F ᤋ(_220F ᤉ)
	{
		if (BarFunctions.IsOffice2007Style(ᤉ.EffectiveStyle))
		{
			return ᣫ;
		}
		return null;
	}

	public static _192F ᤌ(RibbonTabItem ᆰ)
	{
		if (BarFunctions.IsOffice2007Style(ᆰ.EffectiveStyle) && !ᆰ.IsOnMenu)
		{
			return ᣦ;
		}
		if ((ᆰ.EffectiveStyle == eDotNetBarStyle.Office2003 || ᆰ.EffectiveStyle == eDotNetBarStyle.VS2005) && !ᆰ.IsOnMenu)
		{
			return ᣠ;
		}
		return ᣝ;
	}

	public static _192F ᤍ(TabFormItem ᆰ)
	{
		return __18F9;
	}

	public static _192F ᤎ(NewTabFormItem ᆰ)
	{
		return __18FA;
	}

	public static _24AE ᤏ(ItemContainer ઇ)
	{
		if (BarFunctions.IsOffice2007Style(ઇ.EffectiveStyle))
		{
			return ᣡ;
		}
		return null;
	}

	public static _24B2 ᤐ(Bar ච)
	{
		return ᣢ;
	}

	public static _၁ ᤑ()
	{
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			return ᣖ;
		}
		return ᣣ;
	}

	public static _24B5 ᤒ(RibbonBar ᤓ)
	{
		if (BarFunctions.IsOffice2007Style(ᤓ._൨))
		{
			return ᣤ;
		}
		return null;
	}

	public static _250D ᤔ(eDotNetBarStyle _0657)
	{
		if (_0657 == eDotNetBarStyle.Office2010)
		{
			return ᣗ;
		}
		if (StyleManager.IsMetro(_0657))
		{
			return ᣙ;
		}
		return ᣘ;
	}

	public static Ẓ ᤕ(ColorItem _06EC)
	{
		return ᣧ;
	}

	public static _2205 ᤖ(RibbonControl ल)
	{
		if (BarFunctions.IsOffice2007Style(ल.EffectiveStyle))
		{
			return ᣥ;
		}
		return null;
	}

	public static _24B7 ᤗ(SystemCaptionItem _06EC)
	{
		eDotNetBarStyle effectiveStyle = _06EC.EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2010)
		{
			return ᣛ;
		}
		if (StyleManager.IsMetro(effectiveStyle))
		{
			return ᣚ;
		}
		if (BarFunctions.IsOffice2007Style(effectiveStyle))
		{
			return ᣜ;
		}
		return null;
	}

	public static _24BA ᤘ(MDISystemItem ᤙ)
	{
		if (BarFunctions.IsOffice2007Style(ᤙ.EffectiveStyle))
		{
			return ᣩ;
		}
		return null;
	}

	public static _24BC ᤚ(Form ᕧ)
	{
		return ᣪ;
	}

	public static _20A8 ᤛ(QatOverflowItem ᤜ)
	{
		return ᣬ;
	}

	public static ᯈ ᤝ(QatCustomizeItem ᤞ)
	{
		return ᣭ;
	}

	public static ᢏ _191F(CheckBoxItem _06EC)
	{
		return ᣮ;
	}

	public static void _1920()
	{
	}

	public static _24BE _1921(ProgressBarItem _1922)
	{
		return ᣯ;
	}

	internal static _24C0 _1923()
	{
		return ᣰ;
	}

	internal static _24C1 _1924(SliderItem _1925)
	{
		return ᣱ;
	}

	internal static _24E7 _1926(RangeSliderItem _1925)
	{
		return __18F6;
	}

	internal static _24DE _1927()
	{
		return ᣲ;
	}

	internal static ᣒ _1928(SwitchButtonItem _06EC)
	{
		if (StyleManager.IsMetro(_06EC.EffectiveStyle))
		{
			if (_ᤂ == null)
			{
				_ᤂ = new _250E();
			}
			return _ᤂ;
		}
		if (ᣴ == null)
		{
			ᣴ = new _250F();
		}
		return ᣴ;
	}

	internal static _24A7 _1929(StepItem _06EC)
	{
		if (_ᤃ == null)
		{
			_ᤃ = new _24A8();
		}
		return _ᤃ;
	}

	internal static _245F _192A(ListBoxItem _06EC)
	{
		if (_ᤄ == null)
		{
			_ᤄ = new _245F();
			_ᤄ._2460 = ᣮ;
		}
		return _ᤄ;
	}

	internal static _2511 _192B(SideNavItem _06EC)
	{
		if (_ᤅ == null)
		{
			_ᤅ = new _2511();
		}
		return _ᤅ;
	}

	internal static _2515 _192C(TabFormStripControl _0BDE)
	{
		if (_ᤆ == null)
		{
			_ᤆ = new _2519();
		}
		return _ᤆ;
	}

	internal static _2516 _192D(TabParentForm ᕧ)
	{
		return _ᤇ;
	}
}
