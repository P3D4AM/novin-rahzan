using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Print;
using Stimulsoft.Report.Print.Design;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Units;
using Stimulsoft.Report.Viewer;
using Stimulsoft.Report.Viewer.Design;

namespace Stimulsoft.Report;

[Designer("Stimulsoft.Report.Design.StiReportDesigner, Stimulsoft.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[ToolboxBitmap(typeof(StiReport), "Toolbox.StiReport.bmp")]
[Guid("5A067BAF-802D-4cc8-AAD3-5615641F7050")]
[StiServiceBitmap(typeof(StiReport), "Stimulsoft.Report.Images.Components.StiReport.png")]
public class StiReport : Component, IStiReport, IStiApp, IStiAppCell, IStiSelect, ICloneable, IStiStateSaveRestore, IStiUnitConvert, IStiEngineVersionProperty, IStiPropertyGridObject, IStiGetFonts
{
	private bool isSelected;

	private List<StiBusinessObjectData> businessObjectsStore;

	private string script;

	private static StiCSharpLanguage CSharpLanguage;

	private static StiVBLanguage VBLanguage;

	private string masterReport = string.Empty;

	private RichTextBox cachedRichText;

	private RichTextBox cachedRichTextFormat;

	private StiReportResourceCollection reportResources;

	private string statusString = "";

	private int progressOfRendering;

	private string reportSource;

	private string reportCachePath = "";

	private StiPagesCollection renderedPages;

	internal StiBookmark pointerValue;

	internal StiBookmark bookmarkValue;

	private StiBookmark manualBookmark = new StiBookmark();

	private Graphics reportMeasureGraphics;

	private Hashtable totals = new Hashtable();

	private StiCells cells;

	private bool isModified;

	private bool isStopped;

	private StiEngineVersion engineVersion = (StiEngineVersion)2;

	private StiReportUnitType reportUnit;

	private int stopBeforePage;

	private int collate = 1;

	private StiReportLanguageType scriptLanguage;

	private int refreshTime;

	private static object lockGlobalMeasureGraphics;

	private static Graphics globalMeasureGraphics;

	private static object objLockGlobalRichTextMeasureGraphics;

	private static Graphics globalRichTextMeasureGraphics;

	private Hashtable resourceToFontHash = new Hashtable();

	internal StiReportJsonLoaderHelper jsonLoaderHelper;

	private int pageNumber;

	internal int totalPageCountValue;

	private int groupLine = 1;

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool NeedsCompiling { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public CompilerResults CompilerResults { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiReport CompiledReport { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiReport;

	[Browsable(false)]
	[StiBrowsable(false)]
	public string PropName { get; private set; } = string.Empty;


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiStatesManager States { get; } = new StiStatesManager();


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int SelectionTick { get; private set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsSelected
	{
		get
		{
			return isSelected;
		}
		set
		{
			if (!isSelected)
			{
				SelectionTick = Environment.TickCount;
			}
			isSelected = value;
		}
	}

	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<StiBusinessObjectData> BusinessObjectsStore => businessObjectsStore ?? (businessObjectsStore = new List<StiBusinessObjectData>());

	[Editor("Stimulsoft.Report.Design.StiReportDataSourcesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[StiBrowsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Data")]
	public StiReportDataSourceCollection ReportDataSources { get; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiBrowsable(false)]
	[Browsable(false)]
	public Hashtable Variables { get; internal set; }

	public object this[string name]
	{
		get
		{
			StiReport stiReport = CompiledReport ?? this;
			FieldInfo field = stiReport.GetType().GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (field != null)
			{
				try
				{
					return field.GetValue(stiReport);
				}
				catch
				{
				}
			}
			PropertyInfo property = stiReport.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (property != null)
			{
				try
				{
					return property.GetValue(stiReport, new object[0]);
				}
				catch
				{
				}
			}
			if (stiReport.Variables == null)
			{
				return null;
			}
			return stiReport.Variables[name];
		}
		set
		{
			StiReport stiReport = CompiledReport ?? this;
			if (stiReport.Dictionary != null && stiReport.Dictionary.Variables.Contains(name))
			{
				stiReport.ModifiedVariables[name] = null;
			}
			FieldInfo field = stiReport.GetType().GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (field != null)
			{
				try
				{
					field.SetValue(stiReport, value);
					return;
				}
				catch
				{
				}
			}
			PropertyInfo property = stiReport.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (property != null && property.CanWrite)
			{
				try
				{
					property.SetValue(stiReport, value, new object[0]);
					return;
				}
				catch
				{
				}
			}
			if (stiReport.Variables == null)
			{
				StiParser.PrepareReportVariables(stiReport);
			}
			stiReport.Variables[name] = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object[] AggregateFunctions { get; set; }

	[TypeConverter(typeof(ExpandableObjectConverter))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public StiDictionary Dictionary { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDataSourcesCollection DataSources => Dictionary.DataSources;

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDataCollection DataStore => Dictionary.DataStore;

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public string Script
	{
		get
		{
			if (script == null)
			{
				ScriptNew();
			}
			return script;
		}
		set
		{
			script = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiLanguage Language
	{
		get
		{
			if (ScriptLanguage == StiReportLanguageType.VB)
			{
				return VBLanguage ?? (VBLanguage = new StiVBLanguage());
			}
			return CSharpLanguage ?? (CSharpLanguage = new StiCSharpLanguage());
		}
		set
		{
			ScriptLanguage = ((value is StiVBLanguage) ? StiReportLanguageType.VB : StiReportLanguageType.CSharp);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Occurs when the report rendering starts.")]
	[Category("RenderEvents")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBeginRenderEvent BeginRenderEvent { get; set; } = new StiBeginRenderEvent();


	[Description("Occurs when a page is rendered.")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("RenderEvents")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiRenderingEvent RenderingEvent { get; set; } = new StiRenderingEvent();


	[StiSerializable]
	[Description("Occurs when the report rendering is finished.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("RenderEvents")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiEndRenderEvent EndRenderEvent { get; set; } = new StiEndRenderEvent();


	[StiSerializable]
	[Description("Occurs when report starts exporting.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("ExportEvents")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiExportingEvent ExportingEvent { get; set; } = new StiExportingEvent();


	[StiSerializable]
	[Description("Occurs when report ends exporting.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("ExportEvents")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiExportedEvent ExportedEvent { get; set; } = new StiExportedEvent();


	[StiSerializable]
	[Description("Occurs when report starts printing.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("PrintEvents")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPrintingEvent PrintingEvent { get; set; } = new StiPrintingEvent();


	[StiSerializable]
	[Description("Occurs when report ends printing.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("PrintEvents")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPrintedEvent PrintedEvent { get; set; } = new StiPrintedEvent();


	[StiSerializable]
	[Description("Occurs when report starts refreshing.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("RenderEvents")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiRefreshingEvent RefreshingEvent { get; set; } = new StiRefreshingEvent();


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiNonSerialized]
	public StiUnit Unit
	{
		get
		{
			return StiUnit.GetUnitFromReportUnit(ReportUnit);
		}
		set
		{
			if (value is StiCentimetersUnit)
			{
				ReportUnit = StiReportUnitType.Centimeters;
			}
			else if (value is StiMillimetersUnit)
			{
				ReportUnit = StiReportUnitType.Millimeters;
			}
			else if (value is StiInchesUnit)
			{
				ReportUnit = StiReportUnitType.Inches;
			}
			else if (value is StiHundredthsOfInchUnit)
			{
				ReportUnit = StiReportUnitType.HundredthsOfInch;
			}
		}
	}

	internal AppDomain ReportDomain { get; set; }

	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string MasterReport
	{
		get
		{
			return masterReport;
		}
		set
		{
			if (masterReport != value)
			{
				masterReport = value;
				UpdateInheritedReport();
			}
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	public new IContainer Container => base.Container;

	[Browsable(false)]
	[StiBrowsable(false)]
	public override ISite Site
	{
		get
		{
			return base.Site;
		}
		set
		{
			base.Site = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual bool UseProgressInThread { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual StiMetaTagCollection MetaTags { get; } = new StiMetaTagCollection();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual StiReportResourceCollection ReportResources => reportResources ?? (reportResources = new StiReportResourceCollection());

	[StiSerializable]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string ReportVersion { get; set; } = string.Empty;


	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public StiEngine Engine { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public StiEngineV1 EngineV1 { get; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public StringCollection ReportRenderingMessages { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public Hashtable InteractionCollapsingStates { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public string StatusString
	{
		get
		{
			return statusString;
		}
		set
		{
			if (!(statusString == value))
			{
				statusString = value;
				if (CompiledReport != null)
				{
					CompiledReport.StatusString = value;
				}
				if (ParentReport != null)
				{
					ParentReport.StatusString = value;
				}
				if (Progress != null)
				{
					Progress.Update(StatusString);
				}
				InvokeStatusChanged();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public int ProgressOfRendering
	{
		get
		{
			return progressOfRendering;
		}
		set
		{
			if (progressOfRendering != value)
			{
				progressOfRendering = value;
				if (CompiledReport != null)
				{
					CompiledReport.ProgressOfRendering = value;
				}
				if (ParentReport != null)
				{
					ParentReport.ProgressOfRendering = value;
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public IStiProgressInformation Progress { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public StiReportsCollection SubReports { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	public string ReportSource
	{
		get
		{
			return reportSource;
		}
		set
		{
			reportSource = value;
			if (!string.IsNullOrEmpty(value) && !base.DesignMode)
			{
				if (value.Length > 2 && value[0] == '<' && value[1] == '?')
				{
					LoadFromString(value);
				}
				else
				{
					LoadPackedReportFromString(value);
				}
			}
		}
	}

	[StiBrowsable(false)]
	[Browsable(false)]
	[StiSerializable]
	public string Key { get; set; }

	[StiBrowsable(false)]
	[Browsable(false)]
	[StiSerializable]
	public string ReportGuid { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object Tag { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ReportCachePath
	{
		get
		{
			if (CompiledReport != null)
			{
				return CompiledReport.ReportCachePath;
			}
			return reportCachePath;
		}
		set
		{
			if (CompiledReport == null)
			{
				reportCachePath = value;
			}
			else
			{
				CompiledReport.ReportCachePath = value;
			}
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ImageCachePath { get; set; } = "";


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string RtfCachePath { get; set; } = "";


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiReport ParentReport { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPagesCollection Pages { get; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPagesCollection RenderedPages
	{
		get
		{
			if (CompiledReport == null)
			{
				return renderedPages;
			}
			return CompiledReport.RenderedPages;
		}
		set
		{
			if (CompiledReport != null)
			{
				CompiledReport.renderedPages = value;
			}
			else
			{
				renderedPages = value;
			}
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDesignerInfo Info { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IStiDesignerBase Designer { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBookmark Pointer
	{
		get
		{
			if (CompiledReport == null)
			{
				return pointerValue;
			}
			return CompiledReport.Pointer;
		}
		set
		{
			pointerValue = value;
			if (CompiledReport != null)
			{
				CompiledReport.Pointer = value;
			}
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBookmark Bookmark
	{
		get
		{
			if (CompiledReport == null)
			{
				return bookmarkValue;
			}
			return CompiledReport.Bookmark;
		}
		set
		{
			bookmarkValue = value;
			if (CompiledReport != null)
			{
				CompiledReport.Bookmark = value;
			}
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBookmark ManualBookmark
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion != 1)
			{
				return Bookmark;
			}
			return manualBookmark;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion == 1)
			{
				manualBookmark = value;
			}
			else
			{
				Bookmark = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public Graphics ReportMeasureGraphics
	{
		get
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			if (reportMeasureGraphics == null)
			{
				if (StiOptions.Engine.OldWYSIWYG)
				{
					reportMeasureGraphics = Graphics.FromImage((Image)new Bitmap(1, 1));
					reportMeasureGraphics.PageUnit = (GraphicsUnit)4;
					reportMeasureGraphics.PageScale = 0.01f;
				}
				else
				{
					reportMeasureGraphics = Graphics.FromImage((Image)new Bitmap(1, 1));
					reportMeasureGraphics.PageUnit = (GraphicsUnit)2;
					reportMeasureGraphics.PageScale = 1f;
				}
			}
			return reportMeasureGraphics;
		}
		private set
		{
			reportMeasureGraphics = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	internal Hashtable Totals
	{
		get
		{
			if (ParentReport == null)
			{
				return totals;
			}
			return ParentReport.totals;
		}
		set
		{
			if (ParentReport != null)
			{
				ParentReport.totals = value;
			}
			else
			{
				totals = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	protected internal StiCells Cells => cells ?? (cells = new StiCells());

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string Password { get; private set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiBrowsable(false)]
	[Browsable(false)]
	public string[] DataBandsUsedInPageTotals { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiBrowsable(false)]
	[Browsable(false)]
	public string[] ListOfUsedData { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	public StiRenderedWith RenderedWith { get; internal set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	public CookieContainer CookieContainer { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	public NameValueCollection HttpHeadersContainer { get; set; }

	internal StiReport SubReportsMasterReport { get; set; }

	internal bool SubReportsResetPageNumber { get; set; }

	internal bool SubReportsPrintOnPreviousPage { get; set; }

	internal List<StiRichText> RichTextImageCache { get; set; }

	internal int IndexName { get; set; } = 1;


	internal bool ContainsTables { get; set; }

	internal Hashtable HashViewWpfPainter { get; set; }

	internal Hashtable CachedTotals { get; set; }

	internal bool CachedTotalsLocked { get; set; }

	internal bool FlagFontSmoothing { get; set; }

	internal AppDomain WpfRichTextDomain { get; set; }

	internal Hashtable ModifiedVariables { get; } = new Hashtable();


	internal Hashtable Anchors { get; set; } = new Hashtable();


	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal StiReportPass ReportPass { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use property 'IsEditedInViewer' instead property 'IsEditedInPreview'.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool IsEditedInPreview
	{
		get
		{
			return IsEditedInViewer;
		}
		set
		{
			IsEditedInViewer = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsEditedInViewer { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsRendered { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsRendering { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsModified
	{
		get
		{
			return isModified;
		}
		set
		{
			if (isModified != value)
			{
				isModified = value;
				string reportGuid = GenerateReportGuid();
				ReportGuid = reportGuid;
			}
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsStopped
	{
		get
		{
			if (CompiledReport == null)
			{
				return isStopped;
			}
			return CompiledReport.IsStopped;
		}
		set
		{
			if (CompiledReport != null)
			{
				CompiledReport.IsStopped = value;
			}
			isStopped = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("Please use property IsStopped' instead 'Stop' property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool Stop
	{
		get
		{
			return IsStopped;
		}
		set
		{
			IsStopped = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsExporting { get; internal set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsPageDesigner { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsSerializing { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsPrinting { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ContainsDashboard => Pages.ToList().Any((StiPage p) => p.IsDashboard && p.Enabled);

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ContainsOnlyDashboard => !Pages.ToList().Any((StiPage p) => !p.IsDashboard && p.Enabled);

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDesigning
	{
		get
		{
			if (Designer == null && !Info.ForceDesigningMode)
			{
				return IsPageDesigner;
			}
			return true;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsCompiled => CompiledReport != null;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsReportEmpty
	{
		get
		{
			if (Pages.Count == 1)
			{
				return Pages[0].Components.Count == 0;
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsRenderedReportEmpty
	{
		get
		{
			if (RenderedPages.Count == 1)
			{
				return RenderedPages[0].Components.Count == 0;
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsPackedReport { get; private set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsJsonReport { get; private set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsWpf { get; internal set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use 'Report.IsReportRenderingAfterSubmit' property instead 'Report.IsPreviewDialogs' property.")]
	public bool IsPreviewDialogs
	{
		get
		{
			return IsReportRenderingAfterSubmit;
		}
		set
		{
			IsReportRenderingAfterSubmit = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsReportRenderingAfterSubmit { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("The property 'IsSLPreviewDialogs' is not used more.")]
	public bool IsSLPreviewDialogs { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsDocument { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public bool IsInteractionRendering { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Design")]
	[ParenthesizePropertyName(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Gets or sets a report name.")]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string ReportName
	{
		get
		{
			return PropName;
		}
		set
		{
			if (PropName == ReportAlias)
			{
				ReportAlias = value;
			}
			PropName = (StiOptions.Designer.AutoCorrectReportName ? StiNameValidator.CorrectName(value, this) : value);
			StiOptions.Engine.GlobalEvents.InvokeReportNameChanged(this, EventArgs.Empty);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Design")]
	[ParenthesizePropertyName(true)]
	[Description("Gets or sets a report alias.")]
	[StiOrder(110)]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string ReportAlias { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Design")]
	[ParenthesizePropertyName(true)]
	[Description("Gets or sets a report author.")]
	[StiOrder(120)]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string ReportAuthor { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Design")]
	[ParenthesizePropertyName(true)]
	[Description("Gets or sets a report description.")]
	[StiOrder(130)]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string ReportDescription { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Design")]
	[ParenthesizePropertyName(true)]
	[Description("Gets or sets a report image.")]
	[StiOrder(150)]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiSimpeImageConverter))]
	public Image ReportImage { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Design")]
	[ParenthesizePropertyName(true)]
	[Description("Gets or sets a report icon.")]
	[StiOrder(140)]
	[Editor("Stimulsoft.Report.Components.Design.StiReportIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiReportIconConverter))]
	public byte[] ReportIcon { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets the date of the report creation.")]
	public DateTime ReportCreated { get; set; } = DateTime.Now;


	[StiSerializable]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets the date of the last report changes.")]
	public DateTime ReportChanged { get; set; } = DateTime.Now;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[StiOrder(240)]
	[Description("Gets or sets version of engine which will be used for report rendering. EngineV1 is old version of engine. EngineV2 is newest version of engine. Its more powerful and more flexible.")]
	[Editor("Stimulsoft.Report.Design.StiEngineVersionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiEngineVersion EngineVersion
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return engineVersion;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (engineVersion != value)
			{
				if ((int)value == 0)
				{
					value = (StiEngineVersion)1;
				}
				engineVersion = value;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Engine")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiStylesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiStylesConverter))]
	[Description("Gets a collection which consists of report styles.")]
	[Browsable(false)]
	[StiOrder(350)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiStylesCollection Styles { get; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[DefaultValue(StiNumberOfPass.SinglePass)]
	[StiOrder(260)]
	[Description("Gets or sets the number of passes which the report generator makes while report rendering.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiNumberOfPass NumberOfPass { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[DefaultValue(StiCalculationMode.Compilation)]
	[StiOrder(215)]
	[Description("Gets or sets the method of calculation in report.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[Editor("Stimulsoft.Report.Design.StiCalculationModeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiCalculationMode CalculationMode { get; set; }

	[Browsable(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets the current measurement unit of the report.")]
	[StiOrder(320)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiReportUnitType ReportUnit
	{
		get
		{
			return reportUnit;
		}
		set
		{
			if (!IsSerializing && reportUnit != value)
			{
				Convert(StiUnit.GetUnitFromReportUnit(reportUnit), StiUnit.GetUnitFromReportUnit(value));
			}
			reportUnit = value;
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Data")]
	[Description("Gets or sets value, which indicates whether it is necessary to cache all data of the report in one DataSet or not.")]
	[StiOrder(210)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool CacheAllData { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Data")]
	[Description("Gets or sets value, which indicates how report engine retrieve data - only used in the report data or all data.")]
	[StiOrder(322)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool RetrieveOnlyUsedData { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(StiReportCacheMode.Off)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[Description("Gets or sets value, which indicates how report engine use report cache.")]
	[StiOrder(310)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiReportCacheMode ReportCacheMode { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Data")]
	[StiOrder(220)]
	[Description("Gets or sets value which shows whether it is necessary to convert null or DBNull.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ConvertNulls { get; set; } = true;


	[Browsable(false)]
	[StiBrowsable(false)]
	[Description("Property 'StoreImagesInResources' is obsolete.")]
	[Obsolete("Property 'StoreImagesInResources' is obsolete.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool StoreImagesInResources { get; set; } = true;


	[StiSerializable]
	[DefaultValue(StiPreviewMode.Standard)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("View")]
	[Description("Gets or sets the preview mode of the report.")]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiOrder(270)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiPreviewMode PreviewMode { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(0)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[Description("Gets or sets a page number. When this page is reached then the report rendering is stopped.")]
	[StiOrder(340)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int StopBeforePage
	{
		get
		{
			return stopBeforePage;
		}
		set
		{
			if (value >= 0)
			{
				stopBeforePage = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(260039167)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("View")]
	[Description("Gets or sets controls which will be shown in the Viewer Window.")]
	[TypeConverter(typeof(StiPreviewSettingsConverter))]
	[Editor("Stimulsoft.Report.Viewer.Design.StiPreviewSettingsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiOrder(280)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int PreviewSettings { get; set; } = 260039167;


	[StiSerializable]
	[DefaultValue(StiDashboardViewerSettings.All)]
	[Description("Gets or sets controls which will be shown in the Dashboard Viewer.")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiBrowsable(false)]
	public StiDashboardViewerSettings DashboardViewerSettings { get; set; } = StiDashboardViewerSettings.All;


	[StiSerializable]
	[DefaultValue(1)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[Description("Gets or sets value which can be used for pages collating. The value of the property cannot be less then 1.")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiOrder(230)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int Collate
	{
		get
		{
			return collate;
		}
		set
		{
			if (collate != value)
			{
				collate = Math.Max(1, value);
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("View")]
	[Description("Gets or sets printer settings of current report.")]
	[TypeConverter(typeof(StiPrinterSettingsConverter))]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiOrder(290)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiPrinterSettings PrinterSettings { get; set; } = new StiPrinterSettings();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[Description("Gets or sets an array of referenced assemblies.")]
	[StiOrder(300)]
	[TypeConverter(typeof(StiReferencedAssembliesConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string[] ReferencedAssemblies { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[StiOrder(330)]
	[Description("Gets or sets the current script language.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiReportLanguageType ScriptLanguage
	{
		get
		{
			return scriptLanguage;
		}
		set
		{
			if (scriptLanguage != value)
			{
				scriptLanguage = value;
				if (!IsSerializing)
				{
					ScriptNew();
					IsModified = true;
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[StiBrowsable(false)]
	[Category("View")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a date format for datetime parameters in parameters panel.")]
	public string ParametersDateFormat { get; set; } = string.Empty;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(StiOrientation.Horizontal)]
	[Category("View")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(324)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value which indicates parameters panel orientation.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiOrientation ParametersOrientation { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(false)]
	[Category("View")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(325)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether to request parameters from a user before report rendering or render a report with the default value of parameters.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool RequestParameters { get; set; }

	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Main")]
	[StiBrowsable(false)]
	[Description("Gets or sets value, which allows to save report in resources.")]
	public bool SaveReportInResources { get; set; } = true;


	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Editor("Stimulsoft.Report.Design.StiAboutEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiAboutConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Main")]
	[StiBrowsable(false)]
	[Description("Returns information about the report.")]
	public object About => null;

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Engine")]
	[Description("Gets or sets value, which allows to cache totals.")]
	[StiOrder(212)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool CacheTotals { get; set; }

	[StiSerializable]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("View")]
	[StiBrowsable(true)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiOrder(305)]
	[Description("Gets or sets the time (in seconds) after which the report will be automatically rebuilt in the viewer.")]
	[Editor("Stimulsoft.Report.Design.StiRefreshTimeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public int RefreshTime
	{
		get
		{
			return refreshTime;
		}
		set
		{
			refreshTime = ((value > 0) ? value : 0);
		}
	}

	[Obsolete("StiReport.DefaultReportLanguage property is obsolete. Please use StiOptions.Engine.DefaultReportLanguage property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static StiReportLanguageType DefaultReportLanguage
	{
		get
		{
			return StiOptions.Engine.DefaultReportLanguage;
		}
		set
		{
			StiOptions.Engine.DefaultReportLanguage = value;
		}
	}

	[Obsolete("StiReport.ReportType property is obsolete. Please use StiOptions.Engine.BaseReportType property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Type ReportType
	{
		get
		{
			return StiOptions.Engine.BaseReportType;
		}
		set
		{
			StiOptions.Engine.BaseReportType = value;
		}
	}

	[Obsolete("StiReport.HideMessages property is obsolete. Please use StiOptions.Engine.HideMessages property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool HideMessages
	{
		get
		{
			return StiOptions.Engine.HideMessages;
		}
		set
		{
			StiOptions.Engine.HideMessages = value;
		}
	}

	[Obsolete("StiReport.HideExceptions property is obsolete. Please use StiOptions.Engine.HideExceptions property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool HideExceptions
	{
		get
		{
			return StiOptions.Engine.HideExceptions;
		}
		set
		{
			StiOptions.Engine.HideExceptions = value;
		}
	}

	[Obsolete("Please use GlobalMeasureGraphics property instead MeasureGraphics property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Graphics MeasureGraphics => GlobalMeasureGraphics;

	public static Graphics GlobalMeasureGraphics
	{
		get
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			lock (lockGlobalMeasureGraphics)
			{
				if (globalMeasureGraphics == null)
				{
					if (StiOptions.Engine.OldWYSIWYG)
					{
						globalMeasureGraphics = Graphics.FromImage((Image)new Bitmap(1, 1));
						globalMeasureGraphics.PageUnit = (GraphicsUnit)4;
						globalMeasureGraphics.PageScale = 0.01f;
					}
					else
					{
						globalMeasureGraphics = Graphics.FromImage((Image)new Bitmap(1, 1));
						globalMeasureGraphics.PageUnit = (GraphicsUnit)2;
						globalMeasureGraphics.PageScale = 1f;
					}
				}
				return globalMeasureGraphics;
			}
		}
	}

	public static Graphics GlobalRichTextMeasureGraphics
	{
		get
		{
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			if (globalRichTextMeasureGraphics == null)
			{
				lock (objLockGlobalRichTextMeasureGraphics)
				{
					if (globalRichTextMeasureGraphics != null)
					{
						return globalRichTextMeasureGraphics;
					}
					try
					{
						StringCollection installedPrinters = PrinterSettings.InstalledPrinters;
						if (StiOptions.Engine.CheckInstalledPrintersForRichTextMeasureGraphics && installedPrinters.Count > 0)
						{
							PrintDocument val = new PrintDocument();
							try
							{
								globalRichTextMeasureGraphics = val.PrinterSettings.CreateMeasurementGraphics();
								long num = Environment.TickCount;
								foreach (string item in installedPrinters)
								{
									val.PrinterSettings.PrinterName = item;
									Graphics val2 = val.PrinterSettings.CreateMeasurementGraphics();
									if (val2.DpiX > globalRichTextMeasureGraphics.DpiX)
									{
										globalRichTextMeasureGraphics = val2;
									}
									if (Environment.TickCount - num > 1000)
									{
										break;
									}
								}
							}
							finally
							{
								((IDisposable)val)?.Dispose();
							}
						}
					}
					catch
					{
					}
					if (globalRichTextMeasureGraphics == null)
					{
						globalRichTextMeasureGraphics = Graphics.FromImage((Image)new Bitmap(1, 1));
						globalRichTextMeasureGraphics.PageUnit = (GraphicsUnit)2;
						globalRichTextMeasureGraphics.PageScale = 1f;
					}
				}
			}
			return globalRichTextMeasureGraphics;
		}
		set
		{
			if (globalRichTextMeasureGraphics != value)
			{
				globalRichTextMeasureGraphics = value;
			}
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Category("Globalization")]
	[Description("Gets or sets default report culture.")]
	[StiOrder(235)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiBrowsable(true)]
	[Editor("Stimulsoft.Report.Design.StiCultureEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiCultureConverter))]
	public string Culture { get; set; } = string.Empty;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Globalization")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiGlobalizationContainerConverter))]
	[Editor("Stimulsoft.Report.Design.StiGlobalizationContainerCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets a collection which consists of globalization strings.")]
	[Browsable(false)]
	[StiOrder(250)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiGlobalizationContainerCollection GlobalizationStrings { get; }

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Globalization")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets property which allows automatic localization of the report when running starts.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool AutoLocalizeReportOnRun { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IStiGlobalizationManager GlobalizationManager { get; set; } = new StiNullGlobalizationManager();


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizedName
	{
		get
		{
			if (!(StiLocalization.CultureName == "en"))
			{
				return StiLocalization.Get("Components", "StiReport");
			}
			return "Report";
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[StiSerializable]
	[DefaultValue("")]
	public string ReportFile { get; set; } = string.Empty;


	[Browsable(false)]
	[StiBrowsable(false)]
	internal bool SaveInteractionParametersToDocument { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public IStiViewerControl ViewerControl { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public Type ViewerForm { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use property 'ViewerControl' instead 'PreviewControl' property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public IStiViewerControl PreviewControl
	{
		get
		{
			return ViewerControl;
		}
		set
		{
			ViewerControl = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("Please use property 'ViewerForm' instead 'PreviewForm' property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Type PreviewForm
	{
		get
		{
			return ViewerForm;
		}
		set
		{
			ViewerForm = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets an index of the current page printed taking into consideration segmented pages. Number starts from 1. Property ResetPageNumber can reset value of this variable.")]
	public int PageNumber
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion == 1)
			{
				if (EngineV1.ReportPageNumbers != null && EngineV1.ReportPageNumbers[CurrentPrintPage] is int && CurrentPrintPage < EngineV1.ReportPageNumbers.Count)
				{
					return (int)EngineV1.ReportPageNumbers[CurrentPrintPage];
				}
				return pageNumber;
			}
			if (Engine == null || Engine.PageNumbers == null)
			{
				return pageNumber;
			}
			return Engine.PageNumbers.GetPageNumber(CurrentPrintPage - 1);
		}
		set
		{
			pageNumber = value;
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets an index of the current page printed taking into consideration segmented pages. Number starts from 1. Property ResetPageNumber is ignored.")]
	public int PageNumberThrough
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion == 1)
			{
				throw new Exception("You can use system variable 'PageNumberThrough' only with EngineV2.");
			}
			if (Engine == null || Engine.PageNumbers == null)
			{
				return 0;
			}
			return Engine.PageNumbers.GetPageNumberThrough(CurrentPrintPage - 1);
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets the number of pages in a report. Property ResetPageNumber can reset value of this variable.")]
	public int TotalPageCount
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion == 1)
			{
				if (EngineV1.ReportTotalPageCounts != null && EngineV1.ReportTotalPageCounts[CurrentPrintPage] is int && CurrentPrintPage < EngineV1.ReportTotalPageCounts.Count)
				{
					return (int)EngineV1.ReportTotalPageCounts[CurrentPrintPage];
				}
				return totalPageCountValue;
			}
			if (Engine != null && Engine.PageNumbers != null)
			{
				return Engine.PageNumbers.GetTotalPageCount(CurrentPrintPage - 1);
			}
			return 0;
		}
		set
		{
			totalPageCountValue = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets the number of pages in a report. Property ResetPageNumber is ignored.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public int TotalPageCountThrough
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion == 1)
			{
				throw new Exception("You can use system variable 'TotalPageCountThrough' only with EngineV2.");
			}
			if (Engine != null && Engine.PageNumbers != null)
			{
				return Engine.PageNumbers.GetTotalPageCountThrough(CurrentPrintPage - 1);
			}
			return 0;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets string value which contains \"Page N of M\". Property ResetPageNumber can reset value of N.")]
	public string PageNofM => StiSystemVariableLocHelper.GetPageNofM(this);

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets string value which contains \"Page N of M\". Property ResetPageNumber is ignored.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public string PageNofMThrough
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion == 1)
			{
				throw new Exception("You can use system variable 'PageNofMThrough' only with EngineV2.");
			}
			return StiSystemVariableLocHelper.GetPageNofMThrough(this);
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets string value which contains localization template for 'Page N of M'. For example: Page {0} of {1}.By default this property equal to null and report engine use global localization string.")]
	public string PageNofMLocalizationString { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets current line that starts at the beginning of a group.")]
	public int Line { get; set; } = 1;


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets the current group number.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public int GroupLine
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion == 1)
			{
				throw new Exception("You can use system variable 'GroupLine' only with EngineV2.");
			}
			return groupLine;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)EngineVersion == 1)
			{
				throw new Exception("You can use system variable 'GroupLine' only with EngineV2.");
			}
			groupLine = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets the current line (Roman numerals) that starts at the beginning of a group.")]
	public string LineRoman => Func.Convert.ToRoman(Line);

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets current line (Alphabetical) that starts at the beginning of a group.")]
	public string LineABC => Func.Convert.ToABC(Line);

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets current column.")]
	public int Column { get; set; } = 1;


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets current line starts at the beginning of a report.")]
	public int LineThrough { get; set; } = 1;


	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets the current date.")]
	public DateTime Date => DateTime.Today;

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets the current date.")]
	public DateTime Today => DateTime.Today;

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets the current time.")]
	public DateTime Time => DateTime.Now;

	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsFirstPage => PageNumber == 1;

	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsLastPage => PageNumber == TotalPageCount;

	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsFirstPageThrough => PageNumberThrough == 1;

	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsLastPageThrough => PageNumberThrough == TotalPageCountThrough;

	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsFirstPass
	{
		get
		{
			if (ReportPass != StiReportPass.First)
			{
				return ReportPass == StiReportPass.None;
			}
			return true;
		}
	}

	[StiBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsSecondPass => ReportPass == StiReportPass.Second;

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int CurrentPage { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int CurrentPrintPage { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets a number of a current copy of a page.")]
	public int PageCopyNumber { get; set; } = 1;


	public event StiReportControlUpdateEventHandler RefreshControl;

	[Obsolete("Please use event 'RefreshViewer' instead event 'RefreshPreview'.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler RefreshPreview;

	public event EventHandler RefreshViewer;

	public event EventHandler Click;

	public event EventHandler DoubleClick;

	public event StiGotoCompEventHandler GotoComp;

	public event EventHandler Paint;

	public event EventHandler BeginRender;

	public event EventHandler Rendering;

	public event EventHandler EndRender;

	public event EventHandler StatusChanged;

	public event StiExportEventHandler Exporting;

	public event StiExportEventHandler Exported;

	public event EventHandler Printing;

	public event EventHandler Printed;

	public event EventHandler Refreshing;

	public event StiGetSubReportEventHandler GetSubReport;

	internal bool CheckNeedsCompiling()
	{
		if (!StiOptions.Engine.FullTrust || StiOptions.Engine.ForceInterpretationMode || CalculationMode == StiCalculationMode.Interpretation)
		{
			return false;
		}
		return NeedsCompiling;
	}

	public StiReport CreateInstance(string path)
	{
		bool flag = string.IsNullOrEmpty(path);
		if (!flag)
		{
			CompiledReport = GetReportFromAssembly(path);
		}
		else
		{
			CompiledReport = GetReportFromAssembly(CompilerResults.CompiledAssembly);
			CompiledReport.ReportVersion = ReportVersion;
			CompiledReport.ReportName = ReportName;
			CompiledReport.ReportAlias = ReportAlias;
			CompiledReport.ReportDescription = ReportDescription;
		}
		CompiledReport.Dictionary.DataStore.RegData(Dictionary.DataStore);
		CompiledReport.RegBusinessObject(BusinessObjectsStore);
		CompiledReport.GlobalizationManager = GlobalizationManager;
		CompiledReport.ReportFile = ReportFile;
		CompiledReport.IsWpf = IsWpf;
		CompiledReport.CookieContainer = CookieContainer;
		if (flag)
		{
			StiReportResourcesLoader.LoadReportResourcesFromReport(this, CompiledReport);
		}
		else
		{
			StiReportResourcesLoader.LoadReportResourcesFromAssembly(CompiledReport);
		}
		return this;
	}

	public async Task<StiReport> CompileAsync(StiOutputType outputType)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return await Task.Run(() => Compile(outputType));
	}

	public StiReport Compile(StiOutputType outputType)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Compile("", outputType);
	}

	public async Task<StiReport> CompileAsync(string path)
	{
		return await Task.Run(() => Compile(path));
	}

	public StiReport Compile(string path)
	{
		return Compile(path, (StiOutputType)0);
	}

	public async Task<StiReport> CompileAsync()
	{
		return await Task.Run(() => Compile());
	}

	public StiReport Compile()
	{
		return Compile((StiOutputType)0);
	}

	public async Task<StiReport> CompileAsync(string path, StiOutputType outputType)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		return await Task.Run(() => Compile(path, outputType));
	}

	public StiReport Compile(string path, StiOutputType outputType)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return Compile(path, outputType, autoCreate: true);
	}

	public async Task<StiReport> CompileAsync(Stream stream)
	{
		return await Task.Run(() => Compile(stream));
	}

	public StiReport Compile(Stream stream)
	{
		Compile(null, stream, (StiOutputType)0, autoCreate: true);
		return this;
	}

	public async Task<StiReport> CompileAsync(string path, StiOutputType outputType, bool autoCreate)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		return await Task.Run(() => Compile(path, outputType, autoCreate));
	}

	public StiReport Compile(string path, StiOutputType outputType, bool autoCreate)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		Compile(path, null, outputType, autoCreate);
		return this;
	}

	private string[] GetReferencedAssemblies()
	{
		bool flag = false;
		List<string> list = new List<string>();
		foreach (StiDataSource dataSource in Dictionary.DataSources)
		{
			if (!StiOptions.Dictionary.BusinessObjects.AddBusinessObjectAssemblyToReferencedAssembliesAutomatically)
			{
				continue;
			}
			StiDataStoreSource stiDataStoreSource = dataSource as StiDataStoreSource;
			if (dataSource == null || flag)
			{
				continue;
			}
			foreach (StiData item in DataStore)
			{
				if (item.IsReportData || !(item.Name == stiDataStoreSource.NameInSource) || !item.IsBusinessObjectData || item.ViewData == null)
				{
					continue;
				}
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					string path = Path.GetFileName(entryAssembly.Location).ToLowerInvariant();
					if (ReferencedAssemblies.All((string str) => str.ToLowerInvariant() != path))
					{
						list.Add(entryAssembly.Location);
					}
					flag = true;
				}
			}
		}
		if (ReferencedAssemblies != null)
		{
			string[] referencedAssemblies = ReferencedAssemblies;
			foreach (string text in referencedAssemblies)
			{
				if (text == null || text.Trim().Length == 0)
				{
					continue;
				}
				switch (text.Trim().ToLowerInvariant())
				{
				case "stimulsoft.controls.dll":
				case "stimulsoft.base.dll":
				case "stimulsoft.data.dll":
				case "stimulsoft.report.dll":
				case "stimulsoft.report.design.dll":
				case "stimulsoft.report.odbcdatabase.dll":
				case "stimulsoft.report.oracledatabase.dll":
					continue;
				}
				Assembly assembly = StiAssemblyFinder.GetAssembly(text, true);
				if (assembly == null)
				{
					throw new Exception("Referenced assembly '" + text + "' not found");
				}
				list.Add(assembly.Location);
			}
		}
		list.Add(typeof(StiActivator).Assembly.Location);
		list.Add(typeof(StiReport).Assembly.Location);
		list.Add(typeof(StiDataSortDirection).Assembly.Location);
		if (GetType().Name != "StiReport")
		{
			list.Add(GetType().Assembly.Location);
		}
		Hashtable hashtable = new Hashtable();
		foreach (string item2 in list)
		{
			hashtable[item2.ToLower(CultureInfo.InvariantCulture)] = item2;
		}
		string[] assebliesOfFunctions = StiFunctions.GetAssebliesOfFunctions();
		string[] array = assebliesOfFunctions;
		foreach (string text2 in array)
		{
			if (hashtable[text2.ToLower(CultureInfo.InvariantCulture)] == null)
			{
				hashtable[text2.ToLower(CultureInfo.InvariantCulture)] = text2;
			}
		}
		try
		{
			string location = typeof(StiChartOptions).Assembly.Location;
			hashtable[location] = location;
		}
		catch
		{
		}
		try
		{
			if (StiDashboardAssembly.IsAssemblyLoaded)
			{
				hashtable[StiDashboardAssembly.Assembly.Location] = StiDashboardAssembly.Assembly.Location;
			}
		}
		catch
		{
		}
		string[] array2 = new string[hashtable.Keys.Count];
		hashtable.Values.CopyTo(array2, 0);
		return array2;
	}

	private void Compile(string path, Stream stream, StiOutputType outputType, bool autoCreate)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		Compile(path, stream, outputType, autoCreate, null);
	}

	public async Task<StiReport> CompileStandaloneReportAsync(string path, StiStandaloneReportType standaloneReportType)
	{
		return await Task.Run(() => CompileStandaloneReport(path, standaloneReportType));
	}

	public StiReport CompileStandaloneReport(string path, StiStandaloneReportType standaloneReportType)
	{
		return Compile(path, null, (StiOutputType)2, autoCreate: false, standaloneReportType);
	}

	private StiReport Compile(string path, Stream stream, StiOutputType outputType, bool autoCreate, object standaloneReportType)
	{
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		if (!StiOptions.Engine.FullTrust)
		{
			return this;
		}
		StiOptions.Engine.GlobalEvents.InvokeReportCompiling(this, EventArgs.Empty);
		if (!string.IsNullOrEmpty(path) && File.Exists(path))
		{
			File.Delete(path);
		}
		if (!NeedsCompiling)
		{
			throw new Exception("Report already compiled");
		}
		bool flag = string.IsNullOrEmpty(path);
		if (stream != null && flag)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("Temp");
			if (environmentVariable == null)
			{
				throw new Exception("Can't get Temp directory");
			}
			path = environmentVariable + "\\" + Guid.NewGuid().ToString().Replace("-", "") + ".dll";
		}
		if (StiOptions.Engine.AllowSetCurrentDirectory)
		{
			Directory.SetCurrentDirectory(StiOptions.Configuration.ApplicationDirectory);
		}
		StiLogService.Write(GetType(), "Compiling report");
		string[] referencedAssemblies = GetReferencedAssemblies();
		ProcessAutoLocalizeReportOnRun();
		if (StiOptions.Dictionary.RemoveUnusedDataBeforeReportRendering)
		{
			Dictionary.RemoveUnusedData();
		}
		string text = Script;
		try
		{
			bool inMemory = string.IsNullOrWhiteSpace(path);
			using StiReportResourcesCompiler stiReportResourcesCompiler = new StiReportResourcesCompiler(this, inMemory);
			ScriptUpdate(standaloneReportType, allowUseResources: false);
			LanguageType val = (LanguageType)0;
			if (ScriptLanguage == StiReportLanguageType.VB)
			{
				val = (LanguageType)1;
			}
			string text2 = Script;
			if (!string.IsNullOrEmpty(StiOptions.Engine.PathToAssemblyKeyFile) && File.Exists(StiOptions.Engine.PathToAssemblyKeyFile))
			{
				string pathToAssemblyKeyFile = StiOptions.Engine.PathToAssemblyKeyFile;
				pathToAssemblyKeyFile = pathToAssemblyKeyFile.Replace("\\", "\\\\");
				string value;
				string value2;
				if (ScriptLanguage == StiReportLanguageType.CSharp || ScriptLanguage == StiReportLanguageType.JS)
				{
					value = "namespace";
					value2 = "[assembly: System.Reflection.AssemblyKeyFile(\"" + pathToAssemblyKeyFile + "\")]\n";
				}
				else
				{
					value = "Namespace";
					value2 = "<Assembly: System.Reflection.AssemblyKeyFile(\"" + pathToAssemblyKeyFile + "\")>\n";
				}
				int startIndex = text2.IndexOf(value, StringComparison.InvariantCulture);
				text2 = text2.Insert(startIndex, value2);
			}
			CompilerResults = StiCompiler.Compile(text2, path, val, outputType, referencedAssemblies, stiReportResourcesCompiler.Files);
		}
		catch (Exception ex)
		{
			if (CompilerResults == null)
			{
				CompilerResults = new CompilerResults((TempFileCollection)null);
			}
			CompilerResults.Errors.Add(new CompilerError
			{
				Column = -1,
				Line = -1,
				IsWarning = false,
				ErrorText = ex.Message
			});
			StiLogService.Write(GetType(), "Compiling report...ERROR");
			StiLogService.Write(GetType(), ex);
			throw;
		}
		finally
		{
			Script = text;
		}
		bool flag2 = true;
		if (CompilerResults != null && CompilerResults.Errors != null && ((CollectionBase)(object)CompilerResults.Errors).Count > 0 && CompilerResults.Errors.HasErrors)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CompilerError item in (CollectionBase)(object)CompilerResults.Errors)
			{
				CompilerError val2 = item;
				stringBuilder.Append(val2);
				StiLogService.Write(GetType(), ((object)val2).ToString());
				if (!val2.IsWarning)
				{
					flag2 = false;
				}
			}
			if (!IsDesigning)
			{
				throw new Exception(stringBuilder.ToString());
			}
		}
		if (flag2)
		{
			if (autoCreate)
			{
				CreateInstance(path);
			}
			if (CompiledReport != null)
			{
				CompiledReport.Variables = Variables;
			}
			if (StiOptions.Engine.AllowResetAssemblyAfterCompilation)
			{
				CompilerResults.CompiledAssembly = null;
			}
			if (stream != null)
			{
				FileStream fileStream = null;
				try
				{
					StiFileUtils.ProcessReadOnly(path);
					fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					stream.Write(array, 0, array.Length);
				}
				finally
				{
					fileStream?.Close();
					if (flag && File.Exists(path))
					{
						File.Delete(path);
					}
				}
			}
		}
		IsDocument = false;
		StiOptions.Engine.GlobalEvents.InvokeReportCompiled(this, EventArgs.Empty);
		return this;
	}

	internal void ProcessAutoLocalizeReportOnRun()
	{
		if (!AutoLocalizeReportOnRun)
		{
			return;
		}
		try
		{
			GlobalizationStrings.SkipException = true;
			CultureInfo info = CultureInfo.CurrentCulture;
			if (!string.IsNullOrWhiteSpace(Culture))
			{
				try
				{
					info = new CultureInfo(Culture, useUserOverride: false);
				}
				catch
				{
				}
			}
			GlobalizationStrings.LocalizeReport(info);
		}
		finally
		{
			GlobalizationStrings.SkipException = false;
		}
	}

	public async Task<StiReport> CompileReportsToAssemblyAsync(string assemblyPath, StiReport[] reports)
	{
		await Task.Run(() => CompileReportsToAssembly(assemblyPath, reports));
		return this;
	}

	public CompilerResults CompileReportsToAssembly(string assemblyPath, StiReport[] reports)
	{
		return CompileReportsToAssembly(assemblyPath, reports, StiReportLanguageType.CSharp);
	}

	public async Task<StiReport> CompileReportsToAssemblyAsync(string assemblyPath, StiReport[] reports, StiReportLanguageType languageType)
	{
		await Task.Run(() => CompileReportsToAssembly(assemblyPath, reports, languageType));
		return this;
	}

	public static CompilerResults CompileReportsToAssembly(string assemblyPath, StiReport[] reports, StiReportLanguageType languageType)
	{
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[reports.Length];
		int num = 0;
		Hashtable hashtable = new Hashtable();
		StiReportResourcesCompiler stiReportResourcesCompiler = new StiReportResourcesCompiler(null, inMemory: false);
		foreach (StiReport stiReport in reports)
		{
			StiOptions.Engine.GlobalEvents.InvokeReportCompiling(stiReport, EventArgs.Empty);
			stiReportResourcesCompiler.ProcessReport(stiReport);
			stiReport.ScriptUpdate();
			string text = stiReport.Script;
			if (StiCompiler.AssemblyVersion != null)
			{
				int startIndex = text.IndexOf("namespace", StringComparison.Ordinal);
				text = text.Insert(startIndex, "[assembly: System.Reflection.AssemblyVersion(\"" + StiCompiler.AssemblyVersion + "\")]\n");
			}
			array[num++] = text;
			string[] referencedAssemblies = stiReport.GetReferencedAssemblies();
			string[] array2 = referencedAssemblies;
			foreach (string text2 in array2)
			{
				hashtable[text2] = text2;
			}
			StiOptions.Engine.GlobalEvents.InvokeReportCompiled(stiReport, EventArgs.Empty);
		}
		string[] array3 = new string[hashtable.Keys.Count];
		hashtable.Keys.CopyTo(array3, 0);
		CompilerParameters val = new CompilerParameters();
		val.ReferencedAssemblies.AddRange(array3);
		val.OutputAssembly = assemblyPath;
		val.CompilerOptions += " /target:library";
		val.GenerateExecutable = false;
		val.ReferencedAssemblies.AddRange(array3);
		if (stiReportResourcesCompiler.Files != null)
		{
			foreach (string file in stiReportResourcesCompiler.Files)
			{
				val.EmbeddedResources.Add(file);
			}
		}
		CodeDomProvider val2 = (CodeDomProvider)((languageType == StiReportLanguageType.CSharp || languageType == StiReportLanguageType.JS) ? new CSharpCodeProvider() : new VBCodeProvider());
		CompilerResults result = val2.CompileAssemblyFromSource(val, array);
		((Component)(object)val2).Dispose();
		stiReportResourcesCompiler.Dispose();
		return result;
	}

	public static bool CheckNeedForceInterpretationMode()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		bool result = false;
		try
		{
			object obj = TypeDescriptor.GetConverter(typeof(DateTime)).ConvertTo(DateTime.Now, typeof(InstanceDescriptor));
			obj = TypeDescriptor.GetConverter(typeof(Font)).ConvertTo((object?)new Font("Arial", 8f), typeof(InstanceDescriptor));
		}
		catch
		{
			result = true;
		}
		return result;
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Invalid comparison between Unknown and I4
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Invalid comparison between Unknown and I4
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Invalid comparison between Unknown and I4
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Invalid comparison between Unknown and I4
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[6]
		{
			propertiesHelper.ReportName(),
			propertiesHelper.ReportAlias(),
			propertiesHelper.ReportAuthor(),
			propertiesHelper.ReportDescription(),
			propertiesHelper.ReportIcon(),
			propertiesHelper.ReportImage()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Design, value);
		if ((int)level == 1)
		{
			value = new StiPropertyObject[1] { propertiesHelper.ConvertNulls() };
			stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		}
		else if ((int)level == 2)
		{
			value = new StiPropertyObject[3]
			{
				propertiesHelper.CacheAllData(),
				propertiesHelper.ConvertNulls(),
				propertiesHelper.RetrieveOnlyUsedData()
			};
			stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		}
		if ((int)level == 1)
		{
			value = new StiPropertyObject[1] { propertiesHelper.ReportCulture() };
			stiPropertyCollection.Add(StiPropertyCategories.Globalization, value);
		}
		else if ((int)level == 2)
		{
			value = new StiPropertyObject[3]
			{
				propertiesHelper.AutoLocalizeReportOnRun(),
				propertiesHelper.ReportCulture(),
				propertiesHelper.GlobalizationStrings()
			};
			stiPropertyCollection.Add(StiPropertyCategories.Globalization, value);
		}
		if ((int)level == 1)
		{
			value = new StiPropertyObject[4]
			{
				propertiesHelper.NumberOfPass(),
				propertiesHelper.ReportUnit(),
				propertiesHelper.ScriptLanguage(),
				propertiesHelper.ReportStyles()
			};
			stiPropertyCollection.Add(StiPropertyCategories.Engine, value);
		}
		else if ((int)level == 2)
		{
			value = new StiPropertyObject[11]
			{
				propertiesHelper.CacheTotals(),
				propertiesHelper.CalculationMode(),
				propertiesHelper.Collate(),
				propertiesHelper.ReportEngineVersion(),
				propertiesHelper.NumberOfPass(),
				propertiesHelper.ReferencedAssemblies(),
				propertiesHelper.ReportCacheMode(),
				propertiesHelper.ReportUnit(),
				propertiesHelper.ScriptLanguage(),
				propertiesHelper.StopBeforePage(),
				propertiesHelper.ReportStyles()
			};
			stiPropertyCollection.Add(StiPropertyCategories.Engine, value);
		}
		if ((int)level == 1)
		{
			value = new StiPropertyObject[4]
			{
				propertiesHelper.PreviewSettings(),
				propertiesHelper.ReportPrinterSettings(),
				propertiesHelper.ParametersOrientation(),
				propertiesHelper.RequestParameters()
			};
			stiPropertyCollection.Add(StiPropertyCategories.View, value);
		}
		else if ((int)level == 2)
		{
			value = new StiPropertyObject[5]
			{
				propertiesHelper.PreviewSettings(),
				propertiesHelper.ReportPrinterSettings(),
				propertiesHelper.RefreshTime(),
				propertiesHelper.ParametersOrientation(),
				propertiesHelper.RequestParameters()
			};
			stiPropertyCollection.Add(StiPropertyCategories.View, value);
		}
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		StiPropertyEventId[] value = new StiPropertyEventId[3]
		{
			StiPropertyEventId.BeginRenderEvent,
			StiPropertyEventId.RenderingEvent,
			StiPropertyEventId.EndRenderEvent
		};
		stiEventCollection.Add(StiPropertyCategories.RenderEvents, value);
		value = new StiPropertyEventId[2]
		{
			StiPropertyEventId.ExportingEvent,
			StiPropertyEventId.ExportedEvent
		};
		stiEventCollection.Add(StiPropertyCategories.ExportEvents, value);
		value = new StiPropertyEventId[2]
		{
			StiPropertyEventId.PrintingEvent,
			StiPropertyEventId.PrintedEvent
		};
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, value);
		return stiEventCollection;
	}

	public virtual void SaveState(string stateName)
	{
		Pages.SaveState(stateName);
		Dictionary.SaveState(stateName);
		if (AggregateFunctions != null && AggregateFunctions.Length != 0)
		{
			object[] aggregateFunctions = AggregateFunctions;
			for (int i = 0; i < aggregateFunctions.Length; i++)
			{
				StiAggregateFunctionService stiAggregateFunctionService = (StiAggregateFunctionService)aggregateFunctions[i];
				stiAggregateFunctionService.SaveState(stateName);
			}
		}
	}

	public virtual void RestoreState(string stateName)
	{
		Pages.RestoreState(stateName);
		Dictionary.RestoreState(stateName);
		if (AggregateFunctions != null && AggregateFunctions.Length != 0)
		{
			object[] aggregateFunctions = AggregateFunctions;
			for (int i = 0; i < aggregateFunctions.Length; i++)
			{
				StiAggregateFunctionService stiAggregateFunctionService = (StiAggregateFunctionService)aggregateFunctions[i];
				stiAggregateFunctionService.RestoreState(stateName);
			}
		}
	}

	public void ClearAllStates()
	{
		States.Clear();
		Pages.ClearAllStates();
		if (AggregateFunctions != null && AggregateFunctions.Length != 0)
		{
			object[] aggregateFunctions = AggregateFunctions;
			for (int i = 0; i < aggregateFunctions.Length; i++)
			{
				StiAggregateFunctionService stiAggregateFunctionService = (StiAggregateFunctionService)aggregateFunctions[i];
				stiAggregateFunctionService.ClearAllStates();
			}
		}
	}

	public void Select()
	{
		if (!isSelected)
		{
			SelectionTick = Environment.TickCount;
		}
		isSelected = true;
	}

	public void Invert()
	{
		if (!isSelected)
		{
			SelectionTick = Environment.TickCount;
		}
		isSelected = !isSelected;
	}

	public void Reset()
	{
		isSelected = false;
	}

	public virtual object Clone()
	{
		StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		string reportFile = ReportFile;
		bool flag = IsModified;
		string reportStr = SaveToString();
		stiReport.LoadFromString(reportStr);
		IsModified = flag;
		if (this.GotoComp != null)
		{
			stiReport.GotoComp = (StiGotoCompEventHandler)this.GotoComp.Clone();
		}
		if (this.Paint != null)
		{
			stiReport.Paint = (EventHandler)this.Paint.Clone();
		}
		if (this.BeginRender != null)
		{
			stiReport.BeginRender = (EventHandler)this.BeginRender.Clone();
		}
		if (this.Rendering != null)
		{
			stiReport.Rendering = (EventHandler)this.Rendering.Clone();
		}
		if (this.EndRender != null)
		{
			stiReport.EndRender = (EventHandler)this.EndRender.Clone();
		}
		if (this.Refreshing != null)
		{
			stiReport.Refreshing = (EventHandler)this.Refreshing.Clone();
		}
		if (this.Printing != null)
		{
			stiReport.Printing = (EventHandler)this.Printing.Clone();
		}
		if (this.Printed != null)
		{
			stiReport.Printed = (EventHandler)this.Printed.Clone();
		}
		if (this.Exporting != null)
		{
			stiReport.Exporting = (StiExportEventHandler)this.Exporting.Clone();
		}
		if (this.Exported != null)
		{
			stiReport.Exported = (StiExportEventHandler)this.Exported.Clone();
		}
		if (BeginRenderEvent != null)
		{
			stiReport.BeginRenderEvent = (StiBeginRenderEvent)BeginRenderEvent.Clone();
		}
		if (RenderingEvent != null)
		{
			stiReport.RenderingEvent = (StiRenderingEvent)RenderingEvent.Clone();
		}
		if (EndRenderEvent != null)
		{
			stiReport.EndRenderEvent = (StiEndRenderEvent)EndRenderEvent.Clone();
		}
		if (PrintingEvent != null)
		{
			stiReport.PrintingEvent = (StiPrintingEvent)PrintingEvent.Clone();
		}
		if (PrintedEvent != null)
		{
			stiReport.PrintedEvent = (StiPrintedEvent)PrintedEvent.Clone();
		}
		if (ExportingEvent != null)
		{
			stiReport.ExportingEvent = (StiExportingEvent)ExportingEvent.Clone();
		}
		if (ExportedEvent != null)
		{
			stiReport.ExportedEvent = (StiExportedEvent)ExportedEvent.Clone();
		}
		stiReport.RegData(DataStore);
		stiReport.RegBusinessObject(BusinessObjectsStore);
		stiReport.CookieContainer = CookieContainer;
		stiReport.HttpHeadersContainer = HttpHeadersContainer;
		stiReport.ReportFile = reportFile;
		ReportFile = reportFile;
		return stiReport;
	}

	IStiAppDictionary IStiApp.GetDictionary()
	{
		return (IStiAppDictionary)(object)Dictionary;
	}

	string IStiAppCell.GetKey()
	{
		return Key;
	}

	void IStiAppCell.SetKey(string key)
	{
		Key = key;
	}

	IEnumerable<IStiReportPage> IStiReport.FetchPages()
	{
		return (IEnumerable<IStiReportPage>)(Pages?.ToList());
	}

	public List<StiFont> GetFonts()
	{
		List<StiFont> list = new List<StiFont>();
		foreach (object page in Pages)
		{
			if (page is IStiGetFonts)
			{
				list.AddRange(((IStiGetFonts)((page is IStiGetFonts) ? page : null)).GetFonts());
			}
		}
		return list.Distinct().ToList();
	}

	public bool IsVariableExist(string variableName)
	{
		if (CompiledReport != null || !CheckNeedsCompiling())
		{
			StiReport stiReport = CompiledReport ?? this;
			FieldInfo field = stiReport.GetType().GetField(variableName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
			if (field != null)
			{
				return true;
			}
			PropertyInfo property = stiReport.GetType().GetProperty(variableName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
			return property != null;
		}
		return Dictionary.Variables.IndexOf(variableName) != -1;
	}

	public StiReport ScriptPack()
	{
		script = StiLanguage.RemoveGeneratedCode(Script, Language);
		StiLanguage.GetGeneratedCodePos(script, out var index, out var _, Language);
		if (StiOptions.Engine.PackScriptAfterReportLoaded && index > 0)
		{
			script = StiLanguage.InsertGeneratedCode(script, "\r\n\t\t", Language);
		}
		return this;
	}

	public StiReport ScriptUnpack()
	{
		ScriptUnpack(saveForInheritedReports: false);
		return this;
	}

	public StiReport ScriptUnpack(bool saveForInheritedReports)
	{
		StiCodeDomSerializator stiCodeDomSerializator = new StiCodeDomSerializator();
		string text = stiCodeDomSerializator.Serialize(this, GetReportName(), Language, saveForInheritedReports);
		script = StiLanguage.ReplaceGeneratedCode(Script, text, Language);
		return this;
	}

	public StiReport ScriptUpdate()
	{
		ScriptUpdate(null, allowUseResources: true);
		return this;
	}

	public StiReport ScriptUpdate(bool allowUseResources)
	{
		ScriptUpdate(null, allowUseResources);
		return this;
	}

	private void ScriptUpdate(object standaloneReportType, bool allowUseResources)
	{
		UpdateReportVersion();
		StiCodeDomSerializator stiCodeDomSerializator = new StiCodeDomSerializator();
		string text = stiCodeDomSerializator.Serialize(this, GetReportName(), Language, standaloneReportType);
		script = StiLanguage.ReplaceGeneratedCode(Script, text, Language);
	}

	public StiReport ScriptNew()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (ScriptLanguage == StiReportLanguageType.CSharp || ScriptLanguage == StiReportLanguageType.JS)
		{
			if (StiOptions.Engine.Namespaces != null)
			{
				string[] namespaces = StiOptions.Engine.Namespaces;
				foreach (string arg in namespaces)
				{
					stringBuilder = stringBuilder.AppendFormat("using {0};", arg);
					stringBuilder = stringBuilder.AppendLine();
				}
			}
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendFormat("namespace {0}", StiOptions.Engine.DefaultNamespace);
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendLine("{");
			stringBuilder = stringBuilder.AppendFormat("    public class {0} : {1}", GetReportName(), StiOptions.Engine.BaseReportType);
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendLine("    {");
			stringBuilder = stringBuilder.AppendFormat("        public {0}()", GetReportName());
			stringBuilder = stringBuilder.AppendLine("        {");
			stringBuilder = stringBuilder.AppendLine("            this.InitializeComponent();");
			stringBuilder = stringBuilder.AppendLine("        }");
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendLine("        #region StiReport Designer generated code - do not modify");
			stringBuilder = stringBuilder.AppendLine("        #endregion StiReport Designer generated code - do not modify");
			stringBuilder = stringBuilder.AppendLine("    }");
			stringBuilder = stringBuilder.AppendLine("}");
		}
		else
		{
			if (StiOptions.Engine.Namespaces != null)
			{
				string[] namespaces2 = StiOptions.Engine.Namespaces;
				foreach (string arg2 in namespaces2)
				{
					stringBuilder = stringBuilder.AppendFormat("Imports {0}", arg2);
					stringBuilder = stringBuilder.AppendLine();
				}
			}
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendFormat("Namespace {0}", StiOptions.Engine.DefaultNamespace);
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendFormat("    Public Class {0}", GetReportName());
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendFormat("        Inherits {0}", StiOptions.Engine.BaseReportType);
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendLine("        Public Sub New()");
			stringBuilder = stringBuilder.AppendLine("            MyBase.New");
			stringBuilder = stringBuilder.AppendLine("            Me.InitializeComponent");
			stringBuilder = stringBuilder.AppendLine("        End Sub");
			stringBuilder = stringBuilder.AppendLine();
			stringBuilder = stringBuilder.AppendLine("        #Region \"StiReport Designer generated code - do not modify\"");
			stringBuilder = stringBuilder.AppendLine("        #End Region 'StiReport Designer generated code - do not modify");
			stringBuilder = stringBuilder.AppendLine("    End Class");
			stringBuilder = stringBuilder.AppendLine("End Namespace");
		}
		script = stringBuilder.ToString();
		return this;
	}

	public void InvokeRefreshControl(object sender, string propertyName)
	{
		InvokeRefreshControl(sender, new StiReportControlUpdateEventArgs(propertyName));
	}

	public void InvokeRefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		this.RefreshControl?.Invoke(sender, e);
		CompiledReport?.InvokeRefreshControl(sender, e);
	}

	[Obsolete("Please use method 'InvokeRefreshViewer' instead method 'InvokeRefreshPreview'.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InvokeRefreshPreview()
	{
		InvokeRefreshViewer();
	}

	public void InvokeRefreshViewer()
	{
		this.RefreshPreview?.Invoke(this, EventArgs.Empty);
		this.RefreshViewer?.Invoke(this, EventArgs.Empty);
		if (CompiledReport != null)
		{
			CompiledReport.InvokeRefreshViewer();
		}
	}

	public void InvokeClick(object sender, EventArgs e)
	{
		this.Click?.Invoke(sender, e);
	}

	public void InvokeDoubleClick(object sender, EventArgs e)
	{
		this.DoubleClick?.Invoke(sender, e);
	}

	public void InvokeGotoComp(StiGotoCompEventArgs e)
	{
		this.GotoComp?.Invoke(this, e);
	}

	public void InvokePaint(object sender, EventArgs e)
	{
		this.Paint?.Invoke(sender ?? this, e);
	}

	public void InvokeBeginRender()
	{
		try
		{
			this.BeginRender?.Invoke(this, EventArgs.Empty);
			StiBlocklyHelper.InvokeBlockly(this, this, BeginRenderEvent);
			StiOptions.Engine.GlobalEvents.InvokeReportBeginRender(this, EventArgs.Empty);
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(StiReport), "InvokeBeginRender...ERROR");
			StiLogService.Write(GetType(), ex.Message);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	public void InvokeRendering()
	{
		try
		{
			this.Rendering?.Invoke(this, EventArgs.Empty);
			StiBlocklyHelper.InvokeBlockly(this, this, RenderingEvent);
			StiOptions.Engine.GlobalEvents.InvokeReportRendering(this, EventArgs.Empty);
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(StiReport), "InvokeRendering...ERROR");
			StiLogService.Write(GetType(), ex.Message);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	public void InvokeEndRender()
	{
		try
		{
			if (CalculationMode == StiCalculationMode.Interpretation)
			{
				Hashtable hashtable = new Hashtable(Totals);
				foreach (DictionaryEntry item in hashtable)
				{
					if (item.Key is string text && text.StartsWith("#%#"))
					{
						(((StiService)GetComponentByName(text.Substring(3))).Clone() as StiText).SetText(null, null);
					}
				}
				StiSimpleText.ProcessEndRenderSetText(this);
			}
			this.EndRender?.Invoke(this, EventArgs.Empty);
			StiBlocklyHelper.InvokeBlockly(this, this, EndRenderEvent);
			StiOptions.Engine.GlobalEvents.InvokeReportEndRender(this, EventArgs.Empty);
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(StiReport), "InvokeEndRender...ERROR");
			StiLogService.Write(GetType(), ex.Message);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	public void InvokeStatusChanged()
	{
		try
		{
			this.StatusChanged?.Invoke(this, EventArgs.Empty);
		}
		catch (Exception ex)
		{
			StiLogService.Write(typeof(StiReport), "InvokeStatusChanged...ERROR");
			StiLogService.Write(GetType(), ex.Message);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	protected virtual void OnExporting(StiExportEventArgs e)
	{
		this.Exporting?.Invoke(this, e);
	}

	public void InvokeExporting(StiExportFormat exportFormat)
	{
		IsExporting = true;
		if (CompiledReport != null)
		{
			CompiledReport.IsExporting = true;
		}
		StiExportEventArgs e = new StiExportEventArgs(exportFormat);
		OnExporting(e);
		StiBlocklyHelper.InvokeBlockly(this, this, ExportingEvent);
		StiOptions.Engine.GlobalEvents.InvokeReportExporting(this, e);
	}

	protected virtual void OnExported(StiExportEventArgs e)
	{
		this.Exported?.Invoke(this, e);
	}

	public void InvokeExported(StiExportFormat exportFormat)
	{
		IsExporting = false;
		if (CompiledReport != null)
		{
			CompiledReport.IsExporting = false;
		}
		StiExportEventArgs e = new StiExportEventArgs(exportFormat);
		OnExported(e);
		StiBlocklyHelper.InvokeBlockly(this, this, ExportedEvent);
		StiOptions.Engine.GlobalEvents.InvokeReportExported(this, e);
	}

	protected virtual void OnPrinting(EventArgs e)
	{
		this.Printing?.Invoke(this, e);
	}

	public void InvokePrinting()
	{
		OnPrinting(EventArgs.Empty);
		StiBlocklyHelper.InvokeBlockly(this, this, PrintingEvent);
		StiOptions.Engine.GlobalEvents.InvokeReportPrinting(this, EventArgs.Empty);
	}

	protected virtual void OnPrinted(EventArgs e)
	{
		this.Printed?.Invoke(this, e);
	}

	public void InvokePrinted()
	{
		OnPrinted(EventArgs.Empty);
		StiBlocklyHelper.InvokeBlockly(this, this, PrintedEvent);
		StiOptions.Engine.GlobalEvents.InvokeReportPrinted(this, EventArgs.Empty);
	}

	protected virtual void OnRefreshing(EventArgs e)
	{
		this.Refreshing?.Invoke(this, e);
	}

	public void InvokeRefreshing()
	{
		OnRefreshing(EventArgs.Empty);
		StiBlocklyHelper.InvokeBlockly(this, this, RefreshingEvent);
		StiOptions.Engine.GlobalEvents.InvokeReportRefreshing(this, EventArgs.Empty);
	}

	protected virtual void OnGetSubReport(StiGetSubReportEventArgs e)
	{
		this.GetSubReport?.Invoke(this, e);
	}

	internal void InvokeGetSubReport(StiGetSubReportEventArgs e)
	{
		OnGetSubReport(e);
		if (CompiledReport != null)
		{
			CompiledReport.InvokeGetSubReport(e);
		}
		if (ParentReport != null)
		{
			ParentReport.OnGetSubReport(e);
		}
		StiOptions.Engine.GlobalEvents.InvokeReportGetSubReport(this, e);
	}

	public void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		if (oldUnit.GetType() == newUnit.GetType() || Pages == null)
		{
			return;
		}
		foreach (StiPage page in Pages)
		{
			page.Convert(oldUnit, newUnit);
		}
	}

	public static object ChangeType(object value, Type conversionType)
	{
		return ChangeType(value, conversionType, convertNulls: true);
	}

	public static object ChangeType(object value, Type conversionType, bool convertNulls)
	{
		return StiConvert.ChangeType(value, conversionType, convertNulls);
	}

	public StiReport ApplyStyleCollection(string collectionName)
	{
		if (string.IsNullOrEmpty(collectionName))
		{
			return this;
		}
		collectionName = collectionName.ToLowerInvariant().Trim();
		List<StiBaseStyle> list = new List<StiBaseStyle>();
		foreach (StiBaseStyle style in Styles)
		{
			if (style.CollectionName.ToLowerInvariant().Trim() == collectionName)
			{
				list.Add(style);
			}
		}
		StiComponentsCollection components = GetComponents();
		foreach (StiComponent item in components)
		{
			if (item is StiPage)
			{
				continue;
			}
			foreach (StiBaseStyle item2 in list)
			{
				if (!StiStyleConditionHelper.IsAllowStyle(item, item2) || item is StiChart)
				{
					continue;
				}
				if (item is StiDataBand)
				{
					bool flag = false;
					bool flag2 = false;
					foreach (StiStyleCondition condition in item2.Conditions)
					{
						if ((condition.Placement & StiStyleComponentPlacement.DataOddStyle) > StiStyleComponentPlacement.None)
						{
							flag = true;
							break;
						}
						if ((condition.Placement & StiStyleComponentPlacement.DataEvenStyle) > StiStyleComponentPlacement.None)
						{
							flag2 = true;
							break;
						}
					}
					if (flag)
					{
						((StiDataBand)item).OddStyle = item2.Name;
					}
					else if (flag2)
					{
						((StiDataBand)item).EvenStyle = item2.Name;
					}
					else
					{
						item.ComponentStyle = item2.Name;
					}
				}
				else
				{
					item.ComponentStyle = item2.Name;
				}
			}
		}
		ApplyStyles();
		return this;
	}

	public StiReport ApplyStyleCollection(string collectionName, List<StiComponent> comps)
	{
		if (string.IsNullOrEmpty(collectionName))
		{
			return this;
		}
		collectionName = collectionName.ToLowerInvariant().Trim();
		List<StiBaseStyle> list = new List<StiBaseStyle>();
		foreach (StiBaseStyle style in Styles)
		{
			if (style.CollectionName.ToLowerInvariant().Trim() == collectionName)
			{
				list.Add(style);
			}
		}
		foreach (StiComponent comp in comps)
		{
			if (comp is StiPage)
			{
				continue;
			}
			foreach (StiBaseStyle item in list)
			{
				if (!StiStyleConditionHelper.IsAllowStyle(comp, item) || comp is StiChart)
				{
					continue;
				}
				if (comp is StiDataBand)
				{
					bool flag = false;
					bool flag2 = false;
					foreach (StiStyleCondition condition in item.Conditions)
					{
						if ((condition.Placement & StiStyleComponentPlacement.DataOddStyle) > StiStyleComponentPlacement.None)
						{
							flag = true;
							break;
						}
						if ((condition.Placement & StiStyleComponentPlacement.DataEvenStyle) > StiStyleComponentPlacement.None)
						{
							flag2 = true;
							break;
						}
					}
					if (flag)
					{
						((StiDataBand)comp).OddStyle = item.Name;
					}
					else if (flag2)
					{
						((StiDataBand)comp).EvenStyle = item.Name;
					}
					else
					{
						comp.ComponentStyle = item.Name;
					}
				}
				else
				{
					comp.ComponentStyle = item.Name;
				}
			}
		}
		ApplyStyles();
		return this;
	}

	public StiReport ApplyStyles()
	{
		StiComponentsCollection components = GetComponents();
		foreach (StiComponent item in components)
		{
			if (item is StiChart { AllowApplyStyle: not false, Style: not null, Style: var style } stiChart)
			{
				stiChart.Core.ApplyStyle(style);
				if (style is StiCustomStyle stiCustomStyle)
				{
					((StiCustomStyleCoreXF)stiCustomStyle.Core).ReportStyle?.SetStyleToComponent(item);
				}
			}
			else if (!string.IsNullOrEmpty(item.ComponentStyle))
			{
				Styles[item.ComponentStyle]?.SetStyleToComponent(item);
			}
		}
		return this;
	}

	internal void RenameStyle(string oldStylename, string newStyleName)
	{
		StiComponentsCollection components = GetComponents();
		foreach (StiComponent item in components)
		{
			StiStylesHelper.ChangeComponentStyleName(item, oldStylename, newStyleName);
		}
	}

	public StiReport CreateReportInNewAppDomain()
	{
		AppDomainSetup appDomainSetup = new AppDomainSetup
		{
			ShadowCopyFiles = "false",
			ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
		};
		AppDomain appDomain = AppDomain.CreateDomain(StiGuidUtils.NewGuid(), AppDomain.CurrentDomain.Evidence, appDomainSetup);
		StiReport stiReport = appDomain.CreateInstanceAndUnwrap(GetType().Assembly.FullName, GetType().FullName) as StiReport;
		ReportDomain = appDomain;
		stiReport.LoadFromString(SaveToString());
		foreach (StiData item in DataStore)
		{
			stiReport.RegData(item.Name, item.Data);
			stiReport.DataStore[stiReport.DataStore.Count - 1].ViewData = item.ViewData;
		}
		return stiReport;
	}

	public void UnloadReportAppDomain()
	{
		if (ReportDomain != null)
		{
			AppDomain.Unload(ReportDomain);
			ReportDomain = null;
		}
	}

	public void UpdateInheritedReport(StiReport masterReport = null)
	{
		if (masterReport != null)
		{
			masterReport.SetInheritedMode(inherited: true);
			StiInheritedReportComparer.Compare(this, masterReport);
		}
		else if (!string.IsNullOrEmpty(MasterReport))
		{
			masterReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
			masterReport.LoadFromString(MasterReport);
			masterReport.SetInheritedMode(inherited: true);
			StiInheritedReportComparer.Compare(this, masterReport);
		}
	}

	public void SetInheritedMode(bool inherited)
	{
		foreach (StiVariable variable in Dictionary.Variables)
		{
			variable.Inherited = inherited;
		}
		foreach (StiDataSource dataSource in Dictionary.DataSources)
		{
			dataSource.Inherited = inherited;
		}
		foreach (StiBusinessObject businessObject in Dictionary.BusinessObjects)
		{
			businessObject.Inherited = inherited;
		}
		foreach (StiDatabase database in Dictionary.Databases)
		{
			database.Inherited = inherited;
		}
		foreach (StiDataRelation relation in Dictionary.Relations)
		{
			relation.Inherited = inherited;
		}
		StiComponentsCollection components = GetComponents();
		foreach (StiComponent item in components)
		{
			item.Inherited = inherited;
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (!disposing)
		{
			return;
		}
		if (CompiledReport != null)
		{
			CompiledReport.Dispose();
			CompiledReport = null;
		}
		if (!string.IsNullOrEmpty(ReportCachePath))
		{
			StiReportCache.DeleteCache(ReportCachePath);
			ReportCachePath = null;
		}
		if (ReportMeasureGraphics != null)
		{
			ReportMeasureGraphics.Dispose();
			ReportMeasureGraphics = null;
		}
		if (!string.IsNullOrEmpty(ImageCachePath))
		{
			StiFileImageCacheV2.DeleteCache(ImageCachePath);
			ImageCachePath = null;
		}
		if (!string.IsNullOrEmpty(RtfCachePath))
		{
			StiRtfCache.DeleteCache(RtfCachePath);
			RtfCachePath = null;
		}
		foreach (StiPage page in Pages)
		{
			page.Watermark.ResetImage();
			foreach (StiComponent component in page.GetComponents())
			{
				if (component is StiImage stiImage)
				{
					stiImage.ResetImage();
					stiImage.ResetImageToDraw();
				}
			}
		}
		foreach (StiPage renderedPage in RenderedPages)
		{
			renderedPage.Watermark.ResetImage();
			foreach (StiComponent component2 in renderedPage.GetComponents())
			{
				if (component2 is StiImage stiImage2)
				{
					stiImage2.ResetImage();
					stiImage2.ResetImageToDraw();
				}
			}
		}
		if (InteractionCollapsingStates != null)
		{
			InteractionCollapsingStates.Clear();
			InteractionCollapsingStates = null;
		}
		if (RenderedPages != null)
		{
			RenderedPages.Clear();
		}
		if (HashViewWpfPainter != null)
		{
			HashViewWpfPainter.Clear();
			HashViewWpfPainter = null;
		}
		if (RichTextImageCache != null)
		{
			RichTextImageCache.Clear();
			RichTextImageCache = null;
		}
		FontVHelper.RemoveFontsFromResources(this);
		StiOptions.Engine.GlobalEvents.InvokeReportDisposed(this, EventArgs.Empty);
	}

	public void ResetSelection()
	{
		isSelected = false;
		foreach (StiPage page in Pages)
		{
			page.ResetSelection();
		}
	}

	public StiPage GetCurrentPage()
	{
		if (CurrentPage < 0)
		{
			CurrentPage = 0;
		}
		if (CurrentPage > Pages.Count - 1)
		{
			return null;
		}
		return Pages[CurrentPage];
	}

	public object[] GetSelected()
	{
		StiPage currentPage = GetCurrentPage();
		StiComponentsCollection stiComponentsCollection = currentPage?.GetSelectedComponents();
		if (stiComponentsCollection != null && stiComponentsCollection.Count > 0)
		{
			return stiComponentsCollection.ToList().ToArray();
		}
		if (currentPage != null && currentPage.IsSelected)
		{
			return new object[1] { currentPage };
		}
		if (IsSelected)
		{
			return new object[1] { this };
		}
		return new object[0];
	}

	internal void DisposeCachedRichText()
	{
		if (cachedRichText != null)
		{
			((Component)(object)cachedRichText).Dispose();
			cachedRichText = null;
		}
	}

	internal string GetReportName()
	{
		if (!StiOptions.Engine.FullTrust)
		{
			return ReportName;
		}
		return StiNameValidator.CorrectName(ReportName, this);
	}

	public static string GetReportVersion()
	{
		Assembly assembly = Assembly.GetAssembly(typeof(StiReport));
		Version version = assembly.GetName().Version;
		return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
	}

	private void UpdateReportVersion()
	{
		ReportVersion = GetReportVersion();
	}

	public void WriteToReportRenderingMessages(string str)
	{
		if (ReportRenderingMessages == null)
		{
			ReportRenderingMessages = new StringCollection();
		}
		ReportRenderingMessages.Add(str);
	}

	public StiComponent GetComponentByName(string componentName)
	{
		foreach (StiPage page in Pages)
		{
			if (page.Name == componentName)
			{
				return page;
			}
		}
		return Pages.GetComponentByName(componentName);
	}

	public StiComponent GetComponentByGuid(string guid)
	{
		foreach (StiPage page in Pages)
		{
			if (page.Guid == guid)
			{
				return page;
			}
		}
		return Pages.GetComponentByGuid(guid);
	}

	protected internal string ToString(object obj)
	{
		if (obj != null && obj != DBNull.Value)
		{
			return obj.ToString();
		}
		return string.Empty;
	}

	protected internal object CheckExcelValue(object sender, object value)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		StiText stiText = sender as StiText;
		decimal? num = StiValueHelper.TryToNullableDecimal(value);
		if (num.HasValue)
		{
			decimal? num2 = num;
			if ((num2.GetValueOrDefault() < default(decimal)) & num2.HasValue)
			{
				if (StiNegativeColorChecker.IsNegativeInRed(stiText.TextFormat))
				{
					stiText.TextBrush = (StiBrush)new StiSolidBrush(StiOptions.Engine.NegativeColor);
				}
				else if (!string.IsNullOrWhiteSpace(stiText.ComponentStyle) && Styles[stiText.ComponentStyle] is StiStyle { AllowUseNegativeTextBrush: not false } stiStyle)
				{
					stiText.TextBrush = stiStyle.NegativeTextBrush;
				}
			}
		}
		if (stiText != null && stiText.Format != null && stiText.Format != "G" && stiText.Format.Length != 0)
		{
			stiText.ExcelDataValue = ToString(value);
		}
		return value;
	}

	protected internal string ToString(object sender, object obj)
	{
		return ToString(sender, obj, allowExcelCheck: false);
	}

	protected internal string ToString(object sender, object obj, bool allowExcelCheck)
	{
		return ToString(allowExcelCheck ? CheckExcelValue(sender, obj) : obj);
	}

	internal void DisposeCachedRichTextFormat()
	{
		if (cachedRichTextFormat != null)
		{
			((Component)(object)cachedRichTextFormat).Dispose();
			cachedRichTextFormat = null;
		}
	}

	protected string ConvertRtf(params object[] args)
	{
		if (args.Length < 2)
		{
			return StiRichText.PackRtf(args[0] as string);
		}
		if (cachedRichText == null)
		{
			cachedRichText = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
		}
		((Control)cachedRichText).Text = string.Empty;
		bool flag = false;
		for (int i = 0; i < args.Length; i++)
		{
			string text = args[i] as string;
			int length = ((Control)cachedRichText).Text.Length;
			((TextBoxBase)cachedRichText).SelectionStart = length;
			((TextBoxBase)cachedRichText).SelectionLength = 0;
			if (StringExt.StartsWithInvariant(text, "{\\"))
			{
				if (cachedRichTextFormat == null)
				{
					cachedRichTextFormat = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
				}
				cachedRichTextFormat.Rtf = text;
				((TextBoxBase)cachedRichTextFormat).SelectionStart = 0;
				((TextBoxBase)cachedRichTextFormat).SelectionLength = 1;
				if (((TextBoxBase)cachedRichTextFormat).SelectedText == "{")
				{
					flag = true;
				}
				else
				{
					cachedRichText.SelectedRtf = text;
				}
				continue;
			}
			((TextBoxBase)cachedRichText).SelectedText = text;
			((TextBoxBase)cachedRichText).SelectionStart = length;
			((TextBoxBase)cachedRichText).SelectionLength = ((Control)cachedRichText).Text.Length - length;
			if (flag && cachedRichTextFormat != null)
			{
				flag = false;
				(cachedRichText as StiRichTextBox).SelectionAlignment = (cachedRichTextFormat as StiRichTextBox).SelectionAlignment;
				cachedRichText.SelectionBullet = cachedRichTextFormat.SelectionBullet;
				cachedRichText.SelectionCharOffset = cachedRichTextFormat.SelectionCharOffset;
				cachedRichText.SelectionColor = cachedRichTextFormat.SelectionColor;
				cachedRichText.SelectionFont = cachedRichTextFormat.SelectionFont;
				cachedRichText.SelectionHangingIndent = cachedRichTextFormat.SelectionHangingIndent;
				cachedRichText.SelectionIndent = cachedRichTextFormat.SelectionIndent;
				cachedRichText.SelectionProtected = cachedRichTextFormat.SelectionProtected;
				cachedRichText.SelectionRightIndent = cachedRichTextFormat.SelectionRightIndent;
				cachedRichText.SelectionTabs = cachedRichTextFormat.SelectionTabs;
			}
		}
		string text2 = cachedRichText.Rtf;
		try
		{
			string text3 = "\\par\r\n}\r\n";
			if (text2 != null && text2.EndsWith(text3))
			{
				int num = text2.Substring(0, text2.Length - text3.Length).LastIndexOf("\r\n");
				if (num != -1)
				{
					string input = text2.Substring(num + 2);
					Match match = Regex.Match(input, "\\\\f(?'font'\\d{1,2})\\\\fs17\\\\lang\\d{1,4}\\\\par", RegexOptions.Multiline | RegexOptions.Compiled);
					if (!match.Success)
					{
						match = Regex.Match(input, "\\\\f(?'font'\\d{1,2})\\\\fs17\\\\par", RegexOptions.Multiline | RegexOptions.Compiled);
					}
					if (match.Success)
					{
						string value = match.Groups["font"].Value;
						MatchCollection matchCollection = Regex.Matches(text2, "{\\\\f(?'font'\\d{1,2})\\\\fnil\\\\fcharset\\d{1,3} Microsoft Sans Serif;}", RegexOptions.Multiline | RegexOptions.Compiled);
						foreach (Match item in matchCollection)
						{
							if (item.Groups["font"].Value == value)
							{
								text2 = text2.Substring(0, num + 2 + match.Index) + "}\r\n";
								break;
							}
						}
					}
				}
			}
		}
		catch
		{
		}
		return StiRichText.PackRtf(text2);
	}

	public void SetReportSource(string value)
	{
		reportSource = value;
	}

	public string GetReportAssemblyCacheName()
	{
		return GetReportAssemblyCacheName(ReportFile);
	}

	private string GetReportAssemblyCacheName(string reportFile)
	{
		if (!string.IsNullOrEmpty(reportFile))
		{
			return Path.GetFileNameWithoutExtension(reportFile) + "." + GetReportVersion() + "." + ReportGuid + ".dll";
		}
		return GetReportVersion() + "." + ReportGuid + ".dll";
	}

	internal string GenerateReportGuid()
	{
		return Guid.NewGuid().ToString().Replace("-", "");
	}

	public override string ToString()
	{
		if ((Designer?.UseAliases).GetValueOrDefault(false) && !string.IsNullOrWhiteSpace(ReportAlias))
		{
			return ReportAlias;
		}
		if (ReportName == ReportAlias || string.IsNullOrWhiteSpace(ReportAlias))
		{
			return ReportName;
		}
		return ReportName + " [" + ReportAlias + "]";
	}

	protected DateTime ParseDateTime(string value)
	{
		return StiVariable.GetDateTimeFromValue(value);
	}

	protected TimeSpan ParseTimeSpan(string value)
	{
		if (string.IsNullOrWhiteSpace(value) || value == "null")
		{
			return TimeSpan.Zero;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			return TimeSpan.Parse(value);
		}
		catch
		{
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
		return TimeSpan.Zero;
	}

	public virtual StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = new StiEventsCollection();
		if (BeginRenderEvent != null)
		{
			stiEventsCollection.Add(BeginRenderEvent);
		}
		if (EndRenderEvent != null)
		{
			stiEventsCollection.Add(EndRenderEvent);
		}
		if (ExportingEvent != null)
		{
			stiEventsCollection.Add(ExportingEvent);
		}
		if (ExportedEvent != null)
		{
			stiEventsCollection.Add(ExportedEvent);
		}
		if (PrintingEvent != null)
		{
			stiEventsCollection.Add(PrintingEvent);
		}
		if (PrintedEvent != null)
		{
			stiEventsCollection.Add(PrintedEvent);
		}
		if (RenderingEvent != null)
		{
			stiEventsCollection.Add(RenderingEvent);
		}
		return stiEventsCollection;
	}

	public StiComponentsCollection GetComponents()
	{
		StiComponentsCollection comps = new StiComponentsCollection();
		foreach (StiPage page in Pages)
		{
			comps.Add(page);
			page.GetComponents(ref comps);
		}
		return comps;
	}

	public StiComponentsCollection GetRenderedComponents()
	{
		StiComponentsCollection comps = new StiComponentsCollection();
		foreach (StiPage renderedPage in RenderedPages)
		{
			comps.Add(renderedPage);
			renderedPage.GetComponents(ref comps);
		}
		return comps;
	}

	public int GetComponentsCount()
	{
		int num = 0;
		foreach (StiPage page in Pages)
		{
			num++;
			num += page.GetComponentsCount();
		}
		return num;
	}

	public int GetRenderedComponentsCount()
	{
		int num = 0;
		foreach (StiPage renderedPage in RenderedPages)
		{
			num++;
			num += renderedPage.GetComponentsCount();
		}
		return num;
	}

	public void GotoComponent(StiComponent component)
	{
		InvokeGotoComp(new StiGotoCompEventArgs(component));
	}

	public StiComponent FindComponentTag(object tag)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiPage renderedPage in RenderedPages)
		{
			stiComponentsCollection.Add(renderedPage);
			stiComponentsCollection.AddRange(renderedPage.GetComponents());
		}
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item.TagValue is string && tag is string)
			{
				if (item.TagValue.ToString() == tag.ToString())
				{
					return item;
				}
			}
			else if (item.TagValue == tag)
			{
				return item;
			}
		}
		return null;
	}

	public StiComponent FindComponentBookmark(object bookmark)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiPage renderedPage in RenderedPages)
		{
			stiComponentsCollection.Add(renderedPage);
			stiComponentsCollection.AddRange(renderedPage.GetComponents());
		}
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item.BookmarkValue is string && bookmark is string)
			{
				if (item.BookmarkValue.ToString() == bookmark.ToString())
				{
					return item;
				}
			}
			else if (item.TagValue == bookmark)
			{
				return item;
			}
		}
		return null;
	}

	public void GotoComponentTag(object tag)
	{
		StiComponent stiComponent = FindComponentTag(tag);
		if (stiComponent != null)
		{
			GotoComponent(stiComponent);
		}
	}

	public void GotoComponentBookmark(object tag)
	{
		StiComponent stiComponent = FindComponentBookmark(tag);
		if (stiComponent != null)
		{
			GotoComponent(stiComponent);
		}
	}

	public void Invalidate(StiComponent comp = null)
	{
		InvokePaint(comp, EventArgs.Empty);
	}

	public void AddAnchor(object value)
	{
		Anchors[value] = new StiRuntimeVariables(this);
	}

	public void AddAnchor(object value, object component)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)EngineVersion == 1)
		{
			Anchors[value] = new StiRuntimeVariables(this);
		}
		else if (!Anchors.ContainsKey(value))
		{
			Anchors[value] = new KeyValuePair<StiRuntimeVariables, object>(new StiRuntimeVariables(this), component);
			if (component is StiComponent { Guid: null } stiComponent)
			{
				stiComponent.Guid = value as string;
			}
		}
	}

	public int GetAnchorPageNumber(object value)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Invalid comparison between Unknown and I4
		object obj = ((value != null) ? Anchors[value] : null);
		if ((int)EngineVersion == 2 && obj is KeyValuePair<StiRuntimeVariables, object> keyValuePair)
		{
			int num = 0;
			if (keyValuePair.Key != null)
			{
				num = Engine.PageNumbers.GetPageNumber(keyValuePair.Key.Page);
			}
			for (int i = num - 1; i < RenderedPages.Count; i++)
			{
				StiPage stiPage = RenderedPages[i];
				RenderedPages.GetPage(stiPage);
				StiComponentsCollection components = stiPage.GetComponents();
				foreach (StiComponent item in components)
				{
					if (item == keyValuePair.Value)
					{
						if (stiPage == keyValuePair.Key.Page)
						{
							return num;
						}
						return Engine.PageNumbers.GetPageNumber(stiPage);
					}
				}
			}
			return num;
		}
		StiRuntimeVariables anchor = GetAnchor(value);
		if (anchor == null)
		{
			return 0;
		}
		if ((int)EngineVersion != 1)
		{
			return Engine.PageNumbers.GetPageNumber(anchor.Page);
		}
		return anchor.Page.PageInfoV1.PageNumber;
	}

	public int GetAnchorPageNumberThrough(object value)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Invalid comparison between Unknown and I4
		object obj = Anchors[value];
		if ((int)EngineVersion == 2 && obj is KeyValuePair<StiRuntimeVariables, object> keyValuePair)
		{
			int num = 0;
			if (keyValuePair.Key != null)
			{
				num = Engine.PageNumbers.GetPageNumberThrough(keyValuePair.Key.Page);
			}
			for (int i = num - 1; i < RenderedPages.Count; i++)
			{
				StiPage stiPage = RenderedPages[i];
				RenderedPages.GetPage(stiPage);
				StiComponentsCollection components = stiPage.GetComponents();
				foreach (StiComponent item in components)
				{
					if (item == keyValuePair.Value)
					{
						return (stiPage == keyValuePair.Key.Page) ? num : Engine.PageNumbers.GetPageNumberThrough(stiPage);
					}
				}
			}
			return num;
		}
		StiRuntimeVariables anchor = GetAnchor(value);
		if (anchor == null)
		{
			return 0;
		}
		if ((int)EngineVersion != 1)
		{
			return Engine.PageNumbers.GetPageNumberThrough(anchor.Page);
		}
		return anchor.Page.PageInfoV1.PageNumber;
	}

	public int GetTotalRenderedPageCount()
	{
		int num = 0;
		foreach (StiPage renderedPage in RenderedPages)
		{
			num += renderedPage.SegmentPerWidth * renderedPage.SegmentPerHeight;
		}
		return num;
	}

	public StiRuntimeVariables GetAnchor(object value)
	{
		if (value == null)
		{
			return new StiRuntimeVariables(this);
		}
		return Anchors[value] as StiRuntimeVariables;
	}

	internal int GetPageIndex(int pageNumber)
	{
		int x;
		int y;
		return GetPageIndex(pageNumber, out x, out y);
	}

	public int GetPageIndex(int pageNumber, out int x, out int y)
	{
		int num = 0;
		StiPagesCollection stiPagesCollection = null;
		stiPagesCollection = ((!IsDesigning) ? RenderedPages : Pages);
		pageNumber--;
		int num2 = 0;
		foreach (StiPage item in stiPagesCollection)
		{
			int num3 = num2;
			int num4 = Math.Max(item.SegmentPerWidth, 1);
			int num5 = Math.Max(item.SegmentPerHeight, 1);
			int num6 = num2 + num4 * num5;
			if (pageNumber >= num3 && pageNumber < num6)
			{
				pageNumber -= num3;
				y = pageNumber / num4;
				x = pageNumber - y * num4;
				return num;
			}
			num2 = num6;
			num++;
		}
		x = (y = 0);
		return -1;
	}

	public int GetPageNumber(int index)
	{
		int startPageNumber = 0;
		int endPageNumber = 0;
		GetPageNumber(index, out startPageNumber, out endPageNumber);
		return startPageNumber;
	}

	public void GetPageNumber(int index, out int startPageNumber, out int endPageNumber)
	{
		int num = 1;
		int num2 = 0;
		startPageNumber = 0;
		endPageNumber = 0;
		StiPagesCollection stiPagesCollection = (IsDesigning ? Pages : RenderedPages);
		foreach (StiPage item in stiPagesCollection)
		{
			if (num2 == index)
			{
				startPageNumber = num;
				endPageNumber = num + item.SegmentPerWidth * item.SegmentPerHeight - 1;
				break;
			}
			num += item.SegmentPerWidth * item.SegmentPerHeight;
			num2++;
		}
	}

	public static void ClearReportCache()
	{
		StiReportCache.ClearCache();
	}

	public static void ClearImageCache()
	{
		StiFileImageCacheV2.ClearCache();
	}

	public static StiReport GetReport()
	{
		return StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
	}

	private bool ShouldSerializePrinterSettings()
	{
		if (PrinterSettings != null)
		{
			return !PrinterSettings.IsDefault;
		}
		return true;
	}

	public StiReport()
	{
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Key = StiKeyHelper.GenerateKey();
			SubReports = new StiReportsCollection(this);
			engineVersion = StiOptions.Engine.DefaultEngineVersion;
			ReportGuid = GenerateReportGuid();
			ReportDataSources = new StiReportDataSourceCollection(this);
			Info = new StiDesignerInfo(this);
			try
			{
				if (StiOptions.Engine.DefaultUnit is StiReportUnitType)
				{
					ReportUnit = (StiReportUnitType)StiOptions.Engine.DefaultUnit;
				}
				else
				{
					ReportUnit = ((!RegionInfo.CurrentRegion.IsMetric) ? StiReportUnitType.Inches : StiReportUnitType.Centimeters);
				}
			}
			catch
			{
			}
			Dictionary = new StiDictionary(this);
			Styles = new StiStylesCollection(this);
			GlobalizationStrings = new StiGlobalizationContainerCollection(this);
			try
			{
				Pages = new StiPagesCollection(this);
				renderedPages = new StiPagesCollection(this);
			}
			catch
			{
			}
			ReferencedAssemblies = StiOptions.Engine.ReferencedAssemblies ?? new string[9] { "System.Dll", "System.Drawing.Dll", "System.Windows.Forms.Dll", "System.Data.Dll", "System.Xml.Dll", "Stimulsoft.Controls.Dll", "Stimulsoft.Base.Dll", "Stimulsoft.Data.Dll", "Stimulsoft.Report.Dll" };
			StiPage stiPage = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage", new object[1] { this }) as StiPage;
			try
			{
				if (!RegionInfo.CurrentRegion.IsMetric)
				{
					stiPage.PaperSize = (PaperKind)1;
					stiPage.PageWidth = Unit.ConvertFromHInches(850.0);
					stiPage.PageHeight = Unit.ConvertFromHInches(1100.0);
				}
			}
			catch
			{
			}
			Pages.Add(stiPage);
			stiPage.Name = StiNameCreation.GenerateName(stiPage) + "1";
			RenderedPages.Add(stiPage);
			try
			{
				StiSettings.Load();
				bool flag = StiSettings.GetBool("StiDesigner", "GenerateLocalizedName");
				if (StiLocalization.CultureName == "en")
				{
					flag = false;
				}
				else if (StiOptions.Engine.ForceGenerationNonLocalizedName)
				{
					flag = false;
				}
				if (StiOptions.Engine.ForceGenerationLocalizedName)
				{
					flag = true;
				}
				if (flag)
				{
					PropName = StiLocalization.Get("Components", "StiReport");
					ReportAlias = StiLocalization.Get("Components", "StiReport");
				}
				else
				{
					PropName = "Report";
					ReportAlias = "Report";
				}
			}
			catch
			{
				PropName = "Report";
				ReportAlias = "Report";
			}
			EngineV1 = new StiEngineV1(this);
			pointerValue = StiBookmarksV1Helper.CreateBookmark(ReportAlias, this);
			bookmarkValue = StiBookmarksV1Helper.CreateBookmark(ReportAlias, this);
			manualBookmark = StiBookmarksV1Helper.CreateBookmark(ReportAlias, this);
			scriptLanguage = StiOptions.Engine.DefaultReportLanguage;
			NeedsCompiling = true;
		}
		catch
		{
		}
	}

	static StiReport()
	{
		lockGlobalMeasureGraphics = new object();
		objLockGlobalRichTextMeasureGraphics = new object();
		if (!StiOptions.Configuration.IsWeb)
		{
			StiOptions.Localization.TryLoadDefault();
		}
		StiFunctionsXmlParser.TryLoadDefaultFunctions();
		StiDpiHelper.CheckWysiwygScaling();
	}

	public DialogResult Design()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		DialogResult result = Design(dialogForm: true);
		ResetRenderedState();
		return result;
	}

	public DialogResult Design(bool dialogForm)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return Design(null, dialogForm, null);
	}

	public DialogResult Design(bool dialogForm, IWin32Window win32Window)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return Design(null, dialogForm, win32Window);
	}

	public DialogResult Design(Form parentForm)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return Design(parentForm, dialogForm: false, null);
	}

	private DialogResult Design(Form parentForm, bool dialogForm, IWin32Window win32Window)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (!StiGuiOptions.AllowWinDesigner)
		{
			if (StiGuiOptions.AllowWpfDesigner)
			{
				if (DesignWithWpf(dialogWindow: true))
				{
					return (DialogResult)1;
				}
				return (DialogResult)2;
			}
			if (StiGuiOptions.AllowWpfV2Designer)
			{
				if (DesignV2WithWpf(dialogWindow: true))
				{
					return (DialogResult)1;
				}
				return (DialogResult)2;
			}
		}
		UpdateInheritedReport();
		IsRendered = false;
		DialogResult result = StartDesigner(parentForm, dialogForm, win32Window);
		ResetRenderedState();
		return result;
	}

	private DialogResult StartDesigner(Form parentForm, bool dialogForm, IWin32Window win32Window)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiLogService.Write(GetType(), "Starting report designer");
			RegReportDataSources();
			IStiDesigner stiDesigner = PrepareForDesign();
			Pages.SortByPriority();
			if (parentForm == null)
			{
				if (dialogForm)
				{
					stiDesigner.ShowInTaskbar = StiOptions.Designer.ShowDesignerInTaskbar;
					stiDesigner.DesignerControl.IsModalMode = true;
					if (win32Window == null)
					{
						return stiDesigner.ShowDialogDesigner();
					}
					return stiDesigner.ShowDialogDesigner(win32Window);
				}
				stiDesigner.ShowInTaskbar = StiOptions.Designer.ShowDesignerInTaskbar;
				if (win32Window == null)
				{
					stiDesigner.ShowDesigner();
				}
				else
				{
					stiDesigner.ShowDesigner(win32Window);
				}
				return (DialogResult)1;
			}
			stiDesigner.TopLevel = false;
			stiDesigner.MdiParent = parentForm;
			stiDesigner.ShowInTaskbar = StiOptions.Designer.ShowDesignerInTaskbar;
			stiDesigner.ShowDesigner();
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Starting report designer...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return (DialogResult)0;
	}

	internal IStiDesigner PrepareForDesign()
	{
		string typeName = "Stimulsoft.Report.Design.StiRibbonDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a";
		Type type = Type.GetType(typeName);
		if (type == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Design' is not found!");
		}
		Designer = StiActivator.CreateObject(type, new object[1] { this }) as IStiDesigner;
		StiDictionary.DoAutoSynchronize(this);
		return Designer as IStiDesigner;
	}

	public bool DesignWithWpf()
	{
		return DesignWithWpf(dialogWindow: true);
	}

	public bool DesignWithWpf(bool dialogWindow)
	{
		return DesignWithWpf(null, dialogWindow);
	}

	public bool DesignWithWpf(object ownerWindow, bool dialogWindow)
	{
		if (!StiGuiOptions.AllowWpfDesigner && StiGuiOptions.AllowWpfV2Designer)
		{
			return DesignV2WithWpf(dialogWindow);
		}
		UpdateInheritedReport();
		IsRendered = false;
		bool result = StartDesignerWithWpf(ownerWindow, dialogWindow);
		ResetRenderedState();
		return result;
	}

	private bool StartDesignerWithWpf(object ownerWindow, bool dialogWindow)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		try
		{
			StiLogService.Write(GetType(), "Starting report designer");
			StiOptions.Configuration.IsWPF = true;
			RegReportDataSources();
			IStiDesigner stiDesigner = PrepareForDesignWithWpf();
			Pages.SortByPriority();
			if (dialogWindow)
			{
				if (stiDesigner.ShowInTaskbar != StiOptions.Designer.ShowDesignerInTaskbar)
				{
					stiDesigner.ShowInTaskbar = StiOptions.Designer.ShowDesignerInTaskbar;
				}
				stiDesigner.DesignerControl.IsModalMode = true;
				return (int)stiDesigner.ShowDialogDesigner(ownerWindow) == 1;
			}
			if (stiDesigner.ShowInTaskbar != StiOptions.Designer.ShowDesignerInTaskbar)
			{
				stiDesigner.ShowInTaskbar = StiOptions.Designer.ShowDesignerInTaskbar;
			}
			stiDesigner.ShowDesigner(ownerWindow);
			return true;
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Starting report designer...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return false;
	}

	internal IStiDesigner PrepareForDesignWithWpf()
	{
		Type type = Type.GetType("Stimulsoft.Report.WpfDesign.StiWpfDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.WpfDesign' is not found!");
		}
		Designer = StiActivator.CreateObject(type, new object[1] { this }) as IStiDesigner;
		StiDictionary.DoAutoSynchronize(this);
		return Designer as IStiDesigner;
	}

	public bool DesignV2WithWpf()
	{
		return DesignV2WithWpf(null, dialogWindow: true);
	}

	public bool DesignV2WithWpf(object ownerWindow)
	{
		return DesignV2WithWpf(ownerWindow, dialogWindow: true);
	}

	public bool DesignV2WithWpf(bool dialogWindow)
	{
		return DesignV2WithWpf(null, dialogWindow);
	}

	public bool DesignV2WithWpf(object ownerWindow, bool dialogWindow)
	{
		UpdateInheritedReport();
		IsRendered = false;
		bool result = StartDesignerV2WithWpf(ownerWindow, dialogWindow);
		ResetRenderedState();
		return result;
	}

	private bool StartDesignerV2WithWpf(object ownerWindow, bool dialogForm)
	{
		try
		{
			StiLogService.Write(GetType(), "Starting report designerV2");
			StiOptions.Configuration.IsWPF = true;
			RegReportDataSources();
			IStiDeignerLauncher val = PrepareForDesignV2WithWpf();
			Pages.SortByPriority();
			val.Launch(ownerWindow, (object)this, dialogForm);
			return true;
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Starting report designerV2...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return false;
	}

	internal IStiDeignerLauncher PrepareForDesignV2WithWpf()
	{
		Type type = Type.GetType("Stimulsoft.Client.Designer.StiDesigner, Stimulsoft.Client.Designer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Client.Designer' is not found!");
		}
		object obj = StiActivator.CreateObject(type, new object[0]);
		IStiDeignerLauncher result = (IStiDeignerLauncher)((obj is IStiDeignerLauncher) ? obj : null);
		StiDictionary.DoAutoSynchronize(this);
		return result;
	}

	public async Task<StiReport> ExportDocumentAsync(StiExportService service, string path)
	{
		return await Task.Run(() => ExportDocument(service, path));
	}

	public StiReport ExportDocument(StiExportService service, string path)
	{
		return ExportDocument(service, path, sendEMail: false);
	}

	public async Task<StiReport> ExportDocumentAsync(StiExportService service, string path, bool sendEMail)
	{
		return await Task.Run(() => ExportDocument(service, path, sendEMail));
	}

	public StiReport ExportDocument(StiExportService service, string path, bool sendEMail)
	{
		try
		{
			StiLogService.Write(typeof(StiReport), "Exporting rendered report");
			service.Export(this, path, sendEMail);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Exporting rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return this;
	}

	public async Task<StiReport> ExportDocumentAsync(StiExportFormat exportFormat, string file)
	{
		return await Task.Run(() => ExportDocument(exportFormat, file));
	}

	public StiReport ExportDocument(StiExportFormat exportFormat, string file)
	{
		return ExportDocument(exportFormat, file, null);
	}

	public async Task<StiReport> ExportDocumentAsync(StiExportFormat exportFormat, string file, StiExportSettings settings)
	{
		return await Task.Run(() => ExportDocument(exportFormat, file, settings));
	}

	public StiReport ExportDocument(StiExportFormat exportFormat, string file, StiExportSettings settings)
	{
		if (RenderedPages.Count > 1 && (exportFormat == StiExportFormat.ImageBmp || exportFormat == StiExportFormat.ImageEmf || exportFormat == StiExportFormat.ImageGif || exportFormat == StiExportFormat.ImageJpeg || exportFormat == StiExportFormat.ImagePcx || exportFormat == StiExportFormat.ImagePng || exportFormat == StiExportFormat.ImageSvg || exportFormat == StiExportFormat.ImageSvgz || exportFormat == StiExportFormat.ImageTiff))
		{
			string text = file;
			string text2 = string.Empty;
			if (Path.HasExtension(file))
			{
				text2 = Path.GetExtension(file).Substring(1);
				text = Path.ChangeExtension(file, "");
				text = text.Substring(0, text.Length - 1);
			}
			string orderFileName = StiExportService.GetOrderFileName(null, 1, RenderedPages.Count, null);
			if (!text.EndsWith(orderFileName))
			{
				file = text + orderFileName + "." + text2;
			}
		}
		StiFileUtils.ProcessReadOnly(file);
		FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);
		ExportDocument(exportFormat, fileStream, settings);
		fileStream.Close();
		return this;
	}

	public async Task<StiReport> ExportDocumentAsync(StiExportFormat exportFormat, Stream stream)
	{
		return await Task.Run(() => ExportDocument(exportFormat, stream));
	}

	public StiReport ExportDocument(StiExportFormat exportFormat, Stream stream)
	{
		return ExportDocument(exportFormat, stream, null);
	}

	public async Task<StiReport> ExportDocumentAsync(StiExportFormat exportFormat, Stream stream, StiExportSettings settings)
	{
		return await Task.Run(() => ExportDocument(exportFormat, stream, settings));
	}

	public StiReport ExportDocument(StiExportFormat exportFormat, Stream stream, StiExportSettings settings)
	{
		return ExportDocument(exportFormat, null, stream, settings);
	}

	public StiReport ExportDocument(StiExportFormat exportFormat, StiExportService exportService, Stream stream, StiExportSettings settings)
	{
		InvokeExporting(exportFormat);
		IStiDesignerBase designer = Designer;
		bool forceDesigningMode = Info.ForceDesigningMode;
		bool isPageDesigner = IsPageDesigner;
		Designer = null;
		Info.ForceDesigningMode = false;
		IsPageDesigner = false;
		try
		{
			StiLogService.Write(typeof(StiReport), "Exporting rendered report");
			StiProcessExportEventArgs stiProcessExportEventArgs = new StiProcessExportEventArgs(exportFormat, exportService, stream, settings);
			StiOptions.Engine.GlobalEvents.InvokeProcessExport(this, stiProcessExportEventArgs);
			if (!stiProcessExportEventArgs.Processed)
			{
				switch (exportFormat)
				{
				case StiExportFormat.Csv:
				{
					if (exportService != null && !(exportService is StiCsvExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiCsvExportService stiCsvExportService = exportService as StiCsvExportService;
					if (stiCsvExportService == null)
					{
						stiCsvExportService = new StiCsvExportService();
					}
					if (settings == null)
					{
						settings = new StiCsvExportSettings();
					}
					if (!(settings is StiCsvExportSettings) && (!(settings is StiDataExportSettings) || (settings as StiDataExportSettings).DataType != Stimulsoft.Report.Export.StiDataType.Csv))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiCsvExportSettings is need to be used.");
					}
					stiCsvExportService.ExportCsv(this, stream, settings as StiDataExportSettings);
					break;
				}
				case StiExportFormat.Data:
				{
					if (exportService != null && !(exportService is StiDataExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiDataExportService stiDataExportService = exportService as StiDataExportService;
					if (stiDataExportService == null)
					{
						stiDataExportService = new StiDataExportService();
					}
					if (settings == null)
					{
						settings = new StiDataExportSettings();
					}
					if (!(settings is StiDataExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiCsvExportSettings is need to be used.");
					}
					stiDataExportService.ExportTo(this, stream, settings as StiDataExportSettings);
					break;
				}
				case StiExportFormat.Dif:
				{
					if (exportService != null && !(exportService is StiDifExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiDifExportService stiDifExportService = exportService as StiDifExportService;
					if (stiDifExportService == null)
					{
						stiDifExportService = new StiDifExportService();
					}
					if (settings == null)
					{
						settings = new StiDifExportSettings();
					}
					if (!(settings is StiDifExportSettings) && (!(settings is StiDataExportSettings) || (settings as StiDataExportSettings).DataType != Stimulsoft.Report.Export.StiDataType.Dif))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiDifExportSettings is need to be used.");
					}
					stiDifExportService.ExportDif(this, stream, settings as StiDataExportSettings);
					break;
				}
				case StiExportFormat.Sylk:
				{
					if (exportService != null && !(exportService is StiSylkExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiSylkExportService stiSylkExportService = exportService as StiSylkExportService;
					if (stiSylkExportService == null)
					{
						stiSylkExportService = new StiSylkExportService();
					}
					if (settings == null)
					{
						settings = new StiSylkExportSettings();
					}
					if (!(settings is StiSylkExportSettings) && (!(settings is StiDataExportSettings) || (settings as StiDataExportSettings).DataType != Stimulsoft.Report.Export.StiDataType.Sylk))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiSylkExportSettings is need to be used.");
					}
					stiSylkExportService.ExportSylk(this, stream, settings as StiDataExportSettings);
					break;
				}
				case StiExportFormat.Excel:
				{
					if (exportService != null && !(exportService is StiExcelExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiExcelExportService stiExcelExportService = exportService as StiExcelExportService;
					if (stiExcelExportService == null)
					{
						stiExcelExportService = new StiExcelExportService();
					}
					if (settings == null)
					{
						settings = new StiExcelExportSettings();
					}
					if (!(settings is StiExcelExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiExcelExportSettings is need to be used.");
					}
					stiExcelExportService.ExportExcel(this, stream, settings as StiExcelExportSettings);
					break;
				}
				case StiExportFormat.ExcelXml:
				{
					if (exportService != null && !(exportService is StiExcelXmlExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiExcelXmlExportService stiExcelXmlExportService = exportService as StiExcelXmlExportService;
					if (stiExcelXmlExportService == null)
					{
						stiExcelXmlExportService = new StiExcelXmlExportService();
					}
					if (settings == null)
					{
						settings = new StiExcelXmlExportSettings();
					}
					if (!(settings is StiExcelXmlExportSettings) && (!(settings is StiExcelExportSettings) || (settings as StiExcelExportSettings).ExcelType != StiExcelType.ExcelXml))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiExcelXmlExportSettings is need to be used.");
					}
					stiExcelXmlExportService.ExportExcel(this, stream, settings as StiExcelExportSettings);
					break;
				}
				case StiExportFormat.Excel2007:
				{
					if (exportService != null && !(exportService is StiExcel2007ExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiExcel2007ExportService stiExcel2007ExportService = exportService as StiExcel2007ExportService;
					if (stiExcel2007ExportService == null)
					{
						stiExcel2007ExportService = new StiExcel2007ExportService();
					}
					if (settings == null)
					{
						settings = new StiExcel2007ExportSettings();
					}
					if (!(settings is StiExcel2007ExportSettings) && (!(settings is StiExcelExportSettings) || (settings as StiExcelExportSettings).ExcelType != StiExcelType.Excel2007))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiExcel2007ExportSettings is need to be used.");
					}
					stiExcel2007ExportService.ExportExcel(this, stream, settings as StiExcelExportSettings);
					break;
				}
				case StiExportFormat.Word2007:
				{
					if (exportService != null && !(exportService is StiWord2007ExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiWord2007ExportService stiWord2007ExportService = exportService as StiWord2007ExportService;
					if (stiWord2007ExportService == null)
					{
						stiWord2007ExportService = new StiWord2007ExportService();
					}
					if (settings == null)
					{
						settings = new StiWord2007ExportSettings();
					}
					if (!(settings is StiWord2007ExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiWord2007ExportSettings is need to be used.");
					}
					stiWord2007ExportService.ExportWord(this, stream, settings as StiWord2007ExportSettings);
					break;
				}
				case StiExportFormat.Dbf:
				{
					if (exportService != null && !(exportService is StiDbfExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiDbfExportService stiDbfExportService = exportService as StiDbfExportService;
					if (stiDbfExportService == null)
					{
						stiDbfExportService = new StiDbfExportService();
					}
					if (settings == null)
					{
						settings = new StiDbfExportSettings();
					}
					if (!(settings is StiDbfExportSettings) && (!(settings is StiDataExportSettings) || (settings as StiDataExportSettings).DataType != Stimulsoft.Report.Export.StiDataType.Dbf))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiDbfExportSettings is need to be used.");
					}
					stiDbfExportService.ExportDbf(this, stream, settings as StiDbfExportSettings);
					break;
				}
				case StiExportFormat.Mht:
				{
					if (exportService != null && !(exportService is StiMhtExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiMhtExportService stiMhtExportService = exportService as StiMhtExportService;
					if (stiMhtExportService == null)
					{
						stiMhtExportService = new StiMhtExportService();
					}
					if (settings == null)
					{
						settings = new StiMhtExportSettings();
					}
					if (!(settings is StiMhtExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiMhtExportSettings is need to be used.");
					}
					stiMhtExportService.ExportMht(this, stream, settings as StiMhtExportSettings);
					break;
				}
				case StiExportFormat.HtmlSpan:
				{
					if (exportService != null && !(exportService is StiHtmlExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiHtmlExportService stiHtmlExportService3 = exportService as StiHtmlExportService;
					if (stiHtmlExportService3 == null)
					{
						stiHtmlExportService3 = new StiHtmlExportService();
					}
					if (settings == null)
					{
						settings = new StiHtmlExportSettings();
					}
					if (!(settings is StiHtmlExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiHtmlExportSettings is need to be used.");
					}
					((StiHtmlExportSettings)settings).ExportMode = StiHtmlExportMode.Span;
					stiHtmlExportService3.ExportHtml(this, stream, settings as StiHtmlExportSettings);
					break;
				}
				case StiExportFormat.HtmlDiv:
				{
					if (exportService != null && !(exportService is StiHtmlExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiHtmlExportService stiHtmlExportService = exportService as StiHtmlExportService;
					if (stiHtmlExportService == null)
					{
						stiHtmlExportService = new StiHtmlExportService();
					}
					if (settings == null)
					{
						settings = new StiHtmlExportSettings();
					}
					if (!(settings is StiHtmlExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiHtmlExportSettings is need to be used.");
					}
					((StiHtmlExportSettings)settings).ExportMode = StiHtmlExportMode.Div;
					stiHtmlExportService.ExportHtml(this, stream, settings as StiHtmlExportSettings);
					break;
				}
				case StiExportFormat.HtmlTable:
				case StiExportFormat.Html:
				{
					if (exportService != null && !(exportService is StiHtmlExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiHtmlExportService stiHtmlExportService2 = exportService as StiHtmlExportService;
					if (stiHtmlExportService2 == null)
					{
						stiHtmlExportService2 = new StiHtmlExportService();
					}
					if (settings == null)
					{
						settings = new StiHtmlExportSettings();
					}
					if (!(settings is StiHtmlExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiHtmlExportSettings is need to be used.");
					}
					if (exportFormat != StiExportFormat.Html)
					{
						((StiHtmlExportSettings)settings).ExportMode = StiHtmlExportMode.Table;
					}
					stiHtmlExportService2.ExportHtml(this, stream, settings as StiHtmlExportSettings);
					break;
				}
				case StiExportFormat.Html5:
				{
					if (exportService != null && !(exportService is StiHtml5ExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiHtml5ExportService stiHtml5ExportService = exportService as StiHtml5ExportService;
					if (stiHtml5ExportService == null)
					{
						stiHtml5ExportService = new StiHtml5ExportService();
					}
					if (settings == null)
					{
						settings = new StiHtml5ExportSettings();
					}
					if (!(settings is StiHtml5ExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiHtml5ExportSettings is need to be used.");
					}
					stiHtml5ExportService.ExportHtml(this, stream, settings as StiHtml5ExportSettings);
					break;
				}
				case StiExportFormat.Image:
				{
					if (exportService != null && !(exportService is StiImageExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiImageExportService stiImageExportService = exportService as StiImageExportService;
					if (stiImageExportService == null)
					{
						stiImageExportService = new StiImageExportService();
					}
					if (settings == null)
					{
						settings = new StiImageExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiImageExportSettings is need to be used.");
					}
					stiImageExportService.ExportTo(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImageBmp:
				{
					if (exportService != null && !(exportService is StiBmpExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiBmpExportService stiBmpExportService = exportService as StiBmpExportService;
					if (stiBmpExportService == null)
					{
						stiBmpExportService = new StiBmpExportService();
					}
					if (settings == null)
					{
						settings = new StiBmpExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiBmpExportSettings is need to be used.");
					}
					stiBmpExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImageEmf:
				{
					if (exportService != null && !(exportService is StiEmfExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiEmfExportService stiEmfExportService = exportService as StiEmfExportService;
					if (stiEmfExportService == null)
					{
						stiEmfExportService = new StiEmfExportService();
					}
					if (settings == null)
					{
						settings = new StiEmfExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiEmfExportSettings is need to be used.");
					}
					stiEmfExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImageSvg:
				{
					if (exportService != null && !(exportService is StiSvgExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiSvgExportService stiSvgExportService = exportService as StiSvgExportService;
					if (stiSvgExportService == null)
					{
						stiSvgExportService = new StiSvgExportService();
					}
					if (settings == null)
					{
						settings = new StiSvgExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiSvgExportSettings is need to be used.");
					}
					stiSvgExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImageSvgz:
				{
					if (exportService != null && !(exportService is StiSvgzExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiSvgzExportService stiSvgzExportService = exportService as StiSvgzExportService;
					if (stiSvgzExportService == null)
					{
						stiSvgzExportService = new StiSvgzExportService();
					}
					if (settings == null)
					{
						settings = new StiSvgzExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiSvgzExportSettings is need to be used.");
					}
					stiSvgzExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImageGif:
				{
					if (exportService != null && !(exportService is StiGifExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiGifExportService stiGifExportService = exportService as StiGifExportService;
					if (stiGifExportService == null)
					{
						stiGifExportService = new StiGifExportService();
					}
					if (settings == null)
					{
						settings = new StiGifExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiGifExportSettings is need to be used.");
					}
					stiGifExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImageJpeg:
				{
					if (exportService != null && !(exportService is StiJpegExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiJpegExportService stiJpegExportService = exportService as StiJpegExportService;
					if (stiJpegExportService == null)
					{
						stiJpegExportService = new StiJpegExportService();
					}
					if (settings == null)
					{
						settings = new StiJpegExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiJpegExportSettings is need to be used.");
					}
					stiJpegExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImagePng:
				{
					if (exportService != null && !(exportService is StiPngExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiPngExportService stiPngExportService = exportService as StiPngExportService;
					if (stiPngExportService == null)
					{
						stiPngExportService = new StiPngExportService();
					}
					if (settings == null)
					{
						settings = new StiPngExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiPngExportSettings is need to be used.");
					}
					stiPngExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImageTiff:
				{
					if (exportService != null && !(exportService is StiTiffExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiTiffExportService stiTiffExportService = exportService as StiTiffExportService;
					if (stiTiffExportService == null)
					{
						stiTiffExportService = new StiTiffExportService();
					}
					if (settings == null)
					{
						settings = new StiTiffExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiTiffExportSettings is need to be used.");
					}
					stiTiffExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.ImagePcx:
				{
					if (exportService != null && !(exportService is StiPcxExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiPcxExportService stiPcxExportService = exportService as StiPcxExportService;
					if (stiPcxExportService == null)
					{
						stiPcxExportService = new StiPcxExportService();
					}
					if (settings == null)
					{
						settings = new StiPcxExportSettings();
					}
					if (!(settings is StiImageExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiPcxExportSettings is need to be used.");
					}
					stiPcxExportService.ExportImage(this, stream, settings as StiImageExportSettings);
					break;
				}
				case StiExportFormat.Pdf:
				{
					if (exportService != null && !(exportService is StiPdfExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiPdfExportService stiPdfExportService = exportService as StiPdfExportService;
					if (stiPdfExportService == null)
					{
						stiPdfExportService = new StiPdfExportService();
					}
					if (settings == null)
					{
						settings = new StiPdfExportSettings();
					}
					if (!(settings is StiPdfExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiPdfExportSettings is need to be used.");
					}
					stiPdfExportService.ExportPdf(this, stream, settings as StiPdfExportSettings);
					break;
				}
				case StiExportFormat.Xps:
				{
					if (exportService != null && !(exportService is StiXpsExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiXpsExportService stiXpsExportService = exportService as StiXpsExportService;
					if (stiXpsExportService == null)
					{
						stiXpsExportService = new StiXpsExportService();
					}
					if (settings == null)
					{
						settings = new StiXpsExportSettings();
					}
					if (!(settings is StiXpsExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiXpsExportSettings is need to be used.");
					}
					stiXpsExportService.ExportXps(this, stream, settings as StiXpsExportSettings);
					break;
				}
				case StiExportFormat.Rtf:
				case StiExportFormat.RtfTable:
				{
					if (exportService != null && !(exportService is StiRtfExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiRtfExportService stiRtfExportService4 = exportService as StiRtfExportService;
					if (stiRtfExportService4 == null)
					{
						stiRtfExportService4 = new StiRtfExportService();
					}
					if (settings == null)
					{
						settings = new StiRtfExportSettings();
					}
					if (!(settings is StiRtfExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiRtfExportSettings is need to be used.");
					}
					if (exportFormat != StiExportFormat.Rtf)
					{
						((StiRtfExportSettings)settings).ExportMode = StiRtfExportMode.Table;
					}
					stiRtfExportService4.ExportRtf(this, stream, settings as StiRtfExportSettings);
					break;
				}
				case StiExportFormat.RtfFrame:
				{
					if (exportService != null && !(exportService is StiRtfExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiRtfExportService stiRtfExportService2 = exportService as StiRtfExportService;
					if (stiRtfExportService2 == null)
					{
						stiRtfExportService2 = new StiRtfExportService();
					}
					if (settings == null)
					{
						settings = new StiRtfExportSettings();
					}
					if (!(settings is StiRtfExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiRtfExportSettings is need to be used.");
					}
					((StiRtfExportSettings)settings).ExportMode = StiRtfExportMode.Frame;
					stiRtfExportService2.ExportRtf(this, stream, settings as StiRtfExportSettings);
					break;
				}
				case StiExportFormat.RtfWinWord:
				{
					if (exportService != null && !(exportService is StiRtfExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiRtfExportService stiRtfExportService = exportService as StiRtfExportService;
					if (stiRtfExportService == null)
					{
						stiRtfExportService = new StiRtfExportService();
					}
					if (settings == null)
					{
						settings = new StiRtfExportSettings();
					}
					if (!(settings is StiRtfExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiRtfExportSettings is need to be used.");
					}
					((StiRtfExportSettings)settings).ExportMode = StiRtfExportMode.WinWord;
					stiRtfExportService.ExportRtf(this, stream, settings as StiRtfExportSettings);
					break;
				}
				case StiExportFormat.RtfTabbedText:
				{
					if (exportService != null && !(exportService is StiRtfExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiRtfExportService stiRtfExportService3 = exportService as StiRtfExportService;
					if (stiRtfExportService3 == null)
					{
						stiRtfExportService3 = new StiRtfExportService();
					}
					if (settings == null)
					{
						settings = new StiRtfExportSettings();
					}
					if (!(settings is StiRtfExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiRtfExportSettings is need to be used.");
					}
					((StiRtfExportSettings)settings).ExportMode = StiRtfExportMode.TabbedText;
					stiRtfExportService3.ExportRtf(this, stream, settings as StiRtfExportSettings);
					break;
				}
				case StiExportFormat.Text:
				{
					if (exportService != null && !(exportService is StiTxtExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiTxtExportService stiTxtExportService = exportService as StiTxtExportService;
					if (stiTxtExportService == null)
					{
						stiTxtExportService = new StiTxtExportService();
					}
					if (settings == null)
					{
						settings = new StiTxtExportSettings();
					}
					if (!(settings is StiTxtExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiTxtExportSettings is need to be used.");
					}
					stiTxtExportService.ExportTxt(this, stream, settings as StiTxtExportSettings);
					break;
				}
				case StiExportFormat.Xml:
				{
					if (exportService != null && !(exportService is StiXmlExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiXmlExportService stiXmlExportService = exportService as StiXmlExportService;
					if (stiXmlExportService == null)
					{
						stiXmlExportService = new StiXmlExportService();
					}
					if (settings == null)
					{
						settings = new StiXmlExportSettings();
					}
					if (!(settings is StiXmlExportSettings) && (!(settings is StiDataExportSettings) || (settings as StiDataExportSettings).DataType != Stimulsoft.Report.Export.StiDataType.Xml))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiXmlExportSettings is need to be used.");
					}
					stiXmlExportService.ExportXml(this, stream, settings as StiDataExportSettings);
					break;
				}
				case StiExportFormat.Json:
				{
					if (exportService != null && !(exportService is StiJsonExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiJsonExportService stiJsonExportService = exportService as StiJsonExportService;
					if (stiJsonExportService == null)
					{
						stiJsonExportService = new StiJsonExportService();
					}
					if (settings == null)
					{
						settings = new StiJsonExportSettings();
					}
					if (!(settings is StiJsonExportSettings) && (!(settings is StiDataExportSettings) || (settings as StiDataExportSettings).DataType != Stimulsoft.Report.Export.StiDataType.Json))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiJsonExportSettings is need to be used.");
					}
					stiJsonExportService.ExportJson(this, stream, settings as StiDataExportSettings);
					break;
				}
				case StiExportFormat.Ods:
				{
					if (exportService != null && !(exportService is StiOdsExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiOdsExportService stiOdsExportService = exportService as StiOdsExportService;
					if (stiOdsExportService == null)
					{
						stiOdsExportService = new StiOdsExportService();
					}
					if (settings == null)
					{
						settings = new StiOdsExportSettings();
					}
					if (!(settings is StiOdsExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiOdsExportSettings is need to be used.");
					}
					stiOdsExportService.ExportOds(this, stream, settings as StiOdsExportSettings);
					break;
				}
				case StiExportFormat.Odt:
				{
					if (exportService != null && !(exportService is StiOdtExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiOdtExportService stiOdtExportService = exportService as StiOdtExportService;
					if (stiOdtExportService == null)
					{
						stiOdtExportService = new StiOdtExportService();
					}
					if (settings == null)
					{
						settings = new StiOdtExportSettings();
					}
					if (!(settings is StiOdtExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiOdtExportSettings is need to be used.");
					}
					stiOdtExportService.ExportOdt(this, stream, settings as StiOdtExportSettings);
					break;
				}
				case StiExportFormat.Ppt2007:
				{
					if (exportService != null && !(exportService is StiPpt2007ExportService))
					{
						throw new ArgumentException("The incorrect type of the 'exportService' argument is used. The StiExportService must have same type of export as declared in exportFormat argument.");
					}
					StiPpt2007ExportService stiPpt2007ExportService = exportService as StiPpt2007ExportService;
					if (stiPpt2007ExportService == null)
					{
						stiPpt2007ExportService = new StiPpt2007ExportService();
					}
					if (settings == null)
					{
						settings = new StiPpt2007ExportSettings();
					}
					if (!(settings is StiPpt2007ExportSettings))
					{
						throw new ArgumentException("The incorrect type of the 'settings' argument is used. The StiPpt2007ExportSettings is need to be used.");
					}
					stiPpt2007ExportService.ExportPowerPoint(this, stream, settings as StiPpt2007ExportSettings);
					break;
				}
				case StiExportFormat.Document:
					SaveDocument(stream);
					break;
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Exporting rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			IsExporting = false;
			if (CompiledReport != null)
			{
				CompiledReport.IsExporting = false;
			}
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			Designer = designer;
			Info.ForceDesigningMode = forceDesigningMode;
			IsPageDesigner = isPageDesigner;
		}
		InvokeExported(exportFormat);
		return this;
	}

	public StiReport LoadFonts()
	{
		if (FontVHelper.LoadFontsFromResources(this))
		{
			LoadFontsForComponents();
			LoadFontsForConditions();
			LoadFontsForWatermark();
		}
		return this;
	}

	public StiReport LoadDocumentFonts()
	{
		if (FontVHelper.LoadFontsFromResources(this))
		{
			LoadDocumentFontsForComponents();
			LoadDocumentFontsForWatermark();
		}
		return this;
	}

	private void LoadFontsForComponents()
	{
		List<IStiFont> list = (from IStiFont c in from c in GetComponents().ToList()
				where c is IStiFont
				select c
			where c.Font != null && c.Font.OriginalFontName != null && c.Font.OriginalFontName != c.Font.Name
			select c).ToList();
		list.ForEach(delegate(IStiFont c)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			c.Font = StiFontCollection.CreateFont(c.Font.OriginalFontName, c.Font.Size, c.Font.Style);
		});
	}

	private void LoadFontsForConditions()
	{
		List<IStiConditions> list = (from c in GetComponents().ToList()
			where c != null && ((IStiConditions)c).Conditions.Count > 0
			select c).Cast<IStiConditions>().ToList();
		list.ForEach(delegate(IStiConditions c)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			foreach (StiBaseCondition condition in c.Conditions)
			{
				if (condition is StiCondition { Font: not null } stiCondition && stiCondition.Font.OriginalFontName != null && stiCondition.Font.OriginalFontName != stiCondition.Font.Name)
				{
					stiCondition.Font = StiFontCollection.CreateFont(stiCondition.Font.OriginalFontName, stiCondition.Font.Size, stiCondition.Font.Style);
				}
			}
		});
	}

	private void LoadDocumentFontsForComponents()
	{
		for (int i = 0; i < RenderedPages.Count; i++)
		{
			StiPage pageWithoutCache = RenderedPages.GetPageWithoutCache(i);
			List<IStiFont> list = (from IStiFont c in from c in pageWithoutCache.GetComponents().ToList()
					where c is IStiFont
					select c
				where c.Font != null && c.Font.OriginalFontName != null && c.Font.OriginalFontName != c.Font.Name
				select c).ToList();
			list.ForEach(delegate(IStiFont c)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				c.Font = StiFontCollection.CreateFont(c.Font.OriginalFontName, c.Font.Size, c.Font.Style);
			});
		}
	}

	private void LoadFontsForWatermark()
	{
		List<StiWatermark> list = (from p in Pages.ToList()
			select p.Watermark into w
			where w != null && w.Font.OriginalFontName != null && w.Font.OriginalFontName != w.Font.Name
			select w).ToList();
		list.ForEach(delegate(StiWatermark w)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			w.Font = StiFontCollection.CreateFont(w.Font.OriginalFontName, w.Font.Size, w.Font.Style);
		});
	}

	private void LoadDocumentFontsForWatermark()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < RenderedPages.Count; i++)
		{
			StiPage pageWithoutCache = RenderedPages.GetPageWithoutCache(i);
			StiWatermark watermark = pageWithoutCache.Watermark;
			if (watermark != null && watermark.Font.OriginalFontName != null && watermark.Font.OriginalFontName != watermark.Font.Name)
			{
				watermark.Font = StiFontCollection.CreateFont(watermark.Font.OriginalFontName, watermark.Font.Size, watermark.Font.Style);
			}
		}
	}

	public string GetResourceFontName(string resourceName)
	{
		string text = resourceToFontHash[resourceName] as string;
		if (text == null)
		{
			StiResource stiResource = (from r in Dictionary.Resources.ToList()
				where FontVHelper.IsFont(r.Type) && r.Content != null
				select r).ToList().FirstOrDefault((StiResource r) => r.Name == resourceName);
			if (stiResource != null)
			{
				FontFamily fontFamilyByContent = StiFontCollection.GetFontFamilyByContent(resourceName, stiResource.Content, stiResource.Type.ToString().Substring(4), stiResource.Alias);
				if (fontFamilyByContent != null)
				{
					text = fontFamilyByContent.Name;
				}
				if (stiResource.Name != stiResource.Alias)
				{
					text = stiResource.Alias;
				}
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				resourceToFontHash[resourceName] = text;
			}
		}
		return text;
	}

	public string GetFontResourceName(string fontName)
	{
		foreach (DictionaryEntry value in resourceToFontHash.Values)
		{
			if ((string)value.Value == fontName)
			{
				return value.Key as string;
			}
		}
		string resName = null;
		(from r in Dictionary.Resources.ToList()
			where FontVHelper.IsFont(r.Type) && r.Content != null
			select r).ToList().ForEach(delegate(StiResource r)
		{
			string resourceFontName = GetResourceFontName(r.Name);
			if (resourceFontName == fontName)
			{
				resName = r.Name;
			}
		});
		resourceToFontHash[resName] = fontName;
		return resName;
	}

	[Description("Returns a label of the specified variable.")]
	public string GetLabel(string variableName)
	{
		return StiVariableHelper.GetVariableLabel(this, variableName);
	}

	[Description("Returns a value of the specified variable or parameter.")]
	public object GetParam(string paramName)
	{
		StiDictionary dictionary = Dictionary;
		if (dictionary == null)
		{
			return null;
		}
		return ((IStiAppDictionary)dictionary).GetVariableValueByName(paramName);
	}

	[Description("Returns result of dividing value1 and value2. If value2 is zero, then result equal to 0.")]
	public static long Div(long value1, long value2)
	{
		return Div(value1, value2, 0L);
	}

	[Description("Returns result of dividing value1 and value2. If value2 is zero, then result is zeroResult (third argument).")]
	public static long Div(long value1, long value2, long zeroResult)
	{
		if (value2 == 0L)
		{
			return zeroResult;
		}
		return value1 / value2;
	}

	[Description("Returns result of dividing value1 and value2. If value2 is zero, then result equal to 0.")]
	public static double Div(double value1, double value2)
	{
		return Div(value1, value2, 0.0);
	}

	[Description("Returns result of dividing value1 and value2. If value2 is zero, then result is zeroResult (third argument).")]
	public static double Div(double value1, double value2, double zeroResult)
	{
		if (value2 == 0.0)
		{
			return zeroResult;
		}
		return value1 / value2;
	}

	[Description("Returns result of dividing value1 and value2. If value2 is zero, then result is zeroResult (third argument).")]
	public static decimal Div(decimal value1, decimal value2)
	{
		return Div(value1, value2, 0m);
	}

	[Description("Returns result of dividing value1 and value2. If value2 is zero, then result is zeroResult (third argument).")]
	public static decimal Div(decimal value1, decimal value2, decimal zeroResult)
	{
		if (value2 == 0m)
		{
			return zeroResult;
		}
		return value1 / value2;
	}

	[Description("Returns value1 if condition is true and value2 if condition is false. Base type is object.")]
	protected object IIF(bool condition, object value1, object value2)
	{
		if (!condition)
		{
			return value2;
		}
		return value1;
	}

	[Description("Returns true if current row of dataColumn from dataSource contain null or DbNull.Value.")]
	protected bool IsNull(object dataSource, string dataColumn)
	{
		if (dataSource == null)
		{
			throw new ArgumentNullException("Function IsNull: First argument can't be equal to null. Please provide name of Data Source.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function IsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		StiDataSource stiDataSource = dataSource as StiDataSource;
		StiBusinessObject stiBusinessObject = dataSource as StiBusinessObject;
		if (stiDataSource == null && stiBusinessObject == null)
		{
			if (!(dataSource is string))
			{
				throw new ArgumentException($"Function IsNull: Data Source {dataSource} is not exist.");
			}
			stiDataSource = Dictionary.DataSources[dataSource as string];
			if (stiDataSource == null)
			{
				throw new ArgumentException($"Function IsNull: Data Source '{dataSource}' is not exist.");
			}
		}
		if (stiBusinessObject != null)
		{
			object obj = stiBusinessObject[dataColumn];
			if (obj != null)
			{
				return obj == DBNull.Value;
			}
			return true;
		}
		object obj2 = stiDataSource[dataColumn];
		if (obj2 != null)
		{
			return obj2 == DBNull.Value;
		}
		return true;
	}

	[Description("Returns true if the next field value is equal to null or DBNull.Value.")]
	protected bool NextIsNull(object dataSource, string dataColumn)
	{
		if (dataSource == null)
		{
			throw new ArgumentNullException("Function IsNull: First argument can't be equal to null. Please provide name of Data Source.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function IsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		StiDataSource stiDataSource = dataSource as StiDataSource;
		StiBusinessObject stiBusinessObject = dataSource as StiBusinessObject;
		if (stiDataSource == null && stiBusinessObject == null)
		{
			if (!(dataSource is string))
			{
				throw new ArgumentException($"Function IsNull: Data Source {dataSource} is not exist.");
			}
			stiDataSource = Dictionary.DataSources[dataSource as string];
			if (stiDataSource == null)
			{
				throw new ArgumentException($"Function IsNull: Data Source '{dataSource}' is not exist.");
			}
		}
		string stateName = Guid.NewGuid().ToString();
		stiDataSource?.SaveState(stateName);
		stiBusinessObject?.SaveState(stateName);
		object obj = null;
		try
		{
			if (stiDataSource != null)
			{
				stiDataSource.Next();
				if (stiDataSource.IsEof)
				{
					return false;
				}
				obj = stiDataSource[dataColumn];
			}
			if (stiBusinessObject != null)
			{
				stiBusinessObject.Next();
				if (stiBusinessObject.IsEof)
				{
					return false;
				}
				obj = stiBusinessObject[dataColumn];
			}
		}
		finally
		{
			stiDataSource?.RestoreState(stateName);
			stiBusinessObject?.RestoreState(stateName);
		}
		if (obj != null)
		{
			return obj == DBNull.Value;
		}
		return true;
	}

	[Description("Returns true if the previous field value is equal to null or DBNull.Value.")]
	protected object LastOnPage(object dataSource, string dataColumn)
	{
		if (dataSource == null)
		{
			throw new ArgumentNullException("Function IsNull: First argument can't be equal to null. Please provide name of Data Source.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function IsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		StiDataSource stiDataSource = dataSource as StiDataSource;
		StiBusinessObject stiBusinessObject = dataSource as StiBusinessObject;
		if (stiDataSource == null && stiBusinessObject == null)
		{
			if (!(dataSource is string))
			{
				throw new ArgumentException($"Function IsNull: Data Source {dataSource} is not exist.");
			}
			stiDataSource = Dictionary.DataSources[dataSource as string];
			if (stiDataSource == null)
			{
				throw new ArgumentException($"Function IsNull: Data Source '{dataSource}' is not exist.");
			}
		}
		if (stiDataSource != null)
		{
			if (!stiDataSource.IsEof)
			{
				return StiFunctionsPrintState.Previous(dataSource, dataColumn);
			}
			return stiDataSource[dataColumn];
		}
		if (!stiBusinessObject.IsEof)
		{
			return StiFunctionsPrintState.Previous(dataSource, dataColumn);
		}
		return stiBusinessObject[dataColumn];
	}

	[Description("Returns true if the previous field value is equal to null or DBNull.Value.")]
	protected bool PreviousIsNull(object dataSource, string dataColumn)
	{
		if (dataSource == null)
		{
			throw new ArgumentNullException("Function IsNull: First argument can't be equal to null. Please provide name of Data Source.");
		}
		if (dataColumn == null)
		{
			throw new ArgumentNullException("Function IsNull: Second argument can't be equal to null. Please provide name of Data Column.");
		}
		StiDataSource stiDataSource = dataSource as StiDataSource;
		StiBusinessObject stiBusinessObject = dataSource as StiBusinessObject;
		if (stiDataSource == null && stiBusinessObject == null)
		{
			if (!(dataSource is string))
			{
				throw new ArgumentException($"Function IsNull: Data Source {dataSource} is not exist.");
			}
			stiDataSource = Dictionary.DataSources[dataSource as string];
			if (stiDataSource == null)
			{
				throw new ArgumentException($"Function IsNull: Data Source '{dataSource}' is not exist.");
			}
		}
		string stateName = Guid.NewGuid().ToString();
		stiDataSource?.SaveState(stateName);
		stiBusinessObject?.SaveState(stateName);
		object obj = null;
		try
		{
			if (stiDataSource != null)
			{
				stiDataSource.Prior();
				if (stiDataSource.IsBof)
				{
					return false;
				}
				obj = stiDataSource[dataColumn];
			}
			if (stiBusinessObject != null)
			{
				stiBusinessObject.Prior();
				if (stiBusinessObject.IsBof)
				{
					return false;
				}
				obj = stiBusinessObject[dataColumn];
			}
		}
		finally
		{
			stiDataSource?.RestoreState(stateName);
			stiBusinessObject?.RestoreState(stateName);
		}
		if (obj != null)
		{
			return obj == DBNull.Value;
		}
		return true;
	}

	[Description("Returns formated string of the specified arguments.")]
	protected string Format(string format, object arg)
	{
		return string.Format(format, arg);
	}

	private bool ShouldSerializeGlobalizationStrings()
	{
		return GlobalizationStrings.Count > 0;
	}

	public StiReport LocalizeReport(string cultureName)
	{
		GlobalizationStrings.LocalizeReport(cultureName);
		return this;
	}

	public StiReport LocalizeReport(CultureInfo info)
	{
		GlobalizationStrings.LocalizeReport(info);
		return this;
	}

	public static StiReport CreateNewReport()
	{
		return StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
	}

	public static StiReport CreateNewDashboard()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		StiReport stiReport = CreateNewReport();
		stiReport.CalculationMode = StiCalculationMode.Interpretation;
		stiReport.Pages.Clear();
		stiReport.IndexName = 1;
		if (!StiDashboardAssembly.IsAssemblyLoaded)
		{
			StiDashboardAssembly.LoadAssembly();
		}
		StiPage stiPage = StiDashboardCreator.CreateDashboard(stiReport) as StiPage;
		if (stiPage == null)
		{
			stiPage = StiActivator.CreateObject("Stimulsoft.Dashboard.Components.StiDashboard") as StiPage;
		}
		if (stiPage == null)
		{
			throw new StiDashboardAssemblyIsNotFoundException();
		}
		stiPage.Name = StiNameCreation.CreateName(stiReport, StiNameCreation.GenerateName(stiPage));
		stiReport.Pages.Add(stiPage);
		return stiReport;
	}

	public static StiReport CreateNewScreen()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		StiReport stiReport = CreateNewReport();
		stiReport.CalculationMode = StiCalculationMode.Interpretation;
		stiReport.Pages.Clear();
		stiReport.IndexName = 1;
		if (!StiAppAssembly.IsAssemblyLoaded)
		{
			StiAppAssembly.LoadAssembly();
		}
		StiPage stiPage = StiAppCreator.CreateScreen(stiReport) as StiPage;
		if (stiPage == null)
		{
			stiPage = StiActivator.CreateObject("Stimulsoft.App.Components.StiScreen") as StiPage;
		}
		if (stiPage == null)
		{
			throw new StiAppAssemblyIsNotFoundException();
		}
		stiPage.Name = StiNameCreation.CreateName(stiReport, StiNameCreation.GenerateName(stiPage));
		stiReport.Pages.Add(stiPage);
		return stiReport;
	}

	public async Task<StiReport> PrintAsync(PrinterSettings printerSettings)
	{
		return await Task.Run(() => Print(printerSettings));
	}

	public StiReport Print(PrinterSettings printerSettings)
	{
		return Print(PrinterSettings.ShowDialog, printerSettings);
	}

	public StiReport Print(bool showPrintDialog, PrinterSettings printerSettings)
	{
		return Print(showPrintDialog, printerSettings.FromPage, printerSettings.ToPage, printerSettings.Copies, printerSettings);
	}

	public StiReport Print()
	{
		return Print(PrinterSettings.ShowDialog, -1, -1, (short)PrinterSettings.Copies);
	}

	public StiReport Print(int fromPage, int toPage, short copies)
	{
		return Print(PrinterSettings.ShowDialog, fromPage, toPage, copies);
	}

	public async Task<StiReport> PrintAsync(short copies)
	{
		return await Task.Run(() => Print(showPrintDialog: false, copies));
	}

	public StiReport Print(bool showPrintDialog, short copies)
	{
		return Print(showPrintDialog, -1, -1, copies);
	}

	public async Task<StiReport> PrintAsync()
	{
		return await Task.Run(() => Print(showPrintDialog: false));
	}

	public StiReport Print(bool showPrintDialog)
	{
		return Print(showPrintDialog, (short)PrinterSettings.Copies);
	}

	public async Task<StiReport> PrintAsync(int fromPage, int toPage, short copies)
	{
		return await Task.Run(() => Print(showPrintDialog: false, fromPage, toPage, copies));
	}

	public StiReport Print(bool showPrintDialog, int fromPage, int toPage, short copies)
	{
		return Print(showPrintDialog, fromPage, toPage, copies, null);
	}

	public async Task<StiReport> PrintAsync(int fromPage, int toPage, short copies, PrinterSettings printerSettings)
	{
		return await Task.Run(() => Print(showPrintDialog: false, fromPage, toPage, copies, printerSettings));
	}

	public StiReport Print(bool showPrintDialog, int fromPage, int toPage, short copies, PrinterSettings printerSettings, StiPrintProvider printProvider = null)
	{
		InvokePrinting();
		RegReportDataSources();
		if (!IsRendered)
		{
			Render(showPrintDialog);
		}
		StiLogService.Write(GetType(), "Printing report");
		try
		{
			if (printProvider == null)
			{
				printProvider = new StiPrintProvider();
			}
			StiReport report = ((NeedsCompiling && CompiledReport != null) ? CompiledReport : this);
			printProvider.Print(report, showPrintDialog, fromPage, toPage, copies, printerSettings);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Printing report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		InvokePrinted();
		return this;
	}

	public async Task<StiReport> PrintReportsAsync(List<StiReport> reports)
	{
		await Task.Run(delegate
		{
			PrintReports(reports);
		});
		return this;
	}

	public static void PrintReports(List<StiReport> reports)
	{
		PrintReports(reports, showPrintDialog: true, 1);
	}

	public async Task<StiReport> PrintReportsAsync(List<StiReport> reports, short copies)
	{
		await Task.Run(delegate
		{
			PrintReports(reports, showPrintDialog: false, copies);
		});
		return this;
	}

	public static void PrintReports(List<StiReport> reports, bool showPrintDialog, short copies)
	{
		PrintReports(reports, showPrintDialog, copies, null);
	}

	public async Task<StiReport> PrintReportsAsync(List<StiReport> reports, short copies, PrinterSettings printerSettings)
	{
		await Task.Run(delegate
		{
			PrintReports(reports, showPrintDialog: false, copies, printerSettings);
		});
		return this;
	}

	public static void PrintReports(List<StiReport> reports, bool showPrintDialog, short copies, PrinterSettings printerSettings)
	{
		StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		stiReport.IsRendered = true;
		stiReport.NeedsCompiling = false;
		stiReport.RenderedPages.Clear();
		foreach (StiReport report in reports)
		{
			if (!report.IsRendered)
			{
				report.Render(showPrintDialog);
			}
			foreach (StiPage renderedPage in report.RenderedPages)
			{
				stiReport.RenderedPages.Add(renderedPage);
			}
		}
		stiReport.Print(showPrintDialog, copies, -1, -1, printerSettings);
		foreach (StiReport report2 in reports)
		{
			foreach (StiPage renderedPage2 in report2.RenderedPages)
			{
				renderedPage2.Report = report2;
			}
		}
	}

	public async Task<StiReport> PrintWithWpfAsync()
	{
		return await Task.Run(() => PrintWithWpf(showPrintDialog: false));
	}

	public StiReport PrintWithWpf()
	{
		return PrintWithWpf(showPrintDialog: true);
	}

	public async Task<StiReport> PrintWithWpfAsync(string printerName)
	{
		return await Task.Run(() => PrintWithWpf(showPrintDialog: false, -1, printerName));
	}

	public StiReport PrintWithWpf(string printerName)
	{
		return PrintWithWpf(showPrintDialog: true, -1, printerName);
	}

	public async Task<StiReport> PrintWithWpfAsync(object printTicket)
	{
		return await Task.Run(() => PrintWithWpf(printTicket, showPrintDialog: false));
	}

	public StiReport PrintWithWpf(object printTicket)
	{
		return PrintWithWpf(printTicket, showPrintDialog: true);
	}

	public StiReport PrintWithWpf(bool showPrintDialog)
	{
		return PrintWithWpf(null, showPrintDialog);
	}

	public StiReport PrintWithWpf(object printTicket, bool showPrintDialog)
	{
		return PrintWithWpf(printTicket, showPrintDialog, -1, -1, -1, null);
	}

	public async Task<StiReport> PrintWithWpfAsync(int copies)
	{
		return await Task.Run(() => PrintWithWpf(showPrintDialog: false, copies));
	}

	public StiReport PrintWithWpf(int copies)
	{
		return PrintWithWpf(showPrintDialog: true, copies);
	}

	public StiReport PrintWithWpf(bool showPrintDialog, int copies)
	{
		return PrintWithWpf(showPrintDialog, copies, null);
	}

	public async Task<StiReport> PrintWithWpfAsync(int copies, string printerName)
	{
		return await Task.Run(() => PrintWithWpf(showPrintDialog: false, -1, -1, copies, printerName));
	}

	public StiReport PrintWithWpf(bool showPrintDialog, int copies, string printerName)
	{
		return PrintWithWpf(showPrintDialog, -1, -1, copies, printerName);
	}

	public async Task<StiReport> PrintWithWpfAsync(int fromPage, int toPage, int copies)
	{
		return await Task.Run(() => PrintWithWpf(showPrintDialog: false, fromPage, toPage, copies, null));
	}

	public StiReport PrintWithWpf(bool showPrintDialog, int fromPage, int toPage, int copies)
	{
		return PrintWithWpf(showPrintDialog, fromPage, toPage, copies, null);
	}

	public async Task<StiReport> PrintWithWpfAsync(int fromPage, int toPage, int copies, string printerName)
	{
		return await Task.Run(() => PrintWithWpf(showPrintDialog: false, fromPage, toPage, copies, printerName));
	}

	public StiReport PrintWithWpf(bool showPrintDialog, int fromPage, int toPage, int copies, string printerName)
	{
		return PrintWithWpf(null, showPrintDialog, fromPage, toPage, copies, printerName);
	}

	public async Task<StiReport> PrintWithWpfAsync(object printTicket, int fromPage, int toPage, int copies, string printerName)
	{
		return await Task.Run(() => PrintWithWpf(printTicket, showPrintDialog: false, fromPage, toPage, copies, printerName));
	}

	public StiReport PrintWithWpf(object printTicket, bool showPrintDialog, int fromPage, int toPage, int copies, string printerName)
	{
		InvokePrinting();
		RegReportDataSources();
		if (!IsRendered)
		{
			RenderWithWpf(showProgress: true);
		}
		StiLogService.Write(GetType(), "Printing report");
		try
		{
			StiReport stiReport = ((CheckNeedsCompiling() && CompiledReport != null) ? CompiledReport : this);
			stiReport.PrintWithWpfInternal(printTicket, showPrintDialog, fromPage, toPage, copies, printerName);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Printing report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		InvokePrinted();
		return this;
	}

	private StiReport PrintWithWpfInternal(object printTicket, bool showPrintDialog, int fromPage, int toPage, int copies, string printerName)
	{
		Type type = Type.GetType("Stimulsoft.Report.Print.StiWpfPrintProvider, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Wpf' is not found");
		}
		IStiWpfPrintProvider stiWpfPrintProvider = StiActivator.CreateObject(type, new object[1] { this }) as IStiWpfPrintProvider;
		stiWpfPrintProvider.Print(printTicket, showPrintDialog, fromPage, toPage, copies, printerName);
		return this;
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap()
	{
		return PrintWithXbap(showPrintDialog: true);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(string printerName)
	{
		return PrintWithXbap(showPrintDialog: true, -1, printerName);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(object printTicket)
	{
		return PrintWithXbap(printTicket, showPrintDialog: true);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(bool showPrintDialog)
	{
		return PrintWithXbap(null, showPrintDialog);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(object printTicket, bool showPrintDialog)
	{
		return PrintWithXbap(printTicket, showPrintDialog, -1, -1, -1, null);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(int copies)
	{
		return PrintWithXbap(showPrintDialog: true, copies);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(bool showPrintDialog, int copies)
	{
		return PrintWithXbap(showPrintDialog, copies, null);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(bool showPrintDialog, int copies, string printerName)
	{
		return PrintWithXbap(showPrintDialog, -1, -1, copies, printerName);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(bool showPrintDialog, int fromPage, int toPage, int copies)
	{
		return PrintWithXbap(showPrintDialog, fromPage, toPage, copies, null);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(bool showPrintDialog, int fromPage, int toPage, int copies, string printerName)
	{
		return PrintWithXbap(null, showPrintDialog, fromPage, toPage, copies, printerName);
	}

	[Obsolete("The 'PrintWithXbap' method is deprecated!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiReport PrintWithXbap(object printTicket, bool showPrintDialog, int fromPage, int toPage, int copies, string printerName)
	{
		InvokePrinting();
		RegReportDataSources();
		if (!IsRendered)
		{
			RenderWithWpf(showProgress: true);
		}
		StiLogService.Write(GetType(), "Printing report");
		try
		{
			if (CheckNeedsCompiling())
			{
				CompiledReport.PrintWithXbapInternal(printTicket, showPrintDialog, fromPage, toPage, copies, printerName);
			}
			else
			{
				PrintWithXbapInternal(printTicket, showPrintDialog, fromPage, toPage, copies, printerName);
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Printing report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		InvokePrinted();
		return this;
	}

	private void PrintWithXbapInternal(object printTicket, bool showPrintDialog, int fromPage, int toPage, int copies, string printerName)
	{
		Type type = Type.GetType("Stimulsoft.Report.Print.StiWpfPrintProvider, Stimulsoft.Report.Xbap, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Xbap' is not found");
		}
		IStiWpfPrintProvider stiWpfPrintProvider = StiActivator.CreateObject(type, new object[1] { this }) as IStiWpfPrintProvider;
		stiWpfPrintProvider.Print(printTicket, showPrintDialog, fromPage, toPage, copies, printerName);
	}

	public StiReport PrintToDotMatrixPrinter()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		PrinterSettings val = new PrinterSettings();
		return PrintToDotMatrixPrinter(val.PrinterName, Encoding.ASCII);
	}

	public StiReport PrintToDotMatrixPrinter(string printerName)
	{
		return PrintToDotMatrixPrinter(printerName, Encoding.ASCII);
	}

	public StiReport PrintToDotMatrixPrinter(string printerName, Encoding encoding)
	{
		return PrintToDotMatrixPrinter(printerName, encoding, -1, -1);
	}

	public StiReport PrintToDotMatrixPrinter(string printerName, Encoding encoding, int fromPage, int toPage)
	{
		StiDotMatrixPrintProvider.PrintToDotMatrixPrinter(this, printerName, encoding, fromPage, toPage);
		return this;
	}

	public StiReport PrintToDotMatrixPrinter(string printerName, Encoding encoding, bool drawBorder, StiTxtBorderType borderType, bool putFeedPageCode, bool cutLongLines, int fromPage, int toPage)
	{
		StiDotMatrixPrintProvider.PrintToDotMatrixPrinter(this, printerName, encoding, drawBorder, borderType, putFeedPageCode, cutLongLines, fromPage, toPage);
		return this;
	}

	public StiReport PrintToDotMatrixPrinter(string printerName, Encoding encoding, bool drawBorder, StiTxtBorderType borderType, bool killSpaceLines, bool killSpaceGraphLines, bool putFeedPageCode, bool cutLongLines, float zoomX, float zoomY, int fromPage, int toPage, bool useEscapeCodes, string escapeCodesCollectionName)
	{
		StiDotMatrixPrintProvider.PrintToDotMatrixPrinter(this, printerName, encoding, drawBorder, borderType, killSpaceLines, killSpaceGraphLines, putFeedPageCode, cutLongLines, zoomX, zoomY, fromPage, toPage, useEscapeCodes, escapeCodesCollectionName);
		return this;
	}

	public StiReport RegData(string name, object data)
	{
		Dictionary.DataStore.RegData(name, data);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, data);
		return this;
	}

	public StiReport RegData(string name, DataTable dataTable)
	{
		Dictionary.DataStore.RegData(name, dataTable);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, dataTable);
		return this;
	}

	public StiReport RegData(DataTable dataTable)
	{
		Dictionary.DataStore.RegData(dataTable);
		CompiledReport?.Dictionary?.DataStore?.RegData(dataTable);
		return this;
	}

	public StiReport RegData(string name, DataSet dataSet)
	{
		Dictionary.DataStore.RegData(name, dataSet);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, dataSet);
		return this;
	}

	public StiReport RegData(DataSet dataSet)
	{
		Dictionary.DataStore.RegData(dataSet);
		CompiledReport?.Dictionary?.DataStore?.RegData(dataSet);
		return this;
	}

	public StiReport RegData(XElement element, StiRelationDirection relationDirection = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = (((int)StiBaseOptions.DefaultJsonConverterVersion == 1) ? StiJsonToDataSetConverterV2.GetDataSet(element, relationDirection) : StiJsonToDataSetConverter.GetDataSet(element));
		RegData(dataSet);
		return this;
	}

	public StiReport RegData(string name, XElement element, StiRelationDirection relationDirection = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = (((int)StiBaseOptions.DefaultJsonConverterVersion == 1) ? StiJsonToDataSetConverterV2.GetDataSet(element, relationDirection) : StiJsonToDataSetConverter.GetDataSet(element));
		RegData(name, dataSet);
		return this;
	}

	public StiReport RegData(string name, DataView dataView)
	{
		Dictionary.DataStore.RegData(name, dataView);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, dataView);
		return this;
	}

	public StiReport RegData(DataView dataView)
	{
		Dictionary.DataStore.RegData(dataView);
		CompiledReport?.Dictionary?.DataStore?.RegData(dataView);
		return this;
	}

	public StiReport RegData(string name, SqlConnection connection)
	{
		Dictionary.DataStore.RegData(name, connection);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, connection);
		return this;
	}

	public StiReport RegData(string name, OleDbConnection connection)
	{
		Dictionary.DataStore.RegData(name, connection);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, connection);
		return this;
	}

	public StiReport RegData(string name, OdbcConnection connection)
	{
		Dictionary.DataStore.RegData(name, connection);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, connection);
		return this;
	}

	public StiReport RegData(string name, string alias, object data)
	{
		Dictionary.DataStore.RegData(name, alias, data);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, alias, data);
		return this;
	}

	public StiReport RegData(string name, string alias, DataTable dataTable)
	{
		Dictionary.DataStore.RegData(name, alias, dataTable);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, alias, dataTable);
		return this;
	}

	public StiReport RegData(string name, string alias, DataSet dataSet)
	{
		Dictionary.DataStore.RegData(name, alias, dataSet);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, alias, dataSet);
		return this;
	}

	public StiReport RegData(string name, string alias, DataView dataView)
	{
		Dictionary.DataStore.RegData(name, alias, dataView);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, alias, dataView);
		return this;
	}

	public StiReport RegData(string name, string alias, SqlConnection connection)
	{
		Dictionary.DataStore.RegData(name, alias, connection);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, alias, connection);
		return this;
	}

	public StiReport RegData(string name, string alias, OleDbConnection connection)
	{
		Dictionary.DataStore.RegData(name, alias, connection);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, alias, connection);
		return this;
	}

	public StiReport RegData(string name, string alias, OdbcConnection connection)
	{
		Dictionary.DataStore.RegData(name, alias, connection);
		CompiledReport?.Dictionary?.DataStore?.RegData(name, alias, connection);
		return this;
	}

	public StiReport RegData(StiDataCollection datas)
	{
		Dictionary.DataStore.RegData(datas);
		CompiledReport?.Dictionary?.DataStore?.RegData(datas);
		return this;
	}

	public StiReport RegBusinessObject(string name, object value)
	{
		RegBusinessObject(string.Empty, name, name, value);
		return this;
	}

	public StiReport RegBusinessObject(string category, string name, object value)
	{
		RegBusinessObject(category, name, name, value);
		return this;
	}

	public StiReport RegBusinessObject(string category, string name, string alias, object value)
	{
		StiBusinessObjectData businessObject = new StiBusinessObjectData(category, name, alias, value);
		StoreBusinessObjectWithCheckExistingData(businessObject);
		CompiledReport?.StoreBusinessObjectWithCheckExistingData(businessObject);
		return this;
	}

	public StiReport RegBusinessObject(List<StiBusinessObjectData> businessObjects)
	{
		foreach (StiBusinessObjectData businessObject in businessObjects)
		{
			StoreBusinessObjectWithCheckExistingData(businessObject);
		}
		return this;
	}

	private void StoreBusinessObjectWithCheckExistingData(StiBusinessObjectData businessObject)
	{
		if (StiOptions.Dictionary.ReplaceExistingDataAtRegistrationOfNewData)
		{
			for (int i = 0; i < BusinessObjectsStore.Count; i++)
			{
				StiBusinessObjectData stiBusinessObjectData = BusinessObjectsStore[i];
				if (stiBusinessObjectData.Category == businessObject.Category && stiBusinessObjectData.Name == businessObject.Name)
				{
					BusinessObjectsStore[i] = businessObject;
					return;
				}
			}
		}
		BusinessObjectsStore.Add(businessObject);
	}

	public StiReport RegReportDataSources()
	{
		foreach (StiReportDataSource reportDataSource in ReportDataSources)
		{
			if (Dictionary.DataStore[reportDataSource.Name] == null)
			{
				DataSet dataSet = reportDataSource.Item as DataSet;
				if (reportDataSource.Item is Control)
				{
					RegBusinessObject(reportDataSource.Name, reportDataSource.Item);
				}
				else if (dataSet != null)
				{
					RegData(dataSet);
				}
				else
				{
					RegData(reportDataSource.Name, reportDataSource.Item);
				}
				continue;
			}
			object obj = reportDataSource.Item;
			if (obj is BindingSource)
			{
				obj = ((BindingSource)((obj is BindingSource) ? obj : null)).List;
			}
			if (Dictionary.DataStore[reportDataSource.Name].Data != obj)
			{
				Dictionary.DataStore[reportDataSource.Name].Data = obj;
			}
		}
		return this;
	}

	public async Task<StiReport> RenderAsync()
	{
		return await Task.Run(() => Render(showProgress: false));
	}

	public StiReport Render()
	{
		return Render(showProgress: true);
	}

	public StiReport Render(bool showProgress)
	{
		return Render(showProgress, -1, -1);
	}

	public async Task<StiReport> RenderAsync(int fromPage, int toPage)
	{
		return await Task.Run(() => Render(showProgress: false, fromPage, toPage));
	}

	public StiReport Render(bool showProgress, int fromPage, int toPage)
	{
		return Render(new StiRenderState(fromPage, toPage, showProgress));
	}

	public async Task<StiReport> RenderAsync(StiRenderState renderState)
	{
		return await Task.Run(() => Render(renderState));
	}

	public StiReport Render(StiRenderState renderState)
	{
		return Render(renderState, (StiGuiMode)(IsWpf ? 1 : 0));
	}

	private StiReport Render(StiRenderState renderState, StiGuiMode guiMode)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		RenderedWith = StiRenderedWith.Net;
		if ((int)guiMode == 1)
		{
			IsWpf = true;
			if (CompiledReport != null)
			{
				CompiledReport.IsWpf = true;
				CompiledReport.RenderedWith = StiRenderedWith.Wpf;
			}
			StiOptions.Configuration.IsWPF = true;
			RenderedWith = StiRenderedWith.Wpf;
		}
		bookmarkValue.Engine = EngineVersion;
		Bookmark.Engine = EngineVersion;
		IsEditedInViewer = false;
		if (StiOptions.Engine.HideRenderingProgress)
		{
			renderState.ShowProgress = false;
		}
		if (StiOptions.Engine.HideMessages)
		{
			renderState.ShowProgress = false;
		}
		if (StiOptions.Configuration.IsWeb)
		{
			renderState.ShowProgress = false;
		}
		if (renderState.ShowProgress && ReportPass != StiReportPass.First && !renderState.IsSubReportMode)
		{
			BackgroundWorker backgroundWorker = null;
			try
			{
				if (StiOptions.Engine.AllowProgressInThread || UseProgressInThread)
				{
					backgroundWorker = new BackgroundWorker();
					backgroundWorker.WorkerSupportsCancellation = true;
					backgroundWorker.DoWork += RenderWorker_DoWork;
					backgroundWorker.RunWorkerAsync(renderState);
				}
				try
				{
					if (!StiOptions.Engine.AllowProgressExternalAccess || Progress == null)
					{
						Progress = ((Designer is Form) ? StiGuiOptions.GetProgressInformation((object)/*isinst with value type is only supported in some contexts*/, guiMode) : StiGuiOptions.GetProgressInformation(guiMode));
					}
				}
				catch
				{
					Progress = null;
				}
				if (Progress != null)
				{
					Progress.IsMarquee = true;
				}
				if ((StiOptions.Engine.AllowProgressInThread || UseProgressInThread) && Progress != null)
				{
					Progress.AllowUseDoEvents = false;
				}
				if (Progress != null)
				{
					Progress.HideProgressBar();
					Progress.SetAllowClose(allows: false);
					Progress.Start((StiLocalization.CultureName == "en") ? "Preparing Report" : StiLocalization.Get("Report", "PreparingReport"));
				}
				if (backgroundWorker != null)
				{
					while (backgroundWorker.IsBusy)
					{
						Application.DoEvents();
					}
				}
				else
				{
					RenderReport(renderState);
				}
			}
			finally
			{
				backgroundWorker?.Dispose();
				if (Progress != null && ReportPass != StiReportPass.First && !renderState.IsSubReportMode)
				{
					Progress.Close();
					if (!StiOptions.Engine.AllowProgressExternalAccess)
					{
						Progress = null;
					}
					StatusString = "";
					if (CompiledReport != null)
					{
						CompiledReport.Progress = null;
					}
				}
			}
		}
		else
		{
			RenderReport(renderState);
		}
		if (ReportPass != StiReportPass.First)
		{
			IsWpf = false;
			if (CompiledReport != null)
			{
				CompiledReport.IsWpf = false;
			}
		}
		return this;
	}

	private void RenderWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		StiRenderState renderState = (StiRenderState)(e.Result = e.Argument as StiRenderState);
		RenderReport(renderState);
	}

	private void RenderReport(StiRenderState renderState)
	{
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Invalid comparison between Unknown and I4
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Invalid comparison between Unknown and I4
		if (CalculationMode == StiCalculationMode.Interpretation)
		{
			ProcessAutoLocalizeReportOnRun();
		}
		StiCalculationMode calculationMode = CalculationMode;
		CheckNeedsCompiling();
		if (StiOptions.Engine.ForceInterpretationMode && (NeedsCompiling || IsRendered))
		{
			CalculationMode = StiCalculationMode.Interpretation;
		}
		if (NeedsCompiling && CalculationMode == StiCalculationMode.Compilation && !StiOptions.Engine.FullTrust)
		{
			CalculationMode = StiCalculationMode.Interpretation;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		if (!string.IsNullOrEmpty(Culture))
		{
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo(Culture, useUserOverride: false);
			}
			catch
			{
			}
		}
		Hashtable hashtable = new Hashtable();
		try
		{
			if (CachedTotals != null)
			{
				CachedTotals.Clear();
			}
			else
			{
				CachedTotals = new Hashtable();
			}
			if (Engine != null && Engine.PageNumbers != null)
			{
				Engine.PageNumbers.ClearNotFixed();
			}
			if (CalculationMode == StiCalculationMode.Interpretation)
			{
				UpdateReportVersion();
				if (Variables == null)
				{
					StiParser.PrepareReportVariables(this);
				}
			}
			if (CalculationMode == StiCalculationMode.Interpretation && ReportPass != StiReportPass.First && !renderState.IsSubReportMode)
			{
				foreach (StiDataSource dataSource in Dictionary.DataSources)
				{
					if (dataSource is StiSqlSource stiSqlSource)
					{
						Variables["**StoredDataSourceSqlCommandForInterpretationMode**" + dataSource.Name] = stiSqlSource.SqlCommand;
					}
				}
			}
			if (CalculationMode == StiCalculationMode.Interpretation)
			{
				StiComponentsCollection components = GetComponents();
				foreach (StiComponent item in components)
				{
					if (item.Interaction != null && item.Interaction.IsDefault)
					{
						hashtable[item] = item.Interaction;
						item.Interaction = null;
					}
					if (item is StiImage && GlobalizationManager != null && item is IStiGlobalizedName stiGlobalizedName && !string.IsNullOrWhiteSpace(stiGlobalizedName.GlobalizedName))
					{
						object @object = GlobalizationManager.GetObject(stiGlobalizedName.GlobalizedName);
						if (@object is byte[])
						{
							(item as StiImage).PutImage(@object as byte[]);
						}
						if (@object is Image)
						{
							(item as StiImage).PutImage((Image)((@object is Image) ? @object : null));
						}
					}
				}
			}
			if (SubReports != null && SubReports.Count > 0)
			{
				if ((int)EngineVersion == 1)
				{
					StiReportV1Builder.RenderSubReports(this, renderState);
				}
				else
				{
					StiReportV2Builder.RenderSubReports(this, renderState);
				}
			}
			else if ((int)EngineVersion == 1)
			{
				StiReportV1Builder.RenderSingleReport(this, renderState);
			}
			else
			{
				StiReportV2Builder.RenderSingleReport(this, renderState);
			}
		}
		finally
		{
			if (CalculationMode == StiCalculationMode.Interpretation)
			{
				foreach (DictionaryEntry item2 in hashtable)
				{
					StiComponent stiComponent2 = (StiComponent)item2.Key;
					stiComponent2.Interaction = (StiInteraction)item2.Value;
				}
				hashtable.Clear();
			}
			if (CalculationMode == StiCalculationMode.Interpretation && ReportPass != StiReportPass.First && !renderState.IsSubReportMode)
			{
				foreach (StiDataSource dataSource2 in Dictionary.DataSources)
				{
					if (!(dataSource2 is StiSqlSource stiSqlSource2))
					{
						continue;
					}
					string key = "**StoredDataSourceSqlCommandForInterpretationMode**" + dataSource2.Name;
					if (Variables.ContainsKey(key))
					{
						object obj2 = Variables[key];
						if (obj2 is string)
						{
							stiSqlSource2.SqlCommand = obj2 as string;
						}
						Variables.Remove(key);
					}
				}
			}
			CalculationMode = calculationMode;
			if (!string.IsNullOrEmpty(Culture))
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
			if (CachedTotals != null)
			{
				CachedTotals.Clear();
				CachedTotals = null;
			}
			if (ReportPass == StiReportPass.First && Anchors != null)
			{
				Anchors.Clear();
			}
			foreach (StiBusinessObject businessObject in Dictionary.BusinessObjects)
			{
				businessObject.previousResetException = false;
			}
			if (WpfRichTextDomain != null)
			{
				AppDomain.Unload(WpfRichTextDomain);
				WpfRichTextDomain = null;
			}
			if (CompiledReport != null && CompiledReport.WpfRichTextDomain != null)
			{
				AppDomain.Unload(CompiledReport.WpfRichTextDomain);
				CompiledReport.WpfRichTextDomain = null;
			}
			RenderedPages.Flush(final: true);
		}
		IsDocument = false;
	}

	internal void Clear()
	{
		Clear(generateNewScript: true);
	}

	protected internal virtual void Clear(bool generateNewScript)
	{
		RenderingEvent = new StiRenderingEvent();
		BeginRenderEvent = new StiBeginRenderEvent();
		EndRenderEvent = new StiEndRenderEvent();
		PrintingEvent = new StiPrintingEvent();
		PrintedEvent = new StiPrintedEvent();
		ExportedEvent = new StiExportedEvent();
		ExportingEvent = new StiExportingEvent();
		ScriptLanguage = StiOptions.Engine.DefaultReportLanguage;
		if (StiLocalization.CultureName == "en")
		{
			ReportName = "Report";
			ReportAlias = "Report";
		}
		else
		{
			ReportName = StiLocalization.Get("Components", "StiReport");
			ReportAlias = StiLocalization.Get("Components", "StiReport");
		}
		ReportDescription = string.Empty;
		ReportAuthor = string.Empty;
		if (StiOptions.Engine.DefaultUnit is StiReportUnitType)
		{
			ReportUnit = (StiReportUnitType)StiOptions.Engine.DefaultUnit;
		}
		else
		{
			ReportUnit = ((!RegionInfo.CurrentRegion.IsMetric) ? StiReportUnitType.Inches : StiReportUnitType.Centimeters);
		}
		ReferencedAssemblies = StiOptions.Engine.ReferencedAssemblies ?? new string[8] { "System.Dll", "System.Drawing.Dll", "System.Windows.Forms.Dll", "System.Data.Dll", "System.Xml.Dll", "Stimulsoft.Controls.Dll", "Stimulsoft.Base.Dll", "Stimulsoft.Report.Dll" };
		ScriptLanguage = StiOptions.Engine.DefaultReportLanguage;
		Styles.Clear();
		AutoLocalizeReportOnRun = false;
		CacheAllData = false;
		ConvertNulls = true;
		Collate = 1;
		Culture = string.Empty;
		GlobalizationStrings.Clear();
		NumberOfPass = StiNumberOfPass.SinglePass;
		PreviewMode = StiPreviewMode.Standard;
		DashboardViewerSettings = StiDashboardViewerSettings.All;
		PreviewSettings = 260039167;
		PrinterSettings = new StiPrinterSettings();
		ReportCacheMode = StiReportCacheMode.Off;
		StopBeforePage = 0;
		IsEditedInViewer = false;
		MetaTags.Clear();
		CalculationMode = StiCalculationMode.Compilation;
		ParametersOrientation = StiOrientation.Horizontal;
		RequestParameters = false;
		SaveReportInResources = true;
		RefreshTime = 0;
		if (generateNewScript && StiOptions.Engine.FullTrust)
		{
			ScriptNew();
		}
		ResetRenderedState();
		ResetAggregateFunctions();
		if (CompiledReport != null)
		{
			CompiledReport.ResetAggregateFunctions();
		}
		if (HashViewWpfPainter != null)
		{
			HashViewWpfPainter.Clear();
			HashViewWpfPainter = null;
		}
	}

	public StiReport ResetAggregateFunctions()
	{
		if (AggregateFunctions == null)
		{
			return this;
		}
		object[] aggregateFunctions = AggregateFunctions;
		for (int i = 0; i < aggregateFunctions.Length; i++)
		{
			StiAggregateFunctionService stiAggregateFunctionService = (StiAggregateFunctionService)aggregateFunctions[i];
			bool isFirstInit = stiAggregateFunctionService.IsFirstInit;
			stiAggregateFunctionService.IsFirstInit = true;
			stiAggregateFunctionService.Init();
			stiAggregateFunctionService.IsFirstInit = isFirstInit;
		}
		return this;
	}

	public StiReport ResetRenderedState()
	{
		IsRendered = false;
		CompiledReport = null;
		CompilerResults = null;
		RenderedPages.Clear();
		return this;
	}

	public StiReport RecalculateRenderedPagesSegments()
	{
		foreach (StiPage renderedPage in RenderedPages)
		{
			if (renderedPage.SegmentPerWidth <= 1 && renderedPage.SegmentPerHeight <= 1)
			{
				continue;
			}
			RenderedPages.GetPage(renderedPage);
			double num = 0.0;
			double num2 = 0.0;
			foreach (StiComponent component in renderedPage.Components)
			{
				if (component.Right > num)
				{
					num = component.Right;
				}
				if (component.Bottom > num2)
				{
					num2 = component.Bottom;
				}
			}
			int i;
			for (i = 1; (decimal)((renderedPage.PageWidth - renderedPage.Margins.Left - renderedPage.Margins.Right) * (double)i) < (decimal)num; i++)
			{
			}
			int j;
			for (j = 1; (decimal)((renderedPage.PageHeight - renderedPage.Margins.Top - renderedPage.Margins.Bottom) * (double)j) < (decimal)num2; j++)
			{
			}
			renderedPage.SegmentPerWidth = i;
			renderedPage.SegmentPerHeight = j;
		}
		return this;
	}

	public StiReport RenderWithWpf()
	{
		return RenderWithWpf(showProgress: true);
	}

	public StiReport RenderWithWpf(bool showProgress)
	{
		return RenderWithWpf(showProgress, -1, -1);
	}

	public StiReport RenderWithWpf(bool showProgress, int fromPage, int toPage)
	{
		return RenderWithWpf(new StiRenderState(fromPage, toPage, showProgress));
	}

	public StiReport RenderWithWpf(StiRenderState renderState)
	{
		return Render(renderState, (StiGuiMode)1);
	}

	public static bool IsPackedFile(Stream stream)
	{
		if (stream.Length < 4)
		{
			return false;
		}
		int first = stream.ReadByte();
		int second = stream.ReadByte();
		int third = stream.ReadByte();
		stream.Seek(-3L, SeekOrigin.Current);
		return IsPackedFile(first, second, third);
	}

	public static bool IsJsonFile(Stream stream)
	{
		if (stream.Length < 2)
		{
			return false;
		}
		int num = stream.ReadByte();
		if (num == 239 && stream.Length > 3)
		{
			int num2 = stream.ReadByte();
			int num3 = stream.ReadByte();
			num = stream.ReadByte();
			stream.Seek(-3L, SeekOrigin.Current);
			if (num2 != 187 || num3 != 191)
			{
				num = 0;
			}
		}
		stream.Seek(-1L, SeekOrigin.Current);
		return IsJsonFile(num);
	}

	public static bool IsJsonFile(int first)
	{
		return first == 123;
	}

	public static bool IsPackedFile(byte[] bytes)
	{
		if (bytes == null || bytes.Length < 3)
		{
			return false;
		}
		return IsPackedFile(bytes[0], bytes[1], bytes[2]);
	}

	public static bool IsPackedFile(int first, int second, int third)
	{
		if (first == 31 && second == 139 && third == 8)
		{
			return true;
		}
		if (first == 80 && second == 75 && third == 3)
		{
			return true;
		}
		return false;
	}

	public static bool IsEncryptedFile(Stream stream)
	{
		if (stream.Length < 4)
		{
			return false;
		}
		int first = stream.ReadByte();
		int second = stream.ReadByte();
		int third = stream.ReadByte();
		stream.Seek(-3L, SeekOrigin.Current);
		return IsEncryptedFile(first, second, third);
	}

	public static bool IsEncryptedFile(byte[] bytes)
	{
		if (bytes == null || bytes.Length < 3)
		{
			return false;
		}
		return IsEncryptedFile(bytes[0], bytes[1], bytes[2]);
	}

	public static bool IsEncryptedFile(int first, int second, int third)
	{
		if (first != 109 || second != 100 || third != 120)
		{
			if (first == 109 && second == 114)
			{
				return third == 120;
			}
			return false;
		}
		return true;
	}

	public virtual StiReport LoadReportFromResource(Assembly reportAssembly, string reportResource)
	{
		Stream manifestResourceStream = reportAssembly.GetManifestResourceStream(reportResource);
		if (manifestResourceStream != null)
		{
			Load(manifestResourceStream);
			manifestResourceStream.Close();
			manifestResourceStream.Dispose();
			return this;
		}
		throw new Exception("Can't find the report '" + reportResource + "' in the resources!");
	}

	public virtual StiReport LoadReportFromResource(string assemblyName, string reportResource)
	{
		Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetName().Name == assemblyName);
		if (assembly != null)
		{
			LoadReportFromResource(assembly, reportResource);
			return this;
		}
		throw new Exception("Can't find the assembly '" + assemblyName + "'!");
	}

	public string SaveReportSourceCode()
	{
		return SaveReportSourceCode(saveForInheritedReports: false);
	}

	public string SaveReportSourceCode(bool saveForInheritedReports)
	{
		if (saveForInheritedReports)
		{
			StiCodeDomSerializator stiCodeDomSerializator = new StiCodeDomSerializator();
			return stiCodeDomSerializator.Serialize(this, GetReportName(), Language, saveForInheritedReports);
		}
		string text = Script;
		try
		{
			ScriptUnpack(saveForInheritedReports);
			return Script;
		}
		finally
		{
			Script = text;
		}
	}

	public StiReport SaveReportSourceCode(Stream stream)
	{
		SaveReportSourceCode(stream, saveForInheritedReports: false);
		return this;
	}

	public StiReport SaveReportSourceCode(Stream stream, bool saveForInheritedReports)
	{
		StiLogService.Write(typeof(StiReport), "Saving report to source code");
		StreamWriter streamWriter = null;
		Encoding encoding = Encoding.UTF8;
		try
		{
			if (StiOptions.Engine.SourceCodeEncoding != null)
			{
				encoding = StiOptions.Engine.SourceCodeEncoding;
			}
			streamWriter = new StreamWriter(stream, encoding);
			streamWriter.Write(SaveReportSourceCode(saveForInheritedReports));
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report to source code...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			streamWriter?.Flush();
		}
		return this;
	}

	public virtual StiReport SaveReportSourceCode(string path)
	{
		return SaveReportSourceCode(path, saveForInheritedReports: false);
	}

	public virtual StiReport SaveReportSourceCode(string path, bool saveForInheritedReports)
	{
		FileStream fileStream = null;
		try
		{
			if (!IsDesigning || !StiOptions.Designer.ReadOnlyAlertOnSave)
			{
				StiFileUtils.ProcessReadOnly(path);
			}
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			SaveReportSourceCode(fileStream, saveForInheritedReports);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report to source code...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		return this;
	}

	public virtual StiReport SaveReportSourceCodeForSilverlight(string path)
	{
		FileStream fileStream = null;
		try
		{
			if (!IsDesigning || !StiOptions.Designer.ReadOnlyAlertOnSave)
			{
				StiFileUtils.ProcessReadOnly(path);
			}
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			SaveReportSourceCodeForSilverlight(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report to source code...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		return this;
	}

	public StiReport SaveReportSourceCodeForSilverlight(Stream stream)
	{
		StiLogService.Write(typeof(StiReport), "Saving report to source code for Silverlight");
		StreamWriter streamWriter = null;
		Encoding encoding = Encoding.UTF8;
		try
		{
			if (StiOptions.Engine.SourceCodeEncoding != null)
			{
				encoding = StiOptions.Engine.SourceCodeEncoding;
			}
			streamWriter = new StreamWriter(stream, encoding);
			streamWriter.Write(SaveReportSourceCodeForSilverlight());
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report to source code for Silverlight...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			streamWriter?.Flush();
		}
		return this;
	}

	public string SaveReportSourceCodeForSilverlight()
	{
		ReportResources.Clear();
		StiDataRelationsCollection relations = Dictionary.Relations;
		StiDataSourcesCollection dataSources = Dictionary.DataSources;
		StiDatabaseCollection databases = Dictionary.Databases;
		try
		{
			Dictionary.DataSources = new StiDataSourcesCollection(Dictionary);
			Dictionary.Relations = new StiDataRelationsCollection(Dictionary);
			Dictionary.Databases = new StiDatabaseCollection();
			string text = Script;
			ScriptUnpack();
			string text2 = Script;
			Script = text;
			text2 = text2.Replace("System.Drawing.Color.FromArgb", "System.Windows.Media.Color.FromArgb");
			string text3 = "System.Drawing.Color.";
			while (true)
			{
				int num = text2.IndexOf(text3, StringComparison.Ordinal);
				if (num == -1)
				{
					break;
				}
				int num2 = num + text3.Length;
				string text4 = "";
				while (true)
				{
					char c = text2[num2];
					if (!char.IsLetter(c))
					{
						break;
					}
					text4 += c;
					num2++;
				}
				Color color = Color.FromName(text4);
				string text5 = text3 + text4;
				string text6 = $"System.Windows.Media.Color.FromArgb({color.A}, {color.R}, {color.G}, {color.B})";
				int length = text5.Length;
				int length2 = text6.Length;
				int startIndex = 0;
				while (true)
				{
					startIndex = text2.IndexOf(text5, startIndex, StringComparison.Ordinal);
					if (startIndex < 0)
					{
						break;
					}
					if (startIndex + length < text2.Length && !char.IsLetter(text2[startIndex + length]))
					{
						text2 = text2.Substring(0, startIndex) + text6 + text2.Substring(startIndex + length);
						startIndex += length2;
					}
					else
					{
						startIndex += length;
					}
				}
			}
			return text2;
		}
		finally
		{
			Dictionary.Relations = relations;
			Dictionary.DataSources = dataSources;
			Dictionary.Databases = databases;
		}
	}

	public StiReport SaveEditableFields(string path)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(path);
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			SaveEditableFields(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving the editable fields of rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		return this;
	}

	public StiReport SaveEditableFields(Stream stream)
	{
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		StiLogService.Write(typeof(StiReport), "Saving the editable fields of rendered report.");
		try
		{
			StiEditableItemsContainer stiEditableItemsContainer = new StiEditableItemsContainer();
			int num = 0;
			foreach (StiPage renderedPage in RenderedPages)
			{
				Hashtable hashtable = new Hashtable();
				StiComponentsCollection components = renderedPage.GetComponents();
				foreach (StiComponent item in components)
				{
					if (item is IStiEditable { Editable: not false } stiEditable)
					{
						int num2 = 1;
						if (hashtable[item.Name] != null)
						{
							num2 = (int)hashtable[item.Name] + 1;
						}
						stiEditableItemsContainer.Items.Add(new StiEditableItem(num, num2, item.Name, stiEditable.SaveState()));
						hashtable[item.Name] = num2;
					}
				}
				num++;
			}
			StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
			val.Serialize((object)stiEditableItemsContainer, stream, "Editable Fields of Rendered Report");
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving the editable fields of rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return this;
	}

	public StiReport LoadEditableFields(string path)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			LoadEditableFields(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading the editable fields of rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		return this;
	}

	public StiReport LoadEditableFields(Stream stream)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		StiLogService.Write(typeof(StiReport), "Loading the editable fields of rendered report.");
		try
		{
			StiEditableItemsContainer stiEditableItemsContainer = new StiEditableItemsContainer();
			StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
			val.Deserialize((object)stiEditableItemsContainer, stream, "Editable Fields of Rendered Report");
			foreach (StiEditableItem item in stiEditableItemsContainer.Items)
			{
				int pageIndex = item.PageIndex;
				int num = item.Position;
				if (pageIndex >= RenderedPages.Count)
				{
					continue;
				}
				StiPage stiPage = RenderedPages[pageIndex];
				StiComponentsCollection components = stiPage.GetComponents();
				foreach (StiComponent item2 in components)
				{
					if (item2 is IStiEditable { Editable: not false } stiEditable && item2.Name == item.ComponentName)
					{
						if (num == 1)
						{
							stiEditable.RestoreState(item.TextValue);
							break;
						}
						num--;
					}
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading the editable fields of rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return this;
	}

	public static StiReport[] GetReportsFromAssembly(Assembly assembly)
	{
		try
		{
			List<StiReport> list = new List<StiReport>();
			Type[] types = assembly.GetTypes();
			Type[] array = types;
			foreach (Type type in array)
			{
				if (type.IsSubclassOf(typeof(StiReport)))
				{
					StiReport stiReport = StiActivator.CreateObject(type) as StiReport;
					StiReportResourcesLoader.LoadReportResourcesFromAssembly(stiReport);
					stiReport.ApplyStyles();
					list.Add(stiReport);
				}
			}
			return list.ToArray();
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiReport), "Get report from assembly...ERROR");
			StiLogService.Write(typeof(StiReport), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return null;
	}

	public static StiReport GetReportFromAssembly(Assembly assembly)
	{
		try
		{
			StiReport stiReport = GetReportsFromAssembly(assembly)[0];
			stiReport.UpdateReportVersion();
			return stiReport;
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiReport), "Get report from assembly...ERROR");
			StiLogService.Write(typeof(StiReport), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return null;
	}

	public static StiReport GetReportFromAssembly(string assemblyFile)
	{
		return GetReportFromAssembly(assemblyFile, lockFile: false);
	}

	public static StiReport GetReportFromAssembly(string assemblyFile, bool lockFile)
	{
		if (lockFile)
		{
			try
			{
				Assembly assembly = Assembly.LoadFrom(assemblyFile);
				return GetReportFromAssembly(assembly);
			}
			catch (Exception e)
			{
				StiLogService.Write(typeof(StiReport), "Get report from assembly...ERROR");
				StiLogService.Write(typeof(StiReport), e);
				if (!StiOptions.Engine.HideExceptions)
				{
					throw;
				}
			}
		}
		else
		{
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(assemblyFile, FileMode.Open, FileAccess.Read, FileShare.Read);
				return GetReportFromAssembly(fileStream);
			}
			catch (Exception e2)
			{
				StiLogService.Write(typeof(StiReport), "Get report from assembly...ERROR");
				StiLogService.Write(typeof(StiReport), e2);
				if (!StiOptions.Engine.HideExceptions)
				{
					throw;
				}
			}
			finally
			{
				fileStream.Close();
			}
		}
		return null;
	}

	public static StiReport GetReportFromAssembly(Stream assemblyStream)
	{
		try
		{
			byte[] array = new byte[assemblyStream.Length];
			assemblyStream.Seek(0L, SeekOrigin.Begin);
			assemblyStream.Read(array, 0, (int)assemblyStream.Length);
			Assembly assembly = Assembly.Load(array);
			return GetReportsFromAssembly(assembly)[0];
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiReport), "Get report from assembly...ERROR");
			StiLogService.Write(typeof(StiReport), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return null;
	}

	public static StiReport GetReportFromAssembly(byte[] bytes)
	{
		MemoryStream memoryStream = new MemoryStream(bytes);
		StiReport reportFromAssembly = GetReportFromAssembly(memoryStream);
		reportFromAssembly.IsDocument = false;
		memoryStream.Close();
		return reportFromAssembly;
	}

	public StiReport LoadDocumentFromJson(string json)
	{
		LoadFromJsonInternal(json);
		IsRendered = true;
		NeedsCompiling = false;
		IsDocument = true;
		LoadDocumentFonts();
		return this;
	}

	public StiReport LoadDocumentFromJson(Stream stream)
	{
		using StreamReader streamReader = new StreamReader(stream);
		LoadDocumentFromJson(streamReader.ReadToEnd());
		return this;
	}

	public virtual StiReport LoadEncryptedDocument(Stream stream, string key)
	{
		if (!stream.CanSeek)
		{
			byte[] buffer = new byte[stream.Length - stream.Position];
			stream.Read(buffer, (int)stream.Position, (int)(stream.Length - stream.Position));
			stream = new MemoryStream(buffer);
		}
		StiEncryptedDocumentSLService service = new StiEncryptedDocumentSLService
		{
			Key = key
		};
		LoadDocument(service, stream);
		return this;
	}

	public virtual StiReport LoadEncryptedDocument(string path, string key)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			LoadEncryptedDocument(fileStream, key);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading encrypted rendered report from file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport LoadEncryptedDocumentFromString(string reportStr, string key)
	{
		byte[] array = System.Convert.FromBase64String(reportStr);
		if (array[0] != 109 || array[1] != 100 || array[2] != 120)
		{
			throw new Exception("This file is a not '.mdx' format.");
		}
		byte[] array2 = new byte[array.Length - 3];
		Array.Copy(array, 3, array2, 0, array.Length - 3);
		array2 = StiEncryption.Decrypt(array2, key);
		try
		{
			array2 = StiGZipHelper.Unpack(array2);
		}
		catch
		{
			throw new Exception("File decryption error: wrong key.");
		}
		string @string = Encoding.UTF8.GetString(array2);
		LoadDocumentFromString(@string);
		return this;
	}

	public virtual StiReport LoadEncryptedDocument(byte[] bytes, string key)
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream(bytes);
			LoadEncryptedDocument(memoryStream, key);
			return this;
		}
		finally
		{
			memoryStream?.Close();
		}
	}

	public virtual StiReport LoadPackedDocument(Stream stream)
	{
		if (!stream.CanSeek)
		{
			byte[] buffer = new byte[stream.Length - stream.Position];
			stream.Read(buffer, (int)stream.Position, (int)(stream.Length - stream.Position));
			stream = new MemoryStream(buffer);
		}
		if (!IsPackedFile(stream))
		{
			LoadDocument(stream);
		}
		else
		{
			StiPackedDocumentSLService service = new StiPackedDocumentSLService();
			LoadDocument(service, stream);
		}
		return this;
	}

	public virtual StiReport LoadPackedDocument(string path)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			LoadPackedDocument(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading packed rendered report from file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport LoadPackedDocumentFromString(string reportStr)
	{
		string reportStr2 = StiGZipHelper.Unpack(reportStr);
		LoadDocumentFromString(reportStr2);
		return this;
	}

	public virtual StiReport LoadPackedDocument(byte[] bytes)
	{
		if (!IsPackedFile(bytes))
		{
			LoadDocument(bytes);
		}
		else
		{
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream(bytes);
				LoadPackedDocument(memoryStream);
			}
			finally
			{
				memoryStream?.Close();
			}
		}
		return this;
	}

	public virtual StiReport LoadDocumentFromString(string reportStr)
	{
		MemoryStream memoryStream = null;
		StreamWriter streamWriter = null;
		try
		{
			memoryStream = new MemoryStream();
			streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(reportStr);
			streamWriter.Flush();
			memoryStream.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			LoadDocument(memoryStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading rendered report from string");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			streamWriter?.Close();
			memoryStream?.Close();
		}
		return this;
	}

	public virtual StiReport LoadDocument(byte[] bytes)
	{
		if (IsPackedFile(bytes))
		{
			LoadPackedDocument(bytes);
		}
		else
		{
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream(bytes);
				LoadDocument(memoryStream);
			}
			finally
			{
				memoryStream?.Close();
			}
		}
		return this;
	}

	public virtual StiReport LoadDocument(Stream stream)
	{
		if (!stream.CanSeek)
		{
			byte[] buffer = new byte[stream.Length - stream.Position];
			stream.Read(buffer, (int)stream.Position, (int)(stream.Length - stream.Position));
			stream = new MemoryStream(buffer);
		}
		if (IsJsonFile(stream))
		{
			LoadDocumentFromJson(stream);
		}
		else if (IsPackedFile(stream))
		{
			LoadPackedDocument(stream);
		}
		else
		{
			StiXmlDocumentSLService service = new StiXmlDocumentSLService();
			LoadDocument(service, stream);
		}
		return this;
	}

	public virtual StiReport LoadDocument(string path)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			LoadDocument(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading rendered report from file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport LoadDocument(StiDocumentSLService service, Stream stream)
	{
		try
		{
			StiLogService.Write(typeof(StiReport), "Loading rendered report");
			service.Load(this, stream);
			IsRendered = true;
			NeedsCompiling = false;
			IsDocument = true;
			LoadDocumentFonts();
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return this;
	}

	public virtual StiReport LoadDocument(StiDocumentSLService service, string path)
	{
		StiLogService.Write(typeof(StiReport), "Loading rendered report");
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			service.Load(this, fileStream);
			IsRendered = true;
			NeedsCompiling = false;
			IsDocument = true;
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			fileStream.Close();
		}
		return this;
	}

	public virtual StiReport LoadDocumentFromUrl(string url)
	{
		using WebClient webClient = new WebClient();
		webClient.Proxy = StiProxy.GetProxy();
		webClient.Credentials = CredentialCache.DefaultCredentials;
		byte[] buffer = webClient.DownloadData(url);
		MemoryStream stream = new MemoryStream(buffer);
		LoadDocument(stream);
		return this;
	}

	public StiReport LoadFromJson(string json)
	{
		Clear(generateNewScript: false);
		LoadFromJsonInternal(json);
		LoadFonts();
		return this;
	}

	public virtual StiReport LoadEncryptedReport(Stream stream, string key)
	{
		if (!stream.CanSeek)
		{
			byte[] buffer = new byte[stream.Length - stream.Position];
			stream.Read(buffer, (int)stream.Position, (int)(stream.Length - stream.Position));
			stream = new MemoryStream(buffer);
		}
		ReportFile = string.Empty;
		Clear();
		StiEncryptedReportSLService service = new StiEncryptedReportSLService
		{
			Key = key
		};
		StiOptions.Engine.GlobalEvents.InvokeReportLoading(this, EventArgs.Empty);
		Load(service, stream);
		StiOptions.Engine.GlobalEvents.InvokeReportLoaded(this, EventArgs.Empty);
		IsPackedReport = true;
		IsJsonReport = false;
		isModified = false;
		ReportFile = string.Empty;
		Password = key;
		return this;
	}

	public virtual StiReport LoadEncryptedReport(string path, string key)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			LoadEncryptedReport(fileStream, key);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading encrypted report from file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
			Password = key;
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport LoadEncryptedReportFromString(string reportStr, string key)
	{
		byte[] array = System.Convert.FromBase64String(reportStr);
		if (array[0] != 109 || array[1] != 114 || array[2] != 120)
		{
			throw new Exception("This file is a not '.mrx' format.");
		}
		byte[] array2 = new byte[array.Length - 3];
		Array.Copy(array, 3, array2, 0, array.Length - 3);
		array2 = StiEncryption.Decrypt(array2, key);
		try
		{
			array2 = StiGZipHelper.Unpack(array2);
		}
		catch
		{
			throw new Exception("File decryption error: wrong key.");
		}
		string @string = Encoding.UTF8.GetString(array2);
		LoadFromString(@string);
		Password = key;
		return this;
	}

	public virtual StiReport LoadEncryptedReport(byte[] bytes, string key)
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream(bytes);
			LoadEncryptedReport(memoryStream, key);
		}
		finally
		{
			memoryStream?.Close();
			Password = key;
		}
		return this;
	}

	public virtual StiReport LoadPackedReport(Stream stream)
	{
		if (!stream.CanSeek)
		{
			byte[] buffer = new byte[stream.Length - stream.Position];
			stream.Read(buffer, (int)stream.Position, (int)(stream.Length - stream.Position));
			stream = new MemoryStream(buffer);
		}
		if (!IsPackedFile(stream))
		{
			Load(stream);
		}
		else
		{
			ReportFile = string.Empty;
			Clear();
			StiPackedReportSLService service = new StiPackedReportSLService();
			StiOptions.Engine.GlobalEvents.InvokeReportLoading(this, EventArgs.Empty);
			Load(service, stream);
			StiOptions.Engine.GlobalEvents.InvokeReportLoaded(this, EventArgs.Empty);
			IsPackedReport = true;
			isModified = false;
			ReportFile = string.Empty;
		}
		return this;
	}

	public virtual StiReport LoadPackedReport(string path)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			LoadPackedReport(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading packed report from file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport LoadPackedReportFromString(string reportStr)
	{
		string reportStr2 = StiGZipHelper.Unpack(reportStr);
		LoadFromString(reportStr2);
		return this;
	}

	public virtual StiReport LoadPackedReport(byte[] bytes)
	{
		if (!IsPackedFile(bytes))
		{
			Load(bytes);
		}
		else
		{
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream(bytes);
				LoadPackedReport(memoryStream);
			}
			finally
			{
				memoryStream?.Close();
			}
		}
		return this;
	}

	public virtual StiReport Load(Stream stream)
	{
		if (!stream.CanSeek)
		{
			byte[] buffer = new byte[stream.Length - stream.Position];
			stream.Read(buffer, (int)stream.Position, (int)(stream.Length - stream.Position));
			stream = new MemoryStream(buffer);
		}
		if (IsPackedFile(stream))
		{
			LoadPackedReport(stream);
		}
		else
		{
			ReportFile = string.Empty;
			IsJsonReport = false;
			Clear(generateNewScript: false);
			StiReportSLService service;
			if (IsJsonFile(stream))
			{
				service = new StiJsonReportSLService();
				IsJsonReport = true;
			}
			else
			{
				service = new StiXmlReportSLService();
			}
			Load(service, stream);
			IsPackedReport = false;
			isModified = false;
			ReportFile = string.Empty;
			IsDocument = false;
		}
		return this;
	}

	public virtual StiReport Load(byte[] bytes)
	{
		if (IsPackedFile(bytes))
		{
			LoadPackedReport(bytes);
		}
		else
		{
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream(bytes);
				Load(memoryStream);
			}
			finally
			{
				memoryStream?.Close();
			}
		}
		return this;
	}

	public virtual StiReport Load(string path)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			Load(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading report from file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport LoadFromString(string reportStr)
	{
		MemoryStream memoryStream = null;
		StreamWriter streamWriter = null;
		try
		{
			memoryStream = new MemoryStream();
			streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(reportStr);
			streamWriter.Flush();
			memoryStream.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			Load(memoryStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading report from string");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			streamWriter?.Close();
			memoryStream?.Close();
		}
		return this;
	}

	public virtual StiReport Load(StiReportSLService service, Stream stream)
	{
		try
		{
			Clear(generateNewScript: false);
			StiLogService.Write(typeof(StiReport), "Loading report");
			StiOptions.Engine.GlobalEvents.InvokeReportLoading(this, EventArgs.Empty);
			EngineVersion = (StiEngineVersion)1;
			service.Load(this, stream);
			StiOptions.Engine.GlobalEvents.InvokeReportLoaded(this, EventArgs.Empty);
			ApplyStyles();
			LoadFonts();
			CorrectFormatProperties();
			if (StiOptions.Engine.FullTrust && StiOptions.Engine.PackScriptAfterReportLoaded)
			{
				ScriptPack();
			}
			UpdateInheritedReport();
			IsJsonReport = service is StiJsonReportSLService;
			IsPackedReport = false;
			isModified = false;
			IsDocument = false;
			ReportFile = string.Empty;
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		return this;
	}

	private void CorrectFormatProperties()
	{
		StiComponentsCollection components = GetComponents();
		foreach (StiComponent item in components)
		{
			if (item is StiText stiText && !(stiText.TextFormat is StiGeneralFormatService))
			{
				StiFormatService textFormat = stiText.TextFormat;
				stiText.TextFormat = null;
				stiText.TextFormat = textFormat;
			}
		}
	}

	public virtual StiReport Load(StiReportSLService service, string path)
	{
		Clear(generateNewScript: false);
		StiLogService.Write(typeof(StiReport), "Loading report");
		FileStream fileStream = null;
		try
		{
			EngineVersion = (StiEngineVersion)1;
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			service.Load(this, fileStream);
			IsDocument = false;
			UpdateInheritedReport();
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport LoadFromUrl(string url)
	{
		using WebClient webClient = new WebClient();
		webClient.Proxy = StiProxy.GetProxy();
		webClient.Credentials = CredentialCache.DefaultCredentials;
		byte[] buffer = webClient.DownloadData(url);
		MemoryStream stream = new MemoryStream(buffer);
		Load(stream);
		return this;
	}

	internal string SaveToJsonInternal(StiJsonSaveMode mode)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Invalid comparison between Unknown and I4
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			JObject val = new JObject();
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ReportVersion", ReportVersion);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ReportGuid", ReportGuid);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ReportName", PropName);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ReportAlias", ReportAlias);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ReportAuthor", ReportAuthor);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ReportDescription", ReportDescription);
			JsonExtensions.AddPropertyDateTime(val, "ReportCreated", ReportCreated);
			JsonExtensions.AddPropertyDateTime(val, "ReportChanged", ReportChanged);
			JsonExtensions.AddPropertyEnum(val, "EngineVersion", (Enum)(object)EngineVersion);
			JsonExtensions.AddPropertyEnum(val, "NumberOfPass", (Enum)NumberOfPass, (Enum)StiNumberOfPass.SinglePass);
			JsonExtensions.AddPropertyEnum(val, "CalculationMode", (Enum)CalculationMode, (Enum)StiCalculationMode.Compilation);
			JsonExtensions.AddPropertyEnum(val, "ReportUnit", (Enum)ReportUnit, (Enum)StiReportUnitType.Centimeters);
			JsonExtensions.AddPropertyBool(val, "CacheAllData", CacheAllData, false, false);
			JsonExtensions.AddPropertyEnum(val, "ReportCacheMode", (Enum)ReportCacheMode, (Enum)StiReportCacheMode.Off);
			JsonExtensions.AddPropertyBool(val, "ConvertNulls", ConvertNulls, true, false);
			JsonExtensions.AddPropertyBool(val, "RetrieveOnlyUsedData", RetrieveOnlyUsedData, false, false);
			JsonExtensions.AddPropertyEnum(val, "PreviewMode", (Enum)PreviewMode, (Enum)StiPreviewMode.Standard);
			JsonExtensions.AddPropertyInt(val, "StopBeforePage", StopBeforePage, 0);
			JsonExtensions.AddPropertyInt(val, "PreviewSettings", PreviewSettings, 260039167);
			JsonExtensions.AddPropertyInt(val, "DashboardViewerSettings", (int)DashboardViewerSettings, 255);
			JsonExtensions.AddPropertyInt(val, "Collate", Collate, 1);
			JsonExtensions.AddPropertyEnum(val, "ScriptLanguage", (Enum)ScriptLanguage, (Enum)StiReportLanguageType.CSharp);
			JsonExtensions.AddPropertyBool(val, "AutoLocalizeReportOnRun", AutoLocalizeReportOnRun, false, false);
			JsonExtensions.AddPropertyEnum(val, "ParametersOrientation", (Enum)ParametersOrientation, (Enum)StiOrientation.Horizontal);
			JsonExtensions.AddPropertyBool(val, "RequestParameters", RequestParameters, false, false);
			JsonExtensions.AddPropertyBool(val, "CacheTotals", CacheTotals, false, false);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "Culture", Culture);
			if (ReportIcon != null)
			{
				JsonExtensions.AddPropertyByteArray(val, "ReportIcon", ReportIcon);
			}
			if (ReportImage != null)
			{
				JsonExtensions.AddPropertyImage(val, "ReportImage", ReportImage);
			}
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "Script", Script);
			if ((int)mode == 1)
			{
				JsonExtensions.AddPropertyJObject(val, "RenderedPages", RenderedPages.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "Styles", Styles.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "Resources", Dictionary.Resources.SaveToJsonObjectEx(mode));
			}
			else
			{
				JsonExtensions.AddPropertyInt(val, "RefreshTime", RefreshTime, 0);
				JsonExtensions.AddPropertyJObject(val, "PrinterSettings", PrinterSettings.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "MetaTags", MetaTags.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "Styles", Styles.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyStringArray(val, "ReferencedAssemblies", ReferencedAssemblies);
				JsonExtensions.AddPropertyJObject(val, "BeginRenderEvent", BeginRenderEvent.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "RenderingEvent", RenderingEvent.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "EndRenderEvent", EndRenderEvent.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "ExportingEvent", ExportingEvent.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "ExportedEvent", ExportedEvent.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "PrintingEvent", PrintingEvent.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "PrintedEvent", PrintedEvent.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "RefreshingEvent", RefreshingEvent.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "ReportResources", ReportResources.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "GlobalizationStrings", GlobalizationStrings.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "Dictionary", Dictionary.SaveToJsonObject(mode));
				JsonExtensions.AddPropertyJObject(val, "Pages", Pages.SaveToJsonObject(mode));
			}
			return ((JToken)val).ToString(new CultureInfo("en-us"));
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	internal void LoadFromJsonInternal(string text)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0bf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c01: Expected O, but got Unknown
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Expected O, but got Unknown
		//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1d: Expected O, but got Unknown
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc9: Expected O, but got Unknown
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acd: Expected O, but got Unknown
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b91: Expected O, but got Unknown
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b59: Expected O, but got Unknown
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Expected O, but got Unknown
		//IL_0c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Expected O, but got Unknown
		//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Expected O, but got Unknown
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Expected O, but got Unknown
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Expected O, but got Unknown
		//IL_0b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Expected O, but got Unknown
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6e: Expected O, but got Unknown
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			jsonLoaderHelper = new StiReportJsonLoaderHelper();
			Pages.Clear();
			Dictionary.Clear();
			renderedPages.Clear();
			ReportUnit = StiReportUnitType.Centimeters;
			JObject val = (JObject)JsonConvert.DeserializeObject(text);
			foreach (JProperty item in val.Properties())
			{
				switch (item.Name)
				{
				case "ReportVersion":
					ReportVersion = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "ReportGuid":
					ReportGuid = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "ReportName":
					PropName = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "ReportAlias":
					ReportAlias = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "ReportFile":
					ReportFile = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "ReportAuthor":
					ReportAuthor = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "RetrieveOnlyUsedData":
					RetrieveOnlyUsedData = StiJsonReportObjectHelper.DeserializeBool(item);
					break;
				case "ReportIcon":
					ReportIcon = StiJsonReportObjectHelper.DeserializeByteArray(item);
					break;
				case "ReportImage":
					ReportImage = StiJsonReportObjectHelper.DeserializeImage(item);
					break;
				case "ReportDescription":
					ReportDescription = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "ReportCreated":
					ReportCreated = StiJsonReportObjectHelper.DeserializeDateTime(item);
					break;
				case "ReportChanged":
					ReportChanged = StiJsonReportObjectHelper.DeserializeDateTime(item);
					break;
				case "EngineVersion":
					EngineVersion = StiJsonReportObjectHelper.DeserializeEnum<StiEngineVersion>(item);
					break;
				case "NumberOfPass":
					NumberOfPass = StiJsonReportObjectHelper.DeserializeEnum<StiNumberOfPass>(item);
					break;
				case "CalculationMode":
					CalculationMode = StiJsonReportObjectHelper.DeserializeEnum<StiCalculationMode>(item);
					break;
				case "ReportUnit":
					ReportUnit = StiJsonReportObjectHelper.DeserializeEnum<StiReportUnitType>(item);
					break;
				case "CacheAllData":
					CacheAllData = StiJsonReportObjectHelper.DeserializeBool(item);
					break;
				case "ReportCacheMode":
					ReportCacheMode = StiJsonReportObjectHelper.DeserializeEnum<StiReportCacheMode>(item);
					break;
				case "ConvertNulls":
					ConvertNulls = StiJsonReportObjectHelper.DeserializeBool(item);
					break;
				case "PreviewMode":
					PreviewMode = StiJsonReportObjectHelper.DeserializeEnum<StiPreviewMode>(item);
					break;
				case "StopBeforePage":
					StopBeforePage = StiJsonReportObjectHelper.DeserializeInt(item);
					break;
				case "PreviewSettings":
					PreviewSettings = StiJsonReportObjectHelper.DeserializeInt(item);
					break;
				case "DashboardViewerSettings":
					DashboardViewerSettings = (StiDashboardViewerSettings)StiJsonReportObjectHelper.DeserializeInt(item);
					break;
				case "Collate":
					Collate = StiJsonReportObjectHelper.DeserializeInt(item);
					break;
				case "ReferencedAssemblies":
					ReferencedAssemblies = StiJsonReportObjectHelper.DeserializeStringArray(item);
					break;
				case "ScriptLanguage":
					ScriptLanguage = StiJsonReportObjectHelper.DeserializeEnum<StiReportLanguageType>(item);
					break;
				case "AutoLocalizeReportOnRun":
					AutoLocalizeReportOnRun = StiJsonReportObjectHelper.DeserializeBool(item);
					break;
				case "ParametersOrientation":
					ParametersOrientation = StiJsonReportObjectHelper.DeserializeEnum<StiOrientation>(item);
					break;
				case "RequestParameters":
					RequestParameters = StiJsonReportObjectHelper.DeserializeBool(item);
					break;
				case "CacheTotals":
					CacheTotals = StiJsonReportObjectHelper.DeserializeBool(item);
					break;
				case "Culture":
					Culture = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "Script":
					Script = StiJsonReportObjectHelper.DeserializeString(item);
					break;
				case "BeginRenderEvent":
					BeginRenderEvent.LoadFromJsonObject((JObject)item.Value);
					break;
				case "RenderingEvent":
					RenderingEvent.LoadFromJsonObject((JObject)item.Value);
					break;
				case "EndRenderEvent":
					EndRenderEvent.LoadFromJsonObject((JObject)item.Value);
					break;
				case "ExportingEvent":
					ExportingEvent.LoadFromJsonObject((JObject)item.Value);
					break;
				case "ExportedEvent":
					ExportedEvent.LoadFromJsonObject((JObject)item.Value);
					break;
				case "PrintingEvent":
					PrintingEvent.LoadFromJsonObject((JObject)item.Value);
					break;
				case "PrintedEvent":
					PrintedEvent.LoadFromJsonObject((JObject)item.Value);
					break;
				case "RefreshingEvent":
					RefreshingEvent.LoadFromJsonObject((JObject)item.Value);
					break;
				case "MetaTags":
					MetaTags.LoadFromJsonObject((JObject)item.Value);
					break;
				case "ReportResources":
					ReportResources.LoadFromJsonObject((JObject)item.Value);
					break;
				case "GlobalizationStrings":
					GlobalizationStrings.LoadFromJsonObject((JObject)item.Value);
					break;
				case "PrinterSettings":
					PrinterSettings.LoadFromJsonObject((JObject)item.Value);
					break;
				case "Styles":
					Styles.LoadFromJsonObject((JObject)item.Value);
					break;
				case "Dictionary":
					Dictionary.LoadFromJsonObject((JObject)item.Value);
					break;
				case "Resources":
					Dictionary.Resources.LoadFromJsonObjectEx((JObject)item.Value, this);
					break;
				case "Pages":
					Pages.LoadFromJsonObject((JObject)item.Value);
					break;
				case "RenderedPages":
					RenderedPages.LoadFromJsonObject((JObject)item.Value);
					break;
				case "RefreshTime":
					RefreshTime = StiJsonReportObjectHelper.DeserializeInt(item);
					break;
				case "RenderedWith":
					RenderedWith = StiJsonReportObjectHelper.DeserializeEnum<StiRenderedWith>(item);
					break;
				default:
					throw new Exception("Property is not supported!");
				}
			}
			StiComponentsCollection stiComponentsCollection = null;
			if (jsonLoaderHelper.MasterComponents.Count > 0)
			{
				stiComponentsCollection = GetComponents();
				foreach (IStiMasterComponent masterComponent in jsonLoaderHelper.MasterComponents)
				{
					if (masterComponent is StiChart stiChart)
					{
						stiChart.MasterComponent = stiComponentsCollection[stiChart.jsonMasterComponentTemp];
						stiChart.jsonMasterComponentTemp = null;
					}
					else if (masterComponent is StiDataBand stiDataBand)
					{
						stiDataBand.MasterComponent = stiComponentsCollection[stiDataBand.jsonMasterComponentTemp];
						stiDataBand.jsonMasterComponentTemp = null;
					}
				}
			}
			if (jsonLoaderHelper.Clones.Count > 0)
			{
				if (stiComponentsCollection == null)
				{
					stiComponentsCollection = GetComponents();
				}
				foreach (StiClone clone in jsonLoaderHelper.Clones)
				{
					clone.Container = stiComponentsCollection[clone.jsonContainerValueTemp] as StiContainer;
					clone.jsonContainerValueTemp = null;
				}
			}
			if (jsonLoaderHelper.DialogInfo.Count > 0)
			{
				foreach (StiDialogInfo item2 in jsonLoaderHelper.DialogInfo)
				{
					item2.BindingVariable = Dictionary.Variables[item2.jsonLoadedBindingVariableName];
					item2.jsonLoadedBindingVariableName = null;
				}
			}
			jsonLoaderHelper.Clean();
			jsonLoaderHelper = null;
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Loading report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public string SaveToJsonString()
	{
		return SaveToJsonInternal((StiJsonSaveMode)0);
	}

	public virtual StiReport SaveEncryptedReport(Stream stream, string key)
	{
		StiEncryptedReportSLService service = new StiEncryptedReportSLService
		{
			Key = key
		};
		StiOptions.Engine.GlobalEvents.InvokeReportSaving(this, EventArgs.Empty);
		Save(service, stream);
		StiOptions.Engine.GlobalEvents.InvokeReportSaved(this, EventArgs.Empty);
		ReportFile = string.Empty;
		IsModified = false;
		Password = key;
		return this;
	}

	public virtual StiReport SaveEncryptedReport(string path, string key)
	{
		FileStream fileStream = null;
		try
		{
			if (!IsDesigning || !StiOptions.Designer.ReadOnlyAlertOnSave)
			{
				StiFileUtils.ProcessReadOnly(path);
			}
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			SaveEncryptedReport(fileStream, key);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving encrypted report to file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
			Password = key;
		}
		ReportFile = path;
		return this;
	}

	public virtual byte[] SaveEncryptedReportToByteArray(string key)
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream();
			SaveEncryptedReport(memoryStream, key);
		}
		finally
		{
			if (memoryStream != null)
			{
				memoryStream.Flush();
				memoryStream.Close();
			}
		}
		return memoryStream.ToArray();
	}

	public virtual string SaveEncryptedReportToString(string key)
	{
		string s = SaveToString();
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		bytes = StiEncryption.Encrypt(StiGZipHelper.Pack(bytes), key);
		byte[] array = new byte[bytes.Length + 3];
		array[0] = 109;
		array[1] = 114;
		array[2] = 120;
		Array.Copy(bytes, 0, array, 3, bytes.Length);
		return System.Convert.ToBase64String(array);
	}

	public virtual StiReport SavePackedReport(Stream stream)
	{
		StiPackedReportSLService service = new StiPackedReportSLService();
		StiOptions.Engine.GlobalEvents.InvokeReportSaving(this, EventArgs.Empty);
		Save(service, stream);
		StiOptions.Engine.GlobalEvents.InvokeReportSaved(this, EventArgs.Empty);
		ReportFile = string.Empty;
		IsModified = false;
		return this;
	}

	public virtual StiReport SavePackedReport(string path)
	{
		FileStream fileStream = null;
		try
		{
			if (!IsDesigning || !StiOptions.Designer.ReadOnlyAlertOnSave)
			{
				StiFileUtils.ProcessReadOnly(path);
			}
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			SavePackedReport(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving packed report to file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual byte[] SavePackedReportToByteArray()
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream();
			SavePackedReport(memoryStream);
		}
		finally
		{
			if (memoryStream != null)
			{
				memoryStream.Flush();
				memoryStream.Close();
			}
		}
		return memoryStream.ToArray();
	}

	public virtual string SavePackedReportToString()
	{
		string text = SaveToString();
		return StiGZipHelper.Pack(text);
	}

	public virtual StiReport Save(Stream stream)
	{
		StiReportSLService service = ((!IsJsonReport) ? ((StiReportSLService)new StiXmlReportSLService()) : ((StiReportSLService)new StiJsonReportSLService()));
		Save(service, stream);
		ReportFile = string.Empty;
		IsModified = false;
		return this;
	}

	public virtual StiReport Save(StiReportSLService service, Stream stream)
	{
		try
		{
			UpdateReportVersion();
			StiLogService.Write(typeof(StiReport), "Saving report");
			StiOptions.Engine.GlobalEvents.InvokeReportSaving(this, EventArgs.Empty);
			service.Save(this, stream);
			StiOptions.Engine.GlobalEvents.InvokeReportSaved(this, EventArgs.Empty);
			IsModified = false;
			ReportFile = string.Empty;
			IsJsonReport = service is StiJsonReportSLService;
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		return this;
	}

	public virtual StiReport Save(StiReportSLService service, string path)
	{
		UpdateReportVersion();
		StiLogService.Write(typeof(StiReport), "Saving report");
		FileStream fileStream = null;
		try
		{
			if (!IsDesigning || !StiOptions.Designer.ReadOnlyAlertOnSave)
			{
				StiFileUtils.ProcessReadOnly(path);
			}
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			service.Save(this, fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		IsModified = false;
		IsJsonReport = service is StiJsonReportSLService;
		return this;
	}

	public virtual StiReport Save(string path)
	{
		FileStream fileStream = null;
		try
		{
			if (!IsDesigning || !StiOptions.Designer.ReadOnlyAlertOnSave)
			{
				StiFileUtils.ProcessReadOnly(path);
			}
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			Save(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report to file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport SaveSnapshot(string path)
	{
		FileStream fileStream = null;
		try
		{
			if (!IsDesigning || !StiOptions.Designer.ReadOnlyAlertOnSave)
			{
				StiFileUtils.ProcessReadOnly(path);
			}
			StiDataResourceHelper.SaveSnapshot((IStiApp)(object)this);
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			Save(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report to file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport SaveSnapshot(Stream stream)
	{
		try
		{
			StiDataResourceHelper.SaveSnapshot((IStiApp)(object)this);
			Save(stream);
			ReportFile = string.Empty;
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report template with embedded data ...ERROR");
			StiLogService.Write(GetType(), e);
		}
		return this;
	}

	public virtual byte[] SaveSnapshotToByteArray()
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream();
			SaveSnapshot(memoryStream);
		}
		finally
		{
			memoryStream?.Flush();
			memoryStream?.Close();
		}
		return memoryStream?.ToArray();
	}

	public virtual StiReport SaveToJson(string path)
	{
		FileStream fileStream = null;
		try
		{
			if (!IsDesigning || !StiOptions.Designer.ReadOnlyAlertOnSave)
			{
				StiFileUtils.ProcessReadOnly(path);
			}
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			StiJsonReportSLService service = new StiJsonReportSLService();
			StiOptions.Engine.GlobalEvents.InvokeReportSaving(this, EventArgs.Empty);
			Save(service, fileStream);
			StiOptions.Engine.GlobalEvents.InvokeReportSaved(this, EventArgs.Empty);
			ReportFile = string.Empty;
			IsModified = false;
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report to file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual byte[] SaveToByteArray()
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream();
			Save(memoryStream);
		}
		finally
		{
			if (memoryStream != null)
			{
				memoryStream.Flush();
				memoryStream.Close();
			}
		}
		return memoryStream.ToArray();
	}

	public virtual string SaveToString()
	{
		MemoryStream memoryStream = null;
		StreamReader streamReader = null;
		string result = null;
		try
		{
			memoryStream = new MemoryStream();
			streamReader = new StreamReader(memoryStream);
			Save(memoryStream);
			memoryStream.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			result = streamReader.ReadToEnd();
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving report to string");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			streamReader?.Close();
			memoryStream?.Close();
		}
		return result;
	}

	public string SaveDocumentJsonToString()
	{
		return SaveToJsonInternal((StiJsonSaveMode)1);
	}

	public virtual StiReport SaveEncryptedDocument(Stream stream, string key)
	{
		StiEncryptedDocumentSLService service = new StiEncryptedDocumentSLService
		{
			Key = key
		};
		SaveDocument(service, stream);
		ReportFile = string.Empty;
		return this;
	}

	public virtual string SaveEncryptedDocumentToString(string key)
	{
		string s = SaveDocumentToString();
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		bytes = StiEncryption.Encrypt(StiGZipHelper.Pack(bytes), key);
		byte[] array = new byte[bytes.Length + 3];
		array[0] = 109;
		array[1] = 100;
		array[2] = 120;
		Array.Copy(bytes, 0, array, 3, bytes.Length);
		return System.Convert.ToBase64String(array);
	}

	public virtual byte[] SaveEncryptedDocumentToByteArray(string key)
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream();
			SaveEncryptedDocument(memoryStream, key);
		}
		finally
		{
			if (memoryStream != null)
			{
				memoryStream.Flush();
				memoryStream.Close();
			}
		}
		return memoryStream.ToArray();
	}

	public virtual StiReport SaveEncryptedDocument(string path, string key)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(path);
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			SaveEncryptedDocument(fileStream, key);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving encrypted rendered report to file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport SavePackedDocument(Stream stream)
	{
		StiPackedDocumentSLService service = new StiPackedDocumentSLService();
		SaveDocument(service, stream);
		ReportFile = string.Empty;
		return this;
	}

	public virtual string SavePackedDocumentToString()
	{
		string text = SaveDocumentToString();
		return StiGZipHelper.Pack(text);
	}

	public virtual byte[] SavePackedDocumentToByteArray()
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream();
			SavePackedDocument(memoryStream);
		}
		finally
		{
			if (memoryStream != null)
			{
				memoryStream.Flush();
				memoryStream.Close();
			}
		}
		return memoryStream.ToArray();
	}

	public virtual StiReport SavePackedDocument(string path)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(path);
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			SavePackedDocument(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving packed rendered report to file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport SaveDocument(Stream stream)
	{
		StiXmlDocumentSLService service = new StiXmlDocumentSLService();
		SaveDocument(service, stream);
		ReportFile = string.Empty;
		return this;
	}

	public virtual string SaveDocumentToString()
	{
		MemoryStream memoryStream = null;
		StreamReader streamReader = null;
		string result = null;
		try
		{
			memoryStream = new MemoryStream();
			streamReader = new StreamReader(memoryStream);
			SaveDocument(memoryStream);
			memoryStream.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			result = streamReader.ReadToEnd();
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions || IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			streamReader.Close();
			memoryStream.Close();
		}
		return result;
	}

	public virtual byte[] SaveDocumentToByteArray()
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream();
			SaveDocument(memoryStream);
		}
		finally
		{
			if (memoryStream != null)
			{
				memoryStream.Flush();
				memoryStream.Close();
			}
		}
		return memoryStream.ToArray();
	}

	public virtual StiReport SaveDocument(StiDocumentSLService service, Stream stream)
	{
		try
		{
			StiLogService.Write(typeof(StiReport), "Saving rendered report");
			service.Save(this, stream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return this;
	}

	public virtual StiReport SaveDocument(StiDocumentSLService service, string path)
	{
		StiLogService.Write(typeof(StiReport), "Saving rendered report");
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(path);
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			service.Save(this, fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving rendered report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public virtual StiReport SaveDocument(string path)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(path);
			fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			SaveDocument(fileStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Saving rendered report to file '" + path + "'...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			fileStream?.Close();
		}
		ReportFile = path;
		return this;
	}

	public StiReport Show()
	{
		return Show((Form)null, dialogForm: false);
	}

	public StiReport Show(bool dialogForm)
	{
		return Show((Form)null, dialogForm);
	}

	public StiReport Show(Form parentForm)
	{
		return Show(parentForm, dialogForm: false);
	}

	public StiReport Show(IWin32Window owner)
	{
		return Show(null, owner, dialogForm: true);
	}

	public StiReport Show(IWin32Window owner, bool dialogForm)
	{
		return Show(null, owner, dialogForm);
	}

	private StiReport Show(Form parentForm, bool dialogForm)
	{
		return Show(parentForm, null, dialogForm);
	}

	private StiReport Show(Form parentForm, IWin32Window win32Window, bool dialogForm)
	{
		if (!StiGuiOptions.AllowWinViewer && StiGuiOptions.AllowWpfViewer)
		{
			ShowWithWpf(parentForm, dialogForm);
			return this;
		}
		if (PreviewMode == StiPreviewMode.DotMatrix)
		{
			ShowDotMatrix(parentForm, win32Window, dialogForm);
		}
		else
		{
			RegReportDataSources();
			if (ContainsDashboard)
			{
				if (CheckNeedsCompiling())
				{
					Compile();
				}
			}
			else if (!IsRendered)
			{
				Render(showProgress: true);
			}
			StiLogService.Write(GetType(), "Showing report");
			if (CheckNeedsCompiling() && !IsCompiled)
			{
				throw new Exception("Can't showing report because report require compilation.");
			}
			try
			{
				if (!IsStopped)
				{
					StiReport report = (CheckNeedsCompiling() ? CompiledReport : this);
					if (ViewerControl != null)
					{
						ViewerControl.Report = report;
					}
					else
					{
						IStiViewerForm stiViewerForm;
						if (ViewerForm == null)
						{
							stiViewerForm = StiGuiOptions.GetViewerForm(report, (StiGuiMode)0);
							stiViewerForm.Report = report;
						}
						else
						{
							stiViewerForm = StiActivator.CreateObject(ViewerForm, new object[1]) as IStiViewerForm;
							stiViewerForm.Report = report;
						}
						if (parentForm != null && (parentForm.IsMdiContainer || parentForm.MdiParent != null))
						{
							stiViewerForm.TopLevel = false;
							stiViewerForm.ShowInTaskbar = false;
							stiViewerForm.ViewerOwner = stiViewerForm.GetOwner(parentForm) ?? parentForm;
							stiViewerForm.ShowViewer();
						}
						else if (dialogForm)
						{
							stiViewerForm.ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
							if (win32Window == null)
							{
								stiViewerForm.ShowDialogViewer();
							}
							else
							{
								stiViewerForm.ShowDialogViewer(win32Window);
							}
							if (stiViewerForm is IDisposable)
							{
								((IDisposable)stiViewerForm).Dispose();
							}
						}
						else
						{
							stiViewerForm.ShowViewer(((object)win32Window) ?? ((object)parentForm));
						}
					}
				}
			}
			catch (Exception e)
			{
				StiLogService.Write(GetType(), "Showing report...ERROR");
				StiLogService.Write(GetType(), e);
				if (!StiOptions.Engine.HideExceptions)
				{
					throw;
				}
			}
		}
		return this;
	}

	public StiReport ShowWithRibbonGUI()
	{
		return ShowWithRibbonGUI((Form)null, dialogForm: false);
	}

	public StiReport ShowWithRibbonGUI(bool dialogForm)
	{
		return ShowWithRibbonGUI((Form)null, dialogForm);
	}

	public StiReport ShowWithRibbonGUI(Form parentForm)
	{
		return ShowWithRibbonGUI(parentForm, dialogForm: false);
	}

	public StiReport ShowWithRibbonGUI(IWin32Window owner)
	{
		return ShowWithRibbonGUI(null, owner, dialogForm: true);
	}

	public StiReport ShowWithRibbonGUI(IWin32Window owner, bool dialogForm)
	{
		return ShowWithRibbonGUI(null, owner, dialogForm);
	}

	private StiReport ShowWithRibbonGUI(Form parentForm, bool dialogForm)
	{
		return ShowWithRibbonGUI(parentForm, null, dialogForm);
	}

	private StiReport ShowWithRibbonGUI(Form parentForm, IWin32Window win32Window, bool dialogForm)
	{
		if (ContainsDashboard)
		{
			throw new NotSupportedException("Viewer with Ribbon GUI doesn't supported for the dashboards!");
		}
		if (!StiGuiOptions.AllowWinViewer && StiGuiOptions.AllowWpfViewer)
		{
			ShowWithWpfRibbonGUI(parentForm, dialogForm);
			return this;
		}
		if (PreviewMode == StiPreviewMode.DotMatrix)
		{
			ShowDotMatrix(parentForm, win32Window, dialogForm);
		}
		else
		{
			RegReportDataSources();
			if (ContainsDashboard)
			{
				if (CheckNeedsCompiling())
				{
					Compile();
				}
			}
			else if (!IsRendered)
			{
				Render(showProgress: true);
			}
			StiLogService.Write(GetType(), "Showing report");
			if (CheckNeedsCompiling() && !IsCompiled)
			{
				throw new Exception("Can't showing report because report require compilation.");
			}
			try
			{
				if (!IsStopped)
				{
					StiReport report = (CheckNeedsCompiling() ? CompiledReport : this);
					if (ViewerControl != null)
					{
						ViewerControl.Report = report;
					}
					else
					{
						IStiViewerForm stiViewerForm;
						if (ViewerForm == null)
						{
							stiViewerForm = StiGuiOptions.GetViewerFormWithRibbonGUI(report, (StiGuiMode)0);
							stiViewerForm.Report = report;
						}
						else
						{
							Type type = Type.GetType("Stimulsoft.Report.Viewer.StiRibbonViewerForm, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
							if (type == null)
							{
								throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
							}
							stiViewerForm = StiActivator.CreateObject(type, new object[1]) as IStiViewerForm;
							stiViewerForm.Report = report;
						}
						if (parentForm != null && (parentForm.IsMdiContainer || parentForm.MdiParent != null))
						{
							stiViewerForm.TopLevel = false;
							stiViewerForm.ShowInTaskbar = false;
							stiViewerForm.ViewerOwner = stiViewerForm.GetOwner(parentForm) ?? parentForm;
							stiViewerForm.ShowViewer();
						}
						else if (dialogForm)
						{
							stiViewerForm.ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
							if (win32Window == null)
							{
								stiViewerForm.ShowDialogViewer();
							}
							else
							{
								stiViewerForm.ShowDialogViewer(win32Window);
							}
							if (stiViewerForm is IDisposable)
							{
								((IDisposable)stiViewerForm).Dispose();
							}
						}
						else
						{
							stiViewerForm.ShowViewer(((object)win32Window) ?? ((object)parentForm));
						}
					}
				}
			}
			catch (Exception e)
			{
				StiLogService.Write(GetType(), "Showing report...ERROR");
				StiLogService.Write(GetType(), e);
				if (!StiOptions.Engine.HideExceptions)
				{
					throw;
				}
			}
		}
		return this;
	}

	public StiReport ShowWithWpf()
	{
		return ShowWithWpf(dialogWindow: false);
	}

	public StiReport ShowWithWpf(bool dialogWindow)
	{
		return ShowWithWpf(null, dialogWindow);
	}

	public StiReport ShowWithWpf(object ownerWindow)
	{
		return ShowWithWpf(ownerWindow, dialogWindow: false);
	}

	public StiReport ShowWithWpf(object ownerWindow, bool dialogWindow)
	{
		StiOptions.Configuration.IsWPF = true;
		if (PreviewMode == StiPreviewMode.DotMatrix)
		{
			ShowDotMatrixWithWpf(dialogWindow);
		}
		else
		{
			RegReportDataSources();
			if (ContainsDashboard)
			{
				if (CheckNeedsCompiling())
				{
					Compile();
				}
			}
			else if (!IsRendered)
			{
				RenderWithWpf(showProgress: true);
			}
			StiLogService.Write(GetType(), "Showing report");
			if (CheckNeedsCompiling() && !IsCompiled)
			{
				throw new Exception("Can't showing report because report require compilation.");
			}
			try
			{
				if (!IsStopped)
				{
					StiReport report = (CheckNeedsCompiling() ? CompiledReport : this);
					if (ViewerControl != null)
					{
						ViewerControl.Report = report;
					}
					else
					{
						IStiViewerForm stiViewerForm;
						if (ViewerForm == null)
						{
							stiViewerForm = StiGuiOptions.GetViewerForm(report, (StiGuiMode)1);
							stiViewerForm.ViewerOwner = ownerWindow;
							stiViewerForm.Report = report;
						}
						else
						{
							stiViewerForm = StiActivator.CreateObject(ViewerForm, new object[1]) as IStiViewerForm;
							stiViewerForm.ViewerOwner = ownerWindow;
							stiViewerForm.Report = report;
						}
						if (dialogWindow)
						{
							stiViewerForm.ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
							stiViewerForm.ShowDialogViewer(ownerWindow);
						}
						else
						{
							stiViewerForm.ShowViewer(ownerWindow);
						}
					}
				}
			}
			catch (Exception e)
			{
				StiLogService.Write(GetType(), "Showing report...ERROR");
				StiLogService.Write(GetType(), e);
				if (!StiOptions.Engine.HideExceptions)
				{
					throw;
				}
			}
		}
		return this;
	}

	public StiReport ShowWithWpfRibbonGUI()
	{
		return ShowWithWpfRibbonGUI(dialogWindow: false);
	}

	public StiReport ShowWithWpfRibbonGUI(bool dialogWindow)
	{
		return ShowWithWpfRibbonGUI(null, dialogWindow);
	}

	public StiReport ShowWithWpfRibbonGUI(object ownerWindow)
	{
		return ShowWithWpfRibbonGUI(ownerWindow, dialogWindow: false);
	}

	public StiReport ShowWithWpfRibbonGUI(object ownerWindow, bool dialogWindow)
	{
		if (PreviewMode == StiPreviewMode.DotMatrix)
		{
			ShowDotMatrixWithWpf(dialogWindow);
		}
		else
		{
			RegReportDataSources();
			if (ContainsDashboard)
			{
				if (CheckNeedsCompiling())
				{
					Compile();
				}
			}
			else if (!IsRendered)
			{
				RenderWithWpf(showProgress: true);
			}
			StiLogService.Write(GetType(), "Showing report");
			if (CheckNeedsCompiling() && !IsCompiled)
			{
				throw new Exception("Can't showing report because report require compilation.");
			}
			try
			{
				if (!IsStopped)
				{
					StiReport report = (CheckNeedsCompiling() ? CompiledReport : this);
					if (ViewerControl != null)
					{
						ViewerControl.Report = report;
					}
					else
					{
						IStiViewerForm stiViewerForm;
						if (ViewerForm == null)
						{
							stiViewerForm = StiGuiOptions.GetViewerFormWithRibbonGUI(null, (StiGuiMode)1);
							stiViewerForm.ViewerOwner = ownerWindow;
							stiViewerForm.Report = report;
						}
						else
						{
							stiViewerForm = StiActivator.CreateObject(ViewerForm, new object[1]) as IStiViewerForm;
							stiViewerForm.ViewerOwner = ownerWindow;
							stiViewerForm.Report = report;
						}
						if (dialogWindow)
						{
							stiViewerForm.ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
							stiViewerForm.ShowDialogViewer(ownerWindow);
						}
						else
						{
							stiViewerForm.ShowViewer(ownerWindow);
						}
					}
				}
			}
			catch (Exception e)
			{
				StiLogService.Write(GetType(), "Showing report...ERROR");
				StiLogService.Write(GetType(), e);
				if (!StiOptions.Engine.HideExceptions)
				{
					throw;
				}
			}
		}
		return this;
	}

	public StiReport ShowDotMatrix()
	{
		return ShowDotMatrix((Form)null, dialogForm: false);
	}

	public StiReport ShowDotMatrix(bool dialogForm)
	{
		return ShowDotMatrix((Form)null, dialogForm);
	}

	public StiReport ShowDotMatrix(Form parentForm)
	{
		return ShowDotMatrix(parentForm, dialogForm: false);
	}

	public StiReport ShowDotMatrix(IWin32Window owner)
	{
		return ShowDotMatrix(null, owner, dialogForm: true);
	}

	public StiReport ShowDotMatrix(IWin32Window owner, bool dialogForm)
	{
		return ShowDotMatrix(null, owner, dialogForm);
	}

	private StiReport ShowDotMatrix(Form parentForm, bool dialogForm)
	{
		return ShowDotMatrix(parentForm, null, dialogForm);
	}

	private StiReport ShowDotMatrix(Form parentForm, IWin32Window win32Window, bool dialogForm)
	{
		if (ContainsDashboard)
		{
			throw new NotSupportedException("DotMatrix Viewer doesn't supported for the dashboards!");
		}
		if (!StiGuiOptions.AllowWinViewer && StiGuiOptions.AllowWpfViewer)
		{
			ShowDotMatrixWithWpf(dialogForm);
			return this;
		}
		RegReportDataSources();
		if (ContainsDashboard)
		{
			if (CheckNeedsCompiling())
			{
				Compile();
			}
		}
		else if (!IsRendered)
		{
			Render(showProgress: true);
		}
		StiLogService.Write(GetType(), "Showing report in DotMatrixMode");
		if (CheckNeedsCompiling() && !IsCompiled)
		{
			throw new Exception("Can't showing report in DotMatrix mode because report require compilation.");
		}
		try
		{
			if (!IsStopped)
			{
				StiReport stiReport = (CheckNeedsCompiling() ? CompiledReport : this);
				if (ViewerControl != null)
				{
					ViewerControl.Report = stiReport;
				}
				else
				{
					IStiViewerForm stiViewerForm = ((!(ViewerForm == null)) ? (StiActivator.CreateObject(ViewerForm, new object[1] { stiReport }) as IStiViewerForm) : StiGuiOptions.GetDotMatrixViewerForm(stiReport, (StiGuiMode)0));
					if (parentForm != null && (parentForm.IsMdiContainer || parentForm.MdiParent != null))
					{
						stiViewerForm.TopLevel = false;
						stiViewerForm.ShowInTaskbar = false;
						stiViewerForm.ViewerOwner = stiViewerForm.GetOwner(parentForm) ?? parentForm;
						stiViewerForm.ShowViewer();
					}
					else if (dialogForm)
					{
						stiViewerForm.ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
						if (win32Window == null)
						{
							stiViewerForm.ShowDialogViewer();
						}
						else
						{
							stiViewerForm.ShowDialogViewer(win32Window);
						}
					}
					else if (win32Window == null)
					{
						stiViewerForm.ShowViewer();
					}
					else
					{
						stiViewerForm.ShowViewer(win32Window);
					}
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Showing report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return this;
	}

	public StiReport ShowDotMatrixWithRibbonGUI()
	{
		return ShowDotMatrixWithRibbonGUI((Form)null, dialogForm: false);
	}

	public StiReport ShowDotMatrixWithRibbonGUI(bool dialogForm)
	{
		return ShowDotMatrixWithRibbonGUI((Form)null, dialogForm);
	}

	public StiReport ShowDotMatrixWithRibbonGUI(Form parentForm)
	{
		return ShowDotMatrixWithRibbonGUI(parentForm, dialogForm: false);
	}

	public StiReport ShowDotMatrixWithRibbonGUI(IWin32Window owner)
	{
		return ShowDotMatrixWithRibbonGUI(null, owner, dialogForm: true);
	}

	public StiReport ShowDotMatrixWithRibbonGUI(IWin32Window owner, bool dialogForm)
	{
		return ShowDotMatrixWithRibbonGUI(null, owner, dialogForm);
	}

	private StiReport ShowDotMatrixWithRibbonGUI(Form parentForm, bool dialogForm)
	{
		return ShowDotMatrixWithRibbonGUI(parentForm, null, dialogForm);
	}

	private StiReport ShowDotMatrixWithRibbonGUI(Form parentForm, IWin32Window win32Window, bool dialogForm)
	{
		RegReportDataSources();
		if (ContainsDashboard)
		{
			if (CheckNeedsCompiling())
			{
				Compile();
			}
		}
		else if (!IsRendered)
		{
			Render(showProgress: true);
		}
		StiLogService.Write(GetType(), "Showing report in DotMatrixMode");
		if (CheckNeedsCompiling() && !IsCompiled)
		{
			throw new Exception("Can't showing report in DotMatrix mode because report require compilation.");
		}
		try
		{
			if (!IsStopped)
			{
				StiReport stiReport = (CheckNeedsCompiling() ? CompiledReport : this);
				if (ViewerControl != null)
				{
					ViewerControl.Report = stiReport;
				}
				else
				{
					IStiViewerForm stiViewerForm;
					if (ViewerForm == null)
					{
						stiViewerForm = StiGuiOptions.GetDotMatrixViewerFormWithRibbonGUI(stiReport, (StiGuiMode)0);
					}
					else
					{
						Type type = Type.GetType("Stimulsoft.Report.Viewer.StiRibbonDotMatrixViewerForm, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
						if (type == null)
						{
							throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
						}
						stiViewerForm = StiActivator.CreateObject(type, new object[1] { stiReport }) as IStiViewerForm;
					}
					if (parentForm != null && (parentForm.IsMdiContainer || parentForm.MdiParent != null))
					{
						stiViewerForm.TopLevel = false;
						stiViewerForm.ShowInTaskbar = false;
						stiViewerForm.ViewerOwner = stiViewerForm.GetOwner(parentForm) ?? parentForm;
						stiViewerForm.ShowViewer();
					}
					else if (dialogForm)
					{
						stiViewerForm.ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
						if (win32Window == null)
						{
							stiViewerForm.ShowDialogViewer();
						}
						else
						{
							stiViewerForm.ShowDialogViewer(win32Window);
						}
					}
					else if (win32Window == null)
					{
						stiViewerForm.ShowViewer();
					}
					else
					{
						stiViewerForm.ShowViewer(win32Window);
					}
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Showing report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return this;
	}

	public StiReport ShowDotMatrixWithWpf()
	{
		return ShowDotMatrixWithWpf(dialogWindow: false);
	}

	public StiReport ShowDotMatrixWithWpf(bool dialogWindow)
	{
		RegReportDataSources();
		if (ContainsDashboard)
		{
			if (CheckNeedsCompiling())
			{
				Compile();
			}
		}
		else if (!IsRendered)
		{
			Render(showProgress: true);
		}
		StiLogService.Write(GetType(), "Showing report in DotMatrixMode");
		if (CheckNeedsCompiling() && !IsCompiled)
		{
			throw new Exception("Can't showing report in DotMatrix mode because report require compilation.");
		}
		try
		{
			if (!IsStopped)
			{
				StiReport stiReport = (CheckNeedsCompiling() ? CompiledReport : this);
				if (ViewerControl != null)
				{
					ViewerControl.Report = stiReport;
				}
				else
				{
					IStiViewerForm stiViewerForm = ((!(ViewerForm == null)) ? (StiActivator.CreateObject(ViewerForm, new object[1] { stiReport }) as IStiViewerForm) : StiGuiOptions.GetDotMatrixViewerForm(stiReport, (StiGuiMode)1));
					if (dialogWindow)
					{
						stiViewerForm.ShowInTaskbar = StiOptions.Viewer.Windows.ShowInTaskbar;
						stiViewerForm.ShowDialogViewer();
					}
					else
					{
						stiViewerForm.ShowViewer();
					}
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(GetType(), "Showing report...ERROR");
			StiLogService.Write(GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return this;
	}
}
