using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Web;

public class StiCacheHelper
{
	private static class DataSerializer
	{
		public static byte[] Serialize(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is byte[])
			{
				return (byte[])obj;
			}
			return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Stimulsoft.Base.Json.Formatting.None, new StringEnumConverter()));
		}

		public static T Deserialize<T>(byte[] bytes)
		{
			if (bytes == null)
			{
				return default(T);
			}
			if (GetReportContentType(bytes) != StiReportContentType.Undefined)
			{
				return (T)(object)bytes;
			}
			return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes), new JsonConverter[1]
			{
				new StringEnumConverter()
			});
		}
	}

	private static ConcurrentDictionary<string, StiCacheGuid> CacheGuids = new ConcurrentDictionary<string, StiCacheGuid>();

	public const string GUID_ReportSnapshot = "";

	public const string GUID_ReportTemplate = "template";

	public const string GUID_Clipboard = "clipboard";

	public const string GUID_ComponentClone = "clone";

	public const string GUID_ReportCheckers = "checkers";

	public const string GUID_UndoArray = "undo";

	public const string GUID_DataTransformation = "datatransform";

	public HttpContext HttpContext { get; set; }

	public StiServerCacheMode CacheMode { get; set; }

	public TimeSpan Timeout { get; set; }

	public CacheItemPriority Priority { get; set; }

	public StiCacheObjectType GetCacheObjectType(string guid)
	{
		if (string.IsNullOrEmpty(guid))
		{
			return StiCacheObjectType.Undefined;
		}
		if (guid.EndsWith("template"))
		{
			return StiCacheObjectType.ReportTemplate;
		}
		if (guid.EndsWith("clipboard"))
		{
			return StiCacheObjectType.Clipboard;
		}
		if (guid.EndsWith("clone"))
		{
			return StiCacheObjectType.ComponentClone;
		}
		if (guid.EndsWith("checkers"))
		{
			return StiCacheObjectType.ReportCheckers;
		}
		if (guid.EndsWith("undo"))
		{
			return StiCacheObjectType.UndoArray;
		}
		if (guid.EndsWith("datatransform"))
		{
			return StiCacheObjectType.DataTransformation;
		}
		return StiCacheObjectType.ReportSnapshot;
	}

	private void UpdateCacheGuids(StiRequestParams requestParams, string reportGuid, string reportKey)
	{
		string clientGuid = requestParams.Cache.ClientGuid;
		string id = requestParams.Id;
		CacheGuids.TryGetValue(clientGuid, out var currentCacheGuid);
		if (currentCacheGuid == null)
		{
			currentCacheGuid = new StiCacheGuid();
		}
		currentCacheGuid.UpdateTime = DateTime.Now;
		currentCacheGuid.ReportKey = reportKey;
		if (!currentCacheGuid.ReportGuids.Contains(reportGuid))
		{
			currentCacheGuid.ReportGuids.Add(reportGuid);
		}
		CacheGuids.AddOrUpdate(clientGuid, currentCacheGuid, (string prevClientGuid, StiCacheGuid prevCurrentCacheGuid) => currentCacheGuid);
		RemoveObsoleteCacheGuids(requestParams);
	}

	private void RemoveObsoleteCacheGuids(StiRequestParams requestParams)
	{
		DateTime now = DateTime.Now;
		foreach (string key in CacheGuids.Keys)
		{
			CacheGuids.TryGetValue(key, out var value);
			if (value == null)
			{
				continue;
			}
			TimeSpan timeSpan = new TimeSpan((now - value.UpdateTime).Ticks);
			if (!(timeSpan > Timeout))
			{
				continue;
			}
			StiCacheCleaner.Clean(value.ReportKey);
			foreach (string reportGuid in value.ReportGuids)
			{
				RemoveReport(reportGuid);
			}
			RemoveAllAdditionalCaches(requestParams);
			CacheGuids.TryRemove(key, out value);
		}
	}

	private void RemoveAllAdditionalCaches(StiRequestParams requestParams)
	{
		string clientGuid = requestParams.Cache.ClientGuid;
		string id = requestParams.Id;
		string[] array = new string[5] { "clipboard", "clone", "checkers", "undo", "datatransform" };
		string[] array2 = array;
		foreach (string text in array2)
		{
			RemoveObject(id + "_" + clientGuid + "_" + text);
		}
	}

	public virtual object GetObject(string guid)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		object obj = null;
		if (CacheMode == StiServerCacheMode.ObjectCache || CacheMode == StiServerCacheMode.StringCache)
		{
			obj = HttpContext.Cache[guid];
			if (obj != null)
			{
				HttpContext.Cache.Remove(guid);
				HttpContext.Cache.Add(guid, obj, (CacheDependency)null, Cache.NoAbsoluteExpiration, Timeout, Priority, (CacheItemRemovedCallback)null);
			}
		}
		else if (CacheMode == StiServerCacheMode.ObjectSession || CacheMode == StiServerCacheMode.StringSession)
		{
			obj = HttpContext.Session[guid];
			if (obj != null)
			{
				HttpContext.Session.Remove(guid);
				HttpContext.Session.Add(guid, obj);
			}
		}
		if (obj is StiReport)
		{
			return obj as StiReport;
		}
		if (obj is string && (CacheMode == StiServerCacheMode.StringCache || CacheMode == StiServerCacheMode.StringSession))
		{
			StiCacheObjectType cacheObjectType = GetCacheObjectType(guid);
			if (cacheObjectType == StiCacheObjectType.UndoArray || cacheObjectType == StiCacheObjectType.ReportCheckers || cacheObjectType == StiCacheObjectType.Clipboard || cacheObjectType == StiCacheObjectType.ComponentClone || cacheObjectType == StiCacheObjectType.DataTransformation)
			{
				byte[] fileData = StiGZipHelper.Unpack(Convert.FromBase64String((string)obj));
				return GetObjectFromCacheData(fileData);
			}
			StiReport stiReport = new StiReport();
			if (cacheObjectType == StiCacheObjectType.ReportTemplate)
			{
				stiReport.LoadPackedReportFromString(obj as string);
			}
			else
			{
				stiReport.LoadPackedDocumentFromString(obj as string);
			}
			stiReport.Info.Zoom = 1.0;
			return stiReport;
		}
		return obj;
	}

	public virtual void SaveObject(object obj, string guid)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		string text = null;
		if (CacheMode == StiServerCacheMode.ObjectCache || CacheMode == StiServerCacheMode.StringCache)
		{
			HttpContext.Cache.Remove(guid);
		}
		else
		{
			HttpContext.Session.Remove(guid);
		}
		if (CacheMode == StiServerCacheMode.ObjectCache)
		{
			HttpContext.Cache.Add(guid, obj, (CacheDependency)null, Cache.NoAbsoluteExpiration, Timeout, Priority, (CacheItemRemovedCallback)null);
		}
		else if (CacheMode == StiServerCacheMode.ObjectSession)
		{
			HttpContext.Session.Add(guid, obj);
		}
		else if (CacheMode == StiServerCacheMode.StringCache || CacheMode == StiServerCacheMode.StringSession)
		{
			StiCacheObjectType cacheObjectType = GetCacheObjectType(guid);
			text = ((!(obj is StiReport)) ? Convert.ToBase64String(StiGZipHelper.Pack(GetCacheDataFromObject(obj))) : ((cacheObjectType == StiCacheObjectType.ReportTemplate) ? ((StiReport)obj).SavePackedReportToString() : ((StiReport)obj).SavePackedDocumentToString()));
			if (CacheMode == StiServerCacheMode.StringCache)
			{
				HttpContext.Cache.Add(guid, (object)text, (CacheDependency)null, Cache.NoAbsoluteExpiration, Timeout, Priority, (CacheItemRemovedCallback)null);
			}
			else
			{
				HttpContext.Session.Add(guid, (object)text);
			}
		}
	}

	public virtual void RemoveObject(string guid)
	{
		if (CacheMode == StiServerCacheMode.ObjectCache || CacheMode == StiServerCacheMode.StringCache)
		{
			HttpContext.Cache.Remove(guid);
		}
		else if (CacheMode != 0)
		{
			HttpContext.Session.Remove(guid);
		}
	}

	public virtual StiReport GetReport(string guid)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return GetObject(guid, CacheMode, Timeout, Priority) as StiReport;
	}

	public virtual void SaveReport(StiReport report, string guid)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		SaveObject(report, guid, CacheMode, Timeout, Priority);
		SaveObject(report, guid + "_animations", CacheMode, Timeout, Priority);
	}

	public virtual void RemoveReport(string guid)
	{
		RemoveObject(guid, CacheMode);
		if (guid.IndexOf("template") < 0)
		{
			RemoveObject(guid + "_template", CacheMode);
		}
	}

	public virtual string GetResource(string guid)
	{
		if (CacheMode == StiServerCacheMode.StringCache)
		{
			CacheMode = StiServerCacheMode.ObjectCache;
		}
		else if (CacheMode == StiServerCacheMode.StringSession)
		{
			CacheMode = StiServerCacheMode.ObjectSession;
		}
		object @object = GetObject(guid);
		if (@object is string)
		{
			return (string)@object;
		}
		return null;
	}

	public virtual void SaveResource(string text, string guid)
	{
		if (CacheMode == StiServerCacheMode.StringCache)
		{
			CacheMode = StiServerCacheMode.ObjectCache;
		}
		else if (CacheMode == StiServerCacheMode.StringSession)
		{
			CacheMode = StiServerCacheMode.ObjectSession;
		}
		SaveObject(text, guid);
	}

	public virtual void RemoveResource(string guid)
	{
		RemoveObject(guid);
	}

	internal object GetObjectInternal(StiRequestParams requestParams, string objectGuid)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		ApplyRequestParameters(requestParams);
		string guid = (string.IsNullOrEmpty(objectGuid) ? (requestParams.Id + "_" + requestParams.Cache.ClientGuid) : (requestParams.Id + "_" + requestParams.Cache.ClientGuid + "_" + objectGuid));
		return GetObject(guid, CacheMode, Timeout, Priority);
	}

	internal void SaveObjectInternal(object obj, StiRequestParams requestParams, string objectGuid)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		ApplyRequestParameters(requestParams);
		string guid = (string.IsNullOrEmpty(objectGuid) ? (requestParams.Id + "_" + requestParams.Cache.ClientGuid) : (requestParams.Id + "_" + requestParams.Cache.ClientGuid + "_" + objectGuid));
		SaveObject(obj, guid, CacheMode, Timeout, Priority);
	}

	internal void UpdateObjectCacheInternal(StiRequestParams requestParams, string objectGuid)
	{
		GetObjectInternal(requestParams, objectGuid);
		if (string.IsNullOrEmpty(objectGuid))
		{
			GetObjectInternal(requestParams, "template");
			if (!string.IsNullOrEmpty(requestParams.Cache.DashboardDrillDownGuid))
			{
				GetObjectInternal(requestParams, requestParams.Cache.DashboardDrillDownGuid);
			}
			if (!string.IsNullOrEmpty(requestParams.Cache.DrillDownGuid))
			{
				GetObjectInternal(requestParams, requestParams.Cache.DrillDownGuid);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public StiReport GetReportInternal(StiRequestParams requestParams)
	{
		return GetReportInternal(requestParams, useDrillDownReportSnapshot: true);
	}

	internal StiReport GetReportInternal(StiRequestParams requestParams, bool useDrillDownReportSnapshot)
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		StiReport report = requestParams.Report;
		if (report == null && requestParams.Cache.Mode != 0 && !string.IsNullOrEmpty(requestParams.Cache.ClientGuid) && requestParams.Action != 0 && requestParams.Action != StiAction.Resource)
		{
			ApplyRequestParameters(requestParams);
			if (!string.IsNullOrEmpty(requestParams.Cache.DashboardDrillDownGuid) && string.IsNullOrEmpty(requestParams.Cache.DrillDownGuid) && (requestParams.Action == StiAction.DashboardDrillDown || requestParams.Action == StiAction.DashboardElementDrillDown || requestParams.Action == StiAction.DashboardElementDrillUp || requestParams.Action == StiAction.DashboardSorting || requestParams.Action == StiAction.DashboardFiltering || requestParams.Action == StiAction.DashboardGettingFilterItems || requestParams.Action == StiAction.DashboardViewData || requestParams.Action == StiAction.RefreshReport || requestParams.Action == StiAction.GetPages || requestParams.Action == StiAction.ExportDashboard))
			{
				string reportCacheGuid = GetReportCacheGuid(requestParams, requestParams.Cache.DashboardDrillDownGuid + "_template");
				StiReport report2 = GetReport(reportCacheGuid, CacheMode, Timeout, Priority);
				if (report2 != null)
				{
					UpdateCacheGuids(requestParams, reportCacheGuid, report2.Key);
					return report2;
				}
			}
			if ((requestParams.ComponentType == StiComponentType.Designer && requestParams.Action != StiAction.PrintReport && requestParams.Action != StiAction.ExportReport && requestParams.Action != StiAction.EmailReport) || (useDrillDownReportSnapshot && ((requestParams.Action == StiAction.InitVars && !requestParams.Interaction.DrillDownReportFile) || requestParams.Action == StiAction.Variables || requestParams.Action == StiAction.Sorting || requestParams.Action == StiAction.Collapsing || requestParams.Action == StiAction.DrillDown || requestParams.Action == StiAction.DashboardDrillDown || requestParams.Action == StiAction.ExportDashboard || requestParams.Action == StiAction.RefreshReport)))
			{
				string reportCacheGuid2 = GetReportCacheGuid(requestParams, "template");
				report = GetReport(reportCacheGuid2, CacheMode, Timeout, Priority);
				if (report != null && requestParams.Action != StiAction.InitVars)
				{
					report.IsRendered = false;
					UpdateCacheGuids(requestParams, reportCacheGuid2, report.Key);
				}
			}
			else
			{
				string text = (((useDrillDownReportSnapshot || requestParams.Interaction.DrillDownReportFile) && !string.IsNullOrEmpty(requestParams.Cache.DrillDownGuid)) ? GetReportCacheGuid(requestParams, requestParams.Cache.DrillDownGuid) : GetReportCacheGuid(requestParams));
				report = GetReport(text, CacheMode, Timeout, Priority);
				if (report == null)
				{
					text = GetReportCacheGuid(requestParams, "template");
					report = GetReport(text, CacheMode, Timeout, Priority);
				}
				if (report != null)
				{
					UpdateCacheGuids(requestParams, text, report.Key);
				}
			}
		}
		if (report != null)
		{
			if (report.CookieContainer == null || report.CookieContainer.Count == 0 || requestParams.Server.AllowAutoUpdateCookies)
			{
				report.CookieContainer = StiReportHelper.GetCookieContainer(requestParams.HttpContext.Request);
			}
			if (requestParams.ComponentType == StiComponentType.Designer && (requestParams.Cache.Mode == StiServerCacheMode.StringCache || requestParams.Cache.Mode == StiServerCacheMode.StringSession || requestParams.Cache.UseCacheHelper))
			{
				report.Info.ForceDesigningMode = true;
				report.Pages.ToList().ForEach(delegate(StiPage p)
				{
					p.CheckLargeHeight();
				});
			}
		}
		return report;
	}

	internal void SaveReportInternal(StiRequestParams requestParams, StiReport report)
	{
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		if (report == null || requestParams.Cache.Mode == StiServerCacheMode.None || string.IsNullOrEmpty(requestParams.Cache.ClientGuid) || requestParams.Action == StiAction.Undefined || requestParams.Action == StiAction.Resource)
		{
			return;
		}
		ApplyRequestParameters(requestParams);
		if (report.CookieContainer == null || report.CookieContainer.Count == 0 || requestParams.Server.AllowAutoUpdateCookies)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer(requestParams.HttpContext.Request);
		}
		if (!string.IsNullOrEmpty(requestParams.Cache.DashboardDrillDownGuid) && (requestParams.Action == StiAction.DashboardDrillDown || requestParams.Action == StiAction.DashboardElementDrillDown || requestParams.Action == StiAction.DashboardElementDrillUp || requestParams.Action == StiAction.DashboardSorting || requestParams.Action == StiAction.DashboardFiltering || requestParams.Action == StiAction.DashboardGettingFilterItems || requestParams.Action == StiAction.DashboardViewData || requestParams.Action == StiAction.RefreshReport))
		{
			string reportCacheGuid = GetReportCacheGuid(requestParams, requestParams.Cache.DashboardDrillDownGuid + "_template");
			SaveReport(report, reportCacheGuid, CacheMode, Timeout, Priority);
			UpdateCacheGuids(requestParams, reportCacheGuid, report.Key);
			return;
		}
		if (requestParams.Action != StiAction.Variables && requestParams.Action != StiAction.Sorting && requestParams.Action != StiAction.Collapsing && requestParams.Action != StiAction.DrillDown && requestParams.Action != StiAction.PreviewReport && requestParams.Action != StiAction.RefreshReport && !report.IsDocument && (requestParams.ComponentType == StiComponentType.Designer || report.Dictionary.IsRequestFromUserVariablesPresent || StiReportHelper.IsReportHasInteractions(report) || report.ContainsDashboard || report.RefreshTime > 0))
		{
			string reportCacheGuid2 = GetReportCacheGuid(requestParams, "template");
			SaveReport(report, reportCacheGuid2, CacheMode, Timeout, Priority);
			if (requestParams.ComponentType == StiComponentType.Designer)
			{
				UpdateCacheGuids(requestParams, reportCacheGuid2, report.Key);
				return;
			}
		}
		else if (!requestParams.Viewer.ReportDesignerMode && (requestParams.Action == StiAction.GetReport || requestParams.Action == StiAction.OpenReport))
		{
			string reportCacheGuid3 = GetReportCacheGuid(requestParams);
			RemoveReport(reportCacheGuid3);
		}
		if (!report.IsRendered && !report.IsDocument && !report.ContainsOnlyDashboard)
		{
			try
			{
				report.Render(showProgress: false);
			}
			catch
			{
			}
		}
		if (!report.ContainsDashboard)
		{
			string text = ((!string.IsNullOrEmpty(requestParams.Cache.DrillDownGuid)) ? GetReportCacheGuid(requestParams, requestParams.Cache.DrillDownGuid) : GetReportCacheGuid(requestParams));
			report.SaveInteractionParametersToDocument = true;
			SaveReport(report, text, CacheMode, Timeout, Priority);
			report.SaveInteractionParametersToDocument = false;
			UpdateCacheGuids(requestParams, text, report.Key);
		}
	}

	internal void SavePreviewReportInternal(StiRequestParams requestParams, StiReport report, string guid)
	{
		guid = GetReportCacheGuid(requestParams, null, guid);
		ApplyRequestParameters(requestParams);
		RemoveReport(guid);
		string guid2 = guid + "_template";
		SaveReport(report, guid2);
		if (report.IsRendered && !report.ContainsDashboard)
		{
			guid2 = guid;
			report.SaveInteractionParametersToDocument = true;
			SaveReport(report, guid2);
			report.SaveInteractionParametersToDocument = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void RemoveReportInternal(StiRequestParams requestParams)
	{
		ApplyRequestParameters(requestParams);
		string reportCacheGuid = GetReportCacheGuid(requestParams);
		RemoveReport(reportCacheGuid, CacheMode);
	}

	internal string GetResourceInternal(StiRequestParams requestParams, string guid)
	{
		ApplyRequestParameters(requestParams);
		return GetResource(guid);
	}

	internal void SaveResourceInternal(StiRequestParams requestParams, string text, string guid)
	{
		ApplyRequestParameters(requestParams);
		SaveResource(text, guid);
	}

	internal void ApplyRequestParameters(StiRequestParams requestParams)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		HttpContext = requestParams.HttpContext;
		CacheMode = requestParams.Cache.Mode;
		Timeout = requestParams.Cache.Timeout;
		Priority = requestParams.Cache.Priority;
	}

	private string GetReportCacheGuid(StiRequestParams requestParams)
	{
		return GetReportCacheGuid(requestParams, null, null);
	}

	private string GetReportCacheGuid(StiRequestParams requestParams, string typeGuid)
	{
		return GetReportCacheGuid(requestParams, typeGuid, null);
	}

	private string GetReportCacheGuid(StiRequestParams requestParams, string typeGuid, string baseGuid)
	{
		string text = (string.IsNullOrEmpty(baseGuid) ? (requestParams.Id + "_" + requestParams.Cache.ClientGuid) : baseGuid);
		if (requestParams.Cache.UseLocalizedCache)
		{
			text = text + "_" + StiMD5Helper.ComputeHash(requestParams.Localization).Substring(0, 8);
		}
		if (!string.IsNullOrEmpty(typeGuid))
		{
			text = text + "_" + typeGuid;
		}
		return text;
	}

	public static object GetObjectFromCacheData(byte[] fileData)
	{
		object obj = DataSerializer.Deserialize<object>(fileData);
		if (obj is byte[])
		{
			byte[] bytes = obj as byte[];
			StiReportContentType reportContentType = GetReportContentType(bytes);
			if (reportContentType != StiReportContentType.Undefined)
			{
				StiReport stiReport = new StiReport();
				switch (reportContentType)
				{
				case StiReportContentType.XmlTemplate:
				case StiReportContentType.JsonTemplate:
					stiReport.Info.Zoom = 1.0;
					stiReport.Info.ForceDesigningMode = true;
					stiReport.Load(bytes);
					break;
				case StiReportContentType.XmlDocument:
				case StiReportContentType.JsonDocument:
					stiReport.LoadDocument(bytes);
					break;
				}
				return stiReport;
			}
		}
		if (obj is string && obj.ToString().Trim().IndexOf("{\r\n  \"Ident\": \"StiDataTransformation\"") == 0)
		{
			StiDataTransformation stiDataTransformation = new StiDataTransformation();
			stiDataTransformation.LoadFromJsonObject(JsonConvert.DeserializeObject<JObject>(obj.ToString()));
			return stiDataTransformation;
		}
		if (obj is string)
		{
			return obj;
		}
		if (obj is JArray)
		{
			ArrayList arrayList = new ArrayList();
			JArray jArray = (JArray)obj;
			List<double?[]> list = new List<double?[]>();
			foreach (JToken childrenToken in jArray.ChildrenTokens)
			{
				if (childrenToken is JValue)
				{
					arrayList.Add((childrenToken as JValue).Value);
				}
				else if (childrenToken is JObject)
				{
					JObject jObject = childrenToken as JObject;
					Hashtable hashtable = new Hashtable();
					foreach (JProperty item in jObject.Properties())
					{
						hashtable[item.Name] = (item.Value as JValue)?.Value;
					}
					arrayList.Add(hashtable);
				}
				else
				{
					if (!(childrenToken is JArray))
					{
						continue;
					}
					ArrayList arrayList2 = new ArrayList();
					JArray jArray2 = (JArray)childrenToken;
					foreach (JToken childrenToken2 in jArray2.ChildrenTokens)
					{
						if (childrenToken2 is JValue)
						{
							arrayList2.Add((childrenToken2 as JValue).Value);
						}
					}
					list.Add(arrayList2.ToArray(typeof(double?)) as double?[]);
				}
			}
			if (list.Count > 0 && arrayList.Count == 0)
			{
				return list;
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				if (arrayList[i] == null)
				{
					continue;
				}
				if (arrayList[i] is string)
				{
					string text = (string)arrayList[i];
					if (string.IsNullOrEmpty(text))
					{
						continue;
					}
					if (text.IsBase64String())
					{
						byte[] bytes2 = Convert.FromBase64String(text);
						switch (GetReportContentType(bytes2))
						{
						case StiReportContentType.XmlTemplate:
						case StiReportContentType.JsonTemplate:
						{
							StiReport stiReport3 = new StiReport();
							stiReport3.Info.ForceDesigningMode = true;
							stiReport3.Info.Zoom = 1.0;
							stiReport3.Load(bytes2);
							arrayList[i] = stiReport3;
							break;
						}
						case StiReportContentType.XmlDocument:
						case StiReportContentType.JsonDocument:
						{
							StiReport stiReport2 = new StiReport();
							stiReport2.LoadDocument(bytes2);
							arrayList[i] = stiReport2;
							break;
						}
						}
					}
					else
					{
						if (text.StartsWith("<?xml"))
						{
							continue;
						}
						string text2 = text.Substring(0, text.IndexOf(";"));
						string s = text.Substring(text.IndexOf(";") + 1);
						Assembly assembly = typeof(StiReport).Assembly;
						StiComponent stiComponent = assembly.CreateInstance("Stimulsoft.Report.Components." + text2) as StiComponent;
						if (stiComponent == null)
						{
							stiComponent = assembly.CreateInstance("Stimulsoft.Report.Chart." + text2) as StiComponent;
						}
						else if (stiComponent == null)
						{
							stiComponent = assembly.CreateInstance("Stimulsoft.Report.CrossTab." + text2) as StiComponent;
						}
						if (stiComponent != null)
						{
							using StringReader stringReader = new StringReader(s);
							StiSerializing stiSerializing = new StiSerializing(new StiReportObjectStringConverter());
							stiSerializing.Deserialize(stiComponent, stringReader, "StiComponent");
							stringReader.Close();
						}
						arrayList[i] = stiComponent;
					}
				}
				else if (arrayList[i] is Hashtable)
				{
					Hashtable hashtable2 = arrayList[i] as Hashtable;
					StiReport stiReport4 = new StiReport();
					stiReport4.Info.ForceDesigningMode = true;
					stiReport4.Info.Zoom = 1.0;
					stiReport4.Load(Convert.FromBase64String(hashtable2["report"] as string));
					StiDesignerCommand command = (StiDesignerCommand)Enum.Parse(typeof(StiDesignerCommand), hashtable2["command"] as string);
					StiReportContainer value = new StiReportContainer(stiReport4, (bool)hashtable2["resourcesIncluded"], command);
					arrayList[i] = value;
				}
			}
			return arrayList;
		}
		return null;
	}

	public static byte[] GetCacheDataFromObject(object obj)
	{
		byte[] result = null;
		if (obj is StiReport)
		{
			StiReport stiReport = (StiReport)obj;
			result = DataSerializer.Serialize((stiReport.IsDocument && !stiReport.ContainsDashboard && !stiReport.Info.ForceDesigningMode) ? stiReport.SaveDocumentToByteArray() : stiReport.SaveToByteArray());
		}
		else if (obj is string)
		{
			result = DataSerializer.Serialize((string)obj);
		}
		else if (obj is List<double?[]>)
		{
			ArrayList array = new ArrayList();
			((List<double?[]>)obj).ForEach(delegate(double?[] a)
			{
				array.Add(a);
			});
			result = DataSerializer.Serialize(array);
		}
		else if (obj is StiDataTransformation)
		{
			result = DataSerializer.Serialize(((StiDataTransformation)obj).SaveToJsonObject(StiJsonSaveMode.Report).ToString());
		}
		else if (obj is ArrayList)
		{
			ArrayList arrayList = (ArrayList)obj;
			for (int i = 0; i < arrayList.Count; i++)
			{
				if (arrayList[i] == null)
				{
					continue;
				}
				if (arrayList[i] is StiReport)
				{
					arrayList[i] = ((StiReport)arrayList[i]).SaveToByteArray();
				}
				else if (arrayList[i] is StiComponent)
				{
					StiSerializing stiSerializing = new StiSerializing(new StiReportObjectStringConverter());
					StringBuilder stringBuilder = new StringBuilder();
					using StringWriter stringWriter = new StringWriter(stringBuilder);
					stiSerializing.Serialize(arrayList[i], stringWriter, "StiComponent", StiSerializeTypes.SerializeToAll);
					stringWriter.Close();
					arrayList[i] = arrayList[i].GetType().Name + ";" + stringBuilder.ToString();
				}
				else if (arrayList[i] is StiReportContainer)
				{
					Hashtable hashtable = new Hashtable();
					hashtable["command"] = (arrayList[i] as StiReportContainer).command;
					hashtable["resourcesIncluded"] = (arrayList[i] as StiReportContainer).resourcesIncluded;
					hashtable["report"] = (arrayList[i] as StiReportContainer).report.SaveToByteArray();
					arrayList[i] = hashtable;
				}
			}
			result = DataSerializer.Serialize(arrayList);
		}
		return result;
	}

	public static StiReportContentType GetReportContentType(byte[] bytes)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream(bytes);
			if (StiReport.IsJsonFile(memoryStream))
			{
				JsonSerializer jsonSerializer = new JsonSerializer();
				using StreamReader reader = new StreamReader(memoryStream);
				using JsonTextReader reader2 = new JsonTextReader(reader);
				JObject jObject = jsonSerializer.Deserialize(reader2) as JObject;
				foreach (JProperty item in jObject.Properties())
				{
					if (item.Name == "RenderedPages")
					{
						return StiReportContentType.JsonDocument;
					}
					if (item.Name == "Pages")
					{
						return StiReportContentType.JsonTemplate;
					}
				}
			}
			else
			{
				XmlTextReader xmlTextReader = new XmlTextReader(memoryStream);
				xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
				xmlTextReader.Read();
				xmlTextReader.Read();
				if (xmlTextReader.IsStartElement() && xmlTextReader.Name == "StiSerializer")
				{
					string attribute = xmlTextReader.GetAttribute("application");
					if (attribute == "StiDocument")
					{
						return StiReportContentType.XmlDocument;
					}
					if (attribute == "StiReport")
					{
						return StiReportContentType.XmlTemplate;
					}
				}
			}
		}
		catch
		{
		}
		return StiReportContentType.Undefined;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual object GetObject(string guid, StiServerCacheMode cacheMode, TimeSpan timeout, CacheItemPriority priority)
	{
		return GetObject(guid);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void SaveObject(object obj, string guid, StiServerCacheMode cacheMode, TimeSpan timeout, CacheItemPriority priority)
	{
		SaveObject(obj, guid);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void RemoveObject(string guid, StiServerCacheMode cacheMode)
	{
		RemoveObject(guid);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual StiReport GetReport(string guid, StiServerCacheMode cacheMode, TimeSpan timeout, CacheItemPriority priority)
	{
		return GetReport(guid);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void SaveReport(StiReport report, string guid, StiServerCacheMode cacheMode, TimeSpan timeout, CacheItemPriority priority)
	{
		SaveReport(report, guid);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void RemoveReport(string guid, StiServerCacheMode cacheMode)
	{
		RemoveReport(guid);
	}

	[Obsolete("This method is obsolete. It will be removed in next versions. Please use the GetObject() method instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual object GetObjectFromCache(string guid)
	{
		return null;
	}

	[Obsolete("This method is obsolete. It will be removed in next versions. Please use the SaveObject() method instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void SaveObjectToCache(object obj, string guid)
	{
	}
}
