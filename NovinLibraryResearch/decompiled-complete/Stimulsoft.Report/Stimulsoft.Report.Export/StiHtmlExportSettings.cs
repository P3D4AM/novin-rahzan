using System.ComponentModel;
using System.Drawing.Imaging;
using System.Text;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiHtmlExportSettings : StiPageRangeExportSettings
{
	private StiHtmlType htmlType;

	[DefaultValue(StiHtmlType.Html)]
	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiHtmlType HtmlType
	{
		get
		{
			return htmlType;
		}
		set
		{
			htmlType = value;
		}
	}

	[DefaultValue(0.75f)]
	public float ImageQuality { get; set; } = 0.75f;


	[DefaultValue(100f)]
	public float ImageResolution { get; set; } = 100f;


	[JsonConverter(typeof(StiImageFormatJsonConverter))]
	public ImageFormat ImageFormat { get; set; }

	[JsonConverter(typeof(StiEncodingJsonConverter))]
	public Encoding Encoding { get; set; }

	[DefaultValue(1.0)]
	public double Zoom { get; set; } = 1.0;


	[DefaultValue(StiHtmlExportMode.Table)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiHtmlExportMode ExportMode { get; set; } = StiHtmlExportMode.Table;


	[DefaultValue(StiHtmlExportQuality.High)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiHtmlExportQuality ExportQuality { get; set; } = StiHtmlExportQuality.High;


	[DefaultValue(true)]
	public bool AddPageBreaks { get; set; } = true;


	[DefaultValue(150)]
	public int BookmarksTreeWidth { get; set; } = 150;


	[DefaultValue(StiHtmlExportBookmarksMode.All)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiHtmlExportBookmarksMode ExportBookmarksMode { get; set; } = StiHtmlExportBookmarksMode.All;


	[DefaultValue(true)]
	public bool UseStylesTable { get; set; } = true;


	[DefaultValue(true)]
	public bool RemoveEmptySpaceAtBottom { get; set; } = StiOptions.Export.Html.RemoveEmptySpaceAtBottom;


	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiHorAlignment PageHorAlignment { get; set; } = (StiHorAlignment)1;


	[DefaultValue(false)]
	public bool CompressToArchive { get; set; }

	[DefaultValue(false)]
	public bool UseEmbeddedImages { get; set; }

	[DefaultValue(true)]
	public bool ContinuousPages { get; set; } = true;


	[DefaultValue(null)]
	public string OpenLinksTarget { get; set; } = StiOptions.Export.Html.OpenLinksTarget;


	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiHtmlChartType ChartType { get; set; } = StiOptions.Export.Html.ChartAsBitmap ? StiHtmlChartType.Image : (StiOptions.Viewer.InteractionChartOnFirstLook ? StiHtmlChartType.AnimatedVector : StiHtmlChartType.Vector);


	[DefaultValue(false)]
	public bool UseWatermarkMargins { get; set; }

	public override StiExportFormat GetExportFormat()
	{
		return HtmlType switch
		{
			StiHtmlType.Html5 => StiExportFormat.Html5, 
			StiHtmlType.Mht => StiExportFormat.Mht, 
			_ => StiExportFormat.Html, 
		};
	}

	public StiHtmlExportSettings()
		: this(StiHtmlType.Html)
	{
	}

	public StiHtmlExportSettings(StiHtmlType htmlType)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		this.htmlType = htmlType;
		Encoding = Encoding.UTF8;
		ImageFormat = ImageFormat.Png;
	}
}
