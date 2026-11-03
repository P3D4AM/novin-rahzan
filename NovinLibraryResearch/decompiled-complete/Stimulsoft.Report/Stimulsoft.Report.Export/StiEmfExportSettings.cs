using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiEmfExportSettings : StiImageExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Emf;
		}
		set
		{
		}
	}

	public StiEmfExportSettings()
		: base(StiImageType.Emf)
	{
	}
}
