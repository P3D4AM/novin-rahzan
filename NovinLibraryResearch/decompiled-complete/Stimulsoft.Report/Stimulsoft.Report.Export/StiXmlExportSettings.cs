using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiXmlExportSettings : StiDataExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiDataType DataType
	{
		get
		{
			return StiDataType.Xml;
		}
		set
		{
		}
	}

	public StiXmlExportSettings()
		: base(StiDataType.Xml)
	{
	}
}
