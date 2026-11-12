using System;

namespace Stimulsoft.Base.Json.Serialization;

public interface IContractResolver
{
	JsonContract ResolveContract(Type type);
}
