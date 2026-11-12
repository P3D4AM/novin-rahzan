using System;
using Stimulsoft.Base.Json.Utilities;

namespace Stimulsoft.Base.Json.Serialization;

public class JsonPrimitiveContract : JsonContract
{
	internal PrimitiveTypeCode TypeCode { get; set; }

	public JsonPrimitiveContract(Type underlyingType)
		: base(underlyingType)
	{
		ContractType = JsonContractType.Primitive;
		TypeCode = ConvertUtils.GetTypeCode(underlyingType);
		IsReadOnlyOrFixedSize = true;
	}
}
