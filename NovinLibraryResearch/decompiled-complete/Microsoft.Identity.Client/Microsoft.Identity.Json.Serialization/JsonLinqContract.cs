using System;

namespace Microsoft.Identity.Json.Serialization;

internal class JsonLinqContract : JsonContract
{
	public JsonLinqContract(Type underlyingType)
		: base(underlyingType)
	{
		ContractType = JsonContractType.Linq;
	}
}
