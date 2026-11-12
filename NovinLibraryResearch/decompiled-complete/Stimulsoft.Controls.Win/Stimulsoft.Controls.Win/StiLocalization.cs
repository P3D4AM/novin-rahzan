using System;
using System.Reflection;

namespace Stimulsoft.Controls.Win;

internal class StiLocalization
{
	private static bool failed;

	public static string Get(string category, string key)
	{
		if (!failed)
		{
			try
			{
				Assembly assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Base,Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
				if (assembly != null)
				{
					Type type = assembly.GetType("Stimulsoft.Base.Localization.StiLocalization");
					if (type != null)
					{
						MethodInfo method = type.GetMethod("GetValue", BindingFlags.Static | BindingFlags.Public);
						if (method != null)
						{
							return method.Invoke(null, new object[2] { category, key }) as string;
						}
					}
				}
			}
			catch
			{
			}
			failed = true;
		}
		return key switch
		{
			"barname_cancel" => "Cancel", 
			"barname_caption" => "New Toolbar", 
			"barname_msginvalidname" => "Toolbar name cannot be empty.", 
			"barname_name" => "&Toolbar Name:", 
			"barname_ok" => "OK", 
			"barrename_caption" => "Rename Toolbar", 
			"barsys_autohide_tooltip" => "Auto-Hide", 
			"barsys_close_tooltip" => "Close", 
			"barsys_customize_tooltip" => "Customize", 
			"colorpicker_morecolors" => "&More Colors...", 
			"colorpicker_nofill" => "&No Fill", 
			"colorpicker_standardcolorslabel" => "Standard Colors", 
			"colorpicker_themecolorslabel" => "Theme Colors", 
			"colorpickerdialog_bluelabel" => "&Blue:", 
			"colorpickerdialog_cancelbutton" => "Cancel", 
			"colorpickerdialog_caption" => "Colors", 
			"colorpickerdialog_colormodellabel" => "Color Model:", 
			"colorpickerdialog_currentcolorlabel" => "Current", 
			"colorpickerdialog_customcolorslabel" => "Colors:", 
			"colorpickerdialog_greenlabel" => "&Green:", 
			"colorpickerdialog_newcolorlabel" => "New", 
			"colorpickerdialog_okbutton" => "OK", 
			"colorpickerdialog_redlabel" => "&Red:", 
			"colorpickerdialog_rgblabel" => "RGB", 
			"colorpickerdialog_standardcolorslabel" => "Colors:", 
			"colorpickerdialog_tabcustom" => "Custom", 
			"colorpickerdialog_tabstandard" => "Standard", 
			"cust_btn_close" => "Close", 
			"cust_btn_delete" => "Delete", 
			"cust_btn_keyboard" => "&Keyboard...", 
			"cust_btn_new" => "&New...", 
			"cust_btn_rename" => "&Rename...", 
			"cust_btn_reset" => "&Reset...", 
			"cust_btn_resetusage" => "&Reset my usage data", 
			"cust_caption" => "Customize", 
			"cust_cbo_fade" => "Fade", 
			"cust_cbo_none" => "(None)", 
			"cust_cbo_random" => "Random", 
			"cust_cbo_slide" => "Slide", 
			"cust_cbo_system" => "System Default", 
			"cust_cbo_unfold" => "Unfold", 
			"cust_chk_delay" => "Show full menus after a short delay", 
			"cust_chk_fullmenus" => "Always show full menus", 
			"cust_chk_showsk" => "Show &Shortcut Keys in ScreenTips", 
			"cust_chk_showst" => "Show Screen&amp;Tips on Toolbars", 
			"cust_lbl_cats" => "Cate&amp;gories:", 
			"cust_lbl_cmds" => "Comman&amp;ds:", 
			"cust_lbl_cmdsins" => "To add a command to Bar select the category and drag the command out of this box to a Bar.", 
			"cust_lbl_menuan" => "Menu Animation:", 
			"cust_lbl_other" => "Other:", 
			"cust_lbl_pmt" => "Personalized Menus and Toolbars", 
			"cust_lbl_tlbs" => "Toolb&ars:", 
			"cust_mnu_addremove" => "&Add or Remove Buttons", 
			"cust_mnu_cust" => "Customize...", 
			"cust_mnu_reset" => "Reset Bar", 
			"cust_mnu_tooltip" => "Bar Options", 
			"cust_msg_delete" => "Are you sure you want to delete the {barname} toolbar?", 
			"cust_pm_begingroup" => "Begin Group", 
			"cust_pm_delete" => "Delete", 
			"cust_pm_name" => "Name:", 
			"cust_pm_reset" => "Reset", 
			"cust_pm_stydef" => "Default Style", 
			"cust_pm_styimagetext" => "Image and Text (Always)", 
			"cust_pm_stytextonly" => "Text Only (Always)", 
			"cust_tab_commands" => "Commands", 
			"cust_tab_options" => "Options", 
			"cust_tab_toolbars" => "Toolbars", 
			"mdisysmenu_close" => "Close", 
			"mdisysmenu_maximize" => "Maximize", 
			"mdisysmenu_minimize" => "Minimize", 
			"mdisysmenu_move" => "Move", 
			"mdisysmenu_next" => "Next", 
			"mdisysmenu_restore" => "Restore", 
			"mdisysmenu_size" => "Size", 
			"mdisystt_close" => "Close", 
			"mdisystt_minimize" => "Minimize", 
			"mdisystt_restore" => "Restore", 
			"navbar_navpaneoptions" => "Na&vigation Pane Options...", 
			"navbar_showfewerbuttons" => "Show &Fewer Buttons", 
			"navbar_showmorebuttons" => "Show &More Buttons", 
			"navPaneCollapseTooltip" => "Collapse the Navigation Pane", 
			"navPaneExpandTooltip" => "Expand the Navigation Pane", 
			"sys_custombar" => "Custom Bar", 
			"sys_morebuttons" => "More Buttons", 
			_ => null, 
		};
	}
}
