using System;

namespace Microsoft.IdentityModel.Json.Serialization;

internal class JsonISerializableContract : JsonContainerContract
{
	public ObjectConstructor<object> ISerializableCreator { get; set; }

	public JsonISerializableContract(Type underlyingType)
		: base(underlyingType)
	{
		ContractType = JsonContractType.Serializable;
	}
}
