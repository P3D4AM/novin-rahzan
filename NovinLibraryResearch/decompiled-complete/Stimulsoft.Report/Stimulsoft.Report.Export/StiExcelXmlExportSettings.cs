using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiExcelXmlExportSettings : StiExcelExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiExcelType ExcelType
	{
		get
		{
			return StiExcelType.ExcelXml;
		}
		set
		{
		}
	}

	public StiExcelXmlExportSettings()
		: base(StiExcelType.ExcelXml)
	{
	}
}
