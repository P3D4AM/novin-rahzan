using System;

namespace Microsoft.IdentityModel.Json.Serialization;

internal class JsonLinqContract : JsonContract
{
	public JsonLinqContract(Type underlyingType)
		: base(underlyingType)
	{
		ContractType = JsonContractType.Linq;
	}
}
