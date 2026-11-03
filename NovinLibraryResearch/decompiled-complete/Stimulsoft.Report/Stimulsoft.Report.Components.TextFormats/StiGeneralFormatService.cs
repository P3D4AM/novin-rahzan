using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Components.TextFormats;

[StiServiceBitmap(typeof(StiGeneralFormatService), "Stimulsoft.Report.Images.TextFormats.FormatGeneral.png")]
public class StiGeneralFormatService : StiFormatService
{
	public static StiGeneralFormatService Default = new StiGeneralFormatService();

	[JsonIgnore]
	public override string ServiceName => Loc.Get("FormFormatEditor", "General");

	[JsonIgnore]
	public override int Position => 1;

	[JsonIgnore]
	public override object Sample => Loc.Get("FormFormatEditor", "SampleText");

	[StiNonSerialized]
	[JsonIgnore]
	public override string StringFormat
	{
		get
		{
			return base.StringFormat;
		}
		set
		{
			base.StringFormat = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "StringFormat");
		return val;
	}

	public override bool Equals(object obj)
	{
		StiGeneralFormatService stiGeneralFormatService = obj as StiGeneralFormatService;
		return stiGeneralFormatService != null;
	}

	public override int GetHashCode()
	{
		return ((object)this).GetHashCode();
	}

	public override StiFormatService CreateNew()
	{
		return new StiGeneralFormatService();
	}

	public StiGeneralFormatService()
	{
		StringFormat = string.Empty;
	}
}
