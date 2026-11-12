using System.Collections.Generic;

namespace Microsoft.Identity.Json.Linq;

internal class JTokenEqualityComparer : IEqualityComparer<JToken>
{
	public bool Equals(JToken x, JToken y)
	{
		return JToken.DeepEquals(x, y);
	}

	public int GetHashCode(JToken obj)
	{
		return obj?.GetDeepHashCode() ?? 0;
	}
}
