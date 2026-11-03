using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiRtfExportSettings : StiPageRangeExportSettings
{
	[DefaultValue(0)]
	public int CodePage { get; set; }

	[DefaultValue(StiRtfExportMode.Table)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiRtfExportMode ExportMode { get; set; } = StiRtfExportMode.Table;


	[DefaultValue(false)]
	public bool UsePageHeadersAndFooters { get; set; }

	[DefaultValue(0.75f)]
	public float ImageQuality { get; set; } = 0.75f;


	[DefaultValue(100f)]
	public float ImageResolution { get; set; } = 100f;


	[DefaultValue(true)]
	public bool RemoveEmptySpaceAtBottom { get; set; } = StiOptions.Export.RichText.RemoveEmptySpaceAtBottom;


	[DefaultValue(false)]
	public bool StoreImagesAsPng { get; set; }

	public override StiExportFormat GetExportFormat()
	{
		return StiExportFormat.Rtf;
	}
}
