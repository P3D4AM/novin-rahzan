using Stimulsoft.Base.Json.Utilities;

namespace Stimulsoft.Base.Json.Serialization;

public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
{
	public CamelCasePropertyNamesContractResolver()
		: base(shareCache: true)
	{
	}

	protected internal override string ResolvePropertyName(string propertyName)
	{
		return StringUtils.ToCamelCase(propertyName);
	}
}
