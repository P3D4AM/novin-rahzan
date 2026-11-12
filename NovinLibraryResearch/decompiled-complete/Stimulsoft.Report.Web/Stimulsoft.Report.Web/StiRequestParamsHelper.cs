using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Caching;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Web;

internal class StiRequestParamsHelper
{
	internal static string EncryptingKey = "8pTP5X15uKADcSw7";

	public static StiRequestParams Get()
	{
		return Get(HttpContext.Current);
	}

	public static StiRequestParams Get(HttpContext httpContext)
	{
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		StiRequestParams stiRequestParams = new StiRequestParams();
		stiRequestParams.HttpContext = httpContext;
		if (httpContext != null)
		{
			foreach (string key in httpContext.Request.Params.Keys)
			{
				if (key != null && key.StartsWith("stiweb_"))
				{
					stiRequestParams.All[key] = httpContext.Request.Params[key].ToString();
				}
			}
			if (stiRequestParams.Contains("stiweb_component"))
			{
				stiRequestParams.ComponentType = (StiComponentType)stiRequestParams.GetEnum("stiweb_component", typeof(StiComponentType));
			}
			if (stiRequestParams.Contains("stiweb_action"))
			{
				stiRequestParams.Action = (StiAction)stiRequestParams.GetEnum("stiweb_action", typeof(StiAction));
			}
			if (stiRequestParams.Contains("stiweb_theme"))
			{
				stiRequestParams.Theme = stiRequestParams.GetString("stiweb_theme");
			}
			if (stiRequestParams.Contains("stiweb_cloudmode"))
			{
				stiRequestParams.CloudMode = true;
			}
			if (stiRequestParams.Contains("stiweb_version"))
			{
				stiRequestParams.Version = stiRequestParams.GetString("stiweb_version");
			}
			if (stiRequestParams.Contains("stiweb_imagesscalingfactor"))
			{
				stiRequestParams.All["imagesScalingFactor"] = stiRequestParams.GetString("stiweb_imagesscalingfactor");
			}
			if (stiRequestParams.Contains("stiweb_sharinglocalization"))
			{
				stiRequestParams.All["sharingLocalization"] = stiRequestParams.GetString("stiweb_sharinglocalization");
			}
			if (stiRequestParams.Contains("stiweb_loc"))
			{
				string @string = stiRequestParams.GetString("stiweb_loc");
				stiRequestParams.Localization = StiEncryption.Decrypt(Encoding.UTF8.GetString(Convert.FromBase64String(@string)), EncryptingKey);
			}
			if (stiRequestParams.Contains("stiweb_data"))
			{
				if (stiRequestParams.Action == StiAction.Resource)
				{
					stiRequestParams.Resource = stiRequestParams.GetString("stiweb_data");
				}
				else
				{
					stiRequestParams.Data = Convert.FromBase64String(stiRequestParams.GetString("stiweb_data"));
				}
			}
			if (stiRequestParams.Contains("stiweb_parameters"))
			{
				byte[] bytes = Convert.FromBase64String(stiRequestParams.GetString("stiweb_parameters"));
				string string2 = Encoding.UTF8.GetString(bytes);
				JContainer container = JsonConvert.DeserializeObject<JContainer>(string2);
				ParseParameters(container, stiRequestParams.All, null);
				stiRequestParams.HasParameters = true;
				stiRequestParams.Server.UseCompression = false;
			}
			if (stiRequestParams.Contains("stiweb_packed_parameters"))
			{
				byte[] bytes2 = Convert.FromBase64String(stiRequestParams.GetString("stiweb_packed_parameters"));
				byte[] bytes3 = StiGZipHelper.Unpack(bytes2);
				string string3 = Encoding.UTF8.GetString(bytes3);
				JContainer container2 = JsonConvert.DeserializeObject<JContainer>(string3);
				ParseParameters(container2, stiRequestParams.All, null);
				stiRequestParams.HasParameters = true;
				stiRequestParams.Server.UseCompression = true;
			}
			if (stiRequestParams.Contains("stiweb_cachemode"))
			{
				switch (stiRequestParams.GetString("stiweb_cachemode"))
				{
				case "none":
					stiRequestParams.Cache.Mode = StiServerCacheMode.None;
					break;
				case "cache":
					stiRequestParams.Cache.Mode = StiServerCacheMode.ObjectCache;
					break;
				case "session":
					stiRequestParams.Cache.Mode = StiServerCacheMode.ObjectSession;
					break;
				}
			}
			if (stiRequestParams.HasParameters && stiRequestParams.ComponentType == StiComponentType.Viewer)
			{
				stiRequestParams.Id = stiRequestParams.GetString("viewerId");
				stiRequestParams.Routes = stiRequestParams.GetNameValueCollection("routes");
				stiRequestParams.FormValues = stiRequestParams.GetNameValueCollection("formValues");
				stiRequestParams.UserValues = stiRequestParams.GetHashtable("userValues");
				stiRequestParams.CloudMode = stiRequestParams.CloudMode || stiRequestParams.GetBoolean("cloudMode");
				stiRequestParams.ServerMode = stiRequestParams.ServerMode || stiRequestParams.GetBoolean("serverMode");
				stiRequestParams.ExportFormat = (StiExportFormat)stiRequestParams.GetEnum("exportFormat", typeof(StiExportFormat));
				stiRequestParams.ExportSettings = stiRequestParams.GetHashtable("exportSettings");
				stiRequestParams.Version = stiRequestParams.GetString("version");
				stiRequestParams.Localization = stiRequestParams.GetString("localization");
				stiRequestParams.Cache.ClientGuid = stiRequestParams.GetString("clientGuid");
				stiRequestParams.Cache.DrillDownGuid = stiRequestParams.GetString("drillDownGuid");
				stiRequestParams.Cache.DashboardDrillDownGuid = stiRequestParams.GetString("dashboardDrillDownGuid");
				stiRequestParams.Cache.Timeout = new TimeSpan(0, int.Parse(stiRequestParams.GetString("cacheTimeout")), 0);
				stiRequestParams.Cache.Mode = (StiServerCacheMode)stiRequestParams.GetEnum("cacheMode", typeof(StiServerCacheMode));
				stiRequestParams.Cache.Priority = (CacheItemPriority)stiRequestParams.GetEnum("cacheItemPriority", typeof(CacheItemPriority));
				stiRequestParams.Cache.UseLocalizedCache = stiRequestParams.GetBoolean("useLocalizedCache");
				stiRequestParams.Interaction.Variables = stiRequestParams.GetHashtable("variables");
				stiRequestParams.Interaction.Sorting = stiRequestParams.GetHashtable("sortingParameters");
				stiRequestParams.Interaction.Collapsing = stiRequestParams.GetHashtable("collapsingParameters");
				stiRequestParams.Interaction.DrillDown = stiRequestParams.GetArray("drillDownParameters");
				stiRequestParams.Interaction.Editable = stiRequestParams.GetHashtable("editableParameters");
				stiRequestParams.Interaction.DashboardFiltering = stiRequestParams.GetHashtable("dashboardFilteringParameters");
				stiRequestParams.Interaction.DashboardSorting = stiRequestParams.GetHashtable("dashboardSortingParameters");
				stiRequestParams.Interaction.DashboardElementDrillDownParameters = stiRequestParams.GetHashtable("dashboardElementDrillDownParameters");
				stiRequestParams.Interaction.DrillDownReportFile = stiRequestParams.GetBoolean("drillDownReportFile");
				stiRequestParams.Viewer.PageNumber = stiRequestParams.GetInt("pageNumber");
				stiRequestParams.Viewer.OriginalPageNumber = stiRequestParams.GetInt("originalPageNumber");
				stiRequestParams.Viewer.Zoom = Convert.ToDouble(stiRequestParams.GetString("zoom")) / 100.0;
				stiRequestParams.Viewer.ShowBookmarks = stiRequestParams.GetBoolean("showBookmarks");
				stiRequestParams.Viewer.BookmarksPrint = stiRequestParams.GetBoolean("bookmarksPrint");
				stiRequestParams.Viewer.OpeningFileName = stiRequestParams.GetString("openingFileName");
				stiRequestParams.Viewer.OpeningFilePassword = stiRequestParams.GetString("openingFilePassword");
				stiRequestParams.Viewer.ViewMode = (StiWebViewMode)stiRequestParams.GetEnum("viewMode", typeof(StiWebViewMode));
				stiRequestParams.Viewer.ChartRenderType = (StiChartRenderType)stiRequestParams.GetEnum("chartRenderType", typeof(StiChartRenderType));
				stiRequestParams.Viewer.ReportDisplayMode = (StiReportDisplayMode)stiRequestParams.GetEnum("reportDisplayMode", typeof(StiReportDisplayMode));
				stiRequestParams.Viewer.PrintAction = (StiPrintAction)stiRequestParams.GetEnum("printAction", typeof(StiPrintAction));
				stiRequestParams.Viewer.OpenLinksWindow = stiRequestParams.GetString("openLinksWindow");
				stiRequestParams.Viewer.ReportDesignerMode = stiRequestParams.GetBoolean("reportDesignerMode");
				stiRequestParams.Viewer.DashboardWidth = stiRequestParams.GetInt("dashboardWidth");
				stiRequestParams.Viewer.DashboardHeight = stiRequestParams.GetInt("dashboardHeight");
				stiRequestParams.Viewer.ReportType = (StiReportType)stiRequestParams.GetEnum("reportType", typeof(StiReportType));
				stiRequestParams.Viewer.ElementName = stiRequestParams.GetString("elementName");
				stiRequestParams.Viewer.ImagesQuality = (StiImagesQuality)stiRequestParams.GetEnum("imagesQuality", typeof(StiImagesQuality));
				stiRequestParams.Viewer.ParametersPanelSortDataItems = stiRequestParams.GetBoolean("parametersPanelSortDataItems");
				stiRequestParams.Viewer.CombineReportPages = stiRequestParams.GetBoolean("combineReportPages");
				stiRequestParams.Server.UseRelativeUrls = stiRequestParams.GetBoolean("useRelativeUrls");
				stiRequestParams.Server.PassQueryParametersForResources = stiRequestParams.GetBoolean("passQueryParametersForResources");
				stiRequestParams.Server.PassQueryParametersToReport = stiRequestParams.GetBoolean("passQueryParametersToReport");
				stiRequestParams.Server.AllowAutoUpdateCookies = stiRequestParams.GetBoolean("allowAutoUpdateCookies");
				stiRequestParams.ReportResourceParams = stiRequestParams.GetHashtable("reportResourceParams");
			}
			if (stiRequestParams.HasParameters && stiRequestParams.ComponentType == StiComponentType.Designer)
			{
				stiRequestParams.Id = stiRequestParams.GetString("designerId");
				stiRequestParams.Routes = stiRequestParams.GetNameValueCollection("routes");
				stiRequestParams.FormValues = stiRequestParams.GetNameValueCollection("formValues");
				stiRequestParams.CloudMode = stiRequestParams.CloudMode || stiRequestParams.GetBoolean("cloudMode");
				stiRequestParams.ServerMode = stiRequestParams.ServerMode || stiRequestParams.GetBoolean("serverMode");
				stiRequestParams.Version = stiRequestParams.GetString("version");
				stiRequestParams.Cache.ClientGuid = stiRequestParams.GetString("clientGuid");
				stiRequestParams.Cache.Timeout = new TimeSpan(0, int.Parse(stiRequestParams.GetString("cacheTimeout")), 0);
				stiRequestParams.Cache.Mode = (StiServerCacheMode)stiRequestParams.GetEnum("cacheMode", typeof(StiServerCacheMode));
				stiRequestParams.Cache.Priority = (CacheItemPriority)stiRequestParams.GetEnum("cacheItemPriority", typeof(CacheItemPriority));
				stiRequestParams.Cache.UseCacheHelper = stiRequestParams.GetBoolean("useCacheHelper");
				stiRequestParams.Designer.Command = (StiDesignerCommand)stiRequestParams.GetEnum("command", typeof(StiDesignerCommand));
				stiRequestParams.Designer.IsSaveAs = stiRequestParams.Designer.Command == StiDesignerCommand.SaveAsReport;
				stiRequestParams.Designer.IsNewReport = stiRequestParams.GetBoolean("isNewReport");
				stiRequestParams.Designer.UndoMaxLevel = stiRequestParams.GetInt("undoMaxLevel");
				stiRequestParams.Designer.CurrentCultureName = stiRequestParams.GetString("currentCultureName");
				stiRequestParams.Designer.NewReportDictionary = (StiNewReportDictionary)stiRequestParams.GetEnum("newReportDictionary", typeof(StiNewReportDictionary));
				stiRequestParams.Server.UseRelativeUrls = stiRequestParams.GetBoolean("useRelativeUrls");
				stiRequestParams.Server.PassQueryParametersForResources = stiRequestParams.GetBoolean("passQueryParametersForResources");
				stiRequestParams.Server.AllowAutoUpdateCookies = stiRequestParams.GetBoolean("allowAutoUpdateCookies");
				if (stiRequestParams.Contains("checkReportBeforePreview"))
				{
					stiRequestParams.Designer.CheckReportBeforePreview = stiRequestParams.GetBoolean("checkReportBeforePreview");
				}
				if (stiRequestParams.Contains("localization"))
				{
					stiRequestParams.Localization = stiRequestParams.GetString("localization");
				}
				if (stiRequestParams.Contains("openReportFile"))
				{
					stiRequestParams.Designer.FileName = stiRequestParams.GetString("openReportFile");
				}
				else
				{
					stiRequestParams.Designer.FileName = stiRequestParams.GetString("reportFile");
				}
				if (stiRequestParams.Contains("encryptedPassword"))
				{
					stiRequestParams.Designer.Password = stiRequestParams.GetString("encryptedPassword");
				}
				else
				{
					stiRequestParams.Designer.Password = stiRequestParams.GetString("password");
				}
				if (stiRequestParams.Contains("saveType"))
				{
					stiRequestParams.Designer.SaveType = stiRequestParams.GetString("saveType");
				}
				switch (stiRequestParams.Designer.Command)
				{
				case StiDesignerCommand.GetReportForDesigner:
					stiRequestParams.Action = StiAction.GetReport;
					break;
				case StiDesignerCommand.OpenReport:
					stiRequestParams.Action = StiAction.OpenReport;
					break;
				case StiDesignerCommand.CreateReport:
				case StiDesignerCommand.WizardResult:
				case StiDesignerCommand.CreateDashboard:
					stiRequestParams.Action = StiAction.CreateReport;
					break;
				case StiDesignerCommand.SaveReport:
				case StiDesignerCommand.SaveAsReport:
					stiRequestParams.Action = StiAction.SaveReport;
					break;
				case StiDesignerCommand.LoadReportToViewer:
					stiRequestParams.Action = StiAction.PreviewReport;
					break;
				case StiDesignerCommand.ExitDesigner:
					stiRequestParams.Action = StiAction.Exit;
					break;
				}
			}
		}
		return stiRequestParams;
	}

	private static void ParseParameters(JContainer container, Hashtable hash, ArrayList array)
	{
		if (hash == null && array == null)
		{
			return;
		}
		int num = -1;
		foreach (JToken item in container.Children())
		{
			num++;
			string key = ((item.Type == JTokenType.Property) ? ((JProperty)item).Name : num.ToString());
			JToken jToken = ((item.Type == JTokenType.Property) ? ((JProperty)item).Value : item);
			Hashtable hashtable = ((jToken is JObject) ? new Hashtable() : null);
			ArrayList arrayList = ((jToken is JArray) ? new ArrayList() : null);
			if (jToken is JContainer)
			{
				ParseParameters((JContainer)jToken, (hashtable != null) ? hashtable : null, (arrayList != null) ? arrayList : null);
			}
			if (hash != null)
			{
				if (hashtable != null)
				{
					hash[key] = hashtable;
				}
				else if (arrayList != null)
				{
					hash[key] = arrayList;
				}
				else
				{
					hash[key] = ((JValue)jToken).Value;
				}
			}
			else if (hashtable != null)
			{
				array.Add(hashtable);
			}
			else if (arrayList != null)
			{
				array.Add(arrayList);
			}
			else
			{
				array.Add(((JValue)jToken).Value);
			}
		}
	}
}
