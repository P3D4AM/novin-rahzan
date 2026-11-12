using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiDashboardsSvgHelper
{
	private static double GetSvgImageValue(string content, string name)
	{
		string text = content.Substring(content.IndexOf("<image"));
		string text2 = text.Substring(text.IndexOf(name) + name.Length + 2);
		text2 = text2.Substring(0, text2.IndexOf("\""));
		double.TryParse(text2, NumberStyles.Float, CultureInfo.InvariantCulture, out var result);
		return result;
	}

	private static string SaveElementToVectorString(IStiElement element, ref bool needToScroll, double scaleX, double scaleY, string content, bool needAnimation = false)
	{
		if (content == null || content.IndexOf("<image") < 0)
		{
			return content;
		}
		using MemoryStream memoryStream = new MemoryStream();
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		StiSvgData svgData = default(StiSvgData);
		svgData.X = GetSvgImageValue(content, "x");
		svgData.Y = GetSvgImageValue(content, "y");
		svgData.Width = GetSvgImageValue(content, "width");
		svgData.Height = GetSvgImageValue(content, "height");
		svgData.Component = (StiComponent)element;
		int contentHeight = 0;
		if (element is IStiProgressElement)
		{
			StiDashboardHelperCreator.CreateProgressVisualSvgHelper()?.WriteProgress(xmlTextWriter, svgData, ref needToScroll, ref contentHeight);
		}
		else if (element is IStiIndicatorElement)
		{
			StiDashboardHelperCreator.CreateIndicatorVisualSvgHelper()?.WriteIndicator(xmlTextWriter, svgData, ref needToScroll, ref contentHeight);
		}
		else if (element is IStiCardsElement)
		{
			StiDashboardHelperCreator.CreateCardsVisualSvgHelper()?.WriteCards(xmlTextWriter, svgData, ref needToScroll, ref contentHeight);
		}
		else if (element is IStiGaugeElement)
		{
			StiDashboardHelperCreator.CreateGaugeVisualSvgHelper()?.WriteGauge(xmlTextWriter, svgData, needAnimation, ref needToScroll, ref contentHeight);
		}
		xmlTextWriter.Flush();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		using StreamReader streamReader = new StreamReader(memoryStream);
		string text = streamReader.ReadToEnd();
		if (needToScroll)
		{
			return $"<svg x=\"0\" y=\"0\" width=\"{svgData.Width}\" height=\"{((contentHeight > 0) ? ((double)contentHeight) : svgData.Height)}\">{text}</svg>";
		}
		if (content.IndexOf("<image") >= 0)
		{
			string text2 = content.Substring(0, content.IndexOf("<image"));
			return text2 + text + "</svg>";
		}
		return content;
	}

	public static string SaveElementToString(IStiElement element, double scaleX = 1.0, double scaleY = 1.0, bool designMode = false, StiExportFormat exportFormat = StiExportFormat.ImageSvg, StiRequestParams requestParams = null)
	{
		bool needToScroll = false;
		return SaveElementToString(element, ref needToScroll, scaleX, scaleY, designMode, exportFormat, requestParams);
	}

	public static string SaveElementToString(IStiElement element, ref bool needToScroll, double scaleX = 1.0, double scaleY = 1.0, bool designMode = false, StiExportFormat exportFormat = StiExportFormat.ImageSvg, StiRequestParams requestParams = null)
	{
		object obj = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1] { exportFormat }, new Type[1] { typeof(StiExportFormat) });
		StiInvokeMethodsHelper.SetPropertyValue(obj, "RenderBorders", false);
		StiInvokeMethodsHelper.SetPropertyValue(obj, "RenderSinglePage", true);
		StiInvokeMethodsHelper.SetPropertyValue(obj, "RenderEmptyContent", !designMode || element is IStiTextElement || element is IStiOnlineMapElement);
		if (exportFormat == StiExportFormat.ImageSvg)
		{
			StiInvokeMethodsHelper.SetPropertyValue(obj, "DesignMode", designMode);
		}
		using StiReport stiReport = new StiReport();
		using MemoryStream memoryStream = new MemoryStream();
		stiReport.IsDocument = true;
		if (element.Report != null)
		{
			stiReport.Dictionary.Resources.AddRange(element.Report.Dictionary.Resources);
		}
		stiReport.ReportUnit = StiReportUnitType.HundredthsOfInch;
		if (!string.IsNullOrEmpty(element.Report.ReportGuid))
		{
			stiReport.ReportGuid = element.Report.ReportGuid;
		}
		StiComponent stiComponent = (StiComponent)element;
		StiMargin margin = ((IStiMargin)element).Margin;
		StiPage stiPage = stiReport.RenderedPages[0];
		stiPage.Margins = new StiMargins(0.0);
		stiPage.Width = Math.Round(stiComponent.Width * scaleX, MidpointRounding.AwayFromZero) - margin.Left - margin.Right;
		stiPage.Height = Math.Round(stiComponent.Height * scaleY, MidpointRounding.AwayFromZero) - margin.Top - margin.Bottom;
		Color backColor = StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity: true);
		bool flag = backColor.A != byte.MaxValue;
		stiPage.Brush = new StiSolidBrush(flag ? Color.Transparent : backColor);
		object obj2 = null;
		object obj3 = null;
		string text = ((element.Report != null && !string.IsNullOrEmpty(element.Report.ReportGuid)) ? (element.Report.ReportGuid + "_" + element.Name) : element.Name);
		if (!designMode && (element is IStiChartElement || element is IStiGaugeElement) && requestParams != null)
		{
			obj3 = StiChartElementViewHelper.GetChartAnimationsFromCache(text, requestParams);
			if (requestParams.Interaction.DashboardFiltering == null && requestParams.Interaction.DashboardSorting == null)
			{
				StiChartElementViewHelper.RemoveChartAnimationsFromCache(text, requestParams);
				obj3 = null;
			}
			if (requestParams.Interaction.DashboardFiltering == null)
			{
				requestParams.Cache.Helper.RemoveObject(text);
			}
			else if (obj3 == null)
			{
				obj2 = StiChartElementViewHelper.GetChartValuesFromCache(text, requestParams);
			}
		}
		if (obj3 == null)
		{
			obj3 = new List<StiAnimation>();
		}
		StiComponent stiComponent2 = (StiComponent)StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "StiDashboardExportTools", "RenderElement", new object[7] { stiPage, element, scaleX, scaleY, obj, obj2, obj3 }, new Type[7]
		{
			typeof(StiContainer),
			typeof(IStiElement),
			typeof(double),
			typeof(double),
			obj.GetType(),
			typeof(List<double?[]>),
			typeof(List<StiAnimation>)
		});
		stiComponent2.Left = 0.0;
		stiComponent2.Top = 0.0;
		foreach (StiPage finalPage in stiReport.RenderedPages)
		{
			finalPage.GetComponentsList().ForEach(delegate(StiComponent c)
			{
				c.Page = finalPage;
			});
			finalPage.MoveComponentsToPage();
			if (flag)
			{
				ApplyTransparencyToComponents(finalPage, backColor);
			}
			if (!designMode && element is IStiChartElement && requestParams != null)
			{
				StiChartElementViewHelper.SaveChartValuesToCache(text, finalPage, requestParams);
			}
		}
		if (exportFormat == StiExportFormat.Html5)
		{
			StiHtmlExportSettings settings = new StiHtmlExportSettings
			{
				ChartType = (designMode ? StiHtmlChartType.Vector : ((requestParams == null || requestParams.Viewer.ChartRenderType == StiChartRenderType.AnimatedVector) ? StiHtmlChartType.AnimatedVector : StiHtmlChartType.Vector)),
				ExportMode = StiHtmlExportMode.Div,
				AddPageBreaks = false
			};
			if (stiReport.GetComponentByName(element.Name + "_Title") is StiText stiText)
			{
				StiCornerRadius stiCornerRadius = (element as IStiCornerRadius)?.CornerRadius;
				if (stiCornerRadius != null)
				{
					if (stiCornerRadius.TopLeft > 0f)
					{
						stiText.Margins.Left = Math.Max(5.0, (double)stiCornerRadius.TopLeft / 1.5);
					}
					if (stiCornerRadius.TopRight > 0f)
					{
						stiText.Margins.Right = Math.Max(5.0, (double)stiCornerRadius.TopRight / 1.5);
					}
				}
			}
			stiReport.ExportDocument(StiExportFormat.Html, memoryStream, settings);
			foreach (StiPage renderedPage in stiReport.RenderedPages)
			{
				if (!designMode && (element is IStiChartElement || element is IStiGaugeElement) && requestParams != null)
				{
					StiChartElementViewHelper.SaveChartAnimationsToCache(element, text, renderedPage, requestParams);
				}
			}
			return Encoding.UTF8.GetString(memoryStream.ToArray()).Replace(">Trial<", "><");
		}
		StiSvgExportSettings settings2 = new StiSvgExportSettings();
		stiReport.ExportDocument(StiExportFormat.ImageSvg, memoryStream, settings2);
		string text2 = Encoding.UTF8.GetString(memoryStream.ToArray());
		if (element is IStiGaugeElement || element is IStiProgressElement || element is IStiIndicatorElement || element is IStiCardsElement)
		{
			if (element.GetMeters().Count <= 0)
			{
				IStiManuallyEnteredData obj4 = element as IStiManuallyEnteredData;
				if (obj4 == null || obj4.DataMode != StiDataMode.ManuallyEnteringData)
				{
					if (!designMode)
					{
						return string.Empty;
					}
					goto IL_060b;
				}
			}
			text2 = SaveElementToVectorString(element, ref needToScroll, scaleX, scaleY, text2, !designMode);
		}
		goto IL_060b;
		IL_060b:
		foreach (StiPage renderedPage2 in stiReport.RenderedPages)
		{
			if (!designMode && element is IStiGaugeElement && requestParams != null)
			{
				StiChartElementViewHelper.SaveChartAnimationsToCache(element, text, renderedPage2, requestParams);
			}
		}
		return (text2.IndexOf("<svg") >= 0) ? text2.Substring(text2.IndexOf("<svg")) : text2;
	}

	private static void ApplyTransparencyToComponents(StiPage page, Color elementBackColor)
	{
		if (elementBackColor.A == byte.MaxValue)
		{
			return;
		}
		page.Brush = new StiEmptyBrush();
		page.Components.ToList().ForEach(delegate(StiComponent comp)
		{
			StiMap stiMap = comp as StiMap;
			IStiBrush stiBrush = comp as IStiBrush;
			if (stiMap != null && StiBrush.ToColor(stiMap.Brush).Equals((object?)elementBackColor))
			{
				stiMap.Brush = new StiSolidBrush(Color.FromArgb(0, StiBrush.ToColor(stiMap.Brush)));
			}
			else if (stiBrush != null && StiBrush.ToColor(stiBrush.Brush).Equals((object?)elementBackColor))
			{
				stiBrush.Brush = new StiSolidBrush(Color.FromArgb(0, StiBrush.ToColor(stiBrush.Brush)));
			}
		});
	}
}
