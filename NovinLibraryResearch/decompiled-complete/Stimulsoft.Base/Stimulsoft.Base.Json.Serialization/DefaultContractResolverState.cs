using System.Collections.Generic;
using Stimulsoft.Base.Json.Utilities;

namespace Stimulsoft.Base.Json.Serialization;

internal class DefaultContractResolverState
{
	public Dictionary<ResolverContractKey, JsonContract> ContractCache;

	public PropertyNameTable NameTable = new PropertyNameTable();
}
