using System.ComponentModel;

namespace Stimulsoft.Report.Export;

public class StiXpsExportSettings : StiPageRangeExportSettings
{
	[DefaultValue(0.75f)]
	public float ImageQuality { get; set; } = 0.75f;


	[DefaultValue(100f)]
	public float ImageResolution { get; set; } = 100f;


	[DefaultValue(false)]
	public bool ExportRtfTextAsImage { get; set; }

	public override StiExportFormat GetExportFormat()
	{
		return StiExportFormat.Xps;
	}
}
