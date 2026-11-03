using System;
using System.Threading;

namespace DevComponents.DotNetBar;

public class LocalizationKeys
{
	public static string NavPaneCollapseButtonTooltip = "navPaneCollapseTooltip";

	public static string NavPaneExpandButtonTooltip = "navPaneExpandTooltip";

	public static string BarAutoHideButtonTooltip = "barsys_autohide_tooltip";

	public static string BarCustomizeButtonTooltip = "barsys_customize_tooltip";

	public static string BarCloseButtonTooltip = "barsys_close_tooltip";

	public static string BarMaximizeButtonTooltip = "barsys_maximize_tooltip";

	public static string CustomizeItemTooltip = "cust_mnu_tooltip";

	public static string CustomizeItemAddRemove = "cust_mnu_addremove";

	public static string CustomizeItemCustomize = "cust_mnu_cust";

	public static string CustomizeItemReset = "cust_mnu_reset";

	public static string OverlfowDisplayMoreTooltip = "sys_morebuttons";

	public static string CustomizeDialogCaption = "cust_caption";

	public static string CustomizeDialogTabToolbars = "cust_tab_toolbars";

	public static string CustomizeDialogTabCommands = "cust_tab_commands";

	public static string CustomizeDialogTabOptions = "cust_tab_options";

	public static string CustomizeDialogKeyboardButton = "cust_btn_keyboard";

	public static string CustomizeDialogCloseButton = "cust_btn_close";

	public static string CustomizeDialogNewToolbarButton = "cust_btn_new";

	public static string CustomizeDialogRenameToolbarButton = "cust_btn_rename";

	public static string CustomizeDialogDeleteToolbarButton = "cust_btn_delete";

	public static string CustomizeDialogResetToolbarButton = "cust_btn_reset";

	public static string CustomizeDialogLabelDragInfo = "cust_lbl_cmdsins";

	public static string CustomizeDialogLabelCategories = "cust_lbl_cats";

	public static string CustomizeDialogLabelToolbars = "cust_lbl_tlbs";

	public static string CustomizeDialogOptionsPersonalized = "cust_lbl_pmt";

	public static string CustomizeDialogOptionsFullMenus = "cust_chk_fullmenus";

	public static string CustomizeDialogOptionsFullMenusDelay = "cust_chk_delay";

	public static string CustomizeDialogOptionsResetUsage = "cust_btn_resetusage";

	public static string CustomizeDialogOptionsOther = "cust_lbl_other";

	public static string CustomizeDialogOptionsScreenTips = "cust_chk_showst";

	public static string CustomizeDialogOptionsScreenTipsKeys = "cust_chk_showsk";

	public static string CustomizeDialogOptionsMenuAnimation = "cust_lbl_menuan";

	public static string CustomizeDialogOptionsMenuAnimationNone = "cust_cbo_none";

	public static string CustomizeDialogOptionsMenuAnimationSys = "cust_cbo_system";

	public static string CustomizeDialogOptionsMenuAnimationRandom = "cust_cbo_random";

	public static string CustomizeDialogOptionsMenuAnimationUnfold = "cust_cbo_unfold";

	public static string CustomizeDialogOptionsMenuAnimationSlide = "cust_cbo_slide";

	public static string CustomizeDialogOptionsConfirmDelete = "cust_msg_delete";

	public static string CustomizeMenuReset = "cust_pm_reset";

	public static string CustomizeMenuDelete = "cust_pm_delete";

	public static string CustomizeMenuChangeName = "cust_pm_name";

	public static string CustomizeMenuDefaultStyle = "cust_pm_stydef";

	public static string CustomizeMenuTextOnly = "cust_pm_stytextonly";

	public static string CustomizeMenuImageAndText = "cust_pm_styimagetext";

	public static string CustomizeMenuBeginGroup = "cust_pm_begingroup";

	public static string MdiSystemItemMinimizeTooltip = "mdisystt_minimize";

	public static string MdiSystemItemRestoreTooltip = "mdisystt_restore";

	public static string MdiSystemItemCloseTooltip = "mdisystt_close";

	public static string MdiSystemItemMenuRestore = "mdisysmenu_restore";

	public static string MdiSystemItemMenuMove = "mdisysmenu_move";

	public static string MdiSystemItemMenuSize = "mdisysmenu_size";

	public static string MdiSystemItemMenuMinimize = "mdisysmenu_minimize";

	public static string MdiSystemItemMenuMaximize = "mdisysmenu_maximize";

	public static string MdiSystemItemMenuClose = "mdisysmenu_close";

	public static string MdiSystemItemMenuNext = "mdisysmenu_next";

	public static string NavBarShowMoreButtons = "navbar_showmorebuttons";

	public static string NavBarShowFewerButtons = "navbar_showfewerbuttons";

	public static string NavBarOptions = "navbar_navpaneoptions";

	public static string NavBarDialogMoveUp = "navbar_navpanedialogmoveup";

	public static string NavBarDialogMoveDown = "navbar_navpanedialogmovedown";

	public static string NavBarDialogReset = "navbar_navpanedialogreset";

	public static string NavBarDialogOK = "navbar_navpanedialogok";

	public static string NavBarDialogCancel = "navbar_navpanedialogcancel";

	public static string NavBarDialogTitle = "navbar_navpanedialogtitle";

	public static string NavBarDialogListLabel = "navbar_navpanedialoglistlabel";

	public static string RenameBarDialogCaption = "barrename_caption";

	public static string NewBarDialogCaption = "barname_caption";

	public static string BarEditDialogBarNameLabel = "barname_name";

	public static string BarEditDialogBarOkButton = "barname_ok";

	public static string BarEditDialogBarCancelButton = "barname_cancel";

	public static string BarEditDialogInvalidNameMessage = "barname_msginvalidname";

	public static string ColorPickerThemeColorsLabel = "colorpicker_themecolorslabel";

	public static string ColorPickerStandardColorsLabel = "colorpicker_standardcolorslabel";

	public static string ColorPickerMoreColorsMenuItem = "colorpicker_morecolors";

	public static string ColorPickerDialogOKButton = "colorpickerdialog_okbutton";

	public static string ColorPickerDialogCancelButton = "colorpickerdialog_cancelbutton";

	public static string ColorPickerDialogNewColorLabel = "colorpickerdialog_newcolorlabel";

	public static string ColorPickerDialogCurrentColorLabel = "colorpickerdialog_currentcolorlabel";

	public static string ColorPickerDialogStandardColorsLabel = "colorpickerdialog_standardcolorslabel";

	public static string ColorPickerDialogCustomColorsLabel = "colorpickerdialog_customcolorslabel";

	public static string ColorPickerDialogGreenLabel = "colorpickerdialog_greenlabel";

	public static string ColorPickerDialogBlueLabel = "colorpickerdialog_bluelabel";

	public static string ColorPickerDialogRedLabel = "colorpickerdialog_redlabel";

	public static string ColorPickerDialogColorModelLabel = "colorpickerdialog_colormodellabel";

	public static string ColorPickerDialogRgbLabel = "colorpickerdialog_rgblabel";

	public static string ColorPickerTabStandard = "colorpickerdialog_tabstandard";

	public static string ColorPickerTabCustom = "colorpickerdialog_tabcustom";

	public static string ColorPickerCaption = "colorpickerdialog_caption";

	public static string QatCustomizeTooltip = "qat_customizetooltip";

	public static string MonthCalendarTodayButtonText = "monthcalendar_todaybutton";

	public static string MonthCalendarClearButtonText = "monthcalendar_clearbutton";

	public static string MessageBoxAbortButton = "mb_abortbutton";

	public static string MessageBoxCancelButton = "mb_cancelbutton";

	public static string MessageBoxCloseButton = "mb_closebutton";

	public static string MessageBoxContinueButton = "mb_continuebutton";

	public static string MessageBoxHelpButton = "mb_helpbutton";

	public static string MessageBoxIgnoreButton = "mb_ignorebutton";

	public static string MessageBoxNoButton = "mb_nobutton";

	public static string MessageBoxOkButton = "mb_okbutton";

	public static string MessageBoxRetryButton = "mb_retrybutton";

	public static string MessageBoxTryAgainButton = "mb_tryagainbutton";

	public static string MessageBoxYesButton = "mb_yesbutton";

	public static string FormSystemMenuRestore = "FormSystemMenuRestore";

	public static string FormSystemMenuMove = "FormSystemMenuMove";

	public static string FormSystemMenuSize = "FormSystemMenuSize";

	public static string FormSystemMenuMinimize = "FormSystemMenuMinimize";

	public static string FormSystemMenuMaximize = "FormSystemMenuMaximize";

	public static string FormSystemMenuClose = "FormSystemMenuClose";

	public static string TimeSelectorOkButton = "TimeSelectorOkButton";

	public static string TimeSelectorClearButton = "TimeSelectorClearButton";

	public static string TimeSelectorHourLabel = "TimeSelectorHourLabel";

	public static string TimeSelectorMinuteLabel = "TimeSelectorMinuteLabel";

	public static string SuperGridFilterShowAll = "SuperGrid_FilterShowAll";

	public static string SuperGridFilterShowNull = "SuperGrid_FilterShowNull";

	public static string SuperGridFilterShowNotNull = "SuperGrid_FilterShowNotnull";

	public static string SuperGridFilterCustom = "SuperGrid_FilterCustom";

	public static string SuperGridFilterEnterExpr = "SuperGrid_FilterEnterExpr";

	public static string SuperGridFilterSampleExpr = "SuperGrid_FilterSampleExpr";

	public static string SuperGridFilterCustomHelp = "SuperGrid_FilterCustomHelp";

	public static string SuperGridFilterApply = "SuperGrid_FilterApply";

	public static string SuperGridFilterOk = "SuperGrid_FilterOk";

	public static string SuperGridFilterCancel = "SuperGrid_FilterCancel";

	public static string SuperGridFilterClose = "SuperGrid_FilterClose";

	public static string SuperGridFilterClear = "SuperGrid_FilterClear";

	public static string SuperGridFilterReset = "SuperGrid_FilterReset";

	public static string SuperGridFilterByRelativeDate = "SuperGrid_FilterByRelativeDate";

	public static string SuperGridFilterBySpecificDate = "SuperGrid_FilterBySpecificDate";

	public static string SuperGridFilterByDateRange = "SuperGrid_FilterByDateRange";

	public static string SuperGridFilterCurrentMonth = "SuperGrid_FilterCurrentMonth";

	public static string SuperGridFilterCurrentYear = "SuperGrid_FilterCurrentYear";

	public static string SuperGridFilterLastMonthPeriod = "SuperGrid_FilterLastMonthPeriod";

	public static string SuperGridFilterLast3MonthPeriod = "SuperGrid_FilterLast3MonthPeriod";

	public static string SuperGridFilterLast6MonthPeriod = "SuperGrid_FilterLast6MonthPeriod";

	public static string SuperGridFilterLast9MonthPeriod = "SuperGridFilter_Last9MonthPeriod";

	public static string SuperGridFilterLastYear = "SuperGrid_FilterLastYear";

	public static string SuperGridFilterLast5YearPeriod = "SuperGrid_FilterLast5YearPeriod";

	public static string SuperGridFilterLast10YearPeriod = "SuperGrid_FilterLast10YearPeriod";

	public static string SuperGridFilterExprError = "SuperGrid_FilterExprError";

	public static string SuperGridFilterMissingParen = "SuperGrid_FilterMissingParen";

	public static string SuperGridFilterMissingQuote = "SuperGrid_FilterMissingQuote";

	public static string SuperGridFilterInvalidArgCount = "SuperGrid_FilterInvalidArgCount";

	public static string SuperGridFilterInvalidArg = "SuperGrid_FilterInvalidArg";

	public static string SuperGridFilterInvalidEmptyOp = "SuperGrid_FilterInvalidEmptyOp";

	public static string SuperGridFilterInvalidDateTimeOp = "SuperGrid_FilterInvalidDateTimeOp";

	public static string SuperGridFilterInvalidBoolOp = "SuperGrid_FilterInvalidBoolOp";

	public static string SuperGridFilterInvalidStringOp = "SuperGrid_FilterInvalidStringOp";

	public static string SuperGridFilterInvalidNumericOp = "SuperGrid_FilterInvalidNumericOp";

	public static string SuperGridFilterInvalidEval = "SuperGrid_FilterInvalidEval";

	public static string SuperGridFilterUndefinedFunction = "SuperGrid_FilterUndefinedFunction";

	public static string SuperGridFilterCustomFilter = "SuperGrid_FilterCustomFilter";

	public static string SuperGridNewFilterExpr = "SuperGrid_NewFilterExpr";

	public static string SuperGridNewFilter = "SuperGrid_NewFilter";

	public static string SuperGridDeleteFilter = "SuperGrid_DeleteFilter";

	public static string SuperGridFilterName = "SuperGrid_FilterName";

	public static string SuperGridFilterDescription = "SuperGrid_FilterDescription";

	public static string SuperGridShowInFilterPopup = "SuperGrid_ShowInFilterPopup";

	public static string SuperGridFilterHelpTitle = "SuperGrid_FilterHelpTitle";

	public static string SuperGridGroupByWaterMarkText = "SuperGrid_GroupByWaterMarkText";

	private static DotNetBarManager.LocalizeStringEventHandler _09D4;

	public static event DotNetBarManager.LocalizeStringEventHandler LocalizeString
	{
		add
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Combine(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
		remove
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Remove(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
	}

	internal static void _0DF9(LocalizeEventArgs ؿ)
	{
		if (_09D4 != null)
		{
			_09D4(null, ؿ);
		}
	}
}
