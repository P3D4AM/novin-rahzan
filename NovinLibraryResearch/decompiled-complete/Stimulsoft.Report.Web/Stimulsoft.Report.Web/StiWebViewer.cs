using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Serialization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Export;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Web.Helpers.Dashboards;

namespace Stimulsoft.Report.Web;

[ToolboxBitmap(typeof(StiWebViewer), "Viewer.Images.StiWebViewer.bmp")]
public class StiWebViewer : WebControl, INamingContainer
{
	private string parametersPanelDateFormat = "";

	private StiRequestParams requestParams;

	private string clientGuid;

	private StiReport report;

	private static StiCacheHelper cacheHelper;

	private int requestTimeout = 30;

	private int zoom = 100;

	[Category("Appearance")]
	[DefaultValue(StiViewerTheme.Office2013WhiteBlue)]
	[Description("Gets or sets the current visual theme which is used for drawing visual elements of the viewer.")]
	public StiViewerTheme Theme { get; set; } = StiViewerTheme.Office2013WhiteBlue;


	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Gets or sets a path to the custom css file for the viewer.")]
	public string CustomCss { get; set; } = string.Empty;


	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Gets or sets a path to the localization file for the viewer.")]
	public string Localization { get; set; } = string.Empty;


	[Category("Appearance")]
	[DefaultValue(typeof(Color), "White")]
	[Description("Gets or sets the background color of the viewer.")]
	[TypeConverter(typeof(WebColorConverter))]
	public Color BackgroundColor
	{
		get
		{
			return ((WebControl)this).BackColor;
		}
		set
		{
			((WebControl)this).BackColor = value;
		}
	}

	[DefaultValue(typeof(Color), "Gray")]
	[Category("Appearance")]
	[Description("Gets or sets a color of the report page border.")]
	[TypeConverter(typeof(WebColorConverter))]
	public Color PageBorderColor { get; set; } = Color.Gray;


	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which controls of output objects in the right to left mode.")]
	public bool RightToLeft { get; set; }

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which indicates which indicates that the viewer is displayed in full screen mode.")]
	public bool FullScreenMode { get; set; }

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which indicates that the viewer will show the report area with scrollbars.")]
	public bool ScrollbarsMode { get; set; }

	[Category("Appearance")]
	[DefaultValue("_blank")]
	[Description("Gets or sets a browser window to open links from the report.")]
	public string OpenLinksWindow { get; set; } = "_blank";


	[Category("Appearance")]
	[DefaultValue("_blank")]
	[Description("Gets or sets a browser window to open the exported report.")]
	public string OpenExportedReportWindow { get; set; } = "_blank";


	[Category("Appearance")]
	[DefaultValue("_self")]
	[Description("Gets or sets a browser window to open page at design event.")]
	public string DesignWindow { get; set; } = "_self";


	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that show or hide tooltips.")]
	public bool ShowTooltips { get; set; } = true;


	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that show or hide the help link in tooltips.")]
	public bool ShowTooltipsHelp { get; set; } = true;


	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that show or hide the help button in dialogs.")]
	public bool ShowDialogsHelp { get; set; } = true;


	[Category("Appearance")]
	[DefaultValue(StiContentAlignment.Center)]
	[Description("Gets or sets the alignment of the viewer page.")]
	public StiContentAlignment PageAlignment { get; set; } = StiContentAlignment.Center;


	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the shadow of the page will be displayed in the viewer.")]
	public bool ShowPageShadow { get; set; }

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which allows printing report bookmarks.")]
	public bool BookmarksPrint { get; set; }

	[Category("Appearance")]
	[DefaultValue(180)]
	[Description("Gets or sets a width of the bookmarks tree in the viewer.")]
	public int BookmarksTreeWidth { get; set; } = 180;


	[Category("Appearance")]
	[DefaultValue(StiParametersPanelPosition.Top)]
	[Description("Gets or sets a position of the parameters panel.")]
	public StiParametersPanelPosition ParametersPanelPosition { get; set; }

	[Category("Appearance")]
	[DefaultValue(300)]
	[Description("Gets or sets a max height of parameters panel in the viewer.")]
	public int ParametersPanelMaxHeight { get; set; } = 300;


	[Category("Appearance")]
	[DefaultValue(2)]
	[Description("Gets or sets a count columns in parameters panel.")]
	public int ParametersPanelColumnsCount { get; set; } = 2;


	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which indicates that variable items will be sorted.")]
	public bool ParametersPanelSortDataItems { get; set; }

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Gets or sets a date format for datetime parameters in parameters panel. To use a server date format please set the StiDateFormatMode.FromServer or \"FromServer\" string value. The default is the client browser date format.")]
	public string ParametersPanelDateFormat
	{
		get
		{
			if (parametersPanelDateFormat == "FromServer")
			{
				return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
			}
			return parametersPanelDateFormat;
		}
		set
		{
			parametersPanelDateFormat = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(StiInterfaceType.Auto)]
	[Description("Gets or sets the type of the viewer interface.")]
	public StiInterfaceType InterfaceType { get; set; }

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Gets or sets a value which indicates that allows mobile mode of the viewer interface.")]
	public bool AllowMobileMode { get; set; } = true;


	[Category("Appearance")]
	[DefaultValue(StiChartRenderType.AnimatedVector)]
	[Description("Gets or sets the type of the chart in the viewer.")]
	public StiChartRenderType ChartRenderType { get; set; } = StiChartRenderType.AnimatedVector;


	[Category("Appearance")]
	[DefaultValue(StiReportDisplayMode.Table)]
	[Description("Gets or sets a method how the viewer will show a report.")]
	public StiReportDisplayMode ReportDisplayMode { get; set; }

	[Category("Appearance")]
	[DefaultValue(StiFirstDayOfWeek.Auto)]
	[Description("Gets or sets the first day of week in the date picker.")]
	public StiFirstDayOfWeek DatePickerFirstDayOfWeek { get; set; }

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a value, which indicates that the current day will be included in the ranges of the date picker.")]
	public bool DatePickerIncludeCurrentDayForRanges { get; set; }

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which allows touch zoom in the viewer.")]
	public bool AllowTouchZoom { get; set; } = true;


	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that 'The report is not specified' message will be shown.")]
	public bool ShowReportIsNotSpecifiedMessage { get; set; } = true;


	[Category("Appearance")]
	[DefaultValue(StiImagesQuality.Normal)]
	[Description("Gets or sets the image quality that will be used on the viewer page.")]
	public StiImagesQuality ImagesQuality { get; set; } = StiImagesQuality.Normal;


	[Category("Appearance")]
	[DefaultValue(StiPrintToPdfMode.Hidden)]
	[Description("Gets or sets the PDF print mode - hidden or in a pop-up window.")]
	public StiPrintToPdfMode PrintToPdfMode { get; set; }

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that if a report contains several pages, then they will be combined in preview.")]
	public bool CombineReportPages { get; set; }

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates images size of the save menu in the viewer.")]
	public StiImageSize SaveMenuImageSize { get; set; } = StiImageSize.Small;


	[Category("Email")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which allows to display the Email dialog, or send Email with the default settings.")]
	public bool ShowEmailDialog { get; set; } = true;


	[Category("Email")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which allows to display the export dialog for Email, or export report for Email with the default settings.")]
	public bool ShowEmailExportDialog { get; set; } = true;


	[Category("Email")]
	[DefaultValue("")]
	[Description("Gets or sets the default email address of the message created in the viewer.")]
	public string DefaultEmailAddress { get; set; } = string.Empty;


	[Category("Email")]
	[DefaultValue("")]
	[Description("Gets or sets the default subject of the message created in the viewer.")]
	public string DefaultEmailSubject { get; set; } = string.Empty;


	[Category("Email")]
	[DefaultValue("")]
	[Description("Gets or sets the default text of the message created in the viewer.")]
	public string DefaultEmailMessage { get; set; } = string.Empty;


	[Browsable(false)]
	[Description("A class which controls default settings of exports.")]
	public StiDefaultExportSettings DefaultExportSettings { get; set; } = new StiDefaultExportSettings();


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which allows store the export settings in the cookies.")]
	public bool StoreExportSettings { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which allows to display the export dialog, or to export with the default settings.")]
	public bool ShowExportDialog { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the report document file.")]
	public bool ShowExportToDocument { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the PDF format.")]
	public bool ShowExportToPdf { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the XPS format.")]
	public bool ShowExportToXps { get; set; }

	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Power Point 2007-2010 format.")]
	public bool ShowExportToPowerPoint { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the HTML format.")]
	public bool ShowExportToHtml { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the HTML5 format.")]
	public bool ShowExportToHtml5 { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the MHT (Web Archive) format.")]
	public bool ShowExportToMht { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the TEXT format.")]
	public bool ShowExportToText { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Rich Text format.")]
	public bool ShowExportToRtf { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Word 2007-2010 format.")]
	public bool ShowExportToWord2007 { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Open Document Text format.")]
	public bool ShowExportToOpenDocumentWriter { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Excel BIFF format.")]
	public bool ShowExportToExcel { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Excel XML format.")]
	public bool ShowExportToExcelXml { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Excel 2007-2010 format.")]
	public bool ShowExportToExcel2007 { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Open Document Calc format.")]
	public bool ShowExportToOpenDocumentCalc { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the CSV format.")]
	public bool ShowExportToCsv { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the DBF format.")]
	public bool ShowExportToDbf { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the XML format.")]
	public bool ShowExportToXml { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the DIF format.")]
	public bool ShowExportToDif { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Sylk format.")]
	public bool ShowExportToSylk { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Json format.")]
	public bool ShowExportToJson { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the BMP image format.")]
	public bool ShowExportToImageBmp { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the GIF image format.")]
	public bool ShowExportToImageGif { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the JPEG image format.")]
	public bool ShowExportToImageJpeg { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the PCX image format.")]
	public bool ShowExportToImagePcx { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the PNG image format.")]
	public bool ShowExportToImagePng { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the TIFF image format.")]
	public bool ShowExportToImageTiff { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the Metafile image format.")]
	public bool ShowExportToImageMetafile { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the SVG image format.")]
	public bool ShowExportToImageSvg { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the user can save the report from the viewer to the SVGZ image format.")]
	public bool ShowExportToImageSvgz { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which allows to display the option Open After Export.")]
	public bool ShowOpenAfterExport { get; set; } = true;


	[Category("Exports")]
	[DefaultValue(true)]
	[Description("Gets or sets a value the option Open After Export.")]
	public bool OpenAfterExport { get; set; } = true;


	private static bool IsDesignMode => HttpContext.Current == null;

	private StiRequestParams RequestParams
	{
		get
		{
			if (requestParams == null)
			{
				requestParams = GetRequestParams();
			}
			return requestParams;
		}
	}

	internal string ClientGuid
	{
		get
		{
			if (clientGuid == null)
			{
				clientGuid = StiGuidUtils.NewGuid();
			}
			return clientGuid;
		}
		set
		{
			clientGuid = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	internal bool ReportDesignerMode { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	internal bool CloudMode { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	internal bool ServerMode { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			return ((WebControl)this).BackColor;
		}
		set
		{
			((WebControl)this).BackColor = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override ClientIDMode ClientIDMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).ClientIDMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).ClientIDMode = value;
		}
	}

	[Browsable(false)]
	[Description("Gets or sets a report object which is shown in the viewer.")]
	public StiReport Report
	{
		get
		{
			if (report == null)
			{
				report = GetReportObject(RequestParams);
			}
			return report;
		}
		set
		{
			report = value;
			if (RequestParams.ComponentType != StiComponentType.Viewer)
			{
				requestParams = CreateRequestParams();
			}
			InvokeGetReportData();
			StiReportHelper.ApplyQueryParameters(RequestParams, report);
			RequestParams.Cache.Helper.SaveReportInternal(RequestParams, report);
		}
	}

	[Browsable(false)]
	[Description("Gets or sets an instance of the StiCacheHelper class that will be used for report caching on the server side.")]
	public static StiCacheHelper CacheHelper
	{
		get
		{
			if (cacheHelper == null)
			{
				cacheHelper = new StiCacheHelper();
			}
			return cacheHelper;
		}
		set
		{
			cacheHelper = value;
		}
	}

	[Category("Server")]
	[DefaultValue(30)]
	[Description("Gets or sets time which indicates how many seconds the client side will wait for the response from the server side. The default value is 30 seconds.")]
	public int RequestTimeout
	{
		get
		{
			return requestTimeout;
		}
		set
		{
			requestTimeout = Math.Max(1, Math.Min(21600, value));
		}
	}

	[Category("Server")]
	[DefaultValue(10)]
	[Description("Gets or sets time which indicates how many minutes the report will be stored in the server cache or session. The default value is 10 minutes.")]
	public int CacheTimeout { get; set; } = 10;


	[Category("Server")]
	[DefaultValue(StiServerCacheMode.ObjectCache)]
	[Description("Gets or sets the mode of the report caching.")]
	public StiServerCacheMode CacheMode { get; set; } = StiServerCacheMode.ObjectCache;


	[Category("Server")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies the relative priority of report, stored in the system cache.")]
	public CacheItemPriority CacheItemPriority { get; set; } = (CacheItemPriority)3;


	[Category("Server")]
	[DefaultValue(true)]
	[Description("Allows the viewer to update the cache automatically. The cache will be updated about once every three minutes if there are no actions in the viewer.")]
	public bool AllowAutoUpdateCache { get; set; } = true;


	[Category("Server")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that the viewer will use relative or absolute URLs.")]
	public bool UseRelativeUrls { get; set; } = true;


	[Category("Server")]
	[DefaultValue(0)]
	[Description("Gets or sets a value which specifies the port number to use in the URL. A value of 0 defines automatic detection. A value of -1 removes the port number.")]
	public int PortNumber { get; set; }

	[Category("Server")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which enables or disables the transfer of URL parameters when requesting the scripts and styles of the viewer.")]
	public bool PassQueryParametersForResources { get; set; } = true;


	[Category("Server")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which enables or disables the use of URL parameters as report parameters.")]
	public bool PassQueryParametersToReport { get; set; }

	[Category("Server")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which enables or disables the transfer POST parameters of the form.")]
	public bool PassFormValues { get; set; }

	[Category("Server")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which enables or disables the display of the detailed server error in the viewer.")]
	public bool ShowServerErrorPage { get; set; } = true;


	[Category("Server")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which enables or disables the GZip compression of requests to the server. This allows to reduce the volume of Internet traffic, but slightly slows down the viewer actions.")]
	public bool UseCompression { get; set; }

	[Category("Server")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which enables caching for viewer scripts and styles.")]
	public bool UseCacheForResources { get; set; } = true;


	[Category("Server")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which enables the use of a different cache depending on the selected localization.")]
	public bool UseLocalizedCache { get; set; }

	[Category("Server")]
	[DefaultValue(false)]
	[Description("Allows the viewer to update the cookies automatically on every request to the server. By default, cookies are set when creating the viewer, if they are not specified in the report.")]
	public bool AllowAutoUpdateCookies { get; set; }

	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that toolbar will be shown in the viewer.")]
	public bool ShowToolbar { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(StiToolbarDisplayMode.Simple)]
	[Description("Gets or sets the display mode of the toolbar - simple or separated into upper and lower parts.")]
	public StiToolbarDisplayMode ToolbarDisplayMode { get; set; }

	[Category("Toolbar")]
	[DefaultValue(typeof(Color), "Empty")]
	[Description("Gets or sets a color of the toolbar background. The default value is the theme color.")]
	[TypeConverter(typeof(WebColorConverter))]
	public Color ToolbarBackgroundColor { get; set; } = Color.Empty;


	[DefaultValue(typeof(Color), "Empty")]
	[Category("Toolbar")]
	[Description("Gets or sets a color of the toolbar border. The default value is the theme color.")]
	[TypeConverter(typeof(WebColorConverter))]
	public Color ToolbarBorderColor { get; set; } = Color.Empty;


	[DefaultValue(typeof(Color), "Empty")]
	[Category("Toolbar")]
	[Description("Gets or sets a color of the toolbar texts.")]
	[TypeConverter(typeof(WebColorConverter))]
	public Color ToolbarFontColor { get; set; } = Color.Empty;


	[Category("Toolbar")]
	[DefaultValue("Arial")]
	[Description("Gets or sets a value which indicates which font family will be used for drawing texts in the viewer.")]
	public string ToolbarFontFamily { get; set; } = "Arial";


	[Category("Toolbar")]
	[DefaultValue(StiContentAlignment.Default)]
	[Description("Gets or sets the alignment of the viewer toolbar.")]
	public StiContentAlignment ToolbarAlignment { get; set; } = StiContentAlignment.Default;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which allows displaying or hiding toolbar buttons captions.")]
	public bool ShowButtonCaptions { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Print button in the toolbar of the viewer.")]
	public bool ShowPrintButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Open button in the toolbar of the viewer.")]
	public bool ShowOpenButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Save button in the toolbar of the viewer.")]
	public bool ShowSaveButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(false)]
	[Description("Gets or sets a visibility of the Send Email button in the toolbar of the viewer.")]
	public bool ShowSendEmailButton { get; set; }

	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Find button in the toolbar of the viewer.")]
	public bool ShowFindButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Bookmarks button in the toolbar of the viewer.")]
	public bool ShowBookmarksButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Parameters button in the toolbar of the viewer.")]
	public bool ShowParametersButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Resources button in the toolbar of the viewer.")]
	public bool ShowResourcesButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Editor button in the toolbar of the viewer.")]
	public bool ShowEditorButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Full Screen button in the toolbar of the viewer.")]
	public bool ShowFullScreenButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the First Page button in the toolbar of the viewer.")]
	public bool ShowFirstPageButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Prev Page button in the toolbar of the viewer.")]
	public bool ShowPreviousPageButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the current page control in the toolbar of the viewer.")]
	public bool ShowCurrentPageControl { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Next Page button in the toolbar of the viewer.")]
	public bool ShowNextPageButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Last Page button in the toolbar of the viewer.")]
	public bool ShowLastPageButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Zoom control in the toolbar of the viewer.")]
	public bool ShowZoomButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the View Mode button in the toolbar of the viewer.")]
	public bool ShowViewModeButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(false)]
	[Description("Gets or sets a visibility of the Design button in the toolbar of the viewer.")]
	public bool ShowDesignButton { get; set; }

	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the About button in the toolbar of the viewer.")]
	public bool ShowAboutButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Pin button in the toolbar of the viewer in mobile mode.")]
	public bool ShowPinToolbarButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a visibility of the Refresh button in the toolbar of the viewer.")]
	public bool ShowRefreshButton { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(StiPrintDestination.Default)]
	[Description("Gets or sets the default mode of the report print destination.")]
	public StiPrintDestination PrintDestination { get; set; }

	[Category("Toolbar")]
	[DefaultValue(StiWebViewMode.SinglePage)]
	[Description("Gets or sets the mode of showing a report in the viewer - Single Page, Continuous or Multiple Pages.")]
	public StiWebViewMode ViewMode { get; set; }

	[Category("Toolbar")]
	[DefaultValue(100)]
	[Description("Gets or sets the report showing zoom. The default value is 100.")]
	public int Zoom
	{
		get
		{
			return zoom;
		}
		set
		{
			if (value > 500)
			{
				zoom = 500;
			}
			else if (value < 10 && value >= 0)
			{
				zoom = 10;
			}
			else
			{
				zoom = value;
			}
		}
	}

	[Category("Toolbar")]
	[DefaultValue(true)]
	[Description("Gets or sets a value which indicates that menu animation is enabled.")]
	public bool MenuAnimation { get; set; } = true;


	[Category("Toolbar")]
	[DefaultValue(StiShowMenuMode.Click)]
	[Description("Gets or sets the mode that shows menu of the viewer.")]
	public StiShowMenuMode ShowMenuMode { get; set; }

	[Category("Toolbar")]
	[DefaultValue(false)]
	[Description("Gets or sets a value which allows automatically hide the viewer toolbar in mobile mode.")]
	public bool AutoHideToolbar { get; set; }

	[Category("Viewer")]
	[Description("The event occurs for any action of the viewer before it is processed.")]
	public event StiViewerEventHandler ViewerEvent;

	[Category("Viewer")]
	[Description("The event occurs when the report is requested after running the viewer.")]
	public event StiReportDataEventHandler GetReport;

	[Category("Viewer")]
	[Description("The event occurs each time before rendering the report.")]
	public event StiReportDataEventHandler GetReportData;

	[Category("Viewer")]
	[Description("The event occurs before the printing of the report from the viewer menu.")]
	public event StiPrintReportEventHandler PrintReport;

	[Category("Viewer")]
	[Description("The event occurs before the exporting of the report from the viewer menu.")]
	public event StiExportReportEventHandler ExportReport;

	[Category("Viewer")]
	[Description("The event occurs after the report is exported.")]
	public event StiExportReportResponseEventHandler ExportReportResponse;

	[Category("Viewer")]
	[Description("The event occurs when sending a report via Email. In this event it is necessary to set settings for sending Email.")]
	public event StiEmailReportEventHandler EmailReport;

	[Category("Viewer")]
	[Description("The event occurs before the interactive action in the viewer, such as sorting, collapsing, drill-down, request from user variables.")]
	public event StiReportDataEventHandler Interaction;

	[Category("Viewer")]
	[Description("The event occurs when clicking on the Design button in the viewer toolbar.")]
	public event StiReportDataEventHandler DesignReport;

	public static StiRequestParams GetRequestParams()
	{
		StiRequestParams stiRequestParams = StiRequestParamsHelper.Get();
		stiRequestParams.Cache.Helper = CacheHelper;
		return stiRequestParams;
	}

	public static NameValueCollection GetFormValues()
	{
		StiRequestParams stiRequestParams = GetRequestParams();
		return GetFormValues(stiRequestParams);
	}

	public static NameValueCollection GetFormValues(StiRequestParams requestParams)
	{
		return requestParams.FormValues;
	}

	public static StiReport GetReportObject()
	{
		StiRequestParams stiRequestParams = GetRequestParams();
		return GetReportObject(stiRequestParams);
	}

	public static StiReport GetReportObject(StiRequestParams requestParams)
	{
		return requestParams.Cache.Helper.GetReportInternal(requestParams);
	}

	public static StiExportSettings GetExportSettings()
	{
		StiRequestParams stiRequestParams = GetRequestParams();
		return GetExportSettings(stiRequestParams);
	}

	public static StiExportSettings GetExportSettings(StiRequestParams requestParams)
	{
		return StiExportsHelper.GetExportSettings(requestParams);
	}

	public static IStiDashboardExportSettings GetDashboardExportSettings()
	{
		StiRequestParams stiRequestParams = GetRequestParams();
		return GetDashboardExportSettings(stiRequestParams);
	}

	public static IStiDashboardExportSettings GetDashboardExportSettings(StiRequestParams requestParams)
	{
		return StiExportsHelper.GetDashboardExportSettings(requestParams);
	}

	public static StiEmailOptions GetEmailOptions()
	{
		StiRequestParams stiRequestParams = GetRequestParams();
		return GetEmailOptions(stiRequestParams);
	}

	public static StiEmailOptions GetEmailOptions(StiRequestParams requestParams)
	{
		StiEmailOptions stiEmailOptions = new StiEmailOptions();
		if (requestParams.ExportSettings.Contains("Email"))
		{
			stiEmailOptions.AddressTo = (string)requestParams.ExportSettings["Email"];
		}
		if (requestParams.ExportSettings.Contains("Subject"))
		{
			stiEmailOptions.Subject = (string)requestParams.ExportSettings["Subject"];
		}
		if (requestParams.ExportSettings.Contains("Message"))
		{
			stiEmailOptions.Body = (string)requestParams.ExportSettings["Message"];
		}
		return stiEmailOptions;
	}

	private void InvokeViewerEvent()
	{
		StiViewerEventArgs e = new StiViewerEventArgs(RequestParams);
		OnViewerEvent(e);
	}

	protected virtual void OnViewerEvent(StiViewerEventArgs e)
	{
		this.ViewerEvent?.Invoke(this, e);
	}

	private void InvokeGetReport()
	{
		StiRequestParams stiRequestParams = RequestParams;
		StiReport stiReport = stiRequestParams.Report ?? Report;
		if (stiRequestParams.Action == StiAction.GetReport || stiReport == null)
		{
			StiReportDataEventArgs stiReportDataEventArgs = new StiReportDataEventArgs(stiRequestParams, stiReport);
			OnGetReport(stiReportDataEventArgs);
			report = stiReportDataEventArgs.Report;
			InvokeGetReportData();
			if (stiReportDataEventArgs.Report != null)
			{
				StiReportHelper.ApplyQueryParameters(stiRequestParams, stiReportDataEventArgs.Report);
				StiAction action = stiRequestParams.Action;
				stiRequestParams.Action = StiAction.GetReport;
				stiRequestParams.Cache.Helper.SaveReportInternal(stiRequestParams, stiReportDataEventArgs.Report);
				stiRequestParams.Action = action;
			}
		}
		else if (stiReport != null && !stiReport.IsRendered)
		{
			InvokeGetReportData();
		}
	}

	protected virtual void OnGetReport(StiReportDataEventArgs e)
	{
		this.GetReport?.Invoke(this, e);
	}

	private void InvokeGetReportData()
	{
		if (Report != null && !Report.IsDocument)
		{
			StiReportDataEventArgs e = new StiReportDataEventArgs(RequestParams, Report);
			OnGetReportData(e);
		}
	}

	protected virtual void OnGetReportData(StiReportDataEventArgs e)
	{
		this.GetReportData?.Invoke(this, e);
	}

	private void InvokePrintReport(StiExportSettings settings)
	{
		InvokeGetReport();
		StiPrintReportEventArgs e = new StiPrintReportEventArgs(RequestParams, Report, settings);
		OnPrintReport(e);
	}

	protected virtual void OnPrintReport(StiPrintReportEventArgs e)
	{
		this.PrintReport?.Invoke(this, e);
	}

	private void InvokeExportReport(StiExportSettings settings)
	{
		InvokeGetReport();
		StiExportReportEventArgs e = new StiExportReportEventArgs(RequestParams, Report, settings);
		OnExportReport(e);
	}

	protected virtual void OnExportReport(StiExportReportEventArgs e)
	{
		if (this.ExportReport != null)
		{
			this.ExportReport(this, e);
		}
	}

	private void InvokeExportReportResponse(StiExportSettings settings, StiWebActionResult result)
	{
		MemoryStream memoryStream = new MemoryStream(result.Data);
		StiExportReportResponseEventArgs stiExportReportResponseEventArgs = new StiExportReportResponseEventArgs(RequestParams, Report, settings, memoryStream, result.FileName, result.ContentType);
		OnExportReportResponse(stiExportReportResponseEventArgs);
		result.FileName = stiExportReportResponseEventArgs.FileName;
		result.ContentType = stiExportReportResponseEventArgs.ContentType;
		if (stiExportReportResponseEventArgs.Stream != memoryStream && stiExportReportResponseEventArgs.Stream is MemoryStream)
		{
			result.Data = ((MemoryStream)stiExportReportResponseEventArgs.Stream).ToArray();
		}
	}

	protected virtual void OnExportReportResponse(StiExportReportResponseEventArgs e)
	{
		this.ExportReportResponse?.Invoke(this, e);
	}

	private void InvokeEmailReport(StiExportSettings settings, StiEmailOptions options)
	{
		InvokeGetReport();
		StiEmailReportEventArgs e = new StiEmailReportEventArgs(RequestParams, Report, settings, options);
		OnEmailReport(e);
	}

	protected virtual void OnEmailReport(StiEmailReportEventArgs e)
	{
		this.EmailReport?.Invoke(this, e);
	}

	private void InvokeInteraction()
	{
		InvokeGetReport();
		StiReportDataEventArgs e = new StiReportDataEventArgs(RequestParams, Report);
		OnInteraction(e);
	}

	protected virtual void OnInteraction(StiReportDataEventArgs e)
	{
		this.Interaction?.Invoke(this, e);
	}

	private void InvokeDesignReport()
	{
		InvokeGetReport();
		StiReportDataEventArgs e = new StiReportDataEventArgs(RequestParams, Report);
		OnDesignReport(e);
	}

	protected virtual void OnDesignReport(StiReportDataEventArgs e)
	{
		if (this.DesignReport != null)
		{
			this.DesignReport(this, e);
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Unit val = ((WebControl)this).Width;
		if (((Unit)(ref val)).IsEmpty)
		{
			((WebControl)this).Width = Unit.Percentage(100.0);
		}
		val = ((WebControl)this).Height;
		if (((Unit)(ref val)).IsEmpty)
		{
			((WebControl)this).Height = (ScrollbarsMode ? Unit.Pixel(650) : Unit.Percentage(100.0));
		}
		((Control)this).OnPreRender(e);
	}

	private string GetLinksTarget(string value)
	{
		return value switch
		{
			"Blank" => "_blank", 
			"Self" => "_self", 
			"Top" => "_top", 
			_ => value, 
		};
	}

	private string RenderJsonParameters(bool forJsProject = false)
	{
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		Hashtable hashtable = new Hashtable();
		if (PassFormValues && HttpContext.Current != null && HttpContext.Current.Request.Form != null)
		{
			foreach (string key in HttpContext.Current.Request.Form.Keys)
			{
				if (!string.IsNullOrEmpty(key))
				{
					hashtable[key] = HttpContext.Current.Request.Form[key];
				}
			}
		}
		Hashtable hashtable2 = new Hashtable();
		hashtable2["viewerId"] = ((Control)this).ID;
		hashtable2["theme"] = Theme;
		hashtable2["clientGuid"] = ClientGuid;
		hashtable2["requestAbsoluteUrl"] = GetRequestUrl(useRelativeUrls: false, passQueryParameters: true, PortNumber);
		hashtable2["requestUrl"] = GetRequestUrl(UseRelativeUrls, passQueryParameters: true, PortNumber);
		hashtable2["requestResourcesUrl"] = GetRequestUrl(UseRelativeUrls, PassQueryParametersForResources, PortNumber);
		hashtable2["formValues"] = hashtable;
		hashtable2["cultureName"] = StiLocalization.CultureName;
		hashtable2["localization"] = Localization;
		hashtable2["shortProductVersion"] = StiVersionHelper.AssemblyVersion;
		hashtable2["productVersion"] = StiVersionHelper.ProductVersion.Trim();
		Unit height = ((WebControl)this).Height;
		UnitType type = ((Unit)(ref height)).Type;
		hashtable2["heightType"] = ((object)(UnitType)(ref type)).ToString();
		hashtable2["cloudMode"] = CloudMode;
		hashtable2["serverMode"] = ServerMode;
		hashtable2["reportDesignerMode"] = ReportDesignerMode;
		hashtable2["frameworkType"] = "ASP.NET";
		hashtable2["dashboardAssemblyLoaded"] = StiDashboardAssembly.IsAssemblyLoaded && StiDashboardExportAssembly.IsAssemblyLoaded && StiDashboardDrawingAssembly.IsAssemblyLoaded;
		hashtable2["alternateValid"] = StiLicenseHelper.CheckAnyLicense();
		hashtable2["buildDate"] = StiVersion.Created.ToString(CultureInfo.CreateSpecificCulture("en-US"));
		hashtable2["listSeparator"] = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
		if (!forJsProject)
		{
			hashtable2["licenseUserName"] = StiLicenseHelper.GetUserName();
			hashtable2["licenseIsValid"] = StiLicenseHelper.IsValidOnWeb();
			hashtable2["stimulsoftFontContent"] = StiReportResourceHelper.GetStimulsoftFontBase64Data();
		}
		Hashtable hashtable3 = new Hashtable();
		hashtable3["requestTimeout"] = RequestTimeout;
		hashtable3["cacheTimeout"] = CacheTimeout;
		hashtable3["cacheMode"] = CacheMode;
		hashtable3["cacheItemPriority"] = CacheItemPriority;
		hashtable3["allowAutoUpdateCache"] = AllowAutoUpdateCache;
		hashtable3["useRelativeUrls"] = UseRelativeUrls;
		hashtable3["passQueryParametersForResources"] = PassQueryParametersForResources;
		hashtable3["passQueryParametersToReport"] = PassQueryParametersToReport;
		hashtable3["passFormValues"] = PassFormValues;
		hashtable3["showServerErrorPage"] = ShowServerErrorPage;
		hashtable3["useCompression"] = UseCompression;
		hashtable3["useCacheForResources"] = UseCacheForResources;
		hashtable3["useLocalizedCache"] = UseLocalizedCache;
		hashtable3["allowAutoUpdateCookies"] = AllowAutoUpdateCookies;
		hashtable2["server"] = hashtable3;
		Hashtable hashtable4 = new Hashtable();
		hashtable4["customStylesUrl"] = CustomCss;
		hashtable4["backgroundColor"] = BackgroundColor;
		hashtable4["rightToLeft"] = RightToLeft;
		hashtable4["fullScreenMode"] = FullScreenMode;
		hashtable4["scrollbarsMode"] = ScrollbarsMode;
		hashtable4["openLinksWindow"] = GetLinksTarget(OpenLinksWindow);
		hashtable4["openExportedReportWindow"] = GetLinksTarget(OpenExportedReportWindow);
		hashtable4["designWindow"] = GetLinksTarget(DesignWindow);
		hashtable4["showTooltips"] = ShowTooltips;
		hashtable4["showTooltipsHelp"] = ShowTooltipsHelp;
		hashtable4["showDialogsHelp"] = ShowDialogsHelp;
		hashtable4["pageAlignment"] = PageAlignment;
		hashtable4["showPageShadow"] = ShowPageShadow;
		hashtable4["pageBorderColor"] = PageBorderColor;
		hashtable4["bookmarksPrint"] = BookmarksPrint;
		hashtable4["bookmarksTreeWidth"] = BookmarksTreeWidth;
		hashtable4["parametersPanelPosition"] = ParametersPanelPosition;
		hashtable4["parametersPanelMaxHeight"] = ParametersPanelMaxHeight;
		hashtable4["parametersPanelColumnsCount"] = ParametersPanelColumnsCount;
		hashtable4["parametersPanelDateFormat"] = ParametersPanelDateFormat;
		hashtable4["parametersPanelSortDataItems"] = ParametersPanelSortDataItems;
		hashtable4["interfaceType"] = InterfaceType;
		hashtable4["allowMobileMode"] = AllowMobileMode;
		hashtable4["chartRenderType"] = ChartRenderType;
		hashtable4["reportDisplayMode"] = ReportDisplayMode;
		hashtable4["datePickerFirstDayOfWeek"] = DatePickerFirstDayOfWeek;
		hashtable4["datePickerIncludeCurrentDayForRanges"] = DatePickerIncludeCurrentDayForRanges;
		hashtable4["storeExportSettings"] = StoreExportSettings;
		hashtable4["allowTouchZoom"] = AllowTouchZoom;
		hashtable4["showReportIsNotSpecifiedMessage"] = ShowReportIsNotSpecifiedMessage;
		hashtable4["imagesQuality"] = ImagesQuality;
		hashtable4["printToPdfMode"] = PrintToPdfMode;
		hashtable4["combineReportPages"] = CombineReportPages;
		hashtable4["saveMenuImageSize"] = SaveMenuImageSize;
		hashtable2["appearance"] = hashtable4;
		Hashtable hashtable5 = new Hashtable();
		hashtable5["visible"] = ShowToolbar;
		hashtable5["displayMode"] = ToolbarDisplayMode;
		hashtable5["backgroundColor"] = ToolbarBackgroundColor;
		hashtable5["borderColor"] = ToolbarBorderColor;
		hashtable5["fontColor"] = ToolbarFontColor;
		hashtable5["fontFamily"] = ToolbarFontFamily;
		hashtable5["alignment"] = ToolbarAlignment;
		hashtable5["showButtonCaptions"] = ShowButtonCaptions;
		hashtable5["showPrintButton"] = ShowPrintButton;
		hashtable5["showOpenButton"] = ShowOpenButton;
		hashtable5["showSaveButton"] = ShowSaveButton;
		hashtable5["showSendEmailButton"] = ShowSendEmailButton;
		hashtable5["showFindButton"] = ShowFindButton;
		hashtable5["showBookmarksButton"] = ShowBookmarksButton;
		hashtable5["showParametersButton"] = ShowParametersButton;
		hashtable5["showResourcesButton"] = ShowResourcesButton;
		hashtable5["showEditorButton"] = ShowEditorButton;
		hashtable5["showFullScreenButton"] = ShowFullScreenButton;
		hashtable5["showFirstPageButton"] = ShowFirstPageButton;
		hashtable5["showPreviousPageButton"] = ShowPreviousPageButton;
		hashtable5["showCurrentPageControl"] = ShowCurrentPageControl;
		hashtable5["showNextPageButton"] = ShowNextPageButton;
		hashtable5["showLastPageButton"] = ShowLastPageButton;
		hashtable5["showZoomButton"] = ShowZoomButton;
		hashtable5["showViewModeButton"] = ShowViewModeButton;
		hashtable5["showDesignButton"] = ShowDesignButton;
		hashtable5["showAboutButton"] = ShowAboutButton;
		hashtable5["showPinToolbarButton"] = ShowPinToolbarButton;
		hashtable5["showRefreshButton"] = ShowRefreshButton;
		hashtable5["printDestination"] = PrintDestination;
		hashtable5["viewMode"] = ViewMode;
		hashtable5["zoom"] = Zoom;
		hashtable5["menuAnimation"] = MenuAnimation;
		hashtable5["showMenuMode"] = ShowMenuMode;
		hashtable5["autoHide"] = AutoHideToolbar;
		hashtable2["toolbar"] = hashtable5;
		Hashtable hashtable6 = new Hashtable();
		hashtable6["storeExportSettings"] = StoreExportSettings;
		hashtable6["showExportDialog"] = ShowExportDialog;
		hashtable6["showExportToDocument"] = ShowExportToDocument;
		hashtable6["showExportToPdf"] = ShowExportToPdf;
		hashtable6["showExportToXps"] = ShowExportToXps;
		hashtable6["showExportToPowerPoint"] = ShowExportToPowerPoint;
		hashtable6["showExportToHtml"] = ShowExportToHtml;
		hashtable6["showExportToHtml5"] = ShowExportToHtml5;
		hashtable6["showExportToMht"] = ShowExportToMht;
		hashtable6["showExportToText"] = ShowExportToText;
		hashtable6["showExportToRtf"] = ShowExportToRtf;
		hashtable6["showExportToWord2007"] = ShowExportToWord2007;
		hashtable6["showExportToOpenDocumentWriter"] = ShowExportToOpenDocumentWriter;
		hashtable6["showExportToExcel"] = ShowExportToExcel;
		hashtable6["showExportToExcelXml"] = ShowExportToExcelXml;
		hashtable6["showExportToExcel2007"] = ShowExportToExcel2007;
		hashtable6["showExportToOpenDocumentCalc"] = ShowExportToOpenDocumentCalc;
		hashtable6["showExportToCsv"] = ShowExportToCsv;
		hashtable6["showExportToDbf"] = ShowExportToDbf;
		hashtable6["showExportToXml"] = ShowExportToXml;
		hashtable6["showExportToDif"] = ShowExportToDif;
		hashtable6["showExportToSylk"] = ShowExportToSylk;
		hashtable6["showExportToJson"] = ShowExportToJson;
		hashtable6["showExportToImageBmp"] = ShowExportToImageBmp;
		hashtable6["showExportToImageGif"] = ShowExportToImageGif;
		hashtable6["showExportToImageJpeg"] = ShowExportToImageJpeg;
		hashtable6["showExportToImagePcx"] = ShowExportToImagePcx;
		hashtable6["showExportToImagePng"] = ShowExportToImagePng;
		hashtable6["showExportToImageTiff"] = ShowExportToImageTiff;
		hashtable6["showExportToImageMetafile"] = ShowExportToImageMetafile;
		hashtable6["showExportToImageSvg"] = ShowExportToImageSvg;
		hashtable6["showExportToImageSvgz"] = ShowExportToImageSvgz;
		hashtable6["showOpenAfterExport"] = ShowOpenAfterExport;
		hashtable6["openAfterExport"] = OpenAfterExport;
		hashtable2["exports"] = hashtable6;
		Hashtable hashtable7 = new Hashtable();
		hashtable7["showEmailDialog"] = ShowEmailDialog;
		hashtable7["showExportDialog"] = ShowEmailExportDialog;
		hashtable7["defaultEmailAddress"] = DefaultEmailAddress;
		hashtable7["defaultEmailSubject"] = DefaultEmailSubject;
		hashtable7["defaultEmailMessage"] = DefaultEmailMessage;
		hashtable2["email"] = hashtable7;
		Hashtable hashtable8 = new Hashtable();
		hashtable8["viewerEvent"] = "ViewerEvent";
		hashtable2["actions"] = hashtable8;
		string text2 = JsonConvert.SerializeObject(hashtable2, Formatting.None, new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { (JsonConverter)new StringEnumConverter() }
		});
		string text3 = JsonConvert.SerializeObject(StiExportsHelper.GetDefaultExportSettings(DefaultExportSettings), Formatting.None, new StringEnumConverter());
		if (ReportDesignerMode)
		{
			string scriptsUrl = GetScriptsUrl();
			string text4 = JsonConvert.SerializeObject(scriptsUrl);
			return "{options:" + text2 + ",defaultExportSettings:" + text3 + ",scriptsUrl:" + text4 + "}";
		}
		return "{options:" + text2 + ",defaultExportSettings:" + text3 + "}";
	}

	private string GetScriptsUrl()
	{
		string text = (string.IsNullOrEmpty(Localization) ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(StiEncryption.Encrypt(Localization, StiRequestParamsHelper.EncryptingKey))));
		string requestUrl = GetRequestUrl(UseRelativeUrls, PassQueryParametersForResources, PortNumber);
		requestUrl += ((requestUrl.IndexOf("?") > 0) ? "&" : "?");
		requestUrl = requestUrl + "stiweb_component=Viewer&stiweb_action=Resource&stiweb_data=scripts&stiweb_theme=" + Theme;
		if (!string.IsNullOrEmpty(text))
		{
			requestUrl = requestUrl + "&stiweb_loc=" + HttpUtility.UrlEncode(text);
		}
		requestUrl = requestUrl + "&stiweb_cachemode=" + ((!UseCacheForResources) ? "none" : ((CacheMode == StiServerCacheMode.ObjectSession || CacheMode == StiServerCacheMode.StringSession) ? "session" : "cache"));
		return requestUrl + "&stiweb_version=" + StiVersionHelper.AssemblyVersion;
	}

	protected override void RenderContents(HtmlTextWriter writer)
	{
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		if (IsDesignMode)
		{
			StiRequestParams stiRequestParams = CreateRequestParams();
			Panel val = new Panel();
			((WebControl)val).Width = ((((WebControl)this).Width == Unit.Empty) ? Unit.Percentage(100.0) : ((WebControl)this).Width);
			((WebControl)val).Height = ((((WebControl)this).Height == Unit.Empty) ? Unit.Percentage(100.0) : ((WebControl)this).Height);
			((WebControl)val).Style.Add("overflow", "hidden");
			Table val2 = new Table();
			val2.CellPadding = 0;
			val2.CellSpacing = 0;
			((WebControl)val2).Width = Unit.Percentage(100.0);
			((WebControl)val2).Height = Unit.Percentage(100.0);
			((WebControl)val2).BorderColor = (((WebControl)this).BorderColor.IsEmpty ? Color.DarkGray : ((WebControl)this).BorderColor);
			Unit borderWidth = ((WebControl)this).BorderWidth;
			((WebControl)val2).BorderWidth = (((Unit)(ref borderWidth)).IsEmpty ? Unit.op_Implicit(2) : ((WebControl)this).BorderWidth);
			((WebControl)val2).BorderStyle = (BorderStyle)(((int)((WebControl)this).BorderStyle == 0) ? 4 : ((int)((WebControl)this).BorderStyle));
			((Control)val).Controls.Add((Control)(object)val2);
			TableRow val3 = new TableRow();
			val3.VerticalAlign = (VerticalAlign)1;
			val2.Rows.Add(val3);
			TableCell val4 = new TableCell();
			((WebControl)val4).Style.Add("width", "642px");
			((WebControl)val4).Style.Add("height", "31px");
			((WebControl)val4).Style.Add("background", string.Format("url('{0}')", GetImageUrl(stiRequestParams, "DesignToolbarLeftHalf.png")));
			val3.Cells.Add(val4);
			TableCell val5 = new TableCell();
			((WebControl)val5).Style.Add("height", "31px");
			((WebControl)val5).Style.Add("background", string.Format("url('{0}')", GetImageUrl(stiRequestParams, "DesignToolbarMiddleHalf.png")));
			val3.Cells.Add(val5);
			TableCell val6 = new TableCell();
			((WebControl)val6).Style.Add("width", "29px");
			((WebControl)val6).Style.Add("height", "31px");
			val3.Cells.Add(val6);
			Panel val7 = new Panel();
			((WebControl)val7).Style.Add("width", "29px");
			((WebControl)val7).Style.Add("height", "31px");
			((WebControl)val7).Style.Add("background", string.Format("url('{0}')", GetImageUrl(stiRequestParams, "DesignToolbarRightHalf.png")));
			((Control)val6).Controls.Add((Control)(object)val7);
			TableRow val8 = new TableRow();
			val2.Rows.Add(val8);
			TableCell val9 = new TableCell();
			val9.ColumnSpan = 3;
			((WebControl)val9).Height = Unit.Percentage(100.0);
			((WebControl)val9).Font.Name = "Arial";
			val9.Text = "<strong>HTML5 Web Viewer</strong><br />" + ((Control)this).ID;
			val9.HorizontalAlign = (HorizontalAlign)2;
			val8.Cells.Add(val9);
			((Control)val).RenderControl(writer);
		}
		else
		{
			Panel val10 = new Panel();
			((WebControl)val10).CssClass = "stiJsViewerMainPanel";
			((Control)val10).ID = ((Control)this).ID + "_JsViewerMainPanel";
			((Control)this).Controls.Add((Control)(object)val10);
			string arg = RenderJsonParameters();
			if (ReportDesignerMode)
			{
				StiJavaScript stiJavaScript = new StiJavaScript();
				((LiteralControl)stiJavaScript).Text = $"var js{((Control)this).ID}Parameters = {arg};";
				((Control)val10).Controls.Add((Control)(object)stiJavaScript);
			}
			else
			{
				StiJavaScript stiJavaScript2 = new StiJavaScript();
				stiJavaScript2.ScriptUrl = GetScriptsUrl();
				((Control)val10).Controls.Add((Control)(object)stiJavaScript2);
				StiJavaScript stiJavaScript3 = new StiJavaScript();
				((LiteralControl)stiJavaScript3).Text = $"var js{((Control)this).ID} = new StiJsViewer({arg});";
				((Control)val10).Controls.Add((Control)(object)stiJavaScript3);
			}
		}
		((WebControl)this).RenderContents(writer);
	}

	internal void ProcessRequest(StiRequestParams requestParams)
	{
		if (requestParams.Action == StiAction.Undefined || requestParams.ComponentType != StiComponentType.Viewer || (!(requestParams.Id == ((Control)this).ID) && requestParams.Action != StiAction.Resource))
		{
			return;
		}
		this.requestParams = requestParams;
		ClientGuid = requestParams.Cache.ClientGuid;
		InvokeViewerEvent();
		StiExportSettings stiExportSettings = null;
		StiWebActionResult stiWebActionResult = null;
		switch (requestParams.Action)
		{
		case StiAction.Resource:
			stiWebActionResult = StiViewerResourcesHelper.Get(requestParams);
			break;
		case StiAction.GetReport:
			try
			{
				InvokeGetReport();
				if (Report != null)
				{
					Report.Key = StiKeyHelper.GenerateKey();
				}
				stiWebActionResult = StiReportHelper.ViewerResult(requestParams, Report);
			}
			catch (StiDashboardNotSupportedException e)
			{
				Console.Write(StiWebActionResult.GetErrorMessageText(e));
				stiWebActionResult = StiWebActionResult.DashboardNotSupportedResult(requestParams);
			}
			catch (Exception e2)
			{
				stiWebActionResult = StiWebActionResult.ErrorResult(requestParams, StiWebActionResult.GetErrorMessageText(e2));
			}
			break;
		case StiAction.GetPages:
			if (Report == null)
			{
				InvokeGetReport();
			}
			stiWebActionResult = StiReportHelper.ViewerResult(requestParams, Report);
			break;
		case StiAction.OpenReport:
			if (Report != null)
			{
				StiCacheCleaner.Clean(Report);
			}
			stiWebActionResult = StiReportHelper.OpenReportResult(requestParams);
			break;
		case StiAction.RefreshReport:
			if (Report == null)
			{
				InvokeGetReport();
			}
			if (requestParams.Viewer.ReportType == StiReportType.Dashboard)
			{
				StiCacheCleaner.Clean(Report);
			}
			InvokeGetReportData();
			stiWebActionResult = StiReportHelper.InteractionResult(requestParams, Report);
			break;
		case StiAction.PrintReport:
			stiExportSettings = GetExportSettings(requestParams);
			InvokePrintReport(stiExportSettings);
			stiWebActionResult = StiExportsHelper.PrintReportResult(requestParams, Report, stiExportSettings);
			break;
		case StiAction.ExportDashboard:
		{
			IStiDashboardExportSettings dashboardExportSettings = GetDashboardExportSettings(requestParams);
			InvokeExportReport(null);
			StiReport stiReport2 = ((!string.IsNullOrEmpty(requestParams.Cache.DashboardDrillDownGuid)) ? GetReportObject(requestParams) : Report);
			stiWebActionResult = StiExportsHelper.ExportDashboardResult(requestParams, stiReport2, dashboardExportSettings);
			InvokeExportReportResponse(null, stiWebActionResult);
			break;
		}
		case StiAction.ExportReport:
			stiExportSettings = GetExportSettings(requestParams);
			InvokeExportReport(stiExportSettings);
			stiWebActionResult = StiExportsHelper.ExportReportResult(requestParams, Report, stiExportSettings);
			InvokeExportReportResponse(stiExportSettings, stiWebActionResult);
			break;
		case StiAction.EmailReport:
		{
			stiExportSettings = GetExportSettings(requestParams);
			StiEmailOptions emailOptions = GetEmailOptions(requestParams);
			InvokeEmailReport(stiExportSettings, emailOptions);
			stiWebActionResult = StiExportsHelper.EmailReportResult(requestParams, Report, stiExportSettings, emailOptions);
			break;
		}
		case StiAction.ReportResource:
			stiWebActionResult = StiReportHelper.ReportResourceResult(requestParams, Report);
			break;
		case StiAction.InitVars:
		{
			StiReport stiReport = Report;
			if (stiReport != null && !stiReport.IsDocument && stiReport.DataStore.Count == 0 && stiReport.Dictionary.Variables.Count > 0)
			{
				InvokeGetReportData();
			}
			stiWebActionResult = StiReportHelper.InteractionResult(requestParams, Report);
			break;
		}
		case StiAction.Variables:
		case StiAction.Sorting:
		case StiAction.DrillDown:
		case StiAction.Collapsing:
		case StiAction.DashboardFiltering:
		case StiAction.DashboardSorting:
		case StiAction.DashboardDrillDown:
		case StiAction.DashboardElementDrillDown:
		case StiAction.DashboardElementDrillUp:
			InvokeInteraction();
			stiWebActionResult = StiReportHelper.InteractionResult(requestParams, Report);
			break;
		case StiAction.ChangeTableElementSelectColumns:
			stiWebActionResult = StiWebActionResult.JsonResult(requestParams, StiTableElementViewHelper.ChangeTableElementSelectColumns(Report, requestParams));
			break;
		case StiAction.ChangeChartElementViewState:
			stiWebActionResult = StiWebActionResult.JsonResult(requestParams, StiChartElementViewHelper.ChangeChartElementViewState(Report, requestParams));
			break;
		case StiAction.DashboardGettingFilterItems:
			stiWebActionResult = StiWebActionResult.JsonResult(requestParams, StiDataFiltersHelper.GetFilterItems(Report, requestParams));
			break;
		case StiAction.DashboardViewData:
			stiWebActionResult = StiWebActionResult.JsonResult(requestParams, StiDataFiltersHelper.GetViewData(Report, requestParams));
			break;
		case StiAction.GetDatePickerFormattedValues:
			stiWebActionResult = StiWebActionResult.JsonResult(requestParams, StiDatePickerElementViewHelper.GetFormattedValues(Report, requestParams));
			break;
		case StiAction.DashboardGetSingleElementContent:
			stiWebActionResult = StiWebActionResult.JsonResult(requestParams, StiReportHelper.GetSingleElementContent(Report, requestParams));
			break;
		case StiAction.DesignReport:
			InvokeDesignReport();
			break;
		case StiAction.UpdateCache:
			requestParams.Cache.Helper.UpdateObjectCacheInternal(requestParams, null);
			break;
		}
		if (stiWebActionResult != null)
		{
			bool browserCache = requestParams.Action == StiAction.Resource;
			StiReportResponse.ResponseBuffer(stiWebActionResult.Data, stiWebActionResult.ContentType, browserCache, stiWebActionResult.FileName, stiWebActionResult.ShowSaveFileDialog);
		}
	}

	protected override void OnInit(EventArgs e)
	{
		StiRequestParams stiRequestParams = RequestParams;
		ProcessRequest(stiRequestParams);
		((Control)this).OnInit(e);
	}

	internal static string GetRequestUrl(bool useRelativeUrls, bool passQueryParameters, int portNumber)
	{
		if (HttpContext.Current == null)
		{
			return null;
		}
		Uri url = HttpContext.Current.Request.Url;
		UriBuilder uriBuilder = new UriBuilder(url);
		if (HttpContext.Current.Request.Headers != null && HttpContext.Current.Request.Headers.Count > 0 && !string.IsNullOrEmpty(HttpContext.Current.Request.Headers["Host"]))
		{
			string[] array = HttpContext.Current.Request.Headers["Host"].Split(new char[1] { ':' });
			uriBuilder.Host = ((!string.IsNullOrEmpty(array[0])) ? array[0] : uriBuilder.Host);
			if (array.Length > 1 && int.TryParse(array[1], out var result))
			{
				uriBuilder.Port = result;
			}
		}
		if (portNumber > 0)
		{
			uriBuilder.Port = portNumber;
		}
		if (portNumber < 0)
		{
			uriBuilder.Port = -1;
		}
		if (HttpContext.Current.Request.IsSecureConnection)
		{
			uriBuilder.Scheme = "https";
		}
		url = uriBuilder.Uri;
		if (useRelativeUrls)
		{
			return HttpContext.Current.Response.ApplyAppPathModifier(passQueryParameters ? url.PathAndQuery : url.AbsolutePath);
		}
		return url.AbsoluteUri;
	}

	private StiRequestParams CreateRequestParams()
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		StiRequestParams stiRequestParams = new StiRequestParams();
		stiRequestParams.Action = StiAction.GetReport;
		stiRequestParams.ComponentType = StiComponentType.Viewer;
		stiRequestParams.Id = ((Control)this).ID;
		stiRequestParams.CloudMode = CloudMode;
		stiRequestParams.ServerMode = ServerMode;
		stiRequestParams.Cache.Mode = CacheMode;
		stiRequestParams.Cache.Timeout = new TimeSpan(0, CacheTimeout, 0);
		stiRequestParams.Cache.Priority = CacheItemPriority;
		stiRequestParams.Cache.ClientGuid = ClientGuid;
		stiRequestParams.Cache.Helper = CacheHelper;
		stiRequestParams.Viewer.PageNumber = 0;
		stiRequestParams.Viewer.Zoom = 1.0;
		stiRequestParams.Viewer.ViewMode = ViewMode;
		stiRequestParams.Viewer.OpenLinksWindow = OpenLinksWindow;
		stiRequestParams.Viewer.ChartRenderType = ChartRenderType;
		stiRequestParams.Viewer.BookmarksPrint = BookmarksPrint;
		stiRequestParams.Viewer.CombineReportPages = CombineReportPages;
		stiRequestParams.Server.UseRelativeUrls = UseRelativeUrls;
		stiRequestParams.Server.UseCompression = UseCompression;
		stiRequestParams.Server.PassQueryParametersForResources = PassQueryParametersForResources;
		stiRequestParams.Server.PassQueryParametersToReport = PassQueryParametersToReport;
		return stiRequestParams;
	}

	private static void WriteToFile(string file, string text)
	{
		string directoryName = Path.GetDirectoryName(file);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		FileStream fileStream = File.Create(file);
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Flush();
		fileStream.Close();
	}

	internal static string GetImageUrl(StiRequestParams requestParams, string imageName)
	{
		string text = imageName;
		if (IsDesignMode)
		{
			text = Path.Combine(Environment.GetEnvironmentVariable("Temp"), "StiWeb" + requestParams.ComponentType);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (!string.IsNullOrEmpty(imageName))
			{
				text = Path.Combine(text, imageName);
				try
				{
					if (!File.Exists(text))
					{
						Bitmap bitmap = StiViewerResourcesHelper.GetBitmap(requestParams, imageName);
						((Image)bitmap).Save(text);
						((Image)bitmap).Dispose();
					}
				}
				catch
				{
				}
			}
		}
		return text.Replace("'", "\\'").Replace("\"", "&quot;");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SaveResourcesForJS(string path)
	{
		StiRequestParams stiRequestParams = CreateRequestParams();
		string[] names = Enum.GetNames(typeof(StiViewerTheme));
		for (int i = 0; i < names.Length; i++)
		{
			string text = names[i];
			if (text.StartsWith("Simple"))
			{
				text = text.Insert("Simple".Length, ".");
			}
			else if (text.StartsWith("Office") && text != "Office2003")
			{
				text = text.Insert("Office20xx".Length, ".");
			}
			stiRequestParams.Theme = names[i];
			string @string = Encoding.UTF8.GetString(StiViewerResourcesHelper.GetStyles(stiRequestParams));
			WriteToFile(Path.Combine(path, "Css", "stimulsoft.viewer." + text.ToLower() + ".css"), @string);
		}
		stiRequestParams.Theme = "Office2013";
		string text2 = RenderJsonParameters(forJsProject: true);
		string string2 = Encoding.UTF8.GetString(StiViewerResourcesHelper.GetScripts(stiRequestParams, forJSProject: true));
		string2 = string2.Replace("this.defaultParameters = {};", "this.defaultParameters = " + text2 + ";this.mergeOptions(parameters, this.defaultParameters); parameters = this.defaultParameters;");
		Hashtable hashtable = new Hashtable();
		double[] array = new double[6] { 1.25, 1.5, 1.75, 2.0, 2.25, 2.5 };
		double[] array2 = array;
		foreach (double num in array2)
		{
			hashtable[num * 100.0] = StiViewerResourcesHelper.GetImagesArray(stiRequestParams, num);
		}
		string2 = string2 + "//@viewer_scaling_images \r\n var stimulsoftViewerScalingImages = " + JsonConvert.SerializeObject(hashtable, Formatting.None, new StringEnumConverter()) + "\r\n //@end_viewer_scaling_images \r\n";
		WriteToFile(Path.Combine(path, "Scripts", "source.viewer.js"), string2);
	}

	public StiWebViewer()
	{
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		StiWebHelper.InitWeb();
		((Control)this).ClientIDMode = (ClientIDMode)3;
		if (((WebControl)this).BackColor.IsEmpty)
		{
			((WebControl)this).BackColor = Color.White;
		}
		if (IsDesignMode)
		{
			((WebControl)this).Width = Unit.Percentage(100.0);
			((WebControl)this).Height = Unit.Pixel(650);
		}
	}
}
