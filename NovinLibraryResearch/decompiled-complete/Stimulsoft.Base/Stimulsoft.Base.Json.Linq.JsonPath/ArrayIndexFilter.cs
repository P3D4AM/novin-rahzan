using System.Collections.Generic;
using System.Globalization;
using Stimulsoft.Base.Json.Utilities;

namespace Stimulsoft.Base.Json.Linq.JsonPath;

internal class ArrayIndexFilter : PathFilter
{
	public int? Index { get; set; }

	public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
	{
		foreach (JToken item in current)
		{
			if (Index.HasValue)
			{
				JToken tokenIndex = PathFilter.GetTokenIndex(item, errorWhenNoMatch, Index.Value);
				if (tokenIndex != null)
				{
					yield return tokenIndex;
				}
			}
			else if (item is JArray || item is JConstructor)
			{
				foreach (JToken item2 in (IEnumerable<JToken>)item)
				{
					yield return item2;
				}
			}
			else if (errorWhenNoMatch)
			{
				throw new JsonException("Index * not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, item.GetType().Name));
			}
		}
	}
}
