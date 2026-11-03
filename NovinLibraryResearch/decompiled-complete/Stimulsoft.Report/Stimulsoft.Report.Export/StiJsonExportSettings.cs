using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiJsonExportSettings : StiDataExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiDataType DataType
	{
		get
		{
			return StiDataType.Json;
		}
		set
		{
		}
	}

	public StiJsonExportSettings()
		: base(StiDataType.Json)
	{
	}
}
