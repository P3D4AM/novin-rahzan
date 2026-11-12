using System.Collections;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Json.Linq;

internal interface IJEnumerable<out T> : IEnumerable<T>, IEnumerable where T : JToken
{
	IJEnumerable<JToken> this[object key] { get; }
}
