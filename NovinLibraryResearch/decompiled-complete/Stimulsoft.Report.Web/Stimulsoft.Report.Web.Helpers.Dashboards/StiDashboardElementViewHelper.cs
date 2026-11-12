using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiDashboardElementViewHelper
{
	public static Hashtable GetElementContentAttributes(IStiElement element, double scaleX, double scaleY, StiRequestParams requestParams)
	{
		Hashtable hashtable = new Hashtable();
		if (element is IStiListBoxElement)
		{
			IStiListBoxElement stiListBoxElement = element as IStiListBoxElement;
			hashtable["items"] = StiListBoxElementViewHelper.GetElementItems(stiListBoxElement);
			hashtable["filters"] = StiDataFiltersHelper.GetElementFilters(stiListBoxElement);
			hashtable["columnPath"] = StiListBoxElementViewHelper.GetColumnPath(stiListBoxElement);
			hashtable["isStringColumnType"] = StiDataFiltersHelper.IsStringColumnType(stiListBoxElement);
			hashtable["showAllValue"] = stiListBoxElement.ShowAllValue;
			hashtable["selectionMode"] = stiListBoxElement.SelectionMode;
			hashtable["orientation"] = stiListBoxElement.Orientation;
			hashtable["settings"] = StiListBoxElementViewHelper.GetSettings(stiListBoxElement);
			hashtable["title"] = GetTitle(stiListBoxElement);
		}
		else if (element is IStiComboBoxElement)
		{
			IStiComboBoxElement stiComboBoxElement = element as IStiComboBoxElement;
			hashtable["items"] = StiComboBoxElementViewHelper.GetElementItems(stiComboBoxElement);
			hashtable["filters"] = StiDataFiltersHelper.GetElementFilters(stiComboBoxElement);
			hashtable["columnPath"] = StiComboBoxElementViewHelper.GetColumnPath(stiComboBoxElement);
			hashtable["isStringColumnType"] = StiDataFiltersHelper.IsStringColumnType(stiComboBoxElement);
			hashtable["showAllValue"] = stiComboBoxElement.ShowAllValue;
			hashtable["selectionMode"] = stiComboBoxElement.SelectionMode;
			hashtable["settings"] = StiComboBoxElementViewHelper.GetSettings(stiComboBoxElement);
		}
		else if (element is IStiDatePickerElement)
		{
			IStiDatePickerElement stiDatePickerElement = element as IStiDatePickerElement;
			hashtable["filters"] = StiDataFiltersHelper.GetElementFilters(stiDatePickerElement);
			hashtable["columnPath"] = StiDatePickerElementViewHelper.GetColumnPath(stiDatePickerElement);
			hashtable["selectionMode"] = stiDatePickerElement.SelectionMode;
			hashtable["initialRangeSelection"] = stiDatePickerElement.InitialRangeSelection;
			hashtable["initialRangeSelectionSource"] = stiDatePickerElement.InitialRangeSelectionSource;
			hashtable["condition"] = stiDatePickerElement.Condition;
			hashtable["settings"] = StiDatePickerElementViewHelper.GetSettings(stiDatePickerElement);
			bool flag = StiDatePickerElementViewHelper.IsVariablePresent(stiDatePickerElement);
			bool flag2 = StiDatePickerElementViewHelper.IsRangeVariablePresent(stiDatePickerElement);
			hashtable["isVariablePresent"] = flag;
			hashtable["isRangeVariablePresent"] = flag2;
			if (stiDatePickerElement.InitialRangeSelectionSource == StiInitialDateRangeSelectionSource.Variable)
			{
				if (flag2)
				{
					hashtable["variableRangeValues"] = StiDatePickerElementViewHelper.GetVariableRangeValues(stiDatePickerElement);
				}
				else if (flag)
				{
					hashtable["variableValue"] = StiDatePickerElementViewHelper.GetVariableValue(stiDatePickerElement);
				}
			}
			else if (stiDatePickerElement.SelectionMode == StiDateSelectionMode.AutoRange)
			{
				hashtable["autoRangeValues"] = StiDatePickerElementViewHelper.GetAutoRangeValues(stiDatePickerElement);
			}
		}
		else if (element is IStiTreeViewElement)
		{
			IStiTreeViewElement stiTreeViewElement = element as IStiTreeViewElement;
			hashtable["items"] = StiTreeViewElementViewHelper.GetElementItems(stiTreeViewElement);
			hashtable["filters"] = StiDataFiltersHelper.GetElementFilters(stiTreeViewElement);
			hashtable["columnPath"] = StiTreeViewElementViewHelper.GetColumnPath(stiTreeViewElement);
			hashtable["isStringColumnType"] = StiDataFiltersHelper.IsStringColumnType(stiTreeViewElement);
			hashtable["meterKey"] = StiTreeViewElementViewHelper.GetMeterKey(stiTreeViewElement);
			hashtable["showAllValue"] = stiTreeViewElement.ShowAllValue;
			hashtable["selectionMode"] = stiTreeViewElement.SelectionMode;
			hashtable["settings"] = StiTreeViewElementViewHelper.GetSettings(stiTreeViewElement);
			hashtable["title"] = GetTitle(stiTreeViewElement);
		}
		else if (element is IStiTreeViewBoxElement)
		{
			IStiTreeViewBoxElement stiTreeViewBoxElement = element as IStiTreeViewBoxElement;
			hashtable["items"] = StiTreeViewBoxElementViewHelper.GetElementItems(stiTreeViewBoxElement);
			hashtable["filters"] = StiDataFiltersHelper.GetElementFilters(stiTreeViewBoxElement);
			hashtable["columnPath"] = StiTreeViewBoxElementViewHelper.GetColumnPath(stiTreeViewBoxElement);
			hashtable["isStringColumnType"] = StiDataFiltersHelper.IsStringColumnType(stiTreeViewBoxElement);
			hashtable["meterKey"] = StiTreeViewBoxElementViewHelper.GetMeterKey(stiTreeViewBoxElement);
			hashtable["showAllValue"] = stiTreeViewBoxElement.ShowAllValue;
			hashtable["selectionMode"] = stiTreeViewBoxElement.SelectionMode;
			hashtable["settings"] = StiTreeViewBoxElementViewHelper.GetSettings(stiTreeViewBoxElement);
			hashtable["title"] = GetTitle(stiTreeViewBoxElement);
		}
		else if (element is IStiTableElement)
		{
			IStiTableElement stiTableElement = element as IStiTableElement;
			hashtable["data"] = StiTableElementViewHelper.GetTableData(stiTableElement, scaleX);
			hashtable["hiddenData"] = StiTableElementViewHelper.GetTableHiddenData(stiTableElement);
			hashtable["filters"] = StiDataFiltersHelper.GetElementFilters(stiTableElement);
			hashtable["sorts"] = StiDataSortsHelper.GetElementSorts(stiTableElement);
			hashtable["settings"] = StiTableElementViewHelper.GetTableSettings(stiTableElement);
			hashtable["title"] = GetTitle(stiTableElement);
			hashtable["interaction"] = GetDashboardInteraction(stiTableElement);
			hashtable["filtersString"] = StiDataFiltersHelper.GetDataTableFilterQueryStringRepresentation(stiTableElement);
		}
		else if (element is IStiChartElement)
		{
			IStiChartElement stiChartElement = element as IStiChartElement;
			hashtable["filters"] = StiDataFiltersHelper.GetElementFilters(stiChartElement);
			hashtable["htmlContent"] = StiDashboardsSvgHelper.SaveElementToString(stiChartElement, scaleX, scaleY, designMode: false, StiExportFormat.Html5, requestParams);
			hashtable["interaction"] = GetDashboardInteraction(stiChartElement);
			hashtable["filtersString"] = StiDataFiltersHelper.GetDataTableFilterQueryStringRepresentation(stiChartElement);
			hashtable["title"] = GetTitle(stiChartElement);
			hashtable["sortItems"] = StiDataSortsHelper.GetSortMenuItems(stiChartElement);
			hashtable["userViewStates"] = StiChartElementViewHelper.GetUserViewStates(stiChartElement);
			hashtable["selectedViewStateKey"] = (stiChartElement as IStiUserViewStates)?.SelectedViewStateKey;
			hashtable["dataMode"] = stiChartElement.DataMode;
			if (StiChartElementViewHelper.IsBubble(stiChartElement))
			{
				hashtable["isBubble"] = true;
				hashtable["bubleYColumnPath"] = StiChartElementViewHelper.GetBubleYColumnPath(stiChartElement);
				hashtable["bubleXColumnPath"] = StiChartElementViewHelper.GetBubleXColumnPath(stiChartElement);
			}
			else
			{
				hashtable["argumentColumnPath"] = StiChartElementViewHelper.GetArgumentColumnPath(stiChartElement);
				hashtable["seriesColumnPath"] = StiChartElementViewHelper.GetSeriesColumnPath(stiChartElement);
			}
		}
		else if (element is IStiRegionMapElement)
		{
			IStiRegionMapElement stiRegionMapElement = element as IStiRegionMapElement;
			hashtable["columnPath"] = StiRegionMapElementViewHelper.GetColumnPath(stiRegionMapElement);
			hashtable["filters"] = StiDataFiltersHelper.GetElementFilters(stiRegionMapElement);
			hashtable["htmlContent"] = StiDashboardsSvgHelper.SaveElementToString(stiRegionMapElement, scaleX, scaleY, designMode: false, StiExportFormat.Html5);
			hashtable["interaction"] = GetDashboardInteraction(stiRegionMapElement);
			hashtable["filtersString"] = StiDataFiltersHelper.GetDataTableFilterQueryStringRepresentation(stiRegionMapElement);
			hashtable["title"] = GetTitle(stiRegionMapElement);
			hashtable["isDark"] = StiDashboardStyleHelper.IsDarkStyle(stiRegionMapElement);
			hashtable["showZoomPanel"] = StiOptions.Viewer.Map.ShowZoomPanel;
		}
		else if (element is IStiPivotTableElement)
		{
			IStiPivotTableElement stiPivotTableElement = element as IStiPivotTableElement;
			hashtable["data"] = StiPivotTableElementViewHelper.GetPivotTableData(stiPivotTableElement);
			hashtable["settings"] = StiPivotTableElementViewHelper.GetPivotTableSettings(stiPivotTableElement);
			hashtable["title"] = GetTitle(stiPivotTableElement);
		}
		else if (element is IStiOnlineMapElement)
		{
			hashtable["htmlContent"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(StiDashboardOnlimeMapHelper.GetBingMapScript(element, showTitle: true)));
		}
		else if (element is IStiPanel)
		{
			hashtable["dashboardWatermark"] = GetDashboardWatermark(element);
		}
		else if (element is IStiIndicatorElement)
		{
			hashtable["sortItems"] = StiDataSortsHelper.GetSortMenuItems(element);
			hashtable["interaction"] = GetDashboardInteraction(element);
			bool needToScroll = true;
			hashtable["svgContent"] = StiDashboardsSvgHelper.SaveElementToString(element, ref needToScroll, scaleX, scaleY);
			if (needToScroll)
			{
				hashtable["title"] = GetTitle(element);
				hashtable["svgContentIsScrollable"] = true;
			}
		}
		else if (element is IStiGaugeElement)
		{
			bool needToScroll2 = true;
			hashtable["svgContent"] = StiDashboardsSvgHelper.SaveElementToString(element, ref needToScroll2, scaleX, scaleY, designMode: false, StiExportFormat.ImageSvg, requestParams);
			hashtable["sortItems"] = StiDataSortsHelper.GetSortMenuItems(element);
			if (needToScroll2)
			{
				hashtable["title"] = GetTitle(element);
				hashtable["svgContentIsScrollable"] = true;
			}
		}
		else if (element is IStiButtonElement)
		{
			IStiButtonElement stiButtonElement = element as IStiButtonElement;
			hashtable["buttonText"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(stiButtonElement.Text));
			hashtable["buttonType"] = stiButtonElement.Type;
			hashtable["iconAlignment"] = stiButtonElement.IconAlignment;
			hashtable["horAlignment"] = (stiButtonElement as IStiTextHorAlignment).HorAlignment;
			hashtable["vertAlignment"] = (stiButtonElement as IStiVertAlignment).VertAlignment;
			hashtable["wordWrap"] = (stiButtonElement as IStiWordWrap).WordWrap;
			hashtable["buttonShapeType"] = stiButtonElement.ShapeType;
			hashtable["brush"] = GetBrushStr((stiButtonElement as IStiBrush).Brush);
			hashtable["textBrush"] = GetBrushStr((stiButtonElement as IStiTextBrush).TextBrush);
			hashtable["iconBrush"] = GetBrushStr(stiButtonElement.IconBrush);
			hashtable["font"] = GetFontJson(stiButtonElement.Font);
			hashtable["buttonIconSet"] = GetButtonIconSetProperty(stiButtonElement.GetIconSet());
			hashtable["buttonVisualStates"] = GetButtonVisualStatesProperty(stiButtonElement.GetVisualStates());
			hashtable["styleColors"] = GetButtonStyleColors(stiButtonElement);
		}
		else
		{
			if (element is IStiTextElement || element is IStiImageElement)
			{
				hashtable["interaction"] = GetDashboardInteraction(element);
			}
			if (element is IStiTextElement)
			{
				hashtable["plainText"] = ((IStiTextElement)element).GetSimpleText();
				hashtable["isTimeExpression"] = Stimulsoft.Data.Helpers.StiExpressionHelper.IsTimeExpression(((IStiTextElement)element).Text);
			}
			bool needToScroll3 = true;
			hashtable["svgContent"] = StiDashboardsSvgHelper.SaveElementToString(element, ref needToScroll3, scaleX, scaleY);
			if (element is IStiProgressElement || element is IStiIndicatorElement || element is IStiCardsElement)
			{
				if (!(element is IStiCardsElement))
				{
					hashtable["sortItems"] = StiDataSortsHelper.GetSortMenuItems(element);
				}
				if (needToScroll3)
				{
					hashtable["title"] = GetTitle(element);
					hashtable["svgContentIsScrollable"] = true;
				}
			}
		}
		return hashtable;
	}

	public static string GetForeColor(IStiElement element)
	{
		return StiReportHelper.GetHtmlColor(StiDashboardStyleHelper.GetForeColor(element));
	}

	public static string GetBackColor(IStiElement element)
	{
		return StiReportHelper.GetHtmlColor(StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity: true));
	}

	public static Hashtable GetBorder(IStiElement element)
	{
		if (element is IStiSimpleBorder)
		{
			return GetBorderJson((element as IStiSimpleBorder).Border);
		}
		return null;
	}

	public static Hashtable GetBorderJson(StiSimpleBorder border)
	{
		if (border == null)
		{
			return null;
		}
		return new Hashtable
		{
			["left"] = border.IsLeftBorderSidePresent,
			["top"] = border.IsTopBorderSidePresent,
			["right"] = border.IsRightBorderSidePresent,
			["bottom"] = border.IsBottomBorderSidePresent,
			["size"] = border.Size,
			["color"] = StiReportHelper.GetHtmlColor(border.Color),
			["style"] = (int)border.Style
		};
	}

	public static Hashtable GetFont(IStiElement element)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		if (element is IStiFont)
		{
			return GetFontJson((element as IStiFont).Font);
		}
		return GetFontJson(new Font("Arial", 10f));
	}

	public static Hashtable GetFontJson(Font font)
	{
		if (font == null)
		{
			return null;
		}
		return new Hashtable
		{
			["name"] = ((!string.IsNullOrEmpty(font.OriginalFontName)) ? font.OriginalFontName : font.Name),
			["size"] = font.Size.ToString(),
			["bold"] = font.Bold,
			["italic"] = font.Italic,
			["underline"] = font.Underline,
			["strikeout"] = font.Strikeout
		};
	}

	public static Hashtable GetTitle(IStiElement element)
	{
		if (element is IStiTitleElement { Title: not null, Title: var title })
		{
			Hashtable hashtable = new Hashtable();
			Color color = ((title.BackColor == Color.Transparent) ? StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity: true) : title.BackColor);
			hashtable["backColor"] = (color.Equals((object?)StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity: true)) ? "transparent" : StiReportHelper.GetHtmlColor(color));
			hashtable["foreColor"] = StiReportHelper.GetHtmlColor((title.ForeColor == Color.Transparent) ? StiDashboardStyleHelper.GetTitleForeColor(element) : title.ForeColor);
			hashtable["text"] = ((title.Text != null) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(StiReportParser.Parse(title.Text, element))) : string.Empty);
			hashtable["font"] = GetFontJson(title.Font);
			hashtable["horAlignment"] = title.HorAlignment;
			hashtable["visible"] = title.Visible;
			return hashtable;
		}
		return null;
	}

	private static string GetBrushStr(StiBrush brush)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected I4, but got Unknown
		string result = "none";
		if (brush is StiStyleBrush)
		{
			return "isStyleBrush";
		}
		if (brush is StiDefaultBrush)
		{
			return "isDefaultBrush";
		}
		if (brush is StiEmptyBrush)
		{
			return "0";
		}
		if (brush is StiSolidBrush)
		{
			StiSolidBrush stiSolidBrush = brush as StiSolidBrush;
			return $"1;{StiReportHelper.GetHtmlColor(stiSolidBrush.Color)}";
		}
		if (brush is StiHatchBrush)
		{
			StiHatchBrush stiHatchBrush = brush as StiHatchBrush;
			return $"2;{StiReportHelper.GetHtmlColor(stiHatchBrush.ForeColor)};{StiReportHelper.GetHtmlColor(stiHatchBrush.BackColor)};{((int)stiHatchBrush.Style).ToString()}";
		}
		if (brush is StiGradientBrush)
		{
			StiGradientBrush stiGradientBrush = brush as StiGradientBrush;
			return $"3;{StiReportHelper.GetHtmlColor(stiGradientBrush.StartColor)};{StiReportHelper.GetHtmlColor(stiGradientBrush.EndColor)};{stiGradientBrush.Angle.ToString()}";
		}
		if (brush is StiGlareBrush)
		{
			StiGlareBrush stiGlareBrush = brush as StiGlareBrush;
			return $"4;{StiReportHelper.GetHtmlColor(stiGlareBrush.StartColor)};{StiReportHelper.GetHtmlColor(stiGlareBrush.EndColor)};{stiGlareBrush.Angle.ToString()};{stiGlareBrush.Focus.ToString()};{stiGlareBrush.Scale.ToString()}";
		}
		if (brush is StiGlassBrush)
		{
			StiGlassBrush stiGlassBrush = brush as StiGlassBrush;
			return string.Format("5;{0};{1};{2}", StiReportHelper.GetHtmlColor(stiGlassBrush.Color), stiGlassBrush.Blend.ToString(), stiGlassBrush.DrawHatch ? "1" : "0");
		}
		return result;
	}

	private static Hashtable GetButtonIconSetProperty(IStiButtonElementIconSet iconSet)
	{
		if (iconSet != null)
		{
			return new Hashtable
			{
				["icon"] = (iconSet.Icon.HasValue ? StiFontIconsHelper.GetContent(iconSet.Icon) : ""),
				["checkedIcon"] = (iconSet.CheckedIcon.HasValue ? StiFontIconsHelper.GetContent(iconSet.CheckedIcon) : ""),
				["uncheckedIcon"] = (iconSet.UncheckedIcon.HasValue ? StiFontIconsHelper.GetContent(iconSet.UncheckedIcon) : "")
			};
		}
		return null;
	}

	private static Hashtable GetButtonVisualStatesProperty(IStiButtonVisualStates visualStates)
	{
		if (visualStates != null)
		{
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable
			{
				["hover"] = visualStates.GetHoverState(),
				["pressed"] = visualStates.GetPressedState(),
				["check"] = visualStates.GetCheckedState()
			};
			{
				foreach (DictionaryEntry item in hashtable2)
				{
					IStiButtonVisualState stiButtonVisualState = item.Value as IStiButtonVisualState;
					hashtable[item.Key] = new Hashtable
					{
						["border"] = GetBorderJson(stiButtonVisualState.Border),
						["brush"] = GetBrushStr(stiButtonVisualState.Brush),
						["font"] = GetFontJson(stiButtonVisualState.Font),
						["iconBrush"] = GetBrushStr(stiButtonVisualState.IconBrush),
						["textBrush"] = GetBrushStr(stiButtonVisualState.TextBrush),
						["iconSet"] = GetButtonIconSetProperty(stiButtonVisualState.GetIconSet())
					};
				}
				return hashtable;
			}
		}
		return null;
	}

	private static Hashtable GetButtonStyleColors(IStiButtonElement buttonElement)
	{
		Hashtable hashtable = new Hashtable();
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(buttonElement);
		if (controlStyle != null)
		{
			hashtable["backColor"] = StiReportHelper.GetHtmlColor(controlStyle.BackColor);
			hashtable["hoverBackColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotBackColor);
			hashtable["selectedBackColor"] = StiReportHelper.GetHtmlColor(controlStyle.SelectedBackColor);
			hashtable["iconColor"] = StiReportHelper.GetHtmlColor(controlStyle.GlyphColor);
			hashtable["hoverIconColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotGlyphColor);
			hashtable["selectedIconColor"] = StiReportHelper.GetHtmlColor(controlStyle.SelectedGlyphColor);
			hashtable["textColor"] = StiReportHelper.GetHtmlColor(controlStyle.ForeColor);
			hashtable["hoverTextColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotForeColor);
			hashtable["selectedTextColor"] = StiReportHelper.GetHtmlColor(controlStyle.SelectedForeColor);
		}
		return hashtable;
	}

	public static Hashtable GetControlElementSettings(IStiElement element)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["backColor"] = GetBackColor(element);
		hashtable["foreColor"] = GetForeColor(element);
		hashtable["isDarkStyle"] = StiDashboardStyleHelper.IsDarkStyle(element);
		if (StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity: true).A != byte.MaxValue)
		{
			hashtable["backColor"] = "transparent";
		}
		int num;
		if (element is IStiForeColor)
		{
			_ = (element as IStiForeColor).ForeColor;
			num = ((!((element as IStiForeColor).ForeColor != Color.Transparent)) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		if (element is IStiControlElement element2)
		{
			StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element2);
			if (controlStyle != null)
			{
				hashtable["glyphColor"] = StiReportHelper.GetHtmlColor(controlStyle.GlyphColor);
				hashtable["hotBackColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotBackColor);
				hashtable["hotForeColor"] = (flag ? StiReportHelper.GetHtmlColor(controlStyle.HotForeColor) : hashtable["foreColor"]);
				hashtable["hotGlyphColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotGlyphColor);
				hashtable["hotSelectedBackColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotSelectedBackColor);
				hashtable["hotSelectedForeColor"] = (flag ? StiReportHelper.GetHtmlColor(controlStyle.HotSelectedForeColor) : hashtable["foreColor"]);
				hashtable["hotSelectedGlyphColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotSelectedGlyphColor);
				hashtable["selectedBackColor"] = StiReportHelper.GetHtmlColor(controlStyle.SelectedBackColor);
				hashtable["selectedForeColor"] = (flag ? StiReportHelper.GetHtmlColor(controlStyle.SelectedForeColor) : hashtable["foreColor"]);
				hashtable["selectedGlyphColor"] = StiReportHelper.GetHtmlColor(controlStyle.SelectedGlyphColor);
				hashtable["separatorColor"] = StiReportHelper.GetHtmlColor(controlStyle.SeparatorColor);
			}
		}
		return hashtable;
	}

	public static Hashtable GetLayout(IStiElement element)
	{
		Hashtable hashtable = new Hashtable();
		if ((element as IStiElementInteraction)?.DashboardInteraction is IStiInteractionLayout stiInteractionLayout)
		{
			hashtable["fullScreenButton"] = stiInteractionLayout.ShowFullScreenButton;
			hashtable["saveButton"] = stiInteractionLayout.ShowSaveButton;
			hashtable["viewDataButton"] = stiInteractionLayout.ShowViewDataButton;
		}
		return hashtable;
	}

	private static Color FixColor(Color color)
	{
		if (color.A >= byte.MaxValue)
		{
			return color;
		}
		return Color.FromArgb(255, color.R, color.G, color.B);
	}

	public static Hashtable GetActionColors(IStiElement element)
	{
		Hashtable hashtable = new Hashtable();
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
		bool flag = StiDashboardStyleHelper.IsDarkStyle(element);
		hashtable["foreColor"] = StiReportHelper.GetHtmlColor(StiDashboardStyleHelper.GetForeColor(element, controlStyle.ForeColor));
		if (element is IStiDashboard)
		{
			hashtable["backColor"] = StiReportHelper.GetHtmlColor(FixColor(StiDashboardStyleHelper.GetDashboardBackColor(element as IStiDashboard, isViewer: true)));
		}
		else
		{
			hashtable["backColor"] = StiReportHelper.GetHtmlColor(FixColor(StiDashboardStyleHelper.GetBackColor(element, controlStyle.BackColor)));
		}
		hashtable["glyphColor"] = StiReportHelper.GetHtmlColor(controlStyle.GlyphColor);
		hashtable["selectedForeColor"] = StiReportHelper.GetHtmlColor(controlStyle.SelectedForeColor);
		hashtable["selectedBackColor"] = StiReportHelper.GetHtmlColor(controlStyle.SelectedBackColor);
		hashtable["selectedGlyphColor"] = StiReportHelper.GetHtmlColor(controlStyle.SelectedGlyphColor);
		hashtable["hotForeColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotForeColor);
		hashtable["hotBackColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotBackColor);
		hashtable["hotGlyphColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotGlyphColor);
		hashtable["hotSelectedForeColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotSelectedForeColor);
		hashtable["hotSelectedBackColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotSelectedBackColor);
		hashtable["hotSelectedGlyphColor"] = StiReportHelper.GetHtmlColor(controlStyle.HotSelectedGlyphColor);
		if (element is IStiDashboard && flag)
		{
			if (StiDashboardStyleHelper.GetStyle(element) == StiElementStyleIdent.DarkTurquoise)
			{
				hashtable["selectedBackColor"] = StiReportHelper.GetHtmlColor(StiColor.Get("113344"));
				hashtable["hotBackColor"] = StiReportHelper.GetHtmlColor(StiColor.Get("235e6d"));
				hashtable["hotSelectedBackColor"] = StiReportHelper.GetHtmlColor(StiColor.Get("f0621e"));
			}
			else
			{
				hashtable["selectedBackColor"] = StiReportHelper.GetHtmlColor(StiColor.Get("414141"));
				hashtable["hotBackColor"] = StiReportHelper.GetHtmlColor(StiColor.Get("1f1f1f"));
				hashtable["hotSelectedBackColor"] = StiReportHelper.GetHtmlColor(StiColor.Get("494949"));
			}
		}
		hashtable["separatorColor"] = StiReportHelper.GetHtmlColor(controlStyle.SeparatorColor);
		hashtable["isDarkStyle"] = flag;
		hashtable["styleName"] = StiDashboardStyleHelper.GetStyle(element).ToString();
		return hashtable;
	}

	public static Hashtable GetDashboardInteraction(object object_)
	{
		Hashtable interactionObject = new Hashtable();
		IStiDashboardInteraction stiDashboardInteraction = (object_ as IStiElementInteraction)?.DashboardInteraction;
		if (stiDashboardInteraction != null)
		{
			interactionObject["ident"] = stiDashboardInteraction.Ident;
			interactionObject["onHover"] = stiDashboardInteraction.OnHover;
			interactionObject["onClick"] = stiDashboardInteraction.OnClick;
			interactionObject["hyperlinkDestination"] = stiDashboardInteraction.HyperlinkDestination;
			interactionObject["toolTip"] = stiDashboardInteraction.ToolTip;
			interactionObject["hyperlink"] = GetHyperlinkText(stiDashboardInteraction.Hyperlink, object_);
			interactionObject["drillDownPageKey"] = stiDashboardInteraction.DrillDownPageKey;
			List<IStiDashboardDrillDownParameter> drillDownParameters = stiDashboardInteraction.GetDrillDownParameters();
			if (drillDownParameters != null)
			{
				interactionObject["drillDownParameters"] = new ArrayList();
				drillDownParameters.ForEach(delegate(IStiDashboardDrillDownParameter p)
				{
					Hashtable value = new Hashtable
					{
						["name"] = p.Name,
						["expression"] = p.Expression
					};
					((ArrayList)interactionObject["drillDownParameters"]).Add(value);
				});
			}
			if (stiDashboardInteraction is IStiAllowUserColumnSelectionDashboardInteraction)
			{
				interactionObject["allowUserColumnSelection"] = ((IStiAllowUserColumnSelectionDashboardInteraction)stiDashboardInteraction).AllowUserColumnSelection;
			}
			if (stiDashboardInteraction is IStiAllowUserSortingDashboardInteraction)
			{
				interactionObject["allowUserSorting"] = ((IStiAllowUserSortingDashboardInteraction)stiDashboardInteraction).AllowUserSorting;
			}
			if (stiDashboardInteraction is IStiAllowUserFilteringDashboardInteraction)
			{
				interactionObject["allowUserFiltering"] = ((IStiAllowUserFilteringDashboardInteraction)stiDashboardInteraction).AllowUserFiltering;
			}
			if (stiDashboardInteraction is IStiAllowUserDrillDownDashboardInteraction)
			{
				interactionObject["allowUserDrillDown"] = ((IStiAllowUserDrillDownDashboardInteraction)stiDashboardInteraction).AllowUserDrillDown;
			}
			if (stiDashboardInteraction is IStiInteractionLayout)
			{
				interactionObject["showFullScreenButton"] = ((IStiInteractionLayout)stiDashboardInteraction).ShowFullScreenButton;
				interactionObject["showSaveButton"] = ((IStiInteractionLayout)stiDashboardInteraction).ShowSaveButton;
				interactionObject["showViewDataButton"] = ((IStiInteractionLayout)stiDashboardInteraction).ShowViewDataButton;
			}
			if (stiDashboardInteraction is IStiTableDashboardInteraction)
			{
				interactionObject["drillDownFiltered"] = ((IStiTableDashboardInteraction)stiDashboardInteraction).DrillDownFiltered;
				interactionObject["fullRowSelect"] = ((IStiTableDashboardInteraction)stiDashboardInteraction).FullRowSelect;
			}
			if (object_ is IStiDrillDownElement stiDrillDownElement)
			{
				interactionObject["drillDownCurrentLevel"] = stiDrillDownElement.DrillDownCurrentLevel;
				interactionObject["drillDownLevelCount"] = stiDrillDownElement.DrillDownLevelCount;
				interactionObject["drillDownFilters"] = StiDataFiltersHelper.GetDrillDownFilters(stiDrillDownElement);
				interactionObject["drillDownFiltersList"] = StiDataFiltersHelper.GetDrillDownFiltersList(stiDrillDownElement);
			}
		}
		return interactionObject;
	}

	public static Hashtable GetShadow(IStiElement element)
	{
		StiSimpleShadow stiSimpleShadow = (element as IStiSimpleShadow)?.Shadow;
		if (stiSimpleShadow != null)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["visible"] = stiSimpleShadow.Visible;
			hashtable["color"] = StiReportHelper.GetHtmlColor(stiSimpleShadow.Color);
			hashtable["location"] = $"{stiSimpleShadow.Location.X};{stiSimpleShadow.Location.Y}";
			hashtable["size"] = stiSimpleShadow.Size.ToString();
			return hashtable;
		}
		return null;
	}

	public static Hashtable GetCornerRadius(IStiElement element)
	{
		StiCornerRadius stiCornerRadius = (element as IStiCornerRadius)?.CornerRadius;
		if (stiCornerRadius != null)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["topLeft"] = stiCornerRadius.TopLeft;
			hashtable["topRight"] = stiCornerRadius.TopRight;
			hashtable["bottomRight"] = stiCornerRadius.BottomRight;
			hashtable["bottomLeft"] = stiCornerRadius.BottomLeft;
			return hashtable;
		}
		return null;
	}

	private static string GetHyperlinkText(string hyperlink, object element)
	{
		try
		{
			if (!string.IsNullOrEmpty(hyperlink) && hyperlink.Contains("{") && hyperlink.Contains("}"))
			{
				string text = StiReportParser.Parse(hyperlink, element as StiComponent);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
		return hyperlink;
	}

	public static string Format(IStiControlElement element, object value)
	{
		if (value != null && element != null)
		{
			if (value is DateTime && !(element.TextFormat is StiDateFormatService))
			{
				return ((DateTime)value).ToShortDateString();
			}
			return element.TextFormat?.Format(value);
		}
		return string.Empty;
	}

	public static Dictionary<string, object> GetConstants(string value, ArrayList cells)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("Value", value);
		if (cells != null)
		{
			foreach (Hashtable cell in cells)
			{
				string key = "Row." + StiCodeDomSerializator.ReplaceSymbols((cell["owningColumnName"] != null) ? (cell["owningColumnName"] as string) : "");
				dictionary[key] = cell["value"] as string;
			}
		}
		return dictionary;
	}

	internal static void ParseDashboardDrillDownParameters(ArrayList drillDownParameters, StiReport report)
	{
		foreach (Hashtable p2 in drillDownParameters)
		{
			if (p2 == null)
			{
				continue;
			}
			Dictionary<string, object> constants = GetConstants(p2["value"] as string, p2["rowCels"] as ArrayList);
			IStiReportComponent stiReportComponent = (from p in report.Pages.ToList()
				select p.GetComponents().ToList().FirstOrDefault((StiComponent c) => c.Guid == p2["tableKey"] as string))?.FirstOrDefault();
			if (!(p2["parameters"] is ArrayList arrayList) || stiReportComponent == null)
			{
				continue;
			}
			foreach (Hashtable item in arrayList)
			{
				item["value"] = StiReportParser.Parse(item["value"] as string, stiReportComponent, allowCache: false, constants);
			}
		}
	}

	public static Hashtable GetDashboardWatermark(IStiElement element)
	{
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		StiAdvancedWatermark stiAdvancedWatermark = (element as IStiDashboardWatermark)?.DashboardWatermark;
		if (stiAdvancedWatermark != null)
		{
			Hashtable hashtable = new Hashtable();
			if (stiAdvancedWatermark.TextEnabled)
			{
				hashtable["textEnabled"] = stiAdvancedWatermark.TextEnabled;
				hashtable["text"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(stiAdvancedWatermark.Text));
				hashtable["textFont"] = GetFontJson(stiAdvancedWatermark.TextFont);
				hashtable["textColor"] = StiReportHelper.GetHtmlColor(stiAdvancedWatermark.TextColor);
				hashtable["textAngle"] = stiAdvancedWatermark.TextAngle;
			}
			if (stiAdvancedWatermark.ImageEnabled)
			{
				hashtable["imageEnabled"] = stiAdvancedWatermark.ImageEnabled;
				hashtable["image"] = ((stiAdvancedWatermark.Image != null) ? ImageToBase64(StiImageConverter.ImageToBytes(stiAdvancedWatermark.Image)) : string.Empty);
				hashtable["imageSize"] = ((stiAdvancedWatermark.Image != null) ? $"{stiAdvancedWatermark.Image.Width};{stiAdvancedWatermark.Image.Height}" : "0;0");
				hashtable["imageAlignment"] = stiAdvancedWatermark.ImageAlignment;
				hashtable["imageTransparency"] = stiAdvancedWatermark.ImageTransparency;
				hashtable["imageMultipleFactor"] = stiAdvancedWatermark.ImageMultipleFactor;
				hashtable["imageAspectRatio"] = stiAdvancedWatermark.ImageAspectRatio;
				hashtable["imageStretch"] = stiAdvancedWatermark.ImageStretch;
				hashtable["imageTiling"] = stiAdvancedWatermark.ImageTiling;
			}
			if (stiAdvancedWatermark.WeaveEnabled)
			{
				hashtable["weaveEnabled"] = stiAdvancedWatermark.WeaveEnabled;
				hashtable["weaveMajorIcon"] = stiAdvancedWatermark.WeaveMajorIcon;
				hashtable["weaveMajorColor"] = StiReportHelper.GetHtmlColor(stiAdvancedWatermark.WeaveMajorColor);
				hashtable["weaveMajorSize"] = stiAdvancedWatermark.WeaveMajorSize;
				hashtable["weaveMinorIcon"] = stiAdvancedWatermark.WeaveMinorIcon;
				hashtable["weaveMinorColor"] = StiReportHelper.GetHtmlColor(stiAdvancedWatermark.WeaveMinorColor);
				hashtable["weaveMinorSize"] = stiAdvancedWatermark.WeaveMinorSize;
				hashtable["weaveDistance"] = stiAdvancedWatermark.WeaveDistance;
				hashtable["weaveAngle"] = stiAdvancedWatermark.WeaveAngle;
			}
			if (stiAdvancedWatermark.WeaveEnabled && (stiAdvancedWatermark.WeaveMajorIcon.HasValue || stiAdvancedWatermark.WeaveMinorIcon.HasValue))
			{
				GetWeaveWatermarkImages(stiAdvancedWatermark, hashtable);
			}
			return hashtable;
		}
		return null;
	}

	private static void GetWeaveWatermarkImages(StiAdvancedWatermark watermark, Hashtable jsWatermark)
	{
		int num = 5 * watermark.WeaveMajorSize;
		int num2 = 5 * watermark.WeaveMinorSize;
		if (watermark.WeaveMajorIcon.HasValue)
		{
			Image val = StiFontIconsHelper.ConvertFontIconToImage(watermark.WeaveMajorIcon.Value, watermark.WeaveMajorColor, num, num);
			jsWatermark["weaveMajorImage"] = new Hashtable
			{
				["width"] = val.Width,
				["height"] = val.Height,
				["text"] = StiFontIconsHelper.GetContent(watermark.WeaveMajorIcon),
				["size"] = watermark.WeaveMajorSize,
				["color"] = StiReportHelper.GetHtmlColor(watermark.WeaveMajorColor)
			};
		}
		if (watermark.WeaveMinorIcon.HasValue)
		{
			Image val2 = StiFontIconsHelper.ConvertFontIconToImage(watermark.WeaveMinorIcon.Value, watermark.WeaveMinorColor, num2, num2);
			jsWatermark["weaveMinorImage"] = new Hashtable
			{
				["width"] = val2.Width,
				["height"] = val2.Height,
				["text"] = StiFontIconsHelper.GetContent(watermark.WeaveMinorIcon),
				["size"] = watermark.WeaveMinorSize,
				["color"] = StiReportHelper.GetHtmlColor(watermark.WeaveMinorColor)
			};
		}
	}

	private static string ImageToBase64(byte[] image)
	{
		string text = "data:image/png;base64,";
		if (StiImageHelper.IsMetafile(image))
		{
			text = "data:image/x-wmf;base64,";
		}
		else if (StiImageHelper.IsBmp(image))
		{
			text = "data:image/bmp;base64,";
		}
		else if (StiImageHelper.IsJpeg(image))
		{
			text = "data:image/jpeg;base64,";
		}
		else if (StiImageHelper.IsGif(image))
		{
			text = "data:image/gif;base64,";
		}
		else if (StiSvgHelper.IsSvg(image))
		{
			text = "data:image/svg+xml;base64,";
		}
		return text + Convert.ToBase64String(image);
	}
}
