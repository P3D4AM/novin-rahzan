using System;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiPngExportSettings : StiBitmapExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Png;
		}
		set
		{
			throw new ArgumentException("An ImageType property of the StiPngExportSettings class can't be changed!");
		}
	}

	public StiPngExportSettings()
		: base(StiImageType.Png)
	{
	}
}
