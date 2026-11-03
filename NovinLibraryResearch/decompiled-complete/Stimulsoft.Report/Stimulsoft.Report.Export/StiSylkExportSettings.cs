using System.Text;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiSylkExportSettings : StiDataExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiDataType DataType
	{
		get
		{
			return StiDataType.Sylk;
		}
		set
		{
		}
	}

	public StiSylkExportSettings()
		: base(StiDataType.Sylk)
	{
		base.Encoding = Encoding.ASCII;
	}
}
