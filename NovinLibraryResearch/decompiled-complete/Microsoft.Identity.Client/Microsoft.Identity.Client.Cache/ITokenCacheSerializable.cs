using System.Collections.Generic;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache;

internal interface ITokenCacheSerializable
{
	IDictionary<string, JToken> Deserialize(byte[] bytes, bool clearExistingCacheData);

	byte[] Serialize(IDictionary<string, JToken> additionalNodes);
}
