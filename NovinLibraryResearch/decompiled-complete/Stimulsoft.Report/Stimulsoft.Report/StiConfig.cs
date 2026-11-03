using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report;

public sealed class StiConfig
{
	[Obsolete("The property StiConfig.IsWeb is obsolete! Please use property StiOptions.Configuration.IsWeb.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool IsWeb
	{
		get
		{
			return StiOptions.Configuration.IsWeb;
		}
		set
		{
			StiOptions.Configuration.IsWeb = value;
		}
	}

	[Obsolete("The property StiConfig.IsWinForms is obsolete! Please use property StiOptions.Configuration.IsWinForms.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool IsWinForms => StiOptions.Configuration.IsWinForms;

	[Obsolete("The property StiConfig.IsWPF is obsolete! Please use property StiOptions.Configuration.IsWPF.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool IsWPF
	{
		get
		{
			return StiOptions.Configuration.IsWPF;
		}
		set
		{
			StiOptions.Configuration.IsWPF = value;
		}
	}

	[Obsolete("The property StiConfig.IsServer is obsolete! Please use property StiOptions.Configuration.IsServer.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool IsServer
	{
		get
		{
			return StiOptions.Configuration.IsServer;
		}
		set
		{
			StiOptions.Configuration.IsServer = value;
		}
	}

	[Obsolete("Property StiConfig.DefaultReportConfigPath is obsolete! Please use StiOptions.Configuration.DefaultReportConfigPath.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static string DefaultReportConfigPath
	{
		get
		{
			return StiOptions.Configuration.DefaultReportConfigPath;
		}
		set
		{
			StiOptions.Configuration.DefaultReportConfigPath = value;
		}
	}

	[Obsolete("Property StiConfig.ApplicationDirectory is obsolete! Please use StiOptions.Configuration.ApplicationDirectory.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static string ApplicationDirectory
	{
		get
		{
			return StiOptions.Configuration.ApplicationDirectory;
		}
		set
		{
			StiOptions.Configuration.ApplicationDirectory = value;
		}
	}

	public static StiServiceContainer Services { get; set; }

	[Obsolete("Method StiConfig.LoadLocalization is obsolete. Please use method StiOptions.Localization.Load istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void LoadLocalization(string file)
	{
		StiOptions.Localization.Load(file);
	}

	[Obsolete("Method StiConfig.LoadLocalization is obsolete. Please use method StiOptions.Localization.Load istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void LoadLocalization(Stream stream)
	{
		StiOptions.Localization.Load(stream);
	}

	[Obsolete("Method StiConfig.GetDirectoryLocalization is obsolete. Please use method StiOptions.Localization.GetDirectory istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static string GetDirectoryLocalization()
	{
		return StiOptions.Localization.GetDirectory();
	}

	[Obsolete("Method StiConfig.LoadDefaultLocalization is obsolete. Please use method StiOptions.Localization.LoadDefault istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void LoadDefaultLocalization()
	{
		StiOptions.Localization.LoadDefault();
	}

	[Obsolete("Method StiConfig.HideProperty is obsolete. Please use method StiOptions.Designer.Properties.Hide istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void HideProperty(Type type, string propertyName)
	{
		StiOptions.Designer.Properties.Hide(type, propertyName);
	}

	[Obsolete("Method StiConfig.HideProperty is obsolete. Please use method StiOptions.Designer.Properties.Hide istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void HideProperty(string propertyName)
	{
		StiOptions.Designer.Properties.Hide(propertyName);
	}

	[Obsolete("Method StiConfig.ShowProperty is obsolete. Please use method StiOptions.Designer.Properties.Show istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void ShowProperty(Type type, string propertyName)
	{
		StiOptions.Designer.Properties.Show(type, propertyName);
	}

	[Obsolete("Method StiConfig.ShowProperty is obsolete. Please use method StiOptions.Designer.Properties.Show istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void ShowProperty(string propertyName)
	{
		StiOptions.Designer.Properties.Show(propertyName);
	}

	[Obsolete("Method StiConfig.IsAllowedProperty is obsolete. Please use method StiOptions.Designer.Properties.IsAllowed istead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool IsAllowedProperty(Type type, string propertyName)
	{
		return StiOptions.Designer.Properties.IsAllowed(type, propertyName);
	}

	private static void OnBeforeGetService(object sender, StiServiceActionEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.ActionType == 1)
		{
			Type type = sender as Type;
			if (type == null)
			{
				return;
			}
			e.Processed = true;
			if (type == typeof(StiAggregateFunctionService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.AggregateFunctions).ToList();
			}
			else if (type == typeof(StiBarCodeTypeService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.BarCodes).ToList();
			}
			else if (type == typeof(StiArea))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.ChartAreas).ToList();
			}
			else if (type == typeof(StiSeriesLabels))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.ChartSerieLabels).ToList();
			}
			else if (type == typeof(StiSeries))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.ChartSeries).ToList();
			}
			else if (type == typeof(Stimulsoft.Report.Chart.StiChartStyle))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.ChartStyles).ToList();
			}
			else if (type == typeof(StiTrendLine))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.ChartTrendLines).ToList();
			}
			else if (type == typeof(StiDataAdapterService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.DataAdapters).ToList();
			}
			else if (type == typeof(StiDatabase))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.Databases).ToList();
			}
			else if (type == typeof(StiDataTableSetNameService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.DataTableSetName).ToList();
			}
			else if (type == typeof(StiDictionarySLService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.DictionarySLs).ToList();
			}
			else if (type == typeof(StiDocumentSLService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.DocumentSLs).ToList();
			}
			else if (type == typeof(StiPageSLService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.PageSLs).ToList();
			}
			else if (type == typeof(StiReportSLService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.ReportSLs).ToList();
			}
			else if (type == typeof(StiComponent))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.Components).ToList();
			}
			else if (type == typeof(StiDialogsProvider))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.DialogProviders).ToList();
			}
			else if (type == typeof(StiExportService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.Exports).ToList();
			}
			else if (type == typeof(StiFormatService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.Formats).ToList();
			}
			else if (type == typeof(StiLogService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.Logs).ToList();
			}
			else if (type == typeof(StiShapeTypeService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.Shapes).ToList();
			}
			else if (type == typeof(StiViewerConfigService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.ViewerConfig).ToList();
			}
			else if (type == typeof(StiDotMatrixViewerConfigService))
			{
				e.Services = ((IEnumerable<StiService>)StiOptions.Services.DotMatrixViewerConfig).ToList();
			}
			else
			{
				e.Processed = false;
			}
		}
		if ((int)e.ActionType != 0)
		{
			return;
		}
		foreach (StiService service in e.Services)
		{
			e.Processed = true;
			if (service is StiAggregateFunctionService)
			{
				StiOptions.Services.AggregateFunctions.Add(service as StiAggregateFunctionService);
			}
			else if (service is StiBarCodeTypeService)
			{
				StiOptions.Services.BarCodes.Add(service as StiBarCodeTypeService);
			}
			else if (service is StiArea)
			{
				StiOptions.Services.ChartAreas.Add(service as StiArea);
			}
			else if (service is StiSeriesLabels)
			{
				StiOptions.Services.ChartSerieLabels.Add(service as StiSeriesLabels);
			}
			else if (service is StiSeries)
			{
				StiOptions.Services.ChartSeries.Add(service as StiSeries);
			}
			else if (service is Stimulsoft.Report.Chart.StiChartStyle)
			{
				StiOptions.Services.ChartStyles.Add(service as Stimulsoft.Report.Chart.StiChartStyle);
			}
			else if (service is StiTrendLine)
			{
				StiOptions.Services.ChartTrendLines.Add(service as StiTrendLine);
			}
			else if (service is StiDataAdapterService)
			{
				StiOptions.Services.DataAdapters.Add(service as StiDataAdapterService);
			}
			else if (service is StiDatabase)
			{
				StiOptions.Services.Databases.Add(service as StiDatabase);
			}
			else if (service is StiDataTableSetNameService)
			{
				StiOptions.Services.DataTableSetName.Add(service as StiDataTableSetNameService);
			}
			else if (service is StiDictionarySLService)
			{
				StiOptions.Services.DictionarySLs.Add(service as StiDictionarySLService);
			}
			else if (service is StiDocumentSLService)
			{
				StiOptions.Services.DocumentSLs.Add(service as StiDocumentSLService);
			}
			else if (service is StiPageSLService)
			{
				StiOptions.Services.PageSLs.Add(service as StiPageSLService);
			}
			else if (service is StiReportSLService)
			{
				StiOptions.Services.ReportSLs.Add(service as StiReportSLService);
			}
			else if (service is StiComponent)
			{
				StiOptions.Services.Components.Add(service as StiComponent);
			}
			else if (service is StiDialogsProvider)
			{
				StiOptions.Services.DialogProviders.Add(service as StiDialogsProvider);
			}
			else if (service is StiExportService)
			{
				StiOptions.Services.Exports.Add(service as StiExportService);
			}
			else if (service is StiFormatService)
			{
				StiOptions.Services.Formats.Add(service as StiFormatService);
			}
			else if (service is StiLogService)
			{
				StiOptions.Services.Logs.Add(service as StiLogService);
			}
			else if (service is StiShapeTypeService)
			{
				StiOptions.Services.Shapes.Add(service as StiShapeTypeService);
			}
			else if (service is StiViewerConfigService)
			{
				StiOptions.Services.ViewerConfig.Add(service as StiViewerConfigService);
			}
			else if (service is StiDotMatrixViewerConfigService)
			{
				StiOptions.Services.DotMatrixViewerConfig.Add(service as StiDotMatrixViewerConfigService);
			}
			else
			{
				e.Processed = false;
			}
		}
	}

	[Obsolete("The method StiConfig.InitWeb is obsolete! Please use method StiOptions.Configuration.InitWeb.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void InitWeb()
	{
		StiOptions.Configuration.InitWeb();
	}

	public static void Save()
	{
		if (!StiOptions.Engine.DontSaveConfig)
		{
			if (StiOptions.Engine.AllowSetCurrentDirectory)
			{
				Environment.CurrentDirectory = StiOptions.Configuration.ApplicationDirectory;
			}
			Save(StiPath.GetPath(GetDefaultReportConfigPath()));
		}
	}

	public static void Save(string file)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(file);
			fileStream = new FileStream(file, FileMode.Create);
			Save(fileStream);
		}
		finally
		{
			fileStream.Close();
			fileStream.Dispose();
		}
	}

	public static void Save(Stream stream)
	{
		Save(stream, Services);
	}

	public static void Save(Stream stream, StiServiceContainer services)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			StiLogService.Write(typeof(StiReport), "Saving report configuration");
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("StiSerializer");
			xmlTextWriter.WriteAttributeString("version", "1.0");
			xmlTextWriter.WriteAttributeString("application", "StiReportConfig");
			services.Save((XmlWriter)xmlTextWriter);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiReport), "Saving report configuration...ERROR");
			StiLogService.Write(typeof(StiReport), e);
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	[Obsolete("Method StiConfig.Restore is obsolete!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void Restore()
	{
	}

	[Obsolete("Method StiConfig.Synchronize is obsolete!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void Synchronize()
	{
	}

	public static void Load()
	{
		if (StiOptions.Engine.AllowSetCurrentDirectory)
		{
			Environment.CurrentDirectory = StiOptions.Configuration.ApplicationDirectory;
		}
		string path = StiPath.GetPath(GetDefaultReportConfigPath());
		if (!File.Exists(path))
		{
			return;
		}
		using FileStream stream = new FileStream(path, FileMode.Open);
		Load(stream);
	}

	public static void Load(string file)
	{
		using FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
		Load(stream);
	}

	public static void Load(Stream stream)
	{
		Load(stream, Services);
	}

	public static void Load(Stream stream, StiServiceContainer services)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			StiLogService.Write(typeof(StiReport), "Loading report configuration");
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			using XmlTextReader xmlTextReader = new XmlTextReader(stream);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			xmlTextReader.Read();
			xmlTextReader.Read();
			if (!xmlTextReader.IsStartElement() || !(xmlTextReader.Name == "StiSerializer"))
			{
				return;
			}
			string attribute = xmlTextReader.GetAttribute("application");
			if (attribute == "StiReportConfig")
			{
				while (xmlTextReader.Read() && xmlTextReader.Name != "Services")
				{
				}
				services.Load((XmlReader)xmlTextReader);
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiReport), "Loading report configuration...ERROR");
			StiLogService.Write(typeof(StiReport), e);
			throw;
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public static string GetDefaultReportConfigPath()
	{
		string defaultReportConfigPath = StiOptions.Configuration.DefaultReportConfigPath;
		if (!string.IsNullOrEmpty(defaultReportConfigPath))
		{
			return defaultReportConfigPath;
		}
		defaultReportConfigPath = Path.Combine(StiOptions.Configuration.ApplicationDirectory, "Stimulsoft.Report.config");
		if (File.Exists(defaultReportConfigPath))
		{
			return defaultReportConfigPath;
		}
		if (!StiOptions.Engine.FullTrust)
		{
			return defaultReportConfigPath;
		}
		defaultReportConfigPath = "Stimulsoft\\Stimulsoft.Report.config";
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		if (folderPath.Length != 0)
		{
			defaultReportConfigPath = Path.Combine(folderPath, defaultReportConfigPath);
		}
		folderPath = Path.GetDirectoryName(defaultReportConfigPath);
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		return defaultReportConfigPath;
	}

	public static void Reset()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		if (Services != null)
		{
			Services.BeforeGetService -= new StiServiceActionHandler(OnBeforeGetService);
		}
		Services = new StiServiceContainer();
		Services.BeforeGetService += new StiServiceActionHandler(OnBeforeGetService);
	}

	static StiConfig()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Services = new StiServiceContainer();
		Services.BeforeGetService += new StiServiceActionHandler(OnBeforeGetService);
	}
}
