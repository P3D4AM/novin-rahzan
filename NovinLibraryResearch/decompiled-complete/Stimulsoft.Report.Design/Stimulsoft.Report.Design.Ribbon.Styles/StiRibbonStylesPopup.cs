using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Design.Ribbon.Styles;

public sealed class StiRibbonStylesPopup
{
	[ToolboxItem(false)]
	internal class StiStylesPanel : Panel
	{
		public StiStylesPanel()
		{
			((Control)this).DoubleBuffered = true;
		}
	}

	public const int StyleItemWidth = 156;

	public const int StyleItemHeight = 38;

	public const int StyleItemBigHeight = 60;

	private bool isDisposed;

	private StiDesignerControl designerControl;

	private StiStyleItem button;

	private StiPopup popup;

	private StiStylesPanel panel;

	private int columns;

	private CircularProgress progressOperation;

	private List<string> collectionNames = new List<string>();

	private List<StiComponentStyleItem> galleryStyles = new List<StiComponentStyleItem>();

	private List<Control> allControls = new List<Control>();

	public StiRibbonStylesPopup(StiDesignerControl designerControl, StiStyleItem button)
	{
		this.designerControl = designerControl;
		this.button = button;
	}

	private List<StiCrossTabStyle> GetCrossTabStyles()
	{
		List<StiCrossTabStyle> list = new List<StiCrossTabStyle>();
		Color[] styleColors = StiOptions.Designer.CrossTab.StyleColors;
		foreach (Color color in styleColors)
		{
			list.Add(new StiCrossTabStyle
			{
				Color = color
			});
		}
		return list;
	}

	private List<string> GetStylesCollectionName()
	{
		if (designerControl.Report == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		foreach (StiBaseStyle style in designerControl.Report.Styles)
		{
			if (!string.IsNullOrEmpty(style.CollectionName) && !list.Contains(style.CollectionName))
			{
				list.Add(style.CollectionName);
			}
		}
		return list;
	}

	private StiBaseStyle GetAutoStyle(bool isGaugeElementOnly, bool isChartElementOnly, bool isMapRegionElementOnly, bool isIndicatorElement, bool isControlElement, bool isProgressElement, bool isTableElementOnly, bool isPivotElementOnly, bool isCardsElementOnly)
	{
		StiBaseStyle stiBaseStyle = null;
		IStiDashboardElementStyle stiDashboardElementStyle = designerControl.Report.GetCurrentPage() as IStiDashboardElementStyle;
		StiElementStyleIdent style = stiDashboardElementStyle.Style;
		if (isGaugeElementOnly)
		{
			foreach (StiGaugeStyleXF item in StiOptions.Services.GaugeStyles.Where((StiGaugeStyleXF x) => x.AllowDashboard))
			{
				if (item.StyleIdent == style)
				{
					stiBaseStyle = item;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.GaugeStyles.Where((StiGaugeStyleXF x) => x.AllowDashboard).FirstOrDefault();
			}
		}
		else if (isChartElementOnly)
		{
			foreach (Stimulsoft.Report.Chart.StiChartStyle item2 in StiOptions.Services.ChartStyles.Where((Stimulsoft.Report.Chart.StiChartStyle x) => x.AllowDashboard))
			{
				if (item2.StyleIdent == style)
				{
					stiBaseStyle = item2;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.ChartStyles.Where((Stimulsoft.Report.Chart.StiChartStyle x) => x.AllowDashboard).FirstOrDefault();
			}
		}
		else if (isMapRegionElementOnly)
		{
			foreach (StiMapStyleFX item3 in StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard))
			{
				if (item3.StyleIdent == style)
				{
					stiBaseStyle = item3;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard).FirstOrDefault();
			}
		}
		else if (isIndicatorElement)
		{
			foreach (StiIndicatorElementStyle indicatorStyle in StiOptions.Services.Dashboards.IndicatorStyles)
			{
				if (indicatorStyle.Ident == style)
				{
					stiBaseStyle = indicatorStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.IndicatorStyles.FirstOrDefault();
			}
		}
		else if (isControlElement)
		{
			foreach (StiControlElementStyle controlStyle in StiOptions.Services.Dashboards.ControlStyles)
			{
				if (controlStyle.Ident == style)
				{
					stiBaseStyle = controlStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.ControlStyles.FirstOrDefault();
			}
		}
		else if (isProgressElement)
		{
			foreach (StiProgressElementStyle progressStyle in StiOptions.Services.Dashboards.ProgressStyles)
			{
				if (progressStyle.Ident == style)
				{
					stiBaseStyle = progressStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.ProgressStyles.FirstOrDefault();
			}
		}
		else if (isTableElementOnly)
		{
			foreach (StiTableElementStyle tableStyle in StiOptions.Services.Dashboards.TableStyles)
			{
				if (tableStyle.Ident == style)
				{
					stiBaseStyle = tableStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.TableStyles.FirstOrDefault();
			}
		}
		else if (isPivotElementOnly)
		{
			foreach (StiPivotElementStyle pivotStyle in StiOptions.Services.Dashboards.PivotStyles)
			{
				if (pivotStyle.Ident == style)
				{
					stiBaseStyle = pivotStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.PivotStyles.FirstOrDefault();
			}
		}
		else if (isCardsElementOnly)
		{
			foreach (StiCardsElementStyle cardsStyle in StiOptions.Services.Dashboards.CardsStyles)
			{
				if (cardsStyle.Ident == style)
				{
					stiBaseStyle = cardsStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.CardsStyles.FirstOrDefault();
			}
		}
		return stiBaseStyle;
	}

	private void Build()
	{
		if (designerControl.Report == null || designerControl.SelectedComponentsOnPage == null)
		{
			return;
		}
		StiReport report = designerControl.Report;
		List<StiComponent> list = designerControl.SelectedComponentsOnPage.Cast<StiComponent>().ToList();
		if (StiInplaceDesigner.IsEditorActivated && StiInplaceDesigner.EditableComponent != null)
		{
			list = new List<StiComponent> { StiInplaceDesigner.EditableComponent };
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = true;
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		bool flag11 = false;
		bool flag12 = false;
		bool flag13 = false;
		bool flag14 = false;
		bool flag15 = false;
		bool flag16 = false;
		bool flag17 = false;
		bool flag18 = false;
		bool flag19 = false;
		bool flag20 = false;
		bool flag21 = false;
		bool flag22 = false;
		StiProgressElementMode progressMode = StiProgressElementMode.Circle;
		if (list.Count > 0)
		{
			flag = list.All((StiComponent c) => c is StiCrossTab);
			flag2 = list.All((StiComponent c) => c is StiChart);
			flag3 = list.All((StiComponent c) => c is StiGauge);
			flag4 = list.All((StiComponent c) => c is StiMap);
			flag6 = list.All((StiComponent c) => c is IStiTableComponent) && list.FirstOrDefault((StiComponent c) => c is StiTable) != null;
			flag5 = list.All((StiComponent c) => c is StiReportControl);
			flag10 = list.All((StiComponent c) => c is IStiChartElement);
			flag11 = list.All((StiComponent c) => c is IStiGaugeElement);
			flag12 = list.All((StiComponent c) => c is IStiRegionMapElement);
			flag13 = list.All((StiComponent c) => c is IStiOnlineMapElement);
			flag14 = list.All((StiComponent c) => c is IStiTableElement);
			flag9 = list.All((StiComponent c) => c is IStiCardsElement);
			flag16 = list.All((StiComponent c) => c is IStiIndicatorElement);
			flag17 = list.All((StiComponent c) => c is IStiControlElement);
			flag18 = list.All((StiComponent c) => c is IStiProgressElement);
			flag19 = list.All((StiComponent c) => c is IStiTextElement);
			flag15 = list.All((StiComponent c) => c is IStiPivotTableElement);
			flag22 = list.All((StiComponent c) => c is StiSparkline);
			flag8 = list.All((StiComponent c) => c is IStiComponentUI && c is IStiDashboardElementStyle);
			flag7 = list.All((StiComponent c) => !(c is StiCrossTab) && !(c is StiChart) && !(c is StiGauge) && !(c is StiMap) && !(c is StiSparkline) && !(c is StiReportControl) && !(c is IStiTableComponent) && !(c is IStiChartElement) && !(c is IStiGaugeElement) && !(c is IStiRegionMapElement) && !(c is IStiTableElement) && !(c is IStiCardsElement) && !(c is IStiPivotTableElement) && !(c is IStiControlElement) && !(c is IStiIndicatorElement) && !(c is IStiProgressElement) && !(c is IStiComponentUI));
		}
		else
		{
			if (designerControl.Report.GetCurrentPage() is IStiDashboard)
			{
				flag20 = true;
			}
			if (designerControl.Report.GetCurrentPage() is IStiScreen)
			{
				flag21 = true;
			}
		}
		string text = null;
		if (flag7)
		{
			foreach (StiComponent item4 in list)
			{
				if (text == null)
				{
					text = item4.ComponentStyle.Trim();
				}
				else if (text != item4.ComponentStyle)
				{
					text = null;
					break;
				}
			}
		}
		List<StiBaseStyle> list2 = new List<StiBaseStyle>();
		StiGaugeType? gaugeType = null;
		string mapIdent = "USA";
		StiFontIcons indicatorFontIcons = StiFontIcons.Rating4;
		StiBaseStyle stiBaseStyle = null;
		bool isSelected = false;
		bool flag23 = false;
		if (flag2)
		{
			StiChart stiChart = (StiChart)list[0];
			foreach (StiBaseStyle style in designerControl.Report.Styles)
			{
				if (style is StiChartStyle)
				{
					StiCustomStyle stiCustomStyle = new StiCustomStyle(style.Name);
					stiCustomStyle.CustomCore.ReportChartStyle = style as StiChartStyle;
					list2.Add(stiCustomStyle);
					if (stiChart.CustomStyleName == style.Name)
					{
						stiBaseStyle = stiCustomStyle;
					}
				}
			}
			list2.AddRange(StiOptions.Services.ChartStyles.OrderByDescending((Stimulsoft.Report.Chart.StiChartStyle x) => x.GetType().Name));
			if (stiBaseStyle == null)
			{
				foreach (Stimulsoft.Report.Chart.StiChartStyle chartStyle in StiOptions.Services.ChartStyles)
				{
					if (stiChart.Style.Equals(chartStyle))
					{
						stiBaseStyle = chartStyle;
						break;
					}
				}
			}
			StiComponentStyleItem.GlobalChart = StiSimpleChartHelper.GetSimpleChart(stiChart);
			StiStyleItem.GlobalChart = StiSimpleChartHelper.GetSimpleChart(stiChart);
		}
		else if (flag6)
		{
			StiTable stiTable = (StiTable)list[0];
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiTableStyle>());
			if (!string.IsNullOrEmpty(stiTable.ComponentStyle))
			{
				foreach (StiBaseStyle item5 in list2)
				{
					if (item5.Name == stiTable.ComponentStyle)
					{
						stiBaseStyle = item5;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.TableStyles);
			if (stiBaseStyle == null && stiTable.TableStyleFX != null)
			{
				foreach (StiTableStyleFX tableStyle in StiOptions.Services.TableStyles)
				{
					if (tableStyle.StyleId == stiTable.TableStyleFX.StyleId)
					{
						stiBaseStyle = tableStyle;
						break;
					}
				}
			}
		}
		else if (flag4)
		{
			StiMap stiMap = (StiMap)list[0];
			mapIdent = stiMap.MapIdent;
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiMapStyle>());
			if (!string.IsNullOrEmpty(stiMap.ComponentStyle))
			{
				foreach (StiBaseStyle item6 in list2)
				{
					if (item6.Name == stiMap.ComponentStyle)
					{
						stiBaseStyle = item6;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard));
			if (stiBaseStyle == null)
			{
				foreach (StiMapStyleFX item7 in StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard))
				{
					if (item7.StyleId == stiMap.MapStyle)
					{
						stiBaseStyle = item7;
						break;
					}
				}
			}
		}
		else if (flag)
		{
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiCrossTabStyle>());
			StiCrossTab stiCrossTab = (StiCrossTab)list[0];
			List<StiCrossTabStyle> crossTabStyles = GetCrossTabStyles();
			int num = 0;
			foreach (StiCrossTabStyle item8 in crossTabStyles)
			{
				if (num == stiCrossTab.CrossTabStyleIndex)
				{
					stiBaseStyle = item8;
					break;
				}
				num++;
			}
			if (!string.IsNullOrEmpty(stiCrossTab.CrossTabStyle))
			{
				foreach (StiBaseStyle item9 in list2)
				{
					if (item9.Name == stiCrossTab.CrossTabStyle)
					{
						stiBaseStyle = item9;
						break;
					}
				}
			}
			list2.AddRange(crossTabStyles);
		}
		else if (flag3)
		{
			StiGauge stiGauge = (StiGauge)list[0];
			if (StiGaugeV2InitHelper.IsGaugeV2(stiGauge))
			{
				gaugeType = stiGauge.Type;
			}
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiGaugeStyle>());
			if (!string.IsNullOrEmpty(stiGauge.CustomStyleName))
			{
				foreach (StiBaseStyle item10 in list2)
				{
					if (item10.Name == stiGauge.CustomStyleName)
					{
						stiBaseStyle = item10;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.GaugeStyles);
			if (stiBaseStyle == null && (stiGauge.Style != null || stiGauge.CustomStyleName != null))
			{
				foreach (StiGaugeStyleXF gaugeStyle in StiOptions.Services.GaugeStyles)
				{
					if (!string.IsNullOrEmpty(gaugeStyle.Name) && !string.IsNullOrEmpty(stiGauge.CustomStyleName) && gaugeStyle.Name == stiGauge.CustomStyleName)
					{
						stiBaseStyle = gaugeStyle;
						break;
					}
					if (stiGauge.Style != null && gaugeStyle.GetType() == stiGauge.Style.GetType())
					{
						stiBaseStyle = gaugeStyle;
						break;
					}
				}
			}
		}
		else if (flag22)
		{
			StiSparkline stiSparkline = (StiSparkline)list[0];
			indicatorFontIcons = StiFontIcons.LineChart;
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiIndicatorStyle>());
			if (!string.IsNullOrEmpty(stiSparkline.ComponentStyle))
			{
				foreach (StiBaseStyle item11 in list2)
				{
					if (item11.Name == stiSparkline.ComponentStyle)
					{
						stiBaseStyle = item11;
						break;
					}
				}
			}
		}
		else if (flag11)
		{
			flag23 = true;
			IStiGaugeElement stiGaugeElement = (IStiGaugeElement)list[0];
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiGaugeStyle>());
			if (!string.IsNullOrEmpty(stiGaugeElement.CustomStyleName))
			{
				foreach (StiBaseStyle item12 in list2)
				{
					if (item12.Name == stiGaugeElement.CustomStyleName)
					{
						stiBaseStyle = item12;
						break;
					}
				}
			}
			list2.AddRange(from x in StiOptions.Services.GaugeStyles
				orderby x.GetType() != typeof(StiGaugeStyleXF29)
				where x.AllowDashboard
				select x);
			gaugeType = stiGaugeElement.Type;
			if (stiBaseStyle == null)
			{
				if (stiGaugeElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiGaugeStyleXF item13 in StiOptions.Services.GaugeStyles.Where((StiGaugeStyleXF x) => x.AllowDashboard))
					{
						if (item13.StyleIdent == stiGaugeElement.Style)
						{
							stiBaseStyle = item13;
							break;
						}
					}
				}
			}
		}
		else if (flag10)
		{
			flag23 = true;
			IStiChartElement stiChartElement = (IStiChartElement)list[0];
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiChartStyle>());
			if (!string.IsNullOrEmpty(stiChartElement.CustomStyleName))
			{
				foreach (StiBaseStyle item14 in list2)
				{
					if (item14.Name == stiChartElement.CustomStyleName)
					{
						stiBaseStyle = item14;
						break;
					}
				}
			}
			IEnumerable<Stimulsoft.Report.Chart.StiChartStyle> enumerable = from x in StiOptions.Services.ChartStyles
				orderby x.GetType() != typeof(StiStyle29)
				where x.AllowDashboard
				select x;
			list2.AddRange(enumerable);
			if (stiBaseStyle == null)
			{
				if (stiChartElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (Stimulsoft.Report.Chart.StiChartStyle item15 in enumerable)
					{
						if (item15.StyleIdent == stiChartElement.Style)
						{
							stiBaseStyle = item15;
							break;
						}
					}
				}
			}
			StiComponentStyleItem.GlobalChart = StiSimpleChartHelper.GetSimpleChart(stiChartElement);
			StiStyleItem.GlobalChart = StiSimpleChartHelper.GetSimpleChart(stiChartElement);
		}
		else if (flag12)
		{
			flag23 = true;
			IStiRegionMapElement stiRegionMapElement = (IStiRegionMapElement)list[0];
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiMapStyle>());
			if (!string.IsNullOrEmpty(stiRegionMapElement.CustomStyleName))
			{
				foreach (StiBaseStyle item16 in list2)
				{
					if (item16.Name == stiRegionMapElement.CustomStyleName)
					{
						stiBaseStyle = item16;
						break;
					}
				}
			}
			list2.AddRange(from x in StiOptions.Services.MapStyles
				orderby x.GetType() != typeof(StiMap29StyleFX)
				where x.AllowDashboard
				select x);
			mapIdent = stiRegionMapElement.MapIdent;
			if (stiBaseStyle == null)
			{
				if (stiRegionMapElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiMapStyleFX item17 in StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard))
					{
						if (item17.StyleIdent == stiRegionMapElement.Style)
						{
							stiBaseStyle = item17;
							break;
						}
					}
				}
			}
		}
		else if (flag13)
		{
			flag23 = true;
		}
		else if (flag16)
		{
			flag23 = true;
			IStiIndicatorElement stiIndicatorElement = (IStiIndicatorElement)list[0];
			indicatorFontIcons = stiIndicatorElement.Icon;
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiIndicatorStyle>());
			if (!string.IsNullOrEmpty(stiIndicatorElement.CustomStyleName))
			{
				foreach (StiBaseStyle item18 in list2)
				{
					if (item18.Name == stiIndicatorElement.CustomStyleName)
					{
						stiBaseStyle = item18;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.Dashboards.IndicatorStyles);
			if (stiBaseStyle == null)
			{
				if (stiIndicatorElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiIndicatorElementStyle indicatorStyle in StiOptions.Services.Dashboards.IndicatorStyles)
					{
						if (indicatorStyle.Ident == stiIndicatorElement.Style)
						{
							stiBaseStyle = indicatorStyle;
							break;
						}
					}
				}
			}
		}
		else if (flag17)
		{
			flag23 = true;
			IStiControlElement stiControlElement = (IStiControlElement)list[0];
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiDialogStyle>());
			if (!string.IsNullOrEmpty(stiControlElement.CustomStyleName))
			{
				foreach (StiBaseStyle item19 in list2)
				{
					if (item19.Name == stiControlElement.CustomStyleName)
					{
						stiBaseStyle = item19;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.Dashboards.ControlStyles);
			if (stiBaseStyle == null)
			{
				if (stiControlElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiControlElementStyle controlStyle in StiOptions.Services.Dashboards.ControlStyles)
					{
						if (controlStyle.Ident == stiControlElement.Style)
						{
							stiBaseStyle = controlStyle;
							break;
						}
					}
				}
			}
		}
		else if (flag18)
		{
			flag23 = true;
			IStiProgressElement stiProgressElement = (IStiProgressElement)list[0];
			progressMode = stiProgressElement.Mode;
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiProgressStyle>());
			if (!string.IsNullOrEmpty(stiProgressElement.CustomStyleName))
			{
				foreach (StiBaseStyle item20 in list2)
				{
					if (item20.Name == stiProgressElement.CustomStyleName)
					{
						stiBaseStyle = item20;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.Dashboards.ProgressStyles);
			if (stiBaseStyle == null)
			{
				if (stiProgressElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiProgressElementStyle progressStyle in StiOptions.Services.Dashboards.ProgressStyles)
					{
						if (progressStyle.Ident == stiProgressElement.Style)
						{
							stiBaseStyle = progressStyle;
							break;
						}
					}
				}
			}
		}
		else if (flag14)
		{
			flag23 = true;
			IStiTableElement stiTableElement = (IStiTableElement)list[0];
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiTableStyle>());
			if (!string.IsNullOrEmpty(stiTableElement.CustomStyleName))
			{
				foreach (StiBaseStyle item21 in list2)
				{
					if (item21.Name == stiTableElement.CustomStyleName)
					{
						stiBaseStyle = item21;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.Dashboards.TableStyles);
			if (stiBaseStyle == null)
			{
				if (stiTableElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiTableElementStyle tableStyle2 in StiOptions.Services.Dashboards.TableStyles)
					{
						if (tableStyle2.Ident == stiTableElement.Style)
						{
							stiBaseStyle = tableStyle2;
							break;
						}
					}
				}
			}
		}
		else if (flag9)
		{
			flag23 = true;
			IStiCardsElement stiCardsElement = (IStiCardsElement)list[0];
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiCardsStyle>());
			if (!string.IsNullOrEmpty(stiCardsElement.CustomStyleName))
			{
				foreach (StiBaseStyle item22 in list2)
				{
					if (item22.Name == stiCardsElement.CustomStyleName)
					{
						stiBaseStyle = item22;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.Dashboards.CardsStyles);
			if (stiBaseStyle == null)
			{
				if (stiCardsElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiCardsElementStyle cardsStyle in StiOptions.Services.Dashboards.CardsStyles)
					{
						if (cardsStyle.Ident == stiCardsElement.Style)
						{
							stiBaseStyle = cardsStyle;
							break;
						}
					}
				}
			}
		}
		else if (flag19)
		{
			flag23 = true;
		}
		else if (flag15)
		{
			flag23 = true;
			IStiPivotTableElement stiPivotTableElement = (IStiPivotTableElement)list[0];
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiCrossTabStyle>());
			if (!string.IsNullOrEmpty(stiPivotTableElement.CustomStyleName))
			{
				foreach (StiBaseStyle item23 in list2)
				{
					if (item23.Name == stiPivotTableElement.CustomStyleName)
					{
						stiBaseStyle = item23;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.Dashboards.PivotStyles);
			if (stiBaseStyle == null)
			{
				if (stiPivotTableElement.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiPivotElementStyle pivotStyle in StiOptions.Services.Dashboards.PivotStyles)
					{
						if (pivotStyle.Ident == stiPivotTableElement.Style)
						{
							stiBaseStyle = pivotStyle;
							break;
						}
					}
				}
			}
		}
		else if (flag20)
		{
			list2.AddRange(StiOptions.Services.Dashboards.DashboardStyles);
			IStiDashboard stiDashboard = (IStiDashboard)designerControl.Report.GetCurrentPage();
			foreach (StiDashboardStyle dashboardStyle in StiOptions.Services.Dashboards.DashboardStyles)
			{
				if (dashboardStyle.Ident == stiDashboard.Style)
				{
					stiBaseStyle = dashboardStyle;
					break;
				}
			}
		}
		else if (flag8)
		{
			flag23 = true;
			IStiDashboardElementStyle stiDashboardElementStyle = list[0] as IStiDashboardElementStyle;
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>().OfType<StiDialogStyle>());
			if (!string.IsNullOrEmpty(stiDashboardElementStyle.CustomStyleName))
			{
				foreach (StiBaseStyle item24 in list2)
				{
					if (item24.Name == stiDashboardElementStyle.CustomStyleName)
					{
						stiBaseStyle = item24;
						break;
					}
				}
			}
			list2.AddRange(StiOptions.Services.Dashboards.ControlStyles);
			if (stiBaseStyle == null)
			{
				if (stiDashboardElementStyle.Style == StiElementStyleIdent.Auto)
				{
					isSelected = true;
				}
				else
				{
					foreach (StiControlElementStyle controlStyle2 in StiOptions.Services.Dashboards.ControlStyles)
					{
						if (controlStyle2.Ident == stiDashboardElementStyle.Style)
						{
							stiBaseStyle = controlStyle2;
							break;
						}
					}
				}
			}
		}
		else if (flag21)
		{
			list2.AddRange(StiOptions.Services.Dashboards.DashboardStyles);
			IStiScreen stiScreen = (IStiScreen)designerControl.Report.GetCurrentPage();
			foreach (StiDashboardStyle dashboardStyle2 in StiOptions.Services.Dashboards.DashboardStyles)
			{
				if (dashboardStyle2.Ident == stiScreen.Style)
				{
					stiBaseStyle = dashboardStyle2;
					break;
				}
			}
		}
		else
		{
			list2.AddRange(designerControl.Report.Styles.Cast<StiBaseStyle>());
			if (list.Count > 0)
			{
				string compStyleName = list[0].ComponentStyle;
				stiBaseStyle = list2.FirstOrDefault((StiBaseStyle x) => x.Name == compStyleName);
			}
		}
		bool isDashboardPage = report.GetCurrentPage() is IStiDashboard;
		bool isScreenPage = report.GetCurrentPage() is IStiScreen;
		collectionNames = GetStylesCollectionName();
		if (flag23)
		{
			StiBaseStyle autoStyle = GetAutoStyle(flag11, flag10, flag12, flag16, flag17 || flag8, flag18, flag14, flag15, flag9);
			StiComponentStyleItem item = new StiComponentStyleItem(isDashboardPage)
			{
				Width = StiScale.XXI(156.0),
				Height = StiScale.YYI(60.0),
				MapIdent = mapIdent,
				Report = report,
				ProgressMode = progressMode,
				IndicatorFontIcons = indicatorFontIcons,
				GaugeType = gaugeType,
				IsSelected = isSelected,
				IsDashboardAutoStyle = true,
				SelectedStyle = autoStyle
			};
			galleryStyles.Add(item);
		}
		else if ((flag7 || flag22) && !flag20 && !flag21)
		{
			StiComponentStyleItem item2 = new StiComponentStyleItem
			{
				Width = StiScale.XXI(156.0),
				Height = StiScale.YYI(38.0),
				IsNoItem = true,
				ProgressMode = progressMode,
				IsSelected = (text != null && text.Trim().Length == 0)
			};
			galleryStyles.Add(item2);
		}
		foreach (StiBaseStyle item25 in list2)
		{
			if ((!flag || item25 is StiCrossTabStyle) && (!flag22 || item25 is StiIndicatorStyle) && (!flag2 || item25 != null) && (!flag3 || item25 != null) && (!flag4 || item25 is StiMapStyle) && (!flag6 || item25 is StiTableStyle) && (!flag5 || item25 is StiDialogStyle) && (!flag7 || item25 is StiStyle || flag20 || flag21))
			{
				bool isSelected2 = stiBaseStyle?.Equals(item25) ?? false;
				StiComponentStyleItem item3 = new StiComponentStyleItem(isDashboardPage, isScreenPage)
				{
					Width = StiScale.XXI(156.0),
					Height = (((flag7 || flag22) && !flag20 && !flag21) ? StiScale.YYI(38.0) : StiScale.YYI(60.0)),
					MapIdent = mapIdent,
					Report = report,
					ProgressMode = progressMode,
					IndicatorFontIcons = indicatorFontIcons,
					GaugeType = gaugeType,
					IsSelected = isSelected2,
					Text = item25.Name,
					SelectedStyle = item25
				};
				galleryStyles.Add(item3);
			}
		}
	}

	private int CalculateSize()
	{
		int num = StiScale.YYI(2.0);
		if (collectionNames != null && collectionNames.Count > 0)
		{
			int num2 = collectionNames.Count / columns;
			if (collectionNames.Count % columns > 0)
			{
				num2++;
			}
			num += StiScale.YYI(60 * num2 + 18);
		}
		int num3 = 0;
		int num4 = 0;
		foreach (StiComponentStyleItem galleryStyle in galleryStyles)
		{
			if (num3 == columns)
			{
				num3 = 0;
				num += num4;
			}
			if (galleryStyle.IsNoItem)
			{
				num3 = 0;
				num += StiScale.YYI(galleryStyle.IsDashboardAutoStyle ? 60 : 38);
				if (galleryStyles.Count > 1)
				{
					num += StiScale.YYI(15.0);
				}
			}
			else
			{
				num4 = galleryStyle.Height;
				num3++;
			}
		}
		if (num3 > 0)
		{
			num += num4;
		}
		return num + StiScale.YYI(2.0);
	}

	private async Task ViewAsync()
	{
		int xPos2 = StiScale.XXI(2.0);
		int yPos4 = StiScale.YYI(2.0);
		int col2 = 0;
		if (collectionNames != null && collectionNames.Count > 0)
		{
			if (isDisposed)
			{
				return;
			}
			for (int i = 0; i < collectionNames.Count; i++)
			{
				string text = collectionNames[i];
				if (col2 == columns)
				{
					col2 = 0;
					xPos2 = StiScale.XXI(2.0);
					yPos4 += StiScale.YYI(60.0);
				}
				StiCollectionNameStyleButton stiCollectionNameStyleButton = new StiCollectionNameStyleButton(text);
				((Control)stiCollectionNameStyleButton).Tag = text;
				((Control)stiCollectionNameStyleButton).Location = new Point(xPos2, yPos4);
				((Control)stiCollectionNameStyleButton).Width = StiScale.XXI(156.0);
				((Control)stiCollectionNameStyleButton).Height = StiScale.YYI(60.0);
				StiCollectionNameStyleButton stiCollectionNameStyleButton2 = stiCollectionNameStyleButton;
				((Control)stiCollectionNameStyleButton2).Click += ApplyCollectionNameClick;
				if (isDisposed)
				{
					return;
				}
				allControls.Add((Control)(object)stiCollectionNameStyleButton2);
				col2++;
				xPos2 += StiScale.XXI(158.0);
			}
			yPos4 += StiScale.YYI(65.0);
			if (galleryStyles.Count > 0)
			{
				Panel val = new Panel
				{
					Width = ((Control)panel).Width - StiScale.XXI(2.0),
					Height = StiScale.YYI(5.0),
					Location = new Point(StiScale.XXI(1.0), yPos4)
				};
				((Control)val).Paint += new PaintEventHandler(PanelSeparator_Paint);
				if (isDisposed)
				{
					return;
				}
				allControls.Add((Control)(object)val);
				yPos4 += StiScale.YYI(4.0);
			}
			yPos4 += StiScale.YYI(5.0);
			await Task.Delay(1);
		}
		xPos2 = StiScale.XXI(2.0);
		if (isDisposed)
		{
			return;
		}
		Point? noItemLinePosition = null;
		col2 = 0;
		int num = 0;
		for (int index = 0; index < galleryStyles.Count; index++)
		{
			StiComponentStyleItem item = galleryStyles[index];
			if (col2 == columns)
			{
				col2 = 0;
				xPos2 = StiScale.XXI(2.0);
				yPos4 += num;
				await Task.Delay(1);
			}
			StiComponentStyleButton stiComponentStyleButton = item.CreateButton(this);
			((Control)stiComponentStyleButton).Location = new Point(xPos2, yPos4);
			if (!item.IsDashboardAutoStyle && item.SelectedStyle != null)
			{
				item.InitImage(((Control)stiComponentStyleButton).ClientRectangle);
			}
			num = item.Height;
			if (isDisposed)
			{
				break;
			}
			allControls.Add((Control)(object)stiComponentStyleButton);
			if (item.IsSelected && noItemLinePosition.HasValue)
			{
				StiComponentStyleButton stiComponentStyleButton2 = item.CreateButton(this);
				((Control)stiComponentStyleButton2).Location = noItemLinePosition.GetValueOrDefault();
				allControls.Add((Control)(object)stiComponentStyleButton2);
				noItemLinePosition = null;
			}
			col2++;
			xPos2 += StiScale.XXI(158.0);
			if (galleryStyles.Count > 1 && index == 0 && item.IsNoItem)
			{
				noItemLinePosition = new Point(xPos2, yPos4);
				col2 = 0;
				xPos2 = StiScale.XXI(2.0);
				yPos4 += num;
				yPos4 += StiScale.YYI(5.0);
				Panel val2 = new Panel
				{
					Width = ((Control)panel).Width - StiScale.XXI(2.0),
					Height = StiScale.YYI(5.0),
					Location = new Point(StiScale.XXI(1.0), yPos4)
				};
				((Control)val2).Paint += new PaintEventHandler(PanelSeparator_Paint);
				if (isDisposed)
				{
					break;
				}
				allControls.Add((Control)(object)val2);
				yPos4 += StiScale.YYI(9.0);
			}
		}
	}

	public void Show()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		Rectangle screenRectangle = button.ScreenRectangle;
		try
		{
			Build();
		}
		catch
		{
			return;
		}
		int num = Math.Max(galleryStyles.Count, collectionNames.Count);
		if (num < 3)
		{
			columns = 1;
		}
		else if (num < 7)
		{
			columns = 2;
		}
		else
		{
			columns = 3;
		}
		StiStylesPanel stiStylesPanel = new StiStylesPanel();
		((Control)stiStylesPanel).BackColor = Color.White;
		panel = stiStylesPanel;
		((ScrollableControl)panel).Scroll += new ScrollEventHandler(Panel_Scroll);
		int num2 = screenRectangle.Bottom + StiScale.I2;
		int num3 = CalculateSize();
		int num4 = Screen.PrimaryScreen.Bounds.Height - num2 - 30;
		int num5 = 0;
		if (num3 > num4)
		{
			((ScrollableControl)panel).AutoScroll = true;
			num3 = num4;
			num5 += 24;
		}
		((Control)panel).Width = StiScale.XXI(156 * columns + num5 + 4 + (columns - 1) * 2);
		((Control)panel).Height = num3;
		((Control)panel).Paint += new PaintEventHandler(Panel_Paint);
		popup = new StiPopup((Control)(object)panel);
		((ToolStripDropDown)popup).Closed += (ToolStripDropDownClosedEventHandler)delegate
		{
			isDisposed = true;
		};
		if (galleryStyles.Count + collectionNames.Count > 15)
		{
			CircularProgress circularProgress = new CircularProgress();
			((Control)circularProgress).BackColor = Color.White;
			((Control)circularProgress).Margin = new Padding(6);
			circularProgress.ProgressBarType = eCircularProgressType.Donut;
			circularProgress.ProgressColor = Color.SteelBlue;
			((Control)circularProgress).Size = new Size(StiScale.XXI(52.0), StiScale.YYI(52.0));
			circularProgress.Style = eDotNetBarStyle.OfficeXP;
			progressOperation = circularProgress;
			progressOperation.BackgroundStyle.CornerType = eCornerType.Square;
			progressOperation.Value = 0;
			progressOperation.IsRunning = true;
			((Control)progressOperation).Location = new Point((((Control)panel).Width - ((Control)progressOperation).Width) / 2, (((Control)panel).Height - ((Control)progressOperation).Height) / 2);
			((Control)panel).Controls.Add((Control)(object)progressOperation);
		}
		Timer val = new Timer
		{
			Interval = 10
		};
		val.Tick += Timer_Tick;
		val.Start();
		((ToolStripDropDown)popup).Show(new Point(screenRectangle.Left + screenRectangle.Width - ((Control)popup).Width, num2));
	}

	private async void Timer_Tick(object sender, EventArgs e)
	{
		Timer val = (Timer)sender;
		val.Stop();
		try
		{
			await ViewAsync();
			if (progressOperation != null)
			{
				progressOperation.IsRunning = false;
				((Control)panel).Controls.Remove((Control)(object)progressOperation);
			}
			if (!isDisposed)
			{
				((Control)panel).Controls.AddRange(allControls.ToArray());
			}
		}
		catch
		{
		}
	}

	private void Panel_Scroll(object sender, ScrollEventArgs e)
	{
		((Control)panel).Invalidate();
	}

	private void Panel_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Rectangle clientRectangle = ((Control)val).ClientRectangle;
		e.Graphics.FillRectangle(Brushes.White, clientRectangle);
		Pen val2 = new Pen(Color.FromArgb(255, 198, 198, 198));
		try
		{
			e.Graphics.DrawRectangle(val2, 0, 0, ((Control)val).Width - 1, ((Control)val).Height - 1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void PanelSeparator_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Pen val2 = new Pen(Color.FromArgb(255, 198, 198, 198));
		try
		{
			val2.DashStyle = (DashStyle)1;
			e.Graphics.DrawLine(val2, 0, 3, ((Control)val).Width, 3);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void ApplyCollectionNameClick(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		((Control)popup).Hide();
		StiCollectionNameStyleButton stiCollectionNameStyleButton = (StiCollectionNameStyleButton)sender;
		if ((int)MessageBox.Show(StiLocalization.Get("FormStyleDesigner", "qnApplyStyleCollection"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4) == 6)
		{
			designerControl.UndoRedoSave(StiLocalization.Get("FormStyleDesigner", "ApplyStyleCollectionToReportComponents"));
			string collectionName = (string)((Control)stiCollectionNameStyleButton).Tag;
			designerControl.Report.ApplyStyleCollection(collectionName);
		}
	}

	internal void OnDashboardAutoStyleClick(object sender, EventArgs e)
	{
		((Control)popup).Hide();
		StiActions.Style.InvokeSelectDashboardAutoStyle(designerControl);
	}

	internal void OnStyleClick(object sender, EventArgs e)
	{
		((Control)popup).Hide();
		StiComponentStyleButton stiComponentStyleButton = (StiComponentStyleButton)sender;
		StiActions.Style.InvokeSelectStyle(designerControl, stiComponentStyleButton.SelectedStyle);
	}
}
