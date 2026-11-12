using System;

namespace Microsoft.IdentityModel.Json.Serialization;

internal interface ISerializationBinder
{
	Type BindToType(string assemblyName, string typeName);

	void BindToName(Type serializedType, out string assemblyName, out string typeName);
}
