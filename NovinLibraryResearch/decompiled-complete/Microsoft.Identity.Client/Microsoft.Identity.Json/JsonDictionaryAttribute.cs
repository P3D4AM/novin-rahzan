using System;

namespace Microsoft.Identity.Json;

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
