using System.ComponentModel;

namespace Stimulsoft.Report.Export;

public class StiPpt2007ExportSettings : StiPageRangeExportSettings
{
	[DefaultValue(0.75f)]
	public float ImageQuality { get; set; } = 0.75f;


	[DefaultValue(100f)]
	public float ImageResolution { get; set; } = 100f;


	public override StiExportFormat GetExportFormat()
	{
		return StiExportFormat.Ppt2007;
	}
}
