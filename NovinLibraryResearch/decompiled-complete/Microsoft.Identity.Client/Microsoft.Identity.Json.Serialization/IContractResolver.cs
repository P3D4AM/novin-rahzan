using System;

namespace Microsoft.Identity.Json.Serialization;

internal interface IContractResolver
{
	JsonContract ResolveContract(Type type);
}
