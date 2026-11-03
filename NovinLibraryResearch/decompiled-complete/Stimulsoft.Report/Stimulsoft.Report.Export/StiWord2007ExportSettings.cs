using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiWord2007ExportSettings : StiPageRangeExportSettings
{
	[DefaultValue(false)]
	public bool UsePageHeadersAndFooters { get; set; }

	[DefaultValue(0.75f)]
	public float ImageQuality { get; set; } = 0.75f;


	[DefaultValue(100f)]
	public float ImageResolution { get; set; } = 100f;


	[DefaultValue(true)]
	public bool RemoveEmptySpaceAtBottom { get; set; } = StiOptions.Export.Word.RemoveEmptySpaceAtBottom;


	[DefaultValue(null)]
	public string CompanyString { get; set; } = StiExportUtils.GetReportVersion();


	[DefaultValue(null)]
	public string LastModifiedString { get; set; } = StiExportUtils.GetReportVersion();


	[DefaultValue(StiWord2007RestrictEditing.No)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiWord2007RestrictEditing RestrictEditing { get; set; } = StiWord2007RestrictEditing.No;


	public override StiExportFormat GetExportFormat()
	{
		return StiExportFormat.Word2007;
	}
}
