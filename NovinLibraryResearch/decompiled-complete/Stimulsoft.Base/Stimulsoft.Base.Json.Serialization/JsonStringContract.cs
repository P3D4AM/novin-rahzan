using System;

namespace Stimulsoft.Base.Json.Serialization;

public class JsonStringContract : JsonPrimitiveContract
{
	public JsonStringContract(Type underlyingType)
		: base(underlyingType)
	{
		ContractType = JsonContractType.String;
	}
}
