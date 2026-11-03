using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiDbfExportSettings : StiDataExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiDataType DataType
	{
		get
		{
			return StiDataType.Dbf;
		}
		set
		{
		}
	}

	public StiDbfExportSettings()
		: base(StiDataType.Dbf)
	{
	}
}
