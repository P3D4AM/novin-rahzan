using System.Text;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiDifExportSettings : StiDataExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiDataType DataType
	{
		get
		{
			return StiDataType.Dif;
		}
		set
		{
		}
	}

	public StiDifExportSettings()
		: base(StiDataType.Dif)
	{
		base.Encoding = Encoding.ASCII;
	}
}
