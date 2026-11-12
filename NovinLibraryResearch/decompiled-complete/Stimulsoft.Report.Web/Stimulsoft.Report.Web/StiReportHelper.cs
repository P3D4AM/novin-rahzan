using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Plans;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Surface;
using Stimulsoft.Report.Viewer;
using Stimulsoft.Report.Web.Helpers.Dashboards;

namespace Stimulsoft.Report.Web;

internal class StiReportHelper
{
	private class StiBookmarkTreeNode
	{
		public int Parent;

		public string Title;

		public string Url;

		public string ComponentGuid;

		public bool Used;
	}

	private static object lockObject = new object();

	public static string GetHtmlColor(Color color)
	{
		if (color.A < byte.MaxValue)
		{
			return string.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, ((float)(int)color.A / 255f).ToString().Replace(",", "."));
		}
		return "#" + color.R.ToString("x2") + color.G.ToString("x2") + color.B.ToString("x2");
	}

	public static StiReport GetCompiledReport(StiReport report)
	{
		if (report.CheckNeedsCompiling() && !report.IsCompiled && report.CalculationMode == StiCalculationMode.Compilation && !StiOptions.Engine.ForceInterpretationMode)
		{
			try
			{
				report.Compile();
			}
			catch
			{
			}
		}
		if (report.CompiledReport == null)
		{
			return report;
		}
		return report.CompiledReport;
	}

	public static string GetCompileErrorHtmlPage(StiReport report)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		if (report.CompilerResults == null || ((CollectionBase)(object)report.CompilerResults.Errors).Count == 0)
		{
			return null;
		}
		string text = report.ReportName;
		if (report.ReportAlias.Length > 0)
		{
			text = report.ReportAlias;
		}
		string empty = string.Empty;
		empty += "<html><head><title>Compilation Error</title><style>\n";
		empty += "span.header { font-family: Verdana; font-weight: normal; font-size: 18pt; color: Maroon }\n";
		empty += "span.status { font-family: Verdana; font-weight: normal; font-size: 9pt; color: #505050 }\n";
		empty += "table { border-right: 1px solid DarkGray; border-top: 1px solid DarkGray }\n";
		empty += "td.header { border-left: 1px solid DarkGray; border-bottom: 1px solid DarkGray; background-color: #e0e0e0; font-family: Verdana; font-size: 12pt; padding: 3px 0px 3px 0px; text-align: Center }\n";
		empty += "td.error { border-left: 1px solid DarkGray; border-bottom: 1px solid DarkGray; padding: 3px 0px 3px 4px; font-family: Verdana; font-weight: normal; font-size: 12pt; color: #266294 }\n";
		empty += "</style></head>\n";
		empty = empty + "<body bgcolor=\"White\"><span class=\"header\">Compilation Error in " + text + "</span><br><br>\n";
		empty += "<table cellspacing=0 cellpadding=0 border=0 width=\"100%\"><tr>\n";
		empty += "<td class=\"header\" width=\"30%\">File Name</td><td class=\"header\" width=\"10%\">Line, Column</td>\n";
		empty += "<td class=\"header\" width=\"10%\">Error Number</td><td class=\"header\" width=\"50%\">Error Text</td></tr>\n";
		foreach (CompilerError item in (CollectionBase)(object)report.CompilerResults.Errors)
		{
			CompilerError val = item;
			empty += $"<tr><td class=\"error\">{val.FileName}</td><td class=\"error\">{val.Line}, {val.Column}</td><td class=\"error\">{val.ErrorNumber}</td><td class=\"error\">{val.ErrorText}</td></tr>\n";
		}
		return empty + $"</table><br><span class=\"status\">Report {report.EngineVersion}, Version: {StiVersionHelper.ProductVersion}</span></body></html>\n";
	}

	public static CookieContainer GetCookieContainer()
	{
		if (HttpContext.Current != null)
		{
			return GetCookieContainer(HttpContext.Current.Request);
		}
		return null;
	}

	public static CookieContainer GetCookieContainer(HttpRequest request)
	{
		if (request != null && ((NameObjectCollectionBase)(object)request.Cookies).Count > 0)
		{
			CookieContainer cookieContainer = new CookieContainer();
			string[] allKeys = request.Cookies.AllKeys;
			foreach (string text in allKeys)
			{
				if (!string.IsNullOrEmpty(request.Cookies[text].Value))
				{
					Cookie cookie = new Cookie
					{
						Name = request.Cookies[text].Name,
						Value = HttpUtility.UrlEncode(request.Cookies[text].Value),
						Domain = request.Url.Host,
						Path = "/",
						Expires = DateTime.Now.AddYears(1)
					};
					cookieContainer.Add(cookie);
				}
			}
			if (cookieContainer.Count > 0)
			{
				return cookieContainer;
			}
		}
		return null;
	}

	public static string GetReportFileName(StiReport report)
	{
		string text = ((report.ReportAlias == null || report.ReportAlias.Trim().Length == 0) ? report.ReportName : report.ReportAlias);
		if (text == null || text.Trim().Length == 0)
		{
			if (report.ReportFile != null && report.ReportFile.Trim().Length > 0)
			{
				text = report.ReportFile.Replace(".mrt", "").Replace(".mrz", "").Replace(".mrx", "")
					.Replace(".mdc", "")
					.Replace(".mdz", "")
					.Replace(".mdx", "");
				text = text.Substring(text.LastIndexOf("/") + 1);
			}
			else
			{
				text = "Report";
			}
		}
		return string.Join("_", text.Split('<', '>', ':', ';', '"', '/', '\\', '|', '?', '*', '%'));
	}

	public static void ApplyQueryParameters(StiRequestParams requestParams, StiReport report)
	{
		if (!requestParams.Server.PassQueryParametersToReport || report == null || report.Dictionary.Variables.Count <= 0)
		{
			return;
		}
		foreach (string item in requestParams.HttpContext.Request.QueryString)
		{
			if (item != null && !item.StartsWith("stiweb_") && report.Dictionary.Variables.Contains(item))
			{
				if (report.IsCompiled)
				{
					report[item] = requestParams.HttpContext.Request.QueryString[item];
				}
				else
				{
					report.Dictionary.Variables[item].Value = requestParams.HttpContext.Request.QueryString[item];
				}
			}
		}
	}

	private static List<StiPage> GetNestedPages(StiReport report)
	{
		List<StiPage> list = report?.Pages.OfType<StiPage>().ToList();
		List<StiPage> nestedPages = new List<StiPage>();
		foreach (StiPage item in list)
		{
			IEnumerable<StiComponent> enumerable = from c in item.GetComponents().ToList()
				where c is IStiElement
				select c;
			foreach (IStiElement item2 in enumerable)
			{
				List<StiPage> nestedPages2 = item2.GetNestedPages();
				if (nestedPages2 != null)
				{
					nestedPages.AddRange(nestedPages2);
				}
			}
		}
		lock (lockObject)
		{
			StiRenderProviderV2.PrepareSubReportsAndDrillDownPages(report);
			List<StiPage> list2 = (from p in report.Pages.ToList()
				where p.IsPage
				select p).ToList();
			list2.Where((StiPage p) => p.Skip && !nestedPages.Contains(p)).ToList().ForEach(delegate(StiPage p)
			{
				nestedPages.Add(p);
			});
			list2.ForEach(delegate(StiPage p)
			{
				p.Skip = false;
			});
		}
		return nestedPages;
	}

	private static ArrayList GetDashboards(StiReport report, StiRequestParams requestParams)
	{
		List<StiPage> nestedPages = GetNestedPages(report);
		bool flag = false;
		ArrayList arrayList = new ArrayList();
		foreach (StiPage page in report.Pages)
		{
			IStiDashboard stiDashboard = page as IStiDashboard;
			if (page.IsEnabled && (!requestParams.Viewer.CombineReportPages || !flag || stiDashboard != null))
			{
				bool flag2 = nestedPages.Contains(page);
				if (stiDashboard == null && !flag2)
				{
					flag = true;
				}
				Hashtable hashtable = new Hashtable();
				hashtable["type"] = ((stiDashboard == null) ? StiReportType.Report : StiReportType.Dashboard);
				hashtable["name"] = page.Name;
				hashtable["alias"] = (string.IsNullOrWhiteSpace(page.Alias) ? page.Name : page.Alias);
				hashtable["index"] = report.Pages.IndexOf(page);
				hashtable["isNestedPage"] = flag2;
				arrayList.Add(hashtable);
			}
		}
		bool flag3 = false;
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		if (licenseKey != null)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag3 = rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag3 = false;
			}
		}
		bool flag4 = flag3 && StiLicenseKeyValidator.IsValid(StiProductIdent.Web, licenseKey) && typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a");
		bool flag5 = flag3 && StiLicenseKeyValidator.IsValid(StiProductIdent.DbsWeb, licenseKey) && typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a");
		foreach (Hashtable item in arrayList)
		{
			if (((StiReportType)item["type"] == StiReportType.Report && flag4) || ((StiReportType)item["type"] == StiReportType.Dashboard && flag5))
			{
				item["valid"] = true;
			}
		}
		return arrayList;
	}

	public static bool IsReportHasInteractions(StiReport report)
	{
		foreach (StiPage page in report.Pages)
		{
			foreach (StiComponent component in page.GetComponents())
			{
				if (IsComponentHasInteraction(component))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsComponentHasInteraction(StiComponent comp)
	{
		if (comp.Interaction != null)
		{
			if (comp.Interaction.SortingEnabled && !string.IsNullOrWhiteSpace(comp.Interaction.SortingColumn))
			{
				return true;
			}
			if (comp.Interaction.DrillDownEnabled && (!string.IsNullOrEmpty(comp.Interaction.DrillDownPageGuid) || !string.IsNullOrEmpty(comp.Interaction.DrillDownReport)))
			{
				return true;
			}
			if (comp.Interaction is StiBandInteraction && ((StiBandInteraction)comp.Interaction).CollapsingEnabled)
			{
				return true;
			}
			if (comp is StiChart && StiOptions.Engine.AllowInteractionInChartWithComponents)
			{
				return true;
			}
		}
		if (comp is StiChart)
		{
			foreach (StiSeries item in ((StiChart)comp).Series)
			{
				if (item.Interaction != null && item.Interaction.DrillDownEnabled)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void ApplySorting(StiReport report, Hashtable parameters)
	{
		if (parameters == null || parameters.Count == 0)
		{
			return;
		}
		string[] array = parameters["ComponentName"].ToString().Split(new char[1] { ';' });
		StiComponent componentByName = report.GetComponentByName(array[0]);
		bool flag = bool.Parse(array[1]);
		array = parameters["DataBand"].ToString().Split(new char[1] { ';' });
		StiDataBand stiDataBand = report.GetComponentByName(array[0]) as StiDataBand;
		if (stiDataBand != null)
		{
			stiDataBand.Sort = array.Where((string val, int i) => i != 0 && !string.IsNullOrEmpty(val)).ToArray();
		}
		if (componentByName == null || stiDataBand == null)
		{
			return;
		}
		string sortColumnsString = componentByName.Interaction.GetSortColumnsString();
		if (stiDataBand.Sort == null || stiDataBand.Sort.Length == 0)
		{
			stiDataBand.Sort = StiSortHelper.AddColumnToSorting(stiDataBand.Sort, sortColumnsString, isAscending: true);
		}
		else
		{
			int columnIndexInSorting = StiSortHelper.GetColumnIndexInSorting(stiDataBand.Sort, sortColumnsString);
			if (flag)
			{
				if (columnIndexInSorting == -1)
				{
					stiDataBand.Sort = StiSortHelper.AddColumnToSorting(stiDataBand.Sort, sortColumnsString, isAscending: true);
				}
				else
				{
					stiDataBand.Sort = StiSortHelper.ChangeColumnSortDirection(stiDataBand.Sort, sortColumnsString);
				}
			}
			else if (columnIndexInSorting != -1)
			{
				StiInteractionSortDirection stiInteractionSortDirection = ((StiSortHelper.GetColumnSortDirection(stiDataBand.Sort, sortColumnsString) == StiInteractionSortDirection.Ascending) ? StiInteractionSortDirection.Descending : StiInteractionSortDirection.Ascending);
				stiDataBand.Sort = StiSortHelper.AddColumnToSorting(new string[0], sortColumnsString, stiInteractionSortDirection == StiInteractionSortDirection.Ascending);
				componentByName.Interaction.SortingDirection = stiInteractionSortDirection;
			}
			else
			{
				stiDataBand.Sort = StiSortHelper.AddColumnToSorting(new string[0], sortColumnsString, isAscending: true);
				componentByName.Interaction.SortingDirection = StiInteractionSortDirection.Ascending;
			}
		}
		report.IsRendered = false;
	}

	public static void ApplyCollapsing(StiReport report, Hashtable parameters)
	{
		string componentName = parameters["ComponentName"] as string;
		StiComponent componentByName = report.GetComponentByName(componentName);
		IStiInteraction stiInteraction = componentByName;
		if (stiInteraction == null || stiInteraction.Interaction == null)
		{
			return;
		}
		report.InteractionCollapsingStates = parameters["CollapsingStates"] as Hashtable;
		foreach (Hashtable value in report.InteractionCollapsingStates.Values)
		{
			Hashtable hashtable2 = value.Clone() as Hashtable;
			foreach (object key in hashtable2.Keys)
			{
				if (key is string)
				{
					int num = int.Parse((string)key);
					value.Add(num, value[key]);
					value.Remove(key);
				}
			}
			hashtable2 = null;
		}
		if (stiInteraction.Interaction is StiCrossHeaderInteraction { CollapsingEnabled: not false })
		{
			StiCrossHeader masterHeader = componentByName as StiCrossHeader;
			StiCrossTabV2Builder.SetCollapsed(masterHeader, !StiCrossTabV2Builder.IsCollapsed(masterHeader));
		}
		report.IsRendered = false;
	}

	public static StiReport ApplyDrillDown(StiReport report, StiReport renderedReport, Hashtable parameters, Hashtable drillDownParameters)
	{
		if (renderedReport == null)
		{
			renderedReport = report;
		}
		if (parameters == null || parameters.Count == 0)
		{
			return renderedReport;
		}
		int index = Convert.ToInt32(parameters["PageIndex"]);
		int index2 = Convert.ToInt32(parameters["ComponentIndex"]);
		int num = Convert.ToInt32(parameters["ElementIndex"]);
		string text = Convert.ToString(parameters["PageGuid"]);
		string text2 = Convert.ToString(parameters["ReportFile"]);
		StiPage stiPage = null;
		StiReport stiReport = report;
		if (renderedReport == null)
		{
			renderedReport = report;
		}
		if (!renderedReport.IsRendered)
		{
			try
			{
				renderedReport.Render(showProgress: false);
			}
			catch
			{
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			foreach (StiPage page in report.Pages)
			{
				if (page.Guid == text)
				{
					stiPage = page;
					page.Enabled = true;
					page.Skip = false;
				}
				else
				{
					page.Enabled = false;
				}
			}
			StiComponentsCollection components = report.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item.Interaction != null && item.Interaction.DrillDownEnabled && item.Interaction.DrillDownPageGuid == stiPage.Guid)
				{
					item.Interaction.DrillDownPage = null;
				}
				if (!(item is StiChart))
				{
					continue;
				}
				StiChart stiChart = item as StiChart;
				foreach (StiSeries item2 in stiChart.Series)
				{
					StiSeriesInteraction stiSeriesInteraction = (StiSeriesInteraction)item2.Interaction;
					if (item2.Interaction != null && stiSeriesInteraction.DrillDownEnabled && stiSeriesInteraction.DrillDownPageGuid == stiPage.Guid)
					{
						stiSeriesInteraction.DrillDownPage = null;
					}
				}
			}
		}
		else if (!string.IsNullOrEmpty(text2))
		{
			stiReport = new StiReport();
			if (text2.StartsWith("resource://"))
			{
				string name = text2.Replace("resource://", "");
				StiReport stiReport2 = renderedReport ?? report;
				if (stiReport2 != null && stiReport2.Dictionary.Resources.Contains(name))
				{
					stiReport.Load(stiReport2.Dictionary.Resources[name].Content);
				}
			}
			else
			{
				stiReport.Load(text2);
			}
			stiReport = GetCompiledReport(stiReport);
		}
		if (report.ReportAlias == stiReport.ReportAlias && stiPage != null)
		{
			stiReport.ReportAlias = (string.IsNullOrEmpty(stiPage.Alias) ? stiPage.Name : stiPage.Alias);
		}
		if (report.ReportDescription == stiReport.ReportDescription)
		{
			stiReport.ReportDescription = stiReport.ReportAlias;
		}
		StiPage stiPage3 = renderedReport.RenderedPages[index];
		StiComponent stiComponent2 = stiPage3.Components[index2];
		if (stiComponent2 != null && stiComponent2.DrillDownParameters != null)
		{
			foreach (KeyValuePair<string, object> drillDownParameter in stiComponent2.DrillDownParameters)
			{
				stiReport[drillDownParameter.Key] = drillDownParameter.Value;
				drillDownParameters[drillDownParameter.Key] = drillDownParameter.Value;
				if (drillDownParameter.Key != null && drillDownParameter.Key.ToLower() == "title" && drillDownParameter.Value != null && !string.IsNullOrEmpty(drillDownParameter.Value.ToString()))
				{
					stiReport.ReportAlias = drillDownParameter.Value.ToString();
				}
			}
		}
		if (stiComponent2 is StiChart stiChart2)
		{
			StiGdiContextPainter contextPainter = new StiGdiContextPainter(StiReport.GlobalMeasureGraphics);
			StiContext context = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, 1f);
			RectangleD rectangleD = stiChart2.Report.Unit.ConvertToHInches(stiChart2.ClientRectangle);
			StiCellGeom stiCellGeom = stiChart2.Core.Render(context, new RectangleF(0f, 0f, (float)rectangleD.Width, (float)rectangleD.Height), useMargins: true);
			List<StiCellGeom> seriesElementGeoms = stiCellGeom.GetSeriesElementGeoms();
			StiSeriesElementGeom stiSeriesElementGeom = ((num != -1) ? (seriesElementGeoms[num] as StiSeriesElementGeom) : null);
			if (stiSeriesElementGeom != null && stiSeriesElementGeom.Interaction != null)
			{
				stiReport["Series"] = stiSeriesElementGeom.Series.Core;
				stiReport["SeriesIndex"] = stiSeriesElementGeom.Series.Core.Series.Chart.Series.IndexOf(stiSeriesElementGeom.Series.Core.Series);
				stiReport["SeriesArgument"] = stiSeriesElementGeom.Interaction.Argument;
				stiReport["SeriesValue"] = stiSeriesElementGeom.Interaction.Value;
				stiReport["SeriesEndValue"] = stiSeriesElementGeom.Interaction.EndValue;
				stiReport["SeriesPointIndex"] = stiSeriesElementGeom.Interaction.PointIndex;
				stiReport["SeriesTag"] = stiSeriesElementGeom.Interaction.Tag;
				stiReport["SeriesHyperlink"] = stiSeriesElementGeom.Interaction.Hyperlink;
				stiReport["SeriesTooltip"] = stiSeriesElementGeom.Interaction.Tooltip;
				stiReport["SeriesTitle"] = stiSeriesElementGeom.Series.CoreTitle;
				string text3 = ((stiSeriesElementGeom.Interaction.Series != null) ? stiSeriesElementGeom.Interaction.Series.CoreTitle : null);
				string text4 = ((stiSeriesElementGeom.Interaction.Argument != null) ? stiSeriesElementGeom.Interaction.Argument.ToString() : null);
				if (string.IsNullOrEmpty(text4))
				{
					text4 = stiSeriesElementGeom.Interaction.Value.ToString();
				}
				if (!string.IsNullOrEmpty(text3) && !string.IsNullOrEmpty(text4))
				{
					stiReport.ReportAlias = $"{text3} - {text4}";
				}
				else if (!string.IsNullOrEmpty(text4))
				{
					stiReport.ReportAlias = text3;
				}
				else
				{
					stiReport.ReportAlias = text4;
				}
			}
		}
		try
		{
			stiReport.IsInteractionRendering = true;
			try
			{
				stiReport.Render(showProgress: false);
			}
			catch
			{
			}
		}
		finally
		{
			stiReport.IsInteractionRendering = false;
		}
		return stiReport;
	}

	public static StiReport ApplyDashboardDrillDown(StiReport report, Hashtable drillDownParameters)
	{
		if (drillDownParameters == null || drillDownParameters.Count == 0)
		{
			return report;
		}
		string drillDownPageKey = drillDownParameters["drillDownPageKey"] as string;
		ArrayList arrayList = drillDownParameters["parameters"] as ArrayList;
		string text = drillDownParameters["value"] as string;
		StiReport cloneReport = StiReportCopier.CloneReport(report);
		cloneReport.Key = StiKeyHelper.GenerateKey();
		cloneReport.Pages.ToList().ForEach(delegate(StiPage p)
		{
			p.Enabled = p.Guid == drillDownPageKey;
		});
		StiReportCopier.CopyElementsDrillDown(report, cloneReport);
		if (arrayList != null)
		{
			arrayList?.Cast<Hashtable>().ToList().ForEach(delegate(Hashtable p)
			{
				object obj = p["value"];
				try
				{
					if (obj != null && obj is string && ((string)obj).Contains("{") && ((string)obj).Contains("}"))
					{
						obj = StiReportParser.Parse((string)obj, cloneReport.Pages.ToList().FirstOrDefault((StiPage page) => page.Guid == drillDownPageKey), allowCache: false);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				cloneReport[p["key"]?.ToString()] = obj;
			});
		}
		cloneReport.Pages.ToList().ForEach(delegate(StiPage p)
		{
			if (p.Guid == drillDownPageKey)
			{
				cloneReport.ReportAlias = ((!string.IsNullOrEmpty(p.Alias)) ? p.Alias : p.Name);
			}
		});
		if (cloneReport.ContainsDashboard)
		{
			StiDataFiltersHelper.ApplyDefaultFiltersForFilterElements(cloneReport);
		}
		return cloneReport;
	}

	private static int Round(double value)
	{
		return (int)Math.Round(value, MidpointRounding.AwayFromZero);
	}

	private static Rectangle Round(RectangleD rect)
	{
		return new Rectangle(Round(rect.Left), Round(rect.Top), Round(rect.Width), Round(rect.Height));
	}

	private static List<StiRangeBand> GetElementsPositions(List<IStiElement> elements, IStiDashboard dashboard)
	{
		Dictionary<IStiElement, Rectangle> rects = elements.Cast<StiComponent>().ToDictionary((StiComponent e) => e as IStiElement, (StiComponent e) => Round(e.ClientRectangle));
		List<int> list = new List<int>();
		foreach (IStiElement element2 in elements)
		{
			list.Add(Round(rects[element2].Top));
			list.Add(Round(rects[element2].Bottom));
		}
		list.Add(0);
		list.Add(Round(((StiComponent)dashboard).Height));
		list = (from y in list.Distinct()
			orderby y
			select y).ToList();
		List<StiRangeBand> list2 = new List<StiRangeBand>();
		for (int i = 0; i < list.Count - 1; i++)
		{
			list2.Add(new StiRangeBand(list[i], list[i + 1]));
		}
		foreach (IStiElement element in elements.Where((IStiElement e) => e is IStiFixedHeightElement && (e as IStiFixedHeightElement).IsFixedHeight))
		{
			list2.Where((StiRangeBand b) => b.Intersect(rects[element])).ToList().ForEach(delegate(StiRangeBand c)
			{
				c.IsFixed = true;
			});
		}
		return list2;
	}

	private static void CorrectElementLocations(StiPage page, List<IStiElement> elements)
	{
		IStiDashboard stiDashboard = page as IStiDashboard;
		foreach (StiComponent element in elements)
		{
			if (element.Page != stiDashboard)
			{
				continue;
			}
			if (element.Right > page.Width)
			{
				element.Left = page.Width - element.Width;
			}
			if (element.Bottom > page.Height)
			{
				element.Top = page.Height - element.Height;
			}
			if (element.Left < 0.0)
			{
				element.Left = 0.0;
				if (element.Width > page.Width)
				{
					element.Width = page.Width;
				}
			}
			if (element.Top < 0.0)
			{
				element.Top = 0.0;
				if (element.Height > page.Height)
				{
					element.Height = page.Height;
				}
			}
		}
	}

	private static Hashtable GetDashboardPage(StiReport report, int pageIndex, StiRequestParams requestParams)
	{
		Hashtable hashtable = new Hashtable();
		StiPage stiPage = report.Pages[pageIndex];
		IStiDashboard stiDashboard = (IStiDashboard)stiPage;
		List<IStiElement> list = (from e in stiDashboard.GetElements(nested: true)
			where e.IsEnabled
			select e).ToList();
		bool flag = !string.IsNullOrEmpty(requestParams.Viewer.ElementName) && requestParams.Action != StiAction.DashboardDrillDown;
		int totalFixedHeight = 0;
		List<StiRangeBand> bands = null;
		if (requestParams.Viewer.DashboardWidth == 0 && requestParams.Viewer.DashboardHeight == 0)
		{
			requestParams.Viewer.DashboardWidth = Convert.ToInt32(stiPage.Width);
			requestParams.Viewer.DashboardHeight = Convert.ToInt32(stiPage.Height);
		}
		CorrectElementLocations(stiPage, list);
		if (stiDashboard.IsMobileSurfacePresent)
		{
			stiDashboard.SwitchSurfaceViewMode((requestParams.Viewer.DashboardWidth <= stiDashboard.DeviceWidth) ? StiSurfaceViewMode.Mobile : StiSurfaceViewMode.Desktop);
		}
		if (!flag)
		{
			CalculatePositionForEachBand(requestParams, list, stiPage, out bands, out totalFixedHeight);
		}
		Hashtable hashtable2 = new Hashtable();
		hashtable["width"] = requestParams.Viewer.DashboardWidth;
		hashtable["height"] = requestParams.Viewer.DashboardHeight;
		hashtable["margin"] = stiPage.Margins;
		hashtable["background"] = GetHtmlColor(StiDashboardStyleHelper.GetDashboardBackColor(stiDashboard, isViewer: true));
		hashtable["actionColors"] = StiDashboardElementViewHelper.GetActionColors(stiDashboard);
		hashtable["contentAlignment"] = stiDashboard.ContentAlignment.ToString();
		hashtable["dashboardWatermark"] = StiDashboardElementViewHelper.GetDashboardWatermark(stiDashboard);
		ArrayList arrayList = new ArrayList();
		foreach (IStiElement item in list)
		{
			if ((!flag || item.Name == requestParams.Viewer.ElementName) && !((StiComponent)item).ClientRectangle.IsEmpty)
			{
				arrayList.Add(GetElementAttributes(stiPage, item, flag, requestParams, bands, totalFixedHeight));
			}
		}
		hashtable["elements"] = arrayList;
		return hashtable;
	}

	internal static Hashtable GetElementAttributes(StiPage page, IStiElement element, bool renderSingleElement, StiRequestParams requestParams, List<StiRangeBand> bands = null, int totalFixedHeight = 0)
	{
		double? num = null;
		StiComponent stiComponent = (StiComponent)element;
		StiDashboardContentAlignment contentAlignment = ((IStiDashboard)page).ContentAlignment;
		Hashtable hashtable = new Hashtable();
		hashtable["name"] = element.Name;
		hashtable["type"] = element.GetType().Name;
		hashtable["group"] = (element as IStiGroupElement)?.Group;
		hashtable["key"] = element.Key;
		StiMargin margin = ((IStiMargin)element).Margin;
		if (renderSingleElement)
		{
			((IStiMargin)element).Margin = new StiMargin();
		}
		StiMargin margin2 = ((IStiMargin)element).Margin;
		StiPadding padding = ((IStiPadding)element).Padding;
		double num2 = (double)requestParams.Viewer.DashboardWidth / (renderSingleElement ? stiComponent.Width : page.Width);
		double num3 = ((element is IStiFixedHeightElement && (element as IStiFixedHeightElement).IsFixedHeight) ? 1.0 : ((double)(requestParams.Viewer.DashboardHeight - totalFixedHeight) / (renderSingleElement ? stiComponent.Height : (page.Height - (double)totalFixedHeight))));
		if (contentAlignment == StiDashboardContentAlignment.Center || contentAlignment == StiDashboardContentAlignment.Left || contentAlignment == StiDashboardContentAlignment.Right)
		{
			num2 = 1.0;
		}
		if (contentAlignment == StiDashboardContentAlignment.Center || contentAlignment == StiDashboardContentAlignment.Left || contentAlignment == StiDashboardContentAlignment.Right)
		{
			num3 = 1.0;
		}
		if (contentAlignment == StiDashboardContentAlignment.StretchX)
		{
			num3 = num2;
		}
		int num4 = 0;
		if (contentAlignment == StiDashboardContentAlignment.Right)
		{
			num4 = requestParams.Viewer.DashboardWidth - Convert.ToInt32(page.Width);
		}
		if (contentAlignment == StiDashboardContentAlignment.Center)
		{
			num4 = (requestParams.Viewer.DashboardWidth - Convert.ToInt32(page.Width)) / 2;
		}
		hashtable["left"] = 0;
		hashtable["top"] = 0;
		hashtable["width"] = (double)Round(stiComponent.Width * num2) - margin2.Left - margin2.Right;
		hashtable["height"] = (double)Round(stiComponent.Height * num3) - margin2.Top - margin2.Bottom;
		hashtable["margin"] = $"{Round(margin2.Top)},{Round(margin2.Right)},{Round(margin2.Bottom)},{Round(margin2.Left)}";
		hashtable["padding"] = $"{Round(padding.Top)},{Round(padding.Right)},{Round(padding.Bottom)},{Round(padding.Left)}";
		hashtable["parentKey"] = (element as IStiFilterElement)?.GetParentKey();
		hashtable["backColor"] = StiDashboardElementViewHelper.GetBackColor(element);
		hashtable["foreColor"] = StiDashboardElementViewHelper.GetForeColor(element);
		hashtable["border"] = StiDashboardElementViewHelper.GetBorder(element);
		hashtable["font"] = StiDashboardElementViewHelper.GetFont(element);
		hashtable["layout"] = StiDashboardElementViewHelper.GetLayout(element);
		hashtable["actionColors"] = StiDashboardElementViewHelper.GetActionColors(element);
		hashtable["shadow"] = StiDashboardElementViewHelper.GetShadow(element);
		hashtable["cornerRadius"] = StiDashboardElementViewHelper.GetCornerRadius(element);
		hashtable["dashboardWatermark"] = StiDashboardElementViewHelper.GetDashboardWatermark(element);
		if (!renderSingleElement)
		{
			CalculateParentPanelPositions(stiComponent, num2, num3, out var parentLeft, out var parentTop);
			Rectangle componentClientRectangle = Round(stiComponent.ClientRectangle);
			hashtable["left"] = Round((double)componentClientRectangle.Left * num2) + parentLeft + num4;
			hashtable["top"] = bands.FirstOrDefault((StiRangeBand b) => b.OriginalTop == componentClientRectangle.Top).Top + parentTop;
			StiRangeBand stiRangeBand = bands.FirstOrDefault((StiRangeBand b) => b.OriginalBottom == componentClientRectangle.Bottom);
			StiRangeBand stiRangeBand2 = bands.FirstOrDefault((StiRangeBand b) => b.OriginalTop == componentClientRectangle.Top);
			double num5 = (double)((stiRangeBand?.Bottom ?? 0) - (stiRangeBand2?.Top ?? 0)) - margin2.Top - margin2.Bottom;
			if (num5 != (double)hashtable["height"])
			{
				num = stiComponent.Height;
				stiComponent.Height = (double)Round(num5 / num3) + margin2.Top + margin2.Bottom;
			}
			hashtable["height"] = num5;
		}
		hashtable["contentAttributes"] = StiDashboardElementViewHelper.GetElementContentAttributes(element, num2, num3, requestParams);
		if (num.HasValue)
		{
			stiComponent.Height = num.Value;
		}
		((IStiMargin)element).Margin = margin;
		return hashtable;
	}

	internal static Hashtable GetSingleElementContent(StiReport report, StiRequestParams requestParams)
	{
		string @string = requestParams.GetString("elementNameForRefreshing");
		IStiElement stiElement = ((report != null && @string != null) ? (report.Pages.GetComponentByName(@string) as IStiElement) : null);
		if (stiElement != null)
		{
			bool flag = !string.IsNullOrEmpty(requestParams.Viewer.ElementName) && requestParams.Action != StiAction.DashboardDrillDown;
			List<IStiElement> elements = (from e in ((IStiDashboard)stiElement.Page).GetElements(nested: true)
				where e.IsEnabled
				select e).ToList();
			int totalFixedHeight = 0;
			List<StiRangeBand> bands = null;
			if (!flag)
			{
				CalculatePositionForEachBand(requestParams, elements, stiElement.Page, out bands, out totalFixedHeight, stiElement as StiComponent);
			}
			return GetElementAttributes(stiElement.Page, stiElement, flag, requestParams, bands, totalFixedHeight);
		}
		return null;
	}

	internal static void CalculatePositionForEachBand(StiRequestParams requestParams, List<IStiElement> elements, StiPage page, out List<StiRangeBand> bands, out int totalFixedHeight, StiComponent component = null)
	{
		IStiDashboard stiDashboard = page as IStiDashboard;
		bands = GetElementsPositions(elements, stiDashboard);
		totalFixedHeight = bands.Where((StiRangeBand b) => b.IsFixed).Sum((StiRangeBand b) => b.Height);
		StiDashboardContentAlignment contentAlignment = stiDashboard.ContentAlignment;
		double num = (double)requestParams.Viewer.DashboardWidth / (component?.Width ?? page.Width);
		double num2 = (double)(requestParams.Viewer.DashboardHeight - totalFixedHeight) / (page.Height - (double)totalFixedHeight);
		int num3 = 0;
		if (contentAlignment == StiDashboardContentAlignment.Center || contentAlignment == StiDashboardContentAlignment.Left || contentAlignment == StiDashboardContentAlignment.Right)
		{
			num2 = 1.0;
		}
		if (contentAlignment == StiDashboardContentAlignment.StretchX)
		{
			num2 = num;
		}
		foreach (StiRangeBand band in bands)
		{
			band.Top = num3;
			band.Bottom = band.Top + Round(band.IsFixed ? ((double)band.OriginalHeight) : ((double)band.OriginalHeight * num2));
			num3 += band.Bottom - band.Top;
		}
	}

	private static void CalculateParentPanelPositions(StiComponent component, double scaleX, double scaleY, out int parentLeft, out int parentTop)
	{
		parentLeft = 0;
		parentTop = 0;
		StiContainer parent = component.Parent;
		while (parent != null && !(parent is IStiDashboard))
		{
			parentLeft += Round(parent.Left * scaleX);
			parentTop += Round(parent.Top * scaleY);
			parent = parent.Parent;
		}
	}

	private static Hashtable GetReportPage(StiReport report, StiHtmlExportService service, int pageIndex, StiRequestParams requestParams)
	{
		StiHtmlExportSettings stiHtmlExportSettings = new StiHtmlExportSettings();
		stiHtmlExportSettings.PageRange = new StiPagesRange(pageIndex);
		stiHtmlExportSettings.Zoom = requestParams.Viewer.Zoom;
		stiHtmlExportSettings.ImageFormat = null;
		stiHtmlExportSettings.ImageQuality = 0.85f;
		stiHtmlExportSettings.ExportQuality = StiHtmlExportQuality.High;
		stiHtmlExportSettings.ExportBookmarksMode = StiHtmlExportBookmarksMode.ReportOnly;
		stiHtmlExportSettings.RemoveEmptySpaceAtBottom = false;
		stiHtmlExportSettings.OpenLinksTarget = requestParams.Viewer.OpenLinksWindow;
		stiHtmlExportSettings.ChartType = (StiHtmlChartType)Enum.Parse(typeof(StiHtmlChartType), requestParams.Viewer.ChartRenderType.ToString());
		stiHtmlExportSettings.UseWatermarkMargins = true;
		stiHtmlExportSettings.AddPageBreaks = false;
		switch (requestParams.Viewer.ReportDisplayMode)
		{
		case StiReportDisplayMode.Table:
			stiHtmlExportSettings.ExportMode = StiHtmlExportMode.Table;
			break;
		case StiReportDisplayMode.Div:
			stiHtmlExportSettings.ExportMode = StiHtmlExportMode.Div;
			break;
		case StiReportDisplayMode.Span:
			stiHtmlExportSettings.ExportMode = StiHtmlExportMode.Span;
			break;
		}
		switch (requestParams.Viewer.ImagesQuality)
		{
		case StiImagesQuality.Low:
			stiHtmlExportSettings.ImageResolution = 50f;
			break;
		case StiImagesQuality.Normal:
			stiHtmlExportSettings.ImageResolution = 100f;
			break;
		case StiImagesQuality.High:
			stiHtmlExportSettings.ImageResolution = 200f;
			break;
		}
		MemoryStream memoryStream = new MemoryStream();
		service.ExportHtml(report, memoryStream, stiHtmlExportSettings);
		string text = Encoding.UTF8.GetString(memoryStream.ToArray()).Substring(1);
		memoryStream.Close();
		Hashtable hashtable = new Hashtable();
		hashtable["content"] = ((report.RenderedPages.Count > 0) ? text : string.Empty);
		StiPage stiPage = ((report.RenderedPages.Count > 0) ? report.RenderedPages[Math.Min(pageIndex, report.RenderedPages.Count - 1)] : new StiPage(report));
		double num = ((StiOptions.Export.Html.PrintLayoutOptimization && stiHtmlExportSettings.ExportMode == StiHtmlExportMode.Div) ? 0.96 : 1.0);
		double num2 = ((StiOptions.Export.Html.PrintLayoutOptimization && stiHtmlExportSettings.ExportMode == StiHtmlExportMode.Div) ? 0.956 : 1.0);
		hashtable["margins"] = $"{Math.Round(report.Unit.ConvertToHInches(stiPage.Margins.Top) * requestParams.Viewer.Zoom)}px {Math.Round(report.Unit.ConvertToHInches(stiPage.Margins.Right) * requestParams.Viewer.Zoom)}px {Math.Round(report.Unit.ConvertToHInches(stiPage.Margins.Bottom) * requestParams.Viewer.Zoom)}px {Math.Round(report.Unit.ConvertToHInches(stiPage.Margins.Left) * requestParams.Viewer.Zoom)}px";
		hashtable["sizes"] = $"{Math.Round(report.Unit.ConvertToHInches(stiPage.PageWidth * (double)stiPage.SegmentPerWidth * num) * requestParams.Viewer.Zoom)};{Math.Round(report.Unit.ConvertToHInches(stiPage.PageHeight * (double)stiPage.SegmentPerHeight * num2) * requestParams.Viewer.Zoom)}";
		hashtable["background"] = GetHtmlColor(StiBrush.ToColor(stiPage.Brush));
		hashtable["existsWatermark"] = (stiPage.Watermark.Image != null || !string.IsNullOrEmpty(stiPage.Watermark.ImageHyperlink) || !string.IsNullOrEmpty(stiPage.Watermark.Text)) && stiPage.Watermark.Enabled;
		return hashtable;
	}

	private static int GetPagesCount(StiReport report, StiRequestParams requestParams)
	{
		if (!requestParams.Viewer.CombineReportPages && IsMixedReport(report) && report.Pages.Count > requestParams.Viewer.OriginalPageNumber)
		{
			string pageName = report.Pages[requestParams.Viewer.OriginalPageNumber].Name;
			int num = report.RenderedPages.ToList().FindIndex((StiPage p) => p.Name == pageName);
			int num2 = report.RenderedPages.ToList().FindLastIndex((StiPage p) => p.Name == pageName);
			if (num2 >= 0 && num >= 0)
			{
				return num2 - num + 1;
			}
		}
		return report.RenderedPages.Count;
	}

	private static bool IsMixedReport(StiReport report)
	{
		if (report != null)
		{
			List<StiPage> nestedPages = GetNestedPages(report);
			bool flag = false;
			bool flag2 = false;
			foreach (StiPage page in report.Pages)
			{
				if ((page.IsDashboard ? page.IsEnabled : page.Enabled) && !nestedPages.Contains(page))
				{
					if (page is IStiDashboard)
					{
						flag2 = true;
					}
					else
					{
						flag = true;
					}
				}
				if (flag2 && flag)
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static ArrayList GetPagesArray(StiReport report, StiRequestParams requestParams)
	{
		StiHtmlExportService stiHtmlExportService = new StiHtmlExportService();
		stiHtmlExportService.RenderWebViewer = true;
		stiHtmlExportService.RenderWebInteractions = true;
		stiHtmlExportService.RenderAsDocument = false;
		stiHtmlExportService.Styles = new ArrayList();
		stiHtmlExportService.ClearOnFinish = false;
		stiHtmlExportService.RenderStyles = false;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		ArrayList arrayList = new ArrayList();
		if (requestParams.Viewer.ReportType == StiReportType.Dashboard)
		{
			Hashtable hashtable = null;
			int pageNumber = requestParams.Viewer.PageNumber;
			if (!(report.Pages[pageNumber].IsDashboard ? report.Pages[pageNumber].IsEnabled : report.Pages[pageNumber].Enabled))
			{
				int num = report.Pages.IndexOf(report.Pages.ToList().FirstOrDefault((StiPage p) => (!p.IsDashboard) ? p.Enabled : p.IsEnabled));
				if (num >= 0)
				{
					pageNumber = num;
				}
			}
			if (report.Pages[pageNumber].IsDashboard)
			{
				report.CurrentPage = pageNumber;
				hashtable = GetDashboardPage(report, pageNumber, requestParams);
				arrayList.Add(hashtable);
			}
			else
			{
				pageNumber = requestParams.Viewer.PageNumber;
				if (requestParams.Viewer.ViewMode == StiWebViewMode.SinglePage)
				{
					report.CurrentPage = report.Pages.IndexOf(report.Pages.ToList().FirstOrDefault((StiPage p) => p.Name == report.RenderedPages[(pageNumber < report.RenderedPages.Count) ? pageNumber : 0].Name));
					hashtable = GetReportPage(report, stiHtmlExportService, pageNumber, requestParams);
					arrayList.Add(hashtable);
				}
				else
				{
					int firstPageNumber2 = ((requestParams.Viewer.ViewMode == StiWebViewMode.SinglePage) ? requestParams.Viewer.PageNumber : 0);
					int num2 = report.RenderedPages.Count - 1;
					report.CurrentPage = report.Pages.IndexOf(report.Pages.ToList().FirstOrDefault((StiPage p) => p.Name == report.RenderedPages[(firstPageNumber2 < report.RenderedPages.Count) ? firstPageNumber2 : 0].Name));
					for (int i = firstPageNumber2; i <= num2; i++)
					{
						arrayList.Add(GetReportPage(report, stiHtmlExportService, i, requestParams));
					}
				}
				requestParams.Viewer.ReportType = StiReportType.Report;
			}
		}
		else
		{
			int firstPageNumber = ((requestParams.Viewer.ViewMode == StiWebViewMode.SinglePage) ? requestParams.Viewer.PageNumber : 0);
			int num3 = Math.Max(0, report.RenderedPages.Count - 1);
			report.CurrentPage = ((report.RenderedPages.Count > 0) ? report.Pages.IndexOf(report.Pages.ToList().FirstOrDefault((StiPage p) => p.Name == report.RenderedPages[(firstPageNumber < report.RenderedPages.Count) ? firstPageNumber : 0].Name)) : 0);
			if (!requestParams.Viewer.CombineReportPages && IsMixedReport(report) && report.Pages.Count > requestParams.Viewer.OriginalPageNumber)
			{
				string pageName = report.Pages[requestParams.Viewer.OriginalPageNumber].Name;
				report.CurrentPage = requestParams.Viewer.OriginalPageNumber;
				int num4 = report.RenderedPages.ToList().FindIndex((StiPage p) => p.Name == pageName);
				int num5 = report.RenderedPages.ToList().FindLastIndex((StiPage p) => p.Name == pageName);
				if (num4 >= 0 && num5 >= 0)
				{
					firstPageNumber += num4;
					num3 = num5;
				}
			}
			if (requestParams.Viewer.ViewMode == StiWebViewMode.SinglePage)
			{
				arrayList.Add(GetReportPage(report, stiHtmlExportService, firstPageNumber, requestParams));
			}
			else
			{
				for (int j = firstPageNumber; j <= num3; j++)
				{
					arrayList.Add(GetReportPage(report, stiHtmlExportService, j, requestParams));
				}
			}
		}
		StringWriter stringWriter = new StringWriter();
		StiHtmlTextWriter stiHtmlTextWriter = (stiHtmlExportService.HtmlWriter = new StiHtmlTextWriter(stringWriter));
		if (stiHtmlExportService.TableRender != null)
		{
			stiHtmlExportService.TableRender.RenderStylesTable(useBookmarks: true, exportBookmarksOnly: false, addStyleTag: false);
		}
		stiHtmlTextWriter.Flush();
		stringWriter.Flush();
		string value = stringWriter.GetStringBuilder().ToString();
		stringWriter.Close();
		arrayList.Add(value);
		string chartScript = stiHtmlExportService.GetChartScript();
		arrayList.Add(chartScript);
		stiHtmlExportService.Clear();
		return arrayList;
	}

	private static void AddBookmarkNode(StiBookmark bkm, int parentNode, ArrayList bookmarksTree)
	{
		StiBookmarkTreeNode stiBookmarkTreeNode = new StiBookmarkTreeNode();
		stiBookmarkTreeNode.Parent = parentNode;
		stiBookmarkTreeNode.Url = "#" + (stiBookmarkTreeNode.Title = bkm.Text.Replace("'", "\\'").Replace("\r", "").Replace("\n", ""));
		stiBookmarkTreeNode.Used = true;
		stiBookmarkTreeNode.ComponentGuid = bkm.ComponentGuid;
		bookmarksTree.Add(stiBookmarkTreeNode);
		int parentNode2 = bookmarksTree.Count - 1;
		if (bkm.Bookmarks.Count != 0)
		{
			for (int i = 0; i < bkm.Bookmarks.Count; i++)
			{
				AddBookmarkNode(bkm.Bookmarks[i], parentNode2, bookmarksTree);
			}
		}
	}

	private static Hashtable GetBookmarksPageIndexes(StiReport report)
	{
		Hashtable hashtable = new Hashtable();
		int num = 1;
		foreach (StiPage renderedPage in report.RenderedPages)
		{
			report.RenderedPages.GetPage(renderedPage);
			StiComponentsCollection components = renderedPage.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item.Enabled)
				{
					string text = item.BookmarkValue as string;
					string guid = item.Guid;
					if (guid != null && !hashtable.ContainsKey(guid))
					{
						hashtable[guid] = num;
					}
					if (text == null)
					{
						text = string.Empty;
					}
					text = text.Replace("'", "\\'");
					if (text != string.Empty && !hashtable.ContainsKey(text))
					{
						hashtable.Add(text, num);
					}
				}
			}
			num++;
		}
		return hashtable;
	}

	public static string GetBookmarksContent(StiReport report, string viewerId, int pageNumber)
	{
		Hashtable bookmarksPageIndexes = GetBookmarksPageIndexes(report);
		ArrayList arrayList = new ArrayList();
		AddBookmarkNode(report.Bookmark, -1, arrayList);
		string empty = string.Empty;
		empty += $"bookmarks = new stiTree('bookmarks','{viewerId}',{pageNumber}, imagesForBookmarks);";
		for (int i = 0; i < arrayList.Count; i++)
		{
			StiBookmarkTreeNode stiBookmarkTreeNode = (StiBookmarkTreeNode)arrayList[i];
			string empty2 = string.Empty;
			empty2 = ((stiBookmarkTreeNode.ComponentGuid != null && bookmarksPageIndexes.ContainsKey(stiBookmarkTreeNode.ComponentGuid)) ? $"Page {(int)bookmarksPageIndexes[stiBookmarkTreeNode.ComponentGuid]}" : ((!bookmarksPageIndexes.ContainsKey(stiBookmarkTreeNode.Title)) ? "Page 0" : $"Page {(int)bookmarksPageIndexes[stiBookmarkTreeNode.Title]}"));
			empty += $"bookmarks.add({i},{stiBookmarkTreeNode.Parent},'{SecurityElement.Escape(stiBookmarkTreeNode.Title)}','{stiBookmarkTreeNode.Url}','{empty2}','{stiBookmarkTreeNode.ComponentGuid}');";
		}
		return empty;
	}

	public static ArrayList GetBookmarkPointers(StiReport report, StiBookmark bookmark)
	{
		Hashtable bookmarksPageIndexes = GetBookmarksPageIndexes(report);
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		AddBookmarkNode(bookmark, -1, arrayList2);
		for (int i = 0; i < arrayList2.Count; i++)
		{
			Hashtable hashtable = new Hashtable();
			StiBookmarkTreeNode stiBookmarkTreeNode = arrayList2[i] as StiBookmarkTreeNode;
			int num = 1;
			if (stiBookmarkTreeNode.ComponentGuid != null && bookmarksPageIndexes.ContainsKey(stiBookmarkTreeNode.ComponentGuid))
			{
				num = (int)bookmarksPageIndexes[stiBookmarkTreeNode.ComponentGuid];
			}
			else if (bookmarksPageIndexes.ContainsKey(stiBookmarkTreeNode.Title))
			{
				num = (int)bookmarksPageIndexes[stiBookmarkTreeNode.Title];
			}
			hashtable["componentGuid"] = stiBookmarkTreeNode.ComponentGuid;
			hashtable["anchor"] = stiBookmarkTreeNode.Url;
			hashtable["pageIndex"] = num;
			arrayList.Add(hashtable);
		}
		return arrayList;
	}

	public static StiWebActionResult ViewerResult(StiRequestParams requestParams, StiReport report)
	{
		if (report == null)
		{
			return StiWebActionResult.EmptyReportResult();
		}
		if (!report.IsRendered && !report.ContainsOnlyDashboard)
		{
			try
			{
				report.Render(showProgress: false);
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
				{
					text = text + " " + ex.InnerException.Message;
				}
				return new StiWebActionResult("ServerError:" + text);
			}
		}
		if (requestParams.Action == StiAction.ChangeTableElementSelectColumns)
		{
			return StiWebActionResult.JsonResult(requestParams, StiTableElementViewHelper.ChangeTableElementSelectColumns(report, requestParams));
		}
		if (requestParams.Action == StiAction.ChangeChartElementViewState)
		{
			return StiWebActionResult.JsonResult(requestParams, StiChartElementViewHelper.ChangeChartElementViewState(report, requestParams));
		}
		if (requestParams.Action == StiAction.DashboardGetSingleElementContent)
		{
			return StiWebActionResult.JsonResult(requestParams, GetSingleElementContent(report, requestParams));
		}
		if (requestParams.Action == StiAction.DashboardGettingFilterItems)
		{
			return StiWebActionResult.JsonResult(requestParams, StiDataFiltersHelper.GetFilterItems(report, requestParams));
		}
		if (requestParams.Action == StiAction.GetDatePickerFormattedValues)
		{
			return StiWebActionResult.JsonResult(requestParams, StiDatePickerElementViewHelper.GetFormattedValues(report, requestParams));
		}
		if (requestParams.Action == StiAction.DashboardViewData)
		{
			return StiWebActionResult.JsonResult(requestParams, StiDataFiltersHelper.GetViewData(report, requestParams));
		}
		StiEditableFieldsHelper.ApplyEditableFieldsToReport(report, requestParams.Interaction.Editable);
		if (requestParams.Action == StiAction.DrillDown)
		{
			requestParams.Viewer.PageNumber = 0;
		}
		if ((requestParams.Action == StiAction.Variables || requestParams.Action == StiAction.Collapsing) && requestParams.Viewer.ReportType == StiReportType.Report)
		{
			requestParams.Viewer.PageNumber = Math.Min(requestParams.Viewer.PageNumber, report.RenderedPages.Count - 1);
		}
		if (requestParams.Viewer.ReportType == StiReportType.Auto)
		{
			List<StiPage> nestedPages = GetNestedPages(report);
			StiPage stiPage = report.Pages.ToList().FirstOrDefault((StiPage p) => (p.IsDashboard ? p.IsEnabled : p.Enabled) && !nestedPages.Contains(p));
			if (requestParams.Viewer.ReportDesignerMode && requestParams.Viewer.OriginalPageNumber > 0 && requestParams.Viewer.OriginalPageNumber < report.Pages.Count)
			{
				StiPage stiPage2 = report.Pages[requestParams.Viewer.OriginalPageNumber];
				if ((stiPage2.IsDashboard ? stiPage2.IsEnabled : stiPage2.Enabled) && !nestedPages.Contains(stiPage2))
				{
					stiPage = stiPage2;
				}
			}
			requestParams.Viewer.ReportType = ((stiPage == null || !stiPage.IsDashboard) ? StiReportType.Report : StiReportType.Dashboard);
			requestParams.Viewer.OriginalPageNumber = report.Pages.IndexOf(stiPage);
			if (requestParams.Viewer.ReportType == StiReportType.Dashboard)
			{
				requestParams.Viewer.PageNumber = requestParams.Viewer.OriginalPageNumber;
			}
		}
		Hashtable hashtable = new Hashtable();
		hashtable["action"] = requestParams.Action;
		hashtable["refreshTime"] = ((!report.IsDocument) ? report.RefreshTime : 0);
		if (requestParams.Action == StiAction.GetReport || requestParams.Action == StiAction.OpenReport)
		{
			hashtable["customFonts"] = StiReportResourceHelper.GetFontResourcesArray(report);
			hashtable["stimulsoftFontContent"] = ((!requestParams.Viewer.ReportDesignerMode && StiFontIconsExHelper.NeedToUseStimulsoftFont(report)) ? StiReportResourceHelper.GetStimulsoftFontBase64Data() : null);
			if (report.ContainsDashboard)
			{
				StiDataFiltersHelper.ApplyDefaultFiltersForFilterElements(report);
				hashtable["dashboards"] = GetDashboards(report, requestParams);
			}
		}
		if (requestParams.Viewer.ReportType == StiReportType.Report || (requestParams.Action != StiAction.GetReport && requestParams.Action != StiAction.OpenReport && requestParams.Action != StiAction.InitVars))
		{
			hashtable["pagesArray"] = GetPagesArray(report, requestParams);
		}
		if (requestParams.Action == StiAction.DashboardFiltering || requestParams.Action == StiAction.DashboardSorting || requestParams.Action == StiAction.DashboardElementDrillDown || requestParams.Action == StiAction.DashboardElementDrillUp)
		{
			hashtable["repaintOnlyDashboardContent"] = true;
			Hashtable hashtable2 = null;
			if (requestParams.Action == StiAction.DashboardFiltering)
			{
				hashtable2 = requestParams.Interaction.DashboardFiltering;
			}
			else if (requestParams.Action == StiAction.DashboardSorting)
			{
				hashtable2 = requestParams.Interaction.DashboardSorting;
			}
			else if (requestParams.Action == StiAction.DashboardElementDrillDown || requestParams.Action == StiAction.DashboardElementDrillUp)
			{
				hashtable2 = requestParams.Interaction.DashboardElementDrillDownParameters;
			}
			if (hashtable2 != null)
			{
				if (hashtable2["elementName"] != null)
				{
					hashtable["currentElementName"] = hashtable2["elementName"];
				}
				if (hashtable2["elementGroup"] != null)
				{
					hashtable["currentElementGroup"] = hashtable2["elementGroup"];
				}
				if (hashtable2["filterGuid"] != null)
				{
					hashtable["filterGuid"] = hashtable2["filterGuid"];
				}
			}
		}
		if (requestParams.Action != StiAction.GetPages)
		{
			hashtable["reportType"] = requestParams.Viewer.ReportType.ToString();
			hashtable["drillDownGuid"] = requestParams.Cache.DrillDownGuid;
			hashtable["dashboardDrillDownGuid"] = requestParams.Cache.DashboardDrillDownGuid;
			hashtable["zoom"] = Convert.ToInt32(requestParams.Viewer.Zoom * 100.0);
			hashtable["viewMode"] = requestParams.Viewer.ViewMode;
			hashtable["isEditableReport"] = StiEditableFieldsHelper.CheckEditableReport(report);
			hashtable["isCompilationMode"] = report.CalculationMode == StiCalculationMode.Compilation;
			hashtable["pagesCount"] = GetPagesCount(report, requestParams);
			hashtable["reportFileName"] = GetReportFileName(report);
			hashtable["collapsingStates"] = report.InteractionCollapsingStates;
			hashtable["resources"] = StiReportResourceHelper.GetResourcesItems(report);
			hashtable["previewSettings"] = GetReportPreviewSettings(report);
			hashtable["variablesValues"] = StiVariablesHelper.GetVariablesValues(report);
			hashtable["tableOfContentsPointers"] = GetTableOfContentsPointers(report, requestParams);
			hashtable["parametersOrientation"] = report.ParametersOrientation.ToString();
			if (!string.IsNullOrEmpty(report.ParametersDateFormat))
			{
				hashtable["parametersDateFormat"] = report.ParametersDateFormat;
			}
			if (requestParams.Action == StiAction.DrillDown || requestParams.Action == StiAction.DashboardDrillDown)
			{
				hashtable["drillDownParameters"] = requestParams.Interaction.DrillDown;
				hashtable["variablesPresentsInReport"] = report?.Dictionary.Variables.ToList().Any((StiVariable v) => v.RequestFromUser) ?? false;
			}
			if (report.Bookmark != null && report.Bookmark.Bookmarks.Count > 0 && requestParams.Viewer.ShowBookmarks)
			{
				hashtable["bookmarksContent"] = GetBookmarksContent(report, requestParams.Id, (requestParams.Viewer.ViewMode == StiWebViewMode.SinglePage) ? requestParams.Viewer.PageNumber : (-1));
			}
		}
		if (requestParams.UserValues != null)
		{
			hashtable["userValues"] = requestParams.UserValues;
		}
		return StiWebActionResult.JsonResult(requestParams, hashtable);
	}

	public static void CheckCloudLimits(Hashtable parameters, StiReport report)
	{
		StiCloudPlan plan = StiCloudPlan.GetPlan(report.ReportGuid);
		StiCloudReportLimits andRemoveLimits = StiCloudReportResults.GetAndRemoveLimits(report.ReportGuid);
		parameters["maxDataRows"] = andRemoveLimits.MaxDataRows;
		parameters["maxReportPages"] = andRemoveLimits.MaxReportPages;
		if (report.Dictionary.Resources.Count > plan.MaxResources)
		{
			parameters["maxResources"] = plan.MaxResources;
		}
		if (report.Dictionary.Resources.ToList().Any((StiResource r) => r.Content?.Length > plan.MaxResourceSize))
		{
			parameters["maxResourceSize"] = plan.MaxResourceSize;
		}
		if (!plan.AllowDatabases)
		{
			StiDatabase stiDatabase = report.Dictionary.Databases.ToList().FirstOrDefault((StiDatabase d) => !(d is StiFileDatabase));
			if (stiDatabase != null)
			{
				parameters["notAllowDatabase"] = stiDatabase.ServiceName;
			}
		}
		if (!plan.AllowDataTransformation && report.Dictionary.DataSources.ToList().Any((StiDataSource d) => d is StiDataTransformation))
		{
			parameters["notAllowDataTransformation"] = true;
		}
	}

	public static ArrayList GetTableOfContentsPointers(StiReport report, StiRequestParams requestParams)
	{
		ArrayList result = new ArrayList();
		if (report?.GetComponents().ToList().FirstOrDefault((StiComponent comp) => comp is StiTableOfContents) is StiTableOfContents stiTableOfContents)
		{
			StiBookmark pointer = report.Pointer;
			if (string.IsNullOrWhiteSpace(stiTableOfContents.ReportPointer))
			{
				result = GetBookmarkPointers(report, pointer);
			}
			else
			{
				pointer.Text = StiReportParser.Parse(stiTableOfContents.ReportPointer, stiTableOfContents);
				pointer.ParentComponent = report;
				result = GetBookmarkPointers(report, pointer);
			}
		}
		return result;
	}

	public static StiWebActionResult InteractionResult(StiRequestParams requestParams, StiReport report)
	{
		if (requestParams.Action == StiAction.InitVars)
		{
			if (report == null || report.IsDocument)
			{
				return new StiWebActionResult("null", "application/json");
			}
			report = GetCompiledReport(report);
			StiVariablesHelper.FillDialogInfoItems(report);
			StiVariablesHelper.ApplyReportBindingVariables(report, requestParams.Interaction.Variables);
			if (!requestParams.GetBoolean("isBindingVariable"))
			{
				StiVariableHelper.SetDefaultValueForRequestFromUserVariables(report, haveVars: true, allowParseQuery: true);
			}
			Hashtable variables2 = StiVariablesHelper.GetVariables(report, requestParams.Interaction.Variables, requestParams.Viewer.ParametersPanelSortDataItems);
			string data = JsonConvert.SerializeObject(variables2, Formatting.None, new StringEnumConverter());
			return new StiWebActionResult(data, "application/json");
		}
		if (report == null)
		{
			return StiWebActionResult.EmptyReportResult();
		}
		if (report.IsDocument)
		{
			return ViewerResult(requestParams, report);
		}
		report = GetCompiledReport(report);
		if ((requestParams.Cache.Mode == StiServerCacheMode.ObjectCache || requestParams.Cache.Mode == StiServerCacheMode.ObjectSession) && requestParams.Interaction.DrillDown != null && requestParams.Interaction.DrillDown.Count > 0)
		{
			report = StiReportCopier.CloneReport(report);
		}
		ApplyQueryParameters(requestParams, report);
		if (requestParams.Action == StiAction.RefreshReport)
		{
			report.IsRendered = false;
			report.InvokeRefreshing();
			StiCacheCleaner.Clean(report.Key);
		}
		if (requestParams.Action == StiAction.Variables || requestParams.Action == StiAction.Sorting || requestParams.Action == StiAction.Collapsing)
		{
			StiVariablesHelper.ApplyReportParameters(report, requestParams.Interaction.Variables);
			if (requestParams.Action == StiAction.Variables)
			{
				StiCacheCleaner.Clean(report.Key);
			}
		}
		if (requestParams.Interaction.DrillDown != null && requestParams.Interaction.DrillDown.Count > 0)
		{
			StiReport renderedReport = requestParams.Cache.Helper.GetReportInternal(requestParams, useDrillDownReportSnapshot: false);
			if (renderedReport == null)
			{
				renderedReport = report;
			}
			StiDashboardElementViewHelper.ParseDashboardDrillDownParameters(requestParams.Interaction.DrillDown, report);
			Hashtable hashtable = new Hashtable();
			foreach (Hashtable item in requestParams.Interaction.DrillDown)
			{
				if (renderedReport.Dictionary.Variables.Count > 0 && requestParams.GetHashtable("variables") != null)
				{
					Hashtable variables = requestParams.GetHashtable("variables");
					renderedReport.Dictionary.Variables.ToList().ForEach(delegate(StiVariable v)
					{
						if (variables.Contains(v.Name))
						{
							StiVariablesHelper.SetVariableValue(renderedReport, v.Name, variables[v.Name], v);
							renderedReport.IsRendered = false;
						}
					});
				}
				renderedReport = (item.Contains("isDashboardDrillDown") ? ApplyDashboardDrillDown(renderedReport, item) : ApplyDrillDown(report, renderedReport, item, hashtable));
			}
			bool flag = report != renderedReport;
			report = renderedReport;
			if (flag && report != null && requestParams.Action == StiAction.Variables && report.Dictionary.Variables.Count > 0 && requestParams.Interaction.Variables.Count > 0)
			{
				StiVariablesHelper.ApplyReportParameters(report, requestParams.Interaction.Variables);
				foreach (DictionaryEntry item2 in hashtable)
				{
					report[item2.Key?.ToString()] = item2.Value;
				}
			}
		}
		if (requestParams.Action == StiAction.Sorting)
		{
			ApplySorting(report, requestParams.Interaction.Sorting);
		}
		else if (requestParams.Action == StiAction.Collapsing)
		{
			ApplyCollapsing(report, requestParams.Interaction.Collapsing);
		}
		if (requestParams.Action == StiAction.DashboardElementDrillDown && requestParams.Interaction.DashboardElementDrillDownParameters != null)
		{
			StiDashboardElementDrillDownHelper.ApplyDashboardElementDrillDown(report, requestParams.Interaction.DashboardElementDrillDownParameters);
		}
		if (requestParams.Action == StiAction.DashboardElementDrillUp && requestParams.Interaction.DashboardElementDrillDownParameters != null)
		{
			StiDashboardElementDrillDownHelper.ApplyDashboardElementDrillUp(report, requestParams.Interaction.DashboardElementDrillDownParameters);
		}
		if (requestParams.Action == StiAction.DashboardFiltering)
		{
			StiDataFiltersHelper.ApplyFiltering(report, requestParams.Interaction.DashboardFiltering);
		}
		if (requestParams.Action == StiAction.DashboardSorting)
		{
			StiDataSortsHelper.ApplySorting(report, requestParams.Interaction.DashboardSorting);
		}
		if (!report.IsRendered && !report.ContainsOnlyDashboard)
		{
			report.IsReportRenderingAfterSubmit = true;
			try
			{
				report.Render(showProgress: false);
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
				{
					text = text + " " + ex.InnerException.Message;
				}
				return new StiWebActionResult("ServerError:" + text);
			}
			finally
			{
				report.IsReportRenderingAfterSubmit = false;
			}
		}
		requestParams.Cache.Helper.SaveReportInternal(requestParams, report);
		return ViewerResult(requestParams, report);
	}

	public static StiWebActionResult GetReportResult(StiRequestParams requestParams, StiReport report)
	{
		try
		{
			requestParams.Cache.Helper.RemoveReportInternal(requestParams);
			if (report != null)
			{
				report.Key = StiKeyHelper.GenerateKey();
				requestParams.Cache.Helper.SaveReportInternal(requestParams, report);
				ApplyQueryParameters(requestParams, report);
			}
			return ViewerResult(requestParams, report);
		}
		catch (StiDashboardNotSupportedException)
		{
			return StiWebActionResult.DashboardNotSupportedResult(requestParams);
		}
		catch (Exception e)
		{
			return StiWebActionResult.ErrorResult(requestParams, StiWebActionResult.GetErrorMessageText(e));
		}
	}

	public static StiReport LoadReportFromContent(StiRequestParams requestParams)
	{
		StiReport stiReport = new StiReport();
		string openingFileName = requestParams.Viewer.OpeningFileName;
		string openingFilePassword = requestParams.Viewer.OpeningFilePassword;
		bool flag = !string.IsNullOrEmpty(openingFileName) && (openingFileName.ToLower().EndsWith(".mrt") || openingFileName.ToLower().EndsWith(".mrx") || openingFileName.ToLower().EndsWith(".mrz"));
		if (requestParams.Data != null)
		{
			using MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(requestParams.Data, 0, requestParams.Data.Length);
			memoryStream.Position = 0L;
			if (flag)
			{
				if (string.IsNullOrEmpty(openingFilePassword))
				{
					stiReport.Load(memoryStream);
				}
				else
				{
					stiReport.LoadEncryptedReport(memoryStream, openingFilePassword);
				}
			}
			else if (string.IsNullOrEmpty(openingFilePassword))
			{
				stiReport.LoadDocument(memoryStream);
			}
			else
			{
				stiReport.LoadEncryptedDocument(memoryStream, openingFilePassword);
			}
		}
		return stiReport;
	}

	public static StiWebActionResult OpenReportResult(StiRequestParams requestParams)
	{
		try
		{
			StiReport report = LoadReportFromContent(requestParams);
			return GetReportResult(requestParams, report);
		}
		catch (StiDashboardNotSupportedException)
		{
			return StiWebActionResult.DashboardNotSupportedResult(requestParams);
		}
		catch (Exception e)
		{
			return StiWebActionResult.ErrorResult(requestParams, StiWebActionResult.GetErrorMessageText(e));
		}
	}

	public static StiWebActionResult ReportResourceResult(StiRequestParams requestParams, StiReport report)
	{
		string name = requestParams.ReportResourceParams["resourceName"] as string;
		if (report != null)
		{
			StiResource stiResource = report.Dictionary.Resources[name];
			if (stiResource != null && stiResource.Content != null)
			{
				MemoryStream data = new MemoryStream(stiResource.Content);
				string text = requestParams.ReportResourceParams["viewType"] as string;
				string fileName = ((text == "SaveFile") ? (stiResource.Name + stiResource.GetFileExt()) : null);
				return new StiWebActionResult(data, stiResource.GetContentType(), fileName);
			}
		}
		return null;
	}

	public static Hashtable GetReportPreviewSettings(StiReport report)
	{
		Hashtable hashtable = new Hashtable();
		StiPreviewSettings previewSettings = (StiPreviewSettings)report.PreviewSettings;
		hashtable["reportPrint"] = (previewSettings & StiPreviewSettings.Print) > StiPreviewSettings.None;
		hashtable["reportOpen"] = (previewSettings & StiPreviewSettings.Open) > StiPreviewSettings.None;
		hashtable["reportSave"] = (previewSettings & StiPreviewSettings.Save) > StiPreviewSettings.None;
		hashtable["reportSendEMail"] = (previewSettings & StiPreviewSettings.SendEMail) > StiPreviewSettings.None;
		hashtable["reportPageControl"] = (previewSettings & StiPreviewSettings.PageControl) > StiPreviewSettings.None;
		hashtable["reportEditor"] = (previewSettings & StiPreviewSettings.EditorTool) > StiPreviewSettings.None;
		hashtable["reportFind"] = (previewSettings & StiPreviewSettings.FindTool) > StiPreviewSettings.None;
		hashtable["reportPageViewMode"] = (previewSettings & StiPreviewSettings.PageViewMode) > StiPreviewSettings.None;
		hashtable["reportZoom"] = (previewSettings & StiPreviewSettings.Zoom) > StiPreviewSettings.None;
		hashtable["reportBookmarks"] = (previewSettings & StiPreviewSettings.Bookmarks) > StiPreviewSettings.None;
		hashtable["reportParameters"] = (previewSettings & StiPreviewSettings.Parameters) > StiPreviewSettings.None;
		hashtable["reportResources"] = (previewSettings & StiPreviewSettings.Resources) > StiPreviewSettings.None;
		hashtable["reportStatusBar"] = (previewSettings & StiPreviewSettings.StatusBar) > StiPreviewSettings.None;
		hashtable["reportToolbar"] = (previewSettings & StiPreviewSettings.Toolbar) > StiPreviewSettings.None;
		StiDashboardViewerSettings dashboardViewerSettings = report.DashboardViewerSettings;
		hashtable["dashboardToolBar"] = (dashboardViewerSettings & StiDashboardViewerSettings.ShowToolBar) > StiDashboardViewerSettings.None;
		hashtable["dashboardRefreshButton"] = (dashboardViewerSettings & StiDashboardViewerSettings.ShowRefreshButton) > StiDashboardViewerSettings.None;
		hashtable["dashboardOpenButton"] = (dashboardViewerSettings & StiDashboardViewerSettings.ShowOpenButton) > StiDashboardViewerSettings.None;
		hashtable["dashboardEditButton"] = (dashboardViewerSettings & StiDashboardViewerSettings.ShowEditButton) > StiDashboardViewerSettings.None;
		hashtable["dashboardFullScreenButton"] = (dashboardViewerSettings & StiDashboardViewerSettings.ShowFullScreenButton) > StiDashboardViewerSettings.None;
		hashtable["dashboardMenuButton"] = (dashboardViewerSettings & StiDashboardViewerSettings.ShowMenuButton) > StiDashboardViewerSettings.None;
		hashtable["dashboardShowReportSnapshots"] = (dashboardViewerSettings & StiDashboardViewerSettings.ShowReportSnapshots) > StiDashboardViewerSettings.None;
		hashtable["dashboardShowExports"] = (dashboardViewerSettings & StiDashboardViewerSettings.ShowExports) > StiDashboardViewerSettings.None;
		return hashtable;
	}
}
