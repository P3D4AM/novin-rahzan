using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web;
using System.Web.Caching;
using Stimulsoft.Base;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Web;

public class StiRequestParams
{
	public class StiCacheParams
	{
		internal StiCacheHelper Helper { get; set; }

		public string ClientGuid { get; set; }

		public string DrillDownGuid { get; set; }

		public string DashboardDrillDownGuid { get; set; }

		public StiServerCacheMode Mode { get; set; } = StiServerCacheMode.ObjectCache;


		public TimeSpan Timeout { get; set; } = new TimeSpan(0, 20, 0);


		public CacheItemPriority Priority { get; set; } = (CacheItemPriority)3;


		internal bool UseLocalizedCache { get; set; }

		internal bool UseCacheHelper { get; set; }
	}

	public class StiInteractionParams
	{
		public Hashtable Variables { get; set; } = new Hashtable();


		public Hashtable Sorting { get; set; } = new Hashtable();


		public Hashtable Collapsing { get; set; } = new Hashtable();


		public ArrayList DrillDown { get; set; } = new ArrayList();


		public Hashtable Editable { get; set; } = new Hashtable();


		public Hashtable DashboardFiltering { get; set; } = new Hashtable();


		public Hashtable DashboardSorting { get; set; } = new Hashtable();


		public Hashtable DashboardElementDrillDownParameters { get; set; } = new Hashtable();


		public bool DrillDownReportFile { get; set; }
	}

	public class StiViewerParams
	{
		public int PageNumber { get; set; }

		public int OriginalPageNumber { get; set; }

		public double Zoom { get; set; } = 1.0;


		public StiWebViewMode ViewMode { get; set; }

		public bool ShowBookmarks { get; set; } = true;


		public string OpenLinksWindow { get; set; } = "_blank";


		public StiChartRenderType ChartRenderType { get; set; } = StiChartRenderType.AnimatedVector;


		public StiReportDisplayMode ReportDisplayMode { get; set; }

		public StiPrintAction PrintAction { get; set; }

		public bool BookmarksPrint { get; set; } = true;


		public string OpeningFileName { get; set; }

		public string OpeningFilePassword { get; set; }

		internal bool ReportDesignerMode { get; set; }

		internal int DashboardWidth { get; set; }

		internal int DashboardHeight { get; set; }

		public StiReportType ReportType { get; set; }

		internal StiImagesQuality ImagesQuality { get; set; } = StiImagesQuality.Normal;


		internal string ElementName { get; set; }

		internal bool ParametersPanelSortDataItems { get; set; }

		internal bool CombineReportPages { get; set; }
	}

	public class StiDesignerParams
	{
		public StiDesignerCommand Command { get; set; }

		public bool IsAutoSave { get; set; }

		public bool IsNewReport { get; set; }

		public bool IsSaveAs { get; set; }

		public string FileName { get; set; }

		public string Password { get; set; }

		public string SaveType { get; set; }

		internal int UndoMaxLevel { get; set; } = 6;


		internal bool CheckReportBeforePreview { get; set; } = true;


		internal string CurrentCultureName { get; set; }

		internal StiNewReportDictionary NewReportDictionary { get; set; } = StiNewReportDictionary.DictionaryNew;

	}

	public class StiDictionaryParams
	{
		public string ConnectionType { get; set; }

		public string ConnectionString { get; set; }

		public string Query { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public string DataPath { get; set; }

		public string SchemaPath { get; set; }
	}

	public class StiServerParams
	{
		public bool UseRelativeUrls { get; set; } = true;


		public bool UseCompression { get; set; }

		public bool PassQueryParametersForResources { get; set; } = true;


		public bool PassQueryParametersToReport { get; set; }

		public bool AllowAutoUpdateCookies { get; set; }
	}

	internal class StiScriptsParams
	{
		public bool IncludeDashboards
		{
			get
			{
				if (StiDashboardAssembly.IsAssemblyLoaded && StiDashboardExportAssembly.IsAssemblyLoaded)
				{
					return StiDashboardDrawingAssembly.IsAssemblyLoaded;
				}
				return false;
			}
		}

		public bool IncludeClient { get; set; }
	}

	private HttpContext httpContext;

	public Hashtable All { get; } = new Hashtable();


	public HttpContext HttpContext
	{
		get
		{
			return httpContext ?? (httpContext = HttpContext.Current);
		}
		set
		{
			httpContext = value;
		}
	}

	public string HttpMethod
	{
		get
		{
			HttpContext obj = HttpContext;
			if (obj == null)
			{
				return null;
			}
			return obj.Request.HttpMethod;
		}
	}

	[Obsolete("This property is obsolete. It will be removed in next versions. Please use the ComponentType property instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiComponentType Component => ComponentType;

	public StiComponentType ComponentType { get; set; }

	public StiAction Action { get; set; }

	public string Resource { get; set; }

	public byte[] Data { get; set; }

	public StiReport Report { get; set; }

	public string Id { get; set; }

	public bool HasParameters { get; set; }

	public StiCacheParams Cache { get; set; } = new StiCacheParams();


	public StiInteractionParams Interaction { get; set; } = new StiInteractionParams();


	public NameValueCollection Routes { get; set; } = new NameValueCollection();


	public NameValueCollection FormValues { get; set; } = new NameValueCollection();


	public Hashtable UserValues { get; set; }

	public Hashtable ExportSettings { get; set; } = new Hashtable();


	public StiExportFormat ExportFormat { get; set; }

	public string Localization { get; set; } = "default";


	public string Theme { get; set; } = string.Empty;


	public string Version { get; set; } = string.Empty;


	public StiViewerParams Viewer { get; set; } = new StiViewerParams();


	public StiDesignerParams Designer { get; set; } = new StiDesignerParams();


	public StiDictionaryParams Dictionary { get; set; } = new StiDictionaryParams();


	public StiServerParams Server { get; set; } = new StiServerParams();


	internal StiScriptsParams Scripts { get; set; } = new StiScriptsParams();


	internal bool CloudMode { get; set; }

	internal bool ServerMode { get; set; }

	internal string CloudPlanIdent { get; set; }

	public Hashtable ReportResourceParams { get; set; } = new Hashtable();


	public bool Contains(string name)
	{
		return All.ContainsKey(name);
	}

	public string GetString(string name)
	{
		if (All == null || !Contains(name) || All[name] == null)
		{
			return null;
		}
		return All[name].ToString();
	}

	public bool GetBoolean(string name)
	{
		return Convert.ToBoolean(GetString(name));
	}

	public int GetInt(string name)
	{
		return (int)StiObjectConverter.ConvertToInt64(GetString(name));
	}

	public double GetDouble(string name)
	{
		return StiObjectConverter.ConvertToDouble(GetString(name));
	}

	public Hashtable GetHashtable(string name)
	{
		if (!All.Contains(name))
		{
			return null;
		}
		return (Hashtable)All[name];
	}

	public ArrayList GetArray(string name)
	{
		if (!All.Contains(name))
		{
			return null;
		}
		return (ArrayList)All[name];
	}

	public object GetEnum(string name, Type type)
	{
		string @string = GetString(name);
		if (@string == null || @string.Length == 0)
		{
			return 0;
		}
		try
		{
			return Enum.Parse(type, @string);
		}
		catch
		{
			try
			{
				return Enum.Parse(type, @string.Substring(0, 1).ToUpper() + @string.Substring(1));
			}
			catch
			{
				return 0;
			}
		}
	}

	public NameValueCollection GetNameValueCollection(string name)
	{
		if (!All.Contains(name) || All[name] == null || !(All[name] is Hashtable))
		{
			return null;
		}
		Hashtable hashtable = (Hashtable)All[name];
		NameValueCollection nameValueCollection = new NameValueCollection();
		foreach (object key in hashtable.Keys)
		{
			nameValueCollection.Add(key.ToString(), (hashtable[key] != null) ? hashtable[key].ToString() : null);
		}
		return nameValueCollection;
	}
}
