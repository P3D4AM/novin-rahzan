using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiExcel2007ExportSettings : StiExcelExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiExcelType ExcelType
	{
		get
		{
			return StiExcelType.Excel2007;
		}
		set
		{
		}
	}

	public StiExcel2007ExportSettings()
		: base(StiExcelType.Excel2007)
	{
	}
}
