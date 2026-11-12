using System;

namespace Microsoft.IdentityModel.Json.Serialization;

internal interface IContractResolver
{
	JsonContract ResolveContract(Type type);
}
