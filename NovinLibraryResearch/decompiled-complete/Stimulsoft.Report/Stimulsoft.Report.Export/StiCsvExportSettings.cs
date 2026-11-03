using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiCsvExportSettings : StiDataExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiDataType DataType
	{
		get
		{
			return StiDataType.Csv;
		}
		set
		{
		}
	}

	public StiCsvExportSettings()
		: base(StiDataType.Csv)
	{
	}
}
