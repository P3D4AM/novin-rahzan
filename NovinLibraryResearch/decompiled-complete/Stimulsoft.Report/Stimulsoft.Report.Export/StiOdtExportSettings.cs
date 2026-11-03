using System.ComponentModel;

namespace Stimulsoft.Report.Export;

public class StiOdtExportSettings : StiPageRangeExportSettings
{
	[DefaultValue(false)]
	public bool UsePageHeadersAndFooters { get; set; }

	[DefaultValue(0.75f)]
	public float ImageQuality { get; set; } = 0.75f;


	[DefaultValue(100f)]
	public float ImageResolution { get; set; } = 100f;


	[DefaultValue(true)]
	public bool RemoveEmptySpaceAtBottom { get; set; } = StiOptions.Export.OpenDocumentWriter.RemoveEmptySpaceAtBottom;


	public override StiExportFormat GetExportFormat()
	{
		return StiExportFormat.Odt;
	}
}
