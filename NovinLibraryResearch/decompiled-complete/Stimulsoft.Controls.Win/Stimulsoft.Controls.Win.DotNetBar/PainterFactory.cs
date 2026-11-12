using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class PainterFactory
{
	private static Office2003ButtonItemPainter m_Office2003Painter = new Office2003ButtonItemPainter();

	private static Office2007ButtonItemPainter m_Office2007Painter = new Office2007ButtonItemPainter();

	private static MetroButtonItemPainter m_MetroButtonPainter = new MetroButtonItemPainter();

	private static Office2003RibbonTabItemPainter m_RibbonTabItemOffice2003Painter = new Office2003RibbonTabItemPainter();

	private static Office2007ItemContainerPainter m_Office2007ItemContainerPainter = new Office2007ItemContainerPainter();

	private static Office2007BarBackgroundPainter m_Office2007BarBackgroundPainter = new Office2007BarBackgroundPainter();

	private static Office2007KeyTipsPainter m_Office2007KeyTipsPainter = new Office2007KeyTipsPainter();

	private static Office2007DialogLauncherPainter m_Office2007RibbonBarPainter = new Office2007DialogLauncherPainter();

	private static Office2007RibbonControlPainter m_Office2007RibbonControlPainter = new Office2007RibbonControlPainter();

	private static Office2007RibbonTabItemPainter m_Office2007RibbonTabItemPainter = new Office2007RibbonTabItemPainter();

	private static Office2007ColorItemPainter m_Office2007ColorItemPainter = new Office2007ColorItemPainter();

	private static Office2007MdiSystemItemPainter m_Office2007MdiSystemItemPainter = new Office2007MdiSystemItemPainter();

	private static Office2007FormCaptionPainter m_Office2007FormCaptionPainter = new Office2007FormCaptionPainter();

	private static Office2007RibbonOverflowPainter m_Office2007RibbonOverflowPainter = new Office2007RibbonOverflowPainter();

	private static Office2007QatOverflowPainter m_Office2007QatOverflowPainter = new Office2007QatOverflowPainter();

	private static Office2007QatCustomizeItemPainter m_Office2007QatCustomizePainter = new Office2007QatCustomizeItemPainter();

	private static Office2007CheckBoxItemPainter m_Office2007CheckBoxItemPainter = new Office2007CheckBoxItemPainter();

	private static Office2007ProgressBarItemPainter m_Office2007ProgressBarPainter = new Office2007ProgressBarItemPainter();

	private static Office2007NavigationPanePainter m_Office2007NavPanePainter = new Office2007NavigationPanePainter();

	private static SliderPainter m_SliderPainter = new Office2007SliderPainter();

	private static SideBarPainter m_SideBarPainter = new Office2007SideBarPainter();

	private static SwitchButtonPainter m_SwitchButtonPainter = null;

	private static Office2010NavPaneButtonPainter _Office2010NavPaneButtonPainter = new Office2010NavPaneButtonPainter();

	private static ButtonItemPainter _Office2010ApplicationButtonPainter;

	private static ButtonItemPainter _MetroAppButtonPainter;

	private static MetroKeyTipsPainter _MetroKeyTipsPainter = null;

	private static RibbonTabGroupPainter _Office2010RibbonTabGroupPainter = null;

	private static RibbonTabGroupPainter _Office2007RibbonTabGroupPainter = null;

	private static RibbonTabGroupPainter _MetroRibbonTabGroupPainter = null;

	private static SystemCaptionItemPainter _MetroCaptionItemPainter = null;

	private static Office2010SystemCaptionItemPainter _Office2010SystemCaptionItemPainter = null;

	private static Office2007SystemCaptionItemPainter _Office2007SystemCaptionItemPainter = null;

	private static MetroSwitchButtonPainter _MetroSwitchPainter = null;

	public static ButtonItemPainter Office2010ApplicationButtonPainter
	{
		get
		{
			if (_Office2010ApplicationButtonPainter == null)
			{
				_Office2010ApplicationButtonPainter = new Office2010AppButtonPainter();
			}
			return _Office2010ApplicationButtonPainter;
		}
	}

	public static ButtonItemPainter MetroAppButtonPainter
	{
		get
		{
			if (_MetroAppButtonPainter == null)
			{
				_MetroAppButtonPainter = new MetroAppButtonPainter();
			}
			return _MetroAppButtonPainter;
		}
	}

	private static MetroKeyTipsPainter MetroKeyTipsPainter
	{
		get
		{
			if (_MetroKeyTipsPainter == null)
			{
				_MetroKeyTipsPainter = new MetroKeyTipsPainter();
			}
			return _MetroKeyTipsPainter;
		}
	}

	private static RibbonTabGroupPainter Office2010RibbonTabGroupPainter
	{
		get
		{
			if (_Office2010RibbonTabGroupPainter == null)
			{
				_Office2010RibbonTabGroupPainter = new Office2010RibbonTabGroupPainter();
			}
			return _Office2010RibbonTabGroupPainter;
		}
	}

	private static RibbonTabGroupPainter Office2007RibbonTabGroupPainter
	{
		get
		{
			if (_Office2007RibbonTabGroupPainter == null)
			{
				_Office2007RibbonTabGroupPainter = new Office2007RibbonTabGroupPainter();
			}
			return _Office2007RibbonTabGroupPainter;
		}
	}

	private static RibbonTabGroupPainter MetroRibbonTabGroupPainter
	{
		get
		{
			if (_MetroRibbonTabGroupPainter == null)
			{
				_MetroRibbonTabGroupPainter = new MetroRibbonTabGroupPainter();
			}
			return _MetroRibbonTabGroupPainter;
		}
	}

	private static SystemCaptionItemPainter MetroCaptionItemPainter
	{
		get
		{
			if (_MetroCaptionItemPainter == null)
			{
				_MetroCaptionItemPainter = new MetroCaptionItemPainter();
			}
			return _MetroCaptionItemPainter;
		}
	}

	private static SystemCaptionItemPainter Office2010SystemCaptionItemPainter
	{
		get
		{
			if (_Office2010SystemCaptionItemPainter == null)
			{
				_Office2010SystemCaptionItemPainter = new Office2010SystemCaptionItemPainter();
			}
			return _Office2010SystemCaptionItemPainter;
		}
	}

	private static SystemCaptionItemPainter Office2007SystemCaptionItemPainter
	{
		get
		{
			if (_Office2007SystemCaptionItemPainter == null)
			{
				_Office2007SystemCaptionItemPainter = new Office2007SystemCaptionItemPainter();
			}
			return _Office2007SystemCaptionItemPainter;
		}
	}

	public static ButtonItemPainter CreateButtonPainter(ButtonItem button)
	{
		if (button is RibbonTabItem)
		{
			return CreateRibbonTabItemPainter((RibbonTabItem)button);
		}
		if (button is RibbonOverflowButtonItem)
		{
			ButtonItemPainter buttonItemPainter = CreateRibbonOverflowButtonPainter((RibbonOverflowButtonItem)button);
			if (buttonItemPainter != null)
			{
				return buttonItemPainter;
			}
		}
		eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2010 && button.ContainerState == eContainerState.NavigationPane)
		{
			return _Office2010NavPaneButtonPainter;
		}
		if (effectiveStyle == eDotNetBarStyle.Office2010 && button is ApplicationButton)
		{
			return Office2010ApplicationButtonPainter;
		}
		if (button is MetroAppButton || (button is ApplicationButton && effectiveStyle == eDotNetBarStyle.Metro))
		{
			return MetroAppButtonPainter;
		}
		if (BarFunctions.IsOffice2007Style(effectiveStyle))
		{
			if (effectiveStyle == eDotNetBarStyle.Metro)
			{
				return m_MetroButtonPainter;
			}
			_ = button.ContainerControl is RibbonBar;
			return m_Office2007Painter;
		}
		return m_Office2003Painter;
	}

	public static ButtonItemPainter CreateRibbonOverflowButtonPainter(RibbonOverflowButtonItem button)
	{
		if (BarFunctions.IsOffice2007Style(button.EffectiveStyle))
		{
			return m_Office2007RibbonOverflowPainter;
		}
		return null;
	}

	public static ButtonItemPainter CreateRibbonTabItemPainter(RibbonTabItem tab)
	{
		if (BarFunctions.IsOffice2007Style(tab.EffectiveStyle) && !tab.IsOnMenu)
		{
			return m_Office2007RibbonTabItemPainter;
		}
		if ((tab.EffectiveStyle == eDotNetBarStyle.Office2003 || tab.EffectiveStyle == eDotNetBarStyle.VS2005) && !tab.IsOnMenu)
		{
			return m_RibbonTabItemOffice2003Painter;
		}
		return m_Office2003Painter;
	}

	public static ItemContainerPainter CreateItemContainerPainter(ItemContainer container)
	{
		if (BarFunctions.IsOffice2007Style(container.EffectiveStyle))
		{
			return m_Office2007ItemContainerPainter;
		}
		return null;
	}

	public static BarBackgroundPainter CreateBarBackgroundPainter(Bar bar)
	{
		return m_Office2007BarBackgroundPainter;
	}

	public static KeyTipsPainter CreateKeyTipsPainter()
	{
		if (StyleManager.Style == eStyle.Metro)
		{
			return MetroKeyTipsPainter;
		}
		return m_Office2007KeyTipsPainter;
	}

	public static DialogLauncherPainter CreateRibbonBarPainter(RibbonBar ribbon)
	{
		if (BarFunctions.IsOffice2007Style(ribbon.EffectiveStyle))
		{
			return m_Office2007RibbonBarPainter;
		}
		return null;
	}

	public static RibbonTabGroupPainter CreateRibbonTabGroupPainter(eDotNetBarStyle style)
	{
		return style switch
		{
			eDotNetBarStyle.Office2010 => Office2010RibbonTabGroupPainter, 
			eDotNetBarStyle.Metro => MetroRibbonTabGroupPainter, 
			_ => Office2007RibbonTabGroupPainter, 
		};
	}

	public static ColorItemPainter CreateColorItemPainter(ColorItem item)
	{
		return m_Office2007ColorItemPainter;
	}

	public static RibbonControlPainter CreateRibbonControlPainter(RibbonControl r)
	{
		if (BarFunctions.IsOffice2007Style(r.EffectiveStyle))
		{
			return m_Office2007RibbonControlPainter;
		}
		return null;
	}

	public static SystemCaptionItemPainter CreateSystemCaptionItemPainter(SystemCaptionItem item)
	{
		eDotNetBarStyle effectiveStyle = item.EffectiveStyle;
		switch (effectiveStyle)
		{
		case eDotNetBarStyle.Office2010:
			return Office2010SystemCaptionItemPainter;
		case eDotNetBarStyle.Metro:
			return MetroCaptionItemPainter;
		default:
			if (BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				return Office2007SystemCaptionItemPainter;
			}
			return null;
		}
	}

	public static MdiSystemItemPainter CreateMdiSystemItemPainter(MDISystemItem mdiSystemItem)
	{
		if (BarFunctions.IsOffice2007Style(mdiSystemItem.EffectiveStyle))
		{
			return m_Office2007MdiSystemItemPainter;
		}
		return null;
	}

	public static FormCaptionPainter CreateFormCaptionPainter(Form form)
	{
		return m_Office2007FormCaptionPainter;
	}

	public static QatOverflowPainter CreateQatOverflowItemPainter(QatOverflowItem ribbonQatOverflowItem)
	{
		return m_Office2007QatOverflowPainter;
	}

	public static QatCustomizeItemPainter CreateQatCustomizeItemPainter(QatCustomizeItem qatCustomizeItem)
	{
		return m_Office2007QatCustomizePainter;
	}

	public static Office2007CheckBoxItemPainter CreateCheckBoxItemPainter(CheckBoxItem item)
	{
		return m_Office2007CheckBoxItemPainter;
	}

	public static void InitFactory()
	{
	}

	public static ProgressBarItemPainter CreateProgressBarItemPainter(ProgressBarItem progressBarItem)
	{
		return m_Office2007ProgressBarPainter;
	}

	internal static NavigationPanePainter CreateNavigationPanePainter()
	{
		return m_Office2007NavPanePainter;
	}

	internal static SliderPainter CreateSliderPainter(SliderItem slider)
	{
		return m_SliderPainter;
	}

	internal static SideBarPainter CreateSideBarPainter()
	{
		return m_SideBarPainter;
	}

	internal static SwitchButtonPainter CreateSwitchButtonPainter(SwitchButtonItem item)
	{
		if (item.EffectiveStyle == eDotNetBarStyle.Metro)
		{
			if (_MetroSwitchPainter == null)
			{
				_MetroSwitchPainter = new MetroSwitchButtonPainter();
			}
			return _MetroSwitchPainter;
		}
		if (m_SwitchButtonPainter == null)
		{
			m_SwitchButtonPainter = new Office2010SwitchButtonPainter();
		}
		return m_SwitchButtonPainter;
	}
}
