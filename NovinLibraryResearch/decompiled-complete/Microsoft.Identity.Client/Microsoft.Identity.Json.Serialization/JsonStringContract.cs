using System;

namespace Microsoft.Identity.Json.Serialization;

internal class JsonStringContract : JsonPrimitiveContract
{
	public JsonStringContract(Type underlyingType)
		: base(underlyingType)
	{
		ContractType = JsonContractType.String;
	}
}
