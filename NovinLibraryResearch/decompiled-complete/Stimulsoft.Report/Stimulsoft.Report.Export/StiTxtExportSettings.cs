using System.ComponentModel;
using System.Text;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiTxtExportSettings : StiPageRangeExportSettings
{
	[JsonConverter(typeof(StiEncodingJsonConverter))]
	public Encoding Encoding { get; set; } = Encoding.UTF8;


	[DefaultValue(true)]
	public bool DrawBorder { get; set; } = true;


	[DefaultValue(StiTxtBorderType.UnicodeSingle)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiTxtBorderType BorderType { get; set; } = StiTxtBorderType.UnicodeSingle;


	[DefaultValue(true)]
	public bool KillSpaceLines { get; set; } = true;


	[DefaultValue(true)]
	public bool KillSpaceGraphLines { get; set; } = true;


	[DefaultValue(true)]
	public bool PutFeedPageCode { get; set; } = true;


	[DefaultValue(true)]
	public bool CutLongLines { get; set; } = true;


	[DefaultValue(1f)]
	public float ZoomX { get; set; } = 1f;


	[DefaultValue(1f)]
	public float ZoomY { get; set; } = 1f;


	[DefaultValue(false)]
	public bool UseEscapeCodes { get; set; }

	[DefaultValue(null)]
	public string EscapeCodesCollectionName { get; set; }

	public override StiExportFormat GetExportFormat()
	{
		return StiExportFormat.Text;
	}
}
