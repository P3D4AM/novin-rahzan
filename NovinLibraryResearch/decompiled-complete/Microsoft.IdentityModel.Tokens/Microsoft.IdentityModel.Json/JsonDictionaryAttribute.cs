using System;

namespace Microsoft.IdentityModel.Json;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
internal sealed class JsonDictionaryAttribute : JsonContainerAttribute
{
	public JsonDictionaryAttribute()
	{
	}

	public JsonDictionaryAttribute(string id)
		: base(id)
	{
	}
}
