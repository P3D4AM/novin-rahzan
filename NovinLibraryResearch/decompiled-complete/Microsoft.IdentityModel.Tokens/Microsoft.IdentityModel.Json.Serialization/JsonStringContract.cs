using System;

namespace Microsoft.IdentityModel.Json.Serialization;

internal class JsonStringContract : JsonPrimitiveContract
{
	public JsonStringContract(Type underlyingType)
		: base(underlyingType)
	{
		ContractType = JsonContractType.String;
	}
}
